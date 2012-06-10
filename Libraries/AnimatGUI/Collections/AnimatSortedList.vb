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

    Public Class AnimatSortedList
        Inherits SortedList

        Protected m_doParent As Framework.DataObject
        Protected m_bIsDirty As Boolean = False

        Public Property Parent() As Framework.DataObject
            Get
                Return m_doParent
            End Get
            Set(ByVal Value As Framework.DataObject)
                m_doParent = Value
            End Set
        End Property

        Public Property IsDirty() As Boolean
            Get
                Return m_bIsDirty
            End Get
            Set(ByVal Value As Boolean)
                If Not Util.DisableDirtyFlags Then
                    m_bIsDirty = Value

                    If m_bIsDirty AndAlso Not m_doParent Is Nothing Then
                        m_doParent.IsDirty = True
                    End If
                End If
            End Set
        End Property

        Public Sub New(ByVal doParent As Framework.DataObject)
            m_doParent = doParent
        End Sub

        Public Sub ClearIsDirty()
            Me.IsDirty = False

            Dim doObject As Framework.DataObject
            For Each deObject As Object In Me
                If TypeOf deObject Is Framework.DataObject Then
                    doObject = DirectCast(deObject, Framework.DataObject)
                    doObject.ClearIsDirty()
                End If
            Next

        End Sub

        Public Overrides Sub Add(ByVal key As Object, ByVal value As Object)
            MyBase.Add(key, value)
            Me.IsDirty = True
        End Sub

        Public Overrides Sub Clear()
            MyBase.Clear()
            Me.IsDirty = True
        End Sub

        Public Overrides Sub Remove(ByVal key As Object)
            MyBase.Remove(key)
            Me.IsDirty = True
        End Sub

        Public Overrides Sub RemoveAt(ByVal index As Integer)
            MyBase.RemoveAt(index)
            Me.IsDirty = True
        End Sub

        Public Overridable Overloads Function GetItem(ByVal iSelIndex As Integer) As Object

            Dim iIndex As Integer = 0
            For Each deEntry As DictionaryEntry In Me
                If iSelIndex = iIndex Then
                    Return deEntry.Value
                End If
            Next

            Throw New System.Exception("No entry with index '" & iSelIndex & "' was found.")
        End Function

        Protected Overridable Sub CopyInternal(ByVal aryOrig As AnimatSortedList)

            Me.Clear()

            Dim doOrig As AnimatGUI.Framework.DataObject
            For Each deItem As DictionaryEntry In aryOrig
                If TypeOf deItem.Value Is AnimatGUI.Framework.DataObject Then
                    doOrig = DirectCast(deItem.Value, AnimatGUI.Framework.DataObject)
                    Me.Add(doOrig.ID, doOrig)
                End If
            Next

        End Sub

        Protected Overridable Sub CloneInternal(ByVal aryOrig As AnimatSortedList, ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As AnimatGUI.Framework.DataObject)

            Me.Clear()

            Dim doOrig As AnimatGUI.Framework.DataObject
            Dim doItem As AnimatGUI.Framework.DataObject
            For Each deItem As DictionaryEntry In aryOrig
                If TypeOf deItem.Value Is AnimatGUI.Framework.DataObject Then
                    doOrig = DirectCast(deItem.Value, AnimatGUI.Framework.DataObject)
                    doItem = doOrig.Clone(doParent, bCutData, doRoot)
                    Me.Add(doItem.ID, doItem)
                End If
            Next

        End Sub

        Public Overridable Overloads Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                           ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatSortedList
            Throw New System.Exception("You should not be using the base class Clone Method.")
        End Function

        Public Overridable Function Copy() As AnimatSortedList
            Dim aryList As New AnimatSortedList(m_doParent)
            aryList.CopyInternal(Me)
            Return aryList
        End Function

        Public Overridable Function CloneList() As AnimatSortedList
            Dim aryList As New AnimatSortedList(m_doParent)
            aryList.CloneInternal(Me, Me.Parent, False, Nothing)
            Return aryList
        End Function

        Public Overridable Function CopyIntoArraylist() As ArrayList
            Dim aryList As New ArrayList()

            For Each deItem As DictionaryEntry In Me
                aryList.Add(deItem.Value)
            Next

            Return aryList
        End Function

        Public Overridable Function FindDataObjectByName(ByVal strName As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.Framework.DataObject

            Dim doVal As AnimatGUI.Framework.DataObject
            For Each deEntry As DictionaryEntry In Me
                If TypeOf deEntry.Value Is AnimatGUI.Framework.DataObject Then
                    doVal = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                    If doVal.Name = strName Then
                        Return doVal
                    End If
                End If
            Next

            If bThrowError Then
                Throw New System.Exception("No data object with the name '" & strName & "' was found.")
            End If

            Return Nothing
        End Function

        Public Overridable Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject
            Dim doFound As Framework.DataObject
            For Each deEntry As DictionaryEntry In Me
                If TypeOf deEntry.Value Is AnimatGUI.Framework.DataObject Then
                    doObject = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                    doFound = doObject.FindObjectByID(strID)
                    If Not doFound Is Nothing Then
                        Return doFound
                    End If
                End If
            Next

            Return Nothing
        End Function

        Public Overridable Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList)
            Dim doVal As AnimatGUI.Framework.DataObject
            For Each deEntry As DictionaryEntry In Me
                If TypeOf deEntry.Value Is AnimatGUI.Framework.DataObject Then
                    doVal = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                    doVal.AddToReplaceIDList(aryReplaceIDList)
                End If
            Next

        End Sub

        Public Overridable Sub InitializeSimulationReferences()
            Dim doVal As AnimatGUI.Framework.DataObject
            For Each deEntry As DictionaryEntry In Me
                If TypeOf deEntry.Value Is AnimatGUI.Framework.DataObject Then
                    doVal = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                    doVal.InitializeSimulationReferences()
                End If
            Next

        End Sub

        Public Overridable Sub InitializeAfterLoad()
            Dim doVal As AnimatGUI.Framework.DataObject
            For Each deEntry As DictionaryEntry In Me
                If TypeOf deEntry.Value Is AnimatGUI.Framework.DataObject Then
                    doVal = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                    doVal.InitializeAfterLoad()
                End If
            Next

        End Sub

    End Class

End Namespace
