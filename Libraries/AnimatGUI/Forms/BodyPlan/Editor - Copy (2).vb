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
Imports AnimatTools
Imports AnimatTools.Framework
Imports AnimatTools.DataObjects
Imports System.Drawing.Printing
Imports System.Drawing.Imaging

Namespace Forms.BodyPlan

    Public Class Editor
        Inherits Forms.ExternalFileForm

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call
            'Add a new hud item to display the time and the axis by default
            m_aryHudItems.Clear()
            m_aryHudItems.Add(New DataObjects.Visualization.HudItem(Nothing))

        End Sub

        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Editor))
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
            Me.SelectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
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
            Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton
            Me.ShowGraphicsGeometryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowCollisionGeometryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowJointsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowCenterOfMassToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowPartOriginToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ShowContactsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
            Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
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
            Me.EditorMenuStrip.Size = New System.Drawing.Size(284, 24)
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
            Me.PrintToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
            Me.PrintToolStripMenuItem.Text = "&Print"
            '
            'PrintPreviewToolStripMenuItem
            '
            Me.PrintPreviewToolStripMenuItem.Image = CType(resources.GetObject("PrintPreviewToolStripMenuItem.Image"), System.Drawing.Image)
            Me.PrintPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.PrintPreviewToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.PrintPreviewToolStripMenuItem.MergeIndex = 7
            Me.PrintPreviewToolStripMenuItem.Name = "PrintPreviewToolStripMenuItem"
            Me.PrintPreviewToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
            Me.PrintPreviewToolStripMenuItem.Text = "Print Pre&view"
            '
            'toolStripSeparator2
            '
            Me.toolStripSeparator2.MergeAction = System.Windows.Forms.MergeAction.Insert
            Me.toolStripSeparator2.MergeIndex = 8
            Me.toolStripSeparator2.Name = "toolStripSeparator2"
            Me.toolStripSeparator2.Size = New System.Drawing.Size(140, 6)
            '
            'EditToolStripMenuItem
            '
            Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddStimulusToolStripMenuItem, Me.toolStripSeparator3, Me.CutToolStripMenuItem, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.toolStripSeparator4, Me.SelectAllToolStripMenuItem, Me.RelabelToolStripMenuItem, Me.RelabelSelectedToolStripMenuItem, Me.CompareItemsToolStripMenuItem})
            Me.EditToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly
            Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
            Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
            Me.EditToolStripMenuItem.Text = "&Edit"
            '
            'AddStimulusToolStripMenuItem
            '
            Me.AddStimulusToolStripMenuItem.Image = Global.AnimatTools.ModuleInformation.AddStimulus
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
            'SelectAllToolStripMenuItem
            '
            Me.SelectAllToolStripMenuItem.Image = Global.AnimatTools.ModuleInformation.SelectByType
            Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
            Me.SelectAllToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.SelectAllToolStripMenuItem.Text = "Select By Type"
            '
            'RelabelToolStripMenuItem
            '
            Me.RelabelToolStripMenuItem.Image = Global.AnimatTools.ModuleInformation.Relabel
            Me.RelabelToolStripMenuItem.Name = "RelabelToolStripMenuItem"
            Me.RelabelToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.RelabelToolStripMenuItem.Text = "Relabel"
            '
            'RelabelSelectedToolStripMenuItem
            '
            Me.RelabelSelectedToolStripMenuItem.Image = Global.AnimatTools.ModuleInformation.RelabelSelected
            Me.RelabelSelectedToolStripMenuItem.Name = "RelabelSelectedToolStripMenuItem"
            Me.RelabelSelectedToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.RelabelSelectedToolStripMenuItem.Text = "Relabel Selected"
            '
            'CompareItemsToolStripMenuItem
            '
            Me.CompareItemsToolStripMenuItem.Image = Global.AnimatTools.ModuleInformation.Equals
            Me.CompareItemsToolStripMenuItem.Name = "CompareItemsToolStripMenuItem"
            Me.CompareItemsToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.CompareItemsToolStripMenuItem.Text = "Compare Items"
            '
            'EditorToolStrip
            '
            Me.EditorToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddStimulusToolStripButton, Me.SelectByTypeToolStripButton, Me.RelabelToolStripButton, Me.RelabelSelectedToolStripButton, Me.ToolStripSeparator1, Me.AddPartToolStripButton, Me.AddJointToolStripButton, Me.ToolStripDropDownButton1, Me.ToolStripSeparator5})
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
            Me.AddStimulusToolStripButton.Image = Global.AnimatTools.ModuleInformation.AddStimulus
            Me.AddStimulusToolStripButton.ImageTransparentColor = System.Drawing.Color.Cyan
            Me.AddStimulusToolStripButton.Name = "AddStimulusToolStripButton"
            Me.AddStimulusToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddStimulusToolStripButton.Text = "Add Stimulus"
            Me.AddStimulusToolStripButton.ToolTipText = "Add stimulus to the selected part"
            '
            'SelectByTypeToolStripButton
            '
            Me.SelectByTypeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.SelectByTypeToolStripButton.Image = Global.AnimatTools.ModuleInformation.SelectByType
            Me.SelectByTypeToolStripButton.ImageTransparentColor = System.Drawing.Color.Cyan
            Me.SelectByTypeToolStripButton.Name = "SelectByTypeToolStripButton"
            Me.SelectByTypeToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.SelectByTypeToolStripButton.Text = "Select By Type"
            Me.SelectByTypeToolStripButton.ToolTipText = "Select by object type"
            '
            'RelabelToolStripButton
            '
            Me.RelabelToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.RelabelToolStripButton.Image = Global.AnimatTools.ModuleInformation.Relabel
            Me.RelabelToolStripButton.ImageTransparentColor = System.Drawing.Color.Cyan
            Me.RelabelToolStripButton.Name = "RelabelToolStripButton"
            Me.RelabelToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.RelabelToolStripButton.Text = "Relabel"
            Me.RelabelToolStripButton.ToolTipText = "Relabel items using a regular expression"
            '
            'RelabelSelectedToolStripButton
            '
            Me.RelabelSelectedToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.RelabelSelectedToolStripButton.Image = Global.AnimatTools.ModuleInformation.RelabelSelected
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
            Me.ShowGraphicsGeometryToolStripMenuItem.Name = "ShowGraphicsGeometryToolStripMenuItem"
            Me.ShowGraphicsGeometryToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
            Me.ShowGraphicsGeometryToolStripMenuItem.Text = "Show Graphics Geometry"
            '
            'ShowCollisionGeometryToolStripMenuItem
            '
            Me.ShowCollisionGeometryToolStripMenuItem.Name = "ShowCollisionGeometryToolStripMenuItem"
            Me.ShowCollisionGeometryToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
            Me.ShowCollisionGeometryToolStripMenuItem.Text = "Show Collision Geometry"
            '
            'ShowJointsToolStripMenuItem
            '
            Me.ShowJointsToolStripMenuItem.Name = "ShowJointsToolStripMenuItem"
            Me.ShowJointsToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
            Me.ShowJointsToolStripMenuItem.Text = "Show Joints"
            '
            'ShowCenterOfMassToolStripMenuItem
            '
            Me.ShowCenterOfMassToolStripMenuItem.Name = "ShowCenterOfMassToolStripMenuItem"
            Me.ShowCenterOfMassToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
            Me.ShowCenterOfMassToolStripMenuItem.Text = "Show Center of Mass"
            '
            'ShowPartOriginToolStripMenuItem
            '
            Me.ShowPartOriginToolStripMenuItem.Name = "ShowPartOriginToolStripMenuItem"
            Me.ShowPartOriginToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
            Me.ShowPartOriginToolStripMenuItem.Text = "Show Part Origin"
            '
            'ShowContactsToolStripMenuItem
            '
            Me.ShowContactsToolStripMenuItem.Name = "ShowContactsToolStripMenuItem"
            Me.ShowContactsToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
            Me.ShowContactsToolStripMenuItem.Text = "Show Contacts"
            '
            'ToolStripSeparator5
            '
            Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
            Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 25)
            '
            'Editor
            '
            Me.ClientSize = New System.Drawing.Size(387, 262)
            Me.Controls.Add(Me.EditorToolStrip)
            Me.Controls.Add(Me.EditorMenuStrip)
            Me.MainMenuStrip = Me.EditorMenuStrip
            Me.Name = "Editor"
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
        Friend WithEvents SelectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
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
        Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents ToolStripDropDownButton1 As System.Windows.Forms.ToolStripDropDownButton
        Friend WithEvents ShowCollisionGeometryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowJointsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowCenterOfMassToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowPartOriginToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowContactsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddPartToolStripButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents ShowGraphicsGeometryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AddJointToolStripButton As System.Windows.Forms.ToolStripButton

#End Region

#Region " Attributes "

        Protected m_doStructure As DataObjects.Physical.PhysicalStructure
        Protected m_strStructureID As String = ""

        Protected m_clBackColor As Color = Color.Black
        Protected m_aryHudItems As New ArrayList
        Protected m_bTrackCamera As Boolean = True

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                If Not Me.PhysicalStructure Is Nothing Then
                    If Util.IsTypeOf(Me.PhysicalStructure.GetType, GetType(Physical.Organism), False) Then
                        Return "AnimatTools.Organism.gif"
                    Else
                        Return "AnimatTools.Structure.gif"
                    End If
                End If
                Return "AnimatTools.Structure.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property TabImageName() As String
            Get
                If Not Me.PhysicalStructure Is Nothing Then
                    If Util.IsTypeOf(Me.PhysicalStructure.GetType, GetType(Physical.Organism), False) Then
                        Return "AnimatTools.Organism.gif"
                    Else
                        Return "AnimatTools.Structure.gif"
                    End If
                End If
                Return "AnimatTools.Structure.gif"
            End Get
        End Property

        Public Overridable Property PhysicalStructure() As DataObjects.Physical.PhysicalStructure
            Get
                Return m_doStructure
            End Get
            Set(ByVal Value As DataObjects.Physical.PhysicalStructure)
                m_doStructure = Value

                m_strStructureID = ""
                If Not m_doStructure Is Nothing Then
                    m_strStructureID = m_doStructure.ID
                    SetSimData("LookatStructureID", m_doStructure.ID, True)
                End If
            End Set
        End Property

        Public Overrides ReadOnly Property FormMenuStrip() As AnimatGuiCtrls.Controls.AnimatMenuStrip
            Get
                Return Me.EditorMenuStrip
            End Get
        End Property

        Public Overrides ReadOnly Property FormToolStrip() As AnimatGuiCtrls.Controls.AnimatToolStrip
            Get
                Return Me.EditorToolStrip
            End Get
        End Property

        Public Overridable ReadOnly Property StructureID() As String
            Get
                Return m_strStructureID
            End Get
        End Property

        Public Overridable ReadOnly Property CameraPosition() As Vec3d
            Get
                Return New Vec3d(Nothing, 0, 0, 0)
            End Get
        End Property

        Public Overridable Property ViewBackColor() As Color
            Get
                Return m_clBackColor
            End Get
            Set(ByVal Value As Color)
                m_clBackColor = Value
            End Set
        End Property

#End Region

#Region " Methods "


        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            Try

                MyBase.Initialize(frmParent)

                AddHandler Util.Application.UnitsChanged, AddressOf Me.Application_UnitsChanged

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub


        Protected Overrides Sub ReconnectFormToWorkspace()

            For Each deEntry As DictionaryEntry In Util.Environment.Structures
                Dim doStruct As DataObjects.Physical.PhysicalStructure = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)
                If doStruct.ID = Me.StructureID Then
                    Me.PhysicalStructure = doStruct
                    doStruct.BodyEditor = Me
                    Return
                End If
            Next

        End Sub

        Public Overridable Function GenerateSimWindowXml() As String
            Dim oXml As New AnimatTools.Interfaces.StdXml()

            oXml.AddElement("WindowMgr")
            oXml.AddChildElement("Window")

            oXml.IntoElem()
            oXml.AddChildElement("Name", Me.Text)
            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Type", "Basic")
            oXml.AddChildElement("StandAlone", True)

            If Not Me.PhysicalStructure Is Nothing Then
                oXml.AddChildElement("TrackCamera", m_bTrackCamera)
                oXml.AddChildElement("LookAtStructureID", Me.PhysicalStructure.ID)
                oXml.AddChildElement("LookAtBodyID", "")
            Else
                oXml.AddChildElement("TrackCamera", False)
                oXml.AddChildElement("LookAtStructureID", "")
                oXml.AddChildElement("LookAtBodyID", "")
            End If

            oXml.AddChildElement("HudItems")
            oXml.IntoElem()

            For Each hudItem As DataObjects.Visualization.HudItem In m_aryHudItems
                hudItem.SaveData(oXml)
            Next

            oXml.OutOfElem()

            oXml.OutOfElem()

            Return oXml.Serialize()
        End Function

        Public Overrides Sub LoadData(ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()
            m_strStructureID = oXml.GetChildString("StructureID")
            oXml.OutOfElem()

            ReconnectFormToWorkspace()
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()
            oXml.AddChildElement("StructureID", m_strStructureID)
            oXml.OutOfElem()

        End Sub

#End Region

#Region " Events "

        Private Sub AddPartToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddPartToolStripButton.Click
            Try
                If Me.PhysicalStructure.RootBody Is Nothing Then
                    If Me.PhysicalStructure.AddRootBody() Then
                        SetSimData("LookatBodyID", Me.PhysicalStructure.RootBody.ID, True)
                    End If
                End If
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                Dim strWinXml As String = GenerateSimWindowXml()
                Util.Application.SimulationInterface.AddWindow(Me.Handle, strWinXml)
                InitializeSimulationReferences()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overrides Sub OnFormClosing(ByVal e As System.Windows.Forms.FormClosingEventArgs)
            MyBase.OnFormClosing(e)

            Try
                RemoveHandler Util.Application.UnitsChanged, AddressOf Me.Application_UnitsChanged
                Util.Application.SimulationInterface.RemoveWindow(Me.Handle)
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnFormClosed(ByVal e As System.Windows.Forms.FormClosedEventArgs)
            MyBase.OnFormClosed(e)

            If Not m_doStructure Is Nothing Then
                m_doStructure.BodyEditor = Nothing
            End If

        End Sub


        Protected Sub Application_UnitsChanged(ByVal ePrevMass As AnimatTools.DataObjects.Physical.Environment.enumMassUnits, _
                                  ByVal eNewMass As AnimatTools.DataObjects.Physical.Environment.enumMassUnits, _
                                  ByVal fltMassChange As Single, _
                                  ByVal ePrevDistance As AnimatTools.DataObjects.Physical.Environment.enumDistanceUnits, _
                                  ByVal eNewDistance As AnimatTools.DataObjects.Physical.Environment.enumDistanceUnits, _
                                  ByVal fltDistanceChange As Single)
            Try
                Util.ProjectProperties.RefreshProperties()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

