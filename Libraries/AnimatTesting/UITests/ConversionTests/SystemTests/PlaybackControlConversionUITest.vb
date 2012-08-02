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
                Public Class PlaybackControlConversionUITest
                    Inherits ConversionUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"
                    '

                    <TestMethod()>
                    Public Sub Test_PlaybackControl()
                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("TotalStepTimeSmoothed", 0.0005)
                        aryMaxErrors.Add("FrameRate", 2)
                        aryMaxErrors.Add("default", 0.0005)

                        m_strProjectName = "OdorMouthAndEating"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()

                        'Load and convert the project, but do not run it yet.
                        TestConversionProject("", aryMaxErrors)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "SimulationEndTime", "2.2"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "PlaybackControlMode", "MatchPhysicsStep"})

                        CreateChart()

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MatchPhysicsStep_0_4ms_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.8 m"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MatchPhysicsStep_0_8ms_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "FrameRate", "15"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MatchPhysicsStep_0_8ms_FrameRate_15_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1.2 m"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MatchPhysicsStep_1_2ms_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.4 m"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "PlaybackControlMode", "UsePresetValue"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "PresetPlaybackTimeStep", "3 m"})

                        aryMaxErrors("TotalStepTimeSmoothed") = 0.001

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "PresetPlayback_3ms_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "FrameRate", "30"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "PresetPlayback_3ms_FrameRate_30_")

                    End Sub


                    Protected Sub CreateChart()
                        Debug.WriteLine("CreateChart")

                        'Select the LineChart to add.
                        AddChart("Line Chart")

                        'Select the Chart axis
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1", False})

                        'Change the start end time of the chart to 0.5 to 2 seconds.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart", "CollectEndTime", "2"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart", "CollectStartTime", "0.5"})

                        AddItemToChart("Simulation")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Simulation", "Name", "TotalStepTimeSmoothed"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\TotalStepTimeSmoothed", "DataTypeID", "TotalRealTimeForStepSmoothed"})

                        ClickToolbarItem("AddAxisToolStripButton", True)
                        AddItemToChart("Simulation")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 2\Simulation", "Name", "FrameRate"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 2\FrameRate", "DataTypeID", "ActualFrameRate"})

                    End Sub


#Region "Additional test attributes"
                    '
                    ' You can use the following additional attributes as you write your tests:
                    '
                    ' Use TestInitialize to run code before running each test
                    <TestInitialize()> Public Overrides Sub MyTestInitialize()
                        MyBase.MyTestInitialize()

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
