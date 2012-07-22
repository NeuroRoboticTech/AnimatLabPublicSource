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
            Namespace JointTests

                <CodedUITest()>
                Public Class DistanceUITest
                    Inherits JointUITest

#Region "Methods"

                    <TestMethod()>
                    Public Sub TestDistance()

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Block_X", 0.03)
                        aryMaxErrors.Add("Block_Y", 0.03)
                        aryMaxErrors.Add("Block_Z", 0.03)
                        aryMaxErrors.Add("Rotation", 0.03)
                        aryMaxErrors.Add("default", 0.03)

                        StartNewProject()
                        'Set simulation to automatically end at a specified time.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "SetSimulationEnd", "True"})

                        'Set simulation end time.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "SimulationEndTime", "6"})

                        'Add a root part.
                        AddRootPartType(m_strStructureGroup, m_strStruct1Name, "Box")

                        'Set the root part to be frozen.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Freeze", "True"})

                        'We have tested moving/rotating the root part, now test doing it on a child part.
                        AddChildPartTypeWithJoint("Box", "Hinge", "Simulation\Environment\Structures\Structure_1\Body Plan\Root")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_1", "Name", "Arm"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\Body_1_Graphics", "Name", "Arm_Graphics"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Name", "Hinge"})

                        'We have tested moving/rotating the root part, now test doing it on a child part.
                        AddChildPartTypeWithJoint("Box", "Distance", "Simulation\Environment\Structures\Structure_1\Body Plan\Root\Hinge\Arm")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Joint_2\Body_2", "Name", "Block"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Joint_2\Block\Body_2_Graphics", "Name", "Block_Graphics"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Joint_2", "Name", "Distance"})

                        SizeParts()

                        PositionParts()

                        AddStimulus("Motor Velocity", m_strStruct1Name, "\Body Plan\Root\Hinge", "MotorStim")
                        SetMotorVelocityStimulus("MotorStim", True, True, 0, 5, False, True, 2, "")

                        SetupChart()

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Y90_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "Rotation.X", "90"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "Rotation.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "Rotation.Z", "0"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "X90_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "LocalPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "LocalPosition.Y", "1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "LocalPosition.Z", "0"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "WorldPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "WorldPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "WorldPosition.Z", "0"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "X90_Reposition_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "Rotation.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "Rotation.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "Rotation.Z", "90"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Z90_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "LocalPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "LocalPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "LocalPosition.Z", "1"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "WorldPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "WorldPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "WorldPosition.Z", "0"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Z90_Reposition_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "Rotation.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "Rotation.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "Rotation.Z", "45"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Z45_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block", "LocalPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block", "LocalPosition.Y", "0.5"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block", "LocalPosition.Z", "0"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Block_Y_05_")

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan"}, 2000)
                        DeletePart("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Hinge\Arm", "Delete Body Part", True)
                        PasteChildPartTypeWithJoint("Hinge", "Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0, True)
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_3", "Name", "Hinge"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "LocalPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "LocalPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "LocalPosition.Z", "1"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "WorldPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "WorldPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "WorldPosition.Z", "0"})

                        AddStimulus("Motor Velocity", m_strStruct1Name, "\Body Plan\Root\Hinge", "MotorStim")
                        SetMotorVelocityStimulus("MotorStim", True, True, 0, 5, False, True, 2, "")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "EnableLimits", "False"})

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\DataTool_1"}, 2000)
                        AddChartItems()

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Pasted_")

                    End Sub

                    Protected Sub SizeParts()
                        'Resize the root part and graphic.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Height", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Width", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Length", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Height", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Width", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Length", "0.1"})

                        'Resize the child part and graphic.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "Height", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "Width", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "Length", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Arm_Graphics", "Height", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Arm_Graphics", "Width", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Arm_Graphics", "Length", "0.1"})

                        'Resize the child part and graphic.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block", "Height", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block", "Width", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block", "Length", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block\Block_Graphics", "Height", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block\Block_Graphics", "Width", "0.1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block\Block_Graphics", "Length", "0.1"})

                    End Sub

                    Protected Sub PositionParts()
                        'Reposition the child part relative to the parent part
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "LocalPosition.X", "1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "LocalPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm", "LocalPosition.Z", "0"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block", "LocalPosition.X", "0.3"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block", "LocalPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block", "LocalPosition.Z", "0"})

                        'Reposition the joint relative to the parent part
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "WorldPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "WorldPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "WorldPosition.Z", "0"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "Rotation.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "Rotation.Y", "90"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "Rotation.Z", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Hinge", "EnableLimits", "False"})

                    End Sub

                    Protected Sub SetupChart()
                        'Setup Chart
                        AddChart("Line Chart")

                        AddChartItems()
                    End Sub

                    Protected Sub AddChartItems()
                        'Select the Chart axis
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1", False})

                        'Change the end time of the data chart to 45 seconds.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart", "CollectEndTime", "5"})

                        AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Block", "Name", "Block_X"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Block_X", "DataTypeID", "WorldPositionX"})

                        AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Block", "Name", "Block_Y"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Block_Y", "DataTypeID", "WorldPositionY"})

                        AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Hinge\Arm\Distance\Block")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Block", "Name", "Block_Z"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Block_Z", "DataTypeID", "WorldPositionZ"})

                        AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Hinge")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Hinge", "Name", "Rotation"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Rotation", "DataTypeID", "JointRotation"})

                    End Sub

#Region "Additional test attributes"
                    '
                    ' You can use the following additional attributes as you write your tests:
                    '
                    ' Use TestInitialize to run code before running each test
                    <TestInitialize()> Public Overrides Sub MyTestInitialize()
                        MyBase.MyTestInitialize()

                        m_strProjectName = "DistanceTest"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\JointTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\JointTests\" & m_strProjectName
                        m_strJointType = "Distance"

                        CleanupProjectDirectory()
                    End Sub

#End Region

#End Region

                End Class


            End Namespace
        End Namespace
    End Namespace
End Namespace