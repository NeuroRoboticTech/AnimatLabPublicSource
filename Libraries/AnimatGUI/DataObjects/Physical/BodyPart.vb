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

    Public MustInherit Class BodyPart
        Inherits DataObjects.DragObject

#Region " Delegates "

        Delegate Sub PositionChangedDelegate()
        Delegate Sub RotationChangedDelegate()
        Delegate Sub SelectionChangedDelegate(ByVal bSelected As Boolean, ByVal bSelectMultiple As Boolean)

#End Region

#Region " Attributes "

        Protected m_strDescription As String = ""
        Protected m_ButtonImage As System.Drawing.Image

        Protected m_svLocalPosition As ScaledVector3
        Protected m_svWorldPosition As ScaledVector3
        Protected m_svRotation As ScaledVector3

        Protected m_bVisible As Boolean = True
        Protected m_Transparencies As BodyTransparencies

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                If Value Is Nothing OrElse Value.Trim.Length = 0 Then
                    Throw New System.Exception("The name property can not be blank.")
                End If

                SetSimData("Name", Value, True)

                m_strName = Value

                If Not Me.WorkspaceNode Is Nothing Then
                    Me.WorkspaceNode.Text = m_strName
                End If
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

        'Type tells what type of bodypart (hinge, box, etc..
        Public MustOverride ReadOnly Property Type() As String

        'BodyPartType tells if it is a rigidbody or a joint.
        Public Overridable ReadOnly Property BodyPartType() As String
            Get
                Return Me.PartType.ToString
            End Get
        End Property

        'BodyPartName is a descriptive name used in the UI that tells the type of part.
        Public Overridable ReadOnly Property BodyPartName() As String
            Get
                Return Me.Type
            End Get
        End Property

        'AssemblyClass tells if it is a rigidbody or a joint.
        Public MustOverride ReadOnly Property PartType() As System.Type

        'This is the visual selection mode that is default for this object
        Public MustOverride ReadOnly Property DefaultVisualSelectionMode() As Simulation.enumVisualSelectionMode

        <Browsable(False)> _
        Public Overridable ReadOnly Property ParentStructure() As AnimatGUI.DataObjects.Physical.PhysicalStructure
            Get
                Dim doParent As AnimatGUI.DataObjects.Physical.PhysicalStructure
                Dim doTemp As AnimatGUI.DataObjects.Physical.BodyPart

                If Not Me.Parent Is Nothing AndAlso (TypeOf Me.Parent Is Physical.BodyPart OrElse TypeOf Me.Parent Is Physical.PhysicalStructure) Then
                    If TypeOf Me.Parent Is Physical.BodyPart Then
                        doTemp = DirectCast(Me.Parent, Physical.BodyPart)
                        Return doTemp.ParentStructure
                    ElseIf TypeOf Me.Parent Is Physical.PhysicalStructure Then
                        doParent = DirectCast(Me.Parent, Physical.PhysicalStructure)
                        Return doParent
                    End If
                End If

                Return Nothing
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return WorkspaceImageName()
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property ButtonImage() As System.Drawing.Image
            Get
                If m_ButtonImage Is Nothing AndAlso Me.ButtonImageName.Trim.Length > 0 Then
                    Dim myAssembly As System.Reflection.Assembly
                    myAssembly = System.Reflection.Assembly.Load(Me.AssemblyModuleName)
                    m_ButtonImage = ImageManager.LoadImage(myAssembly, Me.ButtonImageName)
                End If

                Return m_ButtonImage
            End Get
            Set(ByVal Value As System.Drawing.Image)
                If Not Value Is Nothing Then
                    m_ButtonImage = Value
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property ButtonImageName() As String
            Get
                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StructureID() As String
            Get
                If Not Me.ParentStructure Is Nothing Then
                    Return Me.ParentStructure.ID
                End If

                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return True
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property AllowUserAdd() As Boolean
            Get
                Return True
            End Get
        End Property

        'If this a rigid body then we do not want to allow the user to be able to change the position or orientation
        'of the body. They need to do this using the structure/organism.
        <Browsable(False)> _
        Public Overridable ReadOnly Property AllowGuiCoordinateChange() As Boolean
            Get
                Return True
            End Get
        End Property

#Region " Location Properties "

        Public Overridable Property LocalPosition() As Framework.ScaledVector3
            Get
                Return m_svLocalPosition
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("LocalPosition", value.GetSimulationXml("LocalPosition"), True)
                m_svLocalPosition.CopyData(value)
                RaiseEvent PartMoved(Me)
            End Set
        End Property

        Public Overridable Property WorldPosition() As Framework.ScaledVector3
            Get
                If Not m_doInterface Is Nothing Then
                    'We need to get the actual world location from the simulation interface before returning it.
                    m_svWorldPosition.CopyData(m_doInterface.WorldPosition(0), m_doInterface.WorldPosition(1), m_doInterface.WorldPosition(2))
                End If
                Return m_svWorldPosition
            End Get
            Set(ByVal value As Framework.ScaledVector3)

                If Not m_doParent Is Nothing AndAlso Util.IsTypeOf(m_doParent.GetType, GetType(DataObjects.Physical.BodyPart)) Then
                    Dim bpParent As DataObjects.Physical.BodyPart = DirectCast(m_doParent, DataObjects.Physical.BodyPart)

                    value.IgnoreChangeValueEvents = True
                    value.X.ActualValue = value.X.ActualValue - bpParent.WorldPosition.X.ActualValue
                    value.Y.ActualValue = value.Y.ActualValue - bpParent.WorldPosition.Y.ActualValue
                    value.Z.ActualValue = value.Z.ActualValue - bpParent.WorldPosition.Z.ActualValue
                    value.IgnoreChangeValueEvents = False

                    Me.SetSimData("LocalPosition", value.GetSimulationXml("LocalPosition"), True)
                    m_svLocalPosition.CopyData(value)

                    If Not m_doInterface Is Nothing Then
                        'We need to get the actual world location from the simulation interface before returning it.
                        m_svWorldPosition.CopyData(m_doInterface.WorldPosition(0), m_doInterface.WorldPosition(1), m_doInterface.WorldPosition(2))
                    End If
                End If
            End Set
        End Property

        Public Overridable Property Rotation() As Framework.ScaledVector3
            Get
                Return m_svRotation
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("Rotation", value.GetSimulationXml("Rotation"), True)
                m_svRotation.CopyData(value)
                RaiseEvent PartRotated(Me)
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

#End Region

        <Browsable(False)> _
        Public Overridable ReadOnly Property HasDynamics() As Boolean
            Get
                Return True
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""

            m_svLocalPosition = New ScaledVector3(Me, "LocalPosition", "Location of the object relative to its parent.", "Meters", "m")
            m_svWorldPosition = New ScaledVector3(Me, "WorldPosition", "Location of the object relative to the center of the world.", "Meters", "m")
            m_svRotation = New ScaledVector3(Me, "Rotation", "Rotation of the object.", "Degrees", "Deg")
            m_Transparencies = New BodyTransparencies(Me)

            AddHandler m_svLocalPosition.ValueChanged, AddressOf Me.OnLocalPositionValueChanged
            AddHandler m_svWorldPosition.ValueChanged, AddressOf Me.OnWorldPositionValueChanged
            AddHandler m_svRotation.ValueChanged, AddressOf Me.OnRotationValueChanged

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_thDataTypes Is Nothing Then m_thDataTypes.ClearIsDirty()
            If Not m_svLocalPosition Is Nothing Then m_svLocalPosition.ClearIsDirty()
            If Not m_svWorldPosition Is Nothing Then m_svWorldPosition.ClearIsDirty()
            If Not m_svRotation Is Nothing Then m_svRotation.ClearIsDirty()
            If Not m_Transparencies Is Nothing Then m_Transparencies.ClearIsDirty()
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bpOrig As BodyPart = DirectCast(doOriginal, BodyPart)

            m_svLocalPosition = DirectCast(bpOrig.m_svLocalPosition.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)
            m_svWorldPosition = DirectCast(bpOrig.m_svWorldPosition.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)
            m_svRotation = DirectCast(bpOrig.m_svRotation.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)
            m_Transparencies = DirectCast(bpOrig.m_Transparencies.Clone(Me, bCutData, doRoot), BodyTransparencies)
            m_bVisible = bpOrig.m_bVisible

            AddHandler m_svLocalPosition.ValueChanged, AddressOf Me.OnLocalPositionValueChanged
            AddHandler m_svWorldPosition.ValueChanged, AddressOf Me.OnWorldPositionValueChanged
            AddHandler m_svRotation.ValueChanged, AddressOf Me.OnRotationValueChanged

        End Sub

        <Browsable(False)> _
        Public Overridable Property Description() As String
            Get
                Return m_strDescription
            End Get
            Set(ByVal Value As String)
                m_strDescription = Value
            End Set
        End Property

        Public Overridable Function FindBodyPart(ByVal strID As String) As BodyPart
            Return Nothing
        End Function

        Public Overridable Function FindBodyPartByName(ByVal strName As String) As BodyPart
            Return Nothing
        End Function

        Public Overridable Function FindBodyPartByCloneID(ByVal strId As String) As BodyPart
            Return Nothing
        End Function

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.DragObject

            Dim oStructure As Object = Util.Environment.FindStructureFromAll(strStructureName, bThrowError)
            If oStructure Is Nothing Then Return Nothing

            Dim doStructure As AnimatGUI.DataObjects.Physical.PhysicalStructure = DirectCast(oStructure, AnimatGUI.DataObjects.Physical.PhysicalStructure)
            Dim doPart As AnimatGUI.DataObjects.Physical.BodyPart = Nothing

            If Not doStructure Is Nothing Then
                doPart = doStructure.FindBodyPart(strDataItemID, False)
                If doPart Is Nothing AndAlso bThrowError Then
                    Throw New System.Exception("The drag object with id '" & strDataItemID & "' was not found.")
                End If
            End If

            Return doPart

        End Function

        Public Overridable Function CreateJointTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, _
                                                        ByVal thSelectedPart As TypeHelpers.LinkedBodyPart) As Crownwood.DotNetMagic.Controls.Node
            Return Nothing
        End Function

        Public Overridable Function CreateRigidBodyTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, _
                                                           ByVal thSelectedPart As TypeHelpers.LinkedBodyPart) As Crownwood.DotNetMagic.Controls.Node
            Return Nothing
        End Function

        Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean

            If bAskToDelete Then
                If MessageBox.Show("Are you certain that you want to permanently delete this " & _
                                    "body part and all its children?", "Delete Body Part", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                    Return True
                End If
            End If

            If Not Me.ParentStructure Is Nothing Then
                Me.ParentStructure.DeleteBodyPart(Me)
                If Not Me.ParentStructure Is Nothing Then
                    Me.ParentStructure.SelectItem()
                End If
            End If

            Return False
        End Function

        Public Overridable Sub CopyBodyPart(ByVal bCutData As Boolean)

            Dim doClone As AnimatGUI.DataObjects.Physical.RigidBody = DirectCast(Me.Clone(Me.Parent, bCutData, Me), AnimatGUI.DataObjects.Physical.RigidBody)

            'Now lets save the xml for this cloned object.
            Dim oXml As New AnimatGUI.Interfaces.StdXml

            oXml.AddElement("CopyData")

            doClone.SaveData(Me.ParentStructure, oXml)

            Dim strXml As String = oXml.Serialize()

            Dim data As New System.Windows.Forms.DataObject
            data.SetData("AnimatLab.Body.XMLFormat", strXml)
            Clipboard.SetDataObject(data, True)

        End Sub

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Physical.BodyPart.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)

                If Me.AllowStimulus AndAlso Me.CompatibleStimuli.Count > 0 Then
                    ' Create the menu items
                    Dim mcAddStimulus As New System.Windows.Forms.ToolStripMenuItem("Add Stimulus", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddStimulus.gif"), New EventHandler(AddressOf Me.OnAddStimulus))
                    popup.Items.Add(mcAddStimulus)
                End If

                Dim mcSwapPart As New System.Windows.Forms.ToolStripMenuItem("Swap Part", Util.Application.ToolStripImages.GetImage("AnimatGUI.Swap.gif"), New EventHandler(AddressOf Me.OnSwapBodyPart))
                popup.Items.Add(mcSwapPart)

                Dim mcCut As New System.Windows.Forms.ToolStripMenuItem("Cut", Util.Application.ToolStripImages.GetImage("AnimatGUI.Cut.gif"), New EventHandler(AddressOf Me.OnCutBodyPart))
                Dim mcCopy As New System.Windows.Forms.ToolStripMenuItem("Copy", Util.Application.ToolStripImages.GetImage("AnimatGUI.Copy.gif"), New EventHandler(AddressOf Me.OnCopyBodyPart))
                Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Chart", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcCut, mcCopy, mcDelete})

                If Not Me.ParentStructure Is Nothing AndAlso Not Me.ParentStructure.BodyEditor Is Nothing Then
                    Dim mcRelabel As New System.Windows.Forms.ToolStripMenuItem("Relabel Children", Util.Application.ToolStripImages.GetImage("AnimatGUI.Relabel.gif"), New EventHandler(AddressOf Me.OnRelabelChildren))
                    popup.Items.Add(mcRelabel)
                End If

                Dim mcSepExpand As New ToolStripSeparator()
                Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
                Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

                mcExpandAll.Tag = tnSelectedNode
                mcCollapseAll.Tag = tnSelectedNode

                ' Create the popup menu object
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcSepExpand, mcExpandAll, mcCollapseAll})

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                Return True
            End If

            Return False
        End Function

        Public Overrides Sub SelectItem(Optional ByVal bSelectMultiple As Boolean = False)
            MyBase.SelectItem(bSelectMultiple)
            Util.Simulation.VisualSelectionMode = Me.DefaultVisualSelectionMode
        End Sub

        Public Overrides Sub SelectStimulusType()
            Dim frmStimulusType As New Forms.ExternalStimuli.SelectStimulusType
            frmStimulusType.CompatibleStimuli = Me.CompatibleStimuli

            If frmStimulusType.ShowDialog(Util.Application) = DialogResult.OK Then
                Dim doStimulus As DataObjects.ExternalStimuli.BodyPartStimulus = DirectCast(frmStimulusType.SelectedStimulus.Clone(Util.Application.FormHelper, False, Nothing), DataObjects.ExternalStimuli.BodyPartStimulus)
                doStimulus.PhysicalStructure = Me.ParentStructure
                doStimulus.BodyPart = Me

                Util.Simulation.NewStimuliIndex = Util.Simulation.NewStimuliIndex + 1
                doStimulus.Name = "Stimulus_" & Util.Simulation.NewStimuliIndex

                Util.Simulation.ProjectStimuli.Add(doStimulus.ID, doStimulus)
            End If
        End Sub

        Public MustOverride Sub RenameBodyParts(ByVal doStructure As AnimatGUI.DataObjects.Physical.PhysicalStructure)
        Public MustOverride Sub ClearSelectedBodyParts()

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strID.GetType(), "Name", _
                                        "Part Properties", "The name of this item.", m_strName))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Part Properties", "ID", Me.ID, True))

            Me.LocalPosition.PropertiesReadOnly = Not AllowGuiCoordinateChange
            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = Me.LocalPosition.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Local Position", pbNumberBag.GetType(), "LocalPosition", _
                                        "Coordinates", "Sets the local location of this body part.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter), Not AllowGuiCoordinateChange()))

            Me.WorldPosition.PropertiesReadOnly = Not AllowGuiCoordinateChange
            pbNumberBag = Me.WorldPosition.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("World Position", pbNumberBag.GetType(), "WorldPosition", _
                                        "Coordinates", "Sets the world location of this body part.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter), Not AllowGuiCoordinateChange()))

            Me.Rotation.PropertiesReadOnly = Not AllowGuiCoordinateChange
            pbNumberBag = Me.Rotation.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Rotation", pbNumberBag.GetType(), "Rotation", _
                                        "Coordinates", "Sets the rotation of this body part.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter), Not AllowGuiCoordinateChange()))


            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Visible", m_bVisible.GetType(), "Visible", _
                                        "Visibility", "Sets whether or not this part is visible in the simulation.", m_bVisible))

            pbNumberBag = Me.Transparencies.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Transparencies", pbNumberBag.GetType(), "Transparencies", _
                                        "Visibility", "Sets the transparencies for this part in the various selection modes.", pbNumberBag, _
                                        "", GetType(BodyTransparencies.BodyTransparencyPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Description", m_strDescription.GetType(), "Description", _
                                        "Part Properties", "Sets the description for this body part.", m_strDescription, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))
        End Sub

        Public Overridable Overloads Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)

            oXml.IntoElem() 'Into BodyPart Element
            m_strID = oXml.GetChildString("ID")
            m_strName = oXml.GetChildString("Name", m_strID)

            If m_strID.Trim.Length = 0 Then
                m_strID = System.Guid.NewGuid().ToString()
            End If

            If m_strName.Trim.Length = 0 Then
                m_strName = m_strID
            End If

            m_strDescription = oXml.GetChildString("Description", "")
            m_Transparencies.LoadData(oXml)
            m_bVisible = oXml.GetChildBool("IsVisible", m_bVisible)

            m_svLocalPosition.LoadData(oXml, "LocalPosition")
            m_svRotation.LoadData(oXml, "Rotation")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overridable Overloads Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement(Me.BodyPartType)

            oXml.IntoElem() 'Into Child Elemement
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Type", Me.Type)
            oXml.AddChildElement("PartType", Me.PartType.ToString)
            oXml.AddChildElement("Description", m_strDescription)
            m_Transparencies.SaveData(oXml)
            oXml.AddChildElement("IsVisible", m_bVisible)

            If Me.ModuleName.Length > 0 Then
                oXml.AddChildElement("ModuleName", Me.ModuleName)
            End If

            m_svLocalPosition.SaveData(oXml, "LocalPosition")
            m_svRotation.SaveData(oXml, "Rotation")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement(Me.BodyPartType)

            oXml.IntoElem() 'Into Child Elemement
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Type", Me.Type)
            oXml.AddChildElement("PartType", Me.PartType.ToString)
            oXml.AddChildElement("Description", m_strDescription)
            m_Transparencies.SaveSimulationXml(oXml, Me)
            oXml.AddChildElement("IsVisible", m_bVisible)

            If Me.ModuleName.Length > 0 Then
                oXml.AddChildElement("ModuleName", Me.ModuleName)
            End If

            m_svLocalPosition.SaveSimulationXml(oXml, Me, "LocalPosition")
            Me.RadianRotation.SaveSimulationXml(oXml, Me, "Rotation")

            oXml.OutOfElem() 'Outof BodyPart Element
        End Sub

        Public Overrides Function ToString() As String
            Return Me.Name
        End Function

        Public Overrides Sub EnsureFormActive()
            If Not Me.ParentStructure Is Nothing AndAlso Me.ParentStructure.BodyEditor Is Nothing Then
                'Return Util.Application.EditBodyPlan(Me.ParentStructure)
            End If
        End Sub

        Public Overridable Sub BeforeAddBody()
        End Sub

        Public Overridable Sub AfterAddBody()
            'The newly added part may be a copy of an entire heirarchy of parts. If htis is the
            'case then it may have things in it like springs and muscles that will need to be re-initialized.
            Me.InitializeAfterLoad()
        End Sub

        Public MustOverride Function SwapBodyPartList() As AnimatGUI.Collections.BodyParts
        Public MustOverride Sub SwapBodyPartCopy(ByVal doOriginal As AnimatGUI.DataObjects.Physical.BodyPart)

        Public Overrides Sub InitializeSimulationReferences()
            If m_doInterface Is Nothing AndAlso Not Util.Application.SimulationInterface Is Nothing AndAlso Util.Application.SimulationInterface.SimOpen Then
                m_doInterface = New Interfaces.DataObjectInterface(Util.Application.SimulationInterface, Me.ID)
                AddHandler m_doInterface.OnPositionChanged, AddressOf Me.OnPositionChanged
                AddHandler m_doInterface.OnRotationChanged, AddressOf Me.OnRotationChanged
                AddHandler m_doInterface.OnSelectionChanged, AddressOf Me.OnSelectionChanged
            End If
        End Sub

#End Region

#Region " Events "

        Public Event PartMoved(ByRef bpPart As AnimatGUI.DataObjects.Physical.BodyPart)
        Public Event PartRotated(ByRef bpPart As AnimatGUI.DataObjects.Physical.BodyPart)
        Public Event PartSized(ByRef bpPart As AnimatGUI.DataObjects.Physical.BodyPart)

        Protected Overridable Sub OnAddStimulus(ByVal sender As Object, ByVal e As System.EventArgs)

            Try

                Me.SelectStimulusType()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        'Protected Overridable Sub OnAddBodyPart(ByVal sender As Object, ByVal e As System.EventArgs)
        'End Sub

        Protected Overridable Sub OnRelabelChildren(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Dim frmRelabel As New AnimatGUI.Forms.BodyPlan.Relabel

                frmRelabel.PhysicalStructure = Me.ParentStructure
                frmRelabel.RootNode = Me
                If frmRelabel.ShowDialog = DialogResult.OK Then
                    Util.Relable(frmRelabel.Items, frmRelabel.txtMatch.Text, frmRelabel.txtReplace.Text)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub OnSwapBodyPart(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                'Me.ParentStructure.BodyEditor.BodyView.SwapBodyPart(Me)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub OnCopyBodyPart(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Me.CopyBodyPart(False)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub OnCutBodyPart(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Me.Delete(False)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        'These three events handlers are called whenever a user manually changes the value of the position or rotation.
        'This is different from the OnPositionChanged event. Those events come up from the simulation.
        Protected Overridable Sub OnLocalPositionValueChanged()
            Try
                Me.SetSimData("LocalPosition", m_svLocalPosition.GetSimulationXml("LocalPosition"), True)
                Util.ProjectProperties.RefreshProperties()
                RaiseEvent PartMoved(Me)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnWorldPositionValueChanged()
            Try
                If Not m_doParent Is Nothing AndAlso Util.IsTypeOf(m_doParent.GetType, GetType(DataObjects.Physical.BodyPart)) Then
                    Dim bpParent As DataObjects.Physical.BodyPart = DirectCast(m_doParent, DataObjects.Physical.BodyPart)

                    m_svLocalPosition.IgnoreChangeValueEvents = True
                    m_svLocalPosition.X.ActualValue = m_svWorldPosition.X.ActualValue - bpParent.WorldPosition.X.ActualValue
                    m_svLocalPosition.Y.ActualValue = m_svWorldPosition.Y.ActualValue - bpParent.WorldPosition.Y.ActualValue
                    m_svLocalPosition.Z.ActualValue = m_svWorldPosition.Z.ActualValue - bpParent.WorldPosition.Z.ActualValue
                    m_svLocalPosition.IgnoreChangeValueEvents = False

                    Me.SetSimData("LocalPosition", m_svLocalPosition.GetSimulationXml("LocalPosition"), True)
                    Util.ProjectProperties.RefreshProperties()
                    RaiseEvent PartMoved(Me)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnRotationValueChanged()
            Try
                Me.SetSimData("Rotation", Me.RadianRotation.GetSimulationXml("Rotation"), True)
                Util.ProjectProperties.RefreshProperties()
                RaiseEvent PartRotated(Me)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#Region " DataObjectInterface Events "

        'All events coming up from the DataObjectInterface are actually coming from a different thread.
        'The one for the simulation. This means that we have to use BeginInvoke to recall a different 
        'method on the GUI thread or it will cause big problems. So all of these methods do that.

        Protected Overridable Sub PositionChangedHandler()
            Try
                If Not m_doInterface Is Nothing Then

                    m_svLocalPosition.CopyData(m_doInterface.LocalPosition(0), m_doInterface.LocalPosition(1), m_doInterface.LocalPosition(2))
                    m_svWorldPosition.CopyData(m_doInterface.WorldPosition(0), m_doInterface.WorldPosition(1), m_doInterface.WorldPosition(2))

                    RaiseEvent PartMoved(Me)

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

                    RaiseEvent PartRotated(Me)

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

#End Region

#End Region

    End Class

End Namespace
