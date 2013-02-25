Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects
Imports System.Drawing.Printing
Imports System.Drawing.Imaging

Namespace Forms
    Namespace Tools

        Public Class ScriptedSimulationWindow
            Inherits ToolForm

#Region " Windows Form Designer generated code "

            Public Sub New()
                MyBase.New()

                'This call is required by the Windows Form Designer.
                InitializeComponent()

            End Sub

            'NOTE: The following procedure is required by the Windows Form Designer
            'It can be modified using the Windows Form Designer.  
            'Do not modify it using the code editor.
            <System.Diagnostics.DebuggerStepThrough()> _
            Private Sub InitializeComponent()
                Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ScriptedSimulationWindow))
                Me.SimWindowToolStrip = New AnimatGuiCtrls.Controls.AnimatToolStrip()
                Me.PasteToolStripButton = New System.Windows.Forms.ToolStripButton()
                Me.CopyToolStripButton = New System.Windows.Forms.ToolStripButton()
                Me.CutToolStripButton = New System.Windows.Forms.ToolStripButton()
                Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
                Me.AddCameraPathToolStripButton = New System.Windows.Forms.ToolStripButton()
                Me.AddWaypointToolStripButton = New System.Windows.Forms.ToolStripButton()
                Me.SimWindowMenuStrip = New AnimatGuiCtrls.Controls.AnimatMenuStrip()
                Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
                Me.AddCameraPathToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.AddWaypointToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
                Me.SimWindowToolStrip.SuspendLayout()
                Me.SimWindowMenuStrip.SuspendLayout()
                Me.SuspendLayout()
                '
                'SimWindowToolStrip
                '
                Me.SimWindowToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PasteToolStripButton, Me.CopyToolStripButton, Me.CutToolStripButton, Me.ToolStripSeparator1, Me.AddCameraPathToolStripButton, Me.AddWaypointToolStripButton})
                Me.SimWindowToolStrip.Location = New System.Drawing.Point(0, 0)
                Me.SimWindowToolStrip.Name = "SimWindowToolStrip"
                Me.SimWindowToolStrip.SecurityMgr = Nothing
                Me.SimWindowToolStrip.Size = New System.Drawing.Size(774, 25)
                Me.SimWindowToolStrip.TabIndex = 0
                Me.SimWindowToolStrip.Text = "ToolStrip1"
                Me.SimWindowToolStrip.ToolName = ""
                Me.SimWindowToolStrip.Visible = False
                '
                'PasteToolStripButton
                '
                Me.PasteToolStripButton.CheckOnClick = True
                Me.PasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.PasteToolStripButton.Image = CType(resources.GetObject("PasteToolStripButton.Image"), System.Drawing.Image)
                Me.PasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.PasteToolStripButton.MergeAction = System.Windows.Forms.MergeAction.Insert
                Me.PasteToolStripButton.MergeIndex = 4
                Me.PasteToolStripButton.Name = "PasteToolStripButton"
                Me.PasteToolStripButton.Size = New System.Drawing.Size(23, 22)
                Me.PasteToolStripButton.Text = "&Paste"
                '
                'CopyToolStripButton
                '
                Me.CopyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.CopyToolStripButton.Image = CType(resources.GetObject("CopyToolStripButton.Image"), System.Drawing.Image)
                Me.CopyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.CopyToolStripButton.MergeAction = System.Windows.Forms.MergeAction.Insert
                Me.CopyToolStripButton.MergeIndex = 4
                Me.CopyToolStripButton.Name = "CopyToolStripButton"
                Me.CopyToolStripButton.Size = New System.Drawing.Size(23, 22)
                Me.CopyToolStripButton.Text = "&Copy"
                '
                'CutToolStripButton
                '
                Me.CutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.CutToolStripButton.Image = CType(resources.GetObject("CutToolStripButton.Image"), System.Drawing.Image)
                Me.CutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.CutToolStripButton.MergeAction = System.Windows.Forms.MergeAction.Insert
                Me.CutToolStripButton.MergeIndex = 4
                Me.CutToolStripButton.Name = "CutToolStripButton"
                Me.CutToolStripButton.Size = New System.Drawing.Size(23, 22)
                Me.CutToolStripButton.Text = "C&ut"
                '
                'ToolStripSeparator1
                '
                Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
                Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
                '
                'AddCameraPathToolStripButton
                '
                Me.AddCameraPathToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.AddCameraPathToolStripButton.Image = Global.AnimatGUI.ModuleInformation.AddCameraPath
                Me.AddCameraPathToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.AddCameraPathToolStripButton.Name = "AddCameraPathToolStripButton"
                Me.AddCameraPathToolStripButton.Size = New System.Drawing.Size(23, 22)
                Me.AddCameraPathToolStripButton.Text = "Add Camera Path"
                '
                'AddWaypointToolStripButton
                '
                Me.AddWaypointToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
                Me.AddWaypointToolStripButton.Image = Global.AnimatGUI.ModuleInformation.AddCameraWaypoint
                Me.AddWaypointToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.AddWaypointToolStripButton.Name = "AddWaypointToolStripButton"
                Me.AddWaypointToolStripButton.Size = New System.Drawing.Size(23, 22)
                Me.AddWaypointToolStripButton.Text = "Add Waypoint"
                '
                'SimWindowMenuStrip
                '
                Me.SimWindowMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditToolStripMenuItem})
                Me.SimWindowMenuStrip.Location = New System.Drawing.Point(0, 0)
                Me.SimWindowMenuStrip.Name = "SimWindowMenuStrip"
                Me.SimWindowMenuStrip.SecurityMgr = Nothing
                Me.SimWindowMenuStrip.Size = New System.Drawing.Size(774, 24)
                Me.SimWindowMenuStrip.TabIndex = 1
                Me.SimWindowMenuStrip.Text = "MenuStrip1"
                Me.SimWindowMenuStrip.ToolName = ""
                Me.SimWindowMenuStrip.Visible = False
                '
                'EditToolStripMenuItem
                '
                Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PasteToolStripMenuItem, Me.CopyToolStripMenuItem, Me.CutToolStripMenuItem, Me.ToolStripSeparator2, Me.AddCameraPathToolStripMenuItem, Me.AddWaypointToolStripMenuItem})
                Me.EditToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly
                Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
                Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
                Me.EditToolStripMenuItem.Text = "&Edit"
                '
                'PasteToolStripMenuItem
                '
                Me.PasteToolStripMenuItem.CheckOnClick = True
                Me.PasteToolStripMenuItem.Image = CType(resources.GetObject("PasteToolStripMenuItem.Image"), System.Drawing.Image)
                Me.PasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.PasteToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert
                Me.PasteToolStripMenuItem.MergeIndex = 4
                Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
                Me.PasteToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
                Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
                Me.PasteToolStripMenuItem.Text = "&Paste"
                '
                'CopyToolStripMenuItem
                '
                Me.CopyToolStripMenuItem.Image = CType(resources.GetObject("CopyToolStripMenuItem.Image"), System.Drawing.Image)
                Me.CopyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.CopyToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert
                Me.CopyToolStripMenuItem.MergeIndex = 4
                Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
                Me.CopyToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
                Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
                Me.CopyToolStripMenuItem.Text = "&Copy"
                '
                'CutToolStripMenuItem
                '
                Me.CutToolStripMenuItem.Image = CType(resources.GetObject("CutToolStripMenuItem.Image"), System.Drawing.Image)
                Me.CutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta
                Me.CutToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert
                Me.CutToolStripMenuItem.MergeIndex = 4
                Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
                Me.CutToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
                Me.CutToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
                Me.CutToolStripMenuItem.Text = "Cu&t"
                '
                'ToolStripSeparator2
                '
                Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
                Me.ToolStripSeparator2.Size = New System.Drawing.Size(164, 6)
                '
                'AddCameraPathToolStripMenuItem
                '
                Me.AddCameraPathToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.AddCameraPath
                Me.AddCameraPathToolStripMenuItem.Name = "AddCameraPathToolStripMenuItem"
                Me.AddCameraPathToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
                Me.AddCameraPathToolStripMenuItem.Text = "Add Camera Path"
                '
                'AddWaypointToolStripMenuItem
                '
                Me.AddWaypointToolStripMenuItem.Image = Global.AnimatGUI.ModuleInformation.AddCameraWaypoint
                Me.AddWaypointToolStripMenuItem.Name = "AddWaypointToolStripMenuItem"
                Me.AddWaypointToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
                Me.AddWaypointToolStripMenuItem.Text = "Add Waypoint"
                '
                'ScriptedSimulationWindow
                '
                Me.ClientSize = New System.Drawing.Size(774, 262)
                Me.Controls.Add(Me.SimWindowToolStrip)
                Me.Controls.Add(Me.SimWindowMenuStrip)
                Me.MainMenuStrip = Me.SimWindowMenuStrip
                Me.Name = "ScriptedSimulationWindow"
                Me.SimWindowToolStrip.ResumeLayout(False)
                Me.SimWindowToolStrip.PerformLayout()
                Me.SimWindowMenuStrip.ResumeLayout(False)
                Me.SimWindowMenuStrip.PerformLayout()
                Me.ResumeLayout(False)
                Me.PerformLayout()

            End Sub
            Friend WithEvents SimWindowToolStrip As AnimatGuiCtrls.Controls.AnimatToolStrip
            Friend WithEvents SimWindowMenuStrip As AnimatGuiCtrls.Controls.AnimatMenuStrip
            Friend WithEvents PasteToolStripButton As System.Windows.Forms.ToolStripButton
            Friend WithEvents CopyToolStripButton As System.Windows.Forms.ToolStripButton
            Friend WithEvents CutToolStripButton As System.Windows.Forms.ToolStripButton
            Friend WithEvents CutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents AddCameraPathToolStripButton As System.Windows.Forms.ToolStripButton
            Friend WithEvents AddWaypointToolStripButton As System.Windows.Forms.ToolStripButton
            Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
            Friend WithEvents AddCameraPathToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
            Friend WithEvents AddWaypointToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

#End Region

#Region " Attributes "

            Protected m_aryCameraPaths As Collections.SortedCameraPathsList

            Protected m_clBackColor As Color = Color.Black

            Protected m_timerStartSimWindow As New System.Timers.Timer

            Protected m_tnCameraPaths As Crownwood.DotNetMagic.Controls.Node

#End Region

#Region " Properties "

            <Browsable(False)> _
            Public Overrides ReadOnly Property ToolType() As String
                Get
                    Return "ScriptedSimWindow"
                End Get
            End Property

            Public Overrides ReadOnly Property IconName() As String
                Get
                    'If Not Me.PhysicalStructure Is Nothing Then
                    '    If Util.IsTypeOf(Me.PhysicalStructure.GetType, GetType(Physical.Organism), False) Then
                    '        Return "AnimatGUI.Organism.gif"
                    '    Else
                    '        Return "AnimatGUI.Structure.gif"
                    '    End If
                    'End If
                    Return "AnimatGUI.Structure.gif"
                End Get
            End Property

            Public Overrides ReadOnly Property TabImageName() As String
                Get
                    'If Not Me.PhysicalStructure Is Nothing Then
                    '    If Util.IsTypeOf(Me.PhysicalStructure.GetType, GetType(Physical.Organism), False) Then
                    '        Return "AnimatGUI.Organism.gif"
                    '    Else
                    '        Return "AnimatGUI.Structure.gif"
                    '    End If
                    'End If
                    Return "AnimatGUI.Structure.gif"
                End Get
            End Property

            Public Overrides ReadOnly Property FormMenuStrip() As AnimatGuiCtrls.Controls.AnimatMenuStrip
                Get
                    Return Me.SimWindowMenuStrip
                End Get
            End Property

            Public Overrides ReadOnly Property FormToolStrip() As AnimatGuiCtrls.Controls.AnimatToolStrip
                Get
                    Return Me.SimWindowToolStrip
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

            Public Overridable ReadOnly Property CameraPaths() As Collections.SortedCameraPathsList
                Get
                    Return m_aryCameraPaths
                End Get
            End Property

#End Region

#Region " Methods "


            Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
                Try

                    MyBase.Initialize(frmParent)

                    m_aryCameraPaths = New Collections.SortedCameraPathsList(Me.FormHelper)

                    AddHandler Util.Application.UnitsChanged, AddressOf Me.Application_UnitsChanged
                    AddHandler Util.Application.BodyPartPasteStarting, AddressOf Me.OnBodyPartPasteStarting
                    AddHandler Util.Application.BodyPartPasteEnding, AddressOf Me.OnBodyPartPasteEnding

                    m_timerStartSimWindow.Enabled = False
                    m_timerStartSimWindow.Interval = 100

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try

            End Sub

            Public Overrides Sub ClearIsDirty()
                MyBase.ClearIsDirty()
                If Not m_aryCameraPaths Is Nothing Then m_aryCameraPaths.ClearIsDirty()
            End Sub

            Public Overrides Function Clone() As ToolForm
                Try
                    Return New Forms.Tools.ScriptedSimulationWindow
                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Function

            Public Overrides Sub InitializeAfterLoad()
                MyBase.InitializeAfterLoad()

                Dim doPath As AnimatGUI.DataObjects.Visualization.CameraPath
                For Each deEntry As DictionaryEntry In m_aryCameraPaths
                    doPath = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.CameraPath)
                    doPath.InitializeAfterLoad()
                Next
            End Sub

            Public Overrides Sub InitializeSimulationReferences(Optional bShowError As Boolean = True)
                MyBase.InitializeSimulationReferences(bShowError)

                Dim doPath As AnimatGUI.DataObjects.Visualization.CameraPath
                For Each deEntry As DictionaryEntry In m_aryCameraPaths
                    doPath = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.CameraPath)
                    doPath.InitializeSimulationReferences(bShowError)
                Next
            End Sub

            Public Overloads Overrides Sub LoadExternalFile(ByVal strFilename As String)
                Try
                    Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

                    'If no file exsists yet then one has not been saved. Just go with the default creation
                    Dim strFile As String = Util.GetFilePath(Util.Application.ProjectPath, strFilename)
                    If File.Exists(strFile) Then
                        oXml.Load(strFile)
                        oXml.FindElement("Form")
                        LoadExternalData(oXml)
                    End If

                    InitializeAfterLoad()
                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try

            End Sub

            Public Overridable Function GenerateSimWindowXml() As String
                Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

                oXml.AddElement("WindowMgr")
                oXml.AddChildElement("Window")

                oXml.IntoElem()
                oXml.AddChildElement("Name", Me.Text)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("Type", "Basic")
                oXml.AddChildElement("StandAlone", True)

                If m_aryCameraPaths.Count > 0 Then
                    oXml.AddChildElement("CameraPaths")
                    oXml.IntoElem()

                    Dim doPath As AnimatGUI.DataObjects.Visualization.CameraPath
                    For Each deEntry As DictionaryEntry In m_aryCameraPaths
                        doPath = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.CameraPath)
                        doPath.SaveSimulationXml(oXml, Me.FormHelper)
                    Next

                    oXml.OutOfElem() 'Outof Waypoints Element
                End If
                oXml.OutOfElem()

                Return oXml.Serialize()
            End Function

            Protected Overrides Sub LoadExternalData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.LoadExternalData(oXml)

                oXml.IntoElem()

                m_aryCameraPaths.Clear()
                If oXml.FindChildElement("CameraPaths", False) Then
                    Dim doPath As DataObjects.Visualization.CameraPath

                    oXml.IntoElem() 'Into paths Element
                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    For iIndex As Integer = 0 To iCount
                        oXml.FindChildByIndex(iIndex)

                        doPath = New DataObjects.Visualization.CameraPath(Me.FormHelper)
                        doPath.LoadData(oXml)

                        'Do not call the sim add method here. We will need to do that later when the window is created.
                        m_aryCameraPaths.Add(doPath.ID, doPath, False)
                    Next
                    oXml.OutOfElem()   'Outof paths Element
                End If

                oXml.OutOfElem()

            End Sub

            Protected Overrides Sub SaveExternalData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.SaveExternalData(oXml)

                oXml.IntoElem()

                If m_aryCameraPaths.Count > 0 Then
                    oXml.AddChildElement("CameraPaths")
                    oXml.IntoElem()

                    Dim doPath As AnimatGUI.DataObjects.Visualization.CameraPath
                    For Each deEntry As DictionaryEntry In m_aryCameraPaths
                        doPath = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.CameraPath)
                        doPath.SaveData(oXml)
                    Next

                    oXml.OutOfElem() 'Outof Waypoints Element
                End If

                oXml.OutOfElem()

            End Sub

#Region " Treeview/Menu Methods "

            Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, _
                                                           ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                           Optional ByVal bRootObject As Boolean = False)
                MyBase.CreateWorkspaceTreeView(doParent, tnParentNode, bRootObject)

                Dim doPath As AnimatGUI.DataObjects.Visualization.CameraPath
                For Each deEntry As DictionaryEntry In m_aryCameraPaths
                    doPath = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.CameraPath)
                    doPath.CreateWorkspaceTreeView(Me.FormHelper, m_tnWorkspaceNode)
                Next

            End Sub

            Public Overrides Function CreateObjectListTreeView(ByVal doParent As Framework.DataObject, _
                                                           ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                           ByVal mgrImageList As AnimatGUI.Framework.ImageManager) As Crownwood.DotNetMagic.Controls.Node
                Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateObjectListTreeView(doParent, tnParentNode, mgrImageList)

                If m_aryCameraPaths.Count > 0 Then
                    Dim doPath As AnimatGUI.DataObjects.Visualization.CameraPath
                    For Each deEntry As DictionaryEntry In m_aryCameraPaths
                        doPath = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Visualization.CameraPath)
                        doPath.CreateObjectListTreeView(Me.FormHelper, tnNode, mgrImageList)
                    Next
                End If

                Return tnNode
            End Function

            Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean

                For Each doPath As DataObjects.Visualization.CameraPath In m_aryCameraPaths
                    If doPath.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then Return True
                Next

                If tnSelectedNode Is m_tnWorkspaceNode Then
                    Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
                    Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

                    Dim mcAddCameraPath As New System.Windows.Forms.ToolStripMenuItem("New Camera Path", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddCameraPath.gif"), New EventHandler(AddressOf Me.AddCameraPathToolStripButton_Click))

                    mcExpandAll.Tag = tnSelectedNode
                    mcCollapseAll.Tag = tnSelectedNode

                    ' Create the popup menu object
                    Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Physical.Environment.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                    popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcExpandAll, mcCollapseAll, mcAddCameraPath})

                    Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup
                    Return True
                End If

                Return False
            End Function

#End Region

#End Region

#Region " Events "

            Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)

                Try
                    AddHandler m_timerStartSimWindow.Elapsed, AddressOf Me.OnStartSimWindow
                    m_timerStartSimWindow.Enabled = True
                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try

            End Sub

            Private Delegate Sub OnStartSimWindowDelegate(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

            Protected Overridable Sub OnStartSimWindow(ByVal sender As Object, ByVal eProps As System.Timers.ElapsedEventArgs)

                'Do not attempt to start the window until the load process is completed.
                If Util.LoadInProgress Then
                    Return
                End If

                m_timerStartSimWindow.Enabled = False

                If Me.InvokeRequired Then
                    Me.Invoke(New OnStartSimWindowDelegate(AddressOf OnStartSimWindow), New Object() {sender, eProps})
                    Return
                End If

                Try
                    'Only do this code once. If for some reason the timer fires multiple times we should only call this code if
                    'if the interface has not already been initialized.
                    If m_doInterface Is Nothing Then

                        Dim strWinXml As String = GenerateSimWindowXml()
                        Util.Application.SimulationInterface.AddWindow(Me.Handle, "ScriptedSimWindow", strWinXml)
                        InitializeSimulationReferences()
                    End If

                    'Reset the visual mode to be collisions.
                    Util.Simulation.VisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectCollisions

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

            Protected Overrides Sub OnFormClosing(ByVal e As System.Windows.Forms.FormClosingEventArgs)
                MyBase.OnFormClosing(e)

                Try
                    RemoveHandler Util.Application.UnitsChanged, AddressOf Me.Application_UnitsChanged
                    RemoveHandler Util.Application.BodyPartPasteStarting, AddressOf Me.OnBodyPartPasteStarting
                    RemoveHandler Util.Application.BodyPartPasteEnding, AddressOf Me.OnBodyPartPasteEnding

                    Util.Application.SimulationInterface.RemoveWindow(Me.Handle)
                    m_doInterface = Nothing
                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

            Protected Overrides Sub OnFormClosed(ByVal e As System.Windows.Forms.FormClosedEventArgs)
                MyBase.OnFormClosed(e)

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

            Protected Overridable Sub Application_UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
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


            Protected Sub OnBodyPartPasteStarting()
                Me.PasteToolStripMenuItem.Checked = True
                Me.PasteToolStripButton.Checked = True
            End Sub

            Protected Sub OnBodyPartPasteEnding()
                Me.PasteToolStripMenuItem.Checked = False
                Me.PasteToolStripButton.Checked = False
            End Sub

            Private Sub AddCameraPathToolStripButton_Click(sender As Object, e As System.EventArgs) Handles AddCameraPathToolStripButton.Click
                Try
                    Dim doCamera As New DataObjects.Visualization.CameraPath(Me.FormHelper)
                    doCamera.Name = "Path"
                    m_aryCameraPaths.Add(doCamera.ID, doCamera)
                    doCamera.CreateWorkspaceTreeView(Me.FormHelper, m_tnWorkspaceNode)
                    doCamera.SelectItem(False)
                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

            Public Sub AddWaypointToolStripButton_Click(sender As Object, e As System.EventArgs) Handles AddWaypointToolStripButton.Click, AddWaypointToolStripMenuItem.Click
                Try
                    If Not Util.ProjectWorkspace.SelectedDataObject Is Nothing Then
                        Dim doCameraPath As DataObjects.Visualization.CameraPath

                        If Util.IsTypeOf(Util.ProjectWorkspace.SelectedDataObject.GetType(), GetType(DataObjects.Visualization.CameraPath), False) Then
                            doCameraPath = DirectCast(Util.ProjectWorkspace.SelectedDataObject, DataObjects.Visualization.CameraPath)
                        ElseIf Util.IsTypeOf(Util.ProjectWorkspace.SelectedDataObject.GetType(), GetType(DataObjects.Visualization.Waypoint), False) Then
                            Dim doWP As DataObjects.Visualization.Waypoint = DirectCast(Util.ProjectWorkspace.SelectedDataObject, DataObjects.Visualization.Waypoint)
                            doCameraPath = DirectCast(doWP.Parent, DataObjects.Visualization.CameraPath)
                        Else
                            Throw New System.Exception("You must have a camera path or waypoint selected in order to add a waypoint.")
                        End If

                        Dim doWaypoint As New DataObjects.Visualization.Waypoint(doCameraPath)
                        doWaypoint.Name = doCameraPath.Waypoints.Count.ToString
                        doWaypoint.Time.ActualValue = doCameraPath.Waypoints.Count

                        If Not m_doInterface Is Nothing Then
                            doWaypoint.Position.X.ActualValue = m_doInterface.GetDataValueImmediate("CameraPositionX")
                            doWaypoint.Position.Y.ActualValue = m_doInterface.GetDataValueImmediate("CameraPositionY")
                            doWaypoint.Position.Z.ActualValue = m_doInterface.GetDataValueImmediate("CameraPositionZ")
                        End If

                        doCameraPath.Waypoints.Add(doWaypoint.ID, doWaypoint)
                        doWaypoint.CreateWorkspaceTreeView(doCameraPath, doCameraPath.WorkspaceNode)
                        doWaypoint.SelectItem(False)
                    End If

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

#End Region

        End Class

    End Namespace
End Namespace


