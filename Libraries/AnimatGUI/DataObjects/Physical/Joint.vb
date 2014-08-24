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

#Region " Enums "

        Public Enum enumJointMotorTypes
            VelocityControl = 0
            PositionControl = 1
            PositionVelocityControl = 2
        End Enum

#End Region

#Region " Attributes "

        Protected m_snSize As AnimatGUI.Framework.ScaledNumber
        Protected m_bEnableLimts As Boolean = True

        Protected m_doRelaxation1 As ConstraintRelaxation
        Protected m_doRelaxation2 As ConstraintRelaxation
        Protected m_doRelaxation3 As ConstraintRelaxation
        Protected m_doRelaxation4 As ConstraintRelaxation
        Protected m_doRelaxation5 As ConstraintRelaxation
        Protected m_doRelaxation6 As ConstraintRelaxation

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
                    Return "rad"
                Else
                    Return "m"
                End If
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

        Public Overridable Property Relaxation1() As ConstraintRelaxation
            Get
                Return m_doRelaxation1
            End Get
            Set(ByVal value As ConstraintRelaxation)
                If Not value Is Nothing Then
                    SetSimData("Relaxation1", value.GetSimulationXml("ConstraintRelaxation", Me), True)
                End If
                m_doRelaxation1 = value
            End Set
        End Property

        Public Overridable Property Relaxation2() As ConstraintRelaxation
            Get
                Return m_doRelaxation2
            End Get
            Set(ByVal value As ConstraintRelaxation)
                If Not value Is Nothing Then
                    SetSimData("Relaxation2", value.GetSimulationXml("ConstraintRelaxation", Me), True)
                End If
                m_doRelaxation2 = value
            End Set
        End Property

        Public Overridable Property Relaxation3() As ConstraintRelaxation
            Get
                Return m_doRelaxation3
            End Get
            Set(ByVal value As ConstraintRelaxation)
                If Not value Is Nothing Then
                    SetSimData("Relaxation3", value.GetSimulationXml("ConstraintRelaxation", Me), True)
                End If
                m_doRelaxation3 = value
            End Set
        End Property

        Public Overridable Property Relaxation4() As ConstraintRelaxation
            Get
                Return m_doRelaxation4
            End Get
            Set(ByVal value As ConstraintRelaxation)
                If Not value Is Nothing Then
                    SetSimData("Relaxation4", value.GetSimulationXml("ConstraintRelaxation", Me), True)
                End If
                m_doRelaxation4 = value
            End Set
        End Property

        Public Overridable Property Relaxation5() As ConstraintRelaxation
            Get
                Return m_doRelaxation5
            End Get
            Set(ByVal value As ConstraintRelaxation)
                If Not value Is Nothing Then
                    SetSimData("Relaxation5", value.GetSimulationXml("ConstraintRelaxation", Me), True)
                End If
                m_doRelaxation5 = value
            End Set
        End Property

        Public Overridable Property Relaxation6() As ConstraintRelaxation
            Get
                Return m_doRelaxation6
            End Get
            Set(ByVal value As ConstraintRelaxation)
                If Not value Is Nothing Then
                    SetSimData("Relaxation6", value.GetSimulationXml("ConstraintRelaxation", Me), True)
                End If
                m_doRelaxation6 = value
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

            m_thIncomingDataTypes.DataTypes.Clear()
            m_thIncomingDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("DesiredVelocity", "Desired Velocity", "m/s", "m/s", -5, 5, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))
            m_thIncomingDataTypes.ID = "DesiredVelocity"

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

            m_doRelaxation1 = Util.Application.Physics.CreateJointRelaxation(Me.Type, ConstraintRelaxation.enumCoordinateID.Relaxation1, Me)
            m_doRelaxation2 = Util.Application.Physics.CreateJointRelaxation(Me.Type, ConstraintRelaxation.enumCoordinateID.Relaxation2, Me)
            m_doRelaxation3 = Util.Application.Physics.CreateJointRelaxation(Me.Type, ConstraintRelaxation.enumCoordinateID.Relaxation3, Me)
            m_doRelaxation4 = Util.Application.Physics.CreateJointRelaxation(Me.Type, ConstraintRelaxation.enumCoordinateID.Relaxation4, Me)
            m_doRelaxation5 = Util.Application.Physics.CreateJointRelaxation(Me.Type, ConstraintRelaxation.enumCoordinateID.Relaxation5, Me)
            m_doRelaxation6 = Util.Application.Physics.CreateJointRelaxation(Me.Type, ConstraintRelaxation.enumCoordinateID.Relaxation6, Me)

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snSize Is Nothing Then m_snSize.ClearIsDirty()
            If Not m_doRelaxation1 Is Nothing Then m_doRelaxation1.ClearIsDirty()
            If Not m_doRelaxation2 Is Nothing Then m_doRelaxation2.ClearIsDirty()
            If Not m_doRelaxation3 Is Nothing Then m_doRelaxation3.ClearIsDirty()
            If Not m_doRelaxation4 Is Nothing Then m_doRelaxation4.ClearIsDirty()
            If Not m_doRelaxation5 Is Nothing Then m_doRelaxation5.ClearIsDirty()
            If Not m_doRelaxation6 Is Nothing Then m_doRelaxation6.ClearIsDirty()
            If Not m_doFriction Is Nothing Then m_doFriction.ClearIsDirty()

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrigJoint As Joint = DirectCast(doOriginal, Joint)
            m_snSize = DirectCast(doOrigJoint.m_snSize.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_bEnableLimts = doOrigJoint.m_bEnableLimts

            If Not doOrigJoint.m_doRelaxation1 Is Nothing Then
                m_doRelaxation1 = DirectCast(doOrigJoint.m_doRelaxation1.Clone(Me, bCutData, doRoot), ConstraintRelaxation)
            Else
                m_doRelaxation1 = Nothing
            End If

            If Not doOrigJoint.m_doRelaxation2 Is Nothing Then
                m_doRelaxation2 = DirectCast(doOrigJoint.m_doRelaxation2.Clone(Me, bCutData, doRoot), ConstraintRelaxation)
            Else
                m_doRelaxation2 = Nothing
            End If

            If Not doOrigJoint.m_doRelaxation3 Is Nothing Then
                m_doRelaxation3 = DirectCast(doOrigJoint.m_doRelaxation3.Clone(Me, bCutData, doRoot), ConstraintRelaxation)
            Else
                m_doRelaxation3 = Nothing
            End If

            If Not doOrigJoint.m_doRelaxation4 Is Nothing Then
                m_doRelaxation4 = DirectCast(doOrigJoint.m_doRelaxation4.Clone(Me, bCutData, doRoot), ConstraintRelaxation)
            Else
                m_doRelaxation4 = Nothing
            End If

            If Not doOrigJoint.m_doRelaxation5 Is Nothing Then
                m_doRelaxation5 = DirectCast(doOrigJoint.m_doRelaxation5.Clone(Me, bCutData, doRoot), ConstraintRelaxation)
            Else
                m_doRelaxation5 = Nothing
            End If

            If Not doOrigJoint.m_doRelaxation6 Is Nothing Then
                m_doRelaxation6 = DirectCast(doOrigJoint.m_doRelaxation6.Clone(Me, bCutData, doRoot), ConstraintRelaxation)
            Else
                m_doRelaxation6 = Nothing
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
            If doObject Is Nothing AndAlso Not m_doRelaxation1 Is Nothing Then doObject = m_doRelaxation1.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doRelaxation2 Is Nothing Then doObject = m_doRelaxation2.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doRelaxation3 Is Nothing Then doObject = m_doRelaxation3.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doRelaxation4 Is Nothing Then doObject = m_doRelaxation4.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doRelaxation5 Is Nothing Then doObject = m_doRelaxation5.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doRelaxation6 Is Nothing Then doObject = m_doRelaxation6.FindObjectByID(strID)
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

            If Not m_doRelaxation1 Is Nothing AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation1.CoordinateAxis) Then
                pbNumberBag = m_doRelaxation1.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec(m_doRelaxation1.Name, pbNumberBag.GetType(), "Relaxation1", _
                                            "Relaxation Properties", m_doRelaxation1.Description, pbNumberBag, _
                                            "", GetType(ConstraintRelaxationPropBagConverter)))
            End If

            If Not m_doRelaxation2 Is Nothing AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation2.CoordinateAxis) Then
                pbNumberBag = m_doRelaxation2.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec(m_doRelaxation2.Name, pbNumberBag.GetType(), "Relaxation2", _
                                            "Relaxation Properties", m_doRelaxation2.Description, pbNumberBag, _
                                            "", GetType(ConstraintRelaxationPropBagConverter)))
            End If

            If Not m_doRelaxation3 Is Nothing AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation3.CoordinateAxis) Then
                pbNumberBag = m_doRelaxation3.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec(m_doRelaxation3.Name, pbNumberBag.GetType(), "Relaxation3", _
                                            "Relaxation Properties", m_doRelaxation3.Description, pbNumberBag, _
                                            "", GetType(ConstraintRelaxationPropBagConverter)))
            End If

            If Not m_doRelaxation4 Is Nothing AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation4.CoordinateAxis) Then
                pbNumberBag = m_doRelaxation4.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec(m_doRelaxation4.Name, pbNumberBag.GetType(), "Relaxation4", _
                                            "Relaxation Properties", m_doRelaxation4.Description, pbNumberBag, _
                                            "", GetType(ConstraintRelaxationPropBagConverter)))
            End If

            If Not m_doRelaxation5 Is Nothing AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation5.CoordinateAxis) Then
                pbNumberBag = m_doRelaxation5.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec(m_doRelaxation5.Name, pbNumberBag.GetType(), "Relaxation5", _
                                            "Relaxation Properties", m_doRelaxation5.Description, pbNumberBag, _
                                            "", GetType(ConstraintRelaxationPropBagConverter)))
            End If

            If Not m_doRelaxation6 Is Nothing AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation6.CoordinateAxis) Then
                pbNumberBag = m_doRelaxation6.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec(m_doRelaxation6.Name, pbNumberBag.GetType(), "Relaxation6", _
                                            "Relaxation Properties", m_doRelaxation6.Description, pbNumberBag, _
                                            "", GetType(ConstraintRelaxationPropBagConverter)))
            End If

            If Not m_doFriction Is Nothing Then
                pbNumberBag = m_doFriction.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Friction", pbNumberBag.GetType(), "Friction", _
                                            "Part Properties", "Sets the friction parameters for this joint.", pbNumberBag, _
                                            "", GetType(ConstraintFrictionPropBagConverter)))
            End If

        End Sub

        Public Overrides Sub AddToReplaceIDList(aryReplaceIDList As System.Collections.ArrayList, ByVal arySelectedItems As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)

            If Not m_doRelaxation1 Is Nothing Then m_doRelaxation1.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            If Not m_doRelaxation2 Is Nothing Then m_doRelaxation2.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            If Not m_doRelaxation3 Is Nothing Then m_doRelaxation3.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            If Not m_doRelaxation4 Is Nothing Then m_doRelaxation4.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            If Not m_doRelaxation5 Is Nothing Then m_doRelaxation5.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            If Not m_doRelaxation6 Is Nothing Then m_doRelaxation6.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            If Not m_doFriction Is Nothing Then m_doFriction.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            If Not m_doRelaxation1 Is Nothing Then m_doRelaxation1.InitializeAfterLoad()
            If Not m_doRelaxation2 Is Nothing Then m_doRelaxation2.InitializeAfterLoad()
            If Not m_doRelaxation3 Is Nothing Then m_doRelaxation3.InitializeAfterLoad()
            If Not m_doRelaxation4 Is Nothing Then m_doRelaxation4.InitializeAfterLoad()
            If Not m_doRelaxation5 Is Nothing Then m_doRelaxation5.InitializeAfterLoad()
            If Not m_doRelaxation6 Is Nothing Then m_doRelaxation6.InitializeAfterLoad()
            If Not m_doFriction Is Nothing Then m_doFriction.InitializeAfterLoad()
        End Sub

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            MyBase.InitializeSimulationReferences(bShowError)

            If Not m_doRelaxation1 Is Nothing AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation1.CoordinateAxis) Then
                m_doRelaxation1.InitializeSimulationReferences(bShowError)
            End If

            If Not m_doRelaxation2 Is Nothing AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation2.CoordinateAxis) Then
                m_doRelaxation2.InitializeSimulationReferences(bShowError)
            End If

            If Not m_doRelaxation3 Is Nothing AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation3.CoordinateAxis) Then
                m_doRelaxation3.InitializeSimulationReferences(bShowError)
            End If

            If Not m_doRelaxation4 Is Nothing AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation4.CoordinateAxis) Then
                m_doRelaxation4.InitializeSimulationReferences(bShowError)
            End If

            If Not m_doRelaxation5 Is Nothing AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation5.CoordinateAxis) Then
                m_doRelaxation5.InitializeSimulationReferences(bShowError)
            End If

            If Not m_doRelaxation6 Is Nothing AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation6.CoordinateAxis) Then
                m_doRelaxation6.InitializeSimulationReferences(bShowError)
            End If

            If Not m_doFriction Is Nothing Then m_doFriction.InitializeSimulationReferences(bShowError)
        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into Joint Element

            m_snSize.LoadData(oXml, "Size")
            m_bEnableLimts = oXml.GetChildBool("EnableLimits", m_bEnableLimts)

            If Not m_doRelaxation1 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation1.CoordinateAxis) _
                AndAlso oXml.FindChildElement("Relaxation1", False) Then
                m_doRelaxation1.LoadData(oXml, "Relaxation1")
            End If

            If Not m_doRelaxation2 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation2.CoordinateAxis) _
                AndAlso oXml.FindChildElement("Relaxation2", False) Then
                m_doRelaxation2.LoadData(oXml, "Relaxation2")
            End If

            If Not m_doRelaxation3 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation3.CoordinateAxis) _
                AndAlso oXml.FindChildElement("Relaxation3", False) Then
                m_doRelaxation3.LoadData(oXml, "Relaxation3")
            End If

            If Not m_doRelaxation4 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation4.CoordinateAxis) _
                AndAlso oXml.FindChildElement("Relaxation4", False) Then
                m_doRelaxation4.LoadData(oXml, "Relaxation4")
            End If

            If Not m_doRelaxation5 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation5.CoordinateAxis) _
                AndAlso oXml.FindChildElement("Relaxation5", False) Then
                m_doRelaxation5.LoadData(oXml, "Relaxation5")
            End If

            If Not m_doRelaxation6 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation6.CoordinateAxis) _
                AndAlso oXml.FindChildElement("Relaxation6", False) Then
                m_doRelaxation5.LoadData(oXml, "Relaxation6")
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

            If Not m_doRelaxation1 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation1.CoordinateAxis) Then
                m_doRelaxation1.SaveData(oXml, "Relaxation1")
            End If

            If Not m_doRelaxation2 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation2.CoordinateAxis) Then
                m_doRelaxation2.SaveData(oXml, "Relaxation2")
            End If

            If Not m_doRelaxation3 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation3.CoordinateAxis) Then
                m_doRelaxation3.SaveData(oXml, "Relaxation3")
            End If

            If Not m_doRelaxation4 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation4.CoordinateAxis) Then
                m_doRelaxation4.SaveData(oXml, "Relaxation4")
            End If

            If Not m_doRelaxation5 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation5.CoordinateAxis) Then
                m_doRelaxation5.SaveData(oXml, "Relaxation5")
            End If

            If Not m_doRelaxation6 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation6.CoordinateAxis) Then
                m_doRelaxation6.SaveData(oXml, "Relaxation6")
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

            If Not m_doRelaxation1 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation1.CoordinateAxis) Then
                m_doRelaxation1.SaveSimulationXml(oXml, Me, "Relaxation1")
            End If

            If Not m_doRelaxation2 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation2.CoordinateAxis) Then
                m_doRelaxation2.SaveSimulationXml(oXml, Me, "Relaxation2")
            End If

            If Not m_doRelaxation3 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation3.CoordinateAxis) Then
                m_doRelaxation3.SaveSimulationXml(oXml, Me, "Relaxation3")
            End If

            If Not m_doRelaxation4 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation4.CoordinateAxis) Then
                m_doRelaxation4.SaveSimulationXml(oXml, Me, "Relaxation4")
            End If

            If Not m_doRelaxation5 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation5.CoordinateAxis) Then
                m_doRelaxation5.SaveSimulationXml(oXml, Me, "Relaxation5")
            End If

            If Not m_doRelaxation6 Is Nothing _
                AndAlso Util.Application.Physics.AllowConstraintRelaxation(Me.Type, m_doRelaxation6.CoordinateAxis) Then
                m_doRelaxation6.SaveSimulationXml(oXml, Me, "Relaxation6")
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

