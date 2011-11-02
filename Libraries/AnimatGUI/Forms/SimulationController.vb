Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI
Imports AnimatGUI.Framework

Namespace Forms

    Public Class SimulationController
        Inherits AnimatForm

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
        Friend WithEvents ctrlTimeRuler As AnimatGUICtrls.Controls.TimeRuler
        Friend WithEvents lblCursorTime As System.Windows.Forms.Label
        Friend WithEvents lblCursorLabel As System.Windows.Forms.Label
        Friend WithEvents grpSimControls As System.Windows.Forms.GroupBox
        Friend WithEvents btnSimGotoStart As System.Windows.Forms.Button
        Friend WithEvents btnSimStepBack As System.Windows.Forms.Button
        Friend WithEvents btnSimStart As System.Windows.Forms.Button
        Friend WithEvents btnSimStop As System.Windows.Forms.Button
        Friend WithEvents btnSimStepForward As System.Windows.Forms.Button
        Friend WithEvents btnSimGotoEnd As System.Windows.Forms.Button
        Friend WithEvents grpVideoControls As System.Windows.Forms.GroupBox
        Friend WithEvents btnVideoGotoStart As System.Windows.Forms.Button
        Friend WithEvents btnVideoStepBack As System.Windows.Forms.Button
        Friend WithEvents btnVideoStart As System.Windows.Forms.Button
        Friend WithEvents btnVideoStepForward As System.Windows.Forms.Button
        Friend WithEvents btnVideoGotoEnd As System.Windows.Forms.Button
        Friend WithEvents lblVideoStart As System.Windows.Forms.Label
        Friend WithEvents lblVideoEnd As System.Windows.Forms.Label
        Friend WithEvents lblVideoCurrent As System.Windows.Forms.Label
        Friend WithEvents txtVideoStart As System.Windows.Forms.TextBox
        Friend WithEvents txtVideoEnd As System.Windows.Forms.TextBox
        Friend WithEvents txtVideoCurrent As System.Windows.Forms.TextBox
        Friend WithEvents btnVideoSave As System.Windows.Forms.Button
        Friend WithEvents grpSnapshotControls As System.Windows.Forms.GroupBox
        Friend WithEvents lblSnapshotTime As System.Windows.Forms.Label
        Friend WithEvents txtSnapshotTime As System.Windows.Forms.TextBox
        Friend WithEvents m_Timer As Timer

        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.ctrlTimeRuler = New AnimatGUICtrls.Controls.TimeRuler

            Me.grpSimControls = New System.Windows.Forms.GroupBox
            Me.btnSimGotoStart = New System.Windows.Forms.Button
            Me.btnSimStepBack = New System.Windows.Forms.Button
            Me.btnSimStart = New System.Windows.Forms.Button
            Me.btnSimStop = New System.Windows.Forms.Button
            Me.btnSimStepForward = New System.Windows.Forms.Button
            Me.btnSimGotoEnd = New System.Windows.Forms.Button

            Me.grpVideoControls = New System.Windows.Forms.GroupBox
            Me.btnVideoGotoStart = New System.Windows.Forms.Button
            Me.btnVideoStepBack = New System.Windows.Forms.Button
            Me.btnVideoStart = New System.Windows.Forms.Button
            Me.btnVideoStepForward = New System.Windows.Forms.Button
            Me.btnVideoGotoEnd = New System.Windows.Forms.Button
            Me.lblVideoStart = New System.Windows.Forms.Label
            Me.lblVideoEnd = New System.Windows.Forms.Label
            Me.lblVideoCurrent = New System.Windows.Forms.Label
            Me.txtVideoStart = New System.Windows.Forms.TextBox
            Me.txtVideoEnd = New System.Windows.Forms.TextBox
            Me.txtVideoCurrent = New System.Windows.Forms.TextBox
            Me.btnVideoSave = New System.Windows.Forms.Button

            Me.grpSnapshotControls = New System.Windows.Forms.GroupBox
            Me.lblSnapshotTime = New System.Windows.Forms.Label
            Me.txtSnapshotTime = New System.Windows.Forms.TextBox
            Me.m_Timer = New Timer

            Me.grpSimControls.SuspendLayout()
            Me.grpVideoControls.SuspendLayout()
            Me.grpSnapshotControls.SuspendLayout()

            Me.SuspendLayout()

            m_Timer.Enabled = False
            m_Timer.Interval = 100

            '
            'ctrlTimeRuler
            '
            Me.ctrlTimeRuler.ActualMillisecond = CType(0, Long)
            Me.ctrlTimeRuler.ActualTimeColor = System.Drawing.Color.LightBlue
            Me.ctrlTimeRuler.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ctrlTimeRuler.AutomaticTimeScale = True
            Me.ctrlTimeRuler.CurrentMillisecond = CType(0, Long)
            Me.ctrlTimeRuler.CurrentTimeColor = System.Drawing.Color.Blue
            Me.ctrlTimeRuler.DivisionMarkFactor = 4
            Me.ctrlTimeRuler.Divisions = 10
            Me.ctrlTimeRuler.EndMillisecond = CType(5000, Long)
            Me.ctrlTimeRuler.ForeColor = System.Drawing.Color.Black
            Me.ctrlTimeRuler.Location = New System.Drawing.Point(200, 5)
            Me.ctrlTimeRuler.MajorInterval = 1
            Me.ctrlTimeRuler.MiddleMarkFactor = 3
            Me.ctrlTimeRuler.MouseTrackingOn = True
            Me.ctrlTimeRuler.Name = "ctrlTimeRuler"
            Me.ctrlTimeRuler.Orientation = AnimatGUICtrls.Controls.enumOrientation.orHorizontal
            Me.ctrlTimeRuler.ProgressBarScale = 40
            Me.ctrlTimeRuler.RulerAlignment = AnimatGUICtrls.Controls.enumRulerAlignment.raBottomOrRight
            Me.ctrlTimeRuler.Size = New System.Drawing.Size(100, 110)
            Me.ctrlTimeRuler.StartMillisecond = CType(0, Long)
            Me.ctrlTimeRuler.TabIndex = 0
            Me.ctrlTimeRuler.Text = "Time Ruler"
            Me.ctrlTimeRuler.TimeScale = AnimatGUICtrls.Controls.enumTimeScale.Minutes
            Me.ctrlTimeRuler.VerticalNumbers = False
            '
            'grpSimControls
            '
            Me.grpSimControls.Controls.Add(Me.btnSimGotoStart)
            Me.grpSimControls.Controls.Add(Me.btnSimStepBack)
            Me.grpSimControls.Controls.Add(Me.btnSimStart)
            Me.grpSimControls.Controls.Add(Me.btnSimStop)
            Me.grpSimControls.Controls.Add(Me.btnSimStepForward)
            Me.grpSimControls.Controls.Add(Me.btnSimGotoEnd)
            Me.grpSimControls.Location = New System.Drawing.Point(5, 2)
            Me.grpSimControls.Name = "grpSimControls"
            Me.grpSimControls.Size = New System.Drawing.Size(190, 55)
            Me.grpSimControls.TabIndex = 0
            Me.grpSimControls.TabStop = False
            Me.grpSimControls.Text = "Simulation Controls"
            '
            'btnSimGotoStart
            '
            Me.btnSimGotoStart.Location = New System.Drawing.Point(5, 20)
            Me.btnSimGotoStart.Name = "btnSimGotoStart"
            Me.btnSimGotoStart.Size = New System.Drawing.Size(27, 27)
            Me.btnSimGotoStart.TabIndex = 0
            Me.btnSimGotoStart.Text = ""
            Me.btnSimGotoStart.ImageAlign = ContentAlignment.MiddleCenter
            Me.btnSimGotoStart.Enabled = False
            '
            'btnSimStepBack
            '
            Me.btnSimStepBack.Location = New System.Drawing.Point(35, 20)
            Me.btnSimStepBack.Name = "btnSimStepBack"
            Me.btnSimStepBack.Size = New System.Drawing.Size(27, 27)
            Me.btnSimStepBack.TabIndex = 0
            Me.btnSimStepBack.Text = ""
            Me.btnSimStepBack.ImageAlign = ContentAlignment.MiddleCenter
            Me.btnSimStepBack.Enabled = False
            '
            'btnSimStart
            '
            Me.btnSimStart.Location = New System.Drawing.Point(65, 20)
            Me.btnSimStart.Name = "btnSimStart"
            Me.btnSimStart.Size = New System.Drawing.Size(27, 27)
            Me.btnSimStart.TabIndex = 0
            Me.btnSimStart.Text = ""
            Me.btnSimStart.ImageAlign = ContentAlignment.MiddleCenter
            Me.btnSimStart.Enabled = False
            '
            'btnSimSotp
            '
            Me.btnSimStop.Location = New System.Drawing.Point(95, 20)
            Me.btnSimStop.Name = "btnSimStop"
            Me.btnSimStop.Size = New System.Drawing.Size(27, 27)
            Me.btnSimStop.TabIndex = 0
            Me.btnSimStop.Text = ""
            Me.btnSimStop.ImageAlign = ContentAlignment.MiddleCenter
            Me.btnSimStop.Enabled = False
            '
            'btnSimStepForward
            '
            Me.btnSimStepForward.Location = New System.Drawing.Point(125, 20)
            Me.btnSimStepForward.Name = "btnSimStepForward"
            Me.btnSimStepForward.Size = New System.Drawing.Size(27, 27)
            Me.btnSimStepForward.TabIndex = 0
            Me.btnSimStepForward.Text = ""
            Me.btnSimStepForward.ImageAlign = ContentAlignment.MiddleCenter
            Me.btnSimStepForward.Enabled = False
            '
            'btnSimGotoEnd
            '
            Me.btnSimGotoEnd.Location = New System.Drawing.Point(155, 20)
            Me.btnSimGotoEnd.Name = "btnSimStepForward"
            Me.btnSimGotoEnd.Size = New System.Drawing.Size(27, 27)
            Me.btnSimGotoEnd.TabIndex = 0
            Me.btnSimGotoEnd.Text = ""
            Me.btnSimGotoEnd.ImageAlign = ContentAlignment.MiddleCenter
            Me.btnSimGotoEnd.Enabled = False
            '
            'grpVideoControls
            '
            Me.grpVideoControls.Controls.Add(Me.btnVideoGotoStart)
            Me.grpVideoControls.Controls.Add(Me.btnVideoStepBack)
            Me.grpVideoControls.Controls.Add(Me.btnVideoStart)
            Me.grpVideoControls.Controls.Add(Me.btnVideoStepForward)
            Me.grpVideoControls.Controls.Add(Me.btnVideoGotoEnd)
            Me.grpVideoControls.Controls.Add(Me.lblVideoStart)
            Me.grpVideoControls.Controls.Add(Me.lblVideoEnd)
            Me.grpVideoControls.Controls.Add(Me.lblVideoCurrent)
            Me.grpVideoControls.Controls.Add(Me.txtVideoStart)
            Me.grpVideoControls.Controls.Add(Me.txtVideoEnd)
            Me.grpVideoControls.Controls.Add(Me.txtVideoCurrent)
            Me.grpVideoControls.Controls.Add(Me.btnVideoSave)
            Me.grpVideoControls.Location = New System.Drawing.Point(5, 60)
            Me.grpVideoControls.Name = "grpVideoControls"
            Me.grpVideoControls.Size = New System.Drawing.Size(190, 140)
            Me.grpVideoControls.TabIndex = 0
            Me.grpVideoControls.TabStop = False
            Me.grpVideoControls.Text = "Video Controls"
            Me.grpVideoControls.Visible = False
            '
            'btnVideoGotoStart
            '
            Me.btnVideoGotoStart.Location = New System.Drawing.Point(5, 20)
            Me.btnVideoGotoStart.Name = "btnVideoGotoStart"
            Me.btnVideoGotoStart.Size = New System.Drawing.Size(27, 27)
            Me.btnVideoGotoStart.TabIndex = 0
            Me.btnVideoGotoStart.Text = ""
            Me.btnVideoGotoStart.ImageAlign = ContentAlignment.MiddleCenter
            '
            'btnVideoStepBack
            '
            Me.btnVideoStepBack.Location = New System.Drawing.Point(35, 20)
            Me.btnVideoStepBack.Name = "btnVideoStepBack"
            Me.btnVideoStepBack.Size = New System.Drawing.Size(27, 27)
            Me.btnVideoStepBack.TabIndex = 0
            Me.btnVideoStepBack.Text = ""
            Me.btnVideoStepBack.ImageAlign = ContentAlignment.MiddleCenter
            '
            'btnVideoStart
            '
            Me.btnVideoStart.Location = New System.Drawing.Point(65, 20)
            Me.btnVideoStart.Name = "btnVideoStart"
            Me.btnVideoStart.Size = New System.Drawing.Size(27, 27)
            Me.btnVideoStart.TabIndex = 0
            Me.btnVideoStart.Text = ""
            Me.btnVideoStart.ImageAlign = ContentAlignment.MiddleCenter
            '
            'btnVideoStepForward
            '
            Me.btnVideoStepForward.Location = New System.Drawing.Point(95, 20)
            Me.btnVideoStepForward.Name = "btnVideoStepForward"
            Me.btnVideoStepForward.Size = New System.Drawing.Size(27, 27)
            Me.btnVideoStepForward.TabIndex = 0
            Me.btnVideoStepForward.Text = ""
            Me.btnVideoStepForward.ImageAlign = ContentAlignment.MiddleCenter
            '
            'btnVideoGotoEnd
            '
            Me.btnVideoGotoEnd.Location = New System.Drawing.Point(125, 20)
            Me.btnVideoGotoEnd.Name = "btnVideoGotoEnd"
            Me.btnVideoGotoEnd.Size = New System.Drawing.Size(27, 27)
            Me.btnVideoGotoEnd.TabIndex = 0
            Me.btnVideoGotoEnd.Text = ""
            Me.btnVideoGotoEnd.ImageAlign = ContentAlignment.MiddleCenter
            '
            'lblVideoStart
            '
            Me.lblVideoStart.Location = New System.Drawing.Point(5, 50)
            Me.lblVideoStart.Name = "lblVideoStart"
            Me.lblVideoStart.Size = New System.Drawing.Size(75, 20)
            Me.lblVideoStart.TabIndex = 0
            Me.lblVideoStart.Text = "Start Time"
            Me.lblVideoStart.TextAlign = ContentAlignment.MiddleCenter
            '
            'lblVideoEnd
            '
            Me.lblVideoEnd.Location = New System.Drawing.Point(80, 50)
            Me.lblVideoEnd.Name = "lblVideoEnd"
            Me.lblVideoEnd.Size = New System.Drawing.Size(75, 20)
            Me.lblVideoEnd.TabIndex = 0
            Me.lblVideoEnd.Text = "End Time"
            Me.lblVideoEnd.TextAlign = ContentAlignment.MiddleCenter
            '
            '
            'txtVideoStart
            '
            Me.txtVideoStart.Location = New System.Drawing.Point(5, 70)
            Me.txtVideoStart.Name = "txtVideoStart"
            Me.txtVideoStart.Size = New System.Drawing.Size(75, 27)
            Me.txtVideoStart.TabIndex = 0
            Me.txtVideoStart.Text = ""
            '
            'txtVideoEnd
            '
            Me.txtVideoEnd.Location = New System.Drawing.Point(80, 70)
            Me.txtVideoEnd.Name = "txtVideoEnd"
            Me.txtVideoEnd.Size = New System.Drawing.Size(75, 27)
            Me.txtVideoEnd.TabIndex = 0
            Me.txtVideoEnd.Text = ""
            '
            'lblVideoCurrent
            '
            Me.lblVideoCurrent.Location = New System.Drawing.Point(5, 90)
            Me.lblVideoCurrent.Name = "lblVideoCurrent"
            Me.lblVideoCurrent.Size = New System.Drawing.Size(75, 20)
            Me.lblVideoCurrent.TabIndex = 0
            Me.lblVideoCurrent.Text = "Current Time"
            Me.lblVideoCurrent.TextAlign = ContentAlignment.MiddleCenter
            '
            'txtVideoCurrent
            '
            Me.txtVideoCurrent.Location = New System.Drawing.Point(5, 110)
            Me.txtVideoCurrent.Name = "txtVideoCurrent"
            Me.txtVideoCurrent.Size = New System.Drawing.Size(75, 27)
            Me.txtVideoCurrent.TabIndex = 0
            Me.txtVideoCurrent.Text = ""
            '
            'btnVideoSave
            '
            Me.btnVideoSave.Location = New System.Drawing.Point(80, 100)
            Me.btnVideoSave.Name = "btnVideoSave"
            Me.btnVideoSave.Size = New System.Drawing.Size(75, 30)
            Me.btnVideoSave.TabIndex = 0
            Me.btnVideoSave.Text = "Save    " & vbCrLf & "Video"
            Me.btnVideoSave.TextAlign = ContentAlignment.MiddleRight
            Me.btnVideoSave.ImageAlign = ContentAlignment.MiddleLeft
            '
            'grpSnapshotControls
            '
            Me.grpSnapshotControls.Controls.Add(Me.lblSnapshotTime)
            Me.grpSnapshotControls.Controls.Add(Me.txtSnapshotTime)
            Me.grpSnapshotControls.Location = New System.Drawing.Point(5, 60)
            Me.grpSnapshotControls.Name = "grpSnapshotControls"
            Me.grpSnapshotControls.Size = New System.Drawing.Size(190, 140)
            Me.grpSnapshotControls.TabIndex = 0
            Me.grpSnapshotControls.TabStop = False
            Me.grpSnapshotControls.Text = "Snapshot Controls"
            Me.grpSnapshotControls.Visible = False
            '
            'lblSnapshotTime
            '
            Me.lblSnapshotTime.Location = New System.Drawing.Point(5, 20)
            Me.lblSnapshotTime.Name = "lblSnapshotTime"
            Me.lblSnapshotTime.Size = New System.Drawing.Size(75, 20)
            Me.lblSnapshotTime.TabIndex = 0
            Me.lblSnapshotTime.Text = "Current Time"
            Me.lblSnapshotTime.TextAlign = ContentAlignment.MiddleCenter
            '
            'txtSnapshotTime
            '
            Me.txtSnapshotTime.Location = New System.Drawing.Point(5, 40)
            Me.txtSnapshotTime.Name = "txtSnapshotTime"
            Me.txtSnapshotTime.Size = New System.Drawing.Size(75, 27)
            Me.txtSnapshotTime.TabIndex = 0
            Me.txtSnapshotTime.Text = ""

            '
            'SimulationController
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(872, 125)
            Me.Controls.Add(Me.grpSimControls)
            Me.Controls.Add(Me.grpVideoControls)
            Me.Controls.Add(Me.grpSnapshotControls)
            Me.Controls.Add(Me.ctrlTimeRuler)
            Me.Name = "SimulationController"
            Me.Text = "SimulationController"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_selKeyFrame As KeyFrame

        Protected m_snTimeBarInterval As ScaledNumber

        Protected m_snStartingBarTime As AnimatGUI.Framework.ScaledNumber
        Protected m_snEndingBarTime As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatGUI.SimulationController.gif"
            End Get
        End Property

        Public Property StartingBarTime() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snStartingBarTime
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The starting bar time must be greater than zero.")
                End If

                If Value.ActualValue >= m_snEndingBarTime.ActualValue Then
                    Throw New System.Exception("The starting bar time must be greater than the ending bar time.")
                End If

                m_snStartingBarTime.CopyData(Value)
                Me.ctrlTimeRuler.StartMillisecond = CLng(m_snStartingBarTime.ValueFromDefaultScale)
            End Set
        End Property

        Public Property EndingBarTime() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snEndingBarTime
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The ending bar time must be greater than zero.")
                End If

                If Value.ActualValue <= m_snStartingBarTime.ActualValue Then
                    Throw New System.Exception("The ending bar time must be greater than the starting bar time.")
                End If

                m_snEndingBarTime.CopyData(Value)
                Me.ctrlTimeRuler.EndMillisecond = CLng(m_snEndingBarTime.ValueFromDefaultScale)
            End Set
        End Property

        Public Property CurrentTimeColor() As System.Drawing.Color
            Get
                Return Me.ctrlTimeRuler.CurrentTimeColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                Me.ctrlTimeRuler.CurrentTimeColor = Value
            End Set
        End Property

        Public Property AutomaticTimeScale() As Boolean
            Get
                Return Me.ctrlTimeRuler.AutomaticTimeScale
            End Get
            Set(ByVal Value As Boolean)
                Me.ctrlTimeRuler.AutomaticTimeScale = Value
            End Set
        End Property

        Public Property ActualTimeColor() As System.Drawing.Color
            Get
                Return Me.ctrlTimeRuler.ActualTimeColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                Me.ctrlTimeRuler.ActualTimeColor = Value
            End Set
        End Property

        Public Property DivisionMarkFactor() As Integer
            Get
                Return Me.ctrlTimeRuler.DivisionMarkFactor
            End Get
            Set(ByVal Value As Integer)
                Me.ctrlTimeRuler.DivisionMarkFactor = Value
            End Set
        End Property

        Public Property Divisions() As Integer
            Get
                Return Me.ctrlTimeRuler.Divisions
            End Get
            Set(ByVal Value As Integer)
                Me.ctrlTimeRuler.Divisions = Value
            End Set
        End Property

        Public Property MajorInterval() As Integer
            Get
                Return Me.ctrlTimeRuler.MajorInterval
            End Get
            Set(ByVal Value As Integer)
                Me.ctrlTimeRuler.MajorInterval = Value
            End Set
        End Property

        Public Property MiddleMarkFactor() As Integer
            Get
                Return Me.ctrlTimeRuler.MiddleMarkFactor
            End Get
            Set(ByVal Value As Integer)
                Me.ctrlTimeRuler.MiddleMarkFactor = Value
            End Set
        End Property

        Public Property TimeScale() As AnimatGUICtrls.Controls.enumTimeScale
            Get
                Return Me.ctrlTimeRuler.TimeScale
            End Get
            Set(ByVal Value As AnimatGUICtrls.Controls.enumTimeScale)
                Me.ctrlTimeRuler.TimeScale = Value
            End Set
        End Property

        Public Property TimeBarInterval() As ScaledNumber
            Get
                Return m_snTimeBarInterval
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    If Value.ActualValue < 0 Then
                        Throw New System.Exception("The time bar interval must be greater than zero.")
                    End If

                    m_snTimeBarInterval.CopyData(Value)
                    m_Timer.Interval = CInt(Value.ActualValue * 1000)
                End If
            End Set
        End Property

        Public Overridable Property EnableSimRecording() As Boolean
            Get
                Return Util.Simulation.EnableSimRecording
            End Get
            Set(ByVal Value As Boolean)
                Util.Simulation.EnableSimRecording = Value
            End Set
        End Property

        Public Overridable Property StartPaused() As Boolean
            Get
                Return Util.Simulation.StartPaused
            End Get
            Set(ByVal Value As Boolean)
                Util.Simulation.StartPaused = Value
            End Set
        End Property

        Public ReadOnly Property CurrentSimulationTime() As Single
            Get
                Return CSng(Me.ctrlTimeRuler.CurrentMillisecond * 0.001)
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            MyBase.Initialize(frmParent)

            m_snStartingBarTime = New AnimatGUI.Framework.ScaledNumber(Me.FormHelper, "StartingBarTime", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            m_snEndingBarTime = New AnimatGUI.Framework.ScaledNumber(Me.FormHelper, "EndingBarTime", 5000, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            m_snTimeBarInterval = New AnimatGUI.Framework.ScaledNumber(Me.FormHelper, "TimeBarInterval", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

            AddHandler Me.ctrlTimeRuler.HooverValue, AddressOf Me.OnCursorMoved
            AddHandler Me.ctrlTimeRuler.KeyFrameSelected, AddressOf Me.OnKeyFrameSelected
            AddHandler Me.ctrlTimeRuler.KeyFrameAdded, AddressOf Me.OnKeyFrameAdded
            AddHandler Me.ctrlTimeRuler.KeyFrameRemoved, AddressOf Me.OnKeyFrameRemoved
            AddHandler Me.ctrlTimeRuler.KeyFrameMoved, AddressOf Me.OnKeyFrameMoved
            AddHandler Me.ctrlTimeRuler.KeyFrameMoving, AddressOf Me.OnKeyFrameMoving
            AddHandler Me.ctrlTimeRuler.CurrentFrameMoved, AddressOf Me.OnCurrentFrameMoved
            AddHandler Util.Application.ProjectCreated, AddressOf Me.OnProjectCreated
            AddHandler Util.Application.ProjectClosed, AddressOf Me.OnProjectClosed
            AddHandler Util.Application.ProjectLoaded, AddressOf Me.OnProjectLoaded
            AddHandler Util.Application.SimulationStarting, AddressOf Me.OnSimulationStarting
            AddHandler Util.Application.SimulationStarted, AddressOf Me.OnSimulationStarted
            AddHandler Util.Application.SimulationPaused, AddressOf Me.OnSimulationPaused
            AddHandler Util.Application.SimulationResuming, AddressOf Me.OnSimulationResuming
            AddHandler Util.Application.SimulationStopped, AddressOf Me.OnSimulationStopped

            Me.Icon = Util.Application.ToolStripImages.LoadIcon(myAssembly, "AnimatGUI.SimulationController.ico")

            If Not Util.Application.Simulation Is Nothing AndAlso Not Util.Application.Simulation.PlaybackControlTreeNode Is Nothing Then
                Util.Application.Simulation.PlaybackControlTreeNode.Tag = Me
            End If

            Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.PlayLarge.gif")
            Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.PauseLarge.gif")
            Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.StopLarge.gif")
            Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.GotoEndLarge.gif")
            Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.GotoStartLarge.gif")
            Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.StepForwardLarge.gif")
            Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.StepBackLarge.gif")
            Util.Application.LargeImages.AddImage(myAssembly, "AnimatGUI.SaveVideoLarge.gif")

            Me.btnSimGotoStart.ImageList = Util.Application.LargeImages.ImageList
            Me.btnSimGotoStart.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.GotoStartLarge.gif")

            Me.btnSimStepBack.ImageList = Util.Application.LargeImages.ImageList
            Me.btnSimStepBack.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.StepBackLarge.gif")

            Me.btnSimStepBack.ImageList = Util.Application.LargeImages.ImageList
            Me.btnSimStepBack.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.StepBackLarge.gif")

            Me.btnSimStart.ImageList = Util.Application.LargeImages.ImageList
            Me.btnSimStart.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.PlayLarge.gif")

            Me.btnSimStop.ImageList = Util.Application.LargeImages.ImageList
            Me.btnSimStop.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.StopLarge.gif")

            Me.btnSimStepForward.ImageList = Util.Application.LargeImages.ImageList
            Me.btnSimStepForward.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.StepForwardLarge.gif")

            Me.btnSimGotoEnd.ImageList = Util.Application.LargeImages.ImageList
            Me.btnSimGotoEnd.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.GotoEndLarge.gif")


            Me.btnVideoGotoStart.ImageList = Util.Application.LargeImages.ImageList
            Me.btnVideoGotoStart.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.GotoStartLarge.gif")

            Me.btnVideoStepBack.ImageList = Util.Application.LargeImages.ImageList
            Me.btnVideoStepBack.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.StepBackLarge.gif")

            Me.btnVideoStepBack.ImageList = Util.Application.LargeImages.ImageList
            Me.btnVideoStepBack.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.StepBackLarge.gif")

            Me.btnVideoStart.ImageList = Util.Application.LargeImages.ImageList
            Me.btnVideoStart.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.PlayLarge.gif")

            Me.btnVideoStepForward.ImageList = Util.Application.LargeImages.ImageList
            Me.btnVideoStepForward.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.StepForwardLarge.gif")

            Me.btnVideoGotoEnd.ImageList = Util.Application.LargeImages.ImageList
            Me.btnVideoGotoEnd.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.GotoEndLarge.gif")

            Me.btnVideoSave.ImageList = Util.Application.LargeImages.ImageList
            Me.btnVideoSave.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.SaveVideoLarge.gif")
        End Sub

        Protected Function GenerateStatusBarText(ByVal dblCursorTime As Double) As String
            Dim strStatus As String = "Current Time: " & Me.ctrlTimeRuler.CurrentMillisecond.ToString & " (ms) "

            If Me.ctrlTimeRuler.CurrentMillisecond < Me.ctrlTimeRuler.ActualMillisecond Then
                strStatus += " Actual Time: " & Me.ctrlTimeRuler.ActualMillisecond.ToString & " (ms) "
            End If

            If dblCursorTime >= 0 Then
                strStatus += " Cursor Time: " & dblCursorTime & " (ms) "
            End If

            Return strStatus
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGUICtrls.Controls.PropertyTable)

            Dim pbNumberBag As AnimatGUICtrls.Controls.PropertyBag = m_snStartingBarTime.Properties
            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Start Time", pbNumberBag.GetType(), "StartingBarTime", _
                                        "Time Bar Settings", "Sets the starting time used for the controller time bar.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snEndingBarTime.Properties
            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("End Time", pbNumberBag.GetType(), "EndingBarTime", _
                                        "Time Bar Settings", "Sets the ending time used for the controller time bar.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snTimeBarInterval.Properties
            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Time Bar Interval", pbNumberBag.GetType(), "TimeBarInterval", _
                                        "Time Bar Settings", "Determines how often the the time bar updates its information.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Actual Time Color", GetType(System.Drawing.Color), "ActualTimeColor", _
                                        "Time Bar Settings", "Sets the color used for the actual time in the controller time bar.", Me.ctrlTimeRuler.ActualTimeColor))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Current Time Color", GetType(System.Drawing.Color), "CurrentTimeColor", _
                                        "Time Bar Settings", "Sets the color used for the current time in the controller time bar.", Me.ctrlTimeRuler.CurrentTimeColor))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Automatic Time Scale", GetType(Boolean), "AutomaticTimeScale", _
                                        "Time Bar Settings", "Determines whether the time bar will automatically scale itself when the time nears " & _
                                        "the end of bar.", Me.ctrlTimeRuler.AutomaticTimeScale))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Division Mark Height", Me.ctrlTimeRuler.DivisionMarkFactor.GetType, "DivisionMarkFactor", _
                                        "Time Bar Settings", "Sets the height of the division mark factor in the time bar.", Me.ctrlTimeRuler.DivisionMarkFactor))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Divisions", Me.ctrlTimeRuler.Divisions.GetType, "Divisions", _
                                        "Time Bar Settings", "Sets the number of divisions used for the time bar.", Me.ctrlTimeRuler.Divisions))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Major Interval", Me.ctrlTimeRuler.Divisions.GetType, "MajorInterval", _
                                        "Time Bar Settings", "Sets the major interval used for the time bar.", Me.ctrlTimeRuler.MajorInterval))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Middle Mark Height", Me.ctrlTimeRuler.MiddleMarkFactor.GetType, "MiddleMarkFactor", _
                                        "Time Bar Settings", "Sets the height of the middle mark factor in the time bar.", Me.ctrlTimeRuler.MiddleMarkFactor))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Time Scale", Me.ctrlTimeRuler.TimeScale.GetType, "TimeScale", _
                                        "Time Bar Settings", "Sets the time scale used for the time bar.", Me.ctrlTimeRuler.TimeScale))

            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Update Data Interval", m_iUpdateDataInterval.GetType(), "UpdateDataInterval", _
            '                            "Playback Control", "This controls how often the data is updated during the run of the simulation. " & _
            '                            "For example, it will control how often the progress toolbar is refreshed.", m_iUpdateDataInterval))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Start Paused", GetType(Boolean), "StartPaused", _
                                        "Playback Control", "This determines whether the simulation is paused when " & _
                                        "it starts or if it begins running immediately.", Util.Simulation.StartPaused))

            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enable Sim Recording", GetType(Boolean), "EnableSimRecording", _
            '                            "Playback Control", "If this is true then the simulation recording and playback feature is enabled. " & _
            '                            "If this is turned off it may slightly improve the simulation speed.", Util.Simulation.EnableSimRecording))

         End Sub


        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem() 'Into Form Element

            m_snStartingBarTime.LoadData(oXml, "StartingBarTime")
            m_snEndingBarTime.LoadData(oXml, "EndingBarTime")

            If ScaledNumber.IsValidXml(oXml, "TimeBarInterval") Then
                m_snTimeBarInterval.LoadData(oXml, "TimeBarInterval")
            Else
                Dim iTimeBarInterval As Integer = oXml.GetChildInt("TimeBarInterval", CInt(m_snTimeBarInterval.ActualValue * 1000))
                m_snTimeBarInterval.ActualValue = (iTimeBarInterval / 1000)
            End If
            m_Timer.Interval = CInt(m_snTimeBarInterval.ActualValue * 1000)

            Me.ctrlTimeRuler.StartMillisecond = CLng(m_snStartingBarTime.ValueFromDefaultScale)
            Me.ctrlTimeRuler.EndMillisecond = CLng(m_snEndingBarTime.ValueFromDefaultScale)

            Me.ctrlTimeRuler.ActualTimeColor = Color.FromArgb(oXml.GetChildInt("ActualTimeColor", Me.ctrlTimeRuler.ActualTimeColor.ToArgb))
            Me.ctrlTimeRuler.CurrentTimeColor = Color.FromArgb(oXml.GetChildInt("CurrentTimeColor", Me.ctrlTimeRuler.CurrentTimeColor.ToArgb))
            Me.ctrlTimeRuler.AutomaticTimeScale = oXml.GetChildBool("AutomaticTimeScale", Me.ctrlTimeRuler.AutomaticTimeScale)
            Me.ctrlTimeRuler.DivisionMarkFactor = oXml.GetChildInt("DivisionMarkFactor", Me.ctrlTimeRuler.DivisionMarkFactor)
            Me.ctrlTimeRuler.Divisions = oXml.GetChildInt("Divisions", Me.ctrlTimeRuler.Divisions)
            Me.ctrlTimeRuler.MajorInterval = oXml.GetChildInt("MajorInterval", Me.ctrlTimeRuler.MajorInterval)
            Me.ctrlTimeRuler.MiddleMarkFactor = oXml.GetChildInt("MiddleMarkFactor", Me.ctrlTimeRuler.MiddleMarkFactor)
            Me.ctrlTimeRuler.TimeScale = DirectCast([Enum].Parse(GetType(AnimatGUICtrls.Controls.enumTimeScale), oXml.GetChildString("TimeScale"), True), AnimatGUICtrls.Controls.enumTimeScale)
            oXml.AddChildElement("TimeScale", Me.ctrlTimeRuler.TimeScale.ToString)

            oXml.OutOfElem() 'Outof Form Element

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            m_snStartingBarTime.SaveData(oXml, "StartingBarTime")
            m_snEndingBarTime.SaveData(oXml, "EndingBarTime")
            m_snTimeBarInterval.SaveData(oXml, "TimeBarInterval")

            oXml.AddChildElement("ActualTimeColor", Me.ctrlTimeRuler.ActualTimeColor.ToArgb)
            oXml.AddChildElement("CurrentTimeColor", Me.ctrlTimeRuler.CurrentTimeColor.ToArgb)
            oXml.AddChildElement("AutomaticTimeScale", Me.ctrlTimeRuler.AutomaticTimeScale)
            oXml.AddChildElement("DivisionMarkFactor", Me.ctrlTimeRuler.DivisionMarkFactor)
            oXml.AddChildElement("Divisions", Me.ctrlTimeRuler.Divisions)
            oXml.AddChildElement("MajorInterval", Me.ctrlTimeRuler.MajorInterval)
            oXml.AddChildElement("MiddleMarkFactor", Me.ctrlTimeRuler.MiddleMarkFactor)
            oXml.AddChildElement("TimeScale", Me.ctrlTimeRuler.TimeScale.ToString)

            oXml.OutOfElem()

        End Sub

#End Region

#Region " Events "

#Region " Simulator Events "

        Protected Overrides Sub WndProc(ByRef m As Message)

            Try
                If (m.Msg = Util.WindowsMessages.WM_AM_UPDATE_DATA) Then
                    Me.ctrlTimeRuler.CurrentMillisecond = Util.Application.SimulationInterface.CurrentMillisecond
                    'Util.Application.StatusBar.Panels(0).Text = GenerateStatusBarText(-1)
                ElseIf (m.Msg = Util.WindowsMessages.WM_AM_SIMULATION_ERROR) Then
                    Me.btnSimStart.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.PlayLarge.gif")
                    Me.btnSimStop.Enabled = False

                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    m_Timer.Enabled = False
                    Util.Application.StopSimulation()
                    MessageBox.Show(Util.Application.SimulationInterface.ErrorMessage, "Simulation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                MyBase.WndProc(m)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        'These events are called by the simulationinterface managed class. They communicate back from the 
        'c++ simulation.
        Private Sub OnStartSimulation()

            Try
                Me.btnSimStart.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.PauseLarge.gif")
                Me.ctrlTimeRuler.AllowKeyFrameSelection = False
                Me.btnSimStop.Enabled = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub OnPauseSimulation()

            Try
                Me.btnSimStart.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.PlayLarge.gif")
                Me.ctrlTimeRuler.AllowKeyFrameSelection = True
                Me.btnSimStop.Enabled = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub OnStopSimulation()

            Try
                Me.btnSimStart.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.PlayLarge.gif")
                Me.ctrlTimeRuler.AllowKeyFrameSelection = True
                Me.btnSimStop.Enabled = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub OnResetSimulation()

        End Sub

        Private Sub OnUpdateData()
            'This is sending a message so the bar processes the message asynchronously and does not lock up on this task. The message is 
            'processed in the WndProc method above.
            AnimatGUI.Framework.Util.PostMessage(Me.Handle.ToInt32(), AnimatGUI.Framework.Util.WindowsMessages.WM_AM_UPDATE_DATA, 0, "")
        End Sub

        Private Delegate Sub OnSimulationErrorCallback()

        Private Sub OnSimulationError()
            If Me.InvokeRequired Then
                Dim simErrorCB As New OnSimulationErrorCallback(AddressOf OnSimulationError)
                Me.Invoke(simErrorCB, Nothing)
            Else
                AnimatGUI.Framework.Util.PostMessage(Me.Handle.ToInt32(), AnimatGUI.Framework.Util.WindowsMessages.WM_AM_SIMULATION_ERROR, 0, "")
            End If

        End Sub

        Private Sub m_Timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_Timer.Tick
            Try

                Me.ctrlTimeRuler.CurrentMillisecond = Util.Application.SimulationInterface.CurrentMillisecond
                'Util.Application.StatusBar.Panels(0).Text = GenerateStatusBarText(-1)

                If Util.Simulation.SimulationAtEndTime Then
                    Util.Application.StopSimulation()
                    Util.Simulation.SimulationAtEndTime = False
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
                m_Timer.Enabled = False
            End Try

        End Sub

#End Region

#Region " KeyFrame Events "

        Private Sub OnCursorMoved(ByVal sender As Object, ByVal e As AnimatGUICtrls.Controls.TimeRuler.HooverValueEventArgs)
            Try
                'Util.Application.StatusBar.Panels(0).Text = GenerateStatusBarText(e.Value)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
                m_Timer.Enabled = False
            End Try

        End Sub

        Private Sub OnKeyFrameSelected(ByVal sender As Object, ByVal e As AnimatGUICtrls.Controls.TimeRuler.KeyFrameEventArgs)

            Try
                'First if the currently selected keyframe is a video type then we need to disable 
                'video playback option.
                If Not m_selKeyFrame Is Nothing Then
                    If m_selKeyFrame.KeyFrameType = KeyFrame.enumKeyFrameType.Video Then
                        Util.Application.SimulationInterface.DisableVideoPlayback()
                    End If
                End If

                m_selKeyFrame = e.SelectedKeyFrame

                txtVideoStart.Text = ""
                txtVideoEnd.Text = ""
                txtVideoCurrent.Text = ""
                txtSnapshotTime.Text = ""

                If m_selKeyFrame Is Nothing Then
                    grpVideoControls.Visible = False
                    grpSnapshotControls.Visible = False
                Else
                    If m_selKeyFrame.KeyFrameType = KeyFrame.enumKeyFrameType.Snapshot Then
                        grpVideoControls.Visible = False
                        grpSnapshotControls.Visible = True
                        txtSnapshotTime.Text = m_selKeyFrame.StartMillisecond.ToString()
                    ElseIf m_selKeyFrame.KeyFrameType = KeyFrame.enumKeyFrameType.Video Then
                        grpSnapshotControls.Visible = False
                        grpVideoControls.Visible = True
                        txtVideoStart.Text = m_selKeyFrame.StartMillisecond.ToString()
                        txtVideoEnd.Text = m_selKeyFrame.EndMillisecond.ToString()

                        Util.Application.SimulationInterface.EnableVideoPlayback(m_selKeyFrame.ID)
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
                m_Timer.Enabled = False
            End Try

        End Sub

        Private Sub OnKeyFrameAdded(ByVal sender As Object, ByVal e As AnimatGUICtrls.Controls.TimeRuler.KeyFrameEventArgs)
            Try
                If Not e.SelectedKeyFrame Is Nothing Then
                    e.SelectedKeyFrame.ID = Util.Application.SimulationInterface.AddKeyFrame(e.SelectedKeyFrame.KeyFrameType.ToString(), _
                                                                            CInt(e.SelectedKeyFrame.StartMillisecond), _
                                                                            CInt(e.SelectedKeyFrame.EndMillisecond))
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnKeyFrameRemoved(ByVal sender As Object, ByVal e As AnimatGUICtrls.Controls.TimeRuler.KeyFrameEventArgs)
            Try
                If Not e.SelectedKeyFrame Is Nothing Then
                    Util.Application.SimulationInterface.RemoveKeyFrame(e.SelectedKeyFrame.ID)
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnKeyFrameMoved(ByVal sender As Object, ByVal e As AnimatGUICtrls.Controls.TimeRuler.KeyFrameEventArgs)
            Try
                If Not e.SelectedKeyFrame Is Nothing _
                   AndAlso e.SelectedKeyFrame.KeyFrameType <> KeyFrame.enumKeyFrameType.CurrentFrame Then
                    e.SelectedKeyFrame.ID = Util.Application.SimulationInterface.MoveKeyFrame(e.SelectedKeyFrame.ID, _
                                                                             CInt(e.SelectedKeyFrame.StartMillisecond), _
                                                                             CInt(e.SelectedKeyFrame.EndMillisecond))
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnKeyFrameMoving(ByVal sender As Object, ByVal e As AnimatGUICtrls.Controls.TimeRuler.KeyFrameEventArgs)
            Try
                m_selKeyFrame = e.SelectedKeyFrame

                If Not m_selKeyFrame Is Nothing Then
                    If m_selKeyFrame.KeyFrameType = KeyFrame.enumKeyFrameType.Snapshot Then
                        Me.txtSnapshotTime.Text = m_selKeyFrame.StartMillisecond.ToString()
                    ElseIf m_selKeyFrame.KeyFrameType = KeyFrame.enumKeyFrameType.Video Then
                        Me.txtVideoStart.Text = m_selKeyFrame.StartMillisecond.ToString()
                        Me.txtVideoEnd.Text = m_selKeyFrame.EndMillisecond.ToString()
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub OnCurrentFrameMoved(ByVal sender As Object, ByVal e As AnimatGUICtrls.Controls.TimeRuler.KeyFrameEventArgs)
            Try
                If Not e.SelectedKeyFrame Is Nothing _
                   AndAlso e.SelectedKeyFrame.KeyFrameType <> KeyFrame.enumKeyFrameType.CurrentFrame Then
                    Util.Application.SimulationInterface.MoveSimulationToKeyFrame(e.SelectedKeyFrame.ID)
                Else
                    Util.Application.SimulationInterface.MoveSimulationToKeyFrame("")
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

#Region " Simulation Group Events "

        Private Sub btnSimStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSimStart.Click
            Util.Application.ToggleSimulation()
        End Sub

        Private Sub btnSimStop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSimStop.Click
            Try
                Util.Application.StopSimulation()
                btnSimStop.Enabled = False
                Me.m_Timer.Enabled = False

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

#Region " Video Group Events "

        Private Sub btnVideoStart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVideoStart.Click
            Try

                If m_selKeyFrame Is Nothing Then
                    Throw New System.Exception("There is no key frame currently selected.")
                End If

                If m_selKeyFrame.KeyFrameType <> KeyFrame.enumKeyFrameType.Video Then
                    Throw New System.Exception("The currently selected keyframe is not a video type.")
                End If

                If Not m_selKeyFrame.Playing Then
                    Me.btnVideoStart.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.PauseLarge.gif")
                    Util.Application.SimulationInterface.StartVideoPlayback()
                    m_selKeyFrame.Playing = True
                Else
                    Me.btnVideoStart.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.PlayLarge.gif")
                    Util.Application.SimulationInterface.StopVideoPlayback()
                    m_selKeyFrame.Playing = False
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub btnVideoStepBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVideoStepBack.Click
            Try
                If m_selKeyFrame Is Nothing Then
                    Throw New System.Exception("There is no key frame currently selected.")
                End If

                If m_selKeyFrame.KeyFrameType <> KeyFrame.enumKeyFrameType.Video Then
                    Throw New System.Exception("The currently selected keyframe is not a video type.")
                End If

                If m_selKeyFrame.Playing Then
                    Throw New System.Exception("You can not step the video back while it is currently playing.")
                End If

                Util.Application.SimulationInterface.StepVideoPlayback(-1)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub btnVideoStepForward_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVideoStepForward.Click

            Try
                If m_selKeyFrame Is Nothing Then
                    Throw New System.Exception("There is no key frame currently selected.")
                End If

                If m_selKeyFrame.KeyFrameType <> KeyFrame.enumKeyFrameType.Video Then
                    Throw New System.Exception("The currently selected keyframe is not a video type.")
                End If

                If m_selKeyFrame.Playing Then
                    Throw New System.Exception("You can not step the video forward while it is currently playing.")
                End If

                Util.Application.SimulationInterface.StepVideoPlayback(1)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub txtVideoStart_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtVideoStart.Leave
            Try

                If Not m_selKeyFrame Is Nothing Then
                    If txtVideoStart.Text.Trim.Length = 0 Then
                        Throw New System.Exception("The start time of the video must be numeric.")
                    End If

                    If Not IsNumeric(txtVideoStart.Text) Then
                        Throw New System.Exception("The start time of the video must be numeric.")
                    End If

                    Dim lStartTime As Long = CLng(txtVideoStart.Text)
                    Dim lEndTime As Long = CLng(txtVideoEnd.Text)

                    If lStartTime < 0 Then
                        Throw New System.Exception("The start time must be greater than zero.")
                    End If

                    If lStartTime >= lEndTime Then
                        Throw New System.Exception("The start time can not be greater than or equal to the end time.")
                    End If

                    m_selKeyFrame.MoveFrame(lStartTime, lEndTime, Me.ctrlTimeRuler)

                    If Not m_selKeyFrame Is Nothing Then
                        m_selKeyFrame.ID = Util.Application.SimulationInterface.MoveKeyFrame(m_selKeyFrame.ID, _
                                                                            CInt(m_selKeyFrame.StartMillisecond), _
                                                                            CInt(m_selKeyFrame.EndMillisecond))
                    End If

                End If

            Catch ex As System.Exception
                txtVideoStart.Text = m_selKeyFrame.StartMillisecond.ToString()
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub txtVideoEnd_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtVideoEnd.Leave
            Try

                If Not m_selKeyFrame Is Nothing Then
                    If txtVideoEnd.Text.Trim.Length = 0 Then
                        Throw New System.Exception("The end time of the video must be numeric.")
                    End If

                    If Not IsNumeric(txtVideoEnd.Text) Then
                        Throw New System.Exception("The end time of the video must be numeric.")
                    End If

                    Dim lStartTime As Long = CLng(txtVideoStart.Text)
                    Dim lEndTime As Long = CLng(txtVideoEnd.Text)

                    If lStartTime < 0 Then
                        Throw New System.Exception("The end time must be greater than zero.")
                    End If

                    If lStartTime >= lEndTime Then
                        Throw New System.Exception("The end time can not be greater than or equal to the end time.")
                    End If

                    m_selKeyFrame.MoveFrame(lStartTime, lEndTime, Me.ctrlTimeRuler)

                    If Not m_selKeyFrame Is Nothing Then
                        m_selKeyFrame.ID = Util.Application.SimulationInterface.MoveKeyFrame(m_selKeyFrame.ID, _
                                                                            CInt(m_selKeyFrame.StartMillisecond), _
                                                                            CInt(m_selKeyFrame.EndMillisecond))
                    End If

                End If

            Catch ex As System.Exception
                txtVideoEnd.Text = m_selKeyFrame.StartMillisecond.ToString()
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub btnVideoSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVideoSave.Click

            Try
                If Not m_selKeyFrame Is Nothing Then
                    Dim dlgBrowser As New FolderBrowserDialog
                    ' Set the Help text description for the FolderBrowserDialog.
                    dlgBrowser.Description = "Select the directory where you want to save the video image frames."

                    Dim result As DialogResult = dlgBrowser.ShowDialog()

                    If result = DialogResult.OK Then
                        Util.Application.SimulationInterface.SaveVideo(dlgBrowser.SelectedPath)
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " Snapshot Group Events "

        Private Sub txtSnapshotTime_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSnapshotTime.Leave
            Try

                If Not m_selKeyFrame Is Nothing Then
                    If txtSnapshotTime.Text.Trim.Length = 0 Then
                        Throw New System.Exception("The time of the snapshot must be numeric.")
                    End If

                    If Not IsNumeric(txtSnapshotTime.Text) Then
                        Throw New System.Exception("The time of the snapshot must be numeric.")
                    End If

                    Dim lTime As Long = CLng(txtSnapshotTime.Text)

                    If lTime < 0 Then
                        Throw New System.Exception("The time must be greater than zero.")
                    End If

                    m_selKeyFrame.MoveFrame(lTime, lTime, Me.ctrlTimeRuler)

                    If Not m_selKeyFrame Is Nothing Then
                        m_selKeyFrame.ID = Util.Application.SimulationInterface.MoveKeyFrame(m_selKeyFrame.ID, _
                                                                            CInt(m_selKeyFrame.StartMillisecond), _
                                                                            CInt(m_selKeyFrame.EndMillisecond))
                    End If

                End If

            Catch ex As System.Exception
                txtSnapshotTime.Text = m_selKeyFrame.StartMillisecond.ToString()
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " Project Events "

        Private Sub OnProjectCreated()
            Try
                'btnSimGotoStart.Enabled = True
                'btnSimStepBack.Enabled = True
                btnSimStart.Enabled = True
                btnSimStop.Enabled = False
                'btnSimStepForward.Enabled = True
                'btnSimGotoEnd.Enabled = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub OnProjectClosed()
            Try
                btnSimGotoStart.Enabled = False
                btnSimStepBack.Enabled = False
                btnSimStart.Enabled = False
                btnSimStop.Enabled = False
                btnSimStepForward.Enabled = False
                btnSimGotoEnd.Enabled = False

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub OnProjectLoaded()
            Try
                'btnSimGotoStart.Enabled = True
                'btnSimStepBack.Enabled = True
                btnSimStart.Enabled = True
                btnSimStop.Enabled = False
                'btnSimStepForward.Enabled = True
                'btnSimGotoEnd.Enabled = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub OnSimulationStarting()
            Try
                Me.ctrlTimeRuler.CurrentMillisecond = 0
                Me.ctrlTimeRuler.ActualMillisecond = 0
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub OnSimulationStarted()
            Try
                m_Timer.Interval = CInt(m_snTimeBarInterval.ActualValue * 1000)
                Me.m_Timer.Enabled = True

                btnSimGotoStart.Enabled = False
                btnSimStepBack.Enabled = False
                btnSimStart.Enabled = True
                btnSimStop.Enabled = True
                btnSimStepForward.Enabled = False
                btnSimGotoEnd.Enabled = False

                Me.btnSimStart.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.PauseLarge.gif")
                Me.ctrlTimeRuler.AllowKeyFrameSelection = False

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub OnSimulationPaused()
            Try
                Me.m_Timer.Enabled = False

                btnSimGotoStart.Enabled = False
                btnSimStepBack.Enabled = False
                btnSimStart.Enabled = True
                btnSimStop.Enabled = False
                btnSimStepForward.Enabled = False
                btnSimGotoEnd.Enabled = False

                Me.btnSimStart.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.PlayLarge.gif")
                Me.ctrlTimeRuler.AllowKeyFrameSelection = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub OnSimulationResuming()
            Try

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub OnSimulationStopped()
            Try
                Me.m_Timer.Enabled = False

                btnSimGotoStart.Enabled = False
                btnSimStepBack.Enabled = False
                btnSimStart.Enabled = True
                btnSimStop.Enabled = False
                btnSimStepForward.Enabled = False
                btnSimGotoEnd.Enabled = False

                Me.btnSimStart.ImageIndex = Util.Application.LargeImages.GetImageIndex("AnimatGUI.PlayLarge.gif")
                Me.ctrlTimeRuler.AllowKeyFrameSelection = True
                Me.ctrlTimeRuler.CurrentMillisecond = 0
                Me.ctrlTimeRuler.ActualMillisecond = 0

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#End Region


    End Class

End Namespace
