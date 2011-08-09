Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework

Namespace TypeHelpers

    Public Class ImageFileEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object

            Try
                Dim openFileDialog1 As New OpenFileDialog()

                Dim strFilter As String = "All files|*.bmp;*.gif;*.exif;*.jpg;*.jpeg;*.png;*.tiff;|" & _
                                          "bmp files (*.bmp)|*.bmp|" & _
                                          "gif files (*.gif)|*.gif|" & _
                                          "exif files (*.exif)|*.exif|" & _
                                          "jpg files (*.jpg)|*.jpg|" & _
                                          "jpeg files (*.jpeg)|*.jpeg|" & _
                                          "png files (*.png)|*.png|" & _
                                          "tiff files (*.tiff)|*.tiff"

                openFileDialog1.InitialDirectory = Util.Application.ProjectPath
                openFileDialog1.Filter = strFilter
                openFileDialog1.FilterIndex = 4
                openFileDialog1.RestoreDirectory = True

                If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    Return openFileDialog1.FileName
                End If

                Return value

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
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

