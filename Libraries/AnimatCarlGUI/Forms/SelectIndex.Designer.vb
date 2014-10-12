Namespace Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectIndex
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
        Me.lbIndices = New System.Windows.Forms.ListBox()
        Me.cboNeurons = New System.Windows.Forms.ComboBox()
        Me.lblNeurons = New System.Windows.Forms.Label()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'lbIndices
            '
            Me.lbIndices.FormattingEnabled = True
            Me.lbIndices.Location = New System.Drawing.Point(13, 13)
            Me.lbIndices.Name = "lbIndices"
            Me.lbIndices.Size = New System.Drawing.Size(127, 355)
            Me.lbIndices.Sorted = True
            Me.lbIndices.TabIndex = 0
            '
            'cboNeurons
            '
            Me.cboNeurons.FormattingEnabled = True
            Me.cboNeurons.Location = New System.Drawing.Point(147, 40)
            Me.cboNeurons.Name = "cboNeurons"
            Me.cboNeurons.Size = New System.Drawing.Size(104, 21)
            Me.cboNeurons.Sorted = True
            Me.cboNeurons.TabIndex = 1
            '
            'lblNeurons
            '
            Me.lblNeurons.Location = New System.Drawing.Point(147, 21)
            Me.lblNeurons.Name = "lblNeurons"
            Me.lblNeurons.Size = New System.Drawing.Size(100, 16)
            Me.lblNeurons.TabIndex = 2
            Me.lblNeurons.Text = "Neurons"
            Me.lblNeurons.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnAdd
            '
            Me.btnAdd.Location = New System.Drawing.Point(151, 74)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(100, 23)
            Me.btnAdd.TabIndex = 3
            Me.btnAdd.Text = "Add"
            Me.btnAdd.UseVisualStyleBackColor = True
            '
            'btnRemove
            '
            Me.btnRemove.Location = New System.Drawing.Point(152, 103)
            Me.btnRemove.Name = "btnRemove"
            Me.btnRemove.Size = New System.Drawing.Size(99, 23)
            Me.btnRemove.TabIndex = 4
            Me.btnRemove.Text = "Remove"
            Me.btnRemove.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(152, 345)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(99, 23)
            Me.btnCancel.TabIndex = 5
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnOk
            '
            Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOk.Location = New System.Drawing.Point(152, 316)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(99, 23)
            Me.btnOk.TabIndex = 6
            Me.btnOk.Text = "OK"
            Me.btnOk.UseVisualStyleBackColor = True
            '
            'SelectIndex
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(262, 380)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnRemove)
            Me.Controls.Add(Me.btnAdd)
            Me.Controls.Add(Me.lblNeurons)
            Me.Controls.Add(Me.cboNeurons)
            Me.Controls.Add(Me.lbIndices)
            Me.Name = "SelectIndex"
            Me.Text = "Select Indices"
            Me.ResumeLayout(False)

        End Sub
    Friend WithEvents lbIndices As System.Windows.Forms.ListBox
    Friend WithEvents cboNeurons As System.Windows.Forms.ComboBox
    Friend WithEvents lblNeurons As System.Windows.Forms.Label
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
End Class

End Namespace