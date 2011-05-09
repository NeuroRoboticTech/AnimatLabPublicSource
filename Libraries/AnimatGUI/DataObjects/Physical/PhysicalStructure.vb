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

    Public Class PhysicalStructure
        Inherits DataObjects.DragObject

#Region " Delegates "

        Delegate Sub PositionChangedDelegate()
        Delegate Sub RotationChangedDelegate()
        Delegate Sub SelectionChangedDelegate(ByVal bSelected As Boolean, ByVal bSelectMultiple As Boolean)

#End Region

#Region " Attributes "

        Protected m_strStructureType As String = "Basic"

        Protected m_strDescription As String = ""

        Protected m_svPosition As ScaledVector3
        Protected m_svRotation As ScaledVector3
        Protected m_snSize As AnimatGUI.Framework.ScaledNumber

        Protected m_bVisible As Boolean = True
        Protected m_Transparencies As BodyTransparencies

        Protected m_clAmbient As System.Drawing.Color
        Protected m_clDiffuse As System.Drawing.Color
        Protected m_clSpecular As System.Drawing.Color
        Protected m_fltShininess As Single = 64

        Protected m_strTexture As String = ""

        Protected m_dbRoot As DataObjects.Physical.RigidBody

        Protected m_frmBodyEditor As Forms.BodyPlan.Editor

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
        Protected Overridable ReadOnly Property Structures(ByVal dsSim As AnimatGUI.DataObjects.Simulation) As Collections.SortedStructures
            Get
                Return dsSim.Environment.Structures
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
        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                If Value.Trim.Length = 0 Then
                    Throw New System.Exception("You can not set the name for the organism to blank.")
                End If

                CheckForUniqueneName(Value)

                Dim strOldName As String = m_strName

                m_strName = Value.Trim

                If Not m_tnWorkspaceNode Is Nothing Then
                    Dim bExpanded As Boolean = m_tnWorkspaceNode.IsExpanded
                    m_tnWorkspaceNode.Remove()
                    If Not m_doParent Is Nothing Then CreateWorkspaceTreeView(m_doParent, m_doParent.WorkspaceNode)
                    Util.ProjectWorkspace.ctrlTreeView.SelectedNode = m_tnWorkspaceNode
                    If bExpanded Then m_tnWorkspaceNode.Expand()
                End If

                RenameFiles(strOldName)

                RenameWindowTitles()

            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Description() As String
            Get
                Return m_strDescription
            End Get
            Set(ByVal Value As String)
                m_strDescription = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property ItemName() As String
            Get
                Return Me.Name
            End Get
            Set(ByVal Value As String)
                Me.Name = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Structure.gif"
            End Get
        End Property

#Region " Location Properties "

        Public Overridable Property Position() As Framework.ScaledVector3
            Get
                Return m_svPosition
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("Position", value.GetSimulationXml("Position"), True)
                m_svPosition.CopyData(value)
                RaiseMovedEvent()
            End Set
        End Property

        Public Overridable Property Rotation() As Framework.ScaledVector3
            Get
                Return m_svRotation
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("Rotation", value.GetSimulationXml("Rotation"), True)
                m_svRotation.CopyData(value)
                RaiseRotatedEvent()
            End Set
        End Property

        Public Overridable ReadOnly Property RadianRotation() As Framework.ScaledVector3
            Get
                Dim svRotation As New Framework.ScaledVector3(Me, "Rotation", "Radian Rotations", "rad", "rad")
                svRotation.IgnoreChangeValueEvents = True
                svRotation.X.ActualValue = Util.DegreesToRadians(CSng(m_svRotation.X.ActualValue))
                svRotation.Y.ActualValue = Util.DegreesToRadians(CSng(m_svRotation.Y.ActualValue))
                svRotation.Z.ActualValue = Util.DegreesToRadians(CSng(m_svRotation.Z.ActualValue))
                svRotation.IgnoreChangeValueEvents = False
                Return svRotation
            End Get
        End Property

#End Region

#Region " Color Properties "

        Public Overridable Property Visible() As Boolean
            Get
                Return m_bVisible
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("Visible", Value.ToString(), True)
                m_bVisible = Value
            End Set
        End Property

        Public Overridable Property Transparencies() As BodyTransparencies
            Get
                Return m_Transparencies
            End Get
            Set(ByVal value As BodyTransparencies)
                m_Transparencies = value
            End Set
        End Property

        Public Overridable Property Ambient() As System.Drawing.Color
            Get
                Return m_clAmbient
            End Get
            Set(ByVal value As System.Drawing.Color)
                SetSimData("Ambient", Util.SaveColorXml("Ambient", value), True)
                m_clAmbient = value
            End Set
        End Property

        Public Overridable Property Diffuse() As System.Drawing.Color
            Get
                Return m_clDiffuse
            End Get
            Set(ByVal value As System.Drawing.Color)
                SetSimData("Diffuse", Util.SaveColorXml("Diffuse", value), True)
                m_clDiffuse = value
            End Set
        End Property

        Public Overridable Property Specular() As System.Drawing.Color
            Get
                Return m_clSpecular
            End Get
            Set(ByVal value As System.Drawing.Color)
                SetSimData("Specular", Util.SaveColorXml("Specular", value), True)
                m_clSpecular = value
            End Set
        End Property

        Public Overridable Property Shininess() As Single
            Get
                Return m_fltShininess
            End Get
            Set(ByVal value As Single)
                If (value < 0) Then
                    Throw New System.Exception("Shininess must be greater than or equal to zero.")
                End If
                If (value > 128) Then
                    Throw New System.Exception("Shininess must be less than 128.")
                End If
                SetSimData("Shininess", value.ToString(), True)
                m_fltShininess = value
            End Set
        End Property

        Public Overridable Property Texture() As String
            Get
                Return m_strTexture
            End Get
            Set(ByVal Value As String)
                If Not Value Is Nothing Then
                    Dim strPath As String, strFile As String
                    If Util.DetermineFilePath(Value, strPath, strFile) Then
                        Value = strFile
                    End If
                End If

                'Check to see if the file exists.
                If Value.Trim.Length > 0 Then
                    If Not File.Exists(Value) Then
                        Throw New System.Exception("The specified file does not exist: " & Value)
                    End If

                    'Attempt to load the file first to make sure it is a valid image file.
                    Try
                        Dim bm As New Bitmap(Value)
                    Catch ex As System.Exception
                        Throw New System.Exception("Unable to load the texture file. This does not appear to be a vaild image file.")
                    End Try
                End If

                SetSimData("Texture", Value, True)
                m_strTexture = Value

            End Set
        End Property

#End Region

        <Browsable(False)> _
        Public Overridable ReadOnly Property BodyPlanFile() As String
            Get
                Return Me.Name & ".astl"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property BodyPlanEditorFile() As String
            Get
                Return Me.Name & ".abpe"
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
        Public Overridable Property BodyEditor() As Forms.BodyPlan.Editor
            Get
                Return m_frmBodyEditor
            End Get
            Set(ByVal Value As Forms.BodyPlan.Editor)
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
                '    MessageBox.Show("Collision pairs messed up!!!")
                'End If

                m_aryCollisionExclusionPairs = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""

            m_clAmbient = Color.FromArgb(255, 25, 25, 25)
            m_clDiffuse = Color.FromArgb(255, 255, 255, 255)
            m_clSpecular = Color.FromArgb(255, 64, 64, 64)
            m_fltShininess = 64
            m_Transparencies = New BodyTransparencies(Me)

            m_svPosition = New ScaledVector3(Me, "Position", "Location of the structure relative to the center of the world.", "Meters", "m")
            m_svRotation = New ScaledVector3(Me, "Rotation", "Rotation of the object.", "Degrees", "Deg")

            AddHandler m_svPosition.ValueChanged, AddressOf Me.OnPositionValueChanged
            AddHandler m_svRotation.ValueChanged, AddressOf Me.OnRotationValueChanged
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strName.GetType(), "Name", _
                                        "Structure Properties", "The name for this structure. ", m_strName))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Structure Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Body Plan", Me.BodyPlanFile.GetType(), "BodyPlanFile", _
                                        "Structure Properties", "Sets the body plan file.", Me.BodyPlanFile, True))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = Me.Position.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Position", pbNumberBag.GetType(), "Position", _
                                        "Coordinates", "Sets the position of this structure.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

            pbNumberBag = Me.Rotation.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Rotation", pbNumberBag.GetType(), "Rotation", _
                                        "Coordinates", "Sets the rotation of this body part.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))


            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Visible", m_bVisible.GetType(), "Visible", _
                                        "Visibility", "Sets whether or not this part is visible in the simulation.", m_bVisible))

            pbNumberBag = Me.Transparencies.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Transparencies", pbNumberBag.GetType(), "Transparencies", _
                                        "Visibility", "Sets the transparencies for this part in the various selection modes.", pbNumberBag, _
                                        "", GetType(BodyTransparencies.BodyTransparencyPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Description", m_strDescription.GetType(), "Description", _
                                        "Part Properties", "Sets the description for this body part.", m_strDescription, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Ambient", m_clAmbient.GetType(), "Ambient", _
                                        "Visibility", "Sets the ambient color for this item.", m_clAmbient))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Diffuse", m_clDiffuse.GetType(), "Diffuse", _
                                        "Visibility", "Sets the diffuse color for this item.", m_clDiffuse))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Specular", m_clSpecular.GetType(), "Specular", _
                                        "Visibility", "Sets the specular color for this item.", m_clSpecular))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Shininess", m_fltShininess.GetType(), "Shininess", _
                                        "Visibility", "Sets the shininess for this item.", m_fltShininess))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Collision Exclusions", m_aryCollisionExclusionPairs.GetType(), "CollisionExclusionPairs", _
                                        "Structure Properties", "Pairs of body parts that should be excluded from collision detection between each other.", m_aryCollisionExclusionPairs, _
                                        GetType(TypeHelpers.CollisionPairsTypeEditor), GetType(TypeHelpers.CollisionPairsTypeConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_svPosition Is Nothing Then m_svPosition.ClearIsDirty()
            If Not m_svRotation Is Nothing Then m_svRotation.ClearIsDirty()
            If Not m_dbRoot Is Nothing Then m_dbRoot.ClearIsDirty()
            If Not m_aryCollisionExclusionPairs Is Nothing Then m_aryCollisionExclusionPairs.ClearIsDirty()
            If Not m_Transparencies Is Nothing Then m_Transparencies.ClearIsDirty()
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

            m_strName = doOrig.m_strName
            m_strDescription = doOrig.m_strDescription
            m_strStructureType = doOrig.m_strStructureType
            If Not doOrig.WorkspaceImage Is Nothing Then m_WorkspaceImage = DirectCast(doOrig.WorkspaceImage.Clone(), Image)
            If Not doOrig.m_DragImage Is Nothing Then m_DragImage = DirectCast(doOrig.m_DragImage.Clone(), Image)
            If Not doOrig.RootBody Is Nothing Then m_dbRoot = DirectCast(doOrig.RootBody.Clone(Me, bCutData, doRoot), RigidBody)

            m_svPosition = DirectCast(doOrig.m_svPosition.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)
            m_svRotation = DirectCast(doOrig.m_svRotation.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)

            m_Transparencies = DirectCast(doOrig.m_Transparencies.Clone(Me, bCutData, doRoot), BodyTransparencies)
            m_bVisible = doOrig.m_bVisible

            m_clAmbient = doOrig.m_clAmbient
            m_clDiffuse = doOrig.m_clDiffuse
            m_clSpecular = doOrig.m_clSpecular
            m_fltShininess = doOrig.m_fltShininess
            m_strTexture = doOrig.m_strTexture

            m_frmBodyEditor = doOrig.m_frmBodyEditor
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

            AddHandler m_svPosition.ValueChanged, AddressOf Me.OnPositionValueChanged
            AddHandler m_svRotation.ValueChanged, AddressOf Me.OnRotationValueChanged

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
            End If

            rbRootToAdd.SetDefaultSizes()
            rbRootToAdd.Name = "Root"
            rbRootToAdd.IsRoot = True
            rbRootToAdd.IsCollisionObject = True
            rbRootToAdd.ContactSensor = False
            rbRootToAdd.Freeze = True
            rbRootToAdd.BeforeAddToList()

            m_dbRoot = rbRootToAdd

            If bAddDefaultGraphics Then
                m_dbRoot.CreateDefaultGraphicsObject()
            End If

            If Not m_dbRoot Is Nothing Then
                m_dbRoot.CreateWorkspaceTreeView(Me, m_tnBodyPlanNode)
            End If

            m_dbRoot.AfterAddToList()

            'Dim vPos As New Vec3d(Nothing, 0.5, 0, 0)
            'Dim vNorm As New Vec3d(Nothing, 0, 1, 0)
            'm_dbRoot.AddChildBody(vPos, vNorm)

            'Me.ManualAddHistory(New AnimatGUI.Framework.UndoSystem.AddBodyPartEvent(Me.BodyEditor, Me, Nothing, m_dbRoot))
            Return True
        End Function

        Public Overridable Sub DeleteBodyPart(ByVal strID As String)
            Dim bpPart As AnimatGUI.DataObjects.Physical.BodyPart = Me.FindBodyPart(strID)
            DeleteBodyPart(bpPart)
        End Sub

        Public Overridable Sub DeleteBodyPart(ByVal bpPart As AnimatGUI.DataObjects.Physical.BodyPart)

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

            If Not Me.BodyEditor Is Nothing Then
                'Me.ManualAddHistory(New AnimatGUI.Framework.UndoSystem.DeleteBodyPartEvent(Me.BodyEditor, Me, bpParentPart, bpDeletedPart))
            End If
        End Sub

        Protected Overridable Sub DeleteBodyPartInternal(ByVal bpPart As AnimatGUI.DataObjects.Physical.BodyPart)

            If Not bpPart.Parent Is Nothing Then
                If TypeOf bpPart Is AnimatGUI.DataObjects.Physical.RigidBody Then

                    If bpPart Is m_dbRoot Then
                        bpPart.BeforeRemoveFromList()
                        m_dbRoot = Nothing
                        bpPart.AfterRemoveFromList()
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

        Public Overrides Sub FindChildrenOfType(ByVal tpTemplate As System.Type, ByRef colDataObjects As Collections.DataObjects)
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

#Region " Workspace TreeView "

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, ByVal doParentNode As Crownwood.DotNetMagic.Controls.Node)
            MyBase.CreateWorkspaceTreeView(doParent, doParentNode)

            m_tnBodyPlanNode = Util.ProjectWorkspace.AddTreeNode(m_tnWorkspaceNode, "Body Plan", "AnimatGUI.Joint.gif")
            m_tnBodyPlanNode.Tag = doParentNode

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

        Public Overrides Function WorkspaceTreeviewDoubleClick(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode OrElse tnSelectedNode Is m_tnBodyPlanNode Then
                Util.Application.EditBodyPlan(Me)
                Return True
            End If

            Return False
        End Function

#End Region

#Region " Data Item TreeView "

        Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, ByVal tpTemplatePartType As Type) As Crownwood.DotNetMagic.Controls.Node

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load(Me.AssemblyModuleName)
            frmDataItem.ImageManager.AddImage(myAssembly, Me.WorkspaceImageName)

            Dim tnNode As New Crownwood.DotNetMagic.Controls.Node(Me.Name)
            frmDataItem.TreeView.Nodes.Add(tnNode)
            tnNode.ImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.WorkspaceImageName)
            tnNode.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.WorkspaceImageName)
            tnNode.Tag = Me

            If Not m_dbRoot Is Nothing Then
                m_dbRoot.CreateDataItemTreeView(frmDataItem, tnNode, tpTemplatePartType)
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

            If m_doInterface Is Nothing AndAlso Not Util.Application.SimulationInterface Is Nothing AndAlso Util.Application.SimulationInterface.SimOpen Then
                m_doInterface = New Interfaces.DataObjectInterface(Util.Application.SimulationInterface, Me.ID)
                AddHandler m_doInterface.OnPositionChanged, AddressOf Me.OnPositionChanged
                AddHandler m_doInterface.OnRotationChanged, AddressOf Me.OnRotationChanged
                AddHandler m_doInterface.OnSelectionChanged, AddressOf Me.OnSelectionChanged
            End If

            If Not m_dbRoot Is Nothing Then
                m_dbRoot.InitializeSimulationReferences()
            End If
        End Sub

        Public Overridable Overloads Sub LoadData(ByRef oXml As Interfaces.StdXml)

            Try
                oXml.IntoElem()

                m_strName = oXml.GetChildString("Name")
                m_strID = oXml.GetChildString("ID", System.Guid.NewGuid().ToString())
                m_strStructureType = oXml.GetChildString("Type", m_strStructureType)

                m_strDescription = oXml.GetChildString("Description", "")
                m_Transparencies.LoadData(oXml)
                m_bVisible = oXml.GetChildBool("IsVisible", m_bVisible)

                m_svPosition.LoadData(oXml, "Position")
                m_svRotation.LoadData(oXml, "Rotation")

                m_clAmbient = Util.LoadColor(oXml, "Ambient", m_clAmbient)
                m_clDiffuse = Util.LoadColor(oXml, "Diffuse", m_clDiffuse)
                m_clSpecular = Util.LoadColor(oXml, "Specular", m_clSpecular)
                m_fltShininess = oXml.GetChildFloat("Shininess", m_fltShininess)

                m_strTexture = oXml.GetChildString("Texture", m_strTexture)
                m_strTexture = Util.VerifyFilePath(m_strTexture)

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

        Public Overridable Overloads Sub SaveData(ByRef oXml As Interfaces.StdXml)

            Try
                oXml.AddChildElement("Structure")
                oXml.IntoElem()

                oXml.AddChildElement("ID", m_strID)
                oXml.AddChildElement("Name", m_strName)
                oXml.AddChildElement("Type", m_strStructureType)

                oXml.AddChildElement("Description", m_strDescription)
                m_Transparencies.SaveData(oXml)
                oXml.AddChildElement("IsVisible", m_bVisible)

                Util.SaveColor(oXml, "Ambient", m_clAmbient)
                Util.SaveColor(oXml, "Diffuse", m_clDiffuse)
                Util.SaveColor(oXml, "Specular", m_clSpecular)
                oXml.AddChildElement("Shininess", m_fltShininess)
                oXml.AddChildElement("Texture", m_strTexture)

                m_svPosition.SaveData(oXml, "Position")
                m_svRotation.SaveData(oXml, "Rotation")

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

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            Try
                oXml.AddChildElement("Structure")
                oXml.IntoElem()

                oXml.AddChildElement("ID", m_strID)
                oXml.AddChildElement("Name", m_strName)
                oXml.AddChildElement("Type", m_strStructureType)

                oXml.AddChildElement("Description", m_strDescription)
                m_Transparencies.SaveSimulationXml(oXml, Me)
                oXml.AddChildElement("IsVisible", m_bVisible)

                Util.SaveColor(oXml, "Ambient", m_clAmbient)
                Util.SaveColor(oXml, "Diffuse", m_clDiffuse)
                Util.SaveColor(oXml, "Specular", m_clSpecular)
                oXml.AddChildElement("Shininess", m_fltShininess)
                oXml.AddChildElement("Texture", m_strTexture)

                m_svPosition.SaveSimulationXml(oXml, Me, "Position")
                m_svRotation.SaveSimulationXml(oXml, Me, "Rotation")

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
        '    Dim oXml As New Interfaces.StdXml

        '    If Util.Application.ProjectPath.Length > 0 Then
        '        If Not System.IO.File.Exists(Util.GetFilePath(Util.Application.ProjectPath, Me.BodyPlanFile)) Then
        '            'Save the body plan file
        '            oXml = New Interfaces.StdXml
        '            oXml.AddElement("Structure")
        '            oXml.Save(Util.GetFilePath(Util.Application.ProjectPath, Me.BodyPlanFile))
        '        End If
        '    End If

        'End Sub

        Public Overridable Sub RenameFiles(ByVal strOriginalName As String)
            Dim strExtension As String, strNewFile As String

            If Util.Application.ProjectPath.Trim.Length > 0 AndAlso strOriginalName.Trim.Length > 0 Then
                Dim di As DirectoryInfo = New DirectoryInfo(Util.Application.ProjectPath)
                Dim fiFiles As FileInfo() = di.GetFiles(strOriginalName & ".*")

                For Each fiFile As FileInfo In fiFiles
                    strExtension = Util.GetFileExtension(fiFile.Name)
                    strNewFile = Util.GetFilePath(Util.Application.ProjectPath, (Me.Name & "." & strExtension))

                    fiFile.MoveTo(strNewFile)
                Next
            End If

        End Sub

        Public Overridable Sub RemoveFiles()

            If Util.Application.ProjectPath.Trim.Length > 0 Then
                Dim di As DirectoryInfo = New DirectoryInfo(Util.Application.ProjectPath)
                Dim fiFiles As FileInfo() = di.GetFiles(Me.Name & ".*")

                For Each fiFile As FileInfo In fiFiles
                    fiFile.Delete()
                Next
            End If

        End Sub

#End Region

        Protected Overridable Sub CheckForUniqueneName(ByVal strName As String)

            Dim doOrganism As DataObjects.Physical.PhysicalStructure
            If Not Me.Structures(Util.Application.Simulation) Is Nothing Then
                For Each deEntry As DictionaryEntry In Me.Structures(Util.Application.Simulation)
                    doOrganism = DirectCast(deEntry.Value, DataObjects.Physical.PhysicalStructure)

                    If Not doOrganism Is Me Then
                        If doOrganism.Name.Trim.ToUpper = strName.Trim.ToUpper Then
                            Throw New System.Exception("The name '" & doOrganism.Name & "' is already being used. Please choose a different name.")
                        End If
                    End If
                Next
            End If

        End Sub

        Protected Overridable Sub RenameWindowTitles()

            Try

                If Not Me.BodyEditor Is Nothing Then
                    Me.BodyEditor.Title = "Edit " & Me.Name

                    'If Not Me.BodyPlanStructureNode Is Nothing Then
                    '    Me.BodyPlanStructureNode.Text = Me.Name
                    'End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

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
                If bAskToDelete AndAlso MessageBox.Show("Are you certain that you want to permanently delete this " & _
                                    "structure\organism and all of its related files?", _
                                    "Delete Structure", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                    bDelete = False
                End If

                If bDelete Then
                    'Lets see if there are any open windows for this organism/Structure
                    Dim frmBehavioral As Forms.Behavior.Editor
                    Dim frmBodyPlan As Forms.BodyPlan.Editor
                    For Each oChild As Form In Util.Application.ChildForms
                        If TypeOf oChild Is Forms.Behavior.Editor Then
                            frmBehavioral = DirectCast(oChild, Forms.Behavior.Editor)
                            If frmBehavioral.Organism Is Me Then
                                frmBehavioral.Close()
                            End If
                        End If

                        If TypeOf oChild Is Forms.BodyPlan.Editor Then
                            frmBodyPlan = DirectCast(oChild, Forms.BodyPlan.Editor)
                            If frmBodyPlan.PhysicalStructure Is Me Then
                                frmBodyPlan.Close()
                            End If
                        End If
                    Next


                    If Not Me.Structures(Util.Application.Simulation) Is Nothing Then
                        Me.Structures(Util.Application.Simulation).Remove(Me.ID)
                    End If
                    Me.RemoveWorksapceTreeView()
                    m_tnWorkspaceNode = Nothing
                    m_tnBodyPlanNode = Nothing
                    Me.RemoveFiles()

                    Util.Application.SaveProject(Util.Application.ProjectFile)
                End If

                Return Not bDelete
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Function

        Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_dbRoot Is Nothing Then doObject = m_dbRoot.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryCollisionExclusionPairs Is Nothing Then doObject = m_aryCollisionExclusionPairs.FindObjectByID(strID)
            Return doObject

        End Function

#Region " Add-Remove to List Methods "

        Public Overrides Sub BeforeAddToList(Optional ByVal bThrowError As Boolean = True)
            Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, "Structure", Me.GetSimulationXml("Structure"), bThrowError)
            InitializeSimulationReferences()
        End Sub

        Public Overrides Sub BeforeRemoveFromList(Optional ByVal bThrowError As Boolean = True)
            Util.Application.SimulationInterface.RemoveItem(Util.Simulation.ID, "Structure", Me.ID, bThrowError)
            m_doInterface = Nothing
        End Sub

#End Region

#End Region

#Region " Events "

        Public Event Moved(ByRef bpPart As AnimatGUI.DataObjects.Physical.BodyPart)
        Public Event Rotated(ByRef bpPart As AnimatGUI.DataObjects.Physical.BodyPart)
        Public Event Sized(ByRef doStruct As AnimatGUI.DataObjects.Physical.PhysicalStructure)

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


        'All events coming up from the DataObjectInterface are actually coming from a different thread.
        'The one for the simulation. This means that we have to use BeginInvoke to recall a different 
        'method on the GUI thread or it will cause big problems. So all of these methods do that.

        Protected Overridable Sub PositionChangedHandler()
            Try
                If Not m_doInterface Is Nothing Then

                    m_svPosition.CopyData(m_doInterface.Position(0), m_doInterface.Position(1), m_doInterface.Position(2))
                    'm_svWorldPosition.CopyData(m_doInterface.WorldPosition(0), m_doInterface.WorldPosition(1), m_doInterface.WorldPosition(2))

                    RaiseMovedEvent()

                    Util.Application.ProjectProperties.RefreshProperties()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnPositionChanged()

            Try
                Util.Application.BeginInvoke(New PositionChangedDelegate(AddressOf Me.PositionChangedHandler), Nothing)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub RotationChangedHandler()
            Try
                If Not m_doInterface Is Nothing Then

                    m_svRotation.IgnoreChangeValueEvents = True
                    m_svRotation.X.ActualValue = Util.RadiansToDegrees(CSng(m_doInterface.Rotation(0)))
                    m_svRotation.Y.ActualValue = Util.RadiansToDegrees(CSng(m_doInterface.Rotation(1)))
                    m_svRotation.Z.ActualValue = Util.RadiansToDegrees(CSng(m_doInterface.Rotation(2)))
                    m_svRotation.IgnoreChangeValueEvents = False

                    RaiseRotatedEvent()

                    Util.Application.ProjectProperties.RefreshProperties()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnRotationChanged()

            Try
                Util.Application.BeginInvoke(New RotationChangedDelegate(AddressOf Me.RotationChangedHandler), Nothing)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub SelectionChangedHandler(ByVal bSelected As Boolean, ByVal bSelectMultiple As Boolean)
            Try
                If bSelected Then
                    Me.SelectItem(bSelectMultiple)
                Else
                    Me.DeselectItem()
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnSelectionChanged(ByVal bSelected As Boolean, ByVal bSelectMultiple As Boolean)
            Try
                Dim aryObjs(1) As Object
                aryObjs(0) = bSelected
                aryObjs(1) = bSelectMultiple
                Util.Application.BeginInvoke(New SelectionChangedDelegate(AddressOf Me.SelectionChangedHandler), aryObjs)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        ''' \brief  Raises the moved event.
        '''
        ''' \author dcofer
        ''' \date   5/9/2011
        Protected Sub RaiseMovedEvent()
            'If Not Me.RootBody Is Nothing Then
            '    Dim bpPart As BodyPart = DirectCast(Me.RootBody, BodyPart)
            '    RaiseEvent Moved(bpPart)
            'End If
        End Sub

        ''' \brief  Raises the rotated event.
        '''
        ''' \author dcofer
        ''' \date   5/9/2011
        Protected Sub RaiseRotatedEvent()
            'If Not Me.RootBody Is Nothing Then
            '    Dim bpPart As BodyPart = DirectCast(Me.RootBody, BodyPart)
            '    RaiseEvent Rotated(bpPart)
            'End If
        End Sub

        'These three events handlers are called whenever a user manually changes the value of the position or rotation.
        'This is different from the OnPositionChanged event. Those events come up from the simulation.
        Protected Overridable Sub OnPositionValueChanged()
            Try
                Me.SetSimData("Position", m_svPosition.GetSimulationXml("Position"), True)
                Util.ProjectProperties.RefreshProperties()
                RaiseMovedEvent()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnRotationValueChanged()
            Try
                Me.SetSimData("Rotation", Me.RadianRotation.GetSimulationXml("Rotation"), True)
                Util.ProjectProperties.RefreshProperties()
                RaiseRotatedEvent()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

