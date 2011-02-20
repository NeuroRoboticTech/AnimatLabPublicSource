Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace Framework.UndoSystem

    Public Class ModificationHistory

#Region " Attributes "

        Protected m_stkUndoStack As New AnimatGUI.Framework.UndoSystem.HistoryStack
        Protected m_stkRedoStack As New AnimatGUI.Framework.UndoSystem.HistoryStack

        Protected m_lModificationCount As Long

        Protected m_bAllowAddHistory As Boolean = True
        Protected m_doGroup As EventGroup

#End Region

#Region " Properties "

        Public Overridable Property AllowAddHistory() As Boolean
            Get
                Return m_bAllowAddHistory
            End Get
            Set(ByVal Value As Boolean)
                m_bAllowAddHistory = Value
            End Set
        End Property

        Public Overridable ReadOnly Property CanUndo() As Boolean
            Get
                If m_stkUndoStack.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public Overridable ReadOnly Property CanRedo() As Boolean
            Get
                If m_stkRedoStack.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public Overridable ReadOnly Property UndoCount() As Integer
            Get
                Return m_stkUndoStack.Count
            End Get
        End Property

        Public Overridable ReadOnly Property RedoCount() As Integer
            Get
                Return m_stkRedoStack.Count
            End Get
        End Property

        Public Overridable ReadOnly Property ModificationCount() As Long
            Get
                Return m_lModificationCount
            End Get
        End Property

        Public Overridable ReadOnly Property HistoryGroup() As EventGroup
            Get
                Return m_doGroup
            End Get
        End Property

        Public Overridable ReadOnly Property GroupInProgress() As Boolean
            Get
                If m_doGroup Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overridable Sub AddHistoryEvent(ByVal undoEvent As HistoryEvent)
            If m_bAllowAddHistory Then
                If Not GroupInProgress() Then
                    m_stkUndoStack.Push(undoEvent)
                    m_stkRedoStack.Clear()
                Else
                    m_doGroup.AddHistoryEvent(undoEvent)
                End If

                m_lModificationCount = m_lModificationCount + 1
            End If
        End Sub

        Public Overridable Sub Undo()
            If m_stkUndoStack.Count > 0 Then
                Dim oEvent As Object = m_stkUndoStack.Pop()

                If TypeOf oEvent Is HistoryEvent Then
                    Dim undoEvent As HistoryEvent = DirectCast(oEvent, HistoryEvent)
                    undoEvent.Undo()
                    m_stkRedoStack.Push(undoEvent)
                ElseIf TypeOf oEvent Is EventGroup Then
                    Dim undoEvent As EventGroup = DirectCast(oEvent, EventGroup)
                    undoEvent.Undo()
                    m_stkRedoStack.Push(undoEvent)
                End If
            End If
        End Sub

        Public Overridable Sub Redo()
            If m_stkRedoStack.Count > 0 Then
                Dim oEvent As Object = m_stkRedoStack.Pop()

                If TypeOf oEvent Is HistoryEvent Then
                    Dim undoEvent As HistoryEvent = DirectCast(oEvent, HistoryEvent)
                    undoEvent.Redo()
                    m_stkUndoStack.Push(undoEvent)
                ElseIf TypeOf oEvent Is EventGroup Then
                    Dim undoEvent As EventGroup = DirectCast(oEvent, EventGroup)
                    undoEvent.Redo()
                    m_stkUndoStack.Push(undoEvent)
                End If
            End If
        End Sub

        Public Overridable Sub BeginHistoryGroup()
            If GroupInProgress() Then
                Throw New System.Exception("A history group is already in progress. You can only have one group started at a time.")
            End If

            m_doGroup = New EventGroup
        End Sub

        Public Overridable Sub CommitHistoryGroup()
            If Not GroupInProgress() Then
                Throw New System.Exception("No history group was in progress to commit.")
            End If

            If m_doGroup.UndoCount > 0 Then
                m_stkUndoStack.Push(m_doGroup)
                m_stkRedoStack.Clear()
            End If

            m_lModificationCount = m_lModificationCount + 1
            m_doGroup = Nothing
        End Sub

        Public Overridable Sub AbortHistoryGroup()
            If Not GroupInProgress() Then
                Throw New System.Exception("No history group was in progress to abort.")
            End If

            m_doGroup = Nothing
        End Sub

        Public Overridable Sub RemoveMdiEvents(ByVal mdiParent As AnimatGUI.Forms.MdiChild)
            m_stkUndoStack.RemoveMdiEvents(mdiParent)
            m_stkRedoStack.RemoveMdiEvents(mdiParent)

            If Not m_doGroup Is Nothing Then m_doGroup.RemoveMdiEvents(mdiParent)
        End Sub

#End Region

    End Class

End Namespace

