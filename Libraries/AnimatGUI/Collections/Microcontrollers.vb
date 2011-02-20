Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls
Imports AnimatTools.DataObjects

Namespace Collections

    Public Class Microcontrollers
        Inherits AnimatCollectionBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As AnimatTools.DataObjects.Physical.Microcontroller
            Get
                Return CType(List(index), AnimatTools.DataObjects.Physical.Microcontroller)
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.Microcontroller)
                List(index) = Value
            End Set
        End Property

        Public Function Add(ByVal value As AnimatTools.DataObjects.Physical.Microcontroller) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As AnimatTools.DataObjects.Physical.Microcontroller) As Integer
            Return List.IndexOf(DirectCast(value, AnimatTools.DataObjects.Physical.Microcontroller))
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As AnimatTools.DataObjects.Physical.Microcontroller)
            Me.IsDirty = True
            List.Insert(index, value)
        End Sub 'Insert


        Public Sub Remove(ByVal value As AnimatTools.DataObjects.Physical.Microcontroller)
            Me.IsDirty = True
            List.Remove(value)
        End Sub 'Remove


        Public Function Contains(ByVal value As AnimatTools.DataObjects.Physical.Microcontroller) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains


        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatTools.DataObjects.Physical.Microcontroller Then
                Throw New ArgumentException("value must be of type AnimatTools.DataObjects.Physical.Microcontroller.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatTools.DataObjects.Physical.Microcontroller Then
                Throw New ArgumentException("value must be of type AnimatTools.DataObjects.Physical.Microcontroller.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is AnimatTools.DataObjects.Physical.Microcontroller Then
                Throw New ArgumentException("newValue must be of type AnimatTools.DataObjects.Physical.Microcontroller.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is AnimatTools.DataObjects.Physical.Microcontroller Then
                Throw New ArgumentException("value must be of type AnimatTools.DataObjects.Physical.Microcontroller.")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatCollectionBase
            Dim aryList As New Microcontrollers(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatCollectionBase
            Dim aryList As New Microcontrollers(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class


    Public Class SortedMicrocontrollers
        Inherits AnimatDictionaryBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal key As [String]) As AnimatTools.DataObjects.Physical.Microcontroller
            Get
                Return CType(Dictionary(key), AnimatTools.DataObjects.Physical.Microcontroller)
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.Microcontroller)
                Dictionary(key) = Value
            End Set
        End Property

        <Browsable(False)> _
        Public ReadOnly Property Keys() As ICollection
            Get
                Return Dictionary.Keys
            End Get
        End Property

        <Browsable(False)> _
        Public ReadOnly Property Values() As ICollection
            Get
                Return Dictionary.Values
            End Get
        End Property

        Public Sub Add(ByVal key As [String], ByVal value As AnimatTools.DataObjects.Physical.Microcontroller)
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

        Public Overloads Function GetItem(ByVal key As String) As AnimatTools.DataObjects.Physical.Microcontroller
            Return (Me.Item(key))
        End Function

        Protected Overrides Sub OnInsert(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatTools.DataObjects.Physical.Microcontroller Then
                Throw New ArgumentException("value must be of type DataObjects.Physical.Joint.", "value")
            End If

            Dim diImage As AnimatTools.DataObjects.Physical.Microcontroller = DirectCast(value, AnimatTools.DataObjects.Physical.Microcontroller)

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

            If Not TypeOf (newValue) Is AnimatTools.DataObjects.Physical.Microcontroller Then
                Throw New ArgumentException("newValue must be of type DataObjects.Physical.Joint.", "newValue")
            End If
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatTools.DataObjects.Physical.Microcontroller Then
                Throw New ArgumentException("value must be of type DataObjects.Physical.Joint.", "value")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatDictionaryBase
            Dim aryList As New SortedMicrocontrollers(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatDictionaryBase
            Dim aryList As New SortedMicrocontrollers(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class 'Joints

End Namespace
