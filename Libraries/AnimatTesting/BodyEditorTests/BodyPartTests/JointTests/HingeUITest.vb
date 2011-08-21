Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard

Namespace BodyEditorTests
    Namespace BodyPartTests
        Namespace JointTests

            <CodedUITest()>
            Public Class HingeUITest
                Inherits JointUITest

#Region "Methods"

                <TestMethod()>
                Public Sub TestHinge()
                    TestJoint()
                End Sub

                'Protected Overrides Sub TestMovableRigidBodyProperties(ByVal strStructure As String, ByVal strPart As String)
                '    MyBase.TestMovableRigidBodyProperties(strStructure, strPart)

                'End Sub

#Region "Additional test attributes"
                '
                ' You can use the following additional attributes as you write your tests:
                '
                ' Use TestInitialize to run code before running each test
                <TestInitialize()> Public Overrides Sub MyTestInitialize()
                    MyBase.MyTestInitialize()

                    m_strProjectName = "HingeTest"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\JointTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\JointTests\" & m_strProjectName

                    CleanupProjectDirectory()
                End Sub

#End Region

#End Region

            End Class


        End Namespace
    End Namespace
End Namespace
