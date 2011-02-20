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

    Public Class Assemblies
        Inherits AnimatCollectionBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As System.Reflection.Assembly
            Get
                Return CType(List(index), System.Reflection.Assembly)
            End Get
            Set(ByVal Value As System.Reflection.Assembly)
                List(index) = Value
            End Set
        End Property


        Public Function Add(ByVal value As System.Reflection.Assembly) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As System.Reflection.Assembly) As Integer
            Return List.IndexOf(value)
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As System.Reflection.Assembly)
            List.Insert(index, value)
            Me.IsDirty = True
        End Sub 'Insert


        Public Sub Remove(ByVal value As System.Reflection.Assembly)
            List.Remove(value)
            Me.IsDirty = True
        End Sub 'Remove

        Public Function Contains(ByVal value As System.Reflection.Assembly) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains

        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is System.Reflection.Assembly Then
                Throw New ArgumentException("value must be of type System.Reflection.Assembly.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is System.Reflection.Assembly Then
                Throw New ArgumentException("value must be of type System.Reflection.Assembly.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is System.Reflection.Assembly Then
                Throw New ArgumentException("newValue must be of type System.Reflection.Assembly.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is System.Reflection.Assembly Then
                Throw New ArgumentException("value must be of type System.Reflection.Assembly.")
            End If
        End Sub 'OnValidate 

    End Class 'NodesCollection


    Public Class SortedAssemblies
        Inherits AnimatDictionaryBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal key As [String]) As System.Reflection.Assembly
            Get
                Return CType(Dictionary(key), System.Reflection.Assembly)
            End Get
            Set(ByVal Value As System.Reflection.Assembly)
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

        Public Sub Add(ByVal key As [String], ByVal value As System.Reflection.Assembly)
            Dictionary.Add(key, value)
            Me.IsDirty = True
        End Sub 'Add

        Public Function Contains(ByVal key As [String]) As Boolean
            Return Dictionary.Contains(key)
        End Function 'Contains

        Public Sub Remove(ByVal key As [String])
            Dictionary.Remove(key)
            Me.IsDirty = True
        End Sub 'Remove

        Protected Overrides Sub OnInsert(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is System.Reflection.Assembly Then
                Throw New ArgumentException("value must be of type System.Reflection.Assembly.", "value")
            End If

            Dim diImage As System.Reflection.Assembly = DirectCast(value, System.Reflection.Assembly)

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

            If Not TypeOf (newValue) Is System.Reflection.Assembly Then
                Throw New ArgumentException("newValue must be of type System.Reflection.Assembly.", "newValue")
            End If
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is System.Reflection.Assembly Then
                Throw New ArgumentException("value must be of type System.Reflection.Assembly.", "value")
            End If
        End Sub 'OnValidate 

    End Class

End Namespace

