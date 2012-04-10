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

    Public Class PhysicalStructure
        Inherits MovableItem

#Region " Delegates "

#End Region

#Region " Attributes "

        Protected m_snSize As AnimatGUI.Framework.ScaledNumber

        Protected m_dbRoot As DataObjects.Physical.RigidBody

        Protected m_frmBodyEditor As Forms.SimulationWindow

        Protected m_tnBodyPlanNode As Crownwood.DotNetMagic.Controls.Node

        Protected m_iNewBodyIndex As Integer = 0
        Protected m_iNewJointIndex As Integer = 0

        Protected m_aryCollisionExclusionPairs As New Collections.CollisionPairs(Me)

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides ReadOnly Property RootForm() As System.Windows.Forms.Form
            Get
                If Not m_frmBodyEditor Is Nothing Then
                    Return m_frmBodyEditor
                Else
                    Return Util.Application
                End If
            End Get
        End Property

        <Browsable(False)> _
        Protected Overridable ReadOnly Property ParentTreeNode(ByVal dsSim As AnimatGUI.DataObjects.Simulation) As Crownwood.DotNetMagic.Controls.Node
            Get
                Return dsSim.Environment.StructuresTreeNode
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property NewBodyIndex() As Integer
            Get
                Return m_iNewBodyIndex
            End Get
            Set(ByVal Value As Integer)
                m_iNewBodyIndex = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property NewJointIndex() As Integer
            Get
                Return m_iNewJointIndex
            End Get
            Set(ByVal Value As Integer)
                m_iNewJointIndex = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Structure.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Structure"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DefaultVisualSelectionMode() As Simulation.enumVisualSelectionMode
            Get
                Return Simulation.enumVisualSelectionMode.SelectCollisions
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property RootBody() As DataObjects.Physical.RigidBody
            Get
                Return m_dbRoot
            End Get
            Set(ByVal Value As DataObjects.Physical.RigidBody)
                m_dbRoot = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property WorkspaceBodyPlanNode() As Crownwood.DotNetMagic.Controls.Node
            Get
                Return m_tnBodyPlanNode
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StructureID() As String
            Get
                Return Me.ID
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property BodyEditor() As Forms.SimulationWindow
            Get
                Return m_frmBodyEditor
            End Get
            Set(ByVal Value As Forms.SimulationWindow)
                m_frmBodyEditor = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property CollisionExclusionPairs() As AnimatGUI.Collections.CollisionPairs
            Get
                Return m_aryCollisionExclusionPairs
            End Get
            Set(ByVal Value As AnimatGUI.Collections.CollisionPairs)
                If Value Is Nothing Then
                    Throw New System.Exception("The collision exclusion pair list can not be null.")
                End If

                'If Me.Name = "Crayfish" AndAlso Not Value.Count = 20 Then
                '    Util.ShowMessage("Collision pairs messed up!!!")
                'End If

                m_aryCollisionExclusionPairs = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("Local Position") Then propTable.Properties.Remove("Local Position")
            If propTable.Properties.Contains("World Position") Then propTable.Properties.Remove("World Position")
            If propTable.Properties.Contains("Rotation") Then propTable.Properties.Remove("Rotation")
            If propTable.Properties.Contains("Visible") Then propTable.Properties.Remove("Visible")
            If propTable.Properties.Contains("Transparencies") Then propTable.Properties.Remove("Transparencies")
            If propTable.Properties.Contains("Ambient") Then propTable.Properties.Remove("Ambient")
            If propTable.Properties.Contains("Diffuse") Then propTable.Properties.Remove("Diffuse")
            If propTable.Properties.Contains("Specular") Then propTable.Properties.Remove("Specular")
            If propTable.Properties.Contains("Shininess") Then propTable.Properties.Remove("Shininess")

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = Me.LocalPosition.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Position", pbNumberBag.GetType(), "LocalPosition", _
                                        "Coordinates", "Sets the position of this structure.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Collision Exclusions", m_aryCollisionExclusionPairs.GetType(), "CollisionExclusionPairs", _
                                        "Structure Properties", "Pairs of body parts that should be excluded from collision detection between each other.", m_aryCollisionExclusionPairs, _
                                        GetType(TypeHelpers.CollisionPairsTypeEditor), GetType(TypeHelpers.CollisionPairsTypeConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_dbRoot Is Nothing Then m_dbRoot.ClearIsDirty()
            If Not m_aryCollisionExclusionPairs Is Nothing Then m_aryCollisionExclusionPairs.ClearIsDirty()
        End Sub

        Public Overrides Sub AfterClone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal doClone As AnimatGUI.Framework.DataObject)
            MyBase.AfterClone(doParent, bCutData, doOriginal, doClone)

            If Not m_dbRoot Is Nothing Then
                m_dbRoot.AfterClone(Me, bCutData, doOriginal, doClone)
            End If
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As AnimatGUI.DataObjects.Physical.PhysicalStructure = DirectCast(doOriginal, PhysicalStructure)

            If Not doOrig.WorkspaceImage Is Nothing Then m_WorkspaceImage = DirectCast(doOrig.WorkspaceImage.Clone(), Image)
            If Not doOrig.m_DragImage Is Nothing Then m_DragImage = DirectCast(doOrig.m_DragImage.Clone(), Image)
            If Not doOrig.RootBody Is Nothing Then m_dbRoot = DirectCast(doOrig.RootBody.Clone(Me, bCutData, doRoot), RigidBody)

            'm_frmBodyEditor = doOrig.m_frmBodyEditor
            m_iNewBodyIndex = doOrig.m_iNewBodyIndex
            m_iNewJointIndex = doOrig.m_iNewJointIndex

            m_aryCollisionExclusionPairs.Clear()

            Dim doOrigPair As CollisionPair
            Dim doOrigBody As AnimatGUI.DataObjects.Physical.BodyPart
            Dim doFoundBody As AnimatGUI.DataObjects.Physical.BodyPart
            For Each deItem As DictionaryEntry In doOrig.m_aryCollisionExclusionPairs
                doOrigPair = DirectCast(deItem.Value, CollisionPair)
                doOrigBody = Me.FindBodyPartByName(doOrigPair.Part1.Name)
                doFoundBody = Me.FindBodyPartByName(doOrigPair.Part2.Name)
                m_aryCollisionExclusionPairs.Add(New CollisionPair(Me, DirectCast(doOrigBody, RigidBody), DirectCast(doFoundBody, RigidBody)))
            Next

        End Sub

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList)

            Me.RootBody.AddToReplaceIDList(aryReplaceIDList)
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New PhysicalStructure(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doItem.AfterClone(Me, bCutData, doRoot, doItem)
            Return doItem
        End Function

        Public Overridable Function FindBodyPart(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As BodyPart
            Dim bpPart As BodyPart

            If Not m_dbRoot Is Nothing Then bpPart = m_dbRoot.FindBodyPart(strID)

            If bpPart Is Nothing AndAlso bThrowError Then
                Throw New System.Exception("The body part with ID '" & strID & "' could not be found in the organism '" & m_strName & "'.")
            End If

            Return bpPart
        End Function

        Public Overridable Function FindBodyPartByName(ByVal strName As String, Optional ByVal bThrowError As Boolean = True) As BodyPart
            Dim bpPart As BodyPart

            If Not m_dbRoot Is Nothing Then bpPart = m_dbRoot.FindBodyPartByName(strName)

            If bpPart Is Nothing AndAlso bThrowError Then
                Throw New System.Exception("The body part with name '" & strName & "' could not be found in the organism '" & m_strName & "'.")
            End If

            Return bpPart
        End Function

        Public Overridable Function FindBodyPartByCloneID(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As BodyPart
            Dim bpPart As BodyPart

            If Not m_dbRoot Is Nothing Then bpPart = m_dbRoot.FindBodyPartByCloneID(strID)

            If bpPart Is Nothing AndAlso bThrowError Then
                Throw New System.Exception("The body part with name '" & strID & "' could not be found in the organism '" & m_strName & "'.")
            End If

            Return bpPart
        End Function

        Public Overrides Sub SetupInitialTransparencies()
            If Not m_Transparencies Is Nothing Then
                m_Transparencies.GraphicsTransparency = 50
                m_Transparencies.CollisionsTransparency = 50
                m_Transparencies.JointsTransparency = 50
                m_Transparencies.ReceptiveFieldsTransparency = 50
                m_Transparencies.SimulationTransparency = 100
            End If
        End Sub

        Public Overridable Function AddRootBody(Optional ByVal rbRootToAdd As AnimatGUI.DataObjects.Physical.RigidBody = Nothing, Optional ByVal bAddDefaultGraphics As Boolean = True) As Boolean

            If Not m_dbRoot Is Nothing Then
                Throw New System.Exception("A root body already exists for this body!")
            End If

            If rbRootToAdd Is Nothing Then
                Dim frmSelectParts As New Forms.BodyPlan.SelectPartType()
                frmSelectParts.PartType = GetType(Physical.RigidBody)
                frmSelectParts.IsRoot = True

                If frmSelectParts.ShowDialog() <> DialogResult.OK Then Return False

                rbRootToAdd = DirectCast(frmSelectParts.SelectedPart.Clone(Me, False, Nothing), RigidBody)
                bAddDefaultGraphics = frmSelectParts.chkAddGraphics.Checked
                rbRootToAdd.SetDefaultSizes()
            End If

            rbRootToAdd.Name = "Root"
            rbRootToAdd.IsRoot = True
            rbRootToAdd.IsCollisionObject = True
            rbRootToAdd.IsContactSensor = False
            rbRootToAdd.Freeze = False
            rbRootToAdd.BeforeAddBody()
            rbRootToAdd.BeforeAddToList(False, True)

            m_dbRoot = rbRootToAdd

            If bAddDefaultGraphics Then
                m_dbRoot.CreateDefaultGraphicsObject()
            End If

            If Not m_dbRoot Is Nothing Then
                m_dbRoot.CreateWorkspaceTreeView(Me, m_tnBodyPlanNode)
            End If

            m_dbRoot.AfterAddToList(False, True)
            rbRootToAdd.AfterAddBody()

            rbRootToAdd.AddToSim(True)

            m_dbRoot.SelectItem()

            If Not Me.BodyEditor Is Nothing Then
                If Me.BodyEditor.cboBodyPart.SelectedItem Is Nothing Then
                    Me.BodyEditor.BodyPart = m_dbRoot
                End If
            End If

            'Me.ManualAddHistory(New AnimatGUI.Framework.UndoSystem.AddBodyPartEvent(Me.BodyEditor, Me, Nothing, m_dbRoot))
            Return True
        End Function

        Public Overridable Sub DeleteBodyPart(ByVal strID As String)
            Dim bpPart As AnimatGUI.DataObjects.Physical.BodyPart = Me.FindBodyPart(strID)
            DeleteBodyPart(bpPart)
        End Sub

        Public Overridable Sub DeleteBodyPart(ByVal bpPart As AnimatGUI.DataObjects.Physical.BodyPart)

            'Deselect it before continuing with the delete.
            If bpPart.Selected Then
                bpPart.DeselectItem()
            End If

            'If the object to delete is a joint, then switch to deleting the parent rigid body it is connected to.
            'You can never just delete a joint by itself. You must always delete the underlying body part to get rid of the joint.
            If TypeOf bpPart Is AnimatGUI.DataObjects.Physical.Joint Then
                bpPart = DirectCast(bpPart.Parent, AnimatGUI.DataObjects.Physical.BodyPart)
            End If

            Dim bpDeletedPart As AnimatGUI.DataObjects.Physical.RigidBody = DirectCast(bpPart, AnimatGUI.DataObjects.Physical.RigidBody)
            Dim bpParentPart As AnimatGUI.DataObjects.Physical.RigidBody
            If Not bpDeletedPart.IsRoot Then
                bpParentPart = DirectCast(bpDeletedPart.Parent, AnimatGUI.DataObjects.Physical.RigidBody)
            End If

            DeleteBodyPartInternal(bpPart)

            'If Not Me.BodyEditor Is Nothing Then
            '    'Me.ManualAddHistory(New AnimatGUI.Framework.UndoSystem.DeleteBodyPartEvent(Me.BodyEditor, Me, bpParentPart, bpDeletedPart))
            'End If
        End Sub

        Protected Overridable Sub DeleteBodyPartInternal(ByVal bpPart As AnimatGUI.DataObjects.Physical.BodyPart)

            If Not bpPart.Parent Is Nothing Then
                If TypeOf bpPart Is AnimatGUI.DataObjects.Physical.RigidBody Then

                    If bpPart Is m_dbRoot Then
                        bpPart.BeforeRemoveFromList(True, True)
                        m_dbRoot = Nothing
                        bpPart.AfterRemoveFromList(True, True)
                    Else
                        Dim bpParent As RigidBody = DirectCast(bpPart.Parent, RigidBody)
                        If bpParent.ChildBodies.Contains(bpPart.ID) Then
                            bpParent.ChildBodies.Remove(bpPart.ID)
                        Else
                            Throw New System.Exception("You are attempting to delete an child item that is not in parents child collection.")
                        End If
                    End If
                ElseIf TypeOf bpPart Is AnimatGUI.DataObjects.Physical.Joint Then
                    Dim bpParent As RigidBody = DirectCast(bpPart.Parent, RigidBody)
                    DeleteBodyPart(bpParent)
                End If

                bpPart.RemoveWorksapceTreeView()
            End If

        End Sub

        Public Overrides Sub FindChildrenOfType(ByVal tpTemplate As System.Type, ByVal colDataObjects As Collections.DataObjects)
            If Not m_dbRoot Is Nothing Then
                m_dbRoot.FindChildrenOfType(tpTemplate, colDataObjects)
            End If
        End Sub

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.DragObject
            Dim oStructure As Object = Util.Environment.FindStructureFromAll(strStructureName, bThrowError)

            If Not oStructure Is Nothing Then
                Return DirectCast(oStructure, DataObjects.DragObject)
            End If

            Return Nothing
        End Function

        Public Overridable Function GetChildPartsList() As AnimatGUI.Collections.SortedBodyParts
            Dim aryParts As New AnimatGUI.Collections.SortedBodyParts(Nothing)
            If Not m_dbRoot Is Nothing Then
                m_dbRoot.GetChildPartsList(aryParts)
            End If

            Return aryParts
        End Function

        Public Overridable Function GetChildBodiesList() As AnimatGUI.Collections.SortedRigidBodies
            Dim aryBodies As New AnimatGUI.Collections.SortedRigidBodies(Nothing)
            If Not m_dbRoot Is Nothing Then
                m_dbRoot.GetChildBodiesList(aryBodies)
            End If

            Return aryBodies
        End Function

        Public Overridable Function GetChildJointsList() As AnimatGUI.Collections.SortedJoints
            Dim aryBodies As New AnimatGUI.Collections.SortedJoints(Nothing)
            If Not m_dbRoot Is Nothing Then
                m_dbRoot.GetChildJointsList(aryBodies)
            End If

            Return aryBodies
        End Function

        Public Overridable Function CreateJointTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, _
                                                      ByVal thSelectedPart As TypeHelpers.LinkedBodyPart) As Crownwood.DotNetMagic.Controls.Node

            Dim tnOrganism As New Crownwood.DotNetMagic.Controls.Node(Me.Name)
            If Not tnParent Is Nothing Then
                tnParent.Nodes.Add(tnOrganism)
            Else
                tvTree.Nodes.Add(tnOrganism)
            End If
            tnOrganism.ForeColor = Color.Black

            If Not m_dbRoot Is Nothing Then
                m_dbRoot.CreateJointTreeView(tvTree, tnOrganism, thSelectedPart)
            End If

            Return tnOrganism
        End Function

        Public Overridable Function CreateRigidBodyTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, _
                                                            ByVal thSelectedPart As TypeHelpers.LinkedBodyPart) As Crownwood.DotNetMagic.Controls.Node

            Dim tnOrganism As New Crownwood.DotNetMagic.Controls.Node(Me.Name)
            If Not tnParent Is Nothing Then
                tnParent.Nodes.Add(tnOrganism)
            Else
                tvTree.Nodes.Add(tnOrganism)
            End If
            tnOrganism.ForeColor = Color.Black

            If Not m_dbRoot Is Nothing Then
                m_dbRoot.CreateRigidBodyTreeView(tvTree, tnOrganism, thSelectedPart)
            End If

            Return tnOrganism
        End Function

        Public Overridable Sub ResetEnableFluidsForRigidBodies()
            If Not m_dbRoot Is Nothing Then
                m_dbRoot.ResetEnableFluidsForRigidBodies()
            End If
        End Sub

#Region " Workspace TreeView "

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, _
                                                      ByVal doParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                      Optional ByVal bRootObject As Boolean = False)
            MyBase.CreateWorkspaceTreeView(doParent, doParentNode, bRootObject)

            If m_tnBodyPlanNode Is Nothing Then
                m_tnBodyPlanNode = Util.ProjectWorkspace.AddTreeNode(m_tnWorkspaceNode, "Body Plan", "AnimatGUI.Joint.gif")
                m_tnBodyPlanNode.Tag = New Framework.DataObjectTreeViewRef(Me)
            End If

            If Not m_dbRoot Is Nothing Then
                m_dbRoot.CreateWorkspaceTreeView(Me, m_tnBodyPlanNode)
            End If

        End Sub

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Structure", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))
                Dim mcClone As New System.Windows.Forms.ToolStripMenuItem("Clone Structure", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Me.OnCloneStructure))

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Physical.PhysicalStructure.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete, mcClone})

                If Me.RootBody Is Nothing Then
                    Dim mcAddRoot As New System.Windows.Forms.ToolStripMenuItem("Add root body", Util.Application.ToolStripImages.GetImage("AnimatGUI.Blank.gif"), New EventHandler(AddressOf Me.OnAddRootBody))
                    popup.Items.Add(mcAddRoot)
                End If

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                Return True
            ElseIf tnSelectedNode Is m_tnBodyPlanNode Then
                Return True
            End If

            Return False
        End Function

        Public Overrides Sub WorkspaceTreeviewDoubleClick(ByVal tnSelectedNode As Crownwood.DotNetMagic.Controls.Node)
            Util.Application.EditBodyPlan(Me)
        End Sub

#End Region

#Region " Data Item TreeView "

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, ByVal tpTemplatePartType As Type) As Crownwood.DotNetMagic.Controls.Node

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load(Me.AssemblyModuleName)

            frmDataItem.ImageManager.AddImage(myAssembly, Me.WorkspaceImageName)
            frmDataItem.ImageManager.AddImage(myAssembly, "AnimatGUI.Neuron.gif")
            frmDataItem.ImageManager.AddImage(myAssembly, "AnimatGUI.Joint.gif")
            frmDataItem.ImageManager.AddImage(myAssembly, "AnimatGUI.DefaultObject.gif")
            frmDataItem.ImageManager.AddImage(myAssembly, "AnimatGUI.DefaultLink.gif")

            Dim tnNode As New Crownwood.DotNetMagic.Controls.Node(Me.Name)
            frmDataItem.TreeView.Nodes.Add(tnNode)
            tnNode.ImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.WorkspaceImageName)
            tnNode.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.WorkspaceImageName)
            tnNode.Tag = Me

            Dim tnBodyplanNode As New Crownwood.DotNetMagic.Controls.Node("Body Plan")
            tnBodyplanNode = tnNode.Nodes.Add(tnBodyplanNode)
            tnBodyplanNode.ImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.Joint.gif")
            tnBodyplanNode.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex("AnimatGUI.Joint.gif")

            If Not m_dbRoot Is Nothing Then
                m_dbRoot.CreateDataItemTreeView(frmDataItem, tnBodyplanNode, tpTemplatePartType)
            End If

        End Function

#End Region

#Region " Load/Save Methods "

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            If Not m_dbRoot Is Nothing Then
                m_dbRoot.InitializeAfterLoad()
            End If
        End Sub

        Public Overrides Sub InitializeSimulationReferences()
            MyBase.InitializeSimulationReferences()

            If Not m_dbRoot Is Nothing Then
                m_dbRoot.InitializeSimulationReferences()
            End If
        End Sub

        Public Overridable Overloads Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            Try
                oXml.IntoElem()

                'm_strStructureType = oXml.GetChildString("Type", m_strStructureType)

                If oXml.FindChildElement("RigidBody", False) Then
                    m_dbRoot = DirectCast(Util.Simulation.CreateObject(oXml, "RigidBody", Me), DataObjects.Physical.RigidBody)
                    m_dbRoot.IsRoot = True
                    m_dbRoot.LoadData(Me, oXml)
                Else
                    m_dbRoot = Nothing
                End If

                'Load collision pairs
                If oXml.FindChildElement("CollisionExclusionPairs", False) Then
                    oXml.IntoElem()

                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    Dim doPair As AnimatGUI.DataObjects.Physical.CollisionPair
                    For iIndex As Integer = 0 To iCount
                        oXml.FindChildByIndex(iIndex)
                        doPair = New AnimatGUI.DataObjects.Physical.CollisionPair(Me)
                        doPair.LoadData(oXml)

                        If Not doPair.Part1 Is Nothing AndAlso Not doPair.Part2 Is Nothing Then
                            m_aryCollisionExclusionPairs.Add(doPair)
                        End If
                    Next

                    oXml.OutOfElem()
                End If

                'Now lets find the index values used for adding new bodies and joints.
                Dim aryBodies As AnimatGUI.Collections.SortedRigidBodies = Me.GetChildBodiesList()
                Dim aryJoints As AnimatGUI.Collections.SortedJoints = Me.GetChildJointsList()

                m_iNewBodyIndex = Util.ExtractIDCount("Body", aryBodies)
                m_iNewJointIndex = Util.ExtractIDCount("Joint", aryJoints)

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Overloads Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml, Me.TypeName)

            Try

                oXml.IntoElem()

                oXml.AddChildElement("Type", "Basic")

                If Not m_dbRoot Is Nothing Then
                    m_dbRoot.SaveData(Me, oXml)
                End If

                'Save collision pairs
                oXml.AddChildElement("CollisionExclusionPairs")
                oXml.IntoElem()
                For Each doPair As AnimatGUI.DataObjects.Physical.CollisionPair In m_aryCollisionExclusionPairs
                    doPair.SaveData(oXml)
                Next
                oXml.OutOfElem()  'Out of CollisionExclusionPairs

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            Try
                oXml.IntoElem()

                oXml.AddChildElement("Type", "Basic")

                If Not m_dbRoot Is Nothing Then
                    m_dbRoot.SaveSimulationXml(oXml, Me)
                End If

                'Save collision pairs
                oXml.AddChildElement("CollisionExclusionPairs")
                oXml.IntoElem()
                For Each doPair As AnimatGUI.DataObjects.Physical.CollisionPair In m_aryCollisionExclusionPairs
                    doPair.SaveSimulationXml(oXml)
                Next
                oXml.OutOfElem()  'Out of CollisionExclusionPairs


                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        'Public Overridable Sub CreateFiles()

        '    'Save the nervous system file
        '    Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

        '    If Util.Application.ProjectPath.Length > 0 Then
        '        If Not System.IO.File.Exists(Util.GetFilePath(Util.Application.ProjectPath, Me.BodyPlanFile)) Then
        '            'Save the body plan file
        '            oXml = New Interfaces.StdXml
        '            oXml.AddElement("Structure")
        '            oXml.Save(Util.GetFilePath(Util.Application.ProjectPath, Me.BodyPlanFile))
        '        End If
        '    End If

        'End Sub

#End Region

        Public Overridable Sub ClearSelectedBodyParts()
            If Not m_dbRoot Is Nothing Then
                m_dbRoot.ClearSelectedBodyParts()
            End If
        End Sub

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)
            If Not m_dbRoot Is Nothing Then
                m_dbRoot.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
            End If
        End Sub

        Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean

            Try
                Dim bDelete As Boolean = True
                If bAskToDelete AndAlso Util.ShowMessage("Are you certain that you want to permanently delete this " & _
                                    "structure\organism and all of its related files?", _
                                    "Delete Structure", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                    bDelete = False
                End If

                If bDelete Then
                    DeleteInternal()
                End If

                Return Not bDelete
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Function

        Public Overridable Sub DeleteInternal()
            If Not Util.Environment.Structures Is Nothing Then
                Util.Environment.Structures.Remove(Me.ID)
            End If
            Me.RemoveWorksapceTreeView()
            m_tnWorkspaceNode = Nothing
            m_tnBodyPlanNode = Nothing
        End Sub

        Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_dbRoot Is Nothing Then doObject = m_dbRoot.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryCollisionExclusionPairs Is Nothing Then doObject = m_aryCollisionExclusionPairs.FindObjectByID(strID)
            Return doObject

        End Function

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If Me.IsInitialized Then
                Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, Me.TypeName, Me.ID, Me.GetSimulationXml(Me.TypeName), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not Util.Simulation Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Util.Simulation.ID, Me.TypeName, Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

#End Region

#Region " Events "

        Protected Overridable Sub OnCloneStructure(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Throw New Exception("Not implemented!")
                'Dim newStructure As DataObjects.Physical.PhysicalStructure = DirectCast(

                'Util.Environment.NewStructureCount = Util.Environment.NewStructureCount + 1
                'newStructure.Name = "Structure_" & Util.Environment.NewStructureCount
                'Util.Environment.Structures.Add(newStructure.ID, newStructure)

                'newStructure.CreateWorkspaceTreeView(Util.Simulation, Util.ProjectWorkspace)
                'Util.ProjectWorkspace.TreeView.SelectedNode = newStructure.WorkspaceStructureNode

                'Util.Application.SaveProject(Util.Application.ProjectFile)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub OnAddRootBody(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Me.AddRootBody()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace

