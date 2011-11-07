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
                Public Class FluidPlaneUITest
                    Inherits BodyPartUITest

#Region "Attributes"

                    Protected m_ptMovePlaneStart As Point = New Point(756, 230)
                    Protected m_ptMovePlaneEnd As Point = New Point(717, 452)
                    Protected m_aryForceStimPosX As New ArrayList
                    Protected m_aryForceStimPosY As New ArrayList
                    Protected m_aryForceStimPosZ As New ArrayList

                    Protected m_aryForceStimForceX As New ArrayList
                    Protected m_aryForceStimForceY As New ArrayList
                    Protected m_aryForceStimForceZ As New ArrayList

                    Protected m_aryForceStimTorqueX As New ArrayList
                    Protected m_aryForceStimTorqueY As New ArrayList
                    Protected m_aryForceStimTorqueZ As New ArrayList

#End Region

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

                    Public Overrides ReadOnly Property HasChildPart() As Boolean
                        Get
                            Return False
                        End Get
                    End Property
#End Region

#Region "Methods"

                    <TestMethod()>
                    Public Sub TestFluidPlane()

                        StartProject()
                        CreateAndTestRoot()
                        CreateChartAndAddBodies()

                        CreateStruct2AndRoot()

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, "AboveSurfaceHeavier_")

                        'Change the density of the body to be lighter than the water.
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2\Body Plan\Root", "Density", "0.8"})

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, "AboveSurfaceLighter_")

                        'move the part below the water surface.
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2", "WorldPosition.Y", "-1.4"})

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, "BelowSurfaceLighter1_")

                        MouseMoveFluidUp()

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, "BelowSurfaceLighter2_")

                        'Change the density of the body to be Heavier than the water.
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2\Body Plan\Root", "Density", "1.1"})

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, "BelowSurfaceHeavier_")
                        'Change the density of the body to be Heavier than the water.
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2\Body Plan\Root", "Density", "0.9"})

                        'Run the simulation and wait for it to end.
                        RunSimulationWaitToEnd()

                        'Compare chart data to verify simulation results.
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, "BelowSurfaceSame_")

                        AddStimulus("Force", "Structure_2", "Root", "ForceStim")

                        For iIdx As Integer = 0 To m_aryForceStimForceX.Count - 1
                            SetForceStimulus("ForceStim", False, True, 1, 1.5, _
                                             CDbl(m_aryForceStimPosX(iIdx)), CDbl(m_aryForceStimPosY(iIdx)), CDbl(m_aryForceStimPosZ(iIdx)), _
                                             CDbl(m_aryForceStimForceX(iIdx)), CDbl(m_aryForceStimForceY(iIdx)), CDbl(m_aryForceStimForceZ(iIdx)), _
                                             CDbl(m_aryForceStimTorqueX(iIdx)), CDbl(m_aryForceStimTorqueY(iIdx)), CDbl(m_aryForceStimTorqueZ(iIdx)))

                            'Run the simulation and wait for it to end.
                            RunSimulationWaitToEnd()

                            'Compare chart data to verify simulation results.
                            CompareSimulation(m_strRootFolder & m_strTestDataPath, "ForceStim" & iIdx & "_")
                        Next


                    End Sub

                    Protected Overrides Sub RecalculatePositionsUsingResolution()
                        MyBase.RecalculatePositionsUsingResolution()

                        m_ptMovePlaneStart.X = CInt(m_ptMovePlaneStart.X * m_dblResScaleWidth)
                        m_ptMovePlaneStart.Y = CInt(m_ptMovePlaneStart.Y * m_dblResScaleHeight)

                        m_ptMovePlaneEnd.X = CInt(m_ptMovePlaneEnd.X * m_dblResScaleWidth)
                        m_ptMovePlaneEnd.Y = CInt(m_ptMovePlaneEnd.Y * m_dblResScaleHeight)

                    End Sub


                    Protected Overridable Sub MouseMoveFluidUp()

                        'Select the simulation window tab so it is visible now.
                        ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Simulation\Environment\Structures\Structure_1"}, 1000)

                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root"})

                        'Set it to track the root of structure 2.
                        ExecuteMethod("SelectTrackItems", New Object() {"Simulation\Environment\Structures\Structure_1", "Structure_1", "Root"})

                        'Zoom in on the part so we can try and move it with the mouse.
                        ZoomInOnPart(m_ptInitialZoomStart, m_iInitialZoomDist1, m_iInitialZoomDist2)

                        'Move the z axis and verify position.
                        MovePartAxis("Structure_1", "Root", _
                                     m_strMoveRootWorldYAxis, m_strMoveRootLocalYAxis, _
                                     m_ptMovePlaneStart, m_ptMovePlaneEnd, _
                                     -2, 0, -2, 0, 0, 0)

                        'Select the simulation window tab so it is visible now.
                        ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Simulation\Environment\Structures\Structure_2"}, 1000)

                        'Set it to track the root of structure 2.
                        ExecuteMethod("SelectTrackItems", New Object() {"Simulation\Environment\Structures\Structure_2", "Structure_2", "Root"})

                    End Sub


                    Protected Overrides Sub TestMovableRigidBodyProperties(ByVal strStructure As String, ByVal strPart As String)
                        MyBase.TestMovableRigidBodyProperties(strStructure, strPart)

                        'Set the Size.X to a valid value.
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.X", "15"})

                        'Set the Size.X to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.X", "0"}, "The size of the plane cannot be less than or equal to zero.")

                        'Set the Size.X to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.X", "-0.2"}, "The size of the plane cannot be less than or equal to zero.")

                        'Set the Size.X to a valid value.
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.X", "10"})


                        'Set the Size.Y to a valid value.
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.Y", "15"})

                        'Set the Size.Y to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.Y", "0"}, "The size of the plane cannot be less than or equal to zero.")

                        'Set the Size.Y to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.Y", "-0.2"}, "The size of the plane cannot be less than or equal to zero.")

                        'Set the Size.Y to a valid value.
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Size.Y", "10"})


                        'Set the WidthSegments to a valid value.
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WidthSegments", "4"})

                        'Set the WidthSegments to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WidthSegments", "0"}, "The width segments cannot be less than or equal to zero.")

                        'Set the WidthSegments to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WidthSegments", "-2"}, "The width segments cannot be less than or equal to zero.")



                        'Set the LengthSegments to a valid value.
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LengthSegments", "4"})

                        'Set the LengthSegments to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LengthSegments", "0"}, "The length segments cannot be less than or equal to zero.")

                        'Set the LengthSegments to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LengthSegments", "-2"}, "The length segments cannot be less than or equal to zero.")


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

                        ''Resize the sphere
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2\Body Plan\Root", "Radius", "0.05"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_2\Body Plan\Root\Root_Graphics", "Radius", "0.05"})
                    End Sub

#Region "Additional test attributes"
                    '
                    ' You can use the following additional attributes as you write your tests:
                    '
                    ' Use TestInitialize to run code before running each test
                    <TestInitialize()> Public Overrides Sub MyTestInitialize()
                        MyBase.MyTestInitialize()

                        m_strPartType = "FluidPlane"
                        m_strProjectName = "FluidPlaneTest"
                        m_strStruct2RootPart = "Box"

                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\RigidBodyTests\" & m_strProjectName

                        m_strAmbient = "#AAAAAA"
                        m_bTestDensity = True
                        m_dblTestDensity = 0.9
                        m_strTextureFile = "Waves.jpg"

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

                        'Pos(0, 0, 0), Force (0,1,0), Torque(0,0,0)
                        m_aryForceStimPosX.Add(0)
                        m_aryForceStimPosY.Add(0)
                        m_aryForceStimPosZ.Add(0)
                        m_aryForceStimForceX.Add(0)
                        m_aryForceStimForceY.Add(1)
                        m_aryForceStimForceZ.Add(0)
                        m_aryForceStimTorqueX.Add(0)
                        m_aryForceStimTorqueY.Add(0)
                        m_aryForceStimTorqueZ.Add(0)

                        'Pos(0, 0, 0), Force (1,0,0), Torque(0,0,0)
                        m_aryForceStimPosX.Add(0)
                        m_aryForceStimPosY.Add(0)
                        m_aryForceStimPosZ.Add(0)
                        m_aryForceStimForceX.Add(1)
                        m_aryForceStimForceY.Add(0)
                        m_aryForceStimForceZ.Add(0)
                        m_aryForceStimTorqueX.Add(0)
                        m_aryForceStimTorqueY.Add(0)
                        m_aryForceStimTorqueZ.Add(0)

                        'Pos(0, 0, 0), Force (0,0,1), Torque(0,0,0)
                        m_aryForceStimPosX.Add(0)
                        m_aryForceStimPosY.Add(0)
                        m_aryForceStimPosZ.Add(0)
                        m_aryForceStimForceX.Add(0)
                        m_aryForceStimForceY.Add(0)
                        m_aryForceStimForceZ.Add(1)
                        m_aryForceStimTorqueX.Add(0)
                        m_aryForceStimTorqueY.Add(0)
                        m_aryForceStimTorqueZ.Add(0)

                        'Pos(0, 0, 0), Force (0,0,0), Torque(1,0,0)
                        m_aryForceStimPosX.Add(0)
                        m_aryForceStimPosY.Add(0)
                        m_aryForceStimPosZ.Add(0)
                        m_aryForceStimForceX.Add(0)
                        m_aryForceStimForceY.Add(0)
                        m_aryForceStimForceZ.Add(0)
                        m_aryForceStimTorqueX.Add(1)
                        m_aryForceStimTorqueY.Add(0)
                        m_aryForceStimTorqueZ.Add(0)

                        'Pos(0, 0, 0), Force (0,0,0), Torque(0,1,0)
                        m_aryForceStimPosX.Add(0)
                        m_aryForceStimPosY.Add(0)
                        m_aryForceStimPosZ.Add(0)
                        m_aryForceStimForceX.Add(0)
                        m_aryForceStimForceY.Add(0)
                        m_aryForceStimForceZ.Add(0)
                        m_aryForceStimTorqueX.Add(0)
                        m_aryForceStimTorqueY.Add(1)
                        m_aryForceStimTorqueZ.Add(0)

                        'Pos(0, 0, 0), Force (0,0,0), Torque(0,0,1)
                        m_aryForceStimPosX.Add(0)
                        m_aryForceStimPosY.Add(0)
                        m_aryForceStimPosZ.Add(0)
                        m_aryForceStimForceX.Add(0)
                        m_aryForceStimForceY.Add(0)
                        m_aryForceStimForceZ.Add(0)
                        m_aryForceStimTorqueX.Add(0)
                        m_aryForceStimTorqueY.Add(0)
                        m_aryForceStimTorqueZ.Add(1)

                        'Pos(0, 0.05, 0), Force (1,0,0), Torque(0,0,0)
                        m_aryForceStimPosX.Add(0)
                        m_aryForceStimPosY.Add(0.05)
                        m_aryForceStimPosZ.Add(0)
                        m_aryForceStimForceX.Add(1)
                        m_aryForceStimForceY.Add(0)
                        m_aryForceStimForceZ.Add(0)
                        m_aryForceStimTorqueX.Add(0)
                        m_aryForceStimTorqueY.Add(0)
                        m_aryForceStimTorqueZ.Add(0)

                        CleanupProjectDirectory()
                    End Sub

#End Region

#End Region

                End Class

            End Namespace
        End Namespace
    End Namespace
End Namespace
