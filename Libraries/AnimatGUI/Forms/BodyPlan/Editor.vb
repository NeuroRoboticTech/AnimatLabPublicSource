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

    Public Class Editor
        Inherits Forms.ExternalFileForm

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

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
            Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddStimulusToolStripMenuItem, Me.toolStripSeparator3, Me.CutToolStripMenuItem, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem, Me.toolStripSeparator4, Me.SelectByTypeToolStripMenuItem, Me.RelabelToolStripMenuItem, Me.RelabelSelectedToolStripMenuItem, Me.CompareItemsToolStripMenuItem})
            Me.EditToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly
            Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
            Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
            Me.EditToolStripMenuItem.Text = "&Edit"
            '
            'AddStimulusToolStripMenuItem
            '
            Me.AddStimulusToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.AddStimulus
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
            Me.SelectByTypeToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.SelectByType
            Me.SelectByTypeToolStripMenuItem.Name = "SelectByTypeToolStripMenuItem"
            Me.SelectByTypeToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.SelectByTypeToolStripMenuItem.Text = "Select By Type"
            '
            'RelabelToolStripMenuItem
            '
            Me.RelabelToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.Relabel
            Me.RelabelToolStripMenuItem.Name = "RelabelToolStripMenuItem"
            Me.RelabelToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.RelabelToolStripMenuItem.Text = "Relabel"
            '
            'RelabelSelectedToolStripMenuItem
            '
            Me.RelabelSelectedToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.RelabelSelected
            Me.RelabelSelectedToolStripMenuItem.Name = "RelabelSelectedToolStripMenuItem"
            Me.RelabelSelectedToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.RelabelSelectedToolStripMenuItem.Text = "Relabel Selected"
            '
            'CompareItemsToolStripMenuItem
            '
            Me.CompareItemsToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.Equals
            Me.CompareItemsToolStripMenuItem.Name = "CompareItemsToolStripMenuItem"
            Me.CompareItemsToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
            Me.CompareItemsToolStripMenuItem.Text = "Compare Items"
            '
            'EditorToolStrip
            '
            Me.EditorToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddStimulusToolStripButton, Me.SelectByTypeToolStripButton, Me.RelabelToolStripButton, Me.RelabelSelectedToolStripButton, Me.CompareItemsToolStripButton, Me.ToolStripSeparator1, Me.AddPartToolStripButton, Me.AddJointToolStripButton})
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
            Me.AddStimulusToolStripButton.Image = Global.AnimatGUI.ModuleInformation.AddStimulus
            Me.AddStimulusToolStripButton.ImageTransparentColor = System.Drawing.Color.Cyan
            Me.AddStimulusToolStripButton.Name = "AddStimulusToolStripButton"
            Me.AddStimulusToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.AddStimulusToolStripButton.Text = "Add Stimulus"
            Me.AddStimulusToolStripButton.ToolTipText = "Add stimulus to the selected part"
            '
            'SelectByTypeToolStripButton
            '
            Me.SelectByTypeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.SelectByTypeToolStripButton.Image = Global.AnimatGUI.ModuleInformation.SelectByType
            Me.SelectByTypeToolStripButton.ImageTransparentColor = System.Drawing.Color.Cyan
            Me.SelectByTypeToolStripButton.Name = "SelectByTypeToolStripButton"
            Me.SelectByTypeToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.SelectByTypeToolStripButton.Text = "Select By Type"
            Me.SelectByTypeToolStripButton.ToolTipText = "Select by object type"
            '
            'RelabelToolStripButton
            '
            Me.RelabelToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.RelabelToolStripButton.Image = Global.AnimatGUI.ModuleInformation.Relabel
            Me.RelabelToolStripButton.ImageTransparentColor = System.Drawing.Color.Cyan
            Me.RelabelToolStripButton.Name = "RelabelToolStripButton"
            Me.RelabelToolStripButton.Size = New System.Drawing.Size(23, 22)
            Me.RelabelToolStripButton.Text = "Relabel"
            Me.RelabelToolStripButton.ToolTipText = "Relabel items using a regular expression"
            '
            'RelabelSelectedToolStripButton
            '
            Me.RelabelSelectedToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.RelabelSelectedToolStripButton.Image = Global.AnimatGUI.ModuleInformation.RelabelSelected
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
            Me.AddPartToolStripButton.CheckOnClick = True
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
            Me.CompareItemsToolStripButton.Image = Util.Application.ToolStripImages.GetImage("AnimatGUI.Equals.gif")
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
        Friend WithEvents CompareItemsToolStripButton As System.Windows.Forms.ToolStripButton

#End Region

#Region " Attributes "

        Protected m_doStructure As DataObjects.Physical.PhysicalStructure
        Protected m_strStructureID As String = ""

        Protected m_clBackColor As Color = Color.Black
        Protected m_bTrackCamera As Boolean = True

        Protected m_timerStartSimWindow As New System.Timers.Timer

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                If Not Me.PhysicalStructure Is Nothing Then
                    If Util.IsTypeOf(Me.PhysicalStructure.GetType, GetType(Physical.Organism), False) Then
                        Return "AnimatGUI.Organism.gif"
                    Else
                        Return "AnimatGUI.Structure.gif"
                    End If
                End If
                Return "AnimatGUI.Structure.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property TabImageName() As String
            Get
                If Not Me.PhysicalStructure Is Nothing Then
                    If Util.IsTypeOf(Me.PhysicalStructure.GetType, GetType(Physical.Organism), False) Then
                        Return "AnimatGUI.Organism.gif"
                    Else
                        Return "AnimatGUI.Structure.gif"
                    End If
                End If
                Return "AnimatGUI.Structure.gif"
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
                AddHandler AddPartToolStripButton.CheckStateChanged, AddressOf Util.Application.AddPartToolStripButton_CheckChanged
                AddHandler Util.Simulation.VisualSelectionModeChanged, AddressOf Me.OnVisualSelectionModeChanged

                m_timerStartSimWindow.Enabled = False
                m_timerStartSimWindow.Interval = 100

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
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
            Dim oXml As New AnimatGUI.Interfaces.StdXml()

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

        Public Overridable Sub AddStimulus()
            Try
                If Util.ProjectWorkspace.TreeView.SelectedNodes.Count > 1 OrElse Util.ProjectWorkspace.TreeView.SelectedNodes.Count <= 0 Then
                    Throw New System.Exception("Please select 1 body part to which you want to add a stimulus.")
                Else
                    If Util.ProjectWorkspace.TreeView.SelectedNode.Tag Is Nothing Then
                        Throw New System.Exception("Please select a body part to which you want to add a stimulus.")
                    End If

                    If Util.IsTypeOf(Util.ProjectWorkspace.TreeView.SelectedNode.Tag.GetType, GetType(AnimatGUI.DataObjects.Physical.BodyPart), False) Then
                        Dim doPart As Physical.BodyPart = DirectCast(Util.ProjectWorkspace.TreeView.SelectedNode.Tag, AnimatGUI.DataObjects.Physical.BodyPart)
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

                frmType.PhysicalStructure = Me.PhysicalStructure
                If frmType.ShowDialog = DialogResult.OK Then

                    Dim colObjects As New AnimatGUI.Collections.DataObjects(Nothing)
                    m_doStructure.FindChildrenOfType(frmType.SelectedType, colObjects)

                    Util.ProjectWorkspace.TreeView.ClearSelection()

                    Dim doPart As AnimatGUI.DataObjects.Physical.BodyPart
                    For Each doData As Framework.DataObject In colObjects
                        If TypeOf doData Is AnimatGUI.DataObjects.Physical.BodyPart Then
                            doPart = DirectCast(doData, AnimatGUI.DataObjects.Physical.BodyPart)
                            doPart.SelectItem(True)
                        End If
                    Next

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Sub Relabel()
            Try
                'Util.Application.ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root", "Freeze", "True"})
                'Util.Application.ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1", "LocalPosition.X", "1"})
                'Util.Application.ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1"})
                'Set the name of the data chart item to root_x.
                'Util.Application.ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Root", "Name", "Root_Y"})
                'Util.Application.ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Root_Y", "DataTypeID", "WorldPositionY"})
                Util.Application.ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart", "CollectEndTime", "45"})

                Dim frmRelabel As New AnimatGUI.Forms.BodyPlan.Relabel

                frmRelabel.PhysicalStructure = Me.PhysicalStructure
                If frmRelabel.ShowDialog = DialogResult.OK Then
                    Util.Relable(frmRelabel.Items, frmRelabel.txtMatch.Text, frmRelabel.txtReplace.Text)
                End If

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
                        Dim doPart As Physical.BodyPart
                        For Each tvItem As Crownwood.DotNetMagic.Controls.Node In Util.ProjectWorkspace.TreeView.SelectedNodes
                            If Not tvItem.Tag Is Nothing Then
                                If Util.IsTypeOf(tvItem.Tag.GetType, GetType(AnimatGUI.DataObjects.Physical.BodyPart), False) Then
                                    doPart = DirectCast(tvItem.Tag, AnimatGUI.DataObjects.Physical.BodyPart)
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
                frmCompare.PhysicalStructure = Me.PhysicalStructure

                If Util.ProjectWorkspace.TreeView.SelectedNodes.Count < 2 Then
                    Throw New System.Exception("You must select at least two objects in order to do a comparison.")
                End If

                frmCompare.SelectedItems().Clear()
                Dim doPart As Physical.BodyPart
                For Each tvItem As Crownwood.DotNetMagic.Controls.Node In Util.ProjectWorkspace.TreeView.SelectedNodes
                    If Not tvItem.Tag Is Nothing Then
                        If Util.IsTypeOf(tvItem.Tag.GetType, GetType(AnimatGUI.DataObjects.Physical.BodyPart), False) Then
                            doPart = DirectCast(tvItem.Tag, AnimatGUI.DataObjects.Physical.BodyPart)
                            frmCompare.SelectedItems.Add(doPart)
                        End If
                    End If
                Next
                frmCompare.ShowDialog()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

#Region " Events "

        Private Sub AddPartToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddPartToolStripButton.Click
            Try
                Util.Simulation.VisualSelectionMode = Simulation.enumVisualSelectionMode.SelectCollisions
                If Me.PhysicalStructure.RootBody Is Nothing Then
                    If Me.PhysicalStructure.AddRootBody() Then
                        SetSimData("LookatBodyID", Me.PhysicalStructure.RootBody.ID, True)
                    End If
                    AddPartToolStripButton.Checked = False
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub AddStimulusToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddStimulusToolStripMenuItem.Click
            AddStimulus()
        End Sub

        Private Sub SelectByTypeToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectByTypeToolStripButton.Click
            SelectByType()
        End Sub

        Private Sub RelabelToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelabelToolStripButton.Click
            Relabel()
        End Sub

        Private Sub RelabelSelectedToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelabelSelectedToolStripButton.Click
            RelabelSelected()
        End Sub

        Private Sub AddStimulusToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddStimulusToolStripButton.Click
            AddStimulus()
        End Sub

        Private Sub CompareItemsToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareItemsToolStripButton.Click
            CompareItems()
        End Sub

        Private Sub SelectByTypeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectByTypeToolStripMenuItem.Click
            SelectByType()
        End Sub

        Private Sub RelabelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelabelToolStripMenuItem.Click
            Relabel()
        End Sub

        Private Sub RelabelSelectedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RelabelSelectedToolStripMenuItem.Click
            RelabelSelected()
        End Sub

        Private Sub CompareItemsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompareItemsToolStripMenuItem.Click
            CompareItems()
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                AddHandler m_timerStartSimWindow.Elapsed, AddressOf Me.OnStartSimWindow
                m_timerStartSimWindow.Enabled = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Delegate Sub OnStartSimWindowDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

        Protected Overridable Sub OnStartSimWindow(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            m_timerStartSimWindow.Enabled = False

            If Me.InvokeRequired Then
                Me.Invoke(New OnStartSimWindowDelegate(AddressOf OnStartSimWindow), New Object() {sender, eProps})
                Return
            End If

            Try
                Dim strWinXml As String = GenerateSimWindowXml()
                Util.Application.SimulationInterface.AddWindow(Me.Handle, strWinXml)
                InitializeSimulationReferences()
 
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnFormClosing(ByVal e As System.Windows.Forms.FormClosingEventArgs)
            MyBase.OnFormClosing(e)

            Try
                RemoveHandler Util.Application.UnitsChanged, AddressOf Me.Application_UnitsChanged
                Util.Application.SimulationInterface.RemoveWindow(Me.Handle)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnFormClosed(ByVal e As System.Windows.Forms.FormClosedEventArgs)
            MyBase.OnFormClosed(e)

            If Not m_doStructure Is Nothing Then
                m_doStructure.BodyEditor = Nothing
            End If

        End Sub

        Public Overrides Sub OnGetFocus()
            MyBase.OnGetFocus()

            Try
                If Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                    Util.Application.SimulationInterface.OnWindowGetFocus(Me.ID)
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Sub OnLoseFocus()
            MyBase.OnLoseFocus()

            Try
                If Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                    Util.Application.SimulationInterface.OnWindowLoseFocus(Me.ID)
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub Application_UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                  ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                  ByVal fltMassChange As Single, _
                                  ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                  ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                  ByVal fltDistanceChange As Single)
            Try
                Util.ProjectProperties.RefreshProperties()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnVisualSelectionModeChanged()
            Try
                If Me.AddPartToolStripButton.Checked AndAlso Util.Simulation.VisualSelectionMode <> Simulation.enumVisualSelectionMode.SelectCollisions Then
                    Me.AddPartToolStripButton.Checked = False
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

