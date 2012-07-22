Imports AnimatGUI.Framework

Namespace Forms

    Public Class About
        Inherits AnimatGUI.Forms.AnimatDialog

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
        Friend WithEvents pictGSU As System.Windows.Forms.PictureBox
        Friend WithEvents pictCmLabs As System.Windows.Forms.PictureBox
        Friend WithEvents picNIH As System.Windows.Forms.PictureBox
        Friend WithEvents lblAnimatLab As System.Windows.Forms.LinkLabel
        Friend WithEvents lblText As System.Windows.Forms.Label
        Friend WithEvents btnExit As System.Windows.Forms.Button
        Friend WithEvents lblUrl As System.Windows.Forms.LinkLabel
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(About))
            Me.pictGSU = New System.Windows.Forms.PictureBox()
            Me.pictCmLabs = New System.Windows.Forms.PictureBox()
            Me.picNIH = New System.Windows.Forms.PictureBox()
            Me.lblAnimatLab = New System.Windows.Forms.LinkLabel()
            Me.lblText = New System.Windows.Forms.Label()
            Me.btnExit = New System.Windows.Forms.Button()
            Me.lblUrl = New System.Windows.Forms.LinkLabel()
            CType(Me.pictGSU, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.pictCmLabs, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picNIH, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pictGSU
            '
            Me.pictGSU.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pictGSU.Image = CType(resources.GetObject("pictGSU.Image"), System.Drawing.Image)
            Me.pictGSU.Location = New System.Drawing.Point(8, 304)
            Me.pictGSU.Name = "pictGSU"
            Me.pictGSU.Size = New System.Drawing.Size(153, 52)
            Me.pictGSU.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
            Me.pictGSU.TabIndex = 0
            Me.pictGSU.TabStop = False
            '
            'pictCmLabs
            '
            Me.pictCmLabs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pictCmLabs.Image = CType(resources.GetObject("pictCmLabs.Image"), System.Drawing.Image)
            Me.pictCmLabs.Location = New System.Drawing.Point(304, 304)
            Me.pictCmLabs.Name = "pictCmLabs"
            Me.pictCmLabs.Size = New System.Drawing.Size(138, 52)
            Me.pictCmLabs.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
            Me.pictCmLabs.TabIndex = 1
            Me.pictCmLabs.TabStop = False
            '
            'picNIH
            '
            Me.picNIH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.picNIH.Image = CType(resources.GetObject("picNIH.Image"), System.Drawing.Image)
            Me.picNIH.Location = New System.Drawing.Point(360, 216)
            Me.picNIH.Name = "picNIH"
            Me.picNIH.Size = New System.Drawing.Size(56, 63)
            Me.picNIH.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
            Me.picNIH.TabIndex = 2
            Me.picNIH.TabStop = False
            '
            'lblAnimatLab
            '
            Me.lblAnimatLab.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblAnimatLab.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
            Me.lblAnimatLab.LinkColor = System.Drawing.Color.Red
            Me.lblAnimatLab.Location = New System.Drawing.Point(88, 8)
            Me.lblAnimatLab.Name = "lblAnimatLab"
            Me.lblAnimatLab.Size = New System.Drawing.Size(256, 40)
            Me.lblAnimatLab.TabIndex = 3
            Me.lblAnimatLab.TabStop = True
            Me.lblAnimatLab.Text = "AnimatLab"
            Me.lblAnimatLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblText
            '
            Me.lblText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblText.Location = New System.Drawing.Point(16, 72)
            Me.lblText.Name = "lblText"
            Me.lblText.Size = New System.Drawing.Size(408, 208)
            Me.lblText.TabIndex = 4
            '
            'btnExit
            '
            Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnExit.Location = New System.Drawing.Point(168, 304)
            Me.btnExit.Name = "btnExit"
            Me.btnExit.Size = New System.Drawing.Size(128, 48)
            Me.btnExit.TabIndex = 5
            Me.btnExit.Text = "Exit"
            '
            'lblUrl
            '
            Me.lblUrl.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblUrl.Location = New System.Drawing.Point(72, 48)
            Me.lblUrl.Name = "lblUrl"
            Me.lblUrl.Size = New System.Drawing.Size(288, 24)
            Me.lblUrl.TabIndex = 6
            Me.lblUrl.TabStop = True
            Me.lblUrl.Text = "http:\\www.AnimatLab.com"
            Me.lblUrl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'About
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(448, 360)
            Me.Controls.Add(Me.lblUrl)
            Me.Controls.Add(Me.btnExit)
            Me.Controls.Add(Me.picNIH)
            Me.Controls.Add(Me.lblText)
            Me.Controls.Add(Me.lblAnimatLab)
            Me.Controls.Add(Me.pictCmLabs)
            Me.Controls.Add(Me.pictGSU)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "About"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "About AnimatLab"
            CType(Me.pictGSU, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.pictCmLabs, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picNIH, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private Sub About_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                lblAnimatLab.Links.Add(0, lblAnimatLab.Text.Length, "http:\\www.AnimatLab.com")

                lblText.Text = "Version " & Util.VersionNumber & vbCrLf & vbCrLf & _
                          "AnimatLab  Project Team" & vbCrLf & vbCrLf & _
                          "Project Managers: & " & vbCrLf & _
                          "    Dr. Donald Edwards, Professor of Biology at GSU. " & vbCrLf & _
                          "    Dr. Ying Zhu, Professor of CSC at GSU. " & vbCrLf & _
                          "    Dr. Gennady Cymbalyuk, Professor of Physics at GSU. " & vbCrLf & vbCrLf & _
                          "    Development Team: " & vbCrLf & _
                          "    David Cofer, Graduate student in Biology at GSU. " & vbCrLf & _
                          "    Dr. William Heitler, Professor of Biology at University of St. Andrews. " & vbCrLf & vbCrLf & _
                          "The physics engine we use in this application is vortex by cm-labs. " & vbCrLf & vbCrLf & _
                          "This project is funded in part by a NIH exploratory grant (GM065762)."

            Catch ex As System.Exception

            End Try
        End Sub

        Private Sub lblAnimatLab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblAnimatLab.Click
            Try
                System.Diagnostics.Process.Start("http:\\www.AnimatLab.com")
            Catch ex As System.Exception

            End Try
        End Sub

        Private Sub pictCmLabs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pictCmLabs.Click
            Try
                System.Diagnostics.Process.Start("http://www.cm-labs.com")
            Catch ex As System.Exception
            End Try
        End Sub

        Private Sub pictGSU_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pictGSU.Click
            Try
                System.Diagnostics.Process.Start("http://biology.gsu.edu/brains&behavior")
            Catch ex As System.Exception
            End Try
        End Sub

        Private Sub lblUrl_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblUrl.LinkClicked
            Try
                System.Diagnostics.Process.Start("http:\\www.AnimatLab.com")
            Catch ex As System.Exception

            End Try
        End Sub

    End Class

End Namespace
