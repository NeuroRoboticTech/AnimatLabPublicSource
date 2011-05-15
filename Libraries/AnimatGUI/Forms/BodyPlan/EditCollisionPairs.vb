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
Imports System.Drawing.Imaging

Namespace Forms.BodyPlan

    Public Class EditCollisionPairs
        Inherits Crownwood.DotNetMagic.Forms.DotNetMagicForm

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()
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
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents cboBodyPart1 As System.Windows.Forms.ComboBox
        Friend WithEvents cboBodyPart2 As System.Windows.Forms.ComboBox
        Friend WithEvents btnAdd As System.Windows.Forms.Button
        Friend WithEvents btnDelete As System.Windows.Forms.Button
        Friend WithEvents lblBodyPart1 As System.Windows.Forms.Label
        Friend WithEvents lblBodyPart2 As System.Windows.Forms.Label
        Friend WithEvents lvCollisionPairs As System.Windows.Forms.ListView
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.btnOk = New System.Windows.Forms.Button
            Me.lvCollisionPairs = New System.Windows.Forms.ListView
            Me.cboBodyPart1 = New System.Windows.Forms.ComboBox
            Me.cboBodyPart2 = New System.Windows.Forms.ComboBox
            Me.btnAdd = New System.Windows.Forms.Button
            Me.btnDelete = New System.Windows.Forms.Button
            Me.lblBodyPart1 = New System.Windows.Forms.Label
            Me.lblBodyPart2 = New System.Windows.Forms.Label
            Me.btnCancel = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(8, 232)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 5
            Me.btnOk.Text = "Ok"
            '
            'lvCollisionPairs
            '
            Me.lvCollisionPairs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lvCollisionPairs.Location = New System.Drawing.Point(152, 8)
            Me.lvCollisionPairs.Name = "lvCollisionPairs"
            Me.lvCollisionPairs.Size = New System.Drawing.Size(224, 248)
            Me.lvCollisionPairs.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.lvCollisionPairs.TabIndex = 7
            Me.lvCollisionPairs.View = System.Windows.Forms.View.List
            '
            'cboBodyPart1
            '
            Me.cboBodyPart1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboBodyPart1.Location = New System.Drawing.Point(8, 24)
            Me.cboBodyPart1.Name = "cboBodyPart1"
            Me.cboBodyPart1.Size = New System.Drawing.Size(136, 21)
            Me.cboBodyPart1.Sorted = True
            Me.cboBodyPart1.TabIndex = 8
            '
            'cboBodyPart2
            '
            Me.cboBodyPart2.Location = New System.Drawing.Point(8, 72)
            Me.cboBodyPart2.Name = "cboBodyPart2"
            Me.cboBodyPart2.Size = New System.Drawing.Size(136, 21)
            Me.cboBodyPart2.Sorted = True
            Me.cboBodyPart2.TabIndex = 9
            '
            'btnAdd
            '
            Me.btnAdd.Location = New System.Drawing.Point(8, 112)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(64, 24)
            Me.btnAdd.TabIndex = 10
            Me.btnAdd.Text = "Add"
            '
            'btnDelete
            '
            Me.btnDelete.Location = New System.Drawing.Point(80, 112)
            Me.btnDelete.Name = "btnDelete"
            Me.btnDelete.Size = New System.Drawing.Size(64, 24)
            Me.btnDelete.TabIndex = 11
            Me.btnDelete.Text = "Delete"
            '
            'lblBodyPart1
            '
            Me.lblBodyPart1.Location = New System.Drawing.Point(8, 8)
            Me.lblBodyPart1.Name = "lblBodyPart1"
            Me.lblBodyPart1.Size = New System.Drawing.Size(136, 16)
            Me.lblBodyPart1.TabIndex = 12
            Me.lblBodyPart1.Text = "Body Part 1"
            Me.lblBodyPart1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblBodyPart2
            '
            Me.lblBodyPart2.Location = New System.Drawing.Point(8, 56)
            Me.lblBodyPart2.Name = "lblBodyPart2"
            Me.lblBodyPart2.Size = New System.Drawing.Size(136, 16)
            Me.lblBodyPart2.TabIndex = 13
            Me.lblBodyPart2.Text = "Body Part 2"
            Me.lblBodyPart2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(80, 232)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 14
            Me.btnCancel.Text = "Cancel"
            '
            'EditCollisionPairs
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(384, 262)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.lblBodyPart2)
            Me.Controls.Add(Me.lblBodyPart1)
            Me.Controls.Add(Me.btnDelete)
            Me.Controls.Add(Me.btnAdd)
            Me.Controls.Add(Me.cboBodyPart2)
            Me.Controls.Add(Me.cboBodyPart1)
            Me.Controls.Add(Me.lvCollisionPairs)
            Me.Controls.Add(Me.btnOk)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "EditCollisionPairs"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Edit Collision Exclusion Pairs"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_aryCollisionPairs As AnimatGUI.Collections.CollisionPairs
        Protected m_aryBodyParts As New AnimatGUI.Collections.DataObjects(Nothing)
        Protected m_bIsDirty As Boolean = False

#End Region

#Region " Properties "

        Public Property CollisionPairs() As AnimatGUI.Collections.CollisionPairs
            Get
                Return m_aryCollisionPairs
            End Get
            Set(ByVal Value As AnimatGUI.Collections.CollisionPairs)
                m_aryCollisionPairs = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)

            Try

                If m_aryCollisionPairs Is Nothing Then
                    Throw New System.Exception("The collision pairs array is empty!")
                End If

                If Not m_aryCollisionPairs.Parent Is Nothing AndAlso TypeOf m_aryCollisionPairs.Parent Is AnimatGUI.DataObjects.Physical.PhysicalStructure Then
                    Dim doParent As AnimatGUI.DataObjects.Physical.PhysicalStructure = DirectCast(m_aryCollisionPairs.Parent, AnimatGUI.DataObjects.Physical.PhysicalStructure)

                    doParent.FindChildrenOfType(GetType(AnimatGUI.DataObjects.Physical.RigidBody), m_aryBodyParts)

                    'Lets populate the two drop down boxes.
                    Dim doBody As AnimatGUI.DataObjects.Physical.RigidBody
                    For Each doPart As AnimatGUI.Framework.DataObject In m_aryBodyParts
                        doBody = DirectCast(doPart, AnimatGUI.DataObjects.Physical.RigidBody)

                        If doBody.HasDynamics Then
                            cboBodyPart1.Items.Add(doBody)
                            cboBodyPart2.Items.Add(doBody)
                        End If
                    Next

                    'Lets populate the listview
                    Dim doPair As AnimatGUI.DataObjects.Physical.CollisionPair
                    lvCollisionPairs.Items.Clear()
                    For Each doPair In m_aryCollisionPairs
                        Dim liItem As New ListViewItem(doPair.ToString())
                        liItem.Tag = doPair
                        lvCollisionPairs.Items.Add(liItem)
                    Next

                Else
                    Throw New System.Exception("The parent of the collision pairs array is either null or not of the correct type.")
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " Events "

        Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click

            Try
                'First make sure we have two objects selected in the combo boxes
                If cboBodyPart1.SelectedItem Is Nothing Then
                    Throw New System.Exception("You have not specified an item for the first body part.")
                End If

                If cboBodyPart2.SelectedItem Is Nothing Then
                    Throw New System.Exception("You have not specified an item for the second body part.")
                End If

                'Now make sure that the two selected body parts are not the same part.
                If cboBodyPart1.SelectedItem Is cboBodyPart2.SelectedItem Then
                    Throw New System.Exception("You can not disable collisions between a part and itself. Please choose two different parts.")
                End If

                'Now make sure this pair of collisions is not already in the list.
                Dim doPart1 As AnimatGUI.DataObjects.Physical.RigidBody = DirectCast(cboBodyPart1.SelectedItem, AnimatGUI.DataObjects.Physical.RigidBody)
                Dim doPart2 As AnimatGUI.DataObjects.Physical.RigidBody = DirectCast(cboBodyPart2.SelectedItem, AnimatGUI.DataObjects.Physical.RigidBody)

                Dim doPair As AnimatGUI.DataObjects.Physical.CollisionPair
                For Each doPair In m_aryCollisionPairs
                    If doPair.Part1 Is doPart1 Then
                        If doPair.Part2 Is doPart2 Then
                            Throw New System.Exception("The pair (" & doPart1.Name & ", " & doPart2.Name & ") is already in the list.")
                        End If
                    ElseIf doPair.Part1 Is doPart2 Then
                        If doPair.Part2 Is doPart1 Then
                            Throw New System.Exception("The pair (" & doPart1.Name & ", " & doPart2.Name & ") is already in the list.")
                        End If
                    End If
                Next

                'Okay then lets add it to the list.
                doPair = New AnimatGUI.DataObjects.Physical.CollisionPair(m_aryCollisionPairs.Parent, doPart1, doPart2)
                m_aryCollisionPairs.Add(doPair)
                Dim liItem As New ListViewItem(doPair.ToString())
                liItem.Tag = doPair
                lvCollisionPairs.Items.Add(liItem)
                m_bIsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click

            Try
                For Each liItem As ListViewItem In lvCollisionPairs.SelectedItems
                    m_aryCollisionPairs.Remove(DirectCast(liItem.Tag, AnimatGUI.DataObjects.Physical.CollisionPair))
                Next

                lvCollisionPairs.Items.Clear()
                Dim doPair As AnimatGUI.DataObjects.Physical.CollisionPair
                lvCollisionPairs.Items.Clear()
                For Each doPair In m_aryCollisionPairs
                    Dim liItem As New ListViewItem(doPair.ToString())
                    liItem.Tag = doPair
                    lvCollisionPairs.Items.Add(liItem)
                Next

                m_bIsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click

            Try
                If m_bIsDirty Then
                    Me.DialogResult = DialogResult.OK
                Else
                    Me.DialogResult = DialogResult.Cancel
                End If
                Me.Close()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
