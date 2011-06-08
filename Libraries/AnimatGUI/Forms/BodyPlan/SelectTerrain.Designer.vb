Namespace Forms.BodyPlan

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
	Partial Class SelectTerrain
		Inherits System.Windows.Forms.Form

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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SelectTerrain))
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnOk = New System.Windows.Forms.Button
            Me.lblMeshFile = New System.Windows.Forms.Label
            Me.btnMeshFileDlg = New System.Windows.Forms.Button
            Me.txtMeshFile = New System.Windows.Forms.TextBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.SuspendLayout()
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(138, 123)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 11
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnOk
            '
            Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOk.Location = New System.Drawing.Point(57, 123)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(75, 23)
            Me.btnOk.TabIndex = 10
            Me.btnOk.Text = "Ok"
            Me.btnOk.UseVisualStyleBackColor = True
            '
            'lblMeshFile
            '
            Me.lblMeshFile.Location = New System.Drawing.Point(10, 78)
            Me.lblMeshFile.Name = "lblMeshFile"
            Me.lblMeshFile.Size = New System.Drawing.Size(230, 16)
            Me.lblMeshFile.TabIndex = 9
            Me.lblMeshFile.Text = "Terrain File"
            Me.lblMeshFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnMeshFileDlg
            '
            Me.btnMeshFileDlg.Location = New System.Drawing.Point(246, 97)
            Me.btnMeshFileDlg.Name = "btnMeshFileDlg"
            Me.btnMeshFileDlg.Size = New System.Drawing.Size(23, 23)
            Me.btnMeshFileDlg.TabIndex = 8
            Me.btnMeshFileDlg.Text = "..."
            Me.btnMeshFileDlg.UseVisualStyleBackColor = True
            '
            'txtMeshFile
            '
            Me.txtMeshFile.Location = New System.Drawing.Point(10, 97)
            Me.txtMeshFile.Name = "txtMeshFile"
            Me.txtMeshFile.Size = New System.Drawing.Size(230, 20)
            Me.txtMeshFile.TabIndex = 7
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(9, 9)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(257, 71)
            Me.Label1.TabIndex = 16
            Me.Label1.Text = resources.GetString("Label1.Text")
            '
            'SelectTerrain
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(284, 158)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.lblMeshFile)
            Me.Controls.Add(Me.btnMeshFileDlg)
            Me.Controls.Add(Me.txtMeshFile)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Name = "SelectTerrain"
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Select Terrain"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents lblMeshFile As System.Windows.Forms.Label
        Friend WithEvents btnMeshFileDlg As System.Windows.Forms.Button
        Friend WithEvents txtMeshFile As System.Windows.Forms.TextBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
    End Class

End Namespace