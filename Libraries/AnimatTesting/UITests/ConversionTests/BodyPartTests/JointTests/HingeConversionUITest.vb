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
                Public Class HingeConversionUITest
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
                               "HingeRotationTestData_Full", _
                               DataAccessMethod.Sequential), _
                    DeploymentItem("TestCases.accdb")>
                    Public Sub Test_HingeJointRotations()
                        m_strProjectName = TestContext.DataRow("TestName").ToString
                        Dim dblJointRotX As Double = CDbl(TestContext.DataRow("X"))
                        Dim dblJointRotY As Double = CDbl(TestContext.DataRow("Y"))
                        Dim dblJointRotZ As Double = CDbl(TestContext.DataRow("Z"))
                        Dim strOrientation As String = CStr(TestContext.DataRow("Orientation"))
                        Dim strDataPrefix As String = CStr(TestContext.DataRow("DataPrefix"))

                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\JointTests\HingeTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\JointTests\HingeTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\JointTests\HingeTests\" & m_strProjectName

                        ModifyJointRotationInProjectFile(m_strOldProjectFolder, dblJointRotX, dblJointRotY, dblJointRotZ, strOrientation)

                        TestConversionProject(strDataPrefix, 2000)

                    End Sub

                    <TestMethod(), _
                      DataSource("System.Data.OleDb", _
                                 "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestCases.accdb;Persist Security Info=False;", _
                                 "HingeLimitTestData", _
                                 DataAccessMethod.Sequential), _
                      DeploymentItem("TestCases.accdb")>
                    Public Sub Test_HingeLimits()
                        m_strProjectName = TestContext.DataRow("TestName").ToString
                        Dim dblMin As Single = CSng(TestContext.DataRow("Min"))
                        Dim dblMax As Single = CSng(TestContext.DataRow("Max"))
                        Dim dblDamping As Single = CSng(TestContext.DataRow("Damping"))
                        Dim strRestitution As Single = CSng(TestContext.DataRow("Restitution"))
                        Dim strStiffness As Single = CSng(TestContext.DataRow("Stiffness"))
                        Dim strDataPrefix As String = CStr(TestContext.DataRow("DataPrefix"))

                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\JointTests\HingeTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\JointTests\HingeTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\JointTests\HingeTests\" & m_strProjectName

                        ModifyJointConstraintsInProjectFile(m_strOldProjectFolder, dblMin, dblMax, True, dblDamping, strRestitution, strStiffness)

                        TestConversionProject(strDataPrefix, 2000)

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

                    Protected Overrides Sub SetWindowsToOpen()
                        m_aryWindowsToOpen.Add("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan")
                        m_aryWindowsToOpen.Add("Tool Viewers\JointData")
                    End Sub
#End Region

#End Region

                End Class

            End Namespace
        End Namespace
    End Namespace
End Namespace
