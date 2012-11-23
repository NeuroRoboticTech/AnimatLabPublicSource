Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports AnimatTesting.Framework

Namespace UITests
    Namespace PerformanceTests

        <CodedUITest()>
        Public Class NumberOfItemsUITest
            Inherits PerformanceUITest

            Protected m_aryStepTimes As New ArrayList

            '<TestMethod()>
            Public Sub Test_NumberOfItems()

                'Start the application.
                StartApplication("")

                'Click the New Project button.
                ClickToolbarItem("NewToolStripButton", False)

                'Set params and hit ok button
                ExecuteActiveDialogMethod("SetProjectParams", New Object() {"NumberOfItems", m_strRootFolder & "\Libraries\AnimatTesting\TestProjects\PerformanceTests"})
                ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, 2000)

                CreateStructure(m_strStructureGroup, m_strStruct1Name, m_strStruct1Name, False)

                'Open the Structure_1 body plan editor window
                ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan"})

                'Add a root cylinder part.
                AddRootPartType(m_strStructureGroup, m_strStruct1Name, "Box")

                'Set the root part to be frozen.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Freeze", "True"})

                'Select the LineChart to add.
                AddChart("Line Chart")

                'Select the Chart axis
                ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1", False})

                'Add root to chart
                AddItemToChart("Simulation")

                'Set the name of the data chart item to root_y.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Simulation", "Name", "StepTime"})

                'Change the end time of the data chart to 45 seconds.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart", "CollectStartTime", "0.1"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart", "CollectEndTime", "0.2"})

                'Set simulation to automatically end at a specified time.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "SetSimulationEnd", "True"})

                'Set simulation end time.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "SimulationEndTime", "0.5"})

                GetStepTime()

                For iJoints As Integer = 1 To 50
                    AddChildPartTypeWithJoint("Box", "Hinge", "Simulation\Environment\Organisms\Organism_1\Body Plan\Root")
                    RepositionChildParts(iJoints)
                    GetStepTime()
                Next

                Debug.WriteLine("Average step times")
                For Each dblAvg As Double In m_aryStepTimes
                    Debug.WriteLine(dblAvg)
                Next

            End Sub

            Protected Sub GetStepTime()
                'Run the simulation and wait for it to end.
                RunSimulationWaitToEnd()

                ExecuteMethod("ExportDataCharts", New Object() {"", ""})

                Dim dblAvgTime As Double = CalculateChartColumnAverage(m_strRootFolder & "\Libraries\AnimatTesting\TestProjects\PerformanceTests\NumberOfItems\DataTool_1.txt", 1)

                m_aryStepTimes.Add(dblAvgTime)
            End Sub

            Protected Sub RepositionChildParts(ByVal iChildIdx As Integer)

                'Reposition the child part relative to the parent part
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx & "\Body_" & iChildIdx, "LocalPosition.X", "20 c"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx & "\Body_" & iChildIdx, "LocalPosition.Y", "0"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx & "\Body_" & iChildIdx, "LocalPosition.Z", ((iChildIdx - 1) * 20) & " c"})

                'Reposition the joint relative to the parent part
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx, "LocalPosition.X", "-10 c"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx, "LocalPosition.Y", "0"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx, "LocalPosition.Z", "0"})

                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx, "Rotation.X", "90"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx, "Rotation.Y", "0"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx, "Rotation.Z", "90"})

            End Sub

#Region "Additional test attributes"
            '
            ' You can use the following additional attributes as you write your tests:
            '
            ' Use TestInitialize to run code before running each test
            <TestInitialize()> Public Overrides Sub MyTestInitialize()
                MyBase.MyTestInitialize()

                'Make sure any left over project directory is cleaned up before starting the test.
                DeleteDirectory(m_strRootFolder & "\Libraries\AnimatTesting\TestProjects\PerformanceTests\NumberOfItems")

                SetStructureNames("1", False)
            End Sub

            ' Use TestCleanup to run code after each test has run
            <TestCleanup()> Public Overrides Sub MyTestCleanup()
                MyBase.MyTestCleanup()

                DeleteDirectory(m_strRootFolder & "\Libraries\AnimatTesting\TestProjects\PerformanceTests\NumberOfItems")
            End Sub

#End Region

        End Class

    End Namespace
End Namespace
