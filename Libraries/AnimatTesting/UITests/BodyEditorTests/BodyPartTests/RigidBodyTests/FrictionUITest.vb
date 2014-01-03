Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports AnimatTesting.Framework

Namespace UITests
    Namespace BodyEditorTests
        Namespace BodyPartTests
            Namespace RigidBodyTests

                <CodedUITest()>
                Public Class FrictionUITest
                    Inherits BodyPartUITest

#Region "Methods"

                    <TestMethod(), _
                    DataSource("System.Data.OleDb", _
                               "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                               "BoxFrictionTestData", _
                               DataAccessMethod.Sequential), _
                    DeploymentItem("TestCases.accdb")>
                    Public Sub Test_BoxFriction()

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Px", 0.01)
                        aryMaxErrors.Add("Py", 0.02)
                        aryMaxErrors.Add("Pz", 0.01)
                        aryMaxErrors.Add("Vx", 0.1)
                        aryMaxErrors.Add("Vy", 2) 'essentially ignore this setting. It is pretty variable.
                        aryMaxErrors.Add("Vz", 0.1)

                        m_strProjectName = TestContext.DataRow("TestName").ToString
                        Dim bEnabled As Boolean = CBool(TestContext.DataRow("Enabled"))
                        Dim strNewDensity As String = TestContext.DataRow("Density").ToString

                        If Not bEnabled Then Return

                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName

                        m_strStructureGroup = "Structures"
                        m_strStruct1Name = "Structure_1"

                        CleanupConversionProjectDirectory()

                        'Load and convert the project.
                        StartExistingProject()

                        WaitForProjectToOpen()

                        Threading.Thread.Sleep(3000)

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Uk_1_F_10_M_1_")

                        'Change the density of the part and the force applied to it
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_1", "Density", strNewDensity})

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Uk_1_F_10_M_0_1_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_1", "ForceX", "1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_1", "ForceZ", "1"})

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Uk_1_F_1_M_0_1_")

                        'Open the matierals dialog
                        IndirectClickToolbarItem("EditMaterialsToolStripButton", True)

                        ExecuteActiveDialogMethod("Automation_SelectMaterialType", New Object() {"Default"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionLinearPrimary", "0.2"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionLinearSecondary", "0.2"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Uk_0_2_F_1_M_0_1_")

                        'Open the matierals dialog
                        IndirectClickToolbarItem("EditMaterialsToolStripButton", True)

                        ExecuteActiveDialogMethod("Automation_AddMaterialType", New Object() {"Test"})
                        ExecuteActiveDialogMethod("Automation_SelectMaterialType", New Object() {"Test"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionLinearPrimary", "0.3"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionLinearSecondary", "0.3"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                        'run the sim again and make sure results not changed.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Uk_0_2_F_1_M_0_1_")

                        'Set default to 0.3
                        IndirectClickToolbarItem("EditMaterialsToolStripButton", True)
                        ExecuteActiveDialogMethod("Automation_SelectMaterialType", New Object() {"Default"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionLinearPrimary", "0.3"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionLinearSecondary", "0.3"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                        'Set the box to use the new material and run again.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_1", "MaterialTypeName", "Test"})

                        If Not MatchSimObjectPropertyString("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_1", "MaterialTypeName", "Test") Then
                            Throw New System.Exception("Material ID was not set to Test material")
                        End If

                        'run the sim again and make sure results not changed.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Uk_0_3_F_1_M_0_1_")

                        'Set default to 0.3
                        IndirectClickToolbarItem("EditMaterialsToolStripButton", True)
                        ExecuteActiveDialogMethod("Automation_SelectMaterialType", New Object() {"Test"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionLinearPrimary", "0.1"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionLinearSecondary", "0.1"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                        'run the sim again and make sure results not changed.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        ' CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Test_Uk_0_1_F_1_M_0_1_")

                        'Now remove the Test material
                        IndirectClickToolbarItem("EditMaterialsToolStripButton", True)
                        ExecuteActiveDialogMethod("Automation_SelectMaterialType", New Object() {"Test"})
                        ExecuteIndirectActiveDialogMethod("Automation_RemoveOdorType", Nothing, , , True)

                        'Select replacement material
                        ExecuteIndirectActiveDialogMethod("Automation_SelectMaterialType", New Object() {"Default"}, , , True)
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                        'Close material edit box
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                        If Not MatchSimObjectPropertyString("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_1", "MaterialTypeName", "Default") Then
                            Throw New System.Exception("Material ID was not reset to default after deletion")
                        End If

                        'run the sim again and make sure results not changed.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Default_Uk_0_3_F_1_M_0_1_")

                        'Turn friction push off
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_1", "Enabled", "False"})

                        'Now move the part up 100 centimeters in the air and let if fall
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_1", "WorldPosition.Y", "100 c"})

                        'run the sim again and make sure results not changed.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Drop_100cm")

                        'Reset the compliance and damping.
                        IndirectClickToolbarItem("EditMaterialsToolStripButton", True)
                        ExecuteActiveDialogMethod("Automation_SelectMaterialType", New Object() {"Default"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"Compliance", "100 u"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"Damping", "50 "})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                        'run the sim again and make sure results not changed.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Drop_100cm_Comp_100u_Damp_50")

                    End Sub


                    <TestMethod(), _
                    DataSource("System.Data.OleDb", _
                               "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                               "SphereFrictionTestData", _
                               DataAccessMethod.Sequential), _
                    DeploymentItem("TestCases.accdb")>
                    Public Sub Test_SphereFriction()

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Px", 0.0005)
                        aryMaxErrors.Add("Py", 0.0005)
                        aryMaxErrors.Add("Pz", 0.0005)
                        aryMaxErrors.Add("Vx", 0.005)
                        aryMaxErrors.Add("Vy", 0.005)
                        aryMaxErrors.Add("Vz", 0.005)

                        m_strProjectName = TestContext.DataRow("TestName").ToString
                        Dim bEnabled As Boolean = CBool(TestContext.DataRow("Enabled"))
                        Dim strNewDensity As String = TestContext.DataRow("Density").ToString

                        If Not bEnabled Then Return

                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName

                        m_strStructureGroup = "Structures"
                        m_strStruct1Name = "Structure_1"

                        CleanupConversionProjectDirectory()

                        'Load and convert the project.
                        StartExistingProject()

                        WaitForProjectToOpen()

                        Threading.Thread.Sleep(3000)

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Uk_0_F_20_M_1_")

                        'Open the matierals dialog
                        IndirectClickToolbarItem("EditMaterialsToolStripButton", True)

                        ExecuteActiveDialogMethod("Automation_SelectMaterialType", New Object() {"Default"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionAngularNormal", "0.02"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionAngularPrimary", "0.02"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionAngularSecondary", "0.02"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Uk_0_02_F_20_M_1_")


                        'Change the density of the part and the force applied to it
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_1", "Density", strNewDensity})

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Uk_0_02_F_20_M_0_1_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_1", "ForceX", "-10"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_1", "ForceZ", "-10"})

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Uk_0_02_F_10_M_0_5_")

                        'Open the matierals dialog
                        IndirectClickToolbarItem("EditMaterialsToolStripButton", True)

                        ExecuteActiveDialogMethod("Automation_SelectMaterialType", New Object() {"Default"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionAngularNormal", "0.01"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionAngularPrimary", "0.01"})
                        ExecuteActiveDialogMethod("Automation_SetSelectedItemProperty", New Object() {"FrictionAngularSecondary", "0.01"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Uk_0_01_F_10_M_0_5_")

                    End Sub


                   <TestMethod(), _
                     DataSource("System.Data.OleDb", _
                                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                                "PhysicsEngines", _
                                DataAccessMethod.Sequential), _
                     DeploymentItem("TestCases.accdb")>
                    Public Sub Test_MassVolumeDensity()
                        If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Px", 0.0005)
                        aryMaxErrors.Add("Py", 0.0005)
                        aryMaxErrors.Add("Pz", 0.0005)
                        aryMaxErrors.Add("Vx", 0.005)
                        aryMaxErrors.Add("Vy", 0.005)
                        aryMaxErrors.Add("Vz", 0.005)

                        m_strProjectName = "MassVolumeDensity_KgM" 'TestContext.DataRow("TestName").ToString
                        Dim bEnabled As Boolean = True 'CBool(TestContext.DataRow("Enabled"))

                        If Not bEnabled Then Return

                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName

                        m_strStructureGroup = "Structures"
                        m_strStruct1Name = "Structure_1"

                        CleanupConversionProjectDirectory()

                        'Load and convert the project.
                        StartExistingProject()

                        WaitForProjectToOpen()

                        Threading.Thread.Sleep(3000)

                        Dim dblDensity As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Density.ActualValue"), Double)
                        Dim dblVolume As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Volume.ActualValue"), Double)
                        Dim dblMass As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Mass.ActualValue"), Double)

                        If dblDensity <> 10 OrElse dblVolume <> 1 OrElse dblMass <> 10 Then
                            Throw New System.Exception("Invalid box mass settings. Mass: " & dblMass & ", Density: " & dblDensity & ", Volume: " & dblVolume)
                        End If

                        'Change the box size and check again.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Width", "2 "})

                        dblDensity = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Density.ActualValue"), Double)
                        dblVolume = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Volume.ActualValue"), Double)
                        dblMass = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Mass.ActualValue"), Double)

                        If dblDensity <> 10 OrElse dblVolume <> 2 OrElse dblMass <> 20 Then
                            Throw New System.Exception("Invalid box mass settings. Mass: " & dblMass & ", Density: " & dblDensity & ", Volume: " & dblVolume)
                        End If

                        'Change the box size and check again.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Height", "2 "})

                        dblDensity = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Density.ActualValue"), Double)
                        dblVolume = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Volume.ActualValue"), Double)
                        dblMass = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Mass.ActualValue"), Double)

                        If dblDensity <> 10 OrElse dblVolume <> 4 OrElse dblMass <> 40 Then
                            Throw New System.Exception("Invalid box mass settings. Mass: " & dblMass & ", Density: " & dblDensity & ", Volume: " & dblVolume)
                        End If

                        'Change the box size and check again.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Length", "2 "})

                        dblDensity = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Density.ActualValue"), Double)
                        dblVolume = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Volume.ActualValue"), Double)
                        dblMass = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Mass.ActualValue"), Double)

                        If dblDensity <> 10 OrElse dblVolume <> 8 OrElse dblMass <> 80 Then
                            Throw New System.Exception("Invalid box mass settings. Mass: " & dblMass & ", Density: " & dblDensity & ", Volume: " & dblVolume)
                        End If

                        'Change the box size and check again.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Length", "1 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Width", "1 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Height", "1 "})

                        dblDensity = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Density.ActualValue"), Double)
                        dblVolume = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Volume.ActualValue"), Double)
                        dblMass = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Mass.ActualValue"), Double)

                        If dblDensity <> 10 OrElse dblVolume <> 1 OrElse dblMass <> 10 Then
                            Throw New System.Exception("Invalid box mass settings. Mass: " & dblMass & ", Density: " & dblDensity & ", Volume: " & dblVolume)
                        End If

                        ''Change the box density
                        'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Length", "1 "})
                        'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Length", "1 "})
                        'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Length", "1 "})

                        'dblDensity = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Density.ActualValue"), Double)
                        'dblVolume = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Volume.ActualValue"), Double)
                        'dblMass = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_Box\Box", "Mass.ActualValue"), Double)

                        'If dblDensity <> 10 OrElse dblVolume <> 1 OrElse dblMass <> 10 Then
                        '    Throw New System.Exception("Invalid box mass settings. Mass: " & dblMass & ", Density: " & dblDensity & ", Volume: " & dblVolume)
                        'End If

                    End Sub

#Region "Additional test attributes"
                    '
                    ' You can use the following additional attributes as you write your tests:
                    '
                    ' Use TestInitialize to run code before running each test
                    <TestInitialize()> Public Overrides Sub MyTestInitialize()
                        MyBase.MyTestInitialize()

                        CleanupProjectDirectory()
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