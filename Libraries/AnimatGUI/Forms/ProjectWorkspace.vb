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

Namespace Forms

    Public Class ProjectWorkspace
        Inherits AnimatForm

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            InitializeComponent()
        End Sub

        Private Sub InitializeComponent()
            Me.ctrlTreeView = New Crownwood.DotNetMagic.Controls.TreeControl
            Me.SuspendLayout()
            '
            'ctrlTreeView
            '
            Me.ctrlTreeView.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ctrlTreeView.DoubleClickExpand = Crownwood.DotNetMagic.Controls.ClickExpandAction.None
            Me.ctrlTreeView.FocusNode = Nothing
            Me.ctrlTreeView.HotBackColor = System.Drawing.Color.Empty
            Me.ctrlTreeView.HotForeColor = System.Drawing.Color.Empty
            Me.ctrlTreeView.Location = New System.Drawing.Point(0, 0)
            Me.ctrlTreeView.Name = "ctrlTreeView"
            Me.ctrlTreeView.SelectedNode = Nothing
            Me.ctrlTreeView.SelectedNoFocusBackColor = System.Drawing.SystemColors.Control
            Me.ctrlTreeView.Size = New System.Drawing.Size(284, 262)
            Me.ctrlTreeView.TabIndex = 0
            Me.ctrlTreeView.Text = "ctrlTreeView"
            '
            'ProjectWorkspace
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(284, 262)
            Me.Controls.Add(Me.ctrlTreeView)
            Me.Name = "ProjectWorkspace"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents ctrlTreeView As Crownwood.DotNetMagic.Controls.TreeControl

#End Region

#Region " Attributes "

        Protected m_PropertyData As AnimatGuiCtrls.Controls.PropertyBag
        Protected m_PropertyArray() As AnimatGuiCtrls.Controls.PropertyBag

#End Region


#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatGUI.ProjectWorkspace.gif"
            End Get
        End Property

        Public ReadOnly Property TreeView() As Crownwood.DotNetMagic.Controls.TreeControl
            Get
                Return ctrlTreeView
            End Get
        End Property

        Public Overridable ReadOnly Property SelectedItem() As Object
            Get
                If Not ctrlTreeView.SelectedNode Is Nothing Then
                    Return ctrlTreeView.SelectedNode.Tag
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Overridable ReadOnly Property SelectedDataObject() As Framework.DataObject
            Get
                If Not ctrlTreeView.SelectedNode Is Nothing AndAlso Not ctrlTreeView.SelectedNode.Tag Is Nothing AndAlso Util.IsTypeOf(ctrlTreeView.SelectedNode.Tag.GetType(), GetType(Framework.DataObject), False) Then
                    Dim doObj As Framework.DataObject = DirectCast(ctrlTreeView.SelectedNode.Tag, Framework.DataObject)
                    Return doObj
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Overridable ReadOnly Property SelectedAnimatform() As Forms.AnimatForm
            Get
                If Not ctrlTreeView.SelectedNode Is Nothing AndAlso Not ctrlTreeView.SelectedNode.Tag Is Nothing AndAlso Util.IsTypeOf(ctrlTreeView.SelectedNode.Tag.GetType(), GetType(Forms.AnimatForm), False) Then
                    Dim doObj As Forms.AnimatForm = DirectCast(ctrlTreeView.SelectedNode.Tag, Forms.AnimatForm)
                    Return doObj
                Else
                    Return Nothing
                End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            MyBase.Initialize(frmParent)

            ctrlTreeView.Nodes.Clear()
            Util.Application.WorkspaceImages.ImageList.ImageSize = New Size(25, 25)
            ctrlTreeView.ImageList = Util.Application.WorkspaceImages.ImageList

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            If Not Util.Application.Simulation Is Nothing Then
                Util.Application.Simulation.CreateWorkspaceTreeView(DirectCast(m_doFormHelper, AnimatGUI.Framework.DataObject), Nothing)
            End If

            ctrlTreeView.ExpandAll()
        End Sub

        Public Overloads Sub CreateWorkspaceTreeView()
            If Not Util.Application.Simulation Is Nothing Then
                Util.Application.Simulation.CreateWorkspaceTreeView(DirectCast(m_doFormHelper, AnimatGUI.Framework.DataObject), Nothing)
            End If

            ctrlTreeView.ExpandAll()
        End Sub

        Public Overridable Function AddTreeNode(ByRef tnParent As Crownwood.DotNetMagic.Controls.Node, ByVal strName As String, ByVal strImage As String, Optional ByVal strSelImage As String = "") As Crownwood.DotNetMagic.Controls.Node
            Util.Application.WorkspaceImages.AddImage(strImage)

            If strSelImage = "" Then
                strSelImage = strImage
            Else
                Util.Application.WorkspaceImages.AddImage(strSelImage)
            End If

            Dim tnNode As New Crownwood.DotNetMagic.Controls.Node(strName)
            If Not tnParent Is Nothing Then
                tnParent.Nodes.Add(tnNode)
            Else
                tnNode = Util.ProjectWorkspace.TreeView.Nodes.Add(tnNode)
            End If

            tnNode.ImageIndex = Util.Application.WorkspaceImages.GetImageIndex(strImage)
            tnNode.SelectedImageIndex = Util.Application.WorkspaceImages.GetImageIndex(strSelImage)

            Return tnNode
        End Function

#End Region

#Region " Events "

        Private Sub ctrlTreeView_BeforeDeselect(ByVal tc As Crownwood.DotNetMagic.Controls.TreeControl, ByVal e As Crownwood.DotNetMagic.Controls.NodeEventArgs) Handles ctrlTreeView.BeforeDeselect
            Try
                If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing Then
                    If Util.IsTypeOf(e.Node.Tag.GetType, GetType(Framework.DataObject), False) Then
                        Dim doObj As Framework.DataObject = DirectCast(e.Node.Tag, Framework.DataObject)
                        doObj.BeforeDeselected()
                    ElseIf Util.IsTypeOf(e.Node.Tag.GetType, GetType(Forms.AnimatForm), False) Then
                        Dim doObj As Forms.AnimatForm = DirectCast(e.Node.Tag, Forms.AnimatForm)
                        doObj.BeforeDeselected()
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_AfterDeselect(ByVal tc As Crownwood.DotNetMagic.Controls.TreeControl, ByVal e As Crownwood.DotNetMagic.Controls.NodeEventArgs) Handles ctrlTreeView.AfterDeselect
            Try
                If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing Then
                    If Util.IsTypeOf(e.Node.Tag.GetType, GetType(Framework.DataObject), False) Then
                        Dim doObj As Framework.DataObject = DirectCast(e.Node.Tag, Framework.DataObject)
                        doObj.DeselectItem()
                        doObj.AfterDeselected()
                    ElseIf Util.IsTypeOf(e.Node.Tag.GetType, GetType(Forms.AnimatForm), False) Then
                        Dim doObj As Forms.AnimatForm = DirectCast(e.Node.Tag, Forms.AnimatForm)
                        doObj.DeselectItem()
                        doObj.AfterDeselected()
                    End If
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ctrlTreeView_BeforeSelect(ByVal tc As Crownwood.DotNetMagic.Controls.TreeControl, ByVal e As Crownwood.DotNetMagic.Controls.CancelNodeEventArgs) Handles ctrlTreeView.BeforeSelect
            Try
                If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing Then
                    If Util.IsTypeOf(e.Node.Tag.GetType, GetType(Framework.DataObject), False) Then
                        Dim doObj As Framework.DataObject = DirectCast(e.Node.Tag, Framework.DataObject)
                        e.Cancel = doObj.BeforeSelected()
                    ElseIf Util.IsTypeOf(e.Node.Tag.GetType, GetType(Forms.AnimatForm), False) Then
                        Dim doObj As Forms.AnimatForm = DirectCast(e.Node.Tag, Forms.AnimatForm)
                        e.Cancel = doObj.BeforeSelected()
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ctrlTreeView_AfterSelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctrlTreeView.AfterSelectionChanged

            Try
                If ctrlTreeView.SelectedNodes.Count > 1 Then
                    Dim iCount As Integer = ctrlTreeView.SelectedNodes.Count - 1
                    Dim aryItems(iCount) As PropertyBag
                    Dim iIndex As Integer = 0
                    For Each tnNode As Crownwood.DotNetMagic.Controls.Node In ctrlTreeView.SelectedNodes
                        If Not tnNode.Tag Is Nothing Then
                            If Util.IsTypeOf(tnNode.Tag.GetType, GetType(Framework.DataObject), False) Then
                                Dim doObj As Framework.DataObject = DirectCast(tnNode.Tag, Framework.DataObject)
                                aryItems(iIndex) = doObj.Properties
                                iIndex = iIndex + 1
                                doObj.SelectItem(True)
                                doObj.AfterSelected()
                            ElseIf Util.IsTypeOf(tnNode.Tag.GetType, GetType(Forms.AnimatForm), False) Then
                                Dim doObj As Forms.AnimatForm = DirectCast(tnNode.Tag, Forms.AnimatForm)
                                aryItems(iIndex) = doObj.Properties
                                iIndex = iIndex + 1
                                doObj.SelectItem(True)
                                doObj.AfterSelected()
                            End If
                        Else
                            iCount = iCount - 1
                            ReDim Preserve aryItems(iCount)
                        End If
                    Next

                    If iCount >= 0 Then
                        Util.ProjectProperties.PropertyData = Nothing
                        Util.ProjectProperties.PropertyArray = aryItems
                    Else
                        Util.ProjectProperties.PropertyData = Nothing
                        Util.ProjectProperties.PropertyArray = Nothing
                    End If
                Else
                    Util.ProjectProperties.PropertyArray = Nothing

                    If Not ctrlTreeView.SelectedNode Is Nothing AndAlso Not ctrlTreeView.SelectedNode.Tag Is Nothing Then
                        If Util.IsTypeOf(ctrlTreeView.SelectedNode.Tag.GetType, GetType(Framework.DataObject), False) Then
                            Dim doObj As Framework.DataObject = DirectCast(ctrlTreeView.SelectedNode.Tag, Framework.DataObject)
                            Util.ProjectProperties.PropertyData = doObj.Properties
                            doObj.SelectItem(False)
                            doObj.AfterSelected()
                        ElseIf Util.IsTypeOf(ctrlTreeView.SelectedNode.Tag.GetType, GetType(Forms.AnimatForm), False) Then
                            Dim doObj As Forms.AnimatForm = DirectCast(ctrlTreeView.SelectedNode.Tag, Forms.AnimatForm)
                            Util.ProjectProperties.PropertyData = doObj.Properties
                            doObj.SelectItem(False)
                            doObj.AfterSelected()
                        Else
                            Util.ProjectProperties.PropertyData = Nothing
                        End If
                    Else
                        Util.ProjectProperties.PropertyData = Nothing
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ctrlTreeView_ShowContextMenuNode(ByVal tc As Crownwood.DotNetMagic.Controls.TreeControl, ByVal e As Crownwood.DotNetMagic.Controls.CancelNodeEventArgs) Handles ctrlTreeView.ShowContextMenuNode

            Try
                'We will set the contextmenunode property within our call to WorkspaceTreeViewPopupMenu
                e.Cancel = False

                'Clear the context menu node before we set it again.
                ctrlTreeView.ContextMenuNode = Nothing

                Dim ptPoint As Point = tc.MousePosition

                If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing Then
                    If Util.IsTypeOf(e.Node.Tag.GetType, GetType(Framework.DataObject), False) Then
                        Dim doObj As Framework.DataObject = DirectCast(e.Node.Tag, Framework.DataObject)
                        doObj.WorkspaceTreeviewPopupMenu(e.Node, ptPoint)
                    ElseIf Util.IsTypeOf(e.Node.Tag.GetType, GetType(Forms.AnimatForm), False) Then
                        Dim doObj As Forms.AnimatForm = DirectCast(e.Node.Tag, Forms.AnimatForm)
                        doObj.WorkspaceTreeviewPopupMenu(e.Node, ptPoint)
                    End If
                Else
                    If Not Util.Application Is Nothing AndAlso Not Util.Simulation Is Nothing Then
                        Util.Simulation.WorkspaceTreeviewPopupMenu(e.Node, ptPoint)
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ctrlTreeView_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctrlTreeView.DoubleClick

            Try
                If Not Util.Application.Simulation Is Nothing AndAlso Not ctrlTreeView.SelectedNode Is Nothing Then
                    Util.Application.Simulation.WorkspaceTreeviewDoubleClick(ctrlTreeView.SelectedNode)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_BeforeLabelEdit(ByVal tc As Crownwood.DotNetMagic.Controls.TreeControl, ByVal e As Crownwood.DotNetMagic.Controls.LabelEditEventArgs) Handles ctrlTreeView.BeforeLabelEdit

            Try
                If Not e.Node.Tag Is Nothing AndAlso _
                   (TypeOf e.Node.Tag Is DataObjects.ToolHolder OrElse _
                   TypeOf e.Node.Tag Is DataObjects.ExternalStimuli.Stimulus OrElse _
                   (TypeOf e.Node.Tag Is DataObjects.Physical.PhysicalStructure)) Then
                    e.Cancel = False
                Else
                    e.Cancel = True
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_AfterLabelEdit(ByVal tc As Crownwood.DotNetMagic.Controls.TreeControl, ByVal e As Crownwood.DotNetMagic.Controls.LabelEditEventArgs) Handles ctrlTreeView.AfterLabelEdit

            Try
                If Not e.Node.Tag Is Nothing Then
                    If Not e.Label Is Nothing AndAlso e.Label.Trim.Length > 0 Then
                        If Util.IsTypeOf(e.Node.Tag.GetType, GetType(Framework.DataObject), False) Then
                            Dim doObj As Framework.DataObject = DirectCast(e.Node.Tag, Framework.DataObject)
                            doObj.Name = e.Label.Trim
                        ElseIf Util.IsTypeOf(e.Node.Tag.GetType, GetType(Forms.AnimatForm), False) Then
                            Dim doObj As Forms.AnimatForm = DirectCast(e.Node.Tag, Forms.AnimatForm)
                            doObj.Title = e.Label.Trim
                        Else
                            e.Cancel = True
                        End If
                    Else
                        e.Cancel = True
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

