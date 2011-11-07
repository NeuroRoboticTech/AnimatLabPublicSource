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
    Namespace FileConverters

        Public Class Convert_V1ToV2
            Inherits FileConverter

            Protected m_dblFluidDensity As Double = 1.0
            Protected m_iSimInterface As ManagedAnimatInterfaces.ISimulatorInterface

            Public Overrides ReadOnly Property ConvertFrom As String
                Get
                    Return "1.0"
                End Get
            End Property

            Public Overrides ReadOnly Property ConvertTo As String
                Get
                    Return "2.0"
                End Get
            End Property

            Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New()

            End Sub

            Protected Overrides Sub ConvertProjectNode(ByVal xnProject As XmlNode)

                m_iSimInterface = Util.Application.CreateSimInterface
                m_iSimInterface.CreateStandAloneSim("VortexAnimatPrivateSim_VC" & Util.Application.SimVCVersion & Util.Application.RuntimeModePrefix & ".dll",
                                                    Application.ExecutablePath)

                m_xnProjectXml.RemoveNode(xnProject, "PhysicsClassName", False)
                m_xnProjectXml.RemoveNode(xnProject, "PhysicsAssemblyName", False)
                m_xnProjectXml.UpdateSingleNodeValue(xnProject, "Version", ConvertTo(), False)

                m_xnProjectXml.RemoveNode(xnProject, "DockingForms", False)
                m_xnProjectXml.RemoveNode(xnProject, "ChildForms", False)
                m_xnProjectXml.RemoveNode(xnProject, "DockingConfig", False)
                m_xnProjectXml.RemoveNode(xnProject, "SimWindowLocation", False)
                m_xnProjectXml.RemoveNode(xnProject, "SimWindowSize", False)

                Dim strLogLevel As String = m_xnProjectXml.GetSingleNodeValue(xnProject, "LogLevel", False, "None")
                If strLogLevel.ToUpper = "ERROR" Then
                    m_xnProjectXml.UpdateSingleNodeValue(xnProject, "LogLevel", "ErrorType")
                End If

                CreateSimulationNode(xnProject)

            End Sub

            Protected Overridable Sub CreateSimulationNode(ByVal xnProject As XmlNode)

                'We need to get the simulation node from the asim file. We will then modify it.
                Dim strAsimFile As String = m_strProjectPath & "\" & m_xnProjectXml.GetSingleNodeValue(xnProject, "SimulationFile")

                Dim xnAST_File As New Framework.XmlDom

                xnAST_File.Load(strAsimFile)

                Dim xnSimNodeOld As XmlNode = xnAST_File.GetRootNode("Simulation")
                Dim xnSimNode As XmlNode = m_xnProjectXml.ApppendNode(xnProject, xnSimNodeOld, "Simulation")

                m_xnProjectXml.RemoveNode(xnSimNode, "APIFile", False)

                ModifySimNode(xnSimNode)
            End Sub

            Protected Overridable Sub ModifySimNode(ByVal xnSimulation As XmlNode)

                m_xnProjectXml.UpdateSingleNodeValue(xnSimulation, "AnimatModule", "VortexAnimatSim_VC10D.dll")

                Dim xnEnvironment As XmlNode = m_xnProjectXml.GetNode(xnSimulation, "Environment")

                m_dblFluidDensity = m_xnProjectXml.GetScaleNumberValue(xnEnvironment, "FluidDensity", False, 1.0)
                m_xnProjectXml.RemoveNode(xnEnvironment, "FluidDensity", False)
                m_xnProjectXml.RemoveNode(xnEnvironment, "MaxHydroForce", False)
                m_xnProjectXml.RemoveNode(xnEnvironment, "MaxHydroTorque", False)
                m_xnProjectXml.RemoveNode(xnEnvironment, "PlaybackRate", False)
                m_xnProjectXml.RemoveNode(xnEnvironment, "UseAlphaBlending", False)
                m_xnProjectXml.RemoveNode(xnEnvironment, "Camera", False)

                Dim xnOrganisms As XmlNode = m_xnProjectXml.GetNode(xnEnvironment, "Organisms")
                For Each xnNode As XmlNode In xnOrganisms.ChildNodes
                    ModifyOrganism(xnNode)
                Next

                Dim xnStructures As XmlNode = m_xnProjectXml.GetNode(xnEnvironment, "Structures")
                For Each xnNode As XmlNode In xnStructures.ChildNodes
                    ModifyStructure(xnNode)
                Next

                AddLight(xnEnvironment)

            End Sub

            Protected Overridable Sub ModifyOrganism(ByVal xnOrganism As XmlNode)

                ModifyStructure(xnOrganism)

            End Sub

#Region "Modify Structure"

            Protected Overridable Sub ModifyStructure(ByVal xnStructure As XmlNode)

                m_xnProjectXml.AddNodeValue(xnStructure, "Description", "")
                m_xnProjectXml.AddTransparency(xnStructure, 50, 50, 50, 50, 100)
                m_xnProjectXml.RemoveNode(xnStructure, "Reference", False)
                m_xnProjectXml.ConvertScaledNumberToScaledVector(xnStructure, "Position", "LocalPosition")
                m_xnProjectXml.AddNodeValue(xnStructure, "IsVisible", "True")
                m_xnProjectXml.AddNodeValue(xnStructure, "Shininess", "64")
                m_xnProjectXml.AddNodeValue(xnStructure, "Texture", "")
                m_xnProjectXml.AddScaledVector(xnStructure, "Rotation", 0, 0, 0)
                m_xnProjectXml.AddColor(xnStructure, "Ambient", 1, 0, 0, 1)
                m_xnProjectXml.AddColor(xnStructure, "Diffuse", 1, 0, 0, 1)
                m_xnProjectXml.AddColor(xnStructure, "Specular", 1, 0, 0, 1)

                CreateRigidBodyRootNode(xnStructure)

            End Sub

#Region "Rigid Body Modifiers"

            Protected Overridable Sub CreateRigidBodyRootNode(ByVal xnStructure As XmlNode)

                'We need to get the simulation node from the asim file. We will then modify it.
                Dim strBodyPlanFile As String = m_strProjectPath & "\" & m_xnProjectXml.GetSingleNodeValue(xnStructure, "BodyPlan")
                Dim xnASTL_File As New Framework.XmlDom

                xnASTL_File.Load(strBodyPlanFile)

                Dim xnStruct As XmlNode = xnASTL_File.GetRootNode("Structure")

                Dim xnRigidBodyNodeOld As XmlNode = xnASTL_File.GetNode(xnStruct, "RigidBody")
                Dim xnRigidBody As XmlNode = m_xnProjectXml.ApppendNode(xnStructure, xnRigidBodyNodeOld, "RigidBody")

                Dim xnCollisionsOld As XmlNode = xnASTL_File.GetNode(xnStruct, "CollisionExclusionPairs")
                Dim xnCollisions As XmlNode = m_xnProjectXml.ApppendNode(xnStructure, xnCollisionsOld, "CollisionExclusionPairs")

                ModifyRigidBody(xnRigidBody)
            End Sub

            Protected Overridable Sub ModifyRigidBody(ByVal xnRigidBody As XmlNode)

                m_xnProjectXml.AddTransparency(xnRigidBody, 0, 0, 50, 50, 0)

                ModifyRigidBodyDrag(xnRigidBody)
                ModifyRigidBodyCOM(xnRigidBody)
                ModifyRigidBodyColor(xnRigidBody)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "IsCollisionObject", "True")

                m_xnProjectXml.RemoveNode(xnRigidBody, "PartType")
                m_xnProjectXml.RemoveNode(xnRigidBody, "OrientationMatrix", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "TranslationMatrix", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "CombinedTransformationMatrix", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "Direction", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "CenterOfMass", False)

                m_xnProjectXml.RemoveNode(xnRigidBody, "Rotation", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "RelativePosition", False)

                m_xnProjectXml.ConvertScaledNumberToScaledVector(xnRigidBody, "LocalPosition", "LocalPosition", 1, 1, -1)
                m_xnProjectXml.ConvertScaledNumberToScaledVector(xnRigidBody, "LocalRotation", "Rotation", Util.RadiansToDegreesRatio, Util.RadiansToDegreesRatio, Util.RadiansToDegreesRatio)

                Dim strType As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "Type").ToUpper

                Select Case strType
                    Case "BOX"
                        ModifyRigidBodyBox(xnRigidBody)
                    Case "CONE"
                        ModifyRigidBodyCone(xnRigidBody)
                    Case "CYLINDER"
                        ModifyRigidBodyCylinder(xnRigidBody)
                    Case "MUSCLEATTACHMENT"
                        ModifyRigidBodyAttachment(xnRigidBody)
                    Case "LINEARHILLMUSCLE"
                        ModifyRigidBodyMuscle(xnRigidBody)
                    Case "LINEARHILLSTRETCHRECEPTOR"
                        ModifyRigidBodyStretchReceptor(xnRigidBody)
                    Case "MESH"
                        ModifyRigidBodyMesh(xnRigidBody)
                    Case "MOUTH"
                        ModifyRigidBodyMouth(xnRigidBody)
                    Case "ODORSENSOR"
                        ModifyRigidBodyOdorSensor(xnRigidBody)
                    Case "SPHERE"
                        ModifyRigidBodySphere(xnRigidBody)
                    Case "SPRING"
                        ModifyRigidBodySpring(xnRigidBody)
                    Case "STOMACH"
                        ModifyRigidBodyStomach(xnRigidBody)
                End Select

                'Modify the joint if it exists.
                Dim xnJoint As XmlNode = m_xnProjectXml.GetNode(xnRigidBody, "Joint", False)
                If Not xnJoint Is Nothing Then
                    ModifyJoint(xnJoint)
                End If

                'Now modify all the child nodes.
                Dim xnChildren As XmlNode = m_xnProjectXml.GetNode(xnRigidBody, "ChildBodies", False)
                If Not xnChildren Is Nothing Then
                    For Each xnChild As XmlNode In xnChildren.ChildNodes
                        ModifyRigidBody(xnChild)
                    Next
                End If

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
                m_xnProjectXml.AddNodeValue(xnRigidBody, "EnableFluids", "False")

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

            Protected Overridable Sub ModifyRigidBodyColor(ByVal xnRigidBody As XmlNode)

                Dim dblDiffuseR As Double = 1
                Dim dblDiffuseG As Double = 0
                Dim dblDiffuseB As Double = 0
                Dim dblDiffuseA As Double = 1

                m_xnProjectXml.ReadColor(xnRigidBody, "Color", dblDiffuseR, dblDiffuseG, dblDiffuseB, dblDiffuseA, False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "Color", False)

                m_xnProjectXml.AddColor(xnRigidBody, "Diffuse", dblDiffuseR, dblDiffuseG, dblDiffuseB, dblDiffuseA)
                m_xnProjectXml.AddColor(xnRigidBody, "Specular", 0.25098, 0.25098, 0.25098, 1)
                m_xnProjectXml.AddColor(xnRigidBody, "Ambient", 0.0980392, 0.0980392, 0.0980392, 1)
                m_xnProjectXml.AddNodeValue(xnRigidBody, "Shininess", "64")

            End Sub

#Region "Rigid Body Part Type Modifiers"

            Protected Overridable Sub ModifyRigidBodyBox(ByVal xnRigidBody As XmlNode)

                Dim dblX As Double = 0
                Dim dblY As Double = 0
                Dim dblZ As Double = 0

                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.Box")
                m_xnProjectXml.ReadVector(xnRigidBody, "CollisionBoxSize", dblX, dblY, dblZ)

                m_xnProjectXml.RemoveNode(xnRigidBody, "CollisionBoxSize", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "GraphicsBoxSize", False)

                m_xnProjectXml.AddScaledNumber(xnRigidBody, "Length", dblX, "None", dblX)
                m_xnProjectXml.AddScaledNumber(xnRigidBody, "Height", dblY, "None", dblY)
                m_xnProjectXml.AddScaledNumber(xnRigidBody, "Width", dblZ, "None", dblZ)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "LengthSections", "1")
                m_xnProjectXml.AddNodeValue(xnRigidBody, "HeightSections", "1")
                m_xnProjectXml.AddNodeValue(xnRigidBody, "WidthSections", "1")

            End Sub

            Protected Overridable Sub ModifyRigidBodyCone(ByVal xnRigidBody As XmlNode)

            End Sub

            Protected Overridable Sub ModifyRigidBodyCylinder(ByVal xnRigidBody As XmlNode)

            End Sub

            Protected Overridable Sub ModifyRigidBodyAttachment(ByVal xnRigidBody As XmlNode)

            End Sub

            Protected Overridable Sub ModifyRigidBodyMuscle(ByVal xnRigidBody As XmlNode)

            End Sub

            Protected Overridable Sub ModifyRigidBodyStretchReceptor(ByVal xnRigidBody As XmlNode)

            End Sub

            Protected Overridable Sub ModifyRigidBodyMesh(ByVal xnRigidBody As XmlNode)

            End Sub

            Protected Overridable Sub ModifyRigidBodyMouth(ByVal xnRigidBody As XmlNode)

            End Sub

            Protected Overridable Sub ModifyRigidBodyOdorSensor(ByVal xnRigidBody As XmlNode)

            End Sub

            Protected Overridable Sub ModifyRigidBodySphere(ByVal xnRigidBody As XmlNode)

            End Sub

            Protected Overridable Sub ModifyRigidBodySpring(ByVal xnRigidBody As XmlNode)

            End Sub

            Protected Overridable Sub ModifyRigidBodyStomach(ByVal xnRigidBody As XmlNode)

            End Sub

#End Region

#End Region

#Region "Joint Modifiers"

            Protected Overridable Sub ModifyJoint(ByVal xnJoint As XmlNode)

                m_xnProjectXml.AddTransparency(xnJoint, 50, 50, 0, 50, 50)

                ModifyRigidBodyColor(xnJoint)

                m_xnProjectXml.RemoveNode(xnJoint, "PartType")
                m_xnProjectXml.RemoveNode(xnJoint, "Rotation", False)

                m_xnProjectXml.ConvertScaledNumberToScaledVector(xnJoint, "RelativePosition", "LocalPosition", 1, 1, -1)
                'm_xnProjectXml.ConvertJointRotation(xnJoint, "Rotation", "Rotation", _
                '                                    Util.RadiansToDegreesRatio, -Util.RadiansToDegreesRatio, Util.RadiansToDegreesRatio, _
                '                                    90, 0, -90)
                'm_xnProjectXml.ConvertJointRotation(xnJoint, "RotationAxis", "Rotation", 180, 180, -180, 90, 0, 90)

                Dim aryOrientation(,) As Single = m_xnProjectXml.LoadMatrix(xnJoint, "OrientationMatrix")
                Dim fltXPos As Single = 0
                Dim fltYPos As Single = 0
                Dim fltZPos As Single = 0
                Dim fltXRot As Single = 0
                Dim fltYRot As Single = 0
                Dim fltZRot As Single = 0
                m_iSimInterface.GetPositionAndRotationFromD3DMatrix(aryOrientation, fltXPos, fltYPos, fltZPos, fltXRot, fltYRot, fltZRot)

                'm_xnProjectXml.AddScaledVector(xnJoint, "Position", fltXPos, fltYPos, fltZPos)
                m_xnProjectXml.AddScaledVector(xnJoint, "Rotation", Util.RadiansToDegrees(fltXRot), Util.RadiansToDegrees(fltYRot), Util.RadiansToDegrees(fltZRot))

                m_xnProjectXml.AddScaledNumber(xnJoint, "Size", 0.2, "None", 0.2)

                m_xnProjectXml.RemoveNode(xnJoint, "OrientationMatrix", False)
                m_xnProjectXml.RemoveNode(xnJoint, "TranslationMatrix", False)

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


            End Sub

#Region "Joint Type Modifiers"

            Protected Overridable Sub ModifyJointHinge(ByVal xnJoint As XmlNode)

                m_xnProjectXml.AddNodeValue(xnJoint, "PartType", "AnimatGUI.DataObjects.Physical.Joints.Hinge")

                Dim dblMaxTorque As Double = m_xnProjectXml.GetScaleNumberValue(xnJoint, "MaxTorque", False, 100)
                m_xnProjectXml.RemoveNode(xnJoint, "MaxTorque", False)
                m_xnProjectXml.AddScaledNumber(xnJoint, "MaxForce", dblMaxTorque, "None", dblMaxTorque)

            End Sub

            Protected Overridable Sub ModifyJointBallSocket(ByVal xnJoint As XmlNode)

            End Sub

            Protected Overridable Sub ModifyJointPrismatic(ByVal xnJoint As XmlNode)

            End Sub

            Protected Overridable Sub ModifyJointStatic(ByVal xnJoint As XmlNode)

            End Sub
#End Region

#End Region

            Protected Overridable Sub AddLight(ByVal xnEnvironment As XmlNode)

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
                                       "        <X Value=""1"" Scale=""None"" Actual=""1""/>" & _
                                       "        <Y Value=""10"" Scale=""None"" Actual=""10""/>" & _
                                       "        <Z Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "    </LocalPosition>" & _
                                       "    <Rotation>" & _
                                       "        <X Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "        <Y Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "        <Z Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "    </Rotation>" & _
                                       "    <Radius Value=""50"" Scale=""milli"" Actual=""0.05""/>" & _
                                       "    <ConstantAttenuation>0</ConstantAttenuation>" & _
                                       "    <LinearAttenuationDistance Value=""0"" Scale=""None"" Actual=""0""/>" & _
                                       "    <QuadraticAttenuationDistance Value=""1"" Scale=""Kilo"" Actual=""1000""/>" & _
                                       "    <LatitudeSegments>10</LatitudeSegments>" & _
                                       "    <LongtitudeSegments>10</LongtitudeSegments>" & _
                                       "</Light>"

                Dim xmlLights As XmlNode = m_xnProjectXml.AddNodeXml(xnEnvironment, "Lights", strXml)

            End Sub

#End Region

#Region "Modify Nervous System"

            Protected Overridable Sub ModifyNervousSystem(ByVal xnOrganism As XmlNode)

            End Sub

#End Region

        End Class

    End Namespace
End Namespace
