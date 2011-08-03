Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI.Framework

Namespace DataObjects.Physical

    Public Class ReceptiveFieldPair
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_vVertex As Vec3d
        Protected m_doNeuron As DataObjects.Behavior.Nodes.Neuron
        Protected m_strNeuronID As String

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property Vertex() As Vec3d
            Get
                Return m_vVertex
            End Get
            Set(ByVal Value As Vec3d)
                m_vVertex = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Neuron() As DataObjects.Behavior.Nodes.Neuron
            Get
                Return m_doNeuron
            End Get
            Set(ByVal Value As DataObjects.Behavior.Nodes.Neuron)
                m_doNeuron = Value
            End Set
        End Property

        Public Overridable Function IsValidPair() As Boolean
            If m_doNeuron Is Nothing Then Return False

            'First verify that the neuron is still around
            Dim doStruct As AnimatGUI.DataObjects.Physical.PhysicalStructure = Util.Environment.FindOrganism(m_doNeuron.Organism.ID, False)

            If doStruct Is Nothing Then
                Return False
            End If

            Dim doOrganism As AnimatGUI.DataObjects.Physical.Organism = DirectCast(doStruct, AnimatGUI.DataObjects.Physical.Organism)
            Dim doNeuron As AnimatGUI.DataObjects.Behavior.Node = doOrganism.FindBehavioralNode(m_doNeuron.ID, False)

            If doNeuron Is Nothing Then
                Return False
            End If

            'Now lets try and verify that the vertex is still valid
            If Me.Parent Is Nothing OrElse Not TypeOf Me.Parent Is AnimatGUI.DataObjects.Physical.RigidBody Then
                Return False
            End If

            Dim doBody As AnimatGUI.DataObjects.Physical.RigidBody = DirectCast(Me.Parent, AnimatGUI.DataObjects.Physical.RigidBody)

            'Only test this if we have previously generated the list of vertices. This list is not always generated. If the list is empty
            'then we should just assume that this vertex is okay.
            If doBody.ReceptiveFields.Count > 0 Then
                Dim iIndex As Integer = -1
                'doBody.DumpReceptiveFields()
                If Not doBody.FindReceptiveField(CSng(m_vVertex.X), CSng(m_vVertex.Y), CSng(m_vVertex.Z), iIndex) Then
                    Return False
                End If
            End If

            Return True
        End Function

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Sub New(ByVal doParent As Framework.DataObject, ByVal vVertex As Vec3d, ByVal doNeuron As DataObjects.Behavior.Nodes.Neuron)
            MyBase.New(doParent)
            m_vVertex = vVertex
            m_doNeuron = doNeuron
            m_strID = doNeuron.ID & "_" & vVertex.ToString()
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doNewRF As New ReceptiveFieldPair(doParent, m_vVertex, m_doNeuron)
            Return doNewRF
        End Function

        Public Overrides Sub InitializeAfterLoad()
            m_doNeuron = DirectCast(Util.Simulation.FindObjectByID(m_strNeuronID), AnimatGUI.DataObjects.Behavior.Nodes.Neuron)
        End Sub

        Public Overridable Overloads Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            oXml.IntoElem()
            m_vVertex = Util.LoadVec3d(oXml, "Vertex", Nothing)
            m_strNeuronID = oXml.GetChildString("NeuronID")
            oXml.OutOfElem()
        End Sub

        Public Overridable Overloads Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)

            If TypeOf doStructure Is AnimatGUI.DataObjects.Physical.Organism Then
                Dim doOrganism As AnimatGUI.DataObjects.Physical.Organism = DirectCast(doStructure, AnimatGUI.DataObjects.Physical.Organism)

                Dim doNode As Framework.DataObject = doOrganism.FindObjectByID(m_doNeuron.ID)
                If Not doNode Is Nothing Then
                    Dim doNeuron As AnimatGUI.DataObjects.Behavior.Node = DirectCast(doNode, AnimatGUI.DataObjects.Behavior.Node)

                    oXml.AddChildElement("Pair")
                    oXml.IntoElem()

                    Util.SaveVector(oXml, "Vertex", m_vVertex)
                    oXml.AddChildElement("NeuronID", doNeuron.ID)

                    oXml.OutOfElem()
                End If
            End If

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            If TypeOf nmParentControl Is AnimatGUI.DataObjects.Physical.Organism Then
                Dim doOrganism As AnimatGUI.DataObjects.Physical.Organism = DirectCast(nmParentControl, AnimatGUI.DataObjects.Physical.Organism)

                'We need to get a new reference to the neuron here because it may be different than the one we originally got. The reason is that when the
                'project is first loaded we load in a list of the neurons. But if the user opens the behavioral editor then we need to reload that list because
                'we have to seperate out the neurons further by subsystem. So the second time they are loaded they would be a different object. Items like the 
                'ID should not be different, but changes to the node index would be different.
                Dim doNode As Framework.DataObject = doOrganism.FindObjectByID(m_doNeuron.ID)
                If Not doNode Is Nothing Then
                    Dim doNeuron As AnimatGUI.DataObjects.Behavior.Node = DirectCast(doNode, AnimatGUI.DataObjects.Behavior.Node)

                    oXml.AddChildElement("Pair")
                    oXml.IntoElem()

                    Util.SaveVector(oXml, "Vertex", m_vVertex)
                    oXml.AddChildElement("TargetNodeID", doNeuron.ID)

                    oXml.OutOfElem()
                End If
            End If

        End Sub

#End Region

    End Class

End Namespace



