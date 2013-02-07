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

    Public Class LinkedDataObjectTree
        Inherits TypeHelpers.LinkedDataObject

#Region " Attributes "

#End Region

#Region " Properties "

#End Region

#Region " Methods "

        Public Sub New(ByVal doItem As AnimatGUI.Framework.DataObject)
            MyBase.New(doItem)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNew As New LinkedDataObjectTree(doParent)
            oNew.CloneInternal(Me, bCutData, doRoot)
            Return oNew
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Sub BuildPropertyDropDown(ByRef ctrlDropDown As System.Windows.Forms.Control)

            If Not TypeOf (ctrlDropDown) Is Crownwood.DotNetMagic.Controls.TreeControl Then
                Throw New System.Exception("The control passed into LinkedSynapse.BuildPropertyDropDown is not a treeview type")
            End If

            Dim tvTree As Crownwood.DotNetMagic.Controls.TreeControl = DirectCast(ctrlDropDown, Crownwood.DotNetMagic.Controls.TreeControl)
            Dim mgrWorkspaceImages As New AnimatGUI.Framework.ImageManager

            mgrWorkspaceImages.ImageList.ImageSize = New Size(25, 25)
            tvTree.ImageList = mgrWorkspaceImages.ImageList

            tvTree.SuspendLayout()
            tvTree.Nodes.Clear()

            Dim tnRoot As Crownwood.DotNetMagic.Controls.Node = Util.Simulation.CreateObjectListTreeView(Nothing, Nothing, mgrWorkspaceImages)
            tvTree.Nodes.Add(tnRoot)

            tvTree.Width = 300
            tvTree.ExpandAll()

            MyBase.FormatDropDownTree(tvTree, 8)

            tvTree.ResumeLayout()
            tvTree.Invalidate()

        End Sub

#End Region

    End Class

End Namespace
