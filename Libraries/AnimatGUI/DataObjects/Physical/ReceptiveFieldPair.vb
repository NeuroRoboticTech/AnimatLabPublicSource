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

Namespace DataObjects.Physical

    Public Class ReceptiveFieldPair
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_doPart As DataObjects.Physical.RigidBody
        Protected m_doField As DataObjects.Physical.ReceptiveField
        Protected m_doNeuron As DataObjects.Behavior.Nodes.Neuron

        Protected m_strFieldID As String = ""
        Protected m_strNeuronID As String

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property Field() As DataObjects.Physical.ReceptiveField
            Get
                Return m_doField
            End Get
            Set(ByVal Value As DataObjects.Physical.ReceptiveField)
                m_doField = Value
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

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType, GetType(DataObjects.Physical.RigidBody)) Then
                m_doPart = DirectCast(doParent, DataObjects.Physical.RigidBody)
            End If
        End Sub

        Public Sub New(ByVal doParent As Framework.DataObject, ByVal doField As DataObjects.Physical.ReceptiveField, ByVal doNeuron As DataObjects.Behavior.Nodes.Neuron)
            MyBase.New(doParent)
            m_doField = doField
            m_doNeuron = doNeuron

            If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType, GetType(DataObjects.Physical.RigidBody)) Then
                m_doPart = DirectCast(doParent, DataObjects.Physical.RigidBody)
            End If
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doNewRF As New ReceptiveFieldPair(doParent, m_doField, m_doNeuron)
            Return doNewRF
        End Function

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            m_doNeuron = DirectCast(Util.Simulation.FindObjectByID(m_strNeuronID), AnimatGUI.DataObjects.Behavior.Nodes.Neuron)
            m_doField = DirectCast(Util.Simulation.FindObjectByID(m_strFieldID), AnimatGUI.DataObjects.Physical.ReceptiveField)

            m_doField.FieldPairs.Add(Me.ID, Me)

        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub BeforeAddToList(Optional ByVal bThrowError As Boolean = True)
            If Not m_doParent Is Nothing Then
                MyBase.BeforeAddToList(bThrowError)
                Util.Application.SimulationInterface.AddItem(m_doParent.ID, "FieldPair", Me.GetSimulationXml("FieldPair"), bThrowError)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub BeforeRemoveFromList(Optional ByVal bThrowError As Boolean = True)
            MyBase.BeforeRemoveFromList(bThrowError)

            If Not m_doInterface Is Nothing AndAlso Not m_doParent Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(m_doParent.ID, "FieldPair", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overridable Overloads Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            oXml.IntoElem()
            m_strID = oXml.GetChildString("ID")
            m_strFieldID = oXml.GetChildString("FieldID")
            m_strNeuronID = oXml.GetChildString("NeuronID")
            oXml.OutOfElem()
        End Sub

        Public Overridable Overloads Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement("FieldPair")
            oXml.IntoElem()
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("FieldID", m_doField.ID)
            oXml.AddChildElement("NeuronID", m_doNeuron.ID)
            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("FieldPair")
            oXml.IntoElem()

            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("FieldID", m_doField.ID)
            oXml.AddChildElement("TargetNodeID", m_doNeuron.ID)

            oXml.OutOfElem()

        End Sub

#End Region

    End Class

End Namespace



