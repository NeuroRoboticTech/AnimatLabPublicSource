Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports AnimatTesting.Framework

Namespace UITests
    Namespace BodyEditorTests
        Namespace BodyPartTests
            Namespace RigidBodyTests

                <CodedUITest()>
                Public Class PlaneUITest
                    Inherits BodyPartUITest

#Region "Properties"

                    Protected Overrides ReadOnly Property HasRootGraphic() As Boolean
                        Get
                            Return False
                        End Get
                    End Property

#End Region

#Region "Methods"

                    <TestMethod()>
                    Public Sub TestPlane()
                        TestPart()
                    End Sub

                    Protected Overrides Sub TestMovableRigidBodyProperties(ByVal strStructure As String, ByVal strPart As String)
                        MyBase.TestMovableRigidBodyProperties(strStructure, strPart)

                        'Set the Size.X to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Size.X", "15"})

                        'Set the Size.X to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Size.X", "0"}, "The size of the plane cannot be less than or equal to zero.")

                        'Set the Size.X to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Size.X", "-0.2"}, "The size of the plane cannot be less than or equal to zero.")

                        'Set the Size.X to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Size.X", "10"})


                        'Set the Size.Y to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Size.Y", "15"})

                        'Set the Size.Y to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Size.Y", "0"}, "The size of the plane cannot be less than or equal to zero.")

                        'Set the Size.Y to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Size.Y", "-0.2"}, "The size of the plane cannot be less than or equal to zero.")

                        'Set the Size.Y to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Size.Y", "10"})


                        'Set the WidthSegments to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WidthSegments", "4"})

                        'Set the WidthSegments to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WidthSegments", "0"}, "The width segments cannot be less than or equal to zero.")

                        'Set the WidthSegments to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WidthSegments", "-2"}, "The width segments cannot be less than or equal to zero.")



                        'Set the LengthSegments to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "LengthSegments", "4"})

                        'Set the LengthSegments to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "LengthSegments", "0"}, "The length segments cannot be less than or equal to zero.")

                        'Set the LengthSegments to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "LengthSegments", "-2"}, "The length segments cannot be less than or equal to zero.")


                    End Sub

                    Protected Overrides Sub RepositionChildPart()

                        'Reposition the child part relative to the landscape part
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_1", "LocalPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_1", "LocalPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_1", "LocalPosition.Z", "0.5"})

                    End Sub

                    Protected Overrides Sub RepositionStruct1BeforeSim()
                        'Reposition the structure
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name, "WorldPosition.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name, "WorldPosition.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name, "WorldPosition.Z", "0"})
                    End Sub

                    Protected Overrides Sub RepositionStruct2BeforeSim()
                        'Move the structure up.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2", "WorldPosition.Y", "1"})

                        'Resize the sphere
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2\Body Plan\Root", "Radius", "0.05"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2\Body Plan\Root\Root_Graphics", "Radius", "0.05"})
                    End Sub

#Region "Additional test attributes"
                    '
                    ' You can use the following additional attributes as you write your tests:
                    '
                    ' Use TestInitialize to run code before running each test
                    <TestInitialize()> Public Overrides Sub MyTestInitialize()
                        MyBase.MyTestInitialize()

                        m_strPartType = "Plane"
                        m_strProjectName = "PlaneTest"
                        m_strSecondaryPartType = "Box"
                        m_strChildJoint = "Hinge"
                        m_strStruct2RootPart = "Sphere"

                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\RigidBodyTests\" & m_strProjectName

                        m_strAmbient = "#AAAAAA"
                        m_bTestDensity = False

                        m_iInitialZoomDist1 = 160
                        m_iInitialZoomDist2 = 0

                        m_ptRotatePartForTranslateStart = New Point(254, 311)
                        m_ptRotatePartForTranslateEnd = New Point(1011, 435)

                        m_ptTranslateXAxisStart = New Point(841, 476)
                        m_ptTranslateXAxisEnd = New Point(1024, 618)

                        m_ptRotateXAxisStart = New Point(633, 388)
                        m_ptRotateXAxisEnd = New Point(620, 480)

                        m_ptRotateYAxisStart = New Point(809, 345)
                        m_ptRotateYAxisEnd = New Point(808, 415)

                        m_ptRotateZAxisStart = New Point(716, 450)
                        m_ptRotateZAxisEnd = New Point(787, 446)

                        m_ptChildTranslateZAxisStart = New Point(905, 395)
                        m_ptChildTranslateZAxisEnd = New Point(645, 398)

                        m_strMoveChildLocalYAxis = "Z"
                        m_strMoveChildLocalZAxis = "Y"

                        m_dblMinTranChildLocalZ = -2
                        m_dblMaxTranChildLocalZ = -0.05

                        m_ptChildRotateYAxisStart = New Point(779, 281)
                        m_ptChildRotateYAxisEnd = New Point(693, 136)

                        m_ptChildRotateZAxisStart = New Point(775, 403)
                        m_ptChildRotateZAxisEnd = New Point(931, 413)

                        m_dblMinRotChildY = -90
                        m_dblMaxRotChildY = -10

                        m_dblManChildWorldLocalZTest = -1
                        m_dblManChildLocalWorldZTest = -2

                        m_dblStructChildRotateTestYX = 2.828
                        m_dblStructChildRotateTestYY = -0.000412
                        m_dblStructChildRotateTestYZ = -2.0

                        m_dblStructChildRotateTestZX = -0.000005119
                        m_dblStructChildRotateTestZY = 1.999
                        m_dblStructChildRotateTestZZ = -2.829


                        m_dblStructJointRotateTestYX = 2.828
                        m_dblStructJointRotateTestYY = -0.000433
                        m_dblStructJointRotateTestYZ = -2.1

                        m_dblStructJointRotateTestZX = -0.070716
                        m_dblStructJointRotateTestZY = 1.999
                        m_dblStructJointRotateTestZZ = -2.9

                        CleanupProjectDirectory()
                    End Sub

#End Region

#End Region

                End Class

            End Namespace
        End Namespace
    End Namespace
End Namespace
