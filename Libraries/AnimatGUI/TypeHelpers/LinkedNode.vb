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

Namespace TypeHelpers

    Public Class LinkedNode
        Inherits AnimatGUI.Framework.DataObject

#Region " Attributes "

        Protected m_doOrganism As AnimatGUI.DataObjects.Physical.Organism
        Protected m_bnLinkedNode As AnimatGUI.DataObjects.Behavior.Node

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Property Organism() As AnimatGUI.DataObjects.Physical.Organism
            Get
                Return m_doOrganism
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.Organism)
                m_doOrganism = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Property Node() As AnimatGUI.DataObjects.Behavior.Node
            Get
                Return m_bnLinkedNode
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.Node)
                m_bnLinkedNode = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property ViewSubProperties() As Boolean
            Get
                Return False
            End Get
            Set(ByVal Value As Boolean)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Sub New(ByVal doOrganism As AnimatGUI.DataObjects.Physical.Organism, _
                       ByVal bnLinkedNode As AnimatGUI.DataObjects.Behavior.Node)
            MyBase.New(bnLinkedNode)

            m_doOrganism = doOrganism
            m_bnLinkedNode = bnLinkedNode
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim origNode As New LinkedNode(doParent)
            origNode.m_doOrganism = m_doOrganism
            origNode.m_bnLinkedNode = m_bnLinkedNode
            Return origNode
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Sub BuildPropertyDropDown(ByRef ctrlDropDown As System.Windows.Forms.Control)
            If m_doOrganism Is Nothing Then Return

            If Not TypeOf (ctrlDropDown) Is Crownwood.DotNetMagic.Controls.TreeControl Then
                Throw New System.Exception("The control passed into BuildPropertyDropDown is not a treeview type")
            End If

            Dim tvTree As Crownwood.DotNetMagic.Controls.TreeControl = DirectCast(ctrlDropDown, Crownwood.DotNetMagic.Controls.TreeControl)

            tvTree.SuspendLayout()
            tvTree.Nodes.Clear()

            m_doOrganism.CreateNodeTreeView(tvTree)

            tvTree.Width = 300
            tvTree.ExpandAll()

            MyBase.FormatDropDownTree(tvTree, 8)

            tvTree.ResumeLayout()
            tvTree.Invalidate()

        End Sub

#End Region

    End Class

End Namespace
