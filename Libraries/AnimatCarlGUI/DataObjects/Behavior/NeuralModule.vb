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

    Public Class NeuralModule
        Inherits AnimatGUI.DataObjects.Behavior.NeuralModule

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property NetworkFilename() As String
            Get
                If Not m_doOrganism Is Nothing Then
                    Return m_doOrganism.Name & ".afnn"
                Else
                    Return ""
                End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_strModuleName = "FiringRateSim"
            m_strModuleType = "FiringRateSimModule"
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewModule As New DataObjects.Behavior.NeuralModule(doParent)
            oNewModule.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewModule.AfterClone(Me, bCutData, doRoot, oNewModule)
            Return oNewModule
        End Function

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()  'neuralmodule xml

            Dim bnNode As NeuronGroup
            oXml.AddChildElement("Neurons")
            oXml.IntoElem()
            For Each deEntry As DictionaryEntry In m_aryNodes
                bnNode = DirectCast(deEntry.Value, NeuronGroup)
                bnNode.SaveSimulationXml(oXml, Me)
            Next
            oXml.OutOfElem()

            oXml.OutOfElem()  'neuralmodule xml

        End Sub

#Region " DataObject Methods "

#End Region

#End Region

    End Class

End Namespace
