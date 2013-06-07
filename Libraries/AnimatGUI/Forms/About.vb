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
        Friend WithEvents lblAnimatLab As System.Windows.Forms.LinkLabel
        Friend WithEvents linkPurchase As System.Windows.Forms.LinkLabel
        Friend WithEvents txtInfo As System.Windows.Forms.TextBox
        Friend WithEvents lblUrl As System.Windows.Forms.LinkLabel
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lblAnimatLab = New System.Windows.Forms.LinkLabel()
            Me.lblUrl = New System.Windows.Forms.LinkLabel()
            Me.linkPurchase = New System.Windows.Forms.LinkLabel()
            Me.txtInfo = New System.Windows.Forms.TextBox()
            Me.SuspendLayout()
            '
            'lblAnimatLab
            '
            Me.lblAnimatLab.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblAnimatLab.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
            Me.lblAnimatLab.LinkColor = System.Drawing.Color.Black
            Me.lblAnimatLab.Location = New System.Drawing.Point(16, 8)
            Me.lblAnimatLab.Name = "lblAnimatLab"
            Me.lblAnimatLab.Size = New System.Drawing.Size(514, 40)
            Me.lblAnimatLab.TabIndex = 3
            Me.lblAnimatLab.TabStop = True
            Me.lblAnimatLab.Text = "AnimatLab"
            Me.lblAnimatLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblUrl
            '
            Me.lblUrl.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblUrl.Location = New System.Drawing.Point(12, 48)
            Me.lblUrl.Name = "lblUrl"
            Me.lblUrl.Size = New System.Drawing.Size(518, 24)
            Me.lblUrl.TabIndex = 6
            Me.lblUrl.TabStop = True
            Me.lblUrl.Text = "http:\\www.AnimatLab.com"
            Me.lblUrl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'linkPurchase
            '
            Me.linkPurchase.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.linkPurchase.LinkColor = System.Drawing.Color.Red
            Me.linkPurchase.Location = New System.Drawing.Point(16, 72)
            Me.linkPurchase.Name = "linkPurchase"
            Me.linkPurchase.Size = New System.Drawing.Size(514, 24)
            Me.linkPurchase.TabIndex = 7
            Me.linkPurchase.TabStop = True
            Me.linkPurchase.Text = "Upgrade to AnimatLab Pro"
            Me.linkPurchase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtInfo
            '
            Me.txtInfo.Location = New System.Drawing.Point(12, 99)
            Me.txtInfo.Multiline = True
            Me.txtInfo.Name = "txtInfo"
            Me.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.txtInfo.Size = New System.Drawing.Size(518, 249)
            Me.txtInfo.TabIndex = 8
            '
            'About
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(542, 360)
            Me.Controls.Add(Me.txtInfo)
            Me.Controls.Add(Me.linkPurchase)
            Me.Controls.Add(Me.lblUrl)
            Me.Controls.Add(Me.lblAnimatLab)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "About"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "About AnimatLab"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private Sub About_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                lblAnimatLab.Links.Add(0, lblAnimatLab.Text.Length, "http:\\www.AnimatLab.com")

                Dim strProductName As String = "AnimatLab"
                Dim strProductExtra As String = ""
                Dim strProductVersion As String = ""
                Dim bFullVersion As Boolean = False

                Util.Application.GetProductInfo(strProductName, strProductVersion, strProductExtra, bFullVersion)

                lblAnimatLab.Text = strProductName
                If Not bFullVersion Then
                    linkPurchase.Visible = True
                    linkPurchase.Links.Add(0, linkPurchase.Text.Length, "http:\\www.AnimatLab.com\Store")
                Else
                    linkPurchase.Visible = False
                End If

                If strProductExtra.Length > 0 Then strProductExtra = vbCrLf & strProductExtra

                txtInfo.Text = lblAnimatLab.Text & vbCrLf & strProductVersion & strProductExtra & vbCrLf & vbCrLf & _
                          "AnimatLab Version 2 Project Team" & vbCrLf & vbCrLf & _
                          "   Devloper and Manger: David Cofer" & vbCrLf & _
                          "   NeuroRobotic Technologies, LLC." & vbCrLf & vbCrLf & _
                          "AnimatLab Version 1 Project Team" & vbCrLf & vbCrLf & _
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

        Private Sub linkPurchase_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkPurchase.Click
            Try
                System.Diagnostics.Process.Start("http:\\www.AnimatLab.com\Store")
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
