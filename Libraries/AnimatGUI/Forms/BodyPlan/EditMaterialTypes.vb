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
        Friend WithEvents pgMaterialProperties As System.Windows.Forms.PropertyGrid
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lvMaterialTypes = New System.Windows.Forms.ListView()
            Me.btnAddMaterial = New System.Windows.Forms.Button()
            Me.btnRemoveMaterial = New System.Windows.Forms.Button()
            Me.pgMaterialProperties = New System.Windows.Forms.PropertyGrid()
            Me.lblOdorTypes = New System.Windows.Forms.Label()
            Me.btnOk = New System.Windows.Forms.Button()
            Me.lblProperties = New System.Windows.Forms.Label()
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
            'pgMaterialProperties
            '
            Me.pgMaterialProperties.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.pgMaterialProperties.Location = New System.Drawing.Point(118, 24)
            Me.pgMaterialProperties.Name = "pgMaterialProperties"
            Me.pgMaterialProperties.Size = New System.Drawing.Size(362, 264)
            Me.pgMaterialProperties.TabIndex = 3
            Me.pgMaterialProperties.ToolbarVisible = False
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
            Me.btnOk.Location = New System.Drawing.Point(121, 296)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(359, 32)
            Me.btnOk.TabIndex = 9
            Me.btnOk.Text = "Close"
            '
            'lblProperties
            '
            Me.lblProperties.Location = New System.Drawing.Point(118, 8)
            Me.lblProperties.Name = "lblProperties"
            Me.lblProperties.Size = New System.Drawing.Size(370, 16)
            Me.lblProperties.TabIndex = 11
            Me.lblProperties.Text = "Material Properties"
            Me.lblProperties.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'EditMaterialTypes
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(491, 334)
            Me.Controls.Add(Me.lblProperties)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.lblOdorTypes)
            Me.Controls.Add(Me.pgMaterialProperties)
            Me.Controls.Add(Me.btnRemoveMaterial)
            Me.Controls.Add(Me.btnAddMaterial)
            Me.Controls.Add(Me.lvMaterialTypes)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "EditMaterialTypes"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Edit Material Types"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_doSelectedGridItem As Framework.DataObject

#End Region

#Region " Properties "


#End Region

#Region " Methods "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnOk

                SetMaterialTypesListView()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub SetMaterialTypesListView()
            lvMaterialTypes.HideSelection = False
            lvMaterialTypes.MultiSelect = False
            lvMaterialTypes.FullRowSelect = True
            lvMaterialTypes.View = View.Details
            lvMaterialTypes.AllowColumnReorder = True
            lvMaterialTypes.LabelEdit = True
            lvMaterialTypes.Sorting = SortOrder.Ascending

            lvMaterialTypes.Columns.Add("Material Types", 200, HorizontalAlignment.Left)

            Dim doType As DataObjects.Physical.MaterialType
            For Each deEntry As DictionaryEntry In Util.Environment.MaterialTypes
                doType = DirectCast(deEntry.Value, DataObjects.Physical.MaterialType)
                AddListItem(doType)
            Next
        End Sub

        Protected Function AddListItem(ByVal doType As DataObjects.Physical.MaterialType) As ListViewItem
            Dim liItem As ListViewItem
            liItem = New ListViewItem(doType.Name)
            liItem.Tag = doType
            lvMaterialTypes.Items.Add(liItem)
        End Function

        Protected Sub RemoveListItem(ByVal doType As DataObjects.Physical.MaterialType)
            For Each liItem As ListViewItem In lvMaterialTypes.Items
                If liItem.Tag Is doType Then
                    lvMaterialTypes.Items.Remove(liItem)
                    Return
                End If
            Next
        End Sub

        Protected Sub SetSelectedGridObject(ByVal doObject As Framework.DataObject)
            If Not doObject Is Nothing Then
                pgMaterialProperties.SelectedObject = doObject.Properties
            Else
                pgMaterialProperties.SelectedObject = Nothing
            End If
            m_doSelectedGridItem = doObject
        End Sub


#End Region

#Region " Events "

        Protected Function AddMaterialType(Optional ByVal strName As String = "") As DataObjects.Physical.MaterialType
            Dim doType As DataObjects.Physical.MaterialType = Util.Environment.AddMaterialType()

            If strName.Trim.Length > 0 Then
                doType.Name = strName
            End If

            AddListItem(doType)

            Return doType
        End Function

        Private Sub btnAddMaterial_Click(sender As System.Object, e As System.EventArgs) Handles btnAddMaterial.Click
            Try
                AddMaterialType()
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub RemoveMaterialType()
            If lvMaterialTypes.SelectedItems.Count > 0 Then
                Dim liItem As ListViewItem = lvMaterialTypes.SelectedItems(0)

                If Not liItem.Tag Is Nothing AndAlso TypeOf liItem.Tag Is DataObjects.Physical.MaterialType Then
                    Dim doType As DataObjects.Physical.MaterialType = DirectCast(liItem.Tag, DataObjects.Physical.MaterialType)

                    If doType.ID = "DEFAULTMATERIAL" Then
                        Throw New System.Exception("You cannot delete the default material type.")
                    End If

                    Dim frmReplaceType As New Forms.BodyPlan.ReplaceMaterialType
                    frmReplaceType.TypeToReplace = doType

                    If frmReplaceType.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        Dim doReplaceType As DataObjects.Physical.MaterialType = DirectCast(frmReplaceType.cboMaterialTypes.SelectedItem, DataObjects.Physical.MaterialType)
                        Util.Environment.RemoveMaterialType(doType, doReplaceType)
                        RemoveListItem(doType)
                    End If
                End If
            End If
        End Sub

        Private Sub btnRemoveMaterial_Click(sender As System.Object, e As System.EventArgs) Handles btnRemoveMaterial.Click
            Try
                RemoveMaterialType()
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub lvMaterialTypes_AfterLabelEdit(sender As Object, e As System.Windows.Forms.LabelEditEventArgs) Handles lvMaterialTypes.AfterLabelEdit
            Try
                If Not e.Label Is Nothing Then
                    If lvMaterialTypes.SelectedItems.Count > 0 Then
                        If e.Label.Trim.Length = 0 Then
                            Throw New System.Exception("Material name cannot be blank.")
                        End If

                        Dim liItem As ListViewItem = lvMaterialTypes.SelectedItems(0)

                        If Not liItem.Tag Is Nothing AndAlso TypeOf liItem.Tag Is DataObjects.Physical.MaterialType Then
                            Dim doType As DataObjects.Physical.MaterialType = DirectCast(liItem.Tag, DataObjects.Physical.MaterialType)
                            doType.Name = e.Label
                        End If
                    End If
                Else
                    e.CancelEdit = True
                End If

            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
                e.CancelEdit = True
            End Try
        End Sub

        Private Sub lvMaterialTypes_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvMaterialTypes.SelectedIndexChanged
            Try
                ClickItem(lvMaterialTypes)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub ClickItem(ByVal lvItems As ListView)
            If lvMaterialTypes.SelectedItems.Count = 1 Then
                Dim doMaterialType As DataObjects.Physical.MaterialType = DirectCast(lvMaterialTypes.SelectedItems(0).Tag, DataObjects.Physical.MaterialType)
                SetSelectedGridObject(doMaterialType)
            ElseIf lvMaterialTypes.SelectedItems.Count > 1 Then
                Throw New System.Exception("You can only select 1 material type to edit at a time.")
            Else
                SetSelectedGridObject(Nothing)
            End If
        End Sub

#End Region

#Region "Automation"
        Public Sub Automation_AddMaterialType(ByVal strName As String)
            Dim doType As DataObjects.Physical.MaterialType = AddMaterialType(strName)
            SetSelectedGridObject(doType)
        End Sub

        Public Sub Automation_RemoveOdorType()
            btnRemoveMaterial.PerformClick()
        End Sub

        Public Sub Automation_SelectMaterialType(ByVal strName As String)
            Dim liItem As ListViewItem = Util.FindListItemByName(strName, lvMaterialTypes.Items)
            lvMaterialTypes.SelectedItems.Clear()
            liItem.Selected = True
            ClickItem(lvMaterialTypes)
        End Sub

        Public Sub Automation_SetSelectedItemProperty(ByVal strProperty As String, ByVal strValue As String)

            If m_doSelectedGridItem Is Nothing Then
                Throw New System.Exception("No material item is currently selected.")
            End If

            Util.SetObjectProperty(m_doSelectedGridItem, strProperty, strValue)

        End Sub

#End Region

    End Class

End Namespace
