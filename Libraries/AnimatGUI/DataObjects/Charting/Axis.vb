Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects.Charting

    Public Class Axis
        Inherits AnimatGUI.DataObjects.DragObject

#Region " Attributes "

        Protected m_frmParentChart As Forms.Tools.DataChart
        Protected m_iWorkingAxis As Integer = -1
        Protected m_aryDataColumns As New AnimatGUI.Collections.SortedDataColumns(Me)

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                If Value.Trim.Length = 0 Then
                    Throw New System.Exception("The axis name must not be blank.")
                End If

                'Check to see if it is the same as an existing name.
                If m_frmParentChart.AxisList.Contains(Value.Trim) Then
                    Throw New System.Exception("There is already an axis with the name '" & Value & "'")
                End If

                If m_frmParentChart.AxisList.Contains(m_strName) Then
                    m_frmParentChart.AxisList.Remove(m_strName)
                End If

                m_strName = Value

                m_frmParentChart.AxisList.Add(m_strName, Me)

                If Not m_doParent Is Nothing AndAlso Not m_doParent.WorkspaceNode Is Nothing Then Me.CreateWorkspaceTreeView(m_doParent, m_doParent.WorkspaceNode)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property WorkingAxis() As Integer
            Get
                Return m_iWorkingAxis
            End Get
            Set(ByVal Value As Integer)
                If Value < 0 OrElse Value > 5 OrElse ((Me.ParentChart.AxisList.Count > 0) AndAlso (Value > (Me.ParentChart.AxisList.Count - 1))) Then
                    Throw New System.Exception("The working axis value must be between 0 and " & (Me.ParentChart.AxisList.Count - 1) & ".")
                End If

                'Lets find the working axis that is currently using this value.
                Dim doSwitchAxis As AnimatGUI.DataObjects.Charting.Axis = Me.ParentChart.FindWorkingAxis(Value)

                If Not doSwitchAxis Is Nothing Then
                    doSwitchAxis.WorkingAxisInternal = m_iWorkingAxis
                End If

                m_iWorkingAxis = Value

                Me.ParentChart.UpdateChartConfiguration(True)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property WorkingAxisInternal() As Integer
            Get
                Return m_iWorkingAxis
            End Get
            Set(ByVal Value As Integer)
                m_iWorkingAxis = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property DataColumns() As AnimatGUI.Collections.SortedDataColumns
            Get
                Return m_aryDataColumns
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property ParentChart() As Forms.Tools.DataChart
            Get
                Return m_frmParentChart
            End Get
            Set(ByVal Value As Forms.Tools.DataChart)
                m_frmParentChart = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property RootForm() As System.Windows.Forms.Form
            Get
                Return m_frmParentChart
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Y.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property TimeStep() As Double
            Get
                Dim dblTimeStep As Double = -1
                Dim dblTemp As Double

                Dim doColumn As DataObjects.Charting.DataColumn
                For Each deEntry As DictionaryEntry In m_aryDataColumns
                    doColumn = DirectCast(deEntry.Value, DataObjects.Charting.DataColumn)

                    If Not doColumn.DataItem Is Nothing Then
                        dblTemp = doColumn.DataItem.TimeStep

                        If dblTimeStep < 0 OrElse dblTemp < dblTimeStep Then
                            dblTimeStep = dblTemp
                        End If
                    End If
                Next

                If dblTimeStep < 0 Then
                    Return Util.Environment.PhysicsTimeStep.ActualValue
                Else
                    Return dblTimeStep
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property RequiresAutoDataCollectInterval() As Boolean
            Get
                Dim doColumn As DataObjects.Charting.DataColumn
                For Each deEntry As DictionaryEntry In m_aryDataColumns
                    doColumn = DirectCast(deEntry.Value, DataObjects.Charting.DataColumn)
                    If doColumn.RequiresAutoDataCollectInterval Then
                        Return True
                    End If
                Next
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Sub New(ByVal frmChart As AnimatGUI.Forms.Tools.ToolForm)
            MyBase.New(frmChart.FormHelper)

            If TypeOf frmChart Is Forms.Tools.DataChart Then
                m_frmParentChart = DirectCast(frmChart, Forms.Tools.DataChart)
            End If
        End Sub

        Public Overridable Sub InitializeChartData()
        End Sub

        Public Overridable Sub UpdateChartConfiguration(ByRef iSubSet As Integer)
        End Sub

        Public Overridable Sub ReconfigureChartData(ByRef aryOldX(,) As Single, ByRef aryOldY(,) As Single, ByRef aryNewX(,) As Single, ByRef aryNewY(,) As Single)
        End Sub

        Public Overridable Function IsValidAxis() As Boolean

            RemoveInvalidDataColumns()

            If m_aryDataColumns.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Protected Overridable Sub RemoveInvalidDataColumns()

            Dim doColumn As AnimatGUI.DataObjects.Charting.DataColumn
            Dim aryDelete As New Collection
            For Each deEntry As DictionaryEntry In m_aryDataColumns
                doColumn = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Charting.DataColumn)

                If Not doColumn.IsValidColumn Then
                    aryDelete.Add(doColumn)
                End If
            Next

            For Each doColumn In aryDelete
                doColumn.RemoveFromAxis()
            Next
        End Sub

        Public Overridable Sub PrepareForCharting()

            'First lets go through and remove any invalid data columns. This is something that
            'could happen if the referneced part in that data was deleted.
            RemoveInvalidDataColumns()

            Dim doColumn As AnimatGUI.DataObjects.Charting.DataColumn
            For Each deEntry As DictionaryEntry In m_aryDataColumns
                doColumn = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Charting.DataColumn)

                If doColumn.IsValidColumn Then
                    doColumn.PrepareForCharting()
                End If
            Next

        End Sub

        Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean

            Try
                If bAskToDelete AndAlso Util.ShowMessage("Are you certain that you want to delete this " & _
                                    "axis and all data columns?", "Delete Axis", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                    DeleteAxis(True)
                    Return False
                End If

                Return True
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Function

        Public Overridable Sub DeleteAxis(Optional ByVal bUpdateChart As Boolean = True)

            If Not m_frmParentChart Is Nothing AndAlso m_frmParentChart.AxisList.Contains(Me.Name) Then
                m_frmParentChart.AxisList.Remove(Me.Name)

                'Now lets go through and decrement the working axis value for all remaining axises greater than this one.
                Dim doAxis As Axis
                For Each deEntry As DictionaryEntry In m_frmParentChart.AxisList
                    doAxis = DirectCast(deEntry.Value, Axis)

                    If doAxis.WorkingAxis > Me.WorkingAxis Then
                        doAxis.WorkingAxisInternal = doAxis.WorkingAxis - 1
                    End If
                Next

                Me.RemoveWorksapceTreeView()

                If bUpdateChart Then
                    m_frmParentChart.UpdateChartConfiguration(True)
                End If
            End If
        End Sub

        Public Overridable Sub AddDataItem()
            OnAddDataItem(Me, New System.EventArgs)
        End Sub

#Region " TreeView Methods "

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, ByVal doParentNode As Crownwood.DotNetMagic.Controls.Node)
            MyBase.CreateWorkspaceTreeView(doParent, doParentNode)

            Dim doColumn As DataObjects.Charting.DataColumn
            For Each deEntry As DictionaryEntry In m_aryDataColumns
                doColumn = DirectCast(deEntry.Value, DataObjects.Charting.DataColumn)
                doColumn.CreateWorkspaceTreeView(Me, Me.WorkspaceNode)
            Next
        End Sub

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Axis", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))
                Dim mcAddItem As New System.Windows.Forms.ToolStripMenuItem("Add Item", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddChartItem.gif"), New EventHandler(AddressOf Me.OnAddDataItem))
                Dim mcSepExpand As New ToolStripSeparator()
                Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
                Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

                mcExpandAll.Tag = tnSelectedNode
                mcCollapseAll.Tag = tnSelectedNode

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Charting.Axis.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete, mcAddItem, mcSepExpand, mcExpandAll, mcCollapseAll})

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                Return True
            Else
                Dim doColumn As DataObjects.Charting.DataColumn
                For Each deEntry As DictionaryEntry In m_aryDataColumns
                    doColumn = DirectCast(deEntry.Value, DataObjects.Charting.DataColumn)
                    If doColumn.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then
                        Return True
                    End If
                Next
            End If

            Return False
        End Function

#End Region

#Region " DragDrop Methods "

        Public Overridable Function DroppedDragData(ByVal doDrag As Framework.DataDragHelper) As Boolean
            Dim bVal As Boolean = True

            If TypeOf doDrag.m_doData Is DataObjects.DragObject Then
                Dim doItem As DataObjects.DragObject = DirectCast(doDrag.m_doData, DataObjects.DragObject)
                If doItem.CanBeCharted Then
                    DroppedItem(doItem)
                End If
            ElseIf TypeOf doDrag.m_doData Is AnimatGUI.DataObjects.Charting.DataColumn Then
                Dim doColumn As AnimatGUI.DataObjects.Charting.DataColumn = DirectCast(doDrag.m_doData, AnimatGUI.DataObjects.Charting.DataColumn)
                MovedDataColumn(doColumn)
            Else
                bVal = False
            End If

            'Cause the entire tree view to reapaint itself because if not it leaves weird stuff on the screen.
            Util.ProjectWorkspace.ctrlTreeView.Invalidate()

            If bVal Then
                m_frmParentChart.UpdateChartConfiguration(True)
            End If

            Return bVal
        End Function

        Public Overridable Function CreateDataColumn(ByVal doItem As AnimatGUI.DataObjects.DragObject, Optional ByVal bAutoAddToAxis As Boolean = True) As AnimatGUI.DataObjects.Charting.DataColumn
            Return Nothing
        End Function

        Protected Overridable Sub DroppedItem(ByVal doItem As DataObjects.DragObject)
        End Sub

        Protected Overridable Sub DroppedItem(ByVal doColumn As DataObjects.Charting.DataColumn)
        End Sub

        Public Overridable Sub MovedDataColumn(ByVal doColumn As DataObjects.Charting.DataColumn)
            If Not doColumn.ParentAxis Is Me Then
                doColumn.RemoveFromAxis()
                doColumn.AddtoAxis(Me)
            End If
        End Sub

#End Region

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Axis Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strName.GetType(), "Name", _
                                        "Axis Properties", "The name of this axis.", m_strName))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Chart Axis", m_iWorkingAxis.GetType(), "WorkingAxis", _
                                        "Axis Properties", "Determines which axis on the chart that this one represents.", m_iWorkingAxis))

        End Sub

        Public Overrides Sub EnsureFormActive()
            If Not Me.ParentChart Is Nothing Then
                Me.ParentChart.MakeVisible()
            End If
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.IntoElem() 'Into Axis Element

            m_strName = oXml.GetChildString("Name")
            m_iWorkingAxis = oXml.GetChildInt("WorkingAxis")

            If oXml.FindChildElement("DataColumns", False) Then
                oXml.IntoChildElement("DataColumns")
                Dim doColumn As AnimatGUI.DataObjects.Charting.DataColumn
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    doColumn = DirectCast(Util.LoadClass(oXml, iIndex, Me), AnimatGUI.DataObjects.Charting.DataColumn)
                    doColumn.LoadData(oXml)

                    If doColumn.IsValidColumn Then
                        m_aryDataColumns.Add(doColumn.ID, doColumn, False)
                    End If
                Next
                oXml.OutOfElem()  'Outof InLinks Element
            End If

            oXml.OutOfElem() 'Outof Form Element
        End Sub


        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.AddChildElement("Axis")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("WorkingAxis", m_iWorkingAxis)

            oXml.AddChildElement("DataColumns")
            oXml.IntoElem()

            Dim aryRemove As New Collection
            Dim doColumn As AnimatGUI.DataObjects.Charting.DataColumn
            For Each deEntry As DictionaryEntry In m_aryDataColumns
                doColumn = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Charting.DataColumn)
                If doColumn.IsValidColumn() Then
                    doColumn.SaveData(oXml)
                Else
                    'Util.ShowMessage("About to remove a column that is showing as invalid. Column: " & doColumn.Name)
                    aryRemove.Add(doColumn)
                End If
            Next

            For Each doColumn In aryRemove
                m_aryDataColumns.Remove(doColumn.Name)
                doColumn.RemoveWorksapceTreeView()
            Next

            oXml.OutOfElem()

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            Me.RemoveInvalidDataColumns()

            Dim doColumn As AnimatGUI.DataObjects.Charting.DataColumn
            For Each deEntry As DictionaryEntry In m_aryDataColumns
                doColumn = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Charting.DataColumn)
                doColumn.SaveSimulationXml(oXml)
            Next

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrigAxis As Axis = DirectCast(doOriginal, Axis)

            m_frmParentChart = doOrigAxis.m_frmParentChart
            m_strName = doOrigAxis.m_strName
            m_iWorkingAxis = doOrigAxis.m_iWorkingAxis
            m_aryDataColumns = DirectCast(doOrigAxis.m_aryDataColumns.Clone(), Collections.SortedDataColumns)
            m_WorkspaceImage = DirectCast(doOrigAxis.m_WorkspaceImage.Clone, Image)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doAxis As New Axis(doParent)
            doAxis.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doAxis.AfterClone(Me, bCutData, doRoot, doAxis)
            Return doAxis
        End Function

#End Region

#End Region

#Region " Events "

        Protected Overridable Sub OnAddDataItem(ByVal sender As Object, ByVal e As System.EventArgs)

            'Try
            '    Util.DisableDirtyFlags = True

            '    Dim frmSelectItem As New Forms.Tools.SelectDataItem

            '    frmSelectItem.Axis = Me
            '    frmSelectItem.BuildTreeView()
            '    If frmSelectItem.ShowDialog(Me.ParentChart) = DialogResult.OK Then
            '        Util.DisableDirtyFlags = False
            '        Me.DroppedItem(frmSelectItem.DataColumn)
            '        m_frmParentChart.UpdateChartConfiguration(True)
            '    End If

            'Catch ex As System.Exception
            '    AnimatGUI.Framework.Util.DisplayError(ex)
            'Finally
            '    Util.DisableDirtyFlags = False
            'End Try
        End Sub

#End Region

#Region " Drag Object Overrides "

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides Property DragImage() As System.Drawing.Image
            Get
                Return Me.WorkspaceImage
            End Get
            Set(ByVal Value As System.Drawing.Image)
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return Me.WorkspaceImageName
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As DragObject
            Return Nothing
        End Function

        <Browsable(False)> _
        Public Overrides Property ItemName() As String
            Get
                Return Me.Name
            End Get
            Set(ByVal Value As String)
                Me.Name = Value
            End Set
        End Property

        Public Overrides Sub InitializeSimulationReferences()
            If Me.IsInitialized Then
                'We explicitly do NOT Call the base init here. The reason is that the Axis does not have a corresponding
                ' object within the simulation. It is only an organiaztional tool within the GUI. It does not exist in the simulation.
                'MyBase.InitializeSimulationReferences()

                Dim doColumn As AnimatGUI.DataObjects.Charting.DataColumn
                For Each deEntry As DictionaryEntry In m_aryDataColumns
                    doColumn = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Charting.DataColumn)
                    doColumn.InitializeSimulationReferences()
                Next
            End If
        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            Dim doColumn As AnimatGUI.DataObjects.Charting.DataColumn
            For Each deEntry As DictionaryEntry In m_aryDataColumns
                doColumn = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Charting.DataColumn)
                doColumn.InitializeAfterLoad()
            Next

        End Sub

        <Browsable(False)> _
        Public Overrides ReadOnly Property StructureID() As String
            Get
                Return ""
            End Get
        End Property

#End Region

    End Class

End Namespace
