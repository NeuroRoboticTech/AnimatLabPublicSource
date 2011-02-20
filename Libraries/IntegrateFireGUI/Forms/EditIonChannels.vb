Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects

Namespace Forms

    Public Class EditIonChannels
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
        Friend WithEvents ctrlProperties As System.Windows.Forms.PropertyGrid
        Friend WithEvents btnAdd As System.Windows.Forms.Button
        Friend WithEvents btnRemove As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents lvIonChannels As System.Windows.Forms.ListView
        Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.ctrlProperties = New System.Windows.Forms.PropertyGrid
            Me.btnAdd = New System.Windows.Forms.Button
            Me.btnRemove = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnOk = New System.Windows.Forms.Button
            Me.lvIonChannels = New System.Windows.Forms.ListView
            Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
            Me.SuspendLayout()
            '
            'ctrlProperties
            '
            Me.ctrlProperties.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ctrlProperties.CommandsVisibleIfAvailable = True
            Me.ctrlProperties.LargeButtons = False
            Me.ctrlProperties.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.ctrlProperties.Location = New System.Drawing.Point(136, 8)
            Me.ctrlProperties.Name = "ctrlProperties"
            Me.ctrlProperties.Size = New System.Drawing.Size(280, 352)
            Me.ctrlProperties.TabIndex = 1
            Me.ctrlProperties.Text = "PropertyGrid1"
            Me.ctrlProperties.ViewBackColor = System.Drawing.SystemColors.Window
            Me.ctrlProperties.ViewForeColor = System.Drawing.SystemColors.WindowText
            '
            'btnAdd
            '
            Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnAdd.Location = New System.Drawing.Point(8, 304)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(56, 24)
            Me.btnAdd.TabIndex = 3
            Me.btnAdd.Text = "Add"
            '
            'btnRemove
            '
            Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnRemove.Location = New System.Drawing.Point(72, 304)
            Me.btnRemove.Name = "btnRemove"
            Me.btnRemove.Size = New System.Drawing.Size(56, 24)
            Me.btnRemove.TabIndex = 4
            Me.btnRemove.Text = "Remove"
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(72, 336)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(56, 24)
            Me.btnCancel.TabIndex = 6
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.Visible = False
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOk.Location = New System.Drawing.Point(8, 336)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(56, 24)
            Me.btnOk.TabIndex = 5
            Me.btnOk.Text = "Ok"
            '
            'lvIonChannels
            '
            Me.lvIonChannels.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lvIonChannels.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
            Me.lvIonChannels.FullRowSelect = True
            Me.lvIonChannels.GridLines = True
            Me.lvIonChannels.HideSelection = False
            Me.lvIonChannels.Location = New System.Drawing.Point(8, 8)
            Me.lvIonChannels.MultiSelect = False
            Me.lvIonChannels.Name = "lvIonChannels"
            Me.lvIonChannels.Size = New System.Drawing.Size(120, 288)
            Me.lvIonChannels.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.lvIonChannels.TabIndex = 7
            Me.lvIonChannels.View = System.Windows.Forms.View.Details
            '
            'ColumnHeader1
            '
            Me.ColumnHeader1.Text = "Ion Channels"
            Me.ColumnHeader1.Width = 115
            '
            'EditIonChannels
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(424, 366)
            Me.Controls.Add(Me.lvIonChannels)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.btnRemove)
            Me.Controls.Add(Me.btnAdd)
            Me.Controls.Add(Me.ctrlProperties)
            Me.Name = "EditIonChannels"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Edit Ion Channels"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_aryIonChannels As Collections.IonChannels
        Protected m_doNeuron As DataObjects.Behavior.Neurons.Spiking

#End Region

#Region " Properties "

        Public Overridable Property IonChannels() As Collections.IonChannels
            Get
                Return m_aryIonChannels
            End Get
            Set(ByVal Value As Collections.IonChannels)
                m_aryIonChannels = Value
                If Not m_aryIonChannels Is Nothing AndAlso Not m_aryIonChannels.Parent Is Nothing Then
                    If TypeOf m_aryIonChannels.Parent Is DataObjects.Behavior.Neurons.Spiking Then
                        m_doNeuron = DirectCast(m_aryIonChannels.Parent, DataObjects.Behavior.Neurons.Spiking)
                    End If
                End If
            End Set
        End Property

#End Region

#Region " Methods "

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)

            Try

                lvIonChannels.Items.Clear()

                If Not Me.IonChannels Is Nothing AndAlso Me.IonChannels.Count > 0 Then
                    Dim iIndex As Integer = 0
                    Dim doChannel As DataObjects.Behavior.Neurons.IonChannel
                    Dim liItem As ListViewItem
                    For Each deEntry As DictionaryEntry In IonChannels
                        doChannel = DirectCast(deEntry.Value, DataObjects.Behavior.Neurons.IonChannel)

                        liItem = New ListViewItem(doChannel.Name)
                        liItem.Tag = doChannel
                        doChannel.ListItem = liItem

                        lvIonChannels.Items.Add(liItem)

                        If iIndex = 0 Then
                            liItem.Selected = True
                        End If
                        iIndex = iIndex + 1
                    Next
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click

            Try
                If m_doNeuron Is Nothing Then
                    Throw New System.Exception("The parent neuron is not defined.")
                End If

                Dim doChannel As DataObjects.Behavior.Neurons.IonChannel = New DataObjects.Behavior.Neurons.IonChannel(m_doNeuron)
                doChannel.Name = "New Channel"
                m_aryIonChannels.Add(doChannel.ID, doChannel, True)

                Dim liItem As New ListViewItem(doChannel.Name)
                liItem.Tag = doChannel
                doChannel.ListItem = liItem

                lvIonChannels.Items.Add(liItem)

                liItem.Selected = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click

            Try
                If lvIonChannels.SelectedItems.Count > 0 Then
                    Dim liItem As ListViewItem = lvIonChannels.SelectedItems(0)

                    If Not liItem.Tag Is Nothing Then
                        Dim doChannel As DataObjects.Behavior.Neurons.IonChannel = DirectCast(liItem.Tag, DataObjects.Behavior.Neurons.IonChannel)
                        If m_aryIonChannels.Contains(doChannel.ID) Then
                            m_aryIonChannels.Remove(doChannel.ID, True)
                        End If
                    End If

                    lvIonChannels.Items.Remove(liItem)

                    If lvIonChannels.Items.Count > 0 Then
                        liItem = lvIonChannels.Items(0)
                        liItem.Selected = True
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub lvIonChannels_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvIonChannels.SelectedIndexChanged

            Try
                If lvIonChannels.SelectedItems.Count > 0 Then
                    Dim liItem As ListViewItem = lvIonChannels.SelectedItems(0)

                    If Not liItem.Tag Is Nothing Then
                        Dim doChannel As DataObjects.Behavior.Neurons.IonChannel = DirectCast(liItem.Tag, DataObjects.Behavior.Neurons.IonChannel)
                        ctrlProperties.SelectedObject = doChannel.Properties
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region


    End Class

End Namespace
