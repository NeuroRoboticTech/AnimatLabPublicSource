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
Imports AnimatCarlGUI.DataObjects.Behavior.NodeTypes
Imports AnimatCarlGUI.DataObjects.Behavior.SynapseTypes

Namespace DataObjects.Behavior.SynapseTypes

    Public Class SpikingCurrentSynapse
        Inherits AnimatGUI.DataObjects.Behavior.Links.Synapse

#Region " Attributes "

        Protected m_strUserText As String = ""

        Protected m_eCoverage As enumCoverage = enumCoverage.WholePopulation

        Protected m_aryCellsToMonitor As New Collections.NeuralIndices(Me)

        Protected m_snPulseDecay As AnimatGUI.Framework.ScaledNumber
        Protected m_snPulseCurrent As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property ModuleName As String
            Get
                Return "AnimatCarlSimCUDA"
            End Get
        End Property

        Public Overrides ReadOnly Property ModuleFilename As String
            Get
                Return "AnimatCarlSimCUDA" & Util.Application.Physics.SimVCVersion & Util.Application.Physics.RuntimeModePrefix & Util.Application.Physics.BinaryModPrefix & Util.Application.Physics.LibraryExtension
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property SimClassName() As String
            Get
                Return "SpikingCurrentSynapse"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(AnimatCarlGUI.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        Public Overridable Property UserText() As String
            Get
                Return m_strUserText
            End Get
            Set(ByVal Value As String)
                m_strUserText = Value

                'If m_strUserText.Trim.Length > 0 Then
                '    Me.Text = m_snWeight.Text & vbCrLf & Replace(m_strUserText, vbCrLf, "")
                'Else
                '    Me.Text = m_snWeight.Text
                'End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Coverage() As enumCoverage
            Get
                Return m_eCoverage
            End Get
            Set(value As enumCoverage)
                SetSimData("Coverage", value.ToString(), True)
                m_eCoverage = value

                If Not Util.ProjectWorkspace Is Nothing Then
                    Util.ProjectWorkspace.RefreshProperties()
                End If
            End Set
        End Property

        Public Overridable Property CellsToMonitor As Collections.NeuralIndices
            Get
                Return m_aryCellsToMonitor
            End Get
            Set(value As Collections.NeuralIndices)
                'Set nothing here. It is set in the property editor

                'We do need to reset the list of indices in the simulator though
                Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()
                oXml.AddElement("Root")
                oXml.AddChildElement("Cells")
                oXml.IntoElem()
                For Each iIdx As Integer In m_aryCellsToMonitor
                    oXml.AddChildElement("Cell", iIdx)
                Next
                oXml.IntoElem()

                SetSimData("Cells", oXml.Serialize(), True)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property PulseDecay() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snPulseDecay
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    If Value.ActualValue <= 0 Then
                        Throw New System.Exception("The pulse decay rate must be greater than zero.")
                    End If

                    SetSimData("PulseDecay", Value.ActualValue.ToString(), True)
                    If Not Value Is Nothing Then m_snPulseDecay.CopyData(Value)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property PulseCurrent() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snPulseCurrent
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("PulseCurrent", Value.ActualValue.ToString(), True)
                    If Not Value Is Nothing Then m_snPulseCurrent.CopyData(Value)
                End If
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_bEnabled = True

            Me.DrawColor = Color.Black

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatCarlGUI")

            Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "AnimatCarlGUI.ExcitatorySynapse.gif", False)
            Me.Name = "Spiking Current Synapse"

            Me.ArrowDestination = New Arrow(Me, AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Fork, AnimatGUI.DataObjects.Behavior.Link.enumArrowSize.Medium, AnimatGUI.DataObjects.Behavior.Link.enumArrowAngle.deg30, False)

            Me.Font = New Font("Arial", 12)
            Me.Description = "A connection from a CarlSIM neuron to a neuron in another module."

            m_snPulseDecay = New AnimatGUI.Framework.ScaledNumber(Me, "PulseDecay", 30, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            m_snPulseCurrent = New AnimatGUI.Framework.ScaledNumber(Me, "PulseCurrent", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Amp", "A")

            'Lets add the data types that this node understands.
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("AppliedCurrent", "AppliedCurrent", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("DecrementCurrent", "DecrementCurrent", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
            m_thDataTypes.ID = "AppliedCurrent"
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewLink As New SpikingCurrentSynapse(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnLink As SpikingCurrentSynapse = DirectCast(doOriginal, SpikingCurrentSynapse)

            m_bEnabled = bnLink.m_bEnabled
            m_strUserText = bnLink.m_strUserText
            m_eCoverage = bnLink.m_eCoverage
            m_aryCellsToMonitor = DirectCast(bnLink.m_aryCellsToMonitor.Clone(Me, bCutData, doRoot), Collections.NeuralIndices)
            m_snPulseDecay = DirectCast(bnLink.m_snPulseDecay.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snPulseCurrent = DirectCast(bnLink.m_snPulseCurrent.Clone(Me, bCutData, doRoot), ScaledNumber)
        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            'Only save this as a synapse if the origin node is another FastNeuralNet neuron
            If Not Util.IsTypeOf(Me.Origin.GetType(), GetType(NeuronGroup), False) Then
                Return
            End If

            Dim fnFrom As NeuronGroup = DirectCast(Me.Origin, NeuronGroup)
            Dim fnTo As AnimatGUI.DataObjects.Behavior.Node = DirectCast(Me.Destination, AnimatGUI.DataObjects.Behavior.Node)

            oXml.AddChildElement("Synapse")
            oXml.IntoElem()

            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Module", Me.ModuleName)
            oXml.AddChildElement("ModuleFilename", Me.ModuleFilename)
            oXml.AddChildElement("Type", SimClassName())
            oXml.AddChildElement("Enabled", m_bEnabled)
            oXml.AddChildElement("FromID", fnFrom.ID)
            oXml.AddChildElement("ToID", fnTo.ID)

            m_snPulseDecay.SaveSimulationXml(oXml, Me, "PulseDecay")
            m_snPulseCurrent.SaveSimulationXml(oXml, Me, "PulseCurrent")

            oXml.AddChildElement("Coverage", m_eCoverage.ToString())

            If m_eCoverage = enumCoverage.Individuals AndAlso m_aryCellsToMonitor.Count > 0 Then
                oXml.AddChildElement("Cells")
                oXml.IntoElem()  'Into Cells

                For Each iCell As Integer In m_aryCellsToMonitor
                    oXml.AddChildElement("Cell", iCell)
                Next

                oXml.OutOfElem() 'Outof Cells
            End If

            oXml.OutOfElem()

        End Sub

#Region " DataObject Methods "

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            'Synpases are stored in the destination neuron object.
            If Not Me.Destination Is Nothing AndAlso Not Me.Destination.NeuralModule Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Me.Destination.NeuralModule.ID, "ExternalSynapse", Me.ID, Me.GetSimulationXml("Synapse"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            'Synpases are stored in the destination neuron object.
            If Not Me.Destination Is Nothing AndAlso Not Me.Destination.NeuralModule Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Me.Destination.NeuralModule.ID, "ExternalSynapse", Me.ID, bThrowError)
                m_doInterface = Nothing
            End If
        End Sub

        Public Overrides Sub AfterAddToList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            Me.SignalAfterAddItem(Me)

            If Not Me.Destination Is Nothing AndAlso Not Me.Destination.NeuralModule Is Nothing Then
                Me.Destination.NeuralModule.Links.Add(Me.ID, Me)
            End If
        End Sub

        Public Overrides Sub AfterRemoveFromList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
            Me.SignalAfterRemoveItem(Me)

            If Not NeuralModule Is Nothing AndAlso NeuralModule.Links.Contains(Me.ID) Then
                Me.Destination.NeuralModule.Links.Remove(Me.ID)
            End If
        End Sub

#End Region

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            If Not Me.Destination Is Nothing AndAlso Not Me.Destination.NeuralModule Is Nothing AndAlso Not Me.Destination.NeuralModule.Links.Contains(Me.ID) Then
                Me.Destination.NeuralModule.Links.Add(Me.ID, Me)
            End If
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("Text") Then propTable.Properties.Remove("Text")
            If propTable.Properties.Contains("Link Type") Then propTable.Properties.Remove("Link Type")

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Text", m_strUserText.GetType(), "UserText", _
                                        "Synapse Properties", "Sets or returns the user text associated with the link.", _
                                        m_strUserText, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Synapse Type", GetType(String), "TypeName", _
                                        "Synapse Properties", "Returns the type of this link.", TypeName(), True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                        "Synapse Properties", "Determines if this synapse is enabled or not.", m_bEnabled))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snPulseDecay.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Pulse Decay Rate", pbNumberBag.GetType(), "PulseDecay", _
                                        "Synapse Properties", "Sets the decay rate of the current pulse that will be applied when a spike occurs. ", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snPulseCurrent.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Pulse Current", pbNumberBag.GetType(), "PulseCurrent", _
                                        "Synapse Properties", "Sets the current that will be applied for a fixed duration when a spike occurs. ", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Coverage", m_eCoverage.GetType(), "Coverage", _
                                        "Synapse Properties", "Determines whether we monitor all cells in the entire population or individual cells.", m_eCoverage))

            If m_eCoverage = enumCoverage.Individuals Then
                pbNumberBag = m_aryCellsToMonitor.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Cells to Monitor", pbNumberBag.GetType(), "CellsToMonitor", _
                                            "Synapse Properties", "The list of cell indices to monitor.", pbNumberBag, _
                                            GetType(TypeHelpers.SelectedIndexEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))
            End If
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_aryCellsToMonitor.ClearIsDirty()
            m_snPulseDecay.ClearIsDirty()
            m_snPulseCurrent.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.LoadData(oXml)

                oXml.IntoElem()

                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)
                m_strUserText = oXml.GetChildString("UserText")

                m_snPulseDecay.LoadData(oXml, "PulseDecay")
                m_snPulseCurrent.LoadData(oXml, "PulseCurrent")

                m_eCoverage = DirectCast([Enum].Parse(GetType(enumCoverage), oXml.GetChildString("Coverage"), True), enumCoverage)

                m_aryCellsToMonitor.Clear()
                If oXml.FindChildElement("Cells", False) Then
                    oXml.IntoElem()

                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    For iIdx As Integer = 0 To iCount
                        oXml.FindChildByIndex(iIdx)
                        Dim iNeuronIdx As Integer = oXml.GetChildInt()
                        m_aryCellsToMonitor.Add(iNeuronIdx)
                    Next

                    oXml.OutOfElem()
                End If

                oXml.OutOfElem()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.SaveData(oXml)

                oXml.IntoElem() 'Into Node Element

                oXml.AddChildElement("Enabled", m_bEnabled)
                oXml.AddChildElement("UserText", m_strUserText)

                m_snPulseDecay.SaveData(oXml, "PulseDecay")
                m_snPulseCurrent.SaveData(oXml, "PulseCurrent")

                oXml.AddChildElement("Coverage", m_eCoverage.ToString)

                If m_eCoverage = enumCoverage.Individuals AndAlso m_aryCellsToMonitor.Count > 0 Then
                    oXml.AddChildElement("Cells")
                    oXml.IntoElem()  'Into Cells

                    For Each iCell As Integer In m_aryCellsToMonitor
                        oXml.AddChildElement("Cell", iCell)
                    Next

                    oXml.OutOfElem() 'Outof Cells
                End If

                oXml.OutOfElem() ' Outof Node Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#End Region

    End Class

End Namespace
