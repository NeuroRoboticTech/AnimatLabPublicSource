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
Imports AnimatTools.Framework

Namespace Forms.Tools

    Public Class Hierarchy
        Inherits AnimatForm

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

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

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents ctrlTreeView As System.Windows.Forms.TreeView
        Friend WithEvents ctrlSplitter As System.Windows.Forms.Splitter
        Friend WithEvents ctrlProperties As System.Windows.Forms.PropertyGrid
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.ctrlTreeView = New System.Windows.Forms.TreeView
            Me.ctrlSplitter = New System.Windows.Forms.Splitter
            Me.ctrlProperties = New System.Windows.Forms.PropertyGrid
            Me.SuspendLayout()
            '
            'ctrlTreeView
            '
            Me.ctrlTreeView.AllowDrop = True
            Me.ctrlTreeView.Dock = System.Windows.Forms.DockStyle.Top
            Me.ctrlTreeView.FullRowSelect = True
            Me.ctrlTreeView.HideSelection = False
            Me.ctrlTreeView.ImageIndex = -1
            Me.ctrlTreeView.ItemHeight = 20
            Me.ctrlTreeView.LabelEdit = True
            Me.ctrlTreeView.Location = New System.Drawing.Point(0, 0)
            Me.ctrlTreeView.Name = "ctrlTreeView"
            Me.ctrlTreeView.SelectedImageIndex = -1
            Me.ctrlTreeView.Size = New System.Drawing.Size(292, 128)
            Me.ctrlTreeView.Sorted = True
            Me.ctrlTreeView.TabIndex = 0
            '
            'ctrlSplitter
            '
            Me.ctrlSplitter.BackColor = System.Drawing.SystemColors.ControlDark
            Me.ctrlSplitter.Dock = System.Windows.Forms.DockStyle.Top
            Me.ctrlSplitter.Location = New System.Drawing.Point(0, 128)
            Me.ctrlSplitter.Name = "ctrlSplitter"
            Me.ctrlSplitter.Size = New System.Drawing.Size(292, 8)
            Me.ctrlSplitter.TabIndex = 1
            Me.ctrlSplitter.TabStop = False
            '
            'ctrlProperties
            '
            Me.ctrlProperties.CommandsVisibleIfAvailable = True
            Me.ctrlProperties.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ctrlProperties.LargeButtons = False
            Me.ctrlProperties.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.ctrlProperties.Location = New System.Drawing.Point(0, 136)
            Me.ctrlProperties.Name = "ctrlProperties"
            Me.ctrlProperties.Size = New System.Drawing.Size(292, 130)
            Me.ctrlProperties.TabIndex = 2
            Me.ctrlProperties.Text = "ctrlProperties"
            Me.ctrlProperties.ViewBackColor = System.Drawing.SystemColors.Window
            Me.ctrlProperties.ViewForeColor = System.Drawing.SystemColors.WindowText
            '
            'Hierarchy
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 266)
            Me.Controls.Add(Me.ctrlProperties)
            Me.Controls.Add(Me.ctrlSplitter)
            Me.Controls.Add(Me.ctrlTreeView)
            Me.Name = "Hierarchy"
            Me.Text = "Workspace"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_frmViewer As AnimatTools.Forms.Tools.Viewer

        Protected m_bFirstSelect As Boolean = True
        Protected m_bManualSelect As Boolean = False
        Protected m_imgManager As New AnimatTools.Framework.ImageManager

        Protected m_dragImageManager As New AnimatTools.Framework.ImageManager
        Protected m_imageDrag As New Crownwood.Magic.Controls.ImageListDrag
        Protected m_szDragImageSize As Size

        Protected m_tnRoot As TreeNode

        Protected m_oSelectedItem As Object
        Protected m_PropertyData As AnimatGuiCtrls.Controls.PropertyBag
        Protected m_PropertyArray() As AnimatGuiCtrls.Controls.PropertyBag

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatTools.ProjectWorkspace.gif"
            End Get
        End Property

        Public Overridable Property Viewer() As Forms.Tools.Viewer
            Get
                Return m_frmViewer
            End Get
            Set(ByVal Value As Forms.Tools.Viewer)
                m_frmViewer = Value
            End Set
        End Property

        Public ReadOnly Property ImageManager() As AnimatTools.Framework.ImageManager
            Get
                Return m_imgManager
            End Get
        End Property

        Public ReadOnly Property DragImageManager() As AnimatTools.Framework.ImageManager
            Get
                Return m_dragImageManager
            End Get
        End Property

        Public ReadOnly Property TreeView() As System.Windows.Forms.TreeView
            Get
                Return ctrlTreeView
            End Get
        End Property

        Public Property RootNode() As TreeNode
            Get
                Return m_tnRoot
            End Get
            Set(ByVal Value As TreeNode)
                m_tnRoot = Value
            End Set
        End Property

        Public Property PropertyData() As AnimatGuiCtrls.Controls.PropertyBag
            Get
                Return m_PropertyData
            End Get
            Set(ByVal Value As AnimatGuiCtrls.Controls.PropertyBag)

                Try
                    m_PropertyArray = Nothing
                    m_PropertyData = Value

                    If Not m_PropertyData Is Nothing Then
                        Me.ctrlProperties.SelectedObjects = Nothing
                        Me.ctrlProperties.SelectedObject = m_PropertyData
                    Else
                        Me.ctrlProperties.SelectedObjects = Nothing
                        Me.ctrlProperties.SelectedObject = Nothing
                    End If

                Catch ex As System.Exception
                    AnimatTools.Framework.Util.DisplayError(ex)
                End Try

            End Set
        End Property

        Public Property PropertyArray() As AnimatGuiCtrls.Controls.PropertyBag()
            Get
                Return m_PropertyArray
            End Get
            Set(ByVal Value As AnimatGuiCtrls.Controls.PropertyBag())

                Try
                    m_PropertyData = Nothing
                    m_PropertyArray = Value

                    If Not m_PropertyArray Is Nothing Then
                        Me.ctrlProperties.SelectedObject = Nothing
                        Me.ctrlProperties.SelectedObjects = m_PropertyArray
                    Else
                        Me.ctrlProperties.SelectedObject = Nothing
                        Me.ctrlProperties.SelectedObjects = Nothing
                    End If

                Catch ex As System.Exception
                    AnimatTools.Framework.Util.DisplayError(ex)
                End Try

            End Set
        End Property

        Public ReadOnly Property SelectedItem() As Object
            Get
                If Not ctrlTreeView.SelectedNode Is Nothing Then
                    Return ctrlTreeView.SelectedNode.Tag
                Else
                    Return Nothing
                End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            Try

                MyBase.Initialize(frmParent)

                'm_frmViewer = DirectCast(frmMdiParent, AnimatTools.Forms.Tools.Viewer)

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatTools")
                Me.Icon = Util.Application.SmallImages.LoadIcon(myAssembly, "AnimatTools.DefaultObject.ico")

                CreateTreeView()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Sub CreateTreeView()

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatTools")

            Me.TreeView.Nodes.Clear()

            Me.TreeView.ImageList = Me.ImageManager.ImageList
            Me.ImageManager.ImageList.ImageSize = New Size(16, 16)
            Me.ImageManager.AddImage(myAssembly, "AnimatTools.Toolbox.gif")

            m_tnRoot = ctrlTreeView.Nodes.Add("Tool Windows")
            m_tnRoot.ImageIndex = Me.ImageManager.GetImageIndex("AnimatTools.Toolbox.gif")
            m_tnRoot.SelectedImageIndex = Me.ImageManager.GetImageIndex("AnimatTools.Toolbox.gif")

            m_szDragImageSize = New Size(Util.Application.BehavioralNodes(0).DragImage.Width, Util.Application.BehavioralNodes(0).DragImage.Height)
            m_dragImageManager.ImageList.ImageSize = m_szDragImageSize
            m_imageDrag.Imagelist = m_dragImageManager.ImageList

            Dim frmTool As Forms.Tools.ToolForm
            If Not m_frmViewer.Control Is Nothing Then
                frmTool = DirectCast(m_frmViewer.Control, Forms.Tools.ToolForm)
                frmTool.CreateTreeView()
            End If

            For Each oContent As Crownwood.Magic.Docking.Content In m_frmViewer.DockManager.Contents
                If TypeOf oContent.Control Is AnimatTools.Forms.Tools.ToolForm Then
                    frmTool = DirectCast(oContent.Control, AnimatTools.Forms.Tools.ToolForm)
                    frmTool.CreateTreeView()
                End If
            Next

            ctrlTreeView.ExpandAll()
        End Sub

        Public Overridable Sub RefreshProperties()
            Me.SelectItem(m_oSelectedItem)
        End Sub

        Public Overridable Sub SelectItem(ByVal oItem As Object)

            m_oSelectedItem = Nothing

            If Not oItem Is Nothing Then
                If TypeOf oItem Is DataObjects.Charting.DataColumn Then
                    Dim doColumn As DataObjects.Charting.DataColumn = DirectCast(oItem, DataObjects.Charting.DataColumn)
                    ctrlTreeView.SelectedNode = doColumn.TreeNode
                    Me.PropertyData = doColumn.Properties()
                    m_oSelectedItem = oItem
                ElseIf TypeOf oItem Is DataObjects.Charting.Axis Then
                    Dim doAxis As DataObjects.Charting.Axis = DirectCast(oItem, DataObjects.Charting.Axis)
                    ctrlTreeView.SelectedNode = doAxis.TreeNode
                    Me.PropertyData = doAxis.Properties()
                    m_oSelectedItem = oItem
                ElseIf TypeOf oItem Is Forms.Tools.ToolForm Then
                    Dim frmTool As Forms.Tools.ToolForm = DirectCast(oItem, Forms.Tools.ToolForm)
                    ctrlTreeView.SelectedNode = frmTool.TreeNode
                    Me.PropertyData = frmTool.Properties()
                    m_oSelectedItem = oItem
                End If
            End If

            Me.ctrlProperties.Refresh()
        End Sub

#End Region

#Region " Events "

        Private Sub ctrlTreeView_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles ctrlTreeView.AfterSelect
            Try

                If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing Then
                    SelectItem(e.Node.Tag)

                    'If TypeOf e.Node.Tag Is DataObjects.Charting.DataColumn Then
                    '    Dim doColumn As DataObjects.Charting.DataColumn = DirectCast(e.Node.Tag, DataObjects.Charting.DataColumn)
                    '    Me.PropertyData = doColumn.Properties()
                    'ElseIf TypeOf e.Node.Tag Is DataObjects.Charting.Axis Then
                    '    Dim doAxis As DataObjects.Charting.Axis = DirectCast(e.Node.Tag, DataObjects.Charting.Axis)
                    '    Me.PropertyData = doAxis.Properties()
                    'ElseIf TypeOf e.Node.Tag Is Forms.Tools.ToolForm Then
                    '    Dim frmTool As Forms.Tools.ToolForm = DirectCast(e.Node.Tag, Forms.Tools.ToolForm)
                    '    Me.PropertyData = frmTool.Properties()
                    'End If
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ctrlTreeView.MouseDown

            Try
                If e.Button = MouseButtons.Right Then
                    Dim ctl As Control = CType(sender, System.Windows.Forms.Control)
                    Dim ptPoint As Point = ctl.PointToScreen(New Point(e.X, e.Y))

                    Dim tnSelected As TreeNode = ctrlTreeView.GetNodeAt(e.X, e.Y)
                    If Not tnSelected Is Nothing Then
                        ctrlTreeView.SelectedNode = tnSelected
                    End If

                    Dim oToolForm As AnimatTools.Forms.Tools.ToolForm
                    oToolForm = DirectCast(m_frmViewer.Control, AnimatTools.Forms.Tools.ToolForm)
                    If oToolForm.TreeviewPopupMenu(tnSelected, ptPoint) Then
                        Return
                    End If

                    For Each oContent As Crownwood.Magic.Docking.Content In m_frmViewer.DockManager.Contents
                        If TypeOf oContent.Control Is AnimatTools.Forms.Tools.ToolForm Then
                            oToolForm = DirectCast(oContent.Control, AnimatTools.Forms.Tools.ToolForm)
                            If oToolForm.TreeviewPopupMenu(tnSelected, ptPoint) Then
                                Return
                            End If
                        End If
                    Next

                    'If we are still here then it must be the root object selected.
                    Dim mcAddTool As New MenuCommand("Add Tool", "AddTool", _
                                                      New EventHandler(AddressOf Me.OnAddTool))

                    mcAddTool.ImageList = Util.Application.SmallImages.ImageList
                    mcAddTool.ImageIndex = Util.Application.SmallImages.GetImageIndex("AnimatTools.AddDataTool.gif")

                    Dim mcSepExpand As MenuCommand = New MenuCommand("-")
                    Dim mcExpandAll As New MenuCommand("Expand All", tnSelected, _
                                                      New EventHandler(AddressOf Me.OnExpandAll))
                    Dim mcCollapseAll As New MenuCommand("Collapse All", tnSelected, _
                                                      New EventHandler(AddressOf Me.OnCollapseAll))

                    mcExpandAll.ImageList = Util.Application.SmallImages.ImageList
                    mcExpandAll.ImageIndex = Util.Application.SmallImages.GetImageIndex("AnimatTools.Expand.gif")
                    mcCollapseAll.ImageList = Util.Application.SmallImages.ImageList
                    mcCollapseAll.ImageIndex = Util.Application.SmallImages.GetImageIndex("AnimatTools.Collapse.gif")

                    ' Create the popup menu object
                    Dim popup As New PopupMenu
                    popup.MenuCommands.AddRange(New MenuCommand() {mcAddTool, mcSepExpand, mcExpandAll, mcCollapseAll})

                    ' Show it!
                    Dim selected As MenuCommand = popup.TrackPopup(ptPoint)

                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_BeforeLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles ctrlTreeView.BeforeLabelEdit
            If e.Node Is m_tnRoot Then
                e.CancelEdit = True
            End If
        End Sub

        Private Sub ctrlTreeView_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles ctrlTreeView.AfterLabelEdit
            Try
                If Not e.Node.Tag Is Nothing AndAlso Not e.Label Is Nothing Then
                    If TypeOf (e.Node.Tag) Is AnimatTools.DataObjects.Charting.Axis Then
                        Dim doAxis As AnimatTools.DataObjects.Charting.Axis = DirectCast(e.Node.Tag, AnimatTools.DataObjects.Charting.Axis)
                        doAxis.Name = e.Label
                    ElseIf TypeOf (e.Node.Tag) Is AnimatTools.DataObjects.Charting.DataColumn Then
                        Dim doColumn As AnimatTools.DataObjects.Charting.DataColumn = DirectCast(e.Node.Tag, AnimatTools.DataObjects.Charting.DataColumn)
                        doColumn.Name = e.Label
                    End If
                End If
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ctrlTreeView_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ctrlTreeView.MouseMove

            Try
                If e.Button = MouseButtons.Left Then
                    Dim tndNode As TreeNode = ctrlTreeView.GetNodeAt(e.X, e.Y)

                    If Not tndNode Is Nothing AndAlso Not tndNode.Tag Is Nothing _
                       AndAlso TypeOf tndNode.Tag Is DataObjects.Charting.DataColumn Then
                        Dim doData As DataObjects.Charting.DataColumn = DirectCast(tndNode.Tag, DataObjects.Charting.DataColumn)
                        Dim dragHelper As New Framework.DataDragHelper
                        dragHelper.m_doData = doData

                        m_imageDrag.StartDrag(m_dragImageManager.GetImageIndex(doData.DataItemImageName), CInt(m_szDragImageSize.Width / 2), CInt(m_szDragImageSize.Height / 2))
                        Me.ctrlTreeView.DoDragDrop(dragHelper, DragDropEffects.Copy)
                        m_imageDrag.CompleteDrag()
                    End If
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_GiveFeedback(ByVal sender As Object, ByVal e As System.Windows.Forms.GiveFeedbackEventArgs) Handles ctrlTreeView.GiveFeedback

            Try
                e.UseDefaultCursors = True

                ' Draw the drag image:
                ' Debug.WriteLine("OnGiveFeedback: DraggingIcon: " + m_bDraggingIcon);
                m_imageDrag.DragDrop()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub DraggingItems(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs)

            Try
                e.Effect = DragDropEffects.None
                Me.Cursor = Cursors.Default

                If e.Data.GetDataPresent(GetType(Crownwood.Magic.Controls.PanelIcon)) OrElse _
                   e.Data.GetDataPresent(GetType(Framework.DataDragHelper)) Then

                    Dim ctl As Control = CType(sender, System.Windows.Forms.Control)
                    Dim ptPoint As Point = ctl.PointToClient(New Point(e.X, e.Y))
                    Dim tnSelected As TreeNode = ctrlTreeView.GetNodeAt(ptPoint.X, ptPoint.Y)

                    If Not tnSelected Is Nothing AndAlso Not tnSelected.Tag Is Nothing Then

                        If (TypeOf tnSelected.Tag Is AnimatTools.DataObjects.Charting.Axis AndAlso e.Data.GetDataPresent(GetType(Framework.DataDragHelper))) OrElse _
                           (TypeOf tnSelected.Tag Is Forms.Tools.ToolForm AndAlso e.Data.GetDataPresent(GetType(Crownwood.Magic.Controls.PanelIcon))) Then
                            e.Effect = DragDropEffects.Copy
                            Me.Cursor = Cursors.Arrow
                        End If

                    End If

                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ctrlTreeView.DragDrop

            Try

                If e.Data.GetDataPresent(GetType(Crownwood.Magic.Controls.PanelIcon)) OrElse _
                   e.Data.GetDataPresent(GetType(Framework.DataDragHelper)) Then

                    Dim ctl As Control = CType(sender, System.Windows.Forms.Control)
                    Dim ptPoint As Point = ctl.PointToClient(New Point(e.X, e.Y))
                    Dim tnSelected As TreeNode = ctrlTreeView.GetNodeAt(ptPoint.X, ptPoint.Y)

                    If Not tnSelected Is Nothing AndAlso Not tnSelected.Tag Is Nothing Then

                        If (TypeOf tnSelected.Tag Is AnimatTools.DataObjects.Charting.Axis AndAlso e.Data.GetDataPresent(GetType(Framework.DataDragHelper))) Then
                            Dim doAxis As AnimatTools.DataObjects.Charting.Axis = DirectCast(tnSelected.Tag, AnimatTools.DataObjects.Charting.Axis)
                            Dim doDrag As Framework.DataDragHelper = DirectCast(e.Data.GetData(GetType(Framework.DataDragHelper)), Framework.DataDragHelper)
                            doAxis.DroppedDragData(doDrag)
                        ElseIf (TypeOf tnSelected.Tag Is Forms.Tools.ToolForm AndAlso e.Data.GetDataPresent(GetType(Crownwood.Magic.Controls.PanelIcon))) Then

                        End If

                    End If

                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ctrlTreeView.DragEnter
            DraggingItems(sender, e)
        End Sub

        Private Sub ctrlTreeView_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ctrlTreeView.DragOver
            DraggingItems(sender, e)
        End Sub

        Private Sub ctrlTreeView_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlTreeView.DragLeave
            Me.Cursor = Cursors.Default
        End Sub

        Protected Overridable Sub OnAddTool(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If TypeOf Me.Viewer.Control Is Forms.Tools.ToolForm Then
                    Dim frmTool As Forms.Tools.ToolForm = DirectCast(Me.Viewer.Control, Forms.Tools.ToolForm)
                    Util.Application.AddNewTool(frmTool)
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace


