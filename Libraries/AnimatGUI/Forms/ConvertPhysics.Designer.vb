Namespace Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ConvertPhysics
        Inherits Forms.AnimatDialog

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
            Me.lblNewPhysics = New System.Windows.Forms.Label()
            Me.cboPhysicsEngine = New System.Windows.Forms.ComboBox()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.btnOk = New System.Windows.Forms.Button()
            Me.lblCurrentPhysics = New System.Windows.Forms.Label()
            Me.lblSaveMessage = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'lblNewPhysics
            '
            Me.lblNewPhysics.Location = New System.Drawing.Point(11, 36)
            Me.lblNewPhysics.Name = "lblNewPhysics"
            Me.lblNewPhysics.Size = New System.Drawing.Size(280, 16)
            Me.lblNewPhysics.TabIndex = 19
            Me.lblNewPhysics.Text = "New Physics Engine"
            Me.lblNewPhysics.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'cboPhysicsEngine
            '
            Me.cboPhysicsEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboPhysicsEngine.FormattingEnabled = True
            Me.cboPhysicsEngine.Location = New System.Drawing.Point(11, 53)
            Me.cboPhysicsEngine.Name = "cboPhysicsEngine"
            Me.cboPhysicsEngine.Size = New System.Drawing.Size(288, 21)
            Me.cboPhysicsEngine.TabIndex = 18
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(163, 121)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 17
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOk.Location = New System.Drawing.Point(91, 121)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 16
            Me.btnOk.Text = "Ok"
            '
            'lblCurrentPhysics
            '
            Me.lblCurrentPhysics.Location = New System.Drawing.Point(8, 9)
            Me.lblCurrentPhysics.Name = "lblCurrentPhysics"
            Me.lblCurrentPhysics.Size = New System.Drawing.Size(280, 16)
            Me.lblCurrentPhysics.TabIndex = 20
            Me.lblCurrentPhysics.Text = "Current Physics Engine: "
            Me.lblCurrentPhysics.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'lblSaveMessage
            '
            Me.lblSaveMessage.Location = New System.Drawing.Point(8, 87)
            Me.lblSaveMessage.Name = "lblSaveMessage"
            Me.lblSaveMessage.Size = New System.Drawing.Size(291, 35)
            Me.lblSaveMessage.TabIndex = 21
            Me.lblSaveMessage.Text = "If you convert to a new physics engine then the project will be automatically sav" & _
        "ed, closed, and re-opened."
            Me.lblSaveMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'ConvertPhysics
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(320, 153)
            Me.Controls.Add(Me.lblSaveMessage)
            Me.Controls.Add(Me.lblCurrentPhysics)
            Me.Controls.Add(Me.lblNewPhysics)
            Me.Controls.Add(Me.cboPhysicsEngine)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Name = "ConvertPhysics"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "ConvertPhysics"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblNewPhysics As System.Windows.Forms.Label
        Friend WithEvents cboPhysicsEngine As System.Windows.Forms.ComboBox
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents lblCurrentPhysics As System.Windows.Forms.Label
        Friend WithEvents lblSaveMessage As System.Windows.Forms.Label
    End Class

End Namespace