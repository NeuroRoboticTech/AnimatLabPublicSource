Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGUI
Imports AnimatGUI.Framework

Namespace Forms

    Public Class AnimatMessageBox
        Inherits Forms.AnimatDialog

        Private Sub InitializeComponent()
            Me.lblMessage = New System.Windows.Forms.Label()
            Me.btnYes = New System.Windows.Forms.Button()
            Me.btnNo = New System.Windows.Forms.Button()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'lblMessage
            '
            Me.lblMessage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblMessage.Location = New System.Drawing.Point(13, 13)
            Me.lblMessage.Name = "lblMessage"
            Me.lblMessage.Size = New System.Drawing.Size(516, 67)
            Me.lblMessage.TabIndex = 0
            Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnYes
            '
            Me.btnYes.Location = New System.Drawing.Point(152, 84)
            Me.btnYes.Name = "btnYes"
            Me.btnYes.Size = New System.Drawing.Size(75, 23)
            Me.btnYes.TabIndex = 1
            Me.btnYes.Text = "Yes"
            Me.btnYes.UseVisualStyleBackColor = True
            '
            'btnNo
            '
            Me.btnNo.Location = New System.Drawing.Point(233, 84)
            Me.btnNo.Name = "btnNo"
            Me.btnNo.Size = New System.Drawing.Size(75, 23)
            Me.btnNo.TabIndex = 2
            Me.btnNo.Text = "No"
            Me.btnNo.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Location = New System.Drawing.Point(314, 83)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 3
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'AnimatMessageBox
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(541, 119)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnNo)
            Me.Controls.Add(Me.btnYes)
            Me.Controls.Add(Me.lblMessage)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AnimatMessageBox"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "AnimatMessageBox"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblMessage As System.Windows.Forms.Label
        Friend WithEvents btnYes As System.Windows.Forms.Button
        Friend WithEvents btnNo As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button

        Protected m_eDialogButtons As System.Windows.Forms.MessageBoxButtons
        Protected m_strMessage As String = ""

        Public Overridable Sub SetMessage(ByVal strMessage As String, ByVal eButtons As System.Windows.Forms.MessageBoxButtons)
            m_strMessage = strMessage
            m_eDialogButtons = eButtons
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                InitializeComponent()

                m_btnOk = Me.btnYes
                m_btnCancel = Me.btnCancel
                m_btnIgnore = Me.btnNo

                lblMessage.Text = m_strMessage

                Select Case m_eDialogButtons
                    Case MessageBoxButtons.AbortRetryIgnore
                        btnYes.Text = "Abort"
                        btnYes.DialogResult = Windows.Forms.DialogResult.Abort
                        btnNo.Text = "Retry"
                        btnNo.DialogResult = Windows.Forms.DialogResult.Retry
                        btnCancel.Text = "Ignore"
                        btnCancel.DialogResult = Windows.Forms.DialogResult.Cancel
                    Case MessageBoxButtons.OK
                        btnYes.Text = "Ok"
                        btnYes.DialogResult = Windows.Forms.DialogResult.OK
                        btnNo.Visible = False
                        btnCancel.Visible = False
                    Case MessageBoxButtons.OKCancel
                        btnYes.Text = "Ok"
                        btnYes.DialogResult = Windows.Forms.DialogResult.OK
                        btnNo.Visible = False
                        btnCancel.Text = "Cancel"
                        btnCancel.DialogResult = Windows.Forms.DialogResult.Cancel
                    Case MessageBoxButtons.RetryCancel
                        btnYes.Text = "Retry"
                        btnYes.DialogResult = Windows.Forms.DialogResult.Retry
                        btnNo.Visible = False
                        btnCancel.Text = "Cancel"
                        btnCancel.DialogResult = Windows.Forms.DialogResult.Cancel
                    Case MessageBoxButtons.YesNo
                        btnYes.Text = "Yes"
                        btnYes.DialogResult = Windows.Forms.DialogResult.Yes
                        btnNo.Text = "No"
                        btnNo.DialogResult = Windows.Forms.DialogResult.No
                        btnCancel.Visible = False
                    Case MessageBoxButtons.YesNoCancel
                        btnYes.Text = "Yes"
                        btnYes.DialogResult = Windows.Forms.DialogResult.Yes
                        btnNo.Text = "No"
                        btnNo.DialogResult = Windows.Forms.DialogResult.No
                        btnCancel.Text = "Cancel"
                        btnCancel.DialogResult = Windows.Forms.DialogResult.Cancel
                    Case Else
                        Throw New System.Exception("Invalid message buttons. '" & m_eDialogButtons.ToString & "'")
                End Select
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub
    End Class

End Namespace
