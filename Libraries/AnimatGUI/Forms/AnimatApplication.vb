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
Imports AnimatGUI.Framework

Namespace Forms

    Public Class AnimatApplication
        Inherits AnimatForm

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            ShowSplashScreen()

            Me.WindowState = FormWindowState.Maximized
            Me.IsMdiContainer = True
            Me.AllowDrop = True

            'This call is required by the Windows Form Designer.
            InitializeComponent()
            Initialize(Nothing)
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
        Friend WithEvents toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents HelpToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents AnimatTabbedGroups As Crownwood.DotNetMagic.Controls.TabbedGroups
        Friend WithEvents SupportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents RunSimulationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents AddOrganismToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddStructureToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddTooToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
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
        Friend WithEvents ToolStripDropDownButton1 As System.Windows.Forms.ToolStripDropDownButton
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

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer


        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.

        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AnimatApplication))
            Me.AnimatStripContainer = New System.Windows.Forms.ToolStripContainer
            Me.AnimatTabbedGroups = New Crownwood.DotNetMagic.Controls.TabbedGroups
            Me.AnimatStatusBar = New Crownwood.DotNetMagic.Controls.StatusBarControl
            Me.AnimatMenuStrip = New AnimatGuiCtrls.Controls.AnimatMenuStrip
            Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.toolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
            Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.toolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
            Me.CloseProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ExportStandaloneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator
            Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.RedoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.RunMacroToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.toolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
            Me.AddOrganismToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.AddStructureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.AddTooToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.RunSimulationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
            Me.CustomizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.PreferencesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ContentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.IndexToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.SearchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.SupportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.toolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
            Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.AnimatToolStrip = New AnimatGuiCtrls.Controls.AnimatToolStrip
            Me.NewToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.OpenToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.SaveToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
            Me.HelpToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
            Me.AddOrganismStripButton = New System.Windows.Forms.ToolStripButton
            Me.AddStructureToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.AddToolToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.DeleteToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator
            Me.SelGraphicsToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.SelCollisionToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.SelJointsToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.SelRecFieldsToolStripButton = New System.Windows.Forms.ToolStripButton
            Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton
            Me.ShowGraphicsGeometryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowCollisionGeometryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowJointsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowCenterOfMassToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowPartOriginToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowContactsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.SelectionModeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.GraphicsObjectsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.CollisionObjectsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.JointsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ReceptiveFieldsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.SimulationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.DisplayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowGraphicsGeometryToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowCollisionGeomoetryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowJointsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowCenterOfMassToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowPartOriginsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowContactsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
            Me.SnapshotSimToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.SelSimToolStripButton = New System.Windows.Forms.ToolStripButton
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
            Me.AnimatStripContainer.ContentPanel.Size = New System.Drawing.Size(586, 234)
            Me.AnimatStripContainer.Dock = System.Windows.Forms.DockStyle.Fill
            Me.AnimatStripContainer.Location = New System.Drawing.Point(0, 0)
            Me.AnimatStripContainer.Name = "AnimatStripContainer"
            Me.AnimatStripContainer.Size = New System.Drawing.Size(586, 283)
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
            Me.AnimatTabbedGroups.Size = New System.Drawing.Size(586, 215)
            Me.AnimatTabbedGroups.TabIndex = 1
            '
            'AnimatStatusBar
            '
            Me.AnimatStatusBar.Location = New System.Drawing.Point(0, 215)
            Me.AnimatStatusBar.Name = "AnimatStatusBar"
            Me.AnimatStatusBar.Size = New System.Drawing.Size(586, 19)
            Me.AnimatStatusBar.TabIndex = 0
            '
            'AnimatMenuStrip
            '
            Me.AnimatMenuStrip.Dock = System.Windows.Forms.DockStyle.None
            Me.AnimatMenuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
            Me.AnimatMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ViewToolStripMenuItem, Me.HelpToolStripMenuItem})
            Me.AnimatMenuStrip.Location = New System.Drawing.Point(0, 0)
            Me.AnimatMenuStrip.Name = "AnimatMenuStrip"
            Me.AnimatMenuStrip.SecurityMgr = Nothing
            Me.AnimatMenuStrip.Size = New System.Drawing.Size(586, 24)
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
            Me.ExportStandaloneToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.ExportStandalone
            Me.ExportStandaloneToolStripMenuItem.Name = "ExportStandaloneToolStripMenuItem"
            Me.ExportStandaloneToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
            Me.ExportStandaloneToolStripMenuItem.Text = "Export Standalone Sim"
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
            Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UndoToolStripMenuItem, Me.RedoToolStripMenuItem, Me.RunMacroToolStripMenuItem, Me.toolStripSeparator6, Me.AddOrganismToolStripMenuItem, Me.AddStructureToolStripMenuItem, Me.AddTooToolStripMenuItem, Me.DeleteToolStripMenuItem})
            Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
            Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
            Me.EditToolStripMenuItem.Text = "&Edit"
            '
            'UndoToolStripMenuItem
            '
            Me.UndoToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.Undo
            Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
            Me.UndoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
            Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.UndoToolStripMenuItem.Text = "&Undo"
            '
            'RedoToolStripMenuItem
            '
            Me.RedoToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.Redo
            Me.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem"
            Me.RedoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
            Me.RedoToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.RedoToolStripMenuItem.Text = "&Redo"
            '
            'RunMacroToolStripMenuItem
            '
            Me.RunMacroToolStripMenuItem.Name = "RunMacroToolStripMenuItem"
            Me.RunMacroToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.RunMacroToolStripMenuItem.Text = "Run Macro"
            '
            'toolStripSeparator6
            '
            Me.toolStripSeparator6.Name = "toolStripSeparator6"
            Me.toolStripSeparator6.Size = New System.Drawing.Size(149, 6)
            '
            'AddOrganismToolStripMenuItem
            '
            Me.AddOrganismToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.AddOrganism
            Me.AddOrganismToolStripMenuItem.Name = "AddOrganismToolStripMenuItem"
            Me.AddOrganismToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.AddOrganismToolStripMenuItem.Text = "Add Organism"
            '
            'AddStructureToolStripMenuItem
            '
            Me.AddStructureToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.AddStructure
            Me.AddStructureToolStripMenuItem.Name = "AddStructureToolStripMenuItem"
            Me.AddStructureToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.AddStructureToolStripMenuItem.Text = "Add Structure"
            '
            'AddTooToolStripMenuItem
            '
            Me.AddTooToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.AddDataTool
            Me.AddTooToolStripMenuItem.Name = "AddTooToolStripMenuItem"
            Me.AddTooToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.AddTooToolStripMenuItem.Text = "Add Tool"
            '
            'DeleteToolStripMenuItem
            '
            Me.DeleteToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.Delete
            Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
            Me.DeleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
            Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
            Me.DeleteToolStripMenuItem.Text = "Delete"
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
            Me.RunSimulationToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.Simulate
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
            Me.SupportToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.TechnicalSupport
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
            Me.AnimatToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.OpenToolStripButton, Me.SaveToolStripButton, Me.toolStripSeparator1, Me.HelpToolStripButton, Me.ToolStripSeparator5, Me.AddOrganismStripButton, Me.AddStructureToolStripButton, Me.AddToolToolStripButton, Me.DeleteToolStripButton, Me.ToolStripSeparator9, Me.SelGraphicsToolStripButton, Me.SelCollisionToolStripButton, Me.SelJointsToolStripButton, Me.SelRecFieldsToolStripButton, Me.SelSimToolStripButton, Me.ToolStripDropDownButton1})
            Me.AnimatToolStrip.Location = New System.Drawing.Point(3, 24)
            Me.AnimatToolStrip.Name = "AnimatToolStrip"
            Me.AnimatToolStrip.SecurityMgr = Nothing
            Me.AnimatToolStrip.Size = New System.Drawing.Size(390, 25)
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
            'toolStripSeparator1
            '
            Me.toolStripSeparator1.Name = "toolStripSeparator1"
            Me.toolStripSeparator1.Size = New System.Drawing.Size(6, 25)
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
            Me.AddOrganismStripButton.Image = Global.AnimatGUI.ModuleInformation.AddOrganism1
            Me.AddOrganismStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AddOrganismStripButton.Name = "AddOrganismStripButton"
            Me.AddOrganismStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddOrganismStripButton.Text = "Add Organism"
            '
            'AddStructureToolStripButton
            '
            Me.AddStructureToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.AddStructureToolStripButton.Image = Global.AnimatGUI.ModuleInformation.AddStructure
            Me.AddStructureToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AddStructureToolStripButton.Name = "AddStructureToolStripButton"
            Me.AddStructureToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddStructureToolStripButton.Text = "Add Structure"
            '
            'AddToolToolStripButton
            '
            Me.AddToolToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.AddToolToolStripButton.Image = Global.AnimatGUI.ModuleInformation.AddDataTool
            Me.AddToolToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AddToolToolStripButton.Name = "AddToolToolStripButton"
            Me.AddToolToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddToolToolStripButton.Text = "Add Data Tool"
            '
            'DeleteToolStripButton
            '
            Me.DeleteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.DeleteToolStripButton.Image = Global.AnimatGUI.ModuleInformation.Delete
            Me.DeleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.DeleteToolStripButton.Name = "DeleteToolStripButton"
            Me.DeleteToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.DeleteToolStripButton.Text = "Delete"
            Me.DeleteToolStripButton.ToolTipText = "Delete selected workspace item"
            '
            'ToolStripSeparator9
            '
            Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
            Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 25)
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
            'ToolStripDropDownButton1
            '
            Me.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowGraphicsGeometryToolStripMenuItem, Me.ShowCollisionGeometryToolStripMenuItem, Me.ShowJointsToolStripMenuItem, Me.ShowCenterOfMassToolStripMenuItem, Me.ShowPartOriginToolStripMenuItem, Me.ShowContactsToolStripMenuItem})
            Me.ToolStripDropDownButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
            Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
            Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(61, 22)
            Me.ToolStripDropDownButton1.Text = "Display "
            Me.ToolStripDropDownButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
            Me.ToolStripDropDownButton1.ToolTipText = "Display Options"
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
            'SnapshotSimToolStripMenuItem
            '
            Me.SnapshotSimToolStripMenuItem.Name = "SnapshotSimToolStripMenuItem"
            Me.SnapshotSimToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
            Me.SnapshotSimToolStripMenuItem.Text = "Snapshot Sim"
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
            Me.GraphicsObjectsToolStripMenuItem.Name = "GraphicsObjectsToolStripMenuItem"
            Me.GraphicsObjectsToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
            Me.GraphicsObjectsToolStripMenuItem.Text = "Graphics Objects"
            Me.GraphicsObjectsToolStripMenuItem.CheckOnClick = True
            '
            'CollisionObjectsToolStripMenuItem
            '
            Me.CollisionObjectsToolStripMenuItem.Name = "CollisionObjectsToolStripMenuItem"
            Me.CollisionObjectsToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
            Me.CollisionObjectsToolStripMenuItem.Text = "Collision Objects"
            Me.CollisionObjectsToolStripMenuItem.CheckOnClick = True
            '
            'JointsToolStripMenuItem
            '
            Me.JointsToolStripMenuItem.Name = "JointsToolStripMenuItem"
            Me.JointsToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
            Me.JointsToolStripMenuItem.Text = "Joints"
            Me.JointsToolStripMenuItem.CheckOnClick = True
            '
            'ReceptiveFieldsToolStripMenuItem
            '
            Me.ReceptiveFieldsToolStripMenuItem.Name = "ReceptiveFieldsToolStripMenuItem"
            Me.ReceptiveFieldsToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
            Me.ReceptiveFieldsToolStripMenuItem.Text = "Receptive Fields"
            Me.ReceptiveFieldsToolStripMenuItem.CheckOnClick = True
            '
            'SimulationToolStripMenuItem
            '
            Me.SimulationToolStripMenuItem.Name = "SimulationToolStripMenuItem"
            Me.SimulationToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
            Me.SimulationToolStripMenuItem.Text = "Simulation"
            Me.SimulationToolStripMenuItem.CheckOnClick = True
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
            Me.ShowGraphicsGeometryToolStripMenuItem.Name = "ShowGraphicsGeometryToolStripMenuItem"
            Me.ShowGraphicsGeometryToolStripMenuItem.Size = New System.Drawing.Size(214, 22)
            Me.ShowGraphicsGeometryToolStripMenuItem.Text = "Show Graphics Geometry"
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
            'SelSimToolStripButton
            '
            Me.SelSimToolStripButton.CheckOnClick = True
            Me.SelSimToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.SelSimToolStripButton.Image = CType(resources.GetObject("SelSimToolStripButton.Image"), System.Drawing.Image)
            Me.SelSimToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.SelSimToolStripButton.Name = "SelSimToolStripButton"
            Me.SelSimToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.SelSimToolStripButton.Text = "Selection Simulation mode"
            Me.SelSimToolStripButton.ToolTipText = "Simulation select mode. This allows you to pick and move objects in the simulatio" & _
                "n using the mouse."
            '
            'AnimatApplication_ToolStrips
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(586, 283)
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

#Region " Enums "

        Public Enum enumAutoUpdateInterval
            Never
            Daily
            Weekly
            Monthly
        End Enum

#End Region

#Region " Attributes "

        Private Declare Function WebUpdate Lib "wuw4.dll" (ByVal URL As String) As Long

        Protected m_strAppVersion As String = "2.0"

        Protected m_mgrToolStripImages As AnimatGUI.Framework.ImageManager
        Protected m_mgrLargeImages As AnimatGUI.Framework.ImageManager
        Protected m_mgrWorkspaceImages As AnimatGUI.Framework.ImageManager
        Protected m_mgrTabPagesImages As AnimatGUI.Framework.ImageManager

        Protected m_selChildMenuStrip As AnimatMenuStrip
        Protected m_selChildToolStrip As AnimatToolStrip

        Protected m_dockManager As Crownwood.DotNetMagic.Docking.DockingManager
        Protected m_mdiClient As Control

        Protected m_strProjectPath As String = ""
        Protected m_strProjectFile As String = ""
        Protected m_strProjectName As String = ""
        Protected m_strPhysicsAssemblyName As String = "AnimatGUI.dll"
        Protected m_strPhysicsClassName As String = "AnimatGUI.DataObjects.Simulation"
        Protected m_strSimulationFile As String = ""
        Protected m_strLogDirectory As String = ""

        Protected m_strBodyEditorDll As String = "AnimatGUI.dll"
        Protected m_strBodyEditorNamespace As String = "AnimatGUI"

        Protected m_aryChildForms As New Collections.ExternalFileForms(Nothing)
        Protected m_arySortedChildForms As New Collections.SortedExternalFileForms(Nothing)

        Protected m_doSimulation As DataObjects.Simulation
        Protected m_doSimInterface As New AnimatGUI.Interfaces.SimulatorInterface

        Protected m_aryAllDataTypes As New Collections.DataObjects(Nothing)
        Protected m_aryNeuralModules As New Collections.SortedNeuralModules(Nothing)
        Protected m_aryPlugInAssemblies As New Collections.SortedAssemblies(Nothing)
        Protected m_aryBehavioralNodes As New Collections.Nodes(Nothing)
        Protected m_aryBehavioralLinks As New Collections.Links(Nothing)
        Protected m_aryBodyPartTypes As New Collections.BodyParts(Nothing)
        Protected m_aryRigidBodyTypes As New Collections.BodyParts(Nothing)
        Protected m_aryJointTypes As New Collections.BodyParts(Nothing)
        Protected m_aryBehavioralPanels As New Collections.SortedPanels(Nothing)
        Protected m_aryAlphabeticalBehavioralPanels As New ArrayList
        Protected m_aryToolPlugins As New Collections.Tools(Nothing)
        Protected m_aryGainTypes As New Collections.Gains(Nothing)
        Protected m_aryProgramModules As New Collections.ProgramModules(Nothing)
        Protected m_aryExternalStimuli As New Collections.Stimuli(Nothing)

        Protected m_wcWorkspaceContent As Crownwood.DotNetMagic.Docking.WindowContent
        Protected m_wcPropertiesContent As Crownwood.DotNetMagic.Docking.WindowContent
        Protected m_wcToolboxContent As Crownwood.DotNetMagic.Docking.WindowContent
        Protected m_wcSimControllerContent As Crownwood.DotNetMagic.Docking.WindowContent
        Protected m_wcRecFieldPairsContent As Crownwood.DotNetMagic.Docking.WindowContent
        Protected m_wcRecFieldGainContent As Crownwood.DotNetMagic.Docking.WindowContent
        Protected m_wcRecFieldCurrentContent As Crownwood.DotNetMagic.Docking.WindowContent

        Protected m_frmWorkspace As Forms.ProjectWorkspace
        Protected m_frmProperties As Forms.projectProperties
        Protected m_frmToolbox As Forms.Toolbox
        Protected m_frmSimulationController As Forms.SimulationController
        Protected m_frmReceptiveFieldPairs As Forms.ReceptiveFieldPairs
        Protected m_frmReceptiveFieldGain As Forms.ReceptiveFieldGain
        Protected m_frmReceptiveFieldCurrent As Forms.ReceptiveFieldCurrent

        'Protected m_ipToolPanel As New IconPanel

        'Protected m_ptSimWindowLocation As System.Drawing.Point
        'Protected m_szSimWindowSize As System.Drawing.Size

        Protected m_bProjectIsOpen As Boolean = False


#Region " Preferences "

        Protected m_strDefaultNewFolder As String = ""

        Protected m_Logger As New AnimatGUI.Interfaces.Logger
        Protected m_ModificationHistory As New AnimatGUI.Framework.UndoSystem.ModificationHistory

        Protected m_eAutoUpdateInterval As enumAutoUpdateInterval = enumAutoUpdateInterval.Daily
        Protected m_dtLastAutoUpdateTime As DateTime

        Protected m_bAnnouceUpdates As Boolean = False

        Protected m_SecurityMgr As New AnimatGuiCtrls.Security.SecurityManager

#End Region

        Protected m_strCommandLineParam As String = ""

#End Region

#Region " Properties "

#Region " Preferences "

        Public Overridable ReadOnly Property SecurityMgr() As AnimatGuiCtrls.Security.SecurityManager
            Get
                Return m_SecurityMgr
            End Get
        End Property

        Public Overridable Property DefaultNewFolder() As String
            Get
                Return m_strDefaultNewFolder
            End Get
            Set(ByVal value As String)
                m_strDefaultNewFolder = value
            End Set
        End Property

        Public Overridable Property AutoUpdateInterval() As enumAutoUpdateInterval
            Get
                Return m_eAutoUpdateInterval
            End Get
            Set(ByVal Value As enumAutoUpdateInterval)
                m_eAutoUpdateInterval = Value
            End Set
        End Property

        Public Overridable Property LastAutoUpdateTime() As DateTime
            Get
                Return m_dtLastAutoUpdateTime
            End Get
            Set(ByVal Value As DateTime)
                m_dtLastAutoUpdateTime = Value
            End Set
        End Property

        Public Property LogDirectory() As String
            Get
                Return m_strLogDirectory
            End Get
            Set(ByVal Value As String)
                m_strLogDirectory = Value

                m_Logger.LogPrefix = m_strLogDirectory & "\AnimatLab"
            End Set
        End Property

        Public ReadOnly Property Logger() As AnimatGUI.Interfaces.Logger
            Get
                Return m_Logger
            End Get
        End Property

#End Region

        Public Overridable ReadOnly Property ChildForms() As Collections.ExternalFileForms
            Get
                Return m_aryChildForms
            End Get
        End Property

        Public Overridable ReadOnly Property SortedChildForms() As Collections.SortedExternalFileForms
            Get
                Return m_arySortedChildForms
            End Get
        End Property

        Public Overridable ReadOnly Property ToolStripImages() As AnimatGUI.Framework.ImageManager
            Get
                Return m_mgrToolStripImages
            End Get
        End Property

        Public Overridable ReadOnly Property LargeImages() As AnimatGUI.Framework.ImageManager
            Get
                Return m_mgrLargeImages
            End Get
        End Property

        Public Overridable ReadOnly Property TabPagesImages() As AnimatGUI.Framework.ImageManager
            Get
                Return m_mgrTabPagesImages
            End Get
        End Property

        Public Overridable ReadOnly Property WorkspaceImages() As AnimatGUI.Framework.ImageManager
            Get
                Return m_mgrWorkspaceImages
            End Get
        End Property

        Public Overridable ReadOnly Property DockingManager() As DockingManager
            Get
                Return m_dockManager
            End Get
        End Property

        Public Overridable ReadOnly Property ProjectWorkspace() As Forms.ProjectWorkspace
            Get
                Return m_frmWorkspace
            End Get
        End Property

        Public Overridable ReadOnly Property ProjectProperties() As Forms.ProjectProperties
            Get
                Return m_frmProperties
            End Get
        End Property

        Public Overridable ReadOnly Property ProjectToolbox() As Forms.Toolbox
            Get
                Return m_frmToolbox
            End Get
        End Property

        Public Overridable ReadOnly Property SimulationController() As Forms.SimulationController
            Get
                Return m_frmSimulationController
            End Get
        End Property

        Public Overridable ReadOnly Property ReceptiveFieldPairs() As Forms.ReceptiveFieldPairs
            Get
                Return m_frmReceptiveFieldPairs
            End Get
        End Property

        Public Overridable ReadOnly Property ReceptiveFieldGain() As Forms.ReceptiveFieldGain
            Get
                Return m_frmReceptiveFieldGain
            End Get
        End Property

        Public Overridable ReadOnly Property ReceptiveFieldCurrent() As Forms.ReceptiveFieldCurrent
            Get
                Return m_frmReceptiveFieldCurrent
            End Get
        End Property

        Public Overridable ReadOnly Property ApplicationDirectory() As String
            Get
                Dim strPath As String, strFile As String
                Util.SplitPathAndFile(Application.ExecutablePath, strPath, strFile)

                Return strPath
            End Get
        End Property

        Public Overridable ReadOnly Property ToolsDirectory() As String
            Get
                Return ApplicationDirectory
                'If InStr(ApplicationDirectory, "AnimatGUI\") > 0 Then
                '    Return ApplicationDirectory
                'Else
                '    Return ApplicationDirectory & "AnimatGUI\"
                'End If
            End Get
        End Property

        Public ReadOnly Property MdiClient() As Control
            Get
                Return m_mdiClient
            End Get
        End Property

        Public Property ProjectPath() As String
            Get
                Return m_strProjectPath
            End Get
            Set(ByVal Value As String)
                m_strProjectPath = Value
                Me.SimulationInterface.SetProjectPath(m_strProjectPath)
            End Set
        End Property

        Public Property ProjectFile() As String
            Get
                Return m_strProjectFile
            End Get
            Set(ByVal Value As String)
                m_strProjectFile = Value
            End Set
        End Property

        Public Property ProjectName() As String
            Get
                Return m_strProjectName
            End Get
            Set(ByVal Value As String)
                m_strProjectName = Value
            End Set
        End Property

        Public Property PhysicsAssemblyName() As String
            Get
                Return m_strPhysicsAssemblyName
            End Get
            Set(ByVal Value As String)
                m_strPhysicsAssemblyName = Value
            End Set
        End Property

        Public Property SimulationFile() As String
            Get
                Return m_strSimulationFile
            End Get
            Set(ByVal Value As String)
                m_strSimulationFile = Value
            End Set
        End Property

        Public ReadOnly Property AppVersion() As String
            Get
                Return m_strAppVersion
            End Get
        End Property

        Public ReadOnly Property StatusBar() As Crownwood.DotNetMagic.Controls.StatusBarControl
            Get
                Return Me.AnimatStatusBar
            End Get
        End Property

        Public Overridable Property Simulation() As DataObjects.Simulation
            Get
                Return m_doSimulation
            End Get
            Set(ByVal Value As DataObjects.Simulation)
                m_doSimulation = Value
            End Set
        End Property

        Public Overridable ReadOnly Property SimulationInterface() As AnimatGUI.Interfaces.SimulatorInterface
            Get
                Return m_doSimInterface
            End Get
        End Property

        Public Overridable ReadOnly Property NeuralModules() As Collections.SortedNeuralModules
            Get
                Return m_aryNeuralModules
            End Get
        End Property

        Public Overridable ReadOnly Property PlugInAssemblies() As Collections.SortedAssemblies
            Get
                Return m_aryPlugInAssemblies
            End Get
        End Property

        Public Overridable ReadOnly Property BehavioralNodes() As Collections.Nodes
            Get
                Return m_aryBehavioralNodes
            End Get
        End Property

        Public Overridable ReadOnly Property BehavioralLinks() As Collections.Links
            Get
                Return m_aryBehavioralLinks
            End Get
        End Property

        Public Overridable ReadOnly Property BodyPartTypes() As Collections.BodyParts
            Get
                Return m_aryBodyPartTypes
            End Get
        End Property

        Public Overridable ReadOnly Property RigidBodyTypes() As Collections.BodyParts
            Get
                Return m_aryRigidBodyTypes
            End Get
        End Property

        Public Overridable ReadOnly Property JointTypes() As Collections.BodyParts
            Get
                Return m_aryJointTypes
            End Get
        End Property

        Public Overridable ReadOnly Property BehavioralPanels() As Collections.SortedPanels
            Get
                Return m_aryBehavioralPanels
            End Get
        End Property

        Public Overridable ReadOnly Property AlphabeticalBehavioralPanels() As ArrayList
            Get
                Return m_aryAlphabeticalBehavioralPanels
            End Get
        End Property

        Public Overridable ReadOnly Property GainTypes() As Collections.Gains
            Get
                Return m_aryGainTypes
            End Get
        End Property

        Public Overridable ReadOnly Property ProgramModules() As Collections.ProgramModules
            Get
                Return m_aryProgramModules
            End Get
        End Property

        Public Overridable ReadOnly Property ExternalStimuli() As Collections.Stimuli
            Get
                Return m_aryExternalStimuli
            End Get
        End Property

        Public Overridable ReadOnly Property ToolPlugins() As Collections.Tools
            Get
                Return m_aryToolPlugins
            End Get
        End Property

        'Public Overridable Property SimWindowLocation() As System.Drawing.Point
        '    Get
        '        Return m_ptSimWindowLocation
        '    End Get
        '    Set(ByVal Value As System.Drawing.Point)
        '        m_ptSimWindowLocation = Value
        '    End Set
        'End Property

        'Public Overridable Property SimWindowSize() As System.Drawing.Size
        '    Get
        '        Return m_szSimWindowSize
        '    End Get
        '    Set(ByVal Value As System.Drawing.Size)
        '        m_szSimWindowSize = Value
        '    End Set
        'End Property

        Public Overridable ReadOnly Property ProjectIsOpen() As Boolean
            Get
                Return m_bProjectIsOpen
            End Get
        End Property

        Public Overridable ReadOnly Property ModificationHistory() As AnimatGUI.Framework.UndoSystem.ModificationHistory
            Get
                Return m_ModificationHistory
            End Get
        End Property

        Public Overridable ReadOnly Property BodyEditorDll() As String
            Get
                Return m_strBodyEditorDll
            End Get
        End Property

        Public Overridable ReadOnly Property BodyEditorNamespace() As String
            Get
                Return m_strBodyEditorNamespace
            End Get
        End Property

#End Region

#Region " Methods "

#Region " Initialization "

        Public Overridable Sub StartApplication(ByVal bModal As Boolean)

            Try
                Dim args() As String = System.Environment.GetCommandLineArgs()

                If args.Length > 1 Then
                    m_strCommandLineParam = args(1)
                End If

                If bModal Then
                    Me.ShowDialog()
                Else
                    Me.Show()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)

            Try
                Util.Application = Me
                m_doSimInterface.Logger = Util.Logger

                Me.AnimatToolStrip.ToolName = "AnimatGUI.Forms.AnimatApplication"
                Me.AnimatToolStrip.SecurityMgr = Me.SecurityMgr

                Me.AnimatMenuStrip.ToolName = "AnimatGUI.Forms.AnimatApplication"
                Me.AnimatMenuStrip.SecurityMgr = Me.SecurityMgr

                'Reset the culture info to be invariant english. I was getting problems 
                'with foriegn culture infos not parsing the xml files correctly.
                Thread.CurrentThread.CurrentCulture = New CultureInfo("")
                'Thread.CurrentThread.CurrentUICulture = New CultureInfo("")

                InitLogging()
                FindMdiClient()
                CatalogPluginModules()
                CheckSimRegistryEntry()
                ResetProject(False)
                LoadUserConfig()
                AutoUpdate()
                UpdateToolstrips()

                'This handler is used as a callback to get the editor to saveout the xml configuration file and send it back to the simulation
                AddHandler m_doSimInterface.OnSimulationCreate, AddressOf Me.OnCreateSimulation
                AddHandler m_doSimInterface.SimulationRunning, AddressOf Me.OnSimulationRunning
                AddHandler m_doSimInterface.NeedToStopSimulation, AddressOf Me.OnNeedToStopSimulation

                AddHandler Me.SimulationStarting, AddressOf Me.OnSimulationStarting
                AddHandler Me.SimulationResuming, AddressOf Me.OnSimulationResuming
                AddHandler Me.SimulationPaused, AddressOf Me.OnSimulationPaused
                AddHandler Me.SimulationStopped, AddressOf Me.OnSimulationStopped

                Util.Simulation.VisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectGraphics

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AutoUpdate()
            Try
#If Not Debug Then
                Dim bDoUpdate As Boolean = False
                If (m_eAutoUpdateInterval = enumAutoUpdateInterval.Daily) Then
                    Dim temp As TimeSpan = Now.Subtract(m_dtLastAutoUpdateTime)
                    If (Now.Subtract(m_dtLastAutoUpdateTime).Days >= 1) Then
                        bDoUpdate = True
                    End If
                ElseIf (m_eAutoUpdateInterval = enumAutoUpdateInterval.Weekly) Then
                    If (Now.Subtract(m_dtLastAutoUpdateTime).Days > 6) Then
                        bDoUpdate = True
                    End If
                ElseIf (m_eAutoUpdateInterval = enumAutoUpdateInterval.Monthly) Then
                    If (Now.Subtract(m_dtLastAutoUpdateTime).Days > 29) Then
                        bDoUpdate = True
                    End If
                ElseIf (m_eAutoUpdateInterval = enumAutoUpdateInterval.Never) Then
                    bDoUpdate = False
                End If

                If bDoUpdate = True Then
                    CheckForUpdates(False)
                End If
#End If
            Catch ex As System.Exception
            End Try

        End Sub

        Public Sub CheckForUpdates(ByVal bAnnouceUpdates As Boolean)

            'For some reason if the user did not have internet access then the auto-update system could
            'lock up when the app starts. So I am having it start a new thread to check for the update
            'so the main app can go ahead.
            m_bAnnouceUpdates = bAnnouceUpdates
            Dim threadUpdates As New Threading.Thread(AddressOf Me.CheckForUpdatesThread)
            threadUpdates.Start()
            Threading.Thread.Sleep(0)

        End Sub

        Private Sub CheckForUpdatesThread()

            'First lets try and ping the server to see if this person is online.
            'If they are not then lets skip trying to check for updates.
            Dim netMon As New Crownwood.Magic.Network.Ping
            Dim response As Crownwood.Magic.Network.PingResponse = netMon.PingHost("www.animatlab.com", 4)

            If Not response Is Nothing AndAlso response.PingResult = Crownwood.Magic.Network.PingResponseType.Ok Then
                Dim myURL As String
                myURL = "http://www.animatlab.com/animatLab_Update.txt"
                WebUpdate(myURL)
                Me.LastAutoUpdateTime = Now
                Util.UpdateConfigFile()
            ElseIf m_bAnnouceUpdates = True Then
                MessageBox.Show("Unable to ping host!")
            End If

        End Sub

        Private Sub LoadUserConfig()
            Try
                m_strDefaultNewFolder = System.Configuration.ConfigurationManager.AppSettings("DefaultNewFolder")

                m_eAutoUpdateInterval = DirectCast([Enum].Parse(GetType(enumAutoUpdateInterval), System.Configuration.ConfigurationManager.AppSettings("UpdateFrequency"), True), enumAutoUpdateInterval)

                Try
                    Dim strDate As String = System.Configuration.ConfigurationManager.AppSettings("UpdateTime")
                    m_dtLastAutoUpdateTime = Date.Parse(strDate)
                Catch exDate As System.Exception
                    'If for some reason it fails on the parsing of the update time then set it to some time way in the past.
                    m_dtLastAutoUpdateTime = DateTime.Parse("1/1/2001")
                End Try

            Catch ex As System.Exception
                'If for some reason it fails on the parsing of the update time then set it to some time way in the past.
                m_dtLastAutoUpdateTime = DateTime.Parse("1/1/2001")
                m_strDefaultNewFolder = ""
                m_eAutoUpdateInterval = enumAutoUpdateInterval.Never

                'The nUnit system eats my configuration settings, so I am changing this to only show this error
                'when we are in production mode so this error does not get shown and these values are just defaulted.
#If Not Debug Then
                AnimatGUI.Framework.Util.DisplayError(ex)
#End If
            End Try
        End Sub

        Protected Overridable Sub InitLogging()
            Try
                If Directory.Exists(Me.ApplicationDirectory & "Logs") Then
                    Me.LogDirectory = Me.ApplicationDirectory & "Logs\"
                Else
                    Me.LogDirectory = Me.ApplicationDirectory
                End If

                Me.Logger.TraceLevel = Interfaces.Logger.enumLogLevel.Error
                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Initialized Logging")

                'Dim frame As New StackFrame(True)
                'Dim iVal As Integer
                'Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "File: " & frame.GetFileName & " Line: " & frame.GetFileLineNumber)
                'iVal = frame.GetFileLineNumber()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub FindMdiClient()
            Dim c As Control

            For Each c In Me.Controls
                If TypeOf c Is MdiClient Then
                    m_mdiClient = c
                End If
            Next

            AddHandler m_mdiClient.Click, AddressOf Me.OnMdiClientClicked

            Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Found Mdi Client Window")
        End Sub

        Public Overridable Sub CheckSimRegistryEntry()

            Try

                'MessageBox.Show("I am about to open the 'software' registry subkey for read-only access!")

                Dim rkSoftware As Microsoft.Win32.RegistryKey
                Try
                    rkSoftware = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software", False)
                Catch ex As System.Exception
                    'MessageBox.Show("Error opening 'software' for read-only access: " & ex.Message)
                    'If we get an error here then assume that we can not open the registry and jump out.
                    Return
                End Try

                'MessageBox.Show("I am about to open the 'FLEXlm License Manager' registry subkey for read-only access!")

                Dim rkKey As Microsoft.Win32.RegistryKey = rkSoftware.OpenSubKey("FLEXlm License Manager", False)

                If rkKey Is Nothing Then

                    'MessageBox.Show("'FLEXlm License Manager' subkey was not found. I am opening it for write access")

                    Try
                        rkSoftware = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software", True)
                    Catch ex As System.Exception
                        'MessageBox.Show("Error opening 'software' for write access: " & ex.Message)
                        'If we get an error here then assume that we can not open the registry for writing and jump out.
                        Return
                    End Try

                    rkKey = rkSoftware.CreateSubKey("FLEXlm License Manager")
                End If

                'MessageBox.Show("I am attempting to get the item names under 'FLEXlm License Manager'")

                Dim aryNames As String() = rkKey.GetValueNames()

                For Each strName As String In aryNames
                    If strName.Trim.ToUpper() = "MATHNGIN_LICENSE_FILE" Then
                        'MessageBox.Show("'MATHNGIN_LICENSE_FILE' was found. I am exiting")
                        Return
                    End If
                Next

                'We may have gotten here by only opening the system to read, now lets try and open it for writing
                Try
                    'MessageBox.Show("I am about to open the 'FLEXlm License Manager' registry subkey for write access!")

                    rkKey = rkSoftware.OpenSubKey("FLEXlm License Manager", True)
                Catch ex As System.Exception
                    'If we get an error here then assume that we can not open the registry for writing and jump out.
                    Return
                End Try

                'MessageBox.Show("I am about to write the 'MATHNGIN_LICENSE_FILE' item")

                'If we get here then the license file registry entry does not exist so lets add it.
                Dim strDir As String = Util.Application.ApplicationDirectory
                strDir = strDir.Substring(0, strDir.Length - 1)
                rkKey.SetValue("MATHNGIN_LICENSE_FILE", strDir)

                'MessageBox.Show("I am finished messing with the registry")

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_doSimulation Is Nothing Then m_doSimulation.ClearIsDirty()
            m_aryNeuralModules.ClearIsDirty()
            m_aryPlugInAssemblies.ClearIsDirty()
            m_aryBehavioralNodes.ClearIsDirty()
            m_aryBehavioralLinks.ClearIsDirty()
            m_aryBehavioralPanels.ClearIsDirty()
            m_aryBodyPartTypes.ClearIsDirty()
            m_aryRigidBodyTypes.ClearIsDirty()
            m_aryJointTypes.ClearIsDirty()

            For Each frmChild As AnimatForm In Me.ChildForms
                frmChild.ClearIsDirty()
            Next

        End Sub

        Public Overridable Sub ChangeUnits(ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                           ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits)

            Dim ePrevDist As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits
            Dim ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits

            Try
                Util.ModificationHistory.AllowAddHistory = False

                ePrevDist = Util.Environment.DistanceUnits
                ePrevMass = Util.Environment.MassUnits

                If Util.Environment.MassUnits <> eNewMass Then
                    Util.Environment.MassUnits = eNewMass
                End If

                If Util.Environment.DistanceUnits <> eNewDistance Then
                    Util.Environment.DistanceUnits = eNewDistance
                End If

                Dim iMassDiff As Integer = CInt(eNewMass) - CInt(ePrevMass)
                Dim iDistDiff As Integer = CInt(eNewDistance) - CInt(ePrevDist)
                Dim fltMassChange As Single = CSng(10 ^ iMassDiff)
                Dim fltDistanceChange As Single = CSng(10 ^ iDistDiff)

                'Now lets go through and set the untis changed for the core simulation objects.
                Util.Simulation.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDist, eNewDistance, fltDistanceChange)

                'Now inform any interested part that the units have changed
                RaiseEvent UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDist, eNewDistance, fltDistanceChange)

            Catch ex As System.Exception
                Throw ex
            Finally
                Util.ModificationHistory.AllowAddHistory = True
            End Try

            If Not Me.UndoRedoInProgress Then
                Util.ModificationHistory.AddHistoryEvent(New Framework.UndoSystem.ScaleUnitChangedEvent(Me, ePrevMass, eNewMass, ePrevDist, eNewDistance))
            End If
        End Sub

#End Region

#Region " Plug-in-Module Management "

        Protected Overridable Sub CatalogPluginModules()
            Dim tpClass As Type
            Dim strFile As String
            Dim strFailedLoad As String = ""
            Dim iFailedLoad As Integer = 0

            Try
                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Debug, "Beginning to catalog plugin modules")

                Dim aryFileNames As New ArrayList
                Dim bAddModule As Boolean = False

                m_aryAllDataTypes.Clear()
                m_aryNeuralModules.Clear()
                m_aryNeuralModules.Clear()
                m_aryPlugInAssemblies.Clear()
                m_aryBehavioralNodes.Clear()
                m_aryBehavioralLinks.Clear()
                m_aryBodyPartTypes.Clear()
                m_aryRigidBodyTypes.Clear()
                m_aryJointTypes.Clear()
                m_aryToolPlugins.Clear()
                m_aryGainTypes.Clear()
                m_aryProgramModules.Clear()
                m_aryExternalStimuli.Clear()

                'First find a list of all possible assemblies. It may be one or it may be a standard win32 dll. We will have to see later.
                Util.FindAssemblies(Me.ApplicationDirectory(), aryFileNames)

                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Debug, "Util.FindAssemblies")

                For Each oFile As Object In aryFileNames
                    strFile = DirectCast(oFile, String)
                    bAddModule = False

                    'If strFile.Contains("nunit.fixtures.dll") Then
                    '    strFile = strFile
                    'End If
                    Try
                        Dim assemModule As System.Reflection.Assembly = Util.LoadAssembly(Util.GetFilePath(Me.ApplicationDirectory, strFile), False)
                        If Not assemModule Is Nothing Then

                            Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Debug, "About to get types: " & assemModule.FullName)

                            Dim aryTypes() As Type
                            Try
                                aryTypes = assemModule.GetTypes()
                            Catch ex As Exception
                                'If we have trouble gettting the object types then this is not 
                                'one of our dlls so skip it.
                                ReDim aryTypes(0)
                            End Try

                            Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Debug, "Starting to loop through: " & assemModule.FullName)

                            If assemModule.GetName().Name <> "UI Components" Then

                                For Each tpClass In aryTypes

                                    If Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Behavior.Node)) Then
                                        Dim bnNode As DataObjects.Behavior.Node = CreateNode(assemModule, tpClass, Nothing)
                                        If Not bnNode Is Nothing Then
                                            m_aryBehavioralNodes.Add(bnNode)
                                            m_aryAllDataTypes.Add(bnNode)
                                            bAddModule = True
                                        End If
                                    ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Behavior.Link)) Then
                                        Dim blLink As DataObjects.Behavior.Link = CreateLink(assemModule, tpClass, Nothing)
                                        If Not blLink Is Nothing Then
                                            m_aryBehavioralLinks.Add(blLink)
                                            m_aryAllDataTypes.Add(blLink)
                                            bAddModule = True
                                        End If
                                    ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Behavior.NeuralModule)) Then
                                        Dim nmModule As DataObjects.Behavior.NeuralModule = CreateNeuralModule(assemModule, tpClass, Nothing)
                                        If Not nmModule Is Nothing Then
                                            m_aryNeuralModules.Add(nmModule.ClassName, nmModule)
                                            m_aryAllDataTypes.Add(nmModule)
                                            bAddModule = True
                                        End If
                                    ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.Forms.Tools.ToolForm)) Then
                                        Dim frmTool As Forms.Tools.ToolForm = CreateToolForm(assemModule, tpClass, Nothing)
                                        If Not frmTool Is Nothing Then
                                            m_aryToolPlugins.Add(frmTool)
                                        End If
                                    ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Physical.RigidBody)) Then
                                        Try
                                            Dim doPart As AnimatGUI.DataObjects.Physical.RigidBody = CreateRigidBody(assemModule, tpClass, Nothing)
                                            If Not doPart Is Nothing Then

                                                m_aryBodyPartTypes.Add(doPart)
                                                m_aryRigidBodyTypes.Add(doPart)
                                                m_aryAllDataTypes.Add(doPart)
                                                bAddModule = True
                                            End If
                                        Catch ex As System.Exception
                                            MessageBox.Show("Error loading rigid body part")
                                        End Try
                                    ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Physical.Joint)) Then
                                        Try
                                            Dim doPart As AnimatGUI.DataObjects.Physical.Joint = CreateJoint(assemModule, tpClass, Nothing)
                                            If Not doPart Is Nothing Then

                                                m_aryBodyPartTypes.Add(doPart)
                                                m_aryJointTypes.Add(doPart)
                                                m_aryAllDataTypes.Add(doPart)
                                                bAddModule = True
                                            End If
                                        Catch ex As System.Exception
                                            MessageBox.Show("Error loading joint part")
                                        End Try
                                    ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Gain)) Then
                                        Dim doGain As DataObjects.Gain = CreateGain(assemModule, tpClass, Nothing)
                                        If Not doGain Is Nothing Then
                                            m_aryGainTypes.Add(doGain)
                                            m_aryAllDataTypes.Add(doGain)
                                        End If
                                    ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.ProgramModule)) Then
                                        Dim doModule As DataObjects.ProgramModule = CreateProgramModule(assemModule, tpClass, Nothing)
                                        If Not doModule Is Nothing Then
                                            m_aryProgramModules.Add(doModule)
                                            m_aryAllDataTypes.Add(doModule)
                                        End If
                                    ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.ExternalStimuli.Stimulus), False) Then
                                        Dim doStim As DataObjects.ExternalStimuli.Stimulus = CreateExternalStimuli(assemModule, tpClass, Nothing)
                                        If Not doStim Is Nothing Then
                                            m_aryExternalStimuli.Add(doStim, False)
                                            m_aryAllDataTypes.Add(doStim)
                                        End If
                                    End If
                                Next

                                tpClass = Nothing
                            End If

                            Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Debug, "Finished looping through: " & assemModule.FullName)
                        End If

                        If bAddModule Then
                            m_aryPlugInAssemblies.Add(Util.RootNamespace(assemModule), assemModule)
                        End If

                    Catch ex As System.Exception
                        iFailedLoad = iFailedLoad + 1
                        If Not tpClass Is Nothing Then
                            strFailedLoad = strFailedLoad + vbTab & tpClass.FullName & vbCrLf
                        End If
                    End Try

                Next

                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Debug, "Initialize dataobjects after Application Start")

                InitializeDataObjectsAfterAppStart()

                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Debug, "Finished Looping through assemblies")

                CreateBehavioralPanels()

                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Finished cataloging plugin modules")

            Catch ex As System.Exception
                If Not tpClass Is Nothing Then
                    Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Debug, "Error in CatalogPluginModules " & tpClass.FullName)
                Else
                    Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Debug, "Error in CatalogPluginModules. Type is nothing")
                End If

                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                If iFailedLoad > 0 Then
                    Dim strMessage As String = "One or more of the vb.net plug-in modules for this application failed to load correctly. " & _
                                 "This could signal an error for a specific type of module, but more often this type of error means " & _
                                 "that the .Net framework or DirectX were not installed correctly on this computer. If this is a new " & _
                                 "installation of Animatlab then this is almost certainly the cause of this error. To fix this please go " & _
                                 "http:\\www.AnimatLab.com\Troubleshooting.html. There you can download both the .Net framework and " & _
                                 "DirectX and install them manually to fix this problem."
                    If strFailedLoad.Trim.Length > 0 Then
                        strMessage = strMessage & vbCrLf & vbCrLf & "Below is a list of the plug-ins that failed to load. If there are " & _
                                     "only a few items, and they appear to be in the same dll then this means that this problem is most " & _
                                     "likely isolated to a specific plug-in module, and you need to replace or repair that module in order " & _
                                     "to fix this error." & vbCrLf & strFailedLoad
                    End If

                    MessageBox.Show(strMessage)
                End If
            End Try

        End Sub

        Protected Overridable Sub InitializeDataObjectsAfterAppStart()

            Try
                For Each doObj As Framework.DataObject In m_aryAllDataTypes
                    doObj.InitAfterAppStart()
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Function CreateNode(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Behavior.Node

            Try
                If Not tpClass.IsAbstract Then
                    Dim bnNode As DataObjects.Behavior.Node = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Behavior.Node)
                    Return bnNode
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    MessageBox.Show("CreateNode: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateLink(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Behavior.Link

            Try
                If Not tpClass.IsAbstract Then
                    Dim blLink As DataObjects.Behavior.Link = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Behavior.Link)
                    Return blLink
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    MessageBox.Show("CreateLink: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateNeuralModule(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Behavior.NeuralModule

            Try
                If Not tpClass.IsAbstract Then
                    Dim nmModule As DataObjects.Behavior.NeuralModule = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Behavior.NeuralModule)
                    Return nmModule
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    MessageBox.Show("CreateNeuralModule: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateToolForm(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As Forms.Tools.ToolForm

            Try
                If Not tpClass.IsAbstract Then
                    Dim frmTool As Forms.Tools.ToolForm = DirectCast(Util.LoadClass(assemModule, tpClass.FullName), Forms.Tools.ToolForm)
                    Return frmTool
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    MessageBox.Show("CreateToolForm: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateRigidBody(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Physical.RigidBody

            Try
                If Not tpClass.IsAbstract Then
                    Dim doBody As DataObjects.Physical.RigidBody = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Physical.RigidBody)
                    Return doBody
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    MessageBox.Show("CreateRigidBody: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateJoint(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Physical.Joint

            Try
                If Not tpClass.IsAbstract Then
                    Dim doJoint As DataObjects.Physical.Joint = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Physical.Joint)
                    Return doJoint
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    MessageBox.Show("CreateJoint: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateGain(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Gain

            Try
                If Not tpClass.IsAbstract Then
                    Dim doGain As DataObjects.Gain = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Gain)
                    Return doGain
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    MessageBox.Show("CreateGain: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateProgramModule(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.ProgramModule

            Try
                If Not tpClass.IsAbstract Then
                    Dim doModule As DataObjects.ProgramModule = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.ProgramModule)
                    Return doModule
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    MessageBox.Show("CreateProgramModule: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateExternalStimuli(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.ExternalStimuli.Stimulus

            Try
                If Not tpClass.IsAbstract Then
                    Dim doStim As DataObjects.ExternalStimuli.Stimulus = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.ExternalStimuli.Stimulus)
                    Return doStim
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    MessageBox.Show("CreateExternalStimuli: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Sub CreateBehavioralPanels()

            Try
                m_aryBehavioralPanels.Clear()
                m_aryAlphabeticalBehavioralPanels.Clear()

                'Lets create the alphabetical band
                Dim pdAlphabetical As New AnimatGUI.DataObjects.Behavior.PanelData
                pdAlphabetical.m_assemModule = Nothing
                m_aryBehavioralPanels.Add("AnimatGUI.Alphabetical", pdAlphabetical)
                pdAlphabetical.m_strPanelName = "Alphabetical Tools"

                'Now lets go through and add icon panels for each of those modules.
                Dim assemModule As System.Reflection.Assembly
                Dim pdPanel As DataObjects.Behavior.PanelData
                For Each deEntry As DictionaryEntry In m_aryPlugInAssemblies
                    assemModule = DirectCast(deEntry.Value, System.Reflection.Assembly)

                    pdPanel = New DataObjects.Behavior.PanelData
                    pdPanel.m_strPanelName = Util.ModuleName(assemModule)
                    pdPanel.m_assemModule = assemModule
                    m_aryBehavioralPanels.Add(DirectCast(deEntry.Key, String), pdPanel)
                Next

                'Now lets make lists of the nodes that will go into the panels
                For Each bnNode As DataObjects.Behavior.Node In m_aryBehavioralNodes
                    If bnNode.IsDisplayedInIconPanel Then
                        pdPanel = DirectCast(m_aryBehavioralPanels(bnNode.AssemblyModuleName), DataObjects.Behavior.PanelData)
                        pdPanel.m_aryNodes.Add(bnNode)
                        pdAlphabetical.m_aryNodes.Add(bnNode)
                    End If
                Next

                'Now we need to go through and sort each node list in the panel datas so that they are alphabetical
                Dim aryIDs As New ArrayList
                For Each deEntry As DictionaryEntry In m_aryBehavioralPanels
                    pdPanel = DirectCast(deEntry.Value, DataObjects.Behavior.PanelData)

                    If pdPanel.m_aryNodes.Count > 0 Then
                        pdPanel.m_aryNodes.Sort(New AnimatGUI.Collections.Comparers.CompareNodeNames)
                        m_aryAlphabeticalBehavioralPanels.Add(pdPanel)
                    Else
                        Dim strID As String = DirectCast(deEntry.Key, String)
                        If strID <> "AnimatGUI.Alphabetical" Then
                            aryIDs.Add(deEntry.Key)
                        End If
                    End If
                Next

                'Remove any panels that do not have any nodes in them.
                For Each strID As String In aryIDs
                    m_aryBehavioralPanels.Remove(strID)
                Next

                'Now sort the panels
                m_aryAlphabeticalBehavioralPanels.Sort(New AnimatGUI.Collections.Comparers.ComparePanelNames)


            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub PopulateToolbox()
            Try
                'If m_frmToolbox Is Nothing Then Return

                ''Only add this if it has not already been added.
                'If m_frmToolbox.ctrlOutlookbar.Controls.Count = 0 Then
                '    m_ipToolPanel = New IconPanel
                '    'ipPanel.IconHeight = 55
                '    m_frmToolbox.ctrlOutlookbar.AddBand("Toolbox", m_ipToolPanel)

                '    For Each frmTool As Forms.Tools.ToolForm In m_aryToolPlugins
                '        Dim panelIcon As PanelIcon = m_ipToolPanel.AddIcon(frmTool.Name, frmTool.WorkspaceImage, frmTool.DragImage, frmTool)
                '        AddHandler panelIcon.DoubleClickIcon, AddressOf Me.OnDoubleClickTool
                '    Next

                'End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " Menu/Toolbar Creation "

        Protected Overridable Sub CreateImageManager()

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

            m_mgrToolStripImages = New AnimatGUI.Framework.ImageManager
            m_mgrToolStripImages.ImageList.ImageSize = New Size(32, 32)
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Open_Toolbar.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Save_Toolbar.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.NewProject.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.ProjectWorkspace.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.ProjectProperties.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Simulate.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.SimulationController.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.ReceptiveField.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Undo.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.UndoGrey.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Redo.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.RedoGrey.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.HelpContents.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.HelpSearch.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.TechnicalSupport.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.ExportStandalone.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.CutFromWorkspace.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AddDataTool.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AddOrganism.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AddStructure.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AddGround.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AddWater.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Expand.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Collapse.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.AddStimulus.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Delete.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Cut.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Copy.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Paste.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.preference.gif")
            m_mgrToolStripImages.AddImage(myAssembly, "AnimatGUI.Swap.gif")

            Me.Icon = m_mgrToolStripImages.LoadIcon(myAssembly, "AnimatGUI.Crayfish.ico")

            m_mgrWorkspaceImages = New AnimatGUI.Framework.ImageManager
            m_mgrTabPagesImages = New AnimatGUI.Framework.ImageManager
            m_mgrTabPagesImages.ImageList.ImageSize = New Size(35, 35)

            m_mgrLargeImages = New AnimatGUI.Framework.ImageManager
            m_mgrLargeImages.ImageList.ImageSize = New Size(16, 16)
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.NewProject.gif")
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.Open_Toolbar.gif")
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.Save_Toolbar.gif")
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.CutFromWorkspace.gif")
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.AddDataTool.gif")
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.AddOrganism.gif")
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.AddStructure.gif")
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.AddGround.gif")
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.AddWater.gif")
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.Expand.gif")
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.Collapse.gif")
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.AddStimulus.gif")
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.Delete.gif")
            m_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.preference.gif")
            'm_mgrLargeImages.AddImage(myAssembly, "AnimatGUI.SimulateLarge.gif")

        End Sub

        Public Overrides Sub UpdateToolstrips()

            If Me.m_bProjectIsOpen Then
                'If a project is not open then disable a lot of stuff
                Me.SaveToolStripMenuItem.Enabled = True
                Me.SaveToolStripButton.Enabled = True
                Me.SaveAsToolStripMenuItem.Enabled = True
                Me.CloseProjectToolStripMenuItem.Enabled = True
                Me.ExportStandaloneToolStripMenuItem.Enabled = True

                Me.UndoToolStripMenuItem.Enabled = True
                Me.RedoToolStripMenuItem.Enabled = True
                Me.RunMacroToolStripMenuItem.Enabled = True

                Me.AddOrganismToolStripMenuItem.Enabled = True
                Me.AddStructureToolStripMenuItem.Enabled = True
                Me.AddTooToolStripMenuItem.Enabled = True

                Me.RunSimulationToolStripMenuItem.Enabled = True
                Me.CustomizeToolStripMenuItem.Enabled = True

                Me.AddOrganismStripButton.Enabled = True
                Me.AddStructureToolStripButton.Enabled = True
                Me.AddToolToolStripButton.Enabled = True

                Me.DeleteToolStripButton.Enabled = True
                Me.DeleteToolStripMenuItem.Enabled = True

                Me.SnapshotSimToolStripMenuItem.Enabled = True
            Else
                'If a project is not open then disable a lot of stuff
                Me.SaveToolStripMenuItem.Enabled = False
                Me.SaveToolStripButton.Enabled = False
                Me.SaveAsToolStripMenuItem.Enabled = False
                Me.CloseProjectToolStripMenuItem.Enabled = False
                Me.ExportStandaloneToolStripMenuItem.Enabled = False

                Me.UndoToolStripMenuItem.Enabled = False
                Me.RedoToolStripMenuItem.Enabled = False
                Me.RunMacroToolStripMenuItem.Enabled = False

                Me.AddOrganismToolStripMenuItem.Enabled = False
                Me.AddStructureToolStripMenuItem.Enabled = False
                Me.AddTooToolStripMenuItem.Enabled = False

                Me.RunSimulationToolStripMenuItem.Enabled = False
                Me.CustomizeToolStripMenuItem.Enabled = False

                Me.AddOrganismStripButton.Enabled = False
                Me.AddStructureToolStripButton.Enabled = False
                Me.AddToolToolStripButton.Enabled = False

                Me.DeleteToolStripButton.Enabled = False
                Me.DeleteToolStripMenuItem.Enabled = False

                Me.SnapshotSimToolStripMenuItem.Enabled = False
            End If

        End Sub

        'Public Overridable Function CreateDefaultMenu() As AnimatGuiCtrls.Controls.AnimatMenuStrip
        '    '    Dim newMenu As New MenuControl

        '    '    newMenu.MdiContainer = Me

        '    '    'Lets create the file menu
        '    '    Dim mcFile As New MenuCommand("File", "File")
        '    '    mcFile.Description = "File Menu Commands"

        '    '    Dim mcNewProject As New MenuCommand("New Project", "NewProject", m_mgrToolStripImages.ImageList, _
        '    '             m_mgrToolStripImages.GetImageIndex("AnimatGUI.NewProject.gif"), Shortcut.CtrlN, _
        '    '             New EventHandler(AddressOf Me.OnNewProject))

        '    '    Dim mcOpenProject As New MenuCommand("Open Project", "OpenProject", m_mgrToolStripImages.ImageList, _
        '    '             m_mgrToolStripImages.GetImageIndex("AnimatGUI.Open_Toolbar.gif"), Shortcut.CtrlO, _
        '    '             New EventHandler(AddressOf Me.OnOpenProject))

        '    '    Dim mcCloseProject As New MenuCommand("Close Project", "CloseProject", New EventHandler(AddressOf Me.OnCloseProject))

        '    '    Dim mcSaveProject As New MenuCommand("Save Project", "SaveProject", m_mgrToolStripImages.ImageList, _
        '    '             m_mgrToolStripImages.GetImageIndex("AnimatGUI.Save_Toolbar.gif"), Shortcut.CtrlS, _
        '    '             New EventHandler(AddressOf Me.OnSaveProject))

        '    '    Dim mcSaveProjectAs As New MenuCommand("Save Project As", "SaveProjectAs", New EventHandler(AddressOf Me.OnSaveProjectAs))

        '    '    Dim mcExportForStandAloneSim As New MenuCommand("Export Standalone Sim", "ExportStandaloneSim", m_mgrToolStripImages.ImageList, _
        '    '                                      m_mgrToolStripImages.GetImageIndex("AnimatGUI.ExportStandalone.gif"), New EventHandler(AddressOf Me.OnExportStandaloneSim))

        '    '    Dim mcSep1 As MenuCommand = New MenuCommand("-")
        '    '    Dim mcExit As New MenuCommand("Exit", "Exit", New EventHandler(AddressOf Me.OnExit))

        '    '    'Dim mcNewFloat As New MenuCommand("New Float", New EventHandler(AddressOf Me.OnNewFloat))
        '    '    'Dim mcNewGraph As New MenuCommand("New Graph", New EventHandler(AddressOf Me.OnNewGraph))
        '    '    'Dim mcNewBehavioral As New MenuCommand("New Behavioral Editor", New EventHandler(AddressOf Me.OnNewBehavioralEditor))
        '    '    'Dim mcNewSimulator As New MenuCommand("NewSimulator", New EventHandler(AddressOf Me.OnNewSimulator))

        '    '    'mcFile.MenuCommands.AddRange(New MenuCommand() {mcNewProject, mcOpenProject, mcSaveProject, mcNewFloat, mcNewGraph, mcNewBehavioral, mcNewSimulator})
        '    '    mcFile.MenuCommands.AddRange(New MenuCommand() {mcNewProject, mcOpenProject, mcSaveProject, mcSaveProjectAs, mcCloseProject, mcExportForStandAloneSim, mcSep1, mcExit})
        '    '    newMenu.MenuCommands.Add(mcFile)

        '    '    Dim mcEdit As New MenuCommand("Edit", "Edit")
        '    '    mcEdit.Description = "Edit Diagram Commands"
        '    '    Dim mcUndo As New MenuCommand("Undo", "Undo", m_mgrToolStripImages.ImageList, _
        '    '                                      m_mgrToolStripImages.GetImageIndex("AnimatGUI.Undo.gif"), _
        '    '                                      System.Windows.Forms.Shortcut.CtrlZ, New EventHandler(AddressOf Me.OnUndo))
        '    '    Dim mcRedo As New MenuCommand("Redo", "Redo", m_mgrToolStripImages.ImageList, _
        '    '                                      m_mgrToolStripImages.GetImageIndex("AnimatGUI.Redo.gif"), _
        '    '                                      System.Windows.Forms.Shortcut.CtrlY, New EventHandler(AddressOf Me.OnRedo))
        '    '    Dim mcCutWorkspace As New MenuCommand("Cut Workspace", "CutWorkspace", m_mgrToolStripImages.ImageList, _
        '    '                                      m_mgrToolStripImages.GetImageIndex("AnimatGUI.CutFromWorkspace.gif"), _
        '    '                                      New EventHandler(AddressOf Me.OnCutWorkspace))
        '    '    Dim mcSep2 As MenuCommand = New MenuCommand("-")
        '    '    Dim mcAddOrganism As New MenuCommand("Add Organism", "AddOrganism", m_mgrToolStripImages.ImageList, _
        '    '                                      m_mgrToolStripImages.GetImageIndex("AnimatGUI.AddOrganism.gif"), _
        '    '                                      New EventHandler(AddressOf Me.OnAddOrganism))
        '    '    Dim mcAddStructure As New MenuCommand("Add Structure", "AddStructure", m_mgrToolStripImages.ImageList, _
        '    '                                      m_mgrToolStripImages.GetImageIndex("AnimatGUI.AddStructure.gif"), _
        '    '                                      New EventHandler(AddressOf Me.OnAddStructure))
        '    '    Dim mcAddGround As New MenuCommand("Add Ground", "AddGround", m_mgrToolStripImages.ImageList, _
        '    '                                      m_mgrToolStripImages.GetImageIndex("AnimatGUI.AddGround.gif"), _
        '    '                                      New EventHandler(AddressOf Me.OnAddGround))
        '    '    Dim mcAddWater As New MenuCommand("Add Water", "AddWater", m_mgrToolStripImages.ImageList, _
        '    '                                      m_mgrToolStripImages.GetImageIndex("AnimatGUI.AddWater.gif"), _
        '    '                                      New EventHandler(AddressOf Me.OnAddWater))
        '    '    Dim mcAddDataTool As New MenuCommand("Add Data Tool", "AddDataTool", m_mgrToolStripImages.ImageList, _
        '    '                                      m_mgrToolStripImages.GetImageIndex("AnimatGUI.AddDataTool.gif"), _
        '    '                                      New EventHandler(AddressOf Me.OnAddDataTool))

        '    '    Dim mcProgramModule As New MenuCommand("Run Program Module", "ProgramModule", New EventHandler(AddressOf Me.OnRunProgramModule))

        '    '    mcEdit.MenuCommands.AddRange(New MenuCommand() {mcUndo, mcRedo, mcProgramModule, mcCutWorkspace, mcSep2, mcAddOrganism, mcAddStructure, mcAddGround, mcAddWater, mcAddDataTool})
        '    '    newMenu.MenuCommands.Add(mcEdit)

        '    '    'Lets create the view menu
        '    '    Dim mcView As New MenuCommand("View", "View")
        '    '    mcView.Description = "View Toolbar Properties"

        '    '    Dim mcpreference As New MenuCommand("Preferences", "Preferences", m_mgrToolStripImages.ImageList, _
        '    '                          m_mgrToolStripImages.GetImageIndex("AnimatGUI.preference.gif"), _
        '    '                          New EventHandler(AddressOf Me.OnPreference))

        '    '    Dim mcToggleSimualation As New MenuCommand("Toggle Simulation", "ToggleSimulation", m_mgrToolStripImages.ImageList, _
        '    '             m_mgrToolStripImages.GetImageIndex("AnimatGUI.Simulate.gif"), Shortcut.F5, _
        '    '             New EventHandler(AddressOf Me.OnToggleSimulation))

        '    '    mcView.MenuCommands.Add(mcToggleSimualation)
        '    '    mcView.MenuCommands.Add(mcpreference)

        '    '    If m_bProjectIsOpen Then
        '    '        mcSaveProject.Enabled = True
        '    '        mcSaveProjectAs.Enabled = True
        '    '        mcCloseProject.Enabled = True
        '    '        mcExportForStandAloneSim.Enabled = True
        '    '        mcToggleSimualation.Enabled = True
        '    '        mcUndo.Enabled = True
        '    '        mcRedo.Enabled = True
        '    '        mcProgramModule.Enabled = True
        '    '        mcCutWorkspace.Enabled = True
        '    '        mcAddOrganism.Enabled = True
        '    '        mcAddStructure.Enabled = True
        '    '        mcAddGround.Enabled = True
        '    '        mcAddWater.Enabled = True
        '    '        mcAddDataTool.Enabled = True
        '    '        mcpreference.Enabled = True
        '    '    Else
        '    '        mcSaveProject.Enabled = False
        '    '        mcSaveProjectAs.Enabled = False
        '    '        mcCloseProject.Enabled = False
        '    '        mcExportForStandAloneSim.Enabled = False
        '    '        mcToggleSimualation.Enabled = False
        '    '        mcUndo.Enabled = False
        '    '        mcRedo.Enabled = False
        '    '        mcProgramModule.Enabled = False
        '    '        mcCutWorkspace.Enabled = False
        '    '        mcAddOrganism.Enabled = False
        '    '        mcAddStructure.Enabled = False
        '    '        mcAddGround.Enabled = False
        '    '        mcAddWater.Enabled = False
        '    '        mcAddDataTool.Enabled = False
        '    '        mcpreference.Enabled = True
        '    '    End If

        '    '    For Each ctDock As Content In m_dockManager.Contents
        '    '        If ctDock.BackgroundForm Then
        '    '            Dim mcViewForm As New MenuCommand(ctDock.Title, ctDock.Title, ctDock.ImageList, _
        '    '                     ctDock.ImageIndex, New EventHandler(AddressOf Me.OnViewDockingForm))
        '    '            mcView.MenuCommands.Add(mcViewForm)
        '    '        End If
        '    '    Next

        '    '    newMenu.MenuCommands.Add(mcView)

        '    '    Dim mcHelp As New MenuCommand("Help", "Help")
        '    '    mcHelp.Description = "Help Commands"
        '    '    Dim mcContents As New MenuCommand("Contents", "Contents", m_mgrToolStripImages.ImageList, _
        '    '                                      m_mgrToolStripImages.GetImageIndex("AnimatGUI.HelpContents.gif"), _
        '    '                                      System.Windows.Forms.Shortcut.CtrlF1, New EventHandler(AddressOf Me.OnHelpContents))
        '    '    Dim mcSearch As New MenuCommand("Search", "Search", m_mgrToolStripImages.ImageList, _
        '    '                                      m_mgrToolStripImages.GetImageIndex("AnimatGUI.HelpSearch.gif"), _
        '    '                                      System.Windows.Forms.Shortcut.CtrlF3, New EventHandler(AddressOf Me.OnHelpSearch))
        '    '    Dim mcTechSupport As New MenuCommand("Technical Support", "TechnicalSupport", m_mgrToolStripImages.ImageList, _
        '    '                                      m_mgrToolStripImages.GetImageIndex("AnimatGUI.TechnicalSupport.gif"), New EventHandler(AddressOf Me.OnTechnicalSupport))
        '    '    Dim mcAbout As New MenuCommand("About AnimatLab", "AboutAnimatLab", New EventHandler(AddressOf Me.OnAboutAnimatLab))

        '    '    mcHelp.MenuCommands.AddRange(New MenuCommand() {mcContents, mcSearch, mcTechSupport, mcAbout})
        '    '    newMenu.MenuCommands.Add(mcHelp)

        '    '    m_aryDefaultMenus.Add(newMenu)

        '    '    Return newMenu
        'End Function

        'Public Overridable Sub AddItemToDefaultMenu(ByVal strMainMenu As String, _
        '                                            ByVal strSubMenu As String, _
        '                                            ByVal strMenuText As String, _
        '                                            ByVal strMenuTag As String, _
        '                                            ByVal strMenuImage As String, _
        '                                            ByRef menuHandler As EventHandler)

        '    Dim mcMain As MenuCommand, mcSubMenu As MenuCommand, mcNew As MenuCommand
        '    Dim iIndex As Integer
        '    For Each menuDefault As MenuControl In m_aryDefaultMenus
        '        'Now lets try and find the specified position where we want to add this menu command
        '        'First find the main menu item
        '        Try
        '            mcMain = menuDefault.MenuCommands(strMainMenu)
        '        Catch ex As System.Exception
        '            Throw New System.Exception("The main menu '" & strMainMenu & "' was not found while attempting to add " & _
        '                                       "the '" & strMenuText & "' menu command to the default menus.")
        '        End Try

        '        If strSubMenu.Trim.Length > 0 Then
        '            Try
        '                mcSubMenu = mcMain.MenuCommands(strSubMenu)
        '            Catch ex As System.Exception
        '                Throw New System.Exception("The main menu '" & strMainMenu & "' was not found while attempting to add " & _
        '                                           "the '" & strMenuText & "' menu command to the default menus.")
        '            End Try

        '            iIndex = mcMain.MenuCommands.IndexOf(mcSubMenu)
        '        Else
        '            iIndex = 0
        '        End If

        '        If strMenuImage.Trim.Length > 0 Then
        '            mcNew = New MenuCommand(strMenuText, strMenuTag, m_mgrToolStripImages.ImageList, _
        '                                    m_mgrToolStripImages.GetImageIndex(strMenuImage), menuHandler)

        '        Else
        '            mcNew = New MenuCommand(strMenuText, strMenuTag, menuHandler)
        '        End If

        '        mcMain.MenuCommands.Insert(iIndex, mcNew)
        '    Next

        'End Sub

        'Public Overridable Sub EnableDefaultMenuItem(ByVal strMainMenu As String, _
        '                                             ByVal strMenuItem As String, _
        '                                             ByVal bEnable As Boolean)

        '    Dim mcMain As MenuCommand
        '    For Each menuDefault As MenuControl In m_aryDefaultMenus
        '        'Now lets try and find the specified position where we want to add this menu command
        '        'First find the main menu item
        '        Try
        '            mcMain = menuDefault.MenuCommands(strMainMenu)
        '        Catch ex As System.Exception
        '            Throw New System.Exception("The main menu '" & strMainMenu & "' was not found while attempting to enable item.")
        '        End Try

        '        mcMain.MenuCommands(strMenuItem).Enabled = bEnable
        '    Next

        'End Sub

        'Public Overridable Sub EnableDefaultToolbarItem(ByVal strID As String, _
        '                                                ByVal bEnable As Boolean)

        '    For Each barMain As Crownwood.Magic.Toolbars.ToolbarControl In m_aryDefaultToolbars

        '        For Each btButton As ToolBarButton In barMain.Buttons
        '            If btButton.ToolTipText = strID Then
        '                btButton.Enabled = bEnable
        '            End If
        '        Next
        '    Next

        'End Sub

        'Public Overridable Function CreateDefaultToolbar(ByRef menuDefault As AnimatGuiCtrls.Controls.AnimatMenuStrip) As AnimatGuiCtrls.Controls.AnimatToolStrip
        '    'Dim newBar As New ToolbarControl

        '    'If menuDefault Is Nothing Then
        '    '    Throw New System.Exception("The default menu was not defined. " & _
        '    '      "You can not create a default toolbar without a reference to the menu control.")
        '    'End If

        '    'newBar.ImageList = m_mgrLargeImages.ImageList

        '    'Dim btnNewProject As New ToolBarButton
        '    'btnNewProject.ImageIndex = m_mgrLargeImages.GetImageIndex("AnimatGUI.NewProject.gif")
        '    'btnNewProject.ToolTipText = "New Project"

        '    'Dim btnOpenProject As New ToolBarButton
        '    'btnOpenProject.ImageIndex = m_mgrLargeImages.GetImageIndex("AnimatGUI.Open_Toolbar.gif")
        '    'btnOpenProject.ToolTipText = "Open Project"

        '    'Dim btnSaveProject As New ToolBarButton
        '    'btnSaveProject.ImageIndex = m_mgrLargeImages.GetImageIndex("AnimatGUI.Save_Toolbar.gif")
        '    'btnSaveProject.ToolTipText = "Save Project"

        '    'Dim btnCutWorkspace As New ToolBarButton
        '    'btnCutWorkspace.ImageIndex = m_mgrLargeImages.GetImageIndex("AnimatGUI.CutFromWorkspace.gif")
        '    'btnCutWorkspace.ToolTipText = "Cut Item from Workspace"

        '    'Dim btnAddOrganism As New ToolBarButton
        '    'btnAddOrganism.ImageIndex = m_mgrLargeImages.GetImageIndex("AnimatGUI.AddOrganism.gif")
        '    'btnAddOrganism.ToolTipText = "Add Organism"

        '    'Dim btnAddStructure As New ToolBarButton
        '    'btnAddStructure.ImageIndex = m_mgrLargeImages.GetImageIndex("AnimatGUI.AddStructure.gif")
        '    'btnAddStructure.ToolTipText = "Add Structure"

        '    'Dim btnAddGround As New ToolBarButton
        '    'btnAddGround.ImageIndex = m_mgrLargeImages.GetImageIndex("AnimatGUI.AddGround.gif")
        '    'btnAddGround.ToolTipText = "Add Ground"

        '    'Dim btnAddWater As New ToolBarButton
        '    'btnAddWater.ImageIndex = m_mgrLargeImages.GetImageIndex("AnimatGUI.AddWater.gif")
        '    'btnAddWater.ToolTipText = "Add Water"

        '    'Dim btnAddDataTool As New ToolBarButton
        '    'btnAddDataTool.ImageIndex = m_mgrLargeImages.GetImageIndex("AnimatGUI.AddDataTool.gif")
        '    'btnAddDataTool.ToolTipText = "Add Data Tool"

        '    'Dim btnPreference As New ToolBarButton
        '    'btnPreference.ImageIndex = m_mgrLargeImages.GetImageIndex("AnimatGUI.preference.gif")
        '    'btnPreference.ToolTipText = "Preferences"

        '    'If m_bProjectIsOpen Then
        '    '    btnSaveProject.Enabled = True
        '    '    btnCutWorkspace.Enabled = True
        '    '    btnAddOrganism.Enabled = True
        '    '    btnAddStructure.Enabled = True
        '    '    btnAddGround.Enabled = True
        '    '    btnAddWater.Enabled = True
        '    '    btnAddDataTool.Enabled = True
        '    'Else
        '    '    btnSaveProject.Enabled = False
        '    '    btnCutWorkspace.Enabled = False
        '    '    btnAddOrganism.Enabled = False
        '    '    btnAddStructure.Enabled = False
        '    '    btnAddGround.Enabled = False
        '    '    btnAddWater.Enabled = False
        '    '    btnAddDataTool.Enabled = False
        '    'End If

        '    'newBar.Appearance = ToolBarAppearance.Flat
        '    'newBar.Buttons.AddRange(New ToolBarButton() {btnNewProject, btnOpenProject, btnSaveProject, btnAddOrganism, btnAddStructure, btnAddGround, btnAddWater, btnAddDataTool, btnCutWorkspace})

        '    ''You can only set the menu items for the buttons AFTER you add the buttons to the toolbar!
        '    'newBar.ButtonManager.SetButtonMenuItem(btnNewProject, menuDefault.MenuCommands.FindMenuCommand("NewProject"))
        '    'newBar.ButtonManager.SetButtonMenuItem(btnOpenProject, menuDefault.MenuCommands.FindMenuCommand("OpenProject"))
        '    'newBar.ButtonManager.SetButtonMenuItem(btnSaveProject, menuDefault.MenuCommands.FindMenuCommand("SaveProject"))
        '    'newBar.ButtonManager.SetButtonMenuItem(btnCutWorkspace, menuDefault.MenuCommands.FindMenuCommand("CutWorkspace"))
        '    'newBar.ButtonManager.SetButtonMenuItem(btnAddOrganism, menuDefault.MenuCommands.FindMenuCommand("AddOrganism"))
        '    'newBar.ButtonManager.SetButtonMenuItem(btnAddStructure, menuDefault.MenuCommands.FindMenuCommand("AddStructure"))
        '    'newBar.ButtonManager.SetButtonMenuItem(btnAddGround, menuDefault.MenuCommands.FindMenuCommand("AddGround"))
        '    'newBar.ButtonManager.SetButtonMenuItem(btnAddWater, menuDefault.MenuCommands.FindMenuCommand("AddWater"))
        '    'newBar.ButtonManager.SetButtonMenuItem(btnAddDataTool, menuDefault.MenuCommands.FindMenuCommand("AddDataTool"))
        '    'newBar.ButtonManager.SetButtonMenuItem(btnPreference, menuDefault.MenuCommands.FindMenuCommand("Preferences"))
        '    ''newBar.ButtonManager.SetButtonMenuItem(btnSimulationWnd, menuDefault.MenuCommands.FindMenuCommand("SimulationWindow"))

        '    'newBar.DropDownArrows = True
        '    'newBar.Location = New System.Drawing.Point(0, 0)
        '    'newBar.Name = "DefaultToolbar"
        '    'newBar.ShowToolTips = True
        '    'newBar.Size = New System.Drawing.Size(292, 42)
        '    'newBar.TabIndex = 1

        '    'm_aryDefaultToolbars.Add(newBar)

        '    'Return newBar
        'End Function

#End Region

#Region " Project Creation "

        Public Overridable Sub ResetProject(ByVal bNewProject As Boolean)
            CloseProject(bNewProject)

            Dim afForm As AnimatForm

            m_wcWorkspaceContent = Nothing

            If bNewProject Then
                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                "AnimatGUI.Forms.Toolbox", _
                                "Toolbox", "Toolbox", True, , _
                                m_wcToolboxContent)
                m_frmToolbox = DirectCast(afForm, Forms.Toolbox)

                Dim cc As ContentCollection = New ContentCollection()
                cc.Add(m_frmToolbox.Content)
                m_dockManager.AutoHideContents(cc, m_wcToolboxContent.state, m_wcToolboxContent.CurrentContent)

                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                  "AnimatGUI.Forms.ProjectProperties", _
                                  "Properties", "Properties", True, _
                                  , m_wcPropertiesContent, , , , 200)
                m_frmProperties = DirectCast(afForm, Forms.ProjectProperties)

                'afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                '                "AnimatGUI.Forms.Toolbox", _
                '                "Toolbox", "Toolbox", True, , _
                '                Nothing, m_wcPropertiesContent)
                'm_frmToolbox = DirectCast(afForm, Forms.Toolbox)

                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                  "AnimatGUI.Forms.ProjectWorkspace", _
                                  "Workspace", "Workspace", True, _
                                  , m_wcWorkspaceContent, , m_wcPropertiesContent.ParentZone, , 200)
                m_frmWorkspace = DirectCast(afForm, Forms.ProjectWorkspace)

                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                "AnimatGUI.Forms.SimulationController", _
                                "Simulation Controller", "SimulationController", _
                                True, State.DockBottom, m_wcSimControllerContent, , , , , 100)
                m_frmSimulationController = DirectCast(afForm, Forms.SimulationController)

                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                "AnimatGUI.Forms.ReceptiveFieldPairs", _
                                "Receptive Field Pairs", "Field Pairs", _
                                True, State.DockRight, m_wcRecFieldPairsContent, , , , 200, )
                m_frmReceptiveFieldPairs = DirectCast(afForm, Forms.ReceptiveFieldPairs)

                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                "AnimatGUI.Forms.ReceptiveFieldGain", _
                                "Receptive Field Gain", "Field Gain", _
                                True, State.DockRight, m_wcRecFieldGainContent, m_wcRecFieldPairsContent, , , 200, )
                m_frmReceptiveFieldGain = DirectCast(afForm, Forms.ReceptiveFieldGain)

                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                "AnimatGUI.Forms.ReceptiveFieldCurrent", _
                                "Receptive Field Current", "Field Current", _
                                True, State.DockRight, m_wcRecFieldCurrentContent, m_wcRecFieldPairsContent, , , 200, )
                m_frmReceptiveFieldCurrent = DirectCast(afForm, Forms.ReceptiveFieldCurrent)

                m_dockManager.ToggleContentAutoHide(m_frmReceptiveFieldPairs.Content)

                m_frmWorkspace.CreateWorkspaceTreeView()
                PopulateToolbox()

                'm_dockManager.AutoHideWindow(m_wcWorkspaceContent)
            End If

            RaiseEvent ProjectClosed()

            Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Reset Project")
        End Sub


        'It is possible that if a project was saved with an earlier version of animatlab that a new toolbar may have been
        'added. If that is true it will not be part of the project config file. The docking bar created in ResetToolbars
        'will be deleted and it will not be recreated during the load. This method checks each of the major docking bars
        'after the load and makes sure they still exist. If it does not then it recreates it.
        Protected Sub VerifyToolbarsAfterLoad()

            Dim afForm As AnimatForm

            Dim ctDock As Content = m_dockManager.Contents("Workspace")
            If ctDock Is Nothing Then
                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                  "AnimatGUI.Forms.ProjectWorkspace", _
                                  "Workspace", "Workspace", True, _
                                  , m_wcWorkspaceContent, , , , 200)
                m_frmWorkspace = DirectCast(afForm, Forms.ProjectWorkspace)
            End If

            ctDock = m_dockManager.Contents("Properties")
            If ctDock Is Nothing Then
                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                  "AnimatGUI.Forms.ProjectProperties", _
                                  "Properties", "Properties", True, , _
                                  Nothing, Nothing)
                m_frmProperties = DirectCast(afForm, Forms.ProjectProperties)
            End If

            ctDock = m_dockManager.Contents("Toolbox")
            If ctDock Is Nothing Then
                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                  "AnimatGUI.Forms.Toolbox", _
                                  "Toolbox", "Toolbox", True, , _
                                  Nothing, Nothing)
                m_frmToolbox = DirectCast(afForm, Forms.Toolbox)
            End If

            ctDock = m_dockManager.Contents("Simulation Controller")
            If ctDock Is Nothing Then
                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                  "AnimatGUI.Forms.SimulationController", _
                                  "Simulation Controller", "SimulationController", _
                                  True, State.DockBottom, , , , , , 150)
                m_frmSimulationController = DirectCast(afForm, Forms.SimulationController)
            End If

            Dim ctRecFieldDock As Content = m_dockManager.Contents("Receptive Field Pairs")
            If ctRecFieldDock Is Nothing Then
                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                "AnimatGUI.Forms.ReceptiveFieldPairs", _
                                "Receptive Field Pairs", "Field Pairs", _
                                True, State.DockRight, m_wcRecFieldPairsContent, , , , 200, )
                m_frmReceptiveFieldPairs = DirectCast(afForm, Forms.ReceptiveFieldPairs)
                'm_dockManager.ToggleContentAutoHide(afForm.Content)
            End If

            ctDock = m_dockManager.Contents("Receptive Field Gain")
            If ctDock Is Nothing Then
                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                "AnimatGUI.Forms.ReceptiveFieldGain", _
                                "Receptive Field Gain", "Field Gain", _
                                True, State.DockRight, m_wcRecFieldGainContent, m_wcRecFieldPairsContent, , , 200, )
                m_frmReceptiveFieldGain = DirectCast(afForm, Forms.ReceptiveFieldGain)
            End If

            ctDock = m_dockManager.Contents("Receptive Field Current")
            If ctDock Is Nothing Then
                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                "AnimatGUI.Forms.ReceptiveFieldCurrent", _
                                "Receptive Field Current", "Field Current", _
                                True, State.DockRight, m_wcRecFieldCurrentContent, m_wcRecFieldPairsContent, , , 200, )
                m_frmReceptiveFieldCurrent = DirectCast(afForm, Forms.ReceptiveFieldCurrent)
            End If

            If ctRecFieldDock Is Nothing Then
                m_dockManager.ToggleContentAutoHide(m_frmReceptiveFieldPairs.Content)
            End If

            PopulateToolbox()
        End Sub

        Public Overridable Sub CloseProject(ByVal bOpeningProject As Boolean)
            If SaveIfDirty() = DialogResult.Cancel Then
                Return
            End If

            m_bProjectIsOpen = bOpeningProject

            If Not m_dockManager Is Nothing Then
                ClearDockingContents()
            End If

            ClearChildForms()

            'If we have a simulation up and running then completely shut it down and start over for the new project
            'This will need to be changed
            Me.SimulationInterface.ShutdownSimulation()

            CreateImageManager()

            ' Create the object that manages the docking state
            m_dockManager = New Crownwood.DotNetMagic.Docking.DockingManager(Me.AnimatStripContainer.ContentPanel, VisualStyle.Office2007Blue)
            m_dockManager.AllowFloating = False
            m_dockManager.OuterControl = Me.StatusBar

            m_frmWorkspace = Nothing
            m_frmToolbox = Nothing
            m_frmSimulationController = Nothing

            m_strPhysicsAssemblyName = "AnimatGUI.dll"
            m_strPhysicsClassName = "AnimatGUI.DataObjects.Simulation"
            m_doSimulation = New DataObjects.Simulation(Me.FormHelper)

            m_ModificationHistory = New AnimatGUI.Framework.UndoSystem.ModificationHistory

            UpdateToolstrips()

            Me.ClearIsDirty()

            Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Closed current project")
        End Sub

        Public Overridable Function SaveStandAlone(ByVal bSaveCharts As Boolean, ByVal bSaveStims As Boolean, ByVal bSaveChartsToFile As Boolean) As AnimatGUI.Interfaces.StdXml

            Try
                Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                Util.DisableDirtyFlags = True
                Util.ExportForStandAloneSim = True
                Util.ExportChartsToFile = bSaveChartsToFile
                Util.ExportChartsInStandAloneSim = bSaveCharts
                Util.ExportStimsInStandAloneSim = bSaveStims

                Dim oXml As New AnimatGUI.Interfaces.StdXml

                Util.Simulation.SaveSimulationXml(oXml, Nothing)

                Return oXml
            Catch ex As System.Exception
                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Unable to save project:")
                Throw ex
            Finally
                Util.DisableDirtyFlags = False
                Util.ExportForStandAloneSim = False
                Util.ExportChartsInStandAloneSim = False
                Util.ExportStimsInStandAloneSim = False
                Me.Cursor = System.Windows.Forms.Cursors.Arrow
            End Try

        End Function

        Public Overridable Sub SaveProject(ByVal strFilename As String)

            Try
                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Starting Save of project: '" & Util.Application.ProjectPath & "\" & strFilename & "'")

                If Not m_bProjectIsOpen Then
                    Throw New System.Exception("You must have an open project before you can save it.")
                End If

                Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                Dim oXml As New AnimatGUI.Interfaces.StdXml

                Me.ClearIsDirty()

                If Util.IsFullPath(strFilename) Then
                    Util.SplitPathAndFile(strFilename, m_strProjectPath, m_strProjectFile)
                End If

                If m_strProjectName.Trim.Length = 0 Then
                    m_strProjectName = m_strProjectFile.Substring(0, m_strProjectFile.Length - 6)
                End If

                Util.DisableDirtyFlags = True
                SaveData(oXml)
                oXml.Save(Util.GetFilePath(Util.Application.ProjectPath, strFilename))
                Util.DisableDirtyFlags = False

                RaiseEvent ProjectSaved()

                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Finished successful save of project: '" & Util.Application.ProjectPath & "\" & strFilename & "'")

            Catch ex As System.Exception
                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Unable to save project: '" & Util.Application.ProjectPath & "\" & strFilename & "'")
                Throw ex
            Finally
                Util.DisableDirtyFlags = False
                Me.Cursor = System.Windows.Forms.Cursors.Arrow
            End Try
        End Sub

        Public Overridable Sub LoadProject(ByVal strFilename As String)

            Try
                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Starting load of project: '" & strFilename & "'")

                Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                Dim oXml As New AnimatGUI.Interfaces.StdXml

                Dim strProjPath As String
                Dim strProjFile As String
                Util.SplitPathAndFile(strFilename, strProjPath, strProjFile)
                Me.ProjectPath = strProjPath
                Me.ProjectFile = strProjFile

                Directory.SetCurrentDirectory(m_strProjectPath)

                oXml.Load(strFilename)

                oXml.FindElement("Project")
                oXml.FindChildElement("")

                Util.DisableDirtyFlags = True
                LoadData(oXml)
                VerifyToolbarsAfterLoad()
                Util.DisableDirtyFlags = False

                Me.ClearIsDirty()

                Me.Title = Me.ProjectName & " Project"

                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Finished successful load of project: '" & strFilename & "'")

                UpdateToolstrips()
                RaiseEvent ProjectLoaded()

            Catch ex As System.Exception
                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Unable to load project: '" & strFilename & "'")
                Throw ex
            Finally
                Util.DisableDirtyFlags = False
                Me.Cursor = System.Windows.Forms.Cursors.Arrow
            End Try
        End Sub

        Protected Overridable Sub OnExportStandaloneSim(ByVal sender As Object, ByVal e As EventArgs) Handles ExportStandaloneToolStripMenuItem.Click

            Try
                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Starting Save of stand alone config file.")

                If Not m_bProjectIsOpen Then
                    Throw New System.Exception("You must have an open project before you can save it.")
                End If

                Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                Me.ClearIsDirty()

                Dim strFilename As String = Me.ProjectName & "_Standalone.asim"

                Util.DisableDirtyFlags = True
                Util.ExportForStandAloneSim = True
                Util.ExportChartsInStandAloneSim = True
                Util.ExportStimsInStandAloneSim = True

                Util.Simulation.SaveSimulationXml(strFilename)

                Util.DisableDirtyFlags = False

                MessageBox.Show("Standalone Control Files Created")

                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Finished successful save of standalone config file")

            Catch ex As System.Exception
                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Unable to save standalone config file.")
                Throw ex
            Finally
                Util.ExportForStandAloneSim = False
                Util.DisableDirtyFlags = False
                Util.ExportChartsInStandAloneSim = False
                Util.ExportStimsInStandAloneSim = False
                Me.Cursor = System.Windows.Forms.Cursors.Arrow
            End Try
        End Sub

        Public Overridable Sub ExportStandAloneSim(ByVal strProjectFile As String)


            Try
                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Starting Save of stand alone config file.")

                If Not m_bProjectIsOpen Then
                    Throw New System.Exception("You must have an open project before you can save it.")
                End If

                Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                Me.ClearIsDirty()

                Dim strFilename As String = strProjectFile

                Util.DisableDirtyFlags = True
                Util.ExportForStandAloneSim = True
                Util.ExportChartsInStandAloneSim = True
                Util.ExportStimsInStandAloneSim = True

                Util.Simulation.SaveSimulationXml(strFilename)

                Util.DisableDirtyFlags = False

                'MessageBox.Show("Standalone Control Files Created")

                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Finished successful save of standalone config file")

            Catch ex As System.Exception
                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Unable to save standalone config file.")
                Throw ex
            Finally
                Util.ExportForStandAloneSim = False
                Util.DisableDirtyFlags = False
                Util.ExportChartsInStandAloneSim = False
                Util.ExportStimsInStandAloneSim = False
                Me.Cursor = System.Windows.Forms.Cursors.Arrow
            End Try

        End Sub

        Public Overridable Sub ExportDataCharts(Optional ByVal strPrefix As String = "")

            Dim frmChart As Tools.DataChart
            For Each frmAnimat As AnimatForm In Me.ChildForms

                If Util.IsTypeOf(frmAnimat.GetType(), GetType(Tools.DataChart), False) Then
                    frmChart = DirectCast(frmAnimat, Tools.DataChart)
                    frmChart.ExportChartData(, strPrefix)
                End If
            Next

        End Sub

        Public Overridable Sub CompareExportedDataCharts(ByVal strPrefix As String, ByVal dblMaxError As Double)

            Dim frmChart As Tools.DataChart
            For Each frmAnimat As AnimatForm In Me.ChildForms

                If Util.IsTypeOf(frmAnimat.GetType(), GetType(Tools.DataChart), False) Then
                    frmChart = DirectCast(frmAnimat, Tools.DataChart)
                    frmChart.CompareExportedData(strPrefix, dblMaxError)
                End If
            Next

        End Sub

#End Region

#Region " Load/Save Methods "

        Public Overloads Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            CloseProject(True)

            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            m_strProjectName = oXml.GetChildString("ProjectName")
            m_strSimulationFile = oXml.GetChildString("SimulationFile", "")
            Dim eLogLevel As AnimatGUI.Interfaces.Logger.enumLogLevel = DirectCast([Enum].Parse(GetType(AnimatGUI.Interfaces.Logger.enumLogLevel), oXml.GetChildString("LogLevel", "None"), True), AnimatGUI.Interfaces.Logger.enumLogLevel)

            If eLogLevel <> Me.Logger.TraceLevel Then
                Me.Logger.TraceLevel = eLogLevel
            End If

            m_doSimulation = New DataObjects.Simulation(Me.FormHelper)
            If m_strSimulationFile.Trim.Length > 0 Then
                Try
                    m_doSimulation.LoadData(oXml)

                    'Now initialize after load
                    m_doSimulation.InitializeAfterLoad()

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End If

            'Start the simulation running
            Me.CreateSimulation(True)

            'Then create the forms. They will create their own sim references as they are loaded
            oXml.IntoChildElement("DockingForms") 'Into DockingForms Element
            Dim iCount As Integer = oXml.NumberOfChildren() - 1
            For iIndex As Integer = 0 To iCount
                oXml.FindChildByIndex(iIndex)
                LoadDockingForm(m_dockManager, oXml)
            Next
            oXml.OutOfElem()   'Outof DockingForms Element

            LoadDockingConfig(m_dockManager, oXml)

            Util.Simulation.NewToolHolderIndex = Util.ExtractIDCount("DataTool", Util.Simulation.ToolHolders)
        End Sub

        Public Overridable Sub LoadDockingConfig(ByRef dockManager As Crownwood.DotNetMagic.Docking.DockingManager, _
                                                 ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try
                Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                If dockManager.Contents.Count > 0 Then
                    If oXml.FindChildElement("DockingConfig", False) Then
                        Dim strDockXml As String = oXml.GetChildString("DockingConfig")

                        Dim aryBytes As Byte() = System.Text.Encoding.ASCII.GetBytes(strDockXml)
                        dockManager.LoadConfigFromArray(aryBytes)
                    End If
                End If

                If oXml.FindChildElement("TabbedGroupsConfig", False) Then
                    Dim strDockXml As String = oXml.GetChildString("TabbedGroupsConfig")

                    Dim aryBytes As Byte() = System.Text.Encoding.ASCII.GetBytes(strDockXml)
                    Me.AnimatTabbedGroups.LoadConfigFromArray(aryBytes)
                End If

                Application.DoEvents()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Sub LoadDockingForm(ByRef dockManager As DockingManager, _
                                               ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try
                Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                oXml.IntoElem()   'Into Form Element

                'Lets get the assembly file and class name
                Dim strFile As String = ToolsDirectory & oXml.GetChildString("AssemblyFile")
                Dim strClass As String = oXml.GetChildString("ClassName")
                Dim strTitle As String = oXml.GetChildString("Title")
                Dim strTabTitle As String = oXml.GetChildString("TabTitle", strTitle)
                Dim bBackgroundForm As Boolean = oXml.GetChildBool("BackgroundForm", False)
                oXml.OutOfElem()   'Outof Form Element

                Dim frmDock As AnimatForm = CreateDockingForm(dockManager, strFile, strClass, strTitle, strTabTitle, bBackgroundForm)

                If strClass = "AnimatGUI.Forms.ProjectWorkspace" Then
                    m_frmWorkspace = DirectCast(frmDock, Forms.ProjectWorkspace)
                ElseIf strClass = "AnimatGUI.Forms.ProjectProperties" Then
                    m_frmProperties = DirectCast(frmDock, Forms.ProjectProperties)
                ElseIf strClass = "AnimatGUI.Forms.Toolbox" Then
                    m_frmToolbox = DirectCast(frmDock, Forms.Toolbox)
                ElseIf strClass = "AnimatGUI.Forms.SimulationController" Then
                    m_frmSimulationController = DirectCast(frmDock, Forms.SimulationController)
                ElseIf strClass = "AnimatGUI.Forms.ReceptiveFieldPairs" Then
                    m_frmReceptiveFieldPairs = DirectCast(frmDock, Forms.ReceptiveFieldPairs)
                ElseIf strClass = "AnimatGUI.Forms.ReceptiveFieldGain" Then
                    m_frmReceptiveFieldGain = DirectCast(frmDock, Forms.ReceptiveFieldGain)
                ElseIf strClass = "AnimatGUI.Forms.ReceptiveFieldCurrent" Then
                    m_frmReceptiveFieldCurrent = DirectCast(frmDock, Forms.ReceptiveFieldCurrent)
                End If

                frmDock.LoadData(oXml)

                Application.DoEvents()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.AddElement("Project")

            If Me.SimulationFile.Trim.Length = 0 Then
                Me.SimulationFile = m_strProjectName & ".asim"
            End If

            oXml.AddChildElement("ProjectName", m_strProjectName)
            oXml.AddChildElement("SimulationFile", Me.SimulationFile)
            oXml.AddChildElement("LogLevel", Me.Logger.TraceLevel.ToString)

            m_doSimulation.SaveData(oXml)

            oXml.AddChildElement("DockingForms")
            oXml.IntoElem()   'Into DockingForms Element

            'First lets save all Docking Forms associated with this application.
            Dim frmAnimat As AnimatForm
            For Each conWindow As Content In m_dockManager.Contents
                frmAnimat = DirectCast(conWindow.Control, AnimatForm)
                frmAnimat.SaveData(oXml)
            Next
            oXml.OutOfElem()   'Outof DockingForms Element

            'oXml.AddChildElement("ChildForms")
            'oXml.IntoElem()   'Into ChildForms Element

            'For Each frmChild As AnimatForm In Me.ChildForms
            '    frmChild.SaveData(oXml)
            'Next
            'oXml.OutOfElem()   'Outof ChildForms Element

            oXml.OutOfElem()   'Outof Stimuli element

            oXml.OutOfElem()   'Outof Project Element

            SaveDockingConfig(m_dockManager, oXml)

        End Sub

        Public Overridable Sub SaveDockingConfig(ByRef dockManager As DockingManager, _
                                                 ByRef oXml As AnimatGUI.Interfaces.StdXml)
            'Save the docking manager configuration
            Dim ascii As Encoding = Encoding.ASCII
            Dim aryBytes As Byte() = dockManager.SaveConfigToArray(ascii)
            Dim strDockXml As String = System.Text.ASCIIEncoding.ASCII.GetString(aryBytes)
            oXml.AddChildElement("DockingConfig", strDockXml)

            'Now save the config for the tabbed groups.
            aryBytes = Me.AnimatTabbedGroups.SaveConfigToArray(ascii)
            strDockXml = System.Text.ASCIIEncoding.ASCII.GetString(aryBytes)
            oXml.AddChildElement("TabbedGroupsConfig", strDockXml)

        End Sub

        Protected Overridable Function SaveIfDirty(Optional ByVal e As System.ComponentModel.CancelEventArgs = Nothing) As DialogResult
            Dim eResult As System.Windows.Forms.DialogResult = DialogResult.OK

            'First check to see if the application is dirty. If it is then ask to save the project
            If Me.IsDirty Then
                eResult = MessageBox.Show("There are unsaved changes in the project. " & _
                                                                                    "Do you want to save them before you exit?", _
                                                                                    "Save Changes", MessageBoxButtons.YesNoCancel)
                If eResult = DialogResult.Cancel Then
                    If Not e Is Nothing Then e.Cancel = True
                    Return eResult
                ElseIf eResult = DialogResult.Yes Then
                    Me.SaveProject(Me.ProjectFile)
                Else
                    Me.ClearIsDirty()
                End If
            End If

            Return eResult
        End Function

#End Region

#Region " Child Form Management "

        Public Overridable Sub AddChildForm(ByVal frmChild As Forms.ExternalFileForm, Optional ByVal tabPage As Crownwood.DotNetMagic.Controls.TabPage = Nothing)

            m_mgrTabPagesImages.AddImage(frmChild.TabImageName, frmChild.TabImage)

            If Not tabPage Is Nothing Then
                frmChild.TabPage = tabPage
            Else
                frmChild.TabPage = New Crownwood.DotNetMagic.Controls.TabPage(frmChild.Title, frmChild)
                frmChild.TabPage.Selected = True
                Me.AnimatTabbedGroups.ActiveLeaf.TabPages.Add(frmChild.TabPage)
            End If

            frmChild.TabPage.ImageList = m_mgrTabPagesImages.ImageList
            frmChild.TabPage.ImageIndex = m_mgrTabPagesImages.GetImageIndex(frmChild.TabImageName)

            Me.SortedChildForms.Add(frmChild.ID, frmChild)
            Me.ChildForms.Add(frmChild)

        End Sub

        Public Overridable Sub RemoveChildForm(ByVal frmChild As Forms.ExternalFileForm)
            If Not frmChild.TabPage Is Nothing Then
                Dim leaf As TabGroupLeaf = Me.AnimatTabbedGroups.FirstLeaf()

                While Not leaf Is Nothing
                    If leaf.TabPages.Contains(frmChild.TabPage) Then
                        leaf.TabPages.Remove(frmChild.TabPage)
                        Return
                    End If
                    leaf = Me.AnimatTabbedGroups.NextLeaf(leaf)
                End While

            End If
        End Sub

        Public Overridable Sub RemoveBodyEditorForm(ByVal doStruct As DataObjects.Physical.PhysicalStructure)
            For Each frmAnimat As AnimatForm In Me.ChildForms
                If Util.IsTypeOf(frmAnimat.GetType, GetType(Forms.BodyPlan.Editor), False) Then
                    Dim frmEditor As Forms.BodyPlan.Editor = DirectCast(frmAnimat, Forms.BodyPlan.Editor)
                    If frmEditor.PhysicalStructure Is doStruct Then
                        RemoveChildForm(frmEditor)
                    End If
                End If
            Next

        End Sub

        Public Overridable Sub ClearChildForms()
            Me.AnimatTabbedGroups.RootSequence.Clear()
            Me.ChildForms.Clear()
            Me.SortedChildForms.Clear()
        End Sub

        Public Overridable Sub ClearDockingContents()
 
            Dim iCount As Integer = m_dockManager.Contents.Count - 1
            For iDock As Integer = 0 To iCount
                'We need to explicitly close the toolbar forms before removing from the docking manager.
                'This ensures that the proper Form.Closing/Form.Closed events are fired.
                Dim ctDock As Content = m_dockManager.Contents(0)
                If Not ctDock.Control Is Nothing AndAlso Util.IsTypeOf(ctDock.Control.GetType, GetType(Forms.AnimatForm)) Then
                    Dim afForm As AnimatForm = DirectCast(ctDock.Control, AnimatForm)
                    afForm.Close()
                End If

                m_dockManager.Contents.RemoveAt(0)
            Next
        End Sub

#End Region

#Region " Docking Form Management "

        Public Overridable Function CreateDockingForm(ByRef dockManager As DockingManager, _
                                                      ByVal strAssemblyFile As String, _
                                                      ByVal strClassname As String, _
                                                      ByVal strPageTitle As String, _
                                                      Optional ByVal strTabTitle As String = "", _
                                                      Optional ByVal bBackgroundForm As Boolean = False, _
                                                      Optional ByVal eState As Crownwood.DotNetMagic.Docking.State = State.DockLeft, _
                                                      Optional ByRef wcCreatedWindow As Crownwood.DotNetMagic.Docking.WindowContent = Nothing, _
                                                      Optional ByRef wcAddToWindow As Crownwood.DotNetMagic.Docking.WindowContent = Nothing, _
                                                      Optional ByRef wzAddToZone As Crownwood.DotNetMagic.Docking.Zone = Nothing, _
                                                      Optional ByVal iZoneIndex As Integer = 0, _
                                                      Optional ByVal iDisplaySizeX As Integer = 150, _
                                                      Optional ByVal iDisplaySizeY As Integer = 150) As AnimatForm

            'First lets make sure that there is not already a docking window with this title.
            'Title is a unique id used by the dockmanager.
            Dim ctFound As Content = m_dockManager.Contents.Item(strPageTitle)

            If ctFound Is Nothing Then
                Dim frmDock As AnimatForm = CreateForm(strAssemblyFile, strClassname, strPageTitle)

                InitDockingForm(dockManager, frmDock, strPageTitle, strTabTitle, bBackgroundForm, _
                                eState, wcCreatedWindow, wcAddToWindow, wzAddToZone, iZoneIndex, _
                                iDisplaySizeX, iDisplaySizeY)

                Return frmDock
            Else
                Throw New System.Exception("You can not insert docking forms with duplicate titles. " & _
                                           "A docking form with the title of '" & strPageTitle & "'.")
            End If

        End Function

        Public Overridable Function AddDockingForm(ByRef dockManager As DockingManager, _
                                                      ByVal frmDock As AnimatForm, _
                                                      ByVal strPageTitle As String, _
                                                      Optional ByVal strTabTitle As String = "", _
                                                      Optional ByVal bBackgroundForm As Boolean = False, _
                                                      Optional ByVal eState As Crownwood.DotNetMagic.Docking.State = State.DockLeft, _
                                                      Optional ByRef wcCreatedWindow As Crownwood.DotNetMagic.Docking.WindowContent = Nothing, _
                                                      Optional ByRef wcAddToWindow As Crownwood.DotNetMagic.Docking.WindowContent = Nothing, _
                                                      Optional ByRef wzAddToZone As Crownwood.DotNetMagic.Docking.Zone = Nothing, _
                                                      Optional ByVal iZoneIndex As Integer = 0, _
                                                      Optional ByVal iDisplaySizeX As Integer = 150, _
                                                      Optional ByVal iDisplaySizeY As Integer = 150) As AnimatForm

            'First lets make sure that there is not already a docking window with this title.
            'Title is a unique id used by the dockmanager.
            Dim ctFound As Content = m_dockManager.Contents.Item(strPageTitle)

            If ctFound Is Nothing Then
                frmDock.Initialize()
                frmDock.Title = strPageTitle

                frmDock.OnBeforeFormAdded()

                InitDockingForm(dockManager, frmDock, strPageTitle, strTabTitle, bBackgroundForm, _
                                eState, wcCreatedWindow, wcAddToWindow, wzAddToZone, iZoneIndex, _
                                iDisplaySizeX, iDisplaySizeY)

                frmDock.OnAfterFormAdded()

                Return frmDock
            Else
                Throw New System.Exception("You can not insert docking forms with duplicate titles. " & _
                                           "A docking form with the title of '" & strPageTitle & "'.")
            End If

        End Function

        Protected Sub InitDockingForm(ByRef dockManager As DockingManager, _
                                                      ByVal frmDock As AnimatForm, _
                                                      ByVal strPageTitle As String, _
                                                      Optional ByVal strTabTitle As String = "", _
                                                      Optional ByVal bBackgroundForm As Boolean = False, _
                                                      Optional ByVal eState As Crownwood.DotNetMagic.Docking.State = State.DockLeft, _
                                                      Optional ByRef wcCreatedWindow As Crownwood.DotNetMagic.Docking.WindowContent = Nothing, _
                                                      Optional ByRef wcAddToWindow As Crownwood.DotNetMagic.Docking.WindowContent = Nothing, _
                                                      Optional ByRef wzAddToZone As Crownwood.DotNetMagic.Docking.Zone = Nothing, _
                                                      Optional ByVal iZoneIndex As Integer = 0, _
                                                      Optional ByVal iDisplaySizeX As Integer = 150, _
                                                      Optional ByVal iDisplaySizeY As Integer = 150)
            Dim iImageIndex As Integer = m_mgrToolStripImages.FindImageName(frmDock.IconName)
            If iImageIndex > -1 Then
                frmDock.Content = New AnimatGuiCtrls.Docking.AnimatContent(dockManager, frmDock, frmDock.Title, m_mgrToolStripImages.ImageList, iImageIndex)
                dockManager.Contents.Add(frmDock.Content)
            Else
                frmDock.Content = New AnimatGuiCtrls.Docking.AnimatContent(dockManager, frmDock, frmDock.Title)
                dockManager.Contents.Add(frmDock.Content)
            End If
            frmDock.Content.BackgroundForm = bBackgroundForm
            frmDock.Content.UserData = frmDock
            frmDock.Content.CloseButton = False

            frmDock.Content.DisplaySize = New System.Drawing.Size(iDisplaySizeX, iDisplaySizeY)
            frmDock.Content.AutoHideSize = New System.Drawing.Size(iDisplaySizeX, iDisplaySizeY)

            'If strTabTitle.Trim.Length <> 0 Then frmDock.Content.TabTitle = strTabTitle

            If Not wcAddToWindow Is Nothing Then
                wcCreatedWindow = dockManager.AddContentToWindowContent(frmDock.Content, wcAddToWindow)
            ElseIf Not wzAddToZone Is Nothing Then
                dockManager.AddContentToZone(frmDock.Content, wzAddToZone, iZoneIndex)
            Else
                wcCreatedWindow = dockManager.AddContentWithState(frmDock.Content, eState)
            End If

            If bBackgroundForm Then
                'AddItemToDefaultMenu("View", "", frmDock.Text, frmDock.Text, _
                '                     frmDock.IconName, New EventHandler(AddressOf Me.OnViewDockingForm))
            End If

            dockManager.ShowContent(frmDock.Content)

        End Sub

        Public Overridable Function CreateForm(ByVal strAssemblyFile As String, _
                                               ByVal strClassname As String, _
                                               ByVal strTitle As String, _
                                               Optional ByVal bInitialize As Boolean = True) As AnimatForm

            Dim oAssembly As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(Util.GetFilePath(Me.ApplicationDirectory, strAssemblyFile))
            Dim frmAnimat As AnimatForm = DirectCast(oAssembly.CreateInstance(strClassname), AnimatForm)

            If frmAnimat Is Nothing Then
                Throw New System.Exception("The system was unable to create the form '" & strClassname & _
                                           "' from the assembly '" & strAssemblyFile & "'.")
            End If

            If bInitialize Then
                frmAnimat.Initialize(Util.Application)
            End If
            frmAnimat.Title = strTitle

            Dim iImageIndex As Integer = -1
            If frmAnimat.IconName.Trim.Length > 0 Then
                If m_mgrToolStripImages.AddImage(oAssembly, frmAnimat.IconName) Then
                    iImageIndex = m_mgrToolStripImages.GetImageIndex(frmAnimat.IconName)
                End If
            End If

            Return frmAnimat
        End Function

        Public Overridable Sub RemoveDockingForm(ByRef dockManager As DockingManager, _
                                                 ByVal strTitle As String, _
                                                 Optional ByVal bThrowError As Boolean = True)
            Dim ctContent As Content = dockManager.Contents(strTitle)
            If Not ctContent Is Nothing Then
                dockManager.Contents.Remove(ctContent)
            ElseIf bThrowError Then
                Throw New System.Exception("No docking form with the title '" & strTitle & "' was found to remove.")
            End If
        End Sub

        Public Overridable Sub RemoveDockingForm(ByRef dockManager As DockingManager, _
                                                 ByRef ctContent As Content, _
                                                 Optional ByVal bThrowError As Boolean = True)
            If Not ctContent Is Nothing Then
                dockManager.Contents.Remove(ctContent)
            ElseIf bThrowError Then
                Throw New System.Exception("The docking form to remove was not defined.")
            End If

        End Sub

        Public Overridable Sub CloseForm(ByVal frmAnimat As Forms.ExternalFileForm, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing, Optional ByVal bThrowError As Boolean = True)
            If frmAnimat Is Nothing OrElse frmAnimat.TabPage Is Nothing Then Return

            'We need to loop through all the leafs in the tabbed groups and then check the tabpages in each one
            'to find the correct place to delete the tab page for this animatform. I originally wnated to
            'just keep a reference to the parent leaf object, but the problem with that is that the user
            'can dynamically change it. I also do not see any public reference from a tabpage back to its
            'parent leaf, so the only option I saw was to loop through them all.
            Dim leaf As TabGroupLeaf = Me.AnimatTabbedGroups.FirstLeaf()

            Dim bFound As Boolean = False
            While Not leaf Is Nothing AndAlso Not bFound
                If leaf.TabPages.Contains(frmAnimat.TabPage) Then
                    'Only remove the tab page if we are doing it programatically.
                    'if there is an event arg then the user has hit the close button.
                    'let the tabbed groups code get rid of the tabpage in that case.
                    If e Is Nothing Then leaf.TabPages.Remove(frmAnimat.TabPage)
                    frmAnimat.Close()

                    bFound = True
                End If

                leaf = Me.AnimatTabbedGroups.NextLeaf(leaf)
            End While

            If Not bFound AndAlso bThrowError Then
                Throw New System.Exception("No leaf was found for form '" & frmAnimat.Title & "'")
            End If

            frmAnimat.RemoveWorksapceTreeView()
            If Me.ChildForms.Contains(frmAnimat) Then Me.ChildForms.Remove(frmAnimat)
            If Me.SortedChildForms.Contains(frmAnimat.ID) Then Me.SortedChildForms.Remove(frmAnimat.ID)

        End Sub

#End Region

#Region " Splash Screen Methods "

        Protected Sub ShowSplashScreen()
            Try

                '#If Not Debug Then
                '                Dim imgSplash As Image = ImageManager.LoadImage("AnimatGUI", "AnimatGUI.Splash.jpg")

                '                If Not imgSplash Is Nothing AndAlso TypeOf imgSplash Is Bitmap Then
                '                    Dim bmpSplash As Bitmap = DirectCast(imgSplash, Bitmap)
                '                    Crownwood.Magic.Forms.SplashForm.StartSplash(bmpSplash, Color.FromArgb(64, 0, 63))
                '                End If
                '#End If

            Catch ex As System.Exception

            End Try
        End Sub

        Protected Sub CloseSplashScreen()
            Try

#If Not Debug Then
#End If
            Catch ex As System.Exception

            End Try
        End Sub

#End Region

#Region " Windows Management "


        Public Overridable Function EditBehavioralSystem(ByVal doOrganism As DataObjects.Physical.Organism) As AnimatForm

            Try
                ''First lets verify that there is not already an open window for this organism.
                'Dim frmEditor As Forms.Behavior.Editor
                'For Each oChild As Form In Util.Application.ChildForms
                '    If TypeOf oChild Is Forms.Behavior.Editor Then
                '        frmEditor = DirectCast(oChild, Forms.Behavior.Editor)
                '        If frmEditor.Organism Is doOrganism Then
                '            frmEditor.MakeVisible()
                '            Return frmEditor
                '        End If
                '    End If
                'Next

                'Dim frmMdi As New AnimatGUI.Forms.Behavior.Editor
                'Dim frmBase As AnimatForm

                'Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                'frmMdi.Organism = doOrganism
                'frmMdi.Initialize(Me, frmBase)
                'frmMdi.Title = "Edit " & doOrganism.Name

                'If System.IO.File.Exists(Util.GetFilePath(Util.Application.ProjectPath, doOrganism.BehavioralEditorFile)) Then
                '    frmMdi.LoadEditorFile(doOrganism.BehavioralEditorFile)
                'End If

                'frmMdi.ShowAnimatForm()

                'doOrganism.BehaviorEditor = frmMdi

                'Me.Cursor = System.Windows.Forms.Cursors.Arrow

                'Return frmMdi
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.Cursor = System.Windows.Forms.Cursors.Arrow
            End Try

        End Function

        Public Overridable Sub EditBodyPlan(ByVal doStructure As DataObjects.Physical.PhysicalStructure)

            Try
                'First check to see if the screen is alread open. If it is then just make sure it is visible.
                If Not doStructure.BodyEditor Is Nothing Then
                    doStructure.BodyEditor.MakeVisible()
                    Return
                End If

                'If there is not already an open window then lets create it.
                Dim frmAnimat As Forms.BodyPlan.Editor = DirectCast(CreateForm("AnimatGUI.dll", "AnimatGUI.Forms.BodyPlan.Editor", doStructure.Name & " Body", False), Forms.BodyPlan.Editor)
                doStructure.BodyEditor = DirectCast(frmAnimat, Forms.BodyPlan.Editor)
                frmAnimat.PhysicalStructure = doStructure
                frmAnimat.Initialize(Me)
                frmAnimat.LoadExternalFile(frmAnimat.ExternalFilename)

                AddChildForm(frmAnimat)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.Cursor = System.Windows.Forms.Cursors.Arrow
            End Try

        End Sub


        Public Overridable Function EditEnvironment() As AnimatForm

            Try

                'Dim frmMdi As Form = DirectCast(Util.LoadClass("VortexOsgAnimatTools", "VortexOsgAnimatTools.Forms.SimTest"), Form)
                'frmMdi.MdiParent = Me
                'frmMdi.Show()

                'Me.Cursor = System.Windows.Forms.Cursors.Arrow

                'Return Nothing
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.Cursor = System.Windows.Forms.Cursors.Arrow
            End Try

        End Function

        Public Overridable Sub DisplayToolViewer(ByVal doTool As DataObjects.ToolHolder)

            Try
                'If the tool already has a window open then just make it visible
                If Not doTool Is Nothing AndAlso Not doTool.ToolForm Is Nothing Then
                    doTool.ToolForm.MakeVisible()
                    Return
                End If

                'If there is not already an open window then lets create it.
                Dim frmAnimat As Forms.ExternalFileForm = DirectCast(CreateForm(doTool.BaseAssemblyFile, doTool.BaseClassName, doTool.Name, False), Forms.ExternalFileForm)
                frmAnimat.Initialize(Me)
                frmAnimat.LoadExternalFile(frmAnimat.ExternalFilename)

                AddChildForm(frmAnimat)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.Cursor = System.Windows.Forms.Cursors.Arrow
            End Try

        End Sub


        Public Overrides Sub UndoRedoRefresh(ByVal doRefresh As AnimatGUI.Framework.DataObject)

            If Not doRefresh Is Nothing Then
                doRefresh.SelectItem()
            End If

        End Sub

#End Region

#Region " Simulation Control "


        Public Sub ToggleSimulation()

            Try
                Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                Me.Simulation.SimulationAtEndTime = False

                'If the simulation is not started then go ahead and get it going.
                If Not Me.SimulationInterface.SimOpen Then
                    Me.CreateSimulation(False)
                End If

                If Me.SimulationInterface.Paused Then
                    If Me.SimulationInterface.CurrentMillisecond <= 0 Then
                        RaiseEvent SimulationStarting()
                    Else
                        RaiseEvent SimulationResuming()
                    End If

                    Me.SimulationInterface.ReInitializeSimulation()
                    Me.SimulationInterface.StartSimulation()

                    RaiseEvent SimulationStarted()

                    Me.RunSimulationToolStripMenuItem.Text = "Run Simulation"
                Else
                    Me.SimulationInterface.PauseSimulation()
                    RaiseEvent SimulationPaused()

                    Me.RunSimulationToolStripMenuItem.Text = "Pause Simulation"
                End If

                Util.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Simulation was started or resumed.")

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.Cursor = System.Windows.Forms.Cursors.Default
            End Try

        End Sub

        Public Sub StopSimulation()

            Try
                Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                Me.SimulationInterface.StopSimulation()
                RaiseEvent SimulationStopped()

                Util.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Simulation was stopped.")

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.Cursor = System.Windows.Forms.Cursors.Default
            End Try

        End Sub

        Public Overridable Sub CreateSimulation(ByVal bPaused As Boolean)
            Try
                Me.SimulationInterface.CreateAndRunSimulation(bPaused)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


#End Region

#End Region

#Region " Application Events "

        Public Event ProjectLoaded()
        Public Event ProjectSaved()
        Public Event ProjectClosed()
        Public Event ProjectCreated()
        Public Event ApplicationExiting()
        Public Event SimulationStarting()
        Public Event SimulationResuming()
        Public Event SimulationStarted()
        Public Event SimulationPaused()
        Public Event SimulationStopped()
        Public Event UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                  ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                  ByVal fltMassChange As Single, _
                                  ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                  ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                  ByVal fltDistanceChange As Single)

#End Region

#Region " Event Handlers "

        Protected Overridable Sub OnNewProject(ByVal sender As Object, ByVal e As EventArgs) Handles NewToolStripMenuItem.Click, NewToolStripButton.Click

            Try
                If SaveIfDirty() = DialogResult.Cancel Then
                    Return
                End If

                Dim frmNewProject As New Forms.NewProject

                frmNewProject.txtProjectName.Text = "NewProject"
                If frmNewProject.ShowDialog = DialogResult.OK Then
                    m_doSimulation = New DataObjects.Simulation(Me.FormHelper)
                    Util.Application.ProjectPath = frmNewProject.txtLocation.Text & "\" & frmNewProject.txtProjectName.Text
                    Util.Application.ProjectName = frmNewProject.txtProjectName.Text
                    Util.Application.ProjectFile = Util.Application.ProjectName & ".aproj"
                    Util.Application.SimulationFile = Util.Application.ProjectName & ".asim"
                    Me.Title = Me.ProjectName & " Project"

                    Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Creating a new Project: '" & Util.Application.ProjectPath & "\" & Util.Application.ProjectFile)

                    Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                    'Create the project directory
                    System.IO.Directory.CreateDirectory(Util.Application.ProjectPath)

                    Util.DisableDirtyFlags = True
                    ResetProject(True)
                    Util.DisableDirtyFlags = False

                    SaveProject(Util.Application.ProjectFile)

                    RaiseEvent ProjectCreated()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.Cursor = System.Windows.Forms.Cursors.Arrow
                Util.DisableDirtyFlags = False
            End Try
        End Sub

        Protected Overridable Sub OnOpenProject(ByVal sender As Object, ByVal e As EventArgs) Handles OpenToolStripMenuItem.Click, OpenToolStripButton.Click

            Try
                If SaveIfDirty() = DialogResult.Cancel Then
                    Return
                End If

                Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                Dim openFileDialog As New OpenFileDialog
                openFileDialog.Filter = "AnimatLab Project|*.aproj"
                openFileDialog.Title = "Open an AnimatLab Project"

                If openFileDialog.ShowDialog() = DialogResult.OK Then
                    LoadProject(openFileDialog.FileName)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.Cursor = System.Windows.Forms.Cursors.Arrow
            End Try
        End Sub

        Protected Overridable Sub OnSaveProject(ByVal sender As Object, ByVal e As EventArgs) Handles SaveToolStripMenuItem.Click, SaveToolStripButton.Click

            Try
                SaveProject(Me.ProjectFile)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnSaveProjectAs(ByVal sender As Object, ByVal e As EventArgs) Handles SaveAsToolStripMenuItem.Click

            Try
                If Not m_bProjectIsOpen Then
                    Throw New System.Exception("You must have an open project before you can save it.")
                End If

                Dim frmSave As New Forms.NewProject
                frmSave.txtProjectName.Text = Me.ProjectName
                frmSave.txtLocation.Text = Util.RemoveStringSections(Me.ProjectPath, "\", 1)
                frmSave.Text = "Save Project As .."
                frmSave.Size = New Size(318, 168)

                If frmSave.ShowDialog = DialogResult.OK Then
                    'Copy the current project directory
                    Dim strNewPath As String = frmSave.txtLocation.Text & "\" & frmSave.txtProjectName.Text
                    Dim strNewProjFile As String = frmSave.txtProjectName.Text & ".aproj"
                    Dim strNewSimFile As String = frmSave.txtProjectName.Text & ".asim"

                    strNewPath = strNewPath.Replace("\\", "\")

                    Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Saving project: '" & Util.Application.ProjectPath & "\" & Util.Application.ProjectFile & "' as '" & strNewPath)

                    Util.CopyDirectory(Util.Application.ProjectPath, strNewPath)

                    'Rename the project files in the new folder.
                    If File.Exists(strNewPath & "\" & Util.Application.ProjectFile) Then File.Delete(strNewPath & "\" & Util.Application.ProjectFile)
                    If File.Exists(strNewPath & "\" & Util.Application.SimulationFile) Then File.Delete(strNewPath & "\" & Util.Application.SimulationFile)

                    Util.Application.ProjectPath = frmSave.txtLocation.Text & "\" & frmSave.txtProjectName.Text & "\"
                    Util.Application.ProjectName = frmSave.txtProjectName.Text
                    Util.Application.ProjectFile = Util.Application.ProjectName & ".aproj"
                    Util.Application.SimulationFile = Util.Application.ProjectName & ".asim"
                    Me.Title = Me.ProjectName & " Project"

                    Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                    SaveProject(Util.Application.ProjectFile)

                    RaiseEvent ProjectSaved()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.Cursor = System.Windows.Forms.Cursors.Default
            End Try
        End Sub

        Protected Overridable Sub OnCloseProject(ByVal sender As Object, ByVal e As EventArgs) Handles CloseProjectToolStripMenuItem.Click

            Try
                ResetProject(False)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnExit(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click

            Try
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnCreateSimulation(ByRef strXml As String)
            Dim oXml As AnimatGUI.Interfaces.StdXml = Util.Application.SaveStandAlone(False, False, False)
            strXml = oXml.Serialize()
        End Sub

        Private Delegate Sub OnSimulationRunningDelegate()

        Protected Overridable Sub OnSimulationRunning()
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New OnSimulationRunningDelegate(AddressOf OnSimulationRunning), Nothing)
                    Return
                End If

                'When the simulation is created we need to initialize all sim refereces.
                Me.Simulation.InitializeSimulationReferences()
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        ''' \brief  Event that is fired when the simulation signals that it needs to be shut down.
        ''' 		
        ''' \details This event only sets the SimulationAtEndTime flag to true, and then exits. This is
        ''' 		 so the simulation processing loop can continue running. We later check within the
        ''' 		 SimulationController timer code whether this flag is set. If it is then we stop the
        ''' 		 simulation there and reset the flag.
        '''
        ''' \author dcofer
        ''' \date   3/26/2011
        Protected Overridable Sub OnNeedToStopSimulation()
            Try
                Util.Simulation.SimulationAtEndTime = True
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        ''' \brief  Called when the simulation starting event is called.
        ''' 		
        ''' \details This is an event handler for the AnimatLab application. When the SimulationStarting event is fired
        ''' 		 this is called. Override this method if you need to know this information in your test.
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Protected Overridable Sub OnSimulationStarting()
            'We need to set the visualselection modes so that only the simulation mode is available.
            Util.Simulation.SetVisualSelectionModeForSimStarting(True)
        End Sub

        ''' \brief  Called when the simulation started event is called.
        ''' 		
        ''' \details This is an event handler for the AnimatLab application. When the SimulationStarted event is fired
        ''' 		 this is called. Override this method if you need to know this information in your test.
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Protected Overridable Sub OnSimulationStarted()

        End Sub

        ''' \brief  Called when the simulation resuming event is called.
        ''' 		
        ''' \details This is an event handler for the AnimatLab application. When the SimulationResuming event is fired
        ''' 		 this is called. Override this method if you need to know this information in your test.
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Protected Overridable Sub OnSimulationResuming()
            'We need to set the visualselection modes so that only the simulation mode is available.
            Util.Simulation.SetVisualSelectionModeForSimStarting(True)
        End Sub

        ''' \brief  Called when the simulation pausing event is called.
        ''' 		
        ''' \details This is an event handler for the AnimatLab application. When the SimulationPausing event is fired
        ''' 		 this is called. Override this method if you need to know this information in your test.
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Protected Overridable Sub OnSimulationPaused()
            'We need to release the other visualselection modes.
            Util.Simulation.SetVisualSelectionModeForSimStarting(False)
        End Sub

        ''' \brief  Called when the simulation stopped event is called.
        ''' 		
        ''' \details This is an event handler for the AnimatLab application. When the SimulationStopped event is fired
        ''' 		 this is called Override this method if you need to know this information in your test, but be sure
        ''' 		 to call the base class method.
        '''
        ''' \author dcofer
        ''' \date   3/16/2011
        Protected Overridable Sub OnSimulationStopped()
            'We need to release the other visualselection modes.
            Util.Simulation.SetVisualSelectionModeForSimStarting(False)
        End Sub

        Protected Overridable Sub OnPreferences(ByVal sender As Object, ByVal e As EventArgs) Handles PreferencesToolStripMenuItem.Click

            Try
                Dim frmPreference As New AnimatGUI.Forms.Tools.Preference

                frmPreference.LoadPreferences()
                frmPreference.ShowDialog()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnToggleSimulation(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunSimulationToolStripMenuItem.Click

            Try
                Me.ToggleSimulation()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnUndo(ByVal sender As Object, ByVal e As System.EventArgs) Handles UndoToolStripMenuItem.Click

            Try
                Me.ModificationHistory.Undo()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnRedo(ByVal sender As Object, ByVal e As System.EventArgs) Handles RedoToolStripMenuItem.Click

            Try
                Me.ModificationHistory.Redo()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnRunMacro(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim frmProgramModules As New Forms.SelectProgramModule

                If frmProgramModules.ShowDialog() = DialogResult.OK AndAlso Not frmProgramModules.SelectedModule Is Nothing Then
                    frmProgramModules.SelectedModule.ShowDialog()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnAddOrganism(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddOrganismToolStripMenuItem.Click, AddOrganismStripButton.Click

            Try
                Util.Environment.AddOrganism()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnAddStructure(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddStructureToolStripMenuItem.Click, AddStructureToolStripButton.Click

            Try
                Util.Environment.AddStructure()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnAddDataTool(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddTooToolStripMenuItem.Click, AddToolToolStripButton.Click

            Try
                Dim frmSelectTool As New Forms.Tools.SelectToolType

                If frmSelectTool.ShowDialog() = DialogResult.OK Then
                    Me.AddNewTool(frmSelectTool.SelectedToolType)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnHelpContents(ByVal sender As Object, ByVal e As System.EventArgs) Handles HelpToolStripMenuItem.Click, HelpToolStripButton.Click
            Try
                Help.ShowHelp(Me, "http:\\www.animatlab.com\Help.htm")
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnHelpSearch(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchToolStripMenuItem.Click
            Try
                Help.ShowHelp(Me, "http:\\www.animatlab.com\Search.htm")
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnHelpIndex(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IndexToolStripMenuItem.Click
            Try
                Help.ShowHelp(Me, "http:\\www.animatlab.com\sitemap.htm")
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnTechnicalSupport(ByVal sender As Object, ByVal e As System.EventArgs) Handles SupportToolStripMenuItem.Click
            Try
                Help.ShowHelp(Me, "http:\\www.animatlab.com\Contact.htm")
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnAboutAnimatLab(ByVal sender As Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
            Try
                Dim frmAbout As New Forms.About
                frmAbout.ShowDialog()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        'This is the general event that is called when someone tries to delete something in the 
        'project workspace
        Public Sub OnDeleteFromWorkspace(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteToolStripButton.Click, DeleteToolStripMenuItem.Click
            Try
                If Not Util.ProjectWorkspace.SelectedDataObject Is Nothing Then
                    Util.ProjectWorkspace.SelectedDataObject.Delete()
                ElseIf Not Util.ProjectWorkspace.SelectedAnimatform Is Nothing Then
                    Util.ProjectWorkspace.SelectedAnimatform.Delete()
                Else
                    Throw New System.Exception("The selected object cannot be deleted")
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Sub AddNewTool(ByVal frmTool As Forms.Tools.ToolForm)

            Try
                'Now lets create a new viewer window for the tool they double clicked.
                If Not frmTool Is Nothing Then
                    Util.Simulation.NewToolHolderIndex = Util.Simulation.NewToolHolderIndex + 1
                    Dim strName As String = "DataTool_" & Util.Simulation.NewToolHolderIndex

                    Dim frmBase As Forms.Tools.ToolForm = DirectCast(frmTool.Clone(), Forms.Tools.ToolForm)

                    Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

                    frmBase.Title = strName
                    frmBase.Initialize(Me)
                    Util.Application.AddChildForm(frmBase)

                    Dim doHolder As New DataObjects.ToolHolder(m_doFormHelper)
                    frmBase.ToolHolder = doHolder
                    doHolder.BaseAssemblyFile = frmTool.AssemblyFile
                    doHolder.BaseClassName = frmTool.ClassName
                    doHolder.Name = strName
                    doHolder.ToolForm = frmBase
                    Util.Simulation.ToolHolders.Add(doHolder.ID, doHolder)
                    doHolder.CreateWorkspaceTreeView(DirectCast(Me.Simulation, Framework.DataObject), Me.Simulation.ToolViewersTreeNode)
                    doHolder.SelectItem()

                    Application.DoEvents()

                    Me.Cursor = System.Windows.Forms.Cursors.Arrow

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.Cursor = System.Windows.Forms.Cursors.Arrow
            End Try
        End Sub

        'Protected Overridable Sub OnViewDockingForm(ByVal sender As Object, ByVal e As EventArgs)

        '    Try
        '        Dim menuCommand As MenuCommand = DirectCast(sender, MenuCommand)

        '        'Now lets try and find a docking content with that title.
        '        Dim ctDock As Content = m_dockManager.Contents(DirectCast(menuCommand.Tag, String))

        '        If Not ctDock Is Nothing Then
        '            If ctDock.Visible Then
        '                m_dockManager.HideContent(ctDock)
        '            Else
        '                m_dockManager.ShowContent(ctDock)
        '            End If
        '        End If

        '    Catch ex As System.Exception
        '        AnimatGUI.Framework.Util.DisplayError(ex)
        '    End Try
        'End Sub


        Protected Sub AnimatApplication_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

            Try
                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Info, "Exiting the application. Project: '" & Util.Application.ProjectPath & "\" & Util.Application.ProjectFile)

                'First check to see if the application is dirty. If it is then ask to save the project
                SaveIfDirty(e)

                If e.Cancel Then Return

                'Deselect all currently selected items so that the property grids do not cause problems when closing.
                If Not Util.ProjectWorkspace Is Nothing AndAlso Not Util.ProjectWorkspace.TreeView Is Nothing AndAlso Not Util.ProjectWorkspace.TreeView.SelectedNodes Is Nothing Then
                    Util.ProjectWorkspace.TreeView.SelectedNodes.Clear()
                End If

                'Shutdown the simulation if it is running. Must shut this down before closing the other stuff below or it throws an exception.
                Me.SimulationInterface.ShutdownSimulation()

                'Inform the other objects that we are shutting down.
                RaiseEvent ApplicationExiting()

                'Lets close all of the children first.
                'We have to do this to make certain that all of the controls are closed 
                'down in the appropriate order.
                For Each oChild As Form In Me.ChildForms
                    oChild.Close()
                Next

                'We also need to explicitly close the docking toolbars.
                If Not m_dockManager Is Nothing Then
                    ClearDockingContents()
                End If

                m_mdiClient = Nothing
                m_doSimulation = Nothing
                'm_ipToolPanel = Nothing
                m_wcWorkspaceContent = Nothing
                m_frmWorkspace = Nothing
                m_frmToolbox = Nothing
                m_frmSimulationController = Nothing

                m_aryNeuralModules.Clear()
                m_aryPlugInAssemblies.Clear()
                m_aryBehavioralNodes.Clear()
                m_aryBehavioralLinks.Clear()
                m_aryBodyPartTypes.Clear()
                m_aryRigidBodyTypes.Clear()
                m_aryJointTypes.Clear()
                m_aryBehavioralPanels.Clear()
                m_aryAlphabeticalBehavioralPanels.Clear()
                m_aryToolPlugins.Clear()

                m_dockManager.Dispose()

                Me.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Debug, "Finished cleanup for application exit.")
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub


        Private Sub AnimatApplication_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                CloseSplashScreen()

                If m_strCommandLineParam.Length > 0 Then
                    Me.LoadProject(m_strCommandLineParam)
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AnimatTabbedGroups_PageCloseRequest(ByVal tg As Crownwood.DotNetMagic.Controls.TabbedGroups, ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs) Handles AnimatTabbedGroups.PageCloseRequest
            Try
                If Not e.TabPage.Control Is Nothing AndAlso Util.IsTypeOf(e.TabPage.Control.GetType(), GetType(Forms.ExternalFileForm), False) Then
                    Dim frmAnimat As Forms.ExternalFileForm = DirectCast(e.TabPage.Control, Forms.ExternalFileForm)

                    If frmAnimat.IsDirty Then
                        Dim eResult As System.Windows.Forms.DialogResult = DialogResult.OK
                        eResult = MessageBox.Show("There are unsaved changes in this form. " & _
                                                                                            "Do you want to save them before you exit?", _
                                                                                            "Save Changes", MessageBoxButtons.YesNoCancel)
                        If eResult = DialogResult.Cancel Then
                            If Not e Is Nothing Then e.Cancel = True
                            Return
                        ElseIf eResult = DialogResult.Yes Then
                            Me.SaveProject(Me.ProjectFile)
                        End If
                    End If

                    If Not e.Cancel Then
                        Util.Application.CloseForm(frmAnimat, e)
                    End If
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnMdiClientClicked(ByVal sender As Object, ByVal e As System.EventArgs)
            Try

                'For Each ctDock As Content In m_dockManager.Contents
                '    If Not ctDock.AutoHidePanel Is Nothing Then
                '        ctDock.AutoHidePanel.RestoreToHiddenState()
                '    End If
                'Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AnimatTabbedGroups_PageLoading(ByVal tg As Crownwood.DotNetMagic.Controls.TabbedGroups, ByVal e As Crownwood.DotNetMagic.Controls.TGPageLoadingEventArgs) Handles AnimatTabbedGroups.PageLoading
            Try
                If e.XmlIn.Read() AndAlso e.XmlIn.NodeType = Xml.XmlNodeType.CDATA Then
                    Dim strXml As String = e.XmlIn.Value

                    Dim oXml As New Interfaces.StdXml
                    oXml.Deserialize(strXml)

                    oXml.FindElement("TabPage")
                    oXml.FindChildElement("Form")

                    oXml.IntoElem()
                    Dim strAssembly As String = oXml.GetChildString("AssemblyFile")
                    Dim strClass As String = oXml.GetChildString("ClassName")
                    Dim strTitle As String = oXml.GetChildString("Title")
                    Dim strFile As String = oXml.GetChildString("ExternalFile")
                    oXml.OutOfElem()

                    Dim frmAnimat As Forms.ExternalFileForm = DirectCast(CreateForm(strAssembly, strClass, strTitle, False), Forms.ExternalFileForm)
                    frmAnimat.Initialize(Me)
                    frmAnimat.LoadExternalFile(frmAnimat.ExternalFilename)

                    AddChildForm(frmAnimat, e.TabPage)
                    e.TabPage.Control = frmAnimat
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AnimatTabbedGroups_PageSaving(ByVal tg As Crownwood.DotNetMagic.Controls.TabbedGroups, ByVal e As Crownwood.DotNetMagic.Controls.TGPageSavingEventArgs) Handles AnimatTabbedGroups.PageSaving
            Try
                If Not e.TabPage.Control Is Nothing AndAlso Util.IsTypeOf(e.TabPage.Control.GetType(), GetType(Forms.ExternalFileForm), False) Then
                    Dim frmAnimat As Forms.ExternalFileForm = DirectCast(e.TabPage.Control, Forms.ExternalFileForm)
                    Dim oXml As New AnimatGUI.Interfaces.StdXml()

                    oXml.AddElement("TabPage")
                    frmAnimat.SaveEditorData(oXml)

                    Dim strXml As String = oXml.Serialize()
                    e.XmlOut.WriteCData(strXml)

                    frmAnimat.SaveExternalFile(frmAnimat.ExternalFilename)

                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AnimatTabbedGroups_PageChanged(ByVal tg As Crownwood.DotNetMagic.Controls.TabbedGroups, ByVal tp As Crownwood.DotNetMagic.Controls.TabPage) Handles AnimatTabbedGroups.PageChanged

            Try

                If Not m_selChildMenuStrip Is Nothing Then
                    ToolStripManager.RevertMerge(Me.AnimatMenuStrip)
                End If

                If Not m_selChildToolStrip Is Nothing Then
                    ToolStripManager.RevertMerge(Me.AnimatToolStrip)
                End If

                If Not tp Is Nothing AndAlso Not tp.Control Is Nothing AndAlso Util.IsTypeOf(tp.Control.GetType, GetType(Forms.AnimatForm), False) Then
                    Dim frmChild As Forms.AnimatForm = DirectCast(tp.Control, Forms.AnimatForm)

                    m_selChildMenuStrip = frmChild.FormMenuStrip
                    If Not m_selChildMenuStrip Is Nothing Then
                        ToolStripManager.Merge(m_selChildMenuStrip, Me.AnimatMenuStrip)
                    End If

                    m_selChildToolStrip = frmChild.FormToolStrip
                    If Not m_selChildToolStrip Is Nothing Then
                        ToolStripManager.Merge(m_selChildToolStrip, Me.AnimatToolStrip)
                    End If

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


#Region " Seleciton and Display Mode Event Handlers "


        Protected Function SenderChecked(ByVal sender As System.Object) As Boolean
            Dim bChecked As Boolean = False
            If Not sender Is Nothing AndAlso (Util.IsTypeOf(sender.GetType(), GetType(ToolStripButton), False) OrElse Util.IsTypeOf(sender.GetType, GetType(ToolStripMenuItem), False)) Then
                If Util.IsTypeOf(sender.GetType(), GetType(ToolStripButton), False) Then
                    Dim ctrl As ToolStripButton = DirectCast(sender, ToolStripButton)
                    Return ctrl.Checked
                Else
                    Dim ctrl As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
                    Return ctrl.Checked
                End If
            End If

            Return bChecked
        End Function

        Private Sub SelGraphicsToolStripButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelGraphicsToolStripButton.CheckedChanged, GraphicsObjectsToolStripMenuItem.CheckedChanged

            Try
                If SenderChecked(sender) Then
                    Me.Simulation.VisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectGraphics
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub SelCollisionToolStripButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelCollisionToolStripButton.CheckedChanged, CollisionObjectsToolStripMenuItem.CheckedChanged

            Try
                If SenderChecked(sender) Then
                    Me.Simulation.VisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectCollisions
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub SelJointsToolStripButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelJointsToolStripButton.CheckedChanged, JointsToolStripMenuItem.CheckedChanged

            Try
                If SenderChecked(sender) Then
                    Me.Simulation.VisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectJoints
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub SelRecFieldsToolStripButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelRecFieldsToolStripButton.CheckedChanged, ReceptiveFieldsToolStripMenuItem.CheckedChanged

            Try
                If SenderChecked(sender) Then
                    Me.Simulation.VisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectReceptiveFields
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub SelSimToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelSimToolStripButton.Click, SimulationToolStripMenuItem.CheckedChanged

            Try
                If SenderChecked(sender) Then
                    Me.Simulation.VisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.Simulation
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Sub AddPartToolStripButton_CheckChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Try
                Dim bVal As Boolean = SenderChecked(sender)
                If bVal <> Util.Simulation.AddBodiesMode Then
                    Util.Simulation.AddBodiesMode = bVal
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ShowGraphicsGeometryToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowGraphicsGeometryToolStripMenuItem.CheckedChanged, ShowGraphicsGeometryToolStripMenuItem.CheckedChanged

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
            Try
                Me.SimulationInterface.SaveSimulationFile(Me.ProjectPath & "Snapshot.vxf")
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

#End Region

    End Class

End Namespace

