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
        Namespace TutorialTests

            <CodedUITest()>
            Public Class TutorialsConversionUITest
                Inherits ConversionUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"

                <TestMethod(), _
                 DataSource("System.Data.OleDb", _
                            "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                            "PhysicsEngines", _
                            DataAccessMethod.Sequential), _
                 DeploymentItem("TestCases.accdb")>
                Public Sub Tutorial_Hinge()
                    If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Angle", 0.005)

                    m_strProjectName = "Hinge"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\BodyParts"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\BodyParts\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\BodyParts\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod(), _
                 DataSource("System.Data.OleDb", _
                            "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                            "PhysicsEngines", _
                            DataAccessMethod.Sequential), _
                 DeploymentItem("TestCases.accdb")>
                Public Sub Tutorial_Muscle()
                    If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("MV", 0.000001)
                    aryMaxErrors.Add("Tension", 0.6)
                    aryMaxErrors.Add("Length", 0.002)
                    aryMaxErrors.Add("MN", 0.005)
                    aryMaxErrors.Add("A", 0.005)


                    m_strProjectName = "Muscle"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\BodyParts"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\BodyParts\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\BodyParts\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_Spring()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Position", 0.005)
                    aryMaxErrors.Add("Velocity", 0.005)
                    aryMaxErrors.Add("FF", 0.005)

                    m_strProjectName = "Spring"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\BodyParts"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\BodyParts\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\BodyParts\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod(), _
                  DataSource("System.Data.OleDb", _
                             "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                             "PhysicsEngines", _
                             DataAccessMethod.Sequential), _
                  DeploymentItem("TestCases.accdb")>
                Public Sub Tutorial_StretchReceptor()
                    If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Length", 0.001)
                    aryMaxErrors.Add("Ia", 0.5)
                    aryMaxErrors.Add("II", 0.3)

                    m_strProjectName = "StretchReceptor"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\BodyParts"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\BodyParts\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\BodyParts\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod(), _
                DataSource("System.Data.OleDb", _
                           "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                           "PhysicsEngines", _
                           DataAccessMethod.Sequential), _
                DeploymentItem("TestCases.accdb")>
                Public Sub Tutorial_Buoyancy()
                    If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Y", 0.05)

                    m_strProjectName = "Buoyancy"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\MechanicalTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\MechanicalTests\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\MechanicalTests\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_MechanicalTestSpring()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Length", 0.05)
                    aryMaxErrors.Add("Velocity", 0.1)
                    aryMaxErrors.Add("Acceleration", 0.5)
                    aryMaxErrors.Add("Damping", 0.05)
                    aryMaxErrors.Add("Tension", 0.05)
                    aryMaxErrors.Add("2", 0.0001)

                    m_strProjectName = "Spring"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\MechanicalTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\MechanicalTests\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\MechanicalTests\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    Dim aryIgnoreRows As New ArrayList

                    aryIgnoreRows.Add(New Point(5000, 10230))

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    Dim strDataPrefix As String = "AfterConversion_"

                    SetWindowsToOpen()

                    Debug.WriteLine("ConvertProject")

                    CleanupConversionProjectDirectory()

                    StartExistingProject()

                    'Converted project should always ask to be converted.
                    OpenDialogAndWait("Convert Project", Nothing, Nothing)

                    'Set Physics Method
                    ExecuteIndirectActiveDialogMethod("SetPhysics", New Object() {m_strPhysicsEngine}, , , True)

                    'Click 'Ok' button
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, , , True)

                    Threading.Thread.Sleep(3000)

                    OpenDialogAndWait("Part not supported", Nothing, Nothing)

                    'Click 'Yes' button
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, , , True)

                    Threading.Thread.Sleep(3000)

                    OpenDialogAndWait("Project Conversion", Nothing, Nothing)

                    'Click 'Ok' button
                    ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, , , True)

                    WaitForProjectToOpen()

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "PlaybackControlMode", "FastestPossible"})

                    'Open any data/sim windows that are needed.
                    For Each strWindowPath As String In m_aryWindowsToOpen
                        'Open the Structure_1 body plan editor window
                        ExecuteMethod("DblClickWorkspaceItem", New Object() {strWindowPath}, 2000)
                    Next

                    Threading.Thread.Sleep(3000)

                    If strDataPrefix.Length > 0 Then
                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, strDataPrefix, , aryIgnoreRows)
                    End If


                End Sub

                <TestMethod(), _
                 DataSource("System.Data.OleDb", _
                            "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                            "PhysicsEngines", _
                            DataAccessMethod.Sequential), _
                 DeploymentItem("TestCases.accdb")>
                Public Sub Tutorial_Motors()
                    If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.0005)
                    aryMaxErrors.Add("2", 0.0005)
                    aryMaxErrors.Add("3", 0.0005)
                    aryMaxErrors.Add("Rotation", 0.05)

                    m_strProjectName = "Motors"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\MotorSystems"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\MotorSystems\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\MotorSystems\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_ContactSensor()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("LJA", 0.05)
                    aryMaxErrors.Add("Rotation", 0.05)
                    aryMaxErrors.Add("AEP", 0.07)
                    aryMaxErrors.Add("PEP", 0.07)
                    aryMaxErrors.Add("Ground", 0.2)

                    m_strProjectName = "ContactSensor"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\SensorySystems"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\SensorySystems\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\SensorySystems\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_Eating()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Contact", 2) 'Ignore this
                    aryMaxErrors.Add("Eat", 0.05)
                    aryMaxErrors.Add("Energy", 5)
                    aryMaxErrors.Add("Hungry", 0.05)
                    aryMaxErrors.Add("LeftOdor", 1)
                    aryMaxErrors.Add("OS", 0.05)
                    aryMaxErrors.Add("RightOdor", 1)

                    m_strProjectName = "Eating"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\SensorySystems"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\SensorySystems\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\SensorySystems\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod(), _
                DataSource("System.Data.OleDb", _
                           "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                           "PhysicsEngines", _
                           DataAccessMethod.Sequential), _
                DeploymentItem("TestCases.accdb")>
                Public Sub Tutorial_OdorTracking()
                    If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("LeftOdor", 1)
                    aryMaxErrors.Add("RightOdor", 1)
                    aryMaxErrors.Add("ROS", 1)
                    aryMaxErrors.Add("LOS", 1)
                    aryMaxErrors.Add("LO", 0.05)
                    aryMaxErrors.Add("RO", 0.05)
                    aryMaxErrors.Add("OS", 0.05)

                    m_strProjectName = "OdorTracking"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\SensorySystems"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\SensorySystems\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\SensorySystems\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod(), _
                 DataSource("System.Data.OleDb", _
                            "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                            "PhysicsEngines", _
                            DataAccessMethod.Sequential), _
                 DeploymentItem("TestCases.accdb")>
                Public Sub Tutorial_JointAngle()
                    If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("LJA", 0.05)
                    aryMaxErrors.Add("Rotation", 0.05)
                    aryMaxErrors.Add("AEP", 0.07)
                    aryMaxErrors.Add("PEP", 0.07)

                    m_strProjectName = "JointAngle"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\SensorySystems"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\SensorySystems\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\SensorySystems\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_TouchReceptiveFields()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("A1", 0.0001)
                    aryMaxErrors.Add("A2", 0.0001)
                    aryMaxErrors.Add("A3", 0.0001)
                    aryMaxErrors.Add("A4", 0.0001)
                    aryMaxErrors.Add("A5", 0.0001)

                    m_strProjectName = "TouchReceptiveFields"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\SensorySystems"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\SensorySystems\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\SensorySystems\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_AddingCurrents()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Ie", 0.0000000005)

                    m_strProjectName = "AddingCurrents"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\StimulusTutorials"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\StimulusTutorials\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\StimulusTutorials\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod(), _
                 DataSource("System.Data.OleDb", _
                            "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                            "PhysicsEngines", _
                            DataAccessMethod.Sequential), _
                 DeploymentItem("TestCases.accdb")>
                Public Sub Tutorial_Enabler()
                    If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("3", 0.0001)
                    aryMaxErrors.Add("Rotation", 0.5)
                    aryMaxErrors.Add("AVm", 0.0005)

                    m_strProjectName = "Enabler"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\StimulusTutorials"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\StimulusTutorials\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\StimulusTutorials\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Enabler_Stim", "InitialValue", "0"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Enabler_Stim", "StimulusValue", "0"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Enabler_Stim", "FinalValue", "1"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "EnableMotor", "True"})

                    Dim aryIgnoreRows As New ArrayList
                    'aryIgnoreRows.Add(New Point(11500, 14500))
                    'aryMaxErrors("Rotation") = 0.3

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SwitchEnabledState_", , aryIgnoreRows)

                    AddStimulus("Property Control", m_strStruct1Name, "\Body Plan\Root\Joint_1\Arm", "ArmVisible")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ArmVisible", "InitialValue", "0"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ArmVisible", "StimulusValue", "0"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ArmVisible", "FinalValue", "1"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ArmVisible", "EndTime", "1.5"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ArmVisible", "StartTime", "0.5"})

                    'Add a new axis to chart the joint rotation.
                    ClickToolbarItem("AddAxisToolStripButton", True)

                    'Now add items to the chart to plot the y position of the root, child part, and joint.
                    'Add root part.
                    AddItemToChart("Simulation\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataChart\LineChart\Y Axis 2\Arm", "Name", "ArmVisible"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataChart\LineChart\Y Axis 2\ArmVisible", "DataTypeID", "Visible"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "PropNotSet_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ArmVisible", "LinkedPropertyName", "Visible"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "StimVisiblePropSet_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "Visible", "False"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ArmVisible", "InitialValue", "1"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ArmVisible", "StimulusValue", "1"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ArmVisible", "FinalValue", "0"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "ReverseStim_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & _
                                          "\Behavioral System\Neural Subsystem\5", "Enabled", "False"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DisabledMN_")

                    AddStimulus("Property Control", m_strStruct1Name, "\Behavioral System\" & m_strRootNeuralSystem & "\A", "A_Cm")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\A_Cm", "InitialValue", "0.000000005"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\A_Cm", "FinalValue", "0.000000005"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\A_Cm", "EndTime", "5"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\A_Cm", "StartTime", "1"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\A_Cm", "SetThreshold", "0.0000000001"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\A_Cm", "ValueType", "Equation"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\A_Cm", "Equation", "0.000000005*(1+t)"})

                    ExecuteMethod("SetLinkedItem", New Object() {"Stimuli\A_Cm", _
                              "Simulation\Environment\" & m_strStructureGroup & _
                              "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\A_Cm", "LinkedPropertyName", "Cm"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "RampUpCm_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\A_Cm", "Enabled", "False"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DisableRampUpCm_")

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ArmVisible", "Enabled", "False"})

                    OpenRootBehavioralSubsystem()

                    AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem", _
                                      "AnimatGUI.DataObjects.Behavior.Nodes.PropertyControl", New Point(350, 250), "PC")

                    AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A", _
                                      "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC", "", _
                                      "", False)
                    AssertErrorDialogShown("You must specify a linked object before you can add an adapter to this node.", enumErrorTextType.Contains)


                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC", _
                                            "LinkedPropertyName", "Cm"}, _
                                            "You cannot set the linked object property name until the linked object is set.", enumErrorTextType.Contains)

                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC", _
                              "Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm"})

                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC", _
                                            "LinkedPropertyName", "Cm"}, _
                                            "No Property was found for object", enumErrorTextType.Contains)

                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC", _
                                                         "LinkedPropertyName", "Visible"})

                    AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A", _
                                      "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC", "", _
                                      "", False)

                    If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3"})) Then
                        Throw New System.Exception("3 adapter was not added")
                    End If
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3", "Name", "A_PC"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AdapterArmVisible_")

                    AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem", _
                                      "FiringRateGUI.DataObjects.Behavior.Neurons.Normal", New Point(150, 300), "B")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B", "Cm", "40 n"})

                    AddStimulus("Tonic Current", "Organism_1", "\Behavioral System\Neural Subsystem\B", "B_Stim_1")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\B_Stim_1", "EndTime", "5 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\B_Stim_1", "StartTime", "1 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\B_Stim_1", "CurrentOn", "10 n"})

                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_PC", "Delete Node")

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Delete_A_PC_")

                    AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B", _
                                      "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC", "", _
                                      "", False)

                    If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\5"})) Then
                        Throw New System.Exception("5 adapter was not added")
                    End If
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\5", "Name", "B_PC"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "ArmVisible_B_PC_")

                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC", _
                              "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC", _
                                                         "LinkedPropertyName", "Cm"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC", "InitialValue", "0.000000005"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC", "FinalValue", "0.000000005"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC", "SetThreshold", "0.0000000001"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B_PC", "Gain.C", "20 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B_PC", "Gain.D", "5 n"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "B_Cm_")

                    AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem", _
                                      "AnimatGUI.DataObjects.Behavior.Nodes.Subsystem", New Point(400, 30), "S1")

                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B", False})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B_PC", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\B_PC\B", True})
                    ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\PC\B_PC", True})
                    DeleteSelectedParts("Delete Group", True)
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1"}, 2000)
                    ClickMenuItem("PasteInPlaceToolStripMenuItem", True)

                    AddStimulus("Tonic Current", "Organism_1", "\Behavioral System\Neural Subsystem\S1\B", "B_Stim_1")
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\B_Stim_1", "EndTime", "5 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\B_Stim_1", "StartTime", "1 "})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\B_Stim_1", "CurrentOn", "10 n"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "B_Cm_")

                    AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                              "\" & m_strStruct1Name & "\Behavioral System\Neural Subsystem\S1", _
                              "AnimatGUI.DataObjects.Behavior.Nodes.OffPage", New Point(100, 100), "OP")
                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\OP", _
                              "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\B"})

                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\B_PC", "Delete Node")

                    AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\OP", _
                                      "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\PC", "", _
                                      "", False)

                    If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\8"})) Then
                        Throw New System.Exception("8 adapter was not added")
                    End If
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\8", "Name", "OP_PC"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\OP_PC", "Gain.C", "20 n"})
                    ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\OP_PC", "Gain.D", "5 n"})

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "B_Cm_")

                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\B", "Delete Node")

                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeleteB_OP_")

                    AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\PC", _
                                      "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\OP", "", _
                                      "", False)
                    AssertErrorDialogShown("he off-page connector node 'OP' must be associated with another node before you can connect it with a link.", enumErrorTextType.Contains)

                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\OP", _
                              "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1"})
                    AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\PC", _
                                      "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S1\OP", "", _
                                      "", False)
                    AssertErrorDialogShown("Property controls can only have incoming links, not outgoing links.", enumErrorTextType.Contains)

                End Sub

                <TestMethod(), _
                 DataSource("System.Data.OleDb", _
                            "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                            "PhysicsEngines", _
                            DataAccessMethod.Sequential), _
                 DeploymentItem("TestCases.accdb")>
                Public Sub Tutorial_Force()
                    If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Rotation", 0.005)

                    m_strProjectName = "Force"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\StimulusTutorials"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\StimulusTutorials\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\StimulusTutorials\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod(), _
                 DataSource("System.Data.OleDb", _
                            "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                            "PhysicsEngines", _
                            DataAccessMethod.Sequential), _
                 DeploymentItem("TestCases.accdb")>
                Public Sub Tutorial_MotorVelocity()
                    If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Rotation", 0.025)

                    m_strProjectName = "MotorVelocity"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\StimulusTutorials"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\StimulusTutorials\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\StimulusTutorials\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_BehavioralEditor()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("10", 0.0005)

                    m_strProjectName = "BehavioralEditor"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\UsingAnimatlab"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\UsingAnimatlab\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\UsingAnimatlab\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_LineChart()

                    Dim aryMaxErrors As New Hashtable
                    'Don't care about the output here. It is using feedback in the oscillators, so no way
                    'to make random vars consistnetly produce exact same output. so just make sure it loads and runs.

                    m_strProjectName = "LineChart"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\UsingAnimatlab"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\UsingAnimatlab\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\UsingAnimatlab\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    'm_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_Relabel()

                    Dim aryMaxErrors As New Hashtable

                    m_strProjectName = "Relabel"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\UsingAnimatlab"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\UsingAnimatlab\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\UsingAnimatlab\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    'm_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_BistableFiringRateNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF", 0.0005)
                    aryMaxErrors.Add("Vm", 0.0005)
                    aryMaxErrors.Add("Ii", 0.0005)

                    m_strProjectName = "BistableFiringRateNeuron"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_Bumps()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)

                    For i As Integer = 1 To 25
                        aryMaxErrors.Add(i & "FF", 0.0005)
                    Next

                    m_strProjectName = "Bumps"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_ClassicalConditioning()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.00005)
                    aryMaxErrors.Add("2", 0.00005)
                    aryMaxErrors.Add("3", 0.00005)

                    m_strProjectName = "ClassicalConditioning"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_CompartmentalModel()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("C", 0.00005)
                    aryMaxErrors.Add("A", 0.00005)
                    aryMaxErrors.Add("B", 0.00005)
                    aryMaxErrors.Add("Soma", 0.00005)
                    aryMaxErrors.Add("DistalExcitation", 0.00005)
                    aryMaxErrors.Add("DistalInhibition", 0.00005)

                    m_strProjectName = "CompartmentalModel"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_Coordination()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.00005)
                    aryMaxErrors.Add("2", 0.00005)
                    aryMaxErrors.Add("3FF", 0.005)
                    aryMaxErrors.Add("4FF", 0.005)
                    'Don't care about the output here. It is using feedback in the oscillators, so no way
                    'to make random vars consistnetly produce exact same output. so just make sure it loads and runs.
                    'TODO: Need to come up with a way of testing if it is qualitatively doing the right thing.

                    m_strProjectName = "Coordination"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    'm_aryWindowsToOpen.Add("Tool Viewers\ToolViewer_1")
                    'm_aryWindowsToOpen.Add("Tool Viewers\ToolViewer_2")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_ElectricalSynapses()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.00005)
                    aryMaxErrors.Add("2", 0.00005)

                    m_strProjectName = "ElectricalSynapses"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_EndogenousBursters()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.00005)
                    aryMaxErrors.Add("FF", 0.005)
                    aryMaxErrors.Add("Vm", 0.00005)
                    aryMaxErrors.Add("Intrinsic", 0.0000000001)
                    aryMaxErrors.Add("External", 0.0000000001)

                    m_strProjectName = "EndogenousBursters"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\ToolViewer_1")
                    m_aryWindowsToOpen.Add("Tool Viewers\ToolViewer_2")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_FastNeuralNetNeurons()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)

                    aryMaxErrors.Add("BiFF", 0.005)
                    aryMaxErrors.Add("BiVm", 0.00005)
                    aryMaxErrors.Add("BiExternalI", 0.0000000001)
                    aryMaxErrors.Add("BiIntrinsicI", 0.0000000001)

                    aryMaxErrors.Add("1External", 0.0000000001)
                    aryMaxErrors.Add("1Synaptic", 0.0000000001)
                    aryMaxErrors.Add("2Synaptic", 0.0000000001)
                    aryMaxErrors.Add("2External", 0.0000000001)
                    aryMaxErrors.Add("2Vm", 0.00005)
                    aryMaxErrors.Add("1Vm", 0.00005)
                    aryMaxErrors.Add("2FF", 0.005)
                    aryMaxErrors.Add("1FF", 0.005)

                    aryMaxErrors.Add("5ExternalOff", 0.0000000001)
                    aryMaxErrors.Add("7ExternalOff", 0.0000000001)
                    aryMaxErrors.Add("6SynapticOff", 0.0000000001)
                    aryMaxErrors.Add("5FFOff", 0.005)
                    aryMaxErrors.Add("6FFOff", 0.005)
                    aryMaxErrors.Add("7FFOff", 0.005)
                    aryMaxErrors.Add("8SynapticOn", 0.0000000001)
                    aryMaxErrors.Add("9ExternalOn", 0.0000000001)
                    aryMaxErrors.Add("5ExternalOn", 0.0000000001)
                    aryMaxErrors.Add("9FFon", 0.005)
                    aryMaxErrors.Add("5FFOn", 0.005)
                    aryMaxErrors.Add("8FFOn", 0.005)

                    aryMaxErrors.Add("3External", 0.0000000001)
                    aryMaxErrors.Add("3Synaptic", 0.0000000001)
                    aryMaxErrors.Add("4Synaptic", 0.0000000001)
                    aryMaxErrors.Add("4External", 0.0000000001)
                    aryMaxErrors.Add("3Vm", 0.00005)
                    aryMaxErrors.Add("4Vm", 0.00005)
                    aryMaxErrors.Add("3FF", 0.005)
                    aryMaxErrors.Add("4FF", 0.005)

                    aryMaxErrors.Add("10ExternalUp", 0.0000000001)
                    aryMaxErrors.Add("11SynapticUp", 0.0000000001)
                    aryMaxErrors.Add("12ExternalUp", 0.0000000001)
                    aryMaxErrors.Add("12FFUp", 0.005)
                    aryMaxErrors.Add("10FFUp", 0.005)
                    aryMaxErrors.Add("11FFUp", 0.005)
                    aryMaxErrors.Add("13SynapticDown", 0.0000000001)
                    aryMaxErrors.Add("14ExternalDown", 0.0000000001)
                    aryMaxErrors.Add("10ExternalDown", 0.0000000001)
                    aryMaxErrors.Add("10FFDown", 0.005)
                    aryMaxErrors.Add("14FFDown", 0.005)
                    aryMaxErrors.Add("13FFDown", 0.005)

                    aryMaxErrors.Add("NExternalI", 0.0000000001)
                    aryMaxErrors.Add("NVm", 0.00005)
                    aryMaxErrors.Add("NFF", 0.005)

                    aryMaxErrors.Add("PIntrinsicI", 0.0000000001)
                    aryMaxErrors.Add("PExternalI", 0.0000000001)
                    aryMaxErrors.Add("PVm", 0.00005)
                    aryMaxErrors.Add("PInterburst", 0.005)
                    aryMaxErrors.Add("PFF", 0.005)

                    aryMaxErrors.Add("RandIntrinsicI", 0.0000000001)
                    aryMaxErrors.Add("RandVm", 0.00005)
                    aryMaxErrors.Add("RandFF", 0.005)


                    m_strProjectName = "FastNeuralNetNeurons"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\BistableNeuronProperties")
                    m_aryWindowsToOpen.Add("Tool Viewers\Excitatory_Synapse_Properties")
                    m_aryWindowsToOpen.Add("Tool Viewers\Gated_Synapse_Properties")

                    m_aryWindowsToOpen.Add("Tool Viewers\Inhibitory_Synapse_Properties")
                    m_aryWindowsToOpen.Add("Tool Viewers\Modulatory_Synapse_Properties")
                    m_aryWindowsToOpen.Add("Tool Viewers\Gated_Synapse_Properties")
                    m_aryWindowsToOpen.Add("Tool Viewers\Normal_Neuron_Properties")
                    m_aryWindowsToOpen.Add("Tool Viewers\Pacemaker_Neuron_Properties")
                    m_aryWindowsToOpen.Add("Tool Viewers\Random_Neuron_Properties")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_FiringRateGatedSynapse()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)

                    aryMaxErrors.Add("1ExternalOn", 0.0000000001)
                    aryMaxErrors.Add("3ExternalOn", 0.0000000001)
                    aryMaxErrors.Add("2SynapticOn", 0.0000000001)
                    aryMaxErrors.Add("1ExternalOff", 0.0000000001)
                    aryMaxErrors.Add("4SynapticOff", 0.0000000001)
                    aryMaxErrors.Add("5ExternalOff", 0.0000000001)
                    aryMaxErrors.Add("3FFOn", 0.005)
                    aryMaxErrors.Add("2FFOn", 0.005)
                    aryMaxErrors.Add("1FFOn", 0.005)
                    aryMaxErrors.Add("4FFOff", 0.005)
                    aryMaxErrors.Add("5FFOff", 0.005)
                    aryMaxErrors.Add("1FFoff", 0.005)

                    m_strProjectName = "FiringRateGatedSynapse"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_FiringRateModulatorySynapse()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)

                    aryMaxErrors.Add("1ExternalPos", 0.0000000001)
                    aryMaxErrors.Add("2SynapticPos", 0.0000000001)
                    aryMaxErrors.Add("3ExternalPos", 0.0000000001)
                    aryMaxErrors.Add("1ExternalNeg", 0.0000000001)
                    aryMaxErrors.Add("5ExternalNeg", 0.0000000001)
                    aryMaxErrors.Add("4SynapticNeg", 0.0000000001)
                    aryMaxErrors.Add("2FFPos", 0.005)
                    aryMaxErrors.Add("1FFPos", 0.005)
                    aryMaxErrors.Add("3FFPos", 0.005)
                    aryMaxErrors.Add("5FFNeg", 0.005)
                    aryMaxErrors.Add("4FFNeg", 0.005)
                    aryMaxErrors.Add("1FFNeg", 0.005)

                    m_strProjectName = "FiringRateModulatorySynapse"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_FiringRateNormalSynapse()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)

                    aryMaxErrors.Add("1Ie", 0.0000000001)
                    aryMaxErrors.Add("2Ii", 0.0000000001)
                    aryMaxErrors.Add("1Vm", 0.0005)
                    aryMaxErrors.Add("2Vm", 0.0005)
                    aryMaxErrors.Add("2FF", 0.005)
                    aryMaxErrors.Add("1FF", 0.005)

                    m_strProjectName = "FiringRateNormalSynapse"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_HH_IonChannels()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)

                    aryMaxErrors.Add("DelayedRectK", 0.0000000001)
                    aryMaxErrors.Add("FastNa", 0.0000000001)
                    aryMaxErrors.Add("1Vm", 0.0005)
                    aryMaxErrors.Add("1", 0.0000000001)

                    m_strProjectName = "HH_IonChannels"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub


                <TestMethod()>
                Public Sub Tutorial_IntegrateAndFireNeurons()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.00005)

                    m_strProjectName = "IntegrateAndFireNeurons"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_IonChannel_HalfCenters()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("A", 0.00005)
                    aryMaxErrors.Add("B", 0.00005)

                    m_strProjectName = "IonChannel_HalfCenters"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_LateralInhibition()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)

                    For i As Integer = 1 To 16
                        aryMaxErrors.Add(i.ToString, 0.0001)
                    Next

                    m_strProjectName = "LateralInhibition"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_LTP()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.00005)
                    aryMaxErrors.Add("2", 0.00005)

                    m_strProjectName = "LTP"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub


                <TestMethod()>
                Public Sub Tutorial_NetworkOscillators()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.00005)
                    aryMaxErrors.Add("2", 0.00005)
                    aryMaxErrors.Add("3", 0.00005)
                    aryMaxErrors.Add("A", 0.005)
                    aryMaxErrors.Add("B", 0.005)

                    m_strProjectName = "NetworkOscillators"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    'Random numbers are again preventing direct comparison of outputs. So skip this test. Just make sure it opens and runs.

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_NonspikingChemicalSynapses()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.00005)
                    aryMaxErrors.Add("2", 0.00005)

                    m_strProjectName = "NonspikingChemicalSynapses"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_NormalFiringRateNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF", 0.005)
                    aryMaxErrors.Add("Vm", 0.00005)
                    aryMaxErrors.Add("Ie", 0.0000000001)

                    m_strProjectName = "NormalFiringRateNeuron"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub


                <TestMethod()>
                Public Sub Tutorial_Pacemaker()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF", 0.005)
                    aryMaxErrors.Add("Vm", 0.00005)
                    aryMaxErrors.Add("External", 0.0000000001)
                    aryMaxErrors.Add("Intrinsic", 0.0000000001)
                    aryMaxErrors.Add("Interval", 0.005)

                    m_strProjectName = "Pacemaker"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_RandomFiringRateNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF", 0.005)
                    aryMaxErrors.Add("Vm", 0.00005)
                    aryMaxErrors.Add("Ii", 0.0000000001)

                    m_strProjectName = "RandomFiringRateNeuron"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_SpikingChemicalSynapses()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.00005)
                    aryMaxErrors.Add("2", 0.00005)
                    aryMaxErrors.Add("3", 0.00005)

                    m_strProjectName = "SpikingChemicalSynapses"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_VoltageDependentSynapses()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.00005)
                    aryMaxErrors.Add("2", 0.00005)
                    aryMaxErrors.Add("3", 0.00005)

                    m_strProjectName = "VoltageDependentSynapses"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                End Sub

                <TestMethod()>
                Public Sub Tutorial_OscillatingBumps()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)

                    For i As Integer = 10 To 16
                        aryMaxErrors.Add(i.ToString, 0.0001)
                    Next

                    m_strProjectName = "OscillatingBumps"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\TutorialTests\NeuralNetworks"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\TutorialTests\NeuralNetworks\" & m_strProjectName
                    m_strStructureGroup = "Organisms"
                    m_strStruct1Name = "Organism_1"

                    m_aryWindowsToOpen.Clear()
                    m_aryWindowsToOpen.Add("Tool Viewers\DataChart")

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

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
