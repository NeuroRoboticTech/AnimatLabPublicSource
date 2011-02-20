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

    Public Class MetaDocuments
        Inherits AnimatCollectionBase

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Default Public Property Item(ByVal index As Integer) As AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument
            Get
                Return CType(List(index), AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument)
                List(index) = Value
            End Set
        End Property


        Public Function Add(ByVal value As AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument) As Integer
            Me.IsDirty = True
            Return List.Add(value)
        End Function 'Add

        Public Function IndexOf(ByVal value As AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument) As Integer
            Return List.IndexOf(value)
        End Function 'IndexOf


        Public Sub Insert(ByVal index As Integer, ByVal value As AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument)
            Me.IsDirty = True
            List.Insert(index, value)
        End Sub 'Insert


        Public Sub Remove(ByVal value As AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument)
            Me.IsDirty = True
            List.Remove(value)
        End Sub 'Remove


        Public Function Contains(ByVal value As AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument) As Boolean
            ' If value is not of type Int16, this will return false.
            Return List.Contains(value)
        End Function 'Contains


        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument.", "value")
            End If
        End Sub 'OnInsert


        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument.", "value")
            End If
        End Sub 'OnRemove


        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument.", "newValue")
            End If
        End Sub 'OnSet


        Protected Overrides Sub OnValidate(ByVal value As [Object])
            If Not TypeOf (value) Is AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument.")
            End If
        End Sub 'OnValidate 

    End Class 'GainCollection

End Namespace
