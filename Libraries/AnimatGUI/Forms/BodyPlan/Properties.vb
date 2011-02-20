Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools
Imports AnimatTools.Framework
Imports AnimatTools.DataObjects
Imports System.Drawing.Imaging

Namespace Forms.BodyPlan

    Public Class Properties
        Inherits AnimatTools.Forms.ProjectProperties

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            '
            'Properties
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 266)
            Me.Name = "Properties"
            Me.Text = "Properties"

        End Sub

#End Region

#Region " Attributes "

        Protected m_bInSelection As Boolean = False
        Protected m_beEditor As AnimatTools.Forms.BodyPlan.Editor

        Protected m_arySelectedParts As AnimatTools.Collections.BodyParts
        Protected m_doSelectedPart As DataObjects.Physical.BodyPart
        Protected m_doSelectedStructure As DataObjects.Physical.PhysicalStructure

#End Region

#Region " Properties "

        Public Overridable Property Editor() As Forms.Bodyplan.Editor
            Get
                Return m_beEditor
            End Get
            Set(ByVal Value As Forms.Bodyplan.Editor)
                m_beEditor = Value
            End Set
        End Property

        Public Overridable ReadOnly Property SelectedPart() As DataObjects.Physical.BodyPart
            Get
                Return m_doSelectedPart
            End Get
        End Property

        Public Overridable ReadOnly Property SelectedParts() As AnimatTools.Collections.BodyParts
            Get
                Return m_arySelectedParts
            End Get
        End Property

        Public Overridable Property SelectedStructure() As DataObjects.Physical.PhysicalStructure
            Get
                Return m_doSelectedStructure
            End Get
            Set(ByVal Value As DataObjects.Physical.PhysicalStructure)

                m_doSelectedStructure = Value

                If Not Value Is Nothing Then
                    m_doSelectedStructure = Value
                    Me.PropertyData = Value.Properties
                End If

                If Not m_doSelectedPart Is Nothing Then
                    'm_doSelectedPart.Selected = False
                    ClearSelectedParts()
                End If

                Me.Editor.HierarchyBar.TreeView.SelectedNode.EnsureVisible()
                Me.Editor.BodyView.Invalidate()
            End Set
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            Try

                MyBase.Initialize(frmParent)

                m_arySelectedParts = New AnimatTools.Collections.BodyParts(Nothing)
                'm_beEditor = DirectCast(frmMdiParent, AnimatTools.Forms.BodyPlan.Editor)

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub ClearSelectedParts()

            'm_doSelectedPart.Selected = False

            For Each doPart As AnimatTools.DataObjects.Physical.BodyPart In m_arySelectedParts
                'doPart.Selected = False
            Next

            m_arySelectedParts.Clear()
        End Sub

        Public Overridable Function PartIsSelected(ByVal doPart As AnimatTools.DataObjects.Physical.BodyPart) As Boolean

            If m_arySelectedParts.Contains(doPart) Then
                Return True
            Else
                Return False
            End If

        End Function

        Protected Overridable Sub DeselectPart(ByVal doPart As AnimatTools.DataObjects.Physical.BodyPart)

            If m_arySelectedParts.Contains(doPart) Then
                m_arySelectedParts.Remove(doPart)
                'doPart.Selected = False

                If m_doSelectedPart Is doPart Then
                    If m_arySelectedParts.Count > 0 Then
                        m_doSelectedPart = m_arySelectedParts(m_arySelectedParts.Count - 1)
                    Else
                        ClearSelectedParts()
                    End If
                End If
            End If

            If Not Me.Editor.HierarchyBar.TreeView.SelectedNode Is Nothing Then
                If m_doSelectedPart Is Nothing Then
                    'Me.Editor.HierarchyBar.TreeView.SelectedNode = Me.Editor.PhysicalStructure.BodyPlanStructureNode
                ElseIf Not Me.Editor.HierarchyBar.TreeView.SelectedNode.Tag Is m_doSelectedPart Then
                    Me.Editor.HierarchyBar.TreeView.SelectedNode = m_doSelectedPart.BodyTreeNode
                End If

                Me.Editor.HierarchyBar.TreeView.SelectedNode.EnsureVisible()
            End If
            Me.Editor.BodyView.Invalidate()

        End Sub

        Public Overridable Sub SelectPart(ByVal doPart As AnimatTools.DataObjects.Physical.BodyPart, ByVal bCtrlDown As Boolean)

            Try

                If m_bInSelection Then Return
                m_bInSelection = True

                m_doSelectedStructure = Nothing

                'If the user clicks blank space with the ctrl key down then do nothing
                If doPart Is Nothing AndAlso bCtrlDown Then
                    m_bInSelection = False
                    Return
                End If

                If bCtrlDown And PartIsSelected(doPart) Then
                    DeselectPart(doPart)
                    m_bInSelection = False
                    Return
                End If

                'Deselect the old part.
                If Not m_doSelectedPart Is Nothing AndAlso Not bCtrlDown Then
                    ClearSelectedParts()
                End If

                If Not doPart Is Nothing Then doPart.BeforeSelected()
                m_doSelectedPart = doPart

                If Not m_doSelectedPart Is Nothing Then

                    'm_doSelectedPart.Selected = True

                    If Not m_doSelectedPart.BodyTreeNode Is Nothing Then
                        Me.Editor.HierarchyBar.TreeView.SelectedNode = m_doSelectedPart.BodyTreeNode
                    End If

                    If Not m_arySelectedParts.Contains(m_doSelectedPart) Then
                        m_arySelectedParts.Add(m_doSelectedPart)
                    End If
                ElseIf Not bCtrlDown Then
                    'Me.Editor.HierarchyBar.TreeView.SelectedNode = Me.Editor.PhysicalStructure.BodyPlanStructureNode
                End If

                If m_arySelectedParts.Count > 1 Then
                    Dim aryItems(m_arySelectedParts.Count - 1) As PropertyBag
                    Dim iIndex As Integer = 0
                    For Each doSelPart As AnimatTools.DataObjects.Physical.BodyPart In m_arySelectedParts
                        If Not doSelPart Is Nothing Then
                            aryItems(iIndex) = doSelPart.Properties
                            iIndex = iIndex + 1
                        End If
                    Next

                    Me.PropertyData = Nothing
                    Me.PropertyArray = aryItems
                Else
                    Me.PropertyArray = Nothing
                    If Not m_doSelectedPart Is Nothing Then
                        Me.PropertyData = m_doSelectedPart.Properties
                    Else
                        If Not Me.Editor Is Nothing AndAlso Not Me.Editor.PhysicalStructure Is Nothing Then
                            Me.PropertyData = Me.Editor.PhysicalStructure.Properties
                        Else
                            Me.PropertyData = Nothing
                        End If
                    End If
                End If

                If Not Me.Editor.ReceptiveFieldsBar Is Nothing Then
                    Me.Editor.ReceptiveFieldsBar.SelectPart(m_doSelectedPart)
                End If

                If Not Me.Editor.HierarchyBar.TreeView.SelectedNode Is Nothing Then
                    Me.Editor.HierarchyBar.TreeView.SelectedNode.EnsureVisible()
                End If
                Me.Editor.BodyView.Invalidate()

                If Not m_doSelectedPart Is Nothing Then m_doSelectedPart.BeforeSelected()
                m_bInSelection = False

            Catch ex As System.Exception
                m_bInSelection = False
                Throw ex
            End Try
        End Sub

#End Region

    End Class

End Namespace

