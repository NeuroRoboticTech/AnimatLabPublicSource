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

Namespace DataObjects
    Namespace Robotics

        Public MustInherit Class RemoteControlLinkage
            Inherits DataObjects.DragObject

#Region " Attributes "

            Protected m_doParentRemoteControl As RemoteControl

            Protected m_bInLink As Boolean = True

            Protected m_thLinkedSource As AnimatGUI.TypeHelpers.LinkedDataObjectTree
            Protected m_thLinkedSourceProperty As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList

            Protected m_thLinkedTarget As AnimatGUI.TypeHelpers.LinkedDataObjectTree
            Protected m_thLinkedTargetProperty As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList

            Protected m_iPropertyID As Integer = 0

            'Only used during loading
            Protected m_strLinkedSourceID As String = ""
            Protected m_strLinkedSourceProperty As String = ""
            Protected m_strLinkedTargetID As String = ""
            Protected m_strLinkedTargetProperty As String = ""

#End Region

#Region " Properties "

            <Browsable(False)> _
            Public Overridable Property LinkedSource() As AnimatGUI.TypeHelpers.LinkedDataObjectTree
                Get
                    Return m_thLinkedSource
                End Get
                Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedDataObjectTree)
                    If Not Value Is Nothing AndAlso Not Value.Item Is Nothing Then
                        SetSimData("SourceID", Value.Item.ID, True)
                    Else
                        SetSimData("SourceID", "", True)
                    End If

                    m_thLinkedSource = Value

                    If Not m_thLinkedSource Is Nothing Then
                        m_thLinkedSourceProperty = New TypeHelpers.LinkedDataObjectPropertiesList(m_thLinkedSource.Item, True, False)
                    End If
                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property LinkedSourceProperty() As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList
                Get
                    Return m_thLinkedSourceProperty
                End Get
                Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList)
                    If Not Value Is Nothing AndAlso Not Value.Item Is Nothing AndAlso Not Value.PropertyName Is Nothing Then
                        SetSimData("SourceDataTypeID", Value.PropertyName, True)

                        If m_bInLink Then
                            SetSimData("PropertyName", Value.PropertyName, True)
                        End If
                    Else
                        SetSimData("SourceDataTypeID", "", True)

                        If m_bInLink Then
                            SetSimData("PropertyName", "", True)
                        End If
                    End If

                    m_thLinkedSourceProperty = Value
                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property LinkedSourcePropertyName() As String
                Get
                    Return m_strLinkedSourceProperty
                End Get
                Set(value As String)
                    If m_bInLink Then
                        SetSimData("SourceDataTypeID", value, True)
                        SetSimData("PropertyName", value, True)
                    End If

                    m_strLinkedSourceProperty = value
                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property LinkedTarget() As AnimatGUI.TypeHelpers.LinkedDataObjectTree
                Get
                    Return m_thLinkedTarget
                End Get
                Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedDataObjectTree)
                    If Not Value Is Nothing AndAlso Not Value.Item Is Nothing Then
                        SetSimData("TargetID", Value.Item.ID, True)
                    Else
                        SetSimData("TargetID", "", True)
                    End If

                    m_thLinkedTarget = Value

                    If Not m_thLinkedTarget Is Nothing Then
                        m_thLinkedTargetProperty = New TypeHelpers.LinkedDataObjectPropertiesList(m_thLinkedTarget.Item, True, False)
                    End If
                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property LinkedTargetProperty() As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList
                Get
                    Return m_thLinkedTargetProperty
                End Get
                Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList)
                    If Not Value Is Nothing AndAlso Not Value.Item Is Nothing AndAlso Not Value.PropertyName Is Nothing Then
                        SetSimData("TargetDataTypeID", Value.PropertyName, True)

                        If Not m_bInLink Then
                            SetSimData("PropertyName", Value.PropertyName, True)
                        End If
                    Else
                        SetSimData("TargetDataTypeID", "", True)

                        If Not m_bInLink Then
                            SetSimData("PropertyName", "", True)
                        End If
                    End If

                    m_thLinkedTargetProperty = Value
                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property LinkedTargetPropertyName() As String
                Get
                    Return m_strLinkedTargetProperty
                End Get
                Set(value As String)
                    If Not m_bInLink Then
                        SetSimData("TargetDataTypeID", value, True)
                        SetSimData("PropertyName", value, True)
                    End If

                    m_strLinkedTargetProperty = value
                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property InLink As Boolean
                Get
                    Return m_bInLink
                End Get
                Set(value As Boolean)
                    m_bInLink = value
                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property PropertyID() As Integer
                Get
                    Return m_iPropertyID
                End Get
                Set(value As Integer)
                    SetSimData("PropertyID", value.ToString(), True)
                    m_iPropertyID = value
                End Set
            End Property

#Region "DragObject Properties"

            Public Overrides Property ItemName As String
                Get
                    Return Me.Name()
                End Get
                Set(value As String)
                    Me.Name = value
                End Set
            End Property

            Public Overrides ReadOnly Property CanBeCharted As Boolean
                Get
                    Return True
                End Get
            End Property

            Public MustOverride ReadOnly Property LinkageType As String

#End Region

            Public Overrides ReadOnly Property WorkspaceImageName As String
                Get
                    Return "AnimatGUI.RobotIO.gif"
                End Get
            End Property

            Public Overridable Property ParentRemoteControl As RemoteControl
                Get
                    Return m_doParentRemoteControl
                End Get
                Set(value As RemoteControl)
                    m_doParentRemoteControl = value
                End Set
            End Property

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                m_strName = "Remote Control Link"
                m_bInLink = True

                If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType(), GetType(RemoteControl), False) Then
                    m_doParentRemoteControl = DirectCast(doParent, RemoteControl)
                End If

                m_thLinkedSource = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(Nothing)
                m_thLinkedSourceProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing, True, False)

                m_thLinkedTarget = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(Nothing)
                m_thLinkedTargetProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing, True, False)

                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("AppliedValue", "Applied Value", "", "", 0, 1))
                m_thDataTypes.ID = "AppliedValue"
            End Sub

            Public Sub New(ByVal doParent As Framework.DataObject, ByVal strName As String, ByVal strDataTypeID As String, ByVal doGain As Gain, ByVal bInLink As Boolean)
                MyBase.New(doParent)

                m_strName = strName
                m_bInLink = bInLink

                If bInLink Then
                    If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType(), GetType(RemoteControl), False) Then
                        m_doParentRemoteControl = DirectCast(doParent, RemoteControl)
                        m_thLinkedSource = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(m_doParentRemoteControl)

                        If m_doParentRemoteControl.UseRemoteDataTypes Then
                            m_thLinkedSourceProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(m_doParentRemoteControl, strDataTypeID, True, False)
                        Else
                            m_thLinkedSourceProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing, True, False)
                        End If

                        m_thLinkedTarget = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(Nothing)
                        m_thLinkedTargetProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing, True, False)
                    Else
                        m_thLinkedSource = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(Nothing)
                        m_thLinkedSourceProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing, True, False)

                        m_thLinkedTarget = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(Nothing)
                        m_thLinkedTargetProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing, True, False)
                    End If

                    m_strLinkedSourceProperty = strDataTypeID
                Else
                    If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType(), GetType(RemoteControl), False) Then
                        m_doParentRemoteControl = DirectCast(doParent, RemoteControl)
                        m_thLinkedTarget = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(m_doParentRemoteControl)

                        If m_doParentRemoteControl.UseRemoteDataTypes Then
                            m_thLinkedTargetProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(m_doParentRemoteControl, strDataTypeID, True, False)
                        Else
                            m_thLinkedTargetProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing, True, False)
                        End If

                        m_thLinkedSource = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(Nothing)
                        m_thLinkedSourceProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing, True, False)
                    Else
                        m_thLinkedSource = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(Nothing)
                        m_thLinkedSourceProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing, True, False)

                        m_thLinkedTarget = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(Nothing)
                        m_thLinkedTargetProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing, True, False)
                    End If

                    m_strLinkedTargetProperty = strDataTypeID
                End If

                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("AppliedValue", "Applied Value", "", "", 0, 1))
                m_thDataTypes.ID = "AppliedValue"
            End Sub

            Public Overrides Sub ClearIsDirty()
                MyBase.ClearIsDirty()

                If Not m_thLinkedTarget Is Nothing Then m_thLinkedSource.ClearIsDirty()
                If Not m_thLinkedTargetProperty Is Nothing Then m_thLinkedSourceProperty.ClearIsDirty()

                If Not m_thLinkedSource Is Nothing Then m_thLinkedSource.ClearIsDirty()
                If Not m_thLinkedSourceProperty Is Nothing Then m_thLinkedSourceProperty.ClearIsDirty()
            End Sub

            Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                                ByVal doRoot As AnimatGUI.Framework.DataObject)
                MyBase.CloneInternal(doOriginal, bCutData, doRoot)

                Dim OrigNode As RemoteControlLinkage = DirectCast(doOriginal, RemoteControlLinkage)

                m_thLinkedSource = DirectCast(OrigNode.m_thLinkedSource.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedDataObjectTree)
                m_thLinkedSourceProperty = DirectCast(OrigNode.m_thLinkedSourceProperty.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedDataObjectPropertiesList)
                m_strLinkedSourceProperty = OrigNode.m_strLinkedSourceProperty

                m_thLinkedTarget = DirectCast(OrigNode.m_thLinkedTarget.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedDataObjectTree)
                m_thLinkedTargetProperty = DirectCast(OrigNode.m_thLinkedTargetProperty.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedDataObjectPropertiesList)
                m_strLinkedTargetProperty = OrigNode.m_strLinkedTargetProperty

                m_iPropertyID = OrigNode.m_iPropertyID

                m_bInLink = OrigNode.m_bInLink
            End Sub

#Region " Workspace TreeView "

            Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

                If tnSelectedNode Is m_tnWorkspaceNode Then
                    ' Create the popup menu object
                    Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Robotics.RobotInterface.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)

                    Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Linkage", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))

                    popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete})

                    If Me.CanBeCharted AndAlso Not Util.Application.LastSelectedChart Is Nothing AndAlso Not Util.Application.LastSelectedChart.LastSelectedAxis Is Nothing Then
                        ' Create the menu items
                        Dim mcAddToChart As New System.Windows.Forms.ToolStripMenuItem("Add to Chart", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddChartItem.gif"), New EventHandler(AddressOf Util.Application.OnAddToChart))
                        popup.Items.Add(mcAddToChart)
                    End If

                    Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                    Return True
                End If

                Return False
            End Function

#End Region


#Region " Find Methods "

            Public Overrides Function FindDragObject(strStructureName As String, strDataItemID As String, Optional bThrowError As Boolean = True) As DragObject
                Throw New System.Exception("FindDragObject not implemented")
            End Function

#End Region

            Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", Me.ID.GetType(), "Name", _
                                            "Properties", "Name", Me.Name))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                            "Properties", "ID", Me.ID, True))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                            "Properties", "Determines if this controller is enabled or not.", m_bEnabled))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("InLink", GetType(Boolean), "InLink", _
                                            "Properties", "Tells whether this is an incoming or outgoing link.", m_bInLink, True))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Target", GetType(AnimatGUI.TypeHelpers.LinkedDataObjectTree), "LinkedTarget", _
                                                "Properties", "Sets the object that is associated with this linkage.", m_thLinkedTarget, _
                                                GetType(AnimatGUI.TypeHelpers.DropDownTreeEditor), _
                                                GetType(AnimatGUI.TypeHelpers.LinkedDataObjectTypeConverter)))

                If m_doParentRemoteControl.UseRemoteDataTypes OrElse m_bInLink Then
                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Target Property", GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList), "LinkedTargetProperty", _
                                                "Properties", "Determines the property that is set by this linkage.", m_thLinkedTargetProperty, _
                                                GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                                GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesTypeConverter)))
                Else
                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Target Property ", GetType(String), "LinkedTargetPropertyName", _
                                           "Properties", "Sets he property that is set by this linkage.", Me.LinkedTargetPropertyName))
                End If

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Source", GetType(AnimatGUI.TypeHelpers.LinkedDataObjectTree), "LinkedSource", _
                                            "Properties", "Sets the object that is associated with this linkage.", m_thLinkedSource, _
                                            GetType(AnimatGUI.TypeHelpers.DropDownTreeEditor), _
                                            GetType(AnimatGUI.TypeHelpers.LinkedDataObjectTypeConverter)))

                If m_doParentRemoteControl.UseRemoteDataTypes OrElse Not m_bInLink Then
                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Source Property", GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList), "LinkedSourceProperty", _
                                                "Properties", "Determines the property that is set by this linkage.", m_thLinkedSourceProperty, _
                                                GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                                GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesTypeConverter)))
                Else
                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Source Property ", GetType(String), "LinkedSourcePropertyName", _
                                           "Properties", "Sets the property that is set by this linkage.", Me.LinkedSourcePropertyName))
                End If

                If Not m_doParentRemoteControl.UseRemoteDataTypes Then
                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Data ID", Me.PropertyID.GetType(), "PropertyID", _
                                                "Properties", "ID of the data type associated with this parameter", Me.PropertyID))
                End If

            End Sub

#Region " Add-Remove to List Methods "

            Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
                If Not Me.Parent Is Nothing Then
                    Util.Application.SimulationInterface.AddItem(Me.Parent.ID, "RemoteControlLinkage", Me.ID, Me.GetSimulationXml("RemoteControlLinkage"), bThrowError, bDoNotInit)
                    InitializeSimulationReferences()
                End If
            End Sub

            Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
                If Not Me.Parent Is Nothing AndAlso Not m_doInterface Is Nothing Then
                    Util.Application.SimulationInterface.RemoveItem(Me.Parent.ID, "RemoteControlLinkage", Me.ID, bThrowError)
                End If
                m_doInterface = Nothing
            End Sub

            Public Overrides Sub AfterRemoveFromList(ByVal bCallSimMethods As Boolean, ByVal bThrowError As Boolean)
                MyBase.AfterRemoveFromList(bCallSimMethods, bThrowError)
                DisconnectLinkedNodeEvents()
            End Sub

#End Region

            Protected Sub ConnectLinkedNodeEvents()
                DisconnectLinkedNodeEvents()

                If m_bInLink Then
                    If Not m_thLinkedTarget Is Nothing AndAlso Not m_thLinkedTarget.Item Is Nothing Then
                        AddHandler m_thLinkedTarget.Item.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedItem
                    End If
                Else
                    If Not m_thLinkedSource Is Nothing AndAlso Not m_thLinkedSource.Item Is Nothing Then
                        AddHandler m_thLinkedSource.Item.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedItem
                    End If
                End If
            End Sub

            Protected Sub DisconnectLinkedNodeEvents()

                If m_bInLink Then
                    If Not m_thLinkedTarget Is Nothing AndAlso Not m_thLinkedTarget.Item Is Nothing Then
                        RemoveHandler m_thLinkedTarget.Item.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedItem
                    End If
                Else
                    If Not m_thLinkedSource Is Nothing AndAlso Not m_thLinkedSource.Item Is Nothing Then
                        RemoveHandler m_thLinkedSource.Item.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedItem
                    End If
                End If
            End Sub

            Public Overrides Sub InitializeAfterLoad()

                If Not Me.ParentRemoteControl Is Nothing AndAlso Not Me.ParentRemoteControl.Organism Is Nothing Then
                    If m_strLinkedSourceID.Trim.Length > 0 Then
                        Dim doSource As Framework.DataObject = Util.Simulation.FindObjectByID(m_strLinkedSourceID)

                        If Not doSource Is Nothing Then
                            Me.LinkedSource = New TypeHelpers.LinkedDataObjectTree(doSource)

                            If Not m_bInLink OrElse m_doParentRemoteControl.UseRemoteDataTypes Then
                                Me.LinkedSourceProperty = New TypeHelpers.LinkedDataObjectPropertiesList(doSource, m_strLinkedSourceProperty, True, False)
                            End If
                        Else
                            Util.Application.DeleteItemAfterLoading(Me)
                            Util.DisplayError(New System.Exception("The remote control linkage connector ID: " & Me.ID & " was unable to find its linked source object ID: " & m_strLinkedSourceID & ". This node and all links will be removed."))
                        End If
                    End If

                    If m_strLinkedTargetID.Trim.Length > 0 Then
                        Dim doTarget As Framework.DataObject = Util.Simulation.FindObjectByID(m_strLinkedTargetID)

                        If Not doTarget Is Nothing Then
                            Me.LinkedTarget = New TypeHelpers.LinkedDataObjectTree(doTarget)

                            If m_bInLink OrElse m_doParentRemoteControl.UseRemoteDataTypes Then
                                Me.LinkedTargetProperty = New TypeHelpers.LinkedDataObjectPropertiesList(doTarget, m_strLinkedTargetProperty, True, False)
                            End If
                        Else
                            Util.Application.DeleteItemAfterLoading(Me)
                            Util.DisplayError(New System.Exception("The remote control linkage connector ID: " & Me.ID & " was unable to find its linked target object ID: " & m_strLinkedTargetID & ". This node and all links will be removed."))
                        End If
                    End If

                    ConnectLinkedNodeEvents()

                    m_bIsInitialized = True
                End If

            End Sub

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.IntoElem()  'Into RobotInterface Element

                m_strName = oXml.GetChildString("Name", Me.Name)
                m_strID = oXml.GetChildString("ID", Me.ID)
                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)
                m_bInLink = oXml.GetChildBool("InLink", m_bInLink)
                m_iPropertyID = oXml.GetChildInt("PropertyID", m_iPropertyID)

                If oXml.FindChildElement("LinkedNodeID", False) Then
                    If Not m_doParentRemoteControl Is Nothing Then
                        m_strLinkedSourceID = m_doParentRemoteControl.ID
                        m_strLinkedTargetProperty = "ExternalCurrent"
                    End If
                    m_strLinkedTargetID = Util.LoadID(oXml, "LinkedNode", True, "")
                    m_strLinkedSourceProperty = Util.LoadID(oXml, "SourceDataType", True, "")
                Else
                    m_strLinkedSourceID = Util.LoadID(oXml, "LinkedSource", True, "")
                    m_strLinkedSourceProperty = Util.LoadID(oXml, "LinkedSourceProperty", True, "")
                    m_strLinkedTargetID = Util.LoadID(oXml, "LinkedTarget", True, "")
                    m_strLinkedTargetProperty = Util.LoadID(oXml, "LinkedTargetProperty", True, "")
                End If

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.AddChildElement("Link")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
                oXml.AddChildElement("ClassName", Me.ClassName)
                oXml.AddChildElement("InLink", Me.InLink)
                oXml.AddChildElement("PropertyID", Me.PropertyID)

                If Not m_thLinkedSource Is Nothing AndAlso Not m_thLinkedSource.Item Is Nothing Then
                    oXml.AddChildElement("LinkedSourceID", m_thLinkedSource.Item.ID)
                End If

                If m_doParentRemoteControl.UseRemoteDataTypes OrElse Not m_bInLink Then
                    If Not m_thLinkedSourceProperty Is Nothing AndAlso Not m_thLinkedSourceProperty.PropertyName Is Nothing Then
                        oXml.AddChildElement("LinkedSourcePropertyID", m_thLinkedSourceProperty.PropertyName)
                    End If
                Else
                    oXml.AddChildElement("LinkedSourcePropertyID", m_strLinkedSourceProperty)
                End If

                If Not m_thLinkedTarget Is Nothing AndAlso Not m_thLinkedTarget.Item Is Nothing Then
                    oXml.AddChildElement("LinkedTargetID", m_thLinkedTarget.Item.ID)
                End If

                If m_doParentRemoteControl.UseRemoteDataTypes OrElse m_bInLink Then
                    If Not m_thLinkedTargetProperty Is Nothing AndAlso Not m_thLinkedTargetProperty.PropertyName Is Nothing Then
                        oXml.AddChildElement("LinkedTargetPropertyID", m_thLinkedTargetProperty.PropertyName)
                    End If
                Else
                    oXml.AddChildElement("LinkedTargetPropertyID", m_strLinkedTargetProperty)
                End If

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

                oXml.AddChildElement("Link")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("Type", Me.LinkageType)
                oXml.AddChildElement("InLink", Me.InLink)
                oXml.AddChildElement("PropertyID", Me.PropertyID)

                If Not m_thLinkedSource Is Nothing AndAlso Not m_thLinkedSource.Item Is Nothing Then
                    oXml.AddChildElement("SourceID", m_thLinkedSource.Item.ID)
                End If

                If Not m_thLinkedSourceProperty Is Nothing AndAlso Not m_thLinkedSourceProperty.PropertyName Is Nothing Then
                    oXml.AddChildElement("SourceDataTypeID", m_thLinkedSourceProperty.PropertyName)

                    If m_bInLink Then
                        oXml.AddChildElement("PropertyName", m_thLinkedSourceProperty.PropertyName)
                    End If
                ElseIf m_bInLink Then
                    oXml.AddChildElement("SourceDataTypeID", m_strLinkedSourceProperty)
                    oXml.AddChildElement("PropertyName", m_strLinkedSourceProperty)
                End If

                If Not m_thLinkedTarget Is Nothing AndAlso Not m_thLinkedTarget.Item Is Nothing Then
                    oXml.AddChildElement("TargetID", m_thLinkedTarget.Item.ID)
                End If

                If Not m_thLinkedTargetProperty Is Nothing AndAlso Not m_thLinkedTargetProperty.PropertyName Is Nothing Then
                    oXml.AddChildElement("TargetDataTypeID", m_thLinkedTargetProperty.PropertyName)

                    If Not m_bInLink Then
                        oXml.AddChildElement("PropertyName", m_thLinkedTargetProperty.PropertyName)
                    End If
                ElseIf Not m_bInLink Then
                    oXml.AddChildElement("TargetDataTypeID", m_strLinkedTargetProperty)
                    oXml.AddChildElement("PropertyName", m_strLinkedTargetProperty)
                End If

                oXml.OutOfElem()

            End Sub

#End Region

#Region " Events "

            Private Sub OnAfterRemoveLinkedItem(ByRef doObject As Framework.DataObject)
                Try
                    If Not Me.ParentRemoteControl Is Nothing AndAlso Not Me.ParentRemoteControl.Organism Is Nothing Then
                        If m_bInLink Then
                            Me.LinkedTarget = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(Nothing)
                        Else
                            Me.LinkedSource = New AnimatGUI.TypeHelpers.LinkedDataObjectTree(Nothing)
                        End If
                    End If
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

            Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean
                Try
                    If bAskToDelete AndAlso Util.ShowMessage("Are you sure you want to remove the remote control linkage?", _
                                                             "Remove remote control linkage", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                        Return False
                    End If

                    Util.Application.AppIsBusy = True
                    Me.RemoveFromSim(True)
                    Me.RemoveWorksapceTreeView()
                    If Not m_doParentRemoteControl Is Nothing Then
                        m_doParentRemoteControl.Links.Remove(Me.ID)
                    End If
                    If Not Me.Parent Is Nothing Then Me.Parent.IsDirty = True
                    Return True
                Catch ex As Exception
                    Throw ex
                Finally
                    Util.Application.AppIsBusy = False
                End Try
            End Function

#End Region

        End Class

    End Namespace
End Namespace
