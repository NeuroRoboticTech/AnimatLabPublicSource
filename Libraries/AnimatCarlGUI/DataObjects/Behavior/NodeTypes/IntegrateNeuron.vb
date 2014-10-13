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

    Public Class IntegrateNeuron
        Inherits AnimatGUI.DataObjects.Behavior.Nodes.Neuron

#Region " Attributes "


#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Integrate Neuron"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(AnimatCarlGUI.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        Public Overridable ReadOnly Property NeuronType() As String
            Get
                Return "IntegrateNeuron"
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
                Me.Name = "Integrate Neuron"

                Me.Font = New Font("Arial", 14, FontStyle.Bold)
                Me.Description = "A neuron that integrates inputs from Izhikevich spiking neurons."

                AddCompatibleLink(New AnimatGUI.DataObjects.Behavior.Links.Adapter(Nothing))
                'AddCompatibleLink(New SynapseTypes.OneToOneSynapse(Nothing))

                'Lets add the data types that this node understands.
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MembraneVoltage", "Membrane Voltage", "Volts", "V", -100, 100, ScaledNumber.enumNumericScale.milli, ScaledNumber.enumNumericScale.milli))
                m_thDataTypes.ID = "MembraneVoltage"

                m_thIncomingDataTypes.DataTypes.Clear()
                m_thIncomingDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Spikes", "Spikes", "", "", 0, 100, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
                m_thIncomingDataTypes.ID = "Spikes"

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub InitAfterAppStart()
            MyBase.InitAfterAppStart()
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New IntegrateNeuron(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As IntegrateNeuron = DirectCast(doOriginal, IntegrateNeuron)


        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)


        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()


        End Sub

        Public Overrides Function CreateNewAdapter(ByRef bnOrigin As AnimatGUI.DataObjects.Behavior.Node, ByRef doParent As AnimatGUI.Framework.DataObject) As AnimatGUI.DataObjects.Behavior.Node

            ''If it does require an adapter then lets add the pieces.
            Dim bnAdapter As AnimatGUI.DataObjects.Behavior.Node
            If bnOrigin.IsPhysicsEngineNode AndAlso Not Me.IsPhysicsEngineNode Then
                'If the origin is physics node and the destination is a regular node
                Throw New System.Exception("You cannot create an adpater coming into a regular neuron group. You must use a Spiking Generator and stimulate it instead.")
            ElseIf Not bnOrigin.IsPhysicsEngineNode AndAlso Me.IsPhysicsEngineNode Then
                'If the origin is regular node and the destination is a physics node
                bnAdapter = New AnimatGUI.DataObjects.Behavior.Nodes.NodeToPhysicalAdapter(doParent)
            ElseIf Not bnOrigin.IsPhysicsEngineNode AndAlso Not Me.IsPhysicsEngineNode Then
                'If both the origin and destination are regular nodes.
                Throw New System.Exception("You cannot create an adpater coming into a regular neuron group. You must use a Spiking Generator and stimulate it instead.")
            Else
                'If both the origin and destination are physics nodes.
                Throw New System.Exception("You can only link two physics nodes using a graphical link.")
            End If

            Return bnAdapter
        End Function

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

