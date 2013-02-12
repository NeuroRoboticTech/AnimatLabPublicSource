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

    Public Class Pro2DAxis
        Inherits AnimatGUI.DataObjects.Charting.Axis


#Region " Enums "

        Public Enum enumScaleControl
            None
            Min
            Max
            MinMax
        End Enum

#End Region

#Region " Attributes "

        Protected m_frmParentLineChart As Forms.Charts.LineChart

        Protected m_AxisColor As System.Drawing.Color
        Protected m_eAutoScale As enumScaleControl
        Protected m_dblMin As Double
        Protected m_dblMax As Double
        Protected m_iAxisScaleValue As Integer

#End Region

#Region " Properties "

        Public Overrides Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal Value As String)
                MyBase.Name = Value

                If Me.WorkingAxis >= 0 AndAlso Not m_frmParentChart Is Nothing Then
                    Dim ctrlGraph As Gigasoft.ProEssentials.Pesgo = DirectCast(m_frmParentChart.Chart, Gigasoft.ProEssentials.Pesgo)
                    ctrlGraph.PeGrid.WorkingAxis = Me.WorkingAxis
                    ctrlGraph.PeString.YAxisLabel = Me.Name

                    ctrlGraph.PeFunction.ReinitializeResetImage()
                    m_frmParentChart.Refresh()
                End If

            End Set
        End Property

        Public Overrides Property ParentChart() As AnimatGUI.Forms.Tools.DataChart
            Get
                Return m_frmParentChart
            End Get
            Set(ByVal Value As AnimatGUI.Forms.Tools.DataChart)
                m_frmParentChart = Value
                m_frmParentLineChart = DirectCast(Value, Forms.Charts.LineChart)
            End Set
        End Property

        Public Overridable Property ParentLineChart() As Forms.Charts.LineChart
            Get
                Return m_frmParentLineChart
            End Get
            Set(ByVal Value As Forms.Charts.LineChart)
                m_frmParentLineChart = Value
            End Set
        End Property

        Public Overridable Property AxisColor() As System.Drawing.Color
            Get
                If Me.WorkingAxis >= 0 AndAlso Not m_frmParentLineChart Is Nothing Then
                    m_frmParentLineChart.ctrlGraph.PeGrid.WorkingAxis = Me.WorkingAxis
                    Return m_frmParentLineChart.ctrlGraph.PeColor.YAxis
                End If
                Return System.Drawing.Color.Black
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_AxisColor = Value
                If Me.WorkingAxis >= 0 AndAlso Not m_frmParentLineChart Is Nothing Then
                    m_frmParentLineChart.ctrlGraph.PeGrid.WorkingAxis = Me.WorkingAxis
                    m_frmParentLineChart.ctrlGraph.PeColor.YAxis = Value

                    m_frmParentLineChart.ctrlGraph.PeFunction.ReinitializeResetImage()
                    m_frmParentLineChart.Refresh()
                End If

            End Set
        End Property

        Public Overridable Property AutoScale() As enumScaleControl
            Get
                If Me.WorkingAxis >= 0 AndAlso Not m_frmParentLineChart Is Nothing Then
                    m_frmParentLineChart.ctrlGraph.PeGrid.WorkingAxis = Me.WorkingAxis
                    Return DirectCast([Enum].Parse(GetType(enumScaleControl), m_frmParentLineChart.ctrlGraph.PeGrid.Configure.ManualScaleControlY.ToString, True), enumScaleControl)
                End If
                Return enumScaleControl.None
            End Get
            Set(ByVal Value As enumScaleControl)

                m_eAutoScale = Value
                If Me.WorkingAxis >= 0 AndAlso Not m_frmParentLineChart Is Nothing Then
                    m_frmParentLineChart.ctrlGraph.PeGrid.Configure.ManualScaleControlY = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.ManualScaleControl), Value.ToString, True), Gigasoft.ProEssentials.Enums.ManualScaleControl)

                    m_frmParentLineChart.ctrlGraph.PeFunction.ReinitializeResetImage()
                    m_frmParentLineChart.Refresh()
                End If

            End Set
        End Property

        Public Overridable Property Min() As Double
            Get
                If Me.WorkingAxis >= 0 AndAlso Not m_frmParentLineChart Is Nothing Then
                    m_frmParentLineChart.ctrlGraph.PeGrid.WorkingAxis = Me.WorkingAxis
                    Return m_frmParentLineChart.ctrlGraph.PeGrid.Configure.ManualMinY
                End If
                Return 0
            End Get
            Set(ByVal Value As Double)

                m_dblMin = Value
                If Me.WorkingAxis >= 0 AndAlso Not m_frmParentLineChart Is Nothing Then

                    m_frmParentLineChart.ctrlGraph.PeGrid.Configure.ManualMinY = Value

                    m_frmParentLineChart.ctrlGraph.PeFunction.ReinitializeResetImage()
                    m_frmParentLineChart.Refresh()
                End If

            End Set
        End Property

        Public Overridable Property Max() As Double
            Get
                If Me.WorkingAxis >= 0 AndAlso Not m_frmParentLineChart Is Nothing Then
                    m_frmParentLineChart.ctrlGraph.PeGrid.WorkingAxis = Me.WorkingAxis
                    Return m_frmParentLineChart.ctrlGraph.PeGrid.Configure.ManualMaxY
                End If
                Return 0
            End Get
            Set(ByVal Value As Double)

                m_dblMax = Value
                If Me.WorkingAxis >= 0 AndAlso Not m_frmParentLineChart Is Nothing Then

                    m_frmParentLineChart.ctrlGraph.PeGrid.Configure.ManualMaxY = Value

                    m_frmParentLineChart.ctrlGraph.PeFunction.ReinitializeResetImage()
                    m_frmParentLineChart.Refresh()
                End If

            End Set
        End Property

        Public Overridable Property AxisScaleValue() As Integer
            Get
                If Me.WorkingAxis >= 0 AndAlso Not m_frmParentLineChart Is Nothing Then
                    m_frmParentLineChart.ctrlGraph.PeGrid.WorkingAxis = Me.WorkingAxis
                    Return m_frmParentLineChart.ctrlGraph.PeData.ScaleForYData
                End If
                Return 0
            End Get
            Set(ByVal Value As Integer)

                m_iAxisScaleValue = Value
                If Me.WorkingAxis >= 0 AndAlso Not m_frmParentLineChart Is Nothing Then

                    m_frmParentLineChart.ctrlGraph.PeData.ScaleForYData = Value

                    m_frmParentLineChart.ctrlGraph.PeFunction.ReinitializeResetImage()
                    m_frmParentLineChart.Refresh()
                End If

            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Sub New(ByVal frmChart As AnimatGUI.Forms.Tools.DataChart)
            MyBase.New(frmChart)
            m_frmParentLineChart = DirectCast(frmChart, Forms.Charts.LineChart)
        End Sub

        Public Overrides Function CreateDataColumn(ByVal doItem As AnimatGUI.DataObjects.DragObject, Optional ByVal bAutoAddToAxis As Boolean = True) As AnimatGUI.DataObjects.Charting.DataColumn
            Dim doColumn As New DataObjects.Charting.Pro2DColumn(Me)

            Dim strName As String = doItem.ItemName
            doColumn.DataItem = doItem.DataColumnItem
            doColumn.AutoAddToAxis = bAutoAddToAxis
            doColumn.Name = strName
            Return doColumn
        End Function

        Protected Overloads Overrides Sub DroppedItem(ByVal doItem As AnimatGUI.DataObjects.DragObject)
            Dim doColumn As AnimatGUI.DataObjects.Charting.DataColumn = CreateDataColumn(doItem)
            doColumn.AutoAddToAxis = True
            Me.DataColumns.Add(doColumn.ID, doColumn)
            doColumn.Name = doColumn.Name

            If doColumn.LineColor.Name = m_frmParentChart.NextAutoFillColor.Name Then
                m_frmParentChart.IncrementAutoFillColor()
            End If

            doColumn.SelectItem()

            'If we are automatically setting the collect data interval then lets recalculate the value
            If Me.ParentChart.AutoCollectDataInterval Then
                Me.ParentChart.ResetCollectDataInterval()
            End If
        End Sub

        Protected Overloads Overrides Sub DroppedItem(ByVal doColumn As AnimatGUI.DataObjects.Charting.DataColumn)
            doColumn.AutoAddToAxis = True
            Me.DataColumns.Add(doColumn.ID, doColumn)
            doColumn.Name = doColumn.Name

            If doColumn.LineColor.Name = m_frmParentChart.NextAutoFillColor.Name Then
                m_frmParentChart.IncrementAutoFillColor()
            End If

            doColumn.SelectItem()

            'If we are automatically setting the collect data interval then lets recalculate the value
            If Me.ParentChart.AutoCollectDataInterval Then
                Me.ParentChart.ResetCollectDataInterval()
            End If
        End Sub

        Public Overrides Sub InitializeChartData()

            If Me.WorkingAxis >= 0 AndAlso Not m_frmParentLineChart Is Nothing Then
                m_frmParentLineChart.ctrlGraph.PeGrid.WorkingAxis = Me.WorkingAxis
                m_AxisColor = m_frmParentLineChart.ctrlGraph.PeColor.YAxis
                m_eAutoScale = DirectCast([Enum].Parse(GetType(enumScaleControl), m_frmParentLineChart.ctrlGraph.PeGrid.Configure.ManualScaleControlY.ToString, True), enumScaleControl)
                m_dblMin = m_frmParentLineChart.ctrlGraph.PeGrid.Configure.ManualMinY
                m_dblMax = m_frmParentLineChart.ctrlGraph.PeGrid.Configure.ManualMaxY
                m_iAxisScaleValue = m_frmParentLineChart.ctrlGraph.PeData.ScaleForYData

                m_frmParentLineChart.ctrlGraph.PeGrid.WorkingAxis = 0
            End If

        End Sub

        Public Overrides Sub UpdateChartConfiguration(ByRef iSubSet As Integer)

            Dim ctrlGraph As Gigasoft.ProEssentials.Pesgo = DirectCast(m_frmParentChart.Chart, Gigasoft.ProEssentials.Pesgo)
            ctrlGraph.PeGrid.MultiAxesSubsets(Me.WorkingAxis) = Me.DataColumns.Count

            ctrlGraph.PeGrid.WorkingAxis = Me.WorkingAxis
            ctrlGraph.PeString.YAxisLabel = Me.Name
            m_frmParentLineChart.ctrlGraph.PeColor.YAxis = m_AxisColor
            m_frmParentLineChart.ctrlGraph.PeGrid.Configure.ManualScaleControlY = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.ManualScaleControl), m_eAutoScale.ToString, True), Gigasoft.ProEssentials.Enums.ManualScaleControl)
            m_frmParentLineChart.ctrlGraph.PeGrid.Configure.ManualMinY = m_dblMin
            m_frmParentLineChart.ctrlGraph.PeGrid.Configure.ManualMaxY = m_dblMax
            m_frmParentLineChart.ctrlGraph.PeData.ScaleForYData = m_iAxisScaleValue

            Dim doColumn As AnimatGUI.DataObjects.Charting.DataColumn
            For Each deColumn As DictionaryEntry In Me.DataColumns
                doColumn = DirectCast(deColumn.Value, AnimatGUI.DataObjects.Charting.DataColumn)
                doColumn.UpdateChartConfiguration(iSubSet)
            Next

        End Sub

        Public Overrides Sub ReconfigureChartData(ByRef aryOldX(,) As Single, ByRef aryOldY(,) As Single, ByRef aryNewX(,) As Single, ByRef aryNewY(,) As Single)

            Dim doColumn As AnimatGUI.DataObjects.Charting.DataColumn
            For Each deColumn As DictionaryEntry In Me.DataColumns
                doColumn = DirectCast(deColumn.Value, AnimatGUI.DataObjects.Charting.DataColumn)
                doColumn.ReconfigureChartData(aryOldX, aryOldY, aryNewX, aryNewY)
            Next

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Axis Color", GetType(System.Drawing.Color), "AxisColor", _
                                        "Axis Properties", "Determines the color of this axis.", Me.AxisColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Axis Scale", GetType(Integer), "AxisScaleValue", _
                                        "Axis Properties", "Determines the exponent to use when drawing the graph. If this is set to -9 then " & _
                                        "all the numeric values will be drawn with n standing for nano."))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("AutoScale", GetType(enumScaleControl), "AutoScale", _
                                        "Axis Properties", "Determines whether this axis is automatically scaled to fit the data."))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Min Value", GetType(Double), "Min", _
                                        "Axis Properties", "Sets the minimum size limit for data for this axis.", Me.Min))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Value", GetType(Double), "Max", _
                                        "Axis Properties", "Sets the maximum size limit for data for this axis.", Me.Max))

            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Chart Axis", m_iWorkingAxis.GetType(), "WorkingAxis", _
            '                            "Axis Properties", "Determines which axis on the chart that this one represents.", m_iWorkingAxis))

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim p2Axis As Pro2DAxis = DirectCast(doOriginal, Pro2DAxis)

            m_frmParentLineChart = p2Axis.m_frmParentLineChart
            m_AxisColor = p2Axis.m_AxisColor
            m_eAutoScale = p2Axis.m_eAutoScale
            m_dblMin = p2Axis.m_dblMin
            m_dblMax = p2Axis.m_dblMax
            m_iAxisScaleValue = p2Axis.m_iAxisScaleValue

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doAxis As New Pro2DAxis(doParent)
            doAxis.CloneInternal(Me, bCutData, doRoot)
            Return doAxis
        End Function

#End Region

#End Region

        Protected Overrides Sub OnAddDataItem(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Util.DisableDirtyFlags = True

                Dim frmSelectItem As New AnimatGUI.Forms.Tools.SelectDataItem

                frmSelectItem.ColumnType = New Pro2DColumn(Me)
                frmSelectItem.BuildTreeView()
                If frmSelectItem.ShowDialog(Me.ParentChart) = DialogResult.OK Then
                    Util.DisableDirtyFlags = False
                    Me.DroppedItem(frmSelectItem.DataColumn)
                    m_frmParentChart.UpdateChartConfiguration(True)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Util.DisableDirtyFlags = False
            End Try
        End Sub

        Public Overrides Sub AddNewDataItem(ByVal doItem As AnimatGUI.DataObjects.DragObject)

            Try
                Util.DisableDirtyFlags = True

                Dim pro As New Pro2DColumn(Me)
                Dim doColumn As AnimatGUI.DataObjects.Charting.DataColumn = pro.CreateDataColumn(doItem, False)
                Me.DroppedItem(doColumn)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Util.DisableDirtyFlags = False
            End Try
        End Sub

    End Class

End Namespace
