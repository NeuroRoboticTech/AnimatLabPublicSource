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

    Public Class Pro2DColumn
        Inherits AnimatGUI.DataObjects.Charting.DataColumn

#Region " Enums "

        Public Enum enumLineTypes
            ThinSolid
            Dash
            Dot
            DashDot
            DashDotDot
            MediumSolid
            ThickSolid
            MediumThinSolid
            MediumThickSolid
            ExtraThinSolid
            ExtraThickSolid
        End Enum

        Public Enum enumPointTypes
            Plus
            Cross
            Dot
            DotSolid
            Square
            SquareSolid
            Diamond
            DiamondSolid
            UpTriangle
            UpTriangleSolid
            DownTriangle
            DownTriangleSolid
            Dash
            Pixel
            ArrowN
            ArrowNE
            ArrowE
            ArrowSE
            ArrowS
            ArrowSW
            ArrowW
            ArrowNW
        End Enum

        Public Enum enumPlotMethodTypes
            Line = 0
            Point = 1
            Bar = 2
            PointsPlusBestFitLine = 8
            Spline = 16
            PointsPlusLine = 17
        End Enum

#End Region

#Region " Attributes "

        Protected m_frmLineChart As LicensedAnimatGUI.Forms.Charts.LineChart

        Protected m_eLineType As enumLineTypes
        Protected m_ePointType As enumPointTypes
        Protected m_ePlotMethodType As enumPlotMethodTypes
        Protected m_clLineColor As System.Drawing.Color
        Protected Const m_iNullValue As Integer = -99999

#End Region

#Region " Properties "

        Public Overrides Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal Value As String)
                MyBase.Name = Value

                If Not Me.SelectionInProgress Then
                    If Me.DataSubSet >= 0 AndAlso Not m_frmParentAxis Is Nothing AndAlso Not m_frmParentAxis.ParentChart Is Nothing Then
                        Dim ctrlGraph As Gigasoft.ProEssentials.Pesgo = DirectCast(m_frmParentAxis.ParentChart.Chart, Gigasoft.ProEssentials.Pesgo)
                        ctrlGraph.PeString.SubsetLabels(m_iDataSubSet) = Me.Name
                        ctrlGraph.PeFunction.ReinitializeResetImage()
                        m_frmParentAxis.ParentChart.Refresh()
                    End If
                End If

            End Set
        End Property

        Public Overridable ReadOnly Property LineChart() As LicensedAnimatGUI.Forms.Charts.LineChart
            Get
                Return m_frmLineChart
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property LineType() As enumLineTypes
            Get
                Return m_eLineType
            End Get
            Set(ByVal Value As enumLineTypes)
                m_eLineType = Value

                If Not Me.SelectionInProgress Then
                    m_frmLineChart.UpdateChartConfiguration(False)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property PointType() As enumPointTypes
            Get
                Return m_ePointType
            End Get
            Set(ByVal Value As enumPointTypes)
                m_ePointType = Value

                If Not Me.SelectionInProgress Then
                    m_frmLineChart.UpdateChartConfiguration(False)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property PlotMethodType() As enumPlotMethodTypes
            Get
                Return m_ePlotMethodType
            End Get
            Set(ByVal Value As enumPlotMethodTypes)
                m_ePlotMethodType = Value

                If Not Me.SelectionInProgress Then
                    m_frmLineChart.UpdateChartConfiguration(False)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property LineColor() As System.Drawing.Color
            Get
                Return m_clLineColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_clLineColor = Value

                If Not m_frmLineChart Is Nothing AndAlso Not Me.SelectionInProgress Then
                    If Not Me.m_frmParentAxis Is Nothing Then
                        If Me.m_frmParentAxis.DataColumns.Count > 0 Then
                            m_frmLineChart.UpdateChartConfiguration(False)
                        End If
                    End If
                End If
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Dim doAxis As DataObjects.Charting.Pro2DAxis = DirectCast(doParent, DataObjects.Charting.Pro2DAxis)
            m_frmLineChart = DirectCast(doAxis.ParentChart, LicensedAnimatGUI.Forms.Charts.LineChart)

            m_eLineType = enumLineTypes.MediumSolid
            m_clLineColor = m_frmLineChart.NextAutoFillColor
            m_ePlotMethodType = enumPlotMethodTypes.Line
        End Sub

        Public Overrides Sub UpdateChartConfiguration(ByRef iSubSet As Integer)

            Dim ctrlGraph As Gigasoft.ProEssentials.Pesgo = DirectCast(m_frmParentAxis.ParentChart.Chart, Gigasoft.ProEssentials.Pesgo)

            Me.DataSubSet = iSubSet
            ctrlGraph.PeString.SubsetLabels(iSubSet) = Me.Name
            ctrlGraph.PeColor.SubsetColors(iSubSet) = m_clLineColor
            ctrlGraph.PeLegend.SubsetLineTypes(iSubSet) = CType(m_eLineType, Gigasoft.ProEssentials.Enums.LineType)
            ctrlGraph.PeLegend.SubsetPointTypes(iSubSet) = CType(m_ePointType, Gigasoft.ProEssentials.Enums.PointType)
            ctrlGraph.PePlot.Method = CType(m_ePlotMethodType, Gigasoft.ProEssentials.Enums.SGraphPlottingMethod)

            iSubSet = iSubSet + 1

            SetSimData("ColumnIndex", Me.DataSubSet.ToString, True)

            'Debug.WriteLine(Me.Name & " DataSubSet: " & Me.DataSubSet & "  PrevSubset: " & Me.PrevDataSubSet)
        End Sub

        Public Overrides Sub ReconfigureChartData(ByRef aryOldX(,) As Single, ByRef aryOldY(,) As Single, ByRef aryNewX(,) As Single, ByRef aryNewY(,) As Single)

            'If this datasubset value has changed then lets move the data to the new position.
            'If Me.DataSubSet <> Me.PrevDataSubSet AndAlso Me.PrevDataSubSet >= 0 Then
            Dim iPoints As Integer = m_frmLineChart.ctrlGraph.PeData.Points
            Dim isize As Integer = UBound(aryNewY, 1)
            isize = UBound(aryNewY, 2)

            If Me.PrevDataSubSet >= 0 AndAlso Me.PrevDataSubSet < m_frmLineChart.ctrlGraph.PeData.Subsets Then
                Dim iOldSourceIndex As Integer = Me.PrevDataSubSet * iPoints
                Dim iNewSourceIndex As Integer = Me.DataSubSet * iPoints
                Dim iOldXSize As Integer = ((UBound(aryOldX, 1) + 1) * (UBound(aryOldX, 2) + 1))
                Dim iNewXSize As Integer = ((UBound(aryNewX, 1) + 1) * (UBound(aryNewX, 2) + 1))
                Dim iOldYSize As Integer = ((UBound(aryOldY, 1) + 1) * (UBound(aryOldY, 2) + 1))
                Dim iNewYSize As Integer = ((UBound(aryNewY, 1) + 1) * (UBound(aryNewY, 2) + 1))

                If iOldXSize >= iOldSourceIndex AndAlso iOldXSize >= (iOldSourceIndex + iPoints - 1) AndAlso _
                   iNewXSize >= iNewSourceIndex AndAlso iNewXSize >= (iNewSourceIndex + iPoints - 1) AndAlso _
                   iOldYSize >= iOldSourceIndex AndAlso iOldYSize >= (iOldSourceIndex + iPoints - 1) AndAlso _
                   iNewYSize >= iNewSourceIndex AndAlso iNewYSize >= (iNewSourceIndex + iPoints - 1) Then

                    Array.Copy(aryOldX, iOldSourceIndex, aryNewX, iNewSourceIndex, iPoints)
                    Array.Copy(aryOldY, iOldSourceIndex, aryNewY, iNewSourceIndex, iPoints)
                End If
            Else
                'If PrevDataSubset is -1 then this is a new column. So we need to init it with 'Blank' data
                For iPoint As Integer = 0 To iPoints - 1
                    aryNewY(Me.DataSubSet, iPoint) = m_iNullValue
                Next
            End If
            'End If

        End Sub

        Public Overrides Function CreateDataColumn(ByVal doItem As AnimatGUI.DataObjects.DragObject, Optional ByVal bAutoAddToAxis As Boolean = True) As AnimatGUI.DataObjects.Charting.DataColumn
            Dim doColumn As New DataObjects.Charting.Pro2DColumn(Me.ParentAxis)

            Dim strName As String = doItem.ItemName

            doColumn.DataItem = doItem.DataColumnItem
            doColumn.AutoAddToAxis = bAutoAddToAxis
            doColumn.Name = strName
            Return doColumn
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Line Type", m_eLineType.GetType(), "LineType", _
                                        "Column Properties", "Sets the type of line to draw on the chart for this data column.", m_eLineType))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Point Type", m_ePointType.GetType(), "PointType", _
                                        "Column Properties", "Sets the type of point to draw on the line for this data column.", m_ePointType))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Plot Method Type", m_ePlotMethodType.GetType(), "PlotMethodType", _
                                        "Column Properties", "Sets the type of plotting method to draw on the line for this data column.", m_ePlotMethodType))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Line Color", m_clLineColor.GetType(), "LineColor", _
                                        "Column Properties", "Sets the color used to draw the line for this data column.", m_clLineColor))

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim p2dColum As Pro2DColumn = DirectCast(doOriginal, Pro2DColumn)

            m_frmLineChart = p2dColum.m_frmLineChart

            m_eLineType = p2dColum.m_eLineType
            m_ePointType = p2dColum.m_ePointType
            m_ePlotMethodType = p2dColum.m_ePlotMethodType
            m_clLineColor = p2dColum.m_clLineColor

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New Pro2DColumn(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            Return doItem
        End Function

        Public Overloads Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_eLineType = DirectCast([Enum].Parse(GetType(enumLineTypes), oXml.GetChildString("LineType"), True), enumLineTypes)
            m_ePointType = DirectCast([Enum].Parse(GetType(enumPointTypes), oXml.GetChildString("PointType"), True), enumPointTypes)
            m_ePlotMethodType = DirectCast([Enum].Parse(GetType(enumPlotMethodTypes), oXml.GetChildString("PlotMethodType", "Line"), True), enumPlotMethodTypes)
            m_clLineColor = Color.FromArgb(oXml.GetChildInt("LineColor"))

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            Dim iColor As Integer = m_clLineColor.ToArgb
            oXml.AddChildElement("LineType", m_eLineType.ToString)
            oXml.AddChildElement("PointType", m_ePointType.ToString)
            oXml.AddChildElement("PlotMethodType", m_ePlotMethodType.ToString)
            oXml.AddChildElement("LineColor", iColor)

            oXml.OutOfElem()

        End Sub

#End Region

#End Region

    End Class

End Namespace
