Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatTools.Framework

Namespace Framework.UndoSystem

    Public Class AddBodyPartEvent
        Inherits AnimatTools.Framework.UndoSystem.HistoryEvent

#Region " Attributes "

        Protected m_frmEditor As AnimatTools.Forms.BodyPlan.Editor
        Protected m_doStructure As AnimatTools.DataObjects.Physical.PhysicalStructure
        Protected m_doParentPart As AnimatTools.DataObjects.Physical.RigidBody
        Protected m_doAddedPart As AnimatTools.DataObjects.Physical.RigidBody

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

        Public Overridable Property ParentPart() As AnimatTools.DataObjects.Physical.RigidBody
            Get
                Return m_doParentPart
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.RigidBody)
                If Value Is Nothing AndAlso (m_doAddedPart Is Nothing OrElse (Not m_doAddedPart Is Nothing AndAlso Not m_doAddedPart.IsRoot)) Then
                    Throw New System.Exception("The body part you are adding to can not be null.")
                End If

                m_doParentPart = Value
            End Set
        End Property

        Public Overridable Property AddedPart() As AnimatTools.DataObjects.Physical.RigidBody
            Get
                Return m_doAddedPart
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Physical.RigidBody)
                If Value Is Nothing Then
                    Throw New System.Exception("The body part you are adding can not be null.")
                End If

                m_doAddedPart = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal frmEditor As AnimatTools.Forms.BodyPlan.Editor, ByVal doStruct As AnimatTools.DataObjects.Physical.PhysicalStructure, _
                       ByVal doParentPart As AnimatTools.DataObjects.Physical.RigidBody, ByVal doAddedPart As AnimatTools.DataObjects.Physical.RigidBody)
            MyBase.New(frmEditor)

            If frmEditor Is Nothing Then
                Throw New System.Exception("The body editor must not be null.")
            End If

            m_frmEditor = frmEditor
            Me.PhysicalStructure = doStruct
            Me.AddedPart = doAddedPart
            Me.ParentPart = doParentPart
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
                m_doStructure.UndoRedoInProgress = True
                m_doStructure.DeleteBodyPart(m_doAddedPart.ID)
                RefreshParent(m_doParentPart)

                If Not m_frmEditor Is Nothing Then
                    m_frmEditor.BodyView.Invalidate()
                End If
            Catch ex As System.Exception
                Throw ex
            Finally
                If Not m_doStructure Is Nothing Then m_doStructure.UndoRedoInProgress = False
            End Try

        End Sub

        Public Overrides Sub Redo()

            Try
                If Not m_doParentPart Is Nothing Then
                    m_doParentPart.UndoRedoInProgress = True
                    m_doParentPart.AddChildBody(m_doAddedPart)
                ElseIf m_doStructure.RootBody Is Nothing Then
                    m_doStructure.UndoRedoInProgress = True
                    m_doStructure.AddRootBody(m_doAddedPart)
                End If

                RefreshParent(m_doAddedPart)

                If Not m_frmEditor Is Nothing Then
                    m_frmEditor.BodyView.Invalidate()
                End If
            Catch ex As System.Exception
                Throw ex
            Finally
                If Not m_doParentPart Is Nothing Then m_doParentPart.UndoRedoInProgress = False
                If Not m_doStructure Is Nothing Then m_doStructure.UndoRedoInProgress = False
            End Try
        End Sub

#End Region

    End Class

End Namespace

