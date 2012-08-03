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
            Me.btn2 = New System.Windows.Forms.Button()
            Me.btn3 = New System.Windows.Forms.Button()
            Me.btn1 = New System.Windows.Forms.Button()
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
            'btn2
            '
            Me.btn2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btn2.Location = New System.Drawing.Point(373, 84)
            Me.btn2.Name = "btn2"
            Me.btn2.Size = New System.Drawing.Size(75, 23)
            Me.btn2.TabIndex = 1
            Me.btn2.Text = "2"
            Me.btn2.UseVisualStyleBackColor = True
            '
            'btn3
            '
            Me.btn3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btn3.Location = New System.Drawing.Point(454, 84)
            Me.btn3.Name = "btn3"
            Me.btn3.Size = New System.Drawing.Size(75, 23)
            Me.btn3.TabIndex = 2
            Me.btn3.Text = "3"
            Me.btn3.UseVisualStyleBackColor = True
            '
            'btn1
            '
            Me.btn1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btn1.Location = New System.Drawing.Point(292, 84)
            Me.btn1.Name = "btn1"
            Me.btn1.Size = New System.Drawing.Size(75, 23)
            Me.btn1.TabIndex = 3
            Me.btn1.Text = "1"
            Me.btn1.UseVisualStyleBackColor = True
            '
            'AnimatMessageBox
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(541, 119)
            Me.Controls.Add(Me.btn1)
            Me.Controls.Add(Me.btn3)
            Me.Controls.Add(Me.btn2)
            Me.Controls.Add(Me.lblMessage)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AnimatMessageBox"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "AnimatMessageBox"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblMessage As System.Windows.Forms.Label
        Friend WithEvents btn2 As System.Windows.Forms.Button
        Friend WithEvents btn3 As System.Windows.Forms.Button
        Friend WithEvents btn1 As System.Windows.Forms.Button

        Protected m_eDialogButtons As System.Windows.Forms.MessageBoxButtons
        Protected m_strMessage As String = ""
        Protected m_strCaption As String = "Animat Message Box"

        Public Overridable Sub SetMessage(ByVal strMessage As String, ByVal eButtons As System.Windows.Forms.MessageBoxButtons, Optional ByVal strCaption As String = "")
            m_strMessage = strMessage
            m_eDialogButtons = eButtons
            If strCaption.Length > 0 Then
                m_strCaption = strCaption
            End If
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                Util.Application.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "Starting OnLoad of AnimatMessageBox")

                InitializeComponent()

                Util.Application.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "AnimatMessageBox::InitializeComponent")

                lblMessage.Text = m_strMessage
                Me.Text = m_strCaption

                Select Case m_eDialogButtons
                    Case MessageBoxButtons.AbortRetryIgnore
                        btn1.Text = "Abort"
                        btn1.DialogResult = Windows.Forms.DialogResult.Abort
                        btn2.Text = "Retry"
                        btn2.DialogResult = Windows.Forms.DialogResult.Retry
                        btn3.Text = "Ignore"
                        btn3.DialogResult = Windows.Forms.DialogResult.Cancel
                        m_btnOk = Me.btn2
                        m_btnCancel = Me.btn1
                        m_btnIgnore = Me.btn3
                    Case MessageBoxButtons.OK
                        btn3.Text = "Ok"
                        btn1.Visible = False
                        btn2.Visible = False
                        btn3.DialogResult = Windows.Forms.DialogResult.OK
                        m_btnOk = Me.btn3
                        m_btnCancel = Me.btn1
                        m_btnIgnore = Me.btn2
                    Case MessageBoxButtons.OKCancel
                        btn1.Visible = False
                        btn2.Text = "Ok"
                        btn2.DialogResult = Windows.Forms.DialogResult.OK
                        btn3.Text = "Cancel"
                        btn3.DialogResult = Windows.Forms.DialogResult.Cancel
                        m_btnOk = Me.btn2
                        m_btnCancel = Me.btn3
                        m_btnIgnore = Me.btn1
                    Case MessageBoxButtons.RetryCancel
                        btn1.Visible = False
                        btn2.Text = "Retry"
                        btn2.DialogResult = Windows.Forms.DialogResult.Retry
                        btn3.Text = "Cancel"
                        btn3.DialogResult = Windows.Forms.DialogResult.Cancel
                        m_btnOk = Me.btn2
                        m_btnCancel = Me.btn3
                        m_btnIgnore = Me.btn1
                    Case MessageBoxButtons.YesNo
                        btn1.Visible = False
                        btn2.Text = "Yes"
                        btn2.DialogResult = Windows.Forms.DialogResult.Yes
                        btn3.Text = "No"
                        btn3.DialogResult = Windows.Forms.DialogResult.No
                        m_btnOk = Me.btn2
                        m_btnCancel = Me.btn3
                        m_btnIgnore = Me.btn1
                    Case MessageBoxButtons.YesNoCancel
                        btn1.Text = "Yes"
                        btn1.DialogResult = Windows.Forms.DialogResult.Yes
                        btn2.Text = "No"
                        btn2.DialogResult = Windows.Forms.DialogResult.No
                        btn3.Text = "Cancel"
                        btn3.DialogResult = Windows.Forms.DialogResult.Cancel
                        m_btnOk = Me.btn1
                        m_btnIgnore = Me.btn2
                        m_btnCancel = Me.btn3
                    Case Else
                        Throw New System.Exception("Invalid message buttons. '" & m_eDialogButtons.ToString & "'")
                End Select

                Util.Application.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "AnimatMessageBox Finished")

            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub
    End Class

End Namespace
