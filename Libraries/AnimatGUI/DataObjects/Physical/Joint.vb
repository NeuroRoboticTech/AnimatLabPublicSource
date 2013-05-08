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

    Public MustInherit Class Joint
        Inherits Physical.BodyPart

#Region " Attributes "

        Protected m_snSize As AnimatGUI.Framework.ScaledNumber
        Protected m_bEnableLimts As Boolean = True

        Protected m_doPrimaryAxisDisplacementRelaxation As ConstraintRelaxation
        Protected m_doSecondaryAxisDisplacement As ConstraintRelaxation
        Protected m_doThirdAxisDisplacement As ConstraintRelaxation
        Protected m_doSecondaryAxisRotation As ConstraintRelaxation
        Protected m_doThirdAxisRotation As ConstraintRelaxation

        Protected m_doFriction As ConstraintFriction

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property BodyPartType() As String
            Get
                Return "Joint"
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultVisualSelectionMode() As Simulation.enumVisualSelectionMode
            Get
                Return Simulation.enumVisualSelectionMode.SelectJoints
            End Get
        End Property

        Public Overridable ReadOnly Property UsesRadians() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overridable ReadOnly Property ScaleUnits() As String
            Get
                If Me.UsesRadians Then
                    Return "rad/s"
                Else
                    Return "m/s"
                End If
            End Get
        End Property

        Public Overridable ReadOnly Property InputStimulus() As String
            Get
                Return "Velocity"
            End Get
        End Property

        Public Overridable Property Size() As ScaledNumber
            Get
                Return m_snSize
            End Get
            Set(ByVal value As ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The size cannot be less than or equal to zero.")
                End If

                SetSimData("Size", value.ActualValue.ToString, True)
                m_snSize.CopyData(value)
            End Set
        End Property

        Public Overridable Property EnableLimits() As Boolean
            Get
                Return m_bEnableLimts
            End Get
            Set(ByVal value As Boolean)
                SetSimData("EnableLimits", value.ToString, True)
                m_bEnableLimts = value
            End Set
        End Property

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Joint"
            End Get
        End Property

        Public Overridable ReadOnly Property AllowAddChildBody() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overridable Property PrimaryAxisDisplacementRelaxation() As ConstraintRelaxation
            Get
                Return m_doPrimaryAxisDisplacementRelaxation
            End Get
            Set(ByVal value As ConstraintRelaxation)
                If Not value Is Nothing Then
                    SetSimData("PrimaryAxisDisplacementRelaxation", value.GetSimulationXml("ConstraintRelaxation", Me), True)
                End If
                m_doPrimaryAxisDisplacementRelaxation = value
            End Set
        End Property

        Public Overridable Property SecondaryAxisDisplacementRelaxation() As ConstraintRelaxation
            Get
                Return m_doSecondaryAxisDisplacement
            End Get
            Set(ByVal value As ConstraintRelaxation)
                If Not value Is Nothing Then
                    SetSimData("SecondaryAxisDisplacement", value.GetSimulationXml("ConstraintRelaxation", Me), True)
                End If
                m_doSecondaryAxisDisplacement = value
            End Set
        End Property

        Public Overridable Property ThirdAxisDisplacementRelaxation() As ConstraintRelaxation
            Get
                Return m_doThirdAxisDisplacement
            End Get
            Set(ByVal value As ConstraintRelaxation)
                If Not value Is Nothing Then
                    SetSimData("ThirdAxisDisplacement", value.GetSimulationXml("ConstraintRelaxation", Me), True)
                End If
                m_doThirdAxisDisplacement = value
            End Set
        End Property

        Public Overridable Property SecondaryAxisRotationRelaxation() As ConstraintRelaxation
            Get
                Return m_doSecondaryAxisRotation
            End Get
            Set(ByVal value As ConstraintRelaxation)
                If Not value Is Nothing Then
                    SetSimData("SecondaryAxisRotation", value.GetSimulationXml("ConstraintRelaxation", Me), True)
                End If
                m_doSecondaryAxisRotation = value
            End Set
        End Property

        Public Overridable Property ThirdAxisRotationRelaxation() As ConstraintRelaxation
            Get
                Return m_doThirdAxisRotation
            End Get
            Set(ByVal value As ConstraintRelaxation)
                If Not value Is Nothing Then
                    SetSimData("ThirdAxisRotation", value.GetSimulationXml("ConstraintRelaxation", Me), True)
                End If
                m_doThirdAxisRotation = value
            End Set
        End Property

        Public Overridable Property Friction() As ConstraintFriction
            Get
                Return m_doFriction
            End Get
            Set(ByVal value As ConstraintFriction)
                If Not value Is Nothing Then
                    SetSimData("Friction", value.GetSimulationXml("Friction", Me), True)
                End If
                m_doFriction = value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_thIncomingDataType = New AnimatGUI.DataObjects.DataType("DesiredVelocity", "Desired Velocity", "m/s", "m/s", -5, 5, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None)

            m_snSize = New AnimatGUI.Framework.ScaledNumber(Me, "Size", 2, AnimatGUI.Framework.ScaledNumber.enumNumericScale.centi, "Meters", "m")

            'Now we need to setup handlers for the parent and child move and rotate events so we can tell the joint object 
            'in the simulation that these changes have occured. It will need to re-adjust the joint to take this into account.
            If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType, GetType(DataObjects.Physical.BodyPart)) Then
                Dim bpChild As DataObjects.Physical.BodyPart = DirectCast(doParent, DataObjects.Physical.BodyPart)
                AddHandler bpChild.Moved, AddressOf Me.OnAttachedPartMoved
                AddHandler bpChild.Rotated, AddressOf Me.OnAttachedPartRotated

                If Not bpChild.Parent Is Nothing AndAlso Util.IsTypeOf(bpChild.Parent.GetType, GetType(DataObjects.Physical.RigidBody)) Then
                    Dim bpParent As DataObjects.Physical.RigidBody = DirectCast(bpChild.Parent, DataObjects.Physical.RigidBody)
                    AddHandler bpParent.Moved, AddressOf Me.OnAttachedPartMoved
                    AddHandler bpParent.Rotated, AddressOf Me.OnAttachedPartRotated
                End If
            End If

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snSize Is Nothing Then m_snSize.ClearIsDirty()
            If Not m_doPrimaryAxisDisplacementRelaxation Is Nothing Then m_doPrimaryAxisDisplacementRelaxation.ClearIsDirty()
            If Not m_doSecondaryAxisDisplacement Is Nothing Then m_doSecondaryAxisDisplacement.ClearIsDirty()
            If Not m_doThirdAxisDisplacement Is Nothing Then m_doThirdAxisDisplacement.ClearIsDirty()
            If Not m_doSecondaryAxisRotation Is Nothing Then m_doSecondaryAxisRotation.ClearIsDirty()
            If Not m_doThirdAxisRotation Is Nothing Then m_doThirdAxisRotation.ClearIsDirty()
            If Not m_doFriction Is Nothing Then m_doFriction.ClearIsDirty()

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrigJoint As Joint = DirectCast(doOriginal, Joint)
            m_snSize = DirectCast(doOrigJoint.m_snSize.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_bEnableLimts = doOrigJoint.m_bEnableLimts

            If Not doOrigJoint.m_doPrimaryAxisDisplacementRelaxation Is Nothing Then
                m_doPrimaryAxisDisplacementRelaxation = DirectCast(doOrigJoint.m_doPrimaryAxisDisplacementRelaxation.Clone(Me, bCutData, doRoot), ConstraintRelaxation)
            Else
                m_doPrimaryAxisDisplacementRelaxation = Nothing
            End If

            If Not doOrigJoint.m_doSecondaryAxisDisplacement Is Nothing Then
                m_doSecondaryAxisDisplacement = DirectCast(doOrigJoint.m_doSecondaryAxisDisplacement.Clone(Me, bCutData, doRoot), ConstraintRelaxation)
            Else
                m_doSecondaryAxisDisplacement = Nothing
            End If

            If Not doOrigJoint.m_doThirdAxisDisplacement Is Nothing Then
                m_doThirdAxisDisplacement = DirectCast(doOrigJoint.m_doThirdAxisDisplacement.Clone(Me, bCutData, doRoot), ConstraintRelaxation)
            Else
                m_doThirdAxisDisplacement = Nothing
            End If

            If Not doOrigJoint.m_doSecondaryAxisRotation Is Nothing Then
                m_doSecondaryAxisRotation = DirectCast(doOrigJoint.m_doSecondaryAxisRotation.Clone(Me, bCutData, doRoot), ConstraintRelaxation)
            Else
                m_doSecondaryAxisRotation = Nothing
            End If

            If Not doOrigJoint.m_doThirdAxisRotation Is Nothing Then
                m_doThirdAxisRotation = DirectCast(doOrigJoint.m_doThirdAxisRotation.Clone(Me, bCutData, doRoot), ConstraintRelaxation)
            Else
                m_doThirdAxisRotation = Nothing
            End If

            If Not m_doFriction Is Nothing Then m_doFriction = DirectCast(doOrigJoint.m_doFriction.Clone(Me, bCutData, doRoot), ConstraintFriction)

        End Sub

        Public Overrides Function CreateJointTreeView(ByRef tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, _
                                                      ByVal thSelectedPart As TypeHelpers.LinkedBodyPart) As Crownwood.DotNetMagic.Controls.Node

            Dim tnJoint As New Crownwood.DotNetMagic.Controls.Node(Me.Name)
            tnParent.Nodes.Add(tnJoint)
            tnJoint.ForeColor = Color.Red
            Dim thPart As TypeHelpers.LinkedBodyPart = DirectCast(thSelectedPart.Clone(thSelectedPart.Parent, False, Nothing), TypeHelpers.LinkedBodyPart)
            thPart.BodyPart = Me
            tnJoint.Tag = thPart

            Return tnJoint
        End Function

        Public Overrides Sub SetupInitialTransparencies()
            If Not m_Transparencies Is Nothing Then
                m_Transparencies.GraphicsTransparency = 50
                m_Transparencies.CollisionsTransparency = 50
                m_Transparencies.JointsTransparency = 0
                m_Transparencies.ReceptiveFieldsTransparency = 50
                m_Transparencies.SimulationTransparency = 50
            End If
        End Sub

        Public Overrides Function FindBodyPart(ByVal strID As String) As BodyPart

            If Me.ID = strID Then
                Return Me
            Else
                Return Nothing
            End If

        End Function

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            m_snSize.ActualValue = 0.2 * Util.Environment.DistanceUnitValue
        End Sub

        Public Overrides Function FindBodyPartByName(ByVal strName As String) As BodyPart

            If Me.Name = strName Then
                Return Me
            Else
                Return Nothing
            End If

        End Function

        Public Overrides Function FindBodyPartByCloneID(ByVal strId As String) As BodyPart

            If Me.CloneID = strId Then
                Return Me
            Else
                Return Nothing
            End If

        End Function

        Public Overrides Sub RenameBodyParts(ByVal doStructure As AnimatGUI.DataObjects.Physical.PhysicalStructure)

            Me.m_strID = System.Guid.NewGuid.ToString()

            Try
                doStructure.NewJointIndex = doStructure.NewJointIndex + 1
                Me.Name = "Joint_" & doStructure.NewJointIndex
            Catch ex As System.Exception
                Me.Name = "Joint_" & System.Guid.NewGuid.ToString()
            End Try
        End Sub

        Public Overrides Function SwapBodyPartList() As AnimatGUI.Collections.BodyParts

            'Go through the list and only use body parts that allow dynamics
            Dim aryPartList As New AnimatGUI.Collections.BodyParts(Nothing)
            For Each doPart As DataObjects.Physical.BodyPart In Util.Application.JointTypes
                If doPart.HasDynamics Then
                    aryPartList.Add(doPart)
                End If
            Next

            Return aryPartList
        End Function

        Public Overrides Sub SwapBodyPartCopy(ByVal doOriginal As AnimatGUI.DataObjects.Physical.BodyPart)

            Dim doExisting As AnimatGUI.DataObjects.Physical.Joint = DirectCast(doOriginal, AnimatGUI.DataObjects.Physical.Joint)

            Me.Name = doExisting.Name
            Me.ID = doExisting.ID
            Me.Description = doExisting.Description
            Me.EnableLimits = doExisting.m_bEnableLimts
            m_tnWorkspaceNode = doOriginal.WorkspaceNode

            Util.Application.WorkspaceImages.AddImage(Me.WorkspaceImageName)

            m_tnWorkspaceNode.Image = Util.Application.WorkspaceImages.GetImage(Me.WorkspaceImageName)
            m_tnWorkspaceNode.Tag = Me

        End Sub

        Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doPrimaryAxisDisplacementRelaxation Is Nothing Then doObject = m_doPrimaryAxisDisplacementRelaxation.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doSecondaryAxisDisplacement Is Nothing Then doObject = m_doSecondaryAxisDisplacement.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doThirdAxisDisplacement Is Nothing Then doObject = m_doThirdAxisDisplacement.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doSecondaryAxisRotation Is Nothing Then doObject = m_doSecondaryAxisRotation.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doThirdAxisRotation Is Nothing Then doObject = m_doThirdAxisRotation.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doFriction Is Nothing Then doObject = m_doFriction.FindObjectByID(strID)

            Return doObject

        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snSize.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Size", pbNumberBag.GetType(), "Size", _
                                        "Part Properties", "Sets the overall size of the part.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enable Limits", m_bEnabled.GetType(), "EnableLimits", _
                                        "Constraints", "Enables or disables this joint limit constraints.", m_bEnabled))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Joint Type", Me.Type.GetType(), "Type", _
                                        "Part Properties", "Type of joint.", Me.Type, True))

            If Not m_doPrimaryAxisDisplacementRelaxation Is Nothing Then
                pbNumberBag = m_doPrimaryAxisDisplacementRelaxation.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Primary Displacement Axis", pbNumberBag.GetType(), "PrimaryAxisDisplacementRelaxation", _
                                            "Relaxation Properties", "Sets the relaxation for the primary displacement axis.", pbNumberBag, _
                                            "", GetType(ConstraintRelaxationPropBagConverter)))
            End If

            If Not m_doSecondaryAxisDisplacement Is Nothing Then
                pbNumberBag = m_doSecondaryAxisDisplacement.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Secondary Displacement Axis", pbNumberBag.GetType(), "SecondaryAxisDisplacementRelaxation", _
                                            "Relaxation Properties", "Sets the relaxation for the secondary displacement axis.", pbNumberBag, _
                                            "", GetType(ConstraintRelaxationPropBagConverter)))
            End If

            If Not m_doThirdAxisDisplacement Is Nothing Then
                pbNumberBag = m_doThirdAxisDisplacement.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Third Displacement Axis", pbNumberBag.GetType(), "ThirdAxisDisplacementRelaxation", _
                                            "Relaxation Properties", "Sets the relaxation for the third displacement axis.", pbNumberBag, _
                                            "", GetType(ConstraintRelaxationPropBagConverter)))
            End If

            If Not m_doSecondaryAxisRotation Is Nothing Then
                pbNumberBag = m_doSecondaryAxisRotation.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Secondary Rotation Axis", pbNumberBag.GetType(), "SecondaryAxisRotationRelaxation", _
                                            "Relaxation Properties", "Sets the relaxation for the secondary rotation axis.", pbNumberBag, _
                                            "", GetType(ConstraintRelaxationPropBagConverter)))
            End If

            If Not m_doThirdAxisRotation Is Nothing Then
                pbNumberBag = m_doThirdAxisRotation.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Third Rotation Axis", pbNumberBag.GetType(), "ThirdAxisRotationRelaxation", _
                                            "Relaxation Properties", "Sets the relaxation for the third rotation axis.", pbNumberBag, _
                                            "", GetType(ConstraintRelaxationPropBagConverter)))
            End If

            If Not m_doFriction Is Nothing Then
                pbNumberBag = m_doFriction.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Friction", pbNumberBag.GetType(), "Friction", _
                                            "Part Properties", "Sets the friction parameters for this joint.", pbNumberBag, _
                                            "", GetType(ConstraintFrictionPropBagConverter)))
            End If


        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            If Not m_doPrimaryAxisDisplacementRelaxation Is Nothing Then m_doPrimaryAxisDisplacementRelaxation.InitializeAfterLoad()
            If Not m_doSecondaryAxisDisplacement Is Nothing Then m_doSecondaryAxisDisplacement.InitializeAfterLoad()
            If Not m_doThirdAxisDisplacement Is Nothing Then m_doThirdAxisDisplacement.InitializeAfterLoad()
            If Not m_doSecondaryAxisRotation Is Nothing Then m_doSecondaryAxisRotation.InitializeAfterLoad()
            If Not m_doThirdAxisRotation Is Nothing Then m_doThirdAxisRotation.InitializeAfterLoad()
            If Not m_doFriction Is Nothing Then m_doFriction.InitializeAfterLoad()
        End Sub

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            MyBase.InitializeSimulationReferences(bShowError)

            If Not m_doPrimaryAxisDisplacementRelaxation Is Nothing Then m_doPrimaryAxisDisplacementRelaxation.InitializeSimulationReferences(bShowError)
            If Not m_doSecondaryAxisDisplacement Is Nothing Then m_doSecondaryAxisDisplacement.InitializeSimulationReferences(bShowError)
            If Not m_doThirdAxisDisplacement Is Nothing Then m_doThirdAxisDisplacement.InitializeSimulationReferences()
            If Not m_doSecondaryAxisRotation Is Nothing Then m_doSecondaryAxisRotation.InitializeSimulationReferences(bShowError)
            If Not m_doThirdAxisRotation Is Nothing Then m_doThirdAxisRotation.InitializeSimulationReferences(bShowError)
            If Not m_doFriction Is Nothing Then m_doFriction.InitializeSimulationReferences(bShowError)
        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into Joint Element

            m_snSize.LoadData(oXml, "Size")
            m_bEnableLimts = oXml.GetChildBool("EnableLimits", m_bEnableLimts)

            If Not m_doPrimaryAxisDisplacementRelaxation Is Nothing AndAlso oXml.FindChildElement("PrimaryDisplacementRelaxation", False) Then
                m_doPrimaryAxisDisplacementRelaxation.LoadData(oXml)
            End If
            If Not m_doSecondaryAxisDisplacement Is Nothing AndAlso oXml.FindChildElement("SecondaryAxisDisplacement", False) Then
                m_doSecondaryAxisDisplacement.LoadData(oXml)
            End If
            If Not m_doThirdAxisDisplacement Is Nothing AndAlso oXml.FindChildElement("ThirdAxisDisplacement", False) Then
                m_doThirdAxisDisplacement.LoadData(oXml)
            End If
            If Not m_doSecondaryAxisRotation Is Nothing AndAlso oXml.FindChildElement("SecondaryAxisRotation", False) Then
                m_doSecondaryAxisRotation.LoadData(oXml)
            End If
            If Not m_doSecondaryAxisRotation Is Nothing AndAlso oXml.FindChildElement("ThirdAxisRotation", False) Then
                m_doThirdAxisRotation.LoadData(oXml)
            End If
            If Not m_doFriction Is Nothing AndAlso oXml.FindChildElement("Friction", False) Then
                m_doFriction.LoadData(oXml)
            End If

            oXml.OutOfElem() 'Outof Joint Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Joint Element

            m_snSize.SaveData(oXml, "Size")
            oXml.AddChildElement("EnableLimits", m_bEnableLimts)

            If Not m_doPrimaryAxisDisplacementRelaxation Is Nothing Then
                m_doPrimaryAxisDisplacementRelaxation.SaveData(oXml)
            End If
            If Not m_doSecondaryAxisDisplacement Is Nothing Then
                m_doSecondaryAxisDisplacement.SaveData(oXml)
            End If
            If Not m_doThirdAxisDisplacement Is Nothing Then
                m_doThirdAxisDisplacement.SaveData(oXml)
            End If
            If Not m_doSecondaryAxisRotation Is Nothing Then
                m_doSecondaryAxisRotation.SaveData(oXml)
            End If
            If Not m_doThirdAxisRotation Is Nothing Then
                m_doThirdAxisRotation.SaveData(oXml)
            End If
            If Not m_doFriction Is Nothing Then
                m_doFriction.SaveData(oXml)
            End If

            oXml.OutOfElem() 'Outof Joint Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem() 'Into Joint Element

            m_snSize.SaveSimulationXml(oXml, Me, "Size")
            oXml.AddChildElement("EnableLimits", m_bEnableLimts)

            If Not m_doPrimaryAxisDisplacementRelaxation Is Nothing Then
                m_doPrimaryAxisDisplacementRelaxation.SaveSimulationXml(oXml, Me)
            End If
            If Not m_doSecondaryAxisDisplacement Is Nothing Then
                m_doSecondaryAxisDisplacement.SaveSimulationXml(oXml, Me)
            End If
            If Not m_doThirdAxisDisplacement Is Nothing Then
                m_doThirdAxisDisplacement.SaveSimulationXml(oXml, Me)
            End If
            If Not m_doSecondaryAxisRotation Is Nothing Then
                m_doSecondaryAxisRotation.SaveSimulationXml(oXml, Me)
            End If
            If Not m_doThirdAxisRotation Is Nothing Then
                m_doThirdAxisRotation.SaveSimulationXml(oXml, Me)
            End If
            If Not m_doFriction Is Nothing Then
                m_doFriction.SaveSimulationXml(oXml, Me)
            End If

            oXml.OutOfElem() 'Outof Joint Element

        End Sub

#End Region

#Region " Events "

        Protected Sub OnAttachedPartMoved(ByRef miPart As AnimatGUI.DataObjects.Physical.MovableItem)
            Try
                Me.SetSimData("AttachedPartMovedOrRotated", miPart.ID, True)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnAttachedPartRotated(ByRef miPart As AnimatGUI.DataObjects.Physical.MovableItem)
            Try
                Me.SetSimData("AttachedPartMovedOrRotated", miPart.ID, True)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

