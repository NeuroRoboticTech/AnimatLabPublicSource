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

    Public Class SortedStructures
        Inherits AnimatDictionaryBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.Physical.PhysicalStructure
            Get
                Return CType(Dictionary(key), AnimatGUI.DataObjects.Physical.PhysicalStructure)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.PhysicalStructure)
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

        Public Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.Physical.PhysicalStructure, Optional ByVal bCallAddMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            If bCallAddMethods Then value.BeforeAddToList(bThrowError)
            Dictionary.Add(key, value)
            If bCallAddMethods Then value.AfterAddToList(bThrowError)

            Me.IsDirty = True
        End Sub 'Add

        Public Function Contains(ByVal key As [String]) As Boolean
            Return Dictionary.Contains(key)
        End Function 'Contains

        Public Sub Remove(ByVal key As [String], Optional ByVal bCallAddMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim doObj As AnimatGUI.DataObjects.Physical.PhysicalStructure = Me.Item(key)

            If bCallAddMethods Then doObj.BeforeRemoveFromList(bThrowError)
            Dictionary.Remove(key)
            If bCallAddMethods Then doObj.AfterRemoveFromList(bThrowError)

            Me.IsDirty = True
        End Sub 'Remove

        Protected Overrides Sub OnInsert(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.PhysicalStructure Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Physical.PhysicalStructure.", "value")
            End If

            Dim diImage As AnimatGUI.DataObjects.Physical.PhysicalStructure = DirectCast(value, AnimatGUI.DataObjects.Physical.PhysicalStructure)

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

            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.Physical.PhysicalStructure Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.DataObjects.Physical.PhysicalStructure.", "newValue")
            End If
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.PhysicalStructure Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Physical.PhysicalStructure.", "value")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatDictionaryBase
            Dim aryList As New SortedStructures(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatDictionaryBase
            Dim aryList As New SortedStructures(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class

End Namespace
