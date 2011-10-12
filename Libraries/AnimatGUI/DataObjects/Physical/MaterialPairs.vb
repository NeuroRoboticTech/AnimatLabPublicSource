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
Imports AnimatGUI.Framework
Imports AnimatGUI.Collections

Namespace DataObjects.Physical

    Public Class MaterialPairs
        Inherits AnimatCollectionBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As Physical.MaterialPair
            Get
                Return CType(List(index), Physical.MaterialPair)
            End Get
            Set(ByVal Value As Physical.MaterialPair)
                List(index) = Value
            End Set
        End Property

        Public Function Add(ByVal value As Physical.MaterialPair) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As Physical.MaterialPair) As Integer
            Return List.IndexOf(value)
        End Function 'IndexOf

        Public Sub Insert(ByVal index As Integer, ByVal value As Physical.MaterialPair)
            Me.IsDirty = True
            List.Insert(index, value)
        End Sub 'Insert

        Public Sub Remove(ByVal value As Physical.MaterialPair)
            Me.IsDirty = True
            List.Remove(value)
        End Sub 'Remove

        Public Function Contains(ByVal value As Physical.MaterialPair) As Boolean
            Return List.Contains(value)
        End Function 'Contains

        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is Physical.MaterialPair Then
                Throw New ArgumentException("value must be of type Physical.MaterialPair.", "value")
            End If
        End Sub 'OnInsert

        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is Physical.MaterialPair Then
                Throw New ArgumentException("value must be of type Physical.MaterialPair.", "value")
            End If
        End Sub 'OnRemove

        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is Physical.MaterialPair Then
                Throw New ArgumentException("newValue must be of type Physical.MaterialPair.", "newValue")
            End If
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is Physical.MaterialPair Then
                Throw New ArgumentException("value must be of type Physical.MaterialPair.")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatCollectionBase
            Dim aryList As New MaterialPairs(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatCollectionBase
            Dim aryList As New MaterialPairs(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

        Public Overridable ReadOnly Property Properties() As AnimatGuiCtrls.Controls.PropertyBag
            Get
                Dim ptTable As New AnimatGuiCtrls.Controls.PropertyTable

                ptTable.Tag = Me
                AddHandler ptTable.GetValue, AddressOf Me.OnGetPropValue

                Dim iIndex As Integer = 0
                For Each doPair As AnimatGUI.DataObjects.Physical.MaterialPair In Me
                    iIndex = iIndex + 1

                    ptTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("MaterialPair " & iIndex, GetType(String), (iIndex - 1).ToString(), _
                                           "Material Pair", "Material Pair for friction interaction", doPair.Name, True))
                Next

                Return ptTable
            End Get
        End Property

        Protected Sub OnGetPropValue(ByVal sender As Object, ByVal e As AnimatGuiCtrls.Controls.PropertySpecEventArgs)
            Try
                Dim iIndex As Integer = Integer.Parse(e.Property.PropertyName)

                Dim doPair As AnimatGUI.DataObjects.Physical.MaterialPair = DirectCast(List(iIndex), AnimatGUI.DataObjects.Physical.MaterialPair)
                e.Value = doPair.Name

            Catch ex As System.Exception

            End Try
        End Sub

        Public Overridable Sub LoadData(ByRef oXml As Interfaces.StdXml, ByVal aryIDs As ArrayList)
            aryIDs.Clear()
            If oXml.FindChildElement("MaterialPairs", False) Then
                oXml.IntoElem()

                Dim strID As String
                Dim iCount As Integer = oXml.NumberOfChildren()
                For iIndex As Integer = 0 To iCount - 1
                    oXml.FindChildByIndex(iIndex)
                    strID = oXml.GetChildString()
                    aryIDs.Add(strID)
                Next

                oXml.OutOfElem()
            End If
        End Sub

        Public Overridable Sub SaveData(ByRef oXml As Interfaces.StdXml, ByVal doStruct As AnimatGUI.DataObjects.Physical.PhysicalStructure)

            oXml.AddChildElement("MaterialPairs")
            oXml.IntoElem()  'Into MuscleAttachments

            For Each doPair As AnimatGUI.DataObjects.Physical.MaterialPair In Me
                oXml.AddChildElement("PairID", doPair.ID)
            Next

            oXml.OutOfElem()  'Outof MuscleAttachments

        End Sub

        Public Overridable Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, ByVal doStruct As AnimatGUI.DataObjects.Physical.PhysicalStructure, Optional ByVal strName As String = "")
            oXml.AddChildElement("MaterialPairs")
            oXml.IntoElem()  'Into MuscleAttachments

            For Each doPair As AnimatGUI.DataObjects.Physical.MaterialPair In Me
                oXml.AddChildElement("PairID", doPair.ID)
            Next

            oXml.OutOfElem()  'Outof MuscleAttachments
        End Sub

        Public Overridable Function GetSimulationXml(ByVal strName As String, ByVal doStruct As AnimatGUI.DataObjects.Physical.PhysicalStructure) As String

            Dim oXml As New AnimatGUI.Interfaces.StdXml
            oXml.AddElement("Root")
            SaveSimulationXml(oXml, doStruct, strName)

            Return oXml.Serialize()
        End Function

    End Class 'NodesCollection


    Public Class SortedMaterialPairs
        Inherits AnimatDictionaryBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.Physical.MaterialPair
            Get
                Return CType(Dictionary(key), AnimatGUI.DataObjects.Physical.MaterialPair)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.MaterialPair)
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

        Public Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.Physical.MaterialPair)
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

            If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.MaterialPair Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Physical.MaterialPair.", "value")
            End If
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

            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.Physical.MaterialPair Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.DataObjects.Physical.MaterialPair.", "newValue")
            End If
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.MaterialPair Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Physical.MaterialPair.", "value")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatDictionaryBase
            Dim aryList As New SortedMaterialPairs(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatDictionaryBase
            Dim aryList As New SortedMaterialPairs(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class


    Public Class SortedMaterialPairList
        Inherits AnimatSortedList

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Overloads Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.Physical.MaterialPair
            Get
                Return CType(MyBase.Item(key), AnimatGUI.DataObjects.Physical.MaterialPair)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.MaterialPair)
                MyBase.Item(key) = Value
            End Set
        End Property

        Public Overloads Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.Physical.MaterialPair, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            value.BeforeAddToList(bCallSimMethods, bThrowError)
            MyBase.Add(key, value)
            value.AfterAddToList(bCallSimMethods, bThrowError)

            Me.IsDirty = True
        End Sub 'Add

        Public Overloads Sub Remove(ByVal key As Object, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As AnimatGUI.DataObjects.Physical.MaterialPair = DirectCast(Me(key), AnimatGUI.DataObjects.Physical.MaterialPair)

            value.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            MyBase.Remove(key)
            value.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub

        Public Overloads Sub RemoveAt(ByVal index As Integer, Optional ByVal bCallSimMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As AnimatGUI.DataObjects.Physical.MaterialPair = DirectCast(Me.GetByIndex(index), AnimatGUI.DataObjects.Physical.MaterialPair)

            value.BeforeRemoveFromList(bCallSimMethods, bThrowError)
            MyBase.RemoveAt(index)
            value.AfterRemoveFromList(bCallSimMethods, bThrowError)
            Me.IsDirty = True
        End Sub

        Public Overrides Function Copy() As AnimatSortedList
            Dim aryList As New SortedMaterialPairList(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function CloneList() As AnimatSortedList
            Dim aryList As New SortedMaterialPairList(m_doParent)
            aryList.CloneInternal(Me)
            Return aryList
        End Function

    End Class


    Namespace Comparers

        Public Class CompareAttachmentNames
            Implements IComparer

            ' Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            Function Compare(ByVal x As [Object], ByVal y As [Object]) As Integer Implements IComparer.Compare
                If Not (TypeOf x Is AnimatGUI.DataObjects.Physical.MaterialPair AndAlso TypeOf y Is AnimatGUI.DataObjects.Physical.MaterialPair) Then Return 0

                Dim bnX As AnimatGUI.DataObjects.Physical.MaterialPair = DirectCast(x, AnimatGUI.DataObjects.Physical.MaterialPair)
                Dim bnY As AnimatGUI.DataObjects.Physical.MaterialPair = DirectCast(y, AnimatGUI.DataObjects.Physical.MaterialPair)

                Return New CaseInsensitiveComparer().Compare(bnX.Name, bnY.Name)

            End Function 'IComparer.Compare

        End Class

    End Namespace

End Namespace

'When a new materialType is added to the collection we need to create new materialpair objects for all
' the various combinations (you will need to make sure that you only do one combination pair. 

'For example, only do wood-concrete once, do not do it again for concrete-wood. 
'There should only be one actual pair object for a given combination.) To do this adding you will need to override the 

'Public Overridable Sub AfterAddToList(Optional ByVal bThrowError As Boolean = True)
'method and add a new method there to add the needed materialpair combinations. 
'    Stub it out at first until the materialpair object has been created.