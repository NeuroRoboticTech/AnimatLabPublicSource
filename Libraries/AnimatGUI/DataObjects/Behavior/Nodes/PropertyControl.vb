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

    Public Class PropertyControl
        Inherits AnimatGUI.DataObjects.Behavior.Node

#Region " Attributes "

        Protected m_thLinkedObject As AnimatGUI.TypeHelpers.LinkedDataObjectTree
        Protected m_thLinkedProperty As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList
        Protected m_fltSetThreshold As Single = 0.5
        Protected m_fltInitialValue As Single = 0
        Protected m_fltFinalValue As Single = 1

        'Only used during loading
        Protected m_strLinkedObjectID As String = ""
        Protected m_strLinkedObjectProperty As String = ""

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property LinkedObject() As AnimatGUI.TypeHelpers.LinkedDataObjectTree
            Get
                Return m_thLinkedObject
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedDataObjectTree)
                Dim thPrevLinked As AnimatGUI.TypeHelpers.LinkedDataObjectTree = m_thLinkedObject

                RemoveLinkages(thPrevLinked, Value)
                DiconnectLinkedPartEvents()
                m_thLinkedObject = Value
                ReaddLinkages(thPrevLinked, Value)
                ConnectLinkedPartEvents()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedProperty() As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList
            Get
                Return m_thLinkedProperty
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList)
                m_thLinkedProperty = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property SetThreshold() As Single
            Get
                Return m_fltSetThreshold
            End Get
            Set(ByVal Value As Single)
                If Value < 0 Then
                    Throw New System.Exception("Set threshold value must be greater than 0.")
                End If

                m_fltSetThreshold = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property InitialValue() As Single
            Get
                Return m_fltInitialValue
            End Get
            Set(ByVal Value As Single)
                m_fltInitialValue = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property FinalValue() As Single
            Get
                Return m_fltFinalValue
            End Get
            Set(ByVal Value As Single)
                m_fltFinalValue = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.PropertyControlNode.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DragImageName As String
            Get
                Return "AnimatGUI.PropertyControlNode.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Property Control"
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
                Return Nothing
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property IncomingDataType() As AnimatGUI.DataObjects.DataType
            Get
                Return Nothing
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property LinkedID() As String
            Get
                If Not m_thLinkedObject Is Nothing AndAlso Not m_thLinkedObject.Item Is Nothing Then
                    Return m_thLinkedObject.Item.ID
                Else
                    Return Me.ID
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedPropertyName() As String
            Get
                If Not m_thLinkedProperty Is Nothing AndAlso Not m_thLinkedProperty.PropertyName Is Nothing Then
                    Return m_thLinkedProperty.PropertyName
                Else
                    Return ""
                End If
            End Get
            Set(value As String)
                If m_thLinkedObject Is Nothing OrElse m_thLinkedObject.Item Is Nothing Then
                    Throw New System.Exception("You cannot set the linked object property name until the linked object is set.")
                End If

                Me.LinkedProperty = New TypeHelpers.LinkedDataObjectPropertiesList(m_thLinkedObject.Item, value, False, True)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Try

                m_thLinkedObject = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(Nothing)
                m_thLinkedProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing, False, True)

                Shape = AnimatGUI.DataObjects.Behavior.Node.enumShape.Losange
                Size = New System.Drawing.SizeF(50, 50)
                Me.DrawColor = Color.Transparent
                Me.FillColor = Color.DarkOrchid
                Me.AutoSize = AnimatGUI.DataObjects.Behavior.Node.enumAutoSize.ImageToNode
                Me.Font = New Font("Arial", 14, FontStyle.Bold)

                Me.Name = "Property Control"
                Me.Description = "This node allows the user to control the properties of any object in the simulation."

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")
                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, Me.WorkspaceImageName)
                Me.DragImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, Me.DragImageName, False)

                AddCompatibleLink(New AnimatGUI.DataObjects.Behavior.Links.Adapter(Nothing))

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.PropertyControl(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim OrigNode As Data = DirectCast(doOriginal, Data)

            Dim bpPart As Nodes.PropertyControl = DirectCast(OrigNode, Nodes.PropertyControl)
            bpPart.m_thLinkedObject = DirectCast(m_thLinkedObject.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedDataObjectTree)
            bpPart.m_thLinkedProperty = DirectCast(m_thLinkedProperty.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedDataObjectPropertiesList)
            bpPart.m_fltSetThreshold = m_fltSetThreshold
            bpPart.m_fltInitialValue = m_fltInitialValue
            bpPart.m_fltFinalValue = m_fltFinalValue
        End Sub

        Protected Overridable Sub RemoveLinkages(ByVal thOldLink As AnimatGUI.TypeHelpers.LinkedDataObjectTree, ByVal thNewLink As AnimatGUI.TypeHelpers.LinkedDataObjectTree)

            m_thLinkedProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing, False, True)

            'If the user changes the item this node is linked to directly in the diagram after it
            'has already been connected up then we need to change the inlink/outlinks for all nodes
            'connected to this one.
            Dim aryRemoveLinks As New ArrayList
            Dim doNewNode As AnimatGUI.Framework.DataObject = Nothing
            If Not thNewLink Is Nothing Then
                doNewNode = thNewLink.Item
            End If

            If Not thOldLink Is Nothing AndAlso Not thOldLink.Item Is Nothing AndAlso Not doNewNode Is thOldLink.Item Then

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

        Protected Overridable Sub ReaddLinkages(ByVal thOldLink As AnimatGUI.TypeHelpers.LinkedDataObjectTree, ByVal thNewLink As AnimatGUI.TypeHelpers.LinkedDataObjectTree)

            If Not thNewLink Is Nothing AndAlso Not thNewLink.Item Is Nothing _
                AndAlso Not thOldLink Is Nothing AndAlso Not thOldLink.Item Is Nothing _
                AndAlso Not thNewLink.Item Is thOldLink.Item Then

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

            If Not thNewLink Is Nothing Then
                m_thLinkedProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(thNewLink.Item, False, True)
            End If

        End Sub

        Public Overrides Sub Automation_SetLinkedItem(ByVal strItemPath As String, ByVal strLinkedItemPath As String)

            Dim tnLinkedNode As Crownwood.DotNetMagic.Controls.Node = Util.FindTreeNodeByPath(strLinkedItemPath, Util.ProjectWorkspace.TreeView.Nodes)

            If tnLinkedNode Is Nothing OrElse tnLinkedNode.Tag Is Nothing OrElse Not Util.IsTypeOf(tnLinkedNode.Tag.GetType, GetType(Framework.DataObject), False) Then
                Throw New System.Exception("The path to the specified linked node was not the correct node type.")
            End If

            Dim doLinkedObject As Framework.DataObject = DirectCast(tnLinkedNode.Tag, Framework.DataObject)

            Me.LinkedObject = New TypeHelpers.LinkedDataObjectTree(doLinkedObject)

            Util.ProjectWorkspace.RefreshProperties()
        End Sub

        Public Overrides Sub InitializeAfterLoad()

            Try
                MyBase.InitializeAfterLoad()

                If m_bIsInitialized Then
                    Dim doObj As Framework.DataObject
                    If (m_strLinkedObjectID.Length > 0) Then
                        doObj = Util.Simulation.FindObjectByID(m_strLinkedObjectID)

                        If Not doObj Is Nothing Then
                            Me.LinkedObject = New TypeHelpers.LinkedDataObjectTree(doObj)
                        Else
                            Util.Application.DeleteItemAfterLoading(Me)
                            Util.DisplayError(New System.Exception("The property control ID: " & Me.ID & " was unable to find its linked item ID: " & m_strLinkedObjectID & ". This node and all links will be removed."))
                        End If
                    End If

                    If m_strLinkedObjectProperty.Trim.Length > 0 AndAlso Not m_thLinkedObject Is Nothing AndAlso Not m_thLinkedObject.Item Is Nothing Then
                        m_thLinkedProperty = New TypeHelpers.LinkedDataObjectPropertiesList(m_thLinkedObject.Item, m_strLinkedObjectProperty, False, True)
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

            If Not m_thLinkedObject Is Nothing Then m_thLinkedObject.ClearIsDirty()
            If Not m_thLinkedProperty Is Nothing Then m_thLinkedProperty.ClearIsDirty()
        End Sub

        Public Overrides Sub CheckCanAttachAdapter()
            'Only allow an adapter if we have a linked body part.
            If Not (Not m_thLinkedObject Is Nothing AndAlso Not m_thLinkedObject.Item Is Nothing) Then
                Throw New System.Exception("You must specify a linked object before you can add an adapter to this node.")
            End If
        End Sub

        Public Overrides Function NeedToUpdateAdapterID(ByVal propInfo As System.Reflection.PropertyInfo) As Boolean
            If propInfo.Name = "LinkedObject" OrElse propInfo.Name = "LinkedProperty" OrElse _
                propInfo.Name = "SetThreshold" OrElse propInfo.Name = "InitialValue" OrElse _
                propInfo.Name = "FinalValue" Then
                Return True
            End If
        End Function

        Protected Overridable Sub ConnectLinkedPartEvents()
            DiconnectLinkedPartEvents()

            If Not m_thLinkedObject Is Nothing AndAlso Not m_thLinkedObject.Item Is Nothing Then
                AddHandler m_thLinkedObject.Item.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedPart
            End If
        End Sub

        Protected Overridable Sub DiconnectLinkedPartEvents()
            If Not m_thLinkedObject Is Nothing AndAlso Not m_thLinkedObject.Item Is Nothing Then
                RemoveHandler m_thLinkedObject.Item.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedPart
            End If
        End Sub

        Public Overrides Function CreateNewAdapter(ByRef bnOrigin As DataObjects.Behavior.Node, ByRef doParent As Framework.DataObject) As DataObjects.Behavior.Node

            ''If it does require an adapter then lets add the pieces.
            Dim bnAdapter As AnimatGUI.DataObjects.Behavior.Node
            If bnOrigin Is Me Then
                Throw New System.Exception("Property controls can only have incoming links, not outgoing links.")
            ElseIf bnOrigin.IsPhysicsEngineNode AndAlso Not Me.IsPhysicsEngineNode Then
                Throw New System.Exception("You cannot connect a physics engine node directly to a property control node.")
            ElseIf Not bnOrigin.IsPhysicsEngineNode AndAlso Me.IsPhysicsEngineNode Then
                'If the origin is regular node and the destination is a physics node
                bnAdapter = New AnimatGUI.DataObjects.Behavior.Nodes.PropertyControlAdapter(doParent)
            Else
                'If both the origin and destination are physics nodes.
                Throw New System.Exception("You can only link two physics nodes using a graphical link.")
            End If

            Return bnAdapter
        End Function

        'For most all nodes this is just a pass through. Some source nodes need to be able to validate the adapter though.
        Public Overrides Function ValidateDestinationAdapterChosen(ByVal bnAdapter As DataObjects.Behavior.Node) As DataObjects.Behavior.Node
            Throw New System.Exception("Property controls can only have incoming links, not outgoing links.")
        End Function

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()
            m_strLinkedObjectID = oXml.GetChildString("LinkedDataObjectID", "")
            m_strLinkedObjectProperty = oXml.GetChildString("LinkedDataObjectProperty", "")
            m_fltSetThreshold = oXml.GetChildFloat("SetThreshold", m_fltSetThreshold)
            m_fltInitialValue = oXml.GetChildFloat("InitialValue", m_fltInitialValue)
            m_fltFinalValue = oXml.GetChildFloat("FinalValue", m_fltFinalValue)
            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            If Not m_thLinkedObject Is Nothing AndAlso Not m_thLinkedObject.Item Is Nothing Then
                oXml.AddChildElement("LinkedDataObjectID", m_thLinkedObject.Item.ID)
                oXml.AddChildElement("LinkedDataObjectProperty", Me.LinkedPropertyName)
            End If

            oXml.AddChildElement("SetThreshold", m_fltSetThreshold)
            oXml.AddChildElement("InitialValue", m_fltInitialValue)
            oXml.AddChildElement("FinalValue", m_fltFinalValue)

            oXml.OutOfElem() ' Outof Node Element
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Remove("ID")
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.LinkedID.GetType(), "LinkedID", _
                                        "Node Properties", "ID", Me.LinkedID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linked Object", GetType(AnimatGUI.TypeHelpers.LinkedDataObjectTree), "LinkedObject", _
                                        "Node Properties", "Sets the object that is associated with this connector.", m_thLinkedObject, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownTreeEditor), _
                                        GetType(AnimatGUI.TypeHelpers.LinkedDataObjectTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linked Property", GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList), "LinkedProperty", _
                                        "Node Properties", "Determines the property that is set by this controller.", m_thLinkedProperty, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                        GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Set Threshold", m_fltSetThreshold.GetType(), "SetThreshold", _
                                        "Node Properties", "Threshold at which the property value will be set. This is the difference between the current value and value when it was last set", Me.SetThreshold))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Initial Value", m_fltInitialValue.GetType(), "InitialValue", _
                                        "Node Properties", "Initial value the property control will set for the property when simulation starts", Me.InitialValue))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Final Value", m_fltFinalValue.GetType(), "FinalValue", _
                                        "Node Properties", "Final value the property control will set for the property when simulation ends", Me.FinalValue))

        End Sub

#End Region

#Region "Events"

        Private Sub OnAfterRemoveLinkedPart(ByRef doObject As Framework.DataObject)
            Try
                Me.LinkedObject = New TypeHelpers.LinkedDataObjectTree(Nothing)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

