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
Imports AnimatGUI.Collections
Imports AnimatGUI

Namespace Collections

    Public Class SynapseTypes
        Inherits AnimatGUI.Collections.AnimatDictionaryBase

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal key As [String]) As IntegrateFireGUI.DataObjects.Behavior.SynapseType
            Get
                Return CType(Dictionary(key), IntegrateFireGUI.DataObjects.Behavior.SynapseType)
            End Get
            Set(ByVal Value As IntegrateFireGUI.DataObjects.Behavior.SynapseType)
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

        Public Sub Add(ByVal key As [String], ByVal value As IntegrateFireGUI.DataObjects.Behavior.SynapseType, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            value.BeforeAddToList(bCallSimMethods, bThrowError)
            Dictionary.Add(key, value)
            value.AfterAddToList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub 'Add

        Public Sub Remove(ByVal key As [String], Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As IntegrateFireGUI.DataObjects.Behavior.SynapseType = Me(key)

            value.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            Dictionary.Remove(key)
            value.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub 'Remove

        Public Function Contains(ByVal key As [String]) As Boolean
            Return Dictionary.Contains(key)
        End Function 'Contains

        Protected Overrides Sub OnInsert(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is IntegrateFireGUI.DataObjects.Behavior.SynapseType Then
                Throw New ArgumentException("value must be of type IntegrateFireGUI.DataObjects.Behavior.SynapseType.", "value")
            End If

            Dim diImage As IntegrateFireGUI.DataObjects.Behavior.SynapseType = DirectCast(value, IntegrateFireGUI.DataObjects.Behavior.SynapseType)

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

            If Not TypeOf (newValue) Is IntegrateFireGUI.DataObjects.Behavior.SynapseType Then
                Throw New ArgumentException("newValue must be of type IntegrateFireGUI.DataObjects.Behavior.SynapseType.", "newValue")
            End If
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is IntegrateFireGUI.DataObjects.Behavior.SynapseType Then
                Throw New ArgumentException("value must be of type IntegrateFireGUI.DataObjects.Behavior.SynapseType.", "value")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatGUI.Collections.AnimatDictionaryBase
            Dim aryList As New SynapseTypes(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Collections.AnimatDictionaryBase
            Dim aryList As New SynapseTypes(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class 'DiagramsDictionary

    Public Class SortedSynapseTypes
        Inherits AnimatSortedList

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Overloads Property Item(ByVal key As [String]) As IntegrateFireGUI.DataObjects.Behavior.SynapseType
            Get
                Return CType(MyBase.Item(key), IntegrateFireGUI.DataObjects.Behavior.SynapseType)
            End Get
            Set(ByVal Value As IntegrateFireGUI.DataObjects.Behavior.SynapseType)
                MyBase.Item(key) = Value
            End Set
        End Property

        Public Overloads Sub Add(ByVal key As [String], ByVal value As IntegrateFireGUI.DataObjects.Behavior.SynapseType, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)

            value.BeforeAddToList(bCallSimMethods, bThrowError)
            MyBase.Add(key, value)
            value.AfterAddToList(bCallSimMethods, bThrowError)

            Me.IsDirty = True
        End Sub 'Add

        Public Overloads Sub Remove(ByVal key As Object, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As IntegrateFireGUI.DataObjects.Behavior.SynapseType = DirectCast(Me(key), IntegrateFireGUI.DataObjects.Behavior.SynapseType)

            value.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            MyBase.Remove(key)
            value.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub

        Public Overloads Sub RemoveAt(ByVal index As Integer, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As IntegrateFireGUI.DataObjects.Behavior.SynapseType = DirectCast(Me.GetByIndex(index), IntegrateFireGUI.DataObjects.Behavior.SynapseType)

            value.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            MyBase.RemoveAt(index)
            value.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub

        Public Overrides Function Copy() As AnimatSortedList
            Dim aryList As New SortedSynapseTypes(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overloads Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatSortedList
            Dim aryList As New SortedSynapseTypes(m_doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

        Public Overrides Function CloneList() As AnimatSortedList
            Dim aryList As New SortedSynapseTypes(m_doParent)
            aryList.CloneInternal(Me, Me.Parent, False, Nothing)
            Return aryList
        End Function

    End Class

End Namespace
