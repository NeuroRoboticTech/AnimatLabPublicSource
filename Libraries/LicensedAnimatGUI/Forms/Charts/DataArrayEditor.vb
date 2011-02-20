Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools
Imports AnimatTools.Forms
Imports AnimatTools.Framework
Imports Gigasoft
Imports Gigasoft.ProEssentials.Api
Imports System.IO
Imports System.Data

Namespace Forms.Charts

    Public Class DataArrayEditor
        Inherits System.Windows.Forms.Form

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
        Friend WithEvents txtRows As System.Windows.Forms.TextBox
        Friend WithEvents lblRows As System.Windows.Forms.Label
        Friend WithEvents lblColumns As System.Windows.Forms.Label
        Friend WithEvents txtColumns As System.Windows.Forms.TextBox
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents grdData As System.Windows.Forms.DataGrid
        Friend WithEvents grdProperties As System.Windows.Forms.PropertyGrid
        Friend WithEvents cboDefaultDataType As System.Windows.Forms.ComboBox
        Friend WithEvents lblDataType As System.Windows.Forms.Label
        Friend WithEvents lblStructure As System.Windows.Forms.Label
        Friend WithEvents cboDefaultStructure As System.Windows.Forms.ComboBox
        Friend WithEvents lblData As System.Windows.Forms.Label
        Friend WithEvents cboData As System.Windows.Forms.ComboBox
        Friend WithEvents btnAddData As System.Windows.Forms.Button
        Friend WithEvents btnClear As System.Windows.Forms.Button
        Friend WithEvents chkByRow As System.Windows.Forms.CheckBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.grdData = New System.Windows.Forms.DataGrid
            Me.grdProperties = New System.Windows.Forms.PropertyGrid
            Me.txtRows = New System.Windows.Forms.TextBox
            Me.lblRows = New System.Windows.Forms.Label
            Me.lblColumns = New System.Windows.Forms.Label
            Me.txtColumns = New System.Windows.Forms.TextBox
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnOk = New System.Windows.Forms.Button
            Me.cboDefaultDataType = New System.Windows.Forms.ComboBox
            Me.lblDataType = New System.Windows.Forms.Label
            Me.lblStructure = New System.Windows.Forms.Label
            Me.cboDefaultStructure = New System.Windows.Forms.ComboBox
            Me.lblData = New System.Windows.Forms.Label
            Me.cboData = New System.Windows.Forms.ComboBox
            Me.btnAddData = New System.Windows.Forms.Button
            Me.btnClear = New System.Windows.Forms.Button
            Me.chkByRow = New System.Windows.Forms.CheckBox
            CType(Me.grdData, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'grdData
            '
            Me.grdData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grdData.DataMember = ""
            Me.grdData.HeaderForeColor = System.Drawing.SystemColors.ControlText
            Me.grdData.Location = New System.Drawing.Point(8, 16)
            Me.grdData.Name = "grdData"
            Me.grdData.ReadOnly = True
            Me.grdData.Size = New System.Drawing.Size(432, 440)
            Me.grdData.TabIndex = 0
            '
            'grdProperties
            '
            Me.grdProperties.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.grdProperties.CommandsVisibleIfAvailable = True
            Me.grdProperties.LargeButtons = False
            Me.grdProperties.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.grdProperties.Location = New System.Drawing.Point(448, 96)
            Me.grdProperties.Name = "grdProperties"
            Me.grdProperties.Size = New System.Drawing.Size(296, 328)
            Me.grdProperties.TabIndex = 1
            Me.grdProperties.Text = "PropertyGrid"
            Me.grdProperties.ViewBackColor = System.Drawing.SystemColors.Window
            Me.grdProperties.ViewForeColor = System.Drawing.SystemColors.WindowText
            '
            'txtRows
            '
            Me.txtRows.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtRows.Location = New System.Drawing.Point(664, 32)
            Me.txtRows.Name = "txtRows"
            Me.txtRows.Size = New System.Drawing.Size(80, 20)
            Me.txtRows.TabIndex = 2
            Me.txtRows.Text = ""
            '
            'lblRows
            '
            Me.lblRows.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblRows.Location = New System.Drawing.Point(664, 16)
            Me.lblRows.Name = "lblRows"
            Me.lblRows.Size = New System.Drawing.Size(80, 16)
            Me.lblRows.TabIndex = 3
            Me.lblRows.Text = "Rows"
            Me.lblRows.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblColumns
            '
            Me.lblColumns.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblColumns.Location = New System.Drawing.Point(576, 16)
            Me.lblColumns.Name = "lblColumns"
            Me.lblColumns.Size = New System.Drawing.Size(80, 16)
            Me.lblColumns.TabIndex = 5
            Me.lblColumns.Text = "Columns"
            Me.lblColumns.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtColumns
            '
            Me.txtColumns.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtColumns.Location = New System.Drawing.Point(576, 32)
            Me.txtColumns.Name = "txtColumns"
            Me.txtColumns.Size = New System.Drawing.Size(80, 20)
            Me.txtColumns.TabIndex = 4
            Me.txtColumns.Text = ""
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(656, 432)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(88, 24)
            Me.btnCancel.TabIndex = 6
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(560, 432)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(88, 24)
            Me.btnOk.TabIndex = 7
            Me.btnOk.Text = "Ok"
            '
            'cboDefaultDataType
            '
            Me.cboDefaultDataType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboDefaultDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboDefaultDataType.Location = New System.Drawing.Point(448, 32)
            Me.cboDefaultDataType.Name = "cboDefaultDataType"
            Me.cboDefaultDataType.Size = New System.Drawing.Size(120, 21)
            Me.cboDefaultDataType.Sorted = True
            Me.cboDefaultDataType.TabIndex = 8
            '
            'lblDataType
            '
            Me.lblDataType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblDataType.Location = New System.Drawing.Point(448, 16)
            Me.lblDataType.Name = "lblDataType"
            Me.lblDataType.Size = New System.Drawing.Size(112, 16)
            Me.lblDataType.TabIndex = 9
            Me.lblDataType.Text = "Default DataType"
            Me.lblDataType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblStructure
            '
            Me.lblStructure.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblStructure.Location = New System.Drawing.Point(448, 56)
            Me.lblStructure.Name = "lblStructure"
            Me.lblStructure.Size = New System.Drawing.Size(112, 16)
            Me.lblStructure.TabIndex = 11
            Me.lblStructure.Text = "Default Structure"
            Me.lblStructure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'cboDefaultStructure
            '
            Me.cboDefaultStructure.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboDefaultStructure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboDefaultStructure.Location = New System.Drawing.Point(448, 72)
            Me.cboDefaultStructure.Name = "cboDefaultStructure"
            Me.cboDefaultStructure.Size = New System.Drawing.Size(104, 21)
            Me.cboDefaultStructure.Sorted = True
            Me.cboDefaultStructure.TabIndex = 10
            '
            'lblData
            '
            Me.lblData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblData.Location = New System.Drawing.Point(560, 56)
            Me.lblData.Name = "lblData"
            Me.lblData.Size = New System.Drawing.Size(112, 16)
            Me.lblData.TabIndex = 13
            Me.lblData.Text = "Data"
            Me.lblData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'cboData
            '
            Me.cboData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboData.Location = New System.Drawing.Point(560, 72)
            Me.cboData.Name = "cboData"
            Me.cboData.Size = New System.Drawing.Size(112, 21)
            Me.cboData.Sorted = True
            Me.cboData.TabIndex = 12
            '
            'btnAddData
            '
            Me.btnAddData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnAddData.Location = New System.Drawing.Point(672, 69)
            Me.btnAddData.Name = "btnAddData"
            Me.btnAddData.Size = New System.Drawing.Size(32, 24)
            Me.btnAddData.TabIndex = 14
            Me.btnAddData.Text = "Add"
            '
            'btnClear
            '
            Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnClear.Location = New System.Drawing.Point(704, 69)
            Me.btnClear.Name = "btnClear"
            Me.btnClear.Size = New System.Drawing.Size(40, 24)
            Me.btnClear.TabIndex = 15
            Me.btnClear.Text = "Clear"
            '
            'chkByRow
            '
            Me.chkByRow.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.chkByRow.Location = New System.Drawing.Point(680, 56)
            Me.chkByRow.Name = "chkByRow"
            Me.chkByRow.Size = New System.Drawing.Size(64, 16)
            Me.chkByRow.TabIndex = 16
            Me.chkByRow.Text = "By Row"
            '
            'DataArrayEditor
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(752, 466)
            Me.Controls.Add(Me.btnClear)
            Me.Controls.Add(Me.btnAddData)
            Me.Controls.Add(Me.chkByRow)
            Me.Controls.Add(Me.lblData)
            Me.Controls.Add(Me.cboData)
            Me.Controls.Add(Me.lblStructure)
            Me.Controls.Add(Me.cboDefaultStructure)
            Me.Controls.Add(Me.lblDataType)
            Me.Controls.Add(Me.cboDefaultDataType)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.lblColumns)
            Me.Controls.Add(Me.txtColumns)
            Me.Controls.Add(Me.txtRows)
            Me.Controls.Add(Me.lblRows)
            Me.Controls.Add(Me.grdProperties)
            Me.Controls.Add(Me.grdData)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "DataArrayEditor"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "DataArrayEditor"
            CType(Me.grdData, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_iColumns As Integer = 5
        Protected m_iRows As Integer = 5
        Protected m_aryData(,) As AnimatTools.DataObjects.Charting.DataColumn
        Protected m_dtData As DataTable
        Protected m_frmParentChart As LicensedAnimatTools.Forms.Charts.DataArray

        Private gridMouseDownTime As DateTime

        Protected m_strDefaultDataType As String = ""
        Protected m_doDefaultStructure As AnimatTools.DataObjects.Physical.PhysicalStructure
        Protected m_tpDefaultDataType As Type

        Protected m_aryComboData As New ArrayList
        Protected m_bLoading As Boolean = False

        Dim m_doColumnType As AnimatTools.DataObjects.Charting.DataColumn

#End Region

#Region " Properties "

        Public Property ParentChart() As LicensedAnimatTools.Forms.Charts.DataArray
            Get
                Return m_frmParentChart
            End Get
            Set(ByVal Value As LicensedAnimatTools.Forms.Charts.DataArray)
                m_frmParentChart = Value
            End Set
        End Property

        Public Property DefaultDataType() As String
            Get
                Return m_strDefaultDataType
            End Get
            Set(ByVal Value As String)
                m_strDefaultDataType = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Protected Function CopyColumnArray(ByVal aryOrig As AnimatTools.DataObjects.Charting.DataColumn(,)) As AnimatTools.DataObjects.Charting.DataColumn(,)

            Dim iOldCols As Integer = UBound(aryOrig, 1)
            Dim iOldRows As Integer = UBound(aryOrig, 2)

            Dim aryData(iOldCols, iOldRows) As AnimatTools.DataObjects.Charting.DataColumn

            Dim doColumn As AnimatTools.DataObjects.Charting.DataColumn
            For iCol As Integer = 0 To iOldCols
                For iRow As Integer = 0 To iOldRows
                    doColumn = aryOrig(iCol, iRow)

                    If Not doColumn Is Nothing AndAlso doColumn.IsValidColumn Then
                        aryData(iCol, iRow) = aryOrig(iCol, iRow)
                    Else
                        aryData(iCol, iRow) = Nothing
                    End If
                Next
            Next

            Return aryData
        End Function

        Protected Sub BuildDataArray(ByVal iCols As Integer, ByVal iRows As Integer)

            If iCols <= 0 Then
                Throw New System.Exception("The number of columns must be greater than 0.")
            End If
            If iRows <= 0 Then
                Throw New System.Exception("The number of rows must be greater than 0.")
            End If
            m_iColumns = iCols
            m_iRows = iRows

            If Not m_aryData Is Nothing AndAlso m_aryData.Rank = 2 Then
                Dim iOldCols As Integer = UBound(m_aryData, 1)
                Dim iOldRows As Integer = UBound(m_aryData, 2)

                Dim aryData(iOldCols, iOldRows) As AnimatTools.DataObjects.Charting.DataColumn

                For iCol As Integer = 0 To iOldCols
                    For iRow As Integer = 0 To iOldRows
                        aryData(iCol, iRow) = m_aryData(iCol, iRow)
                    Next
                Next

                ReDim m_aryData(iCols - 1, iRows - 1)

                If UBound(m_aryData, 1) < iOldCols Then iOldCols = UBound(m_aryData, 1)
                If UBound(m_aryData, 2) < iOldRows Then iOldRows = UBound(m_aryData, 2)

                For iCol As Integer = 0 To iOldCols
                    For iRow As Integer = 0 To iOldRows
                        m_aryData(iCol, iRow) = aryData(iCol, iRow)
                    Next
                Next
            Else
                ReDim m_aryData(iCols - 1, iRows - 1)
            End If

            m_dtData = New DataTable("Data")

            Dim ts1 As DataGridTableStyle
            ts1 = New DataGridTableStyle
            ts1.MappingName = "Data"
            ' Set other properties.
            ts1.AlternatingBackColor = Color.LightGray
            ts1.AllowSorting = False

            'If m_tpDefaultDataType Is Nothing OrElse m_doDefaultStructure Is Nothing Then
            CreateTextColumns(ts1, iCols, iRows)
            'Else
            '    CreateDropDownColumns(ts1, iCols, iRows)
            'End If

            'Now add the correct number of rows.
            Dim drRow As DataRow
            For iRow As Integer = 1 To iRows
                drRow = m_dtData.NewRow()

                'Go through and fill in the data 
                For iCol As Integer = 0 To iCols - 1
                    If Not m_aryData(iCol, iRow - 1) Is Nothing Then
                        drRow(iCol) = m_aryData(iCol, iRow - 1).Name
                    Else
                        drRow(iCol) = ""
                    End If
                Next

                m_dtData.Rows.Add(drRow)
            Next

            grdData.TableStyles.Clear()
            grdData.TableStyles.Add(ts1)

            grdData.DataSource = m_dtData

        End Sub

        Protected Overridable Sub CreateTextColumns(ByVal ts1 As DataGridTableStyle, ByVal iCols As Integer, ByVal iRows As Integer)
            Dim TextCol As DataGridTextBoxColumn
            'Lets add the correct number of columns.
            Dim dcColumn As DataColumn
            For iCol As Integer = 1 To iCols
                dcColumn = New DataColumn("Col" & iCol, System.Type.GetType("System.String"))
                m_dtData.Columns.Add(dcColumn)

                TextCol = New DataGridTextBoxColumn
                TextCol.MappingName = "Col" & iCol
                TextCol.HeaderText = "Col " & iCol
                TextCol.Width = 70
                'add handler
                AddHandler TextCol.TextBox.MouseDown, New MouseEventHandler(AddressOf OnTextBoxMouseDownHandler)
                AddHandler TextCol.TextBox.DoubleClick, New EventHandler(AddressOf OnTextBoxDoubleClickHandler)
                ts1.GridColumnStyles.Add(TextCol)
            Next

        End Sub

        Protected Sub CreateDataDropDown(ByVal cboData As ComboBox)
            Dim colData As New AnimatTools.Collections.DataObjects(Nothing)

            If Not m_tpDefaultDataType Is Nothing AndAlso Not m_doDefaultStructure Is Nothing Then
                cboData.SuspendLayout()
                cboData.Items.Clear()
                m_doDefaultStructure.FindChildrenOfType(m_tpDefaultDataType, colData)

                Dim doDrag As AnimatTools.DataObjects.DragObject
                For Each doData As AnimatTools.Framework.DataObject In colData
                    If TypeOf doData Is AnimatTools.DataObjects.DragObject Then
                        doDrag = DirectCast(doData, AnimatTools.DataObjects.DragObject)
                        cboData.Items.Add(doDrag)
                    End If
                Next

                cboData.ResumeLayout()
            End If
        End Sub

        Protected Overridable Sub SelectDataType(ByVal strType As String)
            Select Case strType
                Case "Joint"
                    m_tpDefaultDataType = GetType(AnimatTools.DataObjects.Physical.Joint)
                Case "Neuron"
                    m_tpDefaultDataType = GetType(AnimatTools.DataObjects.Behavior.Nodes.Neuron)
                Case "Organism"
                    m_tpDefaultDataType = GetType(AnimatTools.DataObjects.Physical.Organism)
                Case "Rigid Body"
                    m_tpDefaultDataType = GetType(AnimatTools.DataObjects.Physical.RigidBody)
                Case "Structure"
                    m_tpDefaultDataType = GetType(AnimatTools.DataObjects.Physical.PhysicalStructure)
                Case ""
                    m_tpDefaultDataType = Nothing
            End Select

            m_strDefaultDataType = strType
            CreateDataDropDown(Me.cboData)
            BuildDataArray(m_iColumns, m_iRows)

        End Sub

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            Try
                m_bLoading = True
                m_doColumnType = New DataObjects.Charting.Pro3DBarColumn(Me.ParentChart.FormHelper)

                If Not m_frmParentChart Is Nothing AndAlso Not m_frmParentChart.DataColumns Is Nothing AndAlso m_frmParentChart.DataColumns.Rank = 2 Then
                    m_aryData = CopyColumnArray(m_frmParentChart.DataColumns)
                    m_iColumns = UBound(m_aryData, 1) + 1
                    m_iRows = UBound(m_aryData, 2) + 1
                End If

                BuildDataArray(m_iColumns, m_iRows)
                txtRows.Text = m_iRows.ToString()
                txtColumns.Text = m_iColumns.ToString()

                cboDefaultDataType.Items.Add("")
                cboDefaultDataType.Items.Add("Joint")
                cboDefaultDataType.Items.Add("Neuron")
                cboDefaultDataType.Items.Add("Rigid Body")
                m_tpDefaultDataType = GetType(AnimatTools.DataObjects.Behavior.Nodes.Neuron)

                Dim strDefaultStructureID As String = ""
                If Not m_frmParentChart Is Nothing Then
                    cboDefaultDataType.SelectedItem = m_frmParentChart.DefaultDataType
                    strDefaultStructureID = m_frmParentChart.DefaultStructureID
                Else
                    cboDefaultDataType.SelectedIndex = 1
                End If

                Dim doStruct As AnimatTools.DataObjects.Physical.PhysicalStructure
                For Each deEntry As DictionaryEntry In Util.Environment.Structures
                    doStruct = DirectCast(deEntry.Value, AnimatTools.DataObjects.Physical.PhysicalStructure)
                    If doStruct.ID = strDefaultStructureID Then
                        m_doDefaultStructure = doStruct
                    End If
                    cboDefaultStructure.Items.Add(doStruct)
                Next

                For Each deEntry As DictionaryEntry In Util.Environment.Organisms
                    doStruct = DirectCast(deEntry.Value, AnimatTools.DataObjects.Physical.PhysicalStructure)
                    If doStruct.ID = strDefaultStructureID Then
                        m_doDefaultStructure = doStruct
                    End If
                    cboDefaultStructure.Items.Add(doStruct)
                Next

                If Not m_doDefaultStructure Is Nothing Then
                    cboDefaultStructure.SelectedItem = m_doDefaultStructure
                ElseIf cboDefaultStructure.Items.Count > 0 Then
                    cboDefaultStructure.SelectedIndex = 0
                    m_doDefaultStructure = DirectCast(cboDefaultStructure.SelectedItem, AnimatTools.DataObjects.Physical.PhysicalStructure)
                End If

                CreateDataDropDown(Me.cboData)

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                m_bLoading = False
            End Try
        End Sub

        Private Sub txtColumns_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtColumns.Validating
            Try
                If txtColumns.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify a number of columns.")
                End If

                If Not IsNumeric(txtColumns.Text) Then
                    Throw New System.Exception("Columns must be a numeric value.")
                End If

                Dim iCols As Integer = CInt(txtColumns.Text)

                If iCols <= 0 Then
                    Throw New System.Exception("The number of columns must be greater than 0.")
                End If

                If iCols <> m_iColumns Then
                    BuildDataArray(iCols, m_iRows)
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
                e.Cancel = True
            End Try
        End Sub

        Private Sub txtRows_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtRows.Validating
            Try
                If txtRows.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify a number of rows.")
                End If

                If Not IsNumeric(txtRows.Text) Then
                    Throw New System.Exception("Rows must be a numeric value.")
                End If

                Dim iRows As Integer = CInt(txtRows.Text)

                If iRows <= 0 Then
                    Throw New System.Exception("The number of rows must be greater than 0.")
                End If

                If iRows <> m_iRows Then
                    BuildDataArray(m_iColumns, iRows)
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
                e.Cancel = True
            End Try
        End Sub

        Private Sub OnDoubleClickGrid()

            Try
                Util.DisableDirtyFlags = True

                Dim frmSelectItem As New AnimatTools.Forms.Tools.SelectDataItem
                Dim doColumn As LicensedAnimatTools.DataObjects.Charting.Pro3DBarColumn

                frmSelectItem.ColumnType = m_doColumnType
                frmSelectItem.BuildTreeView()
                If frmSelectItem.ShowDialog(m_frmParentChart) = DialogResult.OK Then
                    Util.DisableDirtyFlags = False

                    If Not frmSelectItem.DataColumn Is Nothing AndAlso TypeOf frmSelectItem.DataColumn Is LicensedAnimatTools.DataObjects.Charting.Pro3DBarColumn Then
                        doColumn = DirectCast(frmSelectItem.DataColumn, LicensedAnimatTools.DataObjects.Charting.Pro3DBarColumn)
                        doColumn.Column = grdData.CurrentCell.ColumnNumber
                        doColumn.Row = grdData.CurrentCell.RowNumber
                        doColumn.ParentChart = Me.ParentChart
                        m_aryData(grdData.CurrentCell.ColumnNumber, grdData.CurrentCell.RowNumber) = doColumn
                        m_dtData.Rows(grdData.CurrentCell.RowNumber)(grdData.CurrentCell.ColumnNumber) = doColumn.Name

                        'If this is a neuron type then lets default it to firing frequency instead of membrane voltage. 
                        ''That makes more sense as a default for this type of chart.
                        'If doColumn.DataType.DataTypes.Contains("FiringFrequency") Then
                        '    doColumn.DataType.ID = "FiringFrequency"
                        'End If

                        grdProperties.SelectedObject = doColumn.Properties
                    End If
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                Util.DisableDirtyFlags = False
            End Try
        End Sub

        Private Sub OnTextBoxDoubleClickHandler(ByVal sender As Object, ByVal e As EventArgs)
            Try
                OnDoubleClickGrid()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnTextBoxMouseDownHandler(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
            Try
                If (DateTime.Now < gridMouseDownTime.AddMilliseconds(SystemInformation.DoubleClickTime)) Then
                    OnDoubleClickGrid()
                End If
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub grdData_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles grdData.MouseDown
            Try
                gridMouseDownTime = DateTime.Now
            Catch ex As System.Exception
            End Try
        End Sub

        Private Sub grdData_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdData.CurrentCellChanged
            Try

                If Not m_aryData(grdData.CurrentCell.ColumnNumber, grdData.CurrentCell.RowNumber) Is Nothing Then
                    Dim dcCol As AnimatTools.DataObjects.Charting.DataColumn = m_aryData(grdData.CurrentCell.ColumnNumber, grdData.CurrentCell.RowNumber)
                    grdProperties.SelectedObject = dcCol.Properties
                Else
                    grdProperties.SelectedObject = Nothing
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try

                m_frmParentChart.DataColumns = m_aryData
                m_frmParentChart.DefaultDataType = m_strDefaultDataType

                If Not m_doDefaultStructure Is Nothing Then
                    m_frmParentChart.DefaultStructureID = m_doDefaultStructure.ID
                Else
                    m_frmParentChart.DefaultStructureID = ""
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub cboDefaultDataType_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDefaultDataType.SelectedValueChanged
            Try
                If Not cboDefaultDataType.SelectedItem Is Nothing AndAlso Not m_bLoading Then
                    SelectDataType(DirectCast(cboDefaultDataType.SelectedItem, String))
                End If
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub cboDefaultStructure_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDefaultStructure.SelectedValueChanged
            Try
                If Not cboDefaultStructure.SelectedItem Is Nothing AndAlso Not m_bLoading Then
                    m_doDefaultStructure = DirectCast(cboDefaultStructure.SelectedItem, AnimatTools.DataObjects.Physical.PhysicalStructure)
                    CreateDataDropDown(Me.cboData)
                    BuildDataArray(m_iColumns, m_iRows)
                End If
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnAddData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddData.Click
            Try
                If Not m_doDefaultStructure Is Nothing AndAlso Not cboData.SelectedItem Is Nothing AndAlso TypeOf cboData.SelectedItem Is AnimatTools.DataObjects.DragObject Then
                    Dim doDrag As AnimatTools.DataObjects.DragObject = DirectCast(cboData.SelectedItem, AnimatTools.DataObjects.DragObject)
                    Dim doColumn As LicensedAnimatTools.DataObjects.Charting.Pro3DBarColumn
                    Util.DisableDirtyFlags = False

                    doColumn = DirectCast(m_doColumnType.CreateDataColumn(doDrag, False), LicensedAnimatTools.DataObjects.Charting.Pro3DBarColumn)
                    doColumn.Column = grdData.CurrentCell.ColumnNumber
                    doColumn.Row = grdData.CurrentCell.RowNumber
                    doColumn.ParentChart = Me.ParentChart
                    m_aryData(grdData.CurrentCell.ColumnNumber, grdData.CurrentCell.RowNumber) = doColumn
                    m_dtData.Rows(grdData.CurrentCell.RowNumber)(grdData.CurrentCell.ColumnNumber) = doColumn.Name

                    'If this is a neuron type then lets default it to firing frequency instead of membrane voltage. 
                    'That makes more sense as a default for this type of chart.
                    'If doColumn.DataType.DataTypes.Contains("FiringFrequency") Then
                    '    doColumn.DataType.ID = "FiringFrequency"
                    'End If

                    'Now automatically increment the current location in the grid so we can just keep filling things in.
                    If Me.chkByRow.Checked Then
                        If grdData.CurrentCell.ColumnNumber + 1 <= UBound(m_aryData, 1) Then
                            grdData.CurrentCell = New DataGridCell(grdData.CurrentCell.RowNumber, grdData.CurrentCell.ColumnNumber + 1)
                        Else
                            If grdData.CurrentCell.RowNumber + 1 <= UBound(m_aryData, 2) Then
                                grdData.CurrentCell = New DataGridCell(grdData.CurrentCell.RowNumber + 1, 0)
                            Else
                                grdData.CurrentCell = New DataGridCell(0, 0)
                            End If
                        End If
                    Else
                        If grdData.CurrentCell.RowNumber + 1 <= UBound(m_aryData, 2) Then
                            grdData.CurrentCell = New DataGridCell(grdData.CurrentCell.RowNumber + 1, grdData.CurrentCell.ColumnNumber)
                        Else
                            If grdData.CurrentCell.ColumnNumber + 1 <= UBound(m_aryData, 1) Then
                                grdData.CurrentCell = New DataGridCell(0, grdData.CurrentCell.ColumnNumber + 1)
                            Else
                                grdData.CurrentCell = New DataGridCell(0, 0)
                            End If
                        End If
                    End If

                End If

                If cboData.SelectedIndex + 1 = cboData.Items.Count Then
                    cboData.SelectedIndex = 0
                Else
                    cboData.SelectedIndex = cboData.SelectedIndex + 1
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
            Try
                m_aryData(grdData.CurrentCell.ColumnNumber, grdData.CurrentCell.RowNumber) = Nothing
                m_dtData.Rows(grdData.CurrentCell.RowNumber)(grdData.CurrentCell.ColumnNumber) = ""
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
