Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard

Namespace BodyEditorTests
    Namespace BodyPartTests
        Namespace RigidBodyTests


            <CodedUITest()>
            Public Class MeshUITest
                Inherits BodyPartUITest

#Region "Methods"

                <TestMethod()>
                Public Sub TestMesh()
                    TestPart()
                End Sub

                Protected Overrides Sub ProcessExtraAddRootMethods(ByVal strPartType As String)

                    If strPartType = m_strPartType Then
                        'Wait for the collision mesh dialog to show, fill it in and hit ok
                        OpenDialogAndWait("SelectMesh", Nothing, Nothing)
                        ExecuteActiveDialogMethod("SetMeshParameters", New Object() {(m_strRootFolder & "\bin\Resources\" & m_strMeshFile), "Convex"})
                        ExecuteActiveDialogMethod("ClickOkButton", Nothing)

                        'Wait for the graphics mesh to show and hit ok.
                        OpenDialogAndWait("SelectMesh", Nothing, Nothing)
                        ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    End If

                End Sub

                Protected Overrides Sub ProcessExtraChildJointMethods(ByVal strPartType As String, ByVal strJointType As String)

                    'Wait for the collision mesh dialog to show, fill it in and hit ok
                    OpenDialogAndWait("SelectMesh", Nothing, Nothing)
                    ExecuteActiveDialogMethod("SetMeshParameters", New Object() {(m_strRootFolder & "\bin\Resources\" & m_strMeshFile), "Triangular"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)

                    'Wait for the graphics mesh to show and hit ok.
                    OpenDialogAndWait("SelectMesh", Nothing, Nothing)
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                End Sub


#Region "Additional test attributes"
                '
                ' You can use the following additional attributes as you write your tests:
                '
                ' Use TestInitialize to run code before running each test
                <TestInitialize()> Public Overrides Sub MyTestInitialize()
                    MyBase.MyTestInitialize()

                    m_strPartType = "Mesh"
                    m_strProjectName = "MeshTest"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\RigidBodyTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                    m_bTestTexture = False

                    m_iInitialZoomDist2 = 150
                    m_iInitialZoomDist2 = 150

                    m_iSecondaryZoomDist2 = 150
                    m_iSecondaryZoomDist2 = 150

                    CleanupProjectDirectory()
                End Sub

#End Region

#End Region

            End Class

        End Namespace
    End Namespace
End Namespace