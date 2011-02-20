Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools.Framework

Namespace Forms
    Public Class GrasshopperConfig
        Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents txtConfigDir As System.Windows.Forms.TextBox
        Friend WithEvents btnConfigDir As System.Windows.Forms.Button
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents btnSemilunarJumpTest As System.Windows.Forms.Button
        Friend WithEvents btnKickTest As System.Windows.Forms.Button
        Friend WithEvents btnExtShort As System.Windows.Forms.Button
        Friend WithEvents btnJumpPower As System.Windows.Forms.Button
        Friend WithEvents btnAddPosturalControl As System.Windows.Forms.Button
        Friend WithEvents btnPitchAngles As System.Windows.Forms.Button
        Friend WithEvents btnTakeOffAngle As System.Windows.Forms.Button
        Friend WithEvents btnAbFlexGain As System.Windows.Forms.Button
        Friend WithEvents btnVaryBetaPitch As System.Windows.Forms.Button
        Friend WithEvents btnVaryAbFlex As System.Windows.Forms.Button
        Friend WithEvents btnExtStimvsTen As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.txtConfigDir = New System.Windows.Forms.TextBox
            Me.btnConfigDir = New System.Windows.Forms.Button
            Me.Label1 = New System.Windows.Forms.Label
            Me.btnSemilunarJumpTest = New System.Windows.Forms.Button
            Me.btnKickTest = New System.Windows.Forms.Button
            Me.btnExtShort = New System.Windows.Forms.Button
            Me.btnJumpPower = New System.Windows.Forms.Button
            Me.btnAddPosturalControl = New System.Windows.Forms.Button
            Me.btnPitchAngles = New System.Windows.Forms.Button
            Me.btnTakeOffAngle = New System.Windows.Forms.Button
            Me.btnAbFlexGain = New System.Windows.Forms.Button
            Me.btnVaryBetaPitch = New System.Windows.Forms.Button
            Me.btnVaryAbFlex = New System.Windows.Forms.Button
            Me.btnExtStimvsTen = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'txtConfigDir
            '
            Me.txtConfigDir.Location = New System.Drawing.Point(8, 24)
            Me.txtConfigDir.Name = "txtConfigDir"
            Me.txtConfigDir.Size = New System.Drawing.Size(248, 20)
            Me.txtConfigDir.TabIndex = 0
            Me.txtConfigDir.Text = "C:\Projects\bin\Experiments\Program Modules\AnimatLab Job Submitter\bin\SimFiles"
            '
            'btnConfigDir
            '
            Me.btnConfigDir.Location = New System.Drawing.Point(264, 26)
            Me.btnConfigDir.Name = "btnConfigDir"
            Me.btnConfigDir.Size = New System.Drawing.Size(24, 16)
            Me.btnConfigDir.TabIndex = 1
            Me.btnConfigDir.Text = "..."
            '
            'Label1
            '
            Me.Label1.Location = New System.Drawing.Point(8, 8)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(240, 16)
            Me.Label1.TabIndex = 2
            Me.Label1.Text = "Configuration File Directory"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'btnSemilunarJumpTest
            '
            Me.btnSemilunarJumpTest.Location = New System.Drawing.Point(8, 56)
            Me.btnSemilunarJumpTest.Name = "btnSemilunarJumpTest"
            Me.btnSemilunarJumpTest.Size = New System.Drawing.Size(208, 24)
            Me.btnSemilunarJumpTest.TabIndex = 3
            Me.btnSemilunarJumpTest.Text = "Semilunar Jump Test"
            '
            'btnKickTest
            '
            Me.btnKickTest.Location = New System.Drawing.Point(8, 96)
            Me.btnKickTest.Name = "btnKickTest"
            Me.btnKickTest.Size = New System.Drawing.Size(208, 24)
            Me.btnKickTest.TabIndex = 4
            Me.btnKickTest.Text = "Kicking Test"
            '
            'btnExtShort
            '
            Me.btnExtShort.Location = New System.Drawing.Point(8, 128)
            Me.btnExtShort.Name = "btnExtShort"
            Me.btnExtShort.Size = New System.Drawing.Size(208, 24)
            Me.btnExtShort.TabIndex = 5
            Me.btnExtShort.Text = "Extensor Apodem Shortening Test"
            '
            'btnJumpPower
            '
            Me.btnJumpPower.Location = New System.Drawing.Point(8, 160)
            Me.btnJumpPower.Name = "btnJumpPower"
            Me.btnJumpPower.Size = New System.Drawing.Size(208, 24)
            Me.btnJumpPower.TabIndex = 6
            Me.btnJumpPower.Text = "Jump Power"
            '
            'btnAddPosturalControl
            '
            Me.btnAddPosturalControl.Location = New System.Drawing.Point(8, 224)
            Me.btnAddPosturalControl.Name = "btnAddPosturalControl"
            Me.btnAddPosturalControl.Size = New System.Drawing.Size(208, 24)
            Me.btnAddPosturalControl.TabIndex = 7
            Me.btnAddPosturalControl.Text = "Add Postural Control"
            '
            'btnPitchAngles
            '
            Me.btnPitchAngles.Location = New System.Drawing.Point(8, 264)
            Me.btnPitchAngles.Name = "btnPitchAngles"
            Me.btnPitchAngles.Size = New System.Drawing.Size(208, 24)
            Me.btnPitchAngles.TabIndex = 8
            Me.btnPitchAngles.Text = "Pitch Angle During Jump"
            '
            'btnTakeOffAngle
            '
            Me.btnTakeOffAngle.Location = New System.Drawing.Point(8, 296)
            Me.btnTakeOffAngle.Name = "btnTakeOffAngle"
            Me.btnTakeOffAngle.Size = New System.Drawing.Size(208, 24)
            Me.btnTakeOffAngle.TabIndex = 9
            Me.btnTakeOffAngle.Text = "Take Off Angle"
            '
            'btnAbFlexGain
            '
            Me.btnAbFlexGain.Location = New System.Drawing.Point(8, 328)
            Me.btnAbFlexGain.Name = "btnAbFlexGain"
            Me.btnAbFlexGain.Size = New System.Drawing.Size(208, 24)
            Me.btnAbFlexGain.TabIndex = 10
            Me.btnAbFlexGain.Text = "Ab Flex Gain"
            '
            'btnVaryBetaPitch
            '
            Me.btnVaryBetaPitch.Location = New System.Drawing.Point(8, 360)
            Me.btnVaryBetaPitch.Name = "btnVaryBetaPitch"
            Me.btnVaryBetaPitch.Size = New System.Drawing.Size(208, 24)
            Me.btnVaryBetaPitch.TabIndex = 11
            Me.btnVaryBetaPitch.Text = "Vary Beta-Pitch"
            '
            'btnVaryAbFlex
            '
            Me.btnVaryAbFlex.Location = New System.Drawing.Point(8, 392)
            Me.btnVaryAbFlex.Name = "btnVaryAbFlex"
            Me.btnVaryAbFlex.Size = New System.Drawing.Size(208, 24)
            Me.btnVaryAbFlex.TabIndex = 12
            Me.btnVaryAbFlex.Text = "Vary AbFlex"
            '
            'btnExtStimvsTen
            '
            Me.btnExtStimvsTen.Location = New System.Drawing.Point(232, 56)
            Me.btnExtStimvsTen.Name = "btnExtStimvsTen"
            Me.btnExtStimvsTen.Size = New System.Drawing.Size(208, 24)
            Me.btnExtStimvsTen.TabIndex = 13
            Me.btnExtStimvsTen.Text = "Extensor Stim vs Tension"
            '
            'GrasshopperConfig
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(552, 454)
            Me.Controls.Add(Me.btnExtStimvsTen)
            Me.Controls.Add(Me.btnVaryAbFlex)
            Me.Controls.Add(Me.btnVaryBetaPitch)
            Me.Controls.Add(Me.btnAbFlexGain)
            Me.Controls.Add(Me.btnTakeOffAngle)
            Me.Controls.Add(Me.btnPitchAngles)
            Me.Controls.Add(Me.btnAddPosturalControl)
            Me.Controls.Add(Me.btnJumpPower)
            Me.Controls.Add(Me.btnExtShort)
            Me.Controls.Add(Me.btnKickTest)
            Me.Controls.Add(Me.btnSemilunarJumpTest)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.btnConfigDir)
            Me.Controls.Add(Me.txtConfigDir)
            Me.Name = "GrasshopperConfig"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "GrasshopperConfig"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Methods "

        Private Sub VaryExtensorStrength()

            Try
                If txtConfigDir.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify an output directory for the config files.")
                End If

                Dim strPath As String = txtConfigDir.Text
                Dim strFileName As String = ""

                'Lets get a reference to needed objects.
                Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)
                Dim doLeftSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)
                Dim doRightSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)

                Dim doLeftSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)
                Dim doRightSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)

                Dim doLeftTibiaExtensor As VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle = DirectCast(doGrasshopper.FindBodyPartByName("Left Tibia Extensor"), VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle)
                Dim doRightTibiaExtensor As VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle = DirectCast(doGrasshopper.FindBodyPartByName("Right Tibia Extensor"), VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle)

                Dim doLeftStim As AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent = DirectCast(AnimatTools.Framework.Util.Application.ProjectStimuli.FindDataObjectByName("Left Extensor 2a"), AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent)
                Dim doRightStim As AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent = DirectCast(AnimatTools.Framework.Util.Application.ProjectStimuli.FindDataObjectByName("Right Extensor 2a"), AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent)


                'First set things up to do the normal jumps.
                doLeftSemilunar.Enabled = True
                doRightSemilunar.Enabled = True
                doLeftSliderJoint.EnableMotor = False
                doRightSliderJoint.EnableMotor = False
                Dim bAutoSeed As Boolean = AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed
                AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed = False

                Dim rndNum As System.Random = New System.Random
                Dim aryRandNums As New ArrayList
                For iNum As Integer = 1 To 20
                    aryRandNums.Add(CInt(AnimatTools.Framework.Util.Rand(100, 10000, rndNum)))
                Next

                'Dim aryRandNums() As Integer = {4876, 8090, 8851, 4153, 1342, 116, 1684, 3510, 2206, 8127, 7335, 2415, 2569, 9362, 5357, 1966, 9423, 3288, 9919, 4586}
                'Dim aryLeftWithStims() As Double = {19, 28.5, 35, 41, 46}
                'Dim aryRightWithStims() As Double = {19, 28.5, 35, 41, 46}
                'Dim aryLeftWithoutStims() As Double = {26, 33, 39, 43, 48}
                'Dim aryRightWithoutStims() As Double = {26.5, 34, 39, 43, 48}
                Dim aryMaxTen() As Double = {15, 13, 11, 9, 7}
                Dim dblMaxTen As Double = doLeftTibiaExtensor.MaxTension.ActualValue

                AnimatTools.Framework.Util.ExportForStandAloneSim = True
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = True
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = True

                For iTension As Integer = 1 To 5
                    For iTrial As Integer = 1 To 20
                        AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(aryRandNums(iTrial - 1))
                        doLeftTibiaExtensor.MaxTension.ActualValue = aryMaxTen(iTension - 1)
                        doRightTibiaExtensor.MaxTension.ActualValue = aryMaxTen(iTension - 1)

                        strFileName = strPath & "\SLP_With_" & iTension & "_" & iTrial & ".asim"
                        AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                    Next
                Next

                'Now disable the semilunar springs.
                doLeftSemilunar.Enabled = False
                doRightSemilunar.Enabled = False
                doLeftSliderJoint.EnableMotor = True
                doRightSliderJoint.EnableMotor = True

                For iTension As Integer = 1 To 5
                    For iTrial As Integer = 1 To 20
                        AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(aryRandNums(iTrial - 1))
                        doLeftTibiaExtensor.MaxTension.ActualValue = aryMaxTen(iTension - 1)
                        doRightTibiaExtensor.MaxTension.ActualValue = aryMaxTen(iTension - 1)

                        strFileName = strPath & "\SLP_Without_" & iTension & "_" & iTrial & ".asim"
                        AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                    Next
                Next

                'Set everything back for normal jumps.
                doLeftSemilunar.Enabled = True
                doRightSemilunar.Enabled = True
                doLeftSliderJoint.EnableMotor = False
                doRightSliderJoint.EnableMotor = False
                doLeftTibiaExtensor.MaxTension.ActualValue = dblMaxTen
                doRightTibiaExtensor.MaxTension.ActualValue = dblMaxTen

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = False
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = False
            End Try
        End Sub

        'Keep the jump hieght the same for each of the extensor strength values with and without semilunar
        Private Sub VaryExtensorStrength2()

            Try
                If txtConfigDir.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify an output directory for the config files.")
                End If

                Dim strPath As String = txtConfigDir.Text
                Dim strFileName As String = ""

                'Lets get a reference to needed objects.
                Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)
                Dim doLeftSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)
                Dim doRightSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)

                Dim doLeftSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)
                Dim doRightSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)

                Dim doLeftTibiaExtensor As VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle = DirectCast(doGrasshopper.FindBodyPartByName("Left Tibia Extensor"), VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle)
                Dim doRightTibiaExtensor As VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle = DirectCast(doGrasshopper.FindBodyPartByName("Right Tibia Extensor"), VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle)

                Dim doLeftStim As AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent = DirectCast(AnimatTools.Framework.Util.Application.ProjectStimuli.FindDataObjectByName("Left Extensor 2a"), AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent)
                Dim doRightStim As AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent = DirectCast(AnimatTools.Framework.Util.Application.ProjectStimuli.FindDataObjectByName("Right Extensor 2a"), AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent)


                'First set things up to do the normal jumps.
                doLeftSemilunar.Enabled = True
                doRightSemilunar.Enabled = True
                doLeftSliderJoint.EnableMotor = False
                doRightSliderJoint.EnableMotor = False
                Dim bAutoSeed As Boolean = AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed
                AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed = False

                'Dim rndNum As System.Random = New System.Random
                'Dim aryRandNums As New ArrayList
                'For iNum As Integer = 1 To 20
                'aryRandNums.Add(CInt(AnimatTools.Framework.Util.Rand(100, 10000, rndNum)))
                'Next

                Dim aryRandNums() As Integer = {4876, 8090, 8851, 4153, 1342, 116, 1684, 3510, 2206, 8127, 7335, 2415, 2569, 9362, 5357, 1966, 9423, 3288, 9919, 4586}
                Dim aryLeftWithStims() As Double = {19, 28.5, 35, 41, 46}
                Dim aryRightWithStims() As Double = {19, 28.5, 35, 41, 46}
                Dim aryLeftWithoutStims() As Double = {26, 33, 39, 43, 48}
                Dim aryRightWithoutStims() As Double = {26.5, 34, 39, 43, 48}

                AnimatTools.Framework.Util.ExportForStandAloneSim = True
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = True
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = True

                For iTension As Integer = 1 To 5
                    For iTrial As Integer = 1 To 20
                        AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(aryRandNums(iTrial - 1))
                        doLeftStim.CycleOffDuration.ActualValue = aryLeftWithStims(iTension - 1) * 0.001
                        doRightStim.CycleOffDuration.ActualValue = aryRightWithStims(iTension - 1) * 0.001

                        strFileName = strPath & "\SLP_With_" & iTension & "_" & iTrial & ".asim"
                        AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                    Next
                Next

                'Now disable the semilunar springs.
                doLeftSemilunar.Enabled = False
                doRightSemilunar.Enabled = False
                doLeftSliderJoint.EnableMotor = True
                doRightSliderJoint.EnableMotor = True

                For iTension As Integer = 1 To 5
                    For iTrial As Integer = 1 To 20
                        AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(aryRandNums(iTrial - 1))
                        doLeftStim.CycleOffDuration.ActualValue = aryLeftWithoutStims(iTension - 1) * 0.001
                        doRightStim.CycleOffDuration.ActualValue = aryRightWithoutStims(iTension - 1) * 0.001

                        strFileName = strPath & "\SLP_Without_" & iTension & "_" & iTrial & ".asim"
                        AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                    Next
                Next

                'Set everything back for normal jumps.
                doLeftSemilunar.Enabled = True
                doRightSemilunar.Enabled = True
                doLeftSliderJoint.EnableMotor = False
                doRightSliderJoint.EnableMotor = False

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = False
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = False
            End Try
        End Sub

        Private Sub VaryKse()

            Try
                If txtConfigDir.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify an output directory for the config files.")
                End If

                Dim strPath As String = txtConfigDir.Text
                Dim strFileName As String = ""

                'Lets get a reference to needed objects.
                Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)
                Dim doLeftSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)
                Dim doRightSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)

                Dim doLeftSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)
                Dim doRightSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)

                Dim doLeftTibiaExtensor As VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle = DirectCast(doGrasshopper.FindBodyPartByName("Left Tibia Extensor"), VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle)
                Dim doRightTibiaExtensor As VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle = DirectCast(doGrasshopper.FindBodyPartByName("Right Tibia Extensor"), VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle)

                'First set things up to do the normal jumps.
                doLeftSemilunar.Enabled = True
                doRightSemilunar.Enabled = True
                doLeftSliderJoint.EnableMotor = False
                doRightSliderJoint.EnableMotor = False
                doLeftTibiaExtensor.StimTension.Amplitude.ActualValue = 20
                doRightTibiaExtensor.StimTension.Amplitude.ActualValue = 20
                doLeftTibiaExtensor.Kse.ActualValue = 10000
                doRightTibiaExtensor.Kse.ActualValue = 10000

                Dim rndNum As System.Random = New System.Random
                Dim dblOriginalY As Double = doGrasshopper.YLocationScaled.ActualValue

                AnimatTools.Framework.Util.ExportForStandAloneSim = True

                For iSE As Integer = 10000 To 80000 Step 10000
                    doLeftTibiaExtensor.Kse.ActualValue = iSE
                    doRightTibiaExtensor.Kse.ActualValue = iSE

                    For iNum As Integer = 1 To 20
                        doGrasshopper.YLocation = AnimatTools.Framework.Util.Rand(2.4, 2.7, rndNum)
                        strFileName = strPath & "\SY_F15_SE" & CInt(iSE / 1000) & "K_N" & iNum & ".asim"
                        AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                    Next
                Next

                'Now disable the semilunar springs.
                doLeftSemilunar.Enabled = False
                doRightSemilunar.Enabled = False
                doLeftSliderJoint.EnableMotor = True
                doRightSliderJoint.EnableMotor = True
                doLeftTibiaExtensor.StimTension.Amplitude.ActualValue = 18.5
                doRightTibiaExtensor.StimTension.Amplitude.ActualValue = 18.5
                doLeftTibiaExtensor.Kse.ActualValue = 40000
                doRightTibiaExtensor.Kse.ActualValue = 40000

                AnimatTools.Framework.Util.ExportForStandAloneSim = True

                For iSE As Integer = 10000 To 80000 Step 10000
                    doLeftTibiaExtensor.Kse.ActualValue = iSE
                    doRightTibiaExtensor.Kse.ActualValue = iSE

                    For iNum As Integer = 1 To 20
                        doGrasshopper.YLocation = AnimatTools.Framework.Util.Rand(2.4, 2.7, rndNum)
                        strFileName = strPath & "\SN_F15_SE" & CInt(iSE / 1000) & "K_N" & iNum & ".asim"
                        AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                    Next
                Next

                'Set everything back for normal jumps.
                doLeftSemilunar.Enabled = True
                doRightSemilunar.Enabled = True
                doLeftSliderJoint.EnableMotor = False
                doRightSliderJoint.EnableMotor = False
                doLeftTibiaExtensor.StimTension.Amplitude.ActualValue = 20
                doRightTibiaExtensor.StimTension.Amplitude.ActualValue = 20
                doLeftTibiaExtensor.Kse.ActualValue = 40000
                doRightTibiaExtensor.Kse.ActualValue = 40000
                doGrasshopper.YLocationScaled.ActualValue = 2.5

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
            End Try
        End Sub

        'Keep the jump hieght the same for each of the extensor strength values with and without semilunar
        Private Sub KickTest()

            Try
                If txtConfigDir.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify an output directory for the config files.")
                End If

                Dim strPath As String = txtConfigDir.Text
                Dim strFileName As String = ""

                'Lets get a reference to needed objects.
                Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)

                Dim doLeftStim As AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent = DirectCast(AnimatTools.Framework.Util.Application.ProjectStimuli.FindDataObjectByName("Left Extensor 2a"), AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent)
                Dim doRightStim As AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent = DirectCast(AnimatTools.Framework.Util.Application.ProjectStimuli.FindDataObjectByName("Right Extensor 2a"), AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent)
                Dim doPosture As DataObjects.ExternalStimuli.PostureControl = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Locust Posture Control"), DataObjects.ExternalStimuli.PostureControl)

                Dim doRoot As VortexAnimatTools.DataObjects.Physical.RigidBodies.Box = DirectCast(doGrasshopper.FindBodyPartByName("Root"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Box)

                Dim doLeftTC As VortexAnimatTools.DataObjects.Physical.Joints.Hinge = DirectCast(doGrasshopper.FindBodyPartByName("Left Rear Thoracic Coxa Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Hinge)
                Dim doLeftCF As VortexAnimatTools.DataObjects.Physical.Joints.Hinge = DirectCast(doGrasshopper.FindBodyPartByName("Left Rear Coxa Femur"), VortexAnimatTools.DataObjects.Physical.Joints.Hinge)
                Dim doRightTC As VortexAnimatTools.DataObjects.Physical.Joints.Hinge = DirectCast(doGrasshopper.FindBodyPartByName("Right Rear Thoracic Coxa Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Hinge)
                Dim doRightCF As VortexAnimatTools.DataObjects.Physical.Joints.Hinge = DirectCast(doGrasshopper.FindBodyPartByName("Right Rear Coxa Femur"), VortexAnimatTools.DataObjects.Physical.Joints.Hinge)

                Dim doLeftSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)
                Dim doRightSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)

                Dim doLeftSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)
                Dim doRightSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)

                Dim dblIncrement As Double = 1
                Dim iSimTimeStart As Integer = 10
                Dim iSimTimeEnd As Integer = 23
                Dim iTrials As Integer = 20
                Dim iSims As Integer = CInt((iSimTimeEnd - iSimTimeStart) / dblIncrement) + 1

                'First set things up to do the normal jumps.
                Dim bAutoSeed As Boolean = AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed
                Dim dblLDoff As Double = doLeftStim.CycleOffDuration.Value
                Dim dblRDoff As Double = doRightStim.CycleOffDuration.Value
                Dim iSeed As Integer = AnimatTools.Framework.Util.Environment.ManualRandomSeed
                AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed = False

                doLeftSemilunar.Enabled = True
                doRightSemilunar.Enabled = True
                doLeftSliderJoint.EnableMotor = False
                doRightSliderJoint.EnableMotor = False

                doRoot.XRotationScaled.Value = 180
                doRoot.Freeze = True
                doLeftTC.EnableMotor = True
                doLeftCF.EnableMotor = True
                doRightTC.EnableMotor = True
                doRightCF.EnableMotor = True
                doPosture.Enabled = False

                AnimatTools.Framework.Util.ExportForStandAloneSim = True
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = True
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = True

                Dim rndNum As System.Random = New System.Random
                Dim aryRandNums As New ArrayList
                For iNum As Integer = 1 To (iSims * iTrials)
                    aryRandNums.Add(413 + iNum)
                Next

                Dim dblStim As Double = iSimTimeStart
                Dim idxRandNum As Integer = 0
                For iStimStep As Integer = 1 To iSims
                    For iTrial As Integer = 1 To iTrials
                        AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(aryRandNums(idxRandNum))
                        doLeftStim.CycleOffDuration.Value = dblStim
                        doRightStim.CycleOffDuration.Value = dblStim

                        strFileName = strPath & "\KickTest_" & CInt(dblStim * 10) & "_Trial_" & (iTrial) & ".asim"
                        AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                        idxRandNum = idxRandNum + 1
                    Next

                    dblStim = dblStim + dblIncrement
                Next

                'Set everything back for normal jumps.
                doLeftStim.CycleOffDuration.Value = dblLDoff
                doRightStim.CycleOffDuration.Value = dblRDoff
                AnimatTools.Framework.Util.Environment.ManualRandomSeed = iSeed

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = False
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = False
            End Try
        End Sub


        Private Sub btnExtShort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtShort.Click

            Try
                If txtConfigDir.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify an output directory for the config files.")
                End If

                Dim strPath As String = txtConfigDir.Text
                Dim strFileName As String = ""

                'Lets get a reference to needed objects.
                Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)
                Dim doLeftSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)
                Dim doRightSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)

                Dim doLeftSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)
                Dim doRightSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)

                Dim doLeftTibiaExtensor As VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle = DirectCast(doGrasshopper.FindBodyPartByName("Left Tibia Extensor"), VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle)
                Dim doRightTibiaExtensor As VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle = DirectCast(doGrasshopper.FindBodyPartByName("Right Tibia Extensor"), VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle)

                'First set things up to do the normal jumps.
                doLeftSemilunar.Enabled = True
                doRightSemilunar.Enabled = True
                doLeftSliderJoint.EnableMotor = False
                doRightSliderJoint.EnableMotor = False

                Dim rndNum As System.Random = New System.Random
                Dim dblOriginalY As Double = doGrasshopper.YLocationScaled.ActualValue

                AnimatTools.Framework.Util.ExportForStandAloneSim = True

                Dim aryHeights As New ArrayList
                For iNum As Integer = 1 To 10
                    aryHeights.Add(AnimatTools.Framework.Util.Rand(2.4, 2.7, rndNum))
                Next

                For iNum As Integer = 1 To 10
                    doGrasshopper.YLocation = CType(aryHeights(iNum - 1), Double)

                    For iForce As Integer = 15 To 80 Step 5
                        doLeftSemilunar.Stiffness.ActualValue = iForce * 1000
                        doRightSemilunar.Stiffness.ActualValue = iForce * 1000

                        strFileName = strPath & "\ExtApShort_SLP_K" & iForce & "_" & iNum & ".asim"
                        AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                    Next
                Next

                'Set everything back for normal jumps.
                doLeftSemilunar.Enabled = True
                doRightSemilunar.Enabled = True
                doLeftSemilunar.Stiffness.ActualValue = 28000
                doRightSemilunar.Stiffness.ActualValue = 28000
                doGrasshopper.YLocationScaled.ActualValue = 2.5

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
            End Try
        End Sub

#End Region

        Private Sub btnSemilunarJumpTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSemilunarJumpTest.Click

            Try
                VaryExtensorStrength()
                'VaryExtensorStrength2()
                'VaryKse()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
            End Try
        End Sub

        Private Sub btnConfigDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfigDir.Click

            Try
                Dim openFolderDialog As New System.Windows.Forms.FolderBrowserDialog
                openFolderDialog.Description = "Specify the drive location where the  project directory will be created."
                openFolderDialog.ShowNewFolderButton = True

                If openFolderDialog.ShowDialog() = DialogResult.OK Then
                    txtConfigDir.Text = openFolderDialog.SelectedPath
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnKickTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKickTest.Click

            Try
                KickTest()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
            End Try
        End Sub


        Private Sub btnJumpPower_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnJumpPower.Click


            Try
                If txtConfigDir.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify an output directory for the config files.")
                End If

                Dim strPath As String = txtConfigDir.Text
                Dim strFileName As String = ""

                'Lets get a reference to needed objects.
                Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)
                Dim doLeftSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)
                Dim doRightSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)

                Dim doLeftSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)
                Dim doRightSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)

                Dim doLeftTibiaExtensor As VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle = DirectCast(doGrasshopper.FindBodyPartByName("Left Tibia Extensor"), VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle)
                Dim doRightTibiaExtensor As VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle = DirectCast(doGrasshopper.FindBodyPartByName("Right Tibia Extensor"), VortexAnimatTools.DataObjects.Physical.RigidBodies.LinearHillMuscle)

                '                Dim doLeftSLPStim As AnimatTools.DataObjects.ExternalStimuli.MotorVelocity = DirectCast(AnimatTools.Framework.Util.Application.ProjectStimuli.FindDataObjectByName("Left SLP Position"), AnimatTools.DataObjects.ExternalStimuli.MotorVelocity)
                '                Dim doRightSLPStim As AnimatTools.DataObjects.ExternalStimuli.MotorVelocity = DirectCast(AnimatTools.Framework.Util.Application.ProjectStimuli.FindDataObjectByName("Right SLP Position"), AnimatTools.DataObjects.ExternalStimuli.MotorVelocity)

                'First set things up to do the normal jumps.
                doLeftSemilunar.Enabled = True
                doRightSemilunar.Enabled = True
                doLeftSliderJoint.EnableMotor = False
                doRightSliderJoint.EnableMotor = False
                AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed = False
                Dim dblMaxTen As Double = doLeftTibiaExtensor.MaxTension.ActualValue
                doLeftTibiaExtensor.MaxTension.ActualValue = 8
                doRightTibiaExtensor.MaxTension.ActualValue = 8

                Dim rndNum As System.Random = New System.Random
                Dim dblOriginalY As Double = doGrasshopper.YLocationScaled.ActualValue

                AnimatTools.Framework.Util.ExportForStandAloneSim = True
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = True
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = True

                Dim aryRandNums As New ArrayList
                For iNum As Integer = 1 To 25
                    aryRandNums.Add(CInt(AnimatTools.Framework.Util.Rand(100, 10000, rndNum)))
                Next

                For iNum As Integer = 1 To 25
                    AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(aryRandNums(iNum - 1))

                    strFileName = strPath & "\JumpPower" & "_With" & iNum & ".asim"
                    AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                Next

                'Now Disable the SLP.
                doLeftSemilunar.Enabled = False
                doRightSemilunar.Enabled = False
                doLeftSliderJoint.EnableMotor = True
                doRightSliderJoint.EnableMotor = True

                For iNum As Integer = 1 To 25
                    AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(aryRandNums(iNum - 1))

                    strFileName = strPath & "\JumpPower" & "_Without" & iNum & ".asim"
                    AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                Next

                'Set everything back for normal jumps.
                doLeftSemilunar.Enabled = True
                doRightSemilunar.Enabled = True
                doLeftSliderJoint.EnableMotor = False
                doRightSliderJoint.EnableMotor = False
                doLeftTibiaExtensor.MaxTension.ActualValue = dblMaxTen
                doRightTibiaExtensor.MaxTension.ActualValue = dblMaxTen

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = False
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = False
            End Try
        End Sub

        Private Sub btnAddPosturalControl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPosturalControl.Click
            Try

                Dim doStim As New DataObjects.ExternalStimuli.PostureControl(Util.Application.FormHelper)

                Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)

                doStim.Organism = doGrasshopper

                doStim.Name = "Locust Posture Control"

                Util.Application.ProjectStimuli.Add(doStim.ID, doStim)
                doStim.CreateWorkspaceTreeView(Util.Simulation, Util.Application.ProjectWorkspace)

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
            End Try
        End Sub

        Private Sub btnPitchAngles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPitchAngles.Click

            Try
                If txtConfigDir.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify an output directory for the config files.")
                End If

                Dim strPath As String = txtConfigDir.Text
                Dim strFileName As String = ""

                AnimatTools.Framework.Util.ExportForStandAloneSim = True
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = True
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = True

                'Lets get a reference to needed objects.
                Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)
                Dim doStim As DataObjects.ExternalStimuli.PostureControl = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Locust Posture Control"), DataObjects.ExternalStimuli.PostureControl)

                'doStim.Beta.ActualValue = 30
                'doStim.Delta.ActualValue = -15
                'For fltPitch As Double = -1.45 To -1.4 Step 0.01
                '    doStim.Pitch.ActualValue = fltPitch

                '    strFileName = strPath & "\PitchAdj_30_" & CInt(fltPitch * 100) & ".asim"
                '    AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                'Next

                doStim.Beta.ActualValue = 50
                doStim.Delta.ActualValue = 0
                For fltPitch As Double = 0.25 To 0.3 Step 0.01
                    doStim.Pitch.ActualValue = fltPitch

                    strFileName = strPath & "\PitchAdj_50_" & CInt(fltPitch * 100) & ".asim"
                    AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                Next

                'doStim.Beta.ActualValue = 70
                'doStim.Delta.ActualValue = 0
                'For fltPitch As Double = 9.6 To 10.6 Step 0.2
                '    doStim.Pitch.ActualValue = fltPitch

                '    strFileName = strPath & "\PitchAdj_70_" & CInt(fltPitch * 10) & ".asim"
                '    AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                'Next

                doStim.Beta.ActualValue = 90
                doStim.Delta.ActualValue = 0
                For fltPitch As Double = 14 To 15 Step 0.1
                    doStim.Pitch.ActualValue = fltPitch

                    strFileName = strPath & "\PitchAdj_90_" & CInt(fltPitch * 10) & ".asim"
                    AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                Next

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = False
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = False
            End Try
        End Sub

        Private Sub btnTakeOffAngle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTakeOffAngle.Click

            Try
                If txtConfigDir.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify an output directory for the config files.")
                End If

                Dim strPath As String = txtConfigDir.Text
                Dim strFileName As String = ""

                AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed = False
                AnimatTools.Framework.Util.ExportForStandAloneSim = True
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = True
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = True

                'Lets get a reference to needed objects.
                Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)
                Dim doStim As DataObjects.ExternalStimuli.PostureControl = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Locust Posture Control"), DataObjects.ExternalStimuli.PostureControl)

                doStim.Beta.ActualValue = 30
                doStim.Delta.ActualValue = -15
                doStim.Pitch.ActualValue = -0.1
                For iCount As Integer = 1 To 20
                    strFileName = strPath & "\TakeoffAngle_30_" & iCount & ".asim"
                    AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(AnimatTools.Framework.Util.Rand(100, 10000))
                    AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                Next

                doStim.Beta.ActualValue = 50
                doStim.Delta.ActualValue = 0
                doStim.Pitch.ActualValue = 0.5
                For iCount As Integer = 1 To 20
                    strFileName = strPath & "\TakeoffAngle_50_" & iCount & ".asim"
                    AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(AnimatTools.Framework.Util.Rand(100, 10000))
                    AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                Next

                doStim.Beta.ActualValue = 70
                doStim.Delta.ActualValue = 0
                doStim.Pitch.ActualValue = 10.2
                For iCount As Integer = 1 To 20
                    strFileName = strPath & "\TakeoffAngle_70_" & iCount & ".asim"
                    AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(AnimatTools.Framework.Util.Rand(100, 10000))
                    AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                Next

                doStim.Beta.ActualValue = 90
                doStim.Delta.ActualValue = 0
                doStim.Pitch.ActualValue = 14
                For iCount As Integer = 1 To 20
                    strFileName = strPath & "\TakeoffAngle_90_" & iCount & ".asim"
                    AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(AnimatTools.Framework.Util.Rand(100, 10000))
                    AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                Next

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = False
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = False
            End Try
        End Sub

        Private Sub btnAbFlexGain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbFlexGain.Click

            Try
                If txtConfigDir.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify an output directory for the config files.")
                End If

                Dim strPath As String = txtConfigDir.Text
                Dim strFileName As String = ""

                AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed = False
                AnimatTools.Framework.Util.ExportForStandAloneSim = True
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = True
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = True

                'Lets get a reference to needed objects.
                Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)
                Dim doStim As DataObjects.ExternalStimuli.PostureControl = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Locust Posture Control"), DataObjects.ExternalStimuli.PostureControl)
                Dim doAbFlexStim As AnimatTools.DataObjects.ExternalStimuli.TonicCurrent = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("D_Ab_Flex"), AnimatTools.DataObjects.ExternalStimuli.TonicCurrent)

                Dim fltStartTime As Single = 1.454 'Harcoded start time for the one I am testing. Need to change for specific simulatons
                Dim fltEndTime As Single = 1.4808 'Harcoded start time for the one I am testing. Need to change for specific simulatons
                Dim dblOrigBeta As Double = doStim.Beta.ActualValue
                Dim dblOrigPitch As Double = doStim.Pitch.ActualValue
                Dim dblOrigStart As Double = doAbFlexStim.StartTime.ActualValue
                Dim dblOrigEnd As Double = doAbFlexStim.EndTime.ActualValue
                Dim dblOrigMag As Double = doAbFlexStim.CurrentOn.ActualValue
                Dim bAbFlexStim As Boolean = doAbFlexStim.Enabled
                Dim dblOrigGain As Double = doStim.AbPropGain.ActualValue
                Dim iMag As Integer = 15
                For iDelay As Single = -13 To -12 Step 0.1
                    'For iMag As Integer = 0 To 10 Step 2
                    doAbFlexStim.StartTime.ActualValue = (fltStartTime + (iDelay * 0.001))
                    doAbFlexStim.EndTime.ActualValue = fltEndTime
                    doAbFlexStim.CurrentOn.ActualValue = (iMag * 0.000000001)

                    strFileName = strPath & "\AbFlex_Constant_012609_" & CInt(iDelay * 10) & "_" & iMag & ".asim"
                    AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                    'Next
                Next
                doStim.AbPropGain.ActualValue = dblOrigGain
                doStim.Beta.ActualValue = dblOrigBeta
                doStim.Pitch.ActualValue = dblOrigPitch
                doAbFlexStim.StartTime.ActualValue = dblOrigStart
                doAbFlexStim.EndTime.ActualValue = dblOrigEnd
                doAbFlexStim.CurrentOn.ActualValue = dblOrigMag
                doStim.AbPropGain.ActualValue = dblOrigGain
                doAbFlexStim.Enabled = bAbFlexStim

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = False
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = False
            End Try

        End Sub


        Private Sub btnVaryBetaPitch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVaryBetaPitch.Click

            Try
                If txtConfigDir.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify an output directory for the config files.")
                End If

                Dim strPath As String = txtConfigDir.Text
                Dim strFileName As String = ""

                AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed = False
                AnimatTools.Framework.Util.ExportForStandAloneSim = True
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = True
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = True

                'Lets get a reference to needed objects.
                Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)
                Dim doPostureStim As DataObjects.ExternalStimuli.PostureControl = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Locust Posture Control"), DataObjects.ExternalStimuli.PostureControl)
                Dim doAbFlexStim As AnimatTools.DataObjects.ExternalStimuli.TonicCurrent = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Posture Ab Raise"), AnimatTools.DataObjects.ExternalStimuli.TonicCurrent)

                Dim dblOrigBeta As Double = doPostureStim.Beta.ActualValue
                Dim dblOrigPitch As Double = doPostureStim.Pitch.ActualValue
                Dim bAbFlexStim As Boolean = doAbFlexStim.Enabled
                For iBeta As Integer = 20 To 40 Step 2
                    For fltPitch As Double = -4 To 16 Step 2
                        doPostureStim.Beta.ActualValue = iBeta
                        doPostureStim.Pitch.ActualValue = fltPitch

                        'if the pitch is greater than 10 then we need to flex the ab a bit so the feet touch the ground.
                        'If fltPitch >= 10 Then
                        doAbFlexStim.Enabled = True
                        'Else
                        '    doAbFlexStim.Enabled = False
                        'End If

                        strFileName = strPath & "\BetaPitch_013109_" & iBeta & "_" & CInt(fltPitch) & ".asim"
                        AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                    Next
                Next
                doPostureStim.Beta.ActualValue = dblOrigBeta
                doPostureStim.Pitch.ActualValue = dblOrigPitch
                doAbFlexStim.Enabled = bAbFlexStim

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = False
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = False
            End Try

        End Sub

        Private Sub btnVaryAbFlex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVaryAbFlex.Click

            Try
                If txtConfigDir.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify an output directory for the config files.")
                End If

                Dim strPath As String = txtConfigDir.Text
                Dim strFileName As String = ""

                AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed = False
                AnimatTools.Framework.Util.ExportForStandAloneSim = True
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = True
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = True

                'Lets get a reference to needed objects.
                Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)
                Dim doPostureStim As DataObjects.ExternalStimuli.PostureControl = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Locust Posture Control"), DataObjects.ExternalStimuli.PostureControl)
                Dim doAbFlexStim As AnimatTools.DataObjects.ExternalStimuli.TonicCurrent = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Posture Ab Raise"), AnimatTools.DataObjects.ExternalStimuli.TonicCurrent)
                Dim doDorsalAbFlexStim As AnimatTools.DataObjects.ExternalStimuli.TonicCurrent = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Ab Flex Dorsal"), AnimatTools.DataObjects.ExternalStimuli.TonicCurrent)
                Dim doVentralAbFlexStim As AnimatTools.DataObjects.ExternalStimuli.TonicCurrent = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Ab Flex Ventral"), AnimatTools.DataObjects.ExternalStimuli.TonicCurrent)

                Dim dblOrigBeta As Double = doPostureStim.Beta.ActualValue
                Dim dblOrigPitch As Double = doPostureStim.Pitch.ActualValue
                Dim dblOrigDelta As Double = doPostureStim.Delta.ActualValue
                Dim bAbFlexStim As Boolean = doAbFlexStim.Enabled
                Dim dblOrigCurrent As Double = doDorsalAbFlexStim.CurrentOn.ActualValue
                Dim dblOrigStart As Double = doDorsalAbFlexStim.StartTime.ActualValue

                doAbFlexStim.Enabled = True
                doPostureStim.Beta.ActualValue = 29
                doPostureStim.Pitch.ActualValue = 1.5
                doPostureStim.Delta.ActualValue = 0

                Dim rndNum As System.Random = New System.Random
                Dim aryRandNums As New ArrayList
                For iNum As Integer = 1 To 20
                    aryRandNums.Add(411 + iNum)
                Next

                Dim aryCurrents() As Double = {0, 2, 12} '{16.82, 13.88, 12.33, 8.0}
                Dim aryPitches() As Double = {7, 7, 4} '{2.5, 3.0, 3.5, 5.0}
                Dim iIndex As Integer = 0

                For iPitchIdx As Integer = 0 To UBound(aryPitches)
                    doPostureStim.Pitch.ActualValue = aryPitches(iPitchIdx)
                    doDorsalAbFlexStim.Equation = aryCurrents(iPitchIdx) & "*(t/0.05)"

                    For iTrial As Integer = 1 To 20
                        AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(aryRandNums(iTrial - 1))
                        strFileName = strPath & "\AbDoraslMag_B29_P" & CInt(aryPitches(iPitchIdx) * 1000) & "_I" & (aryCurrents(iPitchIdx) * 1000) & "_" & CInt(iTrial + 20) & ".asim"
                        AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                    Next
                Next

                doPostureStim.Beta.ActualValue = dblOrigBeta
                doPostureStim.Pitch.ActualValue = dblOrigPitch
                doPostureStim.Delta.ActualValue = dblOrigDelta
                doDorsalAbFlexStim.CurrentOn.ActualValue = dblOrigCurrent
                doDorsalAbFlexStim.StartTime.ActualValue = dblOrigStart
                doAbFlexStim.Enabled = bAbFlexStim

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = False
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = False
            End Try

        End Sub


        'Private Sub btnVaryAbFlex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVaryAbFlex.Click

        '    Try
        '        If txtConfigDir.Text.Trim.Length = 0 Then
        '            Throw New System.Exception("You must specify an output directory for the config files.")
        '        End If

        '        Dim strPath As String = txtConfigDir.Text
        '        Dim strFileName As String = ""

        '        AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed = False
        '        AnimatTools.Framework.Util.ExportForStandAloneSim = True
        '        AnimatTools.Framework.Util.ExportChartsInStandAloneSim = True
        '        AnimatTools.Framework.Util.ExportStimsInStandAloneSim = True

        '        'Lets get a reference to needed objects.
        '        Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)
        '        Dim doPostureStim As DataObjects.ExternalStimuli.PostureControl = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Locust Posture Control"), DataObjects.ExternalStimuli.PostureControl)
        '        Dim doAbFlexStim As AnimatTools.DataObjects.ExternalStimuli.TonicCurrent = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Posture Ab Raise"), AnimatTools.DataObjects.ExternalStimuli.TonicCurrent)
        '        Dim doDorsalAbFlexStim As AnimatTools.DataObjects.ExternalStimuli.TonicCurrent = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Ab Flex Dorsal"), AnimatTools.DataObjects.ExternalStimuli.TonicCurrent)
        '        Dim doVentralAbFlexStim As AnimatTools.DataObjects.ExternalStimuli.TonicCurrent = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Ab Flex Ventral"), AnimatTools.DataObjects.ExternalStimuli.TonicCurrent)

        '        Dim dblOrigBeta As Double = doPostureStim.Beta.ActualValue
        '        Dim dblOrigPitch As Double = doPostureStim.Pitch.ActualValue
        '        Dim dblOrigDelta As Double = doPostureStim.Delta.ActualValue
        '        Dim bAbFlexStim As Boolean = doAbFlexStim.Enabled
        '        Dim dblOrigCurrent As Double = doDorsalAbFlexStim.CurrentOn.ActualValue
        '        Dim dblOrigStart As Double = doDorsalAbFlexStim.StartTime.ActualValue

        '        doAbFlexStim.Enabled = True
        '        doPostureStim.Beta.ActualValue = 29
        '        doPostureStim.Pitch.ActualValue = 1.5
        '        doPostureStim.Delta.ActualValue = 0

        '        Dim rndNum As System.Random = New System.Random
        '        Dim aryRandNums As New ArrayList
        '        For iNum As Integer = 1 To 10
        '            aryRandNums.Add(413 + iNum)
        '        Next

        '        Dim aryCurrents() As Double = {20, 16.5, 9, 7, 0}
        '        Dim iIndex As Integer = 0

        '        Dim fltPitch As Double = 3.0
        '        For fltCurrent As Double = 13.8 To 14.2 Step 0.01
        '            doPostureStim.Pitch.ActualValue = fltPitch
        '            doDorsalAbFlexStim.Equation = fltCurrent & "*(t/0.05)"

        '            'For iTrial As Integer = 1 To 5
        '            AnimatTools.Framework.Util.Environment.ManualRandomSeed = 412 'CInt(aryRandNums(iTrial - 1))
        '            'strFileName = strPath & "\AbDoraslMag_P" & CInt(fltPitch * 1000) & "_C" & (fltCurrent * 10000) & "_" & CInt(iTrial) & ".asim"
        '            strFileName = strPath & "\AbFlex_B29_P" & CInt(fltPitch * 1000) & "_I" & (fltCurrent * 1000) & ".asim"
        '            AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
        '            'Next

        '            iIndex = iIndex + 1
        '        Next

        '        doPostureStim.Beta.ActualValue = dblOrigBeta
        '        doPostureStim.Pitch.ActualValue = dblOrigPitch
        '        doPostureStim.Delta.ActualValue = dblOrigDelta
        '        doDorsalAbFlexStim.CurrentOn.ActualValue = dblOrigCurrent
        '        doDorsalAbFlexStim.StartTime.ActualValue = dblOrigStart
        '        doAbFlexStim.Enabled = bAbFlexStim

        '    Catch ex As System.Exception
        '        AnimatTools.Framework.Util.DisplayError(ex)
        '    Finally
        '        AnimatTools.Framework.Util.ExportForStandAloneSim = False
        '        AnimatTools.Framework.Util.ExportChartsInStandAloneSim = False
        '        AnimatTools.Framework.Util.ExportStimsInStandAloneSim = False
        '    End Try

        'End Sub


        'Private Sub btnVaryAbFlex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVaryAbFlex.Click

        '    Try
        '        If txtConfigDir.Text.Trim.Length = 0 Then
        '            Throw New System.Exception("You must specify an output directory for the config files.")
        '        End If

        '        Dim strPath As String = txtConfigDir.Text
        '        Dim strFileName As String = ""

        '        AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed = False
        '        AnimatTools.Framework.Util.ExportForStandAloneSim = True
        '        AnimatTools.Framework.Util.ExportChartsInStandAloneSim = True
        '        AnimatTools.Framework.Util.ExportStimsInStandAloneSim = True

        '        'Lets get a reference to needed objects.
        '        Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)
        '        Dim doPostureStim As DataObjects.ExternalStimuli.PostureControl = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Locust Posture Control"), DataObjects.ExternalStimuli.PostureControl)
        '        Dim doAbFlexStim As AnimatTools.DataObjects.ExternalStimuli.TonicCurrent = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Posture Ab Raise"), AnimatTools.DataObjects.ExternalStimuli.TonicCurrent)
        '        Dim doDorsalAbFlexStim As AnimatTools.DataObjects.ExternalStimuli.TonicCurrent = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Ab Flex Dorsal"), AnimatTools.DataObjects.ExternalStimuli.TonicCurrent)
        '        Dim doVentralAbFlexStim As AnimatTools.DataObjects.ExternalStimuli.TonicCurrent = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Ab Flex Ventral"), AnimatTools.DataObjects.ExternalStimuli.TonicCurrent)

        '        Dim dblOrigBeta As Double = doPostureStim.Beta.ActualValue
        '        Dim dblOrigPitch As Double = doPostureStim.Pitch.ActualValue
        '        Dim dblOrigDelta As Double = doPostureStim.Delta.ActualValue
        '        Dim bAbFlexStim As Boolean = doAbFlexStim.Enabled
        '        Dim dblOrigCurrent As Double = doDorsalAbFlexStim.CurrentOn.ActualValue
        '        Dim dblOrigStart As Double = doDorsalAbFlexStim.StartTime.ActualValue

        '        doAbFlexStim.Enabled = True
        '        doPostureStim.Beta.ActualValue = 29
        '        doPostureStim.Pitch.ActualValue = 1.5
        '        doPostureStim.Delta.ActualValue = 0

        '        Dim rndNum As System.Random = New System.Random
        '        Dim aryRandNums As New ArrayList
        '        For iNum As Integer = 1 To 20
        '            aryRandNums.Add(CInt(AnimatTools.Framework.Util.Rand(100, 10000, rndNum)))
        '        Next

        '        doDorsalAbFlexStim.Equation = "16*(t/0.05)"

        '        'Dim fltPitch As Double = 4.5
        '        For fltPitch As Double = 2 To 6 Step 0.5
        '            doPostureStim.Pitch.ActualValue = fltPitch

        '            For iTrial As Integer = 1 To 20
        '                AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(aryRandNums(iTrial - 1))
        '                strFileName = strPath & "\AbDoraslMag_P" & CInt(fltPitch * 10) & "_C16_" & CInt(iTrial) & ".asim"
        '                AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
        '            Next
        '        Next

        '        doPostureStim.Beta.ActualValue = dblOrigBeta
        '        doPostureStim.Pitch.ActualValue = dblOrigPitch
        '        doPostureStim.Delta.ActualValue = dblOrigDelta
        '        doDorsalAbFlexStim.CurrentOn.ActualValue = dblOrigCurrent
        '        doDorsalAbFlexStim.StartTime.ActualValue = dblOrigStart
        '        doAbFlexStim.Enabled = bAbFlexStim

        '    Catch ex As System.Exception
        '        AnimatTools.Framework.Util.DisplayError(ex)
        '    Finally
        '        AnimatTools.Framework.Util.ExportForStandAloneSim = False
        '        AnimatTools.Framework.Util.ExportChartsInStandAloneSim = False
        '        AnimatTools.Framework.Util.ExportStimsInStandAloneSim = False
        '    End Try

        'End Sub

        Private Sub btnExtStimvsTen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtStimvsTen.Click
            Try
                If txtConfigDir.Text.Trim.Length = 0 Then
                    Throw New System.Exception("You must specify an output directory for the config files.")
                End If

                Dim strPath As String = txtConfigDir.Text
                Dim strFileName As String = ""

                'Lets get a reference to needed objects.
                Dim doGrasshopper As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Female Locust"), AnimatTools.DataObjects.Physical.Organism)

                Dim doLeftStim As AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent = DirectCast(AnimatTools.Framework.Util.Application.ProjectStimuli.FindDataObjectByName("Left Extensor 2a"), AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent)
                Dim doRightStim As AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent = DirectCast(AnimatTools.Framework.Util.Application.ProjectStimuli.FindDataObjectByName("Right Extensor 2a"), AnimatTools.DataObjects.ExternalStimuli.RepetitiveCurrent)
                Dim doPosture As DataObjects.ExternalStimuli.PostureControl = DirectCast(AnimatTools.Framework.Util.Application.FindStimulusByName("Locust Posture Control"), DataObjects.ExternalStimuli.PostureControl)

                Dim doLeftSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)
                Dim doRightSemilunar As VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Spring"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Spring)

                Dim doLeftSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Left Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)
                Dim doRightSliderJoint As VortexAnimatTools.DataObjects.Physical.Joints.Prismatic = DirectCast(doGrasshopper.FindBodyPartByName("Right Semilunar Joint"), VortexAnimatTools.DataObjects.Physical.Joints.Prismatic)

                Dim dblIncrement As Double = 1
                Dim iSimTimeStart As Integer = 10
                Dim iSimTimeEnd As Integer = 23
                Dim iTrials As Integer = 5
                Dim iSims As Integer = CInt((iSimTimeEnd - iSimTimeStart) / dblIncrement) + 1
                'Dim aryBetasW As Double() = New Double() {41, 41, 41, 42, 43, 43, 44, 46, 48, 50, 55}
                'Dim aryBetasWo As Double() = New Double() {46, 46, 46, 46, 46, 46, 46, 48, 49, 53, 55}
                'Dim aryBetasW As Double() = New Double() {52, 52, 52, 52, 52, 52, 53, 53, 53, 53, 53, 53, 54, 54, 56, 59, 59, 59, 60, 63, 65}
                'Dim aryBetasWo As Double() = New Double() {58, 58, 58, 58, 58, 58, 58, 58, 58, 58, 58, 58, 58, 59, 60, 61, 62, 63, 64, 65, 66}
                Dim aryBetasW As Double() = New Double() {32, 32, 32, 32, 32, 32, 33, 33, 33, 33, 33, 33, 34, 34, 36, 39, 39, 39, 40, 43, 45}
                Dim aryBetasWo As Double() = New Double() {38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 38, 39, 40, 41, 42, 43, 44, 45, 46}

                'First set things up to do the normal jumps.
                Dim bAutoSeed As Boolean = AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed
                Dim dblLDoff As Double = doLeftStim.CycleOffDuration.Value
                Dim dblRDoff As Double = doRightStim.CycleOffDuration.Value
                Dim iSeed As Integer = AnimatTools.Framework.Util.Environment.ManualRandomSeed
                Dim dblBeta As Double = doPosture.Beta.ActualValue
                AnimatTools.Framework.Util.Environment.AutoGenerateRandomSeed = False

                doLeftSemilunar.Enabled = True
                doRightSemilunar.Enabled = True
                doLeftSliderJoint.EnableMotor = False
                doRightSliderJoint.EnableMotor = False

                AnimatTools.Framework.Util.ExportForStandAloneSim = True
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = True
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = True

                Dim rndNum As System.Random = New System.Random
                Dim aryRandNums As New ArrayList
                For iNum As Integer = 1 To (iSims * iTrials)
                    aryRandNums.Add(413 + iNum)
                Next

                Dim dblStim As Double = iSimTimeStart
                Dim idxRandNum As Integer = 0
                For iStimStep As Integer = 1 To iSims
                    For iTrial As Integer = 1 To iTrials
                        doPosture.Beta.ActualValue = aryBetasW(iStimStep - 1)
                        AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(aryRandNums(idxRandNum))
                        doLeftStim.CycleOffDuration.Value = dblStim
                        doRightStim.CycleOffDuration.Value = dblStim

                        strFileName = strPath & "\SLPTest_With_" & CInt(dblStim * 10) & "_Trial_" & (iTrial) & ".asim"
                        AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                        idxRandNum = idxRandNum + 1
                    Next

                    dblStim = dblStim + dblIncrement
                Next

                doLeftSemilunar.Enabled = False
                doRightSemilunar.Enabled = False
                doLeftSliderJoint.EnableMotor = True
                doRightSliderJoint.EnableMotor = True

                dblStim = iSimTimeStart
                idxRandNum = 0
                For iStimStep As Integer = 1 To iSims
                    For iTrial As Integer = 1 To iTrials
                        doPosture.Beta.ActualValue = aryBetasWo(iStimStep - 1)
                        AnimatTools.Framework.Util.Environment.ManualRandomSeed = CInt(aryRandNums(idxRandNum))
                        doLeftStim.CycleOffDuration.Value = dblStim
                        doRightStim.CycleOffDuration.Value = dblStim

                        strFileName = strPath & "\SLPTest_Without_" & CInt(dblStim * 10) & "_Trial_" & (iTrial) & ".asim"
                        AnimatTools.Framework.Util.Simulation.SaveData(AnimatTools.Framework.Util.Application, strFileName)
                        idxRandNum = idxRandNum + 1
                    Next

                    dblStim = dblStim + dblIncrement
                Next

                'Set everything back for normal jumps.
                doLeftStim.CycleOffDuration.Value = dblLDoff
                doRightStim.CycleOffDuration.Value = dblRDoff
                AnimatTools.Framework.Util.Environment.ManualRandomSeed = iSeed
                doPosture.Beta.ActualValue = dblBeta
                doLeftSemilunar.Enabled = True
                doRightSemilunar.Enabled = True
                doLeftSliderJoint.EnableMotor = False
                doRightSliderJoint.EnableMotor = False

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            Finally
                AnimatTools.Framework.Util.ExportForStandAloneSim = False
                AnimatTools.Framework.Util.ExportChartsInStandAloneSim = False
                AnimatTools.Framework.Util.ExportStimsInStandAloneSim = False
            End Try
        End Sub


    End Class

End Namespace


