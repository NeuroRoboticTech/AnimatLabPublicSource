Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI.Framework

Namespace DataObjects.Physical

    Public Class Organism
        Inherits DataObjects.Physical.PhysicalStructure

#Region " Attributes "

        Protected m_frmBehaviorEditor As Forms.Behavior.Editor
        Protected m_tnBehavioralSystem As Crownwood.DotNetMagic.Controls.Node
        Protected m_aryBehavioralNodes As New Collections.AnimatSortedList(Me)
        Protected m_aryBehavioralLinks As New Collections.AnimatSortedList(Me)
        Protected m_aryNeuralModules As New Collections.SortedNeuralModules(Me)

#End Region

#Region " Properties "

        Public Overridable Property BehaviorEditor() As Forms.Behavior.Editor
            Get
                Return m_frmBehaviorEditor
            End Get
            Set(ByVal Value As Forms.Behavior.Editor)
                m_frmBehaviorEditor = Value
            End Set
        End Property

        Protected Overrides ReadOnly Property Structures(ByVal dsSim As AnimatGUI.DataObjects.Simulation) As Collections.SortedStructures
            Get
                Return dsSim.Environment.Organisms
            End Get
        End Property

        Protected Overrides ReadOnly Property ParentTreeNode(ByVal dsSim As AnimatGUI.DataObjects.Simulation) As Crownwood.DotNetMagic.Controls.Node
            Get
                Return dsSim.Environment.OrganismsTreeNode
            End Get
        End Property

        Public Overridable ReadOnly Property BehavioralSystemFile() As String
            Get
                Return Me.Name & ".absys"
            End Get
        End Property

        Public Overridable ReadOnly Property BehavioralEditorFile() As String
            Get
                Return Me.Name & ".abef"
            End Get
        End Property

        Public Overridable ReadOnly Property BehavioralSystemTreeNode() As Crownwood.DotNetMagic.Controls.Node
            Get
                Return m_tnBehavioralSystem
            End Get
        End Property

        Public Overridable ReadOnly Property BehavioralNodes() As Collections.AnimatSortedList
            Get
                Return m_aryBehavioralNodes
            End Get
        End Property

        Public Overridable ReadOnly Property BehavioralLinks() As Collections.AnimatSortedList
            Get
                Return m_aryBehavioralLinks
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
        Public Overrides ReadOnly Property RootForm() As System.Windows.Forms.Form
            Get
                If Not m_frmBehaviorEditor Is Nothing AndAlso Util.Application.ActiveMdiChild Is m_frmBehaviorEditor Then
                    Return m_frmBehaviorEditor
                ElseIf Not m_frmBodyEditor Is Nothing AndAlso Util.Application.ActiveMdiChild Is m_frmBodyEditor Then
                    Return m_frmBodyEditor
                Else
                    Return Util.Application
                End If
            End Get
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

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strName.GetType(), "Name", _
                                        "Organism Properties", "The name for this organism. ", m_strName))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Organism Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Body Plan", Me.BodyPlanFile.GetType(), "BodyPlanFile", _
                                        "Organism Properties", "Specifies the body plan file.", Me.BodyPlanFile, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Behavioral System", Me.BehavioralSystemFile.GetType(), "BehavioralSystemFile", _
                                        "Organism Properties", "Specifies the behavioral system file.", Me.BehavioralSystemFile, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Behavioral Editor", Me.BehavioralEditorFile.GetType(), "BehavioralEditorFile", _
                                        "Organism Properties", "Specifies the behavioral editor file.", Me.BehavioralEditorFile, True))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = Me.Position.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Position", pbNumberBag.GetType(), "Position", _
                                        "Structure Properties", "Sets the position of this structure.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

            pbNumberBag = Me.Rotation.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Rotation", pbNumberBag.GetType(), "Rotation", _
                                        "Structure Properties", "Sets the rotation of this structure.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Collision Exclusions", m_aryCollisionExclusionPairs.GetType(), "CollisionExclusionPairs", _
                                        "Organism Properties", "Pairs of body parts that should be excluded from collision detection between each other.", m_aryCollisionExclusionPairs, _
                                        GetType(TypeHelpers.CollisionPairsTypeEditor), GetType(TypeHelpers.CollisionPairsTypeConverter)))

        End Sub

#Region " Workspace TreeView "

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, ByVal doParentNode As Crownwood.DotNetMagic.Controls.Node)
            MyBase.CreateWorkspaceTreeView(doParent, doParentNode)

            m_tnBehavioralSystem = Util.ProjectWorkspace.AddTreeNode(m_tnWorkspaceNode, "Behavioral System", "AnimatGUI.Neuron.gif")

        End Sub

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Organism", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))
                Dim mcClone As New System.Windows.Forms.ToolStripMenuItem("Clone Organism", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Me.OnCloneStructure))

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Physical.Organism.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete, mcClone})

                If Me.RootBody Is Nothing Then
                    Dim mcAddRoot As New System.Windows.Forms.ToolStripMenuItem("Add root body", Util.Application.ToolStripImages.GetImage("AnimatGUI.Blank.gif"), New EventHandler(AddressOf Me.OnAddRootBody))
                    popup.Items.Add(mcAddRoot)
                End If

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                Return True
            ElseIf tnSelectedNode Is m_tnBodyPlanNode OrElse tnSelectedNode Is m_tnBehavioralSystem Then
                Return True
            End If

            Return False
        End Function

        Public Overrides Function WorkspaceTreeviewDoubleClick(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node) As Boolean

            If tnSelectedNode Is m_tnBodyPlanNode OrElse tnSelectedNode Is m_tnBehavioralSystem Then
                If tnSelectedNode Is m_tnBodyPlanNode Then
                    Util.Application.EditBodyPlan(Me)
                    Return True
                ElseIf tnSelectedNode Is m_tnBehavioralSystem Then
                    Util.Application.EditBehavioralSystem(Me)
                    Return True
                End If
            End If

            Return False
        End Function

#End Region

#Region " Data Item TreeView "

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As TreeNode, ByVal tpTemplatePartType As Type) As TreeNode

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load(Me.AssemblyModuleName)
            frmDataItem.ImageManager.AddImage(myAssembly, Me.WorkspaceImageName)

            Dim tnNode As TreeNode = frmDataItem.TreeView.Nodes.Add(Me.Name)
            tnNode.ImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.WorkspaceImageName)
            tnNode.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.WorkspaceImageName)
            tnNode.Tag = Me

            frmDataItem.ImageManager.AddImage(myAssembly, "AnimatGUI.Neuron.gif")
            frmDataItem.ImageManager.AddImage(myAssembly, "AnimatGUI.Joint.gif")
            frmDataItem.ImageManager.AddImage(myAssembly, "AnimatGUI.DefaultObject.gif")
            frmDataItem.ImageManager.AddImage(myAssembly, "AnimatGUI.DefaultLink.gif")

            Dim tnBodyplanNode As TreeNode
            tnBodyplanNode = tnNode.Nodes.Add("Body Plan")
            tnBodyplanNode.ImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.Joint.gif")
            tnBodyplanNode.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.Joint.gif")

            If Not m_dbRoot Is Nothing Then
                m_dbRoot.CreateDataItemTreeView(frmDataItem, tnBodyplanNode, tpTemplatePartType)
            End If

            Dim tnBehavioralNode As TreeNode
            tnBehavioralNode = tnNode.Nodes.Add("Behavioral System")
            tnBehavioralNode.ImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.Neuron.gif")
            tnBehavioralNode.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.Neuron.gif")

            Dim tnNodes As TreeNode
            tnNodes = tnBehavioralNode.Nodes.Add("Nodes")
            tnNodes.ImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.DefaultObject.gif")
            tnNodes.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.DefaultObject.gif")

            Dim tnLinks As TreeNode
            tnLinks = tnBehavioralNode.Nodes.Add("Links")
            tnLinks.ImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.DefaultLink.gif")
            tnLinks.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.DefaultLink.gif")

            Dim doData As DataObjects.Behavior.Data
            For Each deEntry As DictionaryEntry In m_aryBehavioralNodes
                doData = DirectCast(deEntry.Value, DataObjects.Behavior.Data)
                If doData.CanBeCharted Then
                    doData.CreateDataItemTreeView(frmDataItem, tnNodes, tpTemplatePartType)
                End If
            Next

            For Each deEntry As DictionaryEntry In m_aryBehavioralLinks
                doData = DirectCast(deEntry.Value, DataObjects.Behavior.Data)
                If doData.CanBeCharted Then
                    doData.CreateDataItemTreeView(frmDataItem, tnLinks, tpTemplatePartType)
                End If
            Next

        End Function

#End Region

#Region " Find Methods "

        Public Overridable Function FindBehavioralNode(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Node
            Dim oNode As Object = m_aryBehavioralNodes(strID)
            If oNode Is Nothing Then
                If bThrowError Then Throw New System.Exception("No node was found with the following id. ID: " & strID)
            Else
                Return DirectCast(oNode, AnimatGUI.DataObjects.Behavior.Node)
            End If
        End Function

        Public Overridable Function FindBehavioralNodeByName(ByVal strName As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Node

            Dim doNode As DataObjects.Behavior.Node
            For Each deEntry As DictionaryEntry In m_aryBehavioralNodes
                doNode = DirectCast(deEntry.Value, DataObjects.Behavior.Node)

                If doNode.Name = strName Then
                    Return doNode
                End If
            Next

            If doNode Is Nothing AndAlso bThrowError Then
                If bThrowError Then Throw New System.Exception("No node was found with the following name: " & strName)
            Else
                Return Nothing
            End If
        End Function

        Public Overridable Function FindBehavioralLink(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Link
            Dim oLink As Object = m_aryBehavioralLinks(strID)
            If oLink Is Nothing Then
                If bThrowError Then Throw New System.Exception("No link was found with the following id. ID: " & strID)
            Else
                Return DirectCast(oLink, AnimatGUI.DataObjects.Behavior.Link)
            End If
        End Function

        Public Overrides Sub FindChildrenOfType(ByVal tpTemplate As System.Type, ByRef colDataObjects As Collections.DataObjects)
            If Not m_dbRoot Is Nothing Then
                m_dbRoot.FindChildrenOfType(tpTemplate, colDataObjects)
            End If

            If tpTemplate Is Nothing OrElse Util.IsTypeOf(tpTemplate, GetType(AnimatGUI.DataObjects.Behavior.Data), False) Then
                Dim doData As AnimatGUI.DataObjects.Behavior.Data
                For Each deEntry As DictionaryEntry In Me.BehavioralNodes
                    doData = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Data)
                    doData.FindChildrenOfType(tpTemplate, colDataObjects)
                Next

                For Each deEntry As DictionaryEntry In Me.BehavioralLinks
                    doData = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Data)
                    doData.FindChildrenOfType(tpTemplate, colDataObjects)
                Next
            End If

        End Sub

        Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryBehavioralNodes Is Nothing Then doObject = m_aryBehavioralNodes.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryBehavioralLinks Is Nothing Then doObject = m_aryBehavioralLinks.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryNeuralModules Is Nothing Then doObject = m_aryNeuralModules.FindObjectByID(strID)
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

        Protected Overridable Sub LoadBehavioralSystem()
            Dim oXml As New AnimatGUI.Interfaces.StdXml

            m_aryBehavioralNodes.Clear()
            m_aryBehavioralLinks.Clear()

            If System.IO.File.Exists(Util.GetFilePath(Util.Application.ProjectPath, Me.BehavioralEditorFile)) Then
                oXml.Load(Util.GetFilePath(Util.Application.ProjectPath, Me.BehavioralEditorFile))

                oXml.FindElement("Editor")

                LoadNeuralModules(oXml)

                oXml.IntoChildElement("Diagrams")
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)
                    LoadDiagram(oXml)
                Next
                oXml.OutOfElem()  'Outof Diagrams Element

            End If

            '********************************************************************8
            '******** This must be changed later. Is not valid to do it this way now.
            AnimatGUI.Forms.Behavior.Diagram.InitializeDataAfterLoad(m_aryBehavioralNodes)
            AnimatGUI.Forms.Behavior.Diagram.InitializeDataAfterLoad(m_aryBehavioralLinks)

        End Sub

        Protected Overridable Sub LoadDiagram(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try

                Dim strAssemblyFile As String
                Dim strClassName As String

                oXml.IntoElem()

                oXml.IntoChildElement("Nodes")
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                Dim bnNode As AnimatGUI.DataObjects.Behavior.Node
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)
                    oXml.IntoElem() 'Into Node element
                    strAssemblyFile = oXml.GetChildString("AssemblyFile")
                    strClassName = oXml.GetChildString("ClassName")
                    'strNodeName = oXml.GetChildString("Text")
                    oXml.OutOfElem() 'Outof Node element

                    bnNode = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me), AnimatGUI.DataObjects.Behavior.Node)
                    bnNode.Organism = Me
                    bnNode.LoadData(oXml)
                    Me.BehavioralNodes.Add(bnNode.ID, bnNode)
                Next
                oXml.OutOfElem() 'Outof Nodes Element

                oXml.IntoChildElement("Links")
                iCount = oXml.NumberOfChildren() - 1
                Dim blLink As AnimatGUI.DataObjects.Behavior.Link
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)
                    oXml.IntoElem() 'Into Node element
                    strAssemblyFile = oXml.GetChildString("AssemblyFile")
                    strClassName = oXml.GetChildString("ClassName")
                    oXml.OutOfElem() 'Outof Node element

                    blLink = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me), AnimatGUI.DataObjects.Behavior.Link)
                    blLink.Organism = Me
                    blLink.LoadData(oXml)
                    Me.BehavioralLinks.Add(blLink.ID, blLink)
                Next
                oXml.OutOfElem() 'Outof Links Element

                oXml.IntoChildElement("Diagrams")
                iCount = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)
                    LoadDiagram(oXml)
                Next
                oXml.OutOfElem() ' OutOf the Diagrams Element

                oXml.OutOfElem()  'Outof Diagram Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Sub LoadNeuralModules(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try
                Dim strAssemblyFile As String
                Dim strClassName As String
                Dim oMod As Object

                m_aryNeuralModules.Clear()

                oXml.IntoChildElement("NeuralModules")
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                Dim nmModule As AnimatGUI.DataObjects.Behavior.NeuralModule
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)
                    oXml.IntoElem() 'Into Diagram element
                    strAssemblyFile = oXml.GetChildString("AssemblyFile")
                    strClassName = oXml.GetChildString("ClassName")
                    oXml.OutOfElem() 'Outof Diagram element

                    'If the module cannot be found then do not die because of this, just keep trying to go on.
                    oMod = Util.LoadClass(strAssemblyFile, strClassName, Me, False)
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
                        m_aryNeuralModules.Add(nmModule.GetType().FullName, nmModule)
                    End If
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Overloads Sub LoadData(ByRef oXml As Interfaces.StdXml)

            Try
                MyBase.LoadData(oXml)

                LoadBehavioralSystem()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Overloads Sub SaveData(ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()  'Into organism

            oXml.AddChildElement("NervousSystem")
            oXml.IntoElem()

            oXml.AddChildElement("NeuralModules")
            oXml.IntoElem()
            Dim nmModule As DataObjects.Behavior.NeuralModule
            Dim nmPhysicsModule As DataObjects.Behavior.NeuralModule
            For Each deEntry As DictionaryEntry In m_aryNeuralModules
                nmModule = DirectCast(deEntry.Value, DataObjects.Behavior.NeuralModule)

                If Not nmModule.GetType() Is GetType(DataObjects.Behavior.PhysicsModule) Then
                    'Only save out the modules that have some nodes in them. Others are unused.
                    If nmModule.HasNodesToSave Then
                        nmModule.SaveData(oXml)
                    End If
                Else
                    nmPhysicsModule = nmModule
                End If
            Next
            oXml.OutOfElem() 'Outof NeuralModules

            oXml.AddChildElement("Adapters")
            If Not nmPhysicsModule Is Nothing Then
                nmPhysicsModule.SaveData(oXml)
            End If

            oXml.OutOfElem() 'Outof Nervous System
            oXml.OutOfElem() 'Outof Organism 

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            Try
                oXml.IntoElem()  'Into organism

                oXml.AddChildElement("NervousSystem")
                oXml.IntoElem()

                oXml.AddChildElement("NeuralModules")
                oXml.IntoElem()
                Dim nmModule As DataObjects.Behavior.NeuralModule
                Dim nmPhysicsModule As DataObjects.Behavior.NeuralModule
                For Each deEntry As DictionaryEntry In m_aryNeuralModules
                    nmModule = DirectCast(deEntry.Value, DataObjects.Behavior.NeuralModule)

                    If Not nmModule.GetType() Is GetType(DataObjects.Behavior.PhysicsModule) Then
                        'Only save out the modules that have some nodes in them. Others are unused.
                        If nmModule.HasNodesToSave Then
                            nmModule.SaveSimulationXml(oXml)
                        End If
                    Else
                        nmPhysicsModule = nmModule
                    End If
                Next
                oXml.OutOfElem() 'Outof NeuralModules

                oXml.AddChildElement("Adapters")
                If Not nmPhysicsModule Is Nothing Then
                    nmPhysicsModule.SaveSimulationXml(oXml)
                End If

                oXml.OutOfElem() 'Outof Nervous System
                oXml.OutOfElem() 'Outof Organism 

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        'Public Overrides Sub CreateFiles()

        '    'Save the nervous system file
        '    Dim oXml As New Interfaces.StdXml

        '    If Util.Application.ProjectPath.Length > 0 Then
        '        If Not System.IO.File.Exists(Util.GetFilePath(Util.Application.ProjectPath, Me.BehavioralSystemFile)) Then
        '            oXml.AddElement("NervousSystem")
        '            oXml.AddChildElement("NeuralModules")
        '            oXml.AddChildElement("Adapters")

        '            oXml.Save(Util.GetFilePath(Util.Application.ProjectPath, Me.BehavioralSystemFile))
        '        End If

        '        If Not System.IO.File.Exists(Util.GetFilePath(Util.Application.ProjectPath, Me.BodyPlanFile)) Then
        '            'Save the body plan file
        '            oXml = New Interfaces.StdXml
        '            oXml.AddElement("Structure")
        '            oXml.Save(Util.GetFilePath(Util.Application.ProjectPath, Me.BodyPlanFile))
        '        End If
        '    End If

        'End Sub

#End Region

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrganism As Organism = DirectCast(doOriginal, Organism)

            m_frmBehaviorEditor = doOrganism.m_frmBehaviorEditor
            m_aryBehavioralNodes = DirectCast(doOrganism.m_aryBehavioralNodes.Clone(), AnimatGUI.Collections.AnimatSortedList)
            m_aryBehavioralLinks = DirectCast(doOrganism.m_aryBehavioralLinks.Clone(), AnimatGUI.Collections.AnimatSortedList)
            m_aryNeuralModules = DirectCast(doOrganism.m_aryNeuralModules.Clone(Me, bCutData, doRoot), AnimatGUI.Collections.SortedNeuralModules)

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

            Dim doData As Behavior.Data
            For Each deEntry As DictionaryEntry In m_aryBehavioralNodes
                doData = DirectCast(deEntry.Value, Behavior.Data)
                doData.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
            Next

            For Each deEntry As DictionaryEntry In m_aryBehavioralLinks
                doData = DirectCast(deEntry.Value, Behavior.Data)
                doData.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
            Next

        End Sub

        Public Overridable Sub AddContactAdapters(ByVal nmPhysicsModule As DataObjects.Behavior.NeuralModule, ByVal m_aryNodes As Collections.SortedNodes)
            If Not m_dbRoot Is Nothing Then
                m_dbRoot.AddContactAdapters(nmPhysicsModule, m_aryNodes)
            End If
        End Sub

        Protected Overrides Sub RenameWindowTitles()

            Try

                MyBase.RenameWindowTitles()

                If Not Me.BehaviorEditor Is Nothing Then
                    Me.BehaviorEditor.Title = "Edit " & Me.Name
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            'Need to loop through here and init all nodes and links.
        End Sub

        Public Overrides Sub InitializeSimulationReferences()
            MyBase.InitializeSimulationReferences()

            Dim nmModule As DataObjects.Behavior.NeuralModule
            For Each deEntry As DictionaryEntry In m_aryNeuralModules
                nmModule = DirectCast(deEntry.Value, DataObjects.Behavior.NeuralModule)
                nmModule.InitializeSimulationReferences()
            Next

            Dim doData As DataObjects.Behavior.Data
            For Each deEntry As DictionaryEntry In m_aryBehavioralNodes
                doData = DirectCast(deEntry.Value, DataObjects.Behavior.Data)
                doData.InitializeSimulationReferences()
            Next

            For Each deEntry As DictionaryEntry In m_aryBehavioralLinks
                doData = DirectCast(deEntry.Value, DataObjects.Behavior.Data)
                doData.InitializeSimulationReferences()
            Next

        End Sub


#Region " Add-Remove to List Methods "

        Public Overrides Sub BeforeAddToList(Optional ByVal bThrowError As Boolean = True)
            Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, "Organism", Me.GetSimulationXml("Organism"), bThrowError)
            InitializeSimulationReferences()
        End Sub

        Public Overrides Sub BeforeRemoveFromList(Optional ByVal bThrowError As Boolean = True)
            Util.Application.SimulationInterface.RemoveItem(Util.Simulation.ID, "Organism", Me.ID, bThrowError)
            m_doInterface = Nothing
        End Sub

#End Region

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

#End Region

    End Class

End Namespace

