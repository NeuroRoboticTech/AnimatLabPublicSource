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

    Public Class EditAttachments
        Inherits AnimatGUI.Forms.AnimatDialog

#Region " Attributes "

        Protected m_aryAttachments As Collections.Attachments
        Protected m_doStructure As DataObjects.Physical.PhysicalStructure
        Protected m_bIsDirty As Boolean = False
        Protected m_iMaxAttachmentsAllowed As Integer = -1

#End Region

#Region " Properties "

        Public Property Attachments() As Collections.Attachments
            Get
                Return m_aryAttachments
            End Get
            Set(ByVal value As Collections.Attachments)
                m_aryAttachments = value
            End Set
        End Property

        Public Property ParentStructure() As DataObjects.Physical.PhysicalStructure
            Get
                Return m_doStructure
            End Get
            Set(ByVal value As DataObjects.Physical.PhysicalStructure)
                m_doStructure = value
            End Set
        End Property

        Public Property MaxAttachmentsAllowed() As Integer
            Get
                Return m_iMaxAttachmentsAllowed
            End Get
            Set(ByVal value As Integer)
                m_iMaxAttachmentsAllowed = value
            End Set
        End Property

#End Region

#Region " Methods "

        Protected Function FindListItem(ByVal lView As ListView, ByVal strID As String, ByVal bThrowError As Boolean) As ListViewItem

            For Each liItem As ListViewItem In lView.Items
                If Not liItem.Tag Is Nothing AndAlso Util.IsTypeOf(liItem.Tag.GetType, GetType(AnimatGUI.Framework.DataObject), True) Then
                    Dim doObject As AnimatGUI.Framework.DataObject = DirectCast(liItem.Tag, AnimatGUI.Framework.DataObject)
                    If doObject.ID = strID Then
                        Return liItem
                    End If
                End If
            Next

            If bThrowError Then
                Throw New System.Exception("No attachment was found with the ID: " & strID)
            End If

            Return Nothing
        End Function

        Protected Function FindListItemByName(ByVal lView As ListView, ByVal strName As String, ByVal bThrowError As Boolean) As ListViewItem

            For Each liItem As ListViewItem In lView.Items
                If Not liItem.Tag Is Nothing AndAlso Util.IsTypeOf(liItem.Tag.GetType, GetType(AnimatGUI.Framework.DataObject), True) Then
                    Dim doObject As AnimatGUI.Framework.DataObject = DirectCast(liItem.Tag, AnimatGUI.Framework.DataObject)
                    If doObject.Name = strName Then
                        Return liItem
                    End If
                End If
            Next

            If bThrowError Then
                Throw New System.Exception("No attachment was found with the name: " & strName)
            End If

            Return Nothing
        End Function

#End Region

#Region " Events "


        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel
                m_lvItems = lvMuscleAttachments

                If m_aryAttachments Is Nothing Then
                    Throw New System.Exception("The attachments array is not defined.")
                End If

                If m_doStructure Is Nothing Then
                    Throw New System.Exception("The parent structure is not defined.")
                End If

                'now lets populate the drop down box with all of the muscle attachments.
                Dim aryAttachments As Collections.DataObjects = New Collections.DataObjects(Nothing)
                m_doStructure.FindChildrenOfType(GetType(DataObjects.Physical.Bodies.Attachment), aryAttachments)

                'Now lets populate the list view with the current muscle attachments.
                For Each doAttach As DataObjects.Physical.Bodies.Attachment In aryAttachments
                    Dim liItem As New ListViewItem(doAttach.ToString)
                    liItem.Tag = doAttach
                    lvAttachments.Items.Add(liItem)
                Next

                'Now lets populate the list box with the current muscle attachments.
                Dim liFindItem As ListViewItem
                For Each doAttach As DataObjects.Physical.Bodies.Attachment In m_aryAttachments
                    Dim liItem As New ListViewItem(doAttach.ToString)
                    liItem.Tag = doAttach
                    lvMuscleAttachments.Items.Add(liItem)

                    liFindItem = FindListItem(lvAttachments, doAttach.ID, False)
                    If Not liFindItem Is Nothing Then
                        lvAttachments.Items.Remove(liFindItem)
                    End If
                Next

            Catch ex As System.Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnUp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUp.Click
            Try
                If lvMuscleAttachments.SelectedItems.Count <= 0 Then
                    Throw New System.Exception("Please select an item to move up.")
                End If

                If lvMuscleAttachments.SelectedItems.Count > 1 Then
                    Throw New System.Exception("You can only move one item up at a time.")
                End If
                Dim liItem As ListViewItem = lvMuscleAttachments.SelectedItems(0)
                Dim iIndex As Integer = liItem.Index

                If iIndex > 0 Then
                    lvMuscleAttachments.Items.Remove(liItem)
                    lvMuscleAttachments.Items.Insert(iIndex - 1, liItem)
                End If

                m_bIsDirty = True

            Catch ex As System.Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnDown_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDown.Click
            Try
                If lvMuscleAttachments.SelectedItems.Count <= 0 Then
                    Throw New System.Exception("Please select an item to move down.")
                End If

                If lvMuscleAttachments.SelectedItems.Count > 1 Then
                    Throw New System.Exception("You can only move one item down at a time.")
                End If
                Dim liItem As ListViewItem = lvMuscleAttachments.SelectedItems(0)
                Dim iIndex As Integer = liItem.Index

                If iIndex > 0 Then
                    lvMuscleAttachments.Items.Remove(liItem)
                    lvMuscleAttachments.Items.Insert(iIndex + 1, liItem)
                End If

                m_bIsDirty = True

            Catch ex As System.Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub AddAttachment(ByVal liSelItem As ListViewItem, ByVal aryItems As ArrayList)
            Try
                For Each liTempItem As ListViewItem In lvMuscleAttachments.Items
                    If liTempItem.Tag Is liSelItem.Tag Then
                        Dim doAttach1 As DataObjects.Physical.Bodies.Attachment = DirectCast(liTempItem.Tag, DataObjects.Physical.Bodies.Attachment)
                        Throw New System.Exception("The muscle attachment '" + doAttach1.Name + "' is already in the list.")
                    End If
                Next

                Dim doAttach As DataObjects.Physical.Bodies.Attachment = DirectCast(liSelItem.Tag, DataObjects.Physical.Bodies.Attachment)

                Dim liItem As New ListViewItem(doAttach.ToString())
                liItem.Tag = doAttach
                lvMuscleAttachments.Items.Add(liItem)

                aryItems.Add(liSelItem)

            Catch ex As System.Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
            Try

                If lvAttachments.SelectedItems.Count <= 0 Then
                    Throw New System.Exception("you must select a muscle attachment to add.")
                End If
                Dim aryItems As New ArrayList()

                For Each liItem As ListViewItem In lvAttachments.SelectedItems
                    AddAttachment(liItem, aryItems)
                Next

                'now remove all the selected attachments that were added
                For Each liItem As ListViewItem In aryItems
                    lvAttachments.Items.Remove(liItem)
                Next

                If lvAttachments.Items.Count > 0 Then
                    lvAttachments.Items(0).Selected = True
                End If

                m_bIsDirty = True

            Catch ex As System.Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
            Try
                Dim aryList As New ArrayList()
                For Each liItem As ListViewItem In lvMuscleAttachments.SelectedItems
                    aryList.Add(liItem)
                Next

                For Each liItem As ListViewItem In aryList
                    lvMuscleAttachments.Items.Remove(liItem)
                    lvAttachments.Items.Add(liItem)
                Next

                If lvMuscleAttachments.Items.Count > 0 Then
                    lvMuscleAttachments.Items(0).Selected = True
                End If

                m_bIsDirty = True

            Catch ex As System.Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                If m_bIsDirty Then
                    'Verify that the number of attachments that were selected is not more than the number allowed
                    If m_iMaxAttachmentsAllowed > 0 AndAlso lvMuscleAttachments.Items.Count > m_iMaxAttachmentsAllowed Then
                        Throw New System.Exception("Only " & m_iMaxAttachmentsAllowed & " are allowed for this part type. " & _
                                                   "Please reduce the number of attachments to this number.")
                    End If

                    m_aryAttachments.Clear()

                    For Each liItem As ListViewItem In lvMuscleAttachments.Items
                        m_aryAttachments.Add(DirectCast(liItem.Tag, DataObjects.Physical.Bodies.Attachment))
                    Next

                    Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Else
                    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
                End If

                Me.Close()

            Catch ex As System.Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Public Sub Automation_AddAttachment(ByVal strName As String)

            Dim liItem As ListViewItem = FindListItemByName(Me.lvAttachments, strName, True)
            lvAttachments.SelectedItems.Clear()
            liItem.Selected = True

            btnAdd_Click(Nothing, Nothing)
        End Sub

        Public Sub Automation_RemoveAttachment(ByVal strName As String)

            Dim liItem As ListViewItem = FindListItemByName(Me.lvMuscleAttachments, strName, True)
            lvMuscleAttachments.SelectedItems.Clear()
            liItem.Selected = True

            btnDelete_Click(Nothing, Nothing)
        End Sub

        Public Sub Automation_AttachmentUp(ByVal strName As String)

            Dim liItem As ListViewItem = FindListItemByName(Me.lvMuscleAttachments, strName, True)
            lvMuscleAttachments.SelectedItems.Clear()
            liItem.Selected = True

            btnUp_Click(Nothing, Nothing)
        End Sub

        Public Sub Automation_AttachmentDown(ByVal strName As String)

            Dim liItem As ListViewItem = FindListItemByName(Me.lvMuscleAttachments, strName, True)
            lvMuscleAttachments.SelectedItems.Clear()
            liItem.Selected = True

            btnDown_Click(Nothing, Nothing)
        End Sub

#End Region

    End Class

End Namespace
