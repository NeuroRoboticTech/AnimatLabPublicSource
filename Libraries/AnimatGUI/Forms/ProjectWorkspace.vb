Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
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
        Protected m_bMutlipleSelectInProgress As Boolean = False

        Protected m_bLoadInProgress As Boolean = False

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

        Public Overridable ReadOnly Property SelectedDataObjects() As Collections.DataObjects
            Get
                Dim arySelected As New Collections.DataObjects(Nothing)

                For Each tvNode As Crownwood.DotNetMagic.Controls.Node In ctrlTreeView.SelectedNodes
                    If Not tvNode.Tag Is Nothing AndAlso Util.IsTypeOf(tvNode.Tag.GetType(), GetType(Framework.DataObject), False) Then
                        Dim doObj As Framework.DataObject = DirectCast(tvNode.Tag, Framework.DataObject)
                        arySelected.Add(doObj)
                    End If
                Next

                Return arySelected
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            MyBase.Initialize(frmParent)

            ctrlTreeView.Nodes.Clear()
            Util.Application.WorkspaceImages.ImageList.ImageSize = New Size(25, 25)
            ctrlTreeView.ImageList = Util.Application.WorkspaceImages.ImageList

            AddHandler Util.Application.ProjectLoaded, AddressOf Me.OnProjectLoaded

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
                tnParent.Nodes.Add(tnNode, Not m_bLoadInProgress)
            Else
                tnNode = Util.ProjectWorkspace.TreeView.Nodes.Add(tnNode, Not m_bLoadInProgress)
            End If

            tnNode.ImageIndex = Util.Application.WorkspaceImages.GetImageIndex(strImage)
            tnNode.SelectedImageIndex = Util.Application.WorkspaceImages.GetImageIndex(strSelImage)

            Return tnNode
        End Function

        Public Sub RefreshProperties()

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
                        ElseIf Util.IsTypeOf(tnNode.Tag.GetType, GetType(Forms.AnimatForm), False) Then
                            Dim doObj As Forms.AnimatForm = DirectCast(tnNode.Tag, Forms.AnimatForm)
                            aryItems(iIndex) = doObj.Properties
                            iIndex = iIndex + 1
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
                    ElseIf Util.IsTypeOf(ctrlTreeView.SelectedNode.Tag.GetType, GetType(Forms.AnimatForm), False) Then
                        Dim doObj As Forms.AnimatForm = DirectCast(ctrlTreeView.SelectedNode.Tag, Forms.AnimatForm)
                        Util.ProjectProperties.PropertyData = doObj.Properties
                    Else
                        Util.ProjectProperties.PropertyData = Nothing
                    End If
                Else
                    Util.ProjectProperties.PropertyData = Nothing
                End If
            End If

        End Sub

        Public Overridable Function SelectedObjectsContains(ByVal doObject As AnimatGUI.Framework.DataObject) As Boolean

            For Each tnNode As Crownwood.DotNetMagic.Controls.Node In Me.TreeView.SelectedNodes
                If Not tnNode.Tag Is Nothing AndAlso Util.IsTypeOf(tnNode.Tag.GetType, GetType(AnimatGUI.Framework.DataObject), False) Then
                    If tnNode.Tag Is doObject Then
                        Return True
                    End If
                End If
            Next

            Return False
        End Function

        Public Overridable Sub ClearSelections()

            Try
                Util.Application.AppIsBusy = True
                For Each tnNode As Crownwood.DotNetMagic.Controls.Node In Me.TreeView.SelectedNodes
                    If Not tnNode.Tag Is Nothing AndAlso Util.IsTypeOf(tnNode.Tag.GetType, GetType(AnimatGUI.Framework.DataObject), False) Then
                        Dim doObj As Framework.DataObject = DirectCast(tnNode.Tag, Framework.DataObject)
                        doObj.DeselectItem()
                    End If
                Next
                Application.DoEvents()

            Catch ex As Exception
                Throw ex
            Finally
                Util.Application.AppIsBusy = False
            End Try
        End Sub

        Protected Overridable Sub SelectMultipleItemsClearCurrent(ByVal arySelectItems As Collections.DataObjects)
            Try
                Util.Application.AppIsBusy = True

                ClearSelections()

                m_bMutlipleSelectInProgress = True
                ctrlTreeView.InstantUpdate = False

                Dim iCount As Integer = arySelectItems.Count - 1
                Dim aryItems(iCount) As PropertyBag
                Dim iIndex As Integer = 0
                For Each doNode As Framework.DataObject In arySelectItems
                    If Util.IsTypeOf(doNode.GetType, GetType(Framework.DataObject), False) Then
                        aryItems(iIndex) = doNode.Properties
                        iIndex = iIndex + 1
                        doNode.SelectItem(True)
                        doNode.AfterSelected()
                    End If
                Next

                If iCount >= 0 Then
                    Util.ProjectProperties.PropertyData = Nothing
                    Util.ProjectProperties.PropertyArray = aryItems
                Else
                    Util.ProjectProperties.PropertyData = Nothing
                    Util.ProjectProperties.PropertyArray = Nothing
                End If

                RaiseEvent WorkspaceSelectionChanged()

            Catch ex As Exception
                Throw ex
            Finally
                Util.Application.AppIsBusy = False
                m_bMutlipleSelectInProgress = False
                ctrlTreeView.InstantUpdate = True
            End Try
        End Sub

        Protected Overridable Sub FindSelectionDifferences(ByVal arySelectItems As Collections.DataObjects, _
                                                           ByRef aryToAdd As Collections.DataObjects, _
                                                           ByRef aryToRemove As Collections.DataObjects)
            'Lets first check this list against what is currently selected. If it matches with just a few differences
            'then lets just do things individually. If it does not then lets redo the whole thing.
            Dim aryCurrentlySelected As Collections.DataObjects = Me.SelectedDataObjects

            For Each doSel As Framework.DataObject In arySelectItems
                If Not aryCurrentlySelected.Contains(doSel) Then
                    aryToAdd.Add(doSel)
                End If
            Next

            For Each doSel As Framework.DataObject In aryCurrentlySelected
                If Not arySelectItems.Contains(doSel) Then
                    aryToRemove.Add(doSel)
                End If
            Next

        End Sub

        Protected Overridable Sub AddRemoveIndividualItems(ByVal aryToAdd As Collections.DataObjects, _
                                                           ByVal aryToRemove As Collections.DataObjects)
            Try
                Util.Application.AppIsBusy = False

                m_bMutlipleSelectInProgress = True
                ctrlTreeView.InstantUpdate = False

                For Each doItem As Framework.DataObject In aryToAdd
                    doItem.SelectItem(True)
                    doItem.AfterSelected()
                Next

                For Each doItem As Framework.DataObject In aryToRemove
                    doItem.DeselectItem()
                    doItem.AfterDeselected()
                Next

                RegenerateObjectPropertiesArray()

            Catch ex As Exception
                Throw ex
            Finally
                Util.Application.AppIsBusy = False
                m_bMutlipleSelectInProgress = False
                ctrlTreeView.InstantUpdate = True
            End Try

        End Sub

        Public Overridable Sub SelectMultipleItems(ByVal arySelectItems As Collections.DataObjects)

            If Math.Abs(ctrlTreeView.SelectedCount - arySelectItems.Count) > 5 Then
                SelectMultipleItemsClearCurrent(arySelectItems)
            Else
                Dim aryToAdd As New Collections.DataObjects(Nothing)
                Dim aryToRemove As New Collections.DataObjects(Nothing)

                FindSelectionDifferences(arySelectItems, aryToAdd, aryToRemove)

                If (aryToAdd.Count + aryToRemove.Count) > 5 Then
                    SelectMultipleItemsClearCurrent(arySelectItems)
                Else
                    AddRemoveIndividualItems(aryToAdd, aryToRemove)
                End If
            End If

        End Sub

        Protected Overridable Sub RegenerateObjectPropertiesArray()

            Dim iCount As Integer = ctrlTreeView.SelectedNodes.Count - 1
            Dim aryItems(iCount) As PropertyBag
            Dim iIndex As Integer = 0
            For Each tnNode As Crownwood.DotNetMagic.Controls.Node In ctrlTreeView.SelectedNodes
                If Not tnNode.Tag Is Nothing Then
                    If Util.IsTypeOf(tnNode.Tag.GetType, GetType(Framework.DataObject), False) Then
                        Dim doObj As Framework.DataObject = DirectCast(tnNode.Tag, Framework.DataObject)
                        aryItems(iIndex) = doObj.Properties
                        iIndex = iIndex + 1
                    ElseIf Util.IsTypeOf(tnNode.Tag.GetType, GetType(Forms.AnimatForm), False) Then
                        Dim doObj As Forms.AnimatForm = DirectCast(tnNode.Tag, Forms.AnimatForm)
                        aryItems(iIndex) = doObj.Properties
                        iIndex = iIndex + 1
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

        End Sub

        Public Sub FindSelectedSubNodes(ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, ByVal aryList As Collections.DataObjects)

            For Each tnChild As Crownwood.DotNetMagic.Controls.Node In tnParent.Nodes
                If tnChild.IsSelected Then
                    If Not tnChild.Tag Is Nothing AndAlso Util.IsTypeOf(tnChild.Tag.GetType(), GetType(Framework.DataObject)) Then
                        Dim doChild As Framework.DataObject = DirectCast(tnChild.Tag, Framework.DataObject)
                        aryList.Add(doChild)
                    End If
                End If

                FindSelectedSubNodes(tnChild, aryList)
            Next

        End Sub

#End Region

#Region " Events "

        Protected Overrides Sub AnimatForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
            Try
                MyBase.AnimatForm_FormClosing(sender, e)
                RemoveHandler Util.Application.ProjectLoaded, AddressOf Me.OnProjectLoaded
            Catch ex As Exception
            End Try

        End Sub

        Private Sub OnProjectLoaded()
            Try
                m_bLoadInProgress = True

                If Not Util.Application.Simulation Is Nothing Then
                    Util.Application.Simulation.CreateWorkspaceTreeView(DirectCast(m_doFormHelper, AnimatGUI.Framework.DataObject), Nothing)
                End If

                ctrlTreeView.Sort()

                ctrlTreeView.ExpandAll()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                m_bLoadInProgress = False
            End Try
        End Sub

        Private Sub ctrlTreeView_BeforeCollapse(tc As Crownwood.DotNetMagic.Controls.TreeControl, e As Crownwood.DotNetMagic.Controls.CancelNodeEventArgs) Handles ctrlTreeView.BeforeCollapse
            Try
                'When we do a collapse and multiple items are selected then it goes through and individually deselects them.
                'This sets up a long loop where it regenerates the multiple selection over and over again. So I am short-circuiting
                ' this by deselecting everything within the collapsed node myself first.
                If ctrlTreeView.SelectedNodes.Count > 1 Then

                    Dim arySelectedNodesInCollapse As New Collections.DataObjects(Nothing)
                    Dim aryToAdd As New Collections.DataObjects(Nothing)
                    FindSelectedSubNodes(e.Node, arySelectedNodesInCollapse)

                    AddRemoveIndividualItems(aryToAdd, arySelectedNodesInCollapse)

                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

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

        Public Event WorkspaceSelectionChanged()

        Protected Sub SelectMultipleFromTree()
            Try
                Util.Application.AppIsBusy = True
                m_bMutlipleSelectInProgress = True
                ctrlTreeView.InstantUpdate = False

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
            Catch ex As Exception
                Throw ex
            Finally
                Util.Application.AppIsBusy = False
                m_bMutlipleSelectInProgress = False
                ctrlTreeView.InstantUpdate = True
            End Try
        End Sub

        Protected Sub SelectSingleFromTree()
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
        End Sub

        Private Sub ctrlTreeView_AfterSelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctrlTreeView.AfterSelectionChanged

            Try
                If m_bMutlipleSelectInProgress Then
                    'If we are doing multiple selection simultainously then let it do this process
                    Return
                End If

                If ctrlTreeView.SelectedNodes.Count > 1 Then
                    SelectMultipleFromTree()
                Else
                    SelectSingleFromTree()
                End If

                RaiseEvent WorkspaceSelectionChanged()

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
                If Not ctrlTreeView.SelectedNode Is Nothing AndAlso Not ctrlTreeView.SelectedNode.Tag Is Nothing Then
                    If Util.IsTypeOf(ctrlTreeView.SelectedNode.Tag.GetType, GetType(Framework.DataObject)) Then
                        Dim doNode As Framework.DataObject = DirectCast(ctrlTreeView.SelectedNode.Tag, Framework.DataObject)
                        doNode.WorkspaceTreeviewDoubleClick(ctrlTreeView.SelectedNode)
                    ElseIf Util.IsTypeOf(ctrlTreeView.SelectedNode.Tag.GetType, GetType(Framework.DataObjectTreeViewRef), False) Then
                        Dim doRef As Framework.DataObjectTreeViewRef = DirectCast(ctrlTreeView.SelectedNode.Tag, Framework.DataObjectTreeViewRef)
                        If Not doRef.m_doObject Is Nothing AndAlso Util.IsTypeOf(doRef.m_doObject.GetType, GetType(Framework.DataObject), False) Then
                            Dim doNode As Framework.DataObject = DirectCast(doRef.m_doObject, Framework.DataObject)
                            doNode.WorkspaceTreeviewDoubleClick(ctrlTreeView.SelectedNode)
                        End If
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_BeforeLabelEdit(ByVal tc As Crownwood.DotNetMagic.Controls.TreeControl, ByVal e As Crownwood.DotNetMagic.Controls.LabelEditEventArgs) Handles ctrlTreeView.BeforeLabelEdit
            Dim bCancel As Boolean = True

            Try
                If Not e.Node.Tag Is Nothing AndAlso Util.IsTypeOf(e.Node.Tag.GetType, GetType(Framework.DataObject)) Then
                    Dim doObj As Framework.DataObject = DirectCast(e.Node.Tag, Framework.DataObject)

                    If doObj.AllowTreeviewNameEdit Then
                        bCancel = False
                    End If
                ElseIf Not e.Node.Tag Is Nothing AndAlso Util.IsTypeOf(e.Node.Tag.GetType, GetType(Forms.AnimatForm)) Then
                    Dim doObj As Forms.AnimatForm = DirectCast(e.Node.Tag, Forms.AnimatForm)

                    If doObj.AllowTreeviewNameEdit Then
                        bCancel = False
                    End If

                End If

                e.Cancel = bCancel
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

