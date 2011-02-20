Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework
Imports System.IO
Imports System.Drawing

Namespace TypeHelpers

    Public Class ImageTypeEditor
        Inherits UITypeEditor
        Private service As IWindowsFormsEditorService

        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object

            Try

                If (Not context Is Nothing And Not context.Instance Is Nothing And Not provider Is Nothing) Then
                    service = CType(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
                    If (Not service Is Nothing) AndAlso ((TypeOf (value) Is AnimatGUI.DataObjects.Behavior.DiagramImage) OrElse value Is Nothing) Then

                        Dim openFileDialog As New OpenFileDialog
                        openFileDialog.Filter = "All files (*.*)|*.*"
                        'openFileDialog.Filter = "*.gif|*.fig|*.jpg|*.jpg|*.jpeg|*.jpeg|*.jpe|*.jpe|*.png|*.png|" & _
                        '                        "*.bmp|*.bmp|*.dib|*.dib|*.tif|*.tif|*.wmf|*.wmf|*.ras|*.ras|" & _
                        '                        "*.eps|*.eps|*.pcx|*.pcx|*.pcd|*.pcd|*.tga|*.tga"
                        openFileDialog.Title = "Open an Image"

                        Dim diImage As DataObjects.Behavior.DiagramImage
                        Dim doParent As Framework.DataObject

                        If Not value Is Nothing Then
                            diImage = DirectCast(value, DataObjects.Behavior.DiagramImage)
                            openFileDialog.FileName = diImage.FilePath
                            doParent = diImage.Parent
                        End If

                        If openFileDialog.ShowDialog() = DialogResult.OK Then
                            Dim imgNew As New Bitmap(openFileDialog.FileName)

                            diImage = New DataObjects.Behavior.DiagramImage(doParent)
                            diImage.WorkspaceImage = imgNew

                            Dim strFilename As String = openFileDialog.FileName
                            If Util.IsFullPath(strFilename) Then
                                Dim strPath As String, strFile As String
                                If Util.DetermineFilePath(strFilename, strPath, strFile) Then
                                    strFilename = strFile
                                End If
                            End If

                            diImage.FilePath = strFilename
                            diImage.ID = strFilename
                            diImage.UserImage = True

                            Return diImage
                        Else
                            Return value
                        End If

                    End If
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
