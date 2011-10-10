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

Namespace Collections

    Public Class Attachments
        Inherits AnimatCollectionBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As Physical.Bodies.Attachment
            Get
                Return CType(List(index), Physical.Bodies.Attachment)
            End Get
            Set(ByVal Value As Physical.Bodies.Attachment)
                List(index) = Value
            End Set
        End Property


        Public Function Add(ByVal value As Physical.Bodies.Attachment) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As Physical.Bodies.Attachment) As Integer
            Return List.IndexOf(value)
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As Physical.Bodies.Attachment)
            Me.IsDirty = True
            List.Insert(index, value)
        End Sub 'Insert


        Public Sub Remove(ByVal value As Physical.Bodies.Attachment)
            Me.IsDirty = True
            List.Remove(value)
        End Sub 'Remove


        Public Function Contains(ByVal value As Physical.Bodies.Attachment) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains


        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is Physical.Bodies.Attachment Then
                Throw New ArgumentException("value must be of type Physical.Bodies.Attachment.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is Physical.Bodies.Attachment Then
                Throw New ArgumentException("value must be of type Physical.Bodies.Attachment.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is Physical.Bodies.Attachment Then
                Throw New ArgumentException("newValue must be of type Physical.Bodies.Attachment.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is Physical.Bodies.Attachment Then
                Throw New ArgumentException("value must be of type Physical.Bodies.Attachment.")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatCollectionBase
            Dim aryList As New Attachments(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatCollectionBase
            Dim aryList As New Attachments(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

        Public Overridable ReadOnly Property Properties() As AnimatGuiCtrls.Controls.PropertyBag
            Get
                Dim ptTable As New AnimatGuiCtrls.Controls.PropertyTable

                ptTable.Tag = Me
                AddHandler ptTable.GetValue, AddressOf Me.OnGetPropValue

                Dim iIndex As Integer = 0
                For Each doAttach As AnimatGUI.DataObjects.Physical.Bodies.Attachment In Me
                    iIndex = iIndex + 1

                    ptTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Attachment " & iIndex, GetType(String), (iIndex - 1).ToString(), _
                                           "Muscle Attachments", "One of the muscle attachment points.", doAttach.Name, True))
                Next

                Return ptTable
            End Get
        End Property

        Protected Sub OnGetPropValue(ByVal sender As Object, ByVal e As AnimatGuiCtrls.Controls.PropertySpecEventArgs)
            Try
                Dim iIndex As Integer = Integer.Parse(e.Property.PropertyName)

                Dim doAttach As AnimatGUI.DataObjects.Physical.Bodies.Attachment = DirectCast(List(iIndex), AnimatGUI.DataObjects.Physical.Bodies.Attachment)
                e.Value = doAttach.Name

            Catch ex As System.Exception

            End Try
        End Sub

        Public Overridable Sub LoadData(ByRef oXml As Interfaces.StdXml, ByVal aryIDs As ArrayList)
            aryIDs.Clear()
            If oXml.FindChildElement("Attachments", False) Then
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

            oXml.AddChildElement("Attachments")
            oXml.IntoElem()  'Into MuscleAttachments

            For Each doAttach As AnimatGUI.DataObjects.Physical.Bodies.Attachment In Me
                'If it is a copy/cut in progress then it may be trying to save parts that are not on the main structure yet.
                If Not doStruct.FindBodyPart(doAttach.ID, False) Is Nothing Then
                    oXml.AddChildElement("AttachID", doAttach.ID)
                End If
            Next

            oXml.OutOfElem()  'Outof MuscleAttachments

        End Sub

        Public Overridable Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, ByVal doStruct As AnimatGUI.DataObjects.Physical.PhysicalStructure, Optional ByVal strName As String = "")
            oXml.AddChildElement("Attachments")
            oXml.IntoElem()  'Into MuscleAttachments

            For Each doAttach As AnimatGUI.DataObjects.Physical.Bodies.Attachment In Me
                'If it is a copy/cut in progress then it may be trying to save parts that are not on the main structure yet.
                If Not doStruct.FindBodyPart(doAttach.ID, False) Is Nothing Then
                    oXml.AddChildElement("AttachID", doAttach.ID)
                End If
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


    Public Class SortedAttachments
        Inherits AnimatDictionaryBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.Physical.Bodies.Attachment
            Get
                Return CType(Dictionary(key), AnimatGUI.DataObjects.Physical.Bodies.Attachment)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.Bodies.Attachment)
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

        Public Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.Physical.Bodies.Attachment)
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

            If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.Bodies.Attachment Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Physical.Bodies.Attachment.", "value")
            End If

            Dim diImage As AnimatGUI.DataObjects.Physical.Bodies.Attachment = DirectCast(value, AnimatGUI.DataObjects.Physical.Bodies.Attachment)

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

            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.Physical.Bodies.Attachment Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.DataObjects.Physical.Bodies.Attachment.", "newValue")
            End If
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatGUI.DataObjects.Physical.Bodies.Attachment Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Physical.Bodies.Attachment.", "value")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatDictionaryBase
            Dim aryList As New SortedAttachments(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatDictionaryBase
            Dim aryList As New SortedAttachments(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class


    Public Class SortedAttachmentList
        Inherits AnimatSortedList

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Overloads Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.Physical.Bodies.Attachment
            Get
                Return CType(MyBase.Item(key), AnimatGUI.DataObjects.Physical.Bodies.Attachment)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.Bodies.Attachment)
                MyBase.Item(key) = Value
            End Set
        End Property

        Public Overloads Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.Physical.Bodies.Attachment, Optional ByVal bCallAddMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            If bCallAddMethods Then value.BeforeAddToList(bThrowError)
            MyBase.Add(key, value)
            If bCallAddMethods Then value.AfterAddToList(bThrowError)

            Me.IsDirty = True
        End Sub 'Add

        Public Overloads Sub Remove(ByVal key As Object, Optional ByVal bCallAddMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As AnimatGUI.DataObjects.Physical.Bodies.Attachment = DirectCast(Me(key), AnimatGUI.DataObjects.Physical.Bodies.Attachment)

            If bCallAddMethods Then value.BeforeRemoveFromList(bThrowError)
            MyBase.Remove(key)
            If bCallAddMethods Then value.AfterRemoveFromList(bThrowError)
            Me.IsDirty = True
        End Sub

        Public Overloads Sub RemoveAt(ByVal index As Integer, Optional ByVal bCallAddMethods As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            Dim value As AnimatGUI.DataObjects.Physical.Bodies.Attachment = DirectCast(Me.GetByIndex(index), AnimatGUI.DataObjects.Physical.Bodies.Attachment)

            If bCallAddMethods Then value.BeforeRemoveFromList(bThrowError)
            MyBase.RemoveAt(index)
            If bCallAddMethods Then value.AfterRemoveFromList(bThrowError)
            Me.IsDirty = True
        End Sub

        Public Overrides Function Copy() As AnimatSortedList
            Dim aryList As New SortedAttachmentList(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function CloneList() As AnimatSortedList
            Dim aryList As New SortedAttachmentList(m_doParent)
            aryList.CloneInternal(Me)
            Return aryList
        End Function

    End Class


    Namespace Comparers

        Public Class CompareAttachmentNames
            Implements IComparer

            ' Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            Function Compare(ByVal x As [Object], ByVal y As [Object]) As Integer Implements IComparer.Compare
                If Not (TypeOf x Is AnimatGUI.DataObjects.Physical.Bodies.Attachment AndAlso TypeOf y Is AnimatGUI.DataObjects.Physical.Bodies.Attachment) Then Return 0

                Dim bnX As AnimatGUI.DataObjects.Physical.Bodies.Attachment = DirectCast(x, AnimatGUI.DataObjects.Physical.Bodies.Attachment)
                Dim bnY As AnimatGUI.DataObjects.Physical.Bodies.Attachment = DirectCast(y, AnimatGUI.DataObjects.Physical.Bodies.Attachment)

                Return New CaseInsensitiveComparer().Compare(bnX.Name, bnY.Name)

            End Function 'IComparer.Compare

        End Class

    End Namespace

End Namespace

