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
            Public Class TerrainUITest
                Inherits BodyPartUITest

#Region "Properties"

                Protected Overrides ReadOnly Property HasRootGraphic() As Boolean
                    Get
                        Return False
                    End Get
                End Property

                Protected Overrides ReadOnly Property AllowRootRotations() As Boolean
                    Get
                        Return False
                    End Get
                End Property

#End Region

#Region "Methods"

                <TestMethod()>
                Public Sub TestTerrain()
                    TestPart()
                End Sub

                Protected Overrides Sub TestMovableRigidBodyProperties(ByVal strStructure As String, ByVal strPart As String)
                    MyBase.TestMovableRigidBodyProperties(strStructure, strPart)

                    ''Set the Size.X to a valid value.
                    'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.X", "15"})

                    ''Set the Size.X to zero
                    'ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.X", "0"}, "The size of the plane cannot be less than or equal to zero.")

                    ''Set the Size.X to a negative value
                    'ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.X", "-0.2"}, "The size of the plane cannot be less than or equal to zero.")

                    ''Set the Size.X to a valid value.
                    'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.X", "10"})


                    ''Set the Size.Y to a valid value.
                    'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.Y", "15"})

                    ''Set the Size.Y to zero
                    'ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.Y", "0"}, "The size of the plane cannot be less than or equal to zero.")

                    ''Set the Size.Y to a negative value
                    'ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.Y", "-0.2"}, "The size of the plane cannot be less than or equal to zero.")

                    ''Set the Size.Y to a valid value.
                    'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.Y", "10"})


                    ''Set the WidthSegments to a valid value.
                    'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WidthSegments", "4"})

                    ''Set the WidthSegments to zero
                    'ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WidthSegments", "0"}, "The width segments cannot be less than or equal to zero.")

                    ''Set the WidthSegments to a negative value
                    'ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WidthSegments", "-2"}, "The width segments cannot be less than or equal to zero.")



                    ''Set the LengthSegments to a valid value.
                    'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LengthSegments", "4"})

                    ''Set the LengthSegments to zero
                    'ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LengthSegments", "0"}, "The length segments cannot be less than or equal to zero.")

                    ''Set the LengthSegments to a negative value
                    'ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LengthSegments", "-2"}, "The length segments cannot be less than or equal to zero.")

                    'For the terrain part we need to do an additional rotation so we can see the axis.
                    DragMouse(m_ptTerrainAxisViewStart, m_ptTerrainAxisViewEnd, MouseButtons.Left, ModifierKeys.None, True)

                End Sub

                Protected Overrides Sub ProcessExtraAddRootMethods(ByVal strPartType As String)

                    If strPartType = m_strPartType Then
                        'Wait for the collision mesh dialog to show, fill it in and hit ok
                        OpenDialogAndWait("SelectTerrain", Nothing, Nothing)
                        ExecuteActiveDialogMethod("SetTerrainParameters", New Object() {(m_strRootFolder & "\bin\Resources\TerrainTest_HeightMap.jpg"), (m_strRootFolder & "\bin\Resources\TerrainTest_TextureMap.jpg"), 0.1, 0.1, 0.5})
                        ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    End If

                End Sub

                Protected Overrides Sub RepositionChildPart()

                    'Reposition the child part relative to the landscape part
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_1", "LocalPosition.X", "0"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_1", "LocalPosition.Y", "0"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_1", "LocalPosition.Z", "0.5"})

                End Sub

                Protected Overrides Sub RepositionStruct1BeforeSim()
                    'Reposition the structure
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1", "WorldPosition.X", "0"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1", "WorldPosition.Y", "0"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1", "WorldPosition.Z", "0"})
                End Sub

                Protected Overrides Sub RepositionStruct2BeforeSim()
                    'Move the structure up.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2", "WorldPosition.Y", "1"})

                    'Resize the sphere
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2\Body Plan\Root", "Radius", "0.05"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2\Body Plan\Root\Root_Graphics", "Radius", "0.05"})
                End Sub

#Region "Additional test attributes"
                '
                ' You can use the following additional attributes as you write your tests:
                '
                ' Use TestInitialize to run code before running each test
                <TestInitialize()> Public Overrides Sub MyTestInitialize()
                    MyBase.MyTestInitialize()

                    m_strPartType = "Terrain"
                    m_strProjectName = "TerrainTest"
                    m_strSecondaryPartType = "Box"
                    m_strChildJoint = "Hinge"
                    m_strStruct2RootPart = "Sphere"

                    m_bTestTexture = False
                    m_bTestDensity = False

                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\RigidBodyTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\RigidBodyTests\" & m_strProjectName

                    m_strAmbient = "#AAAAAA"

                    m_iInitialZoomDist1 = 150
                    m_iInitialZoomDist2 = 0

                    m_ptTranslateXAxisStart = New Point(866, 396)
                    m_ptTranslateXAxisEnd = New Point(1065, 399)

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