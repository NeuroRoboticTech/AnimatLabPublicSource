Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects

    Public Class Simulation
        Inherits DataObjects.DragObject

#Region " Enums "

        Public Enum enumVisualSelectionMode
            SelectGraphics = 1
            SelectCollisions = 2
            SelectJoints = 4
            SelectReceptiveFields = 8
            Simulation = 18   '16 + 2 = 0001 0010
        End Enum

        'Settings for determining how fast the simulation plays back to the user.
        Public Enum enumPlaybackControlMode
            FastestPossible = 0   'No delay between time steps. Sim plays back as fast as possible.
            MatchPhysicsStep = 1  'The simulation steps take place and then the app sleeps for the time remaining to match the physics step.
            UsePresetValue = 2    ' The simulation steps take place, and then the app sleeps for the time remaining in a preset time duration.
        End Enum

#End Region

#Region " Attributes "

        Protected m_iUpdateDataInterval As Integer = 250
        Protected m_bStartPaused As Boolean = False
        Protected m_bUseReleaseLibraries As Boolean = True
        Protected m_bEnableSimRecording As Boolean = True
        Protected m_ePlaybackControlMode As enumPlaybackControlMode = enumPlaybackControlMode.FastestPossible
        Protected m_snPresetPlaybackTimeStep As ScaledNumber

        Protected m_doEnvironment As New DataObjects.Physical.Environment(Me)

        Protected m_aryToolHolders As New Collections.SortedToolHolders(Me)
        Protected m_iNewToolHolderIndex As Integer = 0

        Protected m_aryProjectStimuli As New Collections.SortedStimuli(Me)
        Protected m_iNewStimuliIndex As Integer = 0

        Protected m_tnToolViewers As Crownwood.DotNetMagic.Controls.Node
        Protected m_tnExternalStimuli As Crownwood.DotNetMagic.Controls.Node
        Protected m_aryHudItems As New Collections.SortedHudItems(Me)

        Protected m_strFilePath As String = ""
        Protected m_strFileName As String = ""

        Protected m_nodePlaybackControl As Crownwood.DotNetMagic.Controls.Node

        Protected m_strExternalStimuli As String = ""

        Protected m_bSetSimEnd As Boolean = False
        Protected m_snSimEndTime As ScaledNumber
        Protected m_bSimulationAtEndTime As Boolean = False

        Protected m_strAPI_File As String = ""

        Protected m_eVisualSelectionMode As enumVisualSelectionMode
        Protected m_ePrevSelMode As enumVisualSelectionMode
        Protected m_bAddBodiesMode As Boolean = False

        Protected m_iFrameRate As Integer = 30

#End Region

#Region " Properties "

#Region "DragObject Properties"

        Public Overrides Property ItemName As String
            Get
                Return Me.Name()
            End Get
            Set(value As String)
                Me.Name = value
            End Set
        End Property

        Public Overrides ReadOnly Property CanBeCharted As Boolean
            Get
                Return True
            End Get
        End Property

#End Region

        Public ReadOnly Property AnimatModule() As String
            Get
                Return Util.Application.Physics.Name & "AnimatSim_VC" & Util.Application.SimVCVersion & Util.Application.RuntimeModePrefix & Util.Application.Physics.LibraryVersionPrefix & Util.Application.Physics.BinaryModPrefix & ".dll"
            End Get
        End Property

        <Browsable(False)> _
         Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Simulation.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property AllowTreeviewNameEdit() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Property UpdateDataInterval() As Integer
            Get
                Return m_iUpdateDataInterval
            End Get
            Set(ByVal Value As Integer)
                If Value <= 0 Then
                    Throw New System.Exception("You can not set the update data interval to be less than or equal to zero!")
                End If

                m_iUpdateDataInterval = Value
            End Set
        End Property

        Public Property StartPaused() As Boolean
            Get
                Return m_bStartPaused
            End Get
            Set(ByVal Value As Boolean)
                m_bStartPaused = Value
            End Set
        End Property

        Public Property EnableSimRecording() As Boolean
            Get
                Return m_bEnableSimRecording
            End Get
            Set(ByVal Value As Boolean)
                m_bEnableSimRecording = Value
            End Set
        End Property

        Public Overridable Property LogLevel() As ManagedAnimatInterfaces.ILogger.enumLogLevel
            Get
                Return Util.Logger.TraceLevel
            End Get
            Set(ByVal Value As ManagedAnimatInterfaces.ILogger.enumLogLevel)
                Util.Logger.TraceLevel = Value
            End Set
        End Property

        Public Overridable ReadOnly Property Environment() As DataObjects.Physical.Environment
            Get
                Return m_doEnvironment
            End Get
        End Property

        Public Overridable ReadOnly Property ToolHolders() As Collections.SortedToolHolders
            Get
                Return m_aryToolHolders
            End Get
        End Property

        Public Overridable Property NewToolHolderIndex() As Integer
            Get
                Return m_iNewToolHolderIndex
            End Get
            Set(ByVal Value As Integer)
                m_iNewToolHolderIndex = Value
            End Set
        End Property

        Public Overridable ReadOnly Property ProjectStimuli() As Collections.SortedStimuli
            Get
                Return m_aryProjectStimuli
            End Get
        End Property

        Public Overridable Property NewStimuliIndex() As Integer
            Get
                Return m_iNewStimuliIndex
            End Get
            Set(ByVal Value As Integer)
                m_iNewStimuliIndex = Value
            End Set
        End Property

        Public ReadOnly Property ToolViewersTreeNode() As Crownwood.DotNetMagic.Controls.Node
            Get
                Return m_tnToolViewers
            End Get
        End Property

        Public ReadOnly Property StimuliTreeNode() As Crownwood.DotNetMagic.Controls.Node
            Get
                Return m_tnExternalStimuli
            End Get
        End Property

        Public ReadOnly Property PlaybackControlTreeNode() As Crownwood.DotNetMagic.Controls.Node
            Get
                Return m_nodePlaybackControl
            End Get
        End Property

        Public Overridable ReadOnly Property SimPhysicsSystem() As String
            Get
                Return Util.Application.Physics.Name
            End Get
        End Property

        Public Overridable Property SimPhysicsLibraryVersion() As TypeHelpers.DataTypeID
            Get
                Return Util.Application.Physics.LibraryVersion
            End Get
            Set(value As TypeHelpers.DataTypeID)
                Util.Application.Physics.LibraryVersion = value
            End Set
        End Property

        Public Overridable Property SetSimulationEnd() As Boolean
            Get
                Return m_bSetSimEnd
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("SetEndSimTime", Value.ToString, True)

                m_bSetSimEnd = Value

                'Refresh the property grid 
                If Not Util.ProjectWorkspace Is Nothing Then
                    Util.ProjectWorkspace.RefreshProperties()
                End If
            End Set
        End Property

        Public Overridable Property SimulationEndTime() As ScaledNumber
            Get
                Return m_snSimEndTime
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The simulation end time must be greater than 0.")
                End If

                SetSimData("EndSimTime", Value.ActualValue.ToString, True)

                m_snSimEndTime.CopyData(Value)
            End Set
        End Property

        ''' \brief  When the simulation end time is set to true then the simulation should end after
        ''' 		the user defined time period. While the simulation is processing it checks these values
        ''' 		and when it reaches the correct time it fires a NeedToStopSimulation event. This event
        ''' 		then sets this boolean to true. Each time the SimulationController timer is called it checks
        ''' 		this value. If it is true then it stops the simulation, and resets the value. This was done this
        ''' 		way because it is not possible to call StopSimulation directly from the NeedToStopSimulation event
        ''' 		because the simulation must be running. This creates a blocking event, locking the system up. We need
        ''' 		for the event code to continue on about its business and let the simulation loop keep running, and then
        ''' 		later this thread can call StopSimulation.
        '''
        ''' \value  .
        Public Overridable Property SimulationAtEndTime() As Boolean
            Get
                Return m_bSimulationAtEndTime
            End Get
            Set(ByVal value As Boolean)
                m_bSimulationAtEndTime = value
            End Set
        End Property

        Public Overridable Property APIFile() As String
            Get
                Return m_strAPI_File
            End Get
            Set(ByVal Value As String)
                m_strAPI_File = Value
            End Set
        End Property

        Public Overridable Property VisualSelectionMode() As enumVisualSelectionMode
            Get
                Return m_eVisualSelectionMode
            End Get
            Set(ByVal value As enumVisualSelectionMode)
                If value <> m_eVisualSelectionMode Then
                    Me.SetSimData("VisualSelectionMode", CType(value, Integer).ToString, True)

                    Dim ePrevMode As enumVisualSelectionMode = m_eVisualSelectionMode

                    m_eVisualSelectionMode = value
                    SetVisualSelectionMode()

                    'If we are switching modes then anything selected must be in the previous mode, and 
                    'thus it should not be selected now in the current mode. So lets clear out any selected items.
                    DeselectBodyPartSelections(ePrevMode)
                End If
            End Set
        End Property

        Public Overridable Property AddBodiesMode() As Boolean
            Get
                Return m_bAddBodiesMode
            End Get
            Set(ByVal value As Boolean)
                Me.SetSimData("AddBodiesMode", value.ToString, True)
                m_bAddBodiesMode = value
            End Set
        End Property

        Public Overridable Property FrameRate() As Integer
            Get
                Return m_iFrameRate
            End Get
            Set(ByVal value As Integer)
                If value <= 1 OrElse value > 1000 Then
                    Throw New System.Exception("The frame rate must be greater than 1 and less than 1000.")
                End If

                Me.SetSimData("FrameRate", value.ToString, True)
                m_iFrameRate = value
            End Set
        End Property

        Public Overridable Property PlaybackControlMode() As enumPlaybackControlMode
            Get
                Return m_ePlaybackControlMode
            End Get
            Set(ByVal value As enumPlaybackControlMode)
                Me.SetSimData("PlaybackControlMode", CStr(CType(value, Integer)), True)
                m_ePlaybackControlMode = value
                If Not Util.ProjectWorkspace Is Nothing Then
                    Util.ProjectWorkspace.RefreshProperties()
                End If
            End Set
        End Property

        Public Overridable Property PresetPlaybackTimeStep() As ScaledNumber
            Get
                Return m_snPresetPlaybackTimeStep
            End Get
            Set(ByVal value As ScaledNumber)
                If value.ActualValue < 0 Then
                    Throw New System.Exception("The preset playback time step must be greater than 0.")
                End If

                SetSimData("PresetPlaybackTimeStep", value.ActualValue.ToString, True)

                m_snPresetPlaybackTimeStep.CopyData(value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New()
            MyBase.New(Nothing)

            Util.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "Sim.New started")

            m_strName = "Simulation"
            m_strID = "Simulator"
            m_snSimEndTime = New AnimatGUI.Framework.ScaledNumber(Me, "SimEndTime", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_snPresetPlaybackTimeStep = New AnimatGUI.Framework.ScaledNumber(Me, "PresetPlaybackTimeStep", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")

            Util.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "Sim.New Adding handlers")

            'These events are called when the simulation is starting or resuming so that we can initialize certain objects like stimuli and data charts.
            AddHandlers()

            m_thDataTypes.DataTypes.Clear()

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("SimulationRealTimeToStep", "Time for Sim Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("PlaybackAdditionRealTimeToStep", "Playback Time added for Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("TotalRealTimeForStep", "Total Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("TotalRealTimeForStepSmoothed", "Smoothed Total Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("PhysicsRealTimeForStep", "Physics Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("NeuralRealTimeForStep", "Neural Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("ExternalStimuliRealTimeForStep", "External Stimuli Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("DataChartRealTimeForStep", "Data Chart Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("SimRecorderRealTimeForStep", "Simulation Recorder Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("ActualFrameRate", "Frame Rate", "FPS", "FPS", 0, 60))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("RealTime", "Real vs Sim Time", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("RemainingStepTime", "Remaining Step Time", "Seconds", "s", 0, 1))
            m_thDataTypes.ID = "SimulationRealTimeToStep"

            Util.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "Sim.New Finished")
        End Sub

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            Util.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "Sim.New started")

            m_strName = "Simulation"
            m_strID = "Simulator"
            m_snSimEndTime = New AnimatGUI.Framework.ScaledNumber(Me, "SimEndTime", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_snPresetPlaybackTimeStep = New AnimatGUI.Framework.ScaledNumber(Me, "PresetPlaybackTimeStep", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")

            Util.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "Sim.New Adding handlers")

            'These events are called when the simulation is starting or resuming so that we can initialize certain objects like stimuli and data charts.
            AddHandlers()

            m_thDataTypes.DataTypes.Clear()

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("SimulationRealTimeToStep", "Time for Sim Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("PlaybackAdditionRealTimeToStep", "Playback Time added for Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("TotalRealTimeForStep", "Total Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("TotalRealTimeForStepSmoothed", "Smoothed Total Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("PhysicsRealTimeForStep", "Physics Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("NeuralRealTimeForStep", "Neural Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("ExternalStimuliRealTimeForStep", "External Stimuli Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("DataChartRealTimeForStep", "Data Chart Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("SimRecorderRealTimeForStep", "Simulation Recorder Time For Step", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("ActualFrameRate", "Frame Rate", "FPS", "FPS", 0, 60))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("RealTime", "Real vs Sim Time", "Seconds", "s", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("RemainingStepTime", "Remaining Step Time", "Seconds", "s", 0, 1))
            m_thDataTypes.ID = "SimulationRealTimeToStep"

            Util.Logger.LogMsg(ManagedAnimatInterfaces.ILogger.enumLogLevel.Detail, "Sim.New Finished")
        End Sub

        Public Overridable Sub AddHandlers()
            Try
                AddHandler Util.Application.SimulationStarting, AddressOf Me.OnSimulationStarting
                AddHandler Util.Application.SimulationResuming, AddressOf Me.OnSimulationResuming
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Sub RemoveHandlers()
            Try
                RemoveHandler Util.Application.SimulationStarting, AddressOf Me.OnSimulationStarting
                RemoveHandler Util.Application.SimulationResuming, AddressOf Me.OnSimulationResuming
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Function FindDragObject(strStructureName As String, strDataItemID As String, Optional bThrowError As Boolean = True) As DragObject
            Throw New System.Exception("FindDragObject not implemented")
        End Function

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            'Dim oSim As New Simulation(doParent)
            Return Nothing
        End Function

        Public Overridable Overloads Function CreateObject(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strClassType As String, _
                                                           ByVal doParent As AnimatGUI.Framework.DataObject) As Framework.DataObject
            oXml.IntoElem()

            'If we have a partype listed then it gives the complete assemblyname and classname that we 
            'can use to load with the Util.LoadClass function. Otherwise load things the old, non-modular way.
            Dim strClass As String = oXml.GetChildString("PartType")
            Dim aryClassName() As String = Split(strClass, ".")
            Dim strAssembly As String = aryClassName(0)
            oXml.OutOfElem()

            Dim doObject As Framework.DataObject = DirectCast(Util.LoadClass(strAssembly, strClass, doParent), Framework.DataObject)
            Return doObject

        End Function

        Public Overridable Overloads Function CreateObject(ByVal strClassType As String, _
                                                           ByVal doParent As AnimatGUI.Framework.DataObject) As Framework.DataObject

            Dim aryClassName() As String = Split(strClassType, ".")
            Dim strAssembly As String = aryClassName(0)
            Dim doObject As Framework.DataObject = DirectCast(Util.LoadClass(strAssembly, strClassType, doParent), Framework.DataObject)
            Return doObject
        End Function

        Public Overridable Overloads Function CreateForm(ByVal strClassType As String, _
                                                        ByVal doParent As AnimatGUI.Framework.DataObject) As Forms.AnimatForm

            Dim aryClassName() As String = Split(strClassType, ".")
            Dim strAssembly As String = aryClassName(0)
            Dim frmObject As Forms.AnimatForm = DirectCast(Util.LoadClass(strAssembly, strClassType, doParent), Forms.AnimatForm)
            Return frmObject
        End Function

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, _
                                                       ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                       Optional ByVal bRootObject As Boolean = False)

            Util.Application.WorkspaceImages.ImageList.ImageSize = New Size(25, 25)
            Util.ProjectWorkspace.TreeView.ImageList = Util.Application.WorkspaceImages.ImageList

            MyBase.CreateWorkspaceTreeView(doParent, tnParentNode, True)

            m_doEnvironment.CreateWorkspaceTreeView(Me, m_tnWorkspaceNode)

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

            If m_tnToolViewers Is Nothing Then m_tnToolViewers = Util.ProjectWorkspace.AddTreeNode(Nothing, "Tool Viewers", "AnimatGUI.Toolbox.gif")
            If m_tnExternalStimuli Is Nothing Then m_tnExternalStimuli = Util.ProjectWorkspace.AddTreeNode(Nothing, "Stimuli", "AnimatGUI.ExternalStimulus.gif")
            If m_nodePlaybackControl Is Nothing Then
                m_nodePlaybackControl = Util.ProjectWorkspace.AddTreeNode(m_tnWorkspaceNode, "Playback Control", "AnimatGUI.RemoteControl.gif")
                m_nodePlaybackControl.Tag = Util.Application.SimulationController
            End If

            Dim doTool As DataObjects.ToolHolder
            For Each deEntry As DictionaryEntry In Util.Simulation.ToolHolders
                doTool = DirectCast(deEntry.Value, DataObjects.ToolHolder)
                doTool.CreateWorkspaceTreeView(Me, m_tnToolViewers)
            Next

            Dim doStimulus As DataObjects.ExternalStimuli.Stimulus
            For Each deEntry As DictionaryEntry In Util.Simulation.ProjectStimuli
                doStimulus = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)
                doStimulus.CreateWorkspaceTreeView(Me, m_tnExternalStimuli)
            Next

        End Sub

        Public Overrides Function CreateObjectListTreeView(ByVal doParent As Framework.DataObject, _
                                                       ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                       ByVal mgrImageList As AnimatGUI.Framework.ImageManager) As Crownwood.DotNetMagic.Controls.Node

            Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateObjectListTreeView(doParent, tnParentNode, mgrImageList)

            m_doEnvironment.CreateObjectListTreeView(Me, tnNode, mgrImageList)

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

            Dim tnToolViewers As Crownwood.DotNetMagic.Controls.Node = Util.AddTreeNode(Nothing, "Tool Viewers", "AnimatGUI.Toolbox.gif", "", mgrImageList)
            Dim tnExternalStimuli As Crownwood.DotNetMagic.Controls.Node = Util.AddTreeNode(Nothing, "Stimuli", "AnimatGUI.ExternalStimulus.gif", "", mgrImageList)
            Dim tnPlaybackControl As Crownwood.DotNetMagic.Controls.Node = Util.AddTreeNode(tnNode, "Playback Control", "AnimatGUI.RemoteControl.gif", "", mgrImageList)
            tnPlaybackControl.Tag = New TypeHelpers.LinkedDataObjectTree(Util.Application.SimulationController.FormHelper)

            Dim doTool As DataObjects.ToolHolder
            For Each deEntry As DictionaryEntry In Util.Simulation.ToolHolders
                doTool = DirectCast(deEntry.Value, DataObjects.ToolHolder)
                doTool.CreateObjectListTreeView(Me, tnToolViewers, mgrImageList)
            Next

            Dim doStimulus As DataObjects.ExternalStimuli.Stimulus
            For Each deEntry As DictionaryEntry In Util.Simulation.ProjectStimuli
                doStimulus = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)
                doStimulus.CreateObjectListTreeView(Me, tnExternalStimuli, mgrImageList)
            Next

            Return tnNode
        End Function

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean
            If m_doEnvironment.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then Return True

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
                Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

                mcExpandAll.Tag = tnSelectedNode
                mcCollapseAll.Tag = tnSelectedNode

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Simulation.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcExpandAll, mcCollapseAll})

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                Return True
            End If

            If tnSelectedNode Is m_tnToolViewers Then
                CreateToolViewerPopupMenu(tnSelectedNode, ptPoint)
                Return True
            End If

            If tnSelectedNode Is m_tnExternalStimuli Then
                CreateStimuliPopupMenu(tnSelectedNode, ptPoint)
                Return True
            End If

            Dim doHolder As DataObjects.ToolHolder
            For Each deEntry As DictionaryEntry In Util.Simulation.ToolHolders
                doHolder = DirectCast(deEntry.Value, DataObjects.ToolHolder)
                If doHolder.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then
                    Return True
                End If
            Next

            Dim doStimulus As DataObjects.ExternalStimuli.Stimulus
            For Each deEntry As DictionaryEntry In Util.Simulation.ProjectStimuli
                doStimulus = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)
                If doStimulus.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then
                    Return True
                End If
            Next

            Return False
        End Function

        Protected Overridable Sub CreateToolViewerPopupMenu(ByVal tnSelected As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point)

            Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Simulation.CreateToolViewerPopupMenu", Util.SecurityMgr)

            If Util.Application.ToolPlugins.Count > 0 Then
                Dim mcAddTool As New System.Windows.Forms.ToolStripMenuItem("Add Data Tool", Util.Application.ToolStripImages.GetImage("AnimatGUI.AddDataTool.gif"))

                For Each doTool As Forms.Tools.ToolForm In Util.Application.ToolPlugins
                    Dim mcTool As New System.Windows.Forms.ToolStripMenuItem(doTool.Name, doTool.TabImage, New EventHandler(AddressOf Me.OnAddToolViewer))
                    mcTool.Tag = doTool
                    mcAddTool.DropDown.Items.Add(mcTool)
                Next

                popup.Items.Add(mcAddTool)
            End If

            Dim mcSepExpand As New ToolStripSeparator()
            Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
            Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

            mcExpandAll.Tag = tnSelected
            mcCollapseAll.Tag = tnSelected

            ' Create the popup menu object
            popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcSepExpand, mcExpandAll, mcCollapseAll})

            Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

            Return

        End Sub

        Protected Overridable Sub CreateStimuliPopupMenu(ByVal tnSelected As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point)

            Dim mcExpandAll As New System.Windows.Forms.ToolStripMenuItem("Expand All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Expand.gif"), New EventHandler(AddressOf Me.OnExpandAll))
            Dim mcCollapseAll As New System.Windows.Forms.ToolStripMenuItem("Collapse All", Util.Application.ToolStripImages.GetImage("AnimatGUI.Collapse.gif"), New EventHandler(AddressOf Me.OnCollapseAll))

            mcExpandAll.Tag = tnSelected
            mcCollapseAll.Tag = tnSelected

            ' Create the popup menu object
            Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Simulation.CreateStimuliPopupMenu", Util.SecurityMgr)
            popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcExpandAll, mcCollapseAll})

            Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

            Return

        End Sub


        'This will deselect all body part items. This is used when switching between visual selection modes.
        Protected Sub DeselectBodyPartSelections(ByVal ePrevMode As enumVisualSelectionMode)

            If Not Util.ProjectWorkspace Is Nothing AndAlso Not Util.ProjectWorkspace.TreeView Is Nothing AndAlso Not Util.ProjectWorkspace.TreeView.SelectedNodes Is Nothing Then
                Dim doPart As AnimatGUI.DataObjects.Physical.BodyPart
                Dim aryItems As New ArrayList()
                For Each tvItem As Crownwood.DotNetMagic.Controls.Node In Util.ProjectWorkspace.TreeView.SelectedNodes
                    If Not tvItem.Tag Is Nothing Then
                        If Util.IsTypeOf(tvItem.Tag.GetType, GetType(AnimatGUI.DataObjects.Physical.BodyPart), False) Then
                            doPart = DirectCast(tvItem.Tag, AnimatGUI.DataObjects.Physical.BodyPart)

                            'If the selection mode does not match the current mode then deselect it.
                            If doPart.DefaultVisualSelectionMode <> Me.VisualSelectionMode OrElse ePrevMode = enumVisualSelectionMode.SelectReceptiveFields Then
                                aryItems.Add(doPart)
                            End If
                        End If
                    End If
                Next

                For Each doPart In aryItems
                    doPart.DeselectItem()
                Next
            End If

        End Sub

        Protected Sub SetVisualSelectionMode()

            If m_eVisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectCollisions Then
                Util.Application.SelGraphicsToolStripButton.Checked = False
                Util.Application.SelCollisionToolStripButton.Checked = True
                Util.Application.SelJointsToolStripButton.Checked = False
                Util.Application.SelRecFieldsToolStripButton.Checked = False
                Util.Application.SelSimToolStripButton.Checked = False

                Util.Application.GraphicsObjectsToolStripMenuItem.Checked = False
                Util.Application.CollisionObjectsToolStripMenuItem.Checked = True
                Util.Application.JointsToolStripMenuItem.Checked = False
                Util.Application.ReceptiveFieldsToolStripMenuItem.Checked = False
                Util.Application.SimulationToolStripMenuItem.Checked = False

            ElseIf m_eVisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectGraphics Then
                Util.Application.SelGraphicsToolStripButton.Checked = True
                Util.Application.SelCollisionToolStripButton.Checked = False
                Util.Application.SelJointsToolStripButton.Checked = False
                Util.Application.SelRecFieldsToolStripButton.Checked = False
                Util.Application.SelSimToolStripButton.Checked = False

                Util.Application.GraphicsObjectsToolStripMenuItem.Checked = True
                Util.Application.CollisionObjectsToolStripMenuItem.Checked = False
                Util.Application.JointsToolStripMenuItem.Checked = False
                Util.Application.ReceptiveFieldsToolStripMenuItem.Checked = False
                Util.Application.SimulationToolStripMenuItem.Checked = False

            ElseIf m_eVisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectJoints Then
                Util.Application.SelGraphicsToolStripButton.Checked = False
                Util.Application.SelCollisionToolStripButton.Checked = False
                Util.Application.SelJointsToolStripButton.Checked = True
                Util.Application.SelRecFieldsToolStripButton.Checked = False
                Util.Application.SelSimToolStripButton.Checked = False

                Util.Application.GraphicsObjectsToolStripMenuItem.Checked = False
                Util.Application.CollisionObjectsToolStripMenuItem.Checked = False
                Util.Application.JointsToolStripMenuItem.Checked = True
                Util.Application.ReceptiveFieldsToolStripMenuItem.Checked = False
                Util.Application.SimulationToolStripMenuItem.Checked = False

            ElseIf m_eVisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.SelectReceptiveFields Then
                Util.Application.SelGraphicsToolStripButton.Checked = False
                Util.Application.SelCollisionToolStripButton.Checked = False
                Util.Application.SelJointsToolStripButton.Checked = False
                Util.Application.SelRecFieldsToolStripButton.Checked = True
                Util.Application.SelSimToolStripButton.Checked = False

                Util.Application.GraphicsObjectsToolStripMenuItem.Checked = False
                Util.Application.CollisionObjectsToolStripMenuItem.Checked = False
                Util.Application.JointsToolStripMenuItem.Checked = False
                Util.Application.ReceptiveFieldsToolStripMenuItem.Checked = True
                Util.Application.SimulationToolStripMenuItem.Checked = False

            ElseIf m_eVisualSelectionMode = DataObjects.Simulation.enumVisualSelectionMode.Simulation Then
                Util.Application.SelGraphicsToolStripButton.Checked = False
                Util.Application.SelCollisionToolStripButton.Checked = False
                Util.Application.SelJointsToolStripButton.Checked = False
                Util.Application.SelRecFieldsToolStripButton.Checked = False
                Util.Application.SelSimToolStripButton.Checked = True

                Util.Application.GraphicsObjectsToolStripMenuItem.Checked = False
                Util.Application.CollisionObjectsToolStripMenuItem.Checked = False
                Util.Application.JointsToolStripMenuItem.Checked = False
                Util.Application.ReceptiveFieldsToolStripMenuItem.Checked = False
                Util.Application.SimulationToolStripMenuItem.Checked = True

            End If

            If Util.Application.AddPartToolStripButton.Checked AndAlso Util.Simulation.VisualSelectionMode <> Simulation.enumVisualSelectionMode.SelectCollisions Then
                Util.Application.AddPartToolStripButton.Checked = False
            End If

            RaiseEvent VisualSelectionModeChanged()
        End Sub

        ''' \brief  Enables/Disables the visual selection modes while the simulation is running.
        ''' 		
        ''' \details When the simulation is running then <b>ONLY</b> the simulation mode is available.
        ''' 		 You cannot edit or move the other parts. You must pause the simulation if you want to do that.
        ''' 		 This method ensures that only the simulation mode can be selected.
        '''
        ''' \author dcofer
        ''' \date   3/27/2011
        '''
        ''' \param  bSimStarting    true to simulation starting. 
        Public Sub SetVisualSelectionModeForSimStarting(ByVal bSimStarting As Boolean)

            If bSimStarting Then
                Util.Application.SelGraphicsToolStripButton.Enabled = False
                Util.Application.SelCollisionToolStripButton.Enabled = False
                Util.Application.SelJointsToolStripButton.Enabled = False
                Util.Application.SelRecFieldsToolStripButton.Enabled = False
                Util.Application.SelSimToolStripButton.Enabled = True

                Util.Application.GraphicsObjectsToolStripMenuItem.Enabled = False
                Util.Application.CollisionObjectsToolStripMenuItem.Enabled = False
                Util.Application.JointsToolStripMenuItem.Enabled = False
                Util.Application.ReceptiveFieldsToolStripMenuItem.Enabled = False
                Util.Application.SimulationToolStripMenuItem.Enabled = True

                m_ePrevSelMode = Me.VisualSelectionMode
                Me.VisualSelectionMode = enumVisualSelectionMode.Simulation
            Else
                Util.Application.SelGraphicsToolStripButton.Enabled = True
                Util.Application.SelCollisionToolStripButton.Enabled = True
                Util.Application.SelJointsToolStripButton.Enabled = True
                Util.Application.SelRecFieldsToolStripButton.Enabled = True
                Util.Application.SelSimToolStripButton.Enabled = True

                Util.Application.GraphicsObjectsToolStripMenuItem.Enabled = True
                Util.Application.CollisionObjectsToolStripMenuItem.Enabled = True
                Util.Application.JointsToolStripMenuItem.Enabled = True
                Util.Application.ReceptiveFieldsToolStripMenuItem.Enabled = True
                Util.Application.SimulationToolStripMenuItem.Enabled = True

                Me.VisualSelectionMode = m_ePrevSelMode
            End If

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("FrameRate", m_iFrameRate.GetType(), "FrameRate", _
                                        "Playback Controls", "Determines the video frame rate of the simulation.", m_iFrameRate))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Playback Control Mode", m_ePlaybackControlMode.GetType(), "PlaybackControlMode", _
                                        "Playback Controls", "Determins how the playback speed of the simulation is controlled. " & _
                                        "If set to fastest possible then no delay will be added between simulation time steps. " & _
                                        "If set to use physics time step then it will attempt to set the playback speed to match physics time step. " & _
                                        "If using a preset value then it will attempt to set the playback speed to match this preset value.", m_ePlaybackControlMode))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snPresetPlaybackTimeStep.Properties
            If m_ePlaybackControlMode = enumPlaybackControlMode.UsePresetValue Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Playback Time Step", pbNumberBag.GetType(), "PresetPlaybackTimeStep", _
                                            "Playback Controls", "Controls the pause between simulation time steps. This lets you control the playback speed of a simulation independent of the integration time step.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Settings", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Physics", SimPhysicsSystem.GetType(), "SimPhysicsSystem", _
                                        "Settings", "The physics system for this simulation.", SimPhysicsSystem, True))

            Dim bLibVersionsReadonly As Boolean = True
            If Util.Application.Physics.AvailableLibraryVersions.Count > 1 Then bLibVersionsReadonly = False
            Dim aryTypes As Collections.DataTypes = Util.Application.Physics.AvailableLibraryVersions

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Library Version", GetType(AnimatGUI.TypeHelpers.DataTypeID), "SimPhysicsLibraryVersion", _
                                        "Settings", "Sets the version of the library to use for the physics simulation.", aryTypes, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                        GetType(AnimatGUI.TypeHelpers.DataTypeIDTypeConverter), bLibVersionsReadonly))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("API File", m_strAPI_File.GetType(), "APIFile", _
                                        "Settings", "APIFile", m_strAPI_File))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Log Level", GetType(ManagedAnimatInterfaces.ILogger.enumLogLevel), "LogLevel", _
                                        "Logging", "Sets the level of logging in the application.", Me.LogLevel))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Set Sim To End", m_bSetSimEnd.GetType(), "SetSimulationEnd", _
                                        "Playback Controls", "If this is true then the simulation will automatically end at the Sim End Time.", m_bSetSimEnd))

            pbNumberBag = m_snSimEndTime.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Sim End Time", pbNumberBag.GetType(), "SimulationEndTime", _
                                        "Playback Controls", "Sets the time at which the simulation will end if the SetSimEnd property is true.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter), Not m_bSetSimEnd))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            m_doEnvironment.ClearIsDirty()
            m_aryToolHolders.ClearIsDirty()
            m_aryProjectStimuli.ClearIsDirty()
            m_aryHudItems.ClearIsDirty()
            m_snSimEndTime.ClearIsDirty()
            m_snPresetPlaybackTimeStep.ClearIsDirty()
        End Sub

        Public Overrides Sub UnitsChanged(ByVal ePrevMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal eNewMass As AnimatGUI.DataObjects.Physical.Environment.enumMassUnits, _
                                          ByVal fltMassChange As Single, _
                                          ByVal ePrevDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal eNewDistance As AnimatGUI.DataObjects.Physical.Environment.enumDistanceUnits, _
                                          ByVal fltDistanceChange As Single)
            m_doEnvironment.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)

            Dim doStim As AnimatGUI.Framework.DataObject
            For Each deEntry As DictionaryEntry In Me.ProjectStimuli
                doStim = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                doStim.UnitsChanged(ePrevMass, eNewMass, fltMassChange, ePrevDistance, eNewDistance, fltDistanceChange)
            Next
        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not m_doEnvironment Is Nothing Then m_doEnvironment.RemoveFromSim(bThrowError)
            m_aryHudItems.Clear()
            m_aryProjectStimuli.Clear()
            m_aryToolHolders.Clear()
        End Sub

#End Region

        Public Overridable Overloads Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Util.Application.AppStatusText = "Loading simulation"

            oXml.IntoChildElement("Simulation")

            Me.UpdateDataInterval = oXml.GetChildInt("UpdateDataInterval", m_iUpdateDataInterval)
            Me.StartPaused = oXml.GetChildBool("StartPaused", m_bStartPaused)
            Me.FrameRate = oXml.GetChildInt("FrameRate", m_iFrameRate)
            Me.EnableSimRecording = oXml.GetChildBool("EnableSimRecording", m_bEnableSimRecording)
            m_strExternalStimuli = oXml.GetChildString("ExternalStimuli", "")
            m_strAPI_File = oXml.GetChildString("APIFile", "")

            If oXml.FindChildElement("SetSimEnd", False) Then
                m_bSetSimEnd = oXml.GetChildBool("SetSimEnd", m_bSetSimEnd)
                m_snSimEndTime.LoadData(oXml, "SimEndTime")
            End If

            If oXml.FindChildElement("PlaybackControlMode", False) Then
                Dim strVal As String = oXml.GetChildString("PlaybackControlMode")
                Me.PlaybackControlMode = DirectCast([Enum].Parse(GetType(enumPlaybackControlMode), oXml.GetChildString("PlaybackControlMode"), True), enumPlaybackControlMode)
                m_snPresetPlaybackTimeStep.LoadData(oXml, "PresetPlaybackTimeStep")
            End If

            m_doEnvironment.LoadData(oXml)

            LoadToolHolders(oXml)
            LoadStimuli(oXml)
            LoadHudItems(oXml)

            oXml.OutOfElem()

            Util.Application.AppStatusText = "Finished loading simulation"

        End Sub

        Protected Overridable Sub LoadStimuli(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try

                Util.Application.AppStatusText = "Loading Stimuli"
                Dim doStim As DataObjects.ExternalStimuli.Stimulus
                If oXml.FindChildElement("Stimuli", False) Then
                    oXml.IntoChildElement("Stimuli")
                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    For iIndex As Integer = 0 To iCount
                        doStim = DirectCast(Util.LoadClass(oXml, iIndex, Me), DataObjects.ExternalStimuli.Stimulus)
                        doStim.LoadData(oXml)
                        m_aryProjectStimuli.Add(doStim.ID, doStim, False)
                    Next

                    oXml.OutOfElem()
                End If

            Catch ex As System.Exception
                'If we hit an erorr then it most likely happened while adding or loading the stimulus.
                'If that is the case then we need to step out of the two elements we stepped into.
                oXml.OutOfElem()
                oXml.OutOfElem()
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub LoadToolHolders(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try

                Util.Application.AppStatusText = "Loading tools"
                Dim iCount As Integer
                Dim doTool As DataObjects.ToolHolder
                If oXml.FindChildElement("ToolViewers", False) Then
                    oXml.IntoChildElement("ToolViewers")

                    iCount = oXml.NumberOfChildren() - 1
                    For iIndex As Integer = 0 To iCount
                        oXml.FindChildByIndex(iIndex)
                        doTool = New DataObjects.ToolHolder(Me)
                        doTool.LoadData(oXml)
                        m_aryToolHolders.Add(doTool.ID, doTool)
                    Next

                    oXml.OutOfElem()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub LoadHudItems(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try

                'Dim iCount As Integer
                'If oXml.FindChildElement("HudItems", False) Then
                '    oXml.IntoChildElement("HudItems")

                '    iCount = oXml.NumberOfChildren() - 1
                '    For iIndex As Integer = 0 To iCount
                '        oXml.FindChildByIndex(iIndex)
                '        doTool = New DataObjects.ToolHolder(Me)
                '        doTool.LoadData(oXml)
                '        m_aryToolHolders.Add(doTool.ID, doTool)
                '    Next

                '    oXml.OutOfElem()
                'Else
                'Add any initialization after the InitializeComponent() call
                'Add a new hud item to display the time and the axis by default
                'TODO: We need a Hud manager code in the GUI. For now just hard code it to have a Hud Text Item.
                m_aryHudItems.Clear()
                Dim doHudItem As New DataObjects.Visualization.HudItem(Me, "HudText", Color.White, New System.Drawing.Point(10, 10), 30, "Time: %3.3f", "Simulator", "Time", "Simulator", "RealTime", 1)
                m_aryHudItems.Add(doHudItem.ID, doHudItem)
                'End If


            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub SaveToolHolder(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Util.Application.AppStatusText = "Saving tools"

            oXml.AddChildElement("ToolViewers")
            oXml.IntoElem()

            Dim doTool As DataObjects.ToolHolder
            For Each deEntry As DictionaryEntry In m_aryToolHolders
                doTool = DirectCast(deEntry.Value, DataObjects.ToolHolder)
                doTool.SaveData(oXml)
            Next

            oXml.OutOfElem()   'Outof ToolViewers element
        End Sub

        Protected Overridable Function SaveStimuli(ByVal oXml As ManagedAnimatInterfaces.IStdXml) As Collection

            Util.Application.AppStatusText = "Saving stimuli"

            oXml.AddChildElement("Stimuli")
            oXml.IntoElem()

            Dim aryStimsToDelete As New Collection
            Dim doStim As DataObjects.ExternalStimuli.Stimulus
            For Each deEntry As DictionaryEntry In m_aryProjectStimuli
                doStim = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)

                If doStim.StimulusNoLongerValid Then
                    aryStimsToDelete.Add(doStim)
                Else
                    doStim.SaveData(oXml)
                End If
            Next

            oXml.OutOfElem()   'Outof Stimuli element

            Return aryStimsToDelete
        End Function

        Protected Overridable Sub CleanupStimsToDelete(ByVal aryStimsToDelete As Collection)
            Util.Application.AppStatusText = "Clearing stimuli to be deleted"

            'Now lets delete any stims that are no longer valid
            For Each doStim As DataObjects.ExternalStimuli.Stimulus In aryStimsToDelete
                m_aryProjectStimuli.Remove(doStim.ID)
                doStim.RemoveWorksapceTreeView()
            Next
        End Sub

        Protected Overridable Sub SaveHudItems(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            oXml.AddChildElement("HudItems")
            oXml.IntoElem()   'Into Hud Items element

            Dim hudItem As DataObjects.Visualization.HudItem
            For Each deItem As DictionaryEntry In m_aryHudItems
                hudItem = DirectCast(deItem.Value, DataObjects.Visualization.HudItem)
                hudItem.SaveData(oXml)
            Next

            oXml.OutOfElem()    'Outof Hud Items element
        End Sub

        Public Overridable Overloads Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Util.Application.AppStatusText = "Saving simulation"

            oXml.AddChildElement("Simulation")
            oXml.IntoElem()

            oXml.AddChildElement("ProjectPath", Util.Application.ProjectPath)
            oXml.AddChildElement("AnimatModule", Me.AnimatModule)
            oXml.AddChildElement("UpdateDataInterval", m_iUpdateDataInterval)
            oXml.AddChildElement("StartPaused", m_bStartPaused)
            oXml.AddChildElement("EnableSimRecording", m_bEnableSimRecording)
            oXml.AddChildElement("SetSimEnd", m_bSetSimEnd)
            oXml.AddChildElement("APIFile", m_strAPI_File)
            m_snSimEndTime.SaveData(oXml, "SimEndTime")

            oXml.AddChildElement("PlaybackControlMode", m_ePlaybackControlMode.ToString)
            m_snPresetPlaybackTimeStep.SaveData(oXml, "PresetPlaybackTimeStep")

            m_doEnvironment.SaveData(oXml)

            SaveToolHolder(oXml)
            Dim aryStimsToDelete As Collection = SaveStimuli(oXml)
            SaveHudItems(oXml)

            oXml.OutOfElem() ' OutOfElem Simulation

            CleanupStimsToDelete(aryStimsToDelete)
        End Sub

        Public Overloads Sub SaveSimulationXml(ByVal strFilename As String)

            Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()

            SaveSimulationXml(oXml, Nothing)

            oXml.Save(strFilename)
        End Sub

        Protected Overridable Sub SaveSimStimuli(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            Dim doStim As DataObjects.ExternalStimuli.Stimulus

            oXml.AddChildElement("ExternalStimuli")
            oXml.IntoElem()

            For Each deEntry As DictionaryEntry In m_aryProjectStimuli
                doStim = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)

                If Util.ExportRobotInterface Is Nothing OrElse Util.ExportRobotInterface.Organism Is doStim.PhysicalStructure Then
                    doStim.SaveSimulationXml(oXml)
                End If
            Next

            oXml.OutOfElem()
        End Sub

        Protected Overridable Sub SaveSimDataCharts(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            oXml.AddChildElement("DataCharts")
            oXml.IntoElem()
            For Each frmWindow As System.Windows.Forms.Form In Util.Application.ChildForms
                If TypeOf frmWindow Is Forms.Tools.ToolForm Then
                    Dim frmViewer As Forms.Tools.ToolForm = DirectCast(frmWindow, Forms.Tools.ToolForm)
                    frmViewer.SaveSimulationXml(oXml)
                End If
            Next
            oXml.OutOfElem()
        End Sub

        Protected Overridable Sub SaveSimWindowMgr(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            If Util.ExportRobotInterface Is Nothing Then
                oXml.AddChildElement("WindowMgr")
                oXml.IntoElem()   'Into WindowMgr element

                oXml.AddChildElement("Hud")
                oXml.IntoElem()   'Into Hud element

                oXml.AddChildElement("Type", "Hud")

                oXml.AddChildElement("HudItems")
                oXml.IntoElem()   'Into Hud Items element

                For Each deItem As DictionaryEntry In m_aryHudItems
                    Dim hudItem As DataObjects.Visualization.HudItem = DirectCast(deItem.Value, DataObjects.Visualization.HudItem)
                    hudItem.SaveSimulationXml(oXml)
                Next

                oXml.OutOfElem()    'Outof Hud Items element
                oXml.OutOfElem()    'Outof Hud element

                oXml.AddChildElement("Windows")
                oXml.IntoElem()  'Into Windows element

                If Util.ExportWindowsToFile Then
                    For Each animatForm As Form In Util.Application.ChildForms
                        If Util.IsTypeOf(animatForm.GetType(), "AnimatGUI.Forms.SimulationWindow", False) Then
                            Dim simWindow As AnimatGUI.Forms.SimulationWindow = DirectCast(animatForm, AnimatGUI.Forms.SimulationWindow)
                            simWindow.GenerateSimWindowXml(oXml)
                        End If
                    Next
                End If

                oXml.OutOfElem()    'Outof Windows element

                oXml.OutOfElem()    'Outof WindowMgr element
            End If
        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddElement("Simulation")

            'If it is standalone then do not save the project path. This is typically used for grid applicaitons, 
            'and the project path needs to be the current directory the exe is located within.
            If Not Util.ExportForStandAloneSim Then
                oXml.AddChildElement("ProjectPath", Util.Application.ProjectPath)
            Else
                oXml.AddChildElement("ProjectPath", "")
            End If

            oXml.AddChildElement("AnimatModule", Me.AnimatModule)
            oXml.AddChildElement("UpdateDataInterval", m_iUpdateDataInterval)
            oXml.AddChildElement("StartPaused", m_bStartPaused)
            oXml.AddChildElement("EnableSimRecording", m_bEnableSimRecording)
            oXml.AddChildElement("SetSimEnd", m_bSetSimEnd)
            oXml.AddChildElement("APIFile", m_strAPI_File)
            m_snSimEndTime.SaveSimulationXml(oXml, Me, "SimEndTime")

            Dim ePlaybackMode As enumPlaybackControlMode = m_ePlaybackControlMode
            If Not Util.ExportRobotInterface Is Nothing Then
                ePlaybackMode = enumPlaybackControlMode.MatchPhysicsStep
            End If

            Dim iVal As Integer = CType(ePlaybackMode, Integer)
            oXml.AddChildElement("PlaybackControlMode", iVal)
            m_snPresetPlaybackTimeStep.SaveSimulationXml(oXml, Me, "PresetPlaybackTimeStep")

            m_doEnvironment.SaveSimulationXml(oXml)

            SaveSimStimuli(oXml)
            SaveSimDataCharts(oXml)
            SaveSimWindowMgr(oXml)

            oXml.OutOfElem()
        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            m_doEnvironment.InitializeAfterLoad()
            m_aryProjectStimuli.InitializeAfterLoad()

        End Sub

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            MyBase.InitializeSimulationReferences()

            m_doEnvironment.InitializeSimulationReferences(bShowError)
            m_aryProjectStimuli.InitializeSimulationReferences(bShowError)
            'm_aryMaterialTypes.InitializeSimulationReferences()

            For Each frmWindow As System.Windows.Forms.Form In Util.Application.ChildForms
                If TypeOf frmWindow Is Forms.Tools.ToolForm Then
                    Dim frmViewer As Forms.Tools.ToolForm = DirectCast(frmWindow, Forms.Tools.ToolForm)
                    frmViewer.InitializeSimulationReferences(bShowError)
                End If
            Next
        End Sub

        Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_doEnvironment Is Nothing Then doObject = m_doEnvironment.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryProjectStimuli Is Nothing Then doObject = m_aryProjectStimuli.FindObjectByID(strID)
            Return doObject

        End Function

        Public Overridable Function FindStimulusByName(ByVal strName As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.ExternalStimuli.Stimulus

            Dim doStim As DataObjects.ExternalStimuli.Stimulus
            For Each deEntry As DictionaryEntry In Me.ProjectStimuli
                doStim = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)

                If doStim.Name = strName Then
                    Return doStim
                End If
            Next

            If bThrowError Then
                Throw New System.Exception("No stimulus with the name '" & strName & "' was found.")
            End If

            Return Nothing
        End Function

#End Region

#End Region

#Region " Events "

        Public Event VisualSelectionModeChanged()

        Protected Overridable Sub OnAddToolViewer(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If TypeOf sender Is ToolStripMenuItem Then
                    Dim mcCommand As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)

                    If Not mcCommand.Tag Is Nothing AndAlso TypeOf mcCommand.Tag Is Forms.Tools.ToolForm Then
                        Dim doTool As Forms.Tools.ToolForm = DirectCast(mcCommand.Tag, Forms.Tools.ToolForm)
                        Util.Application.AddNewTool(doTool)
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#Region " Simulation Events "

        Protected Sub OnSimulationStarting()

            Try
                'Lets setup the external stimuli
                Dim aryDelete As New Collection
                Dim doStim As DataObjects.ExternalStimuli.Stimulus
                For Each deEntry As DictionaryEntry In m_aryProjectStimuli
                    doStim = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)

                    'If the stimulus is no longer valid then delete it.
                    If doStim.StimulusNoLongerValid Then
                        aryDelete.Add(doStim)
                    End If
                Next

                For Each doStim In aryDelete
                    doStim.Delete(False)
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSimulationResuming()

            Try
                ''Lets setup the external stimuli
                'Dim doStim As DataObjects.ExternalStimuli.Stimulus
                'For Each deEntry As DictionaryEntry In m_aryProjectStimuli
                '    doStim = DirectCast(deEntry.Value, DataObjects.ExternalStimuli.Stimulus)
                '    doStim.PrepareForSimulation()
                'Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Sub NotifySimTimeStepChanged()
            SetSimData("TimeStepModified", "", True)
        End Sub

#End Region

#End Region

    End Class

End Namespace
