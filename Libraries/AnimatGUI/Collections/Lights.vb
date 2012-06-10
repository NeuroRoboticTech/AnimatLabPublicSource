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

    Public Class Lights
        Inherits AnimatCollectionBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As AnimatGUI.DataObjects.Physical.Light
            Get
                Return CType(List(index), AnimatGUI.DataObjects.Physical.Light)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.Light)
                List(index) = Value
            End Set
        End Property

        Public Function Add(ByVal value As AnimatGUI.DataObjects.Physical.Light) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As AnimatGUI.DataObjects.Physical.Light) As Integer
            Return List.IndexOf(DirectCast(value, AnimatGUI.DataObjects.Physical.Light))
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As AnimatGUI.DataObjects.Physical.Light)
            Me.IsDirty = True
            List.Insert(index, value)
        End Sub 'Insert


        Public Sub Remove(ByVal value As AnimatGUI.DataObjects.Physical.Light)
            Me.IsDirty = True
            List.Remove(value)
        End Sub 'Remove


        Public Function Contains(ByVal value As AnimatGUI.DataObjects.Physical.Light) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains


        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.Light Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Physical.Light.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.Light Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Physical.Light.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.Physical.Light Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.DataObjects.Physical.Light.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.Light Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Physical.Light.")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatCollectionBase
            Dim aryList As New Lights(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatCollectionBase
            Dim aryList As New Lights(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class

    Public Class SortedLights
        Inherits AnimatSortedList

        Protected m_iMaxLights As Integer = 8

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Overridable Property MaxLights() As Integer
            Get
                Return m_iMaxLights
            End Get
            Set(ByVal value As Integer)
                If value <= 0 Then
                    Throw New System.Exception("The maximum number of lights must be greater than zero.")
                End If
                If value < Me.Count Then
                    Throw New System.Exception("You cannot set the maximum number of lights to less than the current number of lights.")
                End If

                m_iMaxLights = value
            End Set
        End Property

        Default Public Overloads Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.Physical.Light
            Get
                Return CType(MyBase.Item(key), AnimatGUI.DataObjects.Physical.Light)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.Light)
                MyBase.Item(key) = Value
            End Set
        End Property

        Public Overloads Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.Physical.Light, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            If Me.Count + 1 > MaxLights Then
                Throw New System.Exception("Only " & MaxLights & " can be added to the environment.")
            End If

            value.BeforeAddToList(bCallSimMethods, bThrowError)
            MyBase.Add(key, value)
            value.AfterAddToList(bCallSimMethods, bThrowError)

            Me.IsDirty = True
        End Sub 'Add

        Public Overloads Sub Remove(ByVal key As Object, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As AnimatGUI.DataObjects.Physical.Light = DirectCast(Me(key), AnimatGUI.DataObjects.Physical.Light)

            value.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            MyBase.Remove(key)
            value.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub

        Public Overloads Sub RemoveAt(ByVal index As Integer, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As AnimatGUI.DataObjects.Physical.Light = DirectCast(Me.GetByIndex(index), AnimatGUI.DataObjects.Physical.Light)

            value.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            MyBase.RemoveAt(index)
            value.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub

        Public Overrides Function Copy() As AnimatSortedList
            Dim aryList As New SortedLights(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overloads Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatSortedList
            Dim aryList As New SortedLights(m_doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

        Public Overrides Function CloneList() As AnimatSortedList
            Dim aryList As New SortedLights(m_doParent)
            aryList.CloneInternal(Me, Me.Parent, False, Nothing)
            Return aryList
        End Function

    End Class

End Namespace
