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

    Public Class RigidBodies
        Inherits AnimatCollectionBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As AnimatGUI.DataObjects.Physical.RigidBody
            Get
                Return CType(List(index), AnimatGUI.DataObjects.Physical.RigidBody)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.RigidBody)
                List(index) = Value
            End Set
        End Property

        Public Function Add(ByVal value As AnimatGUI.DataObjects.Physical.RigidBody) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As AnimatGUI.DataObjects.Physical.RigidBody) As Integer
            Return List.IndexOf(DirectCast(value, AnimatGUI.DataObjects.Physical.RigidBody))
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As AnimatGUI.DataObjects.Physical.RigidBody)
            Me.IsDirty = True
            List.Insert(index, value)
        End Sub 'Insert


        Public Sub Remove(ByVal value As AnimatGUI.DataObjects.Physical.RigidBody)
            Me.IsDirty = True
            List.Remove(value)
        End Sub 'Remove


        Public Function Contains(ByVal value As AnimatGUI.DataObjects.Physical.RigidBody) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains


        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.RigidBody Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Physical.RigidBody.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.RigidBody Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Physical.RigidBody.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.Physical.RigidBody Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.DataObjects.Physical.RigidBody.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.RigidBody Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Physical.RigidBody.")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatCollectionBase
            Dim aryList As New RigidBodies(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatCollectionBase
            Dim aryList As New RigidBodies(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class


    Public Class SortedRigidBodies
        Inherits AnimatSortedList

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Overloads Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.Physical.RigidBody
            Get
                Return CType(MyBase.Item(key), AnimatGUI.DataObjects.Physical.RigidBody)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.RigidBody)
                MyBase.Item(key) = Value
            End Set
        End Property

        Public Overloads Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.Physical.RigidBody, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            value.BeforeAddToList(bCallSimMethods, bThrowError)
            MyBase.Add(key, value)
            value.AfterAddToList(bCallSimMethods, bThrowError)

            Me.IsDirty = True
        End Sub 'Add

        Public Overloads Sub Remove(ByVal key As Object, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As AnimatGUI.DataObjects.Physical.RigidBody = DirectCast(Me(key), AnimatGUI.DataObjects.Physical.RigidBody)

            If Not value Is Nothing Then value.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            MyBase.Remove(key)
            If Not value Is Nothing Then value.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub

        Public Overloads Sub RemoveAt(ByVal index As Integer, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As AnimatGUI.DataObjects.Physical.RigidBody = DirectCast(Me.GetByIndex(index), AnimatGUI.DataObjects.Physical.RigidBody)

            If Not value Is Nothing Then value.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            MyBase.RemoveAt(index)
            If Not value Is Nothing Then value.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub

        Public Overrides Function Copy() As AnimatSortedList
            Dim aryList As New SortedRigidBodies(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overloads Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatSortedList
            Dim aryList As New SortedRigidBodies(m_doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

        Public Overrides Function CloneList() As AnimatSortedList
            Dim aryList As New SortedRigidBodies(m_doParent)
            aryList.CloneInternal(Me, Me.Parent, False, Nothing)
            Return aryList
        End Function

    End Class

    'Public Class SortedRigidBodies
    '    Inherits AnimatDictionaryBase

    '    Public Sub New(ByVal doParent As Framework.DataObject)
    '        MyBase.New(doParent)
    '    End Sub

    '    Default Public Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.Physical.RigidBody
    '        Get
    '            Return CType(Dictionary(key), AnimatGUI.DataObjects.Physical.RigidBody)
    '        End Get
    '        Set(ByVal Value As AnimatGUI.DataObjects.Physical.RigidBody)
    '            Dictionary(key) = Value
    '        End Set
    '    End Property

    '    Public ReadOnly Property Keys() As ICollection
    '        Get
    '            Return Dictionary.Keys
    '        End Get
    '    End Property

    '    Public ReadOnly Property Values() As ICollection
    '        Get
    '            Return Dictionary.Values
    '        End Get
    '    End Property

    '    Public Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.Physical.RigidBody)
    '        Dictionary.Add(key, value)
    '        Me.IsDirty = True
    '    End Sub 'Add

    '    Public Function Contains(ByVal key As [String]) As Boolean
    '        Return Dictionary.Contains(key)
    '    End Function 'Contains

    '    Public Sub Remove(ByVal key As [String])
    '        Dictionary.Remove(key)
    '        Me.IsDirty = True
    '    End Sub 'Remove

    '    Protected Overrides Sub OnInsert(ByVal key As [Object], ByVal value As [Object])
    '        If Not key.GetType() Is Type.GetType("System.String") Then
    '            Throw New ArgumentException("key must be of type String.", "key")
    '        End If

    '        If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.RigidBody Then
    '            Throw New ArgumentException("value must be of type DataObjects.Physical.RigidBody.", "value")
    '        End If

    '        Dim diImage As AnimatGUI.DataObjects.Physical.RigidBody = DirectCast(value, AnimatGUI.DataObjects.Physical.RigidBody)

    '    End Sub 'OnInsert

    '    Protected Overrides Sub OnRemove(ByVal key As [Object], ByVal value As [Object])
    '        If Not key.GetType() Is Type.GetType("System.String") Then
    '            Throw New ArgumentException("key must be of type String.", "key")
    '        End If
    '    End Sub 'OnRemove

    '    Protected Overrides Sub OnSet(ByVal key As [Object], ByVal oldValue As [Object], ByVal newValue As [Object])
    '        If Not key.GetType() Is Type.GetType("System.String") Then
    '            Throw New ArgumentException("key must be of type String.", "key")
    '        End If

    '        If Not TypeOf (newValue) Is AnimatGUI.DataObjects.Physical.RigidBody Then
    '            Throw New ArgumentException("newValue must be of type DataObjects.Physical.RigidBody.", "newValue")
    '        End If
    '    End Sub 'OnSet

    '    Protected Overrides Sub OnValidate(ByVal key As [Object], ByVal value As [Object])
    '        If Not key.GetType() Is Type.GetType("System.String") Then
    '            Throw New ArgumentException("key must be of type String.", "key")
    '        End If

    '        If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.RigidBody Then
    '            Throw New ArgumentException("value must be of type DataObjects.Physical.RigidBody.", "value")
    '        End If
    '    End Sub 'OnValidate 

    '    Public Overrides Function Copy() As AnimatDictionaryBase
    '        Dim aryList As New SortedRigidBodies(m_doParent)
    '        aryList.CopyInternal(Me)
    '        Return aryList
    '    End Function

    '    Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject) As AnimatDictionaryBase
    '        Dim aryList As New SortedRigidBodies(doParent)
    '        aryList.CloneInternal(Me, doParent)
    '        Return aryList
    '    End Function

    'End Class 'RigidBodies

End Namespace
