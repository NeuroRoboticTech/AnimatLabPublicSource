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

Namespace Forms.BodyPlan

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

        Protected m_beEditor As AnimatTools.Forms.BodyPlan.Editor

        Protected m_bFirstSelect As Boolean = True
        Protected m_bManualSelect As Boolean = False
        Protected m_imgManager As New AnimatTools.Framework.ImageManager

        Protected m_dragImageManager As New AnimatTools.Framework.ImageManager
        Protected m_szDragImageSize As Size
        Protected m_imageDrag As New Crownwood.Magic.Controls.ImageListDrag

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatTools.ProjectWorkspace.gif"
            End Get
        End Property

        Public Overridable Property Editor() As Forms.Bodyplan.Editor
            Get
                Return m_beEditor
            End Get
            Set(ByVal Value As Forms.Bodyplan.Editor)
                m_beEditor = Value
            End Set
        End Property

        Public ReadOnly Property ImageManager() As AnimatTools.Framework.ImageManager
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

                'm_beEditor = DirectCast(frmMdiParent, AnimatTools.Forms.BodyPlan.Editor)

                'Dim myAssembly As System.Reflection.Assembly
                'myAssembly = System.Reflection.Assembly.Load("AnimatTools")

                Me.TreeView.ImageList = Me.ImageManager.ImageList

                'Lets go through and add the icons for all of the nodes and links.
                Try

                    Dim doPart As DataObjects.Physical.BodyPart
                    For Each doPart In Util.Application.BodyPartTypes
                        If doPart.WorkspaceImageName.Trim.Length > 0 Then
                            m_imgManager.AddImage(doPart.WorkspaceImageName, doPart.WorkspaceImage)
                            m_dragImageManager.AddImage(doPart.WorkspaceImageName, doPart.DragImage, System.Drawing.Color.Transparent)
                        End If
                    Next

                    If Not Util.Application.BodyPartTypes(0).DragImage Is Nothing Then
                        m_szDragImageSize = New Size(Util.Application.BodyPartTypes(0).DragImage.Width, Util.Application.BodyPartTypes(0).DragImage.Height)
                    Else
                        m_szDragImageSize = New Size(16, 16)
                    End If
                    m_dragImageManager.ImageList.ImageSize = m_szDragImageSize

                Catch ex As System.Exception

                End Try

                m_imageDrag.ImageList = m_dragImageManager.ImageList
                Me.TreeView.ImageList = Me.ImageManager.ImageList

                'Me.Icon = Util.Application.ToolStripImages.LoadIcon(myAssembly, "AnimatTools.DataGraph.ico")

                ctrlTreeView.ExpandAll()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " Events "

        Private Sub ctrlTreeView_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles ctrlTreeView.AfterSelect
            Try

                If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing Then
                    If TypeOf e.Node.Tag Is AnimatTools.DataObjects.Physical.BodyPart Then
                        Dim doPart As AnimatTools.DataObjects.Physical.BodyPart = DirectCast(e.Node.Tag, AnimatTools.DataObjects.Physical.BodyPart)
                        If ctrlTreeView.ModifierKeys = Keys.Control Then
                            Me.Editor.PropertiesBar.SelectPart(doPart, True)
                        Else
                            Me.Editor.PropertiesBar.SelectPart(doPart, False)
                        End If
                    ElseIf TypeOf e.Node.Tag Is AnimatTools.DataObjects.Physical.PhysicalStructure Then
                        Dim doStruct As AnimatTools.DataObjects.Physical.PhysicalStructure = DirectCast(e.Node.Tag, AnimatTools.DataObjects.Physical.PhysicalStructure)
                        Me.Editor.PropertiesBar.SelectedStructure = doStruct
                    End If
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

                    If Not m_beEditor Is Nothing AndAlso Not m_beEditor.PhysicalStructure Is Nothing Then
                        'If m_beEditor.PhysicalStructure.BodyPlanTreeviewPopupMenu(tnSelected, ptPoint) Then
                        '    Return
                        'End If
                    End If

                End If
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_BeforeLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles ctrlTreeView.BeforeLabelEdit
            'If Not m_beEditor Is Nothing AndAlso Not m_beEditor.PhysicalStructure Is Nothing AndAlso e.Node Is m_beEditor.PhysicalStructure.BodyPlanStructureNode Then
            '    e.CancelEdit = True
            'End If
        End Sub

        Private Sub ctrlTreeView_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles ctrlTreeView.AfterLabelEdit
            Try
                If Not Me.Editor Is Nothing AndAlso Not Me.Editor.PhysicalStructure Is Nothing AndAlso Not e.Node.Tag Is Nothing AndAlso Not e.Label Is Nothing AndAlso e.Label.Trim.Length > 0 Then

                    If TypeOf (e.Node.Tag) Is AnimatTools.DataObjects.Physical.BodyPart Then
                        Dim doPart As AnimatTools.DataObjects.Physical.BodyPart = DirectCast(e.Node.Tag, AnimatTools.DataObjects.Physical.BodyPart)
                        doPart.Name = e.Label
                        doPart.IsDirty = True

                        If Me.Editor.PropertiesBar.SelectedPart Is doPart Then
                            Me.Editor.PropertiesBar.RefreshProperties()
                        End If
                    End If
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
                e.CancelEdit = True
            End Try
        End Sub

#Region " Drag Events "

        Private Sub ctrlTreeView_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ctrlTreeView.MouseMove

            Try
                If e.Button = MouseButtons.Left Then
                    Dim tndNode As TreeNode = ctrlTreeView.GetNodeAt(e.X, e.Y)

                    If Not tndNode Is Nothing AndAlso Not tndNode.Tag Is Nothing _
                       AndAlso TypeOf tndNode.Tag Is DataObjects.Physical.BodyPart Then
                        Dim doPart As DataObjects.Physical.BodyPart = DirectCast(tndNode.Tag, DataObjects.Physical.BodyPart)

                        If doPart.CanBeCharted Then
                            Dim dragHelper As New Framework.DataDragHelper
                            dragHelper.m_doData = doPart

                            m_imageDrag.StartDrag(m_dragImageManager.GetImageIndex(doPart.WorkspaceImageName), CInt(m_szDragImageSize.Width / 2), CInt(m_szDragImageSize.Height / 2))
                            Me.ctrlTreeView.DoDragDrop(dragHelper, DragDropEffects.Copy)
                            m_imageDrag.CompleteDrag()
                        End If
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

        Private Sub ctrlTreeView_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ctrlTreeView.DragEnter
            MyBase.OnDragEnter(e)
            e.Effect = DragDropEffects.Copy
        End Sub

#End Region

#End Region

    End Class

End Namespace


