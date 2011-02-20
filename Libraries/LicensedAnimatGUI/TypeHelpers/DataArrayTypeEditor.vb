Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatTools.Framework

Namespace TypeHelpers

    Public Class DataArrayTypeEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            Dim frmEditor As New Forms.Charts.DataArrayEditor

            Try

                If (Not context Is Nothing AndAlso Not context.Instance Is Nothing AndAlso Not provider Is Nothing AndAlso Not value Is Nothing) Then

                    Util.ModificationHistory.AllowAddHistory = False
                    frmEditor.ParentChart = DirectCast(value, Forms.Charts.DataArray)
                    If frmEditor.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    End If

                    Return Nothing
                End If

                Return value

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)

                If Not frmEditor Is Nothing Then
                    Try
                        frmEditor.Close()
                        frmEditor = Nothing
                    Catch ex1 As System.Exception

                    End Try
                End If
            Finally
                Util.ModificationHistory.AllowAddHistory = True
            End Try

        End Function

        Public Overloads Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
            If (Not context Is Nothing And Not context.Instance Is Nothing) Then
                ' we'll show a modal form
                Return UITypeEditorEditStyle.Modal
            End If
            Return MyBase.GetEditStyle(context)
        End Function
    End Class

End Namespace

