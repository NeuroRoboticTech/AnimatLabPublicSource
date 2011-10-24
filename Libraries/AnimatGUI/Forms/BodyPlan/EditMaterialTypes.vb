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

Namespace Forms.BodyPlan

    Public Class EditMaterialTypes
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
        Friend WithEvents btnAddMaterial As System.Windows.Forms.Button
        Friend WithEvents btnRemoveMaterial As System.Windows.Forms.Button
        Friend WithEvents lblOdorTypes As System.Windows.Forms.Label
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents lblProperties As System.Windows.Forms.Label
        Friend WithEvents lvMaterialTypes As System.Windows.Forms.ListView
        Friend WithEvents gridMaterialPairs As System.Windows.Forms.DataGridView
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents pgMaterialPairs As System.Windows.Forms.PropertyGrid
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lvMaterialTypes = New System.Windows.Forms.ListView()
            Me.btnAddMaterial = New System.Windows.Forms.Button()
            Me.btnRemoveMaterial = New System.Windows.Forms.Button()
            Me.pgMaterialPairs = New System.Windows.Forms.PropertyGrid()
            Me.lblOdorTypes = New System.Windows.Forms.Label()
            Me.btnOk = New System.Windows.Forms.Button()
            Me.lblProperties = New System.Windows.Forms.Label()
            Me.gridMaterialPairs = New System.Windows.Forms.DataGridView()
            Me.Label1 = New System.Windows.Forms.Label()
            CType(Me.gridMaterialPairs, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lvMaterialTypes
            '
            Me.lvMaterialTypes.FullRowSelect = True
            Me.lvMaterialTypes.HideSelection = False
            Me.lvMaterialTypes.Location = New System.Drawing.Point(8, 24)
            Me.lvMaterialTypes.MultiSelect = False
            Me.lvMaterialTypes.Name = "lvMaterialTypes"
            Me.lvMaterialTypes.Size = New System.Drawing.Size(104, 228)
            Me.lvMaterialTypes.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.lvMaterialTypes.TabIndex = 0
            Me.lvMaterialTypes.UseCompatibleStateImageBehavior = False
            Me.lvMaterialTypes.View = System.Windows.Forms.View.List
            '
            'btnAddMaterial
            '
            Me.btnAddMaterial.Location = New System.Drawing.Point(8, 258)
            Me.btnAddMaterial.Name = "btnAddMaterial"
            Me.btnAddMaterial.Size = New System.Drawing.Size(104, 32)
            Me.btnAddMaterial.TabIndex = 1
            Me.btnAddMaterial.Text = "Add Material"
            '
            'btnRemoveMaterial
            '
            Me.btnRemoveMaterial.Location = New System.Drawing.Point(8, 296)
            Me.btnRemoveMaterial.Name = "btnRemoveMaterial"
            Me.btnRemoveMaterial.Size = New System.Drawing.Size(104, 32)
            Me.btnRemoveMaterial.TabIndex = 2
            Me.btnRemoveMaterial.Text = "Remove Material"
            '
            'pgMaterialPairs
            '
            Me.pgMaterialPairs.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.pgMaterialPairs.Location = New System.Drawing.Point(544, 24)
            Me.pgMaterialPairs.Name = "pgMaterialPairs"
            Me.pgMaterialPairs.Size = New System.Drawing.Size(224, 264)
            Me.pgMaterialPairs.TabIndex = 3
            Me.pgMaterialPairs.ToolbarVisible = False
            '
            'lblOdorTypes
            '
            Me.lblOdorTypes.Location = New System.Drawing.Point(8, 8)
            Me.lblOdorTypes.Name = "lblOdorTypes"
            Me.lblOdorTypes.Size = New System.Drawing.Size(104, 16)
            Me.lblOdorTypes.TabIndex = 7
            Me.lblOdorTypes.Text = "Material Types"
            Me.lblOdorTypes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnOk
            '
            Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOk.Location = New System.Drawing.Point(547, 296)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(221, 32)
            Me.btnOk.TabIndex = 9
            Me.btnOk.Text = "Close"
            '
            'lblProperties
            '
            Me.lblProperties.Location = New System.Drawing.Point(544, 8)
            Me.lblProperties.Name = "lblProperties"
            Me.lblProperties.Size = New System.Drawing.Size(232, 16)
            Me.lblProperties.TabIndex = 11
            Me.lblProperties.Text = "Material Pair Properties"
            Me.lblProperties.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'gridMaterialPairs
            '
            Me.gridMaterialPairs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.gridMaterialPairs.Location = New System.Drawing.Point(119, 24)
            Me.gridMaterialPairs.Name = "gridMaterialPairs"
            Me.gridMaterialPairs.Size = New System.Drawing.Size(419, 304)
            Me.gridMaterialPairs.TabIndex = 12
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(118, 8)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(420, 16)
            Me.Label1.TabIndex = 13
            Me.Label1.Text = "Material Association Pairs"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'EditMaterialTypes
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(778, 334)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.gridMaterialPairs)
            Me.Controls.Add(Me.lblProperties)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.lblOdorTypes)
            Me.Controls.Add(Me.pgMaterialPairs)
            Me.Controls.Add(Me.btnRemoveMaterial)
            Me.Controls.Add(Me.btnAddMaterial)
            Me.Controls.Add(Me.lvMaterialTypes)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "EditMaterialTypes"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Edit Material Types"
            CType(Me.gridMaterialPairs, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

#End Region

#Region " Properties "

     
#End Region

#Region " Methods "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                'm_btnOk = Me.btnOk
                'm_btnCancel = Me.btnCancel

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " Events "


#End Region

    End Class

End Namespace
