Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports AnimatTesting.Framework

Namespace UITests
    Namespace CreationTests

        <CodedUITest()>
        Public Class NewProjectUITest
            Inherits AnimatUITest

            <TestMethod()>
            Public Sub Test_CreateNewProject()

                'Start the application.
                StartApplication("")

                'Click the New Project button.
                ClickToolbarItem("NewToolStripButton", False)

                'Set params and hit ok button
                ExecuteActiveDialogMethod("SetProjectParams", New Object() {"TestProject", m_strRootFolder & "\Libraries\AnimatTesting\TestProjects\CreationTests"})
                ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, 2000)

                'Click the add structure button.
                ClickToolbarItem("AddStructureToolStripButton", True)

                'Open the Structure_1 body plan editor window
                ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan"})

                'Add a root cylinder part.
                AddRootPartType(m_strStructureGroup, m_strStruct1Name, "Box")

                'Select the LineChart to add.
                AddChart("Line Chart")

                'Select the Chart axis
                ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1", False})

                'Add root to chart
                AddItemToChart("Simulation\" & m_strStruct1Name & "\Body Plan\Root")

                'Set the name of the data chart item to root_y.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Root", "Name", "Root_Y"})

                'Change the data type to track the world Y position.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Root_Y", "DataTypeID", "WorldPositionY"})

                'Change the end time of the data chart to 45 seconds.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart", "CollectEndTime", "45"})

                'Set simulation to automatically end at a specified time.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "SetSimulationEnd", "True"})

                'Set simulation end time.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "SimulationEndTime", "50"})

                'Run the simulation and wait for it to end.
                RunSimulationWaitToEnd()

                'Compare chart data to verify simulation results.
                CompareSimulation(m_strRootFolder & "\Libraries\AnimatTesting\TestData\CreationTests\NewProjectUITest\")

                'Save the project
                ClickToolbarItem("SaveToolStripButton", True)

                'Now verify that if we try and create a new project at the same location we get an error.
                'Click the New Project button.
                ClickToolbarItem("NewToolStripButton", False)

                'Enter text and verify error. Verify the error.
                ExecuteActiveDialogMethod("SetProjectParams", New Object() {"TestProject", m_strRootFolder & "\Libraries\AnimatTesting\TestProjects\CreationTests"})
                ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, 200, True, True)
                AssertErrorDialogShown(" already exists. Please choose a different name or location for the project.", enumErrorTextType.Contains)
                'ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, 200, True, True)
                'ExecuteIndirectActiveDialogMethod("ClickCancelButton", Nothing, 200, True, True)

            End Sub

            <TestMethod()>
            Public Sub Test_CloseOpen()
                'There was a bug where when you open, then close a project and run the simulation it was throwing an error
                'The root cause was that it was not releasing event handlers for the main app/sim events and items in the
                'garbage collector were firing. This tests this to make sure it does not happen anymore.

                m_strProjectName = "TestCloseOpen"
                m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\CreationTests"

                StartExistingProject()

                WaitWhileBusy()
                WaitForProjectToOpen()

                'Run the simulation and wait for it to end.
                RunSimulationWaitToEnd()

                'Close the project
                CloseProjectQuiet()

                OpenExistingProject(True)

                'Run the simulation and wait for it to end.
                RunSimulationWaitToEnd()

            End Sub



#Region "Additional test attributes"
            '
            ' You can use the following additional attributes as you write your tests:
            '
            ' Use TestInitialize to run code before running each test
            <TestInitialize()> Public Overrides Sub MyTestInitialize()
                MyBase.MyTestInitialize()

                'Make sure any left over project directory is cleaned up before starting the test.
                DeleteDirectory(m_strRootFolder & "\Libraries\AnimatTesting\TestProjects\CreationTests\TestProject")
            End Sub

            ' Use TestCleanup to run code after each test has run
            <TestCleanup()> Public Overrides Sub MyTestCleanup()
                MyBase.MyTestCleanup()

                DeleteDirectory(m_strRootFolder & "\Libraries\AnimatTesting\TestProjects\CreationTests\TestProject")
            End Sub

#End Region

        End Class

    End Namespace
End Namespace
