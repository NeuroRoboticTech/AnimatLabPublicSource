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


            For Each liItem As ListViewItem In m_lvItems.Items
                If liItem.Text = strItemName Then
                    'Clear selected items.
                    m_lvItems.SelectedItems.Clear()
                    liItem.Selected = True
                    Return
                End If
            Next

            'If we got here then we did not find what we were looking for.
            Throw New System.Exception("No item named '" & strItemName & "' was found in the list view.")
        End Sub

    End Class

End Namespace

