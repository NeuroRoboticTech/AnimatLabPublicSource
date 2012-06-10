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

    Public Class HudItems
        Inherits AnimatCollectionBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As AnimatGUI.DataObjects.Visualization.HudItem
            Get
                Return CType(List(index), AnimatGUI.DataObjects.Visualization.HudItem)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Visualization.HudItem)
                List(index) = Value
            End Set
        End Property

        Public Function Add(ByVal value As AnimatGUI.DataObjects.Visualization.HudItem) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As AnimatGUI.DataObjects.Visualization.HudItem) As Integer
            Return List.IndexOf(DirectCast(value, AnimatGUI.DataObjects.Visualization.HudItem))
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As AnimatGUI.DataObjects.Visualization.HudItem)
            Me.IsDirty = True
            List.Insert(index, value)
        End Sub 'Insert


        Public Sub Remove(ByVal value As AnimatGUI.DataObjects.Visualization.HudItem)
            Me.IsDirty = True
            List.Remove(value)
        End Sub 'Remove


        Public Function Contains(ByVal value As AnimatGUI.DataObjects.Visualization.HudItem) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains


        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Visualization.HudItem Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Visualization.HudItem.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Visualization.HudItem Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Visualization.HudItem.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.Visualization.HudItem Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.DataObjects.Visualization.HudItem.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Visualization.HudItem Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Visualization.HudItem.")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatCollectionBase
            Dim aryList As New HudItems(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatCollectionBase
            Dim aryList As New HudItems(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class

    Public Class SortedHudItems
        Inherits AnimatSortedList

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Overloads Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.Visualization.HudItem
            Get
                Return CType(MyBase.Item(key), AnimatGUI.DataObjects.Visualization.HudItem)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Visualization.HudItem)
                MyBase.Item(key) = Value
            End Set
        End Property

        Public Overloads Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.Visualization.HudItem, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            value.BeforeAddToList(bCallSimMethods, bThrowError)
            MyBase.Add(key, value)
            value.AfterAddToList(bCallSimMethods, bThrowError)

            Me.IsDirty = True
        End Sub 'Add

        Public Overloads Sub Remove(ByVal key As Object, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As AnimatGUI.DataObjects.Visualization.HudItem = DirectCast(Me(key), AnimatGUI.DataObjects.Visualization.HudItem)

            value.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            MyBase.Remove(key)
            value.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub

        Public Overloads Sub RemoveAt(ByVal index As Integer, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As AnimatGUI.DataObjects.Visualization.HudItem = DirectCast(Me.GetByIndex(index), AnimatGUI.DataObjects.Visualization.HudItem)

            value.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            MyBase.RemoveAt(index)
            value.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub

        Public Overrides Function Copy() As AnimatSortedList
            Dim aryList As New SortedHudItems(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overloads Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatSortedList
            Dim aryList As New SortedHudItems(m_doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

        Public Overrides Function CloneList() As AnimatSortedList
            Dim aryList As New SortedHudItems(m_doParent)
            aryList.CloneInternal(Me, Me.Parent, False, Nothing)
            Return aryList
        End Function

    End Class

End Namespace
