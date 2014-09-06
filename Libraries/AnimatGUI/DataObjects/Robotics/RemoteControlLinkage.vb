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

            Protected m_thLinkedNode As TypeHelpers.LinkedNode

            'Only used during loading
            Protected m_strLinkedNodeID As String = ""

            Protected m_strSourceDataTypeID As String = ""
            Protected m_strTargetDataTypeID As String = ""
            Protected m_thSourceDataTypes As New TypeHelpers.DataTypeID(Me)
            'Protected m_thTargetDataTypes As New TypeHelpers.DataTypeID(Me)

#End Region

#Region " Properties "

            Public Overridable Property LinkedNode() As TypeHelpers.LinkedNode
                Get
                    Return m_thLinkedNode
                End Get
                Set(ByVal Value As TypeHelpers.LinkedNode)
                    Dim thPrevLinked As TypeHelpers.LinkedNode = m_thLinkedNode

                    DisconnectLinkedNodeEvents()
                    m_thLinkedNode = Value
                    ConnectLinkedNodeEvents()

                    If Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing Then
                        Me.SetSimData("LinkedNodeID", m_thLinkedNode.Node.ID, True)
                    Else
                        Me.SetSimData("LinkedNodeID", "", True)
                    End If
                    SetTargetDataTypes()
                End Set
            End Property

            <Browsable(False)> _
            Public Overridable Property SourceDataTypes() As TypeHelpers.DataTypeID
                Get
                    Return m_thSourceDataTypes
                End Get
                Set(ByVal Value As TypeHelpers.DataTypeID)
                    If Not Value Is Nothing Then
                        m_thSourceDataTypes = Value
                        Me.SetSimData("SourceDataTypeID", m_thSourceDataTypes.ID, True)
                    End If
                End Set
            End Property

            '<Browsable(False)> _
            'Public Overridable Property TargetDataTypes() As TypeHelpers.DataTypeID
            '    Get
            '        Return m_thTargetDataTypes
            '    End Get
            '    Set(ByVal Value As TypeHelpers.DataTypeID)
            '        If Not Value Is Nothing Then
            '            m_thTargetDataTypes = Value
            '            Me.SetSimData("TargetDataTypeID", m_thTargetDataTypes.ID, True)
            '        End If
            '    End Set
            'End Property

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

                If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType(), GetType(RemoteControl), False) Then
                    m_doParentRemoteControl = DirectCast(doParent, RemoteControl)
                    m_thSourceDataTypes = DirectCast(m_doParentRemoteControl.DataTypes.Clone(m_doParentRemoteControl.IncomingDataTypes.Parent, False, Nothing), TypeHelpers.DataTypeID)
                    m_thLinkedNode = New TypeHelpers.LinkedNode(m_doParentRemoteControl.Organism, Nothing)

                    AddHandler m_doParentRemoteControl.BeforeRemoveItem, AddressOf Me.OnBeforeParentRemoveFromList
                Else
                    m_thLinkedNode = New TypeHelpers.LinkedNode(Nothing, Nothing)
                End If

                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("AppliedCurrent", "Applied Current", "Amps", "A", 0, 1))
                m_thDataTypes.ID = "AppliedCurrent"
            End Sub

            Public Sub New(ByVal doParent As Framework.DataObject, ByVal strName As String, ByVal strSourceDataTypeID As String, ByVal doGain As Gain)
                MyBase.New(doParent)

                m_strName = strName

                If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType(), GetType(RemoteControl), False) Then
                    m_doParentRemoteControl = DirectCast(doParent, RemoteControl)
                    m_thSourceDataTypes = DirectCast(m_doParentRemoteControl.DataTypes.Clone(m_doParentRemoteControl.IncomingDataTypes.Parent, False, Nothing), TypeHelpers.DataTypeID)
                    m_thLinkedNode = New TypeHelpers.LinkedNode(m_doParentRemoteControl.Organism, Nothing)
                Else
                    m_thLinkedNode = New TypeHelpers.LinkedNode(Nothing, Nothing)
                End If

                m_strSourceDataTypeID = strSourceDataTypeID

                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("AppliedCurrent", "Applied Current", "Amps", "A", 0, 1))
                m_thDataTypes.ID = "AppliedCurrent"
            End Sub

            Public Overrides Sub ClearIsDirty()
                MyBase.ClearIsDirty()

                If Not m_thLinkedNode Is Nothing Then m_thLinkedNode.ClearIsDirty()
            End Sub

            Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                                ByVal doRoot As AnimatGUI.Framework.DataObject)
                MyBase.CloneInternal(doOriginal, bCutData, doRoot)

                Dim OrigNode As RemoteControlLinkage = DirectCast(doOriginal, RemoteControlLinkage)

                m_thLinkedNode = DirectCast(OrigNode.m_thLinkedNode.Clone(Me, bCutData, Nothing), TypeHelpers.LinkedNode)
                m_thSourceDataTypes = DirectCast(OrigNode.m_thSourceDataTypes.Clone(Me, bCutData, Nothing), TypeHelpers.DataTypeID)
                'm_thTargetDataTypes = DirectCast(OrigNode.m_thTargetDataTypes.Clone(Me, bCutData, Nothing), TypeHelpers.DataTypeID)
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

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Source Data Type ID", GetType(AnimatGUI.TypeHelpers.DataTypeID), "SourceDataTypes", _
                                            "Properties", "Sets the type of data to use as an input from the remote control.", m_thSourceDataTypes, _
                                            GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                            GetType(AnimatGUI.TypeHelpers.DataTypeIDTypeConverter)))

                'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Target Data Type ID", GetType(AnimatGUI.TypeHelpers.DataTypeID), "TargetDataTypes", _
                '                            "Properties", "Sets the type of data to add to on the target.", m_thTargetDataTypes, _
                '                            GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                '                            GetType(AnimatGUI.TypeHelpers.DataTypeIDTypeConverter)))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linked Node", GetType(AnimatGUI.TypeHelpers.LinkedNode), "LinkedNode", _
                                            "Properties", "Sets the node that this associated with this linkage.", m_thLinkedNode, _
                                            GetType(AnimatGUI.TypeHelpers.DropDownTreeEditor), _
                                            GetType(AnimatGUI.TypeHelpers.LinkedNodeTypeConverter)))

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

            Protected Overridable Sub SetTargetDataTypes()
                'If Not m_thLinkedNode.Node Is Nothing AndAlso Not m_thLinkedNode.Node.IncomingDataTypes Is Nothing Then
                '    Me.TargetDataTypes = DirectCast(m_thLinkedNode.Node.IncomingDataTypes.Clone(m_thLinkedNode.Node.IncomingDataTypes.Parent, False, Nothing), TypeHelpers.DataTypeID)
                'Else
                '    Me.TargetDataTypes = New AnimatGUI.TypeHelpers.DataTypeID(Me)
                'End If
            End Sub

            Protected Sub ConnectLinkedNodeEvents()
                DisconnectLinkedNodeEvents()

                If Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing Then
                    AddHandler m_thLinkedNode.Node.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedNode
                End If
            End Sub

            Protected Sub DisconnectLinkedNodeEvents()
                If Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing Then
                    RemoveHandler m_thLinkedNode.Node.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedNode
                End If
            End Sub

            Public Overrides Sub InitializeAfterLoad()

                If Not Me.ParentRemoteControl Is Nothing AndAlso Not Me.ParentRemoteControl.Organism Is Nothing Then
                    If m_strLinkedNodeID.Trim.Length > 0 Then
                        Dim bnNode As Behavior.Node = Me.ParentRemoteControl.Organism.FindBehavioralNode(m_strLinkedNodeID, False)
                        If Not bnNode Is Nothing Then
                            Me.LinkedNode = New TypeHelpers.LinkedNode(bnNode.Organism, bnNode)
                        Else
                            Util.Application.DeleteItemAfterLoading(Me)
                            Util.DisplayError(New System.Exception("The remote control linkage connector ID: " & Me.ID & " was unable to find its linked node ID: " & m_strLinkedNodeID & " in the diagram. This node and all links will be removed."))
                        End If
                    End If

                    ConnectLinkedNodeEvents()

                    If Not m_thSourceDataTypes Is Nothing AndAlso m_strSourceDataTypeID.Trim.Length > 0 AndAlso m_strSourceDataTypeID.Trim.Length > 0 Then
                        If Me.m_thSourceDataTypes.DataTypes.Contains(m_strSourceDataTypeID) Then
                            Me.m_thSourceDataTypes.ID = m_strSourceDataTypeID
                        End If
                    End If

                    'If Not Me.LinkedNode Is Nothing AndAlso Not Me.LinkedNode.Node Is Nothing Then
                    '    m_thTargetDataTypes = DirectCast(Me.LinkedNode.Node.IncomingDataTypes.Clone(Me, False, Nothing), TypeHelpers.DataTypeID)

                    '    If Not m_thTargetDataTypes Is Nothing AndAlso m_strTargetDataTypeID.Trim.Length > 0 AndAlso m_strTargetDataTypeID.Trim.Length > 0 Then
                    '        If Me.m_thTargetDataTypes.DataTypes.Contains(m_strTargetDataTypeID) Then
                    '            Me.m_thTargetDataTypes.ID = m_strTargetDataTypeID
                    '        End If
                    '    End If
                    'End If

                    m_bIsInitialized = True
                End If

            End Sub

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.IntoElem()  'Into RobotInterface Element

                m_strName = oXml.GetChildString("Name", Me.Name)
                m_strID = oXml.GetChildString("ID", Me.ID)
                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)

                m_strLinkedNodeID = Util.LoadID(oXml, "LinkedNode", True, "")

                m_strSourceDataTypeID = Util.LoadID(oXml, "SourceDataType", True, "")
                'm_strTargetDataTypeID = Util.LoadID(oXml, "TargetDataType", True, "")

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

                oXml.AddChildElement("Link")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
                oXml.AddChildElement("ClassName", Me.ClassName)

                If Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing Then
                    oXml.AddChildElement("LinkedNodeID", m_thLinkedNode.Node.ID)
                End If

                If Not m_thSourceDataTypes Is Nothing Then
                    oXml.AddChildElement("SourceDataTypeID", m_thSourceDataTypes.ID)
                End If

                'If Not m_thTargetDataTypes Is Nothing Then
                '    oXml.AddChildElement("TargetDataTypeID", m_thTargetDataTypes.ID)
                'End If

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

                oXml.AddChildElement("Link")
                oXml.IntoElem()

                oXml.AddChildElement("Name", Me.Name)
                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("Type", Me.LinkageType)

                If Not m_thLinkedNode Is Nothing AndAlso Not m_thLinkedNode.Node Is Nothing Then
                    oXml.AddChildElement("LinkedNodeID", m_thLinkedNode.Node.ID)
                End If

                If Not m_thSourceDataTypes Is Nothing Then
                    oXml.AddChildElement("SourceDataTypeID", m_thSourceDataTypes.ID)
                End If

                'If Not m_thTargetDataTypes Is Nothing Then
                '    oXml.AddChildElement("TargetDataTypeID", m_thTargetDataTypes.ID)
                'End If

                oXml.OutOfElem()

            End Sub

#End Region

#Region " Events "

            Private Sub OnAfterRemoveLinkedNode(ByRef doObject As Framework.DataObject)
                Try
                    If Not Me.ParentRemoteControl Is Nothing AndAlso Not Me.ParentRemoteControl.Organism Is Nothing Then
                        Me.LinkedNode = New TypeHelpers.LinkedNode(Me.ParentRemoteControl.Organism, Nothing)
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
