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

    Public Class Stimuli
        Inherits AnimatCollectionBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As AnimatGUI.DataObjects.ExternalStimuli.Stimulus
            Get
                Return CType(List(index), AnimatGUI.DataObjects.ExternalStimuli.Stimulus)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.ExternalStimuli.Stimulus)
                List(index) = Value
            End Set
        End Property

        Public Function Add(ByVal value As AnimatGUI.DataObjects.ExternalStimuli.Stimulus, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True) As Integer
            Me.IsDirty = True
            value.BeforeAddToList(bCallSimMethods, bThrowError)
            Dim iVal As Integer = List.Add(value)
            value.AfterAddToList(bCallSimMethods, bThrowError)
            Return iVal
        End Function 'Add

        Public Function IndexOf(ByVal value As AnimatGUI.DataObjects.ExternalStimuli.Stimulus) As Integer
            Return List.IndexOf(value)
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As AnimatGUI.DataObjects.ExternalStimuli.Stimulus, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Me.IsDirty = True
            value.BeforeAddToList(bCallSimMethods, bThrowError)
            List.Insert(index, value)
            value.AfterAddToList(bCallSimMethods, bThrowError)
        End Sub 'Insert


        Public Sub Remove(ByVal value As AnimatGUI.DataObjects.ExternalStimuli.Stimulus, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Me.IsDirty = True
            value.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            List.Remove(value)
            value.AfterRemoveFromList(bCallSimMethods, bThrowError)
        End Sub 'Remove


        Public Function Contains(ByVal value As AnimatGUI.DataObjects.ExternalStimuli.Stimulus) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains


        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.ExternalStimuli.Stimulus Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.ExternalStimuli.Stimulus.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.ExternalStimuli.Stimulus Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.ExternalStimuli.Stimulus.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.ExternalStimuli.Stimulus Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.DataObjects.ExternalStimuli.Stimulus.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.ExternalStimuli.Stimulus Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.ExternalStimuli.Stimulus.")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatCollectionBase
            Dim aryList As New Stimuli(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatCollectionBase
            Dim aryList As New Stimuli(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class 'NodesCollection

    Public Class SortedStimuli
        Inherits AnimatDictionaryBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.ExternalStimuli.Stimulus
            Get
                Return CType(Dictionary(key), AnimatGUI.DataObjects.ExternalStimuli.Stimulus)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.ExternalStimuli.Stimulus)
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

        Public Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.ExternalStimuli.Stimulus, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            value.BeforeAddToList(bCallSimMethods, bThrowError)
            Dictionary.Add(key, value)
            value.AfterAddToList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub 'Add

        Public Function Contains(ByVal key As [String]) As Boolean
            Return Dictionary.Contains(key)
        End Function 'Contains

        Public Sub Remove(ByVal key As [String], Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As ExternalStimuli.Stimulus = Me(key)

            value.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            Dictionary.Remove(key)
            value.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub 'Remove

        Protected Overrides Sub OnInsert(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatGUI.DataObjects.ExternalStimuli.Stimulus Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.ExternalStimuli.Stimulus.", "value")
            End If

            Dim diImage As AnimatGUI.DataObjects.ExternalStimuli.Stimulus = DirectCast(value, AnimatGUI.DataObjects.ExternalStimuli.Stimulus)

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

            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.ExternalStimuli.Stimulus Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.DataObjects.ExternalStimuli.Stimulus.", "newValue")
            End If
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatGUI.DataObjects.ExternalStimuli.Stimulus Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.ExternalStimuli.Stimulus.", "value")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatDictionaryBase
            Dim aryList As New SortedStimuli(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatDictionaryBase
            Dim aryList As New SortedStimuli(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class

End Namespace

