Namespace Forms.BodyPlan

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
	Partial Class SelectMesh
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SelectMesh))
            Me.txtMeshFile = New System.Windows.Forms.TextBox
            Me.btnMeshFileDlg = New System.Windows.Forms.Button
            Me.lblMeshFile = New System.Windows.Forms.Label
            Me.cboMeshType = New System.Windows.Forms.ComboBox
            Me.lblMeshType = New System.Windows.Forms.Label
            Me.btnOk = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.Label1 = New System.Windows.Forms.Label
            Me.SuspendLayout()
            '
            'txtMeshFile
            '
            Me.txtMeshFile.Location = New System.Drawing.Point(10, 103)
            Me.txtMeshFile.Name = "txtMeshFile"
            Me.txtMeshFile.Size = New System.Drawing.Size(230, 20)
            Me.txtMeshFile.TabIndex = 0
            '
            'btnMeshFileDlg
            '
            Me.btnMeshFileDlg.Location = New System.Drawing.Point(246, 103)
            Me.btnMeshFileDlg.Name = "btnMeshFileDlg"
            Me.btnMeshFileDlg.Size = New System.Drawing.Size(23, 23)
            Me.btnMeshFileDlg.TabIndex = 1
            Me.btnMeshFileDlg.Text = "..."
            Me.btnMeshFileDlg.UseVisualStyleBackColor = True
            '
            'lblMeshFile
            '
            Me.lblMeshFile.Location = New System.Drawing.Point(10, 84)
            Me.lblMeshFile.Name = "lblMeshFile"
            Me.lblMeshFile.Size = New System.Drawing.Size(230, 16)
            Me.lblMeshFile.TabIndex = 2
            Me.lblMeshFile.Text = "Mesh File"
            Me.lblMeshFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'cboMeshType
            '
            Me.cboMeshType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboMeshType.FormattingEnabled = True
            Me.cboMeshType.Items.AddRange(New Object() {"Convex", "Triangular"})
            Me.cboMeshType.Location = New System.Drawing.Point(10, 152)
            Me.cboMeshType.Name = "cboMeshType"
            Me.cboMeshType.Size = New System.Drawing.Size(230, 21)
            Me.cboMeshType.TabIndex = 3
            '
            'lblMeshType
            '
            Me.lblMeshType.Location = New System.Drawing.Point(10, 133)
            Me.lblMeshType.Name = "lblMeshType"
            Me.lblMeshType.Size = New System.Drawing.Size(230, 16)
            Me.lblMeshType.TabIndex = 4
            Me.lblMeshType.Text = "Mesh Type"
            Me.lblMeshType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnOk
            '
            Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOk.Location = New System.Drawing.Point(62, 190)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(75, 23)
            Me.btnOk.TabIndex = 5
            Me.btnOk.Text = "Ok"
            Me.btnOk.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(143, 190)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 6
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(10, 13)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(257, 71)
            Me.Label1.TabIndex = 7
            Me.Label1.Text = resources.GetString("Label1.Text")
            '
            'SelectMesh
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(279, 225)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.lblMeshType)
            Me.Controls.Add(Me.cboMeshType)
            Me.Controls.Add(Me.lblMeshFile)
            Me.Controls.Add(Me.btnMeshFileDlg)
            Me.Controls.Add(Me.txtMeshFile)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Name = "SelectMesh"
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Select Mesh"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents txtMeshFile As System.Windows.Forms.TextBox
        Friend WithEvents btnMeshFileDlg As System.Windows.Forms.Button
        Friend WithEvents lblMeshFile As System.Windows.Forms.Label
        Friend WithEvents cboMeshType As System.Windows.Forms.ComboBox
        Friend WithEvents lblMeshType As System.Windows.Forms.Label
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents Label1 As System.Windows.Forms.Label
	End Class

End Namespace
