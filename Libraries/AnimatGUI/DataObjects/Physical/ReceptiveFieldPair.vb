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
                If Not m_doNeuron Is Nothing Then
                    RemoveHandler m_doNeuron.AfterRemoveItem, AddressOf Me.OnNeuronRemoved
                End If

                m_doNeuron = Value

                If Not m_doNeuron Is Nothing Then
                    AddHandler m_doNeuron.AfterRemoveItem, AddressOf Me.OnNeuronRemoved
                End If
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

        Public Sub New(ByVal doParent As Framework.DataObject, ByVal doField As DataObjects.Physical.ReceptiveField, _
                       ByVal doNeuron As DataObjects.Behavior.Nodes.Neuron, ByVal doPart As DataObjects.Physical.RigidBody)
            MyBase.New(doParent)
            Me.Field = doField
            Me.Neuron = doNeuron
            m_doPart = doPart
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doNewRF As New ReceptiveFieldPair(doParent, m_doField, m_doNeuron, m_doPart)
            Return doNewRF
        End Function

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            Me.Neuron = DirectCast(Util.Simulation.FindObjectByID(m_strNeuronID), AnimatGUI.DataObjects.Behavior.Nodes.Neuron)
            Me.Field = DirectCast(Util.Simulation.FindObjectByID(m_strFieldID), AnimatGUI.DataObjects.Physical.ReceptiveField)

            m_doField.FieldPairs.Add(Me.ID, Me)

        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If Not m_doParent Is Nothing Then
                Util.Application.SimulationInterface.AddItem(m_doParent.ID, "FieldPair", Me.ID, Me.GetSimulationXml("FieldPair"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not m_doInterface Is Nothing AndAlso Not m_doParent Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(m_doParent.ID, "FieldPair", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overridable Overloads Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            oXml.IntoElem()
            m_strID = oXml.GetChildString("ID")
            m_strFieldID = oXml.GetChildString("FieldID")
            m_strNeuronID = oXml.GetChildString("NeuronID")
            oXml.OutOfElem()
        End Sub

        Public Overridable Overloads Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("FieldPair")
            oXml.IntoElem()
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("FieldID", m_doField.ID)
            oXml.AddChildElement("NeuronID", m_doNeuron.ID)
            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("FieldPair")
            oXml.IntoElem()

            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("FieldID", m_doField.ID)
            oXml.AddChildElement("TargetNodeID", m_doNeuron.ID)

            oXml.OutOfElem()

        End Sub

#End Region

#Region "Events"

        Private Sub OnNeuronRemoved(ByRef doObject As Framework.DataObject)
            If Not m_doPart Is Nothing AndAlso Not m_doPart.ReceptiveFieldSensor Is Nothing Then
                m_doPart.ReceptiveFieldSensor.RemoveFieldPair(Me)
            End If
        End Sub

#End Region

    End Class

End Namespace



