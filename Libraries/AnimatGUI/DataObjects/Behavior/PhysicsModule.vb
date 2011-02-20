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

    Public Class PhysicsModule
        Inherits NeuralModule

#Region " Properties "

        Public Overrides ReadOnly Property NetworkFilename() As String
            Get
                Return ""
            End Get
        End Property

        Public Overrides ReadOnly Property ModuleFilename() As String
            Get
                Return ""
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_snTimeStep = New AnimatGUI.Framework.ScaledNumber(Me, "TimeStep", 10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
        End Sub

#Region " DataObject Methods "

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewModule As New DataObjects.Behavior.PhysicsModule(doParent)
            oNewModule.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewModule.AfterClone(Me, bCutData, doRoot, oNewModule)
            Return oNewModule
        End Function

        Public Overrides Sub SaveData(ByRef oXml As Interfaces.StdXml)
            PopulateModuleData()
            Me.Organism.AddContactAdapters(Me, m_aryNodes)

            oXml.IntoElem() 'Into the Adapters Element
            Dim bnNode As DataObjects.Behavior.Node
            For Each deEntry As DictionaryEntry In m_aryNodes
                bnNode = DirectCast(deEntry.Value, DataObjects.Behavior.Node)
                bnNode.SaveData(oXml)
            Next
            oXml.OutOfElem()
        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            PopulateModuleData()
            Me.Organism.AddContactAdapters(Me, m_aryNodes)

            oXml.IntoElem() 'Into the Adapters Element
            Dim bnNode As DataObjects.Behavior.Node
            For Each deEntry As DictionaryEntry In m_aryNodes
                bnNode = DirectCast(deEntry.Value, DataObjects.Behavior.Node)
                bnNode.SaveSimulationXml(oXml, Me)
            Next
            oXml.OutOfElem()
        End Sub

#End Region

#End Region

    End Class

End Namespace
