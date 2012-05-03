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


                    <TestMethod()>
                    Public Sub Test_Spring()
                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("XForce", 0.02)
                        aryMaxErrors.Add("Length", 0.04)
                        aryMaxErrors.Add("Length2", 0.04)
                        aryMaxErrors.Add("Position", 0.04)
                        aryMaxErrors.Add("Velocity", 0.02)
                        aryMaxErrors.Add("Acceleration", 0.02)
                        aryMaxErrors.Add("DampingForce", 0.02)
                        aryMaxErrors.Add("SpringTension", 0.02)
                        aryMaxErrors.Add("2", 0.0001)
                        aryMaxErrors.Add("default", 0.02)

                        m_strProjectName = "SpringTest"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Length.ActualValue", 5)

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Disabled", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulationAnalysis(m_strProjectPath & "\" & m_strProjectName, "BodyData", m_strRootFolder & m_strTestDataPath, "EnableAdapter_", "Position")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Disabled", "Enabled", "False"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Damping", "1 K"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Underdamped_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Damping", "6.32 K"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CriticallyDamped_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Damping", "10 K"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "OverDamped_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Damping", "1 K"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "NaturalLength", "3.5"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "K1_L3_5_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Stiffness", "5 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "K1_L3_5_S5_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm", "LocalPosition.X", "-0.5"})
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Length.ActualValue", 5.5)
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "K1_L3_5_S5_P5_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm", "LocalPosition.Y", "1.25"})
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Length.ActualValue", 5.544)
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Up_1_25_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Enabled", "False"})
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Length.ActualValue", 0)
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Disabled_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Enabled", "True"})
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Length.ActualValue", 5.544)
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm", "LocalPosition.Y", "0.55"})
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Length.ActualValue", 5.5)
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "K1_L3_5_S5_P5_")

                        ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "AttachmentPoints"}, 500)
                        ExecuteActiveDialogMethod("Automation_AddAttachment", New Object() {"RightAttach2"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                        AssertErrorDialogShown("Only 2 are allowed for this part type. Please reduce the number of attachments to this number.", enumMatchType.Equals)
                        ExecuteActiveDialogMethod("Automation_RemoveAttachment", New Object() {"RightAttach"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Length.ActualValue", 5.59)

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "RightAttach2_")

                        ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "AttachmentPoints"}, 500)
                        ExecuteActiveDialogMethod("Automation_RemoveAttachment", New Object() {"RightAttach2"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Length.ActualValue", 0)

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Disabled_")

                        ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "AttachmentPoints"}, 500)
                        ExecuteActiveDialogMethod("Automation_RemoveAttachment", New Object() {"ArmAttach"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Length.ActualValue", 0)

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Disabled_")

                        ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "AttachmentPoints"}, 500)
                        ExecuteActiveDialogMethod("Automation_AddAttachment", New Object() {"ArmAttach"})
                        ExecuteActiveDialogMethod("Automation_AddAttachment", New Object() {"RightAttach"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Enabled", "True"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "NaturalLength", "3.5"})
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Length.ActualValue", 5.5)

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "K1_L3_5_S5_P5_")

                        ''These lines are only used when commenting out the above lines for testing.
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm", "LocalPosition.X", "-0.5"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Damping", "1 K"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "NaturalLength", "3.5"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Stiffness", "5 "})

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan"}, 2000)

                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", False})
                        ExecuteMethod("ClickMenuItem", New Object() {"CopyToolStripMenuItem"})

                        PasteChildPartTypeWithJoint("", "Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0, False)
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Name", "Spring1"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Name", "Spring2"})

                        ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Tool Viewers\BodyData"}, 1000)
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 1", False})
                        AddItemToChart("Organism_1\Body Plan\Base\Joint_1\Arm\Spring2")
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 1\Spring2", "Name", "Length2"})
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2", "Length.ActualValue", 5.5)
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Length2_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2", "Damping", "0.5 K"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2", "Stiffness", "10"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring1", "Enabled", "False"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Length1Disable_")

                        DeletePart("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring1", "Delete Body Part")
                        Threading.Thread.Sleep(1000)
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring1"})) Then
                            Throw New System.Exception("Spring1 node was not removed correctly.")
                        End If
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 1\Length"})) Then
                            Throw New System.Exception("Length chart node was not removed correctly.")
                        End If
                        If Not GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B", "LinkedPart.BodyPart") Is Nothing Then
                            Throw New System.Exception("Linked body part not removed.")
                        End If
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Disabled"})) Then
                            Throw New System.Exception("Adapter node was not removed correctly.")
                        End If
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Length1Disable_")


                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem"}, 2000)

                        AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A", _
                                          "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B", "", "", False)
                        AssertErrorDialogShown("You must specify a linked body part before you can add an adapter to this node.", enumMatchType.Equals)

                        ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B", _
                                                                     "Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2"})

                        AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A", _
                                          "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B", "", "", False)
                        If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2"})) Then
                            Throw New System.Exception("A_B adapter node was not removed created.")
                        End If
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Name", "A_B"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2", "Stiffness", "5"})
                        RunSimulationWaitToEnd()
                        CompareSimulationAnalysis(m_strProjectPath & "\" & m_strProjectName, "BodyData", m_strRootFolder & m_strTestDataPath, "CreateA_B_", "Position")

                        'Add subsystem.
                        AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem", _
                                          "AnimatGUI.DataObjects.Behavior.Nodes.Subsystem", New Point(316, 30), "S2")

                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A", False})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_B", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_B\A", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B\A_B", True})
                        DeleteSelectedParts("Delete Group", True)
                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2"}, 2000)
                        ExecuteMethod("ClickMenuItem", New Object() {"PasteInPlaceToolStripMenuItem"})

                        AddStimulus("Tonic Current", "Organism_1", "\Behavioral System\Neural Subsystem\S2\A", "Stim_A")
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stim_A", "EndTime", "10 "})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stim_A", "StartTime", "5 "})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stim_A", "CurrentOn", "30 n"})

                        'Add these neurons to the chart.
                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\BodyData"}, 2000)
                        ExecuteMethod("ClickToolbarItem", New Object() {"AddAxisToolStripButton"})
                        AddItemToChart("Organism_1\Behavioral System\Neural Subsystem\Nodes\S2\Nodes\A")
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 2\A", "Name", "2"})

                        RunSimulationWaitToEnd()
                        CompareSimulationAnalysis(m_strProjectPath & "\" & m_strProjectName, "BodyData", m_strRootFolder & m_strTestDataPath, "CreateA_B_", "Position")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2", "Stiffness", "10"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\A_B", "Enabled", "False"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Length1Disable_")


                        DeletePart("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_2\RightSide\RightAttach", "Delete Body Part")
                        Threading.Thread.Sleep(1000)
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_2\RightSide\RightAttach"})) Then
                            Throw New System.Exception("RightAttach node was not removed correctly.")
                        End If
                         If DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2", "AttachmentPoints.Count"), Integer) <> 1 Then
                            Throw New System.Exception("Attachment not removed from spring.")
                        End If
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Length2Disable_")

                        ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2", "AttachmentPoints"}, 500)
                        ExecuteActiveDialogMethod("Automation_AddAttachment", New Object() {"RightAttach2"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                        If DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2", "Enabled"), Boolean) = False Then
                            Throw New System.Exception("Spring 2 not Reenabled.")
                        End If
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2", "NaturalLength", "3.5"})
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2", "Length.ActualValue", 5.59)
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Spring2Right2_")

                        ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Simulation\Environment\Organisms\Organism_1"}, 1000)
                        DeletePart("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2", "Delete Body Part", True)
                        PasteChildPartTypeWithJoint("", "Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0, False)
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2", "Length.ActualValue", 5.59)
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Spring2Right2_")

                        DeletePart("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_2\RightSide", "Delete Body Part")
                        Threading.Thread.Sleep(1000)
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_2\RightSide"})) Then
                            Throw New System.Exception("RightAttach node was not removed correctly.")
                        End If
                        If DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring2", "AttachmentPoints.Count"), Integer) <> 1 Then
                            Throw New System.Exception("Attachment not removed from spring.")
                        End If
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Length2Disable_")


                    End Sub


                    '<TestMethod()> _
                    Public Sub Generate_SpringTestTemplate()
                        'Generate_SpringEnableAdpaterTemplate()
                        Generate_SpringCreateABTemplate()
                        'Generate_SpringStiff5KTemplate()
                    End Sub

                    Protected Sub Generate_SpringEnableAdpaterTemplate()
                        Dim strDataFile As String = "C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\SpringTest\EnableAdapter_BodyData.txt"
                        Dim aryChartColumns() As String = {""}
                        Dim aryChartData As New List(Of List(Of Double))
                        Util.ReadCSVFileToList(strDataFile, aryChartColumns, aryChartData, True)

                        Dim iPositionIdx As Integer = Util.FindColumnNamed(aryChartColumns, "Position")

                        Dim aryTime As List(Of Double) = aryChartData(0)
                        Dim aryData As List(Of Double) = aryChartData(iPositionIdx)

                        Dim oAnalysis As New DataAnalyzer()
                        oAnalysis.FindCriticalPoints(aryTime, aryData, -1, -1)

                        oAnalysis.SaveData("C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\SpringTest\EnableAdapter_BodyData_Analysis.txt")

                    End Sub

                    Protected Sub Generate_SpringCreateABTemplate()
                        Dim strDataFile As String = "C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\SpringTest\CreateA_B_BodyData.txt"
                        Dim aryChartColumns() As String = {""}
                        Dim aryChartData As New List(Of List(Of Double))
                        Util.ReadCSVFileToList(strDataFile, aryChartColumns, aryChartData, True)

                        Dim iPositionIdx As Integer = Util.FindColumnNamed(aryChartColumns, "Position")

                        Dim aryTime As List(Of Double) = aryChartData(0)
                        Dim aryData As List(Of Double) = aryChartData(iPositionIdx)

                        Dim oAnalysis As New DataAnalyzer()
                        oAnalysis.FindCriticalPoints(aryTime, aryData, -1, -1)

                        oAnalysis.SaveData("C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\SpringTest\CreateA_B_BodyData_Analysis.txt")

                    End Sub

                    Protected Sub Generate_SpringStiff5KTemplate()
                        Dim strDataFile As String = "C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\SpringTest\K1_L3_5_S5_BodyData.txt"
                        Dim aryChartColumns() As String = {""}
                        Dim aryChartData As New List(Of List(Of Double))
                        Util.ReadCSVFileToList(strDataFile, aryChartColumns, aryChartData, True)

                        Dim iPositionIdx As Integer = Util.FindColumnNamed(aryChartColumns, "Position")

                        Dim aryTime As List(Of Double) = aryChartData(0)
                        Dim aryData As List(Of Double) = aryChartData(iPositionIdx)

                        Dim oAnalysis As New DataAnalyzer()
                        oAnalysis.FindCriticalPoints(aryTime, aryData, -1, -1)

                        'Dim oTest As New CriticalPoint
                        'oTest.CompareTime = CriticalPoint.enumComparisonType.None
                        'oTest.CompareValue = CriticalPoint.enumComparisonType.Fixed
                        'oTest.Time = 0.5

                        oAnalysis.SaveData("C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\SpringTest\K1_L3_5_S5_BodyData_Analysis.txt")

                    End Sub

                    <TestMethod()>
                    Public Sub Test_Muscle()
                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("MV", 0.0004)
                        aryMaxErrors.Add("MN", 0.0004)
                        aryMaxErrors.Add("Tension", 0.04)
                        aryMaxErrors.Add("Length", 0.04)
                        aryMaxErrors.Add("A", 0.004)
                        aryMaxErrors.Add("default", 0.04)

                        m_strProjectName = "MuscleTest"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "Length.ActualValue", 0.5)

                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\MV_Stim", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MV_Stim_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\MN_Stim", "Enabled", "True"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\MV_Stim", "Enabled", "False"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stretch", "Enabled", "False"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Relax", "Enabled", "False"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MN_Stim_")

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
