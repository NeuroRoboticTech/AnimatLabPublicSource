Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.ComponentModel.Design.Serialization
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects.Behavior

    Public Class Synapse
        Inherits AnimatGUI.DataObjects.Behavior.Links.Synapse

#Region " Attributes "

        Protected m_stSynapseType As SynapseType

        Protected m_strUserText As String = ""
        Protected m_snSynapticConductance As AnimatGUI.Framework.ScaledNumber
        Protected m_snConductionDelay As AnimatGUI.Framework.ScaledNumber

        'Only used during loading
        Protected m_strSynapticTypeID As String = ""

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                If Not m_stSynapseType Is Nothing Then
                    Return m_stSynapseType.TypeName
                Else
                    Return ""
                End If
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(IntegrateFireGUI.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        <EditorAttribute(GetType(TypeHelpers.SynapseTypeEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Overridable Property SynapseType() As SynapseType
            Get
                Return m_stSynapseType
            End Get
            Set(ByVal Value As SynapseType)
                Dim bReselect As Boolean = False

                Dim stOldSynapse As SynapseType = m_stSynapseType

                SetSimData("SynapseTypeID", Value.ID, True)

                If Not m_stSynapseType Is Nothing Then
                    RemoveHandler m_stSynapseType.SynapseTypeChanged, AddressOf Me.OnSynapseTypeChanged

                    'If we change the class of synapse types. (electical to spiking) then we need to reselect the object
                    'because different types display slightly different data for the synapse
                    If Not Value Is Nothing AndAlso Not m_stSynapseType.GetType() Is Value.GetType() Then
                        bReselect = True
                    End If
                End If

                m_stSynapseType = Value

                If Not m_stSynapseType Is Nothing AndAlso m_stSynapseType.GetType() Is GetType(SynapseTypes.SpikingChemical) _
                    AndAlso (Not stOldSynapse Is Nothing AndAlso m_stSynapseType.Name <> stOldSynapse.Name) Then
                    Dim scType As SynapseTypes.SpikingChemical = DirectCast(m_stSynapseType, SynapseTypes.SpikingChemical)
                    Me.SynapticConductance = DirectCast(scType.SynapticConductance.Clone(Me, False, Nothing), ScaledNumber)
                End If

                If Not m_stSynapseType Is Nothing Then
                    AddHandler m_stSynapseType.SynapseTypeChanged, AddressOf Me.OnSynapseTypeChanged
                End If

                If bReselect AndAlso Not m_ParentDiagram Is Nothing Then
                    Me.SelectItem(False)
                End If

                UpdateChart(True)
            End Set
        End Property

        Public Overridable ReadOnly Property SynapseTypeName() As String
            Get
                If Not m_stSynapseType Is Nothing Then
                    Return m_stSynapseType.Name
                Else
                    Return ""
                End If
            End Get
        End Property

        Public Overridable Property UserText() As String
            Get
                Return m_strUserText
            End Get
            Set(ByVal Value As String)
                m_strUserText = Value

                If Not m_snSynapticConductance Is Nothing AndAlso Not m_stSynapseType Is Nothing _
                   AndAlso m_stSynapseType.GetType() Is GetType(SynapseTypes.SpikingChemical) Then
                    Me.Text = m_snSynapticConductance.Text & vbCrLf & Replace(m_strUserText, vbCrLf, "")
                Else
                    Me.Text = m_strUserText
                End If
            End Set
        End Property

        Public Overridable Property SynapticConductance() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snSynapticConductance
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 0.0001 Then
                    Throw New System.Exception("The synaptic conductance must be between the range 0 to 100 uS/size.")
                End If

                SetSimData("SynapticConductance", Value.ValueFromScale(ScaledNumber.enumNumericScale.micro).ToString, True)
                m_snSynapticConductance.CopyData(Value)

                If Not m_snSynapticConductance Is Nothing AndAlso Not m_stSynapseType Is Nothing _
                   AndAlso m_stSynapseType.GetType() Is GetType(SynapseTypes.SpikingChemical) Then
                    Me.Text = m_snSynapticConductance.Text & vbCrLf & Replace(m_strUserText, vbCrLf, "")
                Else
                    Me.Text = m_strUserText
                End If
            End Set
        End Property

        Public Overridable Property ConductionDelay() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snConductionDelay
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 0.1 Then
                    Throw New System.Exception("The conduction delay must be between the range 0 to 100 ms.")
                End If

                SetSimData("ConductionDelay", Value.ValueFromScale(ScaledNumber.enumNumericScale.milli).ToString, True)
                m_snConductionDelay.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                If Not m_stSynapseType Is Nothing Then
                    Return m_stSynapseType.WorkspaceImageName
                Else
                    Return "IntegrateFireGUI.SpikingSynapse.gif"
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property SimClassName() As String
            Get
                Return "Synapse"
            End Get
        End Property

#Region " Overridden Link Properties "

        Public Overrides Property ArrowDestination() As Arrow
            Get
                If Not m_stSynapseType Is Nothing Then
                    Return m_stSynapseType.ArrowDestination
                End If
            End Get
            Set(ByVal Value As Arrow)
                m_stSynapseType.ArrowDestination = Value
                UpdateChart()
            End Set
        End Property

        Public Overrides Property ArrowMiddle() As Arrow
            Get
                If Not m_stSynapseType Is Nothing Then
                    Return m_stSynapseType.ArrowMiddle
                End If
            End Get
            Set(ByVal Value As Arrow)
                m_stSynapseType.ArrowMiddle = Value
                UpdateChart()
            End Set
        End Property

        Public Overrides Property ArrowOrigin() As Arrow
            Get
                If Not m_stSynapseType Is Nothing Then
                    Return m_stSynapseType.ArrowOrigin
                End If
            End Get
            Set(ByVal Value As Arrow)
                m_stSynapseType.ArrowOrigin = Value
                UpdateChart()
            End Set
        End Property

        Public Overrides Property DashStyle() As System.Drawing.Drawing2D.DashStyle
            Get
                If Not m_stSynapseType Is Nothing Then
                    Return m_stSynapseType.DashStyle
                End If
            End Get
            Set(ByVal Value As System.Drawing.Drawing2D.DashStyle)
                m_stSynapseType.DashStyle = Value
                UpdateChart()
            End Set
        End Property

        Public Overrides Property DrawColor() As System.Drawing.Color
            Get
                If Not m_stSynapseType Is Nothing Then
                    Return m_stSynapseType.DrawColor
                End If
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_stSynapseType.DrawColor = Value
                UpdateChart()
            End Set
        End Property

        Public Overrides Property DrawWidth() As Integer
            Get
                If Not m_stSynapseType Is Nothing Then
                    Return m_stSynapseType.DrawWidth
                End If
            End Get
            Set(ByVal Value As Integer)
                m_stSynapseType.DrawWidth = Value
                UpdateChart()
            End Set
        End Property

        Public Overrides Property Font() As System.Drawing.Font
            Get
                If Not m_stSynapseType Is Nothing Then
                    Return m_stSynapseType.Font
                End If
            End Get
            Set(ByVal Value As System.Drawing.Font)
                m_stSynapseType.Font = Value
                UpdateChart()
            End Set
        End Property

#End Region

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_snSynapticConductance = New AnimatGUI.Framework.ScaledNumber(Me, "SynapticConductance", 0.5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.micro, "Siemens", "S")
            m_snConductionDelay = New AnimatGUI.Framework.ScaledNumber(Me, "ConductionDelay", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")

            'Lets add the data types that this node understands.
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Conductance", "Conductance", "Siemens", "S", 10, 0, ScaledNumber.enumNumericScale.micro, ScaledNumber.enumNumericScale.micro))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Facilitation", "Facilitation", "Siemens", "S", 10, 0, ScaledNumber.enumNumericScale.micro, ScaledNumber.enumNumericScale.micro))
            m_thDataTypes.ID = "Conductance"

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewLink As New DataObjects.Behavior.Synapse(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim blOrig As DataObjects.Behavior.Synapse = DirectCast(doOriginal, DataObjects.Behavior.Synapse)

            m_stSynapseType = blOrig.m_stSynapseType
            m_snSynapticConductance = DirectCast(blOrig.m_snSynapticConductance.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snConductionDelay = DirectCast(blOrig.m_snConductionDelay.Clone(Me, bCutData, doRoot), ScaledNumber)

        End Sub

        Protected Sub OnSynapseTypeChanged()
            UpdateChart(True)
        End Sub

        Public Overrides Sub BeforeAddLink()
            MyBase.BeforeAddLink()

            If Not m_stSynapseType Is Nothing AndAlso m_stSynapseType.GetType() Is GetType(SynapseTypes.SpikingChemical)  Then
                Dim scType As SynapseTypes.SpikingChemical = DirectCast(m_stSynapseType, SynapseTypes.SpikingChemical)
                Me.SynapticConductance = DirectCast(scType.SynapticConductance.Clone(Me, False, Nothing), ScaledNumber)
            End If

        End Sub


        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            'If the synapse type is not set then exit.
            If m_stSynapseType Is Nothing Then
                Return
            End If

            'If either the origin or destinationi is missing then jump out
            If Me.Origin Is Nothing OrElse Me.Destination Is Nothing Then
                Return
            End If

            'Both the origin and destination must be based on a spiking neuron
            If Not Util.IsTypeOf(Me.Origin.GetType(), GetType(DataObjects.Behavior.Neurons.Spiking), False) OrElse _
               Not Util.IsTypeOf(Me.Destination.GetType(), GetType(DataObjects.Behavior.Neurons.Spiking), False) Then
                Return
            End If

            oXml.AddChildElement("Connexion")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("SourceID", Me.Origin.ID)
            'oXml.AddChildElement("Source", Me.Origin.NodeIndex)
            oXml.AddChildElement("TargetID", Me.Destination.ID)
            'oXml.AddChildElement("Target", Me.Destination.NodeIndex)
            oXml.AddChildElement("Type", m_stSynapseType.SynapseType)
            'oXml.AddChildElement("SynapseID", m_stSynapseType.LinkIndex)
            oXml.AddChildElement("SynapseTypeID", m_stSynapseType.ID)
            oXml.AddChildElement("Delay", m_snConductionDelay.ValueFromScale(ScaledNumber.enumNumericScale.milli))
            oXml.AddChildElement("G", m_snSynapticConductance.ValueFromScale(ScaledNumber.enumNumericScale.micro))

            oXml.OutOfElem() 'Outof Connexion

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            'First lets remove the link properties that are not used or can not be set by the synapse.
            If propTable.Properties.Contains("Text") Then propTable.Properties.Remove("Text")
            If propTable.Properties.Contains("Arrow Destination") Then propTable.Properties.Remove("Arrow Destination")
            If propTable.Properties.Contains("Arrow Middle") Then propTable.Properties.Remove("Arrow Middle")
            If propTable.Properties.Contains("Arrow Origin") Then propTable.Properties.Remove("Arrow Origin")
            If propTable.Properties.Contains("Dash Style") Then propTable.Properties.Remove("Dash Style")
            If propTable.Properties.Contains("Draw Color") Then propTable.Properties.Remove("Draw Color")
            If propTable.Properties.Contains("Draw Width") Then propTable.Properties.Remove("Draw Width")
            If propTable.Properties.Contains("Font") Then propTable.Properties.Remove("Font")
            If propTable.Properties.Contains("Link Type") Then propTable.Properties.Remove("Link Type")

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Synapse Class", GetType(String), "TypeName", _
                                        "Synapse Properties", "Returns the classification for the type of this synapse.", TypeName(), True))

            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Synapse Type", GetType(String), "SynapseTypeName", _
            '                            "Synapse Properties", "Returns the name of this synapse type.", TypeName(), True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Synapse Type", GetType(DataObjects.Behavior.SynapseType), "SynapseType", _
                                        "Synapse Properties", "Sets the type of synapse.", m_stSynapseType, _
                                        GetType(TypeHelpers.SynapseTypeEditor), _
                                        GetType(TypeHelpers.SynapseTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strText.GetType(), "UserText", _
                                        "Synapse Properties", "Sets the name of this synapse.", m_strText, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag

            If Not m_stSynapseType Is Nothing AndAlso Not m_stSynapseType.GetType() Is GetType(SynapseTypes.NonSpikingChemical) Then
                pbNumberBag = m_snConductionDelay.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Conduction Delay", pbNumberBag.GetType(), "ConductionDelay", _
                                            "Synapse Properties", "Sets the delay between the spike in the source neuron and the start of the " & _
                                            "conductance change in the target neuron. Acceptable values are in the range 0 to 100 ms", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

            If Not m_stSynapseType Is Nothing AndAlso m_stSynapseType.GetType() Is GetType(SynapseTypes.SpikingChemical) Then
                pbNumberBag = m_snSynapticConductance.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Synaptic Conductance", pbNumberBag.GetType(), "SynapticConductance", _
                                            "Synapse Properties", "Sets the amplitude of the post-synaptic conductance change which this synapse mediates. " & _
                                            "Acceptable values are in the range 0 to 100 uS/size.", _
                                            pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snSynapticConductance Is Nothing Then m_snSynapticConductance.ClearIsDirty()
            If Not m_snConductionDelay Is Nothing Then m_snConductionDelay.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.LoadData(oXml)

                oXml.IntoElem()

                m_strSynapticTypeID = Util.LoadID(oXml, "SynapticType", True, "")
                m_strUserText = oXml.GetChildString("UserText")
                m_snSynapticConductance.LoadData(oXml, "SynapticConductance")
                m_snConductionDelay.LoadData(oXml, "ConductionDelay")

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub InitializeAfterLoad()

            Try
                Dim nmModule As DataObjects.Behavior.NeuralModule = DirectCast(Me.Organism.NeuralModules(Me.NeuralModuleType.FullName()), DataObjects.Behavior.NeuralModule)

                If m_strSynapticTypeID.Trim.Length > 0 Then
                    Me.SynapseType = nmModule.SynapseTypes(m_strSynapticTypeID)
                Else
                    Debug.WriteLine("I failed to load the specified synaptic type: " & m_strSynapticTypeID)
                End If

                If m_stSynapseType Is Nothing Then
                    m_bIsInitialized = False
                End If

                'If Not m_stSynapseType Is Nothing Then
                '    AddHandler m_stSynapseType.SynapseTypeChanged, AddressOf Me.OnSynapseTypeChanged
                'End If

                MyBase.InitializeAfterLoad()

            Catch ex As System.Exception
                m_bIsInitialized = False
            End Try

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.SaveData(oXml)

                oXml.IntoElem() 'Into Node Element

                If Not m_stSynapseType Is Nothing Then
                    oXml.AddChildElement("SynapticTypeID", m_stSynapseType.ID)
                End If

                oXml.AddChildElement("UserText", m_strUserText)
                m_snSynapticConductance.SaveData(oXml, "SynapticConductance")
                m_snConductionDelay.SaveData(oXml, "ConductionDelay")

                oXml.OutOfElem() ' Outof Node Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            'Synpases are stored in the destination neuron object.
            'Synapses are handled different for this model type. They are stored in the neural module, not in the destination neuron.
            If Not Me.NeuralModule Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Me.NeuralModule.ID, "Synapse", Me.ID, Me.GetSimulationXml("Synapse"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            'Synpases are stored in the destination neuron object.
            If Not Me.NeuralModule Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Me.NeuralModule.ID, "Synapse", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

#End Region

#End Region

    End Class

End Namespace
