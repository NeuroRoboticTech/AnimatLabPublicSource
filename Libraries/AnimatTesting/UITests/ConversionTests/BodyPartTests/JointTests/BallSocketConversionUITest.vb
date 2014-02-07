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
            Namespace JointTests

                <CodedUITest()>
                Public Class BallSocketConversionUITest
                    Inherits JointConversionUITest

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
                    Public Sub Test_BallSocketConversion()
                        If Not SetPhysicsEngine(TestContext.DataRow) Then Return

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("BodyX", 0.03)
                        aryMaxErrors.Add("BodyY", 0.03)
                        aryMaxErrors.Add("BodyZ", 0.03)
                        aryMaxErrors.Add("default", 0.03)

                        m_strProjectName = "BallSocketTest"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\JointTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\JointTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\JointTests\" & m_strProjectName

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        'Run the same sim a second time to check for changes between sims.
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ForceStim1", "ForceZ", "0 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ForceStim1", "ForceY", "1 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Y_1N_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ForceStim1", "ForceY", "0 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ForceStim1", "ForceX", "1 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "X_1N_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ForceStim1", "ForceX", "1 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ForceStim1", "ForceY", "0 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ForceStim1", "ForceZ", "1 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "XZ_1N_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ForceStim1", "PositionX", "1 c"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "XZ_1_N_X_1cm_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ForceStim1", "PositionX", "0 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1", "LocalPosition.Y", "-18 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\ForceStim1", "ForceZ", "0 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Joint_-18cmY_X_1N_")

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan"}, 2000)
                        DeletePart("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_2", "Delete Body Part", True)
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1"})) Then
                            Throw New System.Exception("Joint was not deleted")
                        End If
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Stimuli\ForceStim1"})) Then
                            Throw New System.Exception("ForceStim1 was not deleted")
                        End If
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 1\BodyX"})) Then
                            Throw New System.Exception("BodyX chart node was not removed correctly.")
                        End If

                        PasteChildPartTypeWithJoint("BallSocket", "Simulation\Environment\Structures\Structure_1\Body Plan\Root", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0, True)
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Body_2", "LocalPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Body_2", "LocalPosition.Y", "-25 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Body_2", "LocalPosition.Z", "0"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2", "LocalPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2", "LocalPosition.Y", "18 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2", "LocalPosition.Z", "0"})

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\JointData"}, 2000)
                        ClickToolbarItem("AddAxisToolStripButton", True)
                        AddItemToChart("Simulation\" & "Structure_1\Body Plan\Root\Joint_2\Body_2")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 2\Body_2", "Name", "BodyX"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 2\BodyX", "DataTypeID", "WorldPositionX"})
                        AddItemToChart("Simulation\" & "Structure_1\Body Plan\Root\Joint_2\Body_2")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 2\Body_2", "Name", "BodyY"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 2\BodyY", "DataTypeID", "WorldPositionY"})
                        AddItemToChart("Simulation\" & "Structure_1\Body Plan\Root\Joint_2\Body_2")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 2\Body_2", "Name", "BodyZ"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 2\BodyZ", "DataTypeID", "WorldPositionZ"})

                        AddStimulus("Force", "Structure_1", "\Body Plan\Root\Joint_2\Body_2", "ArmForce") ', "Stimulus_3"
                        SetForceStimulus("ArmForce", False, True, 0, 0.5, 0, 0, 0, 0, 0, 1, 0, 0, 0)

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

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
