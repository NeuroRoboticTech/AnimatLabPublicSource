﻿Imports System
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
            Protected m_fltDistanceUnits As Single
            Protected m_aryIdentity As New AnimatGuiCtrls.MatrixLibrary.Matrix(AnimatGuiCtrls.MatrixLibrary.Matrix.Identity(4))

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
                Dim xnSimNode As XmlNode = m_xnProjectXml.AppendNode(xnProject, xnSimNodeOld, "Simulation")

                m_xnProjectXml.RemoveNode(xnSimNode, "APIFile", False)

                ModifySimNode(xnSimNode)
                ModifyStimuli(xnProject, xnSimNode)

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

                m_fltDistanceUnits = Util.ConvertDistanceUnits(m_xnProjectXml.GetSingleNodeValue(xnEnvironment, "DistanceUnits"))

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
                ModifyNervousSystem(xnOrganism)

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

                Dim xnRigidBodyNodeOld As XmlNode = xnASTL_File.GetNode(xnStruct, "RigidBody", False)
                If Not xnRigidBodyNodeOld Is Nothing Then
                    Dim xnRigidBody As XmlNode = m_xnProjectXml.AppendNode(xnStructure, xnRigidBodyNodeOld, "RigidBody")

                    Dim xnCollisionsOld As XmlNode = xnASTL_File.GetNode(xnStruct, "CollisionExclusionPairs")
                    Dim xnCollisions As XmlNode = m_xnProjectXml.AppendNode(xnStructure, xnCollisionsOld, "CollisionExclusionPairs")

                    ModifyRigidBody(xnRigidBody, m_aryIdentity)
                End If
            End Sub

            Protected Overridable Sub ModifyRigidBody(ByVal xnRigidBody As XmlNode, ByVal aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

                m_xnProjectXml.AddTransparency(xnRigidBody, 0, 0, 50, 50, 0)

                ModifyRigidBodyDrag(xnRigidBody)
                ModifyRigidBodyCOM(xnRigidBody)
                ModifyRigidBodyColor(xnRigidBody)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "IsCollisionObject", "True")

                m_xnProjectXml.RemoveNode(xnRigidBody, "PartType")
                m_xnProjectXml.RemoveNode(xnRigidBody, "Direction", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "CenterOfMass", False)

                m_xnProjectXml.RemoveNode(xnRigidBody, "RelativePosition", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "LocalPosition", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "Rotation", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "LocalRotation", False)

                Dim aryOrientation As AnimatGuiCtrls.MatrixLibrary.Matrix = m_xnProjectXml.LoadOrientationPositionMatrix(xnRigidBody, "TranslationMatrix", "OrientationMatrix")

                Dim oPosRot As ManagedAnimatInterfaces.PositionRotationInfo
                oPosRot = m_iSimInterface.GetPositionAndRotationFromD3DMatrix(aryOrientation.toArray(), aryParentTrasform.toArray())

                m_xnProjectXml.AddScaledVector(xnRigidBody, "LocalPosition", oPosRot.m_fltXPos * m_fltDistanceUnits, oPosRot.m_fltYPos * m_fltDistanceUnits, oPosRot.m_fltZPos * m_fltDistanceUnits)
                m_xnProjectXml.AddScaledVector(xnRigidBody, "Rotation", Util.RadiansToDegrees(oPosRot.m_fltXRot), Util.RadiansToDegrees(oPosRot.m_fltYRot), Util.RadiansToDegrees(oPosRot.m_fltZRot))

                Dim strType As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "Type").ToUpper
                Dim aryChildTransform As New AnimatGuiCtrls.MatrixLibrary.Matrix(AnimatGuiCtrls.MatrixLibrary.Matrix.Identity(4))

                Select Case strType
                    Case "BOX"
                        ModifyRigidBodyBox(xnRigidBody, aryChildTransform)
                    Case "CONE"
                        ModifyRigidBodyCone(xnRigidBody, aryChildTransform)
                    Case "CYLINDER"
                        ModifyRigidBodyCylinder(xnRigidBody, aryChildTransform)
                    Case "MUSCLEATTACHMENT"
                        ModifyRigidBodyAttachment(xnRigidBody, aryChildTransform)
                    Case "LINEARHILLMUSCLE"
                        ModifyRigidBodyMuscle(xnRigidBody, aryChildTransform)
                    Case "LINEARHILLSTRETCHRECEPTOR"
                        ModifyRigidBodyStretchReceptor(xnRigidBody, aryChildTransform)
                    Case "MESH"
                        ModifyRigidBodyMesh(xnRigidBody, aryChildTransform)
                    Case "MOUTH"
                        ModifyRigidBodyMouth(xnRigidBody, aryChildTransform)
                    Case "ODORSENSOR"
                        ModifyRigidBodyOdorSensor(xnRigidBody, aryChildTransform)
                    Case "SPHERE"
                        ModifyRigidBodySphere(xnRigidBody, aryChildTransform)
                    Case "SPRING"
                        ModifyRigidBodySpring(xnRigidBody, aryChildTransform)
                    Case "STOMACH"
                        ModifyRigidBodyStomach(xnRigidBody, aryChildTransform)
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
                        ModifyRigidBody(xnChild, aryChildTransform)
                    Next
                End If

                m_xnProjectXml.RemoveNode(xnRigidBody, "OrientationMatrix", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "TranslationMatrix", False)
                m_xnProjectXml.RemoveNode(xnRigidBody, "CombinedTransformationMatrix", False)

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

            Protected Overridable Sub ModifyRigidBodyBox(ByVal xnRigidBody As XmlNode, ByRef aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

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

                m_xnProjectXml.AddNodeValue(xnRigidBody, "LengthSections", "1")
                m_xnProjectXml.AddNodeValue(xnRigidBody, "HeightSections", "1")
                m_xnProjectXml.AddNodeValue(xnRigidBody, "WidthSections", "1")

            End Sub

            Protected Overridable Sub ModifyRigidBodyCone(ByVal xnRigidBody As XmlNode, ByRef aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)
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
                Dim aryTransform As AnimatGuiCtrls.MatrixLibrary.Matrix = AnimatGuiCtrls.MatrixLibrary.Matrix.Multiply(aryConversion, aryOrientation)

                Dim oPosRot As ManagedAnimatInterfaces.PositionRotationInfo
                'oPosRot = m_iSimInterface.GetPositionAndRotationFromD3DMatrix(aryOrientation.toArray, aryConversion.toArray)
                oPosRot = m_iSimInterface.GetPositionAndRotationFromD3DMatrix(aryTransform.toArray, m_aryIdentity.toArray)

                m_xnProjectXml.AddScaledVector(xnRigidBody, "LocalPosition", oPosRot.m_fltXPos * m_fltDistanceUnits, oPosRot.m_fltYPos * m_fltDistanceUnits, oPosRot.m_fltZPos * m_fltDistanceUnits)
                m_xnProjectXml.AddScaledVector(xnRigidBody, "Rotation", Util.RadiansToDegrees(oPosRot.m_fltXRot), Util.RadiansToDegrees(oPosRot.m_fltYRot), Util.RadiansToDegrees(oPosRot.m_fltZRot))

                'aryParentTrasform = AnimatGuiCtrls.MatrixLibrary.Matrix.Inverse(aryTransform)
                'aryParentTrasform(3, 0) = 0
                'aryParentTrasform(3, 1) = 0
                'aryParentTrasform(3, 2) = 0
                'aryParentTrasform(3, 3) = 1

            End Sub

            Protected Overridable Sub ModifyRigidBodyCylinder(ByVal xnRigidBody As XmlNode, ByRef aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

            End Sub

            Protected Overridable Sub ModifyRigidBodyAttachment(ByVal xnRigidBody As XmlNode, ByRef aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

                ModifyRigidBodySphere(xnRigidBody, aryParentTrasform)

                m_xnProjectXml.RemoveNode(xnRigidBody, "PartType")
                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.Attachment")
            End Sub

            Protected Overridable Sub ModifyRigidBodyMuscle(ByVal xnRigidBody As XmlNode, ByRef aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

            End Sub

            Protected Overridable Sub ModifyRigidBodyStretchReceptor(ByVal xnRigidBody As XmlNode, ByRef aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

            End Sub

            Protected Overridable Sub ModifyRigidBodyMesh(ByVal xnRigidBody As XmlNode, ByRef aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

            End Sub

            Protected Overridable Sub ModifyRigidBodyMouth(ByVal xnRigidBody As XmlNode, ByRef aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

            End Sub

            Protected Overridable Sub ModifyRigidBodyOdorSensor(ByVal xnRigidBody As XmlNode, ByRef aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

            End Sub

            Protected Overridable Sub ModifyRigidBodySphere(ByVal xnRigidBody As XmlNode, ByRef aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "PartType", "AnimatGUI.DataObjects.Physical.Bodies.Sphere")

                Dim fltRadius As Single = CSng(m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "Radius")) * m_fltDistanceUnits

                m_xnProjectXml.RemoveNode(xnRigidBody, "Radius", False)
                m_xnProjectXml.AddScaledNumber(xnRigidBody, "Radius", fltRadius, "None", fltRadius)

                m_xnProjectXml.AddNodeValue(xnRigidBody, "LatitudeSegments", "20")
                m_xnProjectXml.AddNodeValue(xnRigidBody, "LongtitudeSegments", "20")

            End Sub

            Protected Overridable Sub ModifyRigidBodySpring(ByVal xnRigidBody As XmlNode, ByRef aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

            End Sub

            Protected Overridable Sub ModifyRigidBodyStomach(ByVal xnRigidBody As XmlNode, ByRef aryParentTrasform As AnimatGuiCtrls.MatrixLibrary.Matrix)

            End Sub

#End Region

#End Region

#Region "Joint Modifiers"

            Protected Overridable Sub ModifyJoint(ByVal xnJoint As XmlNode)

                m_xnProjectXml.AddTransparency(xnJoint, 50, 50, 0, 50, 50)

                ModifyRigidBodyColor(xnJoint)

                m_xnProjectXml.RemoveNode(xnJoint, "PartType")
                m_xnProjectXml.RemoveNode(xnJoint, "Rotation", False)
                m_xnProjectXml.RemoveNode(xnJoint, "RelativePosition", False)
                m_xnProjectXml.RemoveNode(xnJoint, "RotationAxis", False)

                Dim aryOrientation As AnimatGuiCtrls.MatrixLibrary.Matrix = m_xnProjectXml.LoadOrientationPositionMatrix(xnJoint, "TranslationMatrix", "OrientationMatrix")
                Dim aryJointConv As AnimatGuiCtrls.MatrixLibrary.Matrix = MakeJointConversionMatrix()
                Dim aryTransform As AnimatGuiCtrls.MatrixLibrary.Matrix = AnimatGuiCtrls.MatrixLibrary.Matrix.Multiply(aryJointConv, aryOrientation)
                Dim aryData(,) As Double = aryTransform.toArray

                Dim oPosRot As ManagedAnimatInterfaces.PositionRotationInfo
                'oPosRot = m_iSimInterface.GetPositionAndRotationFromD3DMatrix(aryOrientation.toArray, aryJointConv.toArray)
                oPosRot = m_iSimInterface.GetPositionAndRotationFromD3DMatrix(aryTransform.toArray, m_aryIdentity.toArray)

                m_xnProjectXml.AddScaledVector(xnJoint, "LocalPosition", oPosRot.m_fltXPos * m_fltDistanceUnits, oPosRot.m_fltYPos * m_fltDistanceUnits, oPosRot.m_fltZPos * m_fltDistanceUnits)
                m_xnProjectXml.AddScaledVector(xnJoint, "Rotation", Util.RadiansToDegrees(oPosRot.m_fltXRot), Util.RadiansToDegrees(oPosRot.m_fltYRot), Util.RadiansToDegrees(oPosRot.m_fltZRot))

                m_xnProjectXml.RemoveNode(xnJoint, "OrientationMatrix", False)
                m_xnProjectXml.RemoveNode(xnJoint, "TranslationMatrix", False)
                m_xnProjectXml.RemoveNode(xnJoint, "CombinedTransformationMatrix", False)

                m_xnProjectXml.AddScaledNumber(xnJoint, "Size", 0.2 * m_fltDistanceUnits, "None", 0.2 * m_fltDistanceUnits)

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

                Dim xnNervousSys As XmlNode = m_xnProjectXml.AddNodeValue(xnOrganism, "NervousSystem", "")

                Dim xnBodyFile As Framework.XmlDom = LoadBehavioralFile(xnOrganism)

                If Not xnBodyFile Is Nothing Then
                    Dim xnOldRoot As XmlNode = xnBodyFile.GetRootNode("Editor")
                    Dim xnOldNeuralMods As XmlNode = xnBodyFile.GetNode(xnOldRoot, "NeuralModules")

                    CreateNeuralModules(xnNervousSys, xnBodyFile, xnOldNeuralMods)

                    Dim xnOldDiagrams As XmlNode = m_xnProjectXml.GetNode(xnOldRoot, "Diagrams")
                    Dim xnOldDiagram As XmlNode = m_xnProjectXml.GetNode(xnOldDiagrams, "Diagram")
                    ConvertRootDiagramToSubsystem(xnBodyFile, xnNervousSys, xnOldDiagram)
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
                m_xnProjectXml.CopyScaledNumber(xnBodyFile, xnOldMod, xnNewNeuralMod, "TimeStep")
            End Sub

            Protected Overridable Sub CreateFastNeuralModule(ByVal xnBodyFile As Framework.XmlDom, ByVal xnOldMod As XmlNode, ByVal xnNewNeuralMods As XmlNode)
                Dim xnNewNeuralMod As XmlNode = m_xnProjectXml.AddNodeValue(xnNewNeuralMods, "Node", "")

                m_xnProjectXml.AddNodeValue(xnNewNeuralMod, "ID", System.Guid.NewGuid.ToString)
                m_xnProjectXml.AddNodeValue(xnNewNeuralMod, "AssemblyFile", "FiringRateGUI.dll")
                m_xnProjectXml.AddNodeValue(xnNewNeuralMod, "ClassName", "FiringRateGUI.DataObjects.Behavior.NeuralModule")
                m_xnProjectXml.CopyScaledNumber(xnBodyFile, xnOldMod, xnNewNeuralMod, "TimeStep")

            End Sub

            Protected Overridable Sub CreateRealisticToolsNeuralModule(ByVal xnBodyFile As Framework.XmlDom, ByVal xnOldMod As XmlNode, ByVal xnNewNeuralMods As XmlNode)

                Dim aryReplaceText As New Hashtable
                aryReplaceText.Add("RealisticNeuralNetTools", "IntegrateFireGUI")

                Dim xnNewNeuralMod As XmlNode = m_xnProjectXml.AppendNode(xnNewNeuralMods, xnOldMod, "Node", aryReplaceText)
                m_xnProjectXml.AddNodeValue(xnNewNeuralMod, "ID", System.Guid.NewGuid.ToString)

            End Sub
#End Region

#Region "Diagrams"

            Protected Overridable Sub ConvertRootDiagramToSubsystem(ByVal xnBodyFile As Framework.XmlDom, ByVal xnNervousSys As XmlNode, ByVal xnOldDiagram As XmlNode)
                Dim xnRootSubSystem As XmlNode = CreateRootSubsystem(xnNervousSys, xnBodyFile, xnOldDiagram)
                ModifySubSystem(xnBodyFile, xnRootSubSystem)
            End Sub

            Protected Overridable Function CreateRootSubsystem(ByVal xnNervousSys As XmlNode, ByVal xnBodyFile As Framework.XmlDom, ByVal xnOldDiagram As XmlNode) As XmlNode

                Dim strDiagramID As String = xnBodyFile.GetSingleNodeValue(xnOldDiagram, "ID")

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
                                     "<Text>Neural Subsystem</Text>" & _
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

                Return m_xnProjectXml.AppendNode(xnNervousSys, strXml, "Node")

            End Function

            Protected Overridable Sub ModifySubSystem(ByVal xnBodyFile As Framework.XmlDom, ByVal xnSubSystem As XmlNode)

                Dim strSubSystemID As String = m_xnProjectXml.GetSingleNodeValue(xnSubSystem, "SubsystemID")

                Dim xnRoot As XmlNode = xnBodyFile.GetRootNode("Editor")
                Dim xnOldDiagramID As XmlNode = xnBodyFile.GetNode(xnRoot, "//ID[text()=""" & strSubSystemID & """]")

                If xnOldDiagramID Is Nothing Then
                    Throw New System.Exception("No diagram was found with the ID '" & strSubSystemID & "'")
                End If

                Dim xnOldDiagram As XmlNode = xnOldDiagramID.ParentNode
                m_xnProjectXml.RemoveNode(xnSubSystem, "SubsystemID")

                Dim aryReplaceText As New Hashtable()
                aryReplaceText.Add("LicensedAnimatTools", "LicensedAnimatGUI")
                aryReplaceText.Add("FastNeuralNetTools", "FiringRateGUI")
                aryReplaceText.Add("RealisticNeuralNetTools", "IntegrateFireGUI")
                aryReplaceText.Add("AnimatTools", "AnimatGUI")
                aryReplaceText.Add("AHPConductance", "AHP_Conductance")
                aryReplaceText.Add("AHPTimeConstant", "AHP_TimeConstant")

                'Copy nodes
                Dim xnOldNodes As XmlNode = xnBodyFile.GetNode(xnOldDiagram, "Nodes")
                Dim xnNewNodes As XmlNode = m_xnProjectXml.AppendNode(xnSubSystem, xnOldNodes, "Nodes", aryReplaceText)

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
                                  xnAddFlow.InnerXml() & "</AddFlow></Diagram></Root>"

                Dim xnDiagramXml As XmlNode = m_xnProjectXml.CreateElement("DiagramXml")
                Dim xnDiagramCData As XmlNode = m_xnProjectXml.CreateCDataSection(strXml)

                xnDiagramXml.AppendChild(xnDiagramCData)
                xnSubSystem.AppendChild(xnDiagramXml)

            End Sub

#End Region

#End Region

#Region "Stimuli Modifiers"

            Protected Overridable Sub ModifyStimuli(ByVal xnProjectNode As XmlNode, ByVal xnSimNode As XmlNode)

                Dim xnStimuli As XmlNode = m_xnProjectXml.GetNode(xnProjectNode, "Stimuli")

                Dim aryReplaceText As New Hashtable()
                aryReplaceText.Add("LicensedAnimatTools", "LicensedAnimatGUI")
                aryReplaceText.Add("FastNeuralNetTools", "FiringRateGUI")
                aryReplaceText.Add("RealisticNeuralNetTools", "IntegrateFireGUI")
                aryReplaceText.Add("AnimatTools", "AnimatGUI")

                m_xnProjectXml.AppendNode(xnSimNode, xnStimuli, "Stimuli", aryReplaceText)

                m_xnProjectXml.RemoveNode(xnProjectNode, "Stimuli")

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
