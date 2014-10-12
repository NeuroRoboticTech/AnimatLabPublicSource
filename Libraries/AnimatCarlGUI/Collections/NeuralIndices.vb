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

    Public Class NeuralIndices
        Inherits AnimatGUI.Collections.AnimatCollectionBase

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As Integer
            Get
                Return CType(List(index), Integer)
            End Get
            Set(ByVal Value As Integer)
                List(index) = Value
            End Set
        End Property

        Public Function Add(ByVal value As Integer) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As Integer) As Integer
            Return List.IndexOf(value)
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As Integer)
            Me.IsDirty = True
            List.Insert(index, value)
        End Sub 'Insert


        Public Sub Remove(ByVal value As Integer)
            Me.IsDirty = True
            List.Remove(value)
        End Sub 'Remove


        Public Function Contains(ByVal value As Integer) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains


        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is Integer Then
                Throw New ArgumentException("value must be of type Integer.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is Integer Then
                Throw New ArgumentException("value must be of type Integer.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is Integer Then
                Throw New ArgumentException("newValue must be of type Integer.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is Integer Then
                Throw New ArgumentException("value must be of type Integer.")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatGUI.Collections.AnimatCollectionBase
            Dim aryList As New NeuralIndices(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Collections.AnimatCollectionBase
            Dim aryList As New NeuralIndices(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

        Protected Overrides Sub CloneInternal(aryOrig As AnimatGUI.Collections.AnimatCollectionBase, doParent As AnimatGUI.Framework.DataObject, bCutData As Boolean, doRoot As AnimatGUI.Framework.DataObject)
            Me.Clear()

            For Each doOrig As Object In aryOrig
                List.Add(doOrig)
            Next
        End Sub

        Public Overridable ReadOnly Property Properties() As AnimatGuiCtrls.Controls.PropertyBag
            Get
                Dim ptTable As New AnimatGuiCtrls.Controls.PropertyTable

                ptTable.Tag = Me
                AddHandler ptTable.GetValue, AddressOf Me.OnGetPropValue

                Dim iIndex As Integer = 0
                For Each INeuraldx As Integer In Me
                    iIndex = iIndex + 1

                    ptTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Index " & iIndex, GetType(String), (iIndex - 1).ToString(), _
                                           "Linkages", "One of the remote control linkages.", INeuraldx, True))
                Next

                Return ptTable
            End Get
        End Property

        Protected Sub OnGetPropValue(ByVal sender As Object, ByVal e As AnimatGuiCtrls.Controls.PropertySpecEventArgs)
            Try
                Dim iIndex As Integer = Integer.Parse(e.Property.PropertyName)

                Dim doLink As Integer = iIndex
                e.Value = doLink.ToString

            Catch ex As System.Exception

            End Try
        End Sub

        Public Overrides Function ToString() As String
            Return "Click to edit"
        End Function

    End Class


    Public Class SortedNeuralIndices
        Inherits AnimatGUI.Collections.AnimatSortedList

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Overloads Property Item(ByVal key As [String]) As Integer
            Get
                Return CType(MyBase.Item(key), Integer)
            End Get
            Set(ByVal Value As Integer)
                MyBase.Item(key) = Value
            End Set
        End Property

        Public Overloads Sub Add(ByVal key As [String], ByVal value As Integer, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            MyBase.Add(key, value)

            Me.IsDirty = True
        End Sub 'Add

        Public Overloads Sub Remove(ByVal key As Object, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As Integer = DirectCast(Me(key), Integer)

            MyBase.Remove(key)
            Me.IsDirty = True
        End Sub

        Public Overloads Sub RemoveAt(ByVal index As Integer, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As Integer = DirectCast(Me.GetByIndex(index), Integer)

            MyBase.RemoveAt(index)
            Me.IsDirty = True
        End Sub

        Public Overrides Function Copy() As AnimatGUI.Collections.AnimatSortedList
            Dim aryList As New SortedNeuralIndices(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overloads Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Collections.AnimatSortedList
            Dim aryList As New SortedNeuralIndices(m_doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

        Public Overrides Function CloneList() As AnimatGUI.Collections.AnimatSortedList
            Dim aryList As New SortedNeuralIndices(m_doParent)
            aryList.CloneInternal(Me, Me.Parent, False, Nothing)
            Return aryList
        End Function

        Public Overridable ReadOnly Property Properties() As AnimatGuiCtrls.Controls.PropertyBag
            Get
                Dim ptTable As New AnimatGuiCtrls.Controls.PropertyTable

                ptTable.Tag = Me
                AddHandler ptTable.GetValue, AddressOf Me.OnGetPropValue

                Dim iIndex As Integer = 0
                For Each deEntry As DictionaryEntry In Me
                    Dim doLink As Integer = DirectCast(deEntry.Value, Integer)

                    iIndex = iIndex + 1

                    ptTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Index " & iIndex, GetType(String), (iIndex - 1).ToString(), _
                                           "Linkages", "One of the remote control linkages.", doLink.ToString, True))
                Next

                Return ptTable
            End Get
        End Property

        Protected Sub OnGetPropValue(ByVal sender As Object, ByVal e As AnimatGuiCtrls.Controls.PropertySpecEventArgs)
            Try
                Dim iIndex As Integer = Integer.Parse(e.Property.PropertyName)

                Dim doLink As Integer = DirectCast(Me(iIndex), Integer)
                e.Value = doLink.ToString

            Catch ex As System.Exception

            End Try
        End Sub

        Public Overrides Function ToString() As String
            Return "Click to edit"
        End Function
    End Class

End Namespace
