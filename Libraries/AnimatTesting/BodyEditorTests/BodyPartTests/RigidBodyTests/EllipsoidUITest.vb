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
            Public Class EllipsoidUITest
                Inherits BodyPartUITest

#Region "Methods"

                <TestMethod()>
                Public Sub TestEllipsoid()
                    TestPart()
                End Sub

                Protected Overrides Sub TestMovableRigidBodyProperties(ByVal strStructure As String, ByVal strPart As String)
                    MyBase.TestMovableRigidBodyProperties(strStructure, strPart)

                    'Set the major radius to a valid value.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "MajorAxisRadius", "0.04"})

                    'Set the major radius to zero
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "MajorAxisRadius", "0"}, "The major axis radius of the ellipsoid cannot be less than or equal to zero.")

                    'Set the major radius to a negative value
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "MajorAxisRadius", "-0.2"}, "The major axis radius of the ellipsoid cannot be less than or equal to zero.")

                    'Set the major radius to a valid value.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "MajorAxisRadius", "0.03"})


                    'Set the minor radius to a valid value.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "MinorAxisRadius", "0.02"})

                    'Set the minor radius to zero
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "MinorAxisRadius", "0"}, "The minor axis radius of the ellipsoid cannot be less than or equal to zero.")

                    'Set the minor radius to a negative value
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "MinorAxisRadius", "-0.2"}, "The minor axis radius of the ellipsoid cannot be less than or equal to zero.")

                    'Set the minor radius to a valid value.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "MinorAxisRadius", "0.01"})


                    'Set the LatitudeSegments to a valid value.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LatitudeSegments", "30"})

                    'Set the LatitudeSegments to zero
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LatitudeSegments", "5"}, "The number of latitude segments for the ellipsoid cannot be less than ten.")

                    'Set the LatitudeSegments to a negative value
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LatitudeSegments", "-2"}, "The number of latitude segments for the ellipsoid cannot be less than ten.")

                    'Set the LatitudeSegments to original value
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LatitudeSegments", "20"})


                    'Set the LongtitudeSegments to a valid value.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LongtitudeSegments", "30"})

                    'Set the LongtitudeSegments to zero
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LongtitudeSegments", "5"}, "The number of longtitude segments for the ellipsoid cannot be less than ten.")

                    'Set the LongtitudeSegments to a negative value
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LongtitudeSegments", "-2"}, "The number of longtitude segments for the ellipsoid cannot be less than ten.")

                    'Set the LongtitudeSegments to original value
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LongtitudeSegments", "20"})

                End Sub

#Region "Additional test attributes"
                '
                ' You can use the following additional attributes as you write your tests:
                '
                ' Use TestInitialize to run code before running each test
                <TestInitialize()> Public Overrides Sub MyTestInitialize()
                    MyBase.MyTestInitialize()

                    m_strPartType = "Ellipsoid"
                    m_strProjectName = "EllipsoidTest"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\RigidBodyTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\RigidBodyTests\" & m_strProjectName

                    m_iInitialZoomDist1 = 340
                    m_iInitialZoomDist2 = 340

                    m_iSecondaryZoomDist1 = 340
                    m_iSecondaryZoomDist2 = 340

                    'm_iSecondaryZoomDist1 = 340
                    'm_iInitialZoomDist2 = 340

                    m_ptClickToAddChild = New Point(735, 387)

                    m_ptChildTranslateYAxisStart = New Point(712, 207)
                    m_ptChildTranslateYAxisEnd = New Point(713, 25)

                    CleanupProjectDirectory()
                End Sub

#End Region

#End Region

            End Class

        End Namespace
    End Namespace
End Namespace