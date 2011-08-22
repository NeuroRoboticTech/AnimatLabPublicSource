Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard

Namespace BodyEditorTests
    Namespace BodyPartTests
        Namespace JointTests

            <CodedUITest()>
            Public Class UniversialUITest
                Inherits JointUITest

#Region "Properties"

                Public Overrides ReadOnly Property IsMotorizedJoint() As Boolean
                    Get
                        Return False
                    End Get
                End Property

#End Region

#Region "Methods"

                <TestMethod()>
                Public Sub TestUniversal()
                    TestJoint()
                End Sub

                'Protected Overrides Sub TestMovableRigidBodyProperties(ByVal strStructure As String, ByVal strPart As String)
                '    MyBase.TestMovableRigidBodyProperties(strStructure, strPart)


                'End Sub

                'Protected Overrides Sub RepositionChildPart()
                '    MyBase.RepositionChildPart()

                '    'Reposition the joint relative to the parent part
                '    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "LocalPosition.X", "0"})
                '    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "LocalPosition.Y", "0"})
                '    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "LocalPosition.Z", "0"})

                '    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "Rotation.X", "0"})
                '    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "Rotation.Y", "0"})
                '    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "Rotation.Z", "90"})

                'End Sub

                Protected Overrides Sub TestConstraintLimitsByFalling()
                    'First simulate the arm falling down under gravity.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, "Fall1_")

                    'Reposition the joint.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "LocalPosition.X", "0.125"})

                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, "Fall2_")

                    'Reposition the joint.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "LocalPosition.X", "0"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "LocalPosition.Y", "0.2"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "LocalPosition.X", "0.1"})

                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, "Fall3_")

                    'Reset the joint.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "LocalPosition.X", "-0.125"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "LocalPosition.Y", "0"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "LocalPosition.Z", "0"})

                    'Reposition the blocker.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Blocker", "LocalPosition.X", "0"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Y", "-0.25"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Z", "0"})

                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, "Block1_")

                    'Reposition the blocker.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Blocker", "LocalPosition.X", "0"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Y", "0.125"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Z", "0"})

                End Sub

                Protected Overrides Sub TestConstraintLimitsWithForce()
                    'Add force stimulus to child part. 
                    AddStimulus("Force", "Structure_1", "Root\Joint_1\Arm", "ArmForce", "Stimulus_2")
                    SetForceStimulus("ArmForce", False, True, 1, 1.1, 0, 0, 0, 0, 0, 0.05, 0, 0, 0)

                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, "Force1_")

                    SetForceStimulus("ArmForce", False, True, 1, 1.1, 0, 0, 0, 0, 0, 0, 0.05, 0, 0)

                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, "Force2_")

                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Arm"})

                    'Now move the arm over using the mouse.
                    MovePartAxis("Structure_1", "Root\Joint_1\Arm", _
                                 m_strMoveArmWorldZAxis, m_strMoveArmLocalZAxis, _
                                 m_ptTranslateZAxisStart, m_ptTranslateZAxisEnd, _
                                 m_dblMinTranArmWorldZ, m_dblMaxTranArmWorldZ, _
                                 m_dblMinTranArmStructZ, m_dblMaxTranArmStructZ, _
                                 m_dblMinTranArmLocalZ, m_dblMaxTranArmLocalZ)

                    RunSimulationWaitToEnd()
                    LoadDataChart(m_strRootFolder & m_strTestDataPath, "DataTool_1.txt", "MouseMoveArm_")
                    CompareColummData(3, -1, -1, enumDataComparisonType.WithinRange, 0, 0.3)

                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1"})

                    'Now move the arm over using the mouse.
                    MovePartAxis("Structure_1", "Root\Joint_1", _
                                 m_strMoveJointWorldYAxis, m_strMoveJointLocalYAxis, _
                                 m_ptTransJointYAxisStart, m_ptTransJointYAxisEnd, _
                                 m_dblMinTranJointWorldY, m_dblMaxTranJointWorldY, _
                                 m_dblMinTranJointStructY, m_dblMaxTranJointStructY, _
                                 m_dblMinTranJointLocalY, m_dblMaxTranJointLocalY)

                    RunSimulationWaitToEnd()
                    LoadDataChart(m_strRootFolder & m_strTestDataPath, "DataTool_1.txt", "MouseMoveJoint_")
                    CompareColummData(3, -1, -1, enumDataComparisonType.WithinRange, 0, 0.3)

                End Sub

#Region "Additional test attributes"
                '
                ' You can use the following additional attributes as you write your tests:
                '
                ' Use TestInitialize to run code before running each test
                <TestInitialize()> Public Overrides Sub MyTestInitialize()
                    MyBase.MyTestInitialize()

                    m_strProjectName = "UniversalTest"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\JointTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\JointTests\" & m_strProjectName

                    m_strJointType = "Universal"

                    m_strJointChartMovementName = ""
                    m_strJointChartMovementType = ""

                    m_strJointChartVelocityName = ""
                    m_strJointChartVelocityType = ""

                    m_strInitialJointXRot = "0"
                    m_strInitialJointYRot = "0"
                    m_strInitialJointZRot = "90"

                    m_strFallUpper1 = "0.1"
                    m_strFallUpper2 = "0.2"
                    m_strFallUpper3 = "0.05"

                    m_strFallLower1 = "-0.1"
                    m_strFallLower2 = "-0.2"
                    m_strFallLower3 = "-0.05"

                    m_ptTranslateZAxisStart = New Point(790, 634)
                    m_ptTranslateZAxisEnd = New Point(702, 717)

                    m_ptMoveJoint1Start = New Point(639, 428)
                    m_ptMovejoint1End = New Point(671, 204)

                    m_dblMaxMovePos = 0.03863424
                    m_dblMaxMovePosError = 0.005

                    m_dblMaxMoveVel = 0.8681851
                    m_dblMaxMoveVelError = 0.05

                    m_dblMaxRotPos = 0.09949701
                    m_dblMaxRotPosError = 0.01

                    m_strForceXJointRotation = "90"

                    m_ptRotateJoint1Start = New Point(854, 464)
                    m_ptRotatejoint1End = New Point(798, 784)

                    m_dblMouseRotateJointMin = 20
                    m_sblMouseRotateJointMax = 120

                    m_ptTransJointYAxisStart = New Point(641, 430)
                    m_ptTransJointYAxisEnd = New Point(654, 322)

                    CleanupProjectDirectory()
                End Sub


#End Region

#End Region

            End Class


        End Namespace
    End Namespace
End Namespace
