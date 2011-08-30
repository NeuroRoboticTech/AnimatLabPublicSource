Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects
Imports System.Drawing.Imaging
Imports System.Data

Namespace Forms.Behavior

    Public Class Connections
        Inherits AnimatGUI.Forms.AnimatDialog

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
        Friend WithEvents grdInputs As System.Windows.Forms.DataGrid
        Friend WithEvents lblInputs As System.Windows.Forms.Label
        Friend WithEvents grdOutputs As System.Windows.Forms.DataGrid
        Friend WithEvents lblOutputs As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.grdInputs = New System.Windows.Forms.DataGrid
            Me.grdOutputs = New System.Windows.Forms.DataGrid
            Me.lblInputs = New System.Windows.Forms.Label
            Me.lblOutputs = New System.Windows.Forms.Label
            CType(Me.grdInputs, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.grdOutputs, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'grdInputs
            '
            Me.grdInputs.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.grdInputs.DataMember = ""
            Me.grdInputs.HeaderForeColor = System.Drawing.SystemColors.ControlText
            Me.grdInputs.Location = New System.Drawing.Point(8, 24)
            Me.grdInputs.Name = "grdInputs"
            Me.grdInputs.ReadOnly = True
            Me.grdInputs.Size = New System.Drawing.Size(272, 96)
            Me.grdInputs.TabIndex = 0
            '
            'grdOutputs
            '
            Me.grdOutputs.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.grdOutputs.DataMember = ""
            Me.grdOutputs.HeaderForeColor = System.Drawing.SystemColors.ControlText
            Me.grdOutputs.Location = New System.Drawing.Point(8, 144)
            Me.grdOutputs.Name = "grdOutputs"
            Me.grdOutputs.ReadOnly = True
            Me.grdOutputs.Size = New System.Drawing.Size(272, 96)
            Me.grdOutputs.TabIndex = 1
            '
            'lblInputs
            '
            Me.lblInputs.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.lblInputs.Location = New System.Drawing.Point(8, 8)
            Me.lblInputs.Name = "lblInputs"
            Me.lblInputs.Size = New System.Drawing.Size(272, 16)
            Me.lblInputs.TabIndex = 2
            Me.lblInputs.Text = "Input Connections"
            Me.lblInputs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblOutputs
            '
            Me.lblOutputs.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.lblOutputs.Location = New System.Drawing.Point(10, 125)
            Me.lblOutputs.Name = "lblOutputs"
            Me.lblOutputs.Size = New System.Drawing.Size(272, 16)
            Me.lblOutputs.TabIndex = 3
            Me.lblOutputs.Text = "Output Connections"
            Me.lblOutputs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'Connections
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 278)
            Me.Controls.Add(Me.lblOutputs)
            Me.Controls.Add(Me.lblInputs)
            Me.Controls.Add(Me.grdOutputs)
            Me.Controls.Add(Me.grdInputs)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "Connections"
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Connections"
            CType(Me.grdInputs, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.grdOutputs, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_doNode As AnimatGUI.DataObjects.Behavior.Node
        Protected m_doSelectedNode As AnimatGUI.DataObjects.Behavior.Node
        Protected m_doSelectedLink As AnimatGUI.DataObjects.Behavior.Link

        Protected m_aryInLinks As New Collection
        Protected m_aryOutLinks As New Collection

        Private gridMouseDownTime As DateTime

        Public m_aryInputData(,) As String
        Public m_aryInputNames() As String

        Public m_aryOutputData(,) As String
        Public m_aryOutputNames() As String

#End Region

#Region " Properties "

        Public Overridable Property Node() As AnimatGUI.DataObjects.Behavior.Node
            Get
                Return m_doNode
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.Node)
                m_doNode = Value
            End Set
        End Property

        Public Overridable ReadOnly Property SelectedNode() As AnimatGUI.DataObjects.Behavior.Node
            Get
                Return m_doSelectedNode
            End Get
        End Property

        Public Overridable ReadOnly Property SelectedLink() As AnimatGUI.DataObjects.Behavior.Link
            Get
                Return m_doSelectedLink
            End Get
        End Property

#End Region

#Region " Methods "

        Protected Sub BuildDataArray(ByVal bInLinks As Boolean, ByVal aryData As AnimatGUI.Collections.SortedLinks, ByVal grdData As DataGrid, ByVal aryCol As Collection)

            If aryData.Count = 0 Then
                Return
            End If

            Dim dtData As DataTable = New DataTable("Data")

            Dim ts1 As DataGridTableStyle
            ts1 = New DataGridTableStyle
            ts1.MappingName = "Data"
            ' Set other properties.
            ts1.AlternatingBackColor = Color.LightGray
            ts1.AllowSorting = True

            CreateTextColumns(ts1, dtData)

            Dim drRow As DataRow
            Dim doLink As AnimatGUI.DataObjects.Behavior.Link
            For Each deEntry As DictionaryEntry In aryData
                doLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                aryCol.Add(doLink)

                drRow = dtData.NewRow()
                drRow(0) = doLink.ItemName

                If bInLinks Then
                    drRow(1) = doLink.Origin.Name
                Else
                    drRow(1) = doLink.Destination.Name
                End If
                dtData.Rows.Add(drRow)
            Next

            grdData.TableStyles.Clear()
            grdData.TableStyles.Add(ts1)

            grdData.DataSource = dtData

        End Sub

        Protected Overridable Sub CreateTextColumns(ByVal ts1 As DataGridTableStyle, ByVal dtData As DataTable)
            Dim TextCol As DataGridTextBoxColumn
            'Lets add the correct number of columns.

            Dim dcColumn As DataColumn = New DataColumn("Synapse")
            dtData.Columns.Add(dcColumn)
            TextCol = New DataGridTextBoxColumn
            TextCol.MappingName = "Synapse"
            TextCol.HeaderText = "Synapse"
            TextCol.Width = 100
            'add handler
            AddHandler TextCol.TextBox.MouseDown, New MouseEventHandler(AddressOf OnTextBoxMouseDownHandler)
            AddHandler TextCol.TextBox.DoubleClick, New EventHandler(AddressOf OnTextBoxDoubleClickHandler)
            ts1.GridColumnStyles.Add(TextCol)

            dcColumn = New DataColumn("Neuron")
            dtData.Columns.Add(dcColumn)
            TextCol = New DataGridTextBoxColumn
            TextCol.MappingName = "Neuron"
            TextCol.HeaderText = "Neuron"
            TextCol.Width = 100
            'add handler
            AddHandler TextCol.TextBox.MouseDown, New MouseEventHandler(AddressOf OnTextBoxMouseDownHandler)
            AddHandler TextCol.TextBox.DoubleClick, New EventHandler(AddressOf OnTextBoxDoubleClickHandler)
            ts1.GridColumnStyles.Add(TextCol)

        End Sub

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                If m_doNode Is Nothing Then
                    Throw New System.Exception("You must specify a node before you can view its connections.")
                End If

                BuildDataArray(True, m_doNode.InLinks, grdInputs, m_aryInLinks)
                BuildDataArray(False, m_doNode.OutLinks, grdOutputs, m_aryOutLinks)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
            'MyBase.OnResize(e)

            Try
                lblInputs.Top = 10
                lblInputs.Left = 10
                lblInputs.Width = Me.Width - 35

                grdInputs.Top = (lblInputs.Top + lblInputs.Height + 5)
                grdInputs.Height = CInt((Me.Height / 2) - 50)
                grdInputs.Left = 10
                grdInputs.Width = Me.Width - 35

                lblOutputs.Top = (grdInputs.Top + grdInputs.Height + 5)
                lblOutputs.Left = 10
                lblOutputs.Width = Me.Width - 35

                grdOutputs.Top = (lblOutputs.Top + lblOutputs.Height + 5)
                grdOutputs.Height = grdInputs.Height
                grdOutputs.Left = 10
                grdOutputs.Width = Me.Width - 35

            Catch ex As System.Exception

            End Try
        End Sub

        Private Sub OnTextBoxDoubleClickHandler(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If Not sender Is Nothing AndAlso TypeOf sender Is System.Windows.Forms.DataGridTextBox Then
                    Dim doBox As System.Windows.Forms.DataGridTextBox = DirectCast(sender, System.Windows.Forms.DataGridTextBox)

                    If doBox.Parent Is grdInputs Then
                        grdInputs_DoubleClick(Me, New System.EventArgs)
                    ElseIf doBox.Parent Is grdOutputs Then
                        grdOutputs_DoubleClick(Me, New System.EventArgs)
                    End If
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnTextBoxMouseDownHandler(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
            Try
                If (DateTime.Now < gridMouseDownTime.AddMilliseconds(SystemInformation.DoubleClickTime)) Then
                    If Not sender Is Nothing AndAlso TypeOf sender Is System.Windows.Forms.DataGridTextBox Then
                        Dim doBox As System.Windows.Forms.DataGridTextBox = DirectCast(sender, System.Windows.Forms.DataGridTextBox)

                        If doBox.Parent Is grdInputs Then
                            grdInputs_DoubleClick(Me, New System.EventArgs)
                        ElseIf doBox.Parent Is grdOutputs Then
                            grdOutputs_DoubleClick(Me, New System.EventArgs)
                        End If
                    End If
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub grdInputs_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdInputs.DoubleClick

            Try
                Dim doLink As AnimatGUI.DataObjects.Behavior.Link = DirectCast(m_aryInLinks(grdInputs.CurrentCell.RowNumber + 1), AnimatGUI.DataObjects.Behavior.Link)

                If grdInputs.CurrentCell.ColumnNumber = 0 Then
                    m_doSelectedLink = doLink
                Else
                    m_doSelectedNode = doLink.Origin
                End If

                Me.Close()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub grdOutputs_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdOutputs.DoubleClick

            Try
                Dim doLink As AnimatGUI.DataObjects.Behavior.Link = DirectCast(m_aryOutLinks(grdOutputs.CurrentCell.RowNumber + 1), AnimatGUI.DataObjects.Behavior.Link)

                If grdOutputs.CurrentCell.ColumnNumber = 0 Then
                    m_doSelectedLink = doLink
                Else
                    m_doSelectedNode = doLink.Destination
                End If

                Me.Close()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub grdInputs_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles grdInputs.MouseDown
            Try
                gridMouseDownTime = DateTime.Now
            Catch ex As System.Exception
            End Try
        End Sub

        Private Sub grdOutputs_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles grdOutputs.MouseDown
            Try
                gridMouseDownTime = DateTime.Now
            Catch ex As System.Exception
            End Try
        End Sub

#End Region

    End Class

End Namespace
