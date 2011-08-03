Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects
Imports System.Drawing.Imaging

Namespace Forms.Behavior

    Public MustInherit Class Diagram
        Inherits AnimatForm

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call
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
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            components = New System.ComponentModel.Container
            Me.Title = "Diagram"
        End Sub

#End Region

#Region " Attributes "

        Protected m_beEditor As AnimatGUI.Forms.Behavior.Editor
        Protected m_bnParentDiagram As AnimatGUI.Forms.Behavior.Diagram
        Protected m_OutlookBar As OutlookBar
        Protected m_OutlookContent As Content

        Protected m_aryDiagrams As New Collections.SortedDiagrams(Me.FormHelper)
        Protected m_aryDeletedDiagrams As New Collections.SortedDiagrams(Me.FormHelper)

        Protected m_tnDiagramTreeNode As System.Windows.Forms.TreeNode
        Protected m_tnNodesTreeNode As System.Windows.Forms.TreeNode
        Protected m_tnLinksTreeNode As System.Windows.Forms.TreeNode

        Protected m_aryNodes As New AnimatGUI.Collections.SortedNodeList(Me.FormHelper)
        Protected m_aryLinks As New AnimatGUI.Collections.SortedLinkList(Me.FormHelper)

        Protected m_bnSubSystem As DataObjects.Behavior.Nodes.Subsystem
        Protected m_Image As Image

        Protected m_iDiagramIndex As Integer = 1

        'This is used when we are doing a copy/paste operation. We need to save the objects
        'with a different id than the original version. So we generate this temp id and use it
        'when saving.
        Protected m_strTempSelectedID As String = ""

        'This is set to true if the diagram is in the process of deleting itself. I found that if I make certain calls
        'While the entire diagram was being deleted then it caused problems for the addflow control. So I added this
        'flag to let me know when a diagram was being deleted so I did not make those calls.
        Protected m_bDeletingDiagram As Boolean = False

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable ReadOnly Property SelectedID() As String
            Get
                If m_strTempSelectedID.Trim.Length = 0 Then
                    Return m_strID
                Else
                    Return m_strTempSelectedID
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property TempSelectedID() As String
            Get
                Return m_strTempSelectedID
            End Get
        End Property

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatGUI.SubsystemNode.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property TabImageName() As String
            Get
                Return "AnimatGUI.SubsystemNode.gif"
            End Get
        End Property

        'Public Overrides ReadOnly Property DockingHideTabsMode() As Crownwood.Magic.Controls.TabControl.HideTabsModes
        '    Get
        '        'Return Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways
        '    End Get
        'End Property

        Public Overridable Property Editor() As Forms.Behavior.Editor
            Get
                Return m_beEditor
            End Get
            Set(ByVal Value As Forms.Behavior.Editor)
                m_beEditor = Value
            End Set
        End Property

        Public Overridable Property ParentDiagram() As Behavior.Diagram
            Get
                Return m_bnParentDiagram
            End Get
            Set(ByVal Value As Behavior.Diagram)
                m_bnParentDiagram = Value
            End Set
        End Property

        Public Overridable ReadOnly Property Diagrams() As Collections.SortedDiagrams
            Get
                Return m_aryDiagrams
            End Get
        End Property

        Public Overridable Property DiagramTreeNode() As System.Windows.Forms.TreeNode
            Get
                Return m_tnDiagramTreeNode
            End Get
            Set(ByVal Value As System.Windows.Forms.TreeNode)
                m_tnDiagramTreeNode = Value
            End Set
        End Property

        Public Overridable Property NodesTreeNode() As System.Windows.Forms.TreeNode
            Get
                Return m_tnNodesTreeNode
            End Get
            Set(ByVal Value As System.Windows.Forms.TreeNode)
                m_tnNodesTreeNode = Value
            End Set
        End Property

        Public Overridable Property LinksTreeNode() As System.Windows.Forms.TreeNode
            Get
                Return m_tnLinksTreeNode
            End Get
            Set(ByVal Value As System.Windows.Forms.TreeNode)
                m_tnLinksTreeNode = Value
            End Set
        End Property

        Public Overridable Property Subsystem() As DataObjects.Behavior.Nodes.Subsystem
            Get
                Return m_bnSubSystem
            End Get
            Set(ByVal Value As DataObjects.Behavior.Nodes.Subsystem)
                m_bnSubSystem = Value
            End Set
        End Property

        Public MustOverride Property DiagramName() As String

        Public Overridable ReadOnly Property Nodes() As AnimatGUI.Collections.AnimatSortedList
            Get
                Return m_aryNodes
            End Get
        End Property

        Public Overridable ReadOnly Property Links() As AnimatGUI.Collections.AnimatSortedList
            Get
                Return m_aryLinks
            End Get
        End Property

        Public Overridable Property DiagramIndex() As Integer
            Get
                Return m_iDiagramIndex
            End Get
            Set(ByVal Value As Integer)
                m_iDiagramIndex = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property DeletingDiagram() As Boolean
            Get
                Return m_bDeletingDiagram
            End Get
        End Property

#End Region

#Region " Methods "

        Public MustOverride Function GetChartItemAt(ByVal ptPosition As Point, Optional ByVal bThrowError As Boolean = True) As DataObjects.Behavior.Data

        Public MustOverride Sub AddNode(ByRef oNode As DataObjects.Behavior.Node)
        Public MustOverride Sub BeginEditNode(ByRef bnNode As DataObjects.Behavior.Node)
        Public MustOverride Sub EndEditNode(ByRef bnNode As DataObjects.Behavior.Node, ByVal bCancel As Boolean)
        Public MustOverride Sub RemoveNode(ByRef bnNode As DataObjects.Behavior.Node)

        Public MustOverride Sub AddLink(ByRef bnOrigin As DataObjects.Behavior.Node, ByRef bnDestination As DataObjects.Behavior.Node, ByRef blLink As DataObjects.Behavior.Link)
        Public MustOverride Sub RemoveLink(ByRef blLink As DataObjects.Behavior.Link)

        Public MustOverride Sub AddImage(ByRef diImage As DataObjects.Behavior.DiagramImage)
        Public MustOverride Sub RemoveImage(ByRef diImage As DataObjects.Behavior.DiagramImage)
        Public MustOverride Function ImageUseCount(ByVal diImage As DataObjects.Behavior.DiagramImage) As Integer
        Public MustOverride Function FindDiagramImageIndex(ByRef diImage As System.Drawing.Image, Optional ByVal bThrowError As Boolean = True) As Integer

        Public MustOverride Sub UpdateChart(ByRef bdData As DataObjects.Behavior.Data)
        Public MustOverride Sub UpdateData(ByRef bdData As DataObjects.Behavior.Data, Optional ByVal bSimple As Boolean = True, Optional ByVal bThrowError As Boolean = True)

        Public MustOverride Sub BeginGraphicsUpdate()
        Public MustOverride Sub EndGraphicsUpdate()
        Public MustOverride Sub RefreshDiagram()

        Public Overridable Sub TabDeselected()
        End Sub

        Public Overridable Sub TabSelected()
        End Sub

        Public MustOverride Sub SelectDataItem(ByVal bdItem As DataObjects.Behavior.Data, Optional ByVal bOnlyItemSelected As Boolean = True)
        Public MustOverride Function IsItemSelected(ByVal bdItem As DataObjects.Behavior.Data) As Boolean
        Public MustOverride Function FindItem(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.Behavior.Data
        Public MustOverride Function FindLink(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.Behavior.Link
        Public MustOverride Function FindNode(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.Behavior.Node

        Public Overridable Function FindNodeInOrganism(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.Behavior.Node

            If Not Me.Editor Is Nothing AndAlso Not Me.Editor.Organism Is Nothing Then

                Dim doNode As Framework.DataObject = Me.Editor.Organism.FindObjectByID(strID)
                If Not doNode Is Nothing Then
                    Return DirectCast(doNode, DataObjects.Behavior.Node)
                ElseIf bThrowError Then
                    Throw New System.Exception("Node not found in organism. ID: " & strID)
                End If
            End If
        End Function

        Public Overridable Function FindLinkInOrganism(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.Behavior.Link

            If Not Me.Editor Is Nothing AndAlso Not Me.Editor.Organism Is Nothing Then
                Dim doNode As Framework.DataObject = Me.Editor.Organism.FindObjectByID(strID)
                If Not doNode Is Nothing Then
                    Return DirectCast(doNode, DataObjects.Behavior.Link)
                ElseIf bThrowError Then
                    Throw New System.Exception("Link not found in organism. ID: " & strID)
                End If
            End If
        End Function

        'Shape Methods
        Public MustOverride Sub OnAlignTop(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnAlignVerticalCenter(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnAlignBottom(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnAlignLeft(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnAlignHorizontalCenter(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnAlignRight(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnDistributeVertical(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnDistributeHorizontal(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnSizeBoth(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnSizeWidth(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnSizeHeight(ByVal sender As Object, ByVal e As System.EventArgs)

        'Printing
        Public MustOverride Sub GenerateMetafiles(ByVal aryMetaFiles As Collections.MetaDocuments)
        Public MustOverride Sub DumpNodeLinkInfo()

        'Copy/Paste callback methods
        Public MustOverride Sub CutSelected()
        Public MustOverride Sub CopySelected()
        Public MustOverride Sub PasteSelected(ByVal bInPlace As Boolean)
        Public MustOverride Sub DeleteSelected()

        Public MustOverride Sub AddStimulusToSelected()

        Public MustOverride Sub SendSelectedToBack()
        Public MustOverride Sub BringSelectedToFront()

        Public Overridable Sub OnEditPopupStart(ByVal mc As MenuCommand)
        End Sub

        Public Overridable Sub OnEditPopupEnd(ByVal mc As MenuCommand)
        End Sub

        Public Overridable Sub OnShapePopupStart(ByVal mc As MenuCommand)
        End Sub

        Public Overridable Sub OnShapePopupEnd(ByVal mc As MenuCommand)
        End Sub

        'Undo/Redo methods
        Public MustOverride Sub OnUndo()
        Public MustOverride Sub OnRedo()
        Public MustOverride Sub BeginGroupChange()
        Public MustOverride Sub EndGroupChange()

        'Zoom Methods
        Public MustOverride Sub FitToPage()
        Public MustOverride Sub ZoomBy(ByVal fltDelta As Single)
        Public MustOverride Sub ZoomTo(ByVal fltZoom As Single)

        Public Overridable Sub AddImages(ByVal colImages As Collections.DiagramImages)

            'We need to get a list of the diagram images that are in the correct index order
            Dim aryImages(colImages.Count - 1) As DataObjects.Behavior.DiagramImage
            Dim diImage As DataObjects.Behavior.DiagramImage

            Dim iIndex As Integer = 0
            For Each deEntry As DictionaryEntry In colImages
                diImage = DirectCast(deEntry.Value, DataObjects.Behavior.DiagramImage)
                aryImages(iIndex) = diImage
                iIndex = iIndex + 1
            Next

            For Each diImage In aryImages
                Me.AddImage(diImage)
            Next

        End Sub

        Public Overridable Function AddDiagram(ByVal strAssemblyName As String, ByVal strClassName As String, _
                                               Optional ByVal strPageName As String = "", _
                                               Optional ByVal strID As String = "") As Behavior.Diagram
            Dim bdDiagram As Forms.Behavior.Diagram = m_beEditor.CreateDiagram(strAssemblyName, strClassName, Me, strPageName)

            If strID.Trim.Length > 0 Then
                bdDiagram.ID = strID
            End If

            m_aryDiagrams.Add(bdDiagram.ID, bdDiagram)
            m_beEditor.HierarchyBar.AddDiagram(m_tnDiagramTreeNode, bdDiagram)

            Return bdDiagram
        End Function

        Public Overridable Function AddDiagram(ByVal bdDiagram As Forms.Behavior.Diagram) As Behavior.Diagram

            m_beEditor.RestoreDiagram(bdDiagram)
            m_aryDiagrams.Add(bdDiagram.ID, bdDiagram)
            m_beEditor.HierarchyBar.AddDiagram(m_tnDiagramTreeNode, bdDiagram)

            Return bdDiagram
        End Function

        Public Overridable Sub ClearDiagrams()

            Dim aryIDs As New ArrayList
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                aryIDs.Add(deEntry.Key)
            Next

            Dim bdDiagram As Diagram
            For Each strID As String In aryIDs
                bdDiagram = m_aryDiagrams(strID)
                RemoveDiagram(bdDiagram)
            Next

        End Sub

        Public Overridable Sub RemovingDiagram()
            Me.ClearDiagrams()
        End Sub

        Public Overridable Sub RestoringDiagram()

            m_bnParentDiagram.AddDiagram(Me)

            Dim bdChild As Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDeletedDiagrams
                bdChild = DirectCast(deEntry.Value, Forms.Behavior.Diagram)
                bdChild.RestoringDiagram()
            Next

        End Sub

        Public Overridable Sub RemoveDiagram(ByVal bdDiagram As Behavior.Diagram)

            'Allow this diagram to remove any child diagrams.
            If m_aryDiagrams.Contains(bdDiagram.ID) Then
                bdDiagram.RemovingDiagram()
                m_aryDiagrams.Remove(bdDiagram.ID)
            End If

            m_beEditor.RemoveDiagram(bdDiagram)

            If Not m_aryDeletedDiagrams.Contains(bdDiagram.ID) Then
                m_aryDeletedDiagrams.Add(bdDiagram.ID, bdDiagram)
            End If

        End Sub

        Public Overridable Sub RestoreDiagram(ByVal bdDiagram As Behavior.Diagram)

            If m_aryDeletedDiagrams.Contains(bdDiagram.ID) Then
                bdDiagram.RestoringDiagram()
            End If

        End Sub

        Public Overridable Sub CreateDiagramDropDownTree(ByVal tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node)
            Dim tnNode As New Crownwood.DotNetMagic.Controls.Node(Me.TabPageName)
            tnParent.Nodes.Add(tnNode)

            Dim bdDiagram As Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, Forms.Behavior.Diagram)
                bdDiagram.CreateDiagramDropDownTree(tvTree, tnNode)
            Next

            Dim bnNode As DataObjects.Behavior.Node
            For Each deItem As DictionaryEntry In m_aryNodes
                bnNode = DirectCast(deItem.Value, DataObjects.Behavior.Node)
                bnNode.CreateDiagramDropDownTree(tvTree, tnNode)
            Next

        End Sub

        Public Overridable Function FindDiagram(ByVal strID As String) As Forms.Behavior.Diagram

            If Me.ID = strID Then
                Return Me
            End If

            Dim bdDiagram As Forms.Behavior.Diagram
            Dim bdFound As Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, Forms.Behavior.Diagram)

                bdFound = bdDiagram.FindDiagram(strID)
                If Not bdFound Is Nothing Then
                    Return bdFound
                End If
            Next

        End Function

        Public Overridable Function FindDiagramByName(ByVal strName As String) As Forms.Behavior.Diagram

            If Me.DiagramName = strName Then
                Return Me
            End If

            Dim bdDiagram As Forms.Behavior.Diagram
            Dim bdFound As Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, Forms.Behavior.Diagram)

                bdFound = bdDiagram.FindDiagramByName(strName)
                If Not bdFound Is Nothing Then
                    Return bdFound
                End If
            Next

        End Function

        'Select callback methods
        Public Overridable Sub SelectAll()
            Try

                Me.SelectDataItem(Nothing, True)

                Dim doData As DataObjects.Behavior.Data
                For Each deEntry As DictionaryEntry In Me.Nodes
                    doData = DirectCast(deEntry.Value, DataObjects.Behavior.Data)
                    Me.SelectDataItem(doData, False)
                Next

                For Each deEntry As DictionaryEntry In Me.Links
                    doData = DirectCast(deEntry.Value, DataObjects.Behavior.Data)
                    Me.SelectDataItem(doData, False)
                Next

            Catch ex As System.Exception
                Throw ex
            End Try

        End Sub

        Public Overridable Sub SelectByType()
            Try
                Dim frmType As New AnimatGUI.Forms.Behavior.SelectByType

                frmType.Diagram = Me
                If frmType.ShowDialog = DialogResult.OK Then
                    Me.SelectDataItem(Nothing, True)

                    Dim doData As DataObjects.Behavior.Data
                    For Each deEntry As DictionaryEntry In Me.Nodes
                        doData = DirectCast(deEntry.Value, DataObjects.Behavior.Data)

                        If Util.IsTypeOf(doData.GetType, frmType.SelectedType, False) Then
                            Me.SelectDataItem(doData, False)
                        End If
                    Next

                    For Each deEntry As DictionaryEntry In Me.Links
                        doData = DirectCast(deEntry.Value, DataObjects.Behavior.Data)

                        If Util.IsTypeOf(doData.GetType, frmType.SelectedType, False) Then
                            Me.SelectDataItem(doData, False)
                        End If
                    Next
                End If

            Catch ex As System.Exception
                Throw ex
            End Try

        End Sub

        Public Overridable Sub Relabel()
            Dim frmRelabel As New AnimatGUI.Forms.Behavior.Relabel

            frmRelabel.Diagram = Me
            If frmRelabel.ShowDialog = DialogResult.OK Then
                Util.Relable(frmRelabel.Items, frmRelabel.txtMatch.Text, frmRelabel.txtReplace.Text)
            End If

        End Sub

        Public Overridable Sub RelabelSelected()
            Dim frmRelabel As New Forms.RelabelSelected

            If Me.Editor.SelectedObjects.Count > 0 Then
                If frmRelabel.ShowDialog() = DialogResult.OK Then
                    Dim aryObjs As ArrayList = DirectCast(Me.Editor.SelectedObjects.Clone(), ArrayList)
                    Util.RelableSelected(aryObjs, frmRelabel.txtNewLabel.Text, frmRelabel.StartWith, frmRelabel.IncrementBy)
                End If

                Me.IsDirty = True
            End If

        End Sub

        Public MustOverride Sub OnShowConnections(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnCompareItems(ByVal sender As Object, ByVal e As System.EventArgs)

        Public Overridable Sub RetrieveChildren(ByVal bThisDiagramOnly As Boolean, ByRef aryChildren As ArrayList)

            For Each deEntry As DictionaryEntry In m_aryNodes
                aryChildren.Add(deEntry.Value)
            Next

            For Each deEntry As DictionaryEntry In m_aryLinks
                aryChildren.Add(deEntry.Value)
            Next

            If Not bThisDiagramOnly Then
                Dim doChild As Diagram
                For Each deEntry As DictionaryEntry In Me.Diagrams
                    doChild = DirectCast(deEntry.Value, Diagram)
                    doChild.RetrieveChildren(bThisDiagramOnly, aryChildren)
                Next
            End If

        End Sub

        'Loading/Saving functions
        Public MustOverride Sub InitializeAfterLoad()
        Public MustOverride Sub VerifyData()
        Protected MustOverride Sub VerifyNodesExist()

        Public Shared Sub InitializeDataAfterLoad(ByRef aryData As AnimatGUI.Collections.AnimatSortedList)

            Dim aryItems As New AnimatGUI.Collections.BehaviorItems(Nothing)

            Dim bdData As DataObjects.Behavior.Data
            For Each deEntry As DictionaryEntry In aryData
                bdData = DirectCast(deEntry.Value, DataObjects.Behavior.Data)
                aryItems.Add(bdData)
            Next

            InitializeDataAfterLoad(aryItems)
        End Sub

        Protected Shared Sub InitializeDataAfterLoad(ByRef aryItems As AnimatGUI.Collections.BehaviorItems)

            Dim aryFailed As New AnimatGUI.Collections.BehaviorItems(Nothing)

            Dim bdData As DataObjects.Behavior.Data
            Dim iAttempt As Integer = 1

            'Run through and inialize them once.
            For Each bdData In aryItems
                If Not bdData.Initialized Then
                    bdData.InitializeAfterLoad()

                    If Not bdData.Initialized Then
                        Util.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Error, "Failed to initialize Data Item: " & bdData.Text & "  ID: " & bdData.ID)
                        aryFailed.Add(bdData)
                    End If
                End If
            Next

            If aryFailed.Count = 0 Then
                Return
            End If

            'Now if any of them failed to initialize then lets retry the attempt a few times.
            Dim bDone As Boolean = False
            While Not bDone And iAttempt < 5

                bDone = True
                iAttempt = iAttempt + 1

                For Each bdData In aryFailed

                    If Not bdData.Initialized Then
                        bdData.InitializeAfterLoad()

                        If Not bdData.Initialized Then
                            bDone = False
                        End If
                    End If
                Next

            End While

            'Called the failed to init function to remove that item.
            For Each bdData In aryFailed
                If Not bdData.Initialized Then
                    bdData.FailedToInitialize()
                End If
            Next

        End Sub

        Protected Overridable Sub CheckForInvalidLinks()

            Dim dlLink As DataObjects.Behavior.Link
            Dim aryDelete As New ArrayList
            For Each deEntry As DictionaryEntry In m_aryLinks
                dlLink = DirectCast(deEntry.Value, DataObjects.Behavior.Link)

                If dlLink.Origin Is Nothing OrElse dlLink.Destination Is Nothing Then
                    aryDelete.Add(dlLink)
                End If
            Next

            For Each dlLink In aryDelete
                Try
                    m_aryLinks.Remove(dlLink.ID, True, False)
                Catch ex As System.Exception
                    'This method can be called during loading and during normal operation so we need to try
                    'and remove the simulation links also if they exist, but if for some reason they do not, or if
                    'we are loading then we need to just eat the errors and go on.
                End Try
            Next

            'This is in here because prior to the fix that was put in to keep the links for copied
            'offpage connectors correct the data links for these objects could easily get lost.
            'If these links are lost then this will be sure to add them back correctly.
            For Each deEntry As DictionaryEntry In m_aryLinks
                dlLink = DirectCast(deEntry.Value, DataObjects.Behavior.Link)

                'If this link is not an inlink then we have a mismatch
                If dlLink.Initialized Then
                    If dlLink.Origin.Initialized AndAlso dlLink.Origin.Links(dlLink.ID) Is Nothing Then
                        dlLink.Origin.AddOutLink(dlLink)
                    End If
                    If dlLink.ActualOrigin.Initialized AndAlso dlLink.ActualOrigin.Links(dlLink.ID) Is Nothing Then
                        dlLink.ActualOrigin.AddOutLink(dlLink)
                    End If

                    If dlLink.Destination.Initialized AndAlso dlLink.Destination.Links(dlLink.ID) Is Nothing Then
                        dlLink.Destination.AddInLink(dlLink)
                    End If
                    If dlLink.ActualDestination.Initialized AndAlso dlLink.ActualDestination.Links(dlLink.ID) Is Nothing Then
                        dlLink.ActualDestination.AddInLink(dlLink)
                    End If
                End If
            Next

        End Sub

        Public MustOverride Sub SetItemsTempID(ByVal bdItem As AnimatGUI.DataObjects.Behavior.Data, ByVal strID As String)

        Public Overridable Sub GenerateTempSelectedIDs(ByVal bCopy As Boolean)
            m_strTempSelectedID = System.Guid.NewGuid().ToString()

            'Now have all nodes, links, and diagrams of this diagram generate temp ids
            Dim bnNode As AnimatGUI.DataObjects.Behavior.Node
            For Each deEntry As DictionaryEntry In m_aryNodes
                bnNode = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Node)
                bnNode.GenerateTempSelectedID(bCopy)
            Next

            Dim blLink As AnimatGUI.DataObjects.Behavior.Link
            For Each deEntry As DictionaryEntry In m_aryLinks
                blLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                blLink.GenerateTempSelectedID(bCopy)
            Next

            Dim bdDiagram As AnimatGUI.Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
                bdDiagram.GenerateTempSelectedIDs(bCopy)
            Next

        End Sub

        Public Overridable Sub ClearTempSelectedIDs()
            m_strTempSelectedID = ""

            'Now have all nodes, links, and diagrams of this diagram clear temp ids
            Dim bnNode As AnimatGUI.DataObjects.Behavior.Node
            For Each deEntry As DictionaryEntry In m_aryNodes
                bnNode = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Node)
                bnNode.ClearTempSelectedID()
            Next

            Dim blLink As AnimatGUI.DataObjects.Behavior.Link
            For Each deEntry As DictionaryEntry In m_aryLinks
                blLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                blLink.ClearTempSelectedID()
            Next

            Dim bdDiagram As AnimatGUI.Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
                bdDiagram.ClearTempSelectedIDs()
            Next
        End Sub

        Public MustOverride Function SaveSelected(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByVal bCopy As Boolean) As Boolean
        Public MustOverride Sub LoadSelected(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByVal bInPlace As Boolean)

        Public MustOverride Sub SaveDiagram(ByVal strFilename As String, ByVal eFormat As System.Drawing.Imaging.ImageFormat)

        Public Overridable Sub SaveDiagrams(ByVal strPath As String, ByVal eFormat As System.Drawing.Imaging.ImageFormat, ByVal strExtension As String)

            Me.SaveDiagram(strPath & "\" & Me.DiagramName & strExtension, eFormat)

            Dim bdDiagram As Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, Forms.Behavior.Diagram)

                bdDiagram.SaveDiagrams(strPath, eFormat, strExtension)
            Next

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_aryDiagrams.ClearIsDirty()
            m_aryDeletedDiagrams.ClearIsDirty()
            m_aryNodes.ClearIsDirty()
            m_aryLinks.ClearIsDirty()
        End Sub

#End Region

    End Class

End Namespace
