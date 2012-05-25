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
                Public Class StaticConversionUITest
                    Inherits JointConversionUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"

                    <TestMethod()>
                    Public Sub Test_Static()

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Body_3X", 0.6)
                        aryMaxErrors.Add("Body_3Y", 0.6)
                        aryMaxErrors.Add("Body_3Z", 0.6)
                        aryMaxErrors.Add("Body_4X", 0.6)
                        aryMaxErrors.Add("Body_4Y", 0.6)
                        aryMaxErrors.Add("Body_4Z", 0.6)
                        aryMaxErrors.Add("default", 0.6)

                        m_strProjectName = "StaticTest"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\JointTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\JointTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\JointTests\" & m_strProjectName
 
                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        'Run the same sim a second time to check for changes between sims.
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_4\Blocker", "WorldPosition.Z", "0"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Block2_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_4\Blocker", "WorldPosition.X", "-3.3"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Block1_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3", "LocalPosition.Y", "-3.5"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Move1_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_4\Blocker", "WorldPosition.X", "-4.5"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Block1B_")

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan"}, 2000)
                        DeletePart("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3", "Delete Body Part", True)

                        PasteChildPartTypeWithJoint("Static", "Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_2", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0, True)
                        AssertErrorDialogShown("You cannot paste a rigid body using a static joint when the body has children.", enumMatchType.Equals)

                        PasteChildPartTypeWithJoint("RPRO", "Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_2", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0, True)

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_2\Joint_6", "Name", "Joint_2"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3", "LocalPosition.X", "0"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3", "LocalPosition.Y", "-3.5"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3", "LocalPosition.Z", "0"})

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\JointData"}, 2000)
                        ExecuteMethod("ClickToolbarItem", New Object() {"AddAxisToolStripButton"})
                        AddItemToChart("Structure_1\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3")
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 3\Body_3", "Name", "Body_3X"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 3\Body_3X", "DataTypeID", "WorldPositionX"})
                        AddItemToChart("Structure_1\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3")
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 3\Body_3", "Name", "Body_3Y"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 3\Body_3Y", "DataTypeID", "WorldPositionY"})
                        AddItemToChart("Structure_1\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3")
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 3\Body_3", "Name", "Body_3Z"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 3\Body_3Z", "DataTypeID", "WorldPositionZ"})
                        ExecuteMethod("ClickToolbarItem", New Object() {"AddAxisToolStripButton"})
                        AddItemToChart("Structure_1\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3\Joint_3\Body_4")
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 4\Body_4", "Name", "Body_4X"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 4\Body_4X", "DataTypeID", "WorldPositionX"})
                        AddItemToChart("Structure_1\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3\Joint_3\Body_4")
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 4\Body_4", "Name", "Body_4Y"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 4\Body_4Y", "DataTypeID", "WorldPositionY"})
                        AddItemToChart("Structure_1\Body Plan\Root\Joint_1\Body_2\Joint_2\Body_3\Joint_3\Body_4")
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 4\Body_4", "Name", "Body_4Z"})
                        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 4\Body_4Z", "DataTypeID", "WorldPositionZ"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Block1B_")

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
