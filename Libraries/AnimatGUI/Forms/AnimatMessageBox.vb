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

        Protected m_iSetHeight As Integer = -1
        Protected m_iSetWidth As Integer = -1
        Protected m_eSetTextAlign As System.Windows.Forms.HorizontalAlignment
        Protected m_bReadOnly As Boolean = True

        Public Property SetHeight() As Integer
            Get
                Return m_iSetHeight
            End Get
            Set(ByVal value As Integer)
                m_iSetHeight = value
            End Set
        End Property

        Public Property SetWidth() As Integer
            Get
                Return m_iSetWidth
            End Get
            Set(ByVal value As Integer)
                m_iSetWidth = value
            End Set
        End Property

        Public Property SetTextAlign() As System.Windows.Forms.HorizontalAlignment
            Get
                Return m_eSetTextAlign
            End Get
            Set(ByVal value As System.Windows.Forms.HorizontalAlignment)
                m_eSetTextAlign = value
            End Set
        End Property

        Public Property TextReadOnly() As Boolean
            Get
                Return m_bReadOnly
            End Get
            Set(ByVal value As Boolean)
                m_bReadOnly = value
            End Set
        End Property

        Private Sub InitializeComponent()
            Me.btn2 = New System.Windows.Forms.Button()
            Me.btn3 = New System.Windows.Forms.Button()
            Me.btn1 = New System.Windows.Forms.Button()
            Me.txtMessage = New System.Windows.Forms.TextBox()
            Me.SuspendLayout()
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
            'txtMessage
            '
            Me.txtMessage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtMessage.Location = New System.Drawing.Point(13, 13)
            Me.txtMessage.Multiline = True
            Me.txtMessage.Name = "txtMessage"
            Me.txtMessage.Size = New System.Drawing.Size(516, 65)
            Me.txtMessage.TabIndex = 4
            '
            'AnimatMessageBox
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(541, 119)
            Me.Controls.Add(Me.txtMessage)
            Me.Controls.Add(Me.btn1)
            Me.Controls.Add(Me.btn3)
            Me.Controls.Add(Me.btn2)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AnimatMessageBox"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "AnimatMessageBox"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
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

                txtMessage.Text = m_strMessage
                Me.Text = m_strCaption
                Me.txtMessage.TextAlign = m_eSetTextAlign
                Me.txtMessage.ReadOnly = m_bReadOnly

                If m_iSetWidth > 100 Then
                    Me.Width = m_iSetWidth
                End If

                If m_iSetHeight > 100 Then
                    Me.Height = m_iSetHeight
                End If

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
        Friend WithEvents txtMessage As System.Windows.Forms.TextBox
    End Class

End Namespace
