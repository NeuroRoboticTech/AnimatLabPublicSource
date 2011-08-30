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

    Public Class HistoryStack

#Region " Attributes "

        Protected m_aryEvents As New ArrayList

#End Region

#Region " Properties "

        Public ReadOnly Property Count() As Integer
            Get
                Return m_aryEvents.Count
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub Push(ByVal oEvent As Object)
            m_aryEvents.Add(oEvent)
        End Sub

        Public Function Pop() As Object
            Dim oEvent As Object = Nothing

            If m_aryEvents.Count > 0 Then
                'Get the last one added and return it.
                oEvent = m_aryEvents.Item(m_aryEvents.Count - 1)
                m_aryEvents.RemoveAt(m_aryEvents.Count - 1)
            End If

            Return oEvent
        End Function

        Public Function Peek() As Object
            Dim evHistory As Object = Nothing

            If m_aryEvents.Count > 0 Then
                'Get the last one added and return it.
                evHistory = m_aryEvents.Item(m_aryEvents.Count - 1)
            End If

            Return evHistory
        End Function

        'Public Sub RemoveMdiEvents(ByVal mdiParent As AnimatGUI.Forms.MdiChild)

        '    Dim aryRemoveList As New ArrayList
        '    For Each oEvent As Object In m_aryEvents
        '        If TypeOf oEvent Is HistoryEvent Then
        '            Dim evHistory As HistoryEvent = DirectCast(oEvent, HistoryEvent)
        '            If evHistory.MdiParent Is mdiParent Then
        '                aryRemoveList.Add(evHistory)
        '            End If
        '        ElseIf TypeOf oEvent Is EventGroup Then
        '            Dim evGroup As EventGroup = DirectCast(oEvent, EventGroup)
        '            evGroup.RemoveMdiEvents(mdiParent)
        '            If evGroup.UndoCount = 0 Then aryRemoveList.Add(evGroup)
        '        End If
        '    Next

        '    For Each oEvent As Object In aryRemoveList
        '        m_aryEvents.Remove(oEvent)
        '    Next

        'End Sub

        Public Sub Clear()
            m_aryEvents.Clear()
        End Sub

#End Region

    End Class

End Namespace
