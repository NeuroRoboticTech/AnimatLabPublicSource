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

    Public Class DataObjects
        Inherits AnimatCollectionBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As Framework.DataObject
            Get
                Return CType(List(index), Framework.DataObject)
            End Get
            Set(ByVal Value As Framework.DataObject)
                List(index) = Value
            End Set
        End Property


        Public Function Add(ByVal value As AnimatGUI.Framework.DataObject) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As Framework.DataObject) As Integer
            Return List.IndexOf(DirectCast(value, Framework.DataObject))
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As Framework.DataObject)
            List.Insert(index, value)
            Me.IsDirty = True
        End Sub 'Insert


        Public Sub Remove(ByVal value As Framework.DataObject)
            List.Remove(value)
            Me.IsDirty = True
        End Sub 'Remove


        Public Function Contains(ByVal value As Framework.DataObject) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains


        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is Framework.DataObject Then
                Throw New ArgumentException("value must be of type Framework.DataObject.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is Framework.DataObject Then
                Throw New ArgumentException("value must be of type Framework.DataObject.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is Framework.DataObject Then
                Throw New ArgumentException("newValue must be of type Framework.DataObject.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is Framework.DataObject Then
                Throw New ArgumentException("value must be of type Framework.DataObject.")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatCollectionBase
            Dim aryList As New DataObjects(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatCollectionBase
            Dim aryList As New DataObjects(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class 'DataObjects


    Public Class SortedDataObjects
        Inherits AnimatDictionaryBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal key As [String]) As Framework.DataObject
            Get
                Return CType(Dictionary(key), Framework.DataObject)
            End Get
            Set(ByVal Value As Framework.DataObject)
                Dictionary(key) = Value
            End Set
        End Property

        Public ReadOnly Property Keys() As ICollection
            Get
                Return Dictionary.Keys
            End Get
        End Property

        Public ReadOnly Property Values() As ICollection
            Get
                Return Dictionary.Values
            End Get
        End Property

        Public Overridable Sub Add(ByVal key As [String], ByVal value As Framework.DataObject)
            Dictionary.Add(key, value)
            Me.IsDirty = True
        End Sub 'Add

        Public Overridable Function Contains(ByVal key As [String]) As Boolean
            Return Dictionary.Contains(key)
        End Function 'Contains

        Public Overridable Sub Remove(ByVal key As [String])
            Dictionary.Remove(key)
            Me.IsDirty = True
        End Sub 'Remove

        Protected Overrides Sub OnInsert(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is Framework.DataObject Then
                Throw New ArgumentException("value must be of type Framework.DataObject.", "value")
            End If

            Dim diObject As Framework.DataObject = DirectCast(value, Framework.DataObject)

        End Sub 'OnInsert

        Protected Overrides Sub OnRemove(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If
        End Sub 'OnRemove

        Protected Overrides Sub OnSet(ByVal key As [Object], ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (newValue) Is Framework.DataObject Then
                Throw New ArgumentException("newValue must be of type Framework.DataObject.", "newValue")
            End If
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is Framework.DataObject Then
                Throw New ArgumentException("value must be of type Framework.DataObject.", "value")
            End If
        End Sub 'OnValidate 

    End Class

    Public Class CompatibleDataObjects
        Inherits SortedDataObjects

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        'Compatible objects should never set the dirty flag, so we are overriding the add/remove functions to remove this call.
        Public Overrides Sub Add(ByVal key As [String], ByVal value As Framework.DataObject)
            Dictionary.Add(key, value)
        End Sub 'Add

        Public Overrides Sub Remove(ByVal key As [String])
            Dictionary.Remove(key)
        End Sub 'Remove

    End Class



End Namespace

