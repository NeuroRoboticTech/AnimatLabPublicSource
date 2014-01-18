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

        Public Class Migrate_BulletToVortex
            Inherits ProjectMigration

            Protected m_aryIdentity As New AnimatGuiCtrls.MatrixLibrary.Matrix(AnimatGuiCtrls.MatrixLibrary.Matrix.Identity(4))

            Public Overrides ReadOnly Property ConvertFrom As String
                Get
                    Return "Bullet"
                End Get
            End Property

            Public Overrides ReadOnly Property ConvertTo As String
                Get
                    Return "Vortex"
                End Get
            End Property

            Sub New()
                MyBase.New()

            End Sub

            Protected Overrides Sub ConvertProjectNode(ByVal xnProject As XmlNode, ByRef strPhysics As String)

                Dim strOldPhysics As String = m_xnProjectXml.GetSingleNodeValue(xnProject, "Physics", False, "")

                If strOldPhysics.Trim.Length > 0 Then
                    m_xnProjectXml.UpdateSingleNodeValue(xnProject, "Physics", "Vortex")
                Else
                    m_xnProjectXml.AddNodeValue(xnProject, "Physics", "Vortex")
                End If
                strPhysics = "Vortex"

                CreateSimulationNode(xnProject)

            End Sub

            Protected Overridable Sub CreateSimulationNode(ByVal xnProject As XmlNode)

                Dim xnSimNode As XmlNode = m_xnProjectXml.GetNode(xnProject, "Simulation")

                ModifySimNode(xnSimNode)
                ModifyStimuli(xnProject, xnSimNode)
                ModifyToolViewers(xnProject, xnSimNode)

            End Sub

            Protected Overridable Sub ModifySimNode(ByVal xnSimulation As XmlNode)

                Util.Application.AppStatusText = "Converting simulation nodes"

                m_xnProjectXml.UpdateSingleNodeValue(xnSimulation, "AnimatModule", "BulletAnimatSim_VC" & Util.Application.SimVCVersion & Util.Application.RuntimeModePrefix & ".dll")

                'Dim xnEnvironment As XmlNode = m_xnProjectXml.GetNode(xnSimulation, "Environment")

                'Dim xnOrganisms As XmlNode = m_xnProjectXml.GetNode(xnEnvironment, "Organisms")
                'For Each xnNode As XmlNode In xnOrganisms.ChildNodes
                '    ModifyOrganism(xnNode)
                'Next

                'Dim xnStructures As XmlNode = m_xnProjectXml.GetNode(xnEnvironment, "Structures")
                'For Each xnNode As XmlNode In xnStructures.ChildNodes
                '    ModifyStructure(xnNode, False)
                'Next

            End Sub

            Protected Overridable Sub ModifyOrganism(ByVal xnOrganism As XmlNode)

                ModifyStructure(xnOrganism, False)
                ModifyNervousSystem(xnOrganism)

            End Sub

#Region "Modify Structure"

            Protected Overridable Sub ModifyStructure(ByVal xnStructure As XmlNode, ByVal bIsFluidPlane As Boolean, Optional ByVal bSetAmbientColor As Boolean = False)

                Dim strName As String = m_xnProjectXml.GetSingleNodeValue(xnStructure, "Name", False)
                Util.Application.AppStatusText = "Converting organism/structure " & strName & " body"

                Dim xnRoot As XmlNode = m_xnProjectXml.GetNode(xnStructure, "RigidBody")
                ModifyRigidBody(xnRoot)

            End Sub

#Region "Rigid Body Modifiers"

            Protected Overridable Sub ModifyRigidBody(ByVal xnRigidBody As XmlNode)

                'First check to see if this is a rigid body that was added by the converter process. If it was then skip it and do not processes it further.
                If Not m_xnProjectXml.GetNode(xnRigidBody, "Converter", False) Is Nothing Then
                    Return
                End If

                Dim strName As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "Name", False)
                Util.Application.AppStatusText = "Converting body part " & strName

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
                    Case "PLANE"
                        ModifyRigidBodyPlane(xnRigidBody)
                    Case "MULTISEGMENTSPRING"
                        ModifyMultiSegmentSpring(xnRigidBody)
                    Case "TORUS"
                        ModifyRigidBodyTorus(xnRigidBody)
                    Case "ELLIPSIOD"
                        ModifyRigidBodyEllipsoid(xnRigidBody)
                    Case "BOXCONTACTSENSOR"
                        ModifyRigidBodyBox(xnRigidBody)
                    Case "CYLINDERCONTACTSENSOR"
                        ModifyRigidBodyCylinder(xnRigidBody)
                    Case Else
                        Throw New System.Exception("Invalid body part type defined. '" & strType & "'")
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

                Dim strID As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "ID")
                m_hashRigidBodies.Add(strID, xnRigidBody)

            End Sub

#Region "Rigid Body Part Type Modifiers"

            Protected Overridable Sub ModifyRigidBodyBox(ByVal xnRigidBody As XmlNode)
            End Sub

            Protected Overridable Sub ModifyRigidBodyBoxSensor(ByVal xnRigidBody As XmlNode)
            End Sub

            Protected Overridable Sub ModifyRigidBodyCone(ByVal xnRigidBody As XmlNode)
            End Sub

            Protected Overridable Sub ModifyRigidBodyCylinder(ByVal xnRigidBody As XmlNode)
            End Sub

            Protected Overridable Sub ModifyRigidBodyCylinderSensor(ByVal xnRigidBody As XmlNode)
            End Sub

            Protected Overridable Sub ModifyRigidBodyAttachment(ByVal xnRigidBody As XmlNode)
            End Sub

            Protected Overridable Sub ModifyRigidBodyMuscle(ByVal xnRigidBody As XmlNode)
            End Sub

            Protected Overridable Sub ModifyRigidBodyStretchReceptor(ByVal xnRigidBody As XmlNode)
            End Sub

            Protected Overridable Sub ModifyRigidBodySpring(ByVal xnRigidBody As XmlNode)
            End Sub

            Protected Overridable Sub ModifyRigidBodyMesh(ByVal xnRigidBody As XmlNode)

                Dim bIsCollision As Boolean = CBool(m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "IsCollisionObject"))
                Dim bFreeze As Boolean = CBool(m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "Freeze"))
                Dim strType As String = m_xnProjectXml.GetSingleNodeValue(xnRigidBody, "MeshType").ToUpper

                If bIsCollision AndAlso strType = "TRIANGULAR" AndAlso Not bFreeze Then
                    Throw New System.Exception("Dynamic triangle mesh types are not supported with the Bullet physics engine. Please freeze this part type, or convert it to be convex.")
                End If

            End Sub

            Protected Overridable Sub ModifyRigidBodyMouth(ByVal xnRigidBody As XmlNode)
            End Sub

            Protected Overridable Sub ModifyRigidBodyStomach(ByVal xnRigidBody As XmlNode)
            End Sub

            Protected Overridable Sub ModifyRigidBodyOdorSensor(ByVal xnRigidBody As XmlNode)
            End Sub

            Protected Overridable Sub ModifyRigidBodySphere(ByVal xnRigidBody As XmlNode)
            End Sub

            '''-------------------------------------------------------------------------------------------------
            ''' \brief  Modify rigid body plane.
            '''         For the plane we need to go through all children and check to see if there are any that
            '''         have static joints to the plane. If they do then we need to replace them with free joints
            '''         that are frozen.
            '''
            ''' \author David Cofer
            ''' \date   1/17/2014
            '''
            ''' \param  xnRigidBody The xn rigid body.
            '''-------------------------------------------------------------------------------------------------
            Protected Overridable Sub ModifyRigidBodyPlane(ByVal xnRigidBody As XmlNode)

                'Now modify all the child nodes.
                Dim xnChildren As XmlNode = m_xnProjectXml.GetNode(xnRigidBody, "ChildBodies", False)
                If Not xnChildren Is Nothing Then
                    For Each xnChild As XmlNode In xnChildren.ChildNodes
                        ConvertFrozenJointsOnPlane(xnRigidBody, xnChild)
                    Next
                End If

            End Sub

            Protected Overridable Sub ConvertFrozenJointsOnPlane(ByVal xnRigidBody As XmlNode, ByVal xnChild As XmlNode)

                Dim xnJoint As XmlNode = m_xnProjectXml.GetNode(xnChild, "Joint", False)
                If Not xnJoint Is Nothing Then
                    Dim strType As String = m_xnProjectXml.GetSingleNodeValue(xnJoint, "Type").ToUpper

                    If strType = "STATIC" Then
                        'We have found a child part that has a static joint. We need to replace this with a free joint

                        m_xnProjectXml.UpdateSingleNodeValue(xnJoint, "Type", "FreeJoint")
                        m_xnProjectXml.UpdateSingleNodeValue(xnJoint, "PartType", "AnimatGUI.DataObjects.Physical.Joints.FreeJoint")
                        m_xnProjectXml.UpdateSingleNodeValue(xnChild, "Freeze", "True")
                    End If
                End If

            End Sub

            Protected Overridable Sub ModifyMultiSegmentSpring(ByVal xnRigidBody As XmlNode)
            End Sub

            Protected Overridable Sub ModifyRigidBodyTorus(ByVal xnRigidBody As XmlNode)
                Throw New System.Exception("Torus body parts are not currently supported with the Bullet physics engine. Please change this to a different part type before a conversion can happen.")
            End Sub

            Protected Overridable Sub ModifyRigidBodyEllipsoid(ByVal xnRigidBody As XmlNode)
            End Sub

#End Region

#End Region

#Region "Joint Modifiers"

            Protected Overridable Sub ModifyJoint(ByVal xnJoint As XmlNode)

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
                    Case "DISTANCE"
                        ModifyJointDistance(xnJoint)
                End Select

                Dim strID As String = m_xnProjectXml.GetSingleNodeValue(xnJoint, "ID")
                m_hashJoints.Add(strID, xnJoint)

            End Sub

#Region "Joint Type Modifiers"

            Protected Overridable Sub ModifyJointHinge(ByVal xnJoint As XmlNode)
            End Sub

            Protected Overridable Sub ModifyJointBallSocket(ByVal xnJoint As XmlNode)
            End Sub

            Protected Overridable Sub ModifyJointPrismatic(ByVal xnJoint As XmlNode)
            End Sub

            Protected Overridable Sub ModifyJointStatic(ByVal xnJoint As XmlNode)
            End Sub

            Protected Overridable Sub ModifyJointDistance(ByVal xnJoint As XmlNode)
                Throw New System.Exception("Distance joints are not currently supported with the Bullet physics engine. Please change this joint to a different type before a conversion can happen.")
            End Sub

#End Region

#End Region


#End Region

#Region "Modify Nervous System"

            Protected Overridable Sub ModifyNervousSystem(ByVal xnOrganism As XmlNode)

                'Nothing to do for this conversion

            End Sub

#End Region

#Region "Stimuli Modifiers"

            Protected Overridable Sub ModifyStimuli(ByVal xnProjectNode As XmlNode, ByVal xnSimNode As XmlNode)

                'Nothing to do for this conversion

            End Sub

#End Region

#Region "Data Tool Modifiers"

            Protected Overridable Sub ModifyToolViewers(ByVal xnProjectNode As XmlNode, ByVal xnSimNode As XmlNode)

                'Nothing to do for this conversion

            End Sub

#End Region

#Region "Helper Methods"

#End Region

        End Class

    End Namespace
End Namespace
