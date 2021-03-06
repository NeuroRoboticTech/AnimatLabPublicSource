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

    Public Class Gated
        Inherits FastSynapse

#Region " Attributes "

        Protected m_strUserText As String = ""
        Protected m_snWeight As AnimatGUI.Framework.ScaledNumber
        Protected m_bGateInitiallyOn As Boolean
        Protected m_lsGatedSynapse As LinkedSynapse

        'Only used during loading
        Protected m_strGatedSynapseID As String
        Protected m_strGatedOriginID As String

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Gated Synapse"
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
                        m_lsGatedSynapse.Node = doNode.LinkedNode.Node
                    Else
                        Throw New System.Exception("The linked node of the destination offpage connector is not set.")
                    End If
                Else
                    m_lsGatedSynapse.Node = Value
                End If

                UpdateChart()
            End Set
        End Property

        Public Overrides Property Destination() As AnimatGUI.DataObjects.Behavior.Node
            Get
                Return MyBase.Destination
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.Node)
                RemoveFromInLinks(m_bnDestination)

                m_bnDestination = Value
                AddToInLinks(m_bnDestination)

                If Not Value Is Nothing AndAlso TypeOf Value Is AnimatGUI.DataObjects.Behavior.Nodes.OffPage Then
                    Dim doNode As AnimatGUI.DataObjects.Behavior.Nodes.OffPage = DirectCast(Value, AnimatGUI.DataObjects.Behavior.Nodes.OffPage)
                    If Not doNode.LinkedNode Is Nothing AndAlso Not doNode.LinkedNode.Node Is Nothing Then
                        m_lsGatedSynapse.Node = doNode.LinkedNode.Node
                    Else
                        Throw New System.Exception("The linked node of the destination offpage connector is not set.")
                    End If
                Else
                    m_lsGatedSynapse.Node = Value
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
                    Me.Text = m_snWeight.Text & vbCrLf & Replace(m_strUserText, vbCrLf, "")
                Else
                    Me.Text = m_snWeight.Text
                End If
            End Set
        End Property

        Public Overridable Property Weight() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snWeight
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                If Not m_lsGatedSynapse Is Nothing AndAlso Not m_lsGatedSynapse.Link Is Nothing Then
                    SetSimData("Weight", Value.ActualValue.ToString, True)
                End If

                'If they are changing sign of the weight to be negative then lets change the destination arrow.
                If Value.Value < 0 AndAlso m_snWeight.Value > 0 Then
                    Me.ArrowDestination.Filled = True
                ElseIf Value.Value > 0 AndAlso m_snWeight.Value < 0 Then
                    Me.ArrowDestination.Filled = True
                End If

                m_snWeight.CopyData(Value)

                If m_strUserText.Trim.Length > 0 Then
                    Me.Text = m_snWeight.Text & vbCrLf & m_strUserText
                Else
                    Me.Text = m_snWeight.Text
                End If
            End Set
        End Property

        Public Overridable Property GatedSynapse() As LinkedSynapse
            Get
                Return m_lsGatedSynapse
            End Get
            Set(ByVal Value As LinkedSynapse)
                'We must first get rid of the current synapse and then replace it with a new one.
                RemoveFromSim(True)

                'Now lets reset the link.
                m_lsGatedSynapse.Link = Value.Link

                Try
                    AddToSim(True)
                Catch ex As Exception
                    m_lsGatedSynapse.Link = Nothing
                    Throw ex
                End Try

                UpdateChart()
                UpdateTreeNode()
                CheckForErrors()
            End Set
        End Property

        Public Overridable Property GateInitiallyOn() As Boolean
            Get
                Return m_bGateInitiallyOn
            End Get
            Set(ByVal Value As Boolean)

                If Not m_lsGatedSynapse Is Nothing AndAlso Not m_lsGatedSynapse.Link Is Nothing Then
                    SetSimData("GateInitiallyOn", Value.ToString, True)
                End If
                m_bGateInitiallyOn = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property ItemName() As String
            Get
                Dim strName As String = ""
                If Not Me.Origin Is Nothing Then
                    Dim strGated As String = ""
                    If Not GatedSynapse Is Nothing AndAlso Not GatedSynapse.Link Is Nothing Then
                        strGated = GatedSynapse.Link.ItemName
                    End If
                    If Me.Text.Trim.Length = 0 Then
                        strName = Me.Origin.Text & strGated
                    Else
                        strName = Me.Origin.Text.Trim & " ([W " & Me.Text.Trim & "] " & strGated & ")"
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
                Return "Gated"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "FiringRateGUI.GatedSynapse.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_bEnabled = True
            m_lsGatedSynapse = New LinkedSynapse(m_bnDestination, Nothing)
            m_snWeight = New AnimatGUI.Framework.ScaledNumber(Me, "Weight", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")

            Me.DrawColor = Color.Black

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("FiringRateGUI")

            Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "FiringRateGUI.GatedSynapse.gif", False)
            Me.Name = "Gated Synapse"

            Me.ArrowDestination = New Arrow(Me, AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.ClosedFork, AnimatGUI.DataObjects.Behavior.Link.enumArrowSize.Medium, AnimatGUI.DataObjects.Behavior.Link.enumArrowAngle.deg30, False)

            Me.Font = New Font("Arial", 12)
            Me.Description = "A gated heterosynaptic synapse."
            Me.Text = m_snWeight.Text

            'Lets add the data types that this node understands.
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Modulation", "Modulation", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewLink As New Synapses.Gated(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnLink As Synapses.Gated = DirectCast(doOriginal, Synapses.Gated)

            m_bEnabled = bnLink.m_bEnabled
            m_strUserText = bnLink.m_strUserText
            m_lsGatedSynapse = DirectCast(bnLink.m_lsGatedSynapse.Clone(Me, bCutData, doRoot), LinkedSynapse)
            m_bGateInitiallyOn = bnLink.m_bGateInitiallyOn
            m_snWeight = DirectCast(bnLink.m_snWeight.Clone(Me, bCutData, doRoot), ScaledNumber)
        End Sub

        Public Overrides Sub BeforeCopy(ByVal arySelectedItems As ArrayList)

            'If the destination is not something being copied then we cannot copy this link without causing an error
            If Not m_lsGatedSynapse Is Nothing AndAlso Not m_lsGatedSynapse.Link Is Nothing AndAlso Not m_lsGatedSynapse.Link.Destination Is Nothing Then
                If Not arySelectedItems.Contains(m_lsGatedSynapse.Link.Destination) Then
                    Me.DeselectItem()
                End If
            End If

        End Sub

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList, ByVal arySelectedItems As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)

            m_lsGatedSynapse.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
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
            oXml.AddChildElement("Weight", m_snWeight.ActualValue)

            If m_bGateInitiallyOn Then
                oXml.AddChildElement("InitialGateValue", 1)
            Else
                oXml.AddChildElement("InitialGateValue", 0)
            End If

            oXml.OutOfElem()

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("Text") Then propTable.Properties.Remove("Text")
            If propTable.Properties.Contains("Link Type") Then propTable.Properties.Remove("Link Type")

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gated Synapse", m_lsGatedSynapse.GetType(), "GatedSynapse", _
                                        "Synapse Properties", "Sets the secondary syanpse that will be gated.", m_lsGatedSynapse, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), GetType(LinkedSynapseTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gate Initially On", m_bGateInitiallyOn.GetType(), "GateInitiallyOn", _
                                        "Synapse Properties", "Sets whether the gate for this synapse is initially on or off.", m_bGateInitiallyOn))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snWeight.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Weight", pbNumberBag.GetType(), "Weight", _
                                        "Synapse Properties", "Sets the weight of this synaptic connection.", pbNumberBag, _
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

            If Not m_snWeight Is Nothing Then m_snWeight.ClearIsDirty()
            If Not m_lsGatedSynapse Is Nothing Then m_lsGatedSynapse.ClearIsDirty()
        End Sub

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            'We should only try and initialize the sim references here if the modulated synapse is set.
            If Not Me.GatedSynapse Is Nothing AndAlso Not Me.GatedSynapse.Link Is Nothing Then
                MyBase.InitializeSimulationReferences(bShowError)
            End If
        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            Try
                If Not m_lsGatedSynapse Is Nothing AndAlso Not m_lsGatedSynapse.Link Is Nothing Then
                    Util.Application.SimulationInterface.AddItem(m_lsGatedSynapse.Link.ID, "Synapse", Me.ID, Me.GetSimulationXml("Synapse"), True, bDoNotInit)
                    InitializeSimulationReferences()
                End If
            Catch ex As Exception
                Util.ShowMessage("Error occured while adding a gated synapse. This is possible if the gated node is not part of the copied nodes. " & _
                                 "This is a bu I plan to resolve, but for now it will not correctly copy this link '" & Me.Text & "'. Please delete this node manually." & vbCrLf & _
                                 "Original Error message: " & ex.Message)
            End Try
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            'However, we do need to remove it if it is removed from the list.
            If Not m_lsGatedSynapse Is Nothing AndAlso Not m_lsGatedSynapse.Link Is Nothing AndAlso Not m_doInterface Is Nothing Then
                If Util.Application.SimulationInterface.FindItem(m_lsGatedSynapse.Link.ID, False) Then
                    Util.Application.SimulationInterface.RemoveItem(m_lsGatedSynapse.Link.ID, "Synapse", Me.ID, bThrowError)
                    m_doInterface = Nothing
                End If
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
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)
            m_strUserText = oXml.GetChildString("UserText")
            m_bGateInitiallyOn = oXml.GetChildBool("GateInitiallyOn")
            m_snWeight.LoadData(oXml, "Weight")
            m_strGatedSynapseID = Util.LoadID(oXml, "GatedSynapse", True, "")
            m_strGatedOriginID = Util.LoadID(oXml, "GatedOrigin", True, "")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub InitializeAfterLoad()

            Try
                MyBase.InitializeAfterLoad()

                If m_bIsInitialized Then
                    If m_strGatedSynapseID.Trim.Length > 0 Then
                        Dim blLink As AnimatGUI.DataObjects.Behavior.Link = Me.Organism.FindBehavioralLink(m_strGatedSynapseID)
                        Dim bnNode As AnimatGUI.DataObjects.Behavior.Node = Me.Organism.FindBehavioralNode(m_strGatedOriginID)

                        m_lsGatedSynapse = New FiringRateGUI.DataObjects.Behavior.LinkedSynapse(bnNode, blLink)
                    Else
                        m_lsGatedSynapse = New FiringRateGUI.DataObjects.Behavior.LinkedSynapse(m_bnDestination, Nothing)
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
                oXml.AddChildElement("GateInitiallyOn", m_bGateInitiallyOn)
                m_snWeight.SaveData(oXml, "Weight")

                If Not m_lsGatedSynapse Is Nothing AndAlso Not m_lsGatedSynapse.Link Is Nothing AndAlso Not m_lsGatedSynapse.Node Is Nothing Then
                    oXml.AddChildElement("GatedSynapseID", m_lsGatedSynapse.Link.ID)
                    oXml.AddChildElement("GatedOriginID", m_lsGatedSynapse.Node.ID)
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

            Dim lsGatedSynapse As LinkedSynapse = New FiringRateGUI.DataObjects.Behavior.LinkedSynapse(blLinkedSynapse.Origin, blLinkedSynapse)

            Me.GatedSynapse = lsGatedSynapse
            Util.ProjectWorkspace.RefreshProperties()
        End Sub

#End Region

#End Region

    End Class

End Namespace
