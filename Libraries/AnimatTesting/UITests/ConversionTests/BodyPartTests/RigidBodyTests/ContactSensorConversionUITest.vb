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
                Public Class ContactSensorConversionUITest
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
                    Public Sub Test_ContactSensors()
                        If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("CylinderSensor", 1.1)
                        aryMaxErrors.Add("BoxSensor", 1.1)
                        aryMaxErrors.Add("BoxSensorX", 0.05)
                        aryMaxErrors.Add("BoxSensorY", 0.05)
                        aryMaxErrors.Add("CylinderSensorX", 0.05)
                        aryMaxErrors.Add("CylinderSensorY", 0.05)
                        aryMaxErrors.Add("default", 0.05)

                        m_strProjectName = "ContactSensors"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        Dim aryIgnoreRows As New ArrayList

                        aryIgnoreRows.Add(New Point(162, 173))
                        aryIgnoreRows.Add(New Point(405, 406))
                        aryIgnoreRows.Add(New Point(512, 515))
                        aryIgnoreRows.Add(New Point(2405, 2407))
                        aryIgnoreRows.Add(New Point(2491, 2494))
                        aryIgnoreRows.Add(New Point(2628, 2632))

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors, -1, aryIgnoreRows)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\BoxSensor", "WorldPosition.Y", "-55 c"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "BoxY_-55cm_", -1, aryIgnoreRows)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\CylinderSensor", "WorldPosition.Y", "-55 c"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CylinderY_-55cm_", -1, aryIgnoreRows)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\BoxSensor", "LocalPosition.Y", "0 c"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "BoxY_0cm_", -1, aryIgnoreRows)
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\BoxSensor", "WorldPosition.Y", "-55 c"})

                        aryIgnoreRows.Clear()
                        aryIgnoreRows.Add(New Point(131, 137))
                        aryIgnoreRows.Add(New Point(323, 324))
                        aryIgnoreRows.Add(New Point(399, 400))
                        aryIgnoreRows.Add(New Point(529, 530))
                        aryIgnoreRows.Add(New Point(2390, 2391))
                        aryIgnoreRows.Add(New Point(2496, 2497))
                        aryIgnoreRows.Add(New Point(2548, 2549))
                        aryIgnoreRows.Add(New Point(2641, 2641))

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum", "Rotation.X", "-45"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum", "WorldPosition.Y", "-10 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum", "WorldPosition.Z", "-20 c"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "PendulumRotate_", -1, aryIgnoreRows)

                        'Add subsystem.
                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem"}, 2000)
                        AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem", _
                                          "AnimatGUI.DataObjects.Behavior.Nodes.Subsystem", New Point(316, 30), "S2")

                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\BoxSensor", False})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\BoxContact", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\CylinderSensor", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\CylinderContact", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\BS_BC", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\CS_CC", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\BS_BC\BoxSensor", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\CS_CC\CylinderSensor", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\BoxContact\BS_BC", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\CylinderContact\CS_CC", True})
                        DeleteSelectedParts("Delete Group", True)
                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2"}, 2000)
                        ClickMenuItem("PasteInPlaceToolStripMenuItem", True)


                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\BodyData"}, 2000)
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 3", False})
                        AddItemToChart("Simulation\Organism_1\Behavioral System\Neural Subsystem\Nodes\S2\Nodes\BoxContact")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 3\BoxContact", "DataTypeID", "MembraneVoltage"})
                        AddItemToChart("Simulation\Organism_1\Behavioral System\Neural Subsystem\Nodes\S2\Nodes\CylinderContact")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 3\CylinderContact", "DataTypeID", "MembraneVoltage"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "PendulumRotate_", -1, aryIgnoreRows)


                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan"}, 2000)
                        DeletePart("Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\BoxSensor", "Delete Body Part", True)
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\BoxSensor"})) Then
                            Throw New System.Exception("Sensor was not deleted")
                        End If
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 1\BoxSensor"})) Then
                            Throw New System.Exception("BoxSensor chart node was not removed correctly.")
                        End If
                        If Not GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\BoxSensor", "LinkedPart.BodyPart") Is Nothing Then
                            Throw New System.Exception("Linked body part not removed.")
                        End If
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\BS_BC"})) Then
                            Throw New System.Exception("BS_BC adapter node was not removed correctly.")
                        End If

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum", "Rotation.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum", "WorldPosition.Y", "-20 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum", "WorldPosition.Z", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\CylinderSensor", "WorldPosition.Y", "-35 c"})

                        aryIgnoreRows.Clear()
                        aryIgnoreRows.Add(New Point(162, 173))
                        aryIgnoreRows.Add(New Point(405, 406))
                        aryIgnoreRows.Add(New Point(512, 515))
                        aryIgnoreRows.Add(New Point(2405, 2407))
                        aryIgnoreRows.Add(New Point(2491, 2494))
                        aryIgnoreRows.Add(New Point(2628, 2632))

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeleteBoxSensor_", -1, aryIgnoreRows)

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan"}, 2000)
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\CylinderSensor", "WorldPosition.X", "5 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\CylinderSensor", "WorldPosition.Y", "-35 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\CylinderSensor", "WorldPosition.Z", "0"})

                        PasteChildPartTypeWithJoint("", "Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0, False)
                        Threading.Thread.Sleep(1000)

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\BoxSensor", "WorldPosition.X", "-5 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\BoxSensor", "WorldPosition.Y", "-35 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\BoxSensor", "WorldPosition.Z", "0"})

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\BodyData"}, 2000)
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 1", False})
                        AddItemToChart("Simulation\Organism_1\Body Plan\Root\Hinge\Pendulum\BoxSensor")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\BodyData\LineChart\Y Axis 1\BoxSensor", "DataTypeID", "ContactCount"})

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2"}, 2000)
                        ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\BoxSensor", _
                                      "Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\BoxSensor"})

                        AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\BoxSensor", _
                                          "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\BoxContact", "", "", False)
                        If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\2"})) Then
                            Throw New System.Exception("2 adapter was not added")
                        End If
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\2", "Name", "BS_BC"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\BS_BC", "Gain.C", "10 n"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\BS_BC", "DataTypes", "AnimatGUI.DataObjects.Physical.Bodies.Box.DataTypes.ContactCount"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_", -1, aryIgnoreRows)

                        'Click 'Add Part' button
                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan"}, 2000)
                        ClickToolbarItem("AddPartToolStripButton", True)
                        AutomatedClickToAddBody("Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Hinge\Pendulum\BoxSensor", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0)
                        AssertErrorDialogShown("You cannot add children to a contact sensor class.", enumErrorTextType.Contains)

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
