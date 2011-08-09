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

<CodedUITest()>
Public MustInherit Class BodyPartUITest
    Inherits AnimatUITest

#Region "Attributes"

    Protected m_strPartType As String = "Box"
    Protected m_bTestTexture As Boolean = True

    Protected m_ptInitialZoomStart As Point = New Point(877, 100)
    Protected m_iInitialZoomDist1 As Integer = 320
    Protected m_iInitialZoomDist2 As Integer = 320

    Protected m_iSecondaryZoomDist1 As Integer = 320
    Protected m_iSecondaryZoomDist2 As Integer = 320

    Protected m_ptTranslateXAxisStart As Point = New Point(878, 426)
    Protected m_ptTranslateXAxisEnd As Point = New Point(1307, 490)

    Protected m_ptTranslateYAxisStart As Point = New Point(712, 226)
    Protected m_ptTranslateYAxisEnd As Point = New Point(711, 8)

    Protected m_ptTranslateZAxisStart As Point = New Point(534, 394)
    Protected m_ptTranslateZAxisEnd As Point = New Point(186, 394)

    Protected m_ptRotatePartForTranslateStart As Point = New Point(157, 393)
    Protected m_ptRotatePartForTranslateEnd As Point = New Point(1300, 359)

    Protected m_ptRotateXAxisStart As Point = New Point(696, 273)
    Protected m_ptRotateXAxisEnd As Point = New Point(645, 409)

    Protected m_ptRotateYAxisStart As Point = New Point(775, 403)
    Protected m_ptRotateYAxisEnd As Point = New Point(931, 413)

    Protected m_ptRotateZAxisStart As Point = New Point(779, 281)
    Protected m_ptRotateZAxisEnd As Point = New Point(693, 136)

    Protected m_dblMinTranslateAxisRange As Double = 0.05
    Protected m_dblMaxTranslateAxisRange As Double = 1

    Protected m_dblMinRotateAxisRange As Double = 10
    Protected m_dblMaxRotateAxisRange As Double = 90

    Protected m_dblResetStructXPos As Double = 0
    Protected m_dblResetStructYPos As Double = 0
    Protected m_dblResetStructZPos As Double = 0

    Protected m_dblManWorldXPos As Double = 1
    Protected m_dblManWorldXTest As Double = 1
    Protected m_dblManWorldLocalXTest As Double = 0
    Protected m_dblManChildWorldLocalXTest As Double = 1

    Protected m_dblManLocalXPos As Double = 2
    Protected m_dblManLocalXTest As Double = 2
    Protected m_dblManLocalWorldXTest As Double = 2
    Protected m_dblManChildLocalWorldXTest As Double = 2

    Protected m_dblManWorldYPos As Double = 1
    Protected m_dblManWorldYTest As Double = 1
    Protected m_dblManWorldLocalYTest As Double = 0
    Protected m_dblManChildWorldLocalYTest As Double = 1

    Protected m_dblManLocalYPos As Double = 2
    Protected m_dblManLocalYTest As Double = 2
    Protected m_dblManLocalWorldYTest As Double = 2
    Protected m_dblManChildLocalWorldYTest As Double = 2

    Protected m_dblManWorldZPos As Double = 1
    Protected m_dblManWorldZTest As Double = 1
    Protected m_dblManWorldLocalZTest As Double = 0
    Protected m_dblManChildWorldLocalZTest As Double = 1

    Protected m_dblManLocalZPos As Double = 2
    Protected m_dblManLocalZTest As Double = 2
    Protected m_dblManLocalWorldZTest As Double = 2
    Protected m_dblManChildLocalWorldZTest As Double = 2

    Protected m_dblManXRot As Double = 45
    Protected m_dblManYRot As Double = 45
    Protected m_dblManZRot As Double = 45

    Protected m_ptClickToAddChild As New Point(751, 362)

    Protected m_ptChildTranslateXAxisStart As Point = New Point(878, 426)
    Protected m_ptChildTranslateXAxisEnd As Point = New Point(1307, 490)

    Protected m_ptChildTranslateYAxisStart As Point = New Point(712, 226)
    Protected m_ptChildTranslateYAxisEnd As Point = New Point(711, 8)

    Protected m_ptChildTranslateZAxisStart As Point = New Point(534, 394)
    Protected m_ptChildTranslateZAxisEnd As Point = New Point(186, 394)

    Protected m_ptChildRotatePartForTranslateStart As Point = New Point(157, 393)
    Protected m_ptChildRotatePartForTranslateEnd As Point = New Point(1300, 359)

    Protected m_ptChildRotateXAxisStart As Point = New Point(696, 273)
    Protected m_ptChildRotateXAxisEnd As Point = New Point(645, 409)

    Protected m_ptChildRotateYAxisStart As Point = New Point(775, 403)
    Protected m_ptChildRotateYAxisEnd As Point = New Point(931, 413)

    Protected m_ptChildRotateZAxisStart As Point = New Point(779, 281)
    Protected m_ptChildRotateZAxisEnd As Point = New Point(693, 136)

    Protected m_dblRootChildRotation As Double = 45

    Protected m_dblStructChildRotateTestX As Double = 2.0
    Protected m_dblStructChildRotateTestY As Double = -0.000005119
    Protected m_dblStructChildRotateTestZ As Double = 2.828

    Protected m_dblStructJointRotateTestXX As Double = 2.0
    Protected m_dblStructJointRotateTestXY As Double = 0.070705
    Protected m_dblStructJointRotateTestXZ As Double = 2.899

    Protected m_dblStructJointRotateTestYX As Double = 2.828
    Protected m_dblStructJointRotateTestYY As Double = 2.1
    Protected m_dblStructJointRotateTestYZ As Double = -0.000005119

    Protected m_dblStructJointRotateTestZX As Double = -0.070716
    Protected m_dblStructJointRotateTestZY As Double = 2.899
    Protected m_dblStructJointRotateTestZZ As Double = 2.0

    Protected m_dblTestDensity As Double = 1.5

#End Region

#Region "Properties"

#End Region

#Region "Methods"

    '<TestMethod()>
    'Public Sub TestBodyParts()

    '    Me.UIMap.ZoomInOnRootPart()
    '    '            
    '    ' To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
    '    ' For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
    '    '
    'End Sub

    Protected Overrides Sub RecalculatePositionsUsingResolution()
        MyBase.RecalculatePositionsUsingResolution()

        m_ptInitialZoomStart.X = CInt(m_ptInitialZoomStart.X * m_dblResScaleWidth)
        m_ptInitialZoomStart.Y = CInt(m_ptInitialZoomStart.Y * m_dblResScaleHeight)

        m_iInitialZoomDist1 = CInt(m_iInitialZoomDist1 * m_dblResScaleHeight)
        m_iInitialZoomDist2 = CInt(m_iInitialZoomDist2 * m_dblResScaleHeight)

        m_ptTranslateXAxisStart.X = CInt(m_ptTranslateXAxisStart.X * m_dblResScaleWidth)
        m_ptTranslateXAxisStart.Y = CInt(m_ptTranslateXAxisStart.Y * m_dblResScaleHeight)

        m_ptTranslateXAxisEnd.X = CInt(m_ptTranslateXAxisEnd.X * m_dblResScaleWidth)
        m_ptTranslateXAxisEnd.Y = CInt(m_ptTranslateXAxisEnd.Y * m_dblResScaleHeight)

        m_ptTranslateYAxisStart.X = CInt(m_ptTranslateYAxisStart.X * m_dblResScaleWidth)
        m_ptTranslateYAxisStart.Y = CInt(m_ptTranslateYAxisStart.Y * m_dblResScaleHeight)

        m_ptTranslateYAxisEnd.X = CInt(m_ptTranslateYAxisEnd.X * m_dblResScaleWidth)
        m_ptTranslateYAxisEnd.Y = CInt(m_ptTranslateYAxisEnd.Y * m_dblResScaleHeight)

        m_ptTranslateZAxisStart.X = CInt(m_ptTranslateZAxisStart.X * m_dblResScaleWidth)
        m_ptTranslateZAxisStart.Y = CInt(m_ptTranslateZAxisStart.Y * m_dblResScaleHeight)

        m_ptTranslateZAxisEnd.X = CInt(m_ptTranslateZAxisEnd.X * m_dblResScaleWidth)
        m_ptTranslateZAxisEnd.Y = CInt(m_ptTranslateZAxisEnd.Y * m_dblResScaleHeight)

        m_ptRotatePartForTranslateStart.X = CInt(m_ptRotatePartForTranslateStart.X * m_dblResScaleWidth)
        m_ptRotatePartForTranslateStart.Y = CInt(m_ptRotatePartForTranslateStart.Y * m_dblResScaleHeight)

        m_ptRotatePartForTranslateEnd.X = CInt(m_ptRotatePartForTranslateEnd.X * m_dblResScaleWidth)
        m_ptRotatePartForTranslateEnd.Y = CInt(m_ptRotatePartForTranslateEnd.Y * m_dblResScaleHeight)

        m_ptRotateXAxisStart.X = CInt(m_ptRotateXAxisStart.X * m_dblResScaleWidth)
        m_ptRotateXAxisStart.Y = CInt(m_ptRotateXAxisStart.Y * m_dblResScaleHeight)

        m_ptRotateXAxisEnd.X = CInt(m_ptRotateXAxisEnd.X * m_dblResScaleWidth)
        m_ptRotateXAxisEnd.Y = CInt(m_ptRotateXAxisEnd.Y * m_dblResScaleHeight)

        m_ptRotateYAxisStart.X = CInt(m_ptRotateYAxisStart.X * m_dblResScaleWidth)
        m_ptRotateYAxisStart.Y = CInt(m_ptRotateYAxisStart.Y * m_dblResScaleHeight)

        m_ptRotateYAxisEnd.X = CInt(m_ptRotateYAxisEnd.X * m_dblResScaleWidth)
        m_ptRotateYAxisEnd.Y = CInt(m_ptRotateYAxisEnd.Y * m_dblResScaleHeight)

        m_ptRotateZAxisStart.X = CInt(m_ptRotateZAxisStart.X * m_dblResScaleWidth)
        m_ptRotateZAxisStart.Y = CInt(m_ptRotateZAxisStart.Y * m_dblResScaleHeight)

        m_ptRotateZAxisEnd.X = CInt(m_ptRotateZAxisEnd.X * m_dblResScaleWidth)
        m_ptRotateZAxisEnd.Y = CInt(m_ptRotateZAxisEnd.Y * m_dblResScaleHeight)

        m_ptClickToAddChild.X = CInt(m_ptClickToAddChild.X * m_dblResScaleWidth)
        m_ptClickToAddChild.Y = CInt(m_ptClickToAddChild.Y * m_dblResScaleHeight)

        m_ptChildTranslateXAxisStart.X = CInt(m_ptChildTranslateXAxisStart.X * m_dblResScaleWidth)
        m_ptChildTranslateXAxisStart.Y = CInt(m_ptChildTranslateXAxisStart.Y * m_dblResScaleHeight)

        m_ptChildTranslateXAxisEnd.X = CInt(m_ptChildTranslateXAxisEnd.X * m_dblResScaleWidth)
        m_ptChildTranslateXAxisEnd.Y = CInt(m_ptChildTranslateXAxisEnd.Y * m_dblResScaleHeight)

        m_ptChildTranslateYAxisStart.X = CInt(m_ptChildTranslateYAxisStart.X * m_dblResScaleWidth)
        m_ptChildTranslateYAxisStart.Y = CInt(m_ptChildTranslateYAxisStart.Y * m_dblResScaleHeight)

        m_ptChildTranslateYAxisEnd.X = CInt(m_ptChildTranslateYAxisEnd.X * m_dblResScaleWidth)
        m_ptChildTranslateYAxisEnd.Y = CInt(m_ptChildTranslateYAxisEnd.Y * m_dblResScaleHeight)

        m_ptChildTranslateZAxisStart.X = CInt(m_ptChildTranslateZAxisStart.X * m_dblResScaleWidth)
        m_ptChildTranslateZAxisStart.Y = CInt(m_ptChildTranslateZAxisStart.Y * m_dblResScaleHeight)

        m_ptChildTranslateZAxisEnd.X = CInt(m_ptChildTranslateZAxisEnd.X * m_dblResScaleWidth)
        m_ptChildTranslateZAxisEnd.Y = CInt(m_ptChildTranslateZAxisEnd.Y * m_dblResScaleHeight)

        m_ptChildRotatePartForTranslateStart.X = CInt(m_ptChildRotatePartForTranslateStart.X * m_dblResScaleWidth)
        m_ptChildRotatePartForTranslateStart.Y = CInt(m_ptChildRotatePartForTranslateStart.Y * m_dblResScaleHeight)

        m_ptChildRotatePartForTranslateEnd.X = CInt(m_ptChildRotatePartForTranslateEnd.X * m_dblResScaleWidth)
        m_ptChildRotatePartForTranslateEnd.Y = CInt(m_ptChildRotatePartForTranslateEnd.Y * m_dblResScaleHeight)

        m_ptChildRotateXAxisStart.X = CInt(m_ptChildRotateXAxisStart.X * m_dblResScaleWidth)
        m_ptChildRotateXAxisStart.Y = CInt(m_ptChildRotateXAxisStart.Y * m_dblResScaleHeight)

        m_ptChildRotateXAxisEnd.X = CInt(m_ptChildRotateXAxisEnd.X * m_dblResScaleWidth)
        m_ptChildRotateXAxisEnd.Y = CInt(m_ptChildRotateXAxisEnd.Y * m_dblResScaleHeight)

        m_ptChildRotateYAxisStart.X = CInt(m_ptChildRotateYAxisStart.X * m_dblResScaleWidth)
        m_ptChildRotateYAxisStart.Y = CInt(m_ptChildRotateYAxisStart.Y * m_dblResScaleHeight)

        m_ptChildRotateYAxisEnd.X = CInt(m_ptChildRotateYAxisEnd.X * m_dblResScaleWidth)
        m_ptChildRotateYAxisEnd.Y = CInt(m_ptChildRotateYAxisEnd.Y * m_dblResScaleHeight)

        m_ptChildRotateZAxisStart.X = CInt(m_ptChildRotateZAxisStart.X * m_dblResScaleWidth)
        m_ptChildRotateZAxisStart.Y = CInt(m_ptChildRotateZAxisStart.Y * m_dblResScaleHeight)

        m_ptChildRotateZAxisEnd.X = CInt(m_ptChildRotateZAxisEnd.X * m_dblResScaleWidth)
        m_ptChildRotateZAxisEnd.Y = CInt(m_ptChildRotateZAxisEnd.Y * m_dblResScaleHeight)

    End Sub

    Protected Overridable Sub TestPart()
        'Start the application.
        StartApplication("", 8080, False)

        CreateNewProject(m_strProjectName, m_strProjectPath, 15)

        'Add a root part.
        AddRootPartType(m_strPartType)

        'There is some kind of weird timing bug in the testing code here. When I add a part manually it goes to SelectCollisions mode just fine,
        'but when I do it here for some reason I have to set it to something else first and then back. I think it has something to do with the 
        ' timing of the call or something. Regardless, it does not really matter here, I just need it in Collisions mode and that works when done
        ' manually, so I am using this trick to get it to work in the test.
        ExecuteMethod("ClickToolbarItem", New Object() {"SelGraphicsToolStripButton"})
        ExecuteMethod("ClickToolbarItem", New Object() {"SelCollisionToolStripButton"})
        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Structures\Structure_1\Body Plan\Root"})

        'Select the simulation window tab so it is visible.
        ExecuteMethod("SelectTrackItems", New Object() {"Simulation\Environment\Structures\Structure_1", "Structure_1", "Root"})

        TestMovableRigidBodyProperties("Structure_1", "Root")

        RecalculatePositionsUsingResolution()

        'Zoom in on the part so we can try and move it with the mouse.
        ZoomInOnRootPart(m_ptInitialZoomStart, m_iInitialZoomDist1, m_iInitialZoomDist2)

        'Move the z axis and verify position.
        MovePartAxis("Structure_1", "Root", "Z", m_ptTranslateZAxisStart, m_ptTranslateZAxisEnd, _
                     m_dblMinTranslateAxisRange, m_dblMaxTranslateAxisRange, _
                     m_dblMinTranslateAxisRange, m_dblMaxTranslateAxisRange, 0, 0)

        'Move the z axis and verify position.
        MovePartAxis("Structure_1", "Root", "Y", m_ptTranslateYAxisStart, m_ptTranslateYAxisEnd, _
                     m_dblMinTranslateAxisRange, m_dblMaxTranslateAxisRange, _
                     m_dblMinTranslateAxisRange, m_dblMaxTranslateAxisRange, 0, 0)

        'Move view around to see z axis.
        DragMouse(m_ptRotatePartForTranslateStart, m_ptRotatePartForTranslateEnd, MouseButtons.Left, ModifierKeys.None, True)

        'Move the x axis and verify position.
        MovePartAxis("Structure_1", "Root", "X", m_ptTranslateXAxisStart, m_ptTranslateXAxisEnd, _
                     m_dblMinTranslateAxisRange, m_dblMaxTranslateAxisRange, _
                     m_dblMinTranslateAxisRange, m_dblMaxTranslateAxisRange, 0, 0)

        'Rotate the x axis
        RotatePartAxis("Structure_1", "Root", "X", m_ptRotateXAxisStart, m_ptRotateXAxisEnd, m_dblMinRotateAxisRange, m_dblMaxRotateAxisRange, True)

        'Rotate the y axis
        RotatePartAxis("Structure_1", "Root", "Y", m_ptRotateYAxisStart, m_ptRotateYAxisEnd, m_dblMinRotateAxisRange, m_dblMaxRotateAxisRange, True)

        'Rotate the z axis
        RotatePartAxis("Structure_1", "Root", "Z", m_ptRotateZAxisStart, m_ptRotateZAxisEnd, m_dblMinRotateAxisRange, m_dblMaxRotateAxisRange, True)

        ResetStructurePosition("Structure_1", "Root", m_dblResetStructXPos, m_dblResetStructYPos, m_dblResetStructZPos, True, 0.001)

        ManualMovePartAxis("Structure_1", "Root", "X", m_dblManWorldXPos, m_dblManWorldXTest, m_dblManWorldLocalXTest, False, m_dblManLocalXPos, m_dblManLocalXTest, m_dblManLocalWorldXTest, 0.01)
        ManualMovePartAxis("Structure_1", "Root", "Y", m_dblManWorldYPos, m_dblManWorldYTest, m_dblManWorldLocalYTest, False, m_dblManLocalYPos, m_dblManLocalYTest, m_dblManLocalWorldYTest, 0.01)
        ManualMovePartAxis("Structure_1", "Root", "Z", m_dblManWorldZPos, m_dblManWorldZTest, m_dblManWorldLocalZTest, False, m_dblManLocalZPos, m_dblManLocalZTest, m_dblManLocalWorldZTest, 0.01)

        ManualRotatePartAxis("Structure_1", "Root", "X", m_dblManXRot, True, 0.001)
        ManualRotatePartAxis("Structure_1", "Root", "Y", m_dblManYRot, True, 0.001)
        ManualRotatePartAxis("Structure_1", "Root", "Z", m_dblManZRot, True, 0.001)

        ResetStructurePosition("Structure_1", "Root", m_dblResetStructXPos, m_dblResetStructYPos, m_dblResetStructZPos, True, 0.001)

        'We have tested moving/rotating the root part, now test doing it on a child part.
        AddChildPartTypeWithJoint(m_strPartType, "Hinge", m_ptClickToAddChild)

        'Select the simulation window tab so it is visible.
        ExecuteMethod("SelectTrackItems", New Object() {"Simulation\Environment\Structures\Structure_1", "Structure_1", "Body_1"})

        'Zoom in on the part so we can try and move it with the mouse.
        ZoomInOnRootPart(m_ptInitialZoomStart, m_iSecondaryZoomDist1, m_iSecondaryZoomDist2)

        'Move the z axis and verify position.
        MovePartAxis("Structure_1", "Root\Joint_1\Body_1", "Z", m_ptChildTranslateZAxisStart, m_ptChildTranslateZAxisEnd, _
                     m_dblMinTranslateAxisRange, m_dblMaxTranslateAxisRange, 0, 0, _
                     m_dblMinTranslateAxisRange, m_dblMaxTranslateAxisRange)

        'Move the z axis and verify position.
        MovePartAxis("Structure_1", "Root\Joint_1\Body_1", "Y", m_ptChildTranslateYAxisStart, m_ptChildTranslateYAxisEnd, _
                     m_dblMinTranslateAxisRange, m_dblMaxTranslateAxisRange, 0, 0, _
                     m_dblMinTranslateAxisRange, m_dblMaxTranslateAxisRange)

        'Move view around to see z axis.
        DragMouse(m_ptChildRotatePartForTranslateStart, m_ptChildRotatePartForTranslateEnd, MouseButtons.Left, ModifierKeys.None, True)

        'Move the x axis and verify position.
        MovePartAxis("Structure_1", "Root\Joint_1\Body_1", "X", m_ptChildTranslateXAxisStart, m_ptChildTranslateXAxisEnd, _
                     m_dblMinTranslateAxisRange, m_dblMaxTranslateAxisRange, 0, 0, _
                     m_dblMinTranslateAxisRange, m_dblMaxTranslateAxisRange)

        'Rotate the x axis
        RotatePartAxis("Structure_1", "Root\Joint_1\Body_1", "X", m_ptChildRotateXAxisStart, m_ptChildRotateXAxisEnd, m_dblMinRotateAxisRange, m_dblMaxRotateAxisRange, True)

        'Rotate the y axis
        RotatePartAxis("Structure_1", "Root\Joint_1\Body_1", "Y", m_ptChildRotateYAxisStart, m_ptChildRotateYAxisEnd, m_dblMinRotateAxisRange, m_dblMaxRotateAxisRange, True)

        'Rotate the z axis
        RotatePartAxis("Structure_1", "Root\Joint_1\Body_1", "Z", m_ptChildRotateZAxisStart, m_ptChildRotateZAxisEnd, m_dblMinRotateAxisRange, m_dblMaxRotateAxisRange, True)

        ManualMovePartAxis("Structure_1", "Root\Joint_1\Body_1", "X", m_dblManWorldXPos, m_dblManWorldXTest, m_dblManChildWorldLocalXTest, True, m_dblManLocalXPos, m_dblManLocalXTest, m_dblManChildLocalWorldXTest, 0.01)
        ManualMovePartAxis("Structure_1", "Root\Joint_1\Body_1", "Y", m_dblManWorldYPos, m_dblManWorldYTest, m_dblManChildWorldLocalYTest, True, m_dblManLocalYPos, m_dblManLocalYTest, m_dblManChildLocalWorldYTest, 0.01)
        ManualMovePartAxis("Structure_1", "Root\Joint_1\Body_1", "Z", m_dblManWorldZPos, m_dblManWorldZTest, m_dblManChildWorldLocalZTest, True, m_dblManLocalZPos, m_dblManLocalZTest, m_dblManChildLocalWorldZTest, 0.01)

        ManualRotatePartAxis("Structure_1", "Root\Joint_1\Body_1", "X", m_dblManXRot, True, 0.001)
        ManualRotatePartAxis("Structure_1", "Root\Joint_1\Body_1", "Y", m_dblManYRot, True, 0.001)
        ManualRotatePartAxis("Structure_1", "Root\Joint_1\Body_1", "Z", m_dblManZRot, True, 0.001)

        'Verify the child part position after rotating the structure.
        VerifyChildPosAfterRotate("Structure_1", "X", "Root\Joint_1\Body_1", m_dblRootChildRotation, m_dblStructChildRotateTestX, m_dblStructChildRotateTestY, m_dblStructChildRotateTestZ)
        VerifyChildPosAfterRotate("Structure_1", "Y", "Root\Joint_1\Body_1", m_dblRootChildRotation, m_dblStructChildRotateTestZ, m_dblStructChildRotateTestX, m_dblStructChildRotateTestY)
        VerifyChildPosAfterRotate("Structure_1", "Z", "Root\Joint_1\Body_1", m_dblRootChildRotation, m_dblStructChildRotateTestY, m_dblStructChildRotateTestZ, m_dblStructChildRotateTestX)

        'Verify the joint position after rotating the structure.
        VerifyChildPosAfterRotate("Structure_1", "X", "Root\Joint_1", m_dblRootChildRotation, m_dblStructJointRotateTestXX, m_dblStructJointRotateTestXY, m_dblStructJointRotateTestXZ)
        VerifyChildPosAfterRotate("Structure_1", "Y", "Root\Joint_1", m_dblRootChildRotation, m_dblStructJointRotateTestYX, m_dblStructJointRotateTestYY, m_dblStructJointRotateTestYZ)
        VerifyChildPosAfterRotate("Structure_1", "Z", "Root\Joint_1", m_dblRootChildRotation, m_dblStructJointRotateTestZX, m_dblStructJointRotateTestZY, m_dblStructJointRotateTestZZ)

        Threading.Thread.Sleep(1000)

        CreateChartAndAddBodies(10)

        'Move the structure up.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\Structure_1", "WorldPosition.Y", "1"})

        'Run the simulation and wait for it to end.
        RunSimulationWaitToEnd()

        'Compare chart data to verify simulation results.
        CompareSimulation(m_strRootFolder & m_strTestDataPath, "BeforeStruct_")

        'Create a new structure
        CreateStructure("Structure_2", "Structure_2")

        'Add a plane part.
        AddRootPartType("Plane")

        'Run the simulation and wait for it to end.
        RunSimulationWaitToEnd()

        'Compare chart data to verify simulation results.
        CompareSimulation(m_strRootFolder & m_strTestDataPath, "AfterStruct_")

        'Now lets remove the child body of the falling part.
        DeletePart("Simulation\Environment\Structures\Structure_1\Body Plan\Root\Joint_1\Body_1")

        'Run the simulation and wait for it to end.
        RunSimulationWaitToEnd()

        'Compare chart data to verify simulation results.
        CompareSimulation(m_strRootFolder & m_strTestDataPath, "AfterS1Child_")

        'Now lets remove the plane part of the second structure, and that structure.
        DeletePart("Simulation\Environment\Structures\Structure_2\Body Plan\Root")

        'Remove the structure
        DeletePart("Simulation\Environment\Structures\Structure_2")

        'Run the simulation and wait for it to end.
        RunSimulationWaitToEnd()

        'Compare chart data to verify simulation results.
        CompareSimulation(m_strRootFolder & m_strTestDataPath, "AfterS2_")

        'Now lets remove the root of the structure 1, and that structure.
        DeletePart("Simulation\Environment\Structures\Structure_1\Body Plan\Root")

        'Now lets remove structure 1, and that structure.
        DeletePart("Simulation\Environment\Structures\Structure_1")

        'Make sure simulation can still run.
        RunSimulationWaitToEnd()

    End Sub

    Protected Overridable Sub TestMovableRigidBodyProperties(ByVal strStructure As String, ByVal strPart As String)
        TestMovableItemProperties(strStructure, strPart)

        'Set the density to a valid value.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Density", m_dblTestDensity.ToString})

        Dim dblTest As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Density.ActualValue"), Double)

        If dblTest <> m_dblTestDensity Then
            Throw New System.Exception("Body part density does not match the target value: " & m_dblTestDensity & ", recorded value: " & dblTest)
        End If

        'Check for error when density is zero.
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Density", "0"}, "The density can not be less than or equal to zero.")

        'Check for error when density is less than zero.
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Density", "-1"}, "The density can not be less than or equal to zero.")

    End Sub

    Protected Overridable Sub TestMovableItemProperties(ByVal strStructure As String, ByVal strPart As String)


        'Set the ambient to a valid value.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Ambient", "#1E1E1E"})

        'Set the diffuse to a valid value.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Diffuse", "#FFFFFF"})

        'Set the specular to a valid value.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Specular", "#1E1E1E"})

        'Set the shininess to a valid value.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Shininess", "70"})

        'Set the shininess to an valid value.
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Shininess", "-1"}, "Shininess must be greater than or equal to zero.")

        'Set the shininess to an valid value.
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Shininess", "129"}, "Shininess must be less than 128.")

        If m_bTestTexture Then
            'Set the texture to an valid value.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Texture", _
                                                                        (m_strRootFolder & "\bin\Resources\Bricks.bmp")})
            'Set the texture to an invalid value.
            ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Texture", _
                                                                        (m_strRootFolder & "\bin\Resources\Bricks.bmp")}, "The specified file does not exist: ", enumErrorTextType.BeginsWith)

            'Set the texture to an invalid value.
            ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Texture", _
                                                                        (m_strRootFolder & "\bin\Resources\Test.txt")}, "Unable to load the texture file. This does not appear to be a vaild image file.", enumErrorTextType.BeginsWith)
        End If

        'Set the visible to a valid value.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Visible", "False"})

        'Turn visible back on.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Visible", "True"})

        'Set the Transparencies.GraphicsTransparency to a valid value.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.GraphicsTransparency", "50"})

        'Set the Transparencies.GraphicsTransparency to high.
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.GraphicsTransparency", "150"}, "Transparency values cannot be greater than 100%.")

        'Set the Transparencies.GraphicsTransparency too low
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.GraphicsTransparency", "-50"}, "Transparency values cannont be less than 0%.")


        'Set the Transparencies.CollisionsTransparency to a valid value.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.CollisionsTransparency", "50"})

        'Set the Transparencies.CollisionsTransparency to high.
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.CollisionsTransparency", "150"}, "Transparency values cannot be greater than 100%.")

        'Set the Transparencies.CollisionsTransparency too low
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.CollisionsTransparency", "-50"}, "Transparency values cannont be less than 0%.")


        'Set the Transparencies.JointsTransparency to a valid value.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.JointsTransparency", "50"})

        'Set the Transparencies.JointsTransparency to high.
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.JointsTransparency", "150"}, "Transparency values cannot be greater than 100%.")

        'Set the Transparencies.JointsTransparency too low
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.JointsTransparency", "-50"}, "Transparency values cannont be less than 0%.")


        'Set the Transparencies.ReceptiveFieldsTransparency to a valid value.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.ReceptiveFieldsTransparency", "50"})

        'Set the Transparencies.ReceptiveFieldsTransparency to high.
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.ReceptiveFieldsTransparency", "150"}, "Transparency values cannot be greater than 100%.")

        'Set the Transparencies.ReceptiveFieldsTransparency too low
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.ReceptiveFieldsTransparency", "-50"}, "Transparency values cannont be less than 0%.")


        'Set the Transparencies.SimulationTransparency to a valid value.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.SimulationTransparency", "50"})

        'Set the Transparencies.SimulationTransparency to high.
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.SimulationTransparency", "150"}, "Transparency values cannot be greater than 100%.")

        'Set the Transparencies.SimulationTransparency too low
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.SimulationTransparency", "-50"}, "Transparency values cannont be less than 0%.")

        'Reset the Transparencies to their original values.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.GraphicsTransparency", "50"})
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.CollisionsTransparency", "0"})
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.JointsTransparency", "50"})
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.ReceptiveFieldsTransparency", "50"})
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Transparencies.SimulationTransparency", "100"})

        'If this is the root part then set its child graphics object to be clear for the rest of the tests so it does not look funky.
        If strPart = "Root" Then
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart & "\Root_Graphics", "Transparencies.CollisionsTransparency", "100"})
        End If

        'Set the Description to a valid value.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Description", "Test"})

        'Set the Name to a valid value.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Name", "Test"})

        'Set the Name to an valid value.
        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\Test", "Name", ""}, "The name property can not be blank.")

        'Reset the name to root.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\Test", "Name", strPart})

    End Sub

#Region "GenerateCode"

 

#End Region

#End Region

End Class
