Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Forms.BodyPlan

	Public Class SelectTerrain
        Inherits AnimatGUI.Forms.AnimatDialog

        Public m_dblSegmentWidth As Double
        Public m_dblSegmentLength As Double
        Public m_dblMaxHeight As Double

        Private Sub btnMeshFileDlg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMeshFileDlg.Click
            Try
                Util.Application.AppIsBusy = True

                'Office Files|*.doc;*.xls;*.ppt
                Dim openFileDialog As New OpenFileDialog
                Dim strFilter As String = "All files|*.bmp;*.gif;*.exif;*.jpg;*.jpeg;*.png;*.tiff;|" & _
                                          "bmp files (*.bmp)|*.bmp|" & _
                                          "gif files (*.gif)|*.gif|" & _
                                          "exif files (*.exif)|*.exif|" & _
                                          "jpg files (*.jpg)|*.jpg|" & _
                                          "jpeg files (*.jpeg)|*.jpeg|" & _
                                          "png files (*.png)|*.png|" & _
                                          "tiff files (*.tiff)|*.tiff"

                openFileDialog.Title = "Select a terrain height image file - hit cancel to select file later."
                openFileDialog.Filter = strFilter
                openFileDialog.InitialDirectory = Util.Application.ProjectPath

                If openFileDialog.ShowDialog() = DialogResult.OK Then
                    txtMeshFile.Text = openFileDialog.FileName
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Util.Application.AppIsBusy = False
            End Try
        End Sub

        Private Sub btnTextureFileDlg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextureFileDlg.Click
            Try
                Util.Application.AppIsBusy = True

                'Office Files|*.doc;*.xls;*.ppt
                Dim openFileDialog As New OpenFileDialog
                Dim strFilter As String = "All files|*.bmp;*.gif;*.exif;*.jpg;*.jpeg;*.png;*.tiff;|" & _
                                          "bmp files (*.bmp)|*.bmp|" & _
                                          "gif files (*.gif)|*.gif|" & _
                                          "exif files (*.exif)|*.exif|" & _
                                          "jpg files (*.jpg)|*.jpg|" & _
                                          "jpeg files (*.jpeg)|*.jpeg|" & _
                                          "png files (*.png)|*.png|" & _
                                          "tiff files (*.tiff)|*.tiff"

                openFileDialog.Title = "Select a terrain texture image file - hit cancel to select file later."
                openFileDialog.Filter = strFilter
                openFileDialog.InitialDirectory = Util.Application.ProjectPath

                If openFileDialog.ShowDialog() = DialogResult.OK Then
                    txtTextureFile.Text = openFileDialog.FileName
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Util.Application.AppIsBusy = False
            End Try
        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                If txtMeshFile.Text.Length = 0 Then
                    Throw New System.Exception("You must specify a height map file.")
                End If

                m_dblSegmentWidth = Util.ValidateNumericTextBox(txtSegmentWidth.Text, True, 0, False, 0, False, "segment width")
                m_dblSegmentLength = Util.ValidateNumericTextBox(txtSegmentLength.Text, True, 0, False, 0, False, "segment length")
                m_dblMaxHeight = Util.ValidateNumericTextBox(txtMaxHeight.Text, True, 0, False, 0, False, "max height")

                DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Sub SetTerrainParameters(ByVal strMeshFile As String, ByVal strTextureFile As String, _
                                                    ByVal dblSegmLength As Double, ByVal dblSegHeight As Double, ByVal dblMaxHeight As Double)
            txtMeshFile.Text = strMeshFile
            txtTextureFile.Text = strTextureFile
            txtSegmentLength.Text = dblSegmLength.ToString
            txtSegmentWidth.Text = dblSegHeight.ToString
            txtMaxHeight.Text = dblMaxHeight.ToString
        End Sub

    End Class

End Namespace
