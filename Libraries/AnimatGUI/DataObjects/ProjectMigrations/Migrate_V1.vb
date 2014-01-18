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
    Namespace ProjectMigrations

        Public Class Migrate_V1
            Inherits ProjectMigration

            Protected m_bSimHydro As Boolean = False
            Protected m_dblFluidDensity As Double = 1.0
            Protected m_iSimInterface As ManagedAnimatInterfaces.ISimulatorInterface
            Protected m_fltDistanceUnits As Single
            Protected m_fltMassUnits As Single
            Protected m_fltDisplayMassUnits As Single
            Protected m_aryIdentity As New AnimatGuiCtrls.MatrixLibrary.Matrix(AnimatGuiCtrls.MatrixLibrary.Matrix.Identity(4))
            Protected m_aryGainIDs As New ArrayList

            Protected m_xnMouth As XmlNode
            Protected m_xnStomach As XmlNode

            Protected m_strMouthID As String = System.Guid.NewGuid.ToString
            Protected m_strStomachID As String = System.Guid.NewGuid.ToString

            Protected m_arySubsystemIDs As New ArrayList

            'Keeps track of each mesh name as it is used so we do not duplicate any.
            Protected m_aryUsedMeshNames As New ArrayList

            Public Overrides ReadOnly Property ConvertFrom As String
                Get
                    Return "1"
                End Get
            End Property

            Public Overrides ReadOnly Property ConvertTo As String
                Get
                    Return "3"
                End Get
            End Property

            Sub New()
                MyBase.New()

            End Sub

            Protected Overrides Function CreateReplaceStringList() As Hashtable

                Dim aryReplaceText As New Hashtable()
                aryReplaceText.Add(35, New ReplaceText("</MuscleLengthData>", "</LengthData>"))
                aryReplaceText.Add(34, New ReplaceText("<MuscleLengthData>", "<LengthData>"))
                aryReplaceText.Add(33, New ReplaceText("<ID>MOUTH</ID>", "<ID>" & m_strMouthID & "</ID>"))
                aryReplaceText.Add(32, New ReplaceText("<LinkedBodyPartID>MOUTH</LinkedBodyPartID>", "<LinkedBodyPartID>" & m_strMouthID & "</LinkedBodyPartID>"))
                aryReplaceText.Add(31, New ReplaceText("<DataItemID>MOUTH</DataItemID>", "<DataItemID>" & m_strMouthID & "</DataItemID>"))
                aryReplaceText.Add(30, New ReplaceText("<ID>Mouth</ID>", "<ID>" & m_strMouthID & "</ID>"))
                aryReplaceText.Add(29, New ReplaceText("<LinkedBodyPartID>Mouth</LinkedBodyPartID>", "<LinkedBodyPartID>" & m_strMouthID & "</LinkedBodyPartID>"))
                aryReplaceText.Add(28, New ReplaceText("<DataItemID>Mouth</DataItemID>", "<DataItemID>" & m_strMouthID & "</DataItemID>"))
                aryReplaceText.Add(27, New ReplaceText("<ID>STOMACH</ID>", "<ID>" & m_strStomachID & "</ID>"))
                aryReplaceText.Add(26, New ReplaceText("<LinkedBodyPartID>STOMACH</LinkedBodyPartID>", "<LinkedBodyPartID>" & m_strStomachID & "</LinkedBodyPartID>"))
                aryReplaceText.Add(25, New ReplaceText("<DataItemID>STOMACH</DataItemID>", "<DataItemID>" & m_strStomachID & "</DataItemID>"))
                aryReplaceText.Add(24, New ReplaceText("<ID>Stomach</ID>", "<ID>" & m_strStomachID & "</ID>"))
                aryReplaceText.Add(23, New ReplaceText("<LinkedBodyPartID>Stomach</LinkedBodyPartID>", "<LinkedBodyPartID>" & m_strStomachID & "</LinkedBodyPartID>"))
                aryReplaceText.Add(22, New ReplaceText("<DataItemID>Stomach</DataItemID>", "<DataItemID>" & m_strStomachID & "</DataItemID>"))
                aryReplaceText.Add(21, New ReplaceText("CylinderContactSensor", "Cylinder"))
                aryReplaceText.Add(20, New ReplaceText("BoxContactSensor", "Box"))
                aryReplaceText.Add(19, New ReplaceText("InterbusrtLengthDistribution", "InterburstLengthDistribution"))
                aryReplaceText.Add(18, New ReplaceText("LicensedAnimatTools", "LicensedAnimatGUI"))
                aryReplaceText.Add(17, New ReplaceText("FastNeuralNetTools", "FiringRateGUI"))
                aryReplaceText.Add(16, New ReplaceText("RealisticNeuralNetTools", "IntegrateFireGUI"))
                aryReplaceText.Add(15, New ReplaceText("VortexAnimatTools.DataObjects.Physical.RigidBodies.BodyMesh", "AnimatGUI.DataObjects.Physical.Bodies.Mesh"))
                aryReplaceText.Add(14, New ReplaceText("VortexAnimatTools.DataObjects.Physical.RigidBodies", "AnimatGUI.DataObjects.Physical.Bodies"))
                aryReplaceText.Add(13, New ReplaceText("VortexAnimatTools.DataObjects.Physical.Joints", "AnimatGUI.DataObjects.Physical.Joints"))
                aryReplaceText.Add(12, New ReplaceText("VortexAnimatTools.DataObjects.Behavior.Nodes.MuscleSpindle", "AnimatGUI.DataObjects.Behavior.Nodes.StretchReceptor"))
                aryReplaceText.Add(11, New ReplaceText("VortexAnimatTools.DataObjects.ExternalStimuli.InverseMuscleDynamics", "AnimatGUI.DataObjects.ExternalStimuli.InverseMuscleCurrent"))
                aryReplaceText.Add(10, New ReplaceText("VortexAnimatTools", "AnimatGUI"))
                aryReplaceText.Add(9, New ReplaceText("AnimatTools", "AnimatGUI"))
                aryReplaceText.Add(8, New ReplaceText("AHPConductance", "AHP_Conductance"))
                aryReplaceText.Add(7, New ReplaceText("AHPTimeConstant", "AHP_TimeConstant"))
                aryReplaceText.Add(6, New ReplaceText("BodyPositionX", "WorldPositionX"))
                aryReplaceText.Add(5, New ReplaceText("BodyPositionY", "WorldPositionY"))
                aryReplaceText.Add(4, New ReplaceText("BodyPositionZ", "WorldPositionZ"))
                aryReplaceText.Add(3, New ReplaceText("BodyRotationX", "RotationX"))
                aryReplaceText.Add(2, New ReplaceText("BodyRotationY", "RotationY"))
                aryReplaceText.Add(1, New ReplaceText("BodyRotationZ", "RotationZ"))

                Return aryReplaceText
            End Function

            Protected Overrides Sub ConvertProjectNode(ByVal xnProject As XmlNode, ByRef strPhysics As String)

                m_iSimInterface = Util.Application.CreateSimInterface
                m_iSimInterface.CreateStandAloneSim("VortexAnimatSim_VC" & Util.Application.SimVCVersion & Util.Application.RuntimeModePrefix & ".dll",
                                                    Application.ExecutablePath)
                m_iSimInterface.SetProjectPath(m_strProjectPath & "\")

                m_xnProjectXml.RemoveNode(xnProject, "PhysicsClassName", False)
                m_xnProjectXml.RemoveNode(xnProject, "PhysicsAssemblyName", False)
                m_xnProjectXml.UpdateSingleNodeValue(xnProject, "Version", ConvertTo(), False)

                strPhysics = m_xnProjectXml.GetSingleNodeValue(xnProject, "Physics", False, "Vortex")

                m_xnProjectXml.RemoveNode(xnProject, "DockingForms", False)
                m_xnProjectXml.RemoveNode(xnProject, "ChildForms", False)
                m_xnProjectXml.RemoveNode(xnProject, "DockingConfig", False)
                m_xnProjectXml.RemoveNode(xnProject, "SimWindowLocation", False)
                m_xnProjectXml.RemoveNode(xnProject, "SimWindowSize", False)

                Dim strLogLevel As String = m_xnProjectXml.GetSingleNodeValue(xnProject, "LogLevel", False, "None")
                If strLogLevel.ToUpper = "ERROR" Then
                    m_xnProjectXml.UpdateSingleNodeValue(xnProject, "LogLevel", "ErrorType")
                End If

                m_xnProjectXml.RemoveNode(xnProject, "Version", False)
                m_xnProjectXml.AddNodeValue(xnProject, "Version", ConvertTo().ToString)

                CreateSimulationNode(xnProject)

            End Sub

            Protected Overridable Sub CreateSimulationNode(ByVal xnProject As XmlNode)

                'We need to get the simulation node from the asim file. We will then modify it.
                Dim strAsimFile As String = m_strProjectPath & "\" & m_xnProjectXml.GetSingleNodeValue(xnProject, "SimulationFile")

                Dim xnAST_File As New Framework.XmlDom

                xnAST_File.Load(strAsimFile)

                Dim xnSimNodeOld As XmlNode = xnAST_File.GetRootNode("Simulation")
                Dim xnSimNode As XmlNode = m_xnProjectXml.AppendNode(xnProject, xnSimNodeOld, "Simulation")

                m_xnProjectXml.RemoveNode(xnSimNode, "APIFile", False)

                ModifySimNode(xnSimNode)
                ModifyStimuli(xnProject, xnSimNode)
                ModifyToolViewers(xnProject, xnSimNode)

            End Sub

            Protected Overridable Sub ModifySimNode(ByVal xnSimulation As XmlNode)

                Util.Application.AppStatusText = "Converting simulation nodes"

                m_xnProjectXml.UpdateSingleNodeValue(xnSimulation, "AnimatModule", "VortexAnimatSim_VC" & Util.Application.SimVCVersion & Util.Application.RuntimeModePrefix & ".dll")

                Dim xnEnvironment As XmlNode = m_xnProjectXml.GetNode(xnSimulation, "Environment")

                m_dblFluidDensity = m_xnProjectXml.GetScaleNumberValue(xnEnvironment, "FluidDensity", False, 1.0)
                m_bSimHydro = CBool(m_xnProjectXml.GetSingleNodeValue(xnEnvironment, "SimulateHydrodynamics", True, "False"))
                m_xnProjectXml.RemoveNode(xnEnvironment, "FluidDensity", False)
                m_xnProjectXml.RemoveNode(xnEnvironment, "MaxHydroForce", False)
                m_xnProjectXml.RemoveNode(xnEnvironment, "MaxHydroTorque", False)
                m_xnProjectXml.RemoveNode(xnEnvironment, "PlaybackRate", False)
                m_xnProjectXml.RemoveNode(xnEnvironment, "UseAlphaBlending", False)
                m_xnProjectXml.RemoveNode(xnEnvironment, "Camera", False)

                m_fltDistanceUnits = Util.ConvertDistanceUnits(m_xnProjectXml.GetSingleNodeValue(xnEnvironment, "DistanceUnits"))
                m_fltMassUnits = Util.ConvertMassUnits(m_xnProjectXml.GetSingleNodeValue(xnEnvironment, "MassUnits"))
                m_fltDisplayMassUnits = Util.ConvertDisplayMassUnits(m_xnProjectXml.GetSingleNodeValue(xnEnvironment, "MassUnits"))

                ''Now put back the defaults for linear and angular compliance.
                'm_xnProjectXml.RemoveNode(xnEnvironment, "LinearDamping", False)
                'm_xnProjectXml.RemoveNode(xnEnvironment, "LinearKineticLoss", False)
                'm_xnProjectXml.RemoveNode(xnEnvironment, "AngularDamping", False)
                'm_xnProjectXml.RemoveNode(xnEnvironment, "AngularKineticLoss", False)

                'Within V1 we are not correctly scaling these values based on the mass and distance units. In order to make the values be the same as the
                'original simulation I need to rescale them so they will match when loaded.
                Dim dblLinearCompliance As Double = m_xnProjectXml.GetScaleNumberValue(xnEnvironment, "LinearCompliance")
                Dim dblAngularCompliance As Double = m_xnProjectXml.GetScaleNumberValue(xnEnvironment, "AngularCompliance")

                dblLinearCompliance = dblLinearCompliance * (1 / m_fltMassUnits)
                dblAngularCompliance = dblAngularCompliance * ((1 / m_fltMassUnits) * (1 / m_fltDistanceUnits) * (1 / m_fltDistanceUnits))

                m_xnProjectXml.RemoveNode(xnEnvironment, "LinearCompliance", False)
                m_xnProjectXml.RemoveNode(xnEnvironment, "AngularCompliance", False)
                m_xnProjectXml.AddScaledNumber(xnEnvironment, "LinearCompliance", dblLinearCompliance, "None", dblLinearCompliance)
                m_xnProjectXml.AddScaledNumber(xnEnvironment, "AngularCompliance", dblAngularCompliance, "None", dblAngularCompliance)

                'Set it so that it will not calculate the other sim params, but use the values specified in the file.
                m_xnProjectXml.AddNodeValue(xnEnvironment, "CalcCriticalSimParams", "False")

                'Now convert the mouse spring. The mouse spring was not converted correctly in the old version, so we need to downgrade
                'its numbers first.
                Dim dblMouseStiff As Double = m_xnProjectXml.GetScaleNumberValue(xnEnvironment, "MouseSpringStiffness", False, 300)
                Dim dblMouseDamp As Double = m_xnProjectXml.GetScaleNumberValue(xnEnvironment, "MouseSpringDamping", False, 100000)

                dblMouseStiff = dblMouseStiff * m_fltMassUnits
                dblMouseDamp = dblMouseDamp * m_fltMassUnits

                m_xnProjectXml.RemoveNode(xnEnvironment, "MouseSpringStiffness", False)
                m_xnProjectXml.RemoveNode(xnEnvironment, "MouseSpringDamping", False)

                m_xnProjectXml.AddScaledNumber(xnEnvironment, "MouseSpringStiffness", dblMouseStiff, "None", dblMouseStiff)
                m_xnProjectXml.AddScaledNumber(xnEnvironment, "MouseSpringDamping", dblMouseDamp, "None", dblMouseDamp)

                Dim xnOrganisms As XmlNode = m_xnProjectXml.GetNode(xnEnvironment, "Organisms")
                For Each xnNode As XmlNode In xnOrganisms.ChildNodes
                    ModifyOrganism(xnNode)
                Next

                Dim xnStructures As XmlNode = m_xnProjectXml.GetNode(xnEnvironment, "Structures")
                For Each xnNode As XmlNode In xnStructures.ChildNodes
                    ModifyStructure(xnNode, False)
                Next

                AddLight(xnEnvironment)
                AddDefaultMaterial(xnEnvironment)

                Dim aryReplaceText As Hashtable = CreateReplaceStringList()

                ModifySurface("Ground", xnEnvironment, xnStructures, aryReplaceText, False)
                ModifySurface("Water", xnEnvironment, xnStructures, aryReplaceText, True)

            End Sub

            Protected Overridable Sub ModifyOrganism(ByVal xnOrganism As XmlNode)

                ModifyStructure(xnOrganism, False)
                ModifyNervousSystem(xnOrganism)

            End Sub

#Region "Modify Structure"

            Protected Overridable Sub ModifyStructure(ByVal xnStructure As XmlNode, ByVal bIsFluidPlane As Boolean, Optional ByVal bSetAmbientColor As Boolean = False)

                'Reset the mouth and stomach nodes for this structure.
                m_xnMouth = Nothing
                m_xnStomach = Nothing

                Dim strName As String = m_xnProjectXml.GetSingleNodeValue(xnStructure, "Name", False)
                Util.Application.AppStatusText = "Converting organism/structure " & strName & " body"

                m_xnProjectXml.AddNodeValue(xnStructure, "Description", "")
                m_xnProjectXml.AddTransparency(xnStructure, 50, 50, 50, 50, 100)
                m_xnProjectXml.RemoveNode(xnStructure, "Reference", False)
                m_xnProjectXml.ConvertScaledNumberToScaledVector(xnStructure, "Position", "LocalPosition", m_fltDistanceUnits, m_fltDistanceUnits, m_fltDistanceUnits)
                m_xnProjectXml.AddNodeValue(xnStructure, "IsVisible", "True")
                m_xnProjectXml.AddNodeValue(xnStructure, "Shininess", "64")
                m_xnProjectXml.AddNodeValue(xnStructure, "Texture", "")
                m_xnProjectXml.AddScaledVector(xnStructure, "Rotation", 0, 0, 0)
                m_xnProjectXml.AddColor(xnStructure, "Ambient", 1, 0, 0, 1)
                m_xnProjectXml.AddColor(xnStructure, "Diffuse", 1, 0, 0, 1)
                m_xnProjectXml.AddColor(xnStructure, "Specular", 1, 0, 0, 1)

                CreateRigidBodyRootNode(xnStructure, bIsFluidPlane, bSetAmbientColor)

            End Sub

#Region "Rigid Body Modifiers"

            Protected Overridable Sub CreateRigidBodyRootNode(ByVal xnStructure As XmlNode, ByVal bIsFluidPlane As Boolean, Optional ByVal bSetAmbientColor As Boolean = False)

                'We need to get the simulation node from the asim file. We will then modify it.
                Dim strBodyPlanFile As String = m_strProjectPath & "\" & m_xnProjectXml.GetSingleNodeValue(xnStructure, "BodyPlan")
                Dim xnASTL_File As New Framework.XmlDom

                xnASTL_File.Load(strBodyPlanFile)

                Dim xnStruct As XmlNode = xnASTL_File.GetRootNode("Structure")

                Dim xnRigidBodyNodeOld As XmlNode = xnASTL_File.GetNode(xnStruct, "RigidBody", False)
                If Not xnRigidBodyNodeOld Is Nothing Then
                    Dim xnRigidBody As XmlNode = m_xnProjectXml.AppendNode(xnStructure, xnRigidBodyNodeOld, "RigidBody")

                    Dim xnCollisionsOld As XmlNode = xnASTL_File.GetNode(xnStruct, "CollisionExclusionPairs")
                    Dim xnCollisions As XmlNode = m_xnProjectXml.AppendNode(xnStructure, xnCollisionsOld, "CollisionExclusionPairs")

                    ModifyRigidBody(xnRigidBody, m_aryIdentity, bIsFluidPlane, bSetAmbientColor)
                End If
            End Sub

            Protected Overridable Sub ModifyRigidBody(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, ByVal bIsFluidPlane As Boolean, Optional ByVal bSetAmbientColor As Boolean = False)

                'First check to see if this is a rigid body that was added by the converter process. If it was then skip it and do not processes it further.
                If Not m_xnProjectXml.GetNode(xnRigidBody, "Converter", False) Is Nothing Then
                    Return
                End If

                Dim strName As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "Name", False)
                Util.Application.AppStatusText = "Converting body part " & strName

                m_xnProjectXml.AddTransparency(xnRigidBody, 0, 0, 50, 50, 0)

                ModifyRigidBodyDrag(xnRigidBody)
                ModifyRigidBodyCOM(xnRigidBody)
                ModifyRigidBodyColor(xnRigidBody, bSetAmbientColor)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "IsCollisionObject", "True")

                m_xnProjectXml.RemoveNode(xnRigidBody, "PartType", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "Direction", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "CenterOfMass", False)

                m_xnProjectXml.RemoveNode(xnRigidBody, "RelativePosition", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "LocalPosition", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "Rotation", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "LocalRotation", False)

                If Not m_xnProjectXml.GetNode(xnRigidBody, "OrientationMatrix", False) Is Nothing Then
                    Dim aryOrientation As AnimatGuiCtrls.MatrixLibrary.Matrix = m_xnProjectXml.LoadOrientationPositionMatrix(xnRigidBody, "TranslationMatrix", "OrientationMatrix")

                    Dim aryTransform As AnimatGuiCtrls.MatrixLibrary.Matrix = AnimatGuiCtrls.MatrixLibrary.Matrix.Multiply(aryOrientation, aryParentTrasform)

                    Dim oPosRot As ManagedAnimatInterfaces.PositionRotationInfo
                    oPosRot = m_iSimInterface.GetPositionAndRotationFromD3DMatrix(aryTransform.toArray())

                    m_xnProjectXml.AddScaledVector(xnRigidBody, "LocalPosition", oPosRot.m_fltXPos * m_fltDistanceUnits, oPosRot.m_fltYPos * m_fltDistanceUnits, oPosRot.m_fltZPos * m_fltDistanceUnits)
                    m_xnProjectXml.AddScaledVector(xnRigidBody, "Rotation", Util.RadiansToDegrees(oPosRot.m_fltXRot), Util.RadiansToDegrees(oPosRot.m_fltYRot), Util.RadiansToDegrees(oPosRot.m_fltZRot))
                End If

                Dim strType As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "Type").ToUpper
                Dim aryChildTransform As New AnimatGuiCtrls.MatrixLibrary.Matrix(AnimatGuiCtrls.MatrixLibrary.Matrix.Identity(4))

                Dim fltReceptiveFieldDistance As Single = ModifyReceptiveFields(xnRigidBody)

                Select Case strType
                    Case "BOX"
                        ModifyRigidBodyBox(xnRigidBody, aryParentTrasform, aryChildTransform, fltReceptiveFieldDistance)
                    Case "BOXCONTACTSENSOR"
                        ModifyRigidBodyBoxSensor(xnRigidBody, aryParentTrasform, aryChildTransform)
                    Case "CONE"
                        ModifyRigidBodyCone(xnRigidBody, aryParentTrasform, aryChildTransform, fltReceptiveFieldDistance)
                    Case "CYLINDER"
                        ModifyRigidBodyCylinder(xnRigidBody, aryParentTrasform, aryChildTransform, fltReceptiveFieldDistance)
                    Case "CYLINDERCONTACTSENSOR"
                        ModifyRigidBodyCylinderSensor(xnRigidBody, aryParentTrasform, aryChildTransform)
                    Case "MUSCLEATTACHMENT"
                        ModifyRigidBodyAttachment(xnRigidBody, aryParentTrasform, aryChildTransform)
                    Case "LINEARHILLMUSCLE"
                        ModifyRigidBodyMuscle(xnRigidBody, aryParentTrasform, aryChildTransform)
                    Case "LINEARHILLSTRETCHRECEPTOR"
                        ModifyRigidBodyStretchReceptor(xnRigidBody, aryParentTrasform, aryChildTransform)
                    Case "MESH"
                        ModifyRigidBodyMesh(xnRigidBody, aryParentTrasform, aryChildTransform)
                    Case "MOUTH"
                        ModifyRigidBodyMouth(xnRigidBody, aryParentTrasform, aryChildTransform)
                    Case "ODORSENSOR"
                        ModifyRigidBodyOdorSensor(xnRigidBody, aryParentTrasform, aryChildTransform)
                    Case "SPHERE"
                        ModifyRigidBodySphere(xnRigidBody, aryParentTrasform, aryChildTransform, fltReceptiveFieldDistance)
                    Case "SPRING"
                        ModifyRigidBodySpring(xnRigidBody, aryParentTrasform, aryChildTransform)
                    Case "STOMACH"
                        ModifyRigidBodyStomach(xnRigidBody, aryParentTrasform, aryChildTransform)
                    Case "PLANE"
                        ModifyRigidBodyPlane(xnRigidBody, aryParentTrasform, aryChildTransform, bIsFluidPlane)
                    Case "MULTISEGMENTSPRING"
                        ModifyMultiSegmentSpring(xnRigidBody, aryParentTrasform, aryChildTransform, bIsFluidPlane)
                    Case Else
                        Throw New System.Exception("Invalid body part type defined. '" & strType & "'")
                End Select

                'Modify the joint if it exists.
                Dim xnJoint As XmlNode = m_xnProjectXml.GetNode(xnRigidBody, "Joint", False)
                If Not xnJoint Is Nothing Then
                    ModifyJoint(xnJoint, aryChildTransform)
                End If

                'Now modify all the child nodes.
                Dim xnChildren As XmlNode = m_xnProjectXml.GetNode(xnRigidBody, "ChildBodies", False)
                If Not xnChildren Is Nothing Then
                    For Each xnChild As XmlNode In xnChildren.ChildNodes
                        ModifyRigidBody(xnChild, aryChildTransform, False)
                    Next
                End If

                m_xnProjectXml.RemoveNode(xnRigidBody, "OrientationMatrix", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "TranslationMatrix", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "CombinedTransformationMatrix", False)

                Dim strID As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "ID")
                m_hashRigidBodies.Add(strID, xnRigidBody)

            End Sub

            Protected Overridable Sub ModifyRigidBodyDrag(ByVal xnRigidBody As XmlNode)

                Dim dblDrag As Double = CDbl(m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "Cd", False, "1"))

                m_xnProjectXml.RemoveNode(xnRigidBody, "Cd", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "Cdr", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "Ca", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "Car", False)

                m_xnProjectXml.AddScaledVector(xnRigidBody, "Drag", dblDrag, dblDrag, dblDrag)
                m_xnProjectXml.AddScaledVector(xnRigidBody, "BuoyancyCenter", 0, 0, 0)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "BuoyancyScale", "1")
                m_xnProjectXml.AddNodeValue(xnRigidBody, "Magnus", "0")
                m_xnProjectXml.AddNodeValue(xnRigidBody, "EnableFluids", m_bSimHydro.ToString)

            End Sub

            Protected Overridable Sub ModifyRigidBodyCOM(ByVal xnRigidBody As XmlNode)

                Dim dblX As Double = m_xnProjectXml.GetScaleNumberValue(xnRigidBody, "XCenterOfMass", False, 0)
                Dim dblY As Double = m_xnProjectXml.GetScaleNumberValue(xnRigidBody, "YCenterOfMass", False, 0)
                Dim dblZ As Double = m_xnProjectXml.GetScaleNumberValue(xnRigidBody, "ZCenterOfMass", False, 0)

                m_xnProjectXml.RemoveNode(xnRigidBody, "XCenterOfMass", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "YCenterOfMass", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "ZCenterOfMass", False)

                m_xnProjectXml.AddScaledVector(xnRigidBody, "COM", dblX, dblY, dblZ)

            End Sub

            Protected Overridable Sub ModifyRigidBodyColor(ByVal xnRigidBody As XmlNode, Optional ByVal bSetAmbientColor As Boolean = False)

                Dim dblDiffuseR As Double = 1
                Dim dblDiffuseG As Double = 0
                Dim dblDiffuseB As Double = 0
                Dim dblDiffuseA As Double = 1

                m_xnProjectXml.ReadColor(xnRigidBody, "Color", dblDiffuseR, dblDiffuseG, dblDiffuseB, dblDiffuseA, False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "Color", False)

                m_xnProjectXml.AddColor(xnRigidBody, "Diffuse", dblDiffuseR, dblDiffuseG, dblDiffuseB, dblDiffuseA)
                m_xnProjectXml.AddColor(xnRigidBody, "Specular", 0.25098, 0.25098, 0.25098, 1)

                If bSetAmbientColor Then
                    m_xnProjectXml.AddColor(xnRigidBody, "Ambient", dblDiffuseR, dblDiffuseG, dblDiffuseB, 1)
                Else
                    m_xnProjectXml.AddColor(xnRigidBody, "Ambient", 0.0980392, 0.0980392, 0.0980392, 1)
                End If

                m_xnProjectXml.AddNodeValue(xnRigidBody, "Shininess", "64")

            End Sub

            Protected Overridable Function ModifyReceptiveFields(ByVal xnRigidBody As XmlNode) As Single

                Dim xnReceptiveField As XmlNode = m_xnProjectXml.GetNode(xnRigidBody, "ContactSensor", False)
                If Not xnReceptiveField Is Nothing Then
                    Dim aryReplaceText As Hashtable = CreateReplaceStringList()

                    Dim xnReceptiveFieldSensor As XmlNode = m_xnProjectXml.AppendNode(xnRigidBody, xnReceptiveField.InnerXml, "ReceptiveFieldSensor", aryReplaceText)
                    m_xnProjectXml.RemoveNode(xnRigidBody, "ContactSensor")

                    m_xnProjectXml.AddNodeValue(xnReceptiveFieldSensor, "ID", Guid.NewGuid.ToString)
                    m_xnProjectXml.AddNodeValue(xnReceptiveFieldSensor, "AssemblyFile", "AnimatGUI.dll")
                    m_xnProjectXml.AddNodeValue(xnReceptiveFieldSensor, "ClassName", "AnimatGUI.DataObjects.Physical.ContactSensor")

                    Return CSng(m_xnProjectXml.GetScaleNumberValue(xnReceptiveField, "ReceptiveFieldDistance"))
                End If

                Return -1
            End Function

#Region "Rigid Body Part Type Modifiers"

            Protected Overridable Sub ModifyRigidBodyBox(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                         ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, ByVal fltReceptiveFieldDistance As Single)

                Dim dblX As Double = 0
                Dim dblY As Double = 0
                Dim dblZ As Double = 0

                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.Box")
                m_xnProjectXml.ReadVector(xnRigidBody, "CollisionBoxSize", dblX, dblY, dblZ)

                dblX = dblX * m_fltDistanceUnits
                dblY = dblY * m_fltDistanceUnits
                dblZ = dblZ * m_fltDistanceUnits

                m_xnProjectXml.RemoveNode(xnRigidBody, "CollisionBoxSize", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "GraphicsBoxSize", False)

                m_xnProjectXml.AddScaledNumber(xnRigidBody, "Length", dblX, "None", dblX)
                m_xnProjectXml.AddScaledNumber(xnRigidBody, "Height", dblY, "None", dblY)
                m_xnProjectXml.AddScaledNumber(xnRigidBody, "Width", dblZ, "None", dblZ)

                Dim iLengthSections As Integer = 1
                Dim iWidthhSections As Integer = 1
                Dim iHeightSections As Integer = 1

                'If a receptive field distance is specified then we need to use the correct number of sections.
                If fltReceptiveFieldDistance > -1 Then
                    iLengthSections = CInt(Math.Ceiling(dblX / fltReceptiveFieldDistance))
                    iHeightSections = CInt(Math.Ceiling(dblY / fltReceptiveFieldDistance))
                    iWidthhSections = CInt(Math.Ceiling(dblZ / fltReceptiveFieldDistance))

                    If iLengthSections <= 0 Then iLengthSections = 1
                    If iHeightSections <= 0 Then iHeightSections = 1
                    If iWidthhSections <= 0 Then iWidthhSections = 1
                End If

                m_xnProjectXml.AddNodeValue(xnRigidBody, "LengthSections", iLengthSections.ToString)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "HeightSections", iHeightSections.ToString)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "WidthSections", iWidthhSections.ToString)

            End Sub

            Protected Overridable Sub ModifyRigidBodyBoxSensor(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                               ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

                ModifyRigidBodyBox(xnRigidBody, aryParentTrasform, aryChildTrasform, -1)

                m_xnProjectXml.RemoveNode(xnRigidBody, "IsContactSensor", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "IsCollisionObject", False)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "IsContactSensor", "True")
                m_xnProjectXml.AddNodeValue(xnRigidBody, "IsCollisionObject", "True")

            End Sub

            Protected Overridable Sub ModifyRigidBodyCone(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                          ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, ByVal fltReceptiveFieldDistance As Single)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.Cone")

                Dim fltLowerRadius As Single = CSng(m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "CollisionLowerRadius")) * m_fltDistanceUnits
                Dim fltUpperRadius As Single = CSng(m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "CollisionUpperRadius")) * m_fltDistanceUnits
                Dim fltHeight As Single = CSng(m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "CollisionHeight")) * m_fltDistanceUnits

                m_xnProjectXml.RemoveNode(xnRigidBody, "LowerRadius")
                m_xnProjectXml.RemoveNode(xnRigidBody, "UpperRadius")
                m_xnProjectXml.RemoveNode(xnRigidBody, "Height")
                m_xnProjectXml.RemoveNode(xnRigidBody, "CollisionLowerRadius")
                m_xnProjectXml.RemoveNode(xnRigidBody, "CollisionUpperRadius")
                m_xnProjectXml.RemoveNode(xnRigidBody, "CollisionHeight")

                m_xnProjectXml.AddScaledNumber(xnRigidBody, "LowerRadius", fltLowerRadius, "None", fltLowerRadius)
                m_xnProjectXml.AddScaledNumber(xnRigidBody, "UpperRadius", fltUpperRadius, "None", fltUpperRadius)
                m_xnProjectXml.AddScaledNumber(xnRigidBody, "Height", fltHeight, "None", fltHeight)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "Sides", "30")

                'Have to redo the rotation on the cone to make it match up.
                m_xnProjectXml.RemoveNode(xnRigidBody, "LocalPosition", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "Rotation", False)

                Dim aryOrientation As AnimatGuiCtrls.MatrixLibrary.Matrix = m_xnProjectXml.LoadOrientationPositionMatrix(xnRigidBody, "TranslationMatrix", "OrientationMatrix")
                Dim aryConversion As AnimatGuiCtrls.MatrixLibrary.Matrix = MakeConeConversionMatrix()
                Dim aryPreTrans As AnimatGuiCtrls.MatrixLibrary.Matrix = AnimatGuiCtrls.MatrixLibrary.Matrix.Multiply(aryConversion, aryOrientation)
                Dim aryTransform As AnimatGuiCtrls.MatrixLibrary.Matrix = AnimatGuiCtrls.MatrixLibrary.Matrix.Multiply(aryPreTrans, aryParentTrasform)

                Dim oPosRot As ManagedAnimatInterfaces.PositionRotationInfo
                oPosRot = m_iSimInterface.GetPositionAndRotationFromD3DMatrix(aryTransform.toArray)

                m_xnProjectXml.AddScaledVector(xnRigidBody, "LocalPosition", oPosRot.m_fltXPos * m_fltDistanceUnits, oPosRot.m_fltYPos * m_fltDistanceUnits, oPosRot.m_fltZPos * m_fltDistanceUnits)
                m_xnProjectXml.AddScaledVector(xnRigidBody, "Rotation", Util.RadiansToDegrees(oPosRot.m_fltXRot), Util.RadiansToDegrees(oPosRot.m_fltYRot), Util.RadiansToDegrees(oPosRot.m_fltZRot))

                aryChildTrasform = AnimatGuiCtrls.MatrixLibrary.Matrix.Inverse(aryConversion)

            End Sub

            Protected Overridable Sub ModifyRigidBodyCylinder(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                              ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, ByVal fltReceptiveFieldDistance As Single)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.Cylinder")

                Dim fltRadius As Single = CSng(m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "CollisionRadius")) * m_fltDistanceUnits
                Dim fltHeight As Single = CSng(m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "CollisionHeight")) * m_fltDistanceUnits

                m_xnProjectXml.RemoveNode(xnRigidBody, "Radius")
                m_xnProjectXml.RemoveNode(xnRigidBody, "Height")
                m_xnProjectXml.RemoveNode(xnRigidBody, "CollisionRadius")
                m_xnProjectXml.RemoveNode(xnRigidBody, "CollisionHeight")

                m_xnProjectXml.AddScaledNumber(xnRigidBody, "Radius", fltRadius, "None", fltRadius)
                m_xnProjectXml.AddScaledNumber(xnRigidBody, "Height", fltHeight, "None", fltHeight)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "Sides", "30")

            End Sub

            Protected Overridable Sub ModifyRigidBodyCylinderSensor(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                                    ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

                ModifyRigidBodyCylinder(xnRigidBody, aryParentTrasform, aryChildTrasform, -1)

                m_xnProjectXml.RemoveNode(xnRigidBody, "IsContactSensor", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "IsCollisionObject", False)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "IsContactSensor", "True")
                m_xnProjectXml.AddNodeValue(xnRigidBody, "IsCollisionObject", "True")


            End Sub

            Protected Overridable Sub ModifyRigidBodyAttachment(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                                ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

                ModifyRigidBodySphere(xnRigidBody, aryParentTrasform, aryChildTrasform, -1)

                m_xnProjectXml.RemoveNode(xnRigidBody, "PartType", False)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.Attachment")
            End Sub

            Protected Overridable Sub ModifyRigidBodyMuscle(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                            ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.LinearHillMuscle")

                'Rename the muscle attachments to attachments.
                Dim xnMuscleAttachs As XmlNode = m_xnProjectXml.GetNode(xnRigidBody, "MuscleAttachments")
                m_xnProjectXml.AppendNode(xnRigidBody, xnMuscleAttachs, "Attachments")
                m_xnProjectXml.RemoveNode(xnRigidBody, "MuscleAttachments")

                'Move the Pe data from the muscle to the length-tension node
                Dim xnLengthTen As XmlNode = m_xnProjectXml.GetNode(xnRigidBody, "LengthTension")
                m_xnProjectXml.CopyScaledNumber(m_xnProjectXml, xnRigidBody, xnLengthTen, "PeLength", "PeLength")
                m_xnProjectXml.CopyScaledNumber(m_xnProjectXml, xnRigidBody, xnLengthTen, "MinPeLength", "MinPeLength")
                m_xnProjectXml.RemoveNode(xnRigidBody, "PeLength")
                m_xnProjectXml.RemoveNode(xnRigidBody, "MinPeLength")

                'Convert the stim-tension A1 to A,B,C,D
                Dim nxStimTen As XmlNode = m_xnProjectXml.GetNode(xnRigidBody, "StimulusTension")
                m_xnProjectXml.CopyScaledNumber(m_xnProjectXml, nxStimTen, nxStimTen, "A1", "A")
                m_xnProjectXml.CopyScaledNumber(m_xnProjectXml, nxStimTen, nxStimTen, "A2", "B")
                m_xnProjectXml.CopyScaledNumber(m_xnProjectXml, nxStimTen, nxStimTen, "A3", "C")
                m_xnProjectXml.CopyScaledNumber(m_xnProjectXml, nxStimTen, nxStimTen, "A4", "D")
                m_xnProjectXml.RemoveNode(nxStimTen, "A1")
                m_xnProjectXml.RemoveNode(nxStimTen, "A2")
                m_xnProjectXml.RemoveNode(nxStimTen, "A3")
                m_xnProjectXml.RemoveNode(nxStimTen, "A4")

            End Sub

            Protected Overridable Sub ModifyRigidBodyStretchReceptor(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                                     ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

                ModifyRigidBodyMuscle(xnRigidBody, aryParentTrasform, aryChildTrasform)

                m_xnProjectXml.RemoveNode(xnRigidBody, "PartType", False)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.LinearHillStretchReceptor")

            End Sub

            Protected Overridable Sub ModifyRigidBodySpring(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                            ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.Spring")

                Dim strPrimID As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "PrimaryAttachmentID")
                Dim strSecID As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "SecondaryAttachmentID")

                Dim xnAttachments As XmlNode = m_xnProjectXml.AddNodeValue(xnRigidBody, "Attachments", "")
                Dim xnPrimAttach As XmlNode = m_xnProjectXml.AddNodeValue(xnAttachments, "AttachID", strPrimID)
                Dim xnSecAttach As XmlNode = m_xnProjectXml.AddNodeValue(xnAttachments, "AttachID", strSecID)

                m_xnProjectXml.RemoveNode(xnRigidBody, "PrimaryAttachmentID")
                m_xnProjectXml.RemoveNode(xnRigidBody, "SecondaryAttachmentID")

            End Sub

            Protected Overridable Sub ModifyRigidBodyMesh(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                          ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

                m_xnProjectXml.RemoveNode(xnRigidBody, "Scale", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "Transparencies")
                m_xnProjectXml.AddTransparency(xnRigidBody, 50, 0, 50, 50, 100)

                Dim strMeshFileOld As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "MeshFile", False, "")
                Dim strExt As String = Util.GetFileExtension(strMeshFileOld)
                Dim strBaseMeshName As String = strMeshFileOld.Replace("." & strExt, "")
                Dim strCollisionMeshFileOld As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "CollisionMeshFile", False, "")
                strExt = Util.GetFileExtension(strCollisionMeshFileOld)
                Dim strBaseColliosionMeshName As String = strCollisionMeshFileOld.Replace("." & strExt, "")
                Dim strCollisionMeshType As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "CollisionMeshType", False, "Convex")
                Dim strTexture As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "Texture", False, "")

                Dim strMeshFile As String = EnsureUniqueMeshName(strBaseMeshName) & ".osg"
                Dim strCollisionMeshFile As String = EnsureUniqueMeshName(strBaseColliosionMeshName) & ".osg"

                If strCollisionMeshType = "Regular" Then
                    strCollisionMeshType = "Triangular"
                End If

                m_xnProjectXml.RemoveNode(xnRigidBody, "MeshFile", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "CollisionMeshFile", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "CollisionMeshType", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "Texture", False)

                'If we have a texture image then we need to flip it first.
                If strTexture.Length > 0 Then
                    Dim bm As New Bitmap(m_strProjectPath & "\" & strTexture)
                    bm.RotateFlip(RotateFlipType.Rotate180FlipX)
                    strTexture = strTexture.Replace(".", "_Converted.")
                    bm.Save(m_strProjectPath & "\" & strTexture, System.Drawing.Imaging.ImageFormat.Bmp)
                End If

                'convert the mesh file to the new work with the new system.
                ConvertV1MeshFile(strMeshFileOld, strMeshFile, strTexture)

                'First create the collision mesh object out of this one.
                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.Mesh")

                m_xnProjectXml.AddNodeValue(xnRigidBody, "MeshFile", strCollisionMeshFile)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "MeshType", strCollisionMeshType)

                If strCollisionMeshType.ToUpper() = "CONVEX" Then
                    strExt = Util.GetFileExtension(strCollisionMeshFileOld)
                    Dim strConvexMeshFile As String = strCollisionMeshFileOld.Replace("." & strExt, "_Convex.osg")
                    m_iSimInterface.GenerateCollisionMeshFile(strCollisionMeshFileOld, strConvexMeshFile, 1, 1, 1)
                    ConvertV1MeshFile(strConvexMeshFile, strCollisionMeshFile, "")

                    m_xnProjectXml.AddNodeValue(xnRigidBody, "ConvexMeshFile", strCollisionMeshFile)
                Else
                    ConvertV1MeshFile(strCollisionMeshFileOld, strCollisionMeshFile, strTexture)
                End If

                AddGraphicsMesh(xnRigidBody, strMeshFile, strTexture)

                'Dim strAseMeshFile As String = strMeshFileOld.Replace(".obj", ".ase")
                'Dim strMtlMeshFile As String = strMeshFileOld.Replace(".obj", ".mtl")
                'Dim strMtlCollisionMeshFile As String = strCollisionMeshFileOld.Replace(".obj", ".mtl")

                ''If File.Exists(m_strProjectPath & "\" & strMeshFile) Then File.Delete(m_strProjectPath & "\" & strMeshFile)
                ''If File.Exists(m_strProjectPath & "\" & strAseMeshFile) Then File.Delete(m_strProjectPath & "\" & strAseMeshFile)
                ''If File.Exists(m_strProjectPath & "\" & strMtlMeshFile) Then File.Delete(m_strProjectPath & "\" & strMtlMeshFile)
                ''If File.Exists(m_strProjectPath & "\" & strCollisionMeshFileOld) Then File.Delete(m_strProjectPath & "\" & strCollisionMeshFileOld)
                ''If File.Exists(m_strProjectPath & "\" & strMtlCollisionMeshFile) Then File.Delete(m_strProjectPath & "\" & strMtlCollisionMeshFile)
                ''If File.Exists(m_strProjectPath & "\" & strConvexMeshFile) Then File.Delete(m_strProjectPath & "\" & strConvexMeshFile)

            End Sub

            Protected Function EnsureUniqueMeshName(ByVal strBaseMeshName As String, Optional ByVal iIndex As Integer = 0) As String

                Dim strTestName As String = strBaseMeshName
                If iIndex > 0 Then
                    strTestName = strTestName & iIndex
                End If

                If m_aryUsedMeshNames.Contains(strTestName) Then
                    Return EnsureUniqueMeshName(strBaseMeshName, iIndex + 1)
                Else
                    m_aryUsedMeshNames.Add(strTestName)
                    Return strTestName
                End If

            End Function

            Protected Sub ConvertV1MeshFile(ByVal strOldFile As String, ByVal strNewFile As String, ByVal strTexture As String)

                m_iSimInterface.ConvertV1MeshFile(strOldFile, strNewFile, strTexture)

                'We need to use relative paths to the image here. So lets replace the absolute path that osg saves in it.
                If strTexture.Length > 0 Then
                    ReplaceFileText(m_strProjectPath & "\" & strNewFile, (m_strProjectPath.Replace("\", "\\") & "\\"), "")
                End If

            End Sub

            Protected Sub ReplaceFileText(ByVal strFilename As String, ByVal strFind As String, ByVal strReplace As String)
                ' Open a file for reading
                Dim srReader As StreamReader
                srReader = File.OpenText(strFilename)
                ' Now, read the entire file into a strin
                Dim contents As String = srReader.ReadToEnd()
                srReader.Close()

                ' Write the modification into the same fil
                Dim swWriter As StreamWriter = File.CreateText(strFilename)

                swWriter.Write(contents.Replace(strFind, strReplace))
                swWriter.Close()
            End Sub

            Protected Overridable Sub AddGraphicsMesh(ByVal xnRigidBody As XmlNode, ByVal strMeshFile As String, ByVal strTexture As String)

                Dim strXml As String = "<Name>" & m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "Name", False, "Root") & "_Graphics</Name>" & vbCrLf & _
                                        "<ID>" & System.Guid.NewGuid.ToString & "</ID>" & vbCrLf & _
                                        "<Converter>Added</Converter>" & vbCrLf & _
                                        "<Description/>" & vbCrLf & _
                                        "<Transparencies>" & vbCrLf & _
                                        "<Graphics>0</Graphics>" & vbCrLf & _
                                        "<Collisions>50</Collisions>" & vbCrLf & _
                                        "<Joints>50</Joints>" & vbCrLf & _
                                        "<RecFields>50</RecFields>" & vbCrLf & _
                                        "<Simulation>0</Simulation>" & vbCrLf & _
                                        "</Transparencies>" & vbCrLf & _
                                        "<IsVisible>True</IsVisible>" & vbCrLf & _
                                        "<Diffuse " & m_xnProjectXml.GetSingleNodeAttributes(xnRigidBody, "Diffuse", False, "Red=""1"" Green=""1"" Blue=""1"" Alpha=""1""") & "/>" & vbCrLf & _
                                        "<Ambient Red=""0.0980392"" Green=""0.0980392"" Blue=""0.0980392"" Alpha=""1""/>" & vbCrLf & _
                                        "<Specular Red=""0.25098"" Green=""0.25098"" Blue=""0.25098"" Alpha=""1""/>" & vbCrLf & _
                                        "<Shininess>64</Shininess>" & vbCrLf & _
                                        "<Texture/>" & _
                                        "<ModuleName>VortexAnimatSim_VC" & Util.Application.SimVCVersion & Util.Application.RuntimeModePrefix & ".dll</ModuleName>" & vbCrLf & _
                                        "<LocalPosition>" & vbCrLf & _
                                        "<X Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                                        "<Y Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                                        "<Z Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                                        "</LocalPosition>" & vbCrLf & _
                                        "<Rotation>" & vbCrLf & _
                                        "<X Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                                        "<Y Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                                        "<Z Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                                        "</Rotation>" & vbCrLf & _
                                        "<Type>Mesh</Type>" & vbCrLf & _
                                        "<PartType>AnimatGUI.DataObjects.Physical.Bodies.Mesh</PartType>" & vbCrLf & _
                                        "<IsContactSensor>False</IsContactSensor>" & vbCrLf & _
                                        "<IsCollisionObject>False</IsCollisionObject>" & vbCrLf & _
                                        "<BuoyancyCenter>" & vbCrLf & _
                                        "<X Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                                        "<Y Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                                        "<Z Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                                        "</BuoyancyCenter>" & vbCrLf & _
                                        "<BuoyancyScale>1</BuoyancyScale>" & vbCrLf & _
                                        "<Drag>" & vbCrLf & _
                                        "<X Value=""1"" Scale=""None"" Actual=""1""/>" & vbCrLf & _
                                        "<Y Value=""1"" Scale=""None"" Actual=""1""/>" & vbCrLf & _
                                        "<Z Value=""1"" Scale=""None"" Actual=""1""/>" & vbCrLf & _
                                        "</Drag>" & vbCrLf & _
                                        "<Magnus>0</Magnus>" & vbCrLf & _
                                        "<EnableFluids>False</EnableFluids>" & vbCrLf & _
                                        "<Density Value=""1"" Scale=""None"" Actual=""1""/>" & vbCrLf & _
                                        "<COM>" & vbCrLf & _
                                        "<X Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                                        "<Y Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                                        "<Z Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                                        "</COM>" & vbCrLf & _
                                        "<MaterialTypeID>DEFAULTMATERIAL</MaterialTypeID>" & vbCrLf & _
                                        "<FoodSource>False</FoodSource>" & vbCrLf & _
                                        "<MeshFile>" & strMeshFile & "</MeshFile>" & vbCrLf & _
                                        "<MeshType>Triangular</MeshType>" & vbCrLf

                Dim xnChildBodies As XmlNode = m_xnProjectXml.GetNode(xnRigidBody, "ChildBodies", False)
                If xnChildBodies Is Nothing Then
                    xnChildBodies = m_xnProjectXml.AddNodeValue(xnRigidBody, "ChildBodies", "")
                    xnChildBodies.RemoveAll() 'Remove any automatically added child nodes.
                End If

                Dim xnGraphics As XmlNode = m_xnProjectXml.AddNodeXml(xnChildBodies, "RigidBody", strXml)

                'For some reason it is adding a blank node to the child bodies.

                Debug.WriteLine("ChildBodies: " & xnChildBodies.ChildNodes.Count & vbCrLf)

            End Sub

            Protected Overridable Sub ModifyRigidBodyMouth(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                           ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)
                ModifyRigidBodySphere(xnRigidBody, aryParentTrasform, aryChildTrasform, -1)

                m_xnProjectXml.RemoveNode(xnRigidBody, "ID")
                m_xnProjectXml.RemoveNode(xnRigidBody, "PartType", False)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "ID", m_strMouthID)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.Mouth")

                m_xnMouth = xnRigidBody

                'If stomach is already set then use it.
                If Not m_xnStomach Is Nothing Then
                    'Dim strStomachID As String = m_xnProjectXml.GetSingleNodeValue(m_xnStomach, "ID")
                    m_xnProjectXml.AddNodeValue(m_xnMouth, "StomachID", m_strStomachID)
                End If
            End Sub

            Protected Overridable Sub ModifyRigidBodyStomach(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                             ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)
                ModifyRigidBodySphere(xnRigidBody, aryParentTrasform, aryChildTrasform, -1)

                m_xnProjectXml.RemoveNode(xnRigidBody, "ID")
                m_xnProjectXml.RemoveNode(xnRigidBody, "PartType", False)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "ID", m_strStomachID)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.Stomach")
                m_xnProjectXml.AddNodeValue(xnRigidBody, "ModuleName", "VortexAnimatSim_VC" & Util.Application.SimVCVersion & Util.Application.RuntimeModePrefix & ".dll")

                m_xnStomach = xnRigidBody

                'If mouth is already set then use it.
                If Not m_xnMouth Is Nothing Then
                    'Dim strStomachID As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "ID")
                    m_xnProjectXml.AddNodeValue(m_xnMouth, "StomachID", m_strStomachID)
                End If

            End Sub

            Protected Overridable Sub ModifyRigidBodyOdorSensor(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                                ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)
                ModifyRigidBodySphere(xnRigidBody, aryParentTrasform, aryChildTrasform, -1)

                m_xnProjectXml.RemoveNode(xnRigidBody, "PartType", False)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.OdorSensor")

            End Sub

            Protected Overridable Sub ModifyRigidBodySphere(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                            ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, ByVal fltReceptiveFieldDistance As Single)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.Sphere")

                Dim fltRadius As Single = CSng(m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "Radius")) * m_fltDistanceUnits

                m_xnProjectXml.RemoveNode(xnRigidBody, "Radius", False)
                m_xnProjectXml.AddScaledNumber(xnRigidBody, "Radius", fltRadius, "None", fltRadius)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "LatitudeSegments", "20")
                m_xnProjectXml.AddNodeValue(xnRigidBody, "LongtitudeSegments", "20")

            End Sub

            Protected Overridable Sub ModifyRigidBodyPlane(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                           ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, ByVal bIsFluidPlane As Boolean)

                If bIsFluidPlane Then
                    m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.FluidPlane")

                    'Set the density of the fluid plane to be the fluid density from the old sim.
                    m_xnProjectXml.RemoveNode(xnRigidBody, "Density")
                    m_xnProjectXml.AddScaledNumber(xnRigidBody, "Density", m_dblFluidDensity, "None", m_dblFluidDensity)

                    'Add fluid velocity
                    m_xnProjectXml.AddScaledVector(xnRigidBody, "Velocity", 0, 0, 0)
                Else
                    m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.Plane")
                End If

                m_xnProjectXml.RemoveNode(xnRigidBody, "Rotation")
                m_xnProjectXml.AddScaledVector(xnRigidBody, "Rotation", -90, 0, 0)

                Dim fltSize As Single = 100 * m_fltDistanceUnits
                Dim fltSegSize As Single = 10 * m_fltDistanceUnits

                m_xnProjectXml.AddScaledVector(xnRigidBody, "Size", fltSize, fltSize, 0)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "WidthSegments", fltSegSize.ToString)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "LengthSegments", fltSegSize.ToString)

            End Sub

            Protected Overridable Sub ModifyMultiSegmentSpring(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, _
                                                           ByRef aryChildTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix, ByVal bIsFluidPlane As Boolean)

                Dim strWarn As String = "This project contains a multi-segment spring. These are no longer supported in AnimatLab. " & _
                                        "Would you like to try and remove it now, or abort this operation and swap that part out in " & _
                                        "AnimatLab 1. If you remove it now then any referneces to it will error out while loading and be " & _
                                        "removed. Should I remove it Now?"

                If Util.ShowMessage(strWarn, "Part not supported", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                    'We no longer support multi-segment springs. Remove it from the file.
                    xnRigidBody.ParentNode.RemoveChild(xnRigidBody)
                Else
                    Throw New System.Exception("Process aborted.")
                End If

            End Sub

#End Region

#End Region

#Region "Joint Modifiers"

            Protected Overridable Sub ModifyJoint(ByVal xnJoint As XmlNode, ByRef aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

                m_xnProjectXml.AddTransparency(xnJoint, 50, 50, 0, 50, 50)

                ModifyRigidBodyColor(xnJoint)

                m_xnProjectXml.RemoveNode(xnJoint, "PartType", False)
                m_xnProjectXml.RemoveNode(xnJoint, "Rotation", False)
                m_xnProjectXml.RemoveNode(xnJoint, "RelativePosition", False)
                m_xnProjectXml.RemoveNode(xnJoint, "RotationAxis", False)

                Dim aryOrientation As AnimatGuiCtrls.MatrixLibrary.Matrix = m_xnProjectXml.LoadOrientationPositionMatrix(xnJoint, "TranslationMatrix", "OrientationMatrix")
                Dim aryJointConv As AnimatGuiCtrls.MatrixLibrary.Matrix = MakeJointConversionMatrix()
                Dim aryPreTrans As AnimatGuiCtrls.MatrixLibrary.Matrix = AnimatGuiCtrls.MatrixLibrary.Matrix.Multiply(aryJointConv, aryOrientation)
                Dim aryTransform As AnimatGuiCtrls.MatrixLibrary.Matrix = AnimatGuiCtrls.MatrixLibrary.Matrix.Multiply(aryPreTrans, aryParentTrasform)
                Dim aryData(,) As Double = aryTransform.toArray

                Dim oPosRot As ManagedAnimatInterfaces.PositionRotationInfo
                oPosRot = m_iSimInterface.GetPositionAndRotationFromD3DMatrix(aryTransform.toArray)

                m_xnProjectXml.AddScaledVector(xnJoint, "LocalPosition", oPosRot.m_fltXPos * m_fltDistanceUnits, oPosRot.m_fltYPos * m_fltDistanceUnits, oPosRot.m_fltZPos * m_fltDistanceUnits)
                m_xnProjectXml.AddScaledVector(xnJoint, "Rotation", Util.RadiansToDegrees(oPosRot.m_fltXRot), Util.RadiansToDegrees(oPosRot.m_fltYRot), Util.RadiansToDegrees(oPosRot.m_fltZRot))

                m_xnProjectXml.RemoveNode(xnJoint, "OrientationMatrix", False)
                m_xnProjectXml.RemoveNode(xnJoint, "TranslationMatrix", False)
                m_xnProjectXml.RemoveNode(xnJoint, "CombinedTransformationMatrix", False)

                Dim dblRadius As Double = CDbl(m_xnProjectXml.GetSingleNodeValue(xnJoint, "Radius", False, "0.02")) * m_fltDistanceUnits
                m_xnProjectXml.RemoveNode(xnJoint, "Radius", False)
                m_xnProjectXml.AddScaledNumber(xnJoint, "Size", dblRadius, "None", dblRadius)

                Dim strType As String = m_xnProjectXml.GetSingleNodeValue(xnJoint, "Type").ToUpper

                Select Case strType
                    Case "HINGE"
                        ModifyJointHinge(xnJoint)
                    Case "BALLSOCKET"
                        ModifyJointBallSocket(xnJoint)
                    Case "PRISMATIC"
                        ModifyJointPrismatic(xnJoint)
                    Case "STATIC"
                        ModifyJointStatic(xnJoint)
                End Select

                Dim strID As String = m_xnProjectXml.GetSingleNodeValue(xnJoint, "ID")
                m_hashJoints.Add(strID, xnJoint)

            End Sub

#Region "Joint Type Modifiers"

            Protected Overridable Sub ModifyJointHinge(ByVal xnJoint As XmlNode)

                m_xnProjectXml.AddNodeValue(xnJoint, "PartType", "AnimatGUI.DataObjects.Physical.Joints.Hinge")

                ModifyJointConstraints(xnJoint, "-0.785398", "0.785398", True)
            End Sub

            Protected Overridable Sub ModifyJointBallSocket(ByVal xnJoint As XmlNode)

                m_xnProjectXml.AddNodeValue(xnJoint, "PartType", "AnimatGUI.DataObjects.Physical.Joints.BallSocket")

            End Sub

            Protected Overridable Sub ModifyJointPrismatic(ByVal xnJoint As XmlNode)

                m_xnProjectXml.AddNodeValue(xnJoint, "PartType", "AnimatGUI.DataObjects.Physical.Joints.Prismatic")

                ModifyJointConstraints(xnJoint, "-10", "10", False)
            End Sub

            Protected Overridable Sub ModifyJointStatic(ByVal xnJoint As XmlNode)

                m_xnProjectXml.AddNodeValue(xnJoint, "PartType", "AnimatGUI.DataObjects.Physical.Joints.RPRO")

            End Sub

            Protected Overridable Sub ModifyJointConstraints(ByVal xnJoint As XmlNode, ByVal strLowDefault As String, ByVal strHighDefault As String, ByVal bRotational As Boolean)

                Dim xnConstraint As XmlNode = m_xnProjectXml.GetNode(xnJoint, "Constraint", False)

                If Not xnConstraint Is Nothing Then
                    Dim fltLow As Single = CSng(m_xnProjectXml.GetSingleNodeAttribute(xnConstraint, "Low", False, strLowDefault))
                    Dim fltHigh As Single = CSng(m_xnProjectXml.GetSingleNodeAttribute(xnConstraint, "High", False, strHighDefault))

                    If Not bRotational Then
                        fltLow = fltLow * m_fltDistanceUnits
                        fltHigh = fltHigh * m_fltDistanceUnits
                    Else
                        fltLow = Util.RadiansToDegrees(fltLow)
                        fltHigh = Util.RadiansToDegrees(fltHigh)
                    End If

                    Dim bEnableLimits As Boolean = CBool(m_xnProjectXml.GetSingleNodeValue(xnConstraint, "EnableLimits", False, "True"))

                    'Create Lower Constraint Limit
                    Dim strID As String = System.Guid.NewGuid.ToString()
                    Dim strLowerXml As String = "<Name>" & strID & "</Name>" & vbCrLf & _
                     "<ID>" & strID & "</ID>" & vbCrLf & _
                     "<LimitPos Value=""" & fltLow & """ Scale=""None"" Actual=""" & fltLow & """/>" & vbCrLf & _
                     "<Damping " & m_xnProjectXml.GetSingleNodeAttributes(xnConstraint, "Damping", False, "Value=""0"" Scale=""Kilo"" Actual=""0""") & "/>" & vbCrLf & _
                     "<Restitution " & m_xnProjectXml.GetSingleNodeAttributes(xnConstraint, "Restitution", False, "Value=""0"" Scale=""Kilo"" Actual=""0""") & "/>" & vbCrLf & _
                     "<Stiffness " & m_xnProjectXml.GetSingleNodeAttributes(xnConstraint, "Stiffness", False, "Value=""5"" Scale=""Mega"" Actual=""5e+006""") & "/>" & vbCrLf
                    Dim xnLowerLimit As XmlNode = m_xnProjectXml.AddNodeXml(xnJoint, "LowerLimit", strLowerXml)

                    'Create Upper Constraint Limit
                    strID = System.Guid.NewGuid.ToString()
                    Dim strUpperXml As String = "<Name>" & strID & "</Name>" & vbCrLf & _
                     "<ID>" & strID & "</ID>" & vbCrLf & _
                     "<LimitPos Value=""" & fltHigh & """ Scale=""None"" Actual=""" & fltHigh & """/>" & vbCrLf & _
                     "<Damping " & m_xnProjectXml.GetSingleNodeAttributes(xnConstraint, "Damping", False, "Value=""0"" Scale=""Kilo"" Actual=""0""") & "/>" & vbCrLf & _
                     "<Restitution " & m_xnProjectXml.GetSingleNodeAttributes(xnConstraint, "Restitution", False, "Value=""0"" Scale=""Kilo"" Actual=""0""") & "/>" & vbCrLf & _
                     "<Stiffness " & m_xnProjectXml.GetSingleNodeAttributes(xnConstraint, "Stiffness", False, "Value=""5"" Scale=""Mega"" Actual=""5e+006""") & "/>" & vbCrLf
                    Dim xnUpperLimit As XmlNode = m_xnProjectXml.AddNodeXml(xnJoint, "UpperLimit", strUpperXml)

                    'Create Friction Constraint. Turn off by default.
                    strID = System.Guid.NewGuid.ToString()
                    Dim strFrictionXml As String = "<AssemblyFile>AnimatGUI.dll</AssemblyFile>" & vbCrLf & _
                                "<ClassName>AnimatGUI.DataObjects.Physical.ConstraintFriction</ClassName>" & vbCrLf & _
                                "<ID>" & strID & "</ID>" & vbCrLf & _
                                "<Name/>" & vbCrLf & _
                                "<Enabled>False</Enabled>" & vbCrLf & _
                                "<Proportional>True</Proportional>" & vbCrLf & _
                                "<Coefficient Value=""0.02"" Scale=""None"" Actual=""0.02""/>" & vbCrLf & _
                                "<MaxForce Value=""10"" Scale=""None"" Actual=""10""/>" & vbCrLf & _
                                "<Loss Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                                "<StaticFrictionScale Value=""1"" Scale=""None"" Actual=""1""/>"
                    Dim xnFriction As XmlNode = m_xnProjectXml.AddNodeXml(xnJoint, "Friction", strFrictionXml)


                    'Add the enable limits
                    m_xnProjectXml.AddNodeValue(xnJoint, "EnableLimits", bEnableLimits.ToString)

                    'Remove the constraint.
                    m_xnProjectXml.RemoveNode(xnJoint, "Constraint")
                End If

                Dim dblMaxTorque As Double = CDbl(m_xnProjectXml.GetScaleNumberValue(xnJoint, "MaxTorque", False, 100))
                m_xnProjectXml.RemoveNode(xnJoint, "MaxTorque", False)
                m_xnProjectXml.AddScaledNumber(xnJoint, "MaxForce", dblMaxTorque, "None", dblMaxTorque)

            End Sub

#End Region

#End Region

            Protected Overridable Sub AddLight(ByVal xnEnvironment As XmlNode)

                Util.Application.AppStatusText = "Adding lights"

                Dim fltDistY As Single = 20 * m_fltDistanceUnits
                Dim fltDistXZ As Single = 10 * m_fltDistanceUnits
                Dim fltAttenuation As Single = 750 * m_fltDistanceUnits

                Dim strXml As String = "<Light>" & _
                                       "    <Name>Light_1</Name>" & _
                                       "   <ID>" & System.Guid.NewGuid.ToString & "</ID>" & _
                                       "    <Description/>" & _
                                       "    <Transparencies>" & _
                                       "        <Graphics>50</Graphics>" & _
                                       "        <Collisions>50</Collisions>" & _
                                       "        <Joints>50</Joints>" & _
                                       "        <RecFields>50</RecFields>" & _
                                       "        <Simulation>100</Simulation>" & _
                                       "    </Transparencies>" & _
                                       "    <IsVisible>True</IsVisible>" & _
                                       "    <Ambient Red=""1"" Green=""1"" Blue=""1"" Alpha=""1""/>" & _
                                       "    <Diffuse Red=""1"" Green=""1"" Blue=""1"" Alpha=""1""/>" & _
                                       "    <Specular Red=""1"" Green=""1"" Blue=""1"" Alpha=""1""/>" & _
                                       "    <Shininess>64</Shininess>" & _
                                       "    <Texture/>" & _
                                       "    <LocalPosition>" & _
                                       "        <X Value=""" & fltDistXZ & """ Scale=""None"" Actual=""" & fltDistXZ & """/>" & _
                                       "        <Y Value=""" & fltDistY & """ Scale=""None"" Actual=""" & fltDistY & """/>" & _
                                       "        <Z Value=""" & fltDistXZ & """ Scale=""None"" Actual=""" & fltDistXZ & """/>" & _
                                       "    </LocalPosition>" & _
                                       "    <Rotation>" & _
                                       "        <X Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "        <Y Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "        <Z Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "    </Rotation>" & _
                                       "    <Radius Value=""50"" Scale=""milli"" Actual=""0.05""/>" & _
                                       "    <ConstantAttenuation>0</ConstantAttenuation>" & _
                                       "    <LinearAttenuationDistance Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "    <QuadraticAttenuationDistance Value=""" & fltAttenuation & """ Scale=""None"" Actual=""" & fltAttenuation & """/>" & _
                                       "    <LatitudeSegments>10</LatitudeSegments>" & _
                                       "    <LongtitudeSegments>10</LongtitudeSegments>" & _
                                       "</Light>" & _
                                       "<Light>" & _
                                       "    <Name>Light_2</Name>" & _
                                       "   <ID>" & System.Guid.NewGuid.ToString & "</ID>" & _
                                       "    <Description/>" & _
                                       "    <Transparencies>" & _
                                       "        <Graphics>50</Graphics>" & _
                                       "        <Collisions>50</Collisions>" & _
                                       "        <Joints>50</Joints>" & _
                                       "        <RecFields>50</RecFields>" & _
                                       "        <Simulation>100</Simulation>" & _
                                       "    </Transparencies>" & _
                                       "    <IsVisible>True</IsVisible>" & _
                                       "    <Ambient Red=""1"" Green=""1"" Blue=""1"" Alpha=""1""/>" & _
                                       "    <Diffuse Red=""1"" Green=""1"" Blue=""1"" Alpha=""1""/>" & _
                                       "    <Specular Red=""1"" Green=""1"" Blue=""1"" Alpha=""1""/>" & _
                                       "    <Shininess>64</Shininess>" & _
                                       "    <Texture/>" & _
                                       "    <LocalPosition>" & _
                                       "        <X Value=""" & -fltDistXZ & """ Scale=""None"" Actual=""" & -fltDistXZ & """/>" & _
                                       "        <Y Value=""" & fltDistY & """ Scale=""None"" Actual=""" & fltDistY & """/>" & _
                                       "        <Z Value=""" & -fltDistXZ & """ Scale=""None"" Actual=""" & -fltDistXZ & """/>" & _
                                       "    </LocalPosition>" & _
                                       "    <Rotation>" & _
                                       "        <X Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "        <Y Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "        <Z Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "    </Rotation>" & _
                                       "    <Radius Value=""50"" Scale=""milli"" Actual=""0.05""/>" & _
                                       "    <ConstantAttenuation>0.7</ConstantAttenuation>" & _
                                       "    <LinearAttenuationDistance Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "    <QuadraticAttenuationDistance Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "    <LatitudeSegments>10</LatitudeSegments>" & _
                                       "    <LongtitudeSegments>10</LongtitudeSegments>" & _
                                       "</Light>"

                Dim xmlLights As XmlNode = m_xnProjectXml.AddNodeXml(xnEnvironment, "Lights", strXml)

            End Sub

            Protected Overridable Sub AddDefaultMaterial(ByVal xnEnvironment As XmlNode)

                Util.Application.AppStatusText = "Adding default materials"

                Dim strXml As String = "<MaterialType>" & vbCrLf & _
                            "<AssemblyFile>AnimatGUI.dll</AssemblyFile>" & vbCrLf & _
                            "<ClassName>AnimatGUI.DataObjects.Physical.MaterialType</ClassName>" & vbCrLf & _
                            "<ID>DEFAULTMATERIAL</ID>" & vbCrLf & _
                            "<Name>Default</Name>" & vbCrLf & _
                            "<FrictionLinearPrimary Value=""1"" Scale=""None"" Actual=""1""/>" & vbCrLf & _
                            "<FrictionLinearSecondary Value=""1"" Scale=""None"" Actual=""1""/>" & vbCrLf & _
                            "<FrictionAngularNormal Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<FrictionAngularPrimary Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<FrictionAngularSecondary Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<FrictionLinearPrimaryMax Value=""1"" Scale=""None"" Actual=""1""/>" & vbCrLf & _
                            "<FrictionLinearSecondaryMax Value=""1"" Scale=""None"" Actual=""1""/>" & vbCrLf & _
                            "<FrictionAngularNormalMax Value=""1"" Scale=""None"" Actual=""1""/>" & vbCrLf & _
                            "<FrictionAngularPrimaryMax Value=""1"" Scale=""None"" Actual=""1""/>" & vbCrLf & _
                            "<FrictionAngularSecondaryMax Value=""1"" Scale=""None"" Actual=""1""/>" & vbCrLf & _
                            "<Compliance Value=""1"" Scale=""nano"" Actual=""1e-009""/>" & vbCrLf & _
                            "<Damping Value=""50"" Scale=""Giga"" Actual=""5e+010""/>" & vbCrLf & _
                            "<Restitution Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<SlipLinearPrimary Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<SlipLinearSecondary Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<SlipAngularNormal Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<SlipAngularPrimary Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<SlipAngularSecondary Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<SlideLinearPrimary Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<SlideLinearSecondary Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<SlideAngularNormal Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<SlideAngularPrimary Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<SlideAngularSecondary Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                            "<MaximumAdhesion Value=""0"" Scale=""None"" Actual=""0""/>" & vbCrLf & _
                        "</MaterialType>" & vbCrLf

                Dim xmlTypes As XmlNode = m_xnProjectXml.AddNodeXml(xnEnvironment, "MaterialTypes", strXml)

            End Sub

            Protected Overridable Sub ModifySurface(ByVal strType As String, ByVal xnEnvironment As XmlNode, ByVal xnStructs As XmlNode, ByVal aryReplaceText As Hashtable, ByVal bIsFluidPlane As Boolean)

                Util.Application.AppStatusText = "Modifying " & strType & " surface"

                Dim xnGround As XmlNode = m_xnProjectXml.GetNode(xnEnvironment, strType & "Surface", False)
                If Not xnGround Is Nothing Then

                    Dim xnStruct As XmlNode = m_xnProjectXml.AppendNode(xnStructs, xnGround.InnerXml, "Structure", aryReplaceText)

                    ModifyStructure(xnStruct, bIsFluidPlane, True)
                End If

            End Sub

#End Region

#Region "Modify Nervous System"

            Protected Overridable Sub ModifyNervousSystem(ByVal xnOrganism As XmlNode)

                Dim strName As String = m_xnProjectXml.GetSingleNodeValue(xnOrganism, "Name", False)
                Util.Application.AppStatusText = "Converting organism " & strName & " nervous system"

                m_arySubsystemIDs.Clear()

                Dim xnNervousSys As XmlNode = m_xnProjectXml.AddNodeValue(xnOrganism, "NervousSystem", "")

                Dim xnBodyFile As Framework.XmlDom = LoadBehavioralFile(xnOrganism)

                If Not xnBodyFile Is Nothing Then
                    Dim xnOldRoot As XmlNode = xnBodyFile.GetRootNode("Editor")
                    Dim xnOldNeuralMods As XmlNode = xnBodyFile.GetNode(xnOldRoot, "NeuralModules")

                    CreateNeuralModules(xnNervousSys, xnBodyFile, xnOldNeuralMods)

                    Dim xnOldDiagrams As XmlNode = m_xnProjectXml.GetNode(xnOldRoot, "Diagrams")

                    Dim xnRootSubSystem As XmlNode
                    Dim xnRootSubNodes As XmlNode
                    If xnOldDiagrams.ChildNodes.Count = 1 Then
                        Dim xnOldDiagram As XmlNode = xnOldDiagrams.FirstChild
                        xnRootSubSystem = ConvertRootDiagramToSubsystem(xnBodyFile, xnNervousSys, xnOldDiagram)
                        xnRootSubNodes = m_xnProjectXml.GetNode(xnRootSubSystem, "Nodes")
                    Else
                        Dim aryReplaceText As Hashtable = CreateReplaceStringList()

                        xnRootSubSystem = CreateDefaultRootDiagram(xnBodyFile, xnNervousSys)
                        xnRootSubNodes = m_xnProjectXml.GetNode(xnRootSubSystem, "Nodes")

                        For Each xnOldDiagram As XmlNode In xnOldDiagrams.ChildNodes
                            CreateChildRootDiagram(xnBodyFile, xnRootSubNodes, xnOldDiagram)
                        Next

                    End If

                    VerifyAllDiagramsConverted(xnBodyFile, xnRootSubNodes, xnOldDiagrams)
                Else
                    CreateDefaultNervousSystem(xnNervousSys)
                End If

            End Sub

            Protected Overridable Function LoadBehavioralFile(ByVal xnOrganism As XmlNode) As Framework.XmlDom

                'We need to get the simulation node from the asim file. We will then modify it.
                Dim strBodyFile As String = m_strProjectPath & "\" & m_xnProjectXml.GetSingleNodeValue(xnOrganism, "Name") & ".abef"

                Dim xnBodyFile As Framework.XmlDom = Nothing

                If File.Exists(strBodyFile) Then
                    xnBodyFile = New Framework.XmlDom
                    xnBodyFile.Load(strBodyFile)

                    m_xnProjectXml.RemoveNode(xnOrganism, "BodyPlan")
                    m_xnProjectXml.RemoveNode(xnOrganism, "BehavioralSystem")
                End If

                Return xnBodyFile
            End Function

            Protected Overridable Sub CreateDefaultNervousSystem(ByVal xnNervousSys As XmlNode)

                Dim strXml As String = "<NeuralModules/>" & vbCrLf & _
                                "<Node>" & vbCrLf & _
                                "<AssemblyFile>AnimatGUI.dll</AssemblyFile>" & vbCrLf & _
                                "<ClassName>AnimatGUI.DataObjects.Behavior.Nodes.Subsystem</ClassName>" & vbCrLf & _
                                "<ID>" & System.Guid.NewGuid.ToString & "</ID>" & vbCrLf & _
                                "<Alignment>CenterMiddle</Alignment>" & vbCrLf & _
                                "<AutoSize>None</AutoSize>" & vbCrLf & _
                                "<BackMode>Transparent</BackMode>" & vbCrLf & _
                                "<DashStyle>Solid</DashStyle>" & vbCrLf & _
                                "<DrawColor>-16777216</DrawColor>" & vbCrLf & _
                                "<DrawWidth>1</DrawWidth>" & vbCrLf & _
                                "<FillColor>-8586240</FillColor>" & vbCrLf & _
                                "<Font Family=""Arial"" Size=""12"" Bold=""True"" Underline=""False"" Strikeout=""False"" Italic=""False""/>" & vbCrLf & _
                                "<Gradient>False</Gradient>" & vbCrLf & _
                                "<GradientColor>0</GradientColor>" & vbCrLf & _
                                "<GradientMode>BackwardDiagonal</GradientMode>" & vbCrLf & _
                                "<DiagramImageName/>" & vbCrLf & _
                                "<ImageName/>" & vbCrLf & _
                                "<ImageLocation x=""0"" y=""0""/>" & vbCrLf & _
                                "<ImagePosition>RelativeToText</ImagePosition>" & vbCrLf & _
                                "<InLinkable>True</InLinkable>" & vbCrLf & _
                                "<LabelEdit>True</LabelEdit>" & vbCrLf & _
                                "<Location x=""0"" y=""0""/>" & vbCrLf & _
                                "<OutLinkable>True</OutLinkable>" & vbCrLf & _
                                "<ShadowStyle>None</ShadowStyle>" & vbCrLf & _
                                "<ShadowColor>-16777216</ShadowColor>" & vbCrLf & _
                                "<ShadowSize Width=""0"" Height=""0""/>" & vbCrLf & _
                                "<Shape>Rectangle</Shape>" & vbCrLf & _
                                "<ShapeOrientation>Angle0</ShapeOrientation>" & vbCrLf & _
                                "<Size Width=""40"" Height=""40""/>" & vbCrLf & _
                                "<Text>Neural Subsystem</Text>" & vbCrLf & _
                                "<TextColor>-16777216</TextColor>" & vbCrLf & _
                                "<TextMargin Width=""0"" Height=""0""/>" & vbCrLf & _
                                "<ToolTip/>" & vbCrLf & _
                                "<Transparent>False</Transparent>" & vbCrLf & _
                                "<Url/>" & vbCrLf & _
                                "<XMoveable>True</XMoveable>" & vbCrLf & _
                                "<XSizeable>True</XSizeable>" & vbCrLf & _
                                "<YMoveable>True</YMoveable>" & vbCrLf & _
                                "<YSizeable>True</YSizeable>" & vbCrLf & _
                                "<ZOrder>0</ZOrder>" & vbCrLf & _
                                "<InLinks/>" & vbCrLf & _
                                "<OutLinks/>" & vbCrLf & _
                                "<Nodes/>" & vbCrLf & _
                                "<Links/>" & vbCrLf & _
                                "<DiagramXml>" & vbCrLf & _
                                "<![CDATA[]]>" & vbCrLf & _
                                "</DiagramXml>" & vbCrLf & _
                                "</Node>"

                xnNervousSys.InnerXml = strXml

            End Sub


#Region "NeuralModules"

            Protected Overridable Sub CreateNeuralModules(ByVal xnNervousSystem As XmlNode, ByVal xnBodyFile As Framework.XmlDom, ByVal xnOldNeuralMods As XmlNode)


                Dim xnNewNeuralMods As XmlNode = m_xnProjectXml.AddNodeValue(xnNervousSystem, "NeuralModules", "")

                For Each xnOldMod As XmlNode In xnOldNeuralMods.ChildNodes
                    CreateNeuralModule(xnBodyFile, xnOldMod, xnNewNeuralMods)
                Next

            End Sub

            Protected Overridable Sub CreateNeuralModule(ByVal xnBodyFile As Framework.XmlDom, ByVal xnOldMod As XmlNode, ByVal xnNewNeuralMods As XmlNode)

                'Get the mod type and create based on that.
                Dim strType As String = xnBodyFile.GetSingleNodeValue(xnOldMod, "AssemblyFile").ToUpper

                Select Case strType
                    Case "ANIMATTOOLS.DLL"
                        CreateAnimatToolsNeuralModule(xnBodyFile, xnOldMod, xnNewNeuralMods)
                    Case "FASTNEURALNETTOOLS.DLL"
                        CreateFastNeuralModule(xnBodyFile, xnOldMod, xnNewNeuralMods)
                    Case "REALISTICNEURALNETTOOLS.DLL"
                        CreateRealisticToolsNeuralModule(xnBodyFile, xnOldMod, xnNewNeuralMods)
                    Case "FPGANEURALNETTOOLS.DLL"
                        'Do nothing.
                End Select

            End Sub

            Protected Overridable Sub CreateAnimatToolsNeuralModule(ByVal xnBodyFile As Framework.XmlDom, ByVal xnOldMod As XmlNode, ByVal xnNewNeuralMods As XmlNode)
                Dim xnNewNeuralMod As XmlNode = m_xnProjectXml.AddNodeValue(xnNewNeuralMods, "Node", "")

                m_xnProjectXml.AddNodeValue(xnNewNeuralMod, "ID", System.Guid.NewGuid.ToString)
                m_xnProjectXml.AddNodeValue(xnNewNeuralMod, "AssemblyFile", "AnimatGUI.dll")
                m_xnProjectXml.AddNodeValue(xnNewNeuralMod, "ClassName", "AnimatGUI.DataObjects.Behavior.PhysicsModule")
                m_xnProjectXml.CopyScaledNumber(xnBodyFile, xnOldMod, xnNewNeuralMod, "TimeStep", "TimeStep")
            End Sub

            Protected Overridable Sub CreateFastNeuralModule(ByVal xnBodyFile As Framework.XmlDom, ByVal xnOldMod As XmlNode, ByVal xnNewNeuralMods As XmlNode)
                Dim xnNewNeuralMod As XmlNode = m_xnProjectXml.AddNodeValue(xnNewNeuralMods, "Node", "")

                m_xnProjectXml.AddNodeValue(xnNewNeuralMod, "ID", System.Guid.NewGuid.ToString)
                m_xnProjectXml.AddNodeValue(xnNewNeuralMod, "AssemblyFile", "FiringRateGUI.dll")
                m_xnProjectXml.AddNodeValue(xnNewNeuralMod, "ClassName", "FiringRateGUI.DataObjects.Behavior.NeuralModule")
                m_xnProjectXml.CopyScaledNumber(xnBodyFile, xnOldMod, xnNewNeuralMod, "TimeStep", "TimeStep")

            End Sub

            Protected Overridable Sub CreateRealisticToolsNeuralModule(ByVal xnBodyFile As Framework.XmlDom, ByVal xnOldMod As XmlNode, ByVal xnNewNeuralMods As XmlNode)

                Dim aryReplaceText As Hashtable = CreateReplaceStringList()

                Dim xnNewNeuralMod As XmlNode = m_xnProjectXml.AppendNode(xnNewNeuralMods, xnOldMod, "Node", aryReplaceText)
                m_xnProjectXml.AddNodeValue(xnNewNeuralMod, "ID", System.Guid.NewGuid.ToString)

            End Sub

#End Region

#Region "Diagrams"

            Protected Overridable Function ConvertRootDiagramToSubsystem(ByVal xnBodyFile As Framework.XmlDom, ByVal xnNervousSys As XmlNode, ByVal xnOldDiagram As XmlNode) As XmlNode
                Dim strDiagramID As String = xnBodyFile.GetSingleNodeValue(xnOldDiagram, "ID")
                Dim xnRootSubSystem As XmlNode = CreateRootSubsystem(xnNervousSys, strDiagramID, "Neural Subsystem", False)
                ModifySubSystem(xnBodyFile, xnRootSubSystem)
                Return xnRootSubSystem
            End Function

            Protected Overridable Function CreateDefaultRootDiagram(ByVal xnBodyFile As Framework.XmlDom, ByVal xnNervousSys As XmlNode) As XmlNode
                Dim xnRootSubSystem As XmlNode = CreateRootSubsystem(xnNervousSys, System.Guid.NewGuid.ToString, "Neural Subsystem", True)
                Return xnRootSubSystem
            End Function

            Protected Overridable Function CreateChildRootDiagram(ByVal xnBodyFile As Framework.XmlDom, ByVal xnRootDiagram As XmlNode, ByVal xnOldDiagram As XmlNode) As XmlNode
                Dim strDiagramID As String = xnBodyFile.GetSingleNodeValue(xnOldDiagram, "ID")
                Dim strDiagramName As String = xnBodyFile.GetSingleNodeValue(xnOldDiagram, "PageName")
                Dim xnRootSubSystem As XmlNode = CreateRootSubsystem(xnRootDiagram, strDiagramID, strDiagramName, False)
                ModifySubSystem(xnBodyFile, xnRootSubSystem)
                Return xnRootSubSystem
            End Function

            Protected Overridable Function CreateRootSubsystem(ByVal xnNervousSys As XmlNode, ByVal strDiagramID As String, ByVal strDiagramName As String, ByVal bAddNodes As Boolean) As XmlNode

                Dim strXml As String = "<AssemblyFile>AnimatGUI.dll</AssemblyFile>" & _
                                    "<ClassName>AnimatGUI.DataObjects.Behavior.Nodes.Subsystem</ClassName>" & _
                                    "<ID>" & System.Guid.NewGuid.ToString & "</ID>" & _
                                    "<Alignment>CenterMiddle</Alignment>" & _
                                    "<AutoSize>None</AutoSize>" & _
                                    "<BackMode>Transparent</BackMode>" & _
                                    "<DashStyle>Solid</DashStyle>" & _
                                    "<DrawColor>-16777216</DrawColor>" & _
                                    "<DrawWidth>1</DrawWidth>" & _
                                    "<FillColor>-8586240</FillColor>" & _
                                    "<Font Family=""Arial"" Size=""12"" Bold=""True"" Underline=""False"" Strikeout=""False"" Italic=""False""/>" & _
                                    "<Gradient>False</Gradient>" & _
                                    "<GradientColor>0</GradientColor>" & _
                                    "<GradientMode>BackwardDiagonal</GradientMode>" & _
                                     "<DiagramImageName/>" & _
                                     "<ImageName/>" & _
                                     "<ImageLocation x=""0"" y=""0""/>" & _
                                     "<ImagePosition>RelativeToText</ImagePosition>" & _
                                     "<InLinkable>True</InLinkable>" & _
                                     "<LabelEdit>True</LabelEdit>" & _
                                     "<Location x=""0"" y=""0""/>" & _
                                     "<OutLinkable>True</OutLinkable>" & _
                                     "<ShadowStyle>None</ShadowStyle>" & _
                                     "<ShadowColor>-16777216</ShadowColor>" & _
                                     "<ShadowSize Width=""0"" Height=""0""/>" & _
                                     "<Shape>Rectangle</Shape>" & _
                                     "<ShapeOrientation>Angle0</ShapeOrientation>" & _
                                     "<Size Width=""40"" Height=""40""/>" & _
                                     "<Text>" & strDiagramName & "</Text>" & _
                                     "<TextColor>-16777216</TextColor>" & _
                                     "<TextMargin Width=""0"" Height=""0""/>" & _
                                     "<ToolTip/>" & _
                                     "<Transparent>False</Transparent>" & _
                                     "<Url/>" & _
                                     "<XMoveable>True</XMoveable>" & _
                                     "<XSizeable>True</XSizeable>" & _
                                     "<YMoveable>True</YMoveable>" & _
                                     "<YSizeable>True</YSizeable>" & _
                                     "<ZOrder>0</ZOrder>" & _
                                     "<InLinks/>" & _
                                     "<OutLinks/>" & _
                                     "<SubsystemID>" & strDiagramID & "</SubsystemID>"

                If bAddNodes Then
                    strXml = strXml & "<Nodes/><Links/>"
                End If

                Return m_xnProjectXml.AppendNode(xnNervousSys, strXml, "Node")

            End Function


            Protected Overridable Sub ModifySubSystem(ByVal xnBodyFile As Framework.XmlDom, ByVal xnSubSystem As XmlNode)

                Dim strName As String = m_xnProjectXml.GetSingleNodeValue(xnBodyFile, "PageName", False)
                Util.Application.AppStatusText = "Converting neural subsystem " & strName

                Dim strSubSystemID As String = m_xnProjectXml.GetSingleNodeValue(xnSubSystem, "SubsystemID")
                m_arySubsystemIDs.Add(strSubSystemID.ToLower)

                Dim xnRoot As XmlNode = xnBodyFile.GetRootNode("Editor")
                Dim xnOldDiagramID As XmlNode = xnBodyFile.GetNode(xnRoot, "//ID[text()=""" & strSubSystemID & """]")

                If xnOldDiagramID Is Nothing Then
                    Throw New System.Exception("No diagram was found with the ID '" & strSubSystemID & "'")
                End If

                Dim xnOldDiagram As XmlNode = xnOldDiagramID.ParentNode
                m_xnProjectXml.RemoveNode(xnSubSystem, "SubsystemID")

                Dim aryReplaceText As Hashtable = CreateReplaceStringList()

                'Copy nodes
                Dim xnOldNodes As XmlNode = xnBodyFile.GetNode(xnOldDiagram, "Nodes")
                Dim xnNewNodes As XmlNode = m_xnProjectXml.AppendNode(xnSubSystem, xnOldNodes, "Nodes", aryReplaceText)

                For Each xnNode As XmlNode In xnNewNodes
                    ModifyNeuralNode(xnSubSystem, xnNode)
                Next

                'Copy Liniks
                Dim xnOldLinks As XmlNode = xnBodyFile.GetNode(xnOldDiagram, "Links")
                Dim xnNewLinks As XmlNode = m_xnProjectXml.AppendNode(xnSubSystem, xnOldLinks, "Links", aryReplaceText)

                'Copy the diagram xml
                ModifySubsystemDiagramXml(xnBodyFile, xnOldDiagram, xnSubSystem)

                'Loop through the xmlnodes and any that are a subsystem lets process them as well.
                For Each xnNode As XmlNode In xnNewNodes.ChildNodes
                    If m_xnProjectXml.GetSingleNodeValue(xnNode, "ClassName").ToUpper = "ANIMATGUI.DATAOBJECTS.BEHAVIOR.NODES.SUBSYSTEM" Then
                        ModifySubSystem(xnBodyFile, xnNode)
                    End If
                Next

            End Sub

            Protected Overridable Sub ModifySubsystemDiagramXml(ByVal xnBodyFile As Framework.XmlDom, ByVal xnOldDiagram As XmlNode, ByVal xnSubSystem As XmlNode)

                Dim strXml As String = "<Root>" & _
                    "<Diagram>" & _
                    "<ID>" & System.Guid.NewGuid.ToString & "</ID>" & vbCrLf & _
                    "<AssemblyFile>LicensedAnimatGUI.dll</AssemblyFile>" & vbCrLf & _
                    "<ClassName>LicensedAnimatGUI.Forms.Behavior.AddFlowDiagram</ClassName>" & vbCrLf & _
                    "<PageName>" & xnBodyFile.GetSingleNodeValue(xnOldDiagram, "PageName", False, "Page 1") & "</PageName>" & vbCrLf & _
                    "<ZoomX>" & xnBodyFile.GetSingleNodeValue(xnOldDiagram, "ZoomX", False, "1") & "</ZoomX>" & vbCrLf & _
                    "<ZoomY>" & xnBodyFile.GetSingleNodeValue(xnOldDiagram, "ZoomY", False, "1") & "</ZoomY>" & vbCrLf & _
                    "<BackColor " & xnBodyFile.GetSingleNodeAttributes(xnOldDiagram, "BackColor", False, "Red=""1"" Green=""1"" Blue=""1"" Alpha=""1""") & "/>" & vbCrLf & _
                    "<ShowGrid>" & xnBodyFile.GetSingleNodeValue(xnOldDiagram, "ShowGrid", False, "True") & "</ShowGrid>" & vbCrLf & _
                    "<GridColor" & xnBodyFile.GetSingleNodeAttributes(xnOldDiagram, "BackColor", False, "Red=""0.427451"" Green=""0.427451"" Blue=""0.427451"" Alpha=""1""") & "/>" & vbCrLf & _
                    "<GridSize " & xnBodyFile.GetSingleNodeAttributes(xnOldDiagram, "GridSize", False, "Width=""16"" Height=""16""") & "/>" & vbCrLf & _
                    "<GridStyle>" & xnBodyFile.GetSingleNodeValue(xnOldDiagram, "GridStyle", False, "DottedLines") & "</GridStyle>" & vbCrLf & _
                    "<JumpSize>" & xnBodyFile.GetSingleNodeValue(xnOldDiagram, "JumpSize", False, "Medium") & "</JumpSize>" & vbCrLf & _
                    "<SnapToGrid>" & xnBodyFile.GetSingleNodeValue(xnOldDiagram, "SnapToGrid", False, "False") & "</SnapToGrid>" & vbCrLf

                'Find the addflow node.
                Dim xnAddFlow As XmlNode = xnBodyFile.GetNode(xnOldDiagram, "AddFlow")
                strXml = strXml & "<AddFlow " & xnBodyFile.GetSingleNodeAttributes(xnOldDiagram, "AddFlow") & ">" & _
                                  Util.RemoveImageIndexTags(xnAddFlow.InnerXml()) & "</AddFlow></Diagram></Root>"

                Dim xnDiagramXml As XmlNode = m_xnProjectXml.CreateElement("DiagramXml")
                Dim xnDiagramCData As XmlNode = m_xnProjectXml.CreateCDataSection(strXml)

                xnDiagramXml.AppendChild(xnDiagramCData)
                xnSubSystem.AppendChild(xnDiagramXml)

            End Sub

            Protected Sub ModifyNeuralNode(ByVal xnSubSystem As XmlNode, ByVal xnNode As XmlNode)

                Dim strType As String = m_xnProjectXml.GetSingleNodeValue(xnNode, "ClassName").ToUpper

                If strType.Contains("ADAPTER") Then
                    Dim xnGain As XmlNode = m_xnProjectXml.GetNode(xnNode, "Gain", False)
                    If Not xnGain Is Nothing Then
                        Dim strID As String = m_xnProjectXml.GetSingleNodeValue(xnGain, "ID", False, "")
                        If strID.Trim.Length = 0 OrElse m_aryGainIDs.Contains(strID) Then
                            strID = Guid.NewGuid.ToString
                            m_xnProjectXml.RemoveNode(xnGain, "ID", False)
                            m_xnProjectXml.UpdateSingleNodeValue(xnGain, "ID", strID, False)
                        End If
                        m_aryGainIDs.Add(strID)
                    End If
                End If

                'If this neural node has ion channels then loop through them and check their IDs.
                'The old version of AnimatLab had a bug in it that would duplicate IDs of Ion channels
                ' when they were copied. We need to check for this here and replace any duplicate IDs
                Dim xnIonChannels As XmlNode = m_xnProjectXml.GetNode(xnNode, "IonChannels", False)
                If Not xnIonChannels Is Nothing Then
                    For Each xnIon As XmlNode In xnIonChannels
                        ModifyIonChannel(xnNode, xnIon)
                    Next
                End If

            End Sub

            Protected Sub ModifyIonChannel(ByVal xnNode As XmlNode, ByVal xnIon As XmlNode)

                Dim strID As String = m_xnProjectXml.GetSingleNodeValue(xnIon, "ID", False, "")

                If m_hashIonChannels.ContainsKey(strID) Then
                    strID = System.Guid.NewGuid.ToString
                    m_xnProjectXml.UpdateSingleNodeValue(xnIon, "ID", strID)
                End If

                m_hashIonChannels.Add(strID, xnIon)

            End Sub

            Protected Sub VerifyAllDiagramsConverted(ByVal xnBodyFile As Framework.XmlDom, ByVal xnRootSubNodes As XmlNode, ByVal xnOldDiagrams As XmlNode)

                Dim aryNonConverted As New ArrayList
                FindNonConvertedDiagrams(xnBodyFile, xnOldDiagrams, aryNonConverted)

                For Each xnOldDiagram As XmlNode In aryNonConverted
                    CreateChildRootDiagram(xnBodyFile, xnRootSubNodes, xnOldDiagram)
                Next

            End Sub

            Protected Sub FindNonConvertedDiagrams(ByVal xnBodyFile As Framework.XmlDom, ByVal xnOldDiagrams As XmlNode, ByVal aryNonConverted As ArrayList)

                For Each xnOldDiagram As XmlNode In xnOldDiagrams
                    Dim strId As String = xnBodyFile.GetSingleNodeValue(xnOldDiagram, "ID")
                    If Not m_arySubsystemIDs.Contains(strId.ToLower) Then
                        aryNonConverted.Add(xnOldDiagram)
                    End If

                    Dim xnSubDiagrams As XmlNode = xnBodyFile.GetNode(xnOldDiagram, "Diagrams", False)
                    FindNonConvertedDiagrams(xnBodyFile, xnSubDiagrams, aryNonConverted)
                Next

            End Sub

#End Region

#End Region

#Region "Stimuli Modifiers"

            Protected Overridable Sub ModifyStimuli(ByVal xnProjectNode As XmlNode, ByVal xnSimNode As XmlNode)

                Util.Application.AppStatusText = "Converting stimuli"

                Dim xnStimuli As XmlNode = m_xnProjectXml.GetNode(xnProjectNode, "Stimuli")

                Dim aryReplaceText As Hashtable = CreateReplaceStringList()

                Dim xnNewStimuliNode As XmlNode = m_xnProjectXml.AppendNode(xnSimNode, xnStimuli, "Stimuli", aryReplaceText)

                m_xnProjectXml.RemoveNode(xnProjectNode, "Stimuli")

                For Each xnStimulus As XmlNode In xnNewStimuliNode.ChildNodes
                    Dim strClass As String = m_xnProjectXml.GetSingleNodeValue(xnStimulus, "ClassName").ToUpper

                    If strClass = "ANIMATGUI.DATAOBJECTS.EXTERNALSTIMULI.ENABLER" Then
                        ModifyEnablerStimulus(xnProjectNode, xnSimNode, xnStimulus, aryReplaceText)
                    End If
                Next

            End Sub

            Protected Overridable Sub ModifyEnablerStimulus(ByVal xnProjectNode As XmlNode, ByVal xnSimNode As XmlNode, ByVal xnNewStimuli As XmlNode, ByVal aryReplaceText As Hashtable)

                Dim bEnabledWhenActive As Boolean = CBool(m_xnProjectXml.GetSingleNodeValue(xnNewStimuli, "EnableWhenActive"))
                Dim strPartID As String = m_xnProjectXml.GetSingleNodeValue(xnNewStimuli, "PartID")

                m_xnProjectXml.RemoveNode(xnNewStimuli, "ClassName")
                m_xnProjectXml.RemoveNode(xnNewStimuli, "EnableWhenActive")
                m_xnProjectXml.RemoveNode(xnNewStimuli, "PartID")
                m_xnProjectXml.RemoveNode(xnNewStimuli, "StructureID")
                m_xnProjectXml.RemoveNode(xnNewStimuli, "Equation")

                Dim strPropertyName As String = "Enabled"
                If m_hashJoints.ContainsKey(strPartID) Then strPropertyName = "EnableMotor"

                m_xnProjectXml.AddNodeValue(xnNewStimuli, "ClassName", "AnimatGUI.DataObjects.ExternalStimuli.PropertyControlStimulus")
                m_xnProjectXml.AddNodeValue(xnNewStimuli, "LinkedDataObjectID", strPartID)
                m_xnProjectXml.AddNodeValue(xnNewStimuli, "LinkedDataObjectProperty", strPropertyName)
                m_xnProjectXml.AddNodeValue(xnNewStimuli, "SetThreshold", "0.5")

                If bEnabledWhenActive Then
                    m_xnProjectXml.AddNodeValue(xnNewStimuli, "InitialValue", "1")
                    m_xnProjectXml.AddNodeValue(xnNewStimuli, "StimulusValue", "1")
                    m_xnProjectXml.AddNodeValue(xnNewStimuli, "FinalValue", "0")
                Else
                    m_xnProjectXml.AddNodeValue(xnNewStimuli, "InitialValue", "0")
                    m_xnProjectXml.AddNodeValue(xnNewStimuli, "StimulusValue", "0")
                    m_xnProjectXml.AddNodeValue(xnNewStimuli, "FinalValue", "1")
                End If

            End Sub

#End Region

#Region "Data Tool Modifiers"

            Protected Overridable Sub ModifyToolViewers(ByVal xnProjectNode As XmlNode, ByVal xnSimNode As XmlNode)

                Dim xnViewersOld As XmlNode = m_xnProjectXml.GetNode(xnProjectNode, "ToolViewers")

                Dim aryReplaceText As Hashtable = CreateReplaceStringList()

                Dim xnToolHolders As XmlNode = m_xnProjectXml.AppendNode(xnSimNode, xnViewersOld, "ToolViewers", aryReplaceText)

                m_xnProjectXml.RemoveNode(xnProjectNode, "ToolViewers")

                For Each xnHolder As XmlNode In xnToolHolders.ChildNodes
                    ModifyToolViewer(xnHolder, aryReplaceText)
                Next

            End Sub

            Protected Overridable Sub ModifyToolViewer(ByVal xnHolder As XmlNode, ByVal aryReplaceText As Hashtable)

                Dim strTitle As String = m_xnProjectXml.GetSingleNodeValue(xnHolder, "Name")

                Dim bFileFound As Boolean = False
                Dim strOldFile As String = ""
                If File.Exists(m_strProjectPath & "\" & strTitle & ".atvf") Then
                    bFileFound = True
                    strOldFile = m_strProjectPath & "\" & strTitle & ".atvf"
                ElseIf File.Exists(m_strProjectPath & "\" & strTitle.Replace(" ", "_") & ".atvf") Then
                    bFileFound = True
                    strOldFile = m_strProjectPath & "\" & strTitle.Replace(" ", "_") & ".atvf"
                Else

                End If

                Dim xnOldToolFile As New Framework.XmlDom

                If bFileFound Then

                    xnOldToolFile.Load(strOldFile)

                    Dim xnEditorOld As XmlNode = xnOldToolFile.GetRootNode("Editor")
                    Dim xnFormOld As XmlNode = xnOldToolFile.GetNode(xnEditorOld, "Form")

                    'Now create a new xml file.
                    Dim strNewFile As String = m_strProjectPath & "\" & strTitle.Replace(" ", "_") & ".aform"

                    Dim xnNewToolFile As New Framework.XmlDom

                    Dim xnNewRoot As XmlNode = xnNewToolFile.AddNodeValue(Nothing, "Form", "")

                    Dim xnNewForm As XmlNode = xnNewToolFile.AppendNode(xnNewRoot, xnFormOld, "Form", aryReplaceText)

                    xnNewToolFile.Save(strNewFile)
                End If
            End Sub


#End Region

#Region "Helper Methods"

            'Public Overridable Function MakeIdentityMatrix() As AnimatGuiCtrls.MatrixLibrary.Matrix
            '    Dim aryIdentity(3, 3) As Single

            '    aryIdentity(0, 0) = 1
            '    aryIdentity(1, 1) = 1
            '    aryIdentity(2, 2) = 1
            '    aryIdentity(3, 3) = 1

            '    Return aryIdentity
            'End Function

            Public Overridable Function MakeJointConversionMatrix() As AnimatGuiCtrls.MatrixLibrary.Matrix
                Dim aryM(3, 3) As Double

                aryM(0, 0) = -0.00020365317653499665
                aryM(0, 1) = 0.00020365317231177792
                aryM(0, 2) = 0.99999995852538359
                aryM(0, 3) = 0

                aryM(1, 0) = 0.99999997926269157
                aryM(1, 1) = 0
                aryM(1, 2) = 0.00020365317231177792
                aryM(1, 3) = 0

                aryM(2, 0) = 0
                aryM(2, 1) = 0.99999997926269157
                aryM(2, 2) = -0.00020365317653499665
                aryM(2, 3) = 0

                aryM(3, 0) = 0
                aryM(3, 1) = 0
                aryM(3, 2) = 0
                aryM(3, 3) = 1

                ' 	//osg::Matrix osgBase(-0.00020365317653499665f, 0.00020365317231177792f, 0.99999995852538359f,   0, 
                '//					 0.99999997926269157f,     0,                       0.00020365317231177792, 0, 
                '//					 0,                        0.99999997926269157,    -0.00020365317653499665, 0, 
                '//					 0,                        0,                       0,                       1);

                Dim aryRetVal As New AnimatGuiCtrls.MatrixLibrary.Matrix(aryM)

                Return aryRetVal
            End Function


            Public Overridable Function MakeConeConversionMatrix() As AnimatGuiCtrls.MatrixLibrary.Matrix
                Dim aryM(3, 3) As Double

                aryM(0, 0) = 1
                aryM(0, 1) = 0
                aryM(0, 2) = 0
                aryM(0, 3) = 0

                aryM(1, 0) = 0
                aryM(1, 1) = -0.00020365317653499665
                aryM(1, 2) = -0.99999997926269157
                aryM(1, 3) = 0

                aryM(2, 0) = 0
                aryM(2, 1) = 0.99999997926269157
                aryM(2, 2) = -0.00020365317653499665
                aryM(2, 3) = 0

                aryM(3, 0) = 0
                aryM(3, 1) = 0
                aryM(3, 2) = 0
                aryM(3, 3) = 1

                '1, 0, 0, 0
                '0, -0.00020365317653499665, -0.99999997926269157, 0
                '0, 0.99999997926269157, -0.00020365317653499665, 0
                '0, 0, 0, 1

                Dim aryRetVal As New AnimatGuiCtrls.MatrixLibrary.Matrix(aryM)

                Return aryRetVal
            End Function

#End Region

        End Class

    End Namespace
End Namespace
