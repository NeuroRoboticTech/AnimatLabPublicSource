Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Forms.Tools

    Public MustInherit Class DataChart
        Inherits ToolForm

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
            Me.Title = "DataChart"
        End Sub

#End Region

#Region " Attributes "

        Protected m_gnGain As AnimatGUI.DataObjects.Gain

        Protected m_bAddedHandlers As Boolean = False

        Protected m_bAutoCollectDataInterval As Boolean = True

        Protected m_snCollectDataInterval As AnimatGUI.Framework.ScaledNumber
        Protected m_snCollectTimeWindow As AnimatGUI.Framework.ScaledNumber
        Protected m_snUpdateDataInterval As AnimatGUI.Framework.ScaledNumber

        Protected m_bSetStartEndTime As Boolean = True
        Protected m_snCollectStartTime As AnimatGUI.Framework.ScaledNumber
        Protected m_snCollectEndTime As AnimatGUI.Framework.ScaledNumber

        Protected m_aryAxisList As New Collections.SortedAxisList(m_doFormHelper)

        Protected m_aryAutoFillColors(20) As System.Drawing.Color
        Protected m_iAutoFillColor As Integer = 0

        Protected m_doLastSelectedAxis As DataObjects.Charting.Axis

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides ReadOnly Property ToolType() As String
            Get
                Return "DataChart"
            End Get
        End Property

        Public Overridable Property Gain() As AnimatGUI.DataObjects.Gain
            Get
                Return m_gnGain
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                m_gnGain = Value
                DrawGainChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property AutoCollectDataInterval() As Boolean
            Get
                Return m_bAutoCollectDataInterval
            End Get
            Set(ByVal Value As Boolean)
                m_bAutoCollectDataInterval = Value
                If m_bAutoCollectDataInterval Then
                    ResetCollectDataInterval()
                End If
                Util.ProjectWorkspace.RefreshProperties()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property CollectDataInterval() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snCollectDataInterval
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                'If we are attempting to set it to -1 then this means there are no axis/data values and we are setting to a default value, so do not set it.
                If Value.ActualValue = -1 Then
                    Return
                End If

                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The collect data interval must be greater than zero.")
                End If

                If Value.ActualValue >= m_snUpdateDataInterval.ActualValue Then
                    Throw New System.Exception("The collect data interval must be less than the update data interval.")
                End If
                SetSimData("CollectInterval", Value.ActualValue.ToString, True)

                m_snCollectDataInterval.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property CollectTimeWindow() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snCollectTimeWindow
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                If Value.ActualValue <= m_snCollectDataInterval.ActualValue Then
                    Throw New System.Exception("The collect time window must be greater than the collect interval.")
                End If
                SetSimData("CollectTimeWindow", Value.ActualValue.ToString, True)

                m_snCollectTimeWindow.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property UpdateDataInterval() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snUpdateDataInterval
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The update data interval must be greater than zero.")
                End If

                If Value.ActualValue <= m_snCollectDataInterval.ActualValue Then
                    Throw New System.Exception("The update data interval must be greater than the collect data interval.")
                End If

                m_snUpdateDataInterval.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property SetStartEndTime() As Boolean
            Get
                Return m_bSetStartEndTime
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("SetStartEndTime", Value.ToString, True)
                m_bSetStartEndTime = Value
                Util.ProjectWorkspace.RefreshProperties()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property CollectStartTime() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snCollectStartTime
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The start time must be greater than zero.")
                End If

                If Value.ActualValue >= m_snCollectEndTime.ActualValue Then
                    Throw New System.Exception("The start time must not be greater than the end time.")
                End If
                SetSimData("StartTime", Value.ActualValue.ToString, True)

                m_snCollectStartTime.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property CollectEndTime() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snCollectEndTime
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The end time must be greater than zero.")
                End If

                If Value.ActualValue <= m_snCollectStartTime.ActualValue Then
                    Throw New System.Exception("The end time must not be less than the start time.")
                End If
                SetSimData("EndTime", Value.ActualValue.ToString, True)

                m_snCollectEndTime.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property AxisList() As Collections.SortedAxisList
            Get
                Return m_aryAxisList
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property DataColumnCount() As Integer
            Get
                Dim iCount As Integer = 0
                Dim doAxis As DataObjects.Charting.Axis
                For Each deEntry As DictionaryEntry In m_aryAxisList
                    doAxis = DirectCast(deEntry.Value, DataObjects.Charting.Axis)
                    iCount = iCount + doAxis.DataColumns.Count
                Next

                Return iCount
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property AutoFillColors() As System.Drawing.Color()
            Get
                Return m_aryAutoFillColors
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property NextAutoFillColor() As System.Drawing.Color
            Get
                Return m_aryAutoFillColors(m_iAutoFillColor)
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property RequiresAutoDataCollectInterval() As Boolean
            Get
                Dim doAxis As DataObjects.Charting.Axis
                For Each deEntry As DictionaryEntry In m_aryAxisList
                    doAxis = DirectCast(deEntry.Value, DataObjects.Charting.Axis)
                    If doAxis.RequiresAutoDataCollectInterval Then
                        Return True
                    End If
                Next
            End Get
        End Property

        Public Overridable Property LastSelectedAxis() As DataObjects.Charting.Axis
            Get
                Return m_doLastSelectedAxis
            End Get
            Set(ByVal Value As DataObjects.Charting.Axis)
                m_doLastSelectedAxis = Value
            End Set
        End Property

        Public MustOverride Property MainTitle() As String
        Public MustOverride Property SubTitle() As String
        Public MustOverride Property XAxisLabel() As String
        Public MustOverride Property YAxisLabel() As String
        Public MustOverride Property AutoScaleData() As Boolean
        Public MustOverride Property XAxisSize() As PointF
        Public MustOverride Property YAxisSize() As PointF
        Public MustOverride ReadOnly Property Chart() As Object

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            MyBase.Initialize(frmParent)

            m_snCollectDataInterval = New AnimatGUI.Framework.ScaledNumber(Me.FormHelper, "CollectDataInterval", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            m_snCollectTimeWindow = New AnimatGUI.Framework.ScaledNumber(Me.FormHelper, "CollectTimeWindow", 3, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_snUpdateDataInterval = New AnimatGUI.Framework.ScaledNumber(Me.FormHelper, "UpdateDataInterval", 200, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")

            m_snCollectStartTime = New AnimatGUI.Framework.ScaledNumber(Me.FormHelper, "CollectStartTime", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_snCollectEndTime = New AnimatGUI.Framework.ScaledNumber(Me.FormHelper, "CollectEndTime", 10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")

            m_aryAutoFillColors(0) = System.Drawing.Color.Red
            m_aryAutoFillColors(1) = System.Drawing.Color.Lime
            m_aryAutoFillColors(2) = System.Drawing.Color.Blue
            m_aryAutoFillColors(3) = System.Drawing.Color.Yellow
            m_aryAutoFillColors(4) = System.Drawing.Color.Orange
            m_aryAutoFillColors(5) = System.Drawing.Color.Cyan
            m_aryAutoFillColors(6) = System.Drawing.Color.Magenta
            m_aryAutoFillColors(7) = System.Drawing.Color.Gold
            m_aryAutoFillColors(8) = System.Drawing.Color.Turquoise
            m_aryAutoFillColors(9) = System.Drawing.Color.Red
            m_aryAutoFillColors(10) = System.Drawing.Color.LightSteelBlue

            m_aryAutoFillColors(11) = System.Drawing.Color.Tomato
            m_aryAutoFillColors(12) = System.Drawing.Color.DarkViolet
            m_aryAutoFillColors(13) = System.Drawing.Color.Pink
            m_aryAutoFillColors(14) = System.Drawing.Color.SeaGreen
            m_aryAutoFillColors(15) = System.Drawing.Color.DeepPink
            m_aryAutoFillColors(16) = System.Drawing.Color.Crimson
            m_aryAutoFillColors(17) = System.Drawing.Color.Violet
            m_aryAutoFillColors(18) = System.Drawing.Color.SpringGreen
            m_aryAutoFillColors(19) = System.Drawing.Color.Navy
            m_aryAutoFillColors(20) = System.Drawing.Color.OliveDrab

            AddHandlers()

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("MainTitle", GetType(String), "MainTitle", _
                                        "Gain Limits", "Sets the main title of this graph.", MainTitle))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Settings", "ID", Me.ID, True))
        End Sub

        Public Overridable Sub DrawGainChart()
        End Sub

        Public Overridable Sub IncrementAutoFillColor()
            m_iAutoFillColor = m_iAutoFillColor + 1
            If m_iAutoFillColor > 20 Then m_iAutoFillColor = 0
        End Sub

#Region " TreeView Methods "

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, _
                                                       ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                       Optional ByVal bRootObject As Boolean = False)
            MyBase.CreateWorkspaceTreeView(doParent, tnParentNode, bRootObject)

            Dim doAxis As DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, DataObjects.Charting.Axis)
                doAxis.CreateWorkspaceTreeView(Me.FormHelper, Me.WorkspaceNode)
            Next

        End Sub

        Public Overrides Function CreateObjectListTreeView(ByVal doParent As Framework.DataObject, _
                                                           ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                           ByVal mgrImageList As AnimatGUI.Framework.ImageManager) As Crownwood.DotNetMagic.Controls.Node
            Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateObjectListTreeView(doParent, tnParentNode, mgrImageList)

            Dim doAxis As DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, DataObjects.Charting.Axis)
                doAxis.CreateObjectListTreeView(Me.FormHelper, tnNode, mgrImageList)
            Next

        End Function

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

            If tnSelectedNode Is Me.WorkspaceNode Then
                Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Chart", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))
                Dim mcAddAxis As New System.Windows.Forms.ToolStripMenuItem("Add Axis", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddChartAxis.gif"), New EventHandler(AddressOf Me.OnAddAxis))
                Dim mcClearCharts As New System.Windows.Forms.ToolStripMenuItem("Clear Charts", Util.Application.ToolStripImages.GetImage("AnimatGUI.Blank.gif"), New EventHandler(AddressOf Me.OnClearCharts))
                Dim mcSepExpand As New ToolStripSeparator()
                Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
                Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

                mcExpandAll.Tag = tnSelectedNode
                mcCollapseAll.Tag = tnSelectedNode

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.Forms.Tools.DataChart.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete, mcAddAxis, mcClearCharts, mcSepExpand, mcExpandAll, mcCollapseAll})

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                Return True
            Else
                Dim doAxis As DataObjects.Charting.Axis
                For Each deEntry As DictionaryEntry In m_aryAxisList
                    doAxis = DirectCast(deEntry.Value, DataObjects.Charting.Axis)
                    If doAxis.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then
                        Return True
                    End If
                Next
            End If

            Return False
        End Function

#End Region

        Protected Overrides Sub DroppedDragData(ByVal doDrag As Framework.DataDragHelper)

            'If there is not y axis then add on.
            If m_aryAxisList.Count = 0 Then
                Dim aryAxis As New AnimatGUI.DataObjects.Charting.Axis(Me)
                aryAxis.Name = "Y Axis 1"
                'Setting the name adds it to the list
            End If

            'Lets find the first axis object.
            Dim doAxis As AnimatGUI.DataObjects.Charting.Axis = DirectCast(m_aryAxisList.GetItem(0), AnimatGUI.DataObjects.Charting.Axis)
            doAxis.DroppedDragData(doDrag)
        End Sub

        Protected Overridable Function FindAvailableWorkingAxis() As Integer

            Dim aryAxis(5) As Boolean

            Dim doAxis As AnimatGUI.DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Charting.Axis)
                If doAxis.WorkingAxis >= 0 AndAlso doAxis.WorkingAxis <= 5 Then
                    aryAxis(doAxis.WorkingAxis) = True
                End If
            Next

            For iAxis As Integer = 0 To 5
                If Not aryAxis(iAxis) Then
                    Return iAxis
                End If
            Next

            Throw New System.Exception("No available working axis was found.")

        End Function

        Public Overridable Function FindWorkingAxis(ByVal iAxis As Integer) As AnimatGUI.DataObjects.Charting.Axis

            Dim doAxis As AnimatGUI.DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Charting.Axis)
                If doAxis.WorkingAxis = iAxis Then
                    Return doAxis
                End If
            Next

            Return Nothing
        End Function

        Protected Overridable Sub RemoveUnusedAxis()

            Dim aryRemove As New Collection
            Dim doAxis As AnimatGUI.DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Charting.Axis)

                If Not doAxis.IsValidAxis Then
                    aryRemove.Add(doAxis)
                End If
            Next

            For Each doAxis In aryRemove
                doAxis.DeleteAxis(False)
            Next
        End Sub

        Public Overridable Sub ResetCollectDataInterval()
            Dim dblVal As Double = CalculateCollectDataInterval()
            Dim snVal As New ScaledNumber(Me.m_doFormHelper)
            snVal.SetFromValue(dblVal)
            Me.CollectDataInterval = snVal
        End Sub

        Protected Overridable Function CalculateCollectDataInterval() As Double

            Dim dblTimeStep As Double = -1
            Dim dblTemp As Double

            Dim doAxis As AnimatGUI.DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Charting.Axis)

                dblTemp = doAxis.TimeStep()
                If dblTimeStep <= 0 OrElse dblTemp < dblTimeStep Then
                    dblTimeStep = dblTemp
                End If
            Next

            Return dblTimeStep
        End Function

        Public Overridable Function ExportDataFilename(Optional ByVal strPrefix As String = "", Optional ByVal strPath As String = "") As String
            If strPath.Trim.Length > 0 Then
                Return strPath & "\" & strPrefix & Me.Title & ".txt"
            Else
                Return Util.Application.ProjectPath & strPrefix & Me.Title & ".txt"
            End If
        End Function

        Public Overridable Function ExportDataFilenameWithPhysicsEngine(ByVal strPhysicsEngine As String, ByVal bTestIfExists As Boolean, Optional ByVal strPrefix As String = "", Optional ByVal strPath As String = "") As String
            Dim strFile As String = ""
            Dim strFileWithPhysics As String = ""
            If strPath.Trim.Length > 0 Then
                strFile = strPath & "\" & strPrefix & Me.Title & ".txt"
                strFileWithPhysics = strPath & "\" & strPhysicsEngine & "_" & strPrefix & Me.Title & ".txt"
            Else
                strFile = Util.Application.ProjectPath & strPrefix & Me.Title & ".txt"
                strFileWithPhysics = Util.Application.ProjectPath & strPhysicsEngine & "_" & strPrefix & Me.Title & ".txt"
            End If

            If bTestIfExists Then
                If System.IO.File.Exists(strFileWithPhysics) Then
                    Return strFileWithPhysics
                Else
                    Return strFile
                End If
            Else
                'If the physics engine is vortex then do not add it as a prefix because default data files without a physics engine are vortex files.
                If strPhysicsEngine.Trim.Length > 0 AndAlso Not strPhysicsEngine = "Vortex" Then
                    Return strFileWithPhysics
                Else
                    Return strFile
                End If
            End If
        End Function

        Public Overridable Function ExportChartData(Optional ByVal strFile As String = "", Optional ByVal strPrefix As String = "") As Boolean

        End Function

        Public Overridable Sub CopyChartData(ByVal strPhysicsEngine As String, Optional ByVal strPath As String = "", Optional ByVal strPrefix As String = "")
            'If the file already exists then delete it.
            Dim strFile As String = Me.ExportDataFilenameWithPhysicsEngine(strPhysicsEngine, False, strPrefix, strPath)

            If System.IO.File.Exists(strFile) Then
                System.IO.File.Delete(strFile)
            End If

            'Now copy the other file over.
            Dim strFrom As String = Me.ExportDataFilename("")
            Dim strTo As String = strFile
            System.IO.File.Copy(strFrom, strTo, True)
        End Sub

        Public Overridable Sub CompareExportedData(ByVal strPhysicsEngine As String, ByVal strPrefix As String, ByVal strTemplatePath As String, _
                                                   ByVal aryMaxErrors As Hashtable, ByVal iMaxRows As Integer, ByVal aryIgnoreRows As ArrayList)

            'Lets try and load the original file and then the new test file.
            Dim aryTemplateColumns() As String
            Dim aryTestColumns() As String
            Dim aryTemplateData(,) As Double
            Dim aryTestData(,) As Double

            'Load the test file data.
            Dim strTestfile As String = Me.ExportDataFilename()
            Util.ReadCSVFileToArray(strTestfile, aryTestColumns, aryTestData)

            If aryTestColumns Is Nothing OrElse aryTestData Is Nothing Then
                Throw New System.Exception("Could not read the test file. ('" & strTestfile & "')")
            End If

            'Load the template file data using the test columns as the template to make sure we 
            'get only the comparable data columns.
            Dim strTempFile As String = Me.ExportDataFilenameWithPhysicsEngine(strPhysicsEngine, True, strPrefix, strTemplatePath)
            Util.ReadCSVFileToArrayUsingTemplate(strTempFile, aryTestColumns, aryTemplateColumns, aryTemplateData)

            If aryTemplateColumns Is Nothing OrElse aryTemplateData Is Nothing Then
                Throw New System.Exception("Could not read the template file. ('" & strTempFile & "')")
            End If

            'Now lets compare the column names. We need to have the same number and they should match identically.
            CompareExportDataColumns(aryTemplateColumns, aryTestColumns)

            'Then Compare data itself. It should be the same amount of data and each entry should be within the 
            'maximum error when comparing between template and test.
            CompareExportedDataRows(strPrefix, aryTemplateColumns, aryTemplateData, aryTestData, aryMaxErrors, iMaxRows, aryIgnoreRows)

        End Sub

        Protected Overridable Sub CompareExportDataColumns(ByVal aryTemplateColumns() As String, ByVal aryTestColumns() As String)

            'First check the number of columns. If they mismatch then there is an obvious problem.
            If (aryTemplateColumns.Length <> aryTestColumns.Length) Then
                Throw New System.Exception("The template and test columns do not match for file: " & Me.ExportDataFilename)
            End If

            Dim iCols As Integer = aryTestColumns.Length - 1
            For iCol As Integer = 0 To iCols
                If aryTemplateColumns(iCol).ToUpper.Trim <> aryTestColumns(iCol).ToUpper.Trim Then
                    Throw New System.Exception("A template and test column does not match for file: '" & Me.ExportDataFilename & _
                                               "', Template Column: '" & aryTemplateColumns(iCol) & "', Test Column: '" & aryTestColumns(iCol) & "'")
                End If
            Next

        End Sub

        Protected Overridable Sub CompareExportedDataRows(ByVal strPrefix As String, ByVal aryTemplateColumns() As String, ByVal aryTemplateData(,) As Double, ByVal aryTestData(,) As Double, ByVal aryMaxErrors As Hashtable, ByVal iMaxRows As Integer, ByVal aryIgnoreRows As ArrayList)

            'First check to make sure the number of rows match.

            Dim iTemplate As Integer = UBound(aryTemplateData, 2)
            Dim iTest As Integer = UBound(aryTestData, 2)
            If (iTemplate <> iTest) Then
                Throw New System.Exception("The template and test row counts do not match for file: " & Me.ExportDataFilename & ". Template: " & iTemplate & ", Test: " & iTest)
            End If

            Dim iCols As Integer = UBound(aryTemplateData, 1)
            Dim iRows As Integer = UBound(aryTemplateData, 2)
            If iMaxRows > 0 Then iRows = Math.Min(iRows, iMaxRows)

            Dim dblMaxError As Double = 0.01

            For iCol As Integer = 0 To iCols

                dblMaxError = GetMaxError(aryTemplateColumns(iCol), aryMaxErrors)

                For iRow As Integer = 0 To iRows
                    If Not IgnoreRow(iRow, aryIgnoreRows) Then
                        If Math.Abs(aryTemplateData(iCol, iRow) - aryTestData(iCol, iRow)) > dblMaxError Then
                            Throw New System.Exception("Data mismatch for test: " & strPrefix & " file: '" & Me.ExportDataFilename & _
                                                       "', Column: '" & aryTemplateColumns(iCol) & "', row: " & iRow & ", Template Value: '" & _
                                                       aryTemplateData(iCol, iRow) & "', Test Data: '" & aryTestData(iCol, iRow) & "'")
                        End If
                    End If
                Next

            Next
        End Sub

        Protected Overridable Function IgnoreRow(ByVal iRow As Integer, ByVal aryIgnoreRows As ArrayList) As Boolean

            For Each ptPoint As Point In aryIgnoreRows
                If iRow >= ptPoint.X AndAlso iRow <= ptPoint.Y Then
                    Return True
                End If
            Next

            Return False
        End Function

        Protected Function GetMaxError(ByVal strColName As String, ByVal aryMaxErrors As Hashtable) As Double
            Dim dblMaxError As Double = 0.05

            If aryMaxErrors.ContainsKey(strColName) Then
                dblMaxError = CDbl(aryMaxErrors(strColName))
            ElseIf aryMaxErrors.ContainsKey("default") Then
                dblMaxError = CDbl(aryMaxErrors("default"))
            End If

            Return dblMaxError
        End Function

        Public Overridable Sub UpdateChartConfiguration(ByVal bReconfigureData As Boolean)

        End Sub

        Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean
            If Not Me.ToolHolder Is Nothing Then
                Return Me.ToolHolder.Delete(bAskToDelete, e)
            End If
        End Function

        Public Overrides Function Clone() As ToolForm

        End Function

        Protected Overrides Sub LoadExternalData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadExternalData(oXml)

            oXml.IntoElem() 'Into Form Element

            m_bAutoCollectDataInterval = oXml.GetChildBool("AutoCollectInterval", False)
            m_snCollectDataInterval.LoadData(oXml, "CollectDataInterval")
            m_snCollectTimeWindow.LoadData(oXml, "CollectTimeWindow")

            If ScaledNumber.IsValidXml(oXml, "UpdateDataInterval") Then
                m_snUpdateDataInterval.LoadData(oXml, "UpdateDataInterval")
            Else
                m_snUpdateDataInterval.SetFromValue((oXml.GetChildFloat("UpdateDataInterval", 200) * 0.001), ScaledNumber.enumNumericScale.milli) 'It is in milliseconds
            End If

            m_bSetStartEndTime = oXml.GetChildBool("SetStartEndTime", False)
            If oXml.FindChildElement("CollectStartTime", False) Then
                m_snCollectStartTime.LoadData(oXml, "CollectStartTime")
                m_snCollectEndTime.LoadData(oXml, "CollectEndTime")
            End If

            'Load the axis information
            m_aryAxisList.Clear()
            If oXml.FindChildElement("AxisList", False) Then
                oXml.IntoChildElement("AxisList")

                Dim doAxis As DataObjects.Charting.Axis
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    doAxis = DirectCast(Util.LoadClass(oXml, iIndex, Me.FormHelper), AnimatGUI.DataObjects.Charting.Axis)
                    doAxis.ParentChart = Me
                    doAxis.LoadData(oXml)
                    m_aryAxisList.Add(doAxis.Name, doAxis)
                Next
                oXml.OutOfElem()  'Outof InLinks Element
            End If

            oXml.OutOfElem() 'Outof Form Element

        End Sub

        Protected Overrides Sub SaveExternalData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveExternalData(oXml)

            oXml.IntoElem()

            oXml.AddChildElement("AutoCollectInterval", m_bAutoCollectDataInterval)
            m_snCollectDataInterval.SaveData(oXml, "CollectDataInterval")
            m_snCollectTimeWindow.SaveData(oXml, "CollectTimeWindow")
            m_snUpdateDataInterval.SaveData(oXml, "UpdateDataInterval")

            oXml.AddChildElement("SetStartEndTime", m_bSetStartEndTime)
            m_snCollectStartTime.SaveData(oXml, "CollectStartTime")
            m_snCollectEndTime.SaveData(oXml, "CollectEndTime")

            'Save axis list
            oXml.AddChildElement("AxisList")
            oXml.IntoElem()

            Dim doAxis As DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, DataObjects.Charting.Axis)
                doAxis.SaveData(oXml)
            Next
            oXml.OutOfElem()

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            MyBase.InitializeSimulationReferences(bShowError)

            Dim doAxis As DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, DataObjects.Charting.Axis)
                doAxis.InitializeSimulationReferences(bShowError)
            Next

        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            Dim doAxis As DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, DataObjects.Charting.Axis)
                doAxis.InitializeAfterLoad()
            Next

        End Sub

        Protected Overridable Sub AddHandlers()

            If Not m_bAddedHandlers Then
                AddHandler Util.Application.TimeStepChanged, AddressOf Me.OnTimeStepChanged
                m_bAddedHandlers = True
            End If

        End Sub

        Protected Overridable Sub RemoveHandlers()

            If m_bAddedHandlers Then
                RemoveHandler Util.Application.TimeStepChanged, AddressOf Me.OnTimeStepChanged
                m_bAddedHandlers = False
            End If

        End Sub

#End Region

#Region " Events "

        Protected Overridable Sub OnAddAxis(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim iIndex As Integer = Util.ExtractIDCount("Y Axis", m_aryAxisList, " ") + 1
                Dim strName As String = "Y Axis " & iIndex

                Dim doAxis As New AnimatGUI.DataObjects.Charting.Axis(Me)
                doAxis.Name = strName
                doAxis.CreateWorkspaceTreeView(Me.FormHelper, Me.WorkspaceNode)
                doAxis.SelectItem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub OnClearCharts(ByVal sender As Object, ByVal e As System.EventArgs)
        End Sub

        Protected Sub OnTimeStepChanged(ByVal doObject As Framework.DataObject)
            If Me.AutoCollectDataInterval Then ResetCollectDataInterval()
        End Sub

        'Protected Overrides Sub AnimatForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        '    MyBase.AnimatForm_FormClosing(sender, e)

        '    If Not e.Cancel Then
        '        If Util.Application.LastSelectedChart Is Me Then
        '            Util.Application.LastSelectedChart = Nothing
        '        End If

        '        RemoveHandlers()

        '        If Util.Application.SimulationInterface.FindItem(m_strID, False) Then
        '            Util.Application.SimulationInterface.RemoveItem("Simulator", "DataChart", m_strID, True)
        '        End If
        '    End If

        'End Sub

        Protected Overrides Sub OnFormClosing(ByVal e As System.Windows.Forms.FormClosingEventArgs)
            MyBase.OnFormClosing(e)

            Try
                If Not e.Cancel Then
                    If Util.Application.LastSelectedChart Is Me Then
                        Util.Application.LastSelectedChart = Nothing
                    End If

                    RemoveHandlers()

                    If Util.Application.SimulationInterface.FindItem(m_strID, False) Then
                        Util.Application.SimulationInterface.RemoveItem("Simulator", "DataChart", m_strID, True)
                    End If
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
