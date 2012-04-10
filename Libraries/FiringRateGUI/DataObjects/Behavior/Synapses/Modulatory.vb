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

Namespace DataObjects.Behavior.Synapses

    Public Class Modulated
        Inherits FastSynapse

#Region " Attributes "

        Protected m_strUserText As String = ""
        Protected m_snGain As AnimatGUI.Framework.ScaledNumber
        Protected m_lsModulatedSynapse As LinkedSynapse

        'Only used during loading
        Protected m_strModulatedSynapseID As String
        Protected m_strModulatedOriginID As String

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Modulatory Synapse"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(FiringRateGUI.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        Public Overrides Property ActualDestination() As AnimatGUI.DataObjects.Behavior.Node
            Get
                Return MyBase.ActualDestination
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.Node)
                m_bnDestination = Value

                If Not Value Is Nothing AndAlso TypeOf Value Is AnimatGUI.DataObjects.Behavior.Nodes.OffPage Then
                    Dim doNode As AnimatGUI.DataObjects.Behavior.Nodes.OffPage = DirectCast(Value, AnimatGUI.DataObjects.Behavior.Nodes.OffPage)
                    If Not doNode.LinkedNode Is Nothing AndAlso Not doNode.LinkedNode.Node Is Nothing Then
                        m_lsModulatedSynapse.Node = doNode.LinkedNode.Node
                    Else
                        Throw New System.Exception("The linked node of the destination offpage connector is not set.")
                    End If
                Else
                    m_lsModulatedSynapse.Node = Value
                End If

                UpdateChart()
            End Set
        End Property

        Public Overrides Property Destination() As AnimatGUI.DataObjects.Behavior.Node
            Get
                Return MyBase.Destination
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.Node)
                m_bnDestination = Value

                If Not Value Is Nothing AndAlso TypeOf Value Is AnimatGUI.DataObjects.Behavior.Nodes.OffPage Then
                    Dim doNode As AnimatGUI.DataObjects.Behavior.Nodes.OffPage = DirectCast(Value, AnimatGUI.DataObjects.Behavior.Nodes.OffPage)
                    If Not doNode.LinkedNode Is Nothing AndAlso Not doNode.LinkedNode.Node Is Nothing Then
                        m_lsModulatedSynapse.Node = doNode.LinkedNode.Node
                    Else
                        Throw New System.Exception("The linked node of the destination offpage connector is not set.")
                    End If
                Else
                    m_lsModulatedSynapse.Node = Value
                End If

                UpdateChart()
            End Set
        End Property

        Public Overridable Property UserText() As String
            Get
                Return m_strUserText
            End Get
            Set(ByVal Value As String)
                m_strUserText = Value

                If m_strUserText.Trim.Length > 0 Then
                    Me.Text = m_snGain.Text & vbCrLf & Replace(m_strUserText, vbCrLf, "")
                Else
                    Me.Text = m_snGain.Text
                End If
            End Set
        End Property

        Public Overridable Property Gain() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snGain
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                If Not m_lsModulatedSynapse Is Nothing AndAlso Not m_lsModulatedSynapse.Link Is Nothing Then
                    SetSimData("Weight", Value.ActualValue.ToString, True)
                End If

                'If they are changing sign of the weight to be negative then lets change the destination arrow.
                If Value.Value < 0 AndAlso m_snGain.Value > 0 Then
                    Me.ArrowDestination.Filled = True
                ElseIf Value.Value > 0 AndAlso m_snGain.Value < 0 Then
                    Me.ArrowDestination.Filled = True
                End If

                m_snGain.CopyData(Value)

                If m_strUserText.Trim.Length > 0 Then
                    Me.Text = m_snGain.Text & vbCrLf & m_strUserText
                Else
                    Me.Text = m_snGain.Text
                End If
            End Set
        End Property

        Public Overridable Property ModulatedSynapse() As LinkedSynapse
            Get
                Return m_lsModulatedSynapse
            End Get
            Set(ByVal Value As LinkedSynapse)
                'We must first get rid of the current synapse and then replace it with a new one.
                RemoveFromSim(True)

                m_lsModulatedSynapse.Link = Value.Link

                Try
                    AddToSim(True)
                Catch ex As Exception
                    m_lsModulatedSynapse.Link = Nothing
                    Throw ex
                End Try

                UpdateChart()
                UpdateTreeNode()
                CheckForErrors()
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property ItemName() As String
            Get
                Dim strName As String = ""
                If Not Me.Origin Is Nothing Then
                    Dim strModulated As String = ""
                    If Not ModulatedSynapse Is Nothing Then
                        strModulated = ModulatedSynapse.Link.ItemName
                    End If
                    If Me.Text.Trim.Length = 0 Then
                        strName = Me.Origin.Text & strModulated
                    Else
                        strName = Me.Origin.Text.Trim & " ([W " & Me.Text.Trim & "] " & strModulated & ")"
                    End If
                Else
                    strName = Me.Text
                End If

                Return strName
            End Get
            Set(ByVal Value As String)
                Me.Text = Value
            End Set
        End Property

        Public Overrides ReadOnly Property SynapseType() As String
            Get
                Return "Modulated"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "FiringRateGUI.ModulatorySynapse.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_bEnabled = True
            m_lsModulatedSynapse = New LinkedSynapse(m_bnDestination, Nothing)
            m_snGain = New AnimatGUI.Framework.ScaledNumber(Me, "Gain", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")

            Me.DrawColor = Color.Black

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("FiringRateGUI")

            Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "FiringRateGUI.ModulatorySynapse.gif", False)
            Me.Name = "Modulatory Synapse"

            Me.ArrowDestination = New Arrow(Me, AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Losange, AnimatGUI.DataObjects.Behavior.Link.enumArrowSize.Medium, AnimatGUI.DataObjects.Behavior.Link.enumArrowAngle.deg30, False)

            Me.Font = New Font("Arial", 12)
            Me.Description = "A modulatory heterosynaptic synapse."
            Me.Text = m_snGain.Text

            'Lets add the data types that this node understands.
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Modulation", "Modulation", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewLink As New Synapses.Modulated(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnLink As Synapses.Modulated = DirectCast(doOriginal, Synapses.Modulated)

            m_bEnabled = bnLink.m_bEnabled
            m_strUserText = bnLink.m_strUserText
            m_lsModulatedSynapse = DirectCast(bnLink.m_lsModulatedSynapse.Clone(Me, bCutData, doRoot), LinkedSynapse)
            m_snGain = DirectCast(bnLink.m_snGain.Clone(Me, bCutData, doRoot), ScaledNumber)
        End Sub

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList)

            m_lsModulatedSynapse.AddToReplaceIDList(aryReplaceIDList)
        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            'Only save this as a synapse if the origin node is another FastNeuralNet neuron
            If Not Util.IsTypeOf(Me.Origin.GetType(), GetType(DataObjects.Behavior.Neurons.Normal), False) Then
                Return
            End If

            Dim fnNeuron As DataObjects.Behavior.Neurons.Normal = DirectCast(Me.Origin, DataObjects.Behavior.Neurons.Normal)

            oXml.AddChildElement("CompoundSynapse")
            oXml.IntoElem()

            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Type", Me.SynapseType)
            oXml.AddChildElement("Enabled", m_bEnabled)
            oXml.AddChildElement("FromID", fnNeuron.ID)
            oXml.AddChildElement("Weight", m_snGain.ActualValue)

            oXml.OutOfElem()

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("Text") Then propTable.Properties.Remove("Text")
            If propTable.Properties.Contains("Link Type") Then propTable.Properties.Remove("Link Type")

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Modulated Synapse", m_lsModulatedSynapse.GetType(), "ModulatedSynapse", _
                                        "Synapse Properties", "Sets the secondary syanpse that will be modulated.", m_lsModulatedSynapse, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), GetType(LinkedSynapseTypeConverter)))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snGain.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gain", pbNumberBag.GetType(), "Gain", _
                                        "Synapse Properties", "Sets the gain of this modulatory synaptic connection.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Text", m_strUserText.GetType(), "UserText", _
                                        "Synapse Properties", "Sets or returns the user text associated with the link.", _
                                        m_strUserText, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Synapse Type", GetType(String), "TypeName", _
                                        "Synapse Properties", "Returns the type of this link.", TypeName(), True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                        "Synapse Properties", "Determines if this synapse is enabled or not.", m_bEnabled))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snGain Is Nothing Then m_snGain.ClearIsDirty()
            If Not m_lsModulatedSynapse Is Nothing Then m_lsModulatedSynapse.ClearIsDirty()
        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If Not m_lsModulatedSynapse Is Nothing AndAlso Not m_lsModulatedSynapse.Link Is Nothing Then
                Util.Application.SimulationInterface.AddItem(m_lsModulatedSynapse.Link.ID, "Synapse", Me.ID, Me.GetSimulationXml("Synapse"), True, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            'However, we do need to remove it if it is removed from the list.
            If Not m_lsModulatedSynapse Is Nothing AndAlso Not m_lsModulatedSynapse.Link Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(m_lsModulatedSynapse.Link.ID, "Synapse", Me.ID, bThrowError)
                m_doInterface = Nothing
            End If
        End Sub

        Public Overrides Sub BeforeAddToList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            'We do not add this one when it is added to the list. Instead, it is added when the user selects the linked synapse.
        End Sub

        Public Overrides Sub BeforeRemoveFromList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            'We do not want to call the base class here because we are doing a completely different simint.RemoveItem
            Me.SignalBeforeRemoveItem(Me)

            If bCallSimMethods Then RemoveFromSim(bThrowError)
        End Sub

#End Region

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.LoadData(oXml)

                oXml.IntoElem()

                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)
                m_strUserText = oXml.GetChildString("UserText")
                m_snGain.LoadData(oXml, "Gain")
                m_strModulatedSynapseID = Util.LoadID(oXml, "ModulatedSynapse", True, "")
                m_strModulatedOriginID = Util.LoadID(oXml, "ModulatedOrigin", True, "")

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub InitializeAfterLoad()

            Try
                MyBase.InitializeAfterLoad()

                If m_bIsInitialized Then
                    If m_strModulatedSynapseID.Trim.Length > 0 Then
                        Dim blLink As AnimatGUI.DataObjects.Behavior.Link = Me.Organism.FindBehavioralLink(m_strModulatedSynapseID)
                        Dim bnNode As AnimatGUI.DataObjects.Behavior.Node = Me.Organism.FindBehavioralNode(m_strModulatedOriginID)

                        m_lsModulatedSynapse = New FiringRateGUI.DataObjects.Behavior.LinkedSynapse(bnNode, blLink)
                    Else
                        m_lsModulatedSynapse = New FiringRateGUI.DataObjects.Behavior.LinkedSynapse(m_bnDestination, Nothing)
                    End If
                End If

            Catch ex As System.Exception
                m_bIsInitialized = False
            End Try

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.SaveData(oXml)

                oXml.IntoElem() 'Into Node Element

                oXml.AddChildElement("Enabled", m_bEnabled)
                oXml.AddChildElement("UserText", m_strUserText)
                m_snGain.SaveData(oXml, "Gain")

                If Not m_lsModulatedSynapse Is Nothing AndAlso Not m_lsModulatedSynapse.Link Is Nothing Then
                    oXml.AddChildElement("ModulatedSynapseID", m_lsModulatedSynapse.Link.ID)
                    oXml.AddChildElement("ModulatedOriginID", m_lsModulatedSynapse.Node.ID)
                End If

                oXml.OutOfElem() ' Outof Node Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub Automation_SetLinkedItem(ByVal strItemPath As String, ByVal strLinkedItemPath As String)

            Dim tnLinkedNode As Crownwood.DotNetMagic.Controls.Node = Util.FindTreeNodeByPath(strLinkedItemPath, Util.ProjectWorkspace.TreeView.Nodes)

            If tnLinkedNode Is Nothing OrElse tnLinkedNode.Tag Is Nothing OrElse Not Util.IsTypeOf(tnLinkedNode.Tag.GetType, GetType(DataObjects.Behavior.Synapses.Normal), False) Then
                Throw New System.Exception("The path to the specified linked node was not the correct link type.")
            End If

            Dim blLinkedSynapse As AnimatGUI.DataObjects.Behavior.Link = DirectCast(tnLinkedNode.Tag, AnimatGUI.DataObjects.Behavior.Link)

            Dim lsModuledSynapse As LinkedSynapse = New FiringRateGUI.DataObjects.Behavior.LinkedSynapse(blLinkedSynapse.Origin, blLinkedSynapse)

            Me.ModulatedSynapse = lsModuledSynapse
            Util.ProjectWorkspace.RefreshProperties()
        End Sub

#End Region

#End Region

    End Class

End Namespace