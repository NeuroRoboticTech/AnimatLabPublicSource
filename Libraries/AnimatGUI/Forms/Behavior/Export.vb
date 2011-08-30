Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Forms.Behavior

    Public Class Export
        Inherits AnimatGUI.Forms.AnimatDialog

        Protected m_strFilter As String = "*.bmp|*.bmp"
        Protected m_strExtension As String = ".bmp"
        Protected m_strFileLocation As String
        Protected m_bAllDiagrams As Boolean
        Protected m_eFormat As System.Drawing.Imaging.ImageFormat

        Public Property FileLocation() As String
            Get
                Return m_strFileLocation
            End Get
            Set(ByVal Value As String)
                m_strFileLocation = Value
            End Set
        End Property

        Public ReadOnly Property AllDiagrams() As Boolean
            Get
                Return m_bAllDiagrams
            End Get
        End Property

        Public ReadOnly Property Format() As System.Drawing.Imaging.ImageFormat
            Get
                Return m_eFormat
            End Get
        End Property

        Public ReadOnly Property Extension() As String
            Get
                Return m_strExtension
            End Get
        End Property

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
        Friend WithEvents gbDiagramsToExport As System.Windows.Forms.GroupBox
        Friend WithEvents rbAllDiagrams As System.Windows.Forms.RadioButton
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents lblFilePath As System.Windows.Forms.Label
        Friend WithEvents txtFileLocation As System.Windows.Forms.TextBox
        Friend WithEvents btnFileLocation As System.Windows.Forms.Button
        Friend WithEvents gbFileType As System.Windows.Forms.GroupBox
        Friend WithEvents rbBmp As System.Windows.Forms.RadioButton
        Friend WithEvents rbEmf As System.Windows.Forms.RadioButton
        Friend WithEvents rbGif As System.Windows.Forms.RadioButton
        Friend WithEvents rbWmf As System.Windows.Forms.RadioButton
        Friend WithEvents rbJpeg As System.Windows.Forms.RadioButton
        Friend WithEvents rbTiff As System.Windows.Forms.RadioButton
        Friend WithEvents rbPng As System.Windows.Forms.RadioButton
        Friend WithEvents rbSelectedDiagram As System.Windows.Forms.RadioButton
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.gbDiagramsToExport = New System.Windows.Forms.GroupBox
            Me.rbSelectedDiagram = New System.Windows.Forms.RadioButton
            Me.rbAllDiagrams = New System.Windows.Forms.RadioButton
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnOk = New System.Windows.Forms.Button
            Me.lblFilePath = New System.Windows.Forms.Label
            Me.txtFileLocation = New System.Windows.Forms.TextBox
            Me.btnFileLocation = New System.Windows.Forms.Button
            Me.gbFileType = New System.Windows.Forms.GroupBox
            Me.rbPng = New System.Windows.Forms.RadioButton
            Me.rbTiff = New System.Windows.Forms.RadioButton
            Me.rbJpeg = New System.Windows.Forms.RadioButton
            Me.rbWmf = New System.Windows.Forms.RadioButton
            Me.rbGif = New System.Windows.Forms.RadioButton
            Me.rbEmf = New System.Windows.Forms.RadioButton
            Me.rbBmp = New System.Windows.Forms.RadioButton
            Me.gbDiagramsToExport.SuspendLayout()
            Me.gbFileType.SuspendLayout()
            Me.SuspendLayout()
            '
            'gbDiagramsToExport
            '
            Me.gbDiagramsToExport.Controls.Add(Me.rbSelectedDiagram)
            Me.gbDiagramsToExport.Controls.Add(Me.rbAllDiagrams)
            Me.gbDiagramsToExport.Location = New System.Drawing.Point(8, 8)
            Me.gbDiagramsToExport.Name = "gbDiagramsToExport"
            Me.gbDiagramsToExport.Size = New System.Drawing.Size(144, 72)
            Me.gbDiagramsToExport.TabIndex = 2
            Me.gbDiagramsToExport.TabStop = False
            Me.gbDiagramsToExport.Text = "Diagrams To Export"
            '
            'rbSelectedDiagram
            '
            Me.rbSelectedDiagram.Checked = True
            Me.rbSelectedDiagram.Location = New System.Drawing.Point(16, 48)
            Me.rbSelectedDiagram.Name = "rbSelectedDiagram"
            Me.rbSelectedDiagram.Size = New System.Drawing.Size(112, 16)
            Me.rbSelectedDiagram.TabIndex = 3
            Me.rbSelectedDiagram.TabStop = True
            Me.rbSelectedDiagram.Text = "Selected Diagram"
            '
            'rbAllDiagrams
            '
            Me.rbAllDiagrams.Location = New System.Drawing.Point(16, 24)
            Me.rbAllDiagrams.Name = "rbAllDiagrams"
            Me.rbAllDiagrams.Size = New System.Drawing.Size(112, 16)
            Me.rbAllDiagrams.TabIndex = 2
            Me.rbAllDiagrams.Text = "All Diagrams"
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(200, 136)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 4
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Location = New System.Drawing.Point(128, 136)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 3
            Me.btnOk.Text = "Ok"
            '
            'lblFilePath
            '
            Me.lblFilePath.Location = New System.Drawing.Point(16, 88)
            Me.lblFilePath.Name = "lblFilePath"
            Me.lblFilePath.Size = New System.Drawing.Size(360, 16)
            Me.lblFilePath.TabIndex = 5
            Me.lblFilePath.Text = "File Location"
            Me.lblFilePath.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'txtFileLocation
            '
            Me.txtFileLocation.Location = New System.Drawing.Point(8, 104)
            Me.txtFileLocation.Name = "txtFileLocation"
            Me.txtFileLocation.Size = New System.Drawing.Size(344, 20)
            Me.txtFileLocation.TabIndex = 6
            Me.txtFileLocation.Text = ""
            '
            'btnFileLocation
            '
            Me.btnFileLocation.Location = New System.Drawing.Point(360, 104)
            Me.btnFileLocation.Name = "btnFileLocation"
            Me.btnFileLocation.Size = New System.Drawing.Size(24, 20)
            Me.btnFileLocation.TabIndex = 7
            Me.btnFileLocation.Text = "..."
            '
            'gbFileType
            '
            Me.gbFileType.Controls.Add(Me.rbPng)
            Me.gbFileType.Controls.Add(Me.rbTiff)
            Me.gbFileType.Controls.Add(Me.rbJpeg)
            Me.gbFileType.Controls.Add(Me.rbWmf)
            Me.gbFileType.Controls.Add(Me.rbGif)
            Me.gbFileType.Controls.Add(Me.rbEmf)
            Me.gbFileType.Controls.Add(Me.rbBmp)
            Me.gbFileType.Location = New System.Drawing.Point(160, 8)
            Me.gbFileType.Name = "gbFileType"
            Me.gbFileType.Size = New System.Drawing.Size(224, 72)
            Me.gbFileType.TabIndex = 8
            Me.gbFileType.TabStop = False
            Me.gbFileType.Text = "File Type"
            '
            'rbPng
            '
            Me.rbPng.Location = New System.Drawing.Point(120, 24)
            Me.rbPng.Name = "rbPng"
            Me.rbPng.Size = New System.Drawing.Size(48, 16)
            Me.rbPng.TabIndex = 6
            Me.rbPng.Text = "Png"
            '
            'rbTiff
            '
            Me.rbTiff.Location = New System.Drawing.Point(120, 40)
            Me.rbTiff.Name = "rbTiff"
            Me.rbTiff.Size = New System.Drawing.Size(44, 16)
            Me.rbTiff.TabIndex = 5
            Me.rbTiff.Text = "Tiff"
            '
            'rbJpeg
            '
            Me.rbJpeg.Location = New System.Drawing.Point(72, 40)
            Me.rbJpeg.Name = "rbJpeg"
            Me.rbJpeg.Size = New System.Drawing.Size(48, 16)
            Me.rbJpeg.TabIndex = 4
            Me.rbJpeg.Text = "Jpeg"
            '
            'rbWmf
            '
            Me.rbWmf.Location = New System.Drawing.Point(168, 24)
            Me.rbWmf.Name = "rbWmf"
            Me.rbWmf.Size = New System.Drawing.Size(48, 16)
            Me.rbWmf.TabIndex = 3
            Me.rbWmf.Text = "Wmf"
            '
            'rbGif
            '
            Me.rbGif.Location = New System.Drawing.Point(72, 24)
            Me.rbGif.Name = "rbGif"
            Me.rbGif.Size = New System.Drawing.Size(40, 16)
            Me.rbGif.TabIndex = 2
            Me.rbGif.Text = "Gif"
            '
            'rbEmf
            '
            Me.rbEmf.Location = New System.Drawing.Point(8, 40)
            Me.rbEmf.Name = "rbEmf"
            Me.rbEmf.Size = New System.Drawing.Size(48, 16)
            Me.rbEmf.TabIndex = 1
            Me.rbEmf.Text = "Emf"
            '
            'rbBmp
            '
            Me.rbBmp.Checked = True
            Me.rbBmp.Location = New System.Drawing.Point(8, 24)
            Me.rbBmp.Name = "rbBmp"
            Me.rbBmp.Size = New System.Drawing.Size(64, 16)
            Me.rbBmp.TabIndex = 0
            Me.rbBmp.TabStop = True
            Me.rbBmp.Text = "Bitmap"
            '
            'Export
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(392, 174)
            Me.Controls.Add(Me.gbFileType)
            Me.Controls.Add(Me.btnFileLocation)
            Me.Controls.Add(Me.txtFileLocation)
            Me.Controls.Add(Me.lblFilePath)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.gbDiagramsToExport)
            Me.Name = "Export"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Export"
            Me.gbDiagramsToExport.ResumeLayout(False)
            Me.gbFileType.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private Sub btnFileLocation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFileLocation.Click

            Try

                If rbSelectedDiagram.Checked Then
                    Dim saveFileDialog As New SaveFileDialog
                    saveFileDialog.Filter = m_strFilter
                    saveFileDialog.Title = "Select File To Save"

                    If saveFileDialog.ShowDialog() = DialogResult.OK Then
                        txtFileLocation.Text = saveFileDialog.FileName
                    End If
                Else
                    Dim folderDialog As New FolderBrowserDialog

                    If folderDialog.ShowDialog() = DialogResult.OK Then
                        txtFileLocation.Text = folderDialog.SelectedPath
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

            Try
                If txtFileLocation.Text.Trim.Length = 0 Then
                    Throw New System.Exception("The file location can not be blank.")
                End If

                m_strFileLocation = txtFileLocation.Text
                m_bAllDiagrams = rbAllDiagrams.Checked
                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub rbAllDiagrams_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbAllDiagrams.CheckedChanged
            txtFileLocation.Text = ""
        End Sub

        Private Sub rbSelectedDiagram_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSelectedDiagram.CheckedChanged
            txtFileLocation.Text = ""
        End Sub

        Private Sub rbBmp_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbBmp.CheckedChanged
            m_strFilter = "*.bmp|*.bmp"
            m_strExtension = ".bmp"
            txtFileLocation.Text = ""
            m_eFormat = System.Drawing.Imaging.ImageFormat.Bmp
        End Sub

        Private Sub rbEmf_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbEmf.CheckedChanged
            m_strFilter = "*.emf|*.emf"
            m_strExtension = ".emf"
            txtFileLocation.Text = ""
            m_eFormat = System.Drawing.Imaging.ImageFormat.Emf
        End Sub

        Private Sub rbGif_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbGif.CheckedChanged
            m_strFilter = "*.gif|*.gif"
            m_strExtension = ".gif"
            txtFileLocation.Text = ""
            m_eFormat = System.Drawing.Imaging.ImageFormat.Gif
        End Sub

        Private Sub rbJpeg_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbJpeg.CheckedChanged
            m_strFilter = "*.jpeg|*.jpeg"
            m_strExtension = ".jpeg"
            txtFileLocation.Text = ""
            m_eFormat = System.Drawing.Imaging.ImageFormat.Jpeg
        End Sub

        Private Sub rbPng_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbPng.CheckedChanged
            m_strFilter = "*.png|*.png"
            m_strExtension = ".png"
            txtFileLocation.Text = ""
            m_eFormat = System.Drawing.Imaging.ImageFormat.Png
        End Sub

        Private Sub rbTiff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTiff.CheckedChanged
            m_strFilter = "*.tiff|*.tiff"
            m_strExtension = ".tiff"
            txtFileLocation.Text = ""
            m_eFormat = System.Drawing.Imaging.ImageFormat.Tiff
        End Sub

        Private Sub rbWmf_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbWmf.CheckedChanged
            m_strFilter = "*.wmf|*.wmf"
            m_strExtension = ".wmf"
            txtFileLocation.Text = ""
            m_eFormat = System.Drawing.Imaging.ImageFormat.Wmf
        End Sub

    End Class

End Namespace
