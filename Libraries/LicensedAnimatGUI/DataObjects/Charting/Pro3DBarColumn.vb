Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatTools.Framework

Namespace DataObjects.Charting

    Public Class Pro3DBarColumn
        Inherits AnimatTools.DataObjects.Charting.DataColumn


#Region " Attributes "

        Protected m_frmChart As LicensedAnimatTools.Forms.Charts.DataArray

        Protected m_clLineColor As System.Drawing.Color
        Protected Const m_iNullValue As Integer = -99999

        Protected m_iColumn As Integer
        Protected m_iRow As Integer

#End Region

#Region " Properties "

        Public Overrides Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal Value As String)
                MyBase.Name = Value

                If Me.DataSubSet >= 0 AndAlso Not m_frmParentAxis Is Nothing AndAlso Not m_frmParentAxis.ParentChart Is Nothing Then
                    Dim ctrlGraph As Gigasoft.ProEssentials.Pesgo = DirectCast(m_frmParentAxis.ParentChart.Chart, Gigasoft.ProEssentials.Pesgo)
                    ctrlGraph.PeString.SubsetLabels(m_iDataSubSet) = Me.Name
                    ctrlGraph.PeFunction.ReinitializeResetImage()
                    m_frmParentAxis.ParentChart.Refresh()
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
                'm_frmLineChart.UpdateChartConfiguration(False)
            End Set
        End Property

        Public Overridable Property ParentChart() As LicensedAnimatTools.Forms.Charts.DataArray
            Get
                Return m_frmChart
            End Get
            Set(ByVal Value As LicensedAnimatTools.Forms.Charts.DataArray)
                m_frmChart = Value
            End Set
        End Property

        Public Overridable Property Column() As Integer
            Get
                Return m_iColumn
            End Get
            Set(ByVal Value As Integer)
                If (Value < 0) Then
                    Throw New System.Exception("Column value can not be less than zero.")
                End If
                m_iColumn = Value
            End Set
        End Property

        Public Overridable Property Row() As Integer
            Get
                Return m_iRow
            End Get
            Set(ByVal Value As Integer)
                If (Value < 0) Then
                    Throw New System.Exception("Row value can not be less than zero.")
                End If
                m_iRow = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)
            m_clLineColor = System.Drawing.Color.Red
        End Sub

        Public Overrides Function CreateDataColumn(ByVal doItem As AnimatTools.DataObjects.DragObject, Optional ByVal bAutoAddToAxis As Boolean = True) As AnimatTools.DataObjects.Charting.DataColumn
            Dim doColumn As New DataObjects.Charting.Pro3DBarColumn(Me.Parent)

            doColumn.DataItem = doItem.DataColumnItem
            doColumn.AutoAddToAxis = bAutoAddToAxis
            doColumn.Name = doItem.DataColumnItem.Name
            Return doColumn
        End Function

        Public Overrides Sub PrepareForCharting()

            If Util.Application.SimulationInterface.FindItem(Me.ParentChart.ID, False) Then
                If Not Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                    Util.DisableDirtyFlags = True
                    Dim strXml As String = Me.SaveDataColumnToXml()
                    Util.Application.SimulationInterface.AddItem(Me.ParentChart.ID, Me.ID, strXml, True)
                    'Util.Application.SimulationInterface.AddDataColumn(Me.ParentChart.ID, Me.ID, Me.ColumnModuleName, Me.ColumnClassType, strXml)

                    'If Not bPreparingChart Then
                    '    Me.ParentChart.UpdateChartConfiguration(True)
                    'End If

                    Util.DisableDirtyFlags = False
                End If
            End If
        End Sub

        Public Overrides Sub SaveDataColumnXml(ByRef oXml As AnimatTools.Interfaces.StdXml)
            Dim doItem As AnimatTools.DataObjects.DragObject = Me.DataItem

            If doItem Is Nothing Then
                Throw New System.Exception("No data item was defined that could be used to create a data column in the simulation.")
            End If

            oXml.IntoElem()
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("ColumnName", m_strName)
            oXml.AddChildElement("DataType", m_thDataType.ID.ToString())
            oXml.AddChildElement("Column", m_iColumn)
            oXml.AddChildElement("Row", m_iRow)
            oXml.OutOfElem()

            doItem.SaveDataColumnToXml(oXml)
        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Bar Color", m_clLineColor.GetType(), "LineColor", _
                                        "Column Properties", "Sets the color used to draw the bar for this data column.", m_clLineColor))

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim p2dColum As Pro3DBarColumn = DirectCast(doOriginal, Pro3DBarColumn)
            m_clLineColor = p2dColum.m_clLineColor

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim doItem As New Pro3DBarColumn(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            Return doItem
        End Function

        Public Overloads Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()
            m_clLineColor = Color.FromArgb(oXml.GetChildInt("LineColor"))
            m_iColumn = oXml.GetChildInt("Column")
            m_iRow = oXml.GetChildInt("Row")
            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            Dim iColor As Integer = m_clLineColor.ToArgb
            oXml.AddChildElement("LineColor", iColor)
            oXml.AddChildElement("Column", m_iColumn)
            oXml.AddChildElement("Row", m_iRow)
            oXml.OutOfElem()

        End Sub

#End Region

#End Region

    End Class

End Namespace
