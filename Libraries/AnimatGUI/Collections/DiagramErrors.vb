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

    Public Class DiagramErrors
        Inherits AnimatDictionaryBase

        Protected m_frmParent As Forms.Errors

        Public Property ParentForm() As Forms.Errors
            Get
                Return m_frmParent
            End Get
            Set(ByVal Value As Forms.Errors)
                If Value Is Nothing Then
                    Throw New System.Exception("You can not set the parent form to null.")
                End If

                m_frmParent = Value
            End Set
        End Property

        Default Public Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.Behavior.DiagramError
            Get
                Return CType(Dictionary(key), AnimatGUI.DataObjects.Behavior.DiagramError)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.DiagramError)
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

        Public Sub New(ByVal doParent As Framework.DataObject, ByVal frmParent As Forms.Errors)
            MyBase.New(doParent)

            If frmParent Is Nothing Then
                Throw New System.Exception("The parent form is not defined.")
            End If

            m_frmParent = frmParent
        End Sub

        Public Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.Behavior.DiagramError)
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

            If Not TypeOf (value) Is AnimatGUI.DataObjects.Behavior.DiagramError Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Behavior.DiagramError.", "value")
            End If

            Dim deError As AnimatGUI.DataObjects.Behavior.DiagramError = DirectCast(value, AnimatGUI.DataObjects.Behavior.DiagramError)

            Dim lvItem As New ListViewItem(deError.ErrorLevel.ToString(), deError.ImageIndex(m_frmParent))
            lvItem.SubItems.Add(New ListViewItem.ListViewSubItem(lvItem, deError.ItemType()))
            lvItem.SubItems.Add(New ListViewItem.ListViewSubItem(lvItem, deError.ItemName()))
            lvItem.SubItems.Add(New ListViewItem.ListViewSubItem(lvItem, deError.ErrorType.ToString))
            lvItem.SubItems.Add(New ListViewItem.ListViewSubItem(lvItem, deError.Message()))
            lvItem.Tag = deError
            deError.ListItem = lvItem

            m_frmParent.AddItem(lvItem)

        End Sub 'OnInsert

        Protected Overrides Sub OnRemove(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            Dim deError As AnimatGUI.DataObjects.Behavior.DiagramError = DirectCast(value, AnimatGUI.DataObjects.Behavior.DiagramError)
            m_frmParent.RemoveItem(deError.ListItem)

        End Sub 'OnRemove

        Protected Overrides Sub OnSet(ByVal key As [Object], ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.Behavior.DiagramError Then
                Throw New ArgumentException("newValue must be of type AnimatGUI.DataObjects.Behavior.DiagramError.", "newValue")
            End If
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatGUI.DataObjects.Behavior.DiagramError Then
                Throw New ArgumentException("value must be of type AnimatGUI.DataObjects.Behavior.DiagramError.", "value")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatDictionaryBase
            Dim aryList As New DiagramErrors(m_doParent, m_frmParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatDictionaryBase
            Dim aryList As New DiagramErrors(doParent, m_frmParent)
            aryList.CloneInternal(Me, doParent, bCutData, doRoot)
            Return aryList
        End Function

    End Class

End Namespace

