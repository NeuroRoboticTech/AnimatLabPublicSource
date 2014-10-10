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
        Protected m_strLinkedNodeID As String = ""

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

                RemoveLinkages(thPrevLinked, Value)
                DisconnectLinkedNodeEvents()
                m_thLinkedNode = Value
                ReaddLinkages(thPrevLinked, Value)
                ConnectLinkedNodeEvents()

                SetDataType()
            End Set
        End Property

        Public Overrides Property Organism As Physical.Organism
            Get
                Return MyBase.Organism
            End Get
            Set(ByVal value As Physical.Organism)
                MyBase.Organism = value

                If Not Me.Organism Is Nothing Then
                    m_thLinkedNode = New TypeHelpers.LinkedNode(Me.Organism, Nothing)
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

        'We do not want offpage connectors to show up in the node tree view drop down.
        Public Overrides Sub CreateNodeTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal aryNodes As Crownwood.DotNetMagic.Controls.NodeCollection)
        End Sub

        Public Overrides Sub DoubleClicked()
            If Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing Then
                If Not m_thLinkedNode.Node.ParentDiagram Is Nothing AndAlso Not m_thLinkedNode.Node.ParentDiagram.TabPage Is Nothing Then
                    m_thLinkedNode.Node.ParentDiagram.TabPage.Selected = True
                End If
                m_thLinkedNode.Node.SelectItem(False)
            End If
        End Sub

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            If Util.Application.ProjectErrors Is Nothing Then Return

            If m_thLinkedNode Is Nothing OrElse m_thLinkedNode.Node Is Nothing Then
                If Not Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.NodeNotSet)) Then
                    Dim deError As New DiagramErrors.DataError(Me, DiagramError.enumErrorLevel.Error, DiagramError.enumErrorTypes.NodeNotSet, _
                                                               "The offpage connector '" & Me.Text & "' has not been linked to another node. ")
                    Util.Application.ProjectErrors.Errors.Add(deError.ID, deError)
                End If
            Else
                If Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.NodeNotSet)) Then
                    Util.Application.ProjectErrors.Errors.Remove(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.NodeNotSet))
                End If
            End If

        End Sub

        Public Overrides Sub CheckCanAttachAdapter()
            'Only allow an adapter if we have a linked body part.
            If Not (Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing) Then
                Throw New System.Exception("You must specify a linked node before you can add an adapter to this node.")
            End If
        End Sub

        Public Overrides Function NeedToUpdateAdapterID(ByVal propInfo As System.Reflection.PropertyInfo) As Boolean
            If propInfo.Name = "LinkedNode" Then
                Return True
            End If
        End Function

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, ByVal tpTemplatePartType As Type) As Crownwood.DotNetMagic.Controls.Node
        End Function

        Protected Overridable Sub RemoveLinkages(ByVal thOldLink As TypeHelpers.LinkedNode, ByVal thNewLink As TypeHelpers.LinkedNode)
            'If the user changes the item this node is linked to directly in the diagram after it
            'has already been connected up then we need to change the inlink/outlinks for all nodes
            'connected to this one.
            Dim aryRemoveLinks As New ArrayList
            Dim doNewNode As Behavior.Node = Nothing
            If Not thNewLink Is Nothing Then
                doNewNode = thNewLink.Node
            End If

            If Not thOldLink Is Nothing AndAlso Not thOldLink.Node Is Nothing AndAlso Not doNewNode Is thOldLink.Node Then

                'switch the inlinks from the prev node to the new one
                Dim bdLink As AnimatGUI.DataObjects.Behavior.Link
                For Each deEntry As DictionaryEntry In Me.InLinks
                    bdLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)

                    If bdLink.IsLinkCompatibleWithNodes(bdLink.Origin, doNewNode) Then
                        If thOldLink.Node.InLinks.Contains(bdLink.ID) Then thOldLink.Node.RemoveInLink(bdLink)
                        If Not thNewLink.Node.InLinks.Contains(bdLink.ID) Then thNewLink.Node.AddInLink(bdLink)

                        bdLink.RemoveFromSim(True)
                    Else
                        aryRemoveLinks.Add(bdLink)
                    End If
                Next

                'switch the outlinks from the prev node to the new one
                For Each deEntry As DictionaryEntry In Me.OutLinks
                    bdLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)

                    If bdLink.IsLinkCompatibleWithNodes(doNewNode, bdLink.Destination) Then
                        If thOldLink.Node.OutLinks.Contains(bdLink.ID) Then thOldLink.Node.RemoveOutLink(bdLink)
                        If Not thNewLink.Node.OutLinks.Contains(bdLink.ID) Then thNewLink.Node.AddOutLink(bdLink)

                        bdLink.RemoveFromSim(True)
                    Else
                        aryRemoveLinks.Add(bdLink)
                    End If
                Next
            End If

                For Each bdLink As AnimatGUI.DataObjects.Behavior.Link In aryRemoveLinks
                    bdLink.Delete(False)
                Next

        End Sub

        Protected Overridable Sub ReaddLinkages(ByVal thOldLink As TypeHelpers.LinkedNode, ByVal thNewLink As TypeHelpers.LinkedNode)

            If Not thNewLink Is Nothing AndAlso Not thNewLink.Node Is Nothing _
                AndAlso Not thOldLink Is Nothing AndAlso Not thOldLink.Node Is Nothing _
                AndAlso Not thNewLink.Node Is thOldLink.Node Then

                'switch the inlinks from the prev node to the new one
                Dim bdLink As AnimatGUI.DataObjects.Behavior.Link
                For Each deEntry As DictionaryEntry In Me.InLinks
                    bdLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                    bdLink.ActualDestination = Me
                    bdLink.RemoveWorksapceTreeView()
                    bdLink.AddWorkspaceTreeNode()
                    bdLink.AddToSim(True)
                Next

                'switch the outlinks from the prev node to the new one
                For Each deEntry As DictionaryEntry In Me.OutLinks
                    bdLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                    bdLink.ActualOrigin = Me
                    bdLink.RemoveWorksapceTreeView()
                    bdLink.AddWorkspaceTreeNode()
                    bdLink.AddToSim(True)
                Next
            End If
        End Sub

        Protected Sub SetDataType()
            If Not m_thLinkedNode.Node Is Nothing AndAlso Not m_thLinkedNode.Node.DataTypes Is Nothing Then
                m_thDataTypes = DirectCast(m_thLinkedNode.Node.DataTypes.Clone(m_thLinkedNode.Node.DataTypes.Parent, False, Nothing), TypeHelpers.DataTypeID)

                'Go through and change all of the adapters connected to this body part.
                For Each deItem As System.Collections.DictionaryEntry In Me.m_aryOutLinks
                    If Util.IsTypeOf(deItem.Value.GetType(), GetType(AnimatGUI.DataObjects.Behavior.Links.Adapter), False) Then
                        Dim blAdapter As AnimatGUI.DataObjects.Behavior.Links.Adapter = DirectCast(deItem.Value, AnimatGUI.DataObjects.Behavior.Links.Adapter)

                        If Not blAdapter.Destination Is Nothing Then
                            blAdapter.Destination.DataTypes = DirectCast(m_thDataTypes.Clone(blAdapter.Destination, False, Nothing), TypeHelpers.DataTypeID)
                        End If
                    End If
                Next
            Else
                m_thDataTypes = New AnimatGUI.TypeHelpers.DataTypeID(Me)
            End If

            CheckForErrors()
        End Sub

        Protected Sub ConnectLinkedNodeEvents()
            DisconnectLinkedNodeEvents()

            If Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing Then
                Me.Text = m_thLinkedNode.Node.Text
                AddHandler m_thLinkedNode.Node.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedNode
            End If
        End Sub


        Protected Sub DisconnectLinkedNodeEvents()
            If Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing Then
                RemoveHandler m_thLinkedNode.Node.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedNode
            End If
        End Sub

        Public Overrides Sub Automation_SetLinkedItem(ByVal strItemPath As String, ByVal strLinkedItemPath As String)

            Dim tnLinkedNode As Crownwood.DotNetMagic.Controls.Node = Util.FindTreeNodeByPath(strLinkedItemPath, Util.ProjectWorkspace.TreeView.Nodes)

            If tnLinkedNode Is Nothing OrElse tnLinkedNode.Tag Is Nothing OrElse Not Util.IsTypeOf(tnLinkedNode.Tag.GetType, GetType(DataObjects.Behavior.Node), False) Then
                Throw New System.Exception("The path to the specified linked node was not the correct node type.")
            End If

            Dim bnLinkedNode As DataObjects.Behavior.Node = DirectCast(tnLinkedNode.Tag, DataObjects.Behavior.Node)

            Dim lnNode As New TypeHelpers.LinkedNode(bnLinkedNode.Organism, bnLinkedNode)

            Dim strOriginalName As String = Me.Name
            Me.LinkedNode = lnNode

            'Reset the original name while doing automation tests so that each object can maintain a unique name.
            Me.Name = strOriginalName

            Util.ProjectWorkspace.RefreshProperties()
        End Sub

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

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_strLinkedNodeID = Util.LoadID(oXml, "LinkedNode", True, "")
            'm_strLinkedDiagramID = Util.LoadID(oXml, "LinkedDiagram", True, "")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub InitializeAfterLoad()

            If m_strLinkedNodeID.Trim.Length > 0 Then
                Dim bnNode As Behavior.Node = Me.Organism.FindBehavioralNode(m_strLinkedNodeID, False)
                If Not bnNode Is Nothing Then
                    Me.LinkedNode = New TypeHelpers.LinkedNode(bnNode.Organism, bnNode)
                Else
                    Util.Application.DeleteItemAfterLoading(Me)
                    Util.DisplayError(New System.Exception("The offpage connector ID: " & Me.ID & " was unable to find its linked node ID: " & m_strLinkedNodeID & " in the diagram. This node and all links will be removed."))
                End If
            End If

            ConnectLinkedNodeEvents()

            m_bIsInitialized = True

        End Sub

        ''' \brief  Initializes the simulation references.
        ''' 		
        ''' \details The offpage connector does not have a corresponding part in the simulation, so we need to skip this method
        ''' 		 for this object.
        '''
        ''' \author dcofer
        ''' \date   9/7/2011
        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            If Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing Then
                'oXml.AddChildElement("LinkedDiagramID", m_thLinkedNode.Node.ParentDiagram.ID)
                oXml.AddChildElement("LinkedNodeID", m_thLinkedNode.Node.ID)
            End If

            oXml.OutOfElem() ' Outof Node Element

        End Sub

        Public Overrides Sub AddInLink(ByRef blLink As Behavior.Link)
            MyBase.AddInLink(blLink)
            If Not Me.LinkedNode Is Nothing AndAlso Not Me.LinkedNode.Node Is Nothing Then
                Me.LinkedNode.Node.AddInLink(blLink)
            End If
        End Sub

        Public Overrides Sub RemoveInLink(ByRef blLink As Behavior.Link)
            MyBase.RemoveInLink(blLink)
            If Not Me.LinkedNode Is Nothing AndAlso Not Me.LinkedNode.Node Is Nothing Then
                Me.LinkedNode.Node.RemoveInLink(blLink)
            End If
        End Sub

        Public Overrides Sub AddOutLink(ByRef blLink As Behavior.Link)
            MyBase.AddOutLink(blLink)
            If Not Me.LinkedNode Is Nothing AndAlso Not Me.LinkedNode.Node Is Nothing Then
                Me.LinkedNode.Node.AddOutLink(blLink)
            End If
        End Sub

        Public Overrides Sub RemoveOutLink(ByRef blLink As Behavior.Link)
            MyBase.RemoveOutLink(blLink)
            If Not Me.LinkedNode Is Nothing AndAlso Not Me.LinkedNode.Node Is Nothing Then
                Me.LinkedNode.Node.RemoveOutLink(blLink)
            End If
        End Sub


#End Region

#End Region

#Region "Events"

        Private Sub OnAfterRemoveLinkedNode(ByRef doObject As Framework.DataObject)
            Try
                Me.LinkedNode = New TypeHelpers.LinkedNode(Me.ParentSubsystem.Organism, Nothing)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnBeforeParentRemoveFromList(ByRef doObject As AnimatGUI.Framework.DataObject)
            Try
                DisconnectLinkedNodeEvents()
                MyBase.OnBeforeParentRemoveFromList(doObject)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region


    End Class

End Namespace
