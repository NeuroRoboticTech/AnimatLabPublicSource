

Public Interface ISimulatorInterface

    Delegate Sub VoidHandler()
    Delegate Sub ErrorHandler(ByVal strError As String)
    Delegate Sub CreateSimHandler(ByRef strXml As String)

#Region "EventSystems"

    Event OnSimulationCreate As CreateSimHandler
    Event SimulationRunning As VoidHandler
    Event NeedToStopSimulation As VoidHandler
    Event HandleNonCriticalError As ErrorHandler
    Event HandleCriticalError As ErrorHandler

    Sub FireNeedToStopSimulationEvent()
    Sub FireHandleNonCriticalErrorEvent(ByVal strError As String)
    Sub FireHandleCriticalErrorEvent(ByVal strError As String)

#End Region

#Region "Properties"

    Function CurrentMillisecond() As Long
    Function Paused() As Boolean
    Function SimRunning() As Boolean
    Function Loaded() As Boolean
    Function SimOpen() As Boolean
    Sub SetProjectPath(ByVal strPath As String)
    Sub SetLogger(ByVal lpLog As ILogger)

#End Region

#Region "SimulationControl"

    Function AddWindow(ByVal hParentWnd As IntPtr, ByVal sWindowXml As String) As Boolean
    Sub RemoveWindow(ByVal hParentWnd As IntPtr)
    Sub OnWindowGetFocus(ByVal sID As String)
    Sub OnWindowLoseFocus(ByVal sID As String)

    Sub CreateAndRunSimulation(ByVal bPaused As Boolean)
    Sub CreateSimulation()
    Sub CreateSimulation(ByVal sXml As String)
    Sub Simulate(ByVal bPaused As Boolean)
    Sub ShutdownSimulation()
    Function StartSimulation() As Boolean
    Function PauseSimulation() As Boolean
    Sub StopSimulation()

    Sub CreateStandAloneSim(ByVal sModuleName As String, ByVal sExePath As String)

    Function ErrorMessage() As String

    Sub SaveSimulationFile(ByVal sFile As String)
    Sub TrackCamera(ByVal bTrackCamera As Boolean, ByVal sLookAtStructureID As String, ByVal sLookAtBodyID As String)

#End Region

#Region "VideoPlayback"

    Function AddKeyFrame(ByVal strType As String, ByVal lStartMillisecond As Long, ByVal lEndMillisecond As Long) As String
    Sub RemoveKeyFrame(ByVal strID As String)
    Function MoveKeyFrame(ByVal strID As String, ByVal lStartMillisecond As Long, ByVal lEndMillisecond As Long) As String
    Sub EnableVideoPlayback(ByVal strID As String)
    Sub DisableVideoPlayback()
    Sub StartVideoPlayback()
    Sub StopVideoPlayback()
    Sub StepVideoPlayback(ByVal iFrameCount As Integer)
    Sub MoveSimulationToKeyFrame(ByVal strID As String)
    Sub SaveVideo(ByVal strPath As String)

#End Region

#Region "HelperMethods"

    Sub ReInitializeSimulation()
    Function RetrieveChartData(ByVal sChartKey As String, ByRef aryTimeData As Single(,), ByRef aryData As Single(,)) As Int32
    Sub GenerateCollisionMeshFile(ByVal sOriginalMeshFile As String, ByVal sCollisionMeshFile As String)
    Sub GetPositionAndRotationFromD3DMatrix(ByVal aryMatrix(,) As Single, ByRef fltPosX As Single, ByRef fltPosY As Single, ByRef fltPosZ As Single, _
                                         ByRef fltRotX As Single, ByRef fltRotY As Single, ByRef fltRotZ As Single)

#End Region

#Region "DataAccess"

    Function AddItem(ByVal sParentID As String, ByVal sItemType As String, ByVal sID As String, ByVal sXml As String, ByVal bThrowError As Boolean) As Boolean
    Function RemoveItem(ByVal sParentID As String, ByVal sItemType As String, ByVal sID As String, ByVal bThrowError As Boolean) As Boolean
    Function SetData(ByVal sID As String, ByVal sDataType As String, ByVal sValue As String, ByVal bThrowError As Boolean) As Boolean
    Function FindItem(ByVal sID As String, ByVal bThrowError As Boolean) As Boolean

#End Region

End Interface
