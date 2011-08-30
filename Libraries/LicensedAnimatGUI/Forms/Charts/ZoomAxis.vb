Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI
Imports AnimatGUI.Forms
Imports AnimatGUI.Framework
Imports Gigasoft
Imports Gigasoft.ProEssentials.Api

Namespace Forms.Charts

    Public Class ZoomAxis
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
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents txtXMin As System.Windows.Forms.TextBox
        Friend WithEvents txtXMax As System.Windows.Forms.TextBox
        Friend WithEvents lblXMin As System.Windows.Forms.Label
        Friend WithEvents lblXMax As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.btnOk = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.txtXMin = New System.Windows.Forms.TextBox
            Me.txtXMax = New System.Windows.Forms.TextBox
            Me.lblXMin = New System.Windows.Forms.Label
            Me.lblXMax = New System.Windows.Forms.Label
            Me.SuspendLayout()
            '
            'btnOk
            '
            Me.btnOk.Location = New System.Drawing.Point(8, 64)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 0
            Me.btnOk.Text = "Ok"
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(80, 64)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 1
            Me.btnCancel.Text = "Cancel"
            '
            'txtXMin
            '
            Me.txtXMin.Location = New System.Drawing.Point(16, 32)
            Me.txtXMin.Name = "txtXMin"
            Me.txtXMin.Size = New System.Drawing.Size(56, 20)
            Me.txtXMin.TabIndex = 2
            Me.txtXMin.Text = ""
            '
            'txtXMax
            '
            Me.txtXMax.Location = New System.Drawing.Point(80, 32)
            Me.txtXMax.Name = "txtXMax"
            Me.txtXMax.Size = New System.Drawing.Size(56, 20)
            Me.txtXMax.TabIndex = 3
            Me.txtXMax.Text = ""
            '
            'lblXMin
            '
            Me.lblXMin.Location = New System.Drawing.Point(16, 16)
            Me.lblXMin.Name = "lblXMin"
            Me.lblXMin.Size = New System.Drawing.Size(56, 16)
            Me.lblXMin.TabIndex = 4
            Me.lblXMin.Text = "X Min"
            Me.lblXMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblXMax
            '
            Me.lblXMax.Location = New System.Drawing.Point(80, 16)
            Me.lblXMax.Name = "lblXMax"
            Me.lblXMax.Size = New System.Drawing.Size(56, 16)
            Me.lblXMax.TabIndex = 5
            Me.lblXMax.Text = "X Max"
            Me.lblXMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'ZoomAxis
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(152, 102)
            Me.Controls.Add(Me.lblXMax)
            Me.Controls.Add(Me.lblXMin)
            Me.Controls.Add(Me.txtXMax)
            Me.Controls.Add(Me.txtXMin)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "ZoomAxis"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Zoom Axis"
            Me.ResumeLayout(False)

        End Sub

#End Region

        Public m_fltMin As Single
        Public m_fltMax As Single

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            txtXMin.Text = m_fltMin.ToString("0.###")
            txtXMax.Text = m_fltMax.ToString("0.###")
        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

            Try
                If txtXMin.Text.Trim.Length = 0 OrElse txtXMax.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify a number for both the minimum and maximum x value.")
                End If

                If Not IsNumeric(txtXMin.Text) OrElse Not IsNumeric(txtXMax.Text) Then
                    Throw New System.Exception("The values must be numeric.")
                End If

                m_fltMin = CSng(txtXMin.Text)
                m_fltMax = CSng(txtXMax.Text)

                If m_fltMin >= m_fltMax Then
                    Throw New System.Exception("The minimum value must be less than or greater than the maximum value.")
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

    End Class

End Namespace
