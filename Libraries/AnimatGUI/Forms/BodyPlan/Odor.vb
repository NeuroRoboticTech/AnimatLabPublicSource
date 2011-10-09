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

    Public Class Odor
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
        Friend WithEvents btnAddOdorType As System.Windows.Forms.Button
        Friend WithEvents btnRemoveOdorType As System.Windows.Forms.Button
        Friend WithEvents btnMoveBack As System.Windows.Forms.Button
        Friend WithEvents btnMoveOver As System.Windows.Forms.Button
        Friend WithEvents lblOdorTypes As System.Windows.Forms.Label
        Friend WithEvents lblOdorSources As System.Windows.Forms.Label
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents lblProperties As System.Windows.Forms.Label
        Friend WithEvents lvOdorTypes As System.Windows.Forms.ListView
        Friend WithEvents lvOdorSources As System.Windows.Forms.ListView
        Friend WithEvents pgOdor As System.Windows.Forms.PropertyGrid
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lvOdorTypes = New System.Windows.Forms.ListView
            Me.btnAddOdorType = New System.Windows.Forms.Button
            Me.btnRemoveOdorType = New System.Windows.Forms.Button
            Me.pgOdor = New System.Windows.Forms.PropertyGrid
            Me.lvOdorSources = New System.Windows.Forms.ListView
            Me.btnMoveBack = New System.Windows.Forms.Button
            Me.btnMoveOver = New System.Windows.Forms.Button
            Me.lblOdorTypes = New System.Windows.Forms.Label
            Me.lblOdorSources = New System.Windows.Forms.Label
            Me.btnOk = New System.Windows.Forms.Button
            Me.lblProperties = New System.Windows.Forms.Label
            Me.SuspendLayout()
            '
            'lvOdorTypes
            '
            Me.lvOdorTypes.FullRowSelect = True
            Me.lvOdorTypes.HideSelection = False
            Me.lvOdorTypes.Location = New System.Drawing.Point(8, 24)
            Me.lvOdorTypes.MultiSelect = False
            Me.lvOdorTypes.Name = "lvOdorTypes"
            Me.lvOdorTypes.Size = New System.Drawing.Size(232, 264)
            Me.lvOdorTypes.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.lvOdorTypes.TabIndex = 0
            Me.lvOdorTypes.View = System.Windows.Forms.View.List
            '
            'btnAddOdorType
            '
            Me.btnAddOdorType.Location = New System.Drawing.Point(8, 296)
            Me.btnAddOdorType.Name = "btnAddOdorType"
            Me.btnAddOdorType.Size = New System.Drawing.Size(104, 32)
            Me.btnAddOdorType.TabIndex = 1
            Me.btnAddOdorType.Text = "Add Odor Type"
            '
            'btnRemoveOdorType
            '
            Me.btnRemoveOdorType.Location = New System.Drawing.Point(120, 296)
            Me.btnRemoveOdorType.Name = "btnRemoveOdorType"
            Me.btnRemoveOdorType.Size = New System.Drawing.Size(112, 32)
            Me.btnRemoveOdorType.TabIndex = 2
            Me.btnRemoveOdorType.Text = "Remove Odor Type"
            '
            'pgOdor
            '
            Me.pgOdor.CommandsVisibleIfAvailable = True
            Me.pgOdor.LargeButtons = False
            Me.pgOdor.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.pgOdor.Location = New System.Drawing.Point(544, 24)
            Me.pgOdor.Name = "pgOdor"
            Me.pgOdor.Size = New System.Drawing.Size(224, 264)
            Me.pgOdor.TabIndex = 3
            Me.pgOdor.Text = "PropertyGrid"
            Me.pgOdor.ToolbarVisible = False
            Me.pgOdor.ViewBackColor = System.Drawing.SystemColors.Window
            Me.pgOdor.ViewForeColor = System.Drawing.SystemColors.WindowText
            '
            'lvOdorSources
            '
            Me.lvOdorSources.FullRowSelect = True
            Me.lvOdorSources.HideSelection = False
            Me.lvOdorSources.Location = New System.Drawing.Point(296, 24)
            Me.lvOdorSources.MultiSelect = False
            Me.lvOdorSources.Name = "lvOdorSources"
            Me.lvOdorSources.Size = New System.Drawing.Size(232, 264)
            Me.lvOdorSources.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.lvOdorSources.TabIndex = 4
            Me.lvOdorSources.View = System.Windows.Forms.View.List
            '
            'btnMoveBack
            '
            Me.btnMoveBack.Location = New System.Drawing.Point(248, 144)
            Me.btnMoveBack.Name = "btnMoveBack"
            Me.btnMoveBack.Size = New System.Drawing.Size(40, 24)
            Me.btnMoveBack.TabIndex = 5
            Me.btnMoveBack.Text = "<<"
            '
            'btnMoveOver
            '
            Me.btnMoveOver.Location = New System.Drawing.Point(248, 120)
            Me.btnMoveOver.Name = "btnMoveOver"
            Me.btnMoveOver.Size = New System.Drawing.Size(40, 24)
            Me.btnMoveOver.TabIndex = 6
            Me.btnMoveOver.Text = ">>"
            '
            'lblOdorTypes
            '
            Me.lblOdorTypes.Location = New System.Drawing.Point(8, 8)
            Me.lblOdorTypes.Name = "lblOdorTypes"
            Me.lblOdorTypes.Size = New System.Drawing.Size(232, 16)
            Me.lblOdorTypes.TabIndex = 7
            Me.lblOdorTypes.Text = "Odor Types"
            Me.lblOdorTypes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblOdorSources
            '
            Me.lblOdorSources.Location = New System.Drawing.Point(296, 8)
            Me.lblOdorSources.Name = "lblOdorSources"
            Me.lblOdorSources.Size = New System.Drawing.Size(232, 16)
            Me.lblOdorSources.TabIndex = 8
            Me.lblOdorSources.Text = "Odor Sources"
            Me.lblOdorSources.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnOk
            '
            Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOk.Location = New System.Drawing.Point(656, 296)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(112, 32)
            Me.btnOk.TabIndex = 9
            Me.btnOk.Text = "Ok"
            '
            'lblProperties
            '
            Me.lblProperties.Location = New System.Drawing.Point(544, 8)
            Me.lblProperties.Name = "lblProperties"
            Me.lblProperties.Size = New System.Drawing.Size(232, 16)
            Me.lblProperties.TabIndex = 11
            Me.lblProperties.Text = "Odor Properties"
            Me.lblProperties.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'Odor
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(778, 334)
            Me.Controls.Add(Me.lblProperties)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.lblOdorSources)
            Me.Controls.Add(Me.lblOdorTypes)
            Me.Controls.Add(Me.btnMoveOver)
            Me.Controls.Add(Me.btnMoveBack)
            Me.Controls.Add(Me.lvOdorSources)
            Me.Controls.Add(Me.pgOdor)
            Me.Controls.Add(Me.btnRemoveOdorType)
            Me.Controls.Add(Me.btnAddOdorType)
            Me.Controls.Add(Me.lvOdorTypes)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "Odor"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Edit Odors"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_aryOdorSources As Collections.SortedOdors

#End Region

#Region " Properties "

        Public Property OdorSources() As Collections.SortedOdors
            Get
                Return m_aryOdorSources
            End Get
            Set(ByVal Value As Collections.SortedOdors)
                m_aryOdorSources = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                'm_btnCancel = Me.btnCancel

                lvOdorTypes.HideSelection = False
                lvOdorTypes.MultiSelect = False
                lvOdorTypes.FullRowSelect = True
                lvOdorTypes.View = View.Details
                lvOdorTypes.AllowColumnReorder = True
                lvOdorTypes.LabelEdit = False
                lvOdorTypes.Sorting = SortOrder.Ascending

                lvOdorTypes.Columns.Add("Odor Types", 200, HorizontalAlignment.Left)

                lvOdorSources.HideSelection = False
                lvOdorSources.MultiSelect = False
                lvOdorSources.FullRowSelect = True
                lvOdorSources.View = View.Details
                lvOdorSources.AllowColumnReorder = True
                lvOdorSources.LabelEdit = False
                lvOdorSources.Sorting = SortOrder.Ascending

                lvOdorSources.Columns.Add("Odor Sources", 200, HorizontalAlignment.Left)

                'Now go through and add all the odor types and then sources.
                Dim liItem As ListViewItem
                Dim doOdorType As DataObjects.Physical.OdorType
                For Each deEntry As DictionaryEntry In Util.Environment.OdorTypes
                    doOdorType = DirectCast(deEntry.Value, DataObjects.Physical.OdorType)
                    liItem = New ListViewItem(doOdorType.Name)
                    liItem.Tag = doOdorType
                    lvOdorTypes.Items.Add(liItem)
                Next

                Dim doOdor As DataObjects.Physical.Odor
                For Each deEntry As DictionaryEntry In m_aryOdorSources
                    doOdor = DirectCast(deEntry.Value, DataObjects.Physical.Odor)
                    liItem = New ListViewItem(doOdor.Name)
                    liItem.Tag = doOdor
                    lvOdorSources.Items.Add(liItem)
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Function FindOdorSourceItem(ByVal strTypeID As String) As ListViewItem

            Dim doOdor As AnimatGUI.DataObjects.Physical.Odor
            For Each liItem As ListViewItem In lvOdorSources.Items
                If Not liItem.Tag Is Nothing AndAlso TypeOf liItem.Tag Is AnimatGUI.DataObjects.Physical.Odor Then
                    doOdor = DirectCast(liItem.Tag, AnimatGUI.DataObjects.Physical.Odor)

                    If doOdor.OdorType.ID = strTypeID Then
                        Return liItem
                    End If
                End If
            Next

            Return Nothing
        End Function

#End Region

#Region " Events "

        Private Sub btnAddOdorType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddOdorType.Click
            Try
                Dim doOdor As New DataObjects.Physical.OdorType(Util.Environment)
                doOdor.Name = "New Odor"
                Util.Environment.OdorTypes.Add(doOdor.ID, doOdor)

                Dim liItem As ListViewItem
                liItem = New ListViewItem(doOdor.Name)
                liItem.Tag = doOdor

                lvOdorTypes.Items.Add(liItem)

                lvOdorTypes.SelectedItems.Clear()
                liItem.Selected = True

                pgOdor.SelectedObject = doOdor.Properties

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnRemoveOdorType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveOdorType.Click
            Try
                If lvOdorTypes.SelectedItems.Count > 0 Then
                    Dim liItem As ListViewItem = lvOdorTypes.SelectedItems(0)

                    If Not liItem.Tag Is Nothing AndAlso TypeOf liItem.Tag Is DataObjects.Physical.OdorType Then
                        Dim doOdor As DataObjects.Physical.OdorType = DirectCast(liItem.Tag, DataObjects.Physical.OdorType)

                        If doOdor.OdorSources.Count > 0 Then
                            Throw New System.Exception("This odor type has an odor source associated with it. You can only delete an odor type of if it is no longer associated with any sources.")
                        End If

                        If Util.Environment.OdorTypes.Contains(doOdor.ID) Then
                            Util.Environment.OdorTypes.Remove(doOdor.ID)
                        End If
                    End If

                    lvOdorTypes.Items.Remove(liItem)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub lvOdorTypes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvOdorTypes.Click

            Try

                If lvOdorTypes.SelectedItems.Count = 1 Then
                    Dim liItem As ListViewItem = lvOdorTypes.SelectedItems(0)

                    If Not liItem.Tag Is Nothing AndAlso TypeOf liItem.Tag Is DataObjects.Physical.OdorType Then
                        Dim doOdor As DataObjects.Physical.OdorType = DirectCast(liItem.Tag, DataObjects.Physical.OdorType)
                        pgOdor.SelectedObject = doOdor.Properties
                    Else
                        pgOdor.SelectedObject = Nothing
                    End If
                Else
                    pgOdor.SelectedObject = Nothing
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub lvOdorSources_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvOdorSources.Click

            Try

                If lvOdorSources.SelectedItems.Count = 1 Then
                    Dim liItem As ListViewItem = lvOdorSources.SelectedItems(0)

                    If Not liItem.Tag Is Nothing AndAlso TypeOf liItem.Tag Is DataObjects.Physical.Odor Then
                        Dim doOdor As DataObjects.Physical.Odor = DirectCast(liItem.Tag, DataObjects.Physical.Odor)
                        pgOdor.SelectedObject = doOdor.Properties
                    Else
                        pgOdor.SelectedObject = Nothing
                    End If
                Else
                    pgOdor.SelectedObject = Nothing
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub pgOdor_PropertyValueChanged(ByVal s As Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles pgOdor.PropertyValueChanged

            Try
                If e.ChangedItem.Label = "Name" Then
                    Dim doOdorType As DataObjects.Physical.OdorType
                    For Each liItem As ListViewItem In lvOdorTypes.Items
                        If Not liItem.Tag Is Nothing AndAlso TypeOf liItem.Tag Is DataObjects.Physical.OdorType Then
                            doOdorType = DirectCast(liItem.Tag, DataObjects.Physical.OdorType)

                            If doOdorType.Name <> liItem.Text Then
                                liItem.Text = doOdorType.Name
                            End If
                        End If
                    Next

                    Dim doOdor As DataObjects.Physical.Odor
                    For Each liItem As ListViewItem In lvOdorSources.Items
                        If Not liItem.Tag Is Nothing AndAlso TypeOf liItem.Tag Is DataObjects.Physical.Odor Then
                            doOdor = DirectCast(liItem.Tag, DataObjects.Physical.Odor)

                            If doOdor.Name <> liItem.Text Then
                                liItem.Text = doOdor.Name
                            End If
                        End If
                    Next

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub btnMoveOver_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveOver.Click

            Try

                If lvOdorTypes.SelectedItems.Count = 1 Then
                    Dim liItem As ListViewItem = lvOdorTypes.SelectedItems(0)

                    Dim doType As DataObjects.Physical.OdorType
                    If Not liItem.Tag Is Nothing AndAlso TypeOf liItem.Tag Is DataObjects.Physical.OdorType Then
                        doType = DirectCast(liItem.Tag, DataObjects.Physical.OdorType)

                        If Not FindOdorSourceItem(doType.ID) Is Nothing Then
                            Throw New System.Exception("There is already an odor source of the '" & doType.Name & "' type.")
                        End If

                        Dim doOdor As New DataObjects.Physical.Odor(m_aryOdorSources.Parent)
                        doOdor.OdorType = doType
                        m_aryOdorSources.Add(doOdor.ID, doOdor)

                        If Not doType.OdorSources.Contains(doOdor.ID) Then
                            doType.OdorSources.Add(doOdor.ID, doOdor, False)
                        End If

                        Dim liOdorItem As ListViewItem
                        liOdorItem = New ListViewItem(doOdor.Name)
                        liOdorItem.Tag = doOdor

                        lvOdorSources.Items.Add(liOdorItem)

                        lvOdorSources.SelectedItems.Clear()
                        liOdorItem.Selected = True

                        pgOdor.SelectedObject = doOdor.Properties
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnMoveBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveBack.Click
            Try
                If lvOdorSources.SelectedItems.Count = 1 Then
                    Dim liItem As ListViewItem = lvOdorSources.SelectedItems(0)

                    Dim doOdor As DataObjects.Physical.Odor
                    If Not liItem.Tag Is Nothing AndAlso TypeOf liItem.Tag Is DataObjects.Physical.Odor Then
                        doOdor = DirectCast(liItem.Tag, DataObjects.Physical.Odor)

                        If m_aryOdorSources.Contains(doOdor.ID) Then
                            m_aryOdorSources.Remove(doOdor.ID)
                        End If

                        If Not doOdor.OdorType Is Nothing AndAlso doOdor.OdorType.OdorSources.Contains(doOdor.ID) Then
                            doOdor.OdorType.OdorSources.Remove(doOdor.ID, False)
                        End If
                    End If

                    lvOdorSources.Items.Remove(liItem)
                    pgOdor.SelectedObject = Nothing
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
