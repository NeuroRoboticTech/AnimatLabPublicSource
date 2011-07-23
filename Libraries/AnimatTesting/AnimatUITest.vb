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
Public MustInherit Class AnimatUITest

#Region "Attributes"

    Protected m_iPort As Integer = -1
    Protected m_oServer As AnimatServer.Server
    Protected m_strRootFolder As String
    Protected m_bGenerateTempates As Boolean = False

#End Region

#Region "Properties"

#End Region

#Region "Methods"

    <TestMethod()>
    Public Sub CodedUITestMethod1()

        Me.UIMap.AssertNewProjectAlreadyExists()
        Me.UIMap.CloseNewProjectErrorWindow()

        Me.UIMap.AddRootPartToChart()

        Me.UIMap.AddLineChart()

        Me.UIMap.NewProjectDlg_EnterNameAndPath()
        Me.UIMap.AddRootPartType()
        Me.UIMap.AddChildPartTypeWithJoint()
    End Sub

#Region "Additional test attributes"

    'You can use the following additional attributes as you write your tests:

    ' Use TestInitialize to run code before running each test
    <TestInitialize()> Public Overridable Sub MyTestInitialize()
        m_strRootFolder = System.Configuration.ConfigurationManager.AppSettings("RootFolder")
        If m_strRootFolder Is Nothing OrElse m_strRootFolder.Trim.Length = 0 Then
            Throw New System.Exception("Root Folder path was not found in configuration file.")
        End If

    End Sub

    ' Use TestCleanup to run code after each test has run
    <TestCleanup()> Public Overridable Sub MyTestCleanup()
        Try
            'Close the project
            'ExecuteMethod("Close", Nothing)
        Catch ex As Exception
        Finally
            Try
                Threading.Thread.Sleep(1000)

                'Now check to see if the process is still running. If it is then we need to kill it.
                Dim aryProcesses() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcessesByName("AnimatLab")
                If aryProcesses.Length > 0 Then
                    For Each oProc As System.Diagnostics.Process In aryProcesses
                        oProc.Kill()
                    Next
                End If
            Catch ex As Exception

            End Try
        End Try
    End Sub

#End Region

    Protected Overridable Sub StartApplication(ByVal strProject As String, ByVal iPort As Integer, Optional ByVal bAttachOnly As Boolean = False)
        If Not bAttachOnly Then
            Dim strArgs = ""
            If strProject.Trim.Length > 0 Then
                strArgs = "-Project " & strProject
            End If
            strArgs = strArgs & "-Port " & iPort.ToString

            Process.Start(m_strRootFolder & "\bin\AnimatLab.exe", strArgs)

            Threading.Thread.Sleep(3000)
        End If

        AttachServer(iPort)
    End Sub

    Protected Overridable Sub AttachServer(ByVal iPort As Integer)

        Dim tcpChannel As New TcpChannel
        System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(tcpChannel, True)

        m_oServer = DirectCast(Activator.GetObject(GetType(AnimatServer.Server), "tcp://localhost:8080/AnimatLab"), AnimatServer.Server)

    End Sub

    Protected Overridable Function ExecuteMethod(ByVal strMethodName As String, ByVal aryParams() As Object, Optional ByVal iWaitMilliseconds As Integer = 200) As Object
        Dim oRet As Object = m_oServer.ExecuteMethod(strMethodName, aryParams)
        Threading.Thread.Sleep(iWaitMilliseconds)
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

    Protected Overridable Sub CompareSimulation(ByVal strTestDataPath As String, Optional ByVal strPrefix As String = "", Optional ByVal dblMaxError As Double = 0.005)

        ExecuteMethod("ExportDataCharts", New Object() {"", strPrefix})

        'If we are flagged as needing to generate the template files then lets do that. Otherwise, lets compare the charts to the templates.
        If m_bGenerateTempates Then
            ExecuteMethod("CopyChartData", New Object() {strTestDataPath, strPrefix})
        Else
            ExecuteMethod("CompareExportedDataCharts", New Object() {strPrefix, strTestDataPath, dblMaxError})
        End If

    End Sub

#Region "GenerateCode"
    '''<summary>
    '''NewProjectDlg_EnterNameAndPath - Use 'NewProjectDlg_EnterNameAndPathParams' to pass parameters into this method.
    '''</summary>
    Protected Overridable Sub NewProjectDlg_EnterNameAndPath(ByVal strProjectName As String, ByVal strPath As String)
        Dim uITxtProjectNameEdit As WinEdit = Me.UIMap.UINewProjectWindow.UINewProjectWindow1.UITxtProjectNameEdit
        Dim uITxtLocationEdit As WinEdit = Me.UIMap.UINewProjectWindow.UITxtLocationWindow.UITxtLocationEdit
        Dim uIOKButton As WinButton = Me.UIMap.UINewProjectWindow.UIOKWindow.UIOKButton

        'Type 'TestProject' in 'txtProjectName' text box
        uITxtProjectNameEdit.Text = strProjectName

        'Type '{Tab}' in 'txtProjectName' text box
        Keyboard.SendKeys(uITxtProjectNameEdit, Me.UIMap.NewProjectDlg_EnterNameAndPathParams.UITxtProjectNameEditSendKeys, ModifierKeys.None)

        'Type 'C:\Projects\AnimatLabSDK\Experiments' in 'txtLocation' text box
        uITxtLocationEdit.Text = strPath

        'Click 'Ok' button
        Mouse.Click(uIOKButton, New Point(34, 15))

        Threading.Thread.Sleep(1000)
    End Sub

    '''<summary>
    '''NewProjectDlg_EnterNameAndPath - Use 'NewProjectDlg_EnterNameAndPathParams' to pass parameters into this method.
    '''</summary>
    Protected Overridable Sub NewProjectDlg_EnterNameAndPath_Error(ByVal strProjectName As String, ByVal strPath As String)
        Dim uITxtProjectNameEdit As WinEdit = Me.UIMap.UINewProjectWindow.UINewProjectWindow1.UITxtProjectNameEdit
        Dim uITxtLocationEdit As WinEdit = Me.UIMap.UINewProjectWindow.UITxtLocationWindow.UITxtLocationEdit
        Dim uIOKButton As WinButton = Me.UIMap.UINewProjectWindow.UIOKWindow.UIOKButton

        'Type 'TestProject' in 'txtProjectName' text box
        uITxtProjectNameEdit.Text = strProjectName

        'Type '{Tab}' in 'txtProjectName' text box
        Keyboard.SendKeys(uITxtProjectNameEdit, Me.UIMap.NewProjectDlg_EnterNameAndPathParams.UITxtProjectNameEditSendKeys, ModifierKeys.None)

        'Type 'C:\Projects\AnimatLabSDK\Experiments' in 'txtLocation' text box
        uITxtLocationEdit.Text = strPath

        'Click 'Ok' button
        Mouse.Click(uIOKButton, New Point(34, 15))

        Threading.Thread.Sleep(100)

        'Assert that the error box showed up with the correct ending text.
        Me.UIMap.AssertNewProjectAlreadyExists()

        'Close the error box and new project window.
        Me.UIMap.CloseNewProjectErrorWindow()

        Threading.Thread.Sleep(1000)
    End Sub

    '''<summary>
    '''AddRootPartType - Use 'AddRootPartTypeParams' to pass parameters into this method.
    '''</summary>
    Protected Overridable Sub AddRootPartType(ByVal strPartType As String)
        Dim uICtrlPartTypesList As WinList = Me.UIMap.UISelectPartTypeWindow.UICtrlPartTypesWindow.UICtrlPartTypesList
        Dim uIOKButton As WinButton = Me.UIMap.UISelectPartTypeWindow.UIOKWindow.UIOKButton

        'Select 'Box' in 'ctrlPartTypes' list box
        uICtrlPartTypesList.SelectedItemsAsString = strPartType

        'Click 'Ok' button
        Mouse.Click(uIOKButton, New Point(22, 10))

        Threading.Thread.Sleep(1000)
    End Sub

    '''<summary>
    '''AddChildPartTypeWithJoint - Use 'AddChildPartTypeWithJointParams' to pass parameters into this method.
    '''</summary>
    Protected Overridable Sub AddChildPartTypeWithJoint(ByVal strPartType As String, ByVal strJointType As String)
        Dim uICtrlPartTypesList As WinList = Me.UIMap.UISelectPartTypeWindow.UICtrlPartTypesWindow.UICtrlPartTypesList
        Dim uIOKButton As WinButton = Me.UIMap.UISelectPartTypeWindow.UIOKWindow.UIOKButton

        'Select 'Box' in 'ctrlPartTypes' list box
        uICtrlPartTypesList.SelectedItemsAsString = strPartType

        'Click 'Ok' button
        Mouse.Click(uIOKButton, New Point(40, 9))

        Threading.Thread.Sleep(1000)

        'Select 'Hinge' in 'ctrlPartTypes' list box
        uICtrlPartTypesList.SelectedItemsAsString = strJointType

        'Click 'Ok' button
        Mouse.Click(uIOKButton, New Point(45, 12))

        Threading.Thread.Sleep(1000)
    End Sub

    '''<summary>
    '''AddLineChart - Use 'AddLineChartParams' to pass parameters into this method.
    '''</summary>
    Protected Overridable Sub AddChart(ByVal strChartType As String)
        Dim uICtrlToolTypesList As WinList = Me.UIMap.UISelectDataToolTypeWindow.UICtrlToolTypesWindow.UICtrlToolTypesList
        Dim uIOKButton As WinButton = Me.UIMap.UISelectDataToolTypeWindow.UIOKWindow.UIOKButton

        'Select 'Line Chart' in 'ctrlToolTypes' list box
        uICtrlToolTypesList.SelectedItemsAsString = strChartType

        'Click 'Ok' button
        Mouse.Click(uIOKButton, New Point(36, 14))

        Threading.Thread.Sleep(1000)
    End Sub

    '''<summary>
    '''AddRootPartToChart
    '''</summary>
    Protected Overridable Sub AddRootPartToChart()
        Dim uITvStructuresClient As WinClient = Me.UIMap.UISelectDataItemWindow.UITvStructuresWindow.UITvStructuresClient
        Dim uIOKButton As WinButton = Me.UIMap.UISelectDataItemWindow.UIOKWindow.UIOKButton

        'Click 'tvStructures' client
        Mouse.Click(uITvStructuresClient, New Point(56, 26))

        'Click 'Ok' button
        Mouse.Click(uIOKButton, New Point(22, 8))

        Threading.Thread.Sleep(1000)
    End Sub

#End Region

#End Region

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = value
        End Set
    End Property

    Protected testContextInstance As TestContext

    Public ReadOnly Property UIMap As AnimatTesting.UIMap
        Get
            If (Me.map Is Nothing) Then
                Me.map = New AnimatTesting.UIMap()
            End If

            Return Me.map
        End Get
    End Property

    Protected map As AnimatTesting.UIMap
End Class
