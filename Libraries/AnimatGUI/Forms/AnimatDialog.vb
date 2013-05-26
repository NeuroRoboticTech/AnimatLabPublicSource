Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGUI
Imports AnimatGUI.Framework

Namespace Forms

    Public Class AnimatDialog
        Inherits Crownwood.DotNetMagic.Forms.DotNetMagicForm

        Protected m_btnOk As Button
        Protected m_btnCancel As Button
        Protected m_btnIgnore As Button
        Protected m_lvItems As ListView
        Protected m_cbItems As ComboBox
        Protected m_tvItems As Crownwood.DotNetMagic.Controls.TreeControl

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Util.AddActiveDialog(Me)
        End Sub

        Protected Overridable Sub AnimatDialog_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
            Util.RemoveActiveDialog(Me)
        End Sub

        Public Overridable Sub ClickOkButton()
            If m_btnOk Is Nothing Then
                Throw New System.Exception("Ok Button is not defined.")
            End If
            m_btnOk.PerformClick()
        End Sub

        Public Overridable Sub ClickCancelButton()
            If m_btnCancel Is Nothing Then
                Throw New System.Exception("Cancel Button is not defined.")
            End If
            m_btnCancel.PerformClick()
        End Sub

        Public Overridable Sub ClickIgnoreButton()
            If m_btnIgnore Is Nothing Then
                Throw New System.Exception("Ignore Button is not defined.")
            End If
            m_btnIgnore.PerformClick()
        End Sub

        Public Overridable Sub SelectItemInListView(ByVal strItemName As String)

            If m_lvItems Is Nothing Then
                Throw New System.Exception("No list view was setup for the form '" & Me.Name & "'")
            End If

            Dim liItem As ListViewItem = Util.FindListItemByName(strItemName, m_lvItems.Items)
            'Clear selected items.
            m_lvItems.SelectedItems.Clear()
            liItem.Selected = True

        End Sub

        Public Sub SetListItemObjectProperty(ByVal strName As String, ByVal strPropertyName As String, ByVal strValue As String)
            If m_lvItems Is Nothing Then
                Throw New System.Exception("No list view was setup for the form '" & Me.Name & "'")
            End If

            Dim lvSelected As ListViewItem = Util.FindListItemByName(strName, m_lvItems.Items)

            If lvSelected.Tag Is Nothing Then
                Throw New System.Exception("No object was found in the list view item '" & strName & "'.")
            End If

            Util.SetObjectProperty(lvSelected.Tag, strPropertyName, strValue)
            m_lvItems.SelectedItems.Clear()
            lvSelected.Selected = True

        End Sub

        Public Function GetListItemObjectProperty(ByVal strName As String, ByVal strPropertyName As String) As Object
            If m_lvItems Is Nothing Then
                Throw New System.Exception("No list view was setup for the form '" & Me.Name & "'")
            End If

            Dim lvSelected As ListViewItem = Util.FindListItemByName(strName, m_lvItems.Items)

            If lvSelected.Tag Is Nothing Then
                Throw New System.Exception("No object was found in the list view item '" & strName & "'.")
            End If

            Return Util.GetObjectProperty(lvSelected.Tag, strPropertyName)
        End Function

        Public Overridable Sub SelectItemInComboBox(ByVal strItemName As String)

            If m_cbItems Is Nothing Then
                Throw New System.Exception("No combo box was setup for the form '" & Me.Name & "'")
            End If

            Dim doItem As Framework.DataObject = Util.FindComboItemByName(strItemName, m_cbItems.Items)
            m_cbItems.SelectedItem = doItem

        End Sub

        Public Overridable Sub SelectItemInTreeView(ByVal strPath As String)

            If m_tvItems Is Nothing Then
                Throw New System.Exception("No tree view was setup for the form '" & Me.Name & "'")
            End If

            Dim tnSelected As Crownwood.DotNetMagic.Controls.Node = Util.FindTreeNodeByPath(strPath, m_tvItems.Nodes)

            tnSelected.Select()

        End Sub

        Public Sub SetTreeNodeObjectProperty(ByVal strPath As String, ByVal strPropertyName As String, ByVal strValue As String)
            If m_tvItems Is Nothing Then
                Throw New System.Exception("No tree view was setup for the form '" & Me.Name & "'")
            End If

            Dim tnSelected As Crownwood.DotNetMagic.Controls.Node = Util.FindTreeNodeByPath(strPath, m_tvItems.Nodes)

            If tnSelected.Tag Is Nothing Then
                Throw New System.Exception("No object was found in the tree node path '" & strPath & "'.")
            End If

            Util.SetObjectProperty(tnSelected.Tag, strPropertyName, strValue)
            tnSelected.Select()

        End Sub

        Public Function GetTreeNodeObjectProperty(ByVal strPath As String, ByVal strPropertyName As String) As Object
            If m_tvItems Is Nothing Then
                Throw New System.Exception("No tree view was setup for the form '" & Me.Name & "'")
            End If

            Dim tnSelected As Crownwood.DotNetMagic.Controls.Node = Util.FindTreeNodeByPath(strPath, m_tvItems.Nodes)

            If tnSelected.Tag Is Nothing Then
                Throw New System.Exception("No object was found in the tree node path '" & strPath & "'.")
            End If

            Return Util.GetObjectProperty(tnSelected.Tag, strPropertyName)
        End Function

    End Class

End Namespace

