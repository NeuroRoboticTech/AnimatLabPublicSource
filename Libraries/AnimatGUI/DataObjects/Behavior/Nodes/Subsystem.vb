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

Namespace DataObjects.Behavior.Nodes

    Public Class Subsystem
        Inherits Behavior.Node

#Region " Attributes "

        Protected m_bdSubsystem As Forms.Behavior.Diagram

        'Only used during loading
        Protected m_strSubsystemID As String

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Subsystem"
            End Get
        End Property

        Public Overridable Property Subsystem() As Forms.Behavior.Diagram
            Get
                Return m_bdSubsystem
            End Get
            Set(ByVal Value As Forms.Behavior.Diagram)
                m_bdSubsystem = Value
            End Set
        End Property

        Public Overrides Property Text() As String
            Get
                Return m_strText
            End Get
            Set(ByVal Value As String)
                If Value.Trim.Length > 0 Then
                    m_strText = Value
                    UpdateChart()
                Else
                    'If the text is set to blank then
                    'keep it the current value and reupdate
                    'the chart to put the text back.
                    UpdateChart(True)
                End If
                CheckForErrors()
            End Set
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.SubsystemNode.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property AllowStimulus() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

#Region " Methods "


        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Try

                Shape = Behavior.Node.enumShape.Rectangle
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.Black
                Me.FillColor = Color.LawnGreen

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "AnimatGUI.SubsystemNode.gif")
                Me.Name = "Neural Subsystem"

                Me.Font = New Font("Arial", 12, FontStyle.Bold)
                Me.Description = "Allows the user to build a hierarchachal neural system. This node expands into a new diagram."

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.Subsystem(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub AfterAddNode()
            m_bdSubsystem = m_ParentDiagram.AddDiagram("LicensedAnimatGUI.dll", "LicensedAnimatGUI.Forms.Behavior.AddFlowDiagram")
            m_bdSubsystem.Subsystem = Me
            Me.Text = m_bdSubsystem.TabPageName
            m_ParentEditor.SelectedDiagram(m_ParentDiagram)
        End Sub

        Public Overrides Sub BeforeRemoveNode()

        End Sub

        Public Overrides Sub AfterRemoveNode()

            If Not m_bdSubsystem Is Nothing Then
                m_ParentDiagram.RemoveDiagram(m_bdSubsystem)
                m_bdSubsystem.Delete()
            End If

            MyBase.AfterRemoveNode()
        End Sub

        Public Overrides Sub AfterUndoRemove()

            If Not m_bdSubsystem Is Nothing Then
                m_ParentDiagram.RestoreDiagram(m_bdSubsystem)
                m_ParentEditor.SelectedDiagram(m_ParentDiagram)
                m_ParentDiagram.SelectDataItem(Me)
            End If

            MyBase.AfterUndoRemove()
        End Sub

        Public Overrides Sub AfterRedoRemove()

            If Not m_bdSubsystem Is Nothing Then
                m_ParentDiagram.RemoveDiagram(m_bdSubsystem)
            End If

            MyBase.AfterRedoRemove()
        End Sub

        Public Overrides Function BeforeEdit(ByVal strNewText As String) As Boolean
            If strNewText.Trim.Length = 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Sub AfterEdit()
            m_ParentEditor.ChangeDiagramName(m_bdSubsystem, Me.Text)
        End Sub

        Public Overrides Sub DoubleClicked()
            If Not m_bdSubsystem Is Nothing Then
                m_ParentEditor.SelectedDiagram(m_bdSubsystem)
            End If
        End Sub

        Public Overrides Sub CreateDiagramDropDownTree(ByVal tvTree As TreeView, ByVal tnParent As TreeNode)
        End Sub

        Public Overrides Sub CheckForErrors()

            If m_ParentEditor Is Nothing OrElse m_ParentEditor.ErrorsBar Is Nothing Then Return

            If Me.Text Is Nothing OrElse Me.Text.Trim.Length = 0 Then
                If Not m_ParentEditor.ErrorsBar.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.EmptyName)) Then
                    Dim deError As New DiagramErrors.DataError(Me, DiagramError.enumErrorLevel.Warning, DiagramError.enumErrorTypes.EmptyName, "A node has no name.")
                    m_ParentEditor.ErrorsBar.Errors.Add(deError.ID, deError)
                End If
            Else
                If m_ParentEditor.ErrorsBar.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.EmptyName)) Then
                    m_ParentEditor.ErrorsBar.Errors.Remove(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.EmptyName))
                End If
            End If

        End Sub

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As TreeNode, ByVal tpTemplatePartType As Type) As TreeNode
        End Function

#Region " DataObject Methods "


        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()
            m_strSubsystemID = Util.LoadID(oXml, "Subsystem", True, "")
            oXml.OutOfElem()

        End Sub

        Public Overrides Sub InitializeAfterLoad()

            Try
                MyBase.InitializeAfterLoad()

                If m_bInitialized AndAlso Not m_ParentEditor Is Nothing Then
                    If m_strSubsystemID.Trim.Length > 0 Then
                        m_bdSubsystem = m_ParentEditor.FindDiagram(m_strSubsystemID)
                        Me.Text = m_bdSubsystem.TabPageName
                        m_bdSubsystem.Subsystem = Me   'Give this diagram a link to this subsystem
                    End If
                End If

            Catch ex As System.Exception
                m_bInitialized = False
                'If iAttempt = 1 Then
                '    AnimatGUI.Framework.Util.DisplayError(ex)
                'End If
            End Try

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            If Not m_bdSubsystem Is Nothing Then
                oXml.AddChildElement("SubsystemID", m_bdSubsystem.SelectedID)
            End If

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace