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

Namespace UITests
    Namespace BodyEditorTests
        Namespace BodyPartTests

            <CodedUITest()>
            Public MustInherit Class BodyPartUITest
                Inherits AnimatUITest

#Region "Attributes"

                Protected m_strPartType As String = "Box"
                Protected m_strSecondaryPartType As String = ""
                Protected m_strChildJoint As String = "Hinge"
                Protected m_strStruct2RootPart As String = "Plane"

                Protected m_bTestDensity As Boolean = True

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

                Protected m_ptTerrainAxisViewStart As Point = New Point(518, 632)
                Protected m_ptTerrainAxisViewEnd As Point = New Point(604, 564)

                Protected m_dblMinTranRootWorldX As Double = 0.05
                Protected m_dblMaxTranRootWorldX As Double = 2

                Protected m_dblMinTranRootWorldY As Double = 0.05
                Protected m_dblMaxTranRootWorldY As Double = 2

                Protected m_dblMinTranRootWorldZ As Double = 0.05
                Protected m_dblMaxTranRootWorldZ As Double = 2

                Protected m_dblMinTranRootStructX As Double = 0.05
                Protected m_dblMaxTranRootStructX As Double = 2

                Protected m_dblMinTranRootStructY As Double = 0.05
                Protected m_dblMaxTranRootStructY As Double = 2

                Protected m_dblMinTranRootStructZ As Double = 0.05
                Protected m_dblMaxTranRootStructZ As Double = 2

                Protected m_dblMinTranRootLocalX As Double = 0
                Protected m_dblMaxTranRootLocalX As Double = 0

                Protected m_dblMinTranRootLocalY As Double = 0
                Protected m_dblMaxTranRootLocalY As Double = 0

                Protected m_dblMinTranRootLocalZ As Double = 0
                Protected m_dblMaxTranRootLocalZ As Double = 0

                Protected m_dblMinTranChildWorldX As Double = 0.05
                Protected m_dblMaxTranChildWorldX As Double = 2

                Protected m_dblMinTranChildWorldY As Double = 0.05
                Protected m_dblMaxTranChildWorldY As Double = 2

                Protected m_dblMinTranChildWorldZ As Double = 0.05
                Protected m_dblMaxTranChildWorldZ As Double = 2

                Protected m_dblMinTranChildStructX As Double = 0
                Protected m_dblMaxTranChildStructX As Double = 0

                Protected m_dblMinTranChildStructY As Double = 0
                Protected m_dblMaxTranChildStructY As Double = 0

                Protected m_dblMinTranChildStructZ As Double = 0
                Protected m_dblMaxTranChildStructZ As Double = 0

                Protected m_dblMinTranChildLocalX As Double = 0.05
                Protected m_dblMaxTranChildLocalX As Double = 2

                Protected m_dblMinTranChildLocalY As Double = 0.05
                Protected m_dblMaxTranChildLocalY As Double = 2

                Protected m_dblMinTranChildLocalZ As Double = 0.05
                Protected m_dblMaxTranChildLocalZ As Double = 2

                Protected m_dblMinRotRootX As Double = 10
                Protected m_dblMaxRotRootX As Double = 90

                Protected m_dblMinRotRootY As Double = 10
                Protected m_dblMaxRotRootY As Double = 90

                Protected m_dblMinRotRootZ As Double = 10
                Protected m_dblMaxRotRootZ As Double = 90

                Protected m_dblMinRotChildX As Double = 10
                Protected m_dblMaxRotChildX As Double = 90

                Protected m_dblMinRotChildY As Double = 10
                Protected m_dblMaxRotChildY As Double = 90

                Protected m_dblMinRotChildZ As Double = 10
                Protected m_dblMaxRotChildZ As Double = 90


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

                Protected m_ptChildTranslateYAxisStart As Point = New Point(712, 210)
                Protected m_ptChildTranslateYAxisEnd As Point = New Point(711, 2)

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

                Protected m_dblStructChildRotateTestXX As Double = 2.0
                Protected m_dblStructChildRotateTestXY As Double = -0.000005119
                Protected m_dblStructChildRotateTestXZ As Double = 2.828

                Protected m_dblStructChildRotateTestYX As Double = 2.828
                Protected m_dblStructChildRotateTestYY As Double = 2.0
                Protected m_dblStructChildRotateTestYZ As Double = -0.000005119

                Protected m_dblStructChildRotateTestZX As Double = -0.000005119
                Protected m_dblStructChildRotateTestZY As Double = 2.828
                Protected m_dblStructChildRotateTestZZ As Double = 2.0

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

                Protected m_strMoveRootWorldXAxis As String = "X"
                Protected m_strMoveRootLocalXAxis As String = "X"

                Protected m_strMoveRootWorldYAxis As String = "Y"
                Protected m_strMoveRootLocalYAxis As String = "Y"

                Protected m_strMoveRootWorldZAxis As String = "Z"
                Protected m_strMoveRootLocalZAxis As String = "Z"

                Protected m_strMoveChildWorldXAxis As String = "X"
                Protected m_strMoveChildLocalXAxis As String = "X"

                Protected m_strMoveChildWorldYAxis As String = "Y"
                Protected m_strMoveChildLocalYAxis As String = "Y"

                Protected m_strMoveChildWorldZAxis As String = "Z"
                Protected m_strMoveChildLocalZAxis As String = "Z"

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


                Protected Overridable Sub TestPart()

                    StartNewProject()
                    CreateAndTestRoot()
                    CreateAndTestChild()
                    CreateChartAndAddBodies()
                    SimulateAndDeleteParts()

                End Sub

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

                    m_ptTerrainAxisViewStart.X = CInt(m_ptTerrainAxisViewStart.X * m_dblResScaleWidth)
                    m_ptTerrainAxisViewEnd.Y = CInt(m_ptTerrainAxisViewEnd.Y * m_dblResScaleHeight)

                End Sub

                Protected Overridable Sub CreateAndTestRoot()

                    'Add a root part.
                    AddRootPartType(m_strStructureGroup, m_strStruct1Name, m_strPartType)

                    'Select the simulation window tab so it is visible.
                    ExecuteMethod("SelectTrackItems", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name, m_strStruct1Name, "Root"})

                    TestMovableRigidBodyProperties(m_strStruct1Name, "Root")

                    RecalculatePositionsUsingResolution()

                    'Zoom in on the part so we can try and move it with the mouse.
                    ZoomInOnPart(m_ptInitialZoomStart, m_iInitialZoomDist1, m_iInitialZoomDist2)

                    'MouseMoveRoot()

                    'MouseRotateRoot()

                    ResetStructurePosition(m_strStruct1Name, "Root", m_dblResetStructXPos, m_dblResetStructYPos, m_dblResetStructZPos, True, 0.001)

                    ManualMoveRoot()

                    ManualRotateRoot()

                    ResetStructurePosition(m_strStruct1Name, "Root", m_dblResetStructXPos, m_dblResetStructYPos, m_dblResetStructZPos, True, 0.001)
                End Sub


                Protected Overridable Sub MouseMoveRoot()
                    'Move the z axis and verify position.
                    MovePartAxis(m_strStruct1Name, "Root", _
                                 m_strMoveRootWorldZAxis, m_strMoveRootLocalZAxis, _
                                 m_ptTranslateZAxisStart, m_ptTranslateZAxisEnd, _
                                 m_dblMinTranRootWorldZ, m_dblMaxTranRootWorldZ, _
                                 m_dblMinTranRootStructZ, m_dblMaxTranRootStructZ, _
                                 m_dblMinTranRootLocalZ, m_dblMaxTranRootLocalZ)

                    'Move the z axis and verify position.
                    MovePartAxis(m_strStruct1Name, "Root", _
                                 m_strMoveRootWorldYAxis, m_strMoveRootLocalYAxis, _
                                 m_ptTranslateYAxisStart, m_ptTranslateYAxisEnd, _
                                 m_dblMinTranRootWorldY, m_dblMaxTranRootWorldY, _
                                 m_dblMinTranRootStructY, m_dblMaxTranRootStructY, _
                                 m_dblMinTranRootLocalY, m_dblMaxTranRootLocalY)

                    'Move view around to see z axis.
                    DragMouse(m_ptRotatePartForTranslateStart, m_ptRotatePartForTranslateEnd, MouseButtons.Left, ModifierKeys.None, True)

                    'Move the x axis and verify position.
                    MovePartAxis(m_strStruct1Name, "Root", _
                                 m_strMoveRootWorldXAxis, m_strMoveRootLocalXAxis, _
                                 m_ptTranslateXAxisStart, m_ptTranslateXAxisEnd, _
                                 m_dblMinTranRootWorldX, m_dblMaxTranRootWorldX, _
                                 m_dblMinTranRootStructX, m_dblMaxTranRootStructX, _
                                 m_dblMinTranRootLocalX, m_dblMaxTranRootLocalX)

                End Sub

                Protected Overridable Sub MouseRotateRoot()
                    If Me.AllowRootRotations Then
                        'Rotate the x axis
                        RotatePartAxis(m_strStruct1Name, "Root", "X", m_ptRotateXAxisStart, m_ptRotateXAxisEnd, m_dblMinRotRootX, m_dblMaxRotRootX, True)

                        'Rotate the y axis
                        RotatePartAxis(m_strStruct1Name, "Root", "Y", m_ptRotateYAxisStart, m_ptRotateYAxisEnd, m_dblMinRotRootY, m_dblMaxRotRootY, True)

                        'Rotate the z axis
                        RotatePartAxis(m_strStruct1Name, "Root", "Z", m_ptRotateZAxisStart, m_ptRotateZAxisEnd, m_dblMinRotRootZ, m_dblMaxRotRootZ, True)
                    End If
                End Sub

                Protected Overridable Sub ManualMoveRoot()
                    ManualMovePartAxis(m_strStruct1Name, "Root", _
                                       m_strMoveRootWorldXAxis, m_strMoveRootLocalXAxis, _
                                       m_dblManWorldXPos, m_dblManWorldXTest, m_dblManWorldLocalXTest, _
                                       False, m_dblManLocalXPos, m_dblManLocalXTest, m_dblManLocalWorldXTest, 0.01)

                    ManualMovePartAxis(m_strStruct1Name, "Root", _
                                       m_strMoveRootWorldYAxis, m_strMoveRootLocalYAxis, _
                                       m_dblManWorldYPos, m_dblManWorldYTest, m_dblManWorldLocalYTest, _
                                       False, m_dblManLocalYPos, m_dblManLocalYTest, m_dblManLocalWorldYTest, 0.01)

                    ManualMovePartAxis(m_strStruct1Name, "Root", _
                                       m_strMoveRootWorldZAxis, m_strMoveRootLocalZAxis, _
                                       m_dblManWorldZPos, m_dblManWorldZTest, m_dblManWorldLocalZTest, _
                                       False, m_dblManLocalZPos, m_dblManLocalZTest, m_dblManLocalWorldZTest, 0.01)
                End Sub

                Protected Overridable Sub ManualRotateRoot()
                    If Me.AllowRootRotations Then
                        ManualRotatePartAxis(m_strStruct1Name, "Root", "X", m_dblManXRot, True, 0.001)
                        ManualRotatePartAxis(m_strStruct1Name, "Root", "Y", m_dblManYRot, True, 0.001)
                        ManualRotatePartAxis(m_strStruct1Name, "Root", "Z", m_dblManZRot, True, 0.001)
                    End If
                End Sub

                Protected Overridable Sub CreateAndTestChild()

                    If m_strSecondaryPartType.Trim.Length = 0 Then
                        m_strSecondaryPartType = m_strPartType
                    End If

                    'We have tested moving/rotating the root part, now test doing it on a child part.
                    AddChildPartTypeWithJoint(m_strSecondaryPartType, m_strChildJoint, m_ptClickToAddChild)

                    RepositionChildPart()

                    'Select the simulation window tab so it is visible.
                    ExecuteMethod("SelectTrackItems", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name, m_strStruct1Name, "Body_1"})

                    'Zoom in on the part so we can try and move it with the mouse.
                    ZoomInOnPart(m_ptInitialZoomStart, m_iSecondaryZoomDist1, m_iSecondaryZoomDist2)

                    'MouseMoveChild()

                    ' MouseRotateChild()

                    ManualMoveChild()

                    ManualRotateChild()

                    VerifyChildPositionAfterRotateStructure()

                    VerifyJointPositionAfterRotateStructure()

                    Threading.Thread.Sleep(1000)

                End Sub

                Protected Overridable Sub MouseMoveChild()
                    'Move the z axis and verify position.
                    MovePartAxis(m_strStruct1Name, "Root\Joint_1\Body_1", _
                                 m_strMoveChildWorldZAxis, m_strMoveChildLocalZAxis, _
                                 m_ptChildTranslateZAxisStart, m_ptChildTranslateZAxisEnd, _
                                 m_dblMinTranChildWorldZ, m_dblMaxTranChildWorldZ, _
                                 m_dblMinTranChildStructZ, m_dblMaxTranChildStructZ, _
                                 m_dblMinTranChildLocalZ, m_dblMaxTranChildLocalZ)

                    'Move the y axis and verify position.
                    MovePartAxis(m_strStruct1Name, "Root\Joint_1\Body_1", _
                                 m_strMoveChildWorldYAxis, m_strMoveChildLocalYAxis, _
                                 m_ptChildTranslateYAxisStart, m_ptChildTranslateYAxisEnd, _
                                 m_dblMinTranChildWorldY, m_dblMaxTranChildWorldY, _
                                 m_dblMinTranChildStructY, m_dblMaxTranChildStructY, _
                                 m_dblMinTranChildLocalY, m_dblMaxTranChildLocalY)

                    'Move view around to see z axis.
                    DragMouse(m_ptChildRotatePartForTranslateStart, m_ptChildRotatePartForTranslateEnd, MouseButtons.Left, ModifierKeys.None, True)

                    'Move the x axis and verify position.
                    MovePartAxis(m_strStruct1Name, "Root\Joint_1\Body_1", _
                                 m_strMoveChildWorldXAxis, m_strMoveChildLocalXAxis, _
                                 m_ptChildTranslateXAxisStart, m_ptChildTranslateXAxisEnd, _
                                 m_dblMinTranChildWorldX, m_dblMaxTranChildWorldX, _
                                 m_dblMinTranChildStructX, m_dblMaxTranChildStructX, _
                                 m_dblMinTranChildLocalX, m_dblMaxTranChildLocalX)

                End Sub

                Protected Overridable Sub MouseRotateChild()
                    If Me.AllowChildRotations Then
                        'Rotate the x axis
                        RotatePartAxis(m_strStruct1Name, "Root\Joint_1\Body_1", "X", m_ptChildRotateXAxisStart, m_ptChildRotateXAxisEnd, m_dblMinRotChildX, m_dblMaxRotChildX, True)

                        'Rotate the y axis
                        RotatePartAxis(m_strStruct1Name, "Root\Joint_1\Body_1", "Y", m_ptChildRotateYAxisStart, m_ptChildRotateYAxisEnd, m_dblMinRotChildY, m_dblMaxRotChildY, True)

                        'Rotate the z axis
                        RotatePartAxis(m_strStruct1Name, "Root\Joint_1\Body_1", "Z", m_ptChildRotateZAxisStart, m_ptChildRotateZAxisEnd, m_dblMinRotChildZ, m_dblMaxRotChildZ, True)
                    End If
                End Sub

                Protected Overridable Sub ManualMoveChild()
                    ManualMovePartAxis(m_strStruct1Name, "Root\Joint_1\Body_1", _
                                       m_strMoveChildWorldXAxis, m_strMoveChildLocalXAxis, _
                                       m_dblManWorldXPos, m_dblManWorldXTest, m_dblManChildWorldLocalXTest, _
                                       True, m_dblManLocalXPos, m_dblManLocalXTest, m_dblManChildLocalWorldXTest, 0.01)

                    ManualMovePartAxis(m_strStruct1Name, "Root\Joint_1\Body_1", _
                                       m_strMoveChildWorldYAxis, m_strMoveChildLocalYAxis, _
                                       m_dblManWorldYPos, m_dblManWorldYTest, m_dblManChildWorldLocalYTest, _
                                       True, m_dblManLocalYPos, m_dblManLocalYTest, m_dblManChildLocalWorldYTest, 0.01)

                    ManualMovePartAxis(m_strStruct1Name, "Root\Joint_1\Body_1", _
                                       m_strMoveChildWorldZAxis, m_strMoveChildLocalZAxis, _
                                       m_dblManWorldZPos, m_dblManWorldZTest, m_dblManChildWorldLocalZTest, _
                                       True, m_dblManLocalZPos, m_dblManLocalZTest, m_dblManChildLocalWorldZTest, 0.01)

                End Sub

                Protected Overridable Sub ManualRotateChild()
                    If Me.AllowChildRotations Then
                        ManualRotatePartAxis(m_strStruct1Name, "Root\Joint_1\Body_1", "X", m_dblManXRot, True, 0.001)
                        ManualRotatePartAxis(m_strStruct1Name, "Root\Joint_1\Body_1", "Y", m_dblManYRot, True, 0.001)
                        ManualRotatePartAxis(m_strStruct1Name, "Root\Joint_1\Body_1", "Z", m_dblManZRot, True, 0.001)
                    End If
                End Sub

                Protected Overridable Sub VerifyChildPositionAfterRotateStructure()
                    If Me.AllowRootRotations Then
                        'Verify the child part position after rotating the structure.
                        VerifyChildPosAfterRotate(m_strStruct1Name, "X", "Root\Joint_1\Body_1", m_dblRootChildRotation, m_dblStructChildRotateTestXX, m_dblStructChildRotateTestXY, m_dblStructChildRotateTestXZ)
                        VerifyChildPosAfterRotate(m_strStruct1Name, "Y", "Root\Joint_1\Body_1", m_dblRootChildRotation, m_dblStructChildRotateTestYX, m_dblStructChildRotateTestYY, m_dblStructChildRotateTestYZ)
                        VerifyChildPosAfterRotate(m_strStruct1Name, "Z", "Root\Joint_1\Body_1", m_dblRootChildRotation, m_dblStructChildRotateTestZX, m_dblStructChildRotateTestZY, m_dblStructChildRotateTestZZ)
                    End If
                End Sub

                Protected Overridable Sub VerifyJointPositionAfterRotateStructure()
                    If Me.AllowRootRotations Then
                        'Verify the joint position after rotating the structure.
                        VerifyChildPosAfterRotate(m_strStruct1Name, "X", "Root\Joint_1", m_dblRootChildRotation, m_dblStructJointRotateTestXX, m_dblStructJointRotateTestXY, m_dblStructJointRotateTestXZ)
                        VerifyChildPosAfterRotate(m_strStruct1Name, "Y", "Root\Joint_1", m_dblRootChildRotation, m_dblStructJointRotateTestYX, m_dblStructJointRotateTestYY, m_dblStructJointRotateTestYZ)
                        VerifyChildPosAfterRotate(m_strStruct1Name, "Z", "Root\Joint_1", m_dblRootChildRotation, m_dblStructJointRotateTestZX, m_dblStructJointRotateTestZY, m_dblStructJointRotateTestZZ)
                    End If
                End Sub

                Protected Overridable Sub RepositionStruct1BeforeSim()
                    'Move the structure up.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name, "WorldPosition.Y", "1"})
                End Sub

                Protected Overridable Sub RepositionStruct2BeforeSim()

                End Sub

                Protected Overridable Sub CreateStruct2AndRoot()

                    'Create a new structure
                    CreateStructure("Structures", "Structure_2", "Structure_2", m_bCreateStructure)

                    'Add a plane part.
                    AddRootPartType(m_strStructureGroup, "Structure_2", m_strStruct2RootPart)

                    RepositionStruct2BeforeSim()

                    'Select the chart tab page.
                    ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Tool Viewers\DataTool_1"}, 1000)

                    'Now add the new part to the chart.
                    'Now add items to the chart to plot the y position of the root of structure 2
                    AddItemToChart("Structure_2\Body Plan\Root")

                    'Set the name of the data chart item to root_y.
                    ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Root", "Name", "Root2_Y"})

                    'Change the data type to track the world Y position.
                    ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Root2_Y", "DataTypeID", "WorldPositionY"})

                    'Select the simulation window tab so it is visible now.
                    ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Simulation\Environment\Structures\Structure_2"}, 1000)

                    'Set it to track the root of structure 2.
                    ExecuteMethod("SelectTrackItems", New Object() {"Simulation\Environment\Structures\Structure_2", "Structure_2", "Root"})
                End Sub

                Protected Overridable Sub SimulateAndDeleteParts()

                    'Reposition the structure and child part before the simulation
                    RepositionStruct1BeforeSim()
                    RepositionChildPart()

                    'Run the simulation and wait for it to end.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, "BeforeStruct_")

                    CreateStruct2AndRoot()

                    'Run the simulation and wait for it to end.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, "AfterStruct_")

                    'Now lets remove the child body of the falling part.
                    DeletePart("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_1")

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
                    DeletePart("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root")

                    'Now lets remove structure 1, and that structure.
                    DeletePart("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name)

                    'Make sure simulation can still run.
                    RunSimulationWaitToEnd()

                End Sub

                Protected Overridable Sub TestMovableRigidBodyProperties(ByVal strStructure As String, ByVal strPart As String)
                    TestMovableItemProperties(strStructure, strPart)

                    If m_bTestDensity Then
                        'Set the density to a valid value.
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Density", m_dblTestDensity.ToString})

                        Dim dblTest As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Density.ActualValue"), Double)

                        If dblTest <> m_dblTestDensity Then
                            Throw New System.Exception("Body part density does not match the target value: " & m_dblTestDensity & ", recorded value: " & dblTest)
                        End If

                        'Check for error when density is zero.
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Density", "0"}, "The density can not be less than or equal to zero.")

                        'Check for error when density is less than zero.
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Density", "-1"}, "The density can not be less than or equal to zero.")
                    End If

                End Sub



                'Protected Overridable Sub TestLandscape()

                '    CreateAndTestRoot()
                '    CreateAndTestChild()

                '    'Reposition the structure
                '    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name, "WorldPosition.X", "0"})
                '    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name, "WorldPosition.Y", "0"})
                '    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name, "WorldPosition.Z", "0"})

                '    'Reposition the Child
                '    RepositionChildPart()

                '    CreateChartAndAddBodies(10)

                '    'Run the simulation and wait for it to end.
                '    RunSimulationWaitToEnd()

                '    'Compare chart data to verify simulation results.
                '    CompareSimulation(m_strRootFolder & m_strTestDataPath, "BeforeStruct_")


                '    'Create a new structure
                '    CreateStructure("Structure_2", "Structure_2")

                '    'Add a plane part.
                '    AddRootPartType("Plane")


                '    'Run the simulation and wait for it to end.
                '    RunSimulationWaitToEnd()

                '    'Compare chart data to verify simulation results.
                '    CompareSimulation(m_strRootFolder & m_strTestDataPath, "AfterStruct_")

                '    ''Now lets remove the child body of the falling part.
                '    'DeletePart("Simulation\Environment\" &m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_1")

                '    ''Run the simulation and wait for it to end.
                '    'RunSimulationWaitToEnd()

                '    ''Compare chart data to verify simulation results.
                '    'CompareSimulation(m_strRootFolder & m_strTestDataPath, "AfterS1Child_")

                '    ''Now lets remove the plane part of the second structure, and that structure.
                '    'DeletePart("Simulation\Environment\Structures\Structure_2\Body Plan\Root")

                '    ''Remove the structure
                '    'DeletePart("Simulation\Environment\Structures\Structure_2")

                '    ''Run the simulation and wait for it to end.
                '    'RunSimulationWaitToEnd()

                '    ''Compare chart data to verify simulation results.
                '    'CompareSimulation(m_strRootFolder & m_strTestDataPath, "AfterS2_")

                '    ''Now lets remove the root of the structure 1, and that structure.
                '    'DeletePart("Simulation\Environment\" &m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root")

                '    ''Now lets remove structure 1, and that structure.
                '    'DeletePart("Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name)

                '    ''Make sure simulation can still run.
                '    'RunSimulationWaitToEnd()

                'End Sub

#End Region

            End Class

        End Namespace
    End Namespace
End Namespace

