Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Framework.UndoSystem

    Public Class DiagramChangedEvent
        Inherits AnimatGUI.Framework.UndoSystem.HistoryEvent

#Region " Attributes "

        Protected m_frmEditor As AnimatGUI.Forms.Behavior.Editor
        Protected m_frmDiagram As AnimatGUI.Forms.Behavior.DiagramOld
        Protected m_bdAlteredData As AnimatGUI.DataObjects.Behavior.Data

#End Region

#Region " Properties "

        Public Overridable Property Diagram() As AnimatGUI.Forms.Behavior.DiagramOld
            Get
                Return m_frmDiagram
            End Get
            Set(ByVal Value As AnimatGUI.Forms.Behavior.DiagramOld)
                If Value Is Nothing Then
                    Throw New System.Exception("A Diagram change event must be associated with a diagram.")
                End If

                m_frmDiagram = Value
            End Set
        End Property

        Public Overridable Property AlteredData() As AnimatGUI.DataObjects.Behavior.Data
            Get
                Return m_bdAlteredData
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.Data)
                m_bdAlteredData = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal frmEditor As AnimatGUI.Forms.Behavior.Editor, ByVal frmDiagram As AnimatGUI.Forms.Behavior.DiagramOld, Optional ByVal bdAltered As AnimatGUI.DataObjects.Behavior.Data = Nothing)
            MyBase.New(frmEditor)

            If frmEditor Is Nothing Then
                Throw New System.Exception("The behavioral editor must not be null.")
            End If

            m_frmEditor = frmEditor
            Me.Diagram = frmDiagram
            Me.AlteredData = bdAltered
        End Sub

        Protected Overridable Sub RefreshParent(ByVal doObject As AnimatGUI.Framework.DataObject)

            If Not m_mdiParent Is Nothing Then
                m_mdiParent.MakeVisible()
                m_mdiParent.UndoRedoRefresh(doObject)
            ElseIf Not m_frmParent Is Nothing Then
                m_frmParent.UndoRedoRefresh(doObject)
            End If

        End Sub

        Public Overrides Sub Undo()
            m_frmEditor.SelectedDiagram(m_frmDiagram)
            m_frmDiagram.OnUndo()

            If Not m_bdAlteredData Is Nothing Then
                m_frmDiagram.UpdateData(m_bdAlteredData, False, False)
            End If
            m_frmEditor.PropertiesBar.RefreshProperties()
        End Sub

        Public Overrides Sub Redo()
            m_frmEditor.SelectedDiagram(m_frmDiagram)
            m_frmDiagram.OnRedo()
            m_frmEditor.PropertiesBar.Refresh()

            If Not m_bdAlteredData Is Nothing Then
                m_frmDiagram.UpdateData(m_bdAlteredData, False, False)
            End If
            m_frmEditor.PropertiesBar.RefreshProperties()
        End Sub

#End Region

    End Class

End Namespace

