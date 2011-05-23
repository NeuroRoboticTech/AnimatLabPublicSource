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

Namespace DataObjects.Physical.Bodies

    Public MustInherit Class MuscleBase
        Inherits Physical.RigidBody
        Implements IMuscle

#Region " Attributes "

        Protected m_aryAttachmentPoints As Collections.Attachments
        Protected m_aryAttachmentPointIDs As ArrayList

        Protected m_snMaxTension As Framework.ScaledNumber
        Protected m_snMuscleLength As Framework.ScaledNumber

        Protected m_StimTension As DataObjects.Gains.MuscleGains.StimulusTension
        Protected m_LengthTension As DataObjects.Gains.MuscleGains.LengthTension

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.MuscleAttachment_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.MuscleAttachment_Button.gif"
            End Get
        End Property

        Public Overrides Property LocalPosition() As Framework.ScaledVector3
            Get
                Return MyBase.LocalPosition
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                'LocalPosition is never changed on a muscle. It is always 0,0,0
            End Set
        End Property

        Public Overrides Property WorldPosition() As Framework.ScaledVector3
            Get
                Return MyBase.WorldPosition
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                'WorldPosition is never changed on a muscle. It is always 0,0,0
            End Set
        End Property

        Public Overrides Property Rotation() As Framework.ScaledVector3
            Get
                Return MyBase.Rotation
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                'Rotation is never changed on an attachment. It is always 0,0,0
            End Set
        End Property

        Public Overrides ReadOnly Property CanBeRootBody() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property UsesAJoint() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property HasDynamics() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultAddGraphics() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overridable Property AttachmentPoints() As Collections.Attachments
            Get
                Return m_aryAttachmentPoints
            End Get
            Set(ByVal value As Collections.Attachments)
                If Not value Is Nothing Then
                    SetSimData("AttachmentPoints", value.GetSimulationXml("Attachments", Me.ParentStructure), True)

                    'Remove the event handlers for attached part moved or rotated for the current attachments.
                    RemoveMoveHandlers(m_aryAttachmentPoints)

                    m_aryAttachmentPoints = value

                    'add back the event handlers for attached part moved or rotated for the new attachments.
                    AddMoveHandlers(m_aryAttachmentPoints)

                    Util.ProjectWorkspace.RefreshProperties()
                End If
            End Set
        End Property

        Public Overridable Property AttachmentPointsProps() As PropertyBag
            Get
                Return m_aryAttachmentPoints.Properties
            End Get
            Set(ByVal value As PropertyBag)

            End Set
        End Property

        Public Overridable ReadOnly Property MuscleLength() As ScaledNumber
            Get
                If Not m_doInterface Is Nothing Then
                    Dim fltLength As Single = m_doInterface.GetDataValue("Length")
                    m_snMuscleLength.SetFromValue(fltLength)
                End If
                Return m_snMuscleLength
            End Get
        End Property

        Public Overridable Property MaxTension() As ScaledNumber
            Get
                Return m_snMaxTension
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 Then
                        Throw New System.Exception("The Max tension must be greater than zero.")
                    End If

                    SetSimData("MaxTension", value.ActualValue.ToString, True)
                    m_snMaxTension.CopyData(value)
                End If
            End Set
        End Property

        <Category("Stimulus-Tension"), Description("Sets the stmilus-tension properties of the muscle.")> _
        Public Overridable Property StimulusTension() As Gains.MuscleGains.StimulusTension Implements IMuscle.StimulusTension
            Get
                If m_StimTension Is Nothing Then
                    m_StimTension = New Gains.MuscleGains.StimulusTension(Me)
                End If

                Return m_StimTension
            End Get
            Set(ByVal value As Gains.MuscleGains.StimulusTension)
                If Not value Is Nothing Then
                    value.SetAllSimData(m_StimTension.SimInterface)
                    m_StimTension = value
                End If
            End Set
        End Property

        Public Property LengthTension() As Gains.MuscleGains.LengthTension Implements IMuscle.LengthTension
            Get
                If m_LengthTension Is Nothing Then
                    m_LengthTension = New Gains.MuscleGains.LengthTension(Me)
                End If

                Return m_LengthTension
            End Get
            Set(ByVal value As Gains.MuscleGains.LengthTension)
                If Not value Is Nothing Then
                    value.SetAllSimData(m_LengthTension.SimInterface)
                    m_LengthTension = value
                End If
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_bIsCollisionObject = False
            m_clDiffuse = Color.Blue

            m_snMaxTension = New ScaledNumber(Me, "MaxTension", 100, ScaledNumber.enumNumericScale.None, "Newtons", "N")
            m_snMuscleLength = New ScaledNumber(Me, "MuscleLength", 0, ScaledNumber.enumNumericScale.None, "Meters", "m")

            m_StimTension = New AnimatGUI.DataObjects.Gains.MuscleGains.StimulusTension(Me)
            m_LengthTension = New AnimatGUI.DataObjects.Gains.MuscleGains.LengthTension(Me)

            m_aryAttachmentPoints = New Collections.Attachments(Me)
            m_aryAttachmentPointIDs = New ArrayList()

        End Sub

        Public Overrides Sub InitAfterAppStart()
            AddCompatibleStimulusType("EnablerInput")
        End Sub

        Public Overrides Sub InitializeSimulationReferences()
            MyBase.InitializeSimulationReferences()

            m_StimTension.InitializeSimulationReferences()
            m_LengthTension.InitializeSimulationReferences()

            'Lets get a data pointer for the muscle length so we can use that to update our length variable.
            If Not m_doInterface Is Nothing Then
                m_doInterface.GetDataPointer("Length")
            End If
        End Sub

        Public Overrides Function AddChildBody(ByVal vPos As Framework.Vec3d, ByVal vNorm As Framework.Vec3d) As Boolean
            'If you try and add a body to a muscle attachment then you really need to add it to the attachment parent.
            If Not Me.Parent Is Nothing AndAlso Util.IsTypeOf(Me.Parent.GetType, GetType(RigidBody), True) Then
                Dim rbParent As RigidBody = DirectCast(Me.Parent, RigidBody)
                Return rbParent.AddChildBody(vPos, vNorm)
            End If

            Throw New System.Exception("You cannot add a child body to a muscle part type.")
        End Function

        Public Overrides Sub AddChildBody(ByVal rbChildBody As RigidBody, ByVal bAddDefaultGraphics As Boolean)
            'If you try and add a body to a muscle attachment then you really need to add it to the attachment parent.
            If Not Me.Parent Is Nothing AndAlso Util.IsTypeOf(Me.Parent.GetType, GetType(RigidBody), True) Then
                Dim rbParent As RigidBody = DirectCast(Me.Parent, RigidBody)
                rbParent.AddChildBody(rbChildBody, bAddDefaultGraphics)
                Return
            End If

            Throw New System.Exception("You cannot add a child body to a muscle part type.")
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrigBody As MuscleBase = DirectCast(doOriginal, MuscleBase)

            m_aryAttachmentPoints = DirectCast(doOrigBody.m_aryAttachmentPoints.Copy(), Collections.Attachments)
            m_bEnabled = doOrigBody.m_bEnabled
            m_StimTension = DirectCast(doOrigBody.m_StimTension.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gains.MuscleGains.StimulusTension)
            m_LengthTension = DirectCast(doOrigBody.m_LengthTension.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gains.MuscleGains.LengthTension)
            m_snMaxTension = DirectCast(doOrigBody.m_snMaxTension.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snMuscleLength = DirectCast(doOrigBody.m_snMuscleLength.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

        End Sub

        Public Overrides Sub AfterClone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doOriginal As Framework.DataObject, ByVal doClone As Framework.DataObject)
            MyBase.AfterClone(doParent, bCutData, doOriginal, doClone)

            If Util.IsTypeOf(doClone.GetType, GetType(Physical.BodyPart), False) Then
                'This is the new cloned root object, not the original root object
                Dim doPart As BodyPart = DirectCast(doClone, BodyPart)
                Dim aryAttach As Collections.Attachments = DirectCast(m_aryAttachmentPoints.Copy, Collections.Attachments)

                m_aryAttachmentPoints.Clear()
                For Each doAttach As Attachment In aryAttach
                    Dim doBase As BodyPart = doPart.FindBodyPartByCloneID(doAttach.ID)
                    If Not doBase Is Nothing AndAlso Util.IsTypeOf(doBase.GetType, GetType(Attachment), False) Then
                        m_aryAttachmentPoints.Add(DirectCast(doBase, Attachment))
                    Else
                        m_aryAttachmentPoints.Add(doAttach)
                    End If
                Next
            End If

        End Sub

        Public Overrides Function SwapBodyPartList() As Collections.BodyParts

            Dim aryList As Collections.BodyParts = New Collections.BodyParts(Nothing)

            For Each doPart As RigidBody In Util.Application.RigidBodyTypes
                If Util.IsTypeOf(doPart.GetType, GetType(MuscleBase), False) Then
                    aryList.Add(doPart)
                End If
            Next
            'aryList.Add(New spring(nothing) TODO !!!!

            Return aryList
        End Function

        Public Overrides Sub SwapBodyPartCopy(ByVal doOriginal As BodyPart)
            MyBase.SwapBodyPartCopy(doOriginal)

            Dim rbOrig As RigidBody = DirectCast(doOriginal, RigidBody)

            Me.Name = rbOrig.Name
            Me.ID = rbOrig.ID
            Me.Description = rbOrig.Description
            Me.Visible = rbOrig.Visible

            m_Transparencies = DirectCast(rbOrig.Transparencies.Clone(Me, False, Nothing), BodyTransparencies)
            m_clAmbient = rbOrig.Ambient
            m_clDiffuse = rbOrig.Diffuse
            m_clSpecular = rbOrig.Specular
            m_fltShininess = rbOrig.Shininess
            m_strTexture = rbOrig.Texture

            If Util.IsTypeOf(doOriginal.GetType, GetType(MuscleBase), False) Then
                Dim msOrig As MuscleBase = DirectCast(doOriginal, MuscleBase)

                m_aryAttachmentPoints.Clear()
                For Each doAttach As Attachment In msOrig.AttachmentPoints
                    Dim doBase As BodyPart = Me.ParentStructure.FindBodyPart(doAttach.ID, False)
                    If Not doBase Is Nothing Then
                        m_aryAttachmentPoints.Add(doAttach)
                    End If
                Next
                m_snMaxTension = msOrig.m_snMaxTension
                m_snMuscleLength = msOrig.m_snMuscleLength
                m_StimTension = msOrig.m_StimTension
                m_LengthTension = msOrig.m_LengthTension
                'TODO
                '			else if(Util.IsTypeOf(doOriginal.GetType(), typeof(VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring), false))
                '			{
                '				VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring msOrig = (VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring) doOriginal;

                '				m_aryAttachmentPoints.Clear();
                '				if(msOrig.PrimaryAttachment != null && msOrig.PrimaryAttachment.BodyPart != null)
                '					m_aryAttachmentPoints.Add((VortexAnimatTools.DataObjects.Physical.RigidBodies.MuscleAttachment) msOrig.PrimaryAttachment.BodyPart);

                '				if(msOrig.SecondaryAttachment != null && msOrig.SecondaryAttachment.BodyPart != null)
                '					m_aryAttachmentPoints.Add((VortexAnimatTools.DataObjects.Physical.RigidBodies.MuscleAttachment) msOrig.SecondaryAttachment.BodyPart);
                '			}
            End If
        End Sub

        Protected Sub AddMoveHandlers(ByVal aryAttachments As Collections.Attachments)

            For Each doAttach As Attachment In aryAttachments
                AddHandler doAttach.Moved, AddressOf Me.OnAttachmentMoved
                AddHandler doAttach.Rotated, AddressOf Me.OnAttachmentRotated
            Next

        End Sub

        Protected Sub RemoveMoveHandlers(ByVal aryAttachments As Collections.Attachments)

            For Each doAttach As Attachment In aryAttachments
                RemoveHandler doAttach.Moved, AddressOf Me.OnAttachmentMoved
                RemoveHandler doAttach.Rotated, AddressOf Me.OnAttachmentRotated
            Next

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strName.GetType(), "Name", _
                                        "Muscle Properties", "The name of this item.", m_strName))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Muscle Properties", "ID.", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Visible", m_bVisible.GetType(), "Visible", _
                                        "Visibility", "Sets whether or not this part is visible in the simulation.", m_bVisible))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Color", m_clDiffuse.GetType(), "Diffuse", _
                                        "Visibility", "Sets the color for this item.", m_clDiffuse))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Description", m_strDescription.GetType(), "Description", _
                                        "Muscle Properties", "Sets the description for this body part.", m_strDescription, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", m_bEnabled.GetType(), "Enabled", _
                                        "Muscle Properties", "Determines whether the muscle will actually develop tension.", m_bEnabled))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snMaxTension.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Maximum Tension", pbNumberBag.GetType(), "MaxTension", _
                                        "Muscle Properties", "A param that determines the maximum tension this muscle can possible generate.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMuscleLength.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Length", pbNumberBag.GetType(), "MuscleLength", _
                                        "Muscle Properties", "The current length of the muscle between the two attachment points.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), True))

            pbNumberBag = m_aryAttachmentPoints.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Attachments", pbNumberBag.GetType(), "AttachmentPointsProps", _
                                        "Muscle Properties", "The list of muscle attachment points.", pbNumberBag, _
                                        GetType(AnimatGUI.TypeHelpers.AttachmentsTypeEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))

            pbNumberBag = m_StimTension.Properties
            propTable.Properties.Add(New PropertySpec("Stimulus-Tension", pbNumberBag.GetType(), _
                          "StimulusTension", "Muscle Properties", "Sets the stmilus-tension properties of the muscle.", pbNumberBag, _
                        GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))

            pbNumberBag = m_LengthTension.Properties
            propTable.Properties.Add(New PropertySpec("Length-Tension", pbNumberBag.GetType(), _
                        "LengthTension", "Muscle Properties", "Sets the length-tension properties of the muscle.", pbNumberBag, _
                         GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snMaxTension Is Nothing Then m_snMaxTension.ClearIsDirty()
            If Not m_StimTension Is Nothing Then m_StimTension.ClearIsDirty()
            If Not m_LengthTension Is Nothing Then m_LengthTension.ClearIsDirty()
        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)

            oXml.IntoElem() 'Into RigidBody Element

            Me.ID = oXml.GetChildString("ID")
            Me.Name = oXml.GetChildString("Name", m_strID)

            Me.Description = oXml.GetChildString("Description", "")
            m_Transparencies.LoadData(oXml)
            m_bVisible = oXml.GetChildBool("IsVisible", m_bVisible)
            m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)

            m_clAmbient = Util.LoadColor(oXml, "Ambient", m_clAmbient)
            m_clDiffuse = Util.LoadColor(oXml, "Diffuse", m_clDiffuse)

            m_aryAttachmentPoints.LoadData(oXml, m_aryAttachmentPointIDs)

            m_StimTension.LoadData(oXml, "StimulusTension", "StimulusTension")
            m_LengthTension.LoadData(oXml, "LengthTension", "LengthTension")

            m_snMaxTension.LoadData(oXml, "MaximumTension")

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            Dim doAttach As DataObjects.Physical.Bodies.Attachment
            For Each strID As String In m_aryAttachmentPointIDs
                doAttach = DirectCast(Me.ParentStructure.FindBodyPart(strID, True), DataObjects.Physical.Bodies.Attachment)
                m_aryAttachmentPoints.Add(doAttach)
            Next

            AddMoveHandlers(m_aryAttachmentPoints)

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            oXml.AddChildElement(Me.BodyPartType)

            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Type", Me.Type)
            oXml.AddChildElement("PartType", Me.PartType.ToString())
            oXml.AddChildElement("Description", m_strDescription)

            m_Transparencies.SaveData(oXml)
            oXml.AddChildElement("IsVisible", m_bVisible)
            oXml.AddChildElement("Enabled", m_bEnabled)

            Util.SaveColor(oXml, "Ambient", m_clAmbient)
            Util.SaveColor(oXml, "Diffuse", m_clDiffuse)

            If Me.ModuleName.Length > 0 Then
                oXml.AddChildElement("ModuleName", Me.ModuleName)
            End If

            m_aryAttachmentPoints.SaveData(oXml, Me.ParentStructure)

            m_StimTension.SaveData(oXml, "StimulusTension")
            m_LengthTension.SaveData(oXml, "LengthTension")

            m_snMaxTension.SaveData(oXml, "MaximumTension")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            oXml.AddChildElement(Me.BodyPartType)

            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Type", Me.Type)
            oXml.AddChildElement("PartType", Me.PartType.ToString())
            oXml.AddChildElement("Description", m_strDescription)

            m_Transparencies.SaveSimulationXml(oXml, Me)
            oXml.AddChildElement("IsVisible", m_bVisible)
            oXml.AddChildElement("Enabled", m_bEnabled)

            Util.SaveColor(oXml, "Ambient", m_clAmbient)
            Util.SaveColor(oXml, "Diffuse", m_clDiffuse)

            If Me.ModuleName.Length > 0 Then
                oXml.AddChildElement("ModuleName", Me.ModuleName)
            End If

            m_aryAttachmentPoints.SaveSimulationXml(oXml, Me.ParentStructure)

            m_StimTension.SaveSimulationXml(oXml, Me, "StimulusTension")
            m_LengthTension.SaveSimulationXml(oXml, Me, "LengthTension")

            m_snMaxTension.SaveSimulationXml(oXml, Me, "MaximumTension")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

#End Region

#Region " Events "

        Protected Sub OnAttachmentMoved(ByRef bpPart As AnimatGUI.DataObjects.Physical.BodyPart)
            Try
                Me.SetSimData("AttachedPartMovedOrRotated", bpPart.ID, True)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnAttachmentRotated(ByRef bpPart As AnimatGUI.DataObjects.Physical.BodyPart)
            Try
                Me.SetSimData("AttachedPartMovedOrRotated", bpPart.ID, True)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class


End Namespace
