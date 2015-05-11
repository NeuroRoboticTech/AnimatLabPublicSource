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

Namespace Forms
    Namespace Robotics

        Public Class EditRemoteLinkages
            Inherits Forms.AnimatDialog

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
            Friend WithEvents lvLinkages As System.Windows.Forms.ListView
            Friend WithEvents Linkage As System.Windows.Forms.ColumnHeader
            Friend WithEvents pgLinkProperties As System.Windows.Forms.PropertyGrid
            Friend WithEvents btnAddLink As System.Windows.Forms.Button
            Friend WithEvents btnDeleteLink As System.Windows.Forms.Button
            Friend WithEvents btnAddOutputLink As System.Windows.Forms.Button
            Friend WithEvents btnOk As System.Windows.Forms.Button
            <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
                Me.btnOk = New System.Windows.Forms.Button()
                Me.lvLinkages = New System.Windows.Forms.ListView()
                Me.Linkage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
                Me.pgLinkProperties = New System.Windows.Forms.PropertyGrid()
                Me.btnAddLink = New System.Windows.Forms.Button()
                Me.btnDeleteLink = New System.Windows.Forms.Button()
                Me.btnAddOutputLink = New System.Windows.Forms.Button()
                Me.SuspendLayout()
                '
                'btnOk
                '
                Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.btnOk.Location = New System.Drawing.Point(312, 432)
                Me.btnOk.Name = "btnOk"
                Me.btnOk.Size = New System.Drawing.Size(64, 24)
                Me.btnOk.TabIndex = 12
                Me.btnOk.Text = "Ok"
                '
                'lvLinkages
                '
                Me.lvLinkages.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.lvLinkages.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Linkage})
                Me.lvLinkages.FullRowSelect = True
                Me.lvLinkages.HideSelection = False
                Me.lvLinkages.Location = New System.Drawing.Point(2, 13)
                Me.lvLinkages.MultiSelect = False
                Me.lvLinkages.Name = "lvLinkages"
                Me.lvLinkages.Size = New System.Drawing.Size(134, 412)
                Me.lvLinkages.Sorting = System.Windows.Forms.SortOrder.Ascending
                Me.lvLinkages.TabIndex = 14
                Me.lvLinkages.UseCompatibleStateImageBehavior = False
                Me.lvLinkages.View = System.Windows.Forms.View.Details
                '
                'Linkage
                '
                Me.Linkage.Width = 130
                '
                'pgLinkProperties
                '
                Me.pgLinkProperties.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
                Me.pgLinkProperties.LineColor = System.Drawing.SystemColors.ScrollBar
                Me.pgLinkProperties.Location = New System.Drawing.Point(142, 13)
                Me.pgLinkProperties.Name = "pgLinkProperties"
                Me.pgLinkProperties.Size = New System.Drawing.Size(234, 411)
                Me.pgLinkProperties.TabIndex = 15
                Me.pgLinkProperties.ToolbarVisible = False
                '
                'btnAddLink
                '
                Me.btnAddLink.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.btnAddLink.Location = New System.Drawing.Point(2, 432)
                Me.btnAddLink.Name = "btnAddLink"
                Me.btnAddLink.Size = New System.Drawing.Size(100, 25)
                Me.btnAddLink.TabIndex = 16
                Me.btnAddLink.Text = "Add Input Link"
                '
                'btnDeleteLink
                '
                Me.btnDeleteLink.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.btnDeleteLink.Enabled = False
                Me.btnDeleteLink.Location = New System.Drawing.Point(214, 432)
                Me.btnDeleteLink.Name = "btnDeleteLink"
                Me.btnDeleteLink.Size = New System.Drawing.Size(81, 25)
                Me.btnDeleteLink.TabIndex = 17
                Me.btnDeleteLink.Text = "Delete Link"
                '
                'btnAddOutputLink
                '
                Me.btnAddOutputLink.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
                Me.btnAddOutputLink.Location = New System.Drawing.Point(108, 432)
                Me.btnAddOutputLink.Name = "btnAddOutputLink"
                Me.btnAddOutputLink.Size = New System.Drawing.Size(100, 25)
                Me.btnAddOutputLink.TabIndex = 18
                Me.btnAddOutputLink.Text = "Add Output Link"
                '
                'EditRemoteLinkages
                '
                Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
                Me.ClientSize = New System.Drawing.Size(388, 463)
                Me.Controls.Add(Me.btnAddOutputLink)
                Me.Controls.Add(Me.btnDeleteLink)
                Me.Controls.Add(Me.btnAddLink)
                Me.Controls.Add(Me.pgLinkProperties)
                Me.Controls.Add(Me.lvLinkages)
                Me.Controls.Add(Me.btnOk)
                Me.Name = "EditRemoteLinkages"
                Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
                Me.Text = "Edit Remote Linkages"
                Me.ResumeLayout(False)

            End Sub

#End Region

#Region " Attributes "

            Protected m_doRemoteControl As AnimatGUI.DataObjects.Robotics.RemoteControl
            Protected m_doSelectedLink As AnimatGUI.DataObjects.Robotics.RemoteControlLinkage
            Protected m_liSelectedItem As ListViewItem

#End Region

#Region " Properties "

            Public Property RemoteControl() As AnimatGUI.DataObjects.Robotics.RemoteControl
                Get
                    Return m_doRemoteControl
                End Get
                Set(ByVal value As AnimatGUI.DataObjects.Robotics.RemoteControl)
                    m_doRemoteControl = value
                End Set
            End Property

#End Region

#Region " Methods "

#Region "Automation Methods"


#End Region

#End Region

#Region " Events "

            Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

                Try
                    Me.DialogResult = DialogResult.OK
                    Me.Close()

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try

            End Sub

            Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
                Me.DialogResult = DialogResult.Cancel
                Me.Close()
            End Sub

            Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
                MyBase.OnLoad(e)

                Try
                    m_btnOk = Me.btnOk

                    'Fill out the list view with all of the Linkages in this remote control
                    If Not m_doRemoteControl Is Nothing Then
                        For Each deEntry As DictionaryEntry In m_doRemoteControl.Links
                            Dim doLink As AnimatGUI.DataObjects.Robotics.RemoteControlLinkage = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Robotics.RemoteControlLinkage)
                            Dim liItem As New ListViewItem(doLink.ToString)
                            liItem.Tag = doLink
                            lvLinkages.Items.Add(liItem)
                        Next
                    End If

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

            Private Sub AddLinkage(ByVal bInLink As Boolean)
                Dim frmSelInterface As New Forms.SelectObject()
                frmSelInterface.Objects = Util.Application.RemoteControlLinkages
                frmSelInterface.PartTypeName = "RemoteControlLinkage"

                If frmSelInterface.ShowDialog() = DialogResult.OK Then
                    'Then create the new one.
                    Dim doLink As DataObjects.Robotics.RemoteControlLinkage = DirectCast(frmSelInterface.Selected.Clone(m_doRemoteControl, False, Nothing), DataObjects.Robotics.RemoteControlLinkage)
                    doLink.InLink = bInLink
                    doLink.Name = "New " + doLink.Name

                    If bInLink Then
                        doLink.LinkedSource = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(m_doRemoteControl)
                    Else
                        doLink.LinkedTarget = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(m_doRemoteControl)
                    End If

                    m_doRemoteControl.Links.Add(doLink.ID, doLink)
                    Dim liItem As New ListViewItem(doLink.ToString)
                    liItem.Tag = doLink
                    lvLinkages.Items.Add(liItem)

                    lvLinkages.SelectedItems.Clear()
                    liItem.Selected = True
                End If

            End Sub

            Private Sub btnAddInputLink_Click(sender As System.Object, e As System.EventArgs) Handles btnAddLink.Click
                Try
                    AddLinkage(True)
                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

            Private Sub btnAddOutputLink_Click(sender As System.Object, e As System.EventArgs) Handles btnAddOutputLink.Click
                Try
                    AddLinkage(False)
                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

            Private Sub btnDeleteLink_Click(sender As System.Object, e As System.EventArgs) Handles btnDeleteLink.Click
                Try
                    If Not m_doSelectedLink Is Nothing Then
                        m_doRemoteControl.Links.Remove(m_doSelectedLink.ID)
                        lvLinkages.Items.Remove(m_liSelectedItem)
                    End If
                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

            Private Sub lvLinkages_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvLinkages.SelectedIndexChanged
                Try
                    If lvLinkages.SelectedItems.Count = 1 Then
                        Dim liItem As ListViewItem = lvLinkages.SelectedItems(0)

                        If Not liItem Is Nothing Then
                            Dim doTag As Object = liItem.Tag
                            If Not doTag Is Nothing Then
                                m_doSelectedLink = DirectCast(doTag, DataObjects.Robotics.RemoteControlLinkage)
                                m_liSelectedItem = liItem
                                pgLinkProperties.SelectedObject = m_doSelectedLink.Properties
                                btnDeleteLink.Enabled = True
                                Return
                            End If
                        End If
                    End If

                    'Else
                    btnDeleteLink.Enabled = False
                    m_doSelectedLink = Nothing
                    m_liSelectedItem = Nothing
                    pgLinkProperties.SelectedObject = Nothing

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

            Private Sub pgLinkProperties_PropertyValueChanged(s As System.Object, e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles pgLinkProperties.PropertyValueChanged
                Try
                    If Not m_liSelectedItem Is Nothing AndAlso e.ChangedItem.Label = "Name" Then
                        m_liSelectedItem.Text = m_doSelectedLink.Name
                    End If
                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

#End Region

        End Class

    End Namespace
End Namespace
