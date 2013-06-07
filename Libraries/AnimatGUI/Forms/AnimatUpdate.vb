Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGUI
Imports AnimatGUI.Framework

Namespace Forms

    Public Class AnimatUpdate
        Inherits Forms.AnimatDialog

        Protected m_iSetHeight As Integer = -1
        Protected m_iSetWidth As Integer = -1

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

        Private Sub InitializeComponent()
            Me.lblMessage = New System.Windows.Forms.Label()
            Me.btnNo = New System.Windows.Forms.Button()
            Me.btnYes = New System.Windows.Forms.Button()
            Me.txtDescription = New System.Windows.Forms.RichTextBox()
            Me.SuspendLayout()
            '
            'lblMessage
            '
            Me.lblMessage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblMessage.Location = New System.Drawing.Point(13, 13)
            Me.lblMessage.Name = "lblMessage"
            Me.lblMessage.Size = New System.Drawing.Size(718, 49)
            Me.lblMessage.TabIndex = 0
            Me.lblMessage.Text = "A new update for AnimatLab is available. Please see the description below." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Would" & _
        " you like to install this update now?"
            Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnNo
            '
            Me.btnNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnNo.Location = New System.Drawing.Point(655, 310)
            Me.btnNo.Name = "btnNo"
            Me.btnNo.Size = New System.Drawing.Size(75, 23)
            Me.btnNo.TabIndex = 1
            Me.btnNo.Text = "No"
            Me.btnNo.UseVisualStyleBackColor = True
            '
            'btnYes
            '
            Me.btnYes.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes
            Me.btnYes.Location = New System.Drawing.Point(574, 310)
            Me.btnYes.Name = "btnYes"
            Me.btnYes.Size = New System.Drawing.Size(75, 23)
            Me.btnYes.TabIndex = 3
            Me.btnYes.Text = "Yes"
            Me.btnYes.UseVisualStyleBackColor = True
            '
            'txtDescription
            '
            Me.txtDescription.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtDescription.Location = New System.Drawing.Point(15, 64)
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Size = New System.Drawing.Size(715, 237)
            Me.txtDescription.TabIndex = 4
            Me.txtDescription.Text = ""
            '
            'AnimatUpdate
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(743, 345)
            Me.Controls.Add(Me.txtDescription)
            Me.Controls.Add(Me.btnYes)
            Me.Controls.Add(Me.btnNo)
            Me.Controls.Add(Me.lblMessage)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AnimatUpdate"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Animat Update"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblMessage As System.Windows.Forms.Label
        Friend WithEvents btnNo As System.Windows.Forms.Button
        Friend WithEvents btnYes As System.Windows.Forms.Button
        Friend WithEvents txtDescription As System.Windows.Forms.RichTextBox

        Protected m_strMessage As String = ""

        Public Overridable Sub SetMessage(ByVal strMessage As String)
            m_strMessage = strMessage
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                Util.Application.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "Starting OnLoad of AnimatMessageBox")

                InitializeComponent()

                Util.Application.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "AnimatUpdate::InitializeComponent")

                txtDescription.Text = m_strMessage

                If m_iSetWidth > 100 Then
                    Me.Width = m_iSetWidth
                End If

                If m_iSetHeight > 100 Then
                    Me.Height = m_iSetHeight
                End If

                Util.Application.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "AnimatUpdate Finished")

            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnYes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnYes.Click
            Me.DialogResult = Windows.Forms.DialogResult.Yes
            Me.Close()
        End Sub

        Private Sub btnNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNo.Click
            Me.DialogResult = Windows.Forms.DialogResult.No
            Me.Close()
        End Sub

    End Class

End Namespace
