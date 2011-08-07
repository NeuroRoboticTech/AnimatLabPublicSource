
Namespace Forms.BodyPlan

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class EditAttachments
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EditAttachments))
            Me.lvAttachments = New System.Windows.Forms.ListView
            Me.colAttachment = New System.Windows.Forms.ColumnHeader
            Me.lvMuscleAttachments = New System.Windows.Forms.ListView
            Me.colSelAttachments = New System.Windows.Forms.ColumnHeader
            Me.btnDown = New System.Windows.Forms.Button
            Me.btnUp = New System.Windows.Forms.Button
            Me.btnDelete = New System.Windows.Forms.Button
            Me.btnAdd = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnOk = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'lvAttachments
            '
            Me.lvAttachments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lvAttachments.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAttachment})
            Me.lvAttachments.FullRowSelect = True
            Me.lvAttachments.GridLines = True
            Me.lvAttachments.HideSelection = False
            Me.lvAttachments.Location = New System.Drawing.Point(8, 16)
            Me.lvAttachments.Name = "lvAttachments"
            Me.lvAttachments.Size = New System.Drawing.Size(168, 176)
            Me.lvAttachments.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.lvAttachments.TabIndex = 0
            Me.lvAttachments.UseCompatibleStateImageBehavior = False
            Me.lvAttachments.View = System.Windows.Forms.View.Details
            '
            'colAttachment
            '
            Me.colAttachment.Text = "Available Attachments"
            Me.colAttachment.Width = 160
            '
            'lvMuscleAttachments
            '
            Me.lvMuscleAttachments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lvMuscleAttachments.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colSelAttachments})
            Me.lvMuscleAttachments.FullRowSelect = True
            Me.lvMuscleAttachments.GridLines = True
            Me.lvMuscleAttachments.HideSelection = False
            Me.lvMuscleAttachments.Location = New System.Drawing.Point(184, 16)
            Me.lvMuscleAttachments.Name = "lvMuscleAttachments"
            Me.lvMuscleAttachments.Size = New System.Drawing.Size(200, 240)
            Me.lvMuscleAttachments.TabIndex = 1
            Me.lvMuscleAttachments.UseCompatibleStateImageBehavior = False
            Me.lvMuscleAttachments.View = System.Windows.Forms.View.Details
            '
            'colSelAttachments
            '
            Me.colSelAttachments.Text = "Selected Attachments"
            Me.colSelAttachments.Width = 170
            '
            'btnDown
            '
            Me.btnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnDown.Image = CType(resources.GetObject("btnDown.Image"), System.Drawing.Image)
            Me.btnDown.Location = New System.Drawing.Point(152, 232)
            Me.btnDown.Name = "btnDown"
            Me.btnDown.Size = New System.Drawing.Size(24, 24)
            Me.btnDown.TabIndex = 21
            '
            'btnUp
            '
            Me.btnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnUp.Image = CType(resources.GetObject("btnUp.Image"), System.Drawing.Image)
            Me.btnUp.Location = New System.Drawing.Point(152, 200)
            Me.btnUp.Name = "btnUp"
            Me.btnUp.Size = New System.Drawing.Size(24, 24)
            Me.btnUp.TabIndex = 20
            '
            'btnDelete
            '
            Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnDelete.Location = New System.Drawing.Point(80, 200)
            Me.btnDelete.Name = "btnDelete"
            Me.btnDelete.Size = New System.Drawing.Size(64, 24)
            Me.btnDelete.TabIndex = 19
            Me.btnDelete.Text = "Remove"
            '
            'btnAdd
            '
            Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnAdd.Location = New System.Drawing.Point(8, 200)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(64, 24)
            Me.btnAdd.TabIndex = 18
            Me.btnAdd.Text = "Add"
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(80, 232)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 17
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(8, 232)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 16
            Me.btnOk.Text = "Ok"
            '
            'EditAttachments
            '
            Me.ClientSize = New System.Drawing.Size(392, 266)
            Me.Controls.Add(Me.btnDown)
            Me.Controls.Add(Me.btnUp)
            Me.Controls.Add(Me.btnDelete)
            Me.Controls.Add(Me.btnAdd)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.lvMuscleAttachments)
            Me.Controls.Add(Me.lvAttachments)
            Me.Name = "EditAttachments"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Edit Attachments"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lvAttachments As System.Windows.Forms.ListView
        Friend WithEvents colAttachment As System.Windows.Forms.ColumnHeader
        Friend WithEvents lvMuscleAttachments As System.Windows.Forms.ListView
        Friend WithEvents colSelAttachments As System.Windows.Forms.ColumnHeader
        Private WithEvents btnDown As System.Windows.Forms.Button
        Private WithEvents btnUp As System.Windows.Forms.Button
        Friend WithEvents btnDelete As System.Windows.Forms.Button
        Friend WithEvents btnAdd As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button

    End Class

End Namespace
