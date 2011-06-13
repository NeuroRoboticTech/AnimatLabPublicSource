Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects
Imports System.Drawing.Printing
Imports System.Drawing.Imaging

Namespace Forms.BodyPlan

    Public Class Editor_ToolStrips
        Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Editor_ToolStrips))
            Me.EditorMenuStrip = New AnimatGuiCtrls.Controls.AnimatMenuStrip
            Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.PrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.PrintPreviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.toolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
            Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.AddStimulusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.toolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
            Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.toolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
            Me.SelectByTypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.RelabelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.RelabelSelectedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.CompareItemsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.EditorToolStrip = New AnimatGuiCtrls.Controls.AnimatToolStrip
            Me.AddStimulusToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.SelectByTypeToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.RelabelToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.RelabelSelectedToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
            Me.AddPartToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.AddJointToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.CompareItemsToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.EditorMenuStrip.SuspendLayout()
            Me.EditorToolStrip.SuspendLayout()
            Me.SuspendLayout()
            '
            'EditorMenuStrip
            '
            Me.EditorMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem})
            Me.EditorMenuStrip.Location = New System.Drawing.Point(0, 0)
            Me.EditorMenuStrip.Name = "EditorMenuStrip"
            Me.EditorMenuStrip.SecurityMgr = Nothing
            Me.EditorMenuStrip.Size = New System.Drawing.Size(387, 24)
            Me.EditorMenuStrip.TabIndex = 0
            Me.EditorMenuStrip.Text = "EditorMenuStrip"
            Me.EditorMenuStrip.ToolName = ""
            Me.EditorMenuStrip.Visible = False
            '
            'FileToolStripMenuItem
            '
            Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PrintToolStripMenuItem, Me.PrintPreviewToolStripMenuItem, Me.toolStripSeparator2})
            Me.FileToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly
            Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
            Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
            Me.FileToolStripMenuItem.Text = "&File"
            '
            'PrintToolStripMenuItem
            '
            Me.PrintToolStripMenuItem.Image = CType(resources.GetObject("PrintToolStripMenuItem.Image"), System.Drawing.Image)
            Me.PrintToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PrintToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.PrintToolStripMenuItem.MergeIndex = 6
            Me.PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
            Me.PrintToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
            Me.PrintToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.PrintToolStripMenuItem.Text = "&Print"
            '
            'PrintPreviewToolStripMenuItem
            '
            Me.PrintPreviewToolStripMenuItem.Image = CType(resources.GetObject("PrintPreviewToolStripMenuItem.Image"), System.Drawing.Image)
            Me.PrintPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PrintPreviewToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.PrintPreviewToolStripMenuItem.MergeIndex = 7
            Me.PrintPreviewToolStripMenuItem.Name = "PrintPreviewToolStripMenuItem"
            Me.PrintPreviewToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.PrintPreviewToolStripMenuItem.Text = "Print Pre&view"
            '
            'toolStripSeparator2
            '
            Me.toolStripSeparator2.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.toolStripSeparator2.MergeIndex = 8
            Me.toolStripSeparator2.Name = "toolStripSeparator2"
            Me.toolStripSeparator2.Size = New System.Drawing.Size(149, 6)
            '
            'EditToolStripMenuItem
            '
            Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddStimulusToolStripMenuItem, Me.toolStripSeparator3, Me.CutToolStripMenuItem, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.toolStripSeparator4, Me.SelectByTypeToolStripMenuItem, Me.RelabelToolStripMenuItem, Me.RelabelSelectedToolStripMenuItem, Me.CompareItemsToolStripMenuItem})
            Me.EditToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly
            Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
            Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
            Me.EditToolStripMenuItem.Text = "&Edit"
            '
            'AddStimulusToolStripMenuItem
            '
            Me.AddStimulusToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.AddStimulusToolStripMenuItem.MergeIndex = 7
            Me.AddStimulusToolStripMenuItem.Name = "AddStimulusToolStripMenuItem"
            Me.AddStimulusToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.AddStimulusToolStripMenuItem.Text = "Add Stimulus"
            '
            'toolStripSeparator3
            '
            Me.toolStripSeparator3.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.toolStripSeparator3.MergeIndex = 8
            Me.toolStripSeparator3.Name = "toolStripSeparator3"
            Me.toolStripSeparator3.Size = New System.Drawing.Size(156, 6)
            '
            'CutToolStripMenuItem
            '
            Me.CutToolStripMenuItem.Image = CType(resources.GetObject("CutToolStripMenuItem.Image"), System.Drawing.Image)
            Me.CutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CutToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.CutToolStripMenuItem.MergeIndex = 9
            Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
            Me.CutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
            Me.CutToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.CutToolStripMenuItem.Text = "Cu&t"
            '
            'CopyToolStripMenuItem
            '
            Me.CopyToolStripMenuItem.Image = CType(resources.GetObject("CopyToolStripMenuItem.Image"), System.Drawing.Image)
            Me.CopyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CopyToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.CopyToolStripMenuItem.MergeIndex = 10
            Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
            Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
            Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.CopyToolStripMenuItem.Text = "&Copy"
            '
            'PasteToolStripMenuItem
            '
            Me.PasteToolStripMenuItem.Image = CType(resources.GetObject("PasteToolStripMenuItem.Image"), System.Drawing.Image)
            Me.PasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PasteToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.PasteToolStripMenuItem.MergeIndex = 11
            Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
            Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
            Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.PasteToolStripMenuItem.Text = "&Paste"
            '
            'toolStripSeparator4
            '
            Me.toolStripSeparator4.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.toolStripSeparator4.MergeIndex = 13
            Me.toolStripSeparator4.Name = "toolStripSeparator4"
            Me.toolStripSeparator4.Size = New System.Drawing.Size(156, 6)
            '
            'SelectByTypeToolStripMenuItem
            '
            Me.SelectByTypeToolStripMenuItem.Name = "SelectByTypeToolStripMenuItem"
            Me.SelectByTypeToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.SelectByTypeToolStripMenuItem.Text = "Select By Type"
            '
            'RelabelToolStripMenuItem
            '
            Me.RelabelToolStripMenuItem.Name = "RelabelToolStripMenuItem"
            Me.RelabelToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.RelabelToolStripMenuItem.Text = "Relabel"
            '
            'RelabelSelectedToolStripMenuItem
            '
            Me.RelabelSelectedToolStripMenuItem.Name = "RelabelSelectedToolStripMenuItem"
            Me.RelabelSelectedToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.RelabelSelectedToolStripMenuItem.Text = "Relabel Selected"
            '
            'CompareItemsToolStripMenuItem
            '
            Me.CompareItemsToolStripMenuItem.Name = "CompareItemsToolStripMenuItem"
            Me.CompareItemsToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.CompareItemsToolStripMenuItem.Text = "Compare Items"
            '
            'EditorToolStrip
            '
            Me.EditorToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddStimulusToolStripButton, Me.SelectByTypeToolStripButton, Me.RelabelToolStripButton, Me.RelabelSelectedToolStripButton, Me.ToolStripSeparator1, Me.AddPartToolStripButton, Me.AddJointToolStripButton, Me.CompareItemsToolStripButton})
            Me.EditorToolStrip.Location = New System.Drawing.Point(0, 0)
            Me.EditorToolStrip.Name = "EditorToolStrip"
            Me.EditorToolStrip.SecurityMgr = Nothing
            Me.EditorToolStrip.Size = New System.Drawing.Size(387, 25)
            Me.EditorToolStrip.TabIndex = 1
            Me.EditorToolStrip.Text = "EditorToolStrip"
            Me.EditorToolStrip.ToolName = ""
            Me.EditorToolStrip.Visible = False
            '
            'AddStimulusToolStripButton
            '
            Me.AddStimulusToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.AddStimulusToolStripButton.ImageTransparentColor = System.Drawing.Color.Cyan
            Me.AddStimulusToolStripButton.Name = "AddStimulusToolStripButton"
            Me.AddStimulusToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddStimulusToolStripButton.Text = "Add Stimulus"
            Me.AddStimulusToolStripButton.ToolTipText = "Add stimulus to the selected part"
            '
            'SelectByTypeToolStripButton
            '
            Me.SelectByTypeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.SelectByTypeToolStripButton.ImageTransparentColor = System.Drawing.Color.Cyan
            Me.SelectByTypeToolStripButton.Name = "SelectByTypeToolStripButton"
            Me.SelectByTypeToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.SelectByTypeToolStripButton.Text = "Select By Type"
            Me.SelectByTypeToolStripButton.ToolTipText = "Select by object type"
            '
            'RelabelToolStripButton
            '
            Me.RelabelToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.RelabelToolStripButton.ImageTransparentColor = System.Drawing.Color.Cyan
            Me.RelabelToolStripButton.Name = "RelabelToolStripButton"
            Me.RelabelToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.RelabelToolStripButton.Text = "Relabel"
            Me.RelabelToolStripButton.ToolTipText = "Relabel items using a regular expression"
            '
            'RelabelSelectedToolStripButton
            '
            Me.RelabelSelectedToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.RelabelSelectedToolStripButton.ImageTransparentColor = System.Drawing.Color.Cyan
            Me.RelabelSelectedToolStripButton.Name = "RelabelSelectedToolStripButton"
            Me.RelabelSelectedToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.RelabelSelectedToolStripButton.Text = "Relabel selected"
            Me.RelabelSelectedToolStripButton.ToolTipText = "Relabel and number selected items"
            '
            'ToolStripSeparator1
            '
            Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
            Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
            '
            'AddPartToolStripButton
            '
            Me.AddPartToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.AddPartToolStripButton.Image = CType(resources.GetObject("AddPartToolStripButton.Image"), System.Drawing.Image)
            Me.AddPartToolStripButton.ImageTransparentColor = System.Drawing.Color.Cyan
            Me.AddPartToolStripButton.Name = "AddPartToolStripButton"
            Me.AddPartToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddPartToolStripButton.Text = "Add Part"
            Me.AddPartToolStripButton.ToolTipText = "Adds a new body part to this structure"
            '
            'AddJointToolStripButton
            '
            Me.AddJointToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.AddJointToolStripButton.Image = CType(resources.GetObject("AddJointToolStripButton.Image"), System.Drawing.Image)
            Me.AddJointToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AddJointToolStripButton.Name = "AddJointToolStripButton"
            Me.AddJointToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddJointToolStripButton.Text = "Add Joint "
            Me.AddJointToolStripButton.ToolTipText = "Adds a new joint between two manually selected parts"
            '
            'CompareItemsToolStripButton
            '
            Me.CompareItemsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.CompareItemsToolStripButton.Image = CType(resources.GetObject("CompareItemsToolStripButton.Image"), System.Drawing.Image)
            Me.CompareItemsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CompareItemsToolStripButton.Name = "CompareItemsToolStripButton"
            Me.CompareItemsToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.CompareItemsToolStripButton.Text = "Compare Items "
            '
            'Editor_ToolStrips
            '
            Me.ClientSize = New System.Drawing.Size(387, 262)
            Me.Controls.Add(Me.EditorToolStrip)
            Me.Controls.Add(Me.EditorMenuStrip)
            Me.MainMenuStrip = Me.EditorMenuStrip
            Me.Name = "Editor_ToolStrips"
            Me.EditorMenuStrip.ResumeLayout(False)
            Me.EditorMenuStrip.PerformLayout()
            Me.EditorToolStrip.ResumeLayout(False)
            Me.EditorToolStrip.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents EditorToolStrip As AnimatGuiCtrls.Controls.AnimatToolStrip
        Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents PrintToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents PrintPreviewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents toolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents toolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents SelectByTypeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents toolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents AddStimulusToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents RelabelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents RelabelSelectedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents CompareItemsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddStimulusToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents SelectByTypeToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents RelabelToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents RelabelSelectedToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents EditorMenuStrip As AnimatGuiCtrls.Controls.AnimatMenuStrip
        Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents AddPartToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents AddJointToolStripButton As System.Windows.Forms.ToolStripButton

#End Region

        Private Sub AddPartToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddPartToolStripButton.Click

        End Sub


        Private Sub AddStimulusToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddStimulusToolStripButton.Click

        End Sub

        Private Sub SelectByTypeToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectByTypeToolStripButton.Click

        End Sub

        Private Sub RelabelToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelabelToolStripButton.Click

        End Sub

        Private Sub RelabelSelectedToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelabelSelectedToolStripButton.Click

        End Sub



        Private Sub RelabelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelabelToolStripMenuItem.Click

        End Sub

        Private Sub RelabelSelectedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelabelSelectedToolStripMenuItem.Click

        End Sub

        Private Sub CompareItemsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareItemsToolStripMenuItem.Click

        End Sub


        Private Sub SelectByTypeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectByTypeToolStripMenuItem.Click

        End Sub

        Private Sub AddStimulusToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddStimulusToolStripMenuItem.Click

        End Sub
        Friend WithEvents CompareItemsToolStripButton As System.Windows.Forms.ToolStripButton

        Private Sub CompareItemsToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareItemsToolStripButton.Click

        End Sub
    End Class

End Namespace

