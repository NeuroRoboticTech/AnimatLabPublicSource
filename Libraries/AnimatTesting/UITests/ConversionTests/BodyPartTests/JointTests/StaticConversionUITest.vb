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
                        aryMaxErrors.Add("RootX", 0.08)
                        aryMaxErrors.Add("RootY", 0.08)
                        aryMaxErrors.Add("RootZ", 0.08)
                        aryMaxErrors.Add("BodyX", 0.08)
                        aryMaxErrors.Add("BodyY", 0.08)
                        aryMaxErrors.Add("BodyZ", 0.08)
                        aryMaxErrors.Add("default", 0.08)

                        m_strProjectName = "StaticTest"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\JointTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\JointTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\JointTests\" & m_strProjectName
 
                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        'Run the same sim a second time to check for changes between sims.
                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                        'ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan"}, 2000)
                        'DeletePart("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Arm", "Delete Body Part", True)
                        ''RunSimulationWaitToEnd()
                        ''CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeleteStatic_")

                        'PasteChildPartTypeWithJoint("Static", "Simulation\Environment\Structures\Structure_1\Body Plan\Root", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0, True)
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Arm", "LocalPosition.X", "0"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Arm", "LocalPosition.Y", "-30 c"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Arm", "LocalPosition.Z", "-30 c"})

                        'ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\JointData"}, 2000)
                        'ExecuteMethod("ClickToolbarItem", New Object() {"AddAxisToolStripButton"})
                        'AddItemToChart("Structure_1\Body Plan\Root\Joint_2\Arm")
                        'ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 4\Arm", "Name", "BodyX"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 4\BodyX", "DataTypeID", "WorldPositionX"})
                        'AddItemToChart("Structure_1\Body Plan\Root\Joint_2\Arm")
                        'ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 4\Arm", "Name", "BodyY"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 4\BodyY", "DataTypeID", "WorldPositionY"})
                        'AddItemToChart("Structure_1\Body Plan\Root\Joint_2\Arm")
                        'ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 4\Arm", "Name", "BodyZ"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\JointData\LineChart\Y Axis 4\BodyZ", "DataTypeID", "WorldPositionZ"})

                        ''RunSimulationWaitToEnd()
                        ''CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                        'AddChildPartTypeWithJoint("Box", "Static", "Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Arm")
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Arm\Joint_3\Body_3", "Name", "Arm2"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Arm\Joint_3\Arm2", "LocalPosition.X", "-30 c"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Arm\Joint_3\Arm2", "LocalPosition.Y", "-30 c"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_2\Arm\Joint_3\Arm2", "LocalPosition.Z", "-30 c"})
                        'RunSimulationWaitToEnd()

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
