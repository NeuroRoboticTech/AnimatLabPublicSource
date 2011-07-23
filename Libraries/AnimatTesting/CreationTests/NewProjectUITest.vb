Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard

Namespace CreationTests

    <CodedUITest()>
    Public Class NewProjectUITest
        Inherits AnimatUITest

        <TestMethod()>
        Public Sub TestCreateNewProject()

            'Start the application.
            StartApplication("", 8080)

            'Click the New Project button.
            ExecuteMethod("ClickToolbarItem", New Object() {"NewToolStripButton"})

            NewProjectDlg_EnterNameAndPath("TestProject", "C:\Projects\AnimatLabSDK\Experiments")

            'Click the add structure button.
            ExecuteMethod("ClickToolbarItem", New Object() {"AddStructureToolStripButton"}, 200)

            'Open the Structure_1 body plan editor window
            ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan"}, 2000)

            'Set simulation to automatically end at a specified time.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation", "SetSimulationEnd", "True"})

            'Set simulation end time.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation", "SimulationEndTime", "50"})

            'Click 'Add Part' button
            ExecuteMethod("ClickToolbarItem", New Object() {"AddPartToolStripButton"})

            'Add a root cylinder part.
            AddRootPartType("Box")

            'Run the simulation and wait for it to end.
            RunSimulationWaitToEnd()

            'Save the project
            ExecuteMethod("ClickToolbarItem", New Object() {"SaveToolStripButton"})

            'Close the project
            ExecuteMethod("Close", Nothing)

        End Sub

#Region "Additional test attributes"
        '
        ' You can use the following additional attributes as you write your tests:
        '
        '' Use TestInitialize to run code before running each test
        '<TestInitialize()> Public Sub MyTestInitialize()
        '    '
        '    ' To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        '    ' For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
        '    '
        'End Sub

        '' Use TestCleanup to run code after each test has run
        '<TestCleanup()> Public Sub MyTestCleanup()
        '    '
        '    ' To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        '    ' For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
        '    '
        'End Sub

#End Region

    End Class

End Namespace
