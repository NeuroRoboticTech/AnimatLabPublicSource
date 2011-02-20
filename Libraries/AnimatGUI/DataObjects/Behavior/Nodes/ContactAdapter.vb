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

Namespace DataObjects.Behavior.Nodes

    Public Class ContactAdapter
        Inherits Behavior.Nodes.Adapter

#Region " Attributes "

        Protected m_doRigidBody As DataObjects.Physical.RigidBody
        Protected m_aryReceptiveFieldPairs As New ArrayList
        Protected m_nmTargetModule As NeuralModule

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Contact Adapter"
            End Get
        End Property

        Public Overrides ReadOnly Property AdapterType() As String
            Get
                Return "Contact"
            End Get
        End Property

        Public Overridable ReadOnly Property ReceptiveFieldPairs() As ArrayList
            Get
                Return m_aryReceptiveFieldPairs
            End Get
        End Property

        Public Overridable Property RigidBody() As DataObjects.Physical.RigidBody
            Get
                Return m_doRigidBody
            End Get
            Set(ByVal Value As DataObjects.Physical.RigidBody)
                m_doRigidBody = Value
            End Set
        End Property

        Public Overridable Property TargetNeuralModule() As DataObjects.Behavior.NeuralModule
            Get
                Return m_nmTargetModule
            End Get
            Set(ByVal Value As DataObjects.Behavior.NeuralModule)
                m_nmTargetModule = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Me.Name = "Contact Adapter"
            Me.Description = "Provides an interface adapter between an receptive field contacts and neurons."
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.ContactAdapter(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            If Not m_doRigidBody Is Nothing AndAlso m_aryReceptiveFieldPairs.Count > 0 Then
                Dim doStruct As Framework.DataObject = m_doRigidBody.ParentStructure

                oXml.AddChildElement("Adapter")
                oXml.IntoElem()

                oXml.AddChildElement("Type", Me.AdapterType)

                oXml.AddChildElement("SourceBodyID", m_doRigidBody.ID)
                oXml.AddChildElement("TargetModule", m_nmTargetModule.ModuleName)

                oXml.AddChildElement("FieldPairs")
                oXml.IntoElem()
                For Each doPair As DataObjects.Physical.ReceptiveFieldPair In m_aryReceptiveFieldPairs
                    doPair.SaveSimulationXml(oXml, doStruct)
                Next
                oXml.OutOfElem()

                oXml.OutOfElem()
            End If

        End Sub

        Public Overrides Sub InitializeSimulationReferences()
            MyBase.InitializeSimulationReferences()

            For Each doPair As DataObjects.Physical.ReceptiveFieldPair In m_aryReceptiveFieldPairs
                doPair.InitializeSimulationReferences()
            Next
        End Sub

#Region " DataObject Methods "

#End Region

#End Region

    End Class

End Namespace


