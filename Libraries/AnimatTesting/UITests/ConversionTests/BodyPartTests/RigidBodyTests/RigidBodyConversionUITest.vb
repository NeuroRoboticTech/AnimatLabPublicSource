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
                Public Class RigidBodyConversionUITest
                    Inherits ConversionUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"
                    '

                    <TestMethod(), _
                    DataSource("System.Data.OleDb", _
                               "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                               "BodyBoxRotationTestData", _
                               DataAccessMethod.Sequential), _
                    DeploymentItem("TestCases.accdb")>
                    Public Sub Test_BodyBoxRotations()
                        If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                        m_strProjectName = TestContext.DataRow("TestName").ToString
                        Dim dblRootRotX As Double = CDbl(TestContext.DataRow("RootRotX"))
                        Dim dblRootRotY As Double = CDbl(TestContext.DataRow("RootRotY"))
                        Dim dblRootRotZ As Double = CDbl(TestContext.DataRow("RootRotZ"))
                        Dim strRootOrientation As String = CStr(TestContext.DataRow("RootOrientation"))
                        Dim dblChild1RotX As Double = CDbl(TestContext.DataRow("ChildRotX"))
                        Dim dblChild1RotY As Double = CDbl(TestContext.DataRow("ChildRotY"))
                        Dim dblChild1RotZ As Double = CDbl(TestContext.DataRow("ChildRotZ"))
                        Dim strChild1Orientation As String = CStr(TestContext.DataRow("ChildOrientation"))
                        Dim dblRootAttachPosX As Double = CDbl(TestContext.DataRow("RootX"))
                        Dim dblRootAttachPosY As Double = CDbl(TestContext.DataRow("RootY"))
                        Dim dblRootAttachPosZ As Double = CDbl(TestContext.DataRow("RootZ"))
                        Dim dblChild1AttachPosX As Double = CDbl(TestContext.DataRow("Child1X"))
                        Dim dblChild1AttachPosY As Double = CDbl(TestContext.DataRow("Child1Y"))
                        Dim dblChild1AttachPosZ As Double = CDbl(TestContext.DataRow("Child1Z"))
                        Dim dblChild2AttachPosX As Double = CDbl(TestContext.DataRow("Child2X"))
                        Dim dblChild2AttachPosY As Double = CDbl(TestContext.DataRow("Child2Y"))
                        Dim dblChild2AttachPosZ As Double = CDbl(TestContext.DataRow("Child2Z"))
                        Dim strDataPrefix As String = CStr(TestContext.DataRow("DataPrefix"))

                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName

                        ModifyRootRotationInProjectFile(m_strOldProjectFolder, dblRootRotX, dblRootRotY, dblRootRotZ, strRootOrientation)
                        ModifyChild1RotationInProjectFile(m_strOldProjectFolder, "1585fba8-bcf1-47a2-8fe5-38403e4e18f0", dblChild1RotX, dblChild1RotY, dblChild1RotZ, strChild1Orientation)

                        'Load and convert the project.
                        ConvertProject()

                        'Now verify that the positions of all the attachments are correct in world space.
                        VerifyPropertyValue("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root Attach", "WorldPosition.X.ActualValue", dblRootAttachPosX)
                        VerifyPropertyValue("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root Attach", "WorldPosition.Y.ActualValue", dblRootAttachPosY)
                        VerifyPropertyValue("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root Attach", "WorldPosition.Z.ActualValue", dblRootAttachPosZ)

                        VerifyPropertyValue("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_2\Body_2 Attach", "WorldPosition.X.ActualValue", dblChild1AttachPosX)
                        VerifyPropertyValue("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_2\Body_2 Attach", "WorldPosition.Y.ActualValue", dblChild1AttachPosY)
                        VerifyPropertyValue("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_2\Body_2 Attach", "WorldPosition.Z.ActualValue", dblChild1AttachPosZ)

                        VerifyPropertyValue("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3\Body_3 Attach", "WorldPosition.X.ActualValue", dblChild2AttachPosX)
                        VerifyPropertyValue("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3\Body_3 Attach", "WorldPosition.Y.ActualValue", dblChild2AttachPosY)
                        VerifyPropertyValue("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3\Body_3 Attach", "WorldPosition.Z.ActualValue", dblChild2AttachPosZ)

                    End Sub


                    <TestMethod(), _
                     DataSource("System.Data.OleDb", _
                                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                                "PhysicsEngines", _
                                DataAccessMethod.Sequential), _
                     DeploymentItem("TestCases.accdb")>
                    Public Sub Test_BalanceBeam()
                        If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Hinge", 1.1)
                        aryMaxErrors.Add("Mlt", 0.01)
                        aryMaxErrors.Add("Mrt", 0.01)
                        aryMaxErrors.Add("Slt", 0.01)
                        aryMaxErrors.Add("Srt", 0.01)
                        aryMaxErrors.Add("Mll", 0.01)
                        aryMaxErrors.Add("Mrl", 0.01)
                        aryMaxErrors.Add("Sll", 0.01)
                        aryMaxErrors.Add("Srl", 0.01)
                        aryMaxErrors.Add("Left", 0.01)
                        aryMaxErrors.Add("Right", 0.01)
                        aryMaxErrors.Add("default", 0.01)

                        m_strProjectName = "BalanceBeam"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "BalanceBeam"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        Dim aryIgnoreRows As New ArrayList


                        'Test forces
                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors, -1, aryIgnoreRows)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Clock", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Clock_2N_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Clock", "ForceY", "-2 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CounterClock_2N_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Clock", "ForceY", "2 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Clock", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\CounterClock", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CounterClock_2N_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\CounterClock", "ForceY", "-2 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Clock_2N_")

                        'Test Muscles
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\CounterClock", "ForceY", "2 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\CounterClock", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Left_Muscle", "Enabled", "True"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\BalanceBeam\Body Plan\Root\Hinge\Arm\Muscle_Left", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Left_Muscle_Only_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\BalanceBeam\Body Plan\Root\Hinge\Arm\Muscle_Right", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Muscle_LA_RP_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Right_Muscle", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Muscle_BothActive_")

                        ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\BalanceBeam\Body Plan\Root\Hinge\Arm\Muscle_Left", "AttachmentPoints"}, 500)
                        ExecuteActiveDialogMethod("Automation_RemoveAttachment", New Object() {"Attach_Arm_Left"})
                        ExecuteActiveDialogMethod("Automation_AddAttachment", New Object() {"Attach_Arm_Right"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Muscle_Swap_L_Attach_")

                        ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\BalanceBeam\Body Plan\Root\Hinge\Arm\Muscle_Left", "AttachmentPoints"}, 500)
                        ExecuteActiveDialogMethod("Automation_RemoveAttachment", New Object() {"Attach_Arm_Right"})
                        ExecuteActiveDialogMethod("Automation_AddAttachment", New Object() {"Attach_Arm_Left"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Left_Muscle", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Right_Muscle", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\BalanceBeam\Body Plan\Root\Hinge\Arm\Muscle_Left", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\BalanceBeam\Body Plan\Root\Hinge\Arm\Muscle_Right", "Enabled", "False"})

                        'Test Springs
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\BalanceBeam\Body Plan\Root\Hinge\Arm\Spring_Left", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Spring_L_2_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\BalanceBeam\Body Plan\Root\Hinge\Arm\Spring_Left", "NaturalLength", "1.8"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Spring_L_1_8_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\BalanceBeam\Body Plan\Root\Hinge\Arm\Spring_Right", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Spring_L_1_8_R_2_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\BalanceBeam\Body Plan\Root\Hinge\Arm\Spring_Right", "NaturalLength", "1.5"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Spring_L_1_8_R_1_5_")

                        ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\BalanceBeam\Body Plan\Root\Hinge\Arm\Spring_Left", "AttachmentPoints"}, 500)
                        ExecuteActiveDialogMethod("Automation_RemoveAttachment", New Object() {"Attach_Arm_Left"})
                        ExecuteActiveDialogMethod("Automation_AddAttachment", New Object() {"Attach_Arm_Right"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Spring_Swap_L_Attach_")

                    End Sub


#Region "Additional test attributes"
                    '
                    ' You can use the following additional attributes as you write your tests:
                    '
                    ' Use TestInitialize to run code before running each test
                    <TestInitialize()> Public Overrides Sub MyTestInitialize()
                        MyBase.MyTestInitialize()

                        'This test compares data to that generated from the old version. We never re-generate the data in V2, so this should always be false 
                        'regardless of the setting in app.config.
                        m_bGenerateTempates = False

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
