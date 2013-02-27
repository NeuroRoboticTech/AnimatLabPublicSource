Imports System.Windows.Forms
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
Imports System
Imports System.CodeDom.Compiler
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.WinControls
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse
Imports MouseButtons = System.Windows.Forms.MouseButtons
Imports AnimatTesting.Framework
Imports System.Xml

Namespace UITests
    Namespace ConversionTests
        Namespace BodyPartTests
            Namespace RigidBodyTests

                <CodedUITest()>
                Public Class ReceptiveFieldConversionUITest
                    Inherits ConversionUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"

                    <TestMethod()>
                    Public Sub Test_ReceptiveFields()

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("A1", 0.01)
                        aryMaxErrors.Add("A2", 0.01)
                        aryMaxErrors.Add("A3", 0.01)
                        aryMaxErrors.Add("A4", 0.01)
                        aryMaxErrors.Add("A5", 0.01)
                        aryMaxErrors.Add("A6", 0.01)
                        aryMaxErrors.Add("A1Ia", 0.000000005)
                        aryMaxErrors.Add("A2Ia", 0.000000005)
                        aryMaxErrors.Add("A3Ia", 0.000000005)
                        aryMaxErrors.Add("A4Ia", 0.000000005)
                        aryMaxErrors.Add("A5Ia", 0.000000005)
                        aryMaxErrors.Add("A6Ia", 0.000000005)
                        aryMaxErrors.Add("default", 0.001)

                        m_strProjectName = "ReceptiveFields"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", "ReceptiveFieldSensor.ReceptiveCurrentGain.C", "100 n"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CurrentGain_C_100n_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", "ReceptiveFieldSensor.ReceptiveFieldGain.Width", "300 "})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FieldGain_Width_300_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", "ReceptiveFieldSensor.ReceptiveFieldGain.Width", "50 "})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FieldGain_Width_50_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", "ReceptiveFieldSensor.ReceptiveFieldGain.Width", "150 "})

                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", False})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_ClearReceptiveFieldPairs", Nothing)
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_VerifyFieldPairCount", New Object() {0})

                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_AddReceptiveFieldPair", New Object() {"A3", CSng(0.0), CSng(2.5), CSng(0)})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_VerifyFieldPairExists", New Object() {"A3", "(0.00, 2.50, 0.00)"})

                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_ClearReceptiveFieldPairs", Nothing)
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", "WidthSections", "10"})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_VerifyFieldPairCount", New Object() {0})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_AddReceptiveFieldPair", New Object() {"A6", CSng(0.0), CSng(2.5), CSng(-1.5)})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_AddReceptiveFieldPair", New Object() {"A5", CSng(0.0), CSng(2.5), CSng(-1)})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_AddReceptiveFieldPair", New Object() {"A4", CSng(0.0), CSng(2.5), CSng(-0.5)})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_AddReceptiveFieldPair", New Object() {"A3", CSng(0.0), CSng(2.5), CSng(0)})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_AddReceptiveFieldPair", New Object() {"A2", CSng(0.0), CSng(2.5), CSng(0.5)})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_AddReceptiveFieldPair", New Object() {"A1", CSng(0.0), CSng(2.5), CSng(1)})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", "ReceptiveFieldSensor.ReceptiveCurrentGain.C", "0.5 n"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", "ReceptiveFieldSensor.ReceptiveCurrentGain.UpperLimit", "100 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", "ReceptiveFieldSensor.ReceptiveFieldGain.Width", "150 "})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "ResetFields_")


                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan"}, 2000)
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", False})
                        ClickMenuItem("CopyToolStripMenuItem", True)
                        PasteChildPartTypeWithJoint("RPRO", "Simulation\Environment\Organisms\Skin\Body Plan\Skin", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0, True)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin\Joint_1\Skin", "Name", "Skin2"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin\Joint_1\Skin2", "WorldPosition.X", "50 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin\Joint_1\Skin2", "WorldPosition.Y", "25 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin\Joint_1\Skin2", "WorldPosition.Z", "0"})

                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin\Joint_1\Skin2", False})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_VerifyFieldPairExists", New Object() {"A6", "(0.00, 2.50, -1.50)"})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_VerifyFieldPairExists", New Object() {"A5", "(0.00, 2.50, -1.00)"})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_VerifyFieldPairExists", New Object() {"A4", "(0.00, 2.50, -0.50)"})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_VerifyFieldPairExists", New Object() {"A3", "(0.00, 2.50, 0.00)"})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_VerifyFieldPairExists", New Object() {"A2", "(0.00, 2.50, 0.50)"})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_VerifyFieldPairExists", New Object() {"A1", "(0.00, 2.50, 1.00)"})

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm\Arm\ArmToPointer", "2_Down1")
                        SetMotorVelocityStimulus("2_Down1", False, True, 1, 2, False, True, 0.05, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm\Arm\ArmToPointer", "2_Down2")
                        SetMotorVelocityStimulus("2_Down2", False, True, 3, 4, False, True, 0.05, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm\Arm\ArmToPointer", "2_Down3")
                        SetMotorVelocityStimulus("2_Down3", False, True, 5, 6, False, True, 0.05, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm\Arm\ArmToPointer", "2_Down4")
                        SetMotorVelocityStimulus("2_Down4", False, True, 7, 8, False, True, 0.05, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm\Arm\ArmToPointer", "2_Down5")
                        SetMotorVelocityStimulus("2_Down5", False, True, 9, 10, False, True, 0.05, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm\Arm\ArmToPointer", "2_Down6")
                        SetMotorVelocityStimulus("2_Down6", False, True, 11, 12, False, True, 0.05, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm", "2_Over1")
                        SetMotorVelocityStimulus("2_Over1", False, True, 2.5, 3, False, True, 0.025, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm", "2_Over2")
                        SetMotorVelocityStimulus("2_Over2", False, True, 4.5, 5, False, True, 0.025, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm", "2_Over3")
                        SetMotorVelocityStimulus("2_Over3", False, True, 6.5, 7, False, True, 0.025, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm", "2_Over4")
                        SetMotorVelocityStimulus("2_Over4", False, True, 8.5, 9, False, True, 0.025, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm", "2_Over5")
                        SetMotorVelocityStimulus("2_Over5", False, True, 10.5, 11, False, True, 0.025, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm\Arm\ArmToPointer", "2_Up1")
                        SetMotorVelocityStimulus("2_Up1", False, True, 2, 3, False, True, -0.05, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm\Arm\ArmToPointer", "2_Up2")
                        SetMotorVelocityStimulus("2_Up2", False, True, 4, 5, False, True, -0.05, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm\Arm\ArmToPointer", "2_Up3")
                        SetMotorVelocityStimulus("2_Up3", False, True, 6, 7, False, True, -0.05, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm\Arm\ArmToPointer", "2_Up4")
                        SetMotorVelocityStimulus("2_Up4", False, True, 8, 9, False, True, -0.05, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm\Arm\ArmToPointer", "2_Up5")
                        SetMotorVelocityStimulus("2_Up5", False, True, 10, 11, False, True, -0.05, "")

                        AddStimulus("Motor Velocity", "Skin", "\Body Plan\Skin\Joint_1\Skin2\SkinToArm\Arm\ArmToPointer", "2_Up6")
                        SetMotorVelocityStimulus("2_Up6", False, True, 12, 13, False, True, -0.05, "")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\BodyData\LineChart", "CollectEndTime", "14"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "SimulationEndTime", "15"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AddSkin2_")

                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", False})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_ClearReceptiveFieldPairs", Nothing)

                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin\Joint_1\Skin2", False})
                        ExecuteAppPropertyMethod("ReceptiveFieldPairs", "Automation_VerifyFieldPairCount", New Object() {6})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "ClearSkin1_")

                    End Sub

                    <TestMethod()>
                    Public Sub Test_ReceptiveFields_Kg_M()

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("1", 0.1)
                        aryMaxErrors.Add("2", 0.1)
                        aryMaxErrors.Add("RootY", 0.1)
                        aryMaxErrors.Add("1Ia", 0.00000001)
                        aryMaxErrors.Add("2Ia", 0.00000001)
                        aryMaxErrors.Add("default", 0.1)

                        m_strProjectName = "ReceptiveFields_Kg_M"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root", "Rotation.X", "180 "})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FlipX_180_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root", "Rotation.X", "0 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root", "ReceptiveFieldSensor.ReceptiveFieldGain.Width", "1 "})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Field_Gain_Width_1_")

                    End Sub


#Region "Additional test attributes"
                    '
                    ' You can use the following additional attributes as you write your tests:
                    '
                    ' Use TestInitialize to run code before running each test
                    <TestInitialize()> Public Overrides Sub MyTestInitialize()
                        MyBase.MyTestInitialize()

                        'This conversion is different than others. The contact collisions are generated differently than before, so I cannot
                        'use the old version data as a template. I will check the output to make sure it is similar, but use the new version as the template.
                        'm_bGenerateTempates = False

                    End Sub

                    <TestCleanup()> Public Overrides Sub MyTestCleanup()
                        MyBase.MyTestCleanup()
                    End Sub
#End Region

#End Region

                End Class

            End Namespace
        End Namespace
    End Namespace
End Namespace
