Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Namespace Forms

    Public Class MultiLineStringEditor
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
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents TextBox1 As System.Windows.Forms.RichTextBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnOk = New System.Windows.Forms.Button
            Me.TextBox1 = New System.Windows.Forms.RichTextBox
            Me.SuspendLayout()
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(232, 120)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(72, 24)
            Me.btnCancel.TabIndex = 4
            Me.btnCancel.Text = "&Cancel"
            '
            'btnOk
            '
            Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOk.Location = New System.Drawing.Point(312, 120)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(72, 24)
            Me.btnOk.TabIndex = 3
            Me.btnOk.Text = "&Ok"
            '
            'TextBox1
            '
            Me.TextBox1.Location = New System.Drawing.Point(0, 0)
            Me.TextBox1.Multiline = True
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Size = New System.Drawing.Size(392, 112)
            Me.TextBox1.TabIndex = 5
            Me.TextBox1.Text = ""
            Me.TextBox1.DetectUrls = True
            '
            'MultiLineStringEditor
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(392, 149)
            Me.Controls.Add(Me.TextBox1)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
            Me.Name = "MultiLineStringEditor"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.ResumeLayout(False)

        End Sub

#End Region

        ' i put the form declaration & initialisation here
        ' then i can have 2 methods to call & open up the editor
        Private Shared Form As New MultiLineStringEditor

        Public Shared Sub Edit(ByRef strInput As String)
            ' set the textbox text to the inputvalues' string-representation
            Form.TextData = strInput
            ' if user hit "ok" then change the InputValue's value to the new string
            If Form.ShowDialog() = Form.DialogResult.OK Then strInput = Form.TextData
        End Sub

        Public Shared Sub Edit(ByRef strInput As String, ByVal Title As String)
            Form.Text = Title
            Edit(strInput)
        End Sub

        Public Property TextData() As String
            Get
                Return CType(TextBox1.Text, String)
            End Get
            Set(ByVal Value As String)
                TextBox1.Text = Value
            End Set
        End Property

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
            MyBase.OnResize(e)

            Try
                btnOk.Left = CInt((Me.Width / 2) - btnOk.Width - 2)
                btnCancel.Left = CInt((Me.Width / 2) + 2)

            Catch ex As System.Exception

            End Try
        End Sub

        Private Sub TextBox1_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles TextBox1.LinkClicked
            Try
                System.Diagnostics.Process.Start(e.LinkText)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub
    End Class

End Namespace
