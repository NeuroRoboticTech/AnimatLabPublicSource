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

Namespace DataObjects.Behavior.NodeTypes

    Public Class SpikeGeneratorGroup
        Inherits NeuronGroup

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Spike Generator Group"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuronType() As String
            Get
                Return "SpikeGeneratorGroup"
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
                Me.Name = "Spike Generator Group"

                Me.Font = New Font("Arial", 14, FontStyle.Bold)
                Me.Description = "A group of spike generators."

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
            AddCompatibleStimulusType("FiringRate")
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New SpikeGeneratorGroup(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As SpikeGeneratorGroup = DirectCast(doOriginal, SpikeGeneratorGroup)

            m_bEnabled = bnOrig.m_bEnabled

        End Sub

        Public Overrides Function CreateNewAdapter(ByRef bnOrigin As AnimatGUI.DataObjects.Behavior.Node, ByRef doParent As AnimatGUI.Framework.DataObject) As AnimatGUI.DataObjects.Behavior.Node

            ''If it does require an adapter then lets add the pieces.
            Dim bnAdapter As AnimatGUI.DataObjects.Behavior.Node
            If bnOrigin.IsPhysicsEngineNode AndAlso Not Me.IsPhysicsEngineNode Then
                'If the origin is physics node and the destination is a regular node
                bnAdapter = New AnimatCarlGUI.DataObjects.Behavior.NodeTypes.PhysicalToNodeAdapter(doParent)
            ElseIf Not bnOrigin.IsPhysicsEngineNode AndAlso Me.IsPhysicsEngineNode Then
                'If the origin is regular node and the destination is a physics node
                bnAdapter = New AnimatGUI.DataObjects.Behavior.Nodes.NodeToPhysicalAdapter(doParent)
            ElseIf Not bnOrigin.IsPhysicsEngineNode AndAlso Not Me.IsPhysicsEngineNode Then
                'If both the origin and destination are regular nodes.
                bnAdapter = New AnimatCarlGUI.DataObjects.Behavior.NodeTypes.NodeToNodeAdapter(doParent)
            Else
                'If both the origin and destination are physics nodes.
                Throw New System.Exception("You can only link two physics nodes using a graphical link.")
            End If

            Return bnAdapter
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("A") Then propTable.Properties.Remove("A")
            If propTable.Properties.Contains("B") Then propTable.Properties.Remove("B")
            If propTable.Properties.Contains("C") Then propTable.Properties.Remove("C")
            If propTable.Properties.Contains("D") Then propTable.Properties.Remove("D")

            If propTable.Properties.Contains("A StdDev") Then propTable.Properties.Remove("A StdDev")
            If propTable.Properties.Contains("B StdDev") Then propTable.Properties.Remove("B StdDev")
            If propTable.Properties.Contains("C StdDev") Then propTable.Properties.Remove("C StdDev")
            If propTable.Properties.Contains("D StdDev") Then propTable.Properties.Remove("D StdDev")

            ' If propTable.Properties.Contains("Enable COBA") Then propTable.Properties.Remove("Enable COBA")

            If propTable.Properties.Contains("TauAMPA") Then propTable.Properties.Remove("TauAMPA")
            If propTable.Properties.Contains("TauNMDA") Then propTable.Properties.Remove("TauNMDA")
            If propTable.Properties.Contains("TauGABAa") Then propTable.Properties.Remove("TauGABAa")
            If propTable.Properties.Contains("TauGABAb") Then propTable.Properties.Remove("TauGABAb")

            If propTable.Properties.Contains("Enable STP") Then propTable.Properties.Remove("Enable STP")
            If propTable.Properties.Contains("U") Then propTable.Properties.Remove("U")
            If propTable.Properties.Contains("Tau Depression") Then propTable.Properties.Remove("Tau Depression")
            If propTable.Properties.Contains("Tau Facilitation") Then propTable.Properties.Remove("Tau Facilitation")

            If propTable.Properties.Contains("Enable STDP") Then propTable.Properties.Remove("Enable STDP")
            If propTable.Properties.Contains("Max LTP") Then propTable.Properties.Remove("Max LTP")
            If propTable.Properties.Contains("Tau LTP") Then propTable.Properties.Remove("Tau LTP")
            If propTable.Properties.Contains("Max LTD") Then propTable.Properties.Remove("Max LTD")
            If propTable.Properties.Contains("Tau LTD") Then propTable.Properties.Remove("Tau LTD")

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            oXml.OutOfElem() ' Outof Node Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            oXml.OutOfElem() 'Outof Neuron

        End Sub

#End Region

#End Region

    End Class

End Namespace

