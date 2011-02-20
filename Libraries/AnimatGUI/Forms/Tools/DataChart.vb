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
        'protected m_dcChart as 

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

#End Region

#Region " Properties "

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
                Util.ProjectProperties.RefreshProperties()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property CollectDataInterval() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snCollectDataInterval
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

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
                Util.ProjectProperties.RefreshProperties()
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

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, ByVal doParentNode As Crownwood.DotNetMagic.Controls.Node)
            MyBase.CreateWorkspaceTreeView(doParent, doParentNode)

            Dim doAxis As DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, DataObjects.Charting.Axis)
                doAxis.CreateWorkspaceTreeView(Me.FormHelper, Me.WorkspaceNode)
            Next

        End Sub

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

            If tnSelectedNode Is Me.WorkspaceNode Then
                ' Create the menu items
                Dim mcDelete As New MenuCommand("Delete Chart", "DeleteChart", Util.Application.ToolStripImages.ImageList, _
                                                  Util.Application.ToolStripImages.GetImageIndex("AnimatGUI.Delete.gif"), _
                                                  New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))
                Dim mcAddAxis As New MenuCommand("Add Axis", "AddAxis", Util.Application.ToolStripImages.ImageList, _
                                                  Util.Application.ToolStripImages.GetImageIndex("AnimatGUI.AddChartAxis.gif"), _
                                                  New EventHandler(AddressOf Me.OnAddAxis))
                Dim mcClearCharts As New MenuCommand("Clear Charts", "ClearCharts", _
                                                  New EventHandler(AddressOf Me.OnClearCharts))

                Dim mcSepExpand As MenuCommand = New MenuCommand("-")
                Dim mcExpandAll As New MenuCommand("Expand All", tnSelectedNode, _
                                                  New EventHandler(AddressOf Me.OnExpandAll))
                Dim mcCollapseAll As New MenuCommand("Collapse All", tnSelectedNode, _
                                                  New EventHandler(AddressOf Me.OnCollapseAll))

                mcExpandAll.ImageList = Util.Application.ToolStripImages.ImageList
                mcExpandAll.ImageIndex = Util.Application.ToolStripImages.GetImageIndex("AnimatGUI.Expand.gif")
                mcCollapseAll.ImageList = Util.Application.ToolStripImages.ImageList
                mcCollapseAll.ImageIndex = Util.Application.ToolStripImages.GetImageIndex("AnimatGUI.Collapse.gif")

                ' Create the popup menu object
                Dim popup As New PopupMenu
                popup.MenuCommands.Add(mcDelete)
                If m_aryAxisList.Count < 6 Then
                    popup.MenuCommands.Add(mcAddAxis)
                End If
                popup.MenuCommands.Add(mcClearCharts)
                popup.MenuCommands.AddRange(New MenuCommand() {mcSepExpand, mcExpandAll, mcCollapseAll})

                ' Show it!
                Dim selected As MenuCommand = popup.TrackPopup(ptPoint)

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

            Dim dblTimeStep As Double = Util.Environment.PhysicsTimeStep.Value
            Dim dblTemp As Double

            Dim doAxis As AnimatGUI.DataObjects.Charting.Axis
            For Each deEntry As DictionaryEntry In m_aryAxisList
                doAxis = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Charting.Axis)

                dblTemp = doAxis.TimeStep()
                If dblTemp < dblTimeStep Then
                    dblTimeStep = dblTemp
                End If
            Next

            Return dblTimeStep
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

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

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

                Dim strAssemblyFile As String = ""
                Dim strClassName As String = ""

                Dim doAxis As DataObjects.Charting.Axis
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)

                    oXml.IntoElem()
                    strAssemblyFile = oXml.GetChildString("AssemblyFile")
                    strClassName = oXml.GetChildString("ClassName")
                    oXml.OutOfElem()

                    doAxis = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me.FormHelper), AnimatGUI.DataObjects.Charting.Axis)
                    doAxis.ParentChart = Me
                    doAxis.LoadData(oXml)
                    m_aryAxisList.Add(doAxis.Name, doAxis)
                Next
                oXml.OutOfElem()  'Outof InLinks Element
            End If

            oXml.OutOfElem() 'Outof Form Element

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.SaveData(oXml)

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

#End Region

    End Class

End Namespace
