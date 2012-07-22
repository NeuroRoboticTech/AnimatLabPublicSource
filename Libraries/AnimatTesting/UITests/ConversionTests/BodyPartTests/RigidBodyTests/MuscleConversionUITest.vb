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
                Public Class MuscleConversionUITest
                    Inherits MuscleBaseConversionUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"

                    <TestMethod()>
                    Public Sub Test_Muscle()
                        m_strProjectName = "MuscleTest"
                        MuscleTest()
                    End Sub

                    <TestMethod()>
                    Public Sub Test_Enabler()

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Tension", 0.3)
                        aryMaxErrors.Add("default", 0.3)

                        m_strProjectName = "EnablerTest"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        'Load and convert the project.
                        TestConversionProject("EnableActive_False_", aryMaxErrors)

                        Dim aryIgnoreRows As New ArrayList
                        aryIgnoreRows.Add(New Point(3001, 3001))

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\EnablerStim", "EnableWhenActive", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "EnableActive_True_", -1, aryIgnoreRows)

                        aryIgnoreRows.Clear()
                        aryIgnoreRows.Add(New Point(1501, 1501))

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\EnablerStim", "EnableWhenActive", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\EnablerStim", "EndTime", "6.5"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\EnablerStim", "StartTime", "5.5"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Times_")
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Times_", -1, aryIgnoreRows)

                    End Sub

                    <TestMethod()>
                    Public Sub Test_Muscle_Hinge_NoMotor()

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Bottom_A", 0.05)
                        aryMaxErrors.Add("Top_A", 0.05)
                        aryMaxErrors.Add("Bottom_Length", 0.05)
                        aryMaxErrors.Add("Top_Length", 0.05)
                        aryMaxErrors.Add("Bottom_Tension", 0.05)
                        aryMaxErrors.Add("Top_Tension", 0.05)
                        aryMaxErrors.Add("Bottom_MN", 0.05)
                        aryMaxErrors.Add("Top_MN", 0.05)
                        aryMaxErrors.Add("Rotation", 0.05)
                        aryMaxErrors.Add("default", 0.05)

                        m_strProjectName = "MuscleTest_Hinge_NoMotor_dmg"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyDataNoMuscle")

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Balance_Force", "PositionZ", "-0.1 c"})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Force_Z_-0_1cm_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Balance_Force", "ForceY", "0.98039215686274509803921568627451 "})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Force_Y_0_98N_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Balance_Force", "PositionZ", "2 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Balance_Force", "ForceY", "1.6666666666666666666666666666667 "})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Force_Z_1.6N_")

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\BodyData"}, 2000)
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Balance_Force", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Arm\Top_Muscle", "Enabled", "True"})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Top_Enabled_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Top_Attach", "WorldPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Top_Attach", "WorldPosition.Y", "3 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Top_Attach", "WorldPosition.Z", "-10 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Top_MN", "CurrentOn", "4.24 n"})
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "TopAttach_0_3_-10_")

                        aryMaxErrors("Top_Tension") = 0.1

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Top_Attach", "WorldPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Top_Attach", "WorldPosition.Y", "0 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Top_Attach", "WorldPosition.Z", "-11 c"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "TopAttach_0_0_-11_")

                        aryMaxErrors("Top_Tension") = 0.05

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
