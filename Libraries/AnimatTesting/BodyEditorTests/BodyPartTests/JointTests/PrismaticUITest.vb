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
            Public Class PrismaticUITest
                Inherits JointUITest

#Region "Methods"

                <TestMethod()>
                Public Sub TestPrismatic()
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

#Region "Additional test attributes"
                '
                ' You can use the following additional attributes as you write your tests:
                '
                ' Use TestInitialize to run code before running each test
                <TestInitialize()> Public Overrides Sub MyTestInitialize()
                    MyBase.MyTestInitialize()

                    m_strProjectName = "PrismaticTest"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\JointTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\JointTests\" & m_strProjectName

                    m_strJointType = "Prismatic"

                    m_strJointChartMovementName = "JointPosition"
                    m_strJointChartMovementType = "JointPosition"

                    m_strInitialJointXPos = "0"

                    m_strInitialJointXRot = "0"
                    m_strInitialJointYRot = "0"
                    m_strInitialJointZRot = "90"

                    m_strFallUpper1 = "0.1"
                    m_strFallUpper2 = "0.2"
                    m_strFallUpper3 = "0.05"

                    m_strFallLower1 = "-0.1"
                    m_strFallLower2 = "-0.2"
                    m_strFallLower3 = "-0.05"

                    m_ptTranslateZAxisStart = New Point(790, 631)
                    m_ptTranslateZAxisEnd = New Point(723, 675)

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

                    CleanupProjectDirectory()
                End Sub

#End Region

#End Region

            End Class


        End Namespace
    End Namespace
End Namespace
