Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI
Imports AnimatGUI.Forms
Imports AnimatGUI.Framework
Imports Gigasoft
Imports Gigasoft.ProEssentials.Api

Namespace Forms.Charts

    Public Class LineData
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
        Friend WithEvents grdData As System.Windows.Forms.DataGrid
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.grdData = New System.Windows.Forms.DataGrid
            CType(Me.grdData, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'grdData
            '
            Me.grdData.DataMember = ""
            Me.grdData.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grdData.HeaderForeColor = System.Drawing.SystemColors.ControlText
            Me.grdData.Location = New System.Drawing.Point(0, 0)
            Me.grdData.Name = "grdData"
            Me.grdData.Size = New System.Drawing.Size(632, 566)
            Me.grdData.TabIndex = 0
            '
            'LineData
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(632, 566)
            Me.Controls.Add(Me.grdData)
            Me.Name = "LineData"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "LineData"
            CType(Me.grdData, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Public m_aryData(,) As Single
        Public m_aryNames() As String

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            grdData.DataSource = New AnimatGuiCtrls.Controls.ArrayDataView(m_aryData, m_aryNames)
        End Sub

    End Class

End Namespace
