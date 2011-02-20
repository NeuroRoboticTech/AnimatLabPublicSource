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

Namespace Forms

    Public Class AnimatTreeView
        Inherits TreeView

        Protected Overrides Sub DefWndProc(ByRef m As Message)
            If m.Msg = 515 Then    'WM_LBUTTONDBLCLK - &H203
                'Do Nothing
            Else
                MyBase.DefWndProc(m)
            End If
        End Sub

    End Class

    Public Class ProjectWorkspace
        Inherits AnimatForm 'System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            ''Add any initialization after the InitializeComponent() call
            'InitializeTreeView()
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
        Friend WithEvents ctrlTreeView As AnimatTools.Forms.AnimatTreeView
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.ctrlTreeView = New AnimatTools.Forms.AnimatTreeView
            Me.SuspendLayout()
            '
            'ctrlTreeView
            '
            Me.ctrlTreeView.FullRowSelect = True
            Me.ctrlTreeView.HideSelection = False
            Me.ctrlTreeView.ImageIndex = -1
            Me.ctrlTreeView.LabelEdit = True
            Me.ctrlTreeView.Location = New System.Drawing.Point(0, 0)
            Me.ctrlTreeView.Name = "ctrlTreeView"
            Me.ctrlTreeView.SelectedImageIndex = -1
            Me.ctrlTreeView.Size = New System.Drawing.Size(272, 168)
            Me.ctrlTreeView.Sorted = True
            Me.ctrlTreeView.TabIndex = 0
            Me.ctrlTreeView.Dock = DockStyle.Fill
            '
            'ProjectWorkspace
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(272, 374)
            Me.Controls.Add(Me.ctrlTreeView)
            Me.Name = "ProjectWorkspace"
            Me.Text = "Workspace"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_bInSelection As Boolean = False
        Protected m_selAnimatForm As Forms.AnimatForm
        Protected m_selDataObject As Framework.DataObject
        Protected m_arySelectedDataObjects As New AnimatTools.Collections.DataObjects(Me.FormHelper)
        Protected m_selObject As Object

        Protected m_PropertyData As AnimatGUICtrls.Controls.PropertyBag
        Protected m_PropertyArray() As AnimatGuiCtrls.Controls.PropertyBag

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatTools.ProjectWorkspace.gif"
            End Get
        End Property

        Public ReadOnly Property TreeView() As AnimatTools.Forms.AnimatTreeView
            Get
                Return ctrlTreeView
            End Get
        End Property

        Public Overridable ReadOnly Property SelectedItem() As Object
            Get
                Return m_selObject
            End Get
        End Property

        Public Overridable ReadOnly Property SelectedDataObject() As Framework.DataObject
            Get
                Return m_selDataObject
            End Get
        End Property

        Public Overridable ReadOnly Property SelectedDataObjects() As AnimatTools.Collections.DataObjects
            Get
                Return m_arySelectedDataObjects
            End Get
        End Property

        Public Overridable ReadOnly Property SelectedAnimatform() As Forms.AnimatForm
            Get
                Return m_selAnimatForm
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

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()
            Me.ctrlTreeView.Height = oXml.GetChildInt("TreeViewHeight", CInt(Me.Height * 0.6))
            oXml.OutOfElem()

            If Not Util.Application.Simulation Is Nothing Then
                Util.Application.Simulation.CreateWorkspaceTreeView(DirectCast(m_doFormHelper, AnimatTools.Framework.DataObject), Nothing)
            End If

            ctrlTreeView.ExpandAll()
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()
            oXml.AddChildElement("TreeViewHeight", Me.ctrlTreeView.Height)
            oXml.OutOfElem()
        End Sub

        Public Overloads Sub CreateWorkspaceTreeView()
            If Not Util.Application.Simulation Is Nothing Then
                Util.Application.Simulation.CreateWorkspaceTreeView(DirectCast(m_doFormHelper, AnimatTools.Framework.DataObject), Nothing)
            End If

            ctrlTreeView.ExpandAll()
        End Sub

        Public Overridable Sub SelectObject(ByVal oData As Object, Optional ByVal bCtrlDown As Boolean = False, Optional ByVal bThrowError As Boolean = True)

            If Util.IsTypeOf(oData.GetType, GetType(Framework.DataObject), False) Then
                Dim doObj As Framework.DataObject = DirectCast(oData, Framework.DataObject)
                SelectDataObject(doObj, bCtrlDown, bThrowError)
            ElseIf Util.IsTypeOf(oData.GetType, GetType(Forms.AnimatForm), False) Then
                Dim frmObj As Forms.AnimatForm = DirectCast(oData, Forms.AnimatForm)
                SelectAnimatform(frmObj, bCtrlDown, bThrowError)
            Else
                Dim doObj As Framework.DataObject
                SelectDataObject(doObj, bCtrlDown, bThrowError)
            End If

        End Sub

        Protected Overridable Sub SelectDataObject(ByVal doData As AnimatTools.Framework.DataObject, ByVal bCtrlDown As Boolean, Optional ByVal bThrowError As Boolean = True)

            Try
                If m_bInSelection Then Return
                m_bInSelection = True

                'If the user clicks blank space with the ctrl key down then do nothing
                If doData Is Nothing AndAlso bCtrlDown Then
                    m_bInSelection = False
                    Return
                End If

                If bCtrlDown And DataObjectIsSelected(doData) Then
                    DeselectDataObject(doData)
                    m_bInSelection = False
                    Return
                End If

                'Deselect the old part.
                If Not m_selDataObject Is Nothing AndAlso Not bCtrlDown Then
                    ClearSelectedDataObjects()
                End If

                If Not doData Is Nothing Then doData.BeforeSelected()
                m_selDataObject = doData

                If Not m_selDataObject Is Nothing Then

                    m_selDataObject.Selected = True

                    If Not m_selDataObject.WorkspaceNode Is Nothing Then
                        ctrlTreeView.SelectedNode = m_selDataObject.WorkspaceNode
                        ctrlTreeView.mu()
                    End If

                    If Not m_arySelectedDataObjects.Contains(m_selDataObject) Then
                        m_arySelectedDataObjects.Add(m_selDataObject)
                    End If
                ElseIf Not bCtrlDown Then
                    'Me.Editor.HierarchyBar.TreeView.SelectedNode = Me.Editor.PhysicalStructure.BodyPlanStructureNode
                End If

                If m_arySelectedParts.Count > 1 Then
                    Dim aryItems(m_arySelectedParts.Count - 1) As PropertyBag
                    Dim iIndex As Integer = 0
                    For Each doSelPart As AnimatTools.DataObjects.Physical.BodyPart In m_arySelectedParts
                        If Not doSelPart Is Nothing Then
                            aryItems(iIndex) = doSelPart.Properties
                            iIndex = iIndex + 1
                        End If
                    Next

                    Me.PropertyData = Nothing
                    Me.PropertyArray = aryItems
                Else
                    Me.PropertyArray = Nothing
                    If Not m_doSelectedPart Is Nothing Then
                        Me.PropertyData = m_doSelectedPart.Properties
                    Else
                        If Not Me.Editor Is Nothing AndAlso Not Me.Editor.PhysicalStructure Is Nothing Then
                            Me.PropertyData = Me.Editor.PhysicalStructure.Properties
                        Else
                            Me.PropertyData = Nothing
                        End If
                    End If
                End If

                If Not Me.Editor.ReceptiveFieldsBar Is Nothing Then
                    Me.Editor.ReceptiveFieldsBar.SelectPart(m_doSelectedPart)
                End If

                If Not Me.Editor.HierarchyBar.TreeView.SelectedNode Is Nothing Then
                    Me.Editor.HierarchyBar.TreeView.SelectedNode.EnsureVisible()
                End If
                Me.Editor.BodyView.Invalidate()

                If Not m_doSelectedPart Is Nothing Then m_doSelectedPart.BeforeSelected()
                m_bInSelection = False

            Catch ex As System.Exception
                m_bInSelection = False
                Throw ex
            End Try

            If doData.WorkspaceNode Is Nothing Then
                If bThrowError Then
                    Throw New System.Exception("Unable to find a treenode that has a dataobject '" & doData.ClassName & "' tagged on it.")
                Else
                    Return
                End If
            End If

            ctrlTreeView.SelectedNode = doData.WorkspaceNode
            m_selDataObject = doData
            Util.ProjectProperties.PropertyData = doData.Properties
        End Sub

        Protected Overridable Sub SelectAnimatform(ByVal frmData As AnimatTools.Forms.AnimatForm, ByVal bCtrlDown As Boolean, Optional ByVal bThrowError As Boolean = True)

            If frmData.WorkspaceNode Is Nothing Then
                If bThrowError Then
                    Throw New System.Exception("Unable to find a treenode that has a dataobject '" & frmData.ClassName & "' tagged on it.")
                Else
                    Return
                End If
            End If

            ctrlTreeView.SelectedNode = frmData.WorkspaceNode
            m_selAnimatForm = frmData
            Util.ProjectProperties.PropertyData = frmData.Properties
        End Sub

        Protected Overridable Sub ClearSelectedDataObjects()

            m_selDataObject.Selected = False

            For Each doPart As AnimatTools.DataObjects.Physical.BodyPart In m_arySelectedParts
                doPart.Selected = False
            Next

            m_arySelectedDataObjects.Clear()
        End Sub

        Public Overridable Function PartIsSelected(ByVal doPart As AnimatTools.Framework.DataObject) As Boolean

            If m_arySelectedDataObjects.Contains(doPart) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overridable Function FindTreeNodeForDataObject(ByVal doData As AnimatTools.Framework.DataObject) As TreeNode

            Dim tnFound As TreeNode
            For Each tnNode As TreeNode In ctrlTreeView.Nodes
                tnFound = FindTreeNodeForDataObject(tnNode, doData)
                If Not tnFound Is Nothing Then
                    Return tnFound
                End If
            Next

        End Function

        Protected Overridable Function FindTreeNodeForDataObject(ByVal tnParent As TreeNode, ByVal doData As AnimatTools.Framework.DataObject) As TreeNode
            If Not tnParent.Tag Is Nothing AndAlso tnParent.Tag Is doData Then
                Return tnParent
            End If

            Dim tnFound As TreeNode
            For Each tnNode As TreeNode In tnParent.Nodes
                tnFound = FindTreeNodeForDataObject(tnNode, doData)
                If Not tnFound Is Nothing Then
                    Return tnFound
                End If
            Next

            Return Nothing
        End Function

        Public Overridable Function AddTreeNode(ByRef tnParent As TreeNode, ByVal strName As String, ByVal strImage As String, Optional ByVal strSelImage As String = "") As TreeNode
            Util.Application.WorkspaceImages.AddImage(strImage)

            If strSelImage = "" Then
                strSelImage = strImage
            Else
                Util.Application.WorkspaceImages.AddImage(strSelImage)
            End If

            Dim tnNode As TreeNode
            If Not tnParent Is Nothing Then
                tnNode = tnParent.Nodes.Add(strName)
            Else
                tnNode = Util.ProjectWorkspace.TreeView.Nodes.Add(strName)
            End If

            tnNode.ImageIndex = Util.Application.WorkspaceImages.GetImageIndex(strImage)
            tnNode.SelectedImageIndex = Util.Application.WorkspaceImages.GetImageIndex(strSelImage)

            Return tnNode
        End Function

#End Region

#Region " Events "

        Private Sub ctrlTreeView_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles ctrlTreeView.AfterSelect

            Try

                If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing Then
                    If TypeOf e.Node.Tag Is Framework.DataObject Then
                        Dim doObject As Framework.DataObject = DirectCast(e.Node.Tag, Framework.DataObject)
                        SelectedItem = doObject
                    ElseIf TypeOf e.Node.Tag Is Forms.AnimatForm Then
                        Dim frmObject As Forms.AnimatForm = DirectCast(e.Node.Tag, Forms.AnimatForm)
                        SelectedItem = frmObject
                    Else
                        SelectedItem = Nothing
                    End If
                Else
                    SelectedItem = Nothing
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

                    If Not Util.Application.Simulation Is Nothing Then
                        If Util.Application.Simulation.WorkspaceTreeviewPopupMenu(tnSelected, ptPoint) Then
                            Return
                        End If
                    End If

                End If
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlTreeView.DoubleClick

            Try
                If Not Util.Application.Simulation Is Nothing AndAlso Not ctrlTreeView.SelectedNode Is Nothing Then
                    Util.Application.Simulation.WorkspaceTreeviewDoubleClick(ctrlTreeView.SelectedNode)
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_BeforeLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles ctrlTreeView.BeforeLabelEdit
            If Not e.Node.Tag Is Nothing AndAlso _
               (TypeOf e.Node.Tag Is DataObjects.ToolHolder OrElse _
               TypeOf e.Node.Tag Is DataObjects.ExternalStimuli.Stimulus OrElse _
               (TypeOf e.Node.Tag Is DataObjects.Physical.PhysicalStructure)) Then
                e.CancelEdit = False
            Else
                e.CancelEdit = True
            End If
        End Sub

        Private Sub ctrlTreeView_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles ctrlTreeView.AfterLabelEdit
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
                            e.CancelEdit = True
                        End If
                    Else
                        e.CancelEdit = True
                    End If
                End If
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace


