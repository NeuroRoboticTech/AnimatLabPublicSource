Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.DataObjects

Namespace Collections

    Public Class HistoryEvents
        Inherits AnimatCollectionBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As AnimatGUI.Framework.UndoSystem.HistoryEvent
            Get
                Return CType(List(index), AnimatGUI.Framework.UndoSystem.HistoryEvent)
            End Get
            Set(ByVal Value As AnimatGUI.Framework.UndoSystem.HistoryEvent)
                List(index) = Value
            End Set
        End Property

        Public Function Add(ByVal value As AnimatGUI.Framework.UndoSystem.HistoryEvent) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As AnimatGUI.Framework.UndoSystem.HistoryEvent) As Integer
            Return List.IndexOf(DirectCast(value, AnimatGUI.Framework.UndoSystem.HistoryEvent))
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As AnimatGUI.Framework.UndoSystem.HistoryEvent)
            Me.IsDirty = True
            List.Insert(index, value)
        End Sub 'Insert


        Public Sub Remove(ByVal value As AnimatGUI.Framework.UndoSystem.HistoryEvent)
            Me.IsDirty = True
            List.Remove(value)
        End Sub 'Remove


        Public Function Contains(ByVal value As AnimatGUI.Framework.UndoSystem.HistoryEvent) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains


        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.Framework.UndoSystem.HistoryEvent Then
                Throw New ArgumentException("value must be of type AnimatGUI.Framework.UndoSystem.HistoryEvent.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.Framework.UndoSystem.HistoryEvent Then
                Throw New ArgumentException("value must be of type AnimatGUI.Framework.UndoSystem.HistoryEvent.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is AnimatGUI.Framework.UndoSystem.HistoryEvent Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.Framework.UndoSystem.HistoryEvent.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.Framework.UndoSystem.HistoryEvent Then
                Throw New ArgumentException("value must be of type AnimatGUI.Framework.UndoSystem.HistoryEvent.")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatCollectionBase
            Dim aryList As New AnimatGUI.Collections.HistoryEvents(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatCollectionBase
            Dim aryList As New AnimatGUI.Collections.HistoryEvents(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class

End Namespace
