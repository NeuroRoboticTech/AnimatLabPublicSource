Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGUI
Imports AnimatGUI.Framework

Namespace Forms

	Public Class ErrorDisplay
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
		Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents lblError As System.Windows.Forms.Label
        Friend WithEvents lblDetails As System.Windows.Forms.Label
        Friend WithEvents btnExpand As System.Windows.Forms.Button
        Friend WithEvents ilImages As System.Windows.Forms.ImageList
        Friend WithEvents txtErrorDetails As System.Windows.Forms.TextBox
        Friend WithEvents txtErrorMsg As System.Windows.Forms.TextBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ErrorDisplay))
            Me.btnOk = New System.Windows.Forms.Button
            Me.txtErrorMsg = New System.Windows.Forms.TextBox
            Me.lblError = New System.Windows.Forms.Label
            Me.lblDetails = New System.Windows.Forms.Label
            Me.btnExpand = New System.Windows.Forms.Button
            Me.ilImages = New System.Windows.Forms.ImageList(Me.components)
            Me.txtErrorDetails = New System.Windows.Forms.TextBox
            Me.SuspendLayout()
            '
            'btnOk
            '
            Me.btnOk.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.btnOk.Location = New System.Drawing.Point(100, 272)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(88, 24)
            Me.btnOk.TabIndex = 0
            Me.btnOk.Text = "Ok"
            '
            'txtErrorMsg
            '
            Me.txtErrorMsg.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.txtErrorMsg.BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(128, Byte), CType(128, Byte))
            Me.txtErrorMsg.Location = New System.Drawing.Point(20, 24)
            Me.txtErrorMsg.Multiline = True
            Me.txtErrorMsg.Name = "txtErrorMsg"
            Me.txtErrorMsg.ReadOnly = True
            Me.txtErrorMsg.Size = New System.Drawing.Size(272, 104)
            Me.txtErrorMsg.TabIndex = 1
            Me.txtErrorMsg.Text = ""
            '
            'lblError
            '
            Me.lblError.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.lblError.Location = New System.Drawing.Point(20, 8)
            Me.lblError.Name = "lblError"
            Me.lblError.Size = New System.Drawing.Size(272, 16)
            Me.lblError.TabIndex = 2
            Me.lblError.Text = "Error Message"
            Me.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblDetails
            '
            Me.lblDetails.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.lblDetails.Location = New System.Drawing.Point(52, 136)
            Me.lblDetails.Name = "lblDetails"
            Me.lblDetails.Size = New System.Drawing.Size(232, 16)
            Me.lblDetails.TabIndex = 3
            Me.lblDetails.Text = "Error Details"
            Me.lblDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnExpand
            '
            Me.btnExpand.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.btnExpand.ImageIndex = 1
            Me.btnExpand.ImageList = Me.ilImages
            Me.btnExpand.Location = New System.Drawing.Point(20, 136)
            Me.btnExpand.Name = "btnExpand"
            Me.btnExpand.Size = New System.Drawing.Size(24, 24)
            Me.btnExpand.TabIndex = 4
            '
            'ilImages
            '
            Me.ilImages.ImageSize = New System.Drawing.Size(9, 9)
            Me.ilImages.ImageStream = CType(resources.GetObject("ilImages.WorkspaceImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ilImages.TransparentColor = System.Drawing.Color.Transparent
            '
            'txtErrorDetails
            '
            Me.txtErrorDetails.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.txtErrorDetails.BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(128, Byte), CType(128, Byte))
            Me.txtErrorDetails.Location = New System.Drawing.Point(20, 168)
            Me.txtErrorDetails.Multiline = True
            Me.txtErrorDetails.Name = "txtErrorDetails"
            Me.txtErrorDetails.ReadOnly = True
            Me.txtErrorDetails.Size = New System.Drawing.Size(272, 100)
            Me.txtErrorDetails.TabIndex = 5
            Me.txtErrorDetails.Text = ""
            Me.txtErrorDetails.Visible = False
            '
            'ErrorDisplay
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(304, 318)
            Me.Controls.Add(Me.btnExpand)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.txtErrorDetails)
            Me.Controls.Add(Me.lblDetails)
            Me.Controls.Add(Me.lblError)
            Me.Controls.Add(Me.txtErrorMsg)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "ErrorDisplay"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Error"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_exError As System.Exception
        Protected m_bExpanded As Boolean = False

#End Region

#Region " Properties "

        Public Property Exception() As System.Exception
            Get
                Return m_exError
            End Get
            Set(ByVal Value As System.Exception)
                m_exError = Value

                If Not m_exError Is Nothing Then
                    Me.txtErrorMsg.Text = m_exError.Message
                    Me.txtErrorDetails.Text = Util.GetErrorDetails("", m_exError)
                    Util.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.ErrorType, m_exError.Message & vbCrLf & Me.txtErrorDetails.Text)
                End If
            End Set
        End Property

        Public Property DisplayErrorDetails() As Boolean
            Get
                Return m_bExpanded
            End Get
            Set(ByVal Value As Boolean)

                If Not Value Then
                    m_bExpanded = False
                    txtErrorDetails.Visible = False
                    btnExpand.ImageIndex = 0
                    lblDetails.Text = "Expand Error Details"
                    lblDetails.TextAlign = ContentAlignment.MiddleLeft
                Else
                    m_bExpanded = True
                    txtErrorDetails.Visible = True
                    btnExpand.ImageIndex = 1
                    lblDetails.Text = "Error Details"
                    lblDetails.TextAlign = ContentAlignment.MiddleCenter
                End If

            End Set
        End Property

#End Region

        Protected Sub ToggleDetailExpansion()

            If m_bExpanded Then
                DisplayErrorDetails = False
                Me.Height = CInt(Me.Height / 2)
            Else
                DisplayErrorDetails = True
                Me.Height = Me.Height * 2
            End If

        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Me.Close()
        End Sub

        Private Sub btnExpand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExpand.Click
            Try
                ToggleDetailExpansion()
            Catch ex As System.Exception
                Util.ShowMessage(ex.Message)
            End Try
        End Sub

        Protected Sub ResizeForm()
            Dim iTextHeight As Integer

            If m_bExpanded Then
                iTextHeight = CInt((Me.Height - lblError.Height - lblDetails.Height - btnOk.Height - 90) / 2)
            Else
                iTextHeight = (Me.Height - lblError.Height - lblDetails.Height - btnOk.Height - 75)
            End If

            txtErrorMsg.Height = iTextHeight
            txtErrorDetails.Height = iTextHeight

            lblError.Top = 10
            lblError.Left = 10
            lblError.Width = Me.Width - 30

            txtErrorMsg.Top = lblError.Top + lblError.Height + 10
            txtErrorMsg.Left = 10
            txtErrorMsg.Width = Me.Width - 30

            btnExpand.Top = txtErrorMsg.Top + txtErrorMsg.Height + 5
            btnExpand.Left = 10

            lblDetails.Top = txtErrorMsg.Top + txtErrorMsg.Height + 10
            lblDetails.Left = btnExpand.Left + btnExpand.Width + 10
            lblDetails.Width = Me.Width - btnExpand.Width - 50

            txtErrorDetails.Top = lblDetails.Top + lblDetails.Height + 10
            txtErrorDetails.Left = 10
            txtErrorDetails.Width = Me.Width - 30

            btnOk.Left = CInt(Me.Width / 2) - CInt(btnOk.Width / 2)
            btnOk.Top = Me.Height - btnOk.Height - 40

            Me.Invalidate()

        End Sub

        Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
            Try
                MyBase.OnResize(e)
                ResizeForm()
            Catch ex As System.Exception
                Util.ShowMessage(ex.Message)
            End Try
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                Util.AddActiveDialog(Me)

                ResizeForm()
            Catch ex As System.Exception
                Util.ShowMessage(ex.Message)
            End Try
        End Sub

        Public Overridable Function GetErrorMessage() As String
            If Not m_exError Is Nothing Then
                Return Me.txtErrorMsg.Text & vbCrLf & Me.txtErrorDetails.Text
            Else
                Return ""
            End If
        End Function

        Protected Overridable Sub AnimatDialog_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
            Util.RemoveActiveDialog(Me)
        End Sub

        Public Overridable Sub ClickOkButton()
            btnOk.PerformClick()
        End Sub

    End Class

End Namespace


