Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Forms.Tools

    Public Class SelectDataItem
        Inherits Forms.AnimatDialog

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
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents tvStructures As Crownwood.DotNetMagic.Controls.TreeControl
        Friend WithEvents ctrlSplitter As System.Windows.Forms.Splitter
        Friend WithEvents pnlItems As System.Windows.Forms.Panel
        Friend WithEvents ctrlProperties As System.Windows.Forms.PropertyGrid

        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnOk = New System.Windows.Forms.Button
            Me.pnlItems = New System.Windows.Forms.Panel
            Me.ctrlProperties = New System.Windows.Forms.PropertyGrid
            Me.ctrlSplitter = New System.Windows.Forms.Splitter
            Me.tvStructures = New Crownwood.DotNetMagic.Controls.TreeControl
            Me.pnlItems.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(440, 448)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 4
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(368, 448)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 3
            Me.btnOk.Text = "Ok"
            '
            'pnlItems
            '
            Me.pnlItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pnlItems.Controls.Add(Me.ctrlProperties)
            Me.pnlItems.Controls.Add(Me.ctrlSplitter)
            Me.pnlItems.Controls.Add(Me.tvStructures)
            Me.pnlItems.Location = New System.Drawing.Point(8, 8)
            Me.pnlItems.Name = "pnlItems"
            Me.pnlItems.Size = New System.Drawing.Size(496, 432)
            Me.pnlItems.TabIndex = 5
            '
            'ctrlProperties
            '
            Me.ctrlProperties.CommandsVisibleIfAvailable = True
            Me.ctrlProperties.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ctrlProperties.LargeButtons = False
            Me.ctrlProperties.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.ctrlProperties.Location = New System.Drawing.Point(288, 0)
            Me.ctrlProperties.Name = "ctrlProperties"
            Me.ctrlProperties.Size = New System.Drawing.Size(208, 432)
            Me.ctrlProperties.TabIndex = 4
            Me.ctrlProperties.Text = "ctrlProperties"
            Me.ctrlProperties.ViewBackColor = System.Drawing.SystemColors.Window
            Me.ctrlProperties.ViewForeColor = System.Drawing.SystemColors.WindowText
            '
            'ctrlSplitter
            '
            Me.ctrlSplitter.Cursor = System.Windows.Forms.Cursors.HSplit
            Me.ctrlSplitter.Location = New System.Drawing.Point(280, 0)
            Me.ctrlSplitter.MinExtra = 100
            Me.ctrlSplitter.MinSize = 75
            Me.ctrlSplitter.Name = "ctrlSplitter"
            Me.ctrlSplitter.Size = New System.Drawing.Size(8, 432)
            Me.ctrlSplitter.TabIndex = 3
            Me.ctrlSplitter.TabStop = False
            '
            'tvStructures
            '
            Me.tvStructures.Dock = System.Windows.Forms.DockStyle.Left
            Me.tvStructures.DoubleClickExpand = Crownwood.DotNetMagic.Controls.ClickExpandAction.None
            Me.tvStructures.FocusNode = Nothing
            Me.tvStructures.HotBackColor = System.Drawing.Color.Empty
            Me.tvStructures.HotForeColor = System.Drawing.Color.Empty
            Me.tvStructures.ImageIndex = -1
            Me.tvStructures.Location = New System.Drawing.Point(0, 0)
            Me.tvStructures.Name = "tvStructures"
            Me.tvStructures.SelectedNode = Nothing
            Me.tvStructures.SelectedNoFocusBackColor = System.Drawing.SystemColors.Control
            Me.tvStructures.SelectedImageIndex = -1
            Me.tvStructures.Size = New System.Drawing.Size(280, 432)
            Me.tvStructures.TabIndex = 2
            Me.tvStructures.SelectMode = Crownwood.DotNetMagic.Controls.SelectMode.Single
            '
            'SelectDataItem
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(512, 478)
            Me.Controls.Add(Me.pnlItems)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "SelectDataItem"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Select Data Item"
            Me.pnlItems.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_imgManager As New AnimatGUI.Framework.ImageManager
        Protected m_doDataColumn As DataObjects.Charting.DataColumn
        Protected m_doColumnType As AnimatGUI.DataObjects.Charting.DataColumn

        Protected m_doSelectedStructure As DataObjects.Physical.PhysicalStructure
        Protected m_tpPartTemplate As Type

#End Region

#Region " Properties "

        Public ReadOnly Property ImageManager() As AnimatGUI.Framework.ImageManager
            Get
                Return m_imgManager
            End Get
        End Property

        Public ReadOnly Property TreeView() As Crownwood.DotNetMagic.Controls.TreeControl
            Get
                Return tvStructures
            End Get
        End Property

        Public Overridable Property ColumnType() As AnimatGUI.DataObjects.Charting.DataColumn
            Get
                Return m_doColumnType
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Charting.DataColumn)
                m_doColumnType = Value
            End Set
        End Property

        Public Property DataColumn() As DataObjects.Charting.DataColumn
            Get
                Return m_doDataColumn
            End Get
            Set(ByVal Value As DataObjects.Charting.DataColumn)
                m_doDataColumn = Value
            End Set
        End Property

        Public Overridable Property SelectedStructure() As DataObjects.Physical.PhysicalStructure
            Get
                Return m_doSelectedStructure
            End Get
            Set(ByVal Value As DataObjects.Physical.PhysicalStructure)
                m_doSelectedStructure = Value
            End Set
        End Property

        Public Overridable Property TemplatePartType() As Type
            Get
                Return m_tpPartTemplate
            End Get
            Set(ByVal Value As Type)
                m_tpPartTemplate = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Overridable Sub BuildTreeView()

            tvStructures.Nodes.Clear()
            tvStructures.ImageList = m_imgManager.ImageList
            Me.ImageManager.ImageList.ImageSize = New Size(16, 16)

            Dim oSimNode As Crownwood.DotNetMagic.Controls.Node = Util.Simulation.CreateDataItemTreeView(Me, Nothing, m_tpPartTemplate)

            If m_doSelectedStructure Is Nothing Then
                Dim doStruct As DataObjects.Physical.PhysicalStructure
                For Each deEntry As DictionaryEntry In Util.Environment.Structures
                    doStruct = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)
                    doStruct.CreateDataItemTreeView(Me, oSimNode, m_tpPartTemplate)
                Next

                For Each deEntry As DictionaryEntry In Util.Environment.Organisms
                    doStruct = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)
                    doStruct.CreateDataItemTreeView(Me, oSimNode, m_tpPartTemplate)
                Next

                Dim doStim As DataObjects.ExternalStimuli.Stimulus
                For Each deEntry As DictionaryEntry In Util.Simulation.ProjectStimuli
                    doStim = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)

                    If doStim.CanBeCharted Then
                        doStim.CreateDataItemTreeView(Me, oSimNode, m_tpPartTemplate)
                    End If
                Next

                If Util.Environment.Structures.Count + Util.Environment.Organisms.Count = 1 Then
                    tvStructures.ExpandAll()
                End If
            Else
                m_doSelectedStructure.CreateDataItemTreeView(Me, oSimNode, m_tpPartTemplate)
                tvStructures.ExpandAll()
            End If

        End Sub

#End Region

#Region " Events "

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

            Try
                If m_doDataColumn Is Nothing Then
                    Throw New System.Exception("You must select a data item to add to the chart")
                Else
                    m_doDataColumn.SelectionInProgress = False
                End If

                If m_doDataColumn.Name.Trim.Length = 0 Then
                    Throw New System.Exception("The name of the data column must not be blank")
                End If

                If m_doDataColumn.DataItem Is Nothing Then
                    Throw New System.Exception("A data item must be set!")
                End If

                If Not m_doDataColumn.DataItem.CanBeCharted Then
                    Throw New System.Exception("Please select a data item that can be charted.")
                End If

                If m_doDataColumn.DataType Is Nothing OrElse m_doDataColumn.DataType.Value Is Nothing OrElse m_doDataColumn.DataType.Value Is Nothing Then
                    Throw New System.Exception("Please select a data type for this data item.")
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_AfterSelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tvStructures.AfterSelectionChanged

            Try
                If tvStructures.SelectedNodes.Count > 0 Then
                    If Not tvStructures.SelectedNode Is Nothing AndAlso Not tvStructures.SelectedNode.Tag Is Nothing AndAlso TypeOf tvStructures.SelectedNode.Tag Is DataObjects.DragObject Then
                        Dim doDrag As DataObjects.DragObject = DirectCast(tvStructures.SelectedNode.Tag, DataObjects.DragObject)
                        m_doDataColumn = m_doColumnType.CreateDataColumn(doDrag, False)
                        m_doDataColumn.SelectionInProgress = True  'Make sure that any property changes do not cause the chart to update

                        If doDrag.CanBeCharted Then
                            ctrlProperties.SelectedObject = m_doDataColumn.Properties()
                        Else
                            ctrlProperties.SelectedObject = Nothing
                        End If
                    Else
                        m_doDataColumn = Nothing
                        ctrlProperties.SelectedObject = Nothing
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub ctrlTreeView_ShowContextMenuNode(ByVal tc As Crownwood.DotNetMagic.Controls.TreeControl, ByVal e As Crownwood.DotNetMagic.Controls.CancelNodeEventArgs) Handles tvStructures.ShowContextMenuNode

            Try
                'We will set the contextmenunode property within our call
                e.Cancel = False

                Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
                Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

                mcExpandAll.Tag = e.Node.Tag
                mcCollapseAll.Tag = e.Node.Tag

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("Forms.Tools.SelectDataItem.ShowContextMenuNode", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcExpandAll, mcCollapseAll})

                tvStructures.ContextMenuNode = popup

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnExpandAll(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not tvStructures.SelectedNode Is Nothing Then
                    tvStructures.SelectedNode.ExpandAll()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnCollapseAll(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If Not tvStructures.SelectedNode Is Nothing Then
                    tvStructures.SelectedNode.Collapse()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Sub SelectItem(ByVal strPath As String)
            Dim tnNode As Crownwood.DotNetMagic.Controls.Node = Util.FindTreeNodeByPath(strPath, tvStructures.Nodes)
            tvStructures.SelectedNode = tnNode
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            m_btnOk = Me.btnOk
            m_btnCancel = Me.btnCancel
        End Sub

#End Region

    End Class

End Namespace
