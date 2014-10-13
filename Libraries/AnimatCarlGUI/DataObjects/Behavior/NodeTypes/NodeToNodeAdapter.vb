Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace DataObjects.Behavior.NodeTypes

    Public Class NodeToNodeAdapter
        Inherits AnimatGUI.DataObjects.Behavior.Nodes.NodeToNodeAdapter

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property ModuleName As String
            Get
                Return "AnimatCarlSimCUDA"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Me.Name = "Node To Node Adapter"
            Me.Description = "Provides an interface adapter between two nodes."
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New NodeToNodeAdapter(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub SaveSimulationXmlInternal(oXml As ManagedAnimatInterfaces.IStdXml, nmSource As AnimatGUI.DataObjects.Behavior.NeuralModule, _
                                                          nmTarget As AnimatGUI.DataObjects.Behavior.NeuralModule, bnOrigin As AnimatGUI.DataObjects.Behavior.Node, _
                                                          bnDestination As AnimatGUI.DataObjects.Behavior.Node, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, _
                                                          Optional strName As String = "")
            MyBase.SaveSimulationXmlInternal(oXml, nmSource, nmTarget, bnOrigin, bnDestination, nmParentControl, strName)

            oXml.IntoElem()
            oXml.OutOfElem()
        End Sub

#Region " DataObject Methods "

#End Region

#End Region

    End Class

End Namespace
