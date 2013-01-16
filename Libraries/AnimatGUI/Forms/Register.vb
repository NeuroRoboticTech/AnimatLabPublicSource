Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports System.Runtime.Remoting
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Forms

    Public Class Register
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
        Friend WithEvents btnRegister As System.Windows.Forms.Button
        Friend WithEvents btnRegisterOffline As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.btnRegister = New System.Windows.Forms.Button()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.txtSerialNumber = New System.Windows.Forms.TextBox()
            Me.btnRegisterOffline = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'btnRegister
            '
            Me.btnRegister.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnRegister.Location = New System.Drawing.Point(273, 64)
            Me.btnRegister.Name = "btnRegister"
            Me.btnRegister.Size = New System.Drawing.Size(95, 24)
            Me.btnRegister.TabIndex = 2
            Me.btnRegister.Text = "Register Online"
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(13, 13)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(299, 14)
            Me.Label1.TabIndex = 3
            Me.Label1.Text = "Please enter the serial number from your invoice to upgrade"
            '
            'txtSerialNumber
            '
            Me.txtSerialNumber.Location = New System.Drawing.Point(13, 30)
            Me.txtSerialNumber.Name = "txtSerialNumber"
            Me.txtSerialNumber.Size = New System.Drawing.Size(460, 20)
            Me.txtSerialNumber.TabIndex = 4
            '
            'btnRegisterOffline
            '
            Me.btnRegisterOffline.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnRegisterOffline.Location = New System.Drawing.Point(374, 64)
            Me.btnRegisterOffline.Name = "btnRegisterOffline"
            Me.btnRegisterOffline.Size = New System.Drawing.Size(94, 24)
            Me.btnRegisterOffline.TabIndex = 5
            Me.btnRegisterOffline.Text = "Register Offline"
            '
            'Register
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(480, 100)
            Me.Controls.Add(Me.btnRegisterOffline)
            Me.Controls.Add(Me.txtSerialNumber)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.btnRegister)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Name = "Register"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Register Application"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents txtSerialNumber As System.Windows.Forms.TextBox

#End Region

        Private Sub btnRegister_Click(sender As System.Object, e As System.EventArgs) Handles btnRegister.Click
            Try
                Util.Application.AppIsBusy = True

                If txtSerialNumber.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must enter a serial number.")
                End If

                Util.Application.SecurityMgr.ValidateSerialNumber(txtSerialNumber.Text)

                If Util.Application.SecurityMgr.IsValidSerialNumber() Then
                    Util.ShowMessage("Registratioin successful. Thank you for purchasing AnimatLab Pro!", "Registration Result", MessageBoxButtons.OK)
                    Util.Application.RegisterStripMenuItem.Visible = False
                    Util.Application.toolStripSeparatorHelp2.Visible = False
                    Me.Close()
                Else
                    Util.ShowMessage(Util.Application.SecurityMgr.ValidationFailureError, "Registration Result", MessageBoxButtons.OK)
                End If

            Catch ex As Exception
                Util.DisplayError(ex)
            Finally
                Util.Application.AppIsBusy = False
            End Try
        End Sub

        Private Sub btnRegisterOffline_Click(sender As System.Object, e As System.EventArgs) Handles btnRegisterOffline.Click

            Try
                If txtSerialNumber.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must enter a serial number.")
                End If

                'Dim strParms As String = "mailto:support@animatlab.com?subject=AnimatLab Pro Offline Activation&body=Send this email to the address above with the following information. " & _
                '                         "A new machine specific registration will be emailed back to you as soon as possible." & _
                '                         "Serial Number: " & txtSerialNumber.Text & ", Machine code: " & Util.Application.SecurityMgr.MachineCode
                'System.Diagnostics.Process.Start(strParms)


                Dim strText As String = "Send an email to the address below with given subject and body text. " & vbCrLf & _
                                         "A new machine specific registration will be emailed back to you as soon as possible." & vbCrLf & vbCrLf & _
                                         "mailto: support@animatlab.com" & vbCrLf & _
                                         "subject=AnimatLab Pro Offline Activation" & vbCrLf & _
                                         "body= Serial Number: " & txtSerialNumber.Text & vbCrLf & _
                                         " Machine code: " & Util.Application.SecurityMgr.MachineCode & vbCrLf
                Util.ShowMessage(strText, "Registration result", MessageBoxButtons.OK, 650, 200, ContentAlignment.MiddleLeft)

            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

    End Class

End Namespace
