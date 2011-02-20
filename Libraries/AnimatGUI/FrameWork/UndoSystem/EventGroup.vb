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

    Public Class EventGroup
        Inherits ModificationHistory

#Region " Attributes "

#End Region

#Region " Properties "

#End Region

#Region " Methods "

        Public Overrides Sub Undo()
            Dim iCount As Integer = m_stkUndoStack.Count - 1
            For iIndex As Integer = 0 To iCount
                MyBase.Undo()
            Next
        End Sub

        Public Overrides Sub Redo()
            Dim iCount As Integer = m_stkRedoStack.Count - 1
            For iIndex As Integer = 0 To iCount
                MyBase.Redo()
            Next
        End Sub

#End Region

    End Class

End Namespace

