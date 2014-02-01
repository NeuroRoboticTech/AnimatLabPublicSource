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

Namespace DataObjects.Physical

    Public Class Organism
        Inherits DataObjects.Physical.PhysicalStructure

#Region " Attributes "

        Protected m_tnBehavioralSystem As Crownwood.DotNetMagic.Controls.Node

        ''' Keeps track of the maximum node count for creating new nodes.
        Protected m_iMaxNodeCount As Integer

        ''' This is a list of all neural modules used by this organism.
        Protected m_aryNeuralModules As New Collections.SortedNeuralModules(Me)

        ''' This is the root subsystem for the organism. All nodes and other subsystems are derived from it.
        Protected m_bnRootSubSystem As New DataObjects.Behavior.Nodes.Subsystem(Me)

        ''' The neural modules treeview node
        Protected m_tnNeuralModules As Crownwood.DotNetMagic.Controls.Node

        Protected m_doRobotInterface As Robotics.RobotInterface

#End Region

#Region " Properties "

        Protected Overrides ReadOnly Property ParentTreeNode(ByVal dsSim As AnimatGUI.DataObjects.Simulation) As Crownwood.DotNetMagic.Controls.Node
            Get
                Return dsSim.Environment.OrganismsTreeNode
            End Get
        End Property

        Public Overridable ReadOnly Property RootSubSystem() As DataObjects.Behavior.Nodes.Subsystem
            Get
                Return m_bnRootSubSystem
            End Get
        End Property

        Public Overridable ReadOnly Property BehavioralSystemTreeNode() As Crownwood.DotNetMagic.Controls.Node
            Get
                Return m_tnBehavioralSystem
            End Get
        End Property

        Public Overridable ReadOnly Property NeuralModules() As Collections.SortedNeuralModules
            Get
                Return m_aryNeuralModules
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Organism.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Organism"
            End Get
        End Property

        Public Overridable Property MaxNodeCount() As Integer
            Get
                Return m_iMaxNodeCount
            End Get
            Set(ByVal Value As Integer)
                m_iMaxNodeCount = Value
            End Set
        End Property

        Public Overridable ReadOnly Property TotalNodeCount() As Integer
            Get
                If Not m_bnRootSubSystem Is Nothing Then
                    Return m_bnRootSubSystem.TotalNodeCount
                End If
            End Get
        End Property

        Public Overridable ReadOnly Property TotalLinkCount() As Integer
            Get
                If Not m_bnRootSubSystem Is Nothing Then
                    Return m_bnRootSubSystem.TotalLinkCount
                End If
            End Get
        End Property

        Public Overridable Property RobotInterface() As Robotics.RobotInterface
            Get
                Return m_doRobotInterface
            End Get
            Set(ByVal Value As Robotics.RobotInterface)
                m_doRobotInterface = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Dim nmModule As DataObjects.Behavior.NeuralModule
            Dim nmNewModule As DataObjects.Behavior.NeuralModule
            For Each deItem As DictionaryEntry In Util.Application.NeuralModules
                nmModule = DirectCast(deItem.Value, DataObjects.Behavior.NeuralModule)
                nmNewModule = DirectCast(nmModule.Clone(Me, False, Nothing), AnimatGUI.DataObjects.Behavior.NeuralModule)
                nmNewModule.Organism = Me
                nmNewModule.Parent = Me
                m_aryNeuralModules.Add(nmModule.GetType().FullName, nmNewModule)
            Next

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Total Nodes", GetType(Integer), "TotalNodeCount", _
                            "Structure Properties", "Tells how many nodes are contained in this organism.", Me.TotalNodeCount, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Total Links", GetType(Integer), "TotalLinkCount", _
                            "Structure Properties", "Tells how many links are contained in this organism.", Me.TotalLinkCount, True))

        End Sub

#Region " Workspace TreeView "

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, _
                                                       ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                       Optional ByVal bRootObject As Boolean = False)
            MyBase.CreateWorkspaceTreeView(doParent, tnParentNode, bRootObject)

            If m_tnBehavioralSystem Is Nothing Then m_tnBehavioralSystem = Util.ProjectWorkspace.AddTreeNode(m_tnWorkspaceNode, "Behavioral System", "AnimatGUI.Neuron.gif")

            m_bnRootSubSystem.CreateWorkspaceTreeView(Me, m_tnBehavioralSystem)

            If m_tnNeuralModules Is Nothing Then m_tnNeuralModules = Util.ProjectWorkspace.AddTreeNode(m_tnWorkspaceNode, "Neural Modules", "AnimatGUI.NeuralModules_Treeview.gif")

            For Each deEntry As DictionaryEntry In Me.NeuralModules
                Dim nmModule As Behavior.NeuralModule = DirectCast(deEntry.Value, Behavior.NeuralModule)
                If Not Util.IsTypeOf(nmModule.GetType, GetType(Behavior.PhysicsModule), False) Then
                    nmModule.CreateWorkspaceTreeView(Me, m_tnNeuralModules)
                End If
            Next
            m_tnNeuralModules.CollapseAll()

            If Not m_doRobotInterface Is Nothing Then
                m_doRobotInterface.CreateWorkspaceTreeView(Me, m_tnWorkspaceNode)
            End If
        End Sub

        Public Overrides Function CreateObjectListTreeView(ByVal doParent As Framework.DataObject, _
                                                       ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                       ByVal mgrImageList As AnimatGUI.Framework.ImageManager) As Crownwood.DotNetMagic.Controls.Node
            Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateObjectListTreeView(doParent, tnParentNode, mgrImageList)

            Dim tnBehavioralSystem As Crownwood.DotNetMagic.Controls.Node = Util.AddTreeNode(tnNode, "Behavioral System", "AnimatGUI.Neuron.gif", "", mgrImageList)

            m_bnRootSubSystem.CreateObjectListTreeView(Me, tnBehavioralSystem, mgrImageList)

            Dim tnNeuralModules As Crownwood.DotNetMagic.Controls.Node = Util.AddTreeNode(tnNode, "Neural Modules", "AnimatGUI.NeuralModules_Treeview.gif", "", mgrImageList)

            For Each deEntry As DictionaryEntry In Me.NeuralModules
                Dim nmModule As Behavior.NeuralModule = DirectCast(deEntry.Value, Behavior.NeuralModule)
                If Not Util.IsTypeOf(nmModule.GetType, GetType(Behavior.PhysicsModule), False) Then
                    nmModule.CreateObjectListTreeView(Me, tnNeuralModules, mgrImageList)
                End If
            Next

            If Not m_doRobotInterface Is Nothing Then
                m_doRobotInterface.CreateObjectListTreeView(Me, tnNode, mgrImageList)
            End If

            Return tnNode
        End Function

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Organism", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))
                Dim mcClone As New System.Windows.Forms.ToolStripMenuItem("Clone Organism", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Me.OnCloneStructure))

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Physical.Organism.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete, mcClone})

                If Me.RootBody Is Nothing Then
                    Dim mcAddRoot As New System.Windows.Forms.ToolStripMenuItem("Add root body", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddPart.gif"), New EventHandler(AddressOf Me.OnAddRootBody))
                    popup.Items.Add(mcAddRoot)
                End If

                If Util.Application.RobotInterfaces.Count > 0 AndAlso m_doRobotInterface Is Nothing Then
                    Dim mcAddInterface As New System.Windows.Forms.ToolStripMenuItem("Add robot interface", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddRobotInterface.gif"), New EventHandler(AddressOf Me.OnAddRobotInterface))
                    popup.Items.Add(mcAddInterface)
                End If

                If Not m_doRobotInterface Is Nothing Then
                    Dim mcExportRobotStandalone As New System.Windows.Forms.ToolStripMenuItem("Export robot standalone simulation", Util.Application.ToolStripImages.GetImage("AnimatGUI.ExportStandalone.gif"), New EventHandler(AddressOf Me.OnExportRobotStandalone))
                    Dim mcRunRobotSim As New System.Windows.Forms.ToolStripMenuItem("Run robot simulation", Nothing, New EventHandler(AddressOf Me.OnRunRobotSimulation))
                    popup.Items.Add(mcExportRobotStandalone)
                    popup.Items.Add(mcRunRobotSim)
                End If

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                Return True
            ElseIf tnSelectedNode Is m_tnBodyPlanNode OrElse tnSelectedNode Is m_tnBehavioralSystem Then
                Return True
            End If

            Return False
        End Function

        Public Overrides Sub WorkspaceTreeviewDoubleClick(ByVal tnSelectedNode As Crownwood.DotNetMagic.Controls.Node)

            If tnSelectedNode Is m_tnBodyPlanNode OrElse tnSelectedNode Is m_tnBehavioralSystem Then
                If tnSelectedNode Is m_tnBodyPlanNode Then
                    Util.Application.EditBodyPlan(Me)
                ElseIf tnSelectedNode Is m_tnBehavioralSystem Then
                    Util.Application.EditBehavioralSystem(Me)
                End If
            End If
        End Sub

#End Region

#Region " Data Item TreeView "

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, ByVal tpTemplatePartType As Type) As Crownwood.DotNetMagic.Controls.Node

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load(Me.AssemblyModuleName)
            frmDataItem.ImageManager.AddImage(myAssembly, Me.WorkspaceImageName)

            Dim tnNode As New Crownwood.DotNetMagic.Controls.Node(Me.Name)
            If tnParent Is Nothing Then
                frmDataItem.TreeView.Nodes.Add(tnNode)
            Else
                tnParent.Nodes.Add(tnNode)
            End If

            tnNode.ImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.WorkspaceImageName)
            tnNode.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.WorkspaceImageName)
            tnNode.Tag = Me

            frmDataItem.ImageManager.AddImage(myAssembly, "AnimatGUI.Neuron.gif")
            frmDataItem.ImageManager.AddImage(myAssembly, "AnimatGUI.Joint.gif")
            frmDataItem.ImageManager.AddImage(myAssembly, "AnimatGUI.DefaultObject.gif")
            frmDataItem.ImageManager.AddImage(myAssembly, "AnimatGUI.DefaultLink.gif")

            Dim tnBodyplanNode As New Crownwood.DotNetMagic.Controls.Node("Body Plan")
            tnBodyplanNode = tnNode.Nodes.Add(tnBodyplanNode)
            tnBodyplanNode.ImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.Joint.gif")
            tnBodyplanNode.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.Joint.gif")

            If Not m_dbRoot Is Nothing Then
                m_dbRoot.CreateDataItemTreeView(frmDataItem, tnBodyplanNode, tpTemplatePartType)
            End If

            Dim tnBehavioralNode As New Crownwood.DotNetMagic.Controls.Node("Behavioral System")
            tnBehavioralNode = tnNode.Nodes.Add(tnBehavioralNode)
            tnBehavioralNode.ImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.Neuron.gif")
            tnBehavioralNode.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.Neuron.gif")

            If Not m_bnRootSubSystem Is Nothing Then
                m_bnRootSubSystem.CreateDataItemTreeView(frmDataItem, tnBehavioralNode, tpTemplatePartType)
            End If

        End Function

        Public Overridable Sub CreateNodeTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl)
            tvTree.ClearSelection()
            tvTree.LabelEdit = False
            tvTree.ImageList = New ImageList
            tvTree.ImageList.ImageSize = New Size(16, 16)
            m_bnRootSubSystem.CreateNodeTreeView(tvTree, tvTree.Nodes)
        End Sub

#End Region

#Region " Find Methods "

        Public Overridable Function FindBehavioralNode(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Node
            Dim oNode As Object = m_bnRootSubSystem.FindObjectByID(strID)
            If oNode Is Nothing Then
                If bThrowError Then Throw New System.Exception("No node was found with the following id. ID: " & strID)
            Else
                Return DirectCast(oNode, AnimatGUI.DataObjects.Behavior.Node)
            End If
        End Function

        Public Overridable Function FindBehavioralNodeByName(ByVal strName As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Node
            Dim aryNodes As Collections.DataObjects = New Collections.DataObjects(Nothing)
            m_bnRootSubSystem.FindChildrenOfType(GetType(Behavior.Node), aryNodes)

            For Each doNode As Framework.DataObject In aryNodes
                If doNode.Name = strName Then
                    Return DirectCast(doNode, Behavior.Node)
                End If
            Next

            If bThrowError Then
                If bThrowError Then Throw New System.Exception("No node was found with the following name: " & strName)
            Else
                Return Nothing
            End If
        End Function

        Public Overridable Function FindBehavioralLink(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Link
            Dim oLink As Object = m_bnRootSubSystem.FindObjectByID(strID)
            If oLink Is Nothing Then
                If bThrowError Then Throw New System.Exception("No link was found with the following id. ID: " & strID)
            Else
                Return DirectCast(oLink, AnimatGUI.DataObjects.Behavior.Link)
            End If
        End Function

        Public Overrides Sub FindChildrenOfType(ByVal tpTemplate As System.Type, ByVal colDataObjects As Collections.DataObjects)
            If Not m_dbRoot Is Nothing Then
                m_dbRoot.FindChildrenOfType(tpTemplate, colDataObjects)
            End If

            If Not m_bnRootSubSystem Is Nothing AndAlso (tpTemplate Is Nothing OrElse Util.IsTypeOf(tpTemplate, GetType(AnimatGUI.DataObjects.Behavior.Data), False)) Then
                m_bnRootSubSystem.FindChildrenOfType(tpTemplate, colDataObjects)
            End If

            If Not m_doRobotInterface Is Nothing Then
                m_doRobotInterface.FindChildrenOfType(tpTemplate, colDataObjects)
            End If

        End Sub

        Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_bnRootSubSystem Is Nothing Then doObject = m_bnRootSubSystem.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryNeuralModules Is Nothing Then doObject = m_aryNeuralModules.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doRobotInterface Is Nothing Then doObject = m_doRobotInterface.FindObjectByID(strID)
            Return doObject

        End Function

        Public Overridable Function FindNeuralModuleByType(ByVal tpTemplate As System.Type, Optional ByVal bThrowError As Boolean = True) As DataObjects.Behavior.NeuralModule

            Dim nmModule As DataObjects.Behavior.NeuralModule
            For Each deEntry As DictionaryEntry In m_aryNeuralModules
                nmModule = DirectCast(deEntry.Value, DataObjects.Behavior.NeuralModule)

                If Util.IsTypeOf(nmModule.GetType(), tpTemplate, False) Then
                    Return nmModule
                End If
            Next

            If bThrowError Then
                Throw New System.Exception("Neural Module of type '" & tpTemplate.ToString() & "' was not found.")
            End If
        End Function

#End Region

#Region " Load/Save Methods "

        Public Overridable Sub LoadNeuralModules(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                Dim oMod As Object
                Dim nmModule As AnimatGUI.DataObjects.Behavior.NeuralModule

                Util.Application.AppStatusText = "Loading " & Me.TypeName & " " & Me.Name & " neural modules"

                m_aryNeuralModules.Clear()

                oXml.IntoChildElement("NeuralModules")
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    'If the module cannot be found then do not die because of this, just keep trying to go on.
                    oMod = Util.LoadClass(oXml, iIndex, Me, False)
                    If Not oMod Is Nothing Then
                        nmModule = DirectCast(oMod, AnimatGUI.DataObjects.Behavior.NeuralModule)
                        nmModule.Organism = Me
                        nmModule.LoadData(oXml)
                        m_aryNeuralModules.Add(nmModule.GetType().FullName, nmModule)
                    End If
                Next
                oXml.OutOfElem() 'Outof NeuralModules Element

                'Now lets go through and see if there are any neural modules found in module init that
                'we have not saved for this organism. If so then we need to add them so the user has
                'access to them.
                Dim nmNewModule As DataObjects.Behavior.NeuralModule
                For Each deItem As DictionaryEntry In Util.Application.NeuralModules
                    nmModule = DirectCast(deItem.Value, DataObjects.Behavior.NeuralModule)
                    If Not m_aryNeuralModules.Contains(nmModule.GetType.FullName) Then
                        nmNewModule = DirectCast(nmModule.Clone(nmModule.Parent, False, Nothing), AnimatGUI.DataObjects.Behavior.NeuralModule)
                        nmNewModule.Organism = Me
                        nmNewModule.Parent = Me
                        m_aryNeuralModules.Add(nmNewModule.GetType().FullName, nmNewModule)
                    End If
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Overloads Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                'Load the structure data
                MyBase.LoadData(oXml)

                Util.Application.AppStatusText = "Loading " & Me.TypeName & " " & Me.Name & " nervous system"

                oXml.IntoElem() 'Into Organism Element
                oXml.IntoChildElement("NervousSystem") 'Into NervousSystem Element

                LoadNeuralModules(oXml)

                Util.Application.AppStatusText = "Loading " & Me.TypeName & " " & Me.Name & " Nodes"
                If oXml.FindChildElement("Node", False) Then
                    Dim strAssemblyFile As String
                    Dim strClassName As String

                    oXml.IntoElem() 'Into Node element
                    strAssemblyFile = oXml.GetChildString("AssemblyFile")
                    strClassName = oXml.GetChildString("ClassName")
                    oXml.OutOfElem() 'Outof Node element

                    m_bnRootSubSystem = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me), DataObjects.Behavior.Nodes.Subsystem)
                    m_bnRootSubSystem.LoadData(oXml)
                End If

                oXml.OutOfElem() 'Outof NervousSystem Element

                If oXml.FindChildElement("RobotInterface", False) Then
                    oXml.IntoChildElement("RobotInterface")
                    Dim strAssemblyFile As String = oXml.GetChildString("AssemblyFile")
                    Dim strClassName As String = oXml.GetChildString("ClassName")
                    oXml.OutOfElem()

                    m_doRobotInterface = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me), AnimatGUI.DataObjects.Robotics.RobotInterface)
                    m_doRobotInterface.LoadData(oXml)
                End If

                oXml.OutOfElem() 'Outof Organism Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Overloads Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()  'Into organism

            Util.Application.AppStatusText = "Saving " & Me.TypeName & " " & Me.Name & " nervous system"

            oXml.AddChildElement("NervousSystem")
            oXml.IntoElem()

            Util.Application.AppStatusText = "Saving " & Me.TypeName & " " & Me.Name & " neural modules"
            oXml.AddChildElement("NeuralModules")
            oXml.IntoElem()
            Dim nmModule As DataObjects.Behavior.NeuralModule
            For Each deEntry As DictionaryEntry In m_aryNeuralModules
                nmModule = DirectCast(deEntry.Value, DataObjects.Behavior.NeuralModule)

                'Only save out the modules that have some nodes in them. Others are unused.
                If nmModule.HasNodesToSave Then
                    nmModule.SaveData(oXml)
                End If
            Next
            oXml.OutOfElem() 'Outof NeuralModules

            Util.Application.AppStatusText = "Saving " & Me.TypeName & " " & Me.Name & " Nodes"
            If Not m_bnRootSubSystem Is Nothing Then
                m_bnRootSubSystem.SaveData(oXml)
            End If

            oXml.OutOfElem() 'Outof Nervous System

            If Not m_doRobotInterface Is Nothing Then
                m_doRobotInterface.SaveData(oXml)
            End If

            oXml.OutOfElem() 'Outof Organism 

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            Try
                oXml.IntoElem()  'Into organism

                oXml.AddChildElement("NervousSystem")
                oXml.IntoElem()

                oXml.AddChildElement("NeuralModules")
                oXml.IntoElem()
                Dim nmModule As DataObjects.Behavior.NeuralModule
                For Each deEntry As DictionaryEntry In m_aryNeuralModules
                    nmModule = DirectCast(deEntry.Value, DataObjects.Behavior.NeuralModule)

                    'Only save out the modules that have some nodes in them. Others are unused.
                    If nmModule.HasNodesToSave Then
                        nmModule.SaveSimulationXml(oXml)
                    End If
                Next
                oXml.OutOfElem() 'Outof NeuralModules

                oXml.OutOfElem() 'Outof Nervous System

                'Only save the robot interface in the simulation xml if we are doing an export for a robot sim.
                If Not m_doRobotInterface Is Nothing AndAlso Not Util.ExportRobotInterface Is Nothing Then
                    m_doRobotInterface.SaveSimulationXml(oXml, Me)
                End If

                oXml.OutOfElem() 'Outof Organism 

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub
#End Region

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrganism As Organism = DirectCast(doOriginal, Organism)

            m_bnRootSubSystem = DirectCast(doOrganism.m_bnRootSubSystem.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Behavior.Nodes.Subsystem)
            m_aryNeuralModules = DirectCast(doOrganism.m_aryNeuralModules.Clone(Me, bCutData, doRoot), AnimatGUI.Collections.SortedNeuralModules)
            m_doRobotInterface = DirectCast(doOrganism.m_doRobotInterface.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Robotics.RobotInterface)
        End Sub

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList, ByVal arySelectedItems As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)

            m_bnRootSubSystem.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            m_aryNeuralModules.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            If Not m_doRobotInterface Is Nothing Then m_doRobotInterface.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
        End Sub

        Public Overrides Sub AddToRecursiveSelectedItemsList(ByVal arySelectedItems As ArrayList)
            MyBase.AddToRecursiveSelectedItemsList(arySelectedItems)

            m_aryNeuralModules.AddToRecursiveSelectedItemsList(arySelectedItems)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New Organism(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doItem.AfterClone(Me, bCutData, doRoot, doItem)
            Return doItem
        End Function

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)
            MyBase.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)

            If Not m_bnRootSubSystem Is Nothing Then
                m_bnRootSubSystem.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
                If Not m_doRobotInterface Is Nothing Then m_doRobotInterface.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
            End If

        End Sub

        Public Overrides Sub DeleteInternal()
            If Not Util.Environment.Organisms Is Nothing Then
                Util.Environment.Organisms.Remove(Me.ID)
            End If
            Me.RemoveWorksapceTreeView()
            m_tnWorkspaceNode = Nothing
            m_tnBodyPlanNode = Nothing
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_aryNeuralModules.ClearIsDirty()
            m_bnRootSubSystem.ClearIsDirty()
            If Not m_doRobotInterface Is Nothing Then m_doRobotInterface.ClearIsDirty()
        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            If Not m_bnRootSubSystem Is Nothing Then
                Dim iRetryCount As Integer = 0
                While Not m_bnRootSubSystem.IsInitialized AndAlso iRetryCount < 5
                    m_bnRootSubSystem.InitializeAfterLoad()
                    iRetryCount = iRetryCount + 1
                    If iRetryCount > 5 Then
                        Util.DisplayError(New System.Exception("Retry count for InitializeAfterLoad method is > 5. Giving up on this."))
                    End If
                End While
            End If

            For Each deEntry As DictionaryEntry In Me.NeuralModules
                Dim nmModule As Behavior.NeuralModule = DirectCast(deEntry.Value, Behavior.NeuralModule)
                nmModule.InitializeAfterLoad()
            Next

            If Not m_bnRootSubSystem Is Nothing AndAlso m_bnRootSubSystem.IsInitialized Then
                m_bnRootSubSystem.AfterInitialized()
            End If

            If Not m_doRobotInterface Is Nothing Then m_doRobotInterface.InitializeAfterLoad()

        End Sub

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            MyBase.InitializeSimulationReferences(bShowError)

            Dim nmModule As DataObjects.Behavior.NeuralModule
            For Each deEntry As DictionaryEntry In m_aryNeuralModules
                nmModule = DirectCast(deEntry.Value, DataObjects.Behavior.NeuralModule)

                'Only attempt to initialize the neural module if it exists in the simulation.
                'The problem is that a neural module is not created in the simulation unless it 
                ' has neurons. 
                If nmModule.SimObjectExists Then
                    nmModule.InitializeSimulationReferences(bShowError)
                End If
            Next

            If Not m_bnRootSubSystem Is Nothing Then
                m_bnRootSubSystem.InitializeSimulationReferences(bShowError)
            End If
        End Sub

#End Region

#Region " Events "

        Protected Overrides Sub OnCloneStructure(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim newOrganism As New DataObjects.Physical.Organism(Me)

                newOrganism = DirectCast(Me.Clone(Me.Parent, False, Nothing), DataObjects.Physical.Organism)
                Util.Simulation.Environment.Organisms.Add(newOrganism.ID, newOrganism)

                Util.Environment.NewOrganismCount = Util.Environment.NewOrganismCount + 1
                newOrganism.Name = "Organism_" & Util.Environment.NewOrganismCount
                'newOrganism.LoadBodyPlan(Util.Simulation)

                newOrganism.CreateWorkspaceTreeView(Util.Environment, Util.Environment.OrganismsTreeNode)
                newOrganism.WorkspaceNode.ExpandAll()
                Util.ProjectWorkspace.TreeView.SelectedNode = newOrganism.WorkspaceNode
                'newOrganism.CreateFiles()

                Util.Application.SaveProject(Util.Application.ProjectFile)

                'If this is the first organism then lets set the camer to track it.
                'If Util.Simulation.Environment.Organisms.Count = 1 Then
                '    Util.Environment.Camera.AutoTrack(newOrganism)
                'End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub OnAddRobotInterface(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Dim frmSelInterface As New Forms.SelectObject()
                frmSelInterface.Objects = Util.Application.RobotInterfaces
                frmSelInterface.PartTypeName = "Robot Intefaces"

                If Not m_doRobotInterface Is Nothing Then
                    If Util.ShowMessage("There is already a robot interface associated with this organism. Do you want to replace it?", "Replace Robot Interface", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                        Return
                    End If
                End If

                If frmSelInterface.ShowDialog() = DialogResult.OK Then
                    'First remove the old one if it exists
                    If Not m_doRobotInterface Is Nothing Then
                        m_doRobotInterface.RemoveWorksapceTreeView()
                        m_doRobotInterface = Nothing
                    End If

                    'Then create the new one.
                    m_doRobotInterface = DirectCast(frmSelInterface.Selected.Clone(Me, False, Nothing), Robotics.RobotInterface)
                    m_doRobotInterface.CreateWorkspaceTreeView(Me, m_tnWorkspaceNode)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnExportRobotStandalone(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                If Not m_doRobotInterface Is Nothing Then
                    m_doRobotInterface.GenerateStandaloneSimFile()
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnRunRobotSimulation(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Throw New System.Exception("This operation is not yet supported.")
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

