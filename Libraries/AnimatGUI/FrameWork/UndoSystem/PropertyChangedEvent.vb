Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace Framework.UndoSystem

    Public Class PropertyChangedEvent
        Inherits HistoryEvent

#Region " Attributes "

        Protected m_AlteredObject As Object
        Protected m_propInfo As System.Reflection.PropertyInfo
        Protected m_OriginalValue As Object
        Protected m_newValue As Object

#End Region

#Region " Properties "

        Public Overridable Property AlteredObject() As Object
            Get
                Return m_AlteredObject
            End Get
            Set(ByVal Value As Object)
                If Value Is Nothing Then
                    Throw New System.Exception("You can not alter the property of an object that is null.")
                End If

                m_AlteredObject = Value
            End Set
        End Property

        Public Overridable Property PropertyInfo() As System.Reflection.PropertyInfo
            Get
                Return m_propInfo
            End Get
            Set(ByVal Value As System.Reflection.PropertyInfo)
                If Value Is Nothing Then
                    Throw New System.Exception("The property info can not be null.")
                End If

                m_propInfo = Value
            End Set
        End Property

        Public Overridable Property OriginalValue() As Object
            Get
                Return m_OriginalValue
            End Get
            Set(ByVal Value As Object)
                m_OriginalValue = Value
            End Set
        End Property

        Public Overridable Property NewValue() As Object
            Get
                Return m_newValue
            End Get
            Set(ByVal Value As Object)
                m_newValue = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal frmParent As System.Windows.Forms.Form, ByVal altObject As Object, ByVal propInfo As System.Reflection.PropertyInfo, ByVal origValue As Object, ByVal newValue As Object)
            MyBase.New(frmParent)

            Me.AlteredObject = altObject
            Me.PropertyInfo = propInfo
            Me.OriginalValue = origValue
            Me.NewValue = newValue

        End Sub

        Protected Overridable Sub RefreshParent(ByVal doObject As AnimatGUI.Framework.DataObject)

            'TODO
            'If Not m_mdiParent Is Nothing Then
            '    m_mdiParent.MakeVisible()
            '    m_mdiParent.UndoRedoRefresh(doObject)
            'ElseIf Not m_frmParent Is Nothing Then
            '    m_frmParent.UndoRedoRefresh(doObject)
            'End If

        End Sub

        Protected Overridable Sub EnsureFormVisible()

            'TODO
            'If Not m_mdiParent Is Nothing AndAlso TypeOf m_AlteredObject Is AnimatGUI.Framework.DataObject Then
            '    Dim doObject As AnimatGUI.Framework.DataObject = DirectCast(m_AlteredObject, AnimatGUI.Framework.DataObject)

            '    doObject.EnsureFormActive()
            'End If

        End Sub

        Protected Overridable Sub RefreshParent(ByVal frmObject As AnimatGUI.Forms.AnimatForm)

            'TODO
            'If Not m_mdiParent Is Nothing Then
            '    m_mdiParent.MakeVisible()
            '    m_mdiParent.UndoRedoRefresh(frmObject)
            'End If

        End Sub

        Public Overrides Sub Undo()
            Dim doObject As DataObject
            Dim frmObject As AnimatGUI.Forms.AnimatForm

            Try
                EnsureFormVisible()

                Dim propType As System.Type = m_propInfo.PropertyType

                If TypeOf m_AlteredObject Is DataObject Then
                    doObject = DirectCast(m_AlteredObject, DataObject)

                    doObject.UndoRedoInProgress = True

                    If m_newValue Is Nothing OrElse propType Is m_newValue.GetType OrElse Util.IsTypeOf(m_newValue.GetType(), propType, False) Then
                        m_propInfo.SetValue(m_AlteredObject, m_OriginalValue, Nothing)
                    Else
                        m_propInfo.SetValue(m_AlteredObject, Convert.ChangeType(m_OriginalValue, propType), Nothing)
                    End If

                    doObject.IsDirty = True
                    doObject.UndoRedoInProgress = False

                    RefreshParent(doObject)
                ElseIf TypeOf m_AlteredObject Is AnimatGUI.Forms.AnimatForm Then
                    frmObject = DirectCast(m_AlteredObject, AnimatGUI.Forms.AnimatForm)

                    frmObject.UndoRedoInProgress = True

                    If m_newValue Is Nothing OrElse propType Is m_newValue.GetType OrElse Util.IsTypeOf(m_newValue.GetType(), propType, False) Then
                        m_propInfo.SetValue(m_AlteredObject, m_OriginalValue, Nothing)
                    Else
                        m_propInfo.SetValue(m_AlteredObject, Convert.ChangeType(m_OriginalValue, propType), Nothing)
                    End If

                    frmObject.IsDirty = True
                    frmObject.UndoRedoInProgress = False

                    RefreshParent(frmObject)
                Else
                    If m_newValue Is Nothing OrElse propType Is m_newValue.GetType OrElse Util.IsTypeOf(m_newValue.GetType(), propType, False) Then
                        m_propInfo.SetValue(m_AlteredObject, m_OriginalValue, Nothing)
                    Else
                        m_propInfo.SetValue(m_AlteredObject, Convert.ChangeType(m_OriginalValue, propType), Nothing)
                    End If

                    Util.Application.IsDirty = True
                End If

            Catch ex As System.Exception
                Throw ex
            Finally
                If Not doObject Is Nothing Then doObject.UndoRedoInProgress = False
                If Not frmObject Is Nothing Then frmObject.UndoRedoInProgress = False
            End Try

        End Sub

        Public Overrides Sub Redo()
            Dim doObject As DataObject
            Dim frmObject As AnimatGUI.Forms.AnimatForm

            Try
                EnsureFormVisible()

                Dim propType As System.Type = m_propInfo.PropertyType

                If TypeOf m_AlteredObject Is DataObject Then
                    doObject = DirectCast(m_AlteredObject, DataObject)

                    doObject.UndoRedoInProgress = True
                    If m_newValue Is Nothing OrElse propType Is m_newValue.GetType OrElse Util.IsTypeOf(m_newValue.GetType(), propType, False) Then
                        m_propInfo.SetValue(m_AlteredObject, m_newValue, Nothing)
                    Else
                        m_propInfo.SetValue(m_AlteredObject, Convert.ChangeType(m_newValue, propType), Nothing)
                    End If

                    doObject.IsDirty = True
                    doObject.UndoRedoInProgress = False

                    RefreshParent(doObject)
                ElseIf TypeOf m_AlteredObject Is AnimatGUI.Forms.AnimatForm Then
                    frmObject = DirectCast(m_AlteredObject, AnimatGUI.Forms.AnimatForm)

                    frmObject.UndoRedoInProgress = True

                    If m_newValue Is Nothing OrElse propType Is m_newValue.GetType OrElse Util.IsTypeOf(m_newValue.GetType(), propType, False) Then
                        m_propInfo.SetValue(m_AlteredObject, m_newValue, Nothing)
                    Else
                        m_propInfo.SetValue(m_AlteredObject, Convert.ChangeType(m_newValue, propType), Nothing)
                    End If

                    frmObject.IsDirty = True
                    frmObject.UndoRedoInProgress = False

                    RefreshParent(frmObject)
                Else
                    If m_newValue Is Nothing OrElse propType Is m_newValue.GetType OrElse Util.IsTypeOf(m_newValue.GetType(), propType, False) Then
                        m_propInfo.SetValue(m_AlteredObject, m_newValue, Nothing)
                    Else
                        m_propInfo.SetValue(m_AlteredObject, Convert.ChangeType(m_newValue, propType), Nothing)
                    End If

                    Util.Application.IsDirty = True
                End If

            Catch ex As System.Exception
                Throw ex
            Finally
                If Not doObject Is Nothing Then doObject.UndoRedoInProgress = False
                If Not frmObject Is Nothing Then frmObject.UndoRedoInProgress = False
            End Try
        End Sub

#End Region

    End Class

End Namespace

