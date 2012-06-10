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

    Public Class AxisList
        Inherits AnimatGUI.Collections.AnimatCollectionBase

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As AnimatGUI.DataObjects.Charting.Axis
            Get
                Return CType(List(index), AnimatGUI.DataObjects.Charting.Axis)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Charting.Axis)
                List(index) = Value
            End Set
        End Property


        Public Function Add(ByVal value As AnimatGUI.DataObjects.Charting.Axis) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As AnimatGUI.DataObjects.Charting.Axis) As Integer
            Return List.IndexOf(value)
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As AnimatGUI.DataObjects.Charting.Axis)
            Me.IsDirty = True
            List.Insert(index, value)
        End Sub 'Insert


        Public Sub Remove(ByVal value As AnimatGUI.DataObjects.Charting.Axis)
            Me.IsDirty = True
            List.Remove(value)
        End Sub 'Remove


        Public Function Contains(ByVal value As AnimatGUI.DataObjects.Charting.Axis) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains


        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Charting.Axis Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Charting.Axis.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Charting.Axis Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Charting.Axis.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.Charting.Axis Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.DataObjects.Charting.Axis.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Charting.Axis Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Charting.Axis.")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatCollectionBase
            Dim aryList As New AxisList(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatCollectionBase
            Dim aryList As New AxisList(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class 'NodesCollection


    Public Class SortedAxisList
        Inherits AnimatSortedList

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Overloads Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.Charting.Axis
            Get
                Return CType(MyBase.Item(key), AnimatGUI.DataObjects.Charting.Axis)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Charting.Axis)
                MyBase.Item(key) = Value
            End Set
        End Property

        Public Overloads Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.Charting.Axis)
            MyBase.Add(key, value)
            Me.IsDirty = True
        End Sub 'Add

        Public Overrides Function Copy() As AnimatSortedList
            Dim aryList As New SortedAxisList(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overloads Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatSortedList
            Dim aryList As New SortedAxisList(m_doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

        Public Overrides Function CloneList() As AnimatSortedList
            Dim aryList As New SortedAxisList(m_doParent)
            aryList.CloneInternal(Me, Me.Parent, False, Nothing)
            Return aryList
        End Function

    End Class

End Namespace

