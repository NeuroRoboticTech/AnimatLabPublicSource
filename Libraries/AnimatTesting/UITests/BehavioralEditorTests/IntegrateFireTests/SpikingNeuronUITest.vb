Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports AnimatTesting.Framework

Namespace UITests
    Namespace BehavioralEditorTests
        Namespace IntegrateFireTests

            <CodedUITest()>
            Public Class SpikingNeuronUITest
                Inherits NeuralUITest

#Region "Properties"


#End Region

#Region "Methods"

                '<TestMethod()>
                'Public Sub TestIGF_SpikingNeuron()
                '    TestNeuron()
                'End Sub


#Region "Additional test attributes"
                '
                ' You can use the following additional attributes as you write your tests:
                '
                ' Use TestInitialize to run code before running each test
                <TestInitialize()> Public Overrides Sub MyTestInitialize()
                    MyBase.MyTestInitialize()

                    m_strProjectName = "SpikingNeuronTest"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BehavioralEditorTests\IntegrateFireTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BehavioralEditorTests\IntegrateFireTests\" & m_strProjectName

                    m_bCreateStructure = False

                    'm_strJointType = "BallSocket"

                    'm_strJointChartMovementName = ""
                    'm_strJointChartMovementType = ""

                    'm_strJointChartVelocityName = ""
                    'm_strJointChartVelocityType = ""

                    'm_strInitialJointXRot = "0"
                    'm_strInitialJointYRot = "0"
                    'm_strInitialJointZRot = "90"

                    'm_strFallUpper1 = "0.1"
                    'm_strFallUpper2 = "0.2"
                    'm_strFallUpper3 = "0.05"

                    'm_strFallLower1 = "-0.1"
                    'm_strFallLower2 = "-0.2"
                    'm_strFallLower3 = "-0.05"

                    ''m_ptTranslateZAxisStart = New Point(790, 634)
                    ''m_ptTranslateZAxisEnd = New Point(702, 717)

                    'm_ptMoveJoint1Start = New Point(639, 428)
                    'm_ptMovejoint1End = New Point(671, 204)

                    'm_dblMaxMovePos = 0.03863424
                    'm_dblMaxMovePosError = 0.005

                    'm_dblMaxMoveVel = 0.8681851
                    'm_dblMaxMoveVelError = 0.05

                    'm_dblMaxRotPos = 0.09949701
                    'm_dblMaxRotPosError = 0.01

                    'm_strForceXJointRotation = "90"

                    'm_ptRotateJoint1Start = New Point(854, 464)
                    'm_ptRotatejoint1End = New Point(798, 784)

                    'm_dblMouseRotateJointMin = 20
                    'm_sblMouseRotateJointMax = 120

                    'm_ptTransJointYAxisStart = New Point(641, 430)
                    'm_ptTransJointYAxisEnd = New Point(654, 322)

                    CleanupProjectDirectory()
                End Sub


#End Region

#End Region

            End Class



        End Namespace
    End Namespace
End Namespace
