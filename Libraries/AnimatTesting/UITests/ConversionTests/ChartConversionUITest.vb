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

        <CodedUITest()>
        Public Class ChartConversionUITest
            Inherits ConversionUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"

            <TestMethod(), _
             DataSource("System.Data.OleDb", _
                        "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                        "PhysicsEngines", _
                        DataAccessMethod.Sequential), _
             DeploymentItem("TestCases.accdb")>
            Public Sub Test_DeleteNodesBeforeOpenChart()
                m_strPhysicsEngine = TestContext.DataRow("Physics").ToString
                Dim bEnabled As Boolean = CBool(TestContext.DataRow("Enabled"))
                If Not bEnabled Then Return

                Dim aryMaxErrors As New Hashtable
                aryMaxErrors.Add("Time", 0.001)
                aryMaxErrors.Add("A1", 0.001)
                aryMaxErrors.Add("A2", 0.001)
                aryMaxErrors.Add("A3", 0.001)
                aryMaxErrors.Add("A4", 0.001)
                aryMaxErrors.Add("A5", 0.001)
                aryMaxErrors.Add("A6", 0.001)
                aryMaxErrors.Add("A1Ia", 0.000000001)
                aryMaxErrors.Add("A2Ia", 0.000000001)
                aryMaxErrors.Add("A3Ia", 0.000000001)
                aryMaxErrors.Add("A4Ia", 0.000000001)
                aryMaxErrors.Add("A5Ia", 0.000000001)
                aryMaxErrors.Add("A6Ia", 0.000000001)
                aryMaxErrors.Add("default", 0.001)

                m_strProjectName = "ReceptiveFields"
                m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                m_strStructureGroup = "Organisms"
                m_strStruct1Name = "Organism_1"

                m_aryWindowsToOpen.Clear()

                ConvertProject()

                'Select several neurons to delete
                ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Skin\Behavioral System\Neural Subsystem\A1", False})
                ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Skin\Behavioral System\Neural Subsystem\A2", True})
                ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Skin\Behavioral System\Neural Subsystem\A3", True})
                ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Skin\Behavioral System\Neural Subsystem\A5", True})
                ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Skin\Behavioral System\Neural Subsystem\A6", True})
                DeleteSelectedParts("Delete Group", False)

                'Open the Structure_1 body plan editor window
                ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\BodyData"}, 2000)

                RunSimulationWaitToEnd()
                CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeleteNeuronsBeforeChartOpen_")

            End Sub


#Region "Additional test attributes"
            '
            ' You can use the following additional attributes as you write your tests:
            '
            ' Use TestInitialize to run code before running each test
            <TestInitialize()> Public Overrides Sub MyTestInitialize()
                MyBase.MyTestInitialize()

                'This conversion is different than others. The contact collisions are generated differently than before, so I cannot
                'use the old version data as a template. I will check the output to make sure it is similar, but use the new version as the template.
                'm_bGenerateTempates = False

            End Sub

            <TestCleanup()> Public Overrides Sub MyTestCleanup()
                MyBase.MyTestCleanup()
            End Sub
#End Region

#End Region

        End Class

    End Namespace
End Namespace
