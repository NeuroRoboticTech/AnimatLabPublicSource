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
Imports System.Reflection

Namespace Framework

    Public MustInherit Class AnimatTest

#Region " Enums "

        Public Enum enumErrorTextType
            Equals
            Contains
            BeginsWith
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
            Debug.WriteLine("Initializing Configuration")

            m_strRootFolder = System.Configuration.ConfigurationManager.AppSettings("RootFolder")
            If m_strRootFolder Is Nothing OrElse m_strRootFolder.Trim.Length = 0 Then
                Throw New System.Exception("Root Folder path was not found in configuration file.")
            End If

            m_bGenerateTempates = CType(System.Configuration.ConfigurationManager.AppSettings("GenerateTemplates"), Boolean)
            m_bAttachServerOnly = CType(System.Configuration.ConfigurationManager.AppSettings("AttachServerOnly"), Boolean)

            Debug.WriteLine("Root Folder: " & m_strRootFolder)
            Debug.WriteLine("Generate Tempates: " & m_bGenerateTempates)
            Debug.WriteLine("Attach Server Only: " & m_bAttachServerOnly)

        End Sub

        Protected Overridable Sub StartApplication(ByVal strProject As String, Optional ByVal bAttachOnly As Boolean = False)
            'Get a new port number each time we spin up a new independent test.
            m_iPort = Util.GetNewPort()
 
            'Start the application.
            StartApplication(strProject, m_iPort, bAttachOnly)
        End Sub

        Protected Overridable Sub StartApplication(ByVal strProject As String, ByVal iPort As Integer, Optional ByVal bAttachOnly As Boolean = False)

            If Not bAttachOnly Then
                Debug.WriteLine("Starting application on port: " & iPort)

                'First just try and attach to an existing app. If one is not found then start the exe.
                If AttachServer(iPort, False) Then
                    Return
                End If

                Dim strArgs = ""
                If strProject.Trim.Length > 0 Then
                    strArgs = "-Project " & strProject
                End If
                strArgs = strArgs & "-Port " & iPort.ToString

                Process.Start(m_strRootFolder & "\bin\AnimatLab2.exe", strArgs)

                Threading.Thread.Sleep(3000)
            End If

            AttachServer(iPort, True)

        End Sub

        Protected Overridable Function AttachServer(ByVal iPort As Integer, ByVal bRepeatAttempt As Boolean) As Boolean

            Debug.WriteLine("Attempting to attach to server on port: " & iPort)

            Dim props As IDictionary = New Hashtable()
            props("name") = "AnimatLab:" & iPort

            m_tcpChannel = New TcpChannel(props, Nothing, Nothing)
            System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(m_tcpChannel, True)
            For iTry As Integer = 0 To 3
                If GetAnimatServer(iPort) Then
                    Debug.WriteLine("Successfully attached to server on port: " & iPort)
                    Return True
                Else
                    Debug.WriteLine("Failed to attached to server on port: " & iPort & ", Attempt: " & iTry)
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
                Debug.WriteLine("Detaching from server")
                System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(m_tcpChannel)
            End If
        End Sub

        Protected Overridable Sub CleanupProjectDirectory()
            'Make sure any left over project directory is cleaned up before starting the test.
            If m_strRootFolder.Length > 0 AndAlso m_strProjectPath.Length > 0 AndAlso m_strProjectName.Length > 0 Then
                Debug.WriteLine("Cleaning up the project directory")
                DeleteDirectory(m_strRootFolder & m_strProjectPath & "\" & m_strProjectName)
            End If
        End Sub

        Protected Overridable Function ExecuteMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Debug.WriteLine("Executing Method: '" & strMethodName & "', Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds)

            Dim oRet As Object = m_oServer.ExecuteMethod(strMethodName, aryParams)
            Threading.Thread.Sleep(iWaitMilliseconds)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Function ExecuteIndirectMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Debug.WriteLine("Executing Indirect Method: '" & strMethodName & "', Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds)

            Dim oRet As Object = m_oServer.ExecuteIndirectMethod(strMethodName, aryParams)
            Threading.Thread.Sleep(iWaitMilliseconds)

            'Check to see if an error dialog is present. If it is then get the error name.
            Dim strFormName As String = DirectCast(ExecuteDirectMethod("ActiveDialogName", Nothing), String)
            If strFormName = "Error" Then
                Dim strError As String = CStr(GetApplicationProperty("ErrorDialogMessage"))
                Throw New System.Exception(strError)
            End If

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Public Overridable Function ExecuteIndirectMethodOnObject(ByVal strPath As String, ByVal strMethod As String, ByVal aryInnerParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Debug.WriteLine("ExecuteIndirectMethodOnObject: Path: '" & strPath & ", Method: " & strMethod & ", Params: '" & Util.ParamsToString(aryInnerParams) & "', Wait: " & iWaitMilliseconds)

            Dim aryParams As Object() = New Object() {strPath, strMethod, aryInnerParams}

            Return ExecuteDirectMethod("ExecuteIndirectMethodOnObject", aryParams)

        End Function

        Public Overridable Function ExecuteAppPropertyMethod(ByVal strPropertyName As String, ByVal strMethodName As String, ByVal aryInnerParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Debug.WriteLine("ExecuteAppPropertyMethod: Path: '" & strPropertyName & ", Method: " & strMethodName & ", Params: '" & Util.ParamsToString(aryInnerParams) & "', Wait: " & iWaitMilliseconds)

            Dim aryParams As Object() = New Object() {strPropertyName, strMethodName, aryInnerParams}

            Return ExecuteDirectMethod("ExecuteAppPropertyMethod", aryParams)

        End Function

        Public Overridable Function ExecuteIndirectAppPropertyMethod(ByVal strPropertyName As String, ByVal strMethodName As String, ByVal aryInnerParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Debug.WriteLine("ExecuteIndirectAppPropertyMethod: Path: '" & strPropertyName & ", Method: " & strMethodName & ", Params: '" & Util.ParamsToString(aryInnerParams) & "', Wait: " & iWaitMilliseconds)

            Dim aryParams As Object() = New Object() {strPropertyName, strMethodName, aryInnerParams}

            Return ExecuteDirectMethod("ExecuteIndirectAppPropertyMethod", aryParams)

        End Function

        Protected Overridable Sub ExecuteMethodAssertError(ByVal strMethodName As String, ByVal aryParams() As Object, ByVal strErrorText As String, _
                                                           Optional ByVal eErrorTextType As enumErrorTextType = enumErrorTextType.EndsWith, _
                                                           Optional ByVal iWaitMilliseconds As Integer = 200)

            Try
                Debug.WriteLine("ExecuteMethodAssertError Method: '" & strMethodName & "', Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds & ", ErrorText: '" & strErrorText & "', ErrorTextType: '" & eErrorTextType.ToString & "'")

                Dim oRet As Object = m_oServer.ExecuteIndirectMethod(strMethodName, aryParams)
                AssertErrorDialogShown(strErrorText, eErrorTextType)

            Catch ex As System.Exception
                Debug.WriteLine("Exception was caught.")

                If Not ex.InnerException Is Nothing Then
                    If eErrorTextType = enumErrorTextType.Contains AndAlso ex.InnerException.Message.Contains(strErrorText) Then
                        Debug.WriteLine("It matched the text")
                        Return
                    ElseIf eErrorTextType = enumErrorTextType.BeginsWith AndAlso ex.InnerException.Message.StartsWith(strErrorText) Then
                        Debug.WriteLine("It matched the text")
                        Return
                    ElseIf eErrorTextType = enumErrorTextType.EndsWith AndAlso ex.InnerException.Message.EndsWith(strErrorText) Then
                        Debug.WriteLine("It matched the text")
                        Return
                    Else
                        Debug.WriteLine("It did not match the text. Message: '" & ex.InnerException.Message & "'")
                        Throw ex
                    End If
                Else
                    Debug.WriteLine("No inner execption was found.")
                    Throw ex
                End If
            End Try
        End Sub

        Protected Overridable Sub OpenDialogAndWait(ByVal strDlgName As String, ByVal oActionMethod As MethodInfo, ByVal aryParams() As Object)

            Debug.WriteLine("OpenDialogAndWait for '" & strDlgName & "'")

            Dim bDlgUp As Boolean = False
            Dim iCount As Integer = 0
            While Not bDlgUp
                If Not oActionMethod Is Nothing Then
                    'Perform the action method
                    oActionMethod.Invoke(Me, aryParams)
                    Debug.WriteLine("Calling actionmethod '" & oActionMethod.ToString & "'")
                End If

                Threading.Thread.Sleep(1000)

                Dim strFormName As String = DirectCast(ExecuteDirectMethod("ActiveDialogName", Nothing), String)
                If strFormName = strDlgName Then
                    bDlgUp = True
                    Debug.WriteLine("Opened '" & strDlgName & "'")
                ElseIf strFormName = "<No Dialog>" Then
                    bDlgUp = False
                    Debug.WriteLine("Dialog was not opened, trying again.")
                Else
                    Throw New System.Exception("The active dialog name does not match the name we are waiting for. Active: '" & strFormName & "', Waiting: '" & strDlgName & "'")
                End If
                iCount = iCount + 1

                If iCount > 5 Then
                    Throw New System.Exception(strDlgName & " dialog would not open.")
                End If
            End While
        End Sub

        Protected Overridable Sub AssertErrorDialogShown(ByVal strErrorMsg As String, ByVal eMatchType As enumErrorTextType)
            Debug.WriteLine("AssertErrorDialogShown. strErrorMsg:, " & strErrorMsg & ", Math Type: " & eMatchType.ToString)

            OpenDialogAndWait("Error", Nothing, Nothing)
            Threading.Thread.Sleep(1000)
            Dim oVal As Object = GetApplicationProperty("ErrorDialogMessage")
            Threading.Thread.Sleep(1000)
            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
            Threading.Thread.Sleep(1000)
            If Not TypeOf oVal Is System.String Then
                Throw New System.Exception("String not returned from error dialog box.")
            End If
            Dim strError As String = CStr(oVal)
            If strError.Trim.Length = 0 Then
                Throw New System.Exception("Error dialog box was not displayed.")
            End If

            Select Case eMatchType
                Case enumErrorTextType.Equals
                    If strError <> strErrorMsg Then
                        Throw New System.Exception("Error did not match.")
                    End If

                Case enumErrorTextType.Contains
                    If Not strError.Contains(strErrorMsg) Then
                        Throw New System.Exception("Error did not match.")
                    End If

                Case enumErrorTextType.BeginsWith
                    If Not strError.StartsWith(strErrorMsg) Then
                        Throw New System.Exception("Error did not match.")
                    End If

                Case enumErrorTextType.EndsWith
                    If Not strError.EndsWith(strErrorMsg) Then
                        Throw New System.Exception("Error did not match.")
                    End If

                Case Else
                    Throw New System.Exception("Inavlid match type provided: " & eMatchType.ToString)
            End Select

            Threading.Thread.Sleep(1000)
            Debug.WriteLine("Error dialog shown correctly.")
        End Sub


        Protected Overridable Function ExecuteDirectMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Debug.WriteLine("Executing Direct Method: '" & strMethodName & "', Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds)

            Dim oRet As Object = m_oServer.ExecuteDirectMethod(strMethodName, aryParams)
            Threading.Thread.Sleep(iWaitMilliseconds)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Function ExecuteActiveDialogMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Debug.WriteLine("ExecuteActiveDialogMethod Method: '" & strMethodName & "', Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds)

            Dim oRet As Object = m_oServer.ExecuteDirectMethod("ExecuteActiveDialogMethod", New Object() {strMethodName, aryParams})
            Threading.Thread.Sleep(iWaitMilliseconds)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Function ExecuteIndirectActiveDialogMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Debug.WriteLine("ExecuteActiveDialogMethod Method: '" & strMethodName & "', Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds)

            Dim oRet As Object = m_oServer.ExecuteDirectMethod("ExecuteIndirecActiveDialogtMethod", New Object() {strMethodName, aryParams})
            Threading.Thread.Sleep(iWaitMilliseconds)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Function GetSimObjectProperty(ByVal strPath As String, ByVal strPropName As String, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
            Debug.WriteLine("GetSimObjectProperty Path: '" & strPath & "', PropName: '" & strPropName & "', Wait: " & iWaitMilliseconds)

            Dim oRet As Object = m_oServer.ExecuteDirectMethod("GetObjectProperty", New Object() {strPath, strPropName})
            Threading.Thread.Sleep(iWaitMilliseconds)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Function UnitTest(ByVal strAssembly As String, ByVal strClassName As String, ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 0) As Object
            Debug.WriteLine("Calling UnitTest Assembly: '" & strAssembly & "', ClassName: '" & strClassName & "', MethodName: '" & strMethodName & "Params: '" & Util.ParamsToString(aryParams) & "', Wait: " & iWaitMilliseconds)

            Dim aryNewParams As Object() = New Object() {strAssembly, strClassName, strMethodName, aryParams}
            Dim oRet As Object = m_oServer.ExecuteDirectMethod("ExecuteObjectMethod", aryNewParams)
            If iWaitMilliseconds > 0 Then
                Threading.Thread.Sleep(iWaitMilliseconds)
            End If

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Function GetApplicationProperty(ByVal strPropertyName As String) As Object
            Debug.WriteLine("GetApplicationProperty PropertyName: '" & strPropertyName & "'")

            Dim oRet As Object = m_oServer.GetProperty(strPropertyName)

            If Not oRet Is Nothing Then Debug.WriteLine("Return: " & oRet.ToString) Else Debug.WriteLine("Return: Nothing")
            Return oRet
        End Function

        Protected Overridable Sub SetApplicationProperty(ByVal strPropertyName As String, ByVal oData As Object)
            Debug.WriteLine("SetApplicationProperty PropertyName: '" & strPropertyName & "', Value: " & oData.ToString)

            m_oServer.SetProperty(strPropertyName, oData)
        End Sub

        Protected Overridable Sub RunSimulationWaitToEnd()

            Threading.Thread.Sleep(1000)

            'Start the simulation
            Debug.WriteLine("Running Simulation")
            m_oServer.ExecuteMethod("ToggleSimulation", Nothing)

            Dim iIdx As Integer = 0
            Dim bDone As Boolean = False
            While Not bDone
                Threading.Thread.Sleep(1000)
                bDone = Not DirectCast(m_oServer.GetProperty("SimIsRunning"), Boolean)
                iIdx = iIdx + 1
                If iIdx = 90 Then
                    Throw New System.Exception("Timed out waiting for simulation to end.")
                End If
            End While

        End Sub

        Protected Overridable Sub DeleteDirectory(ByVal strPath As String)
            Debug.WriteLine("Attempting to delete directory: " & strPath)
            If System.IO.Directory.Exists(strPath) Then
                Debug.WriteLine("Deleting Directory: '" & strPath & "'")
                System.IO.Directory.Delete(strPath, True)
                Debug.WriteLine("Directory deleted")
            Else
                Debug.WriteLine("Directory not found.")
            End If
        End Sub

        Public Overridable Sub AssertMatch(ByVal iFound As Integer, ByVal iExpected As Integer, ByVal strName As String)
            Debug.WriteLine("AssertMatch. Found: " & iFound & ", Expected: " & iExpected & ", Name: " & strName)

            If iFound <> iExpected Then
                Throw New System.Exception("Mimatch for variable '" & strName & "'. Expected: " & iExpected & ", Found: " & iFound)
            End If
        End Sub

#End Region


    End Class

End Namespace
