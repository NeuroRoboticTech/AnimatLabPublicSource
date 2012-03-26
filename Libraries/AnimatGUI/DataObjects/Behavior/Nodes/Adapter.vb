Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects.Behavior.Nodes

    Public MustInherit Class Adapter
        Inherits Behavior.Node

#Region " Attributes "

        Protected m_bnOrigin As Behavior.Node
        Protected m_bnDestination As Behavior.Node

        Protected m_gnGain As AnimatGUI.DataObjects.Gain

        'Only used during the loading process.
        Protected m_strOriginID As String
        Protected m_strDestinationID As String
        Protected m_strDataTypeID As String

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Standard Adapter"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property IsDisplayedInIconPanel() As Boolean
            Get
                Return False
            End Get
        End Property

        <EditorAttribute(GetType(TypeHelpers.GainTypeEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Overridable Property Gain() As AnimatGUI.DataObjects.Gain
            Get
                Return m_gnGain
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                If Not Value Is Nothing Then
                    If Not Value Is Nothing Then
                        SetSimData("Gain", Value.GetSimulationXml("Gain", Me), True)
                        Value.InitializeSimulationReferences()
                    End If

                    If Not m_gnGain Is Nothing Then m_gnGain.ParentData = Nothing
                    m_gnGain = Value
                    If Not m_gnGain Is Nothing Then
                        m_gnGain.ParentData = Me
                        m_gnGain.GainPropertyName = "Gain"
                    End If
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property Origin() As Behavior.Node
            Get
                Return m_bnOrigin
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property Destination() As Behavior.Node
            Get
                Return m_bnDestination
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides Property DataTypes() As TypeHelpers.DataTypeID
            Get
                Return m_thDataTypes
            End Get
            Set(ByVal Value As TypeHelpers.DataTypeID)
                If Not Value Is Nothing Then
                    m_thDataTypes = Value
                    CheckForErrors()
                    SetGainLimits()

                    Me.SetSimData("OriginID", Me.GetSimulationXml("Adapter"), True)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property IncomingDataType() As AnimatGUI.DataObjects.DataType
            Get
                If Not m_bnDestination Is Nothing Then
                    Return m_bnDestination.IncomingDataType
                End If
                Return Nothing
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Behavior.PhysicsModule)
            End Get
        End Property

        Public MustOverride ReadOnly Property AdapterType() As String

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.StandardAdapter.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property AllowStimulus() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Try

                Shape = Behavior.Node.enumShape.Display
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.Black
                Me.FillColor = Color.Salmon
                m_bEnabled = True

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "AnimatGUI.StandardAdapter.gif")
                Me.Name = "Standard Adapter"

                Me.Font = New Font("Arial", 12, FontStyle.Bold)
                Me.Description = "Provides an interface adapter between nodes and the physics engine."

                AddCompatibleLink(New Behavior.Links.Adapter(Nothing))

                m_gnGain = New AnimatGUI.DataObjects.Gains.Polynomial(Me, "Gain", "Input Variable", "Output Variable", False, False)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As Adapter = DirectCast(doOriginal, Adapter)

            m_gnGain = DirectCast(bnOrig.m_gnGain.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gain)
            m_bEnabled = bnOrig.m_bEnabled
        End Sub

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList)

            m_gnGain.AddToReplaceIDList(aryReplaceIDList)
        End Sub

        Public Overrides Sub CheckCanAttachAdapter()
            Throw New System.Exception("You cannot attach an adapter to another adapter.")
        End Sub

        Public Overrides Sub BeforeAddLink(ByVal blLink As Link)

            If blLink.ActualDestination Is Me AndAlso Me.InLinks.Count > 0 Then
                Throw New System.Exception("You can only have one incoming link to an adapter node!")
            End If

            If blLink.ActualOrigin Is Me AndAlso Me.OutLinks.Count > 0 Then
                Throw New System.Exception("You can only have one outgoing link from an adapter node!")
            End If

            If blLink.Origin Is Me AndAlso Not blLink.Destination Is Nothing Then
                blLink.Destination.CheckCanAttachAdapter()
            End If

            If blLink.Destination Is Me AndAlso Not blLink.Origin Is Nothing Then
                blLink.Origin.CheckCanAttachAdapter()
            End If

        End Sub

        Public Overrides Sub AfterAddLink(ByVal blLink As Link)

            'If this is the destination then get the origin from the other end
            If blLink.ActualDestination Is Me Then
                SetOrigin(blLink.Origin, False)
                Me.m_thDataTypes = DirectCast(m_bnOrigin.DataTypes.Clone(Me, False, Nothing), TypeHelpers.DataTypeID)
                SetGainLimits()
            End If

            'If this is the Origin then get the destination from the other end
            If blLink.ActualOrigin Is Me Then
                SetDestination(blLink.Destination, False)
            End If

            MyBase.AfterAddLink(blLink)
        End Sub

        Protected Sub SetGainLimits()
            If Not m_gnGain Is Nothing AndAlso Not m_thDataTypes Is Nothing AndAlso Not m_thDataTypes.Value Is Nothing Then
                m_gnGain.UpperLimit = New ScaledNumber(m_gnGain, "UpperLimit", m_thDataTypes.Value.UpperLimit, _
                                                       m_thDataTypes.Value.UpperLimitscale, _
                                                       m_thDataTypes.Value.Units, _
                                                       m_thDataTypes.Value.UnitsAbbreviation)
                m_gnGain.LowerLimit = New ScaledNumber(m_gnGain, "LowerLimit", m_thDataTypes.Value.LowerLimit, _
                                                       m_thDataTypes.Value.LowerLimitscale, _
                                                       m_thDataTypes.Value.Units, _
                                                       m_thDataTypes.Value.UnitsAbbreviation)
            End If
        End Sub

        Public Overrides Sub BeforeCopy()

            Dim blLink As AnimatGUI.DataObjects.Behavior.Link
            For Each deEntry As DictionaryEntry In m_aryLinks
                blLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                If Not blLink.Selected Then
                    blLink.SelectItem(False)
                End If
            Next

        End Sub

        'We do not want adapters to show up in the node tree view drop down.
        Public Overrides Sub CreateNodeTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal aryNodes As Crownwood.DotNetMagic.Controls.NodeCollection)
        End Sub

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            If Util.Application.ProjectErrors Is Nothing Then Return

            If m_thDataTypes Is Nothing OrElse m_thDataTypes.ID Is Nothing OrElse m_thDataTypes.ID.Trim.Length = 0 Then
                If Not Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.DataTypeNotSet)) Then
                    Dim deError As New DiagramErrors.DataError(Me, DiagramError.enumErrorLevel.Error, DiagramError.enumErrorTypes.DataTypeNotSet, _
                                                               "The adapter '" & Me.Text & "' does not have a defined data type pointer value.")
                    Util.Application.ProjectErrors.Errors.Add(deError.ID, deError)
                End If
            Else
                If Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.DataTypeNotSet)) Then
                    Util.Application.ProjectErrors.Errors.Remove(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.DataTypeNotSet))
                End If
            End If

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gain", GetType(AnimatGUI.DataObjects.Gain), "Gain", _
                                        "Adapter Properties", "Sets the gain that controls the input/output relationship " & _
                                        "between the two selected items.", m_gnGain, _
                                        GetType(AnimatGUI.TypeHelpers.GainTypeEditor), _
                                        GetType(AnimatGUI.TypeHelpers.GainTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Source Data Type ID", GetType(AnimatGUI.TypeHelpers.DataTypeID), "DataTypes", _
                                        "Adapter Properties", "Sets the type of data to use as an input from the source node into the gain function.", m_thDataTypes, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                        GetType(AnimatGUI.TypeHelpers.DataTypeIDTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                        "Adapter Properties", "Determines if this adapter is enabled or not.", m_bEnabled))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_gnGain Is Nothing Then m_gnGain.ClearIsDirty()
        End Sub

        Public Overrides Sub InitializeSimulationReferences()

            If m_doInterface Is Nothing AndAlso Not Util.Application.SimulationInterface Is Nothing AndAlso Util.Application.SimulationInterface.SimOpen Then
                If Not Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                    AddToSim(True)
                End If

                m_doInterface = Util.Application.CreateDataObjectInterface(Me.ID)
            End If

            m_gnGain.InitializeSimulationReferences()
        End Sub

        Public Overrides Function CanCopy(ByVal aryItems As ArrayList) As Boolean

            If m_bnOrigin Is Nothing OrElse m_bnDestination Is Nothing Then
                Return False
            End If

            If Not Util.FindIDInList(aryItems, m_bnOrigin.ID) Then
                Return False
            End If

            If Not Util.FindIDInList(aryItems, m_bnDestination.ID) Then
                Return False
            End If

            Return True
        End Function

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean)
            If m_bIsInitialized AndAlso Not NeuralModule Is Nothing Then
                NeuralModule.VerifyExistsInSim()
                If Not Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                    'If we just created this neuralmodule in the sim then this object might already exist now. We should only add it if it does not exist.
                    Util.Application.SimulationInterface.AddItem(Me.NeuralModule.ID(), "Adapter", Me.ID, Me.GetSimulationXml("Adapter"), bThrowError)
                End If
            End If
            InitializeSimulationReferences()
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not NeuralModule Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Me.NeuralModule.ID(), "Adapter", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
            m_gnGain.RemoveFromSim(True)
        End Sub

        Public Overrides Sub BeforeAddToList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            Me.SignalBeforeAddItem(Me)
            'Adapters do NOT call AddToSim when first added to the list.
            'If bCallSimMethods Then AddToSim(bThrowError)
        End Sub

        Public Overrides Sub AfterAddToList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            MyBase.AfterAddToList(bCallSimMethods, bThrowError)

            If Not NeuralModule Is Nothing Then
                NeuralModule.Nodes.Add(Me.ID, Me)
            End If
        End Sub

        Public Overrides Sub AfterRemoveFromList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            MyBase.AfterRemoveFromList(bCallSimMethods, bThrowError)

            If Not NeuralModule Is Nothing AndAlso NeuralModule.Nodes.Contains(Me.ID) Then
                NeuralModule.Nodes.Remove(Me.ID)
            End If
        End Sub

        Public Overridable Sub CreateAdapterSimReferences(Optional ByVal bThrowError As Boolean = True)


        End Sub


#End Region

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_strOriginID = Util.LoadID(oXml, "Origin", True, "")
            m_strDestinationID = Util.LoadID(oXml, "Destination", True, "")
            m_strDataTypeID = Util.LoadID(oXml, "DataType", True, "")
            m_bEnabled = oXml.GetChildBool("Enabled", True)

            If oXml.FindChildElement("Gain", False) Then
                oXml.IntoChildElement("Gain")
                Dim strAssemblyFile As String = oXml.GetChildString("AssemblyFile")
                Dim strClassName As String = oXml.GetChildString("ClassName")
                oXml.OutOfElem()

                m_gnGain = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me), AnimatGUI.DataObjects.Gain)
                m_gnGain.LoadData(oXml, "Gain", "Gain")
            End If

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub InitializeAfterLoad()

            Try
                If m_bnOrigin Is Nothing Then
                    If m_strOriginID.Trim.Length > 0 Then
                        SetOrigin(Me.Organism.FindBehavioralNode(m_strOriginID), False)

                        If Not m_bnOrigin.IsInitialized Then
                            SetOrigin(Nothing, False)
                            m_bIsInitialized = False
                            Return
                        End If

                        m_thDataTypes = DirectCast(m_bnOrigin.DataTypes.Clone(Me, False, Nothing), TypeHelpers.DataTypeID)

                        If Not m_thDataTypes Is Nothing AndAlso m_strDataTypeID.Trim.Length > 0 AndAlso m_strDataTypeID.Trim.Length > 0 Then
                            If Me.m_thDataTypes.DataTypes.Contains(m_strDataTypeID) Then
                                Me.m_thDataTypes.ID = m_strDataTypeID
                            End If
                        End If
                    End If
                End If

                If m_bnDestination Is Nothing Then
                    If m_strDestinationID.Trim.Length > 0 Then
                        SetDestination(Me.Organism.FindBehavioralNode(m_strDestinationID), False)
                    End If
                End If

                MyBase.InitializeAfterLoad()

                If Not m_gnGain Is Nothing Then
                    m_gnGain.InitializeAfterLoad()
                End If

            Catch ex As System.Exception
                m_bIsInitialized = False
            End Try

        End Sub

        Protected Overridable Sub SetOrigin(ByVal bnValue As Behavior.Node, ByVal bCallSimMethods As Boolean)
            Dim bnPrev As Behavior.Node = m_bnOrigin

            DisconnectOriginEvents()
            If bCallSimMethods Then RemoveFromSim(True)
            m_bnOrigin = bnValue

            If bCallSimMethods Then
                Try
                    AddToSim(True)
                Catch ex As Exception
                    m_bnOrigin = bnPrev
                    AddToSim(True)
                    ConnectOriginEvents()
                    Throw ex
                End Try
            End If

            ConnectOriginEvents()
        End Sub

        Protected Sub ConnectOriginEvents()

            DisconnectOriginEvents()

            If Not m_bnOrigin Is Nothing Then
                AddHandler m_bnOrigin.AfterPropertyChanged, AddressOf Me.OnOriginPropertyChanged
            End If
        End Sub

        Protected Sub DisconnectOriginEvents()
            If Not m_bnOrigin Is Nothing Then
                RemoveHandler m_bnOrigin.AfterPropertyChanged, AddressOf Me.OnOriginPropertyChanged
            End If
        End Sub

        Protected Overridable Sub SetDestination(ByVal bnValue As Behavior.Node, ByVal bCallSimMethods As Boolean)
            Dim bnPrev As Behavior.Node = m_bnDestination

            DisconnectDestinationEvents()
            If bCallSimMethods Then RemoveFromSim(True)
            m_bnDestination = bnValue

            If bCallSimMethods Then
                Try
                    AddToSim(True)
                Catch ex As Exception
                    m_bnDestination = bnPrev
                    AddToSim(True)
                    ConnectDestinationEvents()
                    Throw ex
                End Try
            End If

            ConnectDestinationEvents()
        End Sub

        Protected Sub ConnectDestinationEvents()

            DisconnectDestinationEvents()

            If Not m_bnDestination Is Nothing Then
                AddHandler m_bnDestination.AfterPropertyChanged, AddressOf Me.OnDestinationPropertyChanged
            End If
        End Sub

        Protected Sub DisconnectDestinationEvents()
            If Not m_bnDestination Is Nothing Then
                RemoveHandler m_bnDestination.AfterPropertyChanged, AddressOf Me.OnDestinationPropertyChanged
            End If
        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            If Not m_bnOrigin Is Nothing Then
                oXml.AddChildElement("OriginID", m_bnOrigin.ID)
            Else
                Throw New System.Exception("The orgin node for an adapter has been lost!!")
            End If

            If Not m_bnDestination Is Nothing Then
                oXml.AddChildElement("DestinationID", m_bnDestination.ID)
            Else
                Throw New System.Exception("The destination node for an adapter has been lost!!")
            End If

            If Not m_thDataTypes Is Nothing Then
                oXml.AddChildElement("DataTypeID", m_thDataTypes.ID)
            End If

            If Not m_gnGain Is Nothing Then
                m_gnGain.SaveData(oXml, "Gain")
            End If

            oXml.AddChildElement("Enabled", m_bEnabled)

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

#Region "Event Handlers"

        Protected Overridable Sub OnOriginPropertyChanged(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)
            Try
                If Util.IsTypeOf(doObject.GetType, GetType(Behavior.Node), False) Then
                    Dim bnNode As Behavior.Node = DirectCast(doObject, Behavior.Node)
                    If bnNode.NeedToUpdateAdapterID(propInfo) Then
                        Me.SetSimData("OriginID", Me.GetSimulationXml("Adapter"), True)
                    End If
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnDestinationPropertyChanged(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)
            Try
                If Util.IsTypeOf(doObject.GetType, GetType(Behavior.Node), False) Then
                    Dim bnNode As Behavior.Node = DirectCast(doObject, Behavior.Node)
                    If bnNode.NeedToUpdateAdapterID(propInfo) Then
                        Me.SetSimData("DestinationID", Me.GetSimulationXml("Adapter"), True)
                    End If
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnOriginModified(ByVal blLink As Link)
            Try
                If Not Util.IsTypeOf(blLink.Origin.GetType, GetType(Behavior.Nodes.Adapter), False) Then
                    SetOrigin(blLink.Origin, True)
                End If
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnDestinationModified(ByVal blLink As Link)
            Try
                If Not Util.IsTypeOf(blLink.Destination.GetType, GetType(Behavior.Nodes.Adapter), False) Then
                    SetDestination(blLink.Destination, True)
                End If
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace