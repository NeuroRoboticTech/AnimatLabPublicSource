Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls
Imports AnimatTools.Framework

Namespace Framework.UndoSystem

    Public Class MoveBodyPartEvent
        Inherits AnimatTools.Framework.UndoSystem.HistoryEvent

#Region " Attributes "

        Protected m_frmEditor As AnimatTools.Forms.BodyPlan.Editor
        Protected m_doStructure As AnimatTools.DataObjects.Physical.PhysicalStructure
        Protected m_doMovedPart As AnimatTools.DataObjects.Physical.BodyPart
        Protected m_psPreMoveState As AnimatTools.DataObjects.Physical.PartPositionState
        Protected m_psPostMoveState As AnimatTools.DataObjects.Physical.PartPositionState

#End Region

#Region " Properties "

        Public Overridable Property PhysicalStructure() As AnimatTools.DataObjects.Physical.PhysicalStructure
            Get
                Return m_doStructure
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.PhysicalStructure)
                If Value Is Nothing Then
                    Throw New System.Exception("The structure you are adding to can not be null.")
                End If

                m_doStructure = Value
            End Set
        End Property

        Public Overridable Property MovedPart() As AnimatTools.DataObjects.Physical.BodyPart
            Get
                Return m_doMovedPart
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.BodyPart)
                If Value Is Nothing Then
                    Throw New System.Exception("The body part you are moving can not be null.")
                End If

                m_doMovedPart = Value
            End Set
        End Property

        Public Overridable Property PreMoveState() As AnimatTools.DataObjects.Physical.PartPositionState
            Get
                Return m_psPreMoveState
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.PartPositionState)
                If Value Is Nothing Then
                    Throw New System.Exception("The pre-move position state can not be null.")
                End If

                m_psPreMoveState = Value
            End Set
        End Property

        Public Overridable Property PostMoveState() As AnimatTools.DataObjects.Physical.PartPositionState
            Get
                Return m_psPostMoveState
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.PartPositionState)
                If Value Is Nothing Then
                    Throw New System.Exception("The post-move position state can not be null.")
                End If

                m_psPostMoveState = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal frmEditor As AnimatTools.Forms.BodyPlan.Editor, ByVal doStruct As AnimatTools.DataObjects.Physical.PhysicalStructure, _
                       ByVal doMovedPart As AnimatTools.DataObjects.Physical.BodyPart, ByVal psPreMoveState As AnimatTools.DataObjects.Physical.PartPositionState, _
                       ByVal psPostMoveState As AnimatTools.DataObjects.Physical.PartPositionState)
            MyBase.New(frmEditor)

            If frmEditor Is Nothing Then
                Throw New System.Exception("The body editor must not be null.")
            End If

            m_frmEditor = frmEditor
            Me.PhysicalStructure = doStruct
            Me.MovedPart = doMovedPart
            Me.PreMoveState = psPreMoveState
            Me.PostMoveState = psPostMoveState
        End Sub

        Protected Overridable Sub RefreshParent(ByVal doObject As AnimatTools.Framework.DataObject)

            If Not m_mdiParent Is Nothing Then
                m_mdiParent.MakeVisible()
                m_mdiParent.UndoRedoRefresh(doObject)
            ElseIf Not m_frmParent Is Nothing Then
                m_frmParent.UndoRedoRefresh(doObject)
            End If

        End Sub

        Public Overrides Sub Undo()

            Try
                Util.ModificationHistory.AllowAddHistory = False
                m_doMovedPart.PartPositionState = m_psPreMoveState
                RefreshParent(m_doMovedPart)

                If Not m_frmEditor Is Nothing Then
                    m_frmEditor.BodyView.Invalidate()
                End If
            Catch ex As System.Exception
                Throw ex
            Finally
                Util.ModificationHistory.AllowAddHistory = True
            End Try

        End Sub

        Public Overrides Sub Redo()

            Try
                Util.ModificationHistory.AllowAddHistory = False
                m_doMovedPart.PartPositionState = m_psPostMoveState

                RefreshParent(m_doMovedPart)

                If Not m_frmEditor Is Nothing Then
                    m_frmEditor.BodyView.Invalidate()
                End If
            Catch ex As System.Exception
                Throw ex
            Finally
                Util.ModificationHistory.AllowAddHistory = True
            End Try
        End Sub

#End Region

    End Class

End Namespace

