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

    Public Class DiagramImages
        Inherits AnimatDictionaryBase

        Protected m_doOrganism As AnimatGUI.DataObjects.Physical.Organism

        Default Public Property Item(ByVal key As [String]) As AnimatGUI.DataObjects.Behavior.DiagramImage
            Get
                Return CType(Dictionary(key), AnimatGUI.DataObjects.Behavior.DiagramImage)
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.DiagramImage)
                If Not m_doOrganism Is Nothing Then
                    m_doOrganism.SignalImageRemoved(Me(key))
                End If

                Dictionary(key) = Value

                If Not m_doOrganism Is Nothing Then
                    m_doOrganism.SignalImageAdded(Value)
                End If
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

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            If Not doParent Is Nothing Then
                m_doOrganism = DirectCast(doParent, AnimatGUI.DataObjects.Physical.Organism)
            End If
        End Sub

        Public Sub Add(ByVal key As [String], ByVal value As AnimatGUI.DataObjects.Behavior.DiagramImage)
            Dictionary.Add(key, value)
            Me.IsDirty = True

            If Not m_doOrganism Is Nothing Then
                m_doOrganism.SignalImageAdded(value)
            End If
        End Sub 'Add

        Public Function Contains(ByVal key As [String]) As Boolean
            Return Dictionary.Contains(key)
        End Function 'Contains

        Public Sub Remove(ByVal key As [String])
            Dim value As AnimatGUI.DataObjects.Behavior.DiagramImage = Me(key)

            Dictionary.Remove(key)
            Me.IsDirty = True

            If Not m_doOrganism Is Nothing Then
                m_doOrganism.SignalImageRemoved(value)
            End If
        End Sub 'Remove

        Protected Overrides Sub OnInsert(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatGUI.DataObjects.Behavior.DiagramImage Then
                Throw New ArgumentException("value must be of type DiagramImage.", "value")
            End If

            Dim diImage As AnimatGUI.DataObjects.Behavior.DiagramImage = DirectCast(value, AnimatGUI.DataObjects.Behavior.DiagramImage)

        End Sub 'OnInsert

        Protected Overrides Sub OnRemove(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            Dim diImage As AnimatGUI.DataObjects.Behavior.DiagramImage = DirectCast(value, AnimatGUI.DataObjects.Behavior.DiagramImage)

        End Sub 'OnRemove

        Protected Overrides Sub OnSet(ByVal key As [Object], ByVal oldValue As [Object], ByVal newValue As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (newValue) Is AnimatGUI.DataObjects.Behavior.DiagramImage Then
                Throw New ArgumentException("newValue must be of type DiagramImage.", "newValue")
            End If
        End Sub 'OnSet

        Protected Overrides Sub OnValidate(ByVal key As [Object], ByVal value As [Object])
            If Not key.GetType() Is Type.GetType("System.String") Then
                Throw New ArgumentException("key must be of type String.", "key")
            End If

            If Not TypeOf (value) Is AnimatGUI.DataObjects.Behavior.DiagramImage Then
                Throw New ArgumentException("value must be of type DiagramImage.", "value")
            End If
        End Sub 'OnValidate 

        Public Overrides Function Copy() As AnimatDictionaryBase
            Dim aryList As New DiagramImages(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatDictionaryBase
            Dim aryList As New DiagramImages(doParent)
            aryList.CloneInternal(Me, doParent, bCutData, doParent)
            Return aryList
        End Function

        Public Overridable Function FindIndexByID(ByVal strID As String) As Integer

            Dim bdDiagram As AnimatGUI.DataObjects.Behavior.DiagramImage
            Dim iIndex As Integer = 0
            For Each deEntry As DictionaryEntry In Me
                bdDiagram = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.DiagramImage)
                If bdDiagram.ID = strID Then
                    Return iIndex
                End If
                iIndex = iIndex + 1
            Next

        End Function

        Public Overridable Function FindDiagramImageByID(ByVal strID As String) As AnimatGUI.DataObjects.Behavior.DiagramImage

            Dim bdDiagram As AnimatGUI.DataObjects.Behavior.DiagramImage
            For Each deEntry As DictionaryEntry In Me
                bdDiagram = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.DiagramImage)
                If bdDiagram.ID = strID Then
                    Return bdDiagram
                End If
            Next

        End Function

        Public Overridable Function FindImageByID(ByVal strID As String) As System.Drawing.Image

            Dim bdDiagram As AnimatGUI.DataObjects.Behavior.DiagramImage
            For Each deEntry As DictionaryEntry In Me
                bdDiagram = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.DiagramImage)
                If bdDiagram.ID = strID Then
                    Return bdDiagram.WorkspaceImage
                End If
            Next

        End Function

    End Class 'DiagramImageDictionary

End Namespace
