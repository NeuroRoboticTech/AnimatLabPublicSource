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

    Public MustInherit Class BodyPart
        Inherits AnimatGUI.DataObjects.Behavior.Node

#Region " Attributes "

        Protected m_thLinkedPart As AnimatGUI.TypeHelpers.LinkedBodyPart
        Protected m_tpBodyPartType As System.Type

        'Only used during loading
        Protected m_strLinkedBodyPartID As String = ""

#End Region

#Region " Properties "

        Public Overrides Property Organism As Physical.Organism
            Get
                Return MyBase.Organism
            End Get
            Set(ByVal value As Physical.Organism)
                MyBase.Organism = value

                If Not Me.Organism Is Nothing Then
                    Me.LinkedPart = CreateBodyPartList(Me.Organism, Nothing, m_tpBodyPartType)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedPart() As AnimatGUI.TypeHelpers.LinkedBodyPart
            Get
                Return m_thLinkedPart
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedBodyPart)
                Dim thPrevLinked As AnimatGUI.TypeHelpers.LinkedBodyPart = m_thLinkedPart

                RemoveLinkages(thPrevLinked, Value)
                DiconnectLinkedPartEvents()
                m_thLinkedPart = Value
                ReaddLinkages(thPrevLinked, Value)
                ConnectLinkedPartEvents()

                SetDataType()
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Body Part"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Behavior.PhysicsModule)
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property IsPhysicsEngineNode() As Boolean
            Get
                Return True
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property AllowStimulus() As Boolean
            Get
                Return False
            End Get
        End Property

        'For some objects, like body parts, what we really want to chart is not the behavior.bodypart object, but the underlying linked object.
        'This gives the object a way to return a reference to someing else to be charted instead of itself if this is needed.
        <Browsable(False)> _
        Public Overrides ReadOnly Property DataColumnItem() As DragObject
            Get
                If m_thLinkedPart Is Nothing OrElse m_thLinkedPart.BodyPart Is Nothing Then
                    Throw New System.Exception("You can not add a body part for graphing until you have set the part ID that it is associated to.")
                End If

                Return m_thLinkedPart.BodyPart
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                If m_thLinkedPart Is Nothing OrElse m_thLinkedPart.BodyPart Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides Property IncomingDataTypes As TypeHelpers.DataTypeID
            Get
                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    Return m_thLinkedPart.BodyPart.IncomingDataTypes
                End If
                Return Nothing
            End Get
            Set(value As TypeHelpers.DataTypeID)
                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    m_thLinkedPart.BodyPart.IncomingDataTypes = value
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property LinkedID() As String
            Get
                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    Return m_thLinkedPart.BodyPart.ID
                Else
                    Return Me.ID
                End If
            End Get
        End Property

        Public MustOverride ReadOnly Property BaseErrorType() As DiagramError.enumErrorTypes

        <Browsable(False)> _
        Public Overrides ReadOnly Property IsSensorOrMotor() As Boolean
            Get
                Return True
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Try

                m_thLinkedPart = CreateBodyPartList(Me)

                Shape = AnimatGUI.DataObjects.Behavior.Node.enumShape.Rectangle
                Size = New System.Drawing.SizeF(50, 50)
                Me.DrawColor = Color.Transparent
                Me.FillColor = Color.White
                Me.AutoSize = AnimatGUI.DataObjects.Behavior.Node.enumAutoSize.ImageToNode
                Me.Font = New Font("Arial", 14, FontStyle.Bold)
                Me.Alignment = enumAlignment.CenterBottom

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")
                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, Me.WorkspaceImageName)
                Me.DragImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, Me.DragImageName, False)

                AddCompatibleLink(New AnimatGUI.DataObjects.Behavior.Links.Adapter(Nothing))

                m_tpBodyPartType = GetType(AnimatGUI.DataObjects.Physical.BodyPart)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Overloads Function CreateBodyPartList(ByVal doParent As AnimatGUI.Framework.DataObject) As TypeHelpers.LinkedBodyPart
            Return New AnimatGUI.TypeHelpers.LinkedBodyPartTree(doParent)
        End Function

        Protected Overridable Overloads Function CreateBodyPartList(ByVal doStruct As Physical.PhysicalStructure, ByVal doBodyPart As Physical.BodyPart, ByVal tpBodyPartType As System.Type) As TypeHelpers.LinkedBodyPart
            Return New AnimatGUI.TypeHelpers.LinkedBodyPartTree(doStruct, doBodyPart, tpBodyPartType)
        End Function

        Protected Overridable Function GetBodyPartListDropDownType() As System.Type
            Return GetType(AnimatGUI.TypeHelpers.DropDownTreeEditorNoFirstSelect)
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim OrigNode As Data = DirectCast(doOriginal, Data)

            Dim bpPart As Nodes.BodyPart = DirectCast(OrigNode, Nodes.BodyPart)
            bpPart.m_thLinkedPart = DirectCast(m_thLinkedPart.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedBodyPart)

        End Sub

        Protected Overridable Sub RemoveLinkages(ByVal thOldLink As AnimatGUI.TypeHelpers.LinkedBodyPart, ByVal thNewLink As AnimatGUI.TypeHelpers.LinkedBodyPart)
            'If the user changes the item this node is linked to directly in the diagram after it
            'has already been connected up then we need to change the inlink/outlinks for all nodes
            'connected to this one.
            Dim aryRemoveLinks As New ArrayList
            Dim doNewNode As AnimatGUI.DataObjects.Physical.BodyPart = Nothing
            If Not thNewLink Is Nothing Then
                doNewNode = thNewLink.BodyPart
            End If

            If Not thOldLink Is Nothing AndAlso Not thOldLink.BodyPart Is Nothing AndAlso Not doNewNode Is thOldLink.BodyPart Then

                'switch the inlinks from the prev node to the new one
                Dim bdLink As AnimatGUI.DataObjects.Behavior.Link
                For Each deEntry As DictionaryEntry In Me.Links
                    bdLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)

                    If Not doNewNode Is Nothing Then
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

        Protected Overridable Sub ReaddLinkages(ByVal thOldLink As AnimatGUI.TypeHelpers.LinkedBodyPart, ByVal thNewLink As AnimatGUI.TypeHelpers.LinkedBodyPart)

            If Not thNewLink Is Nothing AndAlso Not thNewLink.BodyPart Is Nothing _
                AndAlso Not thOldLink Is Nothing AndAlso Not thOldLink.BodyPart Is Nothing _
                AndAlso Not thNewLink.BodyPart Is thOldLink.BodyPart Then

                'switch the inlinks from the prev node to the new one
                Dim bdLink As AnimatGUI.DataObjects.Behavior.Link
                For Each deEntry As DictionaryEntry In Me.InLinks
                    bdLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                    bdLink.ActualDestination = Me
                    bdLink.AddToSim(True)
                Next

                'switch the outlinks from the prev node to the new one
                For Each deEntry As DictionaryEntry In Me.OutLinks
                    bdLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                    bdLink.ActualOrigin = Me
                    bdLink.AddToSim(True)
                Next
            End If
        End Sub

        Protected Sub SetDataType()
            If Not m_thLinkedPart.BodyPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart.DataTypes Is Nothing Then
                m_thDataTypes = DirectCast(m_thLinkedPart.BodyPart.DataTypes.Clone(m_thLinkedPart.BodyPart.DataTypes.Parent, False, Nothing), TypeHelpers.DataTypeID)

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

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            If Util.Application.ProjectErrors Is Nothing Then Return

            If m_thLinkedPart Is Nothing OrElse m_thLinkedPart.BodyPart Is Nothing Then
                If Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, Me.BaseErrorType)) Then
                    Dim deError As DiagramErrors.DataError = New DiagramErrors.DataError(Me, DiagramError.enumErrorLevel.Error, Me.BaseErrorType, _
                                                  "The reference for the rigid body '" + Me.Text + "' is not set.")
                    Util.Application.ProjectErrors.Errors.Add(deError.ID, deError)
                End If
            Else
                If Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, Me.BaseErrorType)) Then
                    Util.Application.ProjectErrors.Errors.Remove(DiagramErrors.DataError.GenerateID(Me, Me.BaseErrorType))
                End If
            End If

        End Sub

        Public Overrides Sub Automation_SetLinkedItem(ByVal strItemPath As String, ByVal strLinkedItemPath As String)

            Dim tnLinkedNode As Crownwood.DotNetMagic.Controls.Node = Util.FindTreeNodeByPath(strLinkedItemPath, Util.ProjectWorkspace.TreeView.Nodes)

            If tnLinkedNode Is Nothing OrElse tnLinkedNode.Tag Is Nothing OrElse Not Util.IsTypeOf(tnLinkedNode.Tag.GetType, GetType(DataObjects.Physical.BodyPart), False) Then
                Throw New System.Exception("The path to the specified linked node was not the correct node type.")
            End If

            Dim bpLinkedPart As DataObjects.Physical.BodyPart = DirectCast(tnLinkedNode.Tag, DataObjects.Physical.BodyPart)

            Me.LinkedPart = CreateBodyPartList(m_doOrganism, bpLinkedPart, m_tpBodyPartType)

            Util.ProjectWorkspace.RefreshProperties()
        End Sub

        Public Overrides Sub InitializeAfterLoad()

            Try
                MyBase.InitializeAfterLoad()

                If m_bIsInitialized Then
                    Dim bpPart As AnimatGUI.DataObjects.Physical.BodyPart
                    If (Not Me.LinkedPart Is Nothing AndAlso Me.LinkedPart.BodyPart Is Nothing) AndAlso (m_strLinkedBodyPartID.Length > 0) Then
                        bpPart = m_doOrganism.FindBodyPart(m_strLinkedBodyPartID, False)

                        If Not bpPart Is Nothing Then
                            Me.LinkedPart = CreateBodyPartList(m_doOrganism, bpPart, m_tpBodyPartType)
                        Else
                            Util.Application.DeleteItemAfterLoading(Me)
                            Util.DisplayError(New System.Exception("The body part connector ID: " & Me.ID & " was unable to find its linked item ID: " & m_strLinkedBodyPartID & " in the diagram. This node and all links will be removed."))
                        End If
                    End If
                End If

            Catch ex As System.Exception
                m_bIsInitialized = False
            End Try
        End Sub

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, ByVal tpTemplatePartType As Type) As Crownwood.DotNetMagic.Controls.Node
            Return Nothing
        End Function

        ''' \brief  Initializes the simulation references.
        '''
        ''' \details This type of object has no related simulation object, so do not call base class here.
        ''' 		 
        ''' \author dcofer
        ''' \date   9/25/2011
        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_thLinkedPart Is Nothing Then m_thLinkedPart.ClearIsDirty()
        End Sub

        Public Overrides Sub CheckCanAttachAdapter()
            'Only allow an adapter if we have a linked body part.
            If Not (Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing) Then
                Throw New System.Exception("You must specify a linked body part before you can add an adapter to this node.")
            End If
        End Sub

        Public Overrides Function NeedToUpdateAdapterID(ByVal propInfo As System.Reflection.PropertyInfo) As Boolean
            If propInfo.Name = "LinkedPart" Then
                Return True
            End If
        End Function

        Protected Overridable Sub ConnectLinkedPartEvents()
            DiconnectLinkedPartEvents()

            If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                AddHandler m_thLinkedPart.BodyPart.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedPart
                AddHandler m_thLinkedPart.BodyPart.ReloadSourceDataTypes, AddressOf Me.OnReloadSourceDataTypes
                AddHandler m_thLinkedPart.BodyPart.ReloadTargetDataTypes, AddressOf Me.OnReloadTargetDataTypes
            End If
        End Sub

        Protected Overridable Sub DiconnectLinkedPartEvents()
            If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                RemoveHandler m_thLinkedPart.BodyPart.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedPart
                RemoveHandler m_thLinkedPart.BodyPart.ReloadSourceDataTypes, AddressOf Me.OnReloadSourceDataTypes
                RemoveHandler m_thLinkedPart.BodyPart.ReloadTargetDataTypes, AddressOf Me.OnReloadTargetDataTypes
            End If
        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()
            m_strLinkedBodyPartID = Util.LoadID(oXml, "LinkedBodyPart", True, "") 'Note: The ID of the name is added in the LoadID method.
            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                oXml.AddChildElement("LinkedBodyPartID", m_thLinkedPart.BodyPart.ID)
            End If

            oXml.OutOfElem() ' Outof Node Element
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Remove("ID")
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.LinkedID.GetType(), "LinkedID", _
                                        "Node Properties", "ID", Me.LinkedID, True))

            'Now lets add the property for the linked muscle.
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec(Me.TypeName & " ID", m_thLinkedPart.GetType, "LinkedPart", _
                                        Me.TypeName & " Properties", "Associates this " & Me.TypeName.ToLower & " node to an ID of a " & Me.TypeName.ToLower & " that exists within the body of the organism.", m_thLinkedPart, _
                                        Me.GetBodyPartListDropDownType, _
                                        GetType(AnimatGUI.TypeHelpers.LinkedBodyPartTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Node Type", GetType(String), "TypeName", _
                                        Me.TypeName & " Properties", "Returns the type of this node.", TypeName(), True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Description", m_strDescription.GetType(), "ToolTip", _
                                        Me.TypeName & " Properties", "Sets the description for this " & Me.TypeName.ToLower & " connection.", m_strToolTip, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

        End Sub

#End Region

#Region "Events"

        Private Sub OnAfterRemoveLinkedPart(ByRef doObject As Framework.DataObject)
            Try
                Me.LinkedPart = CreateBodyPartList(Me.Organism, Nothing, m_tpBodyPartType)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnReloadSourceDataTypes()
            Try
                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    SetDataType()
                    SignalReloadSourceDataTypes()
                End If


            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnReloadTargetDataTypes()
            Try
                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    SetDataType()
                    SignalReloadTargetDataTypes()
                End If

            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

