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
Imports AnimatGUI.Framework

Namespace Forms.Behavior

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
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.ctrlTreeView = New System.Windows.Forms.TreeView
            Me.SuspendLayout()
            '
            'ctrlTreeView
            '
            Me.ctrlTreeView.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ctrlTreeView.ImageIndex = -1
            Me.ctrlTreeView.Location = New System.Drawing.Point(0, 0)
            Me.ctrlTreeView.Name = "ctrlTreeView"
            Me.ctrlTreeView.SelectedImageIndex = -1
            Me.ctrlTreeView.Size = New System.Drawing.Size(292, 266)
            Me.ctrlTreeView.TabIndex = 0
            Me.ctrlTreeView.HideSelection = False
            Me.ctrlTreeView.FullRowSelect = True
            Me.ctrlTreeView.Sorted = True
            Me.ctrlTreeView.LabelEdit = True
            Me.ctrlTreeView.ItemHeight = 20
            Me.ctrlTreeView.AllowDrop = True
            '
            'Workspace
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 266)
            Me.Controls.Add(Me.ctrlTreeView)
            Me.Name = "Workspace"
            Me.Text = "Workspace"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_beEditor As AnimatGUI.Forms.Behavior.Editor

        Protected m_bFirstSelect As Boolean = True
        Protected m_bManualSelect As Boolean = False
        Protected m_imgManager As New AnimatGUI.Framework.ImageManager
        Protected m_dragImageManager As New AnimatGUI.Framework.ImageManager
        Protected m_szDragImageSize As Size

        Protected m_imageDrag As New Crownwood.Magic.Controls.ImageListDrag
        Protected m_tnRoot As TreeNode

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatGUI.ProjectWorkspace.gif"
            End Get
        End Property

        Public Overridable Property Editor() As Forms.Behavior.Editor
            Get
                Return m_beEditor
            End Get
            Set(ByVal Value As Forms.Behavior.Editor)
                m_beEditor = Value
            End Set
        End Property

        Public ReadOnly Property ImageManager() As AnimatGUI.Framework.ImageManager
            Get
                Return m_imgManager
            End Get
        End Property

        Public ReadOnly Property TreeView() As System.Windows.Forms.TreeView
            Get
                Return ctrlTreeView
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            Try

                MyBase.Initialize(frmParent)

                'm_beEditor = DirectCast(frmMdiParent, AnimatGUI.Forms.Behavior.Editor)

                m_tnRoot = ctrlTreeView.Nodes.Add("Behavioral Network")
                m_tnRoot.Tag = m_beEditor
                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                m_imgManager.AddImage(myAssembly, "AnimatGUI.Organism.gif")
                m_imgManager.AddImage(myAssembly, "AnimatGUI.DefaultObject.gif")
                m_imgManager.AddImage(myAssembly, "AnimatGUI.DefaultLink.gif")

                m_szDragImageSize = New Size(Util.Application.BehavioralNodes(0).DragImage.Width, Util.Application.BehavioralNodes(0).DragImage.Height)
                m_dragImageManager.ImageList.ImageSize = m_szDragImageSize

                m_tnRoot.ImageIndex = m_imgManager.GetImageIndex("AnimatGUI.Organism.gif")
                m_tnRoot.SelectedImageIndex = m_imgManager.GetImageIndex("AnimatGUI.Organism.gif")

                'Lets go through and add the icons for all of the nodes and links.
                Try

                    Dim doNode As DataObjects.Behavior.Node
                    For Each doNode In Util.Application.BehavioralNodes
                        If doNode.WorkspaceImageName.Trim.Length > 0 Then
                            m_imgManager.AddImage(doNode.WorkspaceImageName, doNode.WorkspaceImage)
                            m_dragImageManager.AddImage(doNode.WorkspaceImageName, doNode.DragImage, System.Drawing.Color.Transparent)
                        End If
                    Next

                    Dim doLink As DataObjects.Behavior.Link
                    For Each doLink In Util.Application.BehavioralLinks
                        If doLink.WorkspaceImageName.Trim.Length > 0 Then
                            m_imgManager.AddImage(doLink.WorkspaceImageName, doLink.WorkspaceImage)
                            m_dragImageManager.AddImage(doLink.WorkspaceImageName, doLink.DragImage, System.Drawing.Color.Transparent)
                        End If
                    Next

                Catch ex As System.Exception

                End Try

                m_imageDrag.ImageList = m_dragImageManager.ImageList
                Me.TreeView.ImageList = Me.ImageManager.ImageList

                ctrlTreeView.ExpandAll()
                'Me.Icon = Util.Application.ToolStripImages.LoadIcon(myAssembly, "AnimatGUI.DataGraph.ico")

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Function AddDiagram(ByVal tnParentNode As TreeNode, ByVal bdDiagram As Behavior.DiagramOld) As TreeNode

            Dim tnNode As TreeNode

            If tnParentNode Is Nothing Then
                tnNode = m_tnRoot.Nodes.Add(bdDiagram.TabPageName)
            Else
                tnNode = tnParentNode.Nodes.Add(bdDiagram.TabPageName)
            End If

            tnNode.Tag = bdDiagram
            bdDiagram.DiagramTreeNode = tnNode
            m_imgManager.AddImage(bdDiagram.TabImageName, bdDiagram.TabImage)
            bdDiagram.DiagramTreeNode.ImageIndex = m_imgManager.GetImageIndex(bdDiagram.TabImageName)
            bdDiagram.DiagramTreeNode.SelectedImageIndex = m_imgManager.GetImageIndex(bdDiagram.TabImageName)

            'Now add the nodes and links tree nodes
            bdDiagram.NodesTreeNode = tnNode.Nodes.Add("Nodes")
            bdDiagram.NodesTreeNode.ImageIndex = m_imgManager.GetImageIndex("AnimatGUI.DefaultObject.gif")
            bdDiagram.NodesTreeNode.SelectedImageIndex = m_imgManager.GetImageIndex("AnimatGUI.DefaultObject.gif")

            bdDiagram.LinksTreeNode = tnNode.Nodes.Add("Links")
            bdDiagram.LinksTreeNode.ImageIndex = m_imgManager.GetImageIndex("AnimatGUI.DefaultLink.gif")
            bdDiagram.LinksTreeNode.SelectedImageIndex = m_imgManager.GetImageIndex("AnimatGUI.DefaultLink.gif")

            DiagramSelected(bdDiagram)

            Return tnNode
        End Function

        Public Overridable Sub RemoveDiagram(ByVal bdDiagram As Behavior.DiagramOld)
            If Not bdDiagram.DiagramTreeNode Is Nothing AndAlso bdDiagram.DiagramTreeNode.Parent.Nodes.Contains(bdDiagram.DiagramTreeNode) Then
                bdDiagram.DiagramTreeNode.Parent.Nodes.Remove(bdDiagram.DiagramTreeNode)
                bdDiagram.DiagramTreeNode = Nothing
            End If
        End Sub

        Public Overridable Sub DiagramSelected(ByVal bdDiagram As Behavior.DiagramOld)
            m_bManualSelect = True
            ctrlTreeView.SelectedNode = bdDiagram.DiagramTreeNode
            m_bManualSelect = False
        End Sub

        Public Overridable Sub DataItemSelected(ByVal doItem As DataObjects.Behavior.Data)
            m_bManualSelect = True
            If Not doItem Is Nothing AndAlso Not doItem.WorkspaceNode Is Nothing Then
                'ctrlTreeView.SelectedNode = doItem.TreeNode
            Else
                ctrlTreeView.SelectedNode = m_tnRoot
            End If
            m_bManualSelect = False

        End Sub

#End Region

#Region " Events "

        Private Sub ctrlTreeView_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles ctrlTreeView.AfterSelect
            Try
                'This is always called the first time the form is shown for some stupid reason.
                If m_bFirstSelect OrElse m_bManualSelect Then
                    m_bFirstSelect = False
                    Return
                End If

                If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing Then
                    If TypeOf e.Node.Tag Is AnimatGUI.Forms.Behavior.DiagramOld Then
                        Dim bdDiagram As Behavior.DiagramOld = DirectCast(e.Node.Tag, Behavior.DiagramOld)
                        m_beEditor.SelectedDiagram(bdDiagram)
                    ElseIf TypeOf e.Node.Tag Is AnimatGUI.DataObjects.Behavior.Data Then
                        Dim doData As AnimatGUI.DataObjects.Behavior.Data = DirectCast(e.Node.Tag, AnimatGUI.DataObjects.Behavior.Data)

                        If Not m_beEditor.TabFiller.SelectedTab Is Nothing AndAlso m_beEditor.TabFiller.SelectedTab.Control Is Nothing _
                           AndAlso Not m_beEditor.TabFiller.SelectedTab.Control Is doData.ParentDiagram Then
                            'm_beEditor.SelectedDiagram(doData.ParentDiagram)
                        End If
                        'doData.ParentDiagram.SelectDataItem(doData, Not Me.IsCtrlKeyPressed)
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ctrlTreeView.MouseDown

            Try
                If e.Button = MouseButtons.Right Then
                    Dim ctl As Control = CType(sender, System.Windows.Forms.Control)

                    Dim tnSelected As TreeNode = ctrlTreeView.GetNodeAt(e.X, e.Y)
                    If Not tnSelected Is Nothing Then
                        ctrlTreeView.SelectedNode = tnSelected
                    End If

                    ' Create the popup menu object
                    Dim popup As New PopupMenu

                    ' Create the menu items
                    If Not tnSelected.Tag Is Nothing AndAlso (TypeOf tnSelected.Tag Is AnimatGUI.Forms.Behavior.Editor OrElse TypeOf tnSelected.Tag Is AnimatGUI.Forms.Behavior.DiagramOld) Then
                        Dim mcInsert As New MenuCommand("Insert Diagram", "InsertPage", _
                                                          New EventHandler(AddressOf Me.OnInsertDiagram))
                        popup.MenuCommands.Add(mcInsert)
                    End If

                    If Not tnSelected.Tag Is Nothing Then
                        Dim mcDelete As New MenuCommand("Delete", "Delete", Util.Application.ToolStripImages.ImageList, _
                                                        Util.Application.ToolStripImages.GetImageIndex("AnimatGUI.Delete.gif"), _
                                                        New EventHandler(AddressOf Me.OnDelete))
                        popup.MenuCommands.Add(mcDelete)
                    End If

                    Dim mcExpandAll As New MenuCommand("Expand All", tnSelected, _
                                                      New EventHandler(AddressOf Me.OnExpandAll))
                    Dim mcCollapseAll As New MenuCommand("Collapse All", tnSelected, _
                                                      New EventHandler(AddressOf Me.OnCollapseAll))

                    mcExpandAll.ImageList = Util.Application.ToolStripImages.ImageList
                    mcExpandAll.ImageIndex = Util.Application.ToolStripImages.GetImageIndex("AnimatGUI.Expand.gif")
                    mcCollapseAll.ImageList = Util.Application.ToolStripImages.ImageList
                    mcCollapseAll.ImageIndex = Util.Application.ToolStripImages.GetImageIndex("AnimatGUI.Collapse.gif")

                    ' Define the list of menu commands
                    If popup.MenuCommands.Count > 0 Then
                        Dim mcSepExpand As MenuCommand = New MenuCommand("-")
                        popup.MenuCommands.Add(mcSepExpand)
                    End If

                    popup.MenuCommands.AddRange(New MenuCommand() {mcExpandAll, mcCollapseAll})

                    ' Show it!
                    Dim selected As MenuCommand = popup.TrackPopup(ctl.PointToScreen(New Point(e.X, e.Y)))
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#Region " Drag Events "

        Private Sub ctrlTreeView_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ctrlTreeView.MouseMove

            Try
                If e.Button = MouseButtons.Left Then
                    Dim tndNode As TreeNode = ctrlTreeView.GetNodeAt(e.X, e.Y)

                    If Not tndNode Is Nothing AndAlso Not tndNode.Tag Is Nothing _
                       AndAlso TypeOf tndNode.Tag Is DataObjects.Behavior.Data Then
                        Dim doData As DataObjects.Behavior.Data = DirectCast(tndNode.Tag, DataObjects.Behavior.Data)

                        If doData.CanBeCharted Then
                            Dim dragHelper As New Framework.DataDragHelper
                            dragHelper.m_doData = doData

                            m_imageDrag.StartDrag(m_dragImageManager.GetImageIndex(doData.WorkspaceImageName), CInt(m_szDragImageSize.Width / 2), CInt(m_szDragImageSize.Height / 2))
                            Me.ctrlTreeView.DoDragDrop(dragHelper, DragDropEffects.Copy)
                            m_imageDrag.CompleteDrag()
                        End If
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub
        Private Sub ctrlTreeView_GiveFeedback(ByVal sender As Object, ByVal e As System.Windows.Forms.GiveFeedbackEventArgs) Handles ctrlTreeView.GiveFeedback

            Try
                e.UseDefaultCursors = True

                ' Draw the drag image:
                ' Debug.WriteLine("OnGiveFeedback: DraggingIcon: " + m_bDraggingIcon);
                m_imageDrag.DragDrop()

                'If Not e.Effect = DragDropEffects.None Then
                '    e.UseDefaultCursors = True
                'End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ctrlTreeView.DragEnter
            MyBase.OnDragEnter(e)
            e.Effect = DragDropEffects.Copy
        End Sub

#End Region

        Private Sub ctrlTreeView_BeforeLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles ctrlTreeView.BeforeLabelEdit
            If e.Node Is m_tnRoot Then
                e.CancelEdit = True
            End If
        End Sub

        Private Sub ctrlTreeView_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles ctrlTreeView.AfterLabelEdit
            Try
                If Not e.Node.Tag Is Nothing AndAlso Not e.Label Is Nothing Then
                    If e.Label.Trim.Length > 0 Then
                        If TypeOf (e.Node.Tag) Is Behavior.DiagramOld Then
                            Dim bdDiagram As Behavior.DiagramOld = DirectCast(e.Node.Tag, Behavior.DiagramOld)
                            m_beEditor.ChangeDiagramName(bdDiagram, e.Label)
                        ElseIf TypeOf (e.Node.Tag) Is DataObjects.Behavior.Data Then
                            Dim doData As DataObjects.Behavior.Data = DirectCast(e.Node.Tag, DataObjects.Behavior.Data)
                            doData.Text = e.Label
                        End If
                    Else
                        e.CancelEdit = True
                    End If
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnInsertDiagram(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim tnSelected As TreeNode = ctrlTreeView.SelectedNode
                If tnSelected Is Nothing Then
                    tnSelected = m_tnRoot
                End If

                If Not tnSelected.Tag Is Nothing AndAlso TypeOf (tnSelected.Tag) Is Behavior.DiagramOld Then
                    Dim bdDiagram As Behavior.DiagramOld = DirectCast(tnSelected.Tag, Behavior.DiagramOld)
                    bdDiagram.AddDiagram("LicensedAnimatGUI.dll", "LicensedAnimatGUI.Forms.Behavior.AddFlowDiagram")
                ElseIf tnSelected Is m_tnRoot Then
                    m_beEditor.AddDiagram("LicensedAnimatGUI.dll", "LicensedAnimatGUI.Forms.Behavior.AddFlowDiagram", Nothing)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnDelete(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim tnSelected As TreeNode = ctrlTreeView.SelectedNode
                If tnSelected Is Nothing Then
                    tnSelected = m_tnRoot
                End If

                If Not tnSelected.Tag Is Nothing AndAlso TypeOf (tnSelected.Tag) Is Behavior.DiagramOld Then
                    Dim bdDiagram As Behavior.DiagramOld = DirectCast(tnSelected.Tag, Behavior.DiagramOld)

                    If Not bdDiagram.ParentDiagram Is Nothing Then
                        bdDiagram.ParentDiagram.RemoveDiagram(bdDiagram)
                    Else
                        m_beEditor.RemoveDiagram(bdDiagram)
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace


