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

    Public Class Panels
        Inherits AnimatCollectionBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As AnimatGUI.DataObjects.Behavior.PanelData
            Get
                Return CType(List(index), AnimatGUI.DataObjects.Behavior.PanelData)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.PanelData)
                List(index) = Value
            End Set
        End Property


        Public Function Add(ByVal value As AnimatGUI.DataObjects.Behavior.PanelData) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As AnimatGUI.DataObjects.Behavior.PanelData) As Integer
            Return List.IndexOf(value)
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As AnimatGUI.DataObjects.Behavior.PanelData)
            Me.IsDirty = True
            List.Insert(index, value)
        End Sub 'Insert


        Public Sub Remove(ByVal value As AnimatGUI.DataObjects.Behavior.PanelData)
            Me.IsDirty = True
            List.Remove(value)
        End Sub 'Remove


        Public Function Contains(ByVal value As AnimatGUI.DataObjects.Behavior.PanelData) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains


        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Behavior.PanelData Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Behavior.PanelData.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Behavior.PanelData Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Behavior.PanelData.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.Behavior.PanelData Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.DataObjects.Behavior.PanelData.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Behavior.PanelData Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Behavior.PanelData.")
            End If
        End Sub 'OnValidate 

    End Class 'NodesCollection


    Public Class SortedPanels
        Inherits AnimatDictionaryBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.Behavior.PanelData
            Get
                Return CType(Dictionary(key), AnimatGUI.DataObjects.Behavior.PanelData)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.PanelData)
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

        Public Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.Behavior.PanelData)
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

            If Not TypeOf (value) Is AnimatGUI.DataObjects.Behavior.PanelData Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Behavior.PanelData.", "value")
            End If

            Dim diImage As AnimatGUI.DataObjects.Behavior.PanelData = DirectCast(value, AnimatGUI.DataObjects.Behavior.PanelData)

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

            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.Behavior.PanelData Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.DataObjects.Behavior.PanelData.", "newValue")
            End If
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatGUI.DataObjects.Behavior.PanelData Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Behavior.PanelData.", "value")
            End If
        End Sub 'OnValidate 

    End Class


    Namespace Comparers

        Public Class ComparePanelNames
            Implements IComparer

            ' Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            Function Compare(ByVal x As [Object], ByVal y As [Object]) As Integer Implements IComparer.Compare
                If Not (TypeOf x Is AnimatGUI.DataObjects.Behavior.PanelData AndAlso TypeOf y Is AnimatGUI.DataObjects.Behavior.PanelData) Then Return 0

                Dim pdX As AnimatGUI.DataObjects.Behavior.PanelData = DirectCast(x, AnimatGUI.DataObjects.Behavior.PanelData)
                Dim pdY As AnimatGUI.DataObjects.Behavior.PanelData = DirectCast(y, AnimatGUI.DataObjects.Behavior.PanelData)

                Return New CaseInsensitiveComparer().Compare(pdX.m_strPanelName, pdY.m_strPanelName)

            End Function 'IComparer.Compare

        End Class

    End Namespace

End Namespace

