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

Namespace Collections

    Public Class NeuronPairIndices
        Inherits AnimatCollectionBase

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair
            Get
                Return CType(List(index), AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair)
            End Get
            Set(ByVal Value As AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair)
                List(index) = Value
            End Set
        End Property

        Public Function Add(ByVal value As AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair) As Integer
            Return List.IndexOf(DirectCast(value, AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair))
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair)
            Me.IsDirty = True
            List.Insert(index, value)
        End Sub 'Insert


        Public Sub Remove(ByVal value As AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair)
            Me.IsDirty = True
            List.Remove(value)
        End Sub 'Remove


        Public Function Contains(ByVal value As AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains


        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair Then
                Throw New ArgumentException("value must be of type AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair Then
                Throw New ArgumentException("value must be of type AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair Then
                Throw New ArgumentException("newValue must be of type AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair Then
                Throw New ArgumentException("value must be of type AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair.")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatCollectionBase
            Dim aryList As New NeuronPairIndices(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatCollectionBase
            Dim aryList As New NeuronPairIndices(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class


    Public Class SortedNeuronPairIndices
        Inherits AnimatSortedList

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Overloads Property Item(ByVal key As [String]) As AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair
            Get
                Return CType(MyBase.Item(key), AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair)
            End Get
            Set(ByVal Value As AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair)
                MyBase.Item(key) = Value
            End Set
        End Property

        Public Overloads Sub Add(ByVal key As [String], ByVal value As AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            MyBase.Add(key, value)

            Me.IsDirty = True
        End Sub 'Add

        Public Overloads Sub Remove(ByVal key As Object, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair = DirectCast(Me(key), AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair)

            MyBase.Remove(key)
            Me.IsDirty = True
        End Sub

        Public Overloads Sub RemoveAt(ByVal index As Integer, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair = DirectCast(Me.GetByIndex(index), AnimatCarlGUI.DataObjects.Behavior.SynapseTypes.NeuronIndexPair)

            MyBase.RemoveAt(index)
            Me.IsDirty = True
        End Sub

        Public Overrides Function Copy() As AnimatSortedList
            Dim aryList As New SortedNeuronPairIndices(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overloads Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatSortedList
            Dim aryList As New SortedNeuronPairIndices(m_doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

        Public Overrides Function CloneList() As AnimatSortedList
            Dim aryList As New SortedNeuronPairIndices(m_doParent)
            aryList.CloneInternal(Me, Me.Parent, False, Nothing)
            Return aryList
        End Function

    End Class

End Namespace
