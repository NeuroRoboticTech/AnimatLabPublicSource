
Public Class SimulatorInterfaceMock
    Implements ISimulatorInterface

    Public Function AddItem(sParentID As String, sItemType As String, sID As String, sXml As String, bThrowError As Boolean, ByVal bDoNotInit As Boolean) As Boolean Implements ISimulatorInterface.AddItem
        Return True
    End Function

    Public Function AddKeyFrame(strType As String, lStartMillisecond As Long, lEndMillisecond As Long) As String Implements ISimulatorInterface.AddKeyFrame
        Return ""
    End Function

    Public Function AddWindow(hParentWnd As System.IntPtr, sWindowType As String, sWindowXml As String) As Boolean Implements ISimulatorInterface.AddWindow
        Return True
    End Function

    Public Sub CreateAndRunSimulation(bPaused As Boolean) Implements ISimulatorInterface.CreateAndRunSimulation

    End Sub

    Public Sub CreateSimulation() Implements ISimulatorInterface.CreateSimulation

    End Sub

    Public Sub CreateSimulation(sXml As String) Implements ISimulatorInterface.CreateSimulation

    End Sub

    Public Function CurrentMillisecond() As Long Implements ISimulatorInterface.CurrentMillisecond
        Return 0
    End Function

    Public Sub DisableVideoPlayback() Implements ISimulatorInterface.DisableVideoPlayback

    End Sub

    Public Sub EnableVideoPlayback(strID As String) Implements ISimulatorInterface.EnableVideoPlayback

    End Sub

    Public Function ErrorMessage() As String Implements ISimulatorInterface.ErrorMessage
        Return ""
    End Function

    Public Function FindItem(sID As String, bThrowError As Boolean) As Boolean Implements ISimulatorInterface.FindItem
        Return False
    End Function

    Public Sub FireHandleCriticalErrorEvent(strError As String) Implements ISimulatorInterface.FireHandleCriticalErrorEvent

    End Sub

    Public Sub FireHandleNonCriticalErrorEvent(strError As String) Implements ISimulatorInterface.FireHandleNonCriticalErrorEvent

    End Sub

    Public Sub FireNeedToStopSimulationEvent() Implements ISimulatorInterface.FireNeedToStopSimulationEvent

    End Sub

    Public Sub ConvertV1MeshFile(ByVal sOriginalMeshFile As String, ByVal NewMeshFile As String, ByVal strTexture As String) Implements ISimulatorInterface.ConvertV1MeshFile

    End Sub

    Public Sub GenerateCollisionMeshFile(ByVal sOriginalMeshFile As String, ByVal sCollisionMeshFile As String, ByVal fltScaleX As Single, ByVal fltScaleY As Single, ByVal fltScaleZ As Single) Implements ISimulatorInterface.GenerateCollisionMeshFile

    End Sub

    Public Event HandleCriticalError(strError As String) Implements ISimulatorInterface.HandleCriticalError

    Public Event HandleNonCriticalError(strError As String) Implements ISimulatorInterface.HandleNonCriticalError

    Public Function Loaded() As Boolean Implements ISimulatorInterface.Loaded
        Return False
    End Function

    Public Function MoveKeyFrame(strID As String, lStartMillisecond As Long, lEndMillisecond As Long) As String Implements ISimulatorInterface.MoveKeyFrame
        Return ""
    End Function

    Public Sub MoveSimulationToKeyFrame(strID As String) Implements ISimulatorInterface.MoveSimulationToKeyFrame

    End Sub

    Public Event NeedToStopSimulation() Implements ISimulatorInterface.NeedToStopSimulation

    Public Event OnSimulationCreate(ByRef strXml As String) Implements ISimulatorInterface.OnSimulationCreate

    Public Sub OnWindowGetFocus(sID As String) Implements ISimulatorInterface.OnWindowGetFocus

    End Sub

    Public Sub OnWindowLoseFocus(sID As String) Implements ISimulatorInterface.OnWindowLoseFocus

    End Sub

    Public Function Paused() As Boolean Implements ISimulatorInterface.Paused
        Return True
    End Function

    Public Function PauseSimulation() As Boolean Implements ISimulatorInterface.PauseSimulation
        Return True
    End Function

    Public Sub ReInitializeSimulation() Implements ISimulatorInterface.ReInitializeSimulation

    End Sub

    Public Function RemoveItem(sParentID As String, sItemType As String, sID As String, bThrowError As Boolean) As Boolean Implements ISimulatorInterface.RemoveItem
        Return True
    End Function

    Public Sub RemoveKeyFrame(strID As String) Implements ISimulatorInterface.RemoveKeyFrame

    End Sub

    Public Sub RemoveWindow(hParentWnd As System.IntPtr) Implements ISimulatorInterface.RemoveWindow

    End Sub

    Public Function RetrieveChartData(sChartKey As String, ByRef aryTimeData(,) As Single, ByRef aryData(,) As Single) As Integer Implements ISimulatorInterface.RetrieveChartData
        Return True
    End Function

    Public Sub SaveSimulationFile(sFile As String) Implements ISimulatorInterface.SaveSimulationFile

    End Sub

    Public Sub SaveVideo(strPath As String) Implements ISimulatorInterface.SaveVideo

    End Sub

    Public Function SetData(sID As String, sDataType As String, sValue As String, bThrowError As Boolean) As Boolean Implements ISimulatorInterface.SetData
        Return True
    End Function

    Public Sub SetLogger(lpLog As ILogger) Implements ISimulatorInterface.SetLogger

    End Sub

    Public Sub SetProjectPath(strPath As String) Implements ISimulatorInterface.SetProjectPath

    End Sub

    Public Sub ShutdownSimulation() Implements ISimulatorInterface.ShutdownSimulation

    End Sub

    Public Function SimOpen() As Boolean Implements ISimulatorInterface.SimOpen
        Return True
    End Function

    Public Function SimRunning() As Boolean Implements ISimulatorInterface.SimRunning
        Return True
    End Function

    Public Sub Simulate(bPaused As Boolean) Implements ISimulatorInterface.Simulate

    End Sub

    Public Event SimulationRunning() Implements ISimulatorInterface.SimulationRunning

    Public Function StartSimulation() As Boolean Implements ISimulatorInterface.StartSimulation
        Return True
    End Function

    Public Sub StartVideoPlayback() Implements ISimulatorInterface.StartVideoPlayback

    End Sub

    Public Sub StepVideoPlayback(iFrameCount As Integer) Implements ISimulatorInterface.StepVideoPlayback

    End Sub

    Public Sub StopSimulation() Implements ISimulatorInterface.StopSimulation

    End Sub

    Public Sub StopVideoPlayback() Implements ISimulatorInterface.StopVideoPlayback

    End Sub

    Public Sub TrackCamera(bTrackCamera As Boolean, sLookAtStructureID As String, sLookAtBodyID As String) Implements ISimulatorInterface.TrackCamera

    End Sub

    Public Function GetPositionAndRotationFromD3DMatrix(ByVal aryTransform(,) As Double) As PositionRotationInfo Implements ISimulatorInterface.GetPositionAndRotationFromD3DMatrix
        Return Nothing
    End Function

    Public Sub CreateStandAloneSim(sModuleName As String, sExePath As String) Implements ISimulatorInterface.CreateStandAloneSim

    End Sub
End Class
