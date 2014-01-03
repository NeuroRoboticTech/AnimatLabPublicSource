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
                               "HingeRotationTestData", _
                               DataAccessMethod.Sequential), _
                    DeploymentItem("TestCases.accdb")>
                    Public Sub Test_HingeJointRotations()
                        If Not SetPhysicsEngine(TestContext.DataRow) Then Return

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
                        Dim bEnabled As Boolean = CBool(TestContext.DataRow("Enabled"))

                        'If test is not enabled then skip it.
                        If Not bEnabled Then Return

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Body_X", CSng(TestContext.DataRow("MaxBodyXError")))
                        aryMaxErrors.Add("Body_Y", CSng(TestContext.DataRow("MaxBodyYError")))
                        aryMaxErrors.Add("Body_Z", CSng(TestContext.DataRow("MaxBodyZError")))
                        aryMaxErrors.Add("Joint_1", CSng(TestContext.DataRow("MaxJointError")))
                        aryMaxErrors.Add("default", 0.04)

                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\JointTests\HingeTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\JointTests\HingeTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\JointTests\HingeTests\" & m_strProjectName

                        ModifyJointConstraintsInProjectFile(m_strOldProjectFolder, dblMin, dblMax, True, dblDamping, strRestitution, strStiffness)

                        TestConversionProject(strDataPrefix, aryMaxErrors)

                    End Sub

                    <TestMethod()>
                    Public Sub Test_HingeMotor()

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("Arm", 0.02)
                        aryMaxErrors.Add("JointPos", 0.02)
                        aryMaxErrors.Add("JointVel", 0.03)
                        aryMaxErrors.Add("AVm", 0.01)
                        aryMaxErrors.Add("BVm", 0.01)
                        aryMaxErrors.Add("BIa", 0.000000003)
                        aryMaxErrors.Add("default", 0.03)

                        m_strProjectName = "HingeMotorTest"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\JointTests\HingeTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\JointTests\HingeTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\JointTests\HingeTests\" & m_strProjectName

                        Test_JointMotor(aryMaxErrors)

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
