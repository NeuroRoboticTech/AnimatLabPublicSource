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

    Public Class OffPage
        Inherits Behavior.Node

#Region " Attributes "

        Protected m_thLinkedNode As TypeHelpers.LinkedNode

        'Only used during loading
        Protected m_strLinkedNodeID As String
        Protected m_strLinkedDiagramID As String

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "OffPage Connector"
            End Get
        End Property

        Public Overridable Property LinkedNode() As TypeHelpers.LinkedNode
            Get
                Return m_thLinkedNode
            End Get
            Set(ByVal Value As TypeHelpers.LinkedNode)
                Dim thPrevLinked As TypeHelpers.LinkedNode = m_thLinkedNode

                m_thLinkedNode = Value

                If Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing Then
                    Me.Text = m_thLinkedNode.Node.Text
                End If

                'If the user changes the item this node is linked to directly in the diagram after it
                'has already been connected up then we need to change the inlink/outlinks for all nodes
                'connected to this one.
                If Not thPrevLinked Is Nothing AndAlso Not thPrevLinked.Node Is Nothing _
                   AndAlso Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing _
                   AndAlso Not thPrevLinked.Node Is m_thLinkedNode.Node Then

                    'switch the inlinks from the prev node to the new one
                    Dim bdLink As AnimatGUI.DataObjects.Behavior.Link
                    For Each deEntry As DictionaryEntry In Me.InLinks
                        bdLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)

                        If thPrevLinked.Node.InLinks.Contains(bdLink.ID) Then thPrevLinked.Node.InLinks.Remove(bdLink.ID)
                        If Not m_thLinkedNode.Node.InLinks.Contains(bdLink.ID) Then m_thLinkedNode.Node.InLinks.Add(bdLink.ID, bdLink)
                        bdLink.ActualDestination = Me
                    Next

                    'switch the outlinks from the prev node to the new one
                    For Each deEntry As DictionaryEntry In Me.OutLinks
                        bdLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)

                        If thPrevLinked.Node.OutLinks.Contains(bdLink.ID) Then thPrevLinked.Node.OutLinks.Remove(bdLink.ID)
                        If Not m_thLinkedNode.Node.OutLinks.Contains(bdLink.ID) Then m_thLinkedNode.Node.OutLinks.Add(bdLink.ID, bdLink)
                        bdLink.ActualOrigin = Me
                    Next
                End If

                CheckForErrors()
            End Set
        End Property

        Public Overrides Property ParentEditor() As Forms.Behavior.Editor
            Get
                Return m_ParentEditor
            End Get
            Set(ByVal Value As Forms.Behavior.Editor)
                m_ParentEditor = Value
                m_thLinkedNode = New TypeHelpers.LinkedNode(m_ParentEditor, Nothing)

                If Not m_ParentEditor Is Nothing AndAlso Not m_ParentEditor.Organism Is Nothing Then
                    Me.Organism = m_ParentEditor.Organism
                End If
            End Set
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.OffPageConnector.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Try

                m_thLinkedNode = New TypeHelpers.LinkedNode(Nothing, Nothing)
                Shape = Behavior.Node.enumShape.OffPageConnection
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.Black
                Me.FillColor = Color.Gold

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "AnimatGUI.OffPageConnector.gif")
                Me.Name = "Off Page Connector"

                Me.Font = New Font("Arial", 12, FontStyle.Bold)
                Me.Description = "This item allows you to connect nodes on one diagram to nodes that reside on a different diagram."

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.OffPage(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As Behavior.Nodes.OffPage = DirectCast(doOriginal, Behavior.Nodes.OffPage)
            bnOrig.m_thLinkedNode = DirectCast(m_thLinkedNode.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedNode)
        End Sub

        Public Overrides Sub CreateDiagramDropDownTree(ByVal tvTree As TreeView, ByVal tnParent As TreeNode)
        End Sub

        Public Overrides Sub DoubleClicked()
            If Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing Then
                m_ParentEditor.SelectedDiagram(m_thLinkedNode.Node.ParentDiagram)
                m_thLinkedNode.Node.ParentDiagram.SelectDataItem(m_thLinkedNode.Node)
            End If
        End Sub


        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            If m_ParentEditor Is Nothing OrElse m_ParentEditor.ErrorsBar Is Nothing Then Return

            If m_thLinkedNode Is Nothing OrElse m_thLinkedNode.Node Is Nothing Then
                If Not m_ParentEditor.ErrorsBar.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.NodeNotSet)) Then
                    Dim deError As New DiagramErrors.DataError(Me, DiagramError.enumErrorLevel.Error, DiagramError.enumErrorTypes.NodeNotSet, _
                                                               "The offpage connector '" & Me.Text & "' has not been linked to another node. ")
                    m_ParentEditor.ErrorsBar.Errors.Add(deError.ID, deError)
                End If
            Else
                If m_ParentEditor.ErrorsBar.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.NodeNotSet)) Then
                    m_ParentEditor.ErrorsBar.Errors.Remove(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.NodeNotSet))
                End If
            End If

        End Sub

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As TreeNode, ByVal tpTemplatePartType As Type) As TreeNode
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linked Node", GetType(AnimatGUI.TypeHelpers.LinkedNode), "LinkedNode", _
                                        "Node Properties", "Sets the node that this associated with this connector.", m_thLinkedNode, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownTreeEditor), _
                                        GetType(AnimatGUI.TypeHelpers.LinkedNodeTypeConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_thLinkedNode Is Nothing Then m_thLinkedNode.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_strLinkedNodeID = Util.LoadID(oXml, "LinkedNode", True, "")
            m_strLinkedDiagramID = Util.LoadID(oXml, "LinkedDiagram", True, "")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub InitializeAfterLoad()

            Try
                If m_strLinkedNodeID.Trim.Length > 0 Then
                    Dim bnNode As Behavior.Node = Me.Organism.FindBehavioralNode(m_strLinkedNodeID, False)
                    If Not bnNode Is Nothing Then
                        m_thLinkedNode = New TypeHelpers.LinkedNode(m_ParentEditor, bnNode)
                    Else
                        Util.Logger.LogMsg(Interfaces.Logger.enumLogLevel.Error, "The offpage connector ID: " & Me.ID & " was unable to find its linked node ID: " & m_strLinkedNodeID & " in the diagram. I am defaulting it to nothing.")
                    End If
                End If

                Dim strID As String = ""

                Dim blLink As Behavior.Link
                For Each strID In m_aryLoadingInLinkIDs
                    If strID.Trim.Length > 0 Then
                        blLink = Me.Organism.FindBehavioralLink(strID, False)

                        If Not blLink Is Nothing Then
                            If Not m_aryInLinks.Contains(strID) Then m_aryInLinks.Add(strID, blLink)
                            If Not m_aryLinks.Contains(strID) Then m_aryLinks.Add(strID, blLink)

                            If Not Me.LinkedNode Is Nothing AndAlso Not Me.LinkedNode.Node Is Nothing Then
                                If Not Me.LinkedNode.Node.InLinks.Contains(strID) Then Me.LinkedNode.Node.InLinks.Add(strID, blLink)
                                If Not Me.LinkedNode.Node.Links.Contains(strID) Then Me.LinkedNode.Node.Links.Add(strID, blLink)
                            End If
                        End If
                    End If
                Next

                For Each strID In m_aryLoadingOutLinkIDs
                    If strID.Trim.Length > 0 Then
                        blLink = Me.Organism.FindBehavioralLink(strID, False)

                        If Not blLink Is Nothing Then
                            If Not m_aryOutLinks.Contains(strID) Then m_aryOutLinks.Add(strID, blLink)
                            If Not m_aryLinks.Contains(strID) Then m_aryLinks.Add(strID, blLink)

                            If Not Me.LinkedNode Is Nothing AndAlso Not Me.LinkedNode.Node Is Nothing Then
                                If Not Me.LinkedNode.Node.OutLinks.Contains(strID) Then Me.LinkedNode.Node.OutLinks.Add(strID, blLink)
                                If Not Me.LinkedNode.Node.Links.Contains(strID) Then Me.LinkedNode.Node.Links.Add(strID, blLink)
                            End If
                        End If
                    End If
                Next

                If m_strImageID.Trim.Length > 0 Then
                    If Not m_parentEditor Is Nothing Then
                        If m_ParentEditor.DiagramImages.Contains(m_strImageID) Then
                            m_diImage = m_ParentEditor.DiagramImages(m_strImageID)
                        End If
                    End If
                End If

                m_aryLoadingInLinkIDs.Clear()
                m_aryLoadingOutLinkIDs.Clear()

                m_bInitialized = True

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

            If Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing Then
                oXml.AddChildElement("LinkedDiagramID", m_thLinkedNode.Node.ParentDiagram.SelectedID)
                oXml.AddChildElement("LinkedNodeID", m_thLinkedNode.Node.SelectedID)
            End If

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace
