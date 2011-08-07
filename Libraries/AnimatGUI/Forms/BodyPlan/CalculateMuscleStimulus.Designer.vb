Namespace Forms.BodyPlan

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CalculateMuscleStimulus
        Inherits AnimatGUI.Forms.AnimatDialog

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
            Me.label6 = New System.Windows.Forms.Label
            Me.txtXdot = New System.Windows.Forms.TextBox
            Me.label7 = New System.Windows.Forms.Label
            Me.txtTdot = New System.Windows.Forms.TextBox
            Me.btnClose = New System.Windows.Forms.Button
            Me.txtVoltage = New System.Windows.Forms.TextBox
            Me.txtActivation = New System.Windows.Forms.TextBox
            Me.txtTl = New System.Windows.Forms.TextBox
            Me.label5 = New System.Windows.Forms.Label
            Me.label4 = New System.Windows.Forms.Label
            Me.label3 = New System.Windows.Forms.Label
            Me.label2 = New System.Windows.Forms.Label
            Me.txtLengthOffset = New System.Windows.Forms.TextBox
            Me.label1 = New System.Windows.Forms.Label
            Me.txtTension = New System.Windows.Forms.TextBox
            Me.btnCalculate = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'label6
            '
            Me.label6.Location = New System.Drawing.Point(12, 128)
            Me.label6.Name = "label6"
            Me.label6.Size = New System.Drawing.Size(136, 16)
            Me.label6.TabIndex = 43
            Me.label6.Text = "Velocity (m/s)"
            Me.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtXdot
            '
            Me.txtXdot.Location = New System.Drawing.Point(12, 144)
            Me.txtXdot.Name = "txtXdot"
            Me.txtXdot.Size = New System.Drawing.Size(136, 20)
            Me.txtXdot.TabIndex = 42
            Me.txtXdot.Text = "0"
            '
            'label7
            '
            Me.label7.Location = New System.Drawing.Point(12, 88)
            Me.label7.Name = "label7"
            Me.label7.Size = New System.Drawing.Size(136, 16)
            Me.label7.TabIndex = 41
            Me.label7.Text = "Tension Derivative (N/s)"
            Me.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtTdot
            '
            Me.txtTdot.Location = New System.Drawing.Point(12, 104)
            Me.txtTdot.Name = "txtTdot"
            Me.txtTdot.Size = New System.Drawing.Size(136, 20)
            Me.txtTdot.TabIndex = 40
            Me.txtTdot.Text = "0"
            '
            'btnClose
            '
            Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnClose.Location = New System.Drawing.Point(84, 176)
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New System.Drawing.Size(64, 24)
            Me.btnClose.TabIndex = 39
            Me.btnClose.Text = "Close"
            '
            'txtVoltage
            '
            Me.txtVoltage.Location = New System.Drawing.Point(156, 104)
            Me.txtVoltage.Name = "txtVoltage"
            Me.txtVoltage.ReadOnly = True
            Me.txtVoltage.Size = New System.Drawing.Size(136, 20)
            Me.txtVoltage.TabIndex = 38
            '
            'txtActivation
            '
            Me.txtActivation.Location = New System.Drawing.Point(156, 64)
            Me.txtActivation.Name = "txtActivation"
            Me.txtActivation.ReadOnly = True
            Me.txtActivation.Size = New System.Drawing.Size(136, 20)
            Me.txtActivation.TabIndex = 37
            '
            'txtTl
            '
            Me.txtTl.Location = New System.Drawing.Point(156, 24)
            Me.txtTl.Name = "txtTl"
            Me.txtTl.ReadOnly = True
            Me.txtTl.Size = New System.Drawing.Size(136, 20)
            Me.txtTl.TabIndex = 36
            '
            'label5
            '
            Me.label5.Location = New System.Drawing.Point(156, 88)
            Me.label5.Name = "label5"
            Me.label5.Size = New System.Drawing.Size(136, 16)
            Me.label5.TabIndex = 35
            Me.label5.Text = "Voltage (mV)"
            Me.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'label4
            '
            Me.label4.Location = New System.Drawing.Point(156, 48)
            Me.label4.Name = "label4"
            Me.label4.Size = New System.Drawing.Size(136, 16)
            Me.label4.TabIndex = 34
            Me.label4.Text = "Activation Tension (N)"
            Me.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'label3
            '
            Me.label3.Location = New System.Drawing.Point(156, 8)
            Me.label3.Name = "label3"
            Me.label3.Size = New System.Drawing.Size(136, 16)
            Me.label3.TabIndex = 33
            Me.label3.Text = "Tension Length %"
            Me.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'label2
            '
            Me.label2.Location = New System.Drawing.Point(12, 48)
            Me.label2.Name = "label2"
            Me.label2.Size = New System.Drawing.Size(136, 16)
            Me.label2.TabIndex = 32
            Me.label2.Text = "Length Offset (m)"
            Me.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtLengthOffset
            '
            Me.txtLengthOffset.Location = New System.Drawing.Point(12, 64)
            Me.txtLengthOffset.Name = "txtLengthOffset"
            Me.txtLengthOffset.Size = New System.Drawing.Size(136, 20)
            Me.txtLengthOffset.TabIndex = 31
            Me.txtLengthOffset.Text = "0"
            '
            'label1
            '
            Me.label1.Location = New System.Drawing.Point(12, 8)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(136, 16)
            Me.label1.TabIndex = 30
            Me.label1.Text = "Desired Tension (N)"
            Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtTension
            '
            Me.txtTension.Location = New System.Drawing.Point(12, 24)
            Me.txtTension.Name = "txtTension"
            Me.txtTension.Size = New System.Drawing.Size(136, 20)
            Me.txtTension.TabIndex = 29
            Me.txtTension.Text = "10"
            '
            'btnCalculate
            '
            Me.btnCalculate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCalculate.Location = New System.Drawing.Point(12, 176)
            Me.btnCalculate.Name = "btnCalculate"
            Me.btnCalculate.Size = New System.Drawing.Size(64, 24)
            Me.btnCalculate.TabIndex = 28
            Me.btnCalculate.Text = "Calculate"
            '
            'CalculateMuscleStimulus
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(303, 216)
            Me.Controls.Add(Me.label6)
            Me.Controls.Add(Me.txtXdot)
            Me.Controls.Add(Me.label7)
            Me.Controls.Add(Me.txtTdot)
            Me.Controls.Add(Me.btnClose)
            Me.Controls.Add(Me.txtVoltage)
            Me.Controls.Add(Me.txtActivation)
            Me.Controls.Add(Me.txtTl)
            Me.Controls.Add(Me.label5)
            Me.Controls.Add(Me.label4)
            Me.Controls.Add(Me.label3)
            Me.Controls.Add(Me.label2)
            Me.Controls.Add(Me.txtLengthOffset)
            Me.Controls.Add(Me.label1)
            Me.Controls.Add(Me.txtTension)
            Me.Controls.Add(Me.btnCalculate)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Name = "CalculateMuscleStimulus"
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "CalculateMuscleStimulus"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Private WithEvents label6 As System.Windows.Forms.Label
        Private WithEvents txtXdot As System.Windows.Forms.TextBox
        Private WithEvents label7 As System.Windows.Forms.Label
        Private WithEvents txtTdot As System.Windows.Forms.TextBox
        Friend WithEvents btnClose As System.Windows.Forms.Button
        Private WithEvents txtVoltage As System.Windows.Forms.TextBox
        Private WithEvents txtActivation As System.Windows.Forms.TextBox
        Private WithEvents txtTl As System.Windows.Forms.TextBox
        Private WithEvents label5 As System.Windows.Forms.Label
        Private WithEvents label4 As System.Windows.Forms.Label
        Private WithEvents label3 As System.Windows.Forms.Label
        Private WithEvents label2 As System.Windows.Forms.Label
        Private WithEvents txtLengthOffset As System.Windows.Forms.TextBox
        Private WithEvents label1 As System.Windows.Forms.Label
        Private WithEvents txtTension As System.Windows.Forms.TextBox
        Friend WithEvents btnCalculate As System.Windows.Forms.Button
End Class

End Namespace
