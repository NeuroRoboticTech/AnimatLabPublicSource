Namespace Forms.BodyPlan

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
	Partial Class SelectTerrain
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SelectTerrain))
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnOk = New System.Windows.Forms.Button
            Me.lblMeshFile = New System.Windows.Forms.Label
            Me.btnMeshFileDlg = New System.Windows.Forms.Button
            Me.txtMeshFile = New System.Windows.Forms.TextBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.txtSegmentWidth = New System.Windows.Forms.TextBox
            Me.Label2 = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.txtSegmentLength = New System.Windows.Forms.TextBox
            Me.Label4 = New System.Windows.Forms.Label
            Me.txtMaxHeight = New System.Windows.Forms.TextBox
            Me.Label5 = New System.Windows.Forms.Label
            Me.btnTextureFileDlg = New System.Windows.Forms.Button
            Me.txtTextureFile = New System.Windows.Forms.TextBox
            Me.Label6 = New System.Windows.Forms.Label
            Me.SuspendLayout()
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(145, 236)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 11
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnOk
            '
            Me.btnOk.Location = New System.Drawing.Point(64, 236)
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
            Me.btnMeshFileDlg.Location = New System.Drawing.Point(252, 97)
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
            Me.txtMeshFile.Size = New System.Drawing.Size(236, 20)
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
            'txtSegmentWidth
            '
            Me.txtSegmentWidth.Location = New System.Drawing.Point(12, 210)
            Me.txtSegmentWidth.Name = "txtSegmentWidth"
            Me.txtSegmentWidth.Size = New System.Drawing.Size(88, 20)
            Me.txtSegmentWidth.TabIndex = 17
            Me.txtSegmentWidth.Text = "1"
            '
            'Label2
            '
            Me.Label2.Location = New System.Drawing.Point(11, 189)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(88, 20)
            Me.Label2.TabIndex = 18
            Me.Label2.Text = "Segment Width"
            Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'Label3
            '
            Me.Label3.Location = New System.Drawing.Point(101, 189)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(88, 20)
            Me.Label3.TabIndex = 20
            Me.Label3.Text = "Segment Length"
            Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtSegmentLength
            '
            Me.txtSegmentLength.Location = New System.Drawing.Point(102, 210)
            Me.txtSegmentLength.Name = "txtSegmentLength"
            Me.txtSegmentLength.Size = New System.Drawing.Size(88, 20)
            Me.txtSegmentLength.TabIndex = 19
            Me.txtSegmentLength.Text = "1"
            '
            'Label4
            '
            Me.Label4.Location = New System.Drawing.Point(191, 189)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(88, 20)
            Me.Label4.TabIndex = 22
            Me.Label4.Text = "Max Height"
            Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtMaxHeight
            '
            Me.txtMaxHeight.Location = New System.Drawing.Point(192, 210)
            Me.txtMaxHeight.Name = "txtMaxHeight"
            Me.txtMaxHeight.Size = New System.Drawing.Size(88, 20)
            Me.txtMaxHeight.TabIndex = 21
            Me.txtMaxHeight.Text = "5"
            '
            'Label5
            '
            Me.Label5.Location = New System.Drawing.Point(10, 120)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(230, 16)
            Me.Label5.TabIndex = 25
            Me.Label5.Text = "Texture File"
            Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnTextureFileDlg
            '
            Me.btnTextureFileDlg.Location = New System.Drawing.Point(252, 139)
            Me.btnTextureFileDlg.Name = "btnTextureFileDlg"
            Me.btnTextureFileDlg.Size = New System.Drawing.Size(23, 23)
            Me.btnTextureFileDlg.TabIndex = 24
            Me.btnTextureFileDlg.Text = "..."
            Me.btnTextureFileDlg.UseVisualStyleBackColor = True
            '
            'txtTextureFile
            '
            Me.txtTextureFile.Location = New System.Drawing.Point(10, 139)
            Me.txtTextureFile.Name = "txtTextureFile"
            Me.txtTextureFile.Size = New System.Drawing.Size(236, 20)
            Me.txtTextureFile.TabIndex = 23
            '
            'Label6
            '
            Me.Label6.Location = New System.Drawing.Point(9, 173)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(271, 16)
            Me.Label6.TabIndex = 26
            Me.Label6.Text = "All distances are in meters."
            Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'SelectTerrain
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(287, 271)
            Me.Controls.Add(Me.Label6)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.btnTextureFileDlg)
            Me.Controls.Add(Me.txtTextureFile)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.txtMaxHeight)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.txtSegmentLength)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.txtSegmentWidth)
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
        Friend WithEvents txtSegmentWidth As System.Windows.Forms.TextBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents txtSegmentLength As System.Windows.Forms.TextBox
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents txtMaxHeight As System.Windows.Forms.TextBox
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents btnTextureFileDlg As System.Windows.Forms.Button
        Friend WithEvents txtTextureFile As System.Windows.Forms.TextBox
        Friend WithEvents Label6 As System.Windows.Forms.Label
    End Class

End Namespace