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

Namespace DataObjects.Behavior

    Public Class NeuronGroup
        Inherits AnimatGUI.DataObjects.Behavior.Nodes.Neuron

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Neuron Group"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(AnimatCarlGUI.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        Public Overridable ReadOnly Property NeuronType() As String
            Get
                Return "Regular"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property SimClassName() As String
            Get
                Return Me.NeuronType
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatCarlGUI.NormalNeuron.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Try

                m_bEnabled = True

                Shape = AnimatGUI.DataObjects.Behavior.Node.enumShape.Ellipse
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.Black
                Me.FillColor = Color.White

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatCarlGUI")

                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "AnimatCarlGUI.NormalNeuron.gif", False)
                Me.Name = "Firing Rate Neuron"

                Me.Font = New Font("Arial", 14, FontStyle.Bold)
                Me.Description = "A group of Izhikevich spiking neurons."

                AddCompatibleLink(New AnimatGUI.DataObjects.Behavior.Links.Adapter(Nothing))
                AddCompatibleLink(New SynapseGroup(Nothing))

                'Lets add the data types that this node understands.
                'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("IntrinsicCurrent", "Intrinsic Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
                'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("ExternalCurrent", "External Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
                'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("AdapterCurrent", "Adapter Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
                'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("SynapticCurrent", "Synaptic Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
                'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MembraneVoltage", "Membrane Voltage", "Volts", "V", -100, 100, ScaledNumber.enumNumericScale.milli, ScaledNumber.enumNumericScale.milli))
                'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("FiringFrequency", "Firing Frequency", "Hertz", "Hz", 0, 1000))
                'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("NoiseVoltage", "Noise Voltage", "Volts", "V", -100, 100, ScaledNumber.enumNumericScale.milli, ScaledNumber.enumNumericScale.milli))
                'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Threshold", "Threshold", "Volts", "V", -100, 100, ScaledNumber.enumNumericScale.milli, ScaledNumber.enumNumericScale.milli))
                'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("AccomTimeMod", "Accom Time Modulation", "Time", "s", 0, 10, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
                'm_thDataTypes.ID = "FiringFrequency"

                'm_thIncomingDataTypes.DataTypes.Clear()
                'm_thIncomingDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("ExternalCurrent", "External Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
                'm_thIncomingDataTypes.ID = "ExternalCurrent"

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub InitAfterAppStart()
            MyBase.InitAfterAppStart()
            'AddCompatibleStimulusType("Current")
            'AddCompatibleStimulusType("InverseMuscleCurrent")
            'AddCompatibleStimulusType("VoltageClamp")
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New NeuronGroup(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As NeuronGroup = DirectCast(doOriginal, NeuronGroup)

            m_bEnabled = bnOrig.m_bEnabled

        End Sub

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            MyBase.InitializeSimulationReferences(bShowError)

            Dim doObject As AnimatGUI.Framework.DataObject
            For Each deEntry As DictionaryEntry In m_aryInLinks
                doObject = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                doObject.InitializeSimulationReferences(bShowError)
            Next
        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            'First lets remove the 'Text' property for node base classs
            If propTable.Properties.Contains("Text") Then propTable.Properties.Remove("Text")
            If propTable.Properties.Contains("Node Type") Then propTable.Properties.Remove("Node Type")
            If propTable.Properties.Contains("Description") Then propTable.Properties.Remove("Description")

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strText.GetType(), "Text", _
                                        "Neural Properties", "Sets the name of this neuron.", m_strText, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Neuron Type", GetType(String), "TypeName", _
                                        "Neural Properties", "Returns the type of this neuron.", TypeName(), True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Description", m_strDescription.GetType(), "ToolTip", _
                                        "Neural Properties", "Sets the description for this neuron.", m_strToolTip, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                        "Neural Properties", "Determines if this neuron is enabled or not.", m_bEnabled))


        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_bEnabled = oXml.GetChildBool("Enabled", True)


            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            oXml.AddChildElement("Enabled", m_bEnabled)


            oXml.OutOfElem() ' Outof Node Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("Neuron")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Text)
            oXml.AddChildElement("Type", Me.NeuronType)
            oXml.AddChildElement("Enabled", Me.Enabled)

            oXml.OutOfElem() 'Outof Neuron

        End Sub

#End Region

#End Region

    End Class

End Namespace

