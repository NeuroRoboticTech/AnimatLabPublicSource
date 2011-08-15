Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Text
imports System.Runtime.Remoting
Imports System.Threading
Imports System.Globalization
Imports AnimatGuiCtrls.Controls
Imports Crownwood.DotNetMagic.Controls
Imports Crownwood.DotNetMagic.Common
Imports Crownwood.DotNetMagic.Docking
Imports AnimatTools.Framework

Namespace Forms

    Public Class AnimatApplication_ToolStrips
        Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            Me.WindowState = FormWindowState.Maximized
            Me.IsMdiContainer = False
            Me.AllowDrop = True

            'This call is required by the Windows Form Designer.
            InitializeComponent()
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

        Friend WithEvents AnimatStripContainer As System.Windows.Forms.ToolStripContainer
        Friend WithEvents AnimatStatusBar As Crownwood.DotNetMagic.Controls.StatusBarControl
        Friend WithEvents AnimatMenuStrip As AnimatGuiCtrls.Controls.AnimatMenuStrip
        Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents toolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SaveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents toolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents UndoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents RedoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents toolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents CustomizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents PreferencesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ContentsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents IndexToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SearchToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents toolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AnimatToolStrip As AnimatGuiCtrls.Controls.AnimatToolStrip
        Friend WithEvents NewToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents OpenToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents SaveToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents HelpToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents AnimatTabbedGroups As Crownwood.DotNetMagic.Controls.TabbedGroups
        Friend WithEvents SupportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents RunSimulationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents CloseProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ExportStandaloneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents RunMacroToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents AddOrganismStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents AddStructureToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents AddToolToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents DeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DeleteToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents DisplayModeDropDown As System.Windows.Forms.ToolStripDropDownButton
        Friend WithEvents ShowGraphicsGeometryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowCollisionGeometryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowJointsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowCenterOfMassToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowPartOriginToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowContactsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SelGraphicsToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents SelCollisionToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents SelJointsToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents SelRecFieldsToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents SelectionModeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents GraphicsObjectsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents CollisionObjectsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents JointsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ReceptiveFieldsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SimulationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DisplayToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowGraphicsGeometryToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowCollisionGeomoetryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowJointsToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowCenterOfMassToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowPartOriginsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowContactsToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SelSimToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents SnapshotSimToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents AddStimulusToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents PasteToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents CopyToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents CutToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents toolStripSeparator As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents SelectByTypeToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents RelabelToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents RelabelSelectedToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents AddPartToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents AddJointToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents CompareItemsToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents SelectByTypeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents RelabelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents RelabelSelectedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents CompareItemsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddItemToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddStructureToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddOrganismToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddTooToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddBodyPartToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddBodyJointToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddStimulusToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents EditMaterialsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents EditMaterialsToolStripButton As System.Windows.Forms.ToolStripButton

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer


        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.

        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AnimatApplication_ToolStrips))
            Me.AnimatStripContainer = New System.Windows.Forms.ToolStripContainer()
            Me.AnimatTabbedGroups = New Crownwood.DotNetMagic.Controls.TabbedGroups()
            Me.AnimatStatusBar = New Crownwood.DotNetMagic.Controls.StatusBarControl()
            Me.AnimatMenuStrip = New AnimatGuiCtrls.Controls.AnimatMenuStrip()
            Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.toolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
            Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.toolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
            Me.CloseProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ExportStandaloneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SnapshotSimToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
            Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.RedoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.RunMacroToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.toolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
            Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.SelectByTypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.RelabelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.RelabelSelectedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.CompareItemsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.RunSimulationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
            Me.CustomizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.PreferencesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SelectionModeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.GraphicsObjectsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.CollisionObjectsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.JointsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ReceptiveFieldsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SimulationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.DisplayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowGraphicsGeometryToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowCollisionGeomoetryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowJointsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowCenterOfMassToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowPartOriginsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowContactsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddItemToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddStructureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddOrganismToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddTooToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddBodyPartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddBodyJointToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddStimulusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ContentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.IndexToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SearchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SupportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.toolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
            Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AnimatToolStrip = New AnimatGuiCtrls.Controls.AnimatToolStrip()
            Me.NewToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.OpenToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.SaveToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.PasteToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.CopyToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.CutToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.DeleteToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.toolStripSeparator = New System.Windows.Forms.ToolStripSeparator()
            Me.HelpToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
            Me.AddOrganismStripButton = New System.Windows.Forms.ToolStripButton()
            Me.AddStructureToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.AddToolToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.AddPartToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.AddJointToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.AddStimulusToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
            Me.SelectByTypeToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.RelabelToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.RelabelSelectedToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.CompareItemsToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
            Me.SelGraphicsToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.SelCollisionToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.SelJointsToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.SelRecFieldsToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.SelSimToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.DisplayModeDropDown = New System.Windows.Forms.ToolStripDropDownButton()
            Me.ShowGraphicsGeometryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowCollisionGeometryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowJointsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowCenterOfMassToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowPartOriginToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowContactsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.EditMaterialsToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.EditMaterialsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AnimatStripContainer.ContentPanel.SuspendLayout()
            Me.AnimatStripContainer.TopToolStripPanel.SuspendLayout()
            Me.AnimatStripContainer.SuspendLayout()
            CType(Me.AnimatTabbedGroups, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.AnimatMenuStrip.SuspendLayout()
            Me.AnimatToolStrip.SuspendLayout()
            Me.SuspendLayout()
            '
            'AnimatStripContainer
            '
            '
            'AnimatStripContainer.ContentPanel
            '
            Me.AnimatStripContainer.ContentPanel.Controls.Add(Me.AnimatTabbedGroups)
            Me.AnimatStripContainer.ContentPanel.Controls.Add(Me.AnimatStatusBar)
            Me.AnimatStripContainer.ContentPanel.Size = New System.Drawing.Size(713, 234)
            Me.AnimatStripContainer.Dock = System.Windows.Forms.DockStyle.Fill
            Me.AnimatStripContainer.Location = New System.Drawing.Point(0, 0)
            Me.AnimatStripContainer.Name = "AnimatStripContainer"
            Me.AnimatStripContainer.Size = New System.Drawing.Size(713, 283)
            Me.AnimatStripContainer.TabIndex = 0
            Me.AnimatStripContainer.Text = "ToolStripContainer1"
            '
            'AnimatStripContainer.TopToolStripPanel
            '
            Me.AnimatStripContainer.TopToolStripPanel.Controls.Add(Me.AnimatMenuStrip)
            Me.AnimatStripContainer.TopToolStripPanel.Controls.Add(Me.AnimatToolStrip)
            '
            'AnimatTabbedGroups
            '
            Me.AnimatTabbedGroups.AllowDrop = True
            Me.AnimatTabbedGroups.AtLeastOneLeaf = True
            Me.AnimatTabbedGroups.Dock = System.Windows.Forms.DockStyle.Fill
            Me.AnimatTabbedGroups.Location = New System.Drawing.Point(0, 0)
            Me.AnimatTabbedGroups.Name = "AnimatTabbedGroups"
            Me.AnimatTabbedGroups.ProminentLeaf = Nothing
            Me.AnimatTabbedGroups.ResizeBarColor = System.Drawing.SystemColors.Control
            Me.AnimatTabbedGroups.SaveControls = False
            Me.AnimatTabbedGroups.Size = New System.Drawing.Size(713, 215)
            Me.AnimatTabbedGroups.TabIndex = 1
            '
            'AnimatStatusBar
            '
            Me.AnimatStatusBar.Location = New System.Drawing.Point(0, 215)
            Me.AnimatStatusBar.Name = "AnimatStatusBar"
            Me.AnimatStatusBar.Size = New System.Drawing.Size(713, 19)
            Me.AnimatStatusBar.TabIndex = 0
            '
            'AnimatMenuStrip
            '
            Me.AnimatMenuStrip.Dock = System.Windows.Forms.DockStyle.None
            Me.AnimatMenuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
            Me.AnimatMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ViewToolStripMenuItem, Me.AddItemToolStripMenuItem, Me.HelpToolStripMenuItem})
            Me.AnimatMenuStrip.Location = New System.Drawing.Point(0, 0)
            Me.AnimatMenuStrip.Name = "AnimatMenuStrip"
            Me.AnimatMenuStrip.SecurityMgr = Nothing
            Me.AnimatMenuStrip.Size = New System.Drawing.Size(713, 24)
            Me.AnimatMenuStrip.TabIndex = 1
            Me.AnimatMenuStrip.Text = "MenuStrip1"
            Me.AnimatMenuStrip.ToolName = ""
            '
            'FileToolStripMenuItem
            '
            Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.OpenToolStripMenuItem, Me.toolStripSeparator2, Me.SaveToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.toolStripSeparator4, Me.CloseProjectToolStripMenuItem, Me.ExportStandaloneToolStripMenuItem, Me.SnapshotSimToolStripMenuItem, Me.ToolStripSeparator8, Me.ExitToolStripMenuItem})
            Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
            Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
            Me.FileToolStripMenuItem.Text = "&File"
            '
            'NewToolStripMenuItem
            '
            Me.NewToolStripMenuItem.Image = CType(resources.GetObject("NewToolStripMenuItem.Image"), System.Drawing.Image)
            Me.NewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
            Me.NewToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
            Me.NewToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
            Me.NewToolStripMenuItem.Text = "&New"
            '
            'OpenToolStripMenuItem
            '
            Me.OpenToolStripMenuItem.Image = CType(resources.GetObject("OpenToolStripMenuItem.Image"), System.Drawing.Image)
            Me.OpenToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
            Me.OpenToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
            Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
            Me.OpenToolStripMenuItem.Text = "&Open"
            '
            'toolStripSeparator2
            '
            Me.toolStripSeparator2.Name = "toolStripSeparator2"
            Me.toolStripSeparator2.Size = New System.Drawing.Size(189, 6)
            '
            'SaveToolStripMenuItem
            '
            Me.SaveToolStripMenuItem.Image = CType(resources.GetObject("SaveToolStripMenuItem.Image"), System.Drawing.Image)
            Me.SaveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
            Me.SaveToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
            Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
            Me.SaveToolStripMenuItem.Text = "&Save"
            '
            'SaveAsToolStripMenuItem
            '
            Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
            Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
            Me.SaveAsToolStripMenuItem.Text = "Save &As"
            '
            'toolStripSeparator4
            '
            Me.toolStripSeparator4.Name = "toolStripSeparator4"
            Me.toolStripSeparator4.Size = New System.Drawing.Size(189, 6)
            '
            'CloseProjectToolStripMenuItem
            '
            Me.CloseProjectToolStripMenuItem.Name = "CloseProjectToolStripMenuItem"
            Me.CloseProjectToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
            Me.CloseProjectToolStripMenuItem.Text = "Close Project"
            '
            'ExportStandaloneToolStripMenuItem
            '
            Me.ExportStandaloneToolStripMenuItem.Name = "ExportStandaloneToolStripMenuItem"
            Me.ExportStandaloneToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
            Me.ExportStandaloneToolStripMenuItem.Text = "Export Standalone Sim"
            '
            'SnapshotSimToolStripMenuItem
            '
            Me.SnapshotSimToolStripMenuItem.Name = "SnapshotSimToolStripMenuItem"
            Me.SnapshotSimToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
            Me.SnapshotSimToolStripMenuItem.Text = "Snapshot Sim"
            '
            'ToolStripSeparator8
            '
            Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
            Me.ToolStripSeparator8.Size = New System.Drawing.Size(189, 6)
            '
            'ExitToolStripMenuItem
            '
            Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
            Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
            Me.ExitToolStripMenuItem.Text = "E&xit"
            '
            'EditToolStripMenuItem
            '
            Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UndoToolStripMenuItem, Me.RedoToolStripMenuItem, Me.RunMacroToolStripMenuItem, Me.toolStripSeparator6, Me.CutToolStripMenuItem, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.DeleteToolStripMenuItem, Me.ToolStripSeparator1, Me.SelectByTypeToolStripMenuItem, Me.RelabelToolStripMenuItem, Me.RelabelSelectedToolStripMenuItem, Me.CompareItemsToolStripMenuItem, Me.EditMaterialsToolStripMenuItem})
            Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
            Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
            Me.EditToolStripMenuItem.Text = "&Edit"
            '
            'UndoToolStripMenuItem
            '
            Me.UndoToolStripMenuItem.Image = CType(resources.GetObject("UndoToolStripMenuItem.Image"), System.Drawing.Image)
            Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
            Me.UndoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
            Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.UndoToolStripMenuItem.Text = "&Undo"
            '
            'RedoToolStripMenuItem
            '
            Me.RedoToolStripMenuItem.Image = CType(resources.GetObject("RedoToolStripMenuItem.Image"), System.Drawing.Image)
            Me.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem"
            Me.RedoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
            Me.RedoToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.RedoToolStripMenuItem.Text = "&Redo"
            '
            'RunMacroToolStripMenuItem
            '
            Me.RunMacroToolStripMenuItem.Name = "RunMacroToolStripMenuItem"
            Me.RunMacroToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.RunMacroToolStripMenuItem.Text = "Run Macro"
            '
            'toolStripSeparator6
            '
            Me.toolStripSeparator6.Name = "toolStripSeparator6"
            Me.toolStripSeparator6.Size = New System.Drawing.Size(156, 6)
            '
            'CutToolStripMenuItem
            '
            Me.CutToolStripMenuItem.Image = CType(resources.GetObject("CutToolStripMenuItem.Image"), System.Drawing.Image)
            Me.CutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
            Me.CutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
            Me.CutToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.CutToolStripMenuItem.Text = "Cu&t"
            '
            'CopyToolStripMenuItem
            '
            Me.CopyToolStripMenuItem.Image = CType(resources.GetObject("CopyToolStripMenuItem.Image"), System.Drawing.Image)
            Me.CopyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
            Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
            Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.CopyToolStripMenuItem.Text = "&Copy"
            '
            'PasteToolStripMenuItem
            '
            Me.PasteToolStripMenuItem.Image = CType(resources.GetObject("PasteToolStripMenuItem.Image"), System.Drawing.Image)
            Me.PasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
            Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
            Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.PasteToolStripMenuItem.Text = "&Paste"
            '
            'DeleteToolStripMenuItem
            '
            Me.DeleteToolStripMenuItem.Image = CType(resources.GetObject("DeleteToolStripMenuItem.Image"), System.Drawing.Image)
            Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
            Me.DeleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
            Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.DeleteToolStripMenuItem.Text = "Delete"
            '
            'ToolStripSeparator1
            '
            Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
            Me.ToolStripSeparator1.Size = New System.Drawing.Size(156, 6)
            '
            'SelectByTypeToolStripMenuItem
            '
            Me.SelectByTypeToolStripMenuItem.Image = CType(resources.GetObject("SelectByTypeToolStripMenuItem.Image"), System.Drawing.Image)
            Me.SelectByTypeToolStripMenuItem.Name = "SelectByTypeToolStripMenuItem"
            Me.SelectByTypeToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.SelectByTypeToolStripMenuItem.Text = "Select By Type"
            Me.SelectByTypeToolStripMenuItem.ToolTipText = "Select items by their type"
            '
            'RelabelToolStripMenuItem
            '
            Me.RelabelToolStripMenuItem.Image = CType(resources.GetObject("RelabelToolStripMenuItem.Image"), System.Drawing.Image)
            Me.RelabelToolStripMenuItem.Name = "RelabelToolStripMenuItem"
            Me.RelabelToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.RelabelToolStripMenuItem.Text = "Relabel"
            Me.RelabelToolStripMenuItem.ToolTipText = "Relabel items using a regular expression"
            '
            'RelabelSelectedToolStripMenuItem
            '
            Me.RelabelSelectedToolStripMenuItem.Image = CType(resources.GetObject("RelabelSelectedToolStripMenuItem.Image"), System.Drawing.Image)
            Me.RelabelSelectedToolStripMenuItem.Name = "RelabelSelectedToolStripMenuItem"
            Me.RelabelSelectedToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.RelabelSelectedToolStripMenuItem.Text = "Relabel Selected"
            Me.RelabelSelectedToolStripMenuItem.ToolTipText = "Relabel selected items."
            '
            'CompareItemsToolStripMenuItem
            '
            Me.CompareItemsToolStripMenuItem.Image = CType(resources.GetObject("CompareItemsToolStripMenuItem.Image"), System.Drawing.Image)
            Me.CompareItemsToolStripMenuItem.Name = "CompareItemsToolStripMenuItem"
            Me.CompareItemsToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.CompareItemsToolStripMenuItem.Text = "Compare Items"
            Me.CompareItemsToolStripMenuItem.ToolTipText = "Compare selected items"
            '
            'ViewToolStripMenuItem
            '
            Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RunSimulationToolStripMenuItem, Me.ToolStripSeparator3, Me.CustomizeToolStripMenuItem, Me.PreferencesToolStripMenuItem, Me.SelectionModeToolStripMenuItem, Me.DisplayToolStripMenuItem})
            Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
            Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
            Me.ViewToolStripMenuItem.Text = "&View"
            '
            'RunSimulationToolStripMenuItem
            '
            Me.RunSimulationToolStripMenuItem.Image = CType(resources.GetObject("RunSimulationToolStripMenuItem.Image"), System.Drawing.Image)
            Me.RunSimulationToolStripMenuItem.Name = "RunSimulationToolStripMenuItem"
            Me.RunSimulationToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
            Me.RunSimulationToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
            Me.RunSimulationToolStripMenuItem.Text = "Run Simulation"
            '
            'ToolStripSeparator3
            '
            Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
            Me.ToolStripSeparator3.Size = New System.Drawing.Size(171, 6)
            '
            'CustomizeToolStripMenuItem
            '
            Me.CustomizeToolStripMenuItem.Name = "CustomizeToolStripMenuItem"
            Me.CustomizeToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
            Me.CustomizeToolStripMenuItem.Text = "&Customize"
            '
            'PreferencesToolStripMenuItem
            '
            Me.PreferencesToolStripMenuItem.Name = "PreferencesToolStripMenuItem"
            Me.PreferencesToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
            Me.PreferencesToolStripMenuItem.Text = "&Preferences"
            '
            'SelectionModeToolStripMenuItem
            '
            Me.SelectionModeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GraphicsObjectsToolStripMenuItem, Me.CollisionObjectsToolStripMenuItem, Me.JointsToolStripMenuItem, Me.ReceptiveFieldsToolStripMenuItem, Me.SimulationToolStripMenuItem})
            Me.SelectionModeToolStripMenuItem.Name = "SelectionModeToolStripMenuItem"
            Me.SelectionModeToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
            Me.SelectionModeToolStripMenuItem.Text = "Selection Mode"
            '
            'GraphicsObjectsToolStripMenuItem
            '
            Me.GraphicsObjectsToolStripMenuItem.Image = CType(resources.GetObject("GraphicsObjectsToolStripMenuItem.Image"), System.Drawing.Image)
            Me.GraphicsObjectsToolStripMenuItem.Name = "GraphicsObjectsToolStripMenuItem"
            Me.GraphicsObjectsToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
            Me.GraphicsObjectsToolStripMenuItem.Text = "Graphics Objects"
            '
            'CollisionObjectsToolStripMenuItem
            '
            Me.CollisionObjectsToolStripMenuItem.Image = CType(resources.GetObject("CollisionObjectsToolStripMenuItem.Image"), System.Drawing.Image)
            Me.CollisionObjectsToolStripMenuItem.Name = "CollisionObjectsToolStripMenuItem"
            Me.CollisionObjectsToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
            Me.CollisionObjectsToolStripMenuItem.Text = "Collision Objects"
            '
            'JointsToolStripMenuItem
            '
            Me.JointsToolStripMenuItem.Image = CType(resources.GetObject("JointsToolStripMenuItem.Image"), System.Drawing.Image)
            Me.JointsToolStripMenuItem.Name = "JointsToolStripMenuItem"
            Me.JointsToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
            Me.JointsToolStripMenuItem.Text = "Joints"
            '
            'ReceptiveFieldsToolStripMenuItem
            '
            Me.ReceptiveFieldsToolStripMenuItem.Image = CType(resources.GetObject("ReceptiveFieldsToolStripMenuItem.Image"), System.Drawing.Image)
            Me.ReceptiveFieldsToolStripMenuItem.Name = "ReceptiveFieldsToolStripMenuItem"
            Me.ReceptiveFieldsToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
            Me.ReceptiveFieldsToolStripMenuItem.Text = "Receptive Fields"
            '
            'SimulationToolStripMenuItem
            '
            Me.SimulationToolStripMenuItem.Image = CType(resources.GetObject("SimulationToolStripMenuItem.Image"), System.Drawing.Image)
            Me.SimulationToolStripMenuItem.Name = "SimulationToolStripMenuItem"
            Me.SimulationToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
            Me.SimulationToolStripMenuItem.Text = "Simulation"
            '
            'DisplayToolStripMenuItem
            '
            Me.DisplayToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowGraphicsGeometryToolStripMenuItem1, Me.ShowCollisionGeomoetryToolStripMenuItem, Me.ShowJointsToolStripMenuItem1, Me.ShowCenterOfMassToolStripMenuItem1, Me.ShowPartOriginsToolStripMenuItem, Me.ShowContactsToolStripMenuItem1})
            Me.DisplayToolStripMenuItem.Name = "DisplayToolStripMenuItem"
            Me.DisplayToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
            Me.DisplayToolStripMenuItem.Text = "Display"
            '
            'ShowGraphicsGeometryToolStripMenuItem1
            '
            Me.ShowGraphicsGeometryToolStripMenuItem1.Name = "ShowGraphicsGeometryToolStripMenuItem1"
            Me.ShowGraphicsGeometryToolStripMenuItem1.Size = New System.Drawing.Size(214, 22)
            Me.ShowGraphicsGeometryToolStripMenuItem1.Text = "Show Graphics Geometry"
            '
            'ShowCollisionGeomoetryToolStripMenuItem
            '
            Me.ShowCollisionGeomoetryToolStripMenuItem.Name = "ShowCollisionGeomoetryToolStripMenuItem"
            Me.ShowCollisionGeomoetryToolStripMenuItem.Size = New System.Drawing.Size(214, 22)
            Me.ShowCollisionGeomoetryToolStripMenuItem.Text = "Show Collision Geomoetry"
            '
            'ShowJointsToolStripMenuItem1
            '
            Me.ShowJointsToolStripMenuItem1.Name = "ShowJointsToolStripMenuItem1"
            Me.ShowJointsToolStripMenuItem1.Size = New System.Drawing.Size(214, 22)
            Me.ShowJointsToolStripMenuItem1.Text = "Show Joints"
            '
            'ShowCenterOfMassToolStripMenuItem1
            '
            Me.ShowCenterOfMassToolStripMenuItem1.Name = "ShowCenterOfMassToolStripMenuItem1"
            Me.ShowCenterOfMassToolStripMenuItem1.Size = New System.Drawing.Size(214, 22)
            Me.ShowCenterOfMassToolStripMenuItem1.Text = "Show Center of Mass"
            '
            'ShowPartOriginsToolStripMenuItem
            '
            Me.ShowPartOriginsToolStripMenuItem.Name = "ShowPartOriginsToolStripMenuItem"
            Me.ShowPartOriginsToolStripMenuItem.Size = New System.Drawing.Size(214, 22)
            Me.ShowPartOriginsToolStripMenuItem.Text = "Show Part Origins"
            '
            'ShowContactsToolStripMenuItem1
            '
            Me.ShowContactsToolStripMenuItem1.Name = "ShowContactsToolStripMenuItem1"
            Me.ShowContactsToolStripMenuItem1.Size = New System.Drawing.Size(214, 22)
            Me.ShowContactsToolStripMenuItem1.Text = "Show Contacts"
            '
            'AddItemToolStripMenuItem
            '
            Me.AddItemToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddStructureToolStripMenuItem, Me.AddOrganismToolStripMenuItem, Me.AddTooToolStripMenuItem, Me.AddBodyPartToolStripMenuItem, Me.AddBodyJointToolStripMenuItem, Me.AddStimulusToolStripMenuItem})
            Me.AddItemToolStripMenuItem.Name = "AddItemToolStripMenuItem"
            Me.AddItemToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
            Me.AddItemToolStripMenuItem.Text = "Add"
            '
            'AddStructureToolStripMenuItem
            '
            Me.AddStructureToolStripMenuItem.Image = CType(resources.GetObject("AddStructureToolStripMenuItem.Image"), System.Drawing.Image)
            Me.AddStructureToolStripMenuItem.Name = "AddStructureToolStripMenuItem"
            Me.AddStructureToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
            Me.AddStructureToolStripMenuItem.Text = "Add Structure"
            '
            'AddOrganismToolStripMenuItem
            '
            Me.AddOrganismToolStripMenuItem.Image = CType(resources.GetObject("AddOrganismToolStripMenuItem.Image"), System.Drawing.Image)
            Me.AddOrganismToolStripMenuItem.Name = "AddOrganismToolStripMenuItem"
            Me.AddOrganismToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
            Me.AddOrganismToolStripMenuItem.Text = "Add Organism"
            '
            'AddTooToolStripMenuItem
            '
            Me.AddTooToolStripMenuItem.Image = CType(resources.GetObject("AddTooToolStripMenuItem.Image"), System.Drawing.Image)
            Me.AddTooToolStripMenuItem.Name = "AddTooToolStripMenuItem"
            Me.AddTooToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
            Me.AddTooToolStripMenuItem.Text = "Add Tool"
            '
            'AddBodyPartToolStripMenuItem
            '
            Me.AddBodyPartToolStripMenuItem.Image = CType(resources.GetObject("AddBodyPartToolStripMenuItem.Image"), System.Drawing.Image)
            Me.AddBodyPartToolStripMenuItem.Name = "AddBodyPartToolStripMenuItem"
            Me.AddBodyPartToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
            Me.AddBodyPartToolStripMenuItem.Text = "Add Body Part"
            '
            'AddBodyJointToolStripMenuItem
            '
            Me.AddBodyJointToolStripMenuItem.Enabled = False
            Me.AddBodyJointToolStripMenuItem.Image = CType(resources.GetObject("AddBodyJointToolStripMenuItem.Image"), System.Drawing.Image)
            Me.AddBodyJointToolStripMenuItem.Name = "AddBodyJointToolStripMenuItem"
            Me.AddBodyJointToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
            Me.AddBodyJointToolStripMenuItem.Text = "Add Body Joint"
            '
            'AddStimulusToolStripMenuItem
            '
            Me.AddStimulusToolStripMenuItem.Image = CType(resources.GetObject("AddStimulusToolStripMenuItem.Image"), System.Drawing.Image)
            Me.AddStimulusToolStripMenuItem.Name = "AddStimulusToolStripMenuItem"
            Me.AddStimulusToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
            Me.AddStimulusToolStripMenuItem.Text = "Add Stimulus"
            '
            'HelpToolStripMenuItem
            '
            Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ContentsToolStripMenuItem, Me.IndexToolStripMenuItem, Me.SearchToolStripMenuItem, Me.SupportToolStripMenuItem, Me.toolStripSeparator7, Me.AboutToolStripMenuItem})
            Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
            Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
            Me.HelpToolStripMenuItem.Text = "&Help"
            '
            'ContentsToolStripMenuItem
            '
            Me.ContentsToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.HelpContents
            Me.ContentsToolStripMenuItem.Name = "ContentsToolStripMenuItem"
            Me.ContentsToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
            Me.ContentsToolStripMenuItem.Text = "&Contents"
            '
            'IndexToolStripMenuItem
            '
            Me.IndexToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.HelpIndex
            Me.IndexToolStripMenuItem.Name = "IndexToolStripMenuItem"
            Me.IndexToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
            Me.IndexToolStripMenuItem.Text = "&Index"
            '
            'SearchToolStripMenuItem
            '
            Me.SearchToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.HelpSearch
            Me.SearchToolStripMenuItem.Name = "SearchToolStripMenuItem"
            Me.SearchToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
            Me.SearchToolStripMenuItem.Text = "&Search"
            '
            'SupportToolStripMenuItem
            '
            Me.SupportToolStripMenuItem.Name = "SupportToolStripMenuItem"
            Me.SupportToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
            Me.SupportToolStripMenuItem.Text = "Support"
            '
            'toolStripSeparator7
            '
            Me.toolStripSeparator7.Name = "toolStripSeparator7"
            Me.toolStripSeparator7.Size = New System.Drawing.Size(119, 6)
            '
            'AboutToolStripMenuItem
            '
            Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
            Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
            Me.AboutToolStripMenuItem.Text = "&About..."
            '
            'AnimatToolStrip
            '
            Me.AnimatToolStrip.Dock = System.Windows.Forms.DockStyle.None
            Me.AnimatToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.OpenToolStripButton, Me.SaveToolStripButton, Me.PasteToolStripButton, Me.CopyToolStripButton, Me.CutToolStripButton, Me.DeleteToolStripButton, Me.toolStripSeparator, Me.HelpToolStripButton, Me.ToolStripSeparator5, Me.AddOrganismStripButton, Me.AddStructureToolStripButton, Me.AddToolToolStripButton, Me.AddPartToolStripButton, Me.AddJointToolStripButton, Me.AddStimulusToolStripButton, Me.ToolStripSeparator9, Me.SelectByTypeToolStripButton, Me.RelabelToolStripButton, Me.RelabelSelectedToolStripButton, Me.CompareItemsToolStripButton, Me.EditMaterialsToolStripButton, Me.ToolStripSeparator10, Me.SelGraphicsToolStripButton, Me.SelCollisionToolStripButton, Me.SelJointsToolStripButton, Me.SelRecFieldsToolStripButton, Me.SelSimToolStripButton, Me.DisplayModeDropDown})
            Me.AnimatToolStrip.Location = New System.Drawing.Point(3, 24)
            Me.AnimatToolStrip.Name = "AnimatToolStrip"
            Me.AnimatToolStrip.SecurityMgr = Nothing
            Me.AnimatToolStrip.Size = New System.Drawing.Size(649, 25)
            Me.AnimatToolStrip.TabIndex = 2
            Me.AnimatToolStrip.ToolName = ""
            '
            'NewToolStripButton
            '
            Me.NewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.NewToolStripButton.Image = CType(resources.GetObject("NewToolStripButton.Image"), System.Drawing.Image)
            Me.NewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.NewToolStripButton.Name = "NewToolStripButton"
            Me.NewToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.NewToolStripButton.Text = "&New"
            '
            'OpenToolStripButton
            '
            Me.OpenToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.OpenToolStripButton.Image = CType(resources.GetObject("OpenToolStripButton.Image"), System.Drawing.Image)
            Me.OpenToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.OpenToolStripButton.Name = "OpenToolStripButton"
            Me.OpenToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.OpenToolStripButton.Text = "&Open"
            '
            'SaveToolStripButton
            '
            Me.SaveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.SaveToolStripButton.Image = CType(resources.GetObject("SaveToolStripButton.Image"), System.Drawing.Image)
            Me.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.SaveToolStripButton.Name = "SaveToolStripButton"
            Me.SaveToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.SaveToolStripButton.Text = "&Save"
            '
            'PasteToolStripButton
            '
            Me.PasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.PasteToolStripButton.Image = CType(resources.GetObject("PasteToolStripButton.Image"), System.Drawing.Image)
            Me.PasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PasteToolStripButton.Name = "PasteToolStripButton"
            Me.PasteToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.PasteToolStripButton.Text = "&Paste"
            '
            'CopyToolStripButton
            '
            Me.CopyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.CopyToolStripButton.Image = CType(resources.GetObject("CopyToolStripButton.Image"), System.Drawing.Image)
            Me.CopyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CopyToolStripButton.Name = "CopyToolStripButton"
            Me.CopyToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.CopyToolStripButton.Text = "&Copy"
            '
            'CutToolStripButton
            '
            Me.CutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.CutToolStripButton.Image = CType(resources.GetObject("CutToolStripButton.Image"), System.Drawing.Image)
            Me.CutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CutToolStripButton.Name = "CutToolStripButton"
            Me.CutToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.CutToolStripButton.Text = "C&ut"
            '
            'DeleteToolStripButton
            '
            Me.DeleteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.DeleteToolStripButton.Image = CType(resources.GetObject("DeleteToolStripButton.Image"), System.Drawing.Image)
            Me.DeleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.DeleteToolStripButton.Name = "DeleteToolStripButton"
            Me.DeleteToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.DeleteToolStripButton.Text = "Delete"
            Me.DeleteToolStripButton.ToolTipText = "Delete selected workspace item"
            '
            'toolStripSeparator
            '
            Me.toolStripSeparator.Name = "toolStripSeparator"
            Me.toolStripSeparator.Size = New System.Drawing.Size(6, 25)
            '
            'HelpToolStripButton
            '
            Me.HelpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.HelpToolStripButton.Image = CType(resources.GetObject("HelpToolStripButton.Image"), System.Drawing.Image)
            Me.HelpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.HelpToolStripButton.Name = "HelpToolStripButton"
            Me.HelpToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.HelpToolStripButton.Text = "He&lp"
            '
            'ToolStripSeparator5
            '
            Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
            Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 25)
            '
            'AddOrganismStripButton
            '
            Me.AddOrganismStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.AddOrganismStripButton.Image = CType(resources.GetObject("AddOrganismStripButton.Image"), System.Drawing.Image)
            Me.AddOrganismStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AddOrganismStripButton.Name = "AddOrganismStripButton"
            Me.AddOrganismStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddOrganismStripButton.Text = "Add Organism"
            '
            'AddStructureToolStripButton
            '
            Me.AddStructureToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.AddStructureToolStripButton.Image = CType(resources.GetObject("AddStructureToolStripButton.Image"), System.Drawing.Image)
            Me.AddStructureToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AddStructureToolStripButton.Name = "AddStructureToolStripButton"
            Me.AddStructureToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddStructureToolStripButton.Text = "Add Structure"
            '
            'AddToolToolStripButton
            '
            Me.AddToolToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.AddToolToolStripButton.Image = CType(resources.GetObject("AddToolToolStripButton.Image"), System.Drawing.Image)
            Me.AddToolToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AddToolToolStripButton.Name = "AddToolToolStripButton"
            Me.AddToolToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddToolToolStripButton.Text = "Add Data Tool"
            '
            'AddPartToolStripButton
            '
            Me.AddPartToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.AddPartToolStripButton.Image = CType(resources.GetObject("AddPartToolStripButton.Image"), System.Drawing.Image)
            Me.AddPartToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AddPartToolStripButton.Name = "AddPartToolStripButton"
            Me.AddPartToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddPartToolStripButton.Text = "AddPartToolStripButton"
            Me.AddPartToolStripButton.ToolTipText = "Starts 'Add Body' mode so you can click on a parent to add a child part."
            '
            'AddJointToolStripButton
            '
            Me.AddJointToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.AddJointToolStripButton.Enabled = False
            Me.AddJointToolStripButton.Image = CType(resources.GetObject("AddJointToolStripButton.Image"), System.Drawing.Image)
            Me.AddJointToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AddJointToolStripButton.Name = "AddJointToolStripButton"
            Me.AddJointToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddJointToolStripButton.Text = "AddJointToolStripButton"
            Me.AddJointToolStripButton.ToolTipText = "Adds a new joint between two manually selected parts."
            '
            'AddStimulusToolStripButton
            '
            Me.AddStimulusToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.AddStimulusToolStripButton.Image = CType(resources.GetObject("AddStimulusToolStripButton.Image"), System.Drawing.Image)
            Me.AddStimulusToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AddStimulusToolStripButton.Name = "AddStimulusToolStripButton"
            Me.AddStimulusToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddStimulusToolStripButton.Text = "AddStimulusToolStripButton"
            Me.AddStimulusToolStripButton.ToolTipText = "Add a stimulus to selected parts."
            '
            'ToolStripSeparator9
            '
            Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
            Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 25)
            '
            'SelectByTypeToolStripButton
            '
            Me.SelectByTypeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.SelectByTypeToolStripButton.Image = CType(resources.GetObject("SelectByTypeToolStripButton.Image"), System.Drawing.Image)
            Me.SelectByTypeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.SelectByTypeToolStripButton.Name = "SelectByTypeToolStripButton"
            Me.SelectByTypeToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.SelectByTypeToolStripButton.Text = "SelectByTypeToolStripButton"
            Me.SelectByTypeToolStripButton.ToolTipText = "Select items by type"
            '
            'RelabelToolStripButton
            '
            Me.RelabelToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.RelabelToolStripButton.Image = CType(resources.GetObject("RelabelToolStripButton.Image"), System.Drawing.Image)
            Me.RelabelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.RelabelToolStripButton.Name = "RelabelToolStripButton"
            Me.RelabelToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.RelabelToolStripButton.Text = "RelabelToolStripButton"
            Me.RelabelToolStripButton.ToolTipText = "Relabel Items."
            '
            'RelabelSelectedToolStripButton
            '
            Me.RelabelSelectedToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.RelabelSelectedToolStripButton.Image = CType(resources.GetObject("RelabelSelectedToolStripButton.Image"), System.Drawing.Image)
            Me.RelabelSelectedToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.RelabelSelectedToolStripButton.Name = "RelabelSelectedToolStripButton"
            Me.RelabelSelectedToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.RelabelSelectedToolStripButton.Text = "RelabelSelectedToolStripButton"
            Me.RelabelSelectedToolStripButton.ToolTipText = "Relabel selected items."
            '
            'CompareItemsToolStripButton
            '
            Me.CompareItemsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.CompareItemsToolStripButton.Image = CType(resources.GetObject("CompareItemsToolStripButton.Image"), System.Drawing.Image)
            Me.CompareItemsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CompareItemsToolStripButton.Name = "CompareItemsToolStripButton"
            Me.CompareItemsToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.CompareItemsToolStripButton.Text = "CompareItemsToolStripButton"
            Me.CompareItemsToolStripButton.ToolTipText = "Compares the properties of selected items that are the same type."
            '
            'ToolStripSeparator10
            '
            Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
            Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 25)
            '
            'SelGraphicsToolStripButton
            '
            Me.SelGraphicsToolStripButton.CheckOnClick = True
            Me.SelGraphicsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.SelGraphicsToolStripButton.Image = CType(resources.GetObject("SelGraphicsToolStripButton.Image"), System.Drawing.Image)
            Me.SelGraphicsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.SelGraphicsToolStripButton.Name = "SelGraphicsToolStripButton"
            Me.SelGraphicsToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.SelGraphicsToolStripButton.Text = "SelectGraphics"
            Me.SelGraphicsToolStripButton.ToolTipText = "Select graphics objects only"
            '
            'SelCollisionToolStripButton
            '
            Me.SelCollisionToolStripButton.CheckOnClick = True
            Me.SelCollisionToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.SelCollisionToolStripButton.Image = CType(resources.GetObject("SelCollisionToolStripButton.Image"), System.Drawing.Image)
            Me.SelCollisionToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.SelCollisionToolStripButton.Name = "SelCollisionToolStripButton"
            Me.SelCollisionToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.SelCollisionToolStripButton.Text = "Select Collision Objects"
            Me.SelCollisionToolStripButton.ToolTipText = "Select collision objects only."
            '
            'SelJointsToolStripButton
            '
            Me.SelJointsToolStripButton.CheckOnClick = True
            Me.SelJointsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.SelJointsToolStripButton.Image = CType(resources.GetObject("SelJointsToolStripButton.Image"), System.Drawing.Image)
            Me.SelJointsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.SelJointsToolStripButton.Name = "SelJointsToolStripButton"
            Me.SelJointsToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.SelJointsToolStripButton.Text = "Select Joints"
            Me.SelJointsToolStripButton.ToolTipText = "Select joints only"
            '
            'SelRecFieldsToolStripButton
            '
            Me.SelRecFieldsToolStripButton.CheckOnClick = True
            Me.SelRecFieldsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.SelRecFieldsToolStripButton.Image = CType(resources.GetObject("SelRecFieldsToolStripButton.Image"), System.Drawing.Image)
            Me.SelRecFieldsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.SelRecFieldsToolStripButton.Name = "SelRecFieldsToolStripButton"
            Me.SelRecFieldsToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.SelRecFieldsToolStripButton.Text = "Select Receptive Fields"
            Me.SelRecFieldsToolStripButton.ToolTipText = "Select receptive fields only"
            '
            'SelSimToolStripButton
            '
            Me.SelSimToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.SelSimToolStripButton.Image = CType(resources.GetObject("SelSimToolStripButton.Image"), System.Drawing.Image)
            Me.SelSimToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.SelSimToolStripButton.Name = "SelSimToolStripButton"
            Me.SelSimToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.SelSimToolStripButton.Text = "Selection Simulation mode"
            Me.SelSimToolStripButton.ToolTipText = "Simulation select mode. This allows you to pick and move objects in the simulatio" & _
                "n using the mouse."
            '
            'DisplayModeDropDown
            '
            Me.DisplayModeDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.DisplayModeDropDown.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowGraphicsGeometryToolStripMenuItem, Me.ShowCollisionGeometryToolStripMenuItem, Me.ShowJointsToolStripMenuItem, Me.ShowCenterOfMassToolStripMenuItem, Me.ShowPartOriginToolStripMenuItem, Me.ShowContactsToolStripMenuItem})
            Me.DisplayModeDropDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
            Me.DisplayModeDropDown.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.DisplayModeDropDown.Name = "DisplayModeDropDown"
            Me.DisplayModeDropDown.Size = New System.Drawing.Size(61, 22)
            Me.DisplayModeDropDown.Text = "Display "
            Me.DisplayModeDropDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
            Me.DisplayModeDropDown.ToolTipText = "Display Options"
            '
            'ShowGraphicsGeometryToolStripMenuItem
            '
            Me.ShowGraphicsGeometryToolStripMenuItem.CheckOnClick = True
            Me.ShowGraphicsGeometryToolStripMenuItem.Name = "ShowGraphicsGeometryToolStripMenuItem"
            Me.ShowGraphicsGeometryToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
            Me.ShowGraphicsGeometryToolStripMenuItem.Text = "Show Graphics Geometry"
            '
            'ShowCollisionGeometryToolStripMenuItem
            '
            Me.ShowCollisionGeometryToolStripMenuItem.CheckOnClick = True
            Me.ShowCollisionGeometryToolStripMenuItem.Name = "ShowCollisionGeometryToolStripMenuItem"
            Me.ShowCollisionGeometryToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
            Me.ShowCollisionGeometryToolStripMenuItem.Text = "Show Collision Geometry"
            '
            'ShowJointsToolStripMenuItem
            '
            Me.ShowJointsToolStripMenuItem.CheckOnClick = True
            Me.ShowJointsToolStripMenuItem.Name = "ShowJointsToolStripMenuItem"
            Me.ShowJointsToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
            Me.ShowJointsToolStripMenuItem.Text = "Show Joints"
            '
            'ShowCenterOfMassToolStripMenuItem
            '
            Me.ShowCenterOfMassToolStripMenuItem.CheckOnClick = True
            Me.ShowCenterOfMassToolStripMenuItem.Name = "ShowCenterOfMassToolStripMenuItem"
            Me.ShowCenterOfMassToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
            Me.ShowCenterOfMassToolStripMenuItem.Text = "Show Center of Mass"
            '
            'ShowPartOriginToolStripMenuItem
            '
            Me.ShowPartOriginToolStripMenuItem.CheckOnClick = True
            Me.ShowPartOriginToolStripMenuItem.Name = "ShowPartOriginToolStripMenuItem"
            Me.ShowPartOriginToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
            Me.ShowPartOriginToolStripMenuItem.Text = "Show Part Origin"
            '
            'ShowContactsToolStripMenuItem
            '
            Me.ShowContactsToolStripMenuItem.CheckOnClick = True
            Me.ShowContactsToolStripMenuItem.Name = "ShowContactsToolStripMenuItem"
            Me.ShowContactsToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
            Me.ShowContactsToolStripMenuItem.Text = "Show Contacts"
            '
            'EditMaterialsToolStripButton
            '
            Me.EditMaterialsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.EditMaterialsToolStripButton.Image = CType(resources.GetObject("EditMaterialsToolStripButton.Image"), System.Drawing.Image)
            Me.EditMaterialsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.EditMaterialsToolStripButton.Name = "EditMaterialsToolStripButton"
            Me.EditMaterialsToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.EditMaterialsToolStripButton.Text = "EditMaterialsToolStripButton"
            Me.EditMaterialsToolStripButton.ToolTipText = "Edit Materials"
            '
            'EditMaterialsToolStripMenuItem
            '
            Me.EditMaterialsToolStripMenuItem.Name = "EditMaterialsToolStripMenuItem"
            Me.EditMaterialsToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.EditMaterialsToolStripMenuItem.Text = "Edit Materials"
            '
            'AnimatApplication_ToolStrips
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(713, 283)
            Me.Controls.Add(Me.AnimatStripContainer)
            Me.MainMenuStrip = Me.AnimatMenuStrip
            Me.Name = "AnimatApplication_ToolStrips"
            Me.Text = "AnimatLab"
            Me.AnimatStripContainer.ContentPanel.ResumeLayout(False)
            Me.AnimatStripContainer.TopToolStripPanel.ResumeLayout(False)
            Me.AnimatStripContainer.TopToolStripPanel.PerformLayout()
            Me.AnimatStripContainer.ResumeLayout(False)
            Me.AnimatStripContainer.PerformLayout()
            CType(Me.AnimatTabbedGroups, System.ComponentModel.ISupportInitialize).EndInit()
            Me.AnimatMenuStrip.ResumeLayout(False)
            Me.AnimatMenuStrip.PerformLayout()
            Me.AnimatToolStrip.ResumeLayout(False)
            Me.AnimatToolStrip.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Protected m_mgrToolStripImages As New AnimatGUI.Framework.ImageManager

        Public Overridable ReadOnly Property ToolStripImages() As AnimatGUI.Framework.ImageManager
            Get
                Return m_mgrToolStripImages
            End Get
        End Property

        Private Sub SelGraphicsToolStripButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelGraphicsToolStripButton.CheckedChanged, GraphicsObjectsToolStripMenuItem.CheckedChanged

        End Sub

        Private Sub SelCollisionToolStripButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelCollisionToolStripButton.CheckedChanged, CollisionObjectsToolStripMenuItem.CheckedChanged

        End Sub

        Private Sub SelJointsToolStripButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelJointsToolStripButton.CheckedChanged, JointsToolStripMenuItem.CheckedChanged

        End Sub

        Private Sub SelRecFieldsToolStripButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelRecFieldsToolStripButton.CheckedChanged, ReceptiveFieldsToolStripMenuItem.CheckedChanged

        End Sub

        Private Sub SelSimToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelSimToolStripButton.Click, SimulationToolStripMenuItem.CheckedChanged

        End Sub

        Private Sub ShowGraphicsGeometryToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowGraphicsGeometryToolStripMenuItem.CheckedChanged, ShowGraphicsGeometryToolStripMenuItem1.CheckedChanged

        End Sub

        Private Sub ShowCollisionGeometryToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowCollisionGeometryToolStripMenuItem.CheckedChanged, ShowCollisionGeomoetryToolStripMenuItem.CheckedChanged

        End Sub

        Private Sub ShowJointsToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowJointsToolStripMenuItem.CheckedChanged, ShowJointsToolStripMenuItem1.CheckedChanged

        End Sub

        Private Sub ShowCenterOfMassToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowCenterOfMassToolStripMenuItem.CheckedChanged, ShowCenterOfMassToolStripMenuItem1.CheckedChanged

        End Sub

        Private Sub ShowPartOriginToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowPartOriginToolStripMenuItem.CheckedChanged, ShowPartOriginsToolStripMenuItem.CheckedChanged

        End Sub

        Private Sub ShowContactsToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowContactsToolStripMenuItem.CheckedChanged, ShowContactsToolStripMenuItem1.CheckedChanged

        End Sub

        Private Sub SnapshotSimToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SnapshotSimToolStripMenuItem.Click

        End Sub

        Private Sub AnimatToolStrip_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles AnimatToolStrip.ItemClicked

        End Sub

        Private Sub AddStimulusToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddStimulusToolStripButton.Click

        End Sub

        Private Sub PasteToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripButton.Click

        End Sub

        Private Sub CopyToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripButton.Click

        End Sub

        Private Sub CutToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripButton.Click

        End Sub

        Private Sub DeleteToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripButton.Click

        End Sub

        Private Sub HelpToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpToolStripButton.Click

        End Sub

        Private Sub AddPartToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddPartToolStripButton.Click

        End Sub

        Private Sub AddJointToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddJointToolStripButton.Click

        End Sub

        Private Sub SelectByTypeToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectByTypeToolStripButton.Click

        End Sub

        Private Sub RelabelToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelabelToolStripButton.Click

        End Sub

        Private Sub RelabelSelectedToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelabelSelectedToolStripButton.Click

        End Sub

        Private Sub CompareItemsToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareItemsToolStripButton.Click

        End Sub

        Private Sub EditMaterialsToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditMaterialsToolStripButton.Click

        End Sub

        Private Sub EditMaterialsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditMaterialsToolStripMenuItem.Click

        End Sub
    End Class

End Namespace

