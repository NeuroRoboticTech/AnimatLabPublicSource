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

Namespace Framework

    Public MustInherit Class AnimatTest

#Region " Enums "

        Public Enum enumErrorTextType
            BeginsWith
            Contains
            EndsWith
        End Enum

        Public Enum enumDataComparisonType
            WithinRange
            Average
            Max
            Min
        End Enum

#End Region

#Region "Attributes"

        Protected Shared m_iPort As Integer = 8080
        Protected Shared m_oServer As AnimatServer.Server
        Protected Shared m_tcpChannel As TcpChannel
        Protected Shared m_strRootFolder As String
        Protected Shared m_bGenerateTempates As Boolean = False
        Protected Shared m_bAttachServerOnly As Boolean = False
        Protected m_strProjectName As String = ""
        Protected m_strProjectPath As String = ""
        Protected m_strTestDataPath As String = ""
        Protected m_dblSimEndTime As Double = 15
        Protected m_dblChartEndTime As Double = 8

#End Region

#Region "Mehods"

        Protected Shared Sub InitializeConfiguration()
            m_strRootFolder = System.Configuration.ConfigurationManager.AppSettings("RootFolder")
            If m_strRootFolder Is Nothing OrElse m_strRootFolder.Trim.Length = 0 Then
                Throw New System.Exception("Root Folder path was not found in configuration file.")
            End If

            m_bGenerateTempates = CType(System.Configuration.ConfigurationManager.AppSettings("GenerateTemplates"), Boolean)
            m_bAttachServerOnly = CType(System.Configuration.ConfigurationManager.AppSettings("AttachServerOnly"), Boolean)
        End Sub

        Protected Overridable Sub StartApplication(ByVal strProject As String, Optional ByVal bAttachOnly As Boolean = False)
            'Get a new port number each time we spin up a new independent test.
            m_iPort = Util.GetNewPort()

            'Start the application.
            StartApplication(strProject, m_iPort, bAttachOnly)
        End Sub

        Protected Overridable Sub StartApplication(ByVal strProject As String, ByVal iPort As Integer, Optional ByVal bAttachOnly As Boolean = False)

            If Not bAttachOnly Then
                'First just try and attach to an existing app. If one is not found then start the exe.
                If AttachServer(iPort, False) Then
                    Return
                End If

                Dim strArgs = ""
                If strProject.Trim.Length > 0 Then
                    strArgs = "-Project " & strProject
                End If
                strArgs = strArgs & "-Port " & iPort.ToString

                Process.Start(m_strRootFolder & "\bin\AnimatLab.exe", strArgs)

                Threading.Thread.Sleep(3000)
            End If

            AttachServer(iPort, True)

        End Sub

        Protected Overridable Function AttachServer(ByVal iPort As Integer, ByVal bRepeatAttempt As Boolean) As Boolean

            Dim props As IDictionary = New Hashtable()
            props("name") = "AnimatLab:" & iPort

            m_tcpChannel = New TcpChannel(props, Nothing, Nothing)
            System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(m_tcpChannel, True)
            For iTry As Integer = 0 To 3
                If GetAnimatServer(iPort) Then
                    Return True
                Else
                    If Not bRepeatAttempt Then
                        DetachServer()
                        Return False
                    End If
                    Threading.Thread.Sleep(3000)
                End If
            Next

            DetachServer()
            Return False
        End Function

        Protected Overridable Function GetAnimatServer(ByVal iPort As Integer) As Boolean
            Try
                m_oServer = DirectCast(Activator.GetObject(GetType(AnimatServer.Server), "tcp://localhost:" & iPort & "/AnimatLab"), AnimatServer.Server)
                'Make sure we can actually communicate.
                m_oServer.GetProperty("SimIsRunning")
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Protected Overridable Sub DetachServer()
            If Not m_tcpChannel Is Nothing Then
                System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(m_tcpChannel)
            End If
        End Sub

        Protected Overridable Sub CleanupProjectDirectory()
            'Make sure any left over project directory is cleaned up before starting the test.
            If m_strRootFolder.Length > 0 AndAlso m_strProjectPath.Length > 0 AndAlso m_strProjectName.Length > 0 Then
                DeleteDirectory(m_strRootFolder & m_strProjectPath & "\" & m_strProjectName)
            End If
        End Sub

        Protected Overridable Function ExecuteMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Dim oRet As Object = m_oServer.ExecuteMethod(strMethodName, aryParams)
            Threading.Thread.Sleep(iWaitMilliseconds)
            Return oRet
        End Function

        Protected Overridable Sub ExecuteMethodAssertError(ByVal strMethodName As String, ByVal aryParams() As Object, ByVal strErrorText As String, _
                                                           Optional ByVal eErrorTextType As enumErrorTextType = enumErrorTextType.EndsWith, _
                                                           Optional ByVal iWaitMilliseconds As Integer = 200)

            Try
                Dim oRet As Object = m_oServer.ExecuteMethod(strMethodName, aryParams)
                Throw New System.Exception("Expected exception from call to '" & strMethodName & "' and did not get it.")
            Catch ex As System.Exception
                If Not ex.InnerException Is Nothing Then
                    If eErrorTextType = enumErrorTextType.Contains AndAlso ex.InnerException.Message.Contains(strErrorText) Then
                        Return
                    ElseIf eErrorTextType = enumErrorTextType.BeginsWith AndAlso ex.InnerException.Message.StartsWith(strErrorText) Then
                        Return
                    ElseIf eErrorTextType = enumErrorTextType.EndsWith AndAlso ex.InnerException.Message.EndsWith(strErrorText) Then
                        Return
                    Else
                        Throw ex
                    End If
                Else
                    Throw ex
                End If
            End Try
        End Sub

        Protected Overridable Function ExecuteDirectMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Dim oRet As Object = m_oServer.ExecuteDirectMethod(strMethodName, aryParams)
            Threading.Thread.Sleep(iWaitMilliseconds)
            Return oRet
        End Function

        Protected Overridable Function ExecuteActiveDialogMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Dim oRet As Object = m_oServer.ExecuteDirectMethod("ExecuteActiveDialogMethod", New Object() {strMethodName, aryParams})
            Threading.Thread.Sleep(iWaitMilliseconds)
            Return oRet
        End Function

        Protected Overridable Function GetSimObjectProperty(ByVal strPath As String, ByVal strPropName As String, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Dim oRet As Object = m_oServer.ExecuteDirectMethod("GetObjectProperty", New Object() {strPath, strPropName})
            Threading.Thread.Sleep(iWaitMilliseconds)
            Return oRet
        End Function

        Protected Overridable Function UnitTest(ByVal strAssebly As String, ByVal strClassName As String, ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 0) As Object
            Dim aryNewParams As Object() = New Object() {strAssebly, strClassName, strMethodName, aryParams}
            Dim oRet As Object = m_oServer.ExecuteDirectMethod("ExecuteObjectMethod", aryNewParams)
            If iWaitMilliseconds > 0 Then
                Threading.Thread.Sleep(iWaitMilliseconds)
            End If
            Return oRet
        End Function

        Protected Overridable Sub RunSimulationWaitToEnd()

            Threading.Thread.Sleep(1000)

            'Start the simulation
            m_oServer.ExecuteMethod("ToggleSimulation", Nothing)

            Dim iIdx As Integer = 0
            Dim bDone As Boolean = False
            While Not bDone
                Threading.Thread.Sleep(1000)
                bDone = Not DirectCast(m_oServer.GetProperty("SimIsRunning"), Boolean)
                iIdx = iIdx + 1
                If iIdx = 500 Then
                    Throw New System.Exception("Timed out waiting for simulation to end.")
                End If
            End While

        End Sub

        Protected Overridable Sub DeleteDirectory(ByVal strPath As String)
            If System.IO.Directory.Exists(strPath) Then
                System.IO.Directory.Delete(strPath, True)
            End If
        End Sub


#End Region


    End Class

End Namespace
