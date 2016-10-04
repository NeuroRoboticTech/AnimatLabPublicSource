Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports System.Runtime.Remoting
Imports System.Threading
Imports System.Globalization
Imports AnimatGuiCtrls.Controls
Imports Crownwood.DotNetMagic.Controls
Imports Crownwood.DotNetMagic.Common
Imports Crownwood.DotNetMagic.Docking
Imports AnimatGUI.Framework
Imports System.Reflection
Imports System.Configuration
Imports System.Runtime.InteropServices
Imports System.Xml

Namespace Forms

    Public Class AnimatApplication
        Inherits AnimatForm
        Implements ManagedAnimatInterfaces.ISimApplication

        Private Declare Function GetConsoleWindow Lib "kernel32.dll" () As IntPtr
        Private Declare Function ShowWindow Lib "user32.dll" (ByVal hwnd As IntPtr, ByVal nCmdShow As Int32) As Int32
        Private Const SW_SHOWMINNOACTIVE As Int32 = 7
        Private Const SW_SHOWNORMAL As Int32 = 1
        Friend WithEvents ConvertPhysicsEngineToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Private Const SW_HIDE As Int32 = 0

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            Me.WindowState = FormWindowState.Maximized
            Me.IsMdiContainer = True
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

        Public WithEvents AnimatStripContainer As System.Windows.Forms.ToolStripContainer
        Public WithEvents AnimatAppStatusPanel As Crownwood.DotNetMagic.Controls.StatusPanel
        Public WithEvents AnimatUpdateStatusPanel As Crownwood.DotNetMagic.Controls.StatusPanel
        Public WithEvents AnimatStatusBar As Crownwood.DotNetMagic.Controls.StatusBarControl
        Public WithEvents AnimatMenuStrip As AnimatGuiCtrls.Controls.AnimatMenuStrip
        Public WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents toolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Public WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents SaveAsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents toolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
        Public WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents UndoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents RedoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents toolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
        Public WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents CustomizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents PreferencesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ContentsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents WarehouseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents TutorialsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents MouseCheatSheetToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents toolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
        Public WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents CheckForUpdatesStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents toolStripSeparatorHelp2 As System.Windows.Forms.ToolStripSeparator
        Public WithEvents RegisterStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents AnimatToolStrip As AnimatGuiCtrls.Controls.AnimatToolStrip
        Public WithEvents NewToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents OpenToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents SaveToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents HelpToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents AnimatTabbedGroups As Crownwood.DotNetMagic.Controls.TabbedGroups
        Public WithEvents SupportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents RunSimulationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
        Public WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
        Public WithEvents CloseProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ExportStandaloneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents RunMacroToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
        Public WithEvents AddOrganismStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents AddStructureToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents AddToolToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents DeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents DeleteToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
        Public WithEvents DisplayModeDropDown As System.Windows.Forms.ToolStripDropDownButton
        Public WithEvents ShowGraphicsGeometryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ShowCollisionGeometryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ShowJointsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ShowCenterOfMassToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ShowPartOriginToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ShowContactsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents SelGraphicsToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents SelCollisionToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents SelJointsToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents SelRecFieldsToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents SelectionModeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents GraphicsObjectsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents CollisionObjectsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents JointsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ReceptiveFieldsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents SimulationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ConsoleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents DisplayToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ShowGraphicsGeometryToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ShowCollisionGeomoetryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ShowJointsToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ShowCenterOfMassToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ShowPartOriginsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ShowContactsToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents SelSimToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents SnapshotSimToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
        Public WithEvents AddStimulusToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents toolStripSeparator As System.Windows.Forms.ToolStripSeparator
        Public WithEvents SelectByTypeToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents RelabelSelectedToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents AddPartToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents AddJointToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents CompareItemsToolStripButton As System.Windows.Forms.ToolStripButton
        Public WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Public WithEvents SelectByTypeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents RelabelSelectedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents CompareItemsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents AddItemToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents AddStructureToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents AddOrganismToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents AddTooToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents AddBodyPartToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents AddBodyJointToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents AddStimulusToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents EditMaterialsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Public WithEvents EditMaterialsToolStripButton As System.Windows.Forms.ToolStripButton

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer


        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.

        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AnimatApplication))
            Me.AnimatStripContainer = New System.Windows.Forms.ToolStripContainer()
            Me.AnimatTabbedGroups = New Crownwood.DotNetMagic.Controls.TabbedGroups()
            Me.AnimatStatusBar = New Crownwood.DotNetMagic.Controls.StatusBarControl()
            Me.AnimatAppStatusPanel = New Crownwood.DotNetMagic.Controls.StatusPanel()
            Me.AnimatUpdateStatusPanel = New Crownwood.DotNetMagic.Controls.StatusPanel()
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
            Me.RunMacroToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.toolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
            Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.SelectByTypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.RelabelSelectedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.CompareItemsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.EditMaterialsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
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
            Me.AddItemToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddStructureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddOrganismToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddTooToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddBodyPartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddBodyJointToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AddStimulusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ContentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.TutorialsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.WarehouseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.SupportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.MouseCheatSheetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.toolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
            Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.CheckForUpdatesStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.toolStripSeparatorHelp2 = New System.Windows.Forms.ToolStripSeparator()
            Me.RegisterStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AnimatToolStrip = New AnimatGuiCtrls.Controls.AnimatToolStrip()
            Me.NewToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.OpenToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.SaveToolStripButton = New System.Windows.Forms.ToolStripButton()
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
            Me.RelabelSelectedToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.CompareItemsToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.EditMaterialsToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
            Me.SelGraphicsToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.SelCollisionToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.SelJointsToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.SelRecFieldsToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.SelSimToolStripButton = New System.Windows.Forms.ToolStripButton()
            Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.RedoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ConsoleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.DisplayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowGraphicsGeometryToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowCollisionGeomoetryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowJointsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowCenterOfMassToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowPartOriginsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowContactsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
            Me.DisplayModeDropDown = New System.Windows.Forms.ToolStripDropDownButton()
            Me.ShowGraphicsGeometryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowCollisionGeometryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowJointsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowCenterOfMassToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowPartOriginToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowContactsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ConvertPhysicsEngineToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
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
            Me.AnimatStatusBar.StatusPanels.AddRange(New Crownwood.DotNetMagic.Controls.StatusPanel() {Me.AnimatAppStatusPanel, Me.AnimatUpdateStatusPanel})
            Me.AnimatStatusBar.TabIndex = 0
            '
            'AnimatAppStatusPanel
            '
            Me.AnimatAppStatusPanel.AutoScrollMargin = New System.Drawing.Size(0, 0)
            Me.AnimatAppStatusPanel.AutoScrollMinSize = New System.Drawing.Size(0, 0)
            Me.AnimatAppStatusPanel.AutoSizing = System.Windows.Forms.StatusBarPanelAutoSize.Contents
            Me.AnimatAppStatusPanel.Location = New System.Drawing.Point(2, 2)
            Me.AnimatAppStatusPanel.Name = "AnimatAppStatusPanel"
            Me.AnimatAppStatusPanel.Size = New System.Drawing.Size(0, 13)
            Me.AnimatAppStatusPanel.TabIndex = 0
            '
            'AnimatUpdateStatusPanel
            '
            Me.AnimatUpdateStatusPanel.AutoScrollMargin = New System.Drawing.Size(0, 0)
            Me.AnimatUpdateStatusPanel.AutoScrollMinSize = New System.Drawing.Size(0, 0)
            Me.AnimatUpdateStatusPanel.AutoSizing = System.Windows.Forms.StatusBarPanelAutoSize.Spring
            Me.AnimatUpdateStatusPanel.Location = New System.Drawing.Point(2, 2)
            Me.AnimatUpdateStatusPanel.Name = "AnimatUpdateStatusPanel"
            Me.AnimatUpdateStatusPanel.Size = New System.Drawing.Size(685, 13)
            Me.AnimatUpdateStatusPanel.TabIndex = 0
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
            Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.OpenToolStripMenuItem, Me.toolStripSeparator2, Me.SaveToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.toolStripSeparator4, Me.CloseProjectToolStripMenuItem, Me.ExportStandaloneToolStripMenuItem, Me.SnapshotSimToolStripMenuItem, Me.ConvertPhysicsEngineToolStripMenuItem, Me.ToolStripSeparator8, Me.ExitToolStripMenuItem})
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
            Me.NewToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
            Me.NewToolStripMenuItem.Text = "&New"
            '
            'OpenToolStripMenuItem
            '
            Me.OpenToolStripMenuItem.Image = CType(resources.GetObject("OpenToolStripMenuItem.Image"), System.Drawing.Image)
            Me.OpenToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
            Me.OpenToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
            Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
            Me.OpenToolStripMenuItem.Text = "&Open"
            '
            'toolStripSeparator2
            '
            Me.toolStripSeparator2.Name = "toolStripSeparator2"
            Me.toolStripSeparator2.Size = New System.Drawing.Size(194, 6)
            '
            'SaveToolStripMenuItem
            '
            Me.SaveToolStripMenuItem.Image = CType(resources.GetObject("SaveToolStripMenuItem.Image"), System.Drawing.Image)
            Me.SaveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
            Me.SaveToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
            Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
            Me.SaveToolStripMenuItem.Text = "&Save"
            '
            'SaveAsToolStripMenuItem
            '
            Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
            Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
            Me.SaveAsToolStripMenuItem.Text = "Save &As"
            '
            'toolStripSeparator4
            '
            Me.toolStripSeparator4.Name = "toolStripSeparator4"
            Me.toolStripSeparator4.Size = New System.Drawing.Size(194, 6)
            '
            'CloseProjectToolStripMenuItem
            '
            Me.CloseProjectToolStripMenuItem.Name = "CloseProjectToolStripMenuItem"
            Me.CloseProjectToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
            Me.CloseProjectToolStripMenuItem.Text = "Close Project"
            '
            'ExportStandaloneToolStripMenuItem
            '
            Me.ExportStandaloneToolStripMenuItem.Name = "ExportStandaloneToolStripMenuItem"
            Me.ExportStandaloneToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
            Me.ExportStandaloneToolStripMenuItem.Text = "Export Standalone Sim"
            '
            'SnapshotSimToolStripMenuItem
            '
            Me.SnapshotSimToolStripMenuItem.Name = "SnapshotSimToolStripMenuItem"
            Me.SnapshotSimToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
            Me.SnapshotSimToolStripMenuItem.Text = "Snapshot Sim"
            '
            'ToolStripSeparator8
            '
            Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
            Me.ToolStripSeparator8.Size = New System.Drawing.Size(194, 6)
            '
            'ExitToolStripMenuItem
            '
            Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
            Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
            Me.ExitToolStripMenuItem.Text = "E&xit"
            '
            'EditToolStripMenuItem
            '
            Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RunMacroToolStripMenuItem, Me.toolStripSeparator6, Me.DeleteToolStripMenuItem, Me.ToolStripSeparator1, Me.SelectByTypeToolStripMenuItem, Me.RelabelSelectedToolStripMenuItem, Me.CompareItemsToolStripMenuItem, Me.EditMaterialsToolStripMenuItem})
            Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
            Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
            Me.EditToolStripMenuItem.Text = "&Edit"
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
            'EditMaterialsToolStripMenuItem
            '
            Me.EditMaterialsToolStripMenuItem.Image = CType(resources.GetObject("EditMaterialsToolStripMenuItem.Image"), System.Drawing.Image)
            Me.EditMaterialsToolStripMenuItem.Name = "EditMaterialsToolStripMenuItem"
            Me.EditMaterialsToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.EditMaterialsToolStripMenuItem.Text = "Edit Materials"
            '
            'ViewToolStripMenuItem
            '
            Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RunSimulationToolStripMenuItem, Me.ToolStripSeparator3, Me.CustomizeToolStripMenuItem, Me.PreferencesToolStripMenuItem, Me.SelectionModeToolStripMenuItem})
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
            Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ContentsToolStripMenuItem, Me.TutorialsToolStripMenuItem, Me.WarehouseToolStripMenuItem, Me.SupportToolStripMenuItem, Me.MouseCheatSheetToolStripMenuItem, Me.toolStripSeparator7, Me.AboutToolStripMenuItem, Me.CheckForUpdatesStripMenuItem, Me.toolStripSeparatorHelp2, Me.RegisterStripMenuItem})
            Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
            Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
            Me.HelpToolStripMenuItem.Text = "&Help"
            '
            'ContentsToolStripMenuItem
            '
            Me.ContentsToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.HelpContents
            Me.ContentsToolStripMenuItem.Name = "ContentsToolStripMenuItem"
            Me.ContentsToolStripMenuItem.Size = New System.Drawing.Size(252, 22)
            Me.ContentsToolStripMenuItem.Text = "&Contents"
            '
            'TutorialsToolStripMenuItem
            '
            Me.TutorialsToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.HelpSearch
            Me.TutorialsToolStripMenuItem.Name = "TutorialsToolStripMenuItem"
            Me.TutorialsToolStripMenuItem.Size = New System.Drawing.Size(252, 22)
            Me.TutorialsToolStripMenuItem.Text = "&Tutorials"
            '
            'WarehouseToolStripMenuItem
            '
            Me.WarehouseToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.HelpIndex
            Me.WarehouseToolStripMenuItem.Name = "WarehouseToolStripMenuItem"
            Me.WarehouseToolStripMenuItem.Size = New System.Drawing.Size(252, 22)
            Me.WarehouseToolStripMenuItem.Text = "&Warehouse"
            '
            'SupportToolStripMenuItem
            '
            Me.SupportToolStripMenuItem.Name = "SupportToolStripMenuItem"
            Me.SupportToolStripMenuItem.Size = New System.Drawing.Size(252, 22)
            Me.SupportToolStripMenuItem.Text = "Support"
            '
            'MouseCheatSheetToolStripMenuItem
            '
            Me.MouseCheatSheetToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.HelpSearch
            Me.MouseCheatSheetToolStripMenuItem.Name = "MouseCheatSheetToolStripMenuItem"
            Me.MouseCheatSheetToolStripMenuItem.Size = New System.Drawing.Size(252, 22)
            Me.MouseCheatSheetToolStripMenuItem.Text = "&Simulation Mouse Controls Guide"
            '
            'toolStripSeparator7
            '
            Me.toolStripSeparator7.Name = "toolStripSeparator7"
            Me.toolStripSeparator7.Size = New System.Drawing.Size(249, 6)
            '
            'AboutToolStripMenuItem
            '
            Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
            Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(252, 22)
            Me.AboutToolStripMenuItem.Text = "&About..."
            '
            'CheckForUpdatesStripMenuItem
            '
            Me.CheckForUpdatesStripMenuItem.Name = "CheckForUpdatesStripMenuItem"
            Me.CheckForUpdatesStripMenuItem.Size = New System.Drawing.Size(252, 22)
            Me.CheckForUpdatesStripMenuItem.Text = "Check for updates"
            '
            'toolStripSeparatorHelp2
            '
            Me.toolStripSeparatorHelp2.Name = "toolStripSeparatorHelp2"
            Me.toolStripSeparatorHelp2.Size = New System.Drawing.Size(249, 6)
            '
            'RegisterStripMenuItem
            '
            Me.RegisterStripMenuItem.Name = "RegisterStripMenuItem"
            Me.RegisterStripMenuItem.Size = New System.Drawing.Size(252, 22)
            Me.RegisterStripMenuItem.Text = "Register AnimatLab Pro"
            '
            'AnimatToolStrip
            '
            Me.AnimatToolStrip.Dock = System.Windows.Forms.DockStyle.None
            Me.AnimatToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripButton, Me.OpenToolStripButton, Me.SaveToolStripButton, Me.DeleteToolStripButton, Me.toolStripSeparator, Me.HelpToolStripButton, Me.ToolStripSeparator5, Me.AddOrganismStripButton, Me.AddStructureToolStripButton, Me.AddToolToolStripButton, Me.AddPartToolStripButton, Me.AddJointToolStripButton, Me.AddStimulusToolStripButton, Me.ToolStripSeparator9, Me.SelectByTypeToolStripButton, Me.RelabelSelectedToolStripButton, Me.CompareItemsToolStripButton, Me.EditMaterialsToolStripButton, Me.ToolStripSeparator10, Me.SelGraphicsToolStripButton, Me.SelCollisionToolStripButton, Me.SelJointsToolStripButton, Me.SelRecFieldsToolStripButton, Me.SelSimToolStripButton})
            Me.AnimatToolStrip.Location = New System.Drawing.Point(3, 24)
            Me.AnimatToolStrip.Name = "AnimatToolStrip"
            Me.AnimatToolStrip.SecurityMgr = Nothing
            Me.AnimatToolStrip.Size = New System.Drawing.Size(496, 25)
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
            Me.AddPartToolStripButton.CheckOnClick = True
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
            Me.AddJointToolStripButton.CheckOnClick = True
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
            'ConsoleToolStripMenuItem
            '
            Me.ConsoleToolStripMenuItem.CheckOnClick = True
            Me.ConsoleToolStripMenuItem.Image = CType(resources.GetObject("ConsoleToolStripMenuItem.Image"), System.Drawing.Image)
            Me.ConsoleToolStripMenuItem.Name = "ConsoleToolStripMenuItem"
            Me.ConsoleToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
            Me.ConsoleToolStripMenuItem.Text = "Console"
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
            'ConvertPhysicsEngineToolStripMenuItem
            '
            Me.ConvertPhysicsEngineToolStripMenuItem.Name = "ConvertPhysicsEngineToolStripMenuItem"
            Me.ConvertPhysicsEngineToolStripMenuItem.Size = New System.Drawing.Size(197, 22)
            Me.ConvertPhysicsEngineToolStripMenuItem.Text = "Convert Physics Engine"
            Me.ConvertPhysicsEngineToolStripMenuItem.Enabled = True
            '
            'AnimatApplication
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(713, 283)
            Me.Controls.Add(Me.AnimatStripContainer)
            Me.MainMenuStrip = Me.AnimatMenuStrip
            Me.Name = "AnimatApplication"
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

        Protected m_iXmlVersion As Integer = 3
        Protected m_bUseMockSimInterface As Boolean = False
        Protected m_bUseMockDataObjectInterface As Boolean = False
        Protected m_bUseMockStdXml As Boolean = False
        Protected m_bUseNetLogger As Boolean = False
        Protected m_bConsoleApp As Boolean = False

        Protected m_mgrToolStripImages As AnimatGUI.Framework.ImageManager
        Protected m_mgrLargeImages As AnimatGUI.Framework.ImageManager
        Protected m_mgrWorkspaceImages As AnimatGUI.Framework.ImageManager
        Protected m_mgrTabPagesImages As AnimatGUI.Framework.ImageManager
        Protected m_selTabPage As Crownwood.DotNetMagic.Controls.TabPage

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

        Protected m_aryChildForms As New Collections.AnimatForms(Nothing)
        Protected m_arySortedChildForms As New Collections.SortedAnimatForms(Nothing)

        Protected m_doSimulation As DataObjects.Simulation
        Protected m_doSimInterface As ManagedAnimatInterfaces.ISimulatorInterface

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
        Protected m_aryMacros As New Collections.Macros(Nothing)
        Protected m_aryExternalStimuli As New Collections.Stimuli(Nothing)
        Protected m_aryProjectMigrations As New Hashtable()
        Protected m_aryPhysicsEngines As New Collections.PhysicsEngines(Nothing)
        Protected m_aryMotorControlSystems As New Collections.MotorControlSystems(Nothing)
        Protected m_aryInputSystems As New Collections.InputSystems(Nothing)
        Protected m_aryRobotPartInterfaces As New Collections.RobotPartInterfaces(Nothing)
        Protected m_aryRobotInterfaces As New Collections.RobotInterfaces(Nothing)
        Protected m_aryRobotIOControls As New Collections.RobotIOControls(Nothing)
        Protected m_aryScriptProcessors As New Collections.ScriptProcessors(Nothing)
        Protected m_aryRemoteControlLinkages As New Collections.RemoteControlLinkages(Nothing)

        'List of adapter pairs
        Protected m_aryAdapterPairs As New ArrayList()
        Protected m_aryLinkPairs As New ArrayList()

        Protected m_wcWorkspaceContent As Crownwood.DotNetMagic.Docking.WindowContent
        Protected m_wcPropertiesContent As Crownwood.DotNetMagic.Docking.WindowContent
        Protected m_wcToolboxContent As Crownwood.DotNetMagic.Docking.WindowContent
        Protected m_wcErrorsContent As Crownwood.DotNetMagic.Docking.WindowContent
        Protected m_wcSimControllerContent As Crownwood.DotNetMagic.Docking.WindowContent
        Protected m_wcRecFieldPairsContent As Crownwood.DotNetMagic.Docking.WindowContent
        Protected m_wcRecFieldGainContent As Crownwood.DotNetMagic.Docking.WindowContent
        Protected m_wcRecFieldCurrentContent As Crownwood.DotNetMagic.Docking.WindowContent

        Protected m_frmWorkspace As Forms.ProjectWorkspace
        Protected m_frmProperties As Forms.ProjectProperties
        Protected m_frmToolbox As Forms.Toolbox
        Protected m_frmErrors As Forms.Errors
        Protected m_frmSimulationController As Forms.SimulationController
        Protected m_frmReceptiveFieldPairs As Forms.ReceptiveFieldPairs
        Protected m_frmReceptiveFieldGain As Forms.ReceptiveFieldGain
        Protected m_frmReceptiveFieldCurrent As Forms.ReceptiveFieldCurrent

        Protected m_frmLastSelectedChart As Forms.Tools.DataChart

        'This keeps track of which part type pairs are exculded from being added to each other.
        'The key is the ParentType_ChildType. If an entry is found in the hashtable for that pair
        ' then that child cannot be added to that parent type.
        Protected m_aryPartTypeExclusions As New ArrayList

        'List of items that need to be deleted after load is finished because they are invalid.
        'This can happen when converting an old version that was not saved out correctly. The old
        'version did not always clean itself up correctly. There could be offpage connectors that 
        'no longer had nodes to them. We need to clean these up.
        Protected m_aryDeleteAfterLoad As New ArrayList

        'Protected m_ipToolPanel As New IconPanel

        'Protected m_ptSimWindowLocation As System.Drawing.Point
        'Protected m_szSimWindowSize As System.Drawing.Size

        Protected m_bProjectIsOpen As Boolean = False

        Protected m_timerShutdown As System.Timers.Timer
        Protected m_strCmdLineProject As String = ""

        Protected m_timerAutomation As System.Timers.Timer
        Protected m_aryToolClicks() As System.Windows.Forms.ToolStripItem
        Protected m_tnAutomationTreeNode As Crownwood.DotNetMagic.Controls.Node
        Protected m_piAutomationPropInfo As PropertyInfo
        Protected m_oAutomationPropertyValue As Object
        Protected m_doAutomationStructure As DataObjects.Physical.PhysicalStructure
        Protected m_doAutomationBodyPart As DataObjects.Physical.BodyPart
        Protected m_bnAutomationNodeOrigin As DataObjects.Behavior.Node
        Protected m_bnAutomationNodeDestination As DataObjects.Behavior.Node
        Protected m_strAutomationPath As String = ""
        Protected m_ptAutomationPosition As Point
        Protected m_strAutomationName As String = ""
        Protected m_strAutomationMethodName As String = ""
        Protected m_aryAutomationParams() As Object
        Protected m_bAutomationPropBoolValue As Boolean = False
        Protected m_bAutomationMethodInProgress As Boolean = False
        Protected m_bAppIsBusy As Boolean = False
        Protected m_iAppBusyCounter As Integer = 0
        Protected m_bAutomation_ExportedChartData As Boolean = False

        Protected m_bBodyPartPasteInProgress As Boolean = False

        Protected m_auBackup As wyDay.Controls.AutomaticUpdaterBackend

        Protected m_bSimStopped As Boolean = True

        Protected m_SplashTimer As New System.Timers.Timer

        Protected m_bStoppingSimulation As Boolean = False

#Region " Preferences "

        Protected m_strDefaultNewFolder As String = ""

        Protected m_Logger As ManagedAnimatInterfaces.ILogger
        Protected m_ModificationHistory As New AnimatGUI.Framework.UndoSystem.ModificationHistory

        Protected m_eAutoUpdateInterval As enumAutoUpdateInterval = enumAutoUpdateInterval.Daily
        Protected m_dtLastAutoUpdateTime As DateTime

        Protected m_bAnnouceUpdates As Boolean = False

        Protected m_SecurityMgr As AnimatGuiCtrls.Security.SecurityManager

        Protected m_eDefaultLogLevel As ManagedAnimatInterfaces.ILogger.enumLogLevel = ManagedAnimatInterfaces.ILogger.enumLogLevel.ErrorType
        Protected m_strSimVCVersion As String = "10"
        Protected m_doPhysics As DataObjects.Physical.PhysicsEngine = New DataObjects.Physical.PhysicsEngines.BulletPhysicsEngine(Nothing)

#End Region

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

                m_Logger.LogPrefix = m_strLogDirectory & "AnimatLab"
            End Set
        End Property

        Public ReadOnly Property Logger() As ManagedAnimatInterfaces.ILogger Implements ManagedAnimatInterfaces.ISimApplication.Logger
            Get
                Return m_Logger
            End Get
        End Property

        Public Overridable ReadOnly Property Physics() As DataObjects.Physical.PhysicsEngine
            Get
                Return m_doPhysics
            End Get
        End Property

        Public Overridable ReadOnly Property SimVCVersion() As String
            Get
                Return m_strSimVCVersion
            End Get
        End Property

#End Region

        Public Overridable ReadOnly Property ConsoleApp() As Boolean
            Get
                Return m_bConsoleApp
            End Get
        End Property

        Public Overridable ReadOnly Property ChildForms() As Collections.AnimatForms
            Get
                Return m_aryChildForms
            End Get
        End Property

        Public Overridable ReadOnly Property SortedChildForms() As Collections.SortedAnimatForms
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

        Public Overridable ReadOnly Property ProjectErrors() As Forms.Errors
            Get
                Return m_frmErrors
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

        Public Overridable ReadOnly Property ApplicationDirectory() As String Implements ManagedAnimatInterfaces.ISimApplication.ApplicationDirectory
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

        Public ReadOnly Property XmlVersion() As Integer
            Get
                Return m_iXmlVersion
            End Get
        End Property

        Public ReadOnly Property StatusBar() As Crownwood.DotNetMagic.Controls.StatusBarControl
            Get
                Return Me.AnimatStatusBar
            End Get
        End Property

        Public Property AppStatusText() As String
            Get
                Return Me.AnimatAppStatusPanel.Text
            End Get
            Set(ByVal Value As String)
                Me.AnimatAppStatusPanel.Text = Value
                If Not Logger Is Nothing Then
                    Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "AppStatusText: " & Value)
                End If
                Application.DoEvents()
            End Set
        End Property

        Public Property UpdateStatusText() As String
            Get
                Return Me.AnimatUpdateStatusPanel.Text
            End Get
            Set(ByVal Value As String)
                Me.AnimatUpdateStatusPanel.Text = Value
                If Not Logger Is Nothing Then
                    Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "UpdateStatusText: " & Value)
                End If
                Application.DoEvents()
            End Set
        End Property

        Public Overridable Property Simulation() As DataObjects.Simulation
            Get
                Return m_doSimulation
            End Get
            Set(ByVal Value As DataObjects.Simulation)
                'If we are replacing an existing sim objec then remove its handlers.
                If Not m_doSimulation Is Nothing Then
                    m_doSimulation.RemoveHandlers()
                End If

                m_doSimulation = Value
            End Set
        End Property

        Public Overridable ReadOnly Property SimulationInterface() As ManagedAnimatInterfaces.ISimulatorInterface
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

        Public Overridable ReadOnly Property Macros() As Collections.Macros
            Get
                Return m_aryMacros
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

        Public Overridable ReadOnly Property PhysicsEngines() As Collections.PhysicsEngines
            Get
                Return m_aryPhysicsEngines
            End Get
        End Property

        Public Overridable ReadOnly Property MotorControlSystems() As Collections.MotorControlSystems
            Get
                Return m_aryMotorControlSystems
            End Get
        End Property

        Public Overridable ReadOnly Property InputSystems() As Collections.InputSystems
            Get
                Return m_aryInputSystems
            End Get
        End Property

        Public Overridable ReadOnly Property RobotPartInterfaces() As Collections.RobotPartInterfaces
            Get
                Return m_aryRobotPartInterfaces
            End Get
        End Property

        Public Overridable ReadOnly Property RobotInterfaces() As Collections.RobotInterfaces
            Get
                Return m_aryRobotInterfaces
            End Get
        End Property

        Public Overridable ReadOnly Property RobotIOControls() As Collections.RobotIOControls
            Get
                Return m_aryRobotIOControls
            End Get
        End Property

        Public Overridable ReadOnly Property ScriptProcessors() As Collections.ScriptProcessors
            Get
                Return m_aryScriptProcessors
            End Get
        End Property

        Public Overridable ReadOnly Property RemoteControlLinkages() As Collections.RemoteControlLinkages
            Get
                Return m_aryRemoteControlLinkages
            End Get
        End Property

        Public Overridable ReadOnly Property AdapterPairs() As ArrayList
            Get
                Return m_aryAdapterPairs
            End Get
        End Property

        Public Overridable ReadOnly Property LinkPairs() As ArrayList
            Get
                Return m_aryLinkPairs
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

        Public Overridable ReadOnly Property AutomationMethodInProgress() As Boolean
            Get
                Return m_bAutomationMethodInProgress
            End Get
        End Property

        Protected Overridable Property InternalAutomationMethodInProgress() As Boolean
            Get
                Return m_bAutomationMethodInProgress
            End Get
            Set(value As Boolean)
                m_bAutomationMethodInProgress = value
            End Set
        End Property

        Public Overridable ReadOnly Property Automation_ExportedChartData() As Boolean
            Get
                Return m_bAutomation_ExportedChartData
            End Get
        End Property

        Public Overridable Property AppIsBusy() As Boolean
            Get
                Return m_bAppIsBusy
            End Get
            Set(ByVal value As Boolean)

                If value AndAlso m_bAppIsBusy Then
                    m_iAppBusyCounter = m_iAppBusyCounter + 1
                ElseIf Not value And m_iAppBusyCounter > 0 Then
                    m_iAppBusyCounter = m_iAppBusyCounter - 1
                ElseIf value AndAlso Not m_bAppIsBusy Then
                    m_iAppBusyCounter = 1
                End If

                If m_iAppBusyCounter = 1 AndAlso m_bAppIsBusy = False Then
                    m_bAppIsBusy = True
                    Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
                ElseIf m_iAppBusyCounter = 0 Then
                    m_bAppIsBusy = False
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                End If
            End Set
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

        Public Overridable ReadOnly Property SimIsRunning() As Boolean
            Get
                If Not m_bSimStopped Then
                    Return True
                Else
                    Return Me.SimulationInterface.SimRunning()
                End If
            End Get
        End Property

        Public Overridable ReadOnly Property BodyPasteInProgress As Boolean
            Get
                Return m_bBodyPartPasteInProgress
            End Get
        End Property

        Public Overridable Property UseMockSimInteface() As Boolean
            Get
                Return m_bUseMockSimInterface
            End Get
            Set(ByVal Value As Boolean)
                m_bUseMockSimInterface = Value
            End Set
        End Property

        Public Overridable Property MockDataObjectInterface() As Boolean
            Get
                Return m_bUseMockDataObjectInterface
            End Get
            Set(ByVal Value As Boolean)
                m_bUseMockDataObjectInterface = Value
            End Set
        End Property

        Public Overridable ReadOnly Property ErrorDialogMessage() As String
            Get
                If Not Util.ErrorForm Is Nothing AndAlso Util.ErrorForm.Visible Then
                    Return Util.ErrorForm.GetErrorMessage
                Else
                    Return ""
                End If
            End Get
        End Property

        Public Overridable Property LastSelectedChart() As Forms.Tools.DataChart
            Get
                Return m_frmLastSelectedChart
            End Get
            Set(ByVal Value As Forms.Tools.DataChart)
                m_frmLastSelectedChart = Value
            End Set
        End Property

#End Region

#Region " Methods "

#Region " Initialization "

        Public Overridable Sub StartApplication(ByVal bModal As Boolean, ByVal bConsoleApp As Boolean, ByVal oSecCtrl As Object) Implements ManagedAnimatInterfaces.ISimApplication.StartApplication

            Try

                m_SecurityMgr = New OpenSourceSecurityManager(Me) 'DirectCast(oSecCtrl, AnimatGuiCtrls.Security.SecurityManager)
                If m_SecurityMgr Is Nothing Then
                    Throw New System.Exception("Security manager was not defined.")
                End If

                ShowSplashScreen()

                Initialize(Nothing)

                m_bConsoleApp = bConsoleApp
                If Me.ConsoleApp Then
                    Me.ViewToolStripMenuItem.DropDownItems.Add(Me.ConsoleToolStripMenuItem)
                    Me.ConsoleToolStripMenuItem.Enabled = True
                End If

                ProcessArguments()

                If bModal Then
                    Me.ShowDialog()
                Else
                    Me.Show()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub ProcessArguments()
            Try
                Dim args() As String = System.Environment.GetCommandLineArgs()
                Dim bProjectFound As Boolean = False

                Dim iCount As Integer = args.Length
                For iIdx As Integer = 0 To iCount - 1
                    If args(iIdx).Trim.ToUpper = "-PROJECT" AndAlso iIdx < (iCount - 1) Then
                        m_strCmdLineProject = args(iIdx + 1)
                        bProjectFound = True
                    End If
                Next

                'If we did not specify the project explicitly and there is only one param and it does not start with dash, then assume it is project file name.
                If Not bProjectFound AndAlso iCount = 2 AndAlso Not args(1).StartsWith("-") Then
                    m_strCmdLineProject = args(1)
                    bProjectFound = True
                End If

            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Function CreateSimInterface() As ManagedAnimatInterfaces.ISimulatorInterface
            Dim iSim As ManagedAnimatInterfaces.ISimulatorInterface

            If m_bUseMockSimInterface Then
                iSim = New ManagedAnimatInterfaces.SimulatorInterfaceMock()
            Else
                iSim = DirectCast(Util.LoadClass("ManagedAnimatTools.dll", "AnimatGUI.Interfaces.SimulatorInterface"), ManagedAnimatInterfaces.ISimulatorInterface)
            End If

            iSim.SetLogger(Util.Logger)

            Return iSim
        End Function

        Public Function CreateDataObjectInterface(ByVal strID As String) As ManagedAnimatInterfaces.IDataObjectInterface
            Dim iDataObject As ManagedAnimatInterfaces.IDataObjectInterface

            If m_bUseMockDataObjectInterface Then
                iDataObject = New ManagedAnimatInterfaces.DataObjectInterfaceMock(Me.SimulationInterface, strID)
            Else
                iDataObject = DirectCast(Util.LoadClass("ManagedAnimatTools.dll", "AnimatGUI.Interfaces.DataObjectInterface", New Object() {Me.SimulationInterface, strID}), ManagedAnimatInterfaces.IDataObjectInterface)
            End If

            Return iDataObject
        End Function

        Public Function CreateStdXml() As ManagedAnimatInterfaces.IStdXml
            Dim iXml As ManagedAnimatInterfaces.IStdXml

            If m_bUseMockStdXml Then
                iXml = New ManagedAnimatInterfaces.StdXmlMock
            Else
                iXml = DirectCast(Util.LoadClass("ManagedAnimatTools.dll", "AnimatGUI.Interfaces.StdXml"), ManagedAnimatInterfaces.IStdXml)
            End If

            iXml.SetLogger(Util.Logger)

            Return iXml
        End Function

        Public Function CreateLogger() As ManagedAnimatInterfaces.ILogger
            Dim iLog As ManagedAnimatInterfaces.ILogger

            If m_bUseNetLogger Then
                iLog = New AnimatGUI.Framework.Logger()
            Else
                iLog = DirectCast(Util.LoadClass("ManagedAnimatTools.dll", "AnimatGUI.Interfaces.Logger"), ManagedAnimatInterfaces.ILogger)
            End If

            Return iLog
        End Function

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)

            Try
                Util.DisableDirtyFlags = True
                Util.Application = Me

                m_doSimInterface = CreateSimInterface()

                Me.AnimatToolStrip.ToolName = "AnimatGUI.Forms.AnimatApplication"
                Me.AnimatToolStrip.SecurityMgr = Me.SecurityMgr

                Me.AnimatMenuStrip.ToolName = "AnimatGUI.Forms.AnimatApplication"
                Me.AnimatMenuStrip.SecurityMgr = Me.SecurityMgr

                'Reset the culture info to be invariant english. I was getting problems 
                'with foriegn culture infos not parsing the xml files correctly.
                Thread.CurrentThread.CurrentCulture = New CultureInfo("")

                Me.AppStatusText = "Starting application"

                LoadUserConfig()
                InitLogging()
                FindMdiClient()
                CatalogPluginModules()
                CheckSimRegistryEntry()
                ResetProject(False)
                AutoUpdate()
                UpdateToolstrips()

                'This handler is used as a callback to get the editor to saveout the xml configuration file and send it back to the simulation
                AddHandler m_doSimInterface.OnSimulationCreate, AddressOf Me.OnCreateSimulation
                AddHandler m_doSimInterface.SimulationRunning, AddressOf Me.OnSimulationRunning
                AddHandler m_doSimInterface.NeedToStopSimulation, AddressOf Me.OnNeedToStopSimulation
                AddHandler m_doSimInterface.HandleCriticalError, AddressOf Me.OnHandleCriticalError
                AddHandler m_doSimInterface.HandleNonCriticalError, AddressOf Me.OnHandleNonCriticalError

                AddHandler Me.SimulationStarting, AddressOf Me.OnSimulationStarting
                AddHandler Me.SimulationResuming, AddressOf Me.OnSimulationResuming
                AddHandler Me.SimulationPaused, AddressOf Me.OnSimulationPaused
                AddHandler Me.SimulationStopped, AddressOf Me.OnSimulationStopped
                AddHandler Me.AddPartToolStripButton.CheckStateChanged, AddressOf Me.AddPartToolStripButton_CheckChanged

                Util.Simulation.VisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectGraphics

                Me.AppStatusText = ""

                If Me.SecurityMgr.IsValidSerialNumber Then
                    Me.Title = "AnimatLab Pro"
                Else
                    Me.Title = "AnimatLab"
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Util.DisableDirtyFlags = False
            End Try
        End Sub

        Private Sub AutoUpdate()
            Try
#If Not Debug Then
                'Setup the autoupdater.
                Me.AppStatusText = "Auto update check"
                m_auBackup = New wyDay.Controls.AutomaticUpdaterBackend
                m_auBackup.GUID = System.Guid.NewGuid.ToString
                m_auBackup.UpdateType = wyDay.Controls.UpdateType.OnlyCheck
                m_auBackup.wyUpdateLocation = "..\AnimatLabUpdater.exe"

                AddHandler m_auBackup.CheckingFailed, AddressOf Me.AutoUpdate_CheckingFailed
                AddHandler m_auBackup.CloseAppNow, AddressOf Me.AutoUpdate_CloseAppNow
                AddHandler m_auBackup.DownloadingFailed, AddressOf Me.AutoUpdate_Downloading_Failed
                AddHandler m_auBackup.ExtractingFailed, AddressOf Me.AutoUpdate_ExtractingFailed
                AddHandler m_auBackup.ReadyToBeInstalled, AddressOf Me.AutoUpdate_ReadyToBeInstalled
                AddHandler m_auBackup.UpdateFailed, AddressOf Me.AutoUpdate_UpdateFailed
                AddHandler m_auBackup.UpdateSuccessful, AddressOf Me.AutoUpdate_UpdateSuccessful
                AddHandler m_auBackup.UpdateAvailable, AddressOf Me.AutoUpdate_UpdateAvailable
                AddHandler m_auBackup.UpToDate, AddressOf Me.AutoUpdate_UpToDate
                AddHandler m_auBackup.ProgressChanged, AddressOf Me.AutoUpdate_ProgressChanged
                AddHandler m_auBackup.BeforeChecking, AddressOf Me.AutoUpdate_BeforeChecking
                AddHandler m_auBackup.BeforeDownloading, AddressOf Me.AutoUpdate_BeforeDownloading
                AddHandler m_auBackup.BeforeExtracting, AddressOf Me.AutoUpdate_BeforeExtracting

                m_auBackup.Initialize()
                m_auBackup.AppLoaded()

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

            If Not m_auBackup Is Nothing Then
                m_bAnnouceUpdates = bAnnouceUpdates
                m_auBackup.ForceCheckForUpdate(True)

                m_dtLastAutoUpdateTime = Now
                Util.SaveUserConfigData()
            End If

        End Sub

        Private Sub LoadUserConfig()
            Try
                Me.AppStatusText = "Loading user configuration"

                m_eDefaultLogLevel = DirectCast([Enum].Parse(GetType(ManagedAnimatInterfaces.ILogger.enumLogLevel), System.Configuration.ConfigurationManager.AppSettings("DefaultLogLevel"), True), ManagedAnimatInterfaces.ILogger.enumLogLevel)

                Util.LoadUserConfigData()

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
                Me.AppStatusText = "Initializing logging"

                m_Logger = CreateLogger()

                If Not Directory.Exists(Me.ApplicationDirectory & "Logs") Then
                    Directory.CreateDirectory(Me.ApplicationDirectory & "Logs")
                End If
                Me.LogDirectory = Me.ApplicationDirectory & "Logs\"

                Me.Logger.TraceLevel = m_eDefaultLogLevel
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Initialized Logging")

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub FindMdiClient()
            Dim c As Control

            Me.AppStatusText = "Finding mdi clients"

            For Each c In Me.Controls
                If TypeOf c Is MdiClient Then
                    m_mdiClient = c
                End If
            Next

            AddHandler m_mdiClient.Click, AddressOf Me.OnMdiClientClicked

            Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Found Mdi Client Window")
        End Sub

        Public Overridable Sub CheckSimRegistryEntry()

            Try

                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "I am about to open the 'software' registry subkey for read-only access!")
                'Util.ShowMessage("I am about to open the 'software' registry subkey for read-only access!")

                Dim rkSoftware As Microsoft.Win32.RegistryKey
                Try
                    rkSoftware = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", False)
                Catch ex As System.Exception
                    Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "Error opening 'software' for read-only access: " & ex.Message)
                    'Util.ShowMessage("Error opening 'software' for read-only access: " & ex.Message)
                    'If we get an error here then assume that we can not open the registry and jump out.
                    Return
                End Try

                'Util.ShowMessage("I am about to open the 'FLEXlm License Manager' registry subkey for read-only access!")

                Dim rkKey As Microsoft.Win32.RegistryKey = rkSoftware.OpenSubKey("FLEXlm License Manager", False)

                If rkKey Is Nothing Then

                    'Util.ShowMessage("'FLEXlm License Manager' subkey was not found. I am opening it for write access")
                    Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "'FLEXlm License Manager' subkey was not found. I am opening it for write access")

                    Try
                        rkSoftware = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software", True)
                    Catch ex As System.Exception
                        Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "Error opening 'software' for write access: " & ex.Message)
                        'Util.ShowMessage("Error opening 'software' for write access: " & ex.Message)
                        'If we get an error here then assume that we can not open the registry for writing and jump out.
                        Return
                    End Try

                    rkKey = rkSoftware.CreateSubKey("FLEXlm License Manager")
                End If

                'Util.ShowMessage("I am attempting to get the item names under 'FLEXlm License Manager'")
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "I am attempting to get the item names under 'FLEXlm License Manager'")

                Dim aryNames As String() = rkKey.GetValueNames()

                For Each strName As String In aryNames
                    If strName.Trim.ToUpper() = "MATHNGIN_LICENSE_FILE" Then
                        'Util.ShowMessage("'MATHNGIN_LICENSE_FILE' was found. I am exiting")
                        Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "'MATHNGIN_LICENSE_FILE' was found.")

                        Dim strVal As String = rkKey.GetValue("MATHNGIN_LICENSE_FILE", "").ToString
                        If strVal.Contains(Util.Application.ApplicationDirectory & "AnimatLab2.lic") Then
                            Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "Application Direcotry was found. exiting.")
                            Return
                        End If

                    End If
                Next

                'We may have gotten here by only opening the system to read, now lets try and open it for writing
                Try
                    'Util.ShowMessage("I am about to open the 'FLEXlm License Manager' registry subkey for write access!")
                    Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "I am about to open the 'FLEXlm License Manager' registry subkey for write access!")

                    rkKey = rkSoftware.OpenSubKey("FLEXlm License Manager", True)
                Catch ex As System.Exception
                    Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "Error opening 'FLEXlm' for write access: " & ex.Message)
                    'If we get an error here then assume that we can not open the registry for writing and jump out.
                    Return
                End Try

                'Util.ShowMessage("I am about to write the 'MATHNGIN_LICENSE_FILE' item")
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "I am about to write the 'MATHNGIN_LICENSE_FILE' item")

                'If we get here then the license file registry entry does not exist so lets add it.
                Dim strDir As String = Util.Application.ApplicationDirectory & "AnimatLab2.lic"
                'strDir = strDir.Substring(0, strDir.Length - 1)
                rkKey.SetValue("MATHNGIN_LICENSE_FILE", strDir)

                'Util.ShowMessage("I am finished messing with the registry")
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "I am finished messing with the registry")

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
                                           ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                           Optional ByVal bAsk As Boolean = True)

            If bAsk AndAlso Util.ShowMessage("The simulation must be restarted for this change to take effect.", "Confirm unit change", MessageBoxButtons.OKCancel) = Windows.Forms.DialogResult.Cancel Then
                Return
            End If

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

                SaveProject(Me.ProjectPath & Me.ProjectFile)
                LoadProject(Me.ProjectPath & Me.ProjectFile)

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
            Dim bDebugOutput As Boolean = False

            Me.AppStatusText = "Cataloging plugin modules"

            Try
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "Beginning to catalog plugin modules")

                Dim aryExcludeDlls As Hashtable = DirectCast(System.Configuration.ConfigurationManager.GetSection("DllExclusionList/Dlls"), Hashtable)

                Dim aryFileNames As New ArrayList

                m_aryAllDataTypes.Clear()
                m_aryNeuralModules.Clear()
                m_aryPlugInAssemblies.Clear()
                m_aryBehavioralNodes.Clear()
                m_aryBehavioralLinks.Clear()
                m_aryBodyPartTypes.Clear()
                m_aryRigidBodyTypes.Clear()
                m_aryJointTypes.Clear()
                m_aryToolPlugins.Clear()
                m_aryGainTypes.Clear()
                m_aryMacros.Clear()
                m_aryExternalStimuli.Clear()
                m_aryPhysicsEngines.Clear()
                m_aryMotorControlSystems.Clear()
                m_aryInputSystems.Clear()
                m_aryRobotPartInterfaces.Clear()
                m_aryRobotInterfaces.Clear()
                m_aryRobotIOControls.Clear()
                m_aryScriptProcessors.Clear()
                m_aryProjectMigrations.Clear()
                m_aryRemoteControlLinkages.Clear()
                m_aryAdapterPairs.Clear()
                m_aryLinkPairs.Clear()

                DataObjects.Physical.MaterialType.ClearRegisteredMaterialTypes()

                Util.DisableDirtyFlags = True

                'First find a list of all possible assemblies. It may be one or it may be a standard win32 dll. We will have to see later.
                Util.FindAssemblies(Me.ApplicationDirectory(), aryFileNames)

                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "Util.FindAssemblies")

                For Each oFile As Object In aryFileNames
                    strFile = DirectCast(oFile, String)
                    Dim strName As String = Util.ExtractFilename(strFile)

                    If Not aryExcludeDlls.ContainsKey(strName) Then
                        CatalogPluginModule(strFile, bDebugOutput, tpClass, iFailedLoad, strFailedLoad)
                    End If
                Next

                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "Initialize dataobjects after Application Start")

                InitializeDataObjectsAfterAppStart()

                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "Finished Looping through assemblies")

                CreateBehavioralPanels()

                SetupPartsExclusionsList()

                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Finished cataloging plugin modules")

            Catch ex As System.Exception
                If Not tpClass Is Nothing Then
                    Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "Error in CatalogPluginModules " & tpClass.FullName)
                Else
                    Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "Error in CatalogPluginModules. Type is nothing")
                End If

                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Util.DisableDirtyFlags = False

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

                    Util.ShowMessage(strMessage)
                End If
            End Try

        End Sub

        Protected Overridable Sub CatalogPluginModule(ByVal strFile As String, ByVal bDebugOutput As Boolean, _
                                                      ByRef tpClass As Type, ByRef iFailedLoad As Integer, ByRef strFailedLoad As String)
            Dim bAddModule As Boolean = False

            Try
                Dim assemModule As System.Reflection.Assembly = Util.LoadAssembly(Util.GetFilePath(Me.ApplicationDirectory, strFile), False)
                If Not assemModule Is Nothing Then

                    Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "About to get types: " & assemModule.FullName)

                    Dim aryTypes() As Type
                    Try
                        aryTypes = assemModule.GetTypes()
                    Catch ex As Exception
                        'If we have trouble gettting the object types then this is not 
                        'one of our dlls so skip it.
                        ReDim aryTypes(0)
                    End Try

                    Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "Starting to loop through: " & assemModule.FullName)

                    If assemModule.GetName().Name <> "UI Components" Then

                        For Each tpClass In aryTypes

                            If Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Behavior.Node)) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Behavior.Node")
                                Dim bnNode As DataObjects.Behavior.Node = CreateNode(assemModule, tpClass, Nothing)
                                If Not bnNode Is Nothing Then
                                    m_aryBehavioralNodes.Add(bnNode)
                                    m_aryAllDataTypes.Add(bnNode)
                                    bAddModule = True
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Behavior.Link)) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Behavior.Link")
                                Dim blLink As DataObjects.Behavior.Link = CreateLink(assemModule, tpClass, Nothing)
                                If Not blLink Is Nothing Then
                                    m_aryBehavioralLinks.Add(blLink)
                                    m_aryAllDataTypes.Add(blLink)
                                    bAddModule = True
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Behavior.NeuralModule)) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Behavior.NeuralModule")
                                Dim nmModule As DataObjects.Behavior.NeuralModule = CreateNeuralModule(assemModule, tpClass, Nothing)
                                If Not nmModule Is Nothing Then
                                    m_aryNeuralModules.Add(nmModule.ClassName, nmModule)
                                    m_aryAllDataTypes.Add(nmModule)
                                    bAddModule = True
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.Forms.Tools.ToolForm)) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.Forms.Tools.ToolForm")
                                Dim frmTool As Forms.Tools.ToolForm = CreateToolForm(assemModule, tpClass, Nothing)
                                If Not frmTool Is Nothing Then
                                    m_aryToolPlugins.Add(frmTool)
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Physical.RigidBody)) Then
                                Try
                                    If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Physical.RigidBody")
                                    Dim doPart As AnimatGUI.DataObjects.Physical.RigidBody = CreateRigidBody(assemModule, tpClass, Nothing)
                                    If Not doPart Is Nothing Then

                                        m_aryBodyPartTypes.Add(doPart)
                                        m_aryRigidBodyTypes.Add(doPart)
                                        m_aryAllDataTypes.Add(doPart)
                                        bAddModule = True
                                    End If
                                Catch ex As System.Exception
                                    Util.ShowMessage("Error loading rigid body part")
                                End Try
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Physical.Joint)) Then
                                Try
                                    If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Physical.Joint")
                                    Dim doPart As AnimatGUI.DataObjects.Physical.Joint = CreateJoint(assemModule, tpClass, Nothing)
                                    If Not doPart Is Nothing Then

                                        m_aryBodyPartTypes.Add(doPart)
                                        m_aryJointTypes.Add(doPart)
                                        m_aryAllDataTypes.Add(doPart)
                                        bAddModule = True
                                    End If
                                Catch ex As System.Exception
                                    Util.ShowMessage("Error loading joint part")
                                End Try
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Gain)) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Gain")
                                Dim doGain As DataObjects.Gain = CreateGain(assemModule, tpClass, Nothing)
                                If Not doGain Is Nothing Then
                                    m_aryGainTypes.Add(doGain)
                                    m_aryAllDataTypes.Add(doGain)
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Macro)) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Macro")
                                Dim doModule As DataObjects.Macro = CreateMacro(assemModule, tpClass, Nothing)
                                If Not doModule Is Nothing Then
                                    m_aryMacros.Add(doModule)
                                    m_aryAllDataTypes.Add(doModule)
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.ExternalStimuli.Stimulus), False) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.ExternalStimuli.Stimulus")
                                Dim doStim As DataObjects.ExternalStimuli.Stimulus = CreateExternalStimuli(assemModule, tpClass, Nothing)
                                If Not doStim Is Nothing Then
                                    m_aryExternalStimuli.Add(doStim, False)
                                    m_aryAllDataTypes.Add(doStim)
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.ProjectMigration), True) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.ProjectMigration")
                                Dim doConv As DataObjects.ProjectMigration = CreateProjectMigration(assemModule, tpClass, Nothing)
                                If Not doConv Is Nothing Then
                                    m_aryProjectMigrations.Add(doConv.ConvertFrom, doConv)
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Physical.MaterialType), True) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Physical.MaterialType")
                                Dim doConv As DataObjects.Physical.MaterialType = CreateMaterialType(assemModule, tpClass, Nothing)
                                If Not doConv Is Nothing Then
                                    doConv.RegisterMaterialType()
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Physical.PhysicsEngine), True) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Physical.PhysicsEngine")
                                Dim doConv As DataObjects.Physical.PhysicsEngine = CreatePhysicsEngine(assemModule, tpClass, Nothing)
                                If Not doConv Is Nothing Then
                                    m_aryPhysicsEngines.Add(doConv)
                                    m_aryAllDataTypes.Add(doConv)
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Robotics.MotorControlSystem), True) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Robotics.MotorControlSystem")
                                Dim doConv As DataObjects.Robotics.MotorControlSystem = CreateMotorControlSystem(assemModule, tpClass, Nothing)
                                If Not doConv Is Nothing Then
                                    m_aryMotorControlSystems.Add(doConv)
                                    m_aryRobotPartInterfaces.Add(doConv)
                                    m_aryAllDataTypes.Add(doConv)
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Robotics.InputSystem), True) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Robotics.InputSystem")
                                Dim doConv As DataObjects.Robotics.InputSystem = CreateInputSystem(assemModule, tpClass, Nothing)
                                If Not doConv Is Nothing Then
                                    m_aryInputSystems.Add(doConv)
                                    m_aryRobotPartInterfaces.Add(doConv)
                                    m_aryAllDataTypes.Add(doConv)
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Robotics.RobotInterface), True) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Robotics.RobotInterface")
                                Dim doConv As DataObjects.Robotics.RobotInterface = CreateRobotInterface(assemModule, tpClass, Nothing)
                                If Not doConv Is Nothing Then
                                    m_aryRobotInterfaces.Add(doConv)
                                    m_aryAllDataTypes.Add(doConv)
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Robotics.RobotIOControl), True) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Robotics.RobotIOControl")
                                Dim doConv As DataObjects.Robotics.RobotIOControl = CreateRobotIOControl(assemModule, tpClass, Nothing)
                                If Not doConv Is Nothing Then
                                    m_aryRobotIOControls.Add(doConv)
                                    m_aryAllDataTypes.Add(doConv)
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Scripting.ScriptProcessor), True) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Scripting.ScriptProcessor")
                                Dim doConv As DataObjects.Scripting.ScriptProcessor = CreateScriptProcessor(assemModule, tpClass, Nothing)
                                If Not doConv Is Nothing Then
                                    m_aryScriptProcessors.Add(doConv)
                                    m_aryAllDataTypes.Add(doConv)
                                End If
                            ElseIf Util.IsTypeOf(tpClass, GetType(AnimatGUI.DataObjects.Robotics.RemoteControlLinkage), True) Then
                                If bDebugOutput Then Debug.WriteLine("Working on AnimatGUI.DataObjects.Robotics.RemoteControlLinkage")
                                Dim doConv As AnimatGUI.DataObjects.Robotics.RemoteControlLinkage = CreateRemoteControlLinkage(assemModule, tpClass, Nothing)
                                If Not doConv Is Nothing Then
                                    m_aryRemoteControlLinkages.Add(doConv)
                                    m_aryAllDataTypes.Add(doConv)
                                End If
                            End If
                        Next

                        tpClass = Nothing
                    End If

                    Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "Finished looping through: " & assemModule.FullName)
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

        Protected Overridable Sub SetupPartsExclusionsList()

            For Each doPart As DataObjects.Physical.BodyPart In m_aryBodyPartTypes
                doPart.SetupPartTypesExclusions()
            Next
        End Sub

        Protected Overridable Function CreateNode(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Behavior.Node

            Try
                If Not tpClass.IsAbstract Then
                    Dim bnNode As DataObjects.Behavior.Node = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Behavior.Node)
                    Return bnNode
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    Util.ShowMessage("CreateNode: " & tpClass.FullName)
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
                    Util.ShowMessage("CreateLink: " & tpClass.FullName)
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
                    Util.ShowMessage("CreateNeuralModule: " & tpClass.FullName)
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
                    Util.ShowMessage("CreateToolForm: " & tpClass.FullName)
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
                    Util.ShowMessage("CreateRigidBody: " & tpClass.FullName)
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
                    Util.ShowMessage("CreateJoint: " & tpClass.FullName)
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
                    Util.ShowMessage("CreateGain: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateMacro(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Macro

            Try
                If Not tpClass.IsAbstract Then
                    Dim doModule As DataObjects.Macro = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Macro)
                    Return doModule
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    Util.ShowMessage("CreateMacro: " & tpClass.FullName)
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
                    Util.ShowMessage("CreateExternalStimuli: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateProjectMigration(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.ProjectMigration

            Try
                If Not tpClass.IsAbstract Then
                    Dim doConv As DataObjects.ProjectMigration = DirectCast(Util.LoadClass(assemModule, tpClass.FullName), DataObjects.ProjectMigration)
                    Return doConv
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    Util.ShowMessage("CreateProjectMigration: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateMaterialType(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Physical.MaterialType

            Try
                If Not tpClass.IsAbstract Then
                    Dim doConv As DataObjects.Physical.MaterialType = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Physical.MaterialType)
                    Return doConv
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    Util.ShowMessage("CreateMaterialType: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreatePhysicsEngine(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Physical.PhysicsEngine

            Try
                If Not tpClass.IsAbstract Then
                    Dim doConv As DataObjects.Physical.PhysicsEngine = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Physical.PhysicsEngine)
                    Return doConv
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    Util.ShowMessage("CreatePhysicsEngine: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreatePhysicsEngine(ByVal strName As String, ByVal strVersion As String, Optional ByVal bThrowException As Boolean = True) As DataObjects.Physical.PhysicsEngine

            For Each doEngine As DataObjects.Physical.PhysicsEngine In m_aryPhysicsEngines
                If doEngine.Name.ToUpper = strName.ToUpper Then
                    Dim doRetEngine As DataObjects.Physical.PhysicsEngine = DirectCast(doEngine.Clone(Nothing, False, Nothing), DataObjects.Physical.PhysicsEngine)
                    doRetEngine.SetLibraryVersion(strVersion, True)
                    Return doRetEngine
                End If
            Next

            If bThrowException Then
                Throw New System.Exception("No physics engine with the name '" & strName & "' was found in the plug-in modules.")
            End If

            Return Nothing
        End Function

        Protected Overridable Function CreateMotorControlSystem(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Robotics.MotorControlSystem

            Try
                If Not tpClass.IsAbstract Then
                    Dim doConv As DataObjects.Robotics.MotorControlSystem = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Robotics.MotorControlSystem)
                    Return doConv
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    Util.ShowMessage("CreateMotorControlSystem: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateInputSystem(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Robotics.InputSystem

            Try
                If Not tpClass.IsAbstract Then
                    Dim doConv As DataObjects.Robotics.InputSystem = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Robotics.InputSystem)
                    Return doConv
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    Util.ShowMessage("CreateInputSystem: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateRobotInterface(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Robotics.RobotInterface

            Try
                If Not tpClass.IsAbstract Then
                    Dim doConv As DataObjects.Robotics.RobotInterface = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Robotics.RobotInterface)
                    Return doConv
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    Util.ShowMessage("CreateRobotInterface: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateRobotIOControl(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Robotics.RobotIOControl

            Try
                If Not tpClass.IsAbstract Then
                    Dim doConv As DataObjects.Robotics.RobotIOControl = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Robotics.RobotIOControl)
                    Return doConv
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    Util.ShowMessage("CreateRobotIOControl: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateScriptProcessor(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As DataObjects.Scripting.ScriptProcessor

            Try
                If Not tpClass.IsAbstract Then
                    Dim doConv As DataObjects.Scripting.ScriptProcessor = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), DataObjects.Scripting.ScriptProcessor)
                    Return doConv
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    Util.ShowMessage("CreateScriptProcessor: " & tpClass.FullName)
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End If
            End Try

        End Function

        Protected Overridable Function CreateRemoteControlLinkage(ByVal assemModule As System.Reflection.Assembly, ByVal tpClass As System.Type, ByVal doParent As AnimatGUI.Framework.DataObject) As AnimatGUI.DataObjects.Robotics.RemoteControlLinkage

            Try
                If Not tpClass.IsAbstract Then
                    Dim doConv As AnimatGUI.DataObjects.Robotics.RemoteControlLinkage = DirectCast(Util.LoadClass(assemModule, tpClass.FullName, doParent), AnimatGUI.DataObjects.Robotics.RemoteControlLinkage)
                    Return doConv
                End If
            Catch ex As System.Exception
                If ex.Message <> "Cannot create an abstract class." Then
                    Util.ShowMessage("CreateRemoteControlLinkage: " & tpClass.FullName)
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
                Dim ipPanel As AnimatGuiCtrls.Controls.IconPanel
                For Each pdPanel As DataObjects.Behavior.PanelData In Util.Application.AlphabeticalBehavioralPanels

                    ipPanel = New AnimatGuiCtrls.Controls.IconPanel
                    ipPanel.IconHeight = 55
                    m_frmToolbox.OutlookBar.AddBand(pdPanel.m_strPanelName, ipPanel)

                    For Each bnNode As DataObjects.Behavior.Node In pdPanel.m_aryNodes
                        ipPanel.AddIcon(bnNode.Name, bnNode.WorkspaceImage, bnNode.DragImage, bnNode)
                        bnNode.AfterAddedToIconBand()
                    Next

                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        'This adds a parent/child body part relationship that is excluded from being able to happen.
        Public Overridable Sub AddPartTypeExclusion(ByVal tpParent As System.Type, ByVal tpChild As System.Type)
            Dim strKey As String = tpParent.ToString & "_" & tpChild.ToString

            If Not m_aryPartTypeExclusions.Contains(strKey) Then
                m_aryPartTypeExclusions.Add(strKey)
            End If
        End Sub

        Public Overridable Sub RemovePartTypeExclusion(ByVal tpParent As System.Type, ByVal tpChild As System.Type)
            Dim strKey As String = tpParent.ToString & "_" & tpChild.ToString

            If m_aryPartTypeExclusions.Contains(strKey) Then
                m_aryPartTypeExclusions.Remove(strKey)
            End If
        End Sub

        'This checks if a parent/child body part relationship is possible or not.
        Public Overridable Overloads Function CanAddPartAsChild(ByVal tpParent As System.Type, ByVal tpChild As System.Type) As Boolean
            Dim strKey As String = tpParent.ToString & "_" & tpChild.ToString
            Return Not m_aryPartTypeExclusions.Contains(strKey)
        End Function

#End Region

#Region " Menu/Toolbar Creation "

        Protected Overridable Sub CreateImageManager()

            Me.AppStatusText = "Creating new image manager"

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

        Public Overridable Sub UpdateToolstrips()

            Me.AppStatusText = "Updating toolbars"

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

                Me.RunSimulationToolStripMenuItem.Enabled = True
                Me.CustomizeToolStripMenuItem.Enabled = True
                Me.ConsoleToolStripMenuItem.Enabled = True

                Me.DeleteToolStripButton.Enabled = True
                Me.DeleteToolStripMenuItem.Enabled = True

                Me.SnapshotSimToolStripMenuItem.Enabled = True

                Me.AddItemToolStripMenuItem.Visible = True
                Me.AddItemToolStripMenuItem.Enabled = True
                Me.AddOrganismStripButton.Enabled = True
                Me.AddOrganismToolStripMenuItem.Enabled = True
                Me.AddStructureToolStripButton.Enabled = True
                Me.AddStructureToolStripMenuItem.Enabled = True
                Me.AddToolToolStripButton.Enabled = True
                Me.AddTooToolStripMenuItem.Enabled = True
                Me.AddBodyJointToolStripMenuItem.Enabled = True
                Me.AddPartToolStripButton.Enabled = True
                Me.AddBodyPartToolStripMenuItem.Enabled = True
                Me.AddJointToolStripButton.Enabled = True
                Me.AddBodyJointToolStripMenuItem.Enabled = True
                Me.AddStimulusToolStripButton.Enabled = True
                Me.AddStimulusToolStripMenuItem.Enabled = True

                Me.SelectByTypeToolStripButton.Enabled = True
                Me.SelectByTypeToolStripMenuItem.Enabled = True
                Me.RelabelSelectedToolStripButton.Enabled = True
                Me.RelabelSelectedToolStripMenuItem.Enabled = True
                Me.CompareItemsToolStripButton.Enabled = True
                Me.CompareItemsToolStripMenuItem.Enabled = True

                Me.SelGraphicsToolStripButton.Enabled = True
                Me.GraphicsObjectsToolStripMenuItem.Enabled = True
                Me.SelCollisionToolStripButton.Enabled = True
                Me.CollisionObjectsToolStripMenuItem.Enabled = True
                Me.JointsToolStripMenuItem.Enabled = True
                Me.SelJointsToolStripButton.Enabled = True
                Me.ReceptiveFieldsToolStripMenuItem.Enabled = True
                Me.SelRecFieldsToolStripButton.Enabled = True
                Me.SelSimToolStripButton.Enabled = True
                Me.SimulationToolStripMenuItem.Enabled = True

                Me.DisplayModeDropDown.Enabled = True
                Me.SelectionModeToolStripMenuItem.Enabled = True
                Me.DisplayToolStripMenuItem.Enabled = True

                Me.EditMaterialsToolStripButton.Enabled = True
                Me.EditMaterialsToolStripMenuItem.Enabled = True
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

                Me.RunSimulationToolStripMenuItem.Enabled = False
                Me.CustomizeToolStripMenuItem.Enabled = False

                Me.DeleteToolStripButton.Enabled = False
                Me.DeleteToolStripMenuItem.Enabled = False

                Me.AddItemToolStripMenuItem.Visible = False
                Me.AddItemToolStripMenuItem.Enabled = False
                Me.AddOrganismStripButton.Enabled = False
                Me.AddOrganismToolStripMenuItem.Enabled = False
                Me.AddStructureToolStripButton.Enabled = False
                Me.AddStructureToolStripMenuItem.Enabled = False
                Me.AddToolToolStripButton.Enabled = False
                Me.AddTooToolStripMenuItem.Enabled = False
                Me.AddBodyJointToolStripMenuItem.Enabled = False
                Me.AddPartToolStripButton.Enabled = False
                Me.AddBodyPartToolStripMenuItem.Enabled = False
                Me.AddJointToolStripButton.Enabled = False
                Me.AddBodyJointToolStripMenuItem.Enabled = False
                Me.AddStimulusToolStripButton.Enabled = False
                Me.AddStimulusToolStripMenuItem.Enabled = False

                Me.SelectByTypeToolStripButton.Enabled = False
                Me.SelectByTypeToolStripMenuItem.Enabled = False
                Me.RelabelSelectedToolStripButton.Enabled = False
                Me.RelabelSelectedToolStripMenuItem.Enabled = False
                Me.CompareItemsToolStripButton.Enabled = False
                Me.CompareItemsToolStripMenuItem.Enabled = False

                Me.SnapshotSimToolStripMenuItem.Enabled = False

                Me.SelGraphicsToolStripButton.Enabled = False
                Me.GraphicsObjectsToolStripMenuItem.Enabled = False
                Me.SelCollisionToolStripButton.Enabled = False
                Me.CollisionObjectsToolStripMenuItem.Enabled = False
                Me.JointsToolStripMenuItem.Enabled = False
                Me.SelJointsToolStripButton.Enabled = False
                Me.ReceptiveFieldsToolStripMenuItem.Enabled = False
                Me.SelRecFieldsToolStripButton.Enabled = False
                Me.SelSimToolStripButton.Enabled = False
                Me.SimulationToolStripMenuItem.Enabled = False

                Me.DisplayModeDropDown.Enabled = False
                Me.SelectionModeToolStripMenuItem.Enabled = False
                Me.DisplayToolStripMenuItem.Enabled = False

                Me.EditMaterialsToolStripButton.Enabled = False
                Me.EditMaterialsToolStripMenuItem.Enabled = False
            End If

            If m_SecurityMgr.IsValidSerialNumber AndAlso Not m_SecurityMgr.IsEvaluationLicense Then
                Me.RegisterStripMenuItem.Visible = False
                Me.toolStripSeparatorHelp2.Visible = False
            Else
                Me.RegisterStripMenuItem.Visible = True
                Me.toolStripSeparatorHelp2.Visible = True
            End If

        End Sub

#End Region

#Region " Project Creation "

        Public Overridable Sub ResetProject(ByVal bNewProject As Boolean)
            Me.AppStatusText = "Resetting project"

            CloseProject(bNewProject)

            Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "Starting Reset Project")

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
                m_dockManager.AutoHideContents(cc, m_wcToolboxContent.State, m_wcToolboxContent.CurrentContent)

                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                  "AnimatGUI.Forms.ProjectProperties", _
                                  "Properties", "Properties", True, _
                                  , m_wcPropertiesContent, , , , 200)
                m_frmProperties = DirectCast(afForm, Forms.ProjectProperties)

                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                  "AnimatGUI.Forms.ProjectWorkspace", _
                                  "Workspace", "Workspace", True, _
                                  , m_wcWorkspaceContent, , m_wcPropertiesContent.ParentZone, , 200)
                m_frmWorkspace = DirectCast(afForm, Forms.ProjectWorkspace)

                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                "AnimatGUI.Forms.SimulationController", _
                                "Simulation Controller", "SimulationController", _
                                True, State.DockBottom, m_wcSimControllerContent, , , , , 125)
                m_frmSimulationController = DirectCast(afForm, Forms.SimulationController)

                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                           "AnimatGUI.Forms.Errors", _
                                           "Errors", "Errors", _
                                           True, State.DockBottom, m_wcErrorsContent, m_wcSimControllerContent, , , , 125)
                m_frmErrors = DirectCast(afForm, Forms.Errors)

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

            Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Finished Reset Project")
        End Sub


        'It is possible that if a project was saved with an earlier version of animatlab that a new toolbar may have been
        'added. If that is true it will not be part of the project config file. The docking bar created in ResetToolbars
        'will be deleted and it will not be recreated during the load. This method checks each of the major docking bars
        'after the load and makes sure they still exist. If it does not then it recreates it.
        Protected Sub VerifyToolbarsAfterLoad()

            Me.AppStatusText = "Verifying toolbars"

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
                                  True, State.DockBottom, m_wcSimControllerContent, , , , , 125)
                m_frmSimulationController = DirectCast(afForm, Forms.SimulationController)
            End If

            ctDock = m_dockManager.Contents("Errors")
            If ctDock Is Nothing Then
                afForm = CreateDockingForm(m_dockManager, "AnimatGUI.dll", _
                                  "AnimatGUI.Forms.Errors", _
                                  "Errors", "Errors", _
                                  True, State.DockBottom, m_wcErrorsContent, m_wcSimControllerContent, , , , 125)
                m_frmErrors = DirectCast(afForm, Forms.Errors)
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

        Protected Overridable Sub HideAllContentWidows()

            If Not m_frmWorkspace Is Nothing AndAlso Not m_frmWorkspace.Content Is Nothing AndAlso Not m_frmWorkspace.Content.IsAutoHidden Then
                m_dockManager.ToggleContentAutoHide(m_frmWorkspace.Content)
            End If

            If Not m_frmProperties Is Nothing AndAlso Not m_frmProperties.Content Is Nothing AndAlso Not m_frmProperties.Content.IsAutoHidden Then
                m_dockManager.ToggleContentAutoHide(m_frmProperties.Content)
            End If

            If Not m_frmToolbox Is Nothing AndAlso Not m_frmToolbox.Content Is Nothing AndAlso Not m_frmToolbox.Content.IsAutoHidden Then
                m_dockManager.ToggleContentAutoHide(m_frmToolbox.Content)
            End If

            If Not m_frmSimulationController Is Nothing AndAlso Not m_frmSimulationController.Content Is Nothing AndAlso Not m_frmSimulationController.Content.IsAutoHidden Then
                m_dockManager.ToggleContentAutoHide(m_frmSimulationController.Content)
            End If

            If Not m_frmErrors Is Nothing AndAlso Not m_frmErrors.Content Is Nothing AndAlso Not m_frmErrors.Content.IsAutoHidden Then
                m_dockManager.ToggleContentAutoHide(m_frmErrors.Content)
            End If

            If Not m_frmReceptiveFieldPairs Is Nothing AndAlso Not m_frmReceptiveFieldPairs.Content Is Nothing AndAlso Not m_frmReceptiveFieldPairs.Content.IsAutoHidden Then
                m_dockManager.ToggleContentAutoHide(m_frmReceptiveFieldPairs.Content)
            End If

            If Not m_frmReceptiveFieldGain Is Nothing AndAlso Not m_frmReceptiveFieldGain.Content Is Nothing AndAlso Not m_frmReceptiveFieldGain.Content.IsAutoHidden Then
                m_dockManager.ToggleContentAutoHide(m_frmReceptiveFieldGain.Content)
            End If

            If Not m_frmReceptiveFieldCurrent Is Nothing AndAlso Not m_frmReceptiveFieldCurrent.Content Is Nothing AndAlso Not m_frmReceptiveFieldCurrent.Content.IsAutoHidden Then
                m_dockManager.ToggleContentAutoHide(m_frmReceptiveFieldCurrent.Content)
            End If

        End Sub

        Public Overridable Sub CloseProject(ByVal bOpeningProject As Boolean, Optional ByVal bCloseQuiet As Boolean = False)
            Me.AppStatusText = "Closing project"

            If Not bCloseQuiet Then
                If SaveIfDirty() = DialogResult.Cancel Then
                    Me.AppStatusText = "Canceling project close"
                    Return
                End If
            End If

            Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Closing project")

            If Not m_dockManager Is Nothing Then
                ClearDockingContents()
            End If

            ClearChildForms()

            'If we have a simulation up and running then completely shut it down and start over for the new project
            'This will need to be changed
            Me.AppStatusText = "Shutting down simulation"
            Me.SimulationInterface.ShutdownSimulation()

            CreateImageManager()

            ' Create the object that manages the docking state
            Me.AppStatusText = "Creating new docking manager"

            m_dockManager = New Crownwood.DotNetMagic.Docking.DockingManager(Me.AnimatStripContainer.ContentPanel, VisualStyle.Office2007Blue)
            m_dockManager.AllowFloating = False
            m_dockManager.OuterControl = Me.StatusBar

            m_frmWorkspace = Nothing
            m_frmToolbox = Nothing
            m_frmErrors = Nothing
            m_frmSimulationController = Nothing

            m_strPhysicsAssemblyName = "AnimatGUI.dll"
            m_strPhysicsClassName = "AnimatGUI.DataObjects.Simulation"

            If Not m_doSimulation Is Nothing Then m_doSimulation.RemoveFromSim(False)
            Me.Simulation = New DataObjects.Simulation(Me.FormHelper)

            m_ModificationHistory = New AnimatGUI.Framework.UndoSystem.ModificationHistory

            UpdateToolstrips()

            Me.ClearIsDirty()

            If Me.SecurityMgr.IsValidSerialNumber Then
                Me.Title = "AnimatLab Pro"
            Else
                Me.Title = "AnimatLab"
            End If

            Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Closed current project")
        End Sub

        Public Overridable Function SaveStandAlone(ByVal bIncludeCharts As Boolean, ByVal bIncludeStims As Boolean, ByVal bSaveChartsToFile As Boolean, ByVal bIncludeWindows As Boolean, _
                                                   ByVal doPhysics As DataObjects.Physical.PhysicsEngine, ByVal doRobotInterface As DataObjects.Robotics.RobotInterface) As ManagedAnimatInterfaces.IStdXml
            Dim doOrigPhysics As DataObjects.Physical.PhysicsEngine = m_doPhysics

            Try
                Me.AppIsBusy = True

                Util.DisableDirtyFlags = True
                Util.ExportForStandAloneSim = True
                Util.ExportChartsToFile = bSaveChartsToFile
                Util.ExportChartsInStandAloneSim = bIncludeCharts
                Util.ExportStimsInStandAloneSim = bIncludeStims
                Util.ExportWindowsToFile = bIncludeWindows
                Util.ExportRobotInterface = doRobotInterface
                Util.ExportPhysicsEngine = doPhysics

                If Not Util.ExportPhysicsEngine Is Nothing Then
                    m_doPhysics = Util.ExportPhysicsEngine
                End If

                Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

                Util.Simulation.SaveSimulationXml(oXml, Nothing)

                Return oXml
            Catch ex As System.Exception
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Unable to save project:")
                Throw ex
            Finally
                Util.DisableDirtyFlags = False
                Util.ExportForStandAloneSim = False
                Util.ExportChartsInStandAloneSim = False
                Util.ExportStimsInStandAloneSim = False
                Util.ExportWindowsToFile = False
                Util.ExportRobotInterface = Nothing
                m_doPhysics = doOrigPhysics
                Me.AppIsBusy = False
            End Try

        End Function

        Public Overridable Sub SaveProject(ByVal strFilename As String, Optional ByVal bOverrideProjectIsOpen As Boolean = False)

            Try
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Starting Save of project: '" & Util.Application.ProjectPath & "\" & strFilename & "'")
                Me.AppStatusText = "Saving project"

                If Not m_bProjectIsOpen AndAlso Not bOverrideProjectIsOpen Then
                    Throw New System.Exception("You must have an open project before you can save it.")
                End If

                Me.AppIsBusy = True

                Dim oXml As ManagedAnimatInterfaces.IStdXml = CreateStdXml()

                Me.ClearIsDirty()

                If Util.IsFullPath(strFilename) Then
                    Util.SplitPathAndFile(strFilename, m_strProjectPath, m_strProjectFile)
                End If

                If m_strProjectName.Trim.Length = 0 Then
                    m_strProjectName = m_strProjectFile.Substring(0, m_strProjectFile.Length - 6)
                End If

                Util.DisableDirtyFlags = True
                SaveData(oXml)

                Me.AppStatusText = "Saving xml file"
                oXml.Save(Util.GetFilePath(Util.Application.ProjectPath, strFilename))
                Util.DisableDirtyFlags = False

                RaiseEvent ProjectSaved()

                Me.AppStatusText = "Project saved"
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Finished successful save of project: '" & Util.Application.ProjectPath & "\" & strFilename & "'")

            Catch ex As System.Exception
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Unable to save project: '" & Util.Application.ProjectPath & "\" & strFilename & "'")
                Throw ex
            Finally
                Util.DisableDirtyFlags = False
                Me.AppIsBusy = False
            End Try
        End Sub

        Public Overridable Sub LoadProject(ByVal strFilename As String)

            Try
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Starting load of project: '" & strFilename & "'")
                Me.AppStatusText = "Starting project load"

                Me.AppIsBusy = True

                ' Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()
                Dim oXml As ManagedAnimatInterfaces.IStdXml = CreateStdXml()

                Dim strProjPath As String
                Dim strProjFile As String
                Util.SplitPathAndFile(strFilename, strProjPath, strProjFile)
                Me.ProjectPath = strProjPath
                Me.ProjectFile = strProjFile

                Directory.SetCurrentDirectory(m_strProjectPath)

                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "Loading xml")
                oXml.Load(strFilename)
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "Loaded xml")
                Me.AppStatusText = "Loaded xml"

                oXml.FindElement("Project")
                oXml.FindChildElement("")

                Util.DisableDirtyFlags = True
                LoadData(oXml)
                VerifyToolbarsAfterLoad()
                Util.DisableDirtyFlags = False

                Me.ClearIsDirty()

                Dim strPro As String = ""
                If SecurityMgr.IsValidSerialNumber Then strPro = "Pro "
                Me.Title = "AnimatLab " & strPro & Me.ProjectName & " Project"

                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Finished successful load of project: '" & strFilename & "'")

                UpdateToolstrips()
                RaiseEvent ProjectLoaded()

                m_bProjectIsOpen = True
                UpdateToolstrips()

                Me.AppStatusText = "Load project complete"

            Catch exOldVersion As OldProjectVersion
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Attempted to load an old project version file: '" & strFilename & "', Old Version: " & exOldVersion.OldVersion)
                Me.CloseProject(False)

                Dim frmConvert As New ConvertPhysics
                frmConvert.lblSaveMessage.Text = "The project you are attempting to load was built with a previous version of AnimatLab. Would you like to convert it to " & _
                                 "be able to run in this version of the application? A backup of all old files will be made in a seperate folder of the project. "
                frmConvert.Text = "Convert Project"
                frmConvert.lblCurrentPhysics.Visible = False
                frmConvert.lblNewPhysics.Text = "Please choose the physics engine for the conversion"
                frmConvert.ShowAllPhysicsOptions = True
                If frmConvert.ShowDialog() = Windows.Forms.DialogResult.Yes Then
                    RaiseEvent ConvertFileVersion(strFilename, exOldVersion.OldVersion, frmConvert.cboPhysicsEngine.SelectedItem.ToString)
                Else
                    Throw exOldVersion
                End If
            Catch ex As System.Exception
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Unable to load project: '" & strFilename & "'")
                Throw ex
            Finally
                Util.DisableDirtyFlags = False
                Me.AppIsBusy = False
            End Try
        End Sub

        Protected Overridable Sub OnExportStandaloneSim(ByVal sender As Object, ByVal e As EventArgs) Handles ExportStandaloneToolStripMenuItem.Click

            Try
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Starting Save of stand alone config file.")

                If Not m_bProjectIsOpen Then
                    Throw New System.Exception("You must have an open project before you can save it.")
                End If

                Dim frmExport As New ExportStandaloneSim()
                frmExport.Physics = Util.Application.Physics
                If frmExport.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                    Return
                End If

                Me.AppIsBusy = True

                Me.ClearIsDirty()

                Dim strFilename As String = frmExport.txtProjectName.Text

                Util.DisableDirtyFlags = True

                Dim oXml As ManagedAnimatInterfaces.IStdXml = SaveStandAlone(True, True, True, frmExport.chkShowGraphics.Checked, frmExport.Physics, Nothing)
                oXml.Save(Util.Application.ProjectPath & "\" & strFilename)

                Util.DisableDirtyFlags = False

                Util.ShowMessage("Standalone Control Files Created")

                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Finished successful save of standalone config file")

            Catch ex As System.Exception
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Unable to save standalone config file.")
                Throw ex
            Finally
                Util.ExportForStandAloneSim = False
                Util.DisableDirtyFlags = False
                Util.ExportChartsInStandAloneSim = False
                Util.ExportStimsInStandAloneSim = False
                Util.ExportChartsToFile = False
                Util.ExportWindowsToFile = False
                Me.AppIsBusy = False
            End Try
        End Sub

        Public Overridable Sub ExportStandAloneSim(ByVal strProjectFile As String)


            Try
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Starting Save of stand alone config file.")

                If Not m_bProjectIsOpen Then
                    Throw New System.Exception("You must have an open project before you can save it.")
                End If

                Me.AppIsBusy = True

                Me.ClearIsDirty()

                Dim strFilename As String = strProjectFile

                Util.DisableDirtyFlags = True
                Util.ExportForStandAloneSim = True
                Util.ExportChartsInStandAloneSim = True
                Util.ExportStimsInStandAloneSim = True
                Util.ExportChartsToFile = True
                Util.ExportWindowsToFile = True

                Util.Simulation.SaveSimulationXml(strFilename)

                Util.DisableDirtyFlags = False

                'Util.ShowMessage("Standalone Control Files Created")

                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Finished successful save of standalone config file")

            Catch ex As System.Exception
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Unable to save standalone config file.")
                Throw ex
            Finally
                Util.ExportForStandAloneSim = False
                Util.DisableDirtyFlags = False
                Util.ExportChartsInStandAloneSim = False
                Util.ExportStimsInStandAloneSim = False
                Util.ExportChartsToFile = False
                Util.ExportWindowsToFile = False
                Me.AppIsBusy = False
            End Try

        End Sub

        Public Overridable Sub ExportDataCharts(Optional ByVal strFileName As String = "", Optional ByVal strPrefix As String = "")
            Dim bRetVal As Boolean = True

            Try
                Me.AppIsBusy = True
                m_bAutomation_ExportedChartData = False

                Dim frmChart As Tools.DataChart
                For Each frmAnimat As AnimatForm In Me.ChildForms
                    If Util.IsTypeOf(frmAnimat.GetType(), GetType(Tools.DataChart), False) Then
                        frmChart = DirectCast(frmAnimat, Tools.DataChart)

                        If Not frmChart.ExportChartData(strFileName, strPrefix) Then
                            bRetVal = False
                        End If
                    End If
                Next

            Catch ex As Exception
                Throw ex
            Finally
                Me.AppIsBusy = False
            End Try

            m_bAutomation_ExportedChartData = bRetVal
        End Sub

        Public Overridable Sub CopyChartData(ByVal strPhysicsEngine As String, Optional ByVal strPath As String = "", Optional ByVal strPrefix As String = "")

            Dim frmChart As Tools.DataChart
            For Each frmAnimat As AnimatForm In Me.ChildForms

                If Util.IsTypeOf(frmAnimat.GetType(), GetType(Tools.DataChart), False) Then
                    frmChart = DirectCast(frmAnimat, Tools.DataChart)
                    frmChart.CopyChartData(strPhysicsEngine, strPath, strPrefix)
                End If
            Next
        End Sub

        Public Overridable Sub CompareExportedDataCharts(ByVal strPhysicsEngine As String, ByVal strPrefix As String, ByVal strTemplatePath As String, _
                                                         ByVal aryMaxErrors As Hashtable, ByVal iMaxRows As Integer, ByVal aryIgnoreRows As ArrayList)

            Dim frmChart As Tools.DataChart
            For Each frmAnimat As AnimatForm In Me.ChildForms

                If Util.IsTypeOf(frmAnimat.GetType(), GetType(Tools.DataChart), False) Then
                    frmChart = DirectCast(frmAnimat, Tools.DataChart)
                    frmChart.CompareExportedData(strPhysicsEngine, strPrefix, strTemplatePath, aryMaxErrors, iMaxRows, aryIgnoreRows)
                End If
            Next

        End Sub

#End Region

#Region " Load/Save Methods "

        Public Overloads Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            CloseProject(True)

            Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "LoadData starting")
            Me.AppStatusText = "Loading project data"

            Try
                Util.LoadInProgress = True
                Me.AppIsBusy = True

                m_aryDeleteAfterLoad.Clear()
                Dim iVersion As Integer = oXml.GetChildInt("Version", 1)

                If iVersion < Me.XmlVersion Then
                    Throw New OldProjectVersion("You cannot open project files from previous versions of the application.", iVersion)
                End If

                m_strProjectName = oXml.GetChildString("ProjectName")
                m_strSimulationFile = oXml.GetChildString("SimulationFile", "")
                Dim eLogLevel As ManagedAnimatInterfaces.ILogger.enumLogLevel = DirectCast([Enum].Parse(GetType(ManagedAnimatInterfaces.ILogger.enumLogLevel), oXml.GetChildString("LogLevel", "None"), True), ManagedAnimatInterfaces.ILogger.enumLogLevel)

                If eLogLevel <> Me.Logger.TraceLevel Then
                    Me.Logger.TraceLevel = eLogLevel
                End If

                Dim doOldEngine As DataObjects.Physical.PhysicsEngine = m_doPhysics
                Dim strVersion As String = oXml.GetChildString("LibraryVersion", "Double")
                m_doPhysics = CreatePhysicsEngine(oXml.GetChildString("Physics", "Vortex"), strVersion)

                'If the physics engine has changed then recreate the list of plugin modules with the new physics engine.
                If Not doOldEngine Is m_doPhysics Then
                    CatalogPluginModules()
                    doOldEngine = Nothing
                End If

                Me.Simulation = New DataObjects.Simulation(Me.FormHelper)
                If m_strSimulationFile.Trim.Length > 0 Then
                    Try
                        m_doSimulation.LoadData(oXml)

                        'Now initialize after load
                        m_doSimulation.InitializeAfterLoad()
                        DeleteItemsAfterLoadFinished()

                    Catch ex As System.Exception
                        AnimatGUI.Framework.Util.DisplayError(ex)
                    End Try
                End If

                'Start the simulation running
                Me.CreateSimulation(True)

                LoadDockingForms(m_dockManager, oXml)
                LoadDockingConfig(m_dockManager, oXml)

                Util.Simulation.NewToolHolderIndex = Util.ExtractIDCount("DataTool", Util.Simulation.ToolHolders)

                Me.AppStatusText = "Finished loading project data"

            Catch ex As System.Exception
                Throw ex
            Finally
                Me.AppIsBusy = False
                Util.LoadInProgress = False
            End Try

        End Sub

        Public Overridable Sub LoadDockingForms(ByRef dockManager As Crownwood.DotNetMagic.Docking.DockingManager, _
                                                 ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            Try
                Me.AppStatusText = "Loading docking forms"

                'Then create the forms. They will create their own sim references as they are loaded
                If oXml.FindChildElement("DockingForms", False) Then
                    oXml.IntoChildElement("DockingForms") 'Into DockingForms Element
                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    For iIndex As Integer = 0 To iCount
                        oXml.FindChildByIndex(iIndex)
                        LoadDockingForm(m_dockManager, oXml)
                    Next
                    oXml.OutOfElem()   'Outof DockingForms Element
                End If
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Sub LoadDockingConfig(ByRef dockManager As Crownwood.DotNetMagic.Docking.DockingManager, _
                                                 ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                Me.AppStatusText = "Loading docking configuration"
                Me.AppIsBusy = True

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
            Finally
                Me.AppIsBusy = False
            End Try

        End Sub

        Public Overridable Sub LoadDockingForm(ByRef dockManager As DockingManager, _
                                               ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                Me.AppIsBusy = True

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
                ElseIf strClass = "AnimatGUI.Forms.Errors" Then
                    m_frmErrors = DirectCast(frmDock, Forms.Errors)
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
            Finally
                Me.AppIsBusy = False
            End Try

        End Sub

        Public Overloads Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                Util.SaveInProgress = True
                Me.AppStatusText = "Saving project data"

                oXml.AddElement("Project")

                If Me.SimulationFile.Trim.Length = 0 Then
                    Me.SimulationFile = m_strProjectName & ".asim"
                End If

                oXml.AddChildElement("ProjectName", m_strProjectName)
                oXml.AddChildElement("SimulationFile", Me.SimulationFile)
                oXml.AddChildElement("LogLevel", Me.Logger.TraceLevel.ToString)
                oXml.AddChildElement("Version", Me.XmlVersion)
                oXml.AddChildElement("Physics", Me.Physics.Name)
                oXml.AddChildElement("LibraryVersion", Me.Physics.LibraryVersion.ID)

                m_doSimulation.SaveData(oXml)

                oXml.AddChildElement("DockingForms")
                oXml.IntoElem()   'Into DockingForms Element

                'First lets save all Docking Forms associated with this application.
                Util.Application.AppStatusText = "Saving docking windows"
                Dim frmAnimat As AnimatForm
                For Each conWindow As Content In m_dockManager.Contents
                    frmAnimat = DirectCast(conWindow.Control, AnimatForm)
                    frmAnimat.SaveData(oXml)
                Next
                oXml.OutOfElem()   'Outof DockingForms Element

                oXml.OutOfElem()   'Outof Stimuli element

                oXml.OutOfElem()   'Outof Project Element

                SaveDockingConfig(m_dockManager, oXml)

            Catch ex As System.Exception
                Throw ex
            Finally
                Util.SaveInProgress = False
            End Try
        End Sub

        Public Overridable Sub SaveDockingConfig(ByRef dockManager As DockingManager, _
                                                 ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Util.Application.AppStatusText = "Saving docking configuration"

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
                eResult = Util.ShowMessage("There are unsaved changes in the project. " & _
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

        Public Overridable Sub DeleteItemAfterLoading(ByVal doItem As Framework.DataObject)
            m_aryDeleteAfterLoad.Add(doItem)
        End Sub

        Protected Overridable Sub DeleteItemsAfterLoadFinished()
            For Each doItem As Framework.DataObject In m_aryDeleteAfterLoad
                doItem.Delete(False)
            Next

            m_aryDeleteAfterLoad.Clear()
        End Sub

        Public Overridable Sub AddLinkPair(ByVal strOriginType As String, ByVal strDestinationType As String, ByVal strLinkType As String)
            Dim doNewLinkPair As New DataObjects.Behavior.LinkPair(strOriginType, strDestinationType, strLinkType)

            If FindLinkPair(strOriginType, strDestinationType, strLinkType) Is Nothing Then
                m_aryLinkPairs.Add(doNewLinkPair)
            End If

        End Sub

        Public Overridable Sub RemoveLinkPair(ByVal strOriginType As String, ByVal strDestinationType As String, ByVal strLinkType As String)
            Dim doPair As DataObjects.Behavior.LinkPair = FindLinkPair(strOriginType, strDestinationType, strLinkType)

            If Not doPair Is Nothing Then
                m_aryLinkPairs.Remove(doPair)
            End If

        End Sub

        Public Overridable Function FindLinkPair(ByVal strOriginType As String, ByVal strDestinationType As String) As DataObjects.Behavior.LinkPair
            Dim doNewLinkPair As New DataObjects.Behavior.LinkPair(strOriginType, strDestinationType, "")
            For Each doPair As DataObjects.Behavior.LinkPair In m_aryLinkPairs
                If DataObjects.Behavior.LinkPair.CompareNodes(doPair, doNewLinkPair) Then
                    Return doPair
                End If
            Next

            Return Nothing
        End Function

        Public Overridable Function FindLinkPair(ByVal strOriginType As String, ByVal strDestinationType As String, ByVal strLinkType As String) As DataObjects.Behavior.LinkPair
            Dim doNewLinkPair As New DataObjects.Behavior.LinkPair(strOriginType, strDestinationType, strLinkType)
            For Each doPair As DataObjects.Behavior.LinkPair In m_aryLinkPairs
                If doPair = doNewLinkPair Then
                    Return doPair
                End If
            Next

            Return Nothing
        End Function

        Public Overridable Sub AddAdapterPair(ByVal strOriginType As String, ByVal strDestinationType As String, ByVal strLinkType As String)
            Dim doNewLinkPair As New DataObjects.Behavior.LinkPair(strOriginType, strDestinationType, strLinkType)

            If FindAdapterPair(strOriginType, strDestinationType) Is Nothing Then
                m_aryAdapterPairs.Add(doNewLinkPair)
            End If

        End Sub

        Public Overridable Sub RemoveAdapterPair(ByVal strOriginType As String, ByVal strDestinationType As String)
            Dim doPair As DataObjects.Behavior.LinkPair = FindAdapterPair(strOriginType, strDestinationType)

            If Not doPair Is Nothing Then
                m_aryAdapterPairs.Remove(doPair)
            End If

        End Sub

        Public Overridable Function FindAdapterPair(ByVal strOriginType As String, ByVal strDestinationType As String) As DataObjects.Behavior.LinkPair
            Dim doNewLinkPair As New DataObjects.Behavior.LinkPair(strOriginType, strDestinationType, "")
            For Each doPair As DataObjects.Behavior.LinkPair In m_aryAdapterPairs
                If DataObjects.Behavior.LinkPair.CompareNodes(doPair, doNewLinkPair) Then
                    Return doPair
                End If
            Next

            Return Nothing
        End Function

        Public Overridable Function FindAdapterPair(ByVal strOriginType As String, ByVal strDestinationType As String, ByVal strLinkType As String) As DataObjects.Behavior.LinkPair
            Dim doNewLinkPair As New DataObjects.Behavior.LinkPair(strOriginType, strDestinationType, strLinkType)
            For Each doPair As DataObjects.Behavior.LinkPair In m_aryAdapterPairs
                If doPair = doNewLinkPair Then
                    Return doPair
                End If
            Next

            Return Nothing
        End Function

#End Region

#Region " Child Form Management "

        Public Overridable Sub AddChildForm(ByVal frmChild As Forms.AnimatForm, Optional ByVal tabPage As Crownwood.DotNetMagic.Controls.TabPage = Nothing)

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

        Public Overridable Sub RemoveChildForm(ByVal frmChild As Forms.AnimatForm)
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
                If Util.IsTypeOf(frmAnimat.GetType, GetType(Forms.SimulationWindow), False) Then
                    Dim frmEditor As Forms.SimulationWindow = DirectCast(frmAnimat, Forms.SimulationWindow)
                    If frmEditor.PhysicalStructure Is doStruct Then
                        RemoveChildForm(frmEditor)
                    End If
                End If
            Next

        End Sub

        Public Overridable Sub ClearChildForms()

            Me.AppStatusText = "Clearing child window contents"

            For Each frmChild As Form In Me.ChildForms
                'I am unsure why, but sometimes some of the forms do not get their formclosing event fired corrctly.
                'This only seems to happen when I close the forms down manually like here. So I am going to call the
                'close method explicitly first. So the forms need to be able to handle the situation where this is 
                'possibly called multiple times.
                If Util.IsTypeOf(frmChild.GetType, GetType(Forms.AnimatForm), False) Then
                    Dim frmAnimat As AnimatForm = DirectCast(frmChild, AnimatForm)
                    frmAnimat.PrepareForClosing()
                End If

                frmChild.Close()
            Next

            Me.AnimatTabbedGroups.RootSequence.Clear()
            Me.ChildForms.Clear()
            Me.SortedChildForms.Clear()
        End Sub

        Public Overridable Sub ClearDockingContents()

            Me.AppStatusText = "Clearing docking window contents"

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

        Public Overridable Sub CloseForm(ByVal frmAnimat As Forms.AnimatForm, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing, Optional ByVal bThrowError As Boolean = True)
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

        Protected Class SplashInfo
            Public m_Image As Bitmap
            Public m_Font As System.Drawing.Font
            Public m_Position As PointF
            Public m_Color As System.Drawing.Color
        End Class

        Protected Sub ShowSplashScreen()
            Try

#If Not Debug Then

                Dim arySplashInfo As New ArrayList
                PopulateSplashArray(arySplashInfo)
                Dim infoSplash As SplashInfo = PickSplashInfo(arySplashInfo)

                If Not infoSplash Is Nothing AndAlso Not infoSplash.m_Image Is Nothing AndAlso TypeOf infoSplash.m_Image Is Bitmap Then
                    Dim strProductName As String = "AnimatLab"
                    Dim strProductExtra As String = ""
                    Dim strProductVersion As String = ""
                    Dim bFullVersion As Boolean = False

                    GetProductInfo(strProductName, strProductVersion, strProductExtra, bFullVersion)

                    Dim strText As String = strProductName & vbCrLf & strProductVersion
                    If strProductExtra.Length > 0 Then
                        strText = strText & vbCrLf & strProductExtra
                    End If
                    If Not bFullVersion Then
                        strText = strText & vbCrLf & "Upgrade to AnimatLab Pro"
                    End If

                    AnimatGuiCtrls.Forms.SplashForm.StartSplash(infoSplash.m_Image, System.Drawing.Color.White, strText, infoSplash.m_Font, infoSplash.m_Position, infoSplash.m_Color, 5, Me)
                End If
#End If

            Catch ex As System.Exception

            End Try
        End Sub

        Protected Overridable Sub PopulateSplashArray(ByVal arySplashInfo As ArrayList)

            Dim infoSplash As New SplashInfo
            infoSplash.m_Image = DirectCast(ImageManager.LoadImage("AnimatGUI", "AnimatGUI.Splash_Crayfish.jpg"), Bitmap)
            infoSplash.m_Font = New System.Drawing.Font("Arial", 12, FontStyle.Bold)
            infoSplash.m_Color = System.Drawing.Color.White
            infoSplash.m_Position = New PointF(350, 45)
            arySplashInfo.Add(infoSplash)

            infoSplash = New SplashInfo
            infoSplash.m_Image = DirectCast(ImageManager.LoadImage("AnimatGUI", "AnimatGUI.Splash_Frog.jpg"), Bitmap)
            infoSplash.m_Font = New System.Drawing.Font("Arial", 12, FontStyle.Bold)
            infoSplash.m_Color = System.Drawing.Color.White
            infoSplash.m_Position = New PointF(280, 50)
            arySplashInfo.Add(infoSplash)

            infoSplash = New SplashInfo
            infoSplash.m_Image = DirectCast(ImageManager.LoadImage("AnimatGUI", "AnimatGUI.Splash_Locust.jpg"), Bitmap)
            infoSplash.m_Font = New System.Drawing.Font("Arial", 12, FontStyle.Bold)
            infoSplash.m_Color = System.Drawing.Color.White
            infoSplash.m_Position = New PointF(130, 50)
            arySplashInfo.Add(infoSplash)

            infoSplash = New SplashInfo
            infoSplash.m_Image = DirectCast(ImageManager.LoadImage("AnimatGUI", "AnimatGUI.Splash_Robot.jpg"), Bitmap)
            infoSplash.m_Font = New System.Drawing.Font("Arial", 12, FontStyle.Bold)
            infoSplash.m_Color = System.Drawing.Color.White
            infoSplash.m_Position = New PointF(10, 100)
            arySplashInfo.Add(infoSplash)

        End Sub

        Protected Overridable Function PickSplashInfo(ByVal arySplashInfo As ArrayList) As SplashInfo

            Dim infoSplash As SplashInfo = Nothing

            If arySplashInfo.Count > 0 Then
                Dim iVal As Integer = CInt(Util.Rand(0, (arySplashInfo.Count - 1)))

                If iVal > (arySplashInfo.Count - 1) Then iVal = arySplashInfo.Count - 1

                infoSplash = DirectCast(arySplashInfo(iVal), SplashInfo)
            End If

            Return infoSplash
        End Function

        Public Sub GetProductInfo(ByRef strProductName As String, ByRef strProductVersion As String, ByRef strProductExtraInfo As String, ByRef bFullVersion As Boolean)

            strProductVersion = "Version " & Util.VersionNumber

            If Not m_SecurityMgr Is Nothing Then
                Dim strTitleExtra As String = ""
                If m_SecurityMgr.IsValidSerialNumber Then
                    If m_SecurityMgr.IsEvaluationLicense Then
                        bFullVersion = False
                        strProductName = "AnimatLab Pro Evaluation"
                        strProductExtraInfo = m_SecurityMgr.EvaluationDaysLeft & " days left in evaluation"
                    Else
                        bFullVersion = True
                        strProductName = "AnimatLab Pro"
                    End If
                Else
                    strProductName = "AnimatLab Standard"
                    bFullVersion = False
                End If
            End If
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

                'Me.AppIsBusy = True

                'frmMdi.Organism = doOrganism
                'frmMdi.Initialize(Me, frmBase)
                'frmMdi.Title = "Edit " & doOrganism.Name

                'If System.IO.File.Exists(Util.GetFilePath(Util.Application.ProjectPath, doOrganism.BehavioralEditorFile)) Then
                '    frmMdi.LoadEditorFile(doOrganism.BehavioralEditorFile)
                'End If

                'frmMdi.ShowAnimatForm()

                'doOrganism.BehaviorEditor = frmMdi

                'Me.AppIsBusy = False

                'Return frmMdi
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.AppIsBusy = False
            End Try

        End Function

        Public Overridable Sub EditBodyPlan(ByVal doStructure As DataObjects.Physical.PhysicalStructure)

            Try
                'First check to see if the screen is alread open. If it is then just make sure it is visible.
                If Not doStructure.BodyEditor Is Nothing Then
                    doStructure.BodyEditor.MakeVisible()
                    Return
                End If

                Util.Application.AppIsBusy = True

                'If there is not already an open window then lets create it.
                Dim frmAnimat As Forms.SimulationWindow = DirectCast(CreateForm("AnimatGUI.dll", "AnimatGUI.Forms.SimulationWindow", doStructure.Name & " Body", False), Forms.SimulationWindow)
                doStructure.BodyEditor = frmAnimat
                frmAnimat.PhysicalStructure = doStructure
                If Not doStructure.RootBody Is Nothing Then
                    frmAnimat.BodyPart = doStructure.RootBody
                End If
                frmAnimat.Initialize(Me)

                AddChildForm(frmAnimat)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.AppIsBusy = False
            End Try

        End Sub


        Public Overridable Function EditEnvironment() As AnimatForm

            Try

                'Dim frmMdi As Form = DirectCast(Util.LoadClass("VortexOsgAnimatTools", "VortexOsgAnimatTools.Forms.SimTest"), Form)
                'frmMdi.MdiParent = Me
                'frmMdi.Show()

                'Me.AppIsBusy = False

                'Return Nothing
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.AppIsBusy = False
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
                Dim frmAnimat As Forms.Tools.ToolForm = DirectCast(CreateForm(doTool.BaseAssemblyFile, doTool.BaseClassName, doTool.Name, False), Forms.Tools.ToolForm)
                frmAnimat.Initialize(Me)
                frmAnimat.LoadExternalFile(frmAnimat.ExternalFilename)

                AddChildForm(frmAnimat)

                doTool.ToolForm = frmAnimat

                doTool.CreateWorkspaceTreeView(Me.Simulation, Me.Simulation.ToolViewersTreeNode)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.AppIsBusy = False
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
                Me.AppIsBusy = True

                Me.Simulation.SimulationAtEndTime = False

                'If the simulation is not started then go ahead and get it going.
                If Not Me.SimulationInterface.SimOpen Then
                    Me.CreateSimulation(False)
                End If

                If Me.SimulationInterface.Paused Then
                    If Me.SimulationInterface.CurrentMillisecond <= 0 Then
                        RaiseEvent SimulationStarting()
                        m_bSimStopped = False
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

                Util.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Simulation was started or resumed.")

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.AppIsBusy = False
            End Try

        End Sub

        Public Sub StopSimulation()

            Try
                Me.AppStatusText = "Stopping simulation"

                Me.AppIsBusy = True

                If Not m_bStoppingSimulation Then
                    m_bStoppingSimulation = True
                    Me.SimulationInterface.PauseSimulation()
                    RaiseEvent SimulationStopped()
                    Me.SimulationInterface.StopSimulation()
                    m_bStoppingSimulation = False
                End If

                Util.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Simulation was stopped.")

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                m_bSimStopped = True
                Me.AppIsBusy = False
            End Try

        End Sub

        Public Overridable Sub CreateSimulation(ByVal bPaused As Boolean)
            Try
                Me.AppStatusText = "Creating simulation"

                Me.SimulationInterface.CreateAndRunSimulation(bPaused)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

#Region " Test Automation Control "

        Private Delegate Sub ExecuteMethodDelegate(ByVal strMethodName As String, ByVal aryParams() As Object)

        Public Function ExecuteMethod(ByVal strMethodName As String, ByVal aryParams() As Object) As Object

            If Me.InvokeRequired Then
                Return Me.Invoke(New ExecuteMethodDelegate(AddressOf ExecuteMethod), New Object() {strMethodName, aryParams})
            End If

            If Util.ActiveDialogs.Count > 0 Then
                Throw New System.Exception("You attempted to execute an application method while there was an active dialog.")
            End If

            Dim oMethod As MethodInfo = Me.GetType().GetMethod(strMethodName)

            If oMethod Is Nothing Then
                Throw New System.Exception("Method name '" & strMethodName & "' not found.")
            End If
            Return oMethod.Invoke(Me, aryParams)

        End Function

        Private Delegate Sub ExecuteMethodOnObjectDelegate(ByVal strPath As String, ByVal strMethodName As String, ByVal aryParams() As Object)

        Public Function ExecuteMethodOnObject(ByVal strPath As String, ByVal strMethodName As String, ByVal aryParams() As Object) As Object Implements ManagedAnimatInterfaces.ISimApplication.ExecuteMethodOnObject

            If Me.InvokeRequired Then
                Return Me.Invoke(New ExecuteMethodOnObjectDelegate(AddressOf ExecuteMethodOnObject), New Object() {strPath, strMethodName, aryParams})
            End If

            If Util.ActiveDialogs.Count > 0 Then
                Throw New System.Exception("You attempted to execute an object method while there was an active dialog.")
            End If

            m_tnAutomationTreeNode = Util.FindTreeNodeByPath(strPath, Util.ProjectWorkspace.TreeView.Nodes)

            Dim oMethod As MethodInfo = m_tnAutomationTreeNode.Tag.GetType().GetMethod(strMethodName)

            If oMethod Is Nothing Then
                Throw New System.Exception("Method name '" & strMethodName & "' not found.")
            End If
            Return oMethod.Invoke(m_tnAutomationTreeNode.Tag, aryParams)

        End Function

        Public Sub ExecuteIndirectMethod(ByVal strMethodName As String, ByVal aryParams() As Object)
            Me.InternalAutomationMethodInProgress = True

            Try

                m_strAutomationMethodName = strMethodName
                m_aryAutomationParams = aryParams

                m_timerAutomation = New System.Timers.Timer(10)
                AddHandler m_timerAutomation.Elapsed, AddressOf Me.OnExecuteIndirectMethodTimer
                m_timerAutomation.Enabled = True

            Catch ex As Exception
                Me.InternalAutomationMethodInProgress = False
                Throw ex
            End Try
        End Sub

        Private Delegate Sub OnExecuteIndirectMethodTimerDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnExecuteIndirectMethodTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerAutomation.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnExecuteIndirectMethodTimerDelegate(AddressOf OnExecuteIndirectMethodTimer), New Object() {sender, eProps})
                Return
            End If

            Try
                RemoveHandler m_timerAutomation.Elapsed, AddressOf OnExecuteIndirectMethodTimer
                m_timerAutomation = Nothing

                ExecuteMethod(m_strAutomationMethodName, m_aryAutomationParams)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.InternalAutomationMethodInProgress = False
            End Try
        End Sub

        Public Sub ExecuteIndirectMethodOnObject(ByVal strPath As String, ByVal strMethodName As String, ByVal aryParams() As Object)
            Me.InternalAutomationMethodInProgress = True

            Try

                m_tnAutomationTreeNode = Util.FindTreeNodeByPath(strPath, Util.ProjectWorkspace.TreeView.Nodes)
                m_strAutomationPath = strPath
                m_strAutomationMethodName = strMethodName
                m_aryAutomationParams = aryParams

                m_timerAutomation = New System.Timers.Timer(10)
                AddHandler m_timerAutomation.Elapsed, AddressOf Me.OnExecuteIndirectMethodOnObjectTimer
                m_timerAutomation.Enabled = True

            Catch ex As Exception
                Me.InternalAutomationMethodInProgress = False
                Throw ex
            End Try
        End Sub

        Private Delegate Sub OnExecuteIndirectMethodOnObjectTimerDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnExecuteIndirectMethodOnObjectTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerAutomation.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnExecuteIndirectMethodTimerDelegate(AddressOf OnExecuteIndirectMethodOnObjectTimer), New Object() {sender, eProps})
                Return
            End If

            Try
                RemoveHandler m_timerAutomation.Elapsed, AddressOf OnExecuteIndirectMethodTimer
                m_timerAutomation = Nothing

                ExecuteMethodOnObject(m_strAutomationPath, m_strAutomationMethodName, m_aryAutomationParams)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.InternalAutomationMethodInProgress = False
            End Try
        End Sub

        Private Delegate Sub ExecuteAppPropertyMethodDelegate(ByVal strPropertyName As String, ByVal strMethodName As String, ByVal aryParams() As Object)

        Public Function ExecuteAppPropertyMethod(ByVal strPropertyName As String, ByVal strMethodName As String, ByVal aryParams() As Object) As Object
            If Me.InvokeRequired Then
                Return Me.Invoke(New ExecuteAppPropertyMethodDelegate(AddressOf ExecuteAppPropertyMethod), New Object() {strPropertyName, strMethodName, aryParams})
            End If

            If Util.ActiveDialogs.Count > 0 Then
                Throw New System.Exception("You attempted to execute an application method while there was an active dialog.")
            End If

            Dim oProperty As PropertyInfo = Me.GetType().GetProperty(strPropertyName)

            Dim oObj As Object = oProperty.GetValue(Me, Nothing)

            Dim oMethod As MethodInfo = oObj.GetType.GetMethod(strMethodName)

            If oMethod Is Nothing Then
                Throw New System.Exception("Method name '" & strMethodName & "' not found on property '" & strPropertyName & "'.")
            End If
            Return oMethod.Invoke(oObj, aryParams)

        End Function

        Public Sub ExecuteIndirectAppPropertyMethod(ByVal strPropertyName As String, ByVal strMethodName As String, ByVal aryParams() As Object)
            Me.InternalAutomationMethodInProgress = True

            Try
                m_strAutomationName = strPropertyName
                m_strAutomationMethodName = strMethodName
                m_aryAutomationParams = aryParams

                m_timerAutomation = New System.Timers.Timer(10)
                AddHandler m_timerAutomation.Elapsed, AddressOf Me.OnExecuteIndirectMethodTimer
                m_timerAutomation.Enabled = True

            Catch ex As Exception
                Me.InternalAutomationMethodInProgress = False
                Throw ex
            End Try
        End Sub

        Private Delegate Sub OnExecuteIndirectAppPropertyMethodTimerDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnExecuteIndirectAppPropertyMethodTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerAutomation.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnExecuteIndirectAppPropertyMethodTimerDelegate(AddressOf OnExecuteIndirectAppPropertyMethodTimer), New Object() {sender, eProps})
                Return
            End If

            Try
                RemoveHandler m_timerAutomation.Elapsed, AddressOf OnExecuteIndirectMethodTimer
                m_timerAutomation = Nothing

                ExecuteAppPropertyMethod(m_strAutomationName, m_strAutomationMethodName, m_aryAutomationParams)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.InternalAutomationMethodInProgress = False
            End Try
        End Sub

        Public Sub SelectWorkspaceItem(ByVal strPath As String, ByVal bSelectMultiple As Boolean)
            Me.InternalAutomationMethodInProgress = True

            Try
                If Util.ProjectWorkspace Is Nothing OrElse Util.ProjectWorkspace.TreeView Is Nothing Then
                    Throw New System.Exception("No project is currently loaded.")
                End If

                m_tnAutomationTreeNode = Util.FindTreeNodeByPath(strPath, Util.ProjectWorkspace.TreeView.Nodes)
                m_bAutomationPropBoolValue = bSelectMultiple

                m_timerAutomation = New System.Timers.Timer(10)
                AddHandler m_timerAutomation.Elapsed, AddressOf Me.OnSelectWorkspaceItemTimer
                m_timerAutomation.Enabled = True
            Catch ex As Exception
                Me.InternalAutomationMethodInProgress = False
                Throw ex
            End Try
        End Sub

        Private Delegate Sub OnSelectWorkspaceItemTimerDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnSelectWorkspaceItemTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerAutomation.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnSelectWorkspaceItemTimerDelegate(AddressOf OnSelectWorkspaceItemTimer), New Object() {sender, eProps})
                Return
            End If

            Try
                RemoveHandler m_timerAutomation.Elapsed, AddressOf OnSelectWorkspaceItemTimer
                m_timerAutomation = Nothing

                Util.ProjectWorkspace.TreeView.SelectNode(m_tnAutomationTreeNode, False, m_bAutomationPropBoolValue)
                Util.ProjectWorkspace.TreeView.EnsureDisplayed(m_tnAutomationTreeNode)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.InternalAutomationMethodInProgress = False
            End Try
        End Sub

        Public Sub DblClickWorkspaceItem(ByVal strPath As String)
            Me.InternalAutomationMethodInProgress = True

            Try
                If Util.ProjectWorkspace Is Nothing OrElse Util.ProjectWorkspace.TreeView Is Nothing Then
                    Throw New System.Exception("No project is currently loaded.")
                End If

                m_tnAutomationTreeNode = Util.FindTreeNodeByPath(strPath, Util.ProjectWorkspace.TreeView.Nodes)

                m_timerAutomation = New System.Timers.Timer(10)
                AddHandler m_timerAutomation.Elapsed, AddressOf Me.OnDblClickWorkspaceItemTimer
                m_timerAutomation.Enabled = True

            Catch ex As Exception
                Me.InternalAutomationMethodInProgress = False
                Throw ex
            End Try
        End Sub

        Private Delegate Sub OnDblClickWorkspaceItemTimerDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnDblClickWorkspaceItemTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerAutomation.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnDblClickWorkspaceItemTimerDelegate(AddressOf OnDblClickWorkspaceItemTimer), New Object() {sender, eProps})
                Return
            End If

            Try
                RemoveHandler m_timerAutomation.Elapsed, AddressOf OnDblClickWorkspaceItemTimer
                m_timerAutomation = Nothing

                If Not m_tnAutomationTreeNode Is Nothing AndAlso Not m_tnAutomationTreeNode.Tag Is Nothing Then
                    If Util.IsTypeOf(m_tnAutomationTreeNode.Tag.GetType, GetType(Framework.DataObjectTreeViewRef), False) Then
                        Dim doRef As DataObjectTreeViewRef = DirectCast(m_tnAutomationTreeNode.Tag, DataObjectTreeViewRef)
                        If Not doRef.m_doObject Is Nothing AndAlso Util.IsTypeOf(doRef.m_doObject.GetType, GetType(Framework.DataObject), False) Then
                            Dim doNode As Framework.DataObject = DirectCast(doRef.m_doObject, Framework.DataObject)
                            doNode.WorkspaceTreeviewDoubleClick(m_tnAutomationTreeNode)
                        End If
                    ElseIf Util.IsTypeOf(m_tnAutomationTreeNode.Tag.GetType, GetType(Framework.DataObject), True) Then
                        Dim doData As Framework.DataObject = DirectCast(m_tnAutomationTreeNode.Tag, Framework.DataObject)
                        doData.WorkspaceTreeviewDoubleClick(m_tnAutomationTreeNode)
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.InternalAutomationMethodInProgress = False
            End Try
        End Sub

        Public Sub ClickToolbarItem(ByVal strToolName As String, ByVal bReturnImmediate As Boolean)
            Me.InternalAutomationMethodInProgress = bReturnImmediate

            Try

                m_aryToolClicks = Me.AnimatToolStrip.Items.Find(strToolName, True)
                If m_aryToolClicks Is Nothing OrElse m_aryToolClicks.Length = 0 Then
                    Throw New System.Exception("No tool item was found with that name.")
                End If

                m_timerAutomation = New System.Timers.Timer(10)
                AddHandler m_timerAutomation.Elapsed, AddressOf Me.OnToolClickTimer
                m_timerAutomation.Enabled = True

            Catch ex As Exception
                Me.InternalAutomationMethodInProgress = False
                Throw ex
            End Try
        End Sub

        Public Sub ClickMenuItem(ByVal strToolName As String, ByVal bReturnImmediate As Boolean)
            Me.InternalAutomationMethodInProgress = bReturnImmediate

            Try

                m_aryToolClicks = Me.AnimatMenuStrip.Items.Find(strToolName, True)
                If m_aryToolClicks Is Nothing OrElse m_aryToolClicks.Length = 0 Then
                    Throw New System.Exception("No menu item was found with that name.")
                End If

                m_timerAutomation = New System.Timers.Timer(10)
                AddHandler m_timerAutomation.Elapsed, AddressOf Me.OnToolClickTimer
                m_timerAutomation.Enabled = True

            Catch ex As Exception
                Me.InternalAutomationMethodInProgress = False
                Throw ex
            End Try

        End Sub

        Private Delegate Sub OnToolClickTimerDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnToolClickTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerAutomation.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnToolClickTimerDelegate(AddressOf OnToolClickTimer), New Object() {sender, eProps})
                Return
            End If

            Try
                RemoveHandler m_timerAutomation.Elapsed, AddressOf OnToolClickTimer
                m_timerAutomation = Nothing

                For Each oTool As System.Windows.Forms.ToolStripItem In m_aryToolClicks
                    oTool.PerformClick()
                Next

                m_aryToolClicks = Nothing

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.InternalAutomationMethodInProgress = False
            End Try
        End Sub

        Public Sub AddBehavioralNode(ByVal strSubsystem As String, ByVal strClassName As String, ByVal ptPosition As Point, ByVal strName As String)
            Me.InternalAutomationMethodInProgress = True

            Try
                If Util.ProjectWorkspace Is Nothing OrElse Util.ProjectWorkspace.TreeView Is Nothing Then
                    Throw New System.Exception("No project is currently loaded.")
                End If

                m_tnAutomationTreeNode = Util.FindTreeNodeByPath(strSubsystem, Util.ProjectWorkspace.TreeView.Nodes)

                If m_tnAutomationTreeNode Is Nothing OrElse m_tnAutomationTreeNode.Tag Is Nothing OrElse Not Util.IsTypeOf(m_tnAutomationTreeNode.Tag.GetType, GetType(DataObjects.Behavior.Nodes.Subsystem), False) Then
                    Throw New System.Exception("The path to the specified subsystem was not the correct object type.")
                End If

                Dim doSubsystem As Framework.DataObject = DirectCast(m_tnAutomationTreeNode.Tag, Framework.DataObject)

                Dim bnSubsystem As DataObjects.Behavior.Nodes.Subsystem = DirectCast(doSubsystem, DataObjects.Behavior.Nodes.Subsystem)

                If bnSubsystem.SubsystemDiagram Is Nothing Then
                    Throw New System.Exception("The diagram for the specified subsystem is not open")
                End If

                m_bnAutomationNodeOrigin = DirectCast(Util.LoadClass(strClassName, doSubsystem), DataObjects.Behavior.Node)
                m_strAutomationName = strName
                m_ptAutomationPosition = ptPosition

                m_timerAutomation = New System.Timers.Timer(10)
                AddHandler m_timerAutomation.Elapsed, AddressOf Me.OnAddBehavioralNodeTimer
                m_timerAutomation.Enabled = True

            Catch ex As Exception
                Me.InternalAutomationMethodInProgress = False
                Throw ex
            End Try
        End Sub

        Private Delegate Sub OnAddBehavioralNodeTimerDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnAddBehavioralNodeTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerAutomation.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnAddBehavioralNodeTimerDelegate(AddressOf OnAddBehavioralNodeTimer), New Object() {sender, eProps})
                Return
            End If

            Try
                RemoveHandler m_timerAutomation.Elapsed, AddressOf OnAddBehavioralNodeTimer
                m_timerAutomation = Nothing

                Dim bnSubsystem As DataObjects.Behavior.Nodes.Subsystem = DirectCast(m_tnAutomationTreeNode.Tag, DataObjects.Behavior.Nodes.Subsystem)

                If bnSubsystem.SubsystemDiagram Is Nothing Then
                    Throw New System.Exception("The diagram for the specified subsystem is not open")
                End If

                Dim bnNode As DataObjects.Behavior.Node = bnSubsystem.SubsystemDiagram.Automation_DropNode(m_bnAutomationNodeOrigin, m_ptAutomationPosition)

                If Not bnNode Is Nothing Then
                    bnNode.Name = m_strAutomationName
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.InternalAutomationMethodInProgress = False
            End Try
        End Sub


        Public Sub AddBehavioralLink(ByVal strNodeOrigin As String, ByVal strNodeDestination As String, ByVal strName As String)
            Me.InternalAutomationMethodInProgress = True

            Try
                If Util.ProjectWorkspace Is Nothing OrElse Util.ProjectWorkspace.TreeView Is Nothing Then
                    Throw New System.Exception("No project is currently loaded.")
                End If

                m_tnAutomationTreeNode = Util.FindTreeNodeByPath(strNodeOrigin, Util.ProjectWorkspace.TreeView.Nodes)

                If m_tnAutomationTreeNode Is Nothing OrElse m_tnAutomationTreeNode.Tag Is Nothing OrElse Not Util.IsTypeOf(m_tnAutomationTreeNode.Tag.GetType, GetType(DataObjects.Behavior.Node), False) Then
                    Throw New System.Exception("The path to the origin node was not the correct object type.")
                End If

                m_bnAutomationNodeOrigin = DirectCast(m_tnAutomationTreeNode.Tag, DataObjects.Behavior.Node)

                m_tnAutomationTreeNode = Util.FindTreeNodeByPath(strNodeDestination, Util.ProjectWorkspace.TreeView.Nodes)

                If m_tnAutomationTreeNode Is Nothing OrElse m_tnAutomationTreeNode.Tag Is Nothing OrElse Not Util.IsTypeOf(m_tnAutomationTreeNode.Tag.GetType, GetType(DataObjects.Behavior.Node), False) Then
                    Throw New System.Exception("The path to the destination node was not the correct object type.")
                End If

                m_bnAutomationNodeDestination = DirectCast(m_tnAutomationTreeNode.Tag, DataObjects.Behavior.Node)

                If m_bnAutomationNodeDestination.ParentDiagram Is Nothing OrElse m_bnAutomationNodeOrigin.ParentDiagram Is Nothing Then
                    Throw New System.Exception("The parent diagram for either the origin or destination is not shown.")
                End If

                If Not m_bnAutomationNodeDestination.ParentDiagram Is m_bnAutomationNodeOrigin.ParentDiagram Then
                    Throw New System.Exception("The parent diagram for the origin and destination are different. They must be the same to add a link.")
                End If

                m_tnAutomationTreeNode = Nothing
                m_strAutomationName = strName

                m_timerAutomation = New System.Timers.Timer(10)
                AddHandler m_timerAutomation.Elapsed, AddressOf Me.OnAddBehavioralLinkTimer
                m_timerAutomation.Enabled = True

            Catch ex As Exception
                Me.InternalAutomationMethodInProgress = False
                Throw ex
            End Try
        End Sub

        Private Delegate Sub OnAddBehavioralLinkTimerDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnAddBehavioralLinkTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerAutomation.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnAddBehavioralLinkTimerDelegate(AddressOf OnAddBehavioralLinkTimer), New Object() {sender, eProps})
                Return
            End If

            Try
                RemoveHandler m_timerAutomation.Elapsed, AddressOf OnAddBehavioralLinkTimer
                m_timerAutomation = Nothing

                If m_bnAutomationNodeOrigin Is Nothing OrElse m_bnAutomationNodeDestination Is Nothing Then
                    Throw New System.Exception("Origin or destination node is not defined.")
                End If

                If m_bnAutomationNodeDestination.ParentDiagram Is Nothing OrElse m_bnAutomationNodeOrigin.ParentDiagram Is Nothing Then
                    Throw New System.Exception("The parent diagram for either the origin or destination is not shown.")
                End If

                If Not m_bnAutomationNodeDestination.ParentDiagram Is m_bnAutomationNodeOrigin.ParentDiagram Then
                    Throw New System.Exception("The parent diagram for the origin and destination are different. They must be the same to add a link.")
                End If

                Dim blLink As DataObjects.Behavior.Link = m_bnAutomationNodeDestination.ParentDiagram.Automation_DrawLink(m_bnAutomationNodeOrigin, m_bnAutomationNodeDestination)

                If Not blLink Is Nothing Then
                    blLink.Text = m_strAutomationName
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.InternalAutomationMethodInProgress = False
            End Try
        End Sub

        Public Sub SetLinkedItem(ByVal strItemPath As String, ByVal strLinkedItemPath As String)
            If Util.ProjectWorkspace Is Nothing OrElse Util.ProjectWorkspace.TreeView Is Nothing Then
                Throw New System.Exception("No project is currently loaded.")
            End If

            m_tnAutomationTreeNode = Util.FindTreeNodeByPath(strItemPath, Util.ProjectWorkspace.TreeView.Nodes)

            If m_tnAutomationTreeNode Is Nothing OrElse m_tnAutomationTreeNode.Tag Is Nothing OrElse Not Util.IsTypeOf(m_tnAutomationTreeNode.Tag.GetType, GetType(Framework.DataObject), False) Then
                Throw New System.Exception("The path to the specified subsystem was not the correct object type.")
            End If

            Dim doSel As Framework.DataObject = DirectCast(m_tnAutomationTreeNode.Tag, Framework.DataObject)
            doSel.Automation_SetLinkedItem(strItemPath, strLinkedItemPath)

        End Sub

        Public Sub SetObjectProperty(ByVal strPath As String, ByVal strPropertyName As String, ByVal strValue As String)
            If Util.ProjectWorkspace Is Nothing OrElse Util.ProjectWorkspace.TreeView Is Nothing Then
                Throw New System.Exception("No project is currently loaded.")
            End If

            m_tnAutomationTreeNode = Util.FindTreeNodeByPath(strPath, Util.ProjectWorkspace.TreeView.Nodes)

            If m_tnAutomationTreeNode.Tag Is Nothing Then
                Throw New System.Exception("No object was found in the tree node path '" & strPath & "'.")
            End If

            Util.SetObjectProperty(m_tnAutomationTreeNode.Tag, strPropertyName, strValue)

        End Sub

        Public Function GetObjectProperty(ByVal strPath As String, ByVal strPropertyName As String) As Object
            If Util.ProjectWorkspace Is Nothing OrElse Util.ProjectWorkspace.TreeView Is Nothing Then
                Throw New System.Exception("No project is currently loaded.")
            End If

            m_tnAutomationTreeNode = Util.FindTreeNodeByPath(strPath, Util.ProjectWorkspace.TreeView.Nodes)

            If m_tnAutomationTreeNode.Tag Is Nothing Then
                Throw New System.Exception("No object was found in the tree node path '" & strPath & "'.")
            End If

            Return Util.GetObjectProperty(m_tnAutomationTreeNode.Tag, strPropertyName)
        End Function

        Public Function DoesObjectExist(ByVal strPath As String) As Object
            If Util.ProjectWorkspace Is Nothing OrElse Util.ProjectWorkspace.TreeView Is Nothing Then
                Throw New System.Exception("No project is currently loaded.")
            End If

            Dim oVal As Object
            Try
                oVal = Util.FindTreeNodeByPath(strPath, Util.ProjectWorkspace.TreeView.Nodes)
            Catch ex As Exception
            End Try

            If oVal Is Nothing Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Sub OpenUITypeEditor(ByVal strPath As String, ByVal strPropertyName As String)
            Me.InternalAutomationMethodInProgress = False 'This always opens a dialog window. So we should not mark it as busy.

            Try
                If Util.ProjectWorkspace Is Nothing OrElse Util.ProjectWorkspace.TreeView Is Nothing Then
                    Throw New System.Exception("No project is currently loaded.")
                End If

                m_tnAutomationTreeNode = Util.FindTreeNodeByPath(strPath, Util.ProjectWorkspace.TreeView.Nodes)

                If m_tnAutomationTreeNode.Tag Is Nothing Then
                    Throw New System.Exception("No object was found in the tree node path '" & strPath & "'.")
                End If

                Dim oObj As Object = m_tnAutomationTreeNode.Tag
                Util.GetObjectProperty(strPropertyName, m_piAutomationPropInfo, oObj)

                'Get the property object.
                Dim oObjProp As Object = m_piAutomationPropInfo.GetValue(oObj, Nothing)

                Dim tpProps As PropertyDescriptorCollection = TypeDescriptor.GetProperties(oObj)
                Dim tpProp As PropertyDescriptor = tpProps(strPropertyName)
                Dim oPropEdit As Object = tpProp.GetEditor(GetType(System.Drawing.Design.UITypeEditor))

                m_strAutomationName = strPropertyName

                m_timerAutomation = New System.Timers.Timer(10)
                AddHandler m_timerAutomation.Elapsed, AddressOf Me.OnOpenUITypeEditorTimer
                m_timerAutomation.Enabled = True

            Catch ex As Exception
                Me.InternalAutomationMethodInProgress = False
                Throw ex
            End Try
        End Sub

        Private Delegate Sub OnOpenUITypeEditorDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnOpenUITypeEditorTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerAutomation.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnOpenUITypeEditorDelegate(AddressOf OnOpenUITypeEditorTimer), New Object() {sender, eProps})
                Return
            End If

            Try
                RemoveHandler m_timerAutomation.Elapsed, AddressOf OnAddBehavioralLinkTimer
                m_timerAutomation = Nothing

                Dim oObj As Object = m_tnAutomationTreeNode.Tag
                Util.GetObjectProperty(m_strAutomationName, m_piAutomationPropInfo, oObj)

                Dim oPropContext As ObjectPropContext
                If Util.IsTypeOf(oObj.GetType(), GetType(Framework.DataObject)) Then
                    Dim doObj As Framework.DataObject = DirectCast(oObj, Framework.DataObject)
                    oPropContext = New ObjectPropContext(doObj)
                End If

                'Get the property object.
                Dim oObjProp As Object = m_piAutomationPropInfo.GetValue(oObj, Nothing)

                Dim tpProps As PropertyDescriptorCollection = TypeDescriptor.GetProperties(oObj)
                Dim tpProp As PropertyDescriptor = tpProps(m_strAutomationName)
                Dim oPropEdit As Object = tpProp.GetEditor(GetType(System.Drawing.Design.UITypeEditor))

                If Not oPropEdit Is Nothing Then
                    Dim edEdit As System.Drawing.Design.UITypeEditor = DirectCast(oPropEdit, System.Drawing.Design.UITypeEditor)
                    Dim doRetVal As Object = edEdit.EditValue(oPropContext, Nothing, oObjProp)
                    m_piAutomationPropInfo.SetValue(oObj, doRetVal, Nothing)
                Else
                    Throw New System.Exception("Attempted to call OnOpenUITypeEditor for " & m_tnAutomationTreeNode.Tag.ToString & _
                                               " with name " & m_strAutomationName & " failed to find  UITypeEditor to work with.")
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.InternalAutomationMethodInProgress = False
            End Try
        End Sub

        Public Sub SelectWorkspaceTabPage(ByVal strPath As String)
            Me.InternalAutomationMethodInProgress = True

            Try
                If Util.ProjectWorkspace Is Nothing OrElse Util.ProjectWorkspace.TreeView Is Nothing Then
                    Throw New System.Exception("No project is currently loaded.")
                End If

                m_tnAutomationTreeNode = Util.FindTreeNodeByPath(strPath, Util.ProjectWorkspace.TreeView.Nodes)

                m_timerAutomation = New System.Timers.Timer(10)
                AddHandler m_timerAutomation.Elapsed, AddressOf Me.OnSelectTabPageTimer
                m_timerAutomation.Enabled = True

            Catch ex As Exception
                Me.InternalAutomationMethodInProgress = False
                Throw ex
            End Try

        End Sub

        Private Delegate Sub OnSelectTabPageTimerDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnSelectTabPageTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerAutomation.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnSelectTabPageTimerDelegate(AddressOf OnSelectTabPageTimer), New Object() {sender, eProps})
                Return
            End If

            Try
                RemoveHandler m_timerAutomation.Elapsed, AddressOf OnSelectTabPageTimer
                m_timerAutomation = Nothing

                Dim oTab As Crownwood.DotNetMagic.Controls.TabPage
                If Not m_tnAutomationTreeNode.Tag Is Nothing AndAlso Util.IsTypeOf(m_tnAutomationTreeNode.Tag.GetType, GetType(AnimatForm)) Then
                    Dim frmPage As AnimatForm = DirectCast(m_tnAutomationTreeNode.Tag, AnimatForm)
                    oTab = frmPage.TabPage
                ElseIf Not m_tnAutomationTreeNode.Tag Is Nothing AndAlso Util.IsTypeOf(m_tnAutomationTreeNode.Tag.GetType, GetType(DataObjects.Physical.PhysicalStructure), False) Then
                    Dim doStruct As DataObjects.Physical.PhysicalStructure = DirectCast(m_tnAutomationTreeNode.Tag, DataObjects.Physical.PhysicalStructure)
                    If Not doStruct.BodyEditor Is Nothing Then
                        oTab = doStruct.BodyEditor.TabPage
                    End If
                ElseIf Not m_tnAutomationTreeNode.Tag Is Nothing AndAlso Util.IsTypeOf(m_tnAutomationTreeNode.Tag.GetType, GetType(DataObjects.ToolHolder), False) Then
                    Dim doToolHolder As DataObjects.ToolHolder = DirectCast(m_tnAutomationTreeNode.Tag, DataObjects.ToolHolder)
                    If Not doToolHolder.ToolForm Is Nothing AndAlso Not doToolHolder.ToolForm.TabPage Is Nothing Then
                        oTab = doToolHolder.ToolForm.TabPage
                    End If
                ElseIf Not m_tnAutomationTreeNode.Tag Is Nothing AndAlso Util.IsTypeOf(m_tnAutomationTreeNode.Tag.GetType, GetType(DataObjects.Behavior.Nodes.Subsystem), False) Then
                    Dim doSub As DataObjects.Behavior.Nodes.Subsystem = DirectCast(m_tnAutomationTreeNode.Tag, DataObjects.Behavior.Nodes.Subsystem)
                    If Not doSub.SubsystemDiagram Is Nothing Then
                        oTab = doSub.SubsystemDiagram.TabPage
                    End If
                End If

                If Not oTab Is Nothing Then
                    oTab.Selected = True
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.InternalAutomationMethodInProgress = False
            End Try
        End Sub

        Public Sub SelectTrackItems(ByVal strPath As String, ByVal strStructure As String, ByVal strPart As String)
            Me.InternalAutomationMethodInProgress = True

            Try
                If Util.ProjectWorkspace Is Nothing OrElse Util.ProjectWorkspace.TreeView Is Nothing Then
                    Throw New System.Exception("No project is currently loaded.")
                End If

                m_tnAutomationTreeNode = Util.FindTreeNodeByPath(strPath, Util.ProjectWorkspace.TreeView.Nodes)

                If strStructure = "No Tracking" Then
                    m_doAutomationStructure = Nothing
                    m_doAutomationBodyPart = Nothing
                Else
                    m_doAutomationStructure = DirectCast(Util.Environment.Structures.FindDataObjectByName(strStructure), DataObjects.Physical.PhysicalStructure)
                    m_doAutomationBodyPart = m_doAutomationStructure.FindBodyPartByName(strPart, False)
                End If

                m_timerAutomation = New System.Timers.Timer(10)
                AddHandler m_timerAutomation.Elapsed, AddressOf Me.OnSelectTrackItemsTimer
                m_timerAutomation.Enabled = True

            Catch ex As Exception
                Me.InternalAutomationMethodInProgress = False
                Throw ex
            End Try

        End Sub

        Private Delegate Sub OnSelectTrackItemsTimerDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnSelectTrackItemsTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerAutomation.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnSelectTrackItemsTimerDelegate(AddressOf OnSelectTrackItemsTimer), New Object() {sender, eProps})
                Return
            End If

            Try
                RemoveHandler m_timerAutomation.Elapsed, AddressOf OnSelectTrackItemsTimer
                m_timerAutomation = Nothing

                Dim frmPage As SimulationWindow
                If Not m_tnAutomationTreeNode.Tag Is Nothing AndAlso Util.IsTypeOf(m_tnAutomationTreeNode.Tag.GetType, GetType(SimulationWindow)) Then
                    frmPage = DirectCast(m_tnAutomationTreeNode.Tag, SimulationWindow)
                ElseIf Not m_tnAutomationTreeNode.Tag Is Nothing AndAlso Util.IsTypeOf(m_tnAutomationTreeNode.Tag.GetType, GetType(DataObjects.Physical.PhysicalStructure), False) Then
                    Dim doStruct As DataObjects.Physical.PhysicalStructure = DirectCast(m_tnAutomationTreeNode.Tag, DataObjects.Physical.PhysicalStructure)
                    frmPage = doStruct.BodyEditor
                End If

                If Not frmPage Is Nothing Then
                    If m_doAutomationStructure Is Nothing Then
                        frmPage.cboStructure.SelectedIndex = 0
                        frmPage.cboBodyPart.SelectedItem = Nothing
                    Else
                        frmPage.cboStructure.SelectedItem = m_doAutomationStructure
                        frmPage.GenerateBodyPartDropDown()
                        frmPage.cboBodyPart.SelectedItem = m_doAutomationBodyPart
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.InternalAutomationMethodInProgress = False
            End Try
        End Sub

        Private Delegate Sub ExecuteActiveDialogMethodDelegate(ByVal strMethodName As String, ByVal aryParams() As Object)

        Public Overridable Function ExecuteActiveDialogMethod(ByVal strMethodName As String, ByVal aryParams() As Object) As Object
            If Me.InvokeRequired Then
                Return Me.Invoke(New ExecuteActiveDialogMethodDelegate(AddressOf ExecuteActiveDialogMethod), New Object() {strMethodName, aryParams})
            End If

            If Util.ActiveDialogs.Count = 0 Then
                Throw New System.Exception("No dialog is currently active.")
            End If

            Dim oDlg As Object = Util.ActiveDialogs(Util.ActiveDialogs.Count - 1)
            Dim oMethod As MethodInfo = oDlg.GetType().GetMethod(strMethodName)

            If oMethod Is Nothing Then
                Throw New System.Exception("Method name '" & strMethodName & "' not found.")
            End If
            Dim oRet As Object = oMethod.Invoke(oDlg, aryParams)
            Return oRet
        End Function

        Public Overridable Function ExecuteDirectActiveDialogMethod(ByVal strMethodName As String, ByVal aryParams() As Object) As Object

            If Util.ActiveDialogs.Count = 0 Then
                Throw New System.Exception("No dialog is currently active.")
            End If

            Dim oDlg As Object = Util.ActiveDialogs(Util.ActiveDialogs.Count - 1)
            Dim oMethod As MethodInfo = oDlg.GetType().GetMethod(strMethodName)

            If oMethod Is Nothing Then
                Throw New System.Exception("Method name '" & strMethodName & "' not found.")
            End If
            Dim oRet As Object = oMethod.Invoke(oDlg, aryParams)
            Return oRet
        End Function

        Public Sub ExecuteIndirecActiveDialogtMethod(ByVal strMethodName As String, ByVal aryParams() As Object)
            Me.InternalAutomationMethodInProgress = True

            Try

                m_strAutomationMethodName = strMethodName
                m_aryAutomationParams = aryParams

                m_timerAutomation = New System.Timers.Timer(10)
                AddHandler m_timerAutomation.Elapsed, AddressOf Me.OnExecuteIndirectActiveDialogMethodTimer
                m_timerAutomation.Enabled = True

            Catch ex As Exception
                Me.InternalAutomationMethodInProgress = False
                Throw ex
            End Try
        End Sub

        Private Delegate Sub OnExecuteIndirecActiveDialogtMethodTimerDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnExecuteIndirectActiveDialogMethodTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerAutomation.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnExecuteIndirectMethodTimerDelegate(AddressOf OnExecuteIndirectActiveDialogMethodTimer), New Object() {sender, eProps})
                Return
            End If

            Try
                RemoveHandler m_timerAutomation.Elapsed, AddressOf OnExecuteIndirectActiveDialogMethodTimer
                m_timerAutomation = Nothing

                ExecuteActiveDialogMethod(m_strAutomationMethodName, m_aryAutomationParams)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.InternalAutomationMethodInProgress = False
            End Try
        End Sub


        Public Overridable Function ActiveDialogName() As String
            If Util.ActiveDialogs.Count > 0 Then
                Dim frmDlg As System.Windows.Forms.Form = DirectCast(Util.ActiveDialogs(Util.ActiveDialogs.Count - 1), System.Windows.Forms.Form)
                Return frmDlg.Text
            End If

            Return "<No Dialog>"
        End Function

        Private Delegate Sub CloseActiveDialogsDelegate()

        Public Overridable Sub CloseActiveDialogs()
            If Me.InvokeRequired Then
                Me.Invoke(New CloseActiveDialogsDelegate(AddressOf CloseActiveDialogs), Nothing)
                Return
            End If

            Util.ClearActiveDialogs(True)
        End Sub

        Private Delegate Sub ExecuteObjectMethodDelegate(ByVal strAssembly As String, ByVal strClassName As String, ByVal strMethodName As String, ByVal aryParams() As Object)

        Public Function ExecuteObjectMethod(ByVal strAssembly As String, ByVal strClassName As String, ByVal strMethodName As String, ByVal aryParams() As Object) As Object
            If Me.InvokeRequired Then
                Return Me.Invoke(New ExecuteObjectMethodDelegate(AddressOf ExecuteObjectMethod), New Object() {strAssembly, strClassName, strMethodName, aryParams})
            End If

            Dim doParent As Framework.DataObject = Nothing
            Dim oObj As Object = Util.LoadClass(strAssembly, strClassName, doParent)

            Dim oMethod As MethodInfo = oObj.GetType().GetMethod(strMethodName)

            If oMethod Is Nothing Then
                Throw New System.Exception("Method name '" & strMethodName & "' not found.")
            End If
            Return oMethod.Invoke(oObj, aryParams)

        End Function

        Public Sub ReloadSimulation(ByVal bIndirect As Boolean)

            Try
                If bIndirect Then
                    m_timerAutomation = New System.Timers.Timer(10)
                    AddHandler m_timerAutomation.Elapsed, AddressOf Me.ReloadSimulationTimer
                    m_timerAutomation.Enabled = True
                Else
                    SaveProject(Me.ProjectPath & Me.ProjectFile)
                    LoadProject(Me.ProjectPath & Me.ProjectFile)
                End If

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Delegate Sub ReloadSimulationDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub ReloadSimulationTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerAutomation.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New ReloadSimulationDelegate(AddressOf ReloadSimulationTimer), New Object() {sender, eProps})
                Return
            End If

            Try
                RemoveHandler m_timerAutomation.Elapsed, AddressOf ReloadSimulationTimer
                m_timerAutomation = Nothing

                SaveProject(Me.ProjectPath & Me.ProjectFile)
                LoadProject(Me.ProjectPath & Me.ProjectFile)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


#End Region

#Region "Toolstrip Handlers "

        Public Overridable Sub AddStimulus()
            Try
                If Util.ProjectWorkspace.TreeView.SelectedNodes.Count > 1 OrElse Util.ProjectWorkspace.TreeView.SelectedNodes.Count <= 0 Then
                    Throw New System.Exception("Please select 1 body part to which you want to add a stimulus.")
                Else
                    If Util.ProjectWorkspace.TreeView.SelectedNode.Tag Is Nothing Then
                        Throw New System.Exception("Please select a body part to which you want to add a stimulus.")
                    End If

                    If Util.IsTypeOf(Util.ProjectWorkspace.TreeView.SelectedNode.Tag.GetType, GetType(AnimatGUI.DataObjects.DragObject), False) Then
                        Dim doPart As DataObjects.DragObject = DirectCast(Util.ProjectWorkspace.TreeView.SelectedNode.Tag, AnimatGUI.DataObjects.DragObject)
                        doPart.SelectStimulusType()
                    Else
                        Throw New System.Exception("Please select a body part to which you want to add a stimulus.")
                    End If

                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Sub SelectByType()
            Try
                Dim frmType As New AnimatGUI.Forms.BodyPlan.SelectByType

                frmType.SelectedItem = Util.ProjectWorkspace.SelectedDataObject

                If frmType.SelectedItem Is Nothing Then
                    If Util.ProjectWorkspace.SelectedItem Is Nothing Then
                        Throw New System.Exception("You must select an item in the workspace before you can select by type.")
                    Else
                        Throw New System.Exception("Please select a part a rigid body part or behavior node so we know what type of part to look for.")
                    End If
                End If

                If frmType.ShowDialog = DialogResult.OK Then

                    Dim colObjects As New AnimatGUI.Collections.DataObjects(Nothing)
                    frmType.SelectedItem.FindChildrenOfType(frmType.SelectedType, colObjects)

                    Util.ProjectWorkspace.SelectMultipleItems(colObjects)

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Sub Relabel()
            Try
                Dim frmRelabel As New AnimatGUI.Forms.BodyPlan.Relabel

                frmRelabel.SelectedItem = Util.ProjectWorkspace.SelectedDataObject
                frmRelabel.ShowDialog()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Sub RelabelSelected()
            Try
                Dim frmRelabel As New Forms.RelabelSelected

                If Util.ProjectWorkspace.TreeView.SelectedNodes.Count > 0 Then
                    If frmRelabel.ShowDialog() = DialogResult.OK Then

                        Dim aryList As New ArrayList
                        Dim doPart As Framework.DataObject
                        For Each tvItem As Crownwood.DotNetMagic.Controls.Node In Util.ProjectWorkspace.TreeView.SelectedNodes
                            If Not tvItem.Tag Is Nothing Then
                                If Util.IsTypeOf(tvItem.Tag.GetType, GetType(Framework.DataObject), False) Then
                                    doPart = DirectCast(tvItem.Tag, Framework.DataObject)
                                    aryList.Add(doPart)
                                End If
                            End If
                        Next

                        Util.RelableSelected(aryList, frmRelabel.txtNewLabel.Text, frmRelabel.StartWith, frmRelabel.IncrementBy)
                    End If

                    Me.IsDirty = True
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Sub CompareItems()

            Try
                Dim frmCompare As New AnimatGUI.Forms.Tools.CompareItems
                'frmCompare.PhysicalStructure = Me.PhysicalStructure

                If Util.ProjectWorkspace.TreeView.SelectedNodes.Count < 2 Then
                    Throw New System.Exception("You must select at least two objects in order to do a comparison.")
                End If

                frmCompare.SelectedItems().Clear()
                Dim doPart As Framework.DataObject
                For Each tvItem As Crownwood.DotNetMagic.Controls.Node In Util.ProjectWorkspace.TreeView.SelectedNodes
                    If Not tvItem.Tag Is Nothing Then
                        If Util.IsTypeOf(tvItem.Tag.GetType, GetType(Framework.DataObject), False) Then
                            doPart = DirectCast(tvItem.Tag, Framework.DataObject)
                            frmCompare.SelectedItems.Add(doPart)
                        End If
                    End If
                Next
                frmCompare.ShowDialog()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Sub ToggleBodyPartPasteInProgress()

            If m_bBodyPartPasteInProgress Then
                RaiseEvent BodyPartPasteEnding()
                m_bBodyPartPasteInProgress = False
            Else
                Dim data As IDataObject = Clipboard.GetDataObject()
                If data.GetDataPresent("AnimatLab.BodyPlan.XMLFormat") Then
                    RaiseEvent BodyPartPasteStarting()
                    m_bBodyPartPasteInProgress = True
                    AddPartToolStripButton.PerformClick()
                End If
            End If

        End Sub

#End Region

#End Region

#Region " Application Events "

        Public Event ProjectLoaded() Implements ManagedAnimatInterfaces.ISimApplication.ProjectLoaded
        Public Event ProjectSaved() Implements ManagedAnimatInterfaces.ISimApplication.ProjectSaved
        Public Event ProjectClosed() Implements ManagedAnimatInterfaces.ISimApplication.ProjectClosed
        Public Event ProjectCreated() Implements ManagedAnimatInterfaces.ISimApplication.ProjectCreated
        Public Event ApplicationExiting() Implements ManagedAnimatInterfaces.ISimApplication.ApplicationExiting
        Public Event SimulationStarting() Implements ManagedAnimatInterfaces.ISimApplication.SimulationStarting
        Public Event SimulationResuming() Implements ManagedAnimatInterfaces.ISimApplication.SimulationResuming
        Public Event SimulationStarted() Implements ManagedAnimatInterfaces.ISimApplication.SimulationStarted
        Public Event SimulationPaused() Implements ManagedAnimatInterfaces.ISimApplication.SimulationPaused
        Public Event SimulationStopped() Implements ManagedAnimatInterfaces.ISimApplication.SimulationStopped
        Public Event TimeStepChanged(ByVal doObject As Framework.DataObject)
        Public Event UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                  ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                  ByVal fltMassChange As Single, _
                                  ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                  ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                  ByVal fltDistanceChange As Single)
        Public Event BodyPartPasteStarting()
        Public Event BodyPartPasteEnding()

        Public Event BeforeAddNode(ByVal doNode As Object) Implements ManagedAnimatInterfaces.ISimApplication.BeforeAddNode
        Public Event AfterAddNode(ByVal doNode As Object) Implements ManagedAnimatInterfaces.ISimApplication.AfterAddNode
        Public Event BeforeRemoveNode(ByVal doNode As Object) Implements ManagedAnimatInterfaces.ISimApplication.BeforeRemoveNode
        Public Event AfterRemoveNode(ByVal doNode As Object) Implements ManagedAnimatInterfaces.ISimApplication.AfterRemoveNode

        Public Event BeforeAddLink(ByVal doLink As Object) Implements ManagedAnimatInterfaces.ISimApplication.BeforeAddLink
        Public Event AfterAddLink(ByVal doLink As Object) Implements ManagedAnimatInterfaces.ISimApplication.AfterAddLink
        Public Event BeforeRemoveLink(ByVal doLink As Object) Implements ManagedAnimatInterfaces.ISimApplication.BeforeRemoveLink
        Public Event AfterRemoveLink(ByVal doLink As Object) Implements ManagedAnimatInterfaces.ISimApplication.AfterRemoveLink

        Public Event BeforeAddBody(ByVal doBody As Object) Implements ManagedAnimatInterfaces.ISimApplication.BeforeAddBody
        Public Event AfterAddBody(ByVal doBody As Object) Implements ManagedAnimatInterfaces.ISimApplication.AfterAddBody
        'Public Event BeforeRemoveBody(ByVal doBody As DataObjects.Physical.BodyPart)
        'Public Event AfterRemoveBody(ByVal doBody As DataObjects.Physical.BodyPart)

        Public Overridable Sub SignalTimeStepChanged(ByVal doObject As Framework.DataObject)
            RaiseEvent TimeStepChanged(doObject)
            m_doSimulation.NotifySimTimeStepChanged()
        End Sub

        Protected Event ConvertFileVersion(ByVal strProjectFile As String, ByVal iOldVersion As Integer, ByVal strPhysics As String)

        Public Overridable Sub SignalBeforeAddNode(ByVal doNode As DataObjects.Behavior.Node)
            RaiseEvent BeforeAddNode(doNode)
        End Sub

        Public Overridable Sub SignalAfterAddNode(ByVal doNode As DataObjects.Behavior.Node)
            RaiseEvent AfterAddNode(doNode)
        End Sub

        Public Overridable Sub SignalBeforeRemoveNode(ByVal doNode As DataObjects.Behavior.Node)
            RaiseEvent BeforeRemoveNode(doNode)
        End Sub

        Public Overridable Sub SignalAfterRemoveNode(ByVal doNode As DataObjects.Behavior.Node)
            RaiseEvent AfterRemoveNode(doNode)
        End Sub

        Public Overridable Sub SignalBeforeAddLink(ByVal doLink As DataObjects.Behavior.Link)
            RaiseEvent BeforeAddLink(doLink)
        End Sub

        Public Overridable Sub SignalAfterAddLink(ByVal doLink As DataObjects.Behavior.Link)
            RaiseEvent AfterAddLink(doLink)
        End Sub

        Public Overridable Sub SignalBeforeRemoveLink(ByVal doLink As DataObjects.Behavior.Link)
            RaiseEvent BeforeRemoveLink(doLink)
        End Sub

        Public Overridable Sub SignalAfterRemoveLink(ByVal doLink As DataObjects.Behavior.Link)
            RaiseEvent AfterRemoveLink(doLink)
        End Sub

        Public Overridable Sub SignalBeforeAddBody(ByVal doBody As DataObjects.Physical.BodyPart)
            RaiseEvent BeforeAddBody(doBody)
        End Sub

        Public Overridable Sub SignalAfterAddBody(ByVal doBody As DataObjects.Physical.BodyPart)
            RaiseEvent AfterAddBody(doBody)
        End Sub

#End Region

#Region " Event Handlers "

        Protected Overridable Sub OnNewProject(ByVal sender As Object, ByVal e As EventArgs) Handles NewToolStripMenuItem.Click, NewToolStripButton.Click

            Try
                Util.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Creating new project")

                If SaveIfDirty() = DialogResult.Cancel Then
                    Return
                End If

                Dim frmNewProject As New Forms.NewProject

                Util.DisableDirtyFlags = True

                Util.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Showing new project dialog")

                frmNewProject.txtProjectName.Text = "NewProject"
                frmNewProject.AllowUserToChoosePhysicsSystem = True
                If frmNewProject.ShowDialog = DialogResult.OK Then
                    Me.AppIsBusy = True

                    Util.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "User hit ok on new project dialog. Creating sim object.")

                    Me.Simulation = New DataObjects.Simulation(Me.FormHelper)

                    Util.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "sim object created.")

                    Util.Application.ProjectPath = frmNewProject.txtLocation.Text & "\" & frmNewProject.txtProjectName.Text & "\"
                    Util.Application.ProjectName = frmNewProject.txtProjectName.Text
                    Util.Application.ProjectFile = Util.Application.ProjectName & ".aproj"
                    Util.Application.SimulationFile = Util.Application.ProjectName & ".asim"

                    Dim strPro As String = ""
                    If SecurityMgr.IsValidSerialNumber Then strPro = "Pro "
                    Me.Title = "AnimatLab " & strPro & Me.ProjectName & " Project"

                    Dim doOldEngine As DataObjects.Physical.PhysicsEngine = m_doPhysics
                    m_doPhysics = DirectCast(frmNewProject.cboPhysicsEngine.SelectedItem, DataObjects.Physical.PhysicsEngine)

                    If m_doPhysics Is Nothing Then
                        Throw New System.Exception("No physics engine defined in new project dialog.")
                    End If

                    'If the physics engine has changed then recreate the list of plugin modules with the new physics engine.
                    If Not doOldEngine Is m_doPhysics Then
                        CatalogPluginModules()
                        doOldEngine = Nothing
                    End If

                    Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Creating a new Project: '" & Util.Application.ProjectPath & "\" & Util.Application.ProjectFile)

                    'Create the project directory
                    System.IO.Directory.CreateDirectory(Util.Application.ProjectPath)

                    ResetProject(True)

                    Util.Environment.MassUnits = frmNewProject.MassUnits
                    Util.Environment.DistanceUnits = frmNewProject.DistanceUnits
                    Util.Application.Physics.SetDefaultLibraryVersion()

                    Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "About to call SaveProject")

                    SaveProject(Util.Application.ProjectFile, True)

                    m_bProjectIsOpen = True
                    UpdateToolstrips()

                    Me.Simulation.VisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectCollisions

                    RaiseEvent ProjectCreated()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.AppIsBusy = False
                Util.DisableDirtyFlags = False
            End Try
        End Sub

        Protected Overridable Sub OnOpenProject(ByVal sender As Object, ByVal e As EventArgs) Handles OpenToolStripMenuItem.Click, OpenToolStripButton.Click

            Try
                If SaveIfDirty() = DialogResult.Cancel Then
                    Return
                End If

                Me.AppIsBusy = True

                Dim openFileDialog As New OpenFileDialog
                openFileDialog.Filter = "AnimatLab Project|*.aproj"
                openFileDialog.Title = "Open an AnimatLab Project"

                If openFileDialog.ShowDialog() = DialogResult.OK Then
                    LoadProject(openFileDialog.FileName)
                End If

                Me.Simulation.VisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectCollisions

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.AppIsBusy = False
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
                frmSave.AllowUserToChoosePhysicsSystem = False

                If frmSave.ShowDialog = DialogResult.OK Then
                    'Copy the current project directory
                    Dim strNewPath As String = frmSave.txtLocation.Text & "\" & frmSave.txtProjectName.Text
                    Dim strNewProjFile As String = frmSave.txtProjectName.Text & ".aproj"
                    Dim strNewSimFile As String = frmSave.txtProjectName.Text & ".asim"

                    strNewPath = strNewPath.Replace("\\", "\")

                    Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Saving project: '" & Util.Application.ProjectPath & "\" & Util.Application.ProjectFile & "' as '" & strNewPath)

                    Util.CopyDirectory(Util.Application.ProjectPath, strNewPath)

                    'Rename the project files in the new folder.
                    If File.Exists(strNewPath & "\" & Util.Application.ProjectFile) Then File.Delete(strNewPath & "\" & Util.Application.ProjectFile)
                    If File.Exists(strNewPath & "\" & Util.Application.SimulationFile) Then File.Delete(strNewPath & "\" & Util.Application.SimulationFile)

                    Util.Application.ProjectPath = frmSave.txtLocation.Text & "\" & frmSave.txtProjectName.Text & "\"
                    Util.Application.ProjectName = frmSave.txtProjectName.Text
                    Util.Application.ProjectFile = Util.Application.ProjectName & ".aproj"
                    Util.Application.SimulationFile = Util.Application.ProjectName & ".asim"

                    Dim strPro As String = ""
                    If SecurityMgr.IsValidSerialNumber Then strPro = "Pro "
                    Me.Title = "AnimatLab " & strPro & Me.ProjectName & " Project"

                    Me.AppIsBusy = True

                    SaveProject(Util.Application.ProjectFile)

                    RaiseEvent ProjectSaved()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.AppIsBusy = False
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

        Private Sub ConvertPhysicsEngineToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ConvertPhysicsEngineToolStripMenuItem.Click
            Try

                Dim frmConvert As New Forms.ConvertPhysics()
                If frmConvert.ShowDialog() = Windows.Forms.DialogResult.Yes Then
                    'First save the current project.
                    SaveProject(Me.ProjectFile)
                    If ConvertPhysicsEngine(Util.Application.Physics.Name, frmConvert.cboPhysicsEngine.SelectedItem.ToString) Then
                        'Now reopen the converted project.
                        LoadProject(Me.ProjectPath & Me.ProjectFile)
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Function FindProjectMigration(ByVal strFrom As String, ByVal strTo As String, Optional ByVal bThrowException As Boolean = True) As DataObjects.ProjectMigration

            For Each deEntry As DictionaryEntry In m_aryProjectMigrations
                Dim doMigration As DataObjects.ProjectMigration = DirectCast(deEntry.Value, DataObjects.ProjectMigration)
                If doMigration.ConvertFrom = strFrom AndAlso doMigration.ConvertTo = strTo Then
                    Return doMigration
                End If
            Next

            If bThrowException Then
                Throw New System.Exception("No project conversion was found to convert from '" & strFrom & "' to '" & strTo & "'")
            End If
            Return Nothing
        End Function

        Protected Overridable Sub OnCreateSimulation(ByRef strXml As String)
            Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.SaveStandAlone(False, False, False, False, Nothing, Nothing)
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

        Private Delegate Sub OnHandleErrorHandler(ByVal strError As String)

        ''' <summary> Executes the handle critical error action.</summary>
        '''
        ''' <remarks> If a critical error occurs then it first shows the exception to the user and starts a force shutoff timer.
        ''' 		  When the timer is up it then closes the app. I had to add a timer here because of the way the mutli-threading
        ''' 		  is working. If I attempted to close the app within this method call it locked everything up because it was the
        ''' 		  sim that was making this call, and the app needs the sim to not be blocked in order to shut down.</remarks>
        '''
        ''' <param name="strError"> The string error.</param>
        '''
        Protected Overridable Sub OnHandleCriticalError(ByVal strError As String)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New OnHandleErrorHandler(AddressOf OnHandleCriticalError), New Object() {strError})
                    Return
                End If

                Dim exInfo As New System.Exception(strError)
                Util.DisplayError(exInfo)

                m_timerShutdown = New System.Timers.Timer
                m_timerShutdown.Interval = 1000
                m_timerShutdown.Enabled = True
                AddHandler m_timerShutdown.Elapsed, AddressOf Me.OnShutDownTimer

            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Delegate Sub OnHandleShutdown()

        ''' <summary> Executes the shut down timer action.</summary>
        '''
        ''' <remarks> This is only called from the shutdown timer from the OnHandleCriticalError event method. It shuts the app down.</remarks>
        '''
        ''' <param name="sender"> Source of the event.</param>
        ''' <param name="eProps"> Event information to send to registered event handlers.</param>
        Protected Overridable Sub OnShutDownTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)
            Try
                m_timerShutdown.Enabled = False
                RemoveHandler m_timerShutdown.Elapsed, AddressOf Me.OnShutDownTimer

                If Me.InvokeRequired Then
                    Me.Invoke(New OnHandleShutdown(AddressOf HandleForceShutdown), Nothing)
                    Return
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        ''' <summary> Forces the app to shutdown. Called from the OnShutDownTimer event.</summary>
        '''
        ''' <remarks> dcofer, 7/6/2011.</remarks>
        Protected Overridable Sub HandleForceShutdown()
            Try
                m_timerShutdown.Enabled = False
                Me.Close()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        ''' <summary> Executes the handle non critical error action.</summary>
        '''
        ''' <remarks> This displays the error to the user and raises the SimulationStopped event to notify the other
        ''' 		  components in the app that the sim has stopped running.</remarks>
        '''
        ''' <param name="strError"> The string error.</param>
        Protected Overridable Sub OnHandleNonCriticalError(ByVal strError As String)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New OnHandleErrorHandler(AddressOf OnHandleNonCriticalError), New Object() {strError})
                    Return
                End If

                Dim exInfo As New System.Exception(strError)
                Util.DisplayError(exInfo)

                m_timerShutdown = New System.Timers.Timer
                m_timerShutdown.Interval = 100
                m_timerShutdown.Enabled = True
                AddHandler m_timerShutdown.Elapsed, AddressOf Me.OnStopSimTimer

            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub


        Private Delegate Sub OnStopSimTimerDel(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        ''' <summary> Executes the shut down timer action.</summary>
        '''
        ''' <remarks> This is only called from the shutdown timer from the OnHandleCriticalError event method. It shuts the app down.</remarks>
        '''
        ''' <param name="sender"> Source of the event.</param>
        ''' <param name="eProps"> Event information to send to registered event handlers.</param>
        Protected Overridable Sub OnStopSimTimer(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)
            Try
                m_timerShutdown.Enabled = False

                If Me.InvokeRequired Then
                    Me.Invoke(New OnStopSimTimerDel(AddressOf OnStopSimTimer), New Object() {sender, eProps})
                    Return
                End If

                RemoveHandler m_timerShutdown.Elapsed, AddressOf Me.OnStopSimTimer
                RaiseEvent SimulationStopped()

            Catch ex As System.Exception
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
            Me.AppStatusText = "Starting simulation"
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
            Me.AppStatusText = "Resuming simulation"
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
            Me.AppStatusText = "Pausing simulation"
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
            Me.AppStatusText = "Stopping simulation"
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

        Protected Sub OnRunMacro(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunMacroToolStripMenuItem.Click

            Try
                Dim frmMacros As New Forms.SelectMacro

                If frmMacros.ShowDialog() = DialogResult.OK AndAlso Not frmMacros.SelectedModule Is Nothing Then
                    frmMacros.SelectedModule.Execute()
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

        Protected Sub OnHelpContents(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContentsToolStripMenuItem.Click, HelpToolStripButton.Click
            Try
                Dim sInfo As New ProcessStartInfo("http://www.animatlab.com/Help/tabid/83/Default.aspx")
                Process.Start(sInfo)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnHelpTutorials(ByVal sender As Object, ByVal e As System.EventArgs) Handles TutorialsToolStripMenuItem.Click
            Try
                Dim sInfo As New ProcessStartInfo("http://www.animatlab.com/Help/Tutorials/tabid/92/Default.aspx")
                Process.Start(sInfo)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnouseCheatSheet(ByVal sender As Object, ByVal e As System.EventArgs) Handles MouseCheatSheetToolStripMenuItem.Click
            Try
                Dim sInfo As New ProcessStartInfo("http://www.animatlab.com/Help/Documentation/Simulator/tabid/89/Default.aspx")
                Process.Start(sInfo)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


        Private Sub OnHelpWarehouse(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WarehouseToolStripMenuItem.Click
            Try
                Dim sInfo As New ProcessStartInfo("http://www.animatlab.com/Community/AnimatWarehouse/tabid/240/Default.aspx")
                Process.Start(sInfo)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnTechnicalSupport(ByVal sender As Object, ByVal e As System.EventArgs) Handles SupportToolStripMenuItem.Click
            Try
                Dim sInfo As New ProcessStartInfo("http://www.animatlab.com/Community/Forum/tabid/204/Default.aspx")
                Process.Start(sInfo)
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

        Private Sub CheckForUpdatesStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckForUpdatesStripMenuItem.Click
            Try
                CheckForUpdates(True)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub RegisterStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RegisterStripMenuItem.Click
            Try
                Dim oRegDlg As New AnimatGUI.Forms.Register
                oRegDlg.ShowDialog()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AddPartToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddPartToolStripButton.Click
            Try
                Util.Simulation.VisualSelectionMode = Simulation.enumVisualSelectionMode.SelectCollisions

                If Not Me.m_selTabPage Is Nothing AndAlso Not Me.m_selTabPage.Control Is Nothing AndAlso Util.IsTypeOf(Me.m_selTabPage.Control.GetType, GetType(SimulationWindow), False) Then
                    Dim frmSimWindow As SimulationWindow = DirectCast(Me.m_selTabPage.Control, SimulationWindow)

                    If Not frmSimWindow.PhysicalStructure Is Nothing AndAlso frmSimWindow.PhysicalStructure.RootBody Is Nothing Then
                        Dim rbPasteBody As DataObjects.Physical.RigidBody = Util.GetPastedBodyPart(frmSimWindow.PhysicalStructure, Nothing, True)
                        If frmSimWindow.PhysicalStructure.AddRootBody(rbPasteBody, False) Then
                            SetSimData("LookatBodyID", frmSimWindow.PhysicalStructure.RootBody.ID, True)
                        End If
                        AddPartToolStripButton.Checked = False
                    End If
                End If

                Util.Simulation.VisualSelectionMode = Simulation.enumVisualSelectionMode.SelectCollisions

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Sub OnAddStimulus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddStimulusToolStripButton.Click, AddStimulusToolStripMenuItem.Click
            AddStimulus()
        End Sub

        Public Overridable Sub OnAddToChart(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not Util.ProjectWorkspace.SelectedDataObject Is Nothing AndAlso Not Util.Application.LastSelectedChart Is Nothing AndAlso Not Util.Application.LastSelectedChart.LastSelectedAxis Is Nothing Then
                    If Util.IsTypeOf(Util.ProjectWorkspace.SelectedDataObject.GetType, GetType(DataObjects.DragObject), False) Then
                        Dim doObj As DataObjects.DragObject = DirectCast(Util.ProjectWorkspace.SelectedDataObject, DataObjects.DragObject)

                        If doObj.CanBeCharted Then
                            Util.Application.LastSelectedChart.LastSelectedAxis.AddNewDataItem(doObj)
                        End If
                    End If

                End If


            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Sub OnSelectByType(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectByTypeToolStripMenuItem.Click, SelectByTypeToolStripButton.Click
            SelectByType()
        End Sub

        Public Sub OnRelabelSelected(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelabelSelectedToolStripMenuItem.Click, RelabelSelectedToolStripButton.Click
            RelabelSelected()
        End Sub

        Public Sub OnCompareItems(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareItemsToolStripMenuItem.Click, CompareItemsToolStripButton.Click
            CompareItems()
        End Sub

        'This is the general event that is called when someone tries to delete something in the 
        'project workspace
        Public Sub OnDeleteFromWorkspace(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteToolStripButton.Click, DeleteToolStripMenuItem.Click
            Try
                'If selected cound is 1 then just delete that object. If it is not then
                'we need some more complicated logic.
                If Util.ProjectWorkspace.TreeView.SelectedCount = 1 Then
                    DeleteSingeItem()
                ElseIf Util.ProjectWorkspace.TreeView.SelectedCount > 1 Then
                    DeleteMultipleItems()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub DeleteSingeItem()
            If Not Util.ProjectWorkspace.SelectedDataObject Is Nothing Then
                Util.ProjectWorkspace.SelectedDataObject.Delete()
            ElseIf Not Util.ProjectWorkspace.SelectedAnimatform Is Nothing Then
                Util.ProjectWorkspace.SelectedAnimatform.Delete()
            Else
                Throw New System.Exception("The selected object cannot be deleted")
            End If
        End Sub

        Protected Overridable Sub DeleteMultipleItems()
            Try
                If Util.ShowMessage("Are you certain that you want to delete the currently selected group of objects?", _
                                    "Delete Group", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                    Return
                End If

                'TODO
                'BeginGroupChange()

                'Lets go through and get a list a seperate list of the selected items 
                'so we can use it. otherwise it may change while we are deleting.
                Dim aryItems As New ArrayList
                Dim doItem As Framework.DataObject
                For Each tnNode As Crownwood.DotNetMagic.Controls.Node In Util.ProjectWorkspace.TreeView.SelectedNodes
                    If Not tnNode.Tag Is Nothing AndAlso Util.IsTypeOf(tnNode.Tag.GetType, GetType(Framework.DataObject)) Then
                        doItem = DirectCast(tnNode.Tag, Framework.DataObject)
                        aryItems.Add(doItem)
                    End If
                Next

                'First lets deselect all of the items in the selected list and select the sim object by default since we cannot delete it.
                Util.Simulation.SelectItem(False)

                aryItems.Sort(New DeleteSortComparer)

                For Each doItem In aryItems
                    doItem.Delete(False)
                Next

            Catch ex As System.Exception
                Throw ex
            Finally
                'TODO
                'EndGroupChange()
            End Try
        End Sub

        Public Sub AddNewTool(ByVal frmTool As Forms.Tools.ToolForm)

            Try
                'Now lets create a new viewer window for the tool they double clicked.
                If Not frmTool Is Nothing Then
                    Util.Simulation.NewToolHolderIndex = Util.Simulation.NewToolHolderIndex + 1
                    Dim strName As String = "DataTool_" & Util.Simulation.NewToolHolderIndex

                    Dim frmBase As Forms.Tools.ToolForm = DirectCast(frmTool.Clone(), Forms.Tools.ToolForm)

                    Me.AppIsBusy = True

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

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                If Not frmTool Is Nothing Then
                    Me.AppIsBusy = False
                End If
            End Try
        End Sub

        'Closes the app without any message boxes.
        Public Overridable Sub CloseQuiet()
            Try
                CloseProject(False, True)
                Me.Close()
            Catch ex As Exception

            End Try
        End Sub

#Region " Menu Opening Event Handlers "


        Public Overrides Sub ValidateFileToolStripItemState()

            If Not m_selTabPage Is Nothing AndAlso Not m_selTabPage.Tag Is Nothing AndAlso Util.IsTypeOf(m_selTabPage.Tag.GetType, GetType(AnimatForm), False) Then
                Dim afForm As AnimatForm = DirectCast(m_selTabPage.Tag, AnimatForm)
                afForm.ValidateFileToolStripItemState()
            End If

        End Sub

        Public Overrides Sub ValidateEditToolStripItemState()

            If Util.ProjectWorkspace Is Nothing OrElse Util.ProjectWorkspace.TreeView.SelectedCount = 0 Then
                Util.Application.DeleteToolStripMenuItem.Enabled = False
                'Util.Application.SelectAllToolStripMenu.Enabled = False
                Util.Application.SelectByTypeToolStripMenuItem.Enabled = False
                Util.Application.RelabelSelectedToolStripMenuItem.Enabled = False
            Else
                Util.Application.DeleteToolStripMenuItem.Enabled = True
                'Util.Application.SelectAllToolStripMenu.Enabled = True
                Util.Application.SelectByTypeToolStripMenuItem.Enabled = True
                Util.Application.RelabelSelectedToolStripMenuItem.Enabled = True
            End If

            'TODO
            'Util.Application.PasteToolStripButton.Enabled = False
            'Dim data As IDataObject = Clipboard.GetDataObject()
            'If Not data Is Nothing AndAlso data.GetDataPresent("AnimatLab.Behavior.XMLFormat") Then
            '    Dim strXml As String = DirectCast(data.GetData("AnimatLab.Behavior.XMLFormat"), String)
            '    If strXml.Trim.Length > 0 Then
            '        Util.Application.PasteToolStripMenuItem.Enabled = True
            '    End If
            'Else
            '    Util.Application.PasteToolStripMenuItem.Enabled = False
            'End If

            If Util.ProjectWorkspace Is Nothing OrElse Util.ProjectWorkspace.TreeView.SelectedCount < 2 Then
                Util.Application.CompareItemsToolStripMenuItem.Enabled = False
            Else
                Util.Application.CompareItemsToolStripMenuItem.Enabled = True
            End If

            If Not m_selTabPage Is Nothing AndAlso Not m_selTabPage.Tag Is Nothing AndAlso Util.IsTypeOf(m_selTabPage.Tag.GetType, GetType(AnimatForm), False) Then
                Dim afForm As AnimatForm = DirectCast(m_selTabPage.Tag, AnimatForm)
                afForm.ValidateEditToolStripItemState()
            End If

        End Sub

        Public Overrides Sub ValidateViewToolStripItemState()

            If Not m_selTabPage Is Nothing AndAlso Not m_selTabPage.Tag Is Nothing AndAlso Util.IsTypeOf(m_selTabPage.Tag.GetType, GetType(AnimatForm), False) Then
                Dim afForm As AnimatForm = DirectCast(m_selTabPage.Tag, AnimatForm)
                afForm.ValidateViewToolStripItemState()
            End If

        End Sub

        Public Overrides Sub ValidateAddToolStripItemState()

            If Not m_selTabPage Is Nothing AndAlso Not m_selTabPage.Tag Is Nothing AndAlso Util.IsTypeOf(m_selTabPage.Tag.GetType, GetType(AnimatForm), False) Then
                Dim afForm As AnimatForm = DirectCast(m_selTabPage.Tag, AnimatForm)
                afForm.ValidateAddToolStripItemState()
            End If

        End Sub

        Public Overrides Sub ValidateHelpToolStripItemState()

            If Not m_selTabPage Is Nothing AndAlso Not m_selTabPage.Tag Is Nothing AndAlso Util.IsTypeOf(m_selTabPage.Tag.GetType, GetType(AnimatForm), False) Then
                Dim afForm As AnimatForm = DirectCast(m_selTabPage.Tag, AnimatForm)
                afForm.ValidateHelpToolStripItemState()
            End If

        End Sub

        Private Sub FileToolStripMenuItem_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles FileToolStripMenuItem.DropDownOpening
            Try
                Me.ValidateFileToolStripItemState()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


        Private Sub EditToolStripMenuItem_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles EditToolStripMenuItem.DropDownOpening
            Try
                Me.ValidateEditToolStripItemState()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ViewToolStripMenuItem_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewToolStripMenuItem.DropDownOpening
            Try
                Me.ValidateViewToolStripItemState()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AddTooToolStripMenuItem_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddTooToolStripMenuItem.DropDownOpening
            Try
                Me.ValidateAddToolStripItemState()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub HelpToolStripMenuItem_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles HelpToolStripMenuItem.DropDownOpening
            Try
                Me.ValidateHelpToolStripItemState()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region


        Private Sub ConsoleToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ConsoleToolStripMenuItem.Click
            If Me.ConsoleToolStripMenuItem.Checked Then
                ShowWindow(GetConsoleWindow(), SW_SHOWNORMAL)
            Else
                ShowWindow(GetConsoleWindow(), SW_HIDE)
            End If
        End Sub

        Protected Sub AnimatApplication_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

            Try
                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Info, "Exiting the application. Project: '" & Util.Application.ProjectPath & "\" & Util.Application.ProjectFile)

                'First check to see if the application is dirty. If it is then ask to save the project
                SaveIfDirty(e)

                If e.Cancel Then Return

                'Deselect all currently selected items so that the property grids do not cause problems when closing.
                If Not Util.ProjectWorkspace Is Nothing AndAlso Not Util.ProjectWorkspace.TreeView Is Nothing AndAlso Not Util.ProjectWorkspace.TreeView.SelectedNodes Is Nothing Then
                    Util.ProjectWorkspace.ClearSelections()
                End If

                'Shutdown the simulation if it is running. Must shut this down before closing the other stuff below or it throws an exception.
                Me.SimulationInterface.ShutdownSimulation()

                'Inform the other objects that we are shutting down.
                RaiseEvent ApplicationExiting()

                'Lets close all of the children first.
                'We have to do this to make certain that all of the controls are closed 
                'down in the appropriate order.
                Dim aryForms As New ArrayList
                For Each oChild As Form In Me.ChildForms
                    aryForms.Add(oChild)
                Next

                For Each frmChild As Form In aryForms
                    frmChild.Close()
                Next

                'We also need to explicitly close the docking toolbars.
                If Not m_dockManager Is Nothing Then
                    ClearDockingContents()
                End If

                'If there is an error showing then close it as well.
                If Not Util.ErrorForm Is Nothing Then
                    Util.ErrorForm.Close()
                End If

                'close down any modal forms that are not me.
                aryForms.Clear()
                For Each frmModal As Form In Application.OpenForms
                    If frmModal.Modal AndAlso Not frmModal Is Me Then
                        aryForms.Add(frmModal)
                    End If
                Next

                For Each frmModal As Form In aryForms
                    frmModal.Close()
                Next

                m_mdiClient = Nothing
                Me.Simulation = Nothing
                'm_ipToolPanel = Nothing
                m_wcWorkspaceContent = Nothing
                m_frmWorkspace = Nothing
                m_frmToolbox = Nothing
                m_frmErrors = Nothing
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

                Me.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Debug, "Finished cleanup for application exit.")
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub


        Private Sub AnimatApplication_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If m_strCmdLineProject.Length > 0 Then
                    Me.LoadProject(m_strCmdLineProject)
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AnimatTabbedGroups_PageCloseRequest(ByVal tg As Crownwood.DotNetMagic.Controls.TabbedGroups, ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs) Handles AnimatTabbedGroups.PageCloseRequest
            Try
                If Not e.TabPage.Control Is Nothing AndAlso Util.IsTypeOf(e.TabPage.Control.GetType(), GetType(Forms.AnimatForm), False) Then
                    Dim frmAnimat As Forms.AnimatForm = DirectCast(e.TabPage.Control, Forms.AnimatForm)

                    frmAnimat.PageCloseSaveRequest(e)

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

                    Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()
                    oXml.Deserialize(strXml)

                    oXml.FindElement("TabPage")
                    oXml.FindChildElement("Form")

                    oXml.IntoElem()
                    Dim strAssembly As String = oXml.GetChildString("AssemblyFile")
                    Dim strClass As String = oXml.GetChildString("ClassName")
                    Dim strTitle As String = oXml.GetChildString("Title")
                    oXml.OutOfElem()

                    Dim frmAnimat As Forms.AnimatForm = DirectCast(CreateForm(strAssembly, strClass, strTitle, False), Forms.AnimatForm)
                    frmAnimat.Initialize(Me)
                    frmAnimat.LoadData(oXml)

                    AddChildForm(frmAnimat, e.TabPage)
                    e.TabPage.Control = frmAnimat
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AnimatTabbedGroups_PageSaving(ByVal tg As Crownwood.DotNetMagic.Controls.TabbedGroups, ByVal e As Crownwood.DotNetMagic.Controls.TGPageSavingEventArgs) Handles AnimatTabbedGroups.PageSaving
            Try
                If Not e.TabPage.Control Is Nothing AndAlso Util.IsTypeOf(e.TabPage.Control.GetType(), GetType(Forms.AnimatForm), False) Then
                    Dim frmAnimat As Forms.AnimatForm = DirectCast(e.TabPage.Control, Forms.AnimatForm)
                    Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

                    oXml.AddElement("TabPage")

                    frmAnimat.SaveData(oXml)

                    Dim strXml As String = oXml.Serialize()
                    e.XmlOut.WriteCData(strXml)

                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AnimatTabbedGroups_PageChanged(ByVal tg As Crownwood.DotNetMagic.Controls.TabbedGroups, ByVal tp As Crownwood.DotNetMagic.Controls.TabPage) Handles AnimatTabbedGroups.PageChanged

            Try
                If Not m_selTabPage Is Nothing AndAlso Not m_selTabPage.Control Is Nothing AndAlso Util.IsTypeOf(m_selTabPage.Control.GetType, GetType(Forms.AnimatForm), False) Then
                    Dim frmChild As Forms.AnimatForm = DirectCast(m_selTabPage.Control, Forms.AnimatForm)
                    frmChild.OnLoseFocus()
                End If

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

                    frmChild.OnGetFocus()
                End If

                m_selTabPage = tp

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

        Private Sub EditMaterialsToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditMaterialsToolStripButton.Click, EditMaterialsToolStripMenuItem.Click
            Try
                Dim frmEditMaterials As New Forms.BodyPlan.EditMaterialTypes
                frmEditMaterials.ShowDialog()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub SnapshotSimToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SnapshotSimToolStripMenuItem.Click
            Try
                Me.SimulationInterface.SaveSimulationFile(Me.ProjectPath & "Snapshot")

                'Me.OpenUITypeEditor("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType")
                'Me.SetObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A", "DataTypes", "IntegrateFireGUI.DataObjects.Behavior.Neurons.NonSpiking.DataTypes.ExternalCurrent")
                'Me.SetObjectProperty("Simulation\Environment\Organisms\Organism_1\Body Plan\Base", "Height", "0.3")

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

#Region "File Conversion Event Handlers"

        Public Overridable Sub ConvertProjectFile(ByVal strProjectFile As String, ByVal iOldVersion As Integer, ByVal strToPhysics As String) Handles Me.ConvertFileVersion
            Try
                Me.AppIsBusy = True

                Dim iVersion As Integer = iOldVersion
                Dim iCurrentVersion As Integer = Me.XmlVersion
                Dim strFilePhysics As String = ""
                Dim bSkipBackup As Boolean = False

                'Keep running the file converter until we convert all the way up to the latest version.
                While iVersion < iCurrentVersion
                    iVersion = ConvertProjectVersion(strProjectFile, iVersion, strFilePhysics, bSkipBackup)

                    'Only do the backup the first time the project conversion is run.
                    bSkipBackup = True
                End While

                'Also convert the physics engine if required.
                If strFilePhysics <> strToPhysics Then
                    ConvertPhysicsEngine(strFilePhysics, strToPhysics)
                End If

                If Util.ShowMessage("The project conversion was successful. Would you like to load this project now?", "Project Conversion", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                    Me.LoadProject(strProjectFile)

                    If iOldVersion = 1 Then
                        HideAllContentWidows()
                    End If

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Me.AppIsBusy = False
            End Try
        End Sub

        Protected Overridable Function ConvertProjectVersion(ByVal strProjectFile As String, ByVal iVersion As Integer, ByRef strPhysics As String, ByVal bSkipBackup As Boolean) As Integer

            'Find a converter that will convert this file type to a newer version.
            If Not m_aryProjectMigrations.ContainsKey(iVersion.ToString()) Then
                Throw New System.Exception("No file converter was found that can convert a project version '" & iVersion & "'")
            End If

            Dim doConv As DataObjects.ProjectMigration = DirectCast(m_aryProjectMigrations(iVersion.ToString()), DataObjects.ProjectMigration)

            doConv.ConvertFiles(strProjectFile, strPhysics, bSkipBackup)

            Return CInt(doConv.ConvertTo)
        End Function

        Protected Overridable Function ConvertPhysicsEngine(ByVal strOldPhysics As String, ByVal strNewPhysics As String) As Boolean

            Dim strConvertTo As String = strNewPhysics

            Dim doConv As DataObjects.ProjectMigration = FindProjectMigration(strOldPhysics, strConvertTo)

            If doConv.ConvertFiles(Me.ProjectPath & Me.ProjectFile, strConvertTo, True) Then
                Return True
            End If

            Return False
        End Function

#End Region

#End Region

#Region " Comparer classes "

        Protected Class DeleteSortComparer
            Implements IComparer

            ' Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            Function Compare(ByVal x As [Object], ByVal y As [Object]) As Integer Implements IComparer.Compare
                If Not (TypeOf x Is AnimatGUI.Framework.DataObject AndAlso TypeOf y Is AnimatGUI.Framework.DataObject) Then Return 0

                Dim doX As AnimatGUI.Framework.DataObject = DirectCast(x, AnimatGUI.Framework.DataObject)
                Dim doY As AnimatGUI.Framework.DataObject = DirectCast(y, AnimatGUI.Framework.DataObject)

                Return doX.DeleteSortCompare(doY)

            End Function 'IComparer.Compare

        End Class

#End Region

#Region "Exceptions"

        Public Class OldProjectVersion
            Inherits System.Exception

            Protected m_iOldVersion As Integer

            Public Property OldVersion() As Integer
                Get
                    Return m_iOldVersion
                End Get
                Set(ByVal Value As Integer)
                    m_iOldVersion = Value
                End Set
            End Property

            Public Sub New(ByVal strMessage As String, ByVal iOldVersion As Integer)
                MyBase.New(strMessage)

                m_iOldVersion = iOldVersion
            End Sub

        End Class

#End Region

        Protected Class ObjectPropContext
            Implements ITypeDescriptorContext

            Protected m_doObj As Framework.DataObject

            Public Sub New(ByVal doObj As Framework.DataObject)
                m_doObj = doObj
            End Sub

            Public ReadOnly Property Container As System.ComponentModel.IContainer Implements System.ComponentModel.ITypeDescriptorContext.Container
                Get

                End Get
            End Property

            Public ReadOnly Property Instance As Object Implements System.ComponentModel.ITypeDescriptorContext.Instance
                Get
                    Return m_doObj.Properties
                End Get
            End Property

            Public Sub OnComponentChanged() Implements System.ComponentModel.ITypeDescriptorContext.OnComponentChanged

            End Sub

            Public Function OnComponentChanging() As Boolean Implements System.ComponentModel.ITypeDescriptorContext.OnComponentChanging

            End Function

            Public ReadOnly Property PropertyDescriptor As System.ComponentModel.PropertyDescriptor Implements System.ComponentModel.ITypeDescriptorContext.PropertyDescriptor
                Get

                End Get
            End Property

            Public Function GetService(ByVal serviceType As System.Type) As Object Implements System.IServiceProvider.GetService

            End Function
        End Class

#Region "Autoupdater"

        Private Delegate Sub AutoUpdate_BeforeDelegate(ByVal sender As Object, ByVal e As wyDay.Controls.BeforeArgs)

        Private Sub AutoUpdate_BeforeChecking(ByVal sender As Object, ByVal e As wyDay.Controls.BeforeArgs)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New AutoUpdate_BeforeDelegate(AddressOf AutoUpdate_BeforeChecking), New Object() {sender, e})
                    Return
                End If

                Debug.WriteLine("AutoUpdate Checking for updates.")
                Me.UpdateStatusText = "Checking for updates"
            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AutoUpdate_BeforeDownloading(ByVal sender As Object, ByVal e As wyDay.Controls.BeforeArgs)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New AutoUpdate_BeforeDelegate(AddressOf AutoUpdate_BeforeDownloading), New Object() {sender, e})
                    Return
                End If

                Debug.WriteLine("AutoUpdate downloading updates.")
                Me.UpdateStatusText = "Downloading updates: 0 %"
            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AutoUpdate_BeforeExtracting(ByVal sender As Object, ByVal e As wyDay.Controls.BeforeArgs)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New AutoUpdate_BeforeDelegate(AddressOf AutoUpdate_BeforeExtracting), New Object() {sender, e})
                    Return
                End If

                Debug.WriteLine("AutoUpdate extracting updates.")
                Me.UpdateStatusText = "Extracting updates: 0 %"
            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Delegate Sub AutoUpdate_EventsDelegate(ByVal sender As Object, ByVal e As EventArgs)

        Private Sub AutoUpdate_UpdateAvailable(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New AutoUpdate_EventsDelegate(AddressOf AutoUpdate_UpdateAvailable), New Object() {sender, e})
                    Return
                End If

                Dim frmUpdate As New Forms.AnimatUpdate
                frmUpdate.SetMessage(m_auBackup.Changes)

                If frmUpdate.ShowDialog() = DialogResult.Yes Then
                    m_auBackup.InstallNow()
                End If

            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Delegate Sub AutoUpdate_SuccessArgsDelegate(ByVal sender As Object, ByVal e As wyDay.Controls.SuccessArgs)

        Private Sub AutoUpdate_UpToDate(ByVal sender As Object, ByVal e As wyDay.Controls.SuccessArgs)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New AutoUpdate_SuccessArgsDelegate(AddressOf AutoUpdate_UpToDate), New Object() {sender, e})
                    Return
                End If

                Me.UpdateStatusText = "Application is up to date"
            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Delegate Sub AutoUpdate_FailedArgsDelegate(ByVal sender As Object, ByVal e As wyDay.Controls.FailArgs)

        Private Sub AutoUpdate_CheckingFailed(ByVal sender As Object, ByVal e As wyDay.Controls.FailArgs)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New AutoUpdate_FailedArgsDelegate(AddressOf AutoUpdate_CheckingFailed), New Object() {sender, e})
                    Return
                End If

                Dim except As New System.Exception("Error occurred while checking for new updates. " & e.ErrorMessage)

                'If it fails because we are not connected to the internet then skip over this and just update the status bar.
                'Otherwise show it.
                If Not e.ErrorMessage.Contains("The remote name could not be resolved: 'www.animatlab.com'") Then
                    Util.DisplayError(except)
                End If
                Me.UpdateStatusText = "Error checking for update"
            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AutoUpdate_CloseAppNow(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New AutoUpdate_EventsDelegate(AddressOf AutoUpdate_CloseAppNow), New Object() {sender, e})
                    Return
                End If

                Me.UpdateStatusText = "Closing application"
                Me.Close()
            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AutoUpdate_Downloading_Failed(ByVal sender As Object, ByVal e As wyDay.Controls.FailArgs)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New AutoUpdate_FailedArgsDelegate(AddressOf AutoUpdate_Downloading_Failed), New Object() {sender, e})
                    Return
                End If

                Dim except As New System.Exception("Error occurred while downloading for new updates. " & e.ErrorMessage)
                Util.DisplayError(except)
                Me.UpdateStatusText = "Error downloading update"
            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AutoUpdate_ExtractingFailed(ByVal sender As Object, ByVal e As wyDay.Controls.FailArgs)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New AutoUpdate_FailedArgsDelegate(AddressOf AutoUpdate_ExtractingFailed), New Object() {sender, e})
                    Return
                End If

                Dim except As New System.Exception("Error occurred while extracting new updates. " & e.ErrorMessage)
                Util.DisplayError(except)
                Me.UpdateStatusText = "Error extracting update"
            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Delegate Sub ProgressChangedDelegate(ByVal sender As Object, ByVal progress As Integer)

        Private Sub AutoUpdate_ProgressChanged(ByVal sender As Object, ByVal progress As Integer)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New ProgressChangedDelegate(AddressOf AutoUpdate_ProgressChanged), New Object() {sender, progress})
                    Return
                End If

                If m_auBackup.UpdateStepOn = wyDay.Controls.UpdateStepOn.DownloadingUpdate Then
                    Me.UpdateStatusText = "Downloading updates: " & progress & " %"
                    Debug.WriteLine("Downloading updates: " & progress & " %.")
                ElseIf m_auBackup.UpdateStepOn = wyDay.Controls.UpdateStepOn.ExtractingUpdate Then
                    Me.UpdateStatusText = "Extracting updates: " & progress & " %"
                    Debug.WriteLine("Extracting updates: " & progress & " %.")
                Else
                    Me.UpdateStatusText = m_auBackup.UpdateStepOn.ToString & ": " & progress & " %"
                    Debug.WriteLine(m_auBackup.UpdateStepOn.ToString & ": " & progress & " %.")
                End If

            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AutoUpdate_ReadyToBeInstalled(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New AutoUpdate_EventsDelegate(AddressOf AutoUpdate_ReadyToBeInstalled), New Object() {sender, e})
                    Return
                End If

                Me.UpdateStatusText = "Installing updates"
                m_auBackup.InstallNow()
            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AutoUpdate_UpdateFailed(ByVal sender As Object, ByVal e As wyDay.Controls.FailArgs)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New AutoUpdate_FailedArgsDelegate(AddressOf AutoUpdate_UpdateFailed), New Object() {sender, e})
                    Return
                End If

                Dim except As New System.Exception("Error occurred while updating. " & e.ErrorMessage)
                Util.DisplayError(except)
                Me.UpdateStatusText = "Error applying updates"
            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AutoUpdate_UpdateSuccessful(ByVal sender As Object, ByVal e As wyDay.Controls.SuccessArgs)
            Try
                If Me.InvokeRequired Then
                    Me.Invoke(New AutoUpdate_SuccessArgsDelegate(AddressOf AutoUpdate_UpdateSuccessful), New Object() {sender, e})
                    Return
                End If

                Me.UpdateStatusText = "Update successful"
                Debug.WriteLine("Update successful.")

            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
        End Sub

#End Region



    End Class

End Namespace

