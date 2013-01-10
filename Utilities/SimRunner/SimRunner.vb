Imports System.IO

Public Class Form1
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents txtSimFiles As System.Windows.Forms.TextBox
    Friend WithEvents btnSimFiles As System.Windows.Forms.Button
    Friend WithEvents lblSimFiles As System.Windows.Forms.Label
    Friend WithEvents lblResultFiles As System.Windows.Forms.Label
    Friend WithEvents btnResultFiles As System.Windows.Forms.Button
    Friend WithEvents txtResultFiles As System.Windows.Forms.TextBox
    Friend WithEvents lblCommonFiles As System.Windows.Forms.Label
    Friend WithEvents btnCommonFiles As System.Windows.Forms.Button
    Friend WithEvents txtCommonFiles As System.Windows.Forms.TextBox
    Friend WithEvents lblSourceFiles As System.Windows.Forms.Label
    Friend WithEvents btnSourceFiles As System.Windows.Forms.Button
    Friend WithEvents txtSourceFiles As System.Windows.Forms.TextBox
    Friend WithEvents btnProcess As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents barProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.txtSimFiles = New System.Windows.Forms.TextBox()
        Me.btnSimFiles = New System.Windows.Forms.Button()
        Me.lblSimFiles = New System.Windows.Forms.Label()
        Me.lblResultFiles = New System.Windows.Forms.Label()
        Me.btnResultFiles = New System.Windows.Forms.Button()
        Me.txtResultFiles = New System.Windows.Forms.TextBox()
        Me.lblCommonFiles = New System.Windows.Forms.Label()
        Me.btnCommonFiles = New System.Windows.Forms.Button()
        Me.txtCommonFiles = New System.Windows.Forms.TextBox()
        Me.lblSourceFiles = New System.Windows.Forms.Label()
        Me.btnSourceFiles = New System.Windows.Forms.Button()
        Me.txtSourceFiles = New System.Windows.Forms.TextBox()
        Me.btnProcess = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.barProgress = New System.Windows.Forms.ProgressBar()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtSimFiles
        '
        Me.txtSimFiles.Location = New System.Drawing.Point(8, 40)
        Me.txtSimFiles.Name = "txtSimFiles"
        Me.txtSimFiles.Size = New System.Drawing.Size(480, 20)
        Me.txtSimFiles.TabIndex = 0
        Me.txtSimFiles.Text = "C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Utilities\SimRunner\SimFiles"
        '
        'btnSimFiles
        '
        Me.btnSimFiles.Location = New System.Drawing.Point(496, 40)
        Me.btnSimFiles.Name = "btnSimFiles"
        Me.btnSimFiles.Size = New System.Drawing.Size(16, 16)
        Me.btnSimFiles.TabIndex = 1
        Me.btnSimFiles.Text = "..."
        '
        'lblSimFiles
        '
        Me.lblSimFiles.Location = New System.Drawing.Point(8, 24)
        Me.lblSimFiles.Name = "lblSimFiles"
        Me.lblSimFiles.Size = New System.Drawing.Size(480, 16)
        Me.lblSimFiles.TabIndex = 2
        Me.lblSimFiles.Text = "Sim Files"
        Me.lblSimFiles.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblResultFiles
        '
        Me.lblResultFiles.Location = New System.Drawing.Point(8, 64)
        Me.lblResultFiles.Name = "lblResultFiles"
        Me.lblResultFiles.Size = New System.Drawing.Size(480, 16)
        Me.lblResultFiles.TabIndex = 5
        Me.lblResultFiles.Text = "Result Files"
        Me.lblResultFiles.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnResultFiles
        '
        Me.btnResultFiles.Location = New System.Drawing.Point(496, 80)
        Me.btnResultFiles.Name = "btnResultFiles"
        Me.btnResultFiles.Size = New System.Drawing.Size(16, 16)
        Me.btnResultFiles.TabIndex = 4
        Me.btnResultFiles.Text = "..."
        '
        'txtResultFiles
        '
        Me.txtResultFiles.Location = New System.Drawing.Point(8, 80)
        Me.txtResultFiles.Name = "txtResultFiles"
        Me.txtResultFiles.Size = New System.Drawing.Size(480, 20)
        Me.txtResultFiles.TabIndex = 3
        Me.txtResultFiles.Text = "C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Utilities\SimRunner\Results"
        '
        'lblCommonFiles
        '
        Me.lblCommonFiles.Location = New System.Drawing.Point(8, 104)
        Me.lblCommonFiles.Name = "lblCommonFiles"
        Me.lblCommonFiles.Size = New System.Drawing.Size(480, 16)
        Me.lblCommonFiles.TabIndex = 8
        Me.lblCommonFiles.Text = "Common Files"
        Me.lblCommonFiles.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnCommonFiles
        '
        Me.btnCommonFiles.Location = New System.Drawing.Point(496, 120)
        Me.btnCommonFiles.Name = "btnCommonFiles"
        Me.btnCommonFiles.Size = New System.Drawing.Size(16, 16)
        Me.btnCommonFiles.TabIndex = 7
        Me.btnCommonFiles.Text = "..."
        '
        'txtCommonFiles
        '
        Me.txtCommonFiles.Location = New System.Drawing.Point(8, 120)
        Me.txtCommonFiles.Name = "txtCommonFiles"
        Me.txtCommonFiles.Size = New System.Drawing.Size(480, 20)
        Me.txtCommonFiles.TabIndex = 6
        Me.txtCommonFiles.Text = "C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Utilities\SimRunner\NeuralTest"
        '
        'lblSourceFiles
        '
        Me.lblSourceFiles.Location = New System.Drawing.Point(8, 144)
        Me.lblSourceFiles.Name = "lblSourceFiles"
        Me.lblSourceFiles.Size = New System.Drawing.Size(480, 16)
        Me.lblSourceFiles.TabIndex = 11
        Me.lblSourceFiles.Text = "Source Files"
        Me.lblSourceFiles.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnSourceFiles
        '
        Me.btnSourceFiles.Location = New System.Drawing.Point(496, 160)
        Me.btnSourceFiles.Name = "btnSourceFiles"
        Me.btnSourceFiles.Size = New System.Drawing.Size(16, 16)
        Me.btnSourceFiles.TabIndex = 10
        Me.btnSourceFiles.Text = "..."
        '
        'txtSourceFiles
        '
        Me.txtSourceFiles.Location = New System.Drawing.Point(8, 160)
        Me.txtSourceFiles.Name = "txtSourceFiles"
        Me.txtSourceFiles.Size = New System.Drawing.Size(480, 20)
        Me.txtSourceFiles.TabIndex = 9
        Me.txtSourceFiles.Text = "C:\Projects\AnimatLabSDK\AnimatLabPublicSource\bin"
        '
        'btnProcess
        '
        Me.btnProcess.Location = New System.Drawing.Point(176, 264)
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(80, 24)
        Me.btnProcess.TabIndex = 12
        Me.btnProcess.Text = "Process"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(264, 264)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 24)
        Me.btnCancel.TabIndex = 13
        Me.btnCancel.Text = "Cancel"
        '
        'barProgress
        '
        Me.barProgress.Location = New System.Drawing.Point(16, 232)
        Me.barProgress.Name = "barProgress"
        Me.barProgress.Size = New System.Drawing.Size(496, 24)
        Me.barProgress.TabIndex = 14
        '
        'lblStatus
        '
        Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStatus.Location = New System.Drawing.Point(16, 216)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(480, 16)
        Me.lblStatus.TabIndex = 15
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(528, 294)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.barProgress)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnProcess)
        Me.Controls.Add(Me.lblSourceFiles)
        Me.Controls.Add(Me.btnSourceFiles)
        Me.Controls.Add(Me.txtSourceFiles)
        Me.Controls.Add(Me.lblCommonFiles)
        Me.Controls.Add(Me.btnCommonFiles)
        Me.Controls.Add(Me.txtCommonFiles)
        Me.Controls.Add(Me.lblResultFiles)
        Me.Controls.Add(Me.btnResultFiles)
        Me.Controls.Add(Me.txtResultFiles)
        Me.Controls.Add(Me.lblSimFiles)
        Me.Controls.Add(Me.btnSimFiles)
        Me.Controls.Add(Me.txtSimFiles)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub btnSimFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSimFiles.Click

        Try
            Dim dlgFolder As New Windows.Forms.FolderBrowserDialog

            dlgFolder.SelectedPath = txtSimFiles.Text
            If dlgFolder.ShowDialog = DialogResult.OK Then
                txtSimFiles.Text = dlgFolder.SelectedPath
            End If

        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnResultFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResultFiles.Click

        Try
            Dim dlgFolder As New Windows.Forms.FolderBrowserDialog

            dlgFolder.SelectedPath = txtResultFiles.Text
            If dlgFolder.ShowDialog = DialogResult.OK Then
                txtResultFiles.Text = dlgFolder.SelectedPath
            End If

        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnCommonFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommonFiles.Click

        Try
            Dim dlgFolder As New Windows.Forms.FolderBrowserDialog

            dlgFolder.SelectedPath = txtCommonFiles.Text
            If dlgFolder.ShowDialog = DialogResult.OK Then
                txtCommonFiles.Text = dlgFolder.SelectedPath
            End If

        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnSourceFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSourceFiles.Click

        Try
            Dim dlgFolder As New Windows.Forms.FolderBrowserDialog

            dlgFolder.SelectedPath = txtSourceFiles.Text
            If dlgFolder.ShowDialog = DialogResult.OK Then
                txtSourceFiles.Text = dlgFolder.SelectedPath
            End If

        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcess.Click

        Try
            'Dim strText As String = txtSourceFiles.Text & "\Animatsimulator -d3d -runtime 2.2 -library vortexanimatlibrary_vc7.dll -project """ & txtCommonFiles.Text & "\BetaPitch_012209_30_6.asim"""
            Dim strFile As String
            Dim strOutFile As String
            Dim strProg As String = txtSourceFiles.Text & "\Animatsimulator"
            Dim strArg As String
            Dim procSim As System.Diagnostics.Process

            Dim aryFiles As String() = Directory.GetFiles(txtSimFiles.Text)

            Dim fltPerc As Single = 1 / aryFiles.Length
            Dim iCount As Integer = 0
            barProgress.Value = 0

            For Each strFFile As String In aryFiles
                strFile = GetFilename(strFFile)
                File.Copy(txtSimFiles.Text & "\" & strFile, txtCommonFiles.Text & "\" & strFile)
                strArg = "-library VortexAnimatPrivateSim_vc10.dll -project """ & txtCommonFiles.Text & "\" & strFile & """"

                lblStatus.Text = "Processing " & iCount & " of " & aryFiles.Length & "   File: " & strFile
                Application.DoEvents()
                procSim = System.Diagnostics.Process.Start(strProg, strArg)
                procSim.WaitForExit()

                Dim aryOutFiles As String() = Directory.GetFiles(txtCommonFiles.Text, "*.txt")

                For Each strFOutFile As String In aryOutFiles
                    strOutFile = GetFilename(strFOutFile)
                    File.Copy(txtCommonFiles.Text & "\" & strOutFile, txtResultFiles.Text & "\" & GetBaseFilename(strFile) & "_" & GetBaseFilename(strOutFile) & ".txt")
                    File.Delete(txtCommonFiles.Text & "\" & strOutFile)
                Next

                File.Delete(txtCommonFiles.Text & "\" & strFile)

                iCount = iCount + 1
                barProgress.Value = (iCount * fltPerc)
                Application.DoEvents()
            Next

            MessageBox.Show("Finished")

        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Protected Function GetFilename(ByVal strFile As String) As String
        Dim aryParts As String() = Split(strFile, "\")
        Return aryParts(aryParts.Length - 1)
    End Function

    Protected Function GetBaseFilename(ByVal strFile As String) As String
        Dim aryParts As String() = Split(strFile, ".")
        Return aryParts(0)
    End Function

End Class
