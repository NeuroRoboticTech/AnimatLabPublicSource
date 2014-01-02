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
                Public Class OdorMouthEatingConversionUITest
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
                               "PhysicsEngines", _
                               DataAccessMethod.Sequential), _
                    DeploymentItem("TestCases.accdb")>
                    Public Sub Test_OdorMouthEating()
                        If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("OdorSensor", 50)
                        aryMaxErrors.Add("Mouth", 0.001)
                        aryMaxErrors.Add("Eat", 0.05)
                        aryMaxErrors.Add("Food_Near", 0.05)
                        aryMaxErrors.Add("Energy", 110)
                        aryMaxErrors.Add("default", 0.05)

                        m_strProjectName = "OdorMouthAndEating"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Odor_ad", "Enabled", "False"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "OdorAd_Off_")

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "OdorAd_Off_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Odor_ad", "Enabled", "True"})

                        ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge2\OdorEmitter", "OdorSources"}, 500)
                        ExecuteActiveDialogMethod("Automation_AddOdorType", New Object() {"TestOdor2"})
                        ExecuteActiveDialogMethod("Automation_SelectOdorSource", New Object() {"TestOdor"})
                        ExecuteActiveDialogMethod("Automation_SelectOdorType", New Object() {"TestOdor"})
                        ExecuteActiveDialogMethod("Automat_SetSelectedItemProperty", New Object() {"DiffusionConstant", "0.75"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Odor_ad", "Gain.XOffset", "5000"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DiffusionRate_0_75_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge2\OdorEmitter", "WorldPosition.Y", "-31 c"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Emitter_-31cm_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Mouth", "MinimumFoodRadius", "6.1 c"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FoodRadius_6cm_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Stomach", "BaseConsumptionRate", "2000 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "BaseCons_2000_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Stomach", "EnergyLevel", "20 k"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "EnergyLevel_20k_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Stomach", "BaseConsumptionRate", "1 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Stomach", "MaxEnergyLevel", "24 k"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MaxEnergy_24k_")

                        aryMaxErrors("OdorSensor") = 1500
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Stomach", "BaseConsumptionRate", "5000 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Stomach", "EnergyLevel", "5 k"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Kill_")

                        'Make sure it gets reset after a kill correctly.
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Kill_")

                        aryMaxErrors("OdorSensor") = 50

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Stomach", "BaseConsumptionRate", "500 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Stomach", "EnergyLevel", "10 k"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge2\OdorEmitter", "FoodQuantity", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge2\OdorEmitter", "FoodReplenishRate", "0"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "NoFood_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge2\OdorEmitter", "FoodQuantity", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge2\OdorEmitter", "FoodReplenishRate", "1000"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FoodReplinish_1k_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge2\OdorEmitter", "FoodSource", "False"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Food_Disabled_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge2\OdorEmitter", "FoodSource", "True"})

                        aryMaxErrors("OdorSensor") = 400
                        aryMaxErrors("Eat") = 0.25
                        aryMaxErrors("Food_Near") = 0.25

                        ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge2\OdorEmitter", "OdorSources"}, 500)
                        ExecuteActiveDialogMethod("Automation_SelectOdorSource", New Object() {"TestOdor"})
                        ExecuteActiveDialogMethod("Automat_SetSelectedItemProperty", New Object() {"UseFoodQuantity", "True"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge2\OdorEmitter", "FoodQuantity", "100"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge2\OdorEmitter", "FoodReplenishRate", "0"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "UseFoodQtyForOdor_")

                        'Test cut/copy/paste.
                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan"}, 2000)

                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\OdorSensor", False})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Stomach", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Mouth", True})
                        ClickMenuItem("CopyToolStripMenuItem", True, , True, True)
                        AssertErrorDialogShown("Only one body part can be selected to be copied at one time.", enumErrorTextType.Contains)

                        DeletePart("Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\OdorSensor", "Delete Body Part", True)
                        PasteChildPartTypeWithJoint("", "Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0, False)

                        DeletePart("Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Stomach", "Delete Body Part", True)
                        PasteChildPartTypeWithJoint("", "Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0, False)

                        DeletePart("Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Mouth", "Delete Body Part", True)
                        PasteChildPartTypeWithJoint("", "Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0, False)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\OdorSensor", "LocalPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\OdorSensor", "LocalPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\OdorSensor", "LocalPosition.Z", "0"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Stomach", "LocalPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Stomach", "LocalPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Stomach", "LocalPosition.Z", "0"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Mouth", "LocalPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Mouth", "LocalPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Mouth", "LocalPosition.Z", "0"})

                        ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Tool Viewers\BodyData"}, 1000)
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 1", False})
                        AddItemToChart("Simulation\Organism_1\Body Plan\Root\Hinge1\Head\OdorSensor")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 1\OdorSensor", "DataTypeID", "OdorValue"})

                        ClickToolbarItem("AddAxisToolStripButton", True)
                        AddItemToChart("Simulation\Organism_1\Body Plan\Root\Hinge1\Head\Mouth")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 5\Mouth", "DataTypeID", "EatingRate"})

                        ClickToolbarItem("AddAxisToolStripButton", True)
                        AddItemToChart("Simulation\Organism_1\Body Plan\Root\Hinge1\Head\Stomach")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 6\Stomach", "DataTypeID", "EnergyLevel"})

                        ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Mouth", _
                                      "Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\Mouth"})
                        ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Odor", _
                                      "Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge1\Head\OdorSensor"})

                        RunSimulationWaitToEnd()

                    End Sub

                    Protected Sub CreateChart()
                        Debug.WriteLine("CreateChart")

                        'Select the LineChart to add.
                        AddChart("Line Chart")

                        'Select the Chart axis
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1", False})

                        'Change the end time of the data chart to 45 seconds.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart", "CollectEndTime", "2"})

                        AddItemToChart("Simulation")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Arm", "Name", "TotalStepTimeSmoothed"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\TotalStepTimeSmoothed", "DataTypeID", "TotalRealTimeForStepSmoothed"})

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
