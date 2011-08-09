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
Imports System.Reflection

<CodedUITest()>
Public MustInherit Class AnimatUITest

#Region " Enums "

    Public Enum enumErrorTextType
        BeginsWith
        Contains
        EndsWith
    End Enum

#End Region

#Region "Attributes"

    Protected m_iPort As Integer = -1
    Protected m_oServer As AnimatServer.Server
    Protected m_strRootFolder As String
    Protected m_bGenerateTempates As Boolean = False
    Protected m_strProjectName As String = ""
    Protected m_strProjectPath As String = ""
    Protected m_strTestDataPath As String = ""

    Protected m_UIBoxTestProjectWindow As UIProjectWindow
    Protected m_szOriginalResoution As New Size(1424, 791)
    Protected m_szCurrrentResoution As New Size(0, 0)
    Protected m_dblResScaleWidth As Double = 1
    Protected m_dblResScaleHeight As Double = 1

#End Region

#Region "Properties"

    Public ReadOnly Property UIProjectWindow(ByVal strProjectName As String) As UIProjectWindow
        Get
            If (Me.m_UIBoxTestProjectWindow Is Nothing) Then
                Me.m_UIBoxTestProjectWindow = New UIProjectWindow(strProjectName)
            End If
            Return Me.m_UIBoxTestProjectWindow
        End Get
    End Property

#End Region

#Region "Methods"

    '<TestMethod()>
    'Public Sub CodedUITestMethod1()

    '    Me.UIMap.MoveChartWindowToSideBySideView()

    '    Me.UIMap.AssertNewProjectAlreadyExists()
    '    Me.UIMap.CloseNewProjectErrorWindow()

    '    Me.UIMap.AddRootPartToChart()

    '    Me.UIMap.AddLineChart()

    '    Me.UIMap.NewProjectDlg_EnterNameAndPath()
    '    Me.UIMap.AddRootPartType()
    '    Me.UIMap.AddChildPartTypeWithJoint()
    'End Sub

#Region "Additional test attributes"

    'You can use the following additional attributes as you write your tests:

    ' Use TestInitialize to run code before running each test
    <TestInitialize()> Public Overridable Sub MyTestInitialize()
        m_strRootFolder = System.Configuration.ConfigurationManager.AppSettings("RootFolder")
        If m_strRootFolder Is Nothing OrElse m_strRootFolder.Trim.Length = 0 Then
            Throw New System.Exception("Root Folder path was not found in configuration file.")
        End If

        m_bGenerateTempates = CType(System.Configuration.ConfigurationManager.AppSettings("GenerateTemplates"), Boolean)
    End Sub

    ' Use TestCleanup to run code after each test has run
    <TestCleanup()> Public Overridable Sub MyTestCleanup()
        Try
            'Close any active dialog boxes before closing app.
            ExecuteDirectMethod("CloseActiveDialogs", Nothing)

            'Save the project
            ExecuteMethod("ClickToolbarItem", New Object() {"SaveToolStripButton"})

            'Close the project
            ExecuteMethod("Close", Nothing)

            'Delete the project directory
            CleanupProjectDirectory()
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

        For iTry As Integer = 0 To 3
            Try
                m_oServer = DirectCast(Activator.GetObject(GetType(AnimatServer.Server), "tcp://localhost:8080/AnimatLab"), AnimatServer.Server)
            Catch ex As Exception
                Threading.Thread.Sleep(3000)
            End Try

            If Not m_oServer Is Nothing Then
                Return
            End If
        Next

        'Try one last time.
        If m_oServer Is Nothing Then
            m_oServer = DirectCast(Activator.GetObject(GetType(AnimatServer.Server), "tcp://localhost:8080/AnimatLab"), AnimatServer.Server)
        End If

    End Sub

    Protected Sub CleanupProjectDirectory()
        'Make sure any left over project directory is cleaned up before starting the test.
        DeleteDirectory(m_strRootFolder & m_strProjectPath & "\" & m_strProjectName)
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
            Threading.Thread.Sleep(iWaitMilliseconds)
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

    Protected Overridable Sub CompareSimulation(ByVal strTestDataPath As String, Optional ByVal strPrefix As String = "", Optional ByVal dblMaxError As Double = 0.1)

        'No prefix on the exported chart.
        ExecuteMethod("ExportDataCharts", New Object() {"", ""})

        'If we are flagged as needing to generate the template files then lets do that. Otherwise, lets compare the charts to the templates.
        If m_bGenerateTempates Then
            ExecuteMethod("CopyChartData", New Object() {strTestDataPath, strPrefix})
        Else
            ExecuteMethod("CompareExportedDataCharts", New Object() {strPrefix, strTestDataPath, dblMaxError})
        End If

    End Sub

    Protected Overridable Sub CreateNewProject(ByVal strProjectName As String, ByVal strProjectPath As String, ByVal dblSimEnd As Double)

         OpenDialogAndWait("NewProject", Me.GetType.GetMethod("ClickToolbarItem"), New Object() {"NewToolStripButton"})

        'Set params and hit ok button
        ExecuteActiveDialogMethod("SetProjectParams", New Object() {strProjectName, m_strRootFolder & strProjectPath})
        ExecuteActiveDialogMethod("ClickOkButton", Nothing)

        'Set simulation to automatically end at a specified time.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation", "SetSimulationEnd", "True"})

        'Set simulation end time.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation", "SimulationEndTime", dblSimEnd.ToString})

        CreateStructure("Structure_1", "Structure_1")

        Threading.Thread.Sleep(2000)

    End Sub

    Protected Overridable Sub CreateStructure(ByVal strOrigStructureName As String, ByVal strNewStructureName As String)

        'Click the add structure button.
        ExecuteMethod("ClickToolbarItem", New Object() {"AddStructureToolStripButton"})

        'Set the name of the structure
        If strOrigStructureName <> strNewStructureName Then
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strOrigStructureName, "Name", strNewStructureName})
        End If

        'Open the Structure_1 body plan editor window
        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Structures\" & strNewStructureName & "\Body Plan"}, 2000)

    End Sub

    Protected Overridable Sub CreateChartAndAddBodies(ByVal dblChartEndTime As Double)

        'Select the LineChart to add.
        AddChart("Line Chart")

        'Select the Chart axis
        ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1"})

        'Change the end time of the data chart to 45 seconds.
        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart", "CollectEndTime", dblChartEndTime.ToString})

        'Now add items to the chart to plot the y position of the root, child part, and joint.
        'Add root part.
        AddItemToChart("Structure_1\Root")

        'Set the name of the data chart item to root_y.
        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Root", "Name", "Root_Y"})

        'Change the data type to track the world Y position.
        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Root_Y", "DataTypeID", "WorldPositionY"})

        'Add child body part
        AddItemToChart("Structure_1\Root\Joint_1\Body_1")

        'Set the name of the data chart item to root_y.
        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Body_1", "Name", "Child_Y"})

        'Change the data type to track the world Y position.
        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Child_Y", "DataTypeID", "WorldPositionY"})

        'Add joint body part
        AddItemToChart("Structure_1\Root\Joint_1")

        'Set the name of the data chart item to root_y.
        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Joint_1", "Name", "Joint_Y"})

        'Change the data type to track the world Y position.
        ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Joint_Y", "DataTypeID", "WorldPositionY"})

        'Select the simulation window tab so it is visible now.
        ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Simulation\Environment\Structures\Structure_1"}, 1000)
    End Sub


    Protected Overridable Sub AssertInRange(ByVal strPartType As String, ByVal strAxis As String, ByVal strOperation As String, _
                                                 ByVal dblDiff As Double, ByVal dblMinRange As Double, ByVal dblMaxRange As Double)

        If dblDiff < dblMinRange OrElse dblDiff > dblMaxRange Then
            Throw New System.Exception(strPartType & " " & strAxis & " " & strOperation & " is not within the specified range. Value: " & dblDiff & ", Range: (" & dblMinRange & " < " & dblMaxRange & ")")
        End If
    End Sub


    Protected Overridable Sub MovePartAxis(ByVal strStructure As String, ByVal strPart As String, ByVal strAxis As String, ByVal ptAxisStart As Point, ByVal ptAxisEnd As Point, _
                                           ByVal dblMinPartRange As Double, ByVal dblMaxPartRange As Double, _
                                           ByVal dblMinStructRange As Double, ByVal dblMaxStructRange As Double, _
                                           ByVal dblMinLocalRange As Double, ByVal dblMaxLocalRange As Double)

        'Get the part and structure x position before movement
        Dim dblBeforePartPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WorldPosition." & strAxis & ".ActualValue"), Double)
        Dim dblBeforeStructPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure, "LocalPosition." & strAxis & ".ActualValue"), Double)
        Dim dblBeforeLocalPartPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LocalPosition." & strAxis & ".ActualValue"), Double)

        'Move axis
        DragMouse(ptAxisStart, ptAxisEnd, MouseButtons.Left)

        'Get the part and structure x position after movement
        Dim dblAfterPartPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WorldPosition." & strAxis & ".ActualValue"), Double)
        Dim dblAfterStructPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure, "LocalPosition." & strAxis & ".ActualValue"), Double)
        Dim dblAfterLocalPartPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LocalPosition." & strAxis & ".ActualValue"), Double)

        AssertInRange("Part", strAxis, "position", (dblAfterPartPos - dblBeforePartPos), dblMinPartRange, dblMaxPartRange)
        AssertInRange("Structure", strAxis, "position", (dblAfterStructPos - dblBeforeStructPos), dblMinStructRange, dblMaxStructRange)
        AssertInRange("Part Local", strAxis, "position", (dblAfterLocalPartPos - dblBeforeLocalPartPos), dblMinLocalRange, dblMaxLocalRange)

    End Sub

    Protected Overridable Sub RotatePartAxis(ByVal strStructure As String, ByVal strPart As String, _
                                             ByVal strAxis As String, ByVal ptAxisStart As Point, ByVal ptAxisEnd As Point, _
                                            ByVal dblMinRange As Double, ByVal dblMaxRange As Double, Optional ByVal bResetPos As Boolean = True)

        'Get the part rotation before movement
        Dim dblBeforePartPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis & ".ActualValue"), Double)

        'Move axis
        DragMouse(ptAxisStart, ptAxisEnd, MouseButtons.Left)

        'Get the part and structure x position before movement
        Dim dblAfterPartPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis & ".ActualValue"), Double)

        AssertInRange("Part", strAxis, "rotation", (dblAfterPartPos - dblBeforePartPos), dblMinRange, dblMaxRange)

        'Reset the rotation to 0.
        If bResetPos Then
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis, dblBeforePartPos.ToString})
        End If

    End Sub

    Protected Overridable Sub ResetStructurePosition(ByVal strStructure As String, ByVal strPart As String, _
                                                     ByVal dblPosX As Double, ByVal dblPosY As Double, ByVal dblPosZ As Double, _
                                                     Optional ByVal bVerifyRoot As Boolean = True, Optional ByVal dblMaxError As Double = 0.001)

        'Reset the position of the Structure.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure, "LocalPosition.X", dblPosX.ToString})
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure, "LocalPosition.Y", dblPosY.ToString})
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure, "LocalPosition.Z", dblPosZ.ToString})

        'Verify that the structure position is correct now.
        Dim dblStructPosX As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure, "LocalPosition.X.ActualValue"), Double)
        Dim dblStructPosY As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure, "LocalPosition.Y.ActualValue"), Double)
        Dim dblStructPosZ As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure, "LocalPosition.Z.ActualValue"), Double)

        If Math.Abs(dblPosX - dblStructPosX) > dblMaxError Then
            Throw New System.Exception("Structure position does not match the target value: " & dblPosX & ", recorded value: " & dblStructPosX)
        End If

        If Math.Abs(dblPosY - dblStructPosY) > dblMaxError Then
            Throw New System.Exception("Structure position does not match the target value: " & dblPosY & ", recorded value: " & dblStructPosY)
        End If

        If Math.Abs(dblPosZ - dblStructPosZ) > dblMaxError Then
            Throw New System.Exception("Structure position does not match the target value: " & dblPosZ & ", recorded value: " & dblStructPosZ)
        End If

        'Verify that the root position is correct now.
        If bVerifyRoot Then
            Dim dblRootPosX As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WorldPosition.X.ActualValue"), Double)
            Dim dblRootPosY As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WorldPosition.Y.ActualValue"), Double)
            Dim dblRootPosZ As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WorldPosition.Z.ActualValue"), Double)

            If Math.Abs(dblPosX - dblRootPosX) > dblMaxError Then
                Throw New System.Exception("Structure position does not match the target value: " & dblPosX & ", recorded value: " & dblRootPosX)
            End If

            If Math.Abs(dblPosY - dblRootPosY) > dblMaxError Then
                Throw New System.Exception("Structure position does not match the target value: " & dblPosY & ", recorded value: " & dblRootPosY)
            End If

            If Math.Abs(dblPosZ - dblRootPosZ) > dblMaxError Then
                Throw New System.Exception("Structure position does not match the target value: " & dblPosZ & ", recorded value: " & dblRootPosZ)
            End If
        End If

    End Sub

    Protected Overridable Sub ManualMovePartAxis(ByVal strStructure As String, ByVal strPart As String, ByVal strAxis As String, _
                                                 ByVal dblWorldPos As Double, ByVal dblWorldTest As Double, _
                                                 ByVal dblWorldLocalTest As Double, ByVal bTestLocal As Boolean, ByVal dblLocalPos As Double, _
                                                 ByVal dblLocalTest As Double, ByVal dblLocalWorldTest As Double, Optional ByVal dblMaxError As Double = 0.001)

        'Move the root part along the axis using world coordinates.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WorldPosition." & strAxis, dblWorldPos.ToString})

        'Now get the world position and verify it.
        Dim dblPosWorld As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WorldPosition." & strAxis & ".ActualValue"), Double)
        Dim dblPosLocal As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LocalPosition." & strAxis & ".ActualValue"), Double)

        If Math.Abs(dblPosWorld - dblWorldTest) > dblMaxError Then
            Throw New System.Exception("Body part position does not match the world target value: " & dblPosWorld & ", recorded value: " & dblWorldTest)
        End If

        If Math.Abs(dblPosLocal - dblWorldLocalTest) > dblMaxError Then
            Throw New System.Exception("Body part position does not match the local target value: " & dblPosLocal & ", recorded value: " & dblWorldLocalTest)
        End If

        'Move the root part along the axis using local coordinates.
        If bTestLocal Then
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LocalPosition." & strAxis, dblLocalPos.ToString})

            'Now get the world position and verify it.
            dblPosWorld = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WorldPosition." & strAxis & ".ActualValue"), Double)
            dblPosLocal = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LocalPosition." & strAxis & ".ActualValue"), Double)

            If Math.Abs(dblPosLocal - dblLocalTest) > dblMaxError Then
                Throw New System.Exception("Body part position does not match the local target value: " & dblPosLocal & ", recorded value: " & dblLocalTest)
            End If

            If Math.Abs(dblPosWorld - dblLocalWorldTest) > dblMaxError Then
                Throw New System.Exception("Body part position does not match the world target value: " & dblPosWorld & ", recorded value: " & dblLocalWorldTest)
            End If
        End If

    End Sub

    Protected Overridable Sub ManualRotatePartAxis(ByVal strStructure As String, ByVal strPart As String, ByVal strAxis As String, _
                                                   ByVal dblRotation As Double, Optional ByVal bReset As Boolean = True, Optional ByVal dblMaxError As Double = 0.001)

        'Now beginning rotation
        Dim dblOrigRot As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis & ".ActualValue"), Double)

        'Move the root part along the axis using world coordinates.
        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis, dblRotation.ToString})

        'Now get the world position and verify it.
        Dim dblRealRot As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis & ".ActualValue"), Double)

        If Math.Abs(dblRealRot - dblRotation) > dblMaxError Then
            Throw New System.Exception("Body part rotation does not match the target value: " & dblRealRot & ", recorded value: " & dblRotation)
        End If

        If bReset Then
            'Reset the rotation
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis, dblOrigRot.ToString})
        End If

    End Sub

    Protected Overridable Sub RecalculatePositionsUsingResolution()
        Dim uIStructure_1BodyClient As WinClient = Me.UIProjectWindow(m_strProjectName).UIStructure_1BodyWindow.UIStructure_1BodyClient

        m_szCurrrentResoution = New Size(uIStructure_1BodyClient.BoundingRectangle.Width, uIStructure_1BodyClient.BoundingRectangle.Height)

        m_dblResScaleWidth = m_szCurrrentResoution.Width / m_szOriginalResoution.Width
        m_dblResScaleHeight = m_szCurrrentResoution.Height / m_szOriginalResoution.Height

    End Sub

    Protected Overridable Sub VerifyChildPosAfterRotate(ByVal strStructure As String, ByVal strAxis As String, ByVal strPart As String, _
                                                        ByVal dblRotation As Double, ByVal dblWorldXTest As Double, _
                                                        ByVal dblWorldYTest As Double, ByVal dblWorldZTest As Double, _
                                                        Optional ByVal dblMaxError As Double = 0.001)
        'rotate the root and verify the child position.
        ManualRotatePartAxis(strStructure, "Root", strAxis, dblRotation, False)

        Dim dblWorldX As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WorldPosition.X.ActualValue"), Double)
        Dim dblWorldY As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WorldPosition.Y.ActualValue"), Double)
        Dim dblWorldZ As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WorldPosition.Z.ActualValue"), Double)

        If Math.Abs(dblWorldX - dblWorldXTest) > dblMaxError Then
            Throw New System.Exception("Body part rotation does not match the target value: " & dblWorldXTest & ", recorded value: " & dblWorldX)
        End If

        If Math.Abs(dblWorldY - dblWorldYTest) > dblMaxError Then
            Throw New System.Exception("Body part rotation does not match the target value: " & dblWorldYTest & ", recorded value: " & dblWorldY)
        End If

        If Math.Abs(dblWorldZ - dblWorldZTest) > dblMaxError Then
            Throw New System.Exception("Body part rotation does not match the target value: " & dblWorldZTest & ", recorded value: " & dblWorldZ)
        End If

        'Reset the rotation.
        ManualRotatePartAxis(strStructure, "Root", strAxis, 0, False)

    End Sub

#Region "GenerateCode"

    Protected Overridable Sub ProcessExtraAddRootMethods(ByVal strPartType As String)

    End Sub

    '''<summary>
    '''AddRootPartType - Use 'AddRootPartTypeParams' to pass parameters into this method.
    '''</summary>
    Protected Overridable Sub AddRootPartType(ByVal strPartType As String)

        OpenDialogAndWait("SelectPartType", Me.GetType.GetMethod("ClickToolbarItem"), New Object() {"AddPartToolStripButton"})

        ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strPartType})

        'Click 'Ok' button
        ExecuteActiveDialogMethod("ClickOkButton", Nothing)

        ProcessExtraAddRootMethods(strPartType)

        Threading.Thread.Sleep(2000)
    End Sub

    Public Overridable Sub ClickToAddBody(ByVal ptClick As Point)
        Dim uIStructure_1BodyClient As WinClient = Me.UIProjectWindow(m_strProjectName).UIStructure_1BodyWindow.UIStructure_1BodyClient
        Mouse.Click(uIStructure_1BodyClient, ptClick)
    End Sub

    Protected Overridable Sub OpenDialogAndWait(ByVal strDlgName As String, ByVal oActionMethod As MethodInfo, ByVal aryParams() As Object)

        Dim bDlgUp As Boolean = False
        Dim iCount As Integer = 0
        While Not bDlgUp
            If Not oActionMethod Is Nothing Then
                'Perform the action method
                oActionMethod.Invoke(Me, aryParams)
            End If

            Threading.Thread.Sleep(1000)

            Dim strFormName As String = DirectCast(ExecuteDirectMethod("ActiveDialogName", Nothing), String)
            If strFormName = strDlgName Then
                bDlgUp = True
            End If
            iCount = iCount + 1
            If iCount > 5 Then
                Throw New System.Exception(strDlgName & " dialog would not open.")
            End If
        End While
    End Sub

    Protected Overridable Sub ProcessExtraChildMethods(ByVal strPartType As String, ByVal strJointType As String)

    End Sub

    Protected Overridable Sub ProcessExtraChildJointMethods(ByVal strPartType As String, ByVal strJointType As String)

    End Sub

    '''<summary>
    '''AddChildPartTypeWithJoint - Use 'AddChildPartTypeWithJointParams' to pass parameters into this method.
    '''</summary>
    Protected Overridable Sub AddChildPartTypeWithJoint(ByVal strPartType As String, ByVal strJointType As String, ByVal ptAddClick As Point)

        'Click 'Add Part' button
        ExecuteMethod("ClickToolbarItem", New Object() {"AddPartToolStripButton"}, 2000)

        OpenDialogAndWait("SelectPartType", Me.GetType.GetMethod("ClickToAddBody"), New Object() {ptAddClick})

        ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strPartType})

        'Click 'Ok' button
        ExecuteActiveDialogMethod("ClickOkButton", Nothing)

        ProcessExtraChildMethods(strPartType, strJointType)

        OpenDialogAndWait("SelectPartType", Nothing, Nothing)

        ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strJointType})

        'Click 'Ok' button
        ExecuteActiveDialogMethod("ClickOkButton", Nothing)

        ProcessExtraChildJointMethods(strPartType, strJointType)

        Threading.Thread.Sleep(1000)
    End Sub

    '''<summary>
    '''AddLineChart - Use 'AddLineChartParams' to pass parameters into this method.
    '''</summary>
    Protected Overridable Sub AddChart(ByVal strChartType As String)

        OpenDialogAndWait("SelectToolType", Me.GetType.GetMethod("ClickToolbarItem"), New Object() {"AddToolToolStripButton"})

        ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strChartType})

        'Dim uICtrlToolTypesList As WinList = Me.UIMap.UISelectDataToolTypeWindow.UICtrlToolTypesWindow.UICtrlToolTypesList
        'Dim uIOKButton As WinButton = Me.UIMap.UISelectDataToolTypeWindow.UIOKWindow.UIOKButton

        ''Select 'Line Chart' in 'ctrlToolTypes' list box
        'uICtrlToolTypesList.SelectedItemsAsString = strChartType

        'Click 'Ok' button
        ExecuteActiveDialogMethod("ClickOkButton", Nothing)

        Threading.Thread.Sleep(1000)
    End Sub

    Public Overridable Sub ClickToolbarItem(ByVal strButton As String)
        'Click 'Add DataTool' button
        ExecuteMethod("ClickToolbarItem", New Object() {strButton})
    End Sub

    '''<summary>
    '''AddRootPartToChart
    '''</summary>
    Protected Overridable Sub AddItemToChart(ByVal strPath As String)

        'Click 'Add Chart Item' button
        OpenDialogAndWait("SelectDataItem", Me.GetType.GetMethod("ClickToolbarItem"), New Object() {"AddDataItemToolStripButton"})

        ExecuteActiveDialogMethod("SelectItem", New Object() {strPath})
        ExecuteActiveDialogMethod("ClickOkButton", Nothing)

        Threading.Thread.Sleep(2000)
    End Sub

    Protected Overridable Sub DeletePart(ByVal strPath As String)
        ExecuteMethod("SelectWorkspaceItem", New Object() {strPath})
        ExecuteMethod("ClickToolbarItem", New Object() {"DeleteToolStripButton"})
        OpenDialogAndWait("AnimatMessageBox", Nothing, Nothing)
        ExecuteActiveDialogMethod("ClickOkButton", Nothing)
    End Sub

    '''<summary>
    '''ZoomInOnRootPart
    '''</summary>
    Public Sub ZoomInOnRootPart(ByVal ptStart As Point, ByVal iAmount1 As Integer, Optional ByVal iAmount2 As Integer = 0)
        Dim uIStructure_1BodyClient As WinClient = Me.UIProjectWindow(m_strProjectName).UIStructure_1BodyWindow.UIStructure_1BodyClient

        'Move using Right button 'Structure_1 Body' client
        Mouse.StartDragging(uIStructure_1BodyClient, ptStart, MouseButtons.Right, ModifierKeys.None)
        Mouse.StopDragging(uIStructure_1BodyClient, 0, iAmount1)
        'Mouse.StopDragging(uISimulationControllerClient, New Point(873, 12))

        'Move using Right button 'Structure_1 Body' client 
        If iAmount2 > 0 Then
            Mouse.StartDragging(uIStructure_1BodyClient, ptStart, MouseButtons.Right, ModifierKeys.None)
            Mouse.StopDragging(uIStructure_1BodyClient, 0, iAmount2)
        End If

        Mouse.Click(uIStructure_1BodyClient, MouseButtons.Right, ModifierKeys.None, ptStart)

    End Sub

    Public Sub DragMouse(ByVal ptStart As Point, ByVal ptEnd As Point, ByVal mButton As MouseButtons, _
                         Optional ByVal mModifiers As ModifierKeys = ModifierKeys.None, _
                         Optional ByVal bEndClick As Boolean = False)
        Dim uIStructure_1BodyClient As WinClient = Me.UIProjectWindow(m_strProjectName).UIStructure_1BodyWindow.UIStructure_1BodyClient

        Mouse.StartDragging(uIStructure_1BodyClient, ptStart, mButton, mModifiers)
        Mouse.StopDragging(uIStructure_1BodyClient, ptEnd)

        If bEndClick Then
            Mouse.Click(uIStructure_1BodyClient, MouseButtons.Right, ModifierKeys.None, ptEnd)
        End If

    End Sub

    '''<summary>
    '''NewProjectDlg_EnterNameAndPath - Use 'NewProjectDlg_EnterNameAndPathParams' to pass parameters into this method.
    '''</summary>
    Protected Overridable Sub CheckForErrorDialog(ByVal strEndError As String)
        Dim uITxtProjectNameEdit As WinEdit = Me.UIMap.UINewProjectWindow.UINewProjectWindow1.UITxtProjectNameEdit
        Dim uITxtLocationEdit As WinEdit = Me.UIMap.UINewProjectWindow.UITxtLocationWindow.UITxtLocationEdit
        Dim uIOKButton As WinButton = Me.UIMap.UINewProjectWindow.UIOKWindow.UIOKButton

        Threading.Thread.Sleep(500)

        'Assert that the error box showed up with the correct ending text.
        AssertErrorDiolog(strEndError)

        'Close the error box and new project window.
        Me.UIMap.CloseNewProjectErrorWindow()

        Threading.Thread.Sleep(1000)
    End Sub

    '''<summary>
    '''AssertNewProjectAlreadyExists - Use 'AssertNewProjectAlreadyExistsExpectedValues' to pass parameters into this method.
    '''</summary>
    Public Sub AssertErrorDiolog(ByVal strEndError As String)
        Dim uITxtErrorMsgEdit As WinEdit = Me.UIMap.UIErrorWindow.UIThedirectoryCProjectWindow.UITxtErrorMsgEdit

        'Verify that 'txtErrorMsg' text box's property 'Text' ends with '' already exists. Please choose a different name or location for the project.'
        StringAssert.EndsWith(uITxtErrorMsgEdit.Text, strEndError)
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


Public Class UIProjectWindow
    Inherits WinWindow

    Public m_strProjectName As String

    Public Sub New(ByVal strProjectName As String)
        MyBase.New()
        Me.SearchProperties(WinWindow.PropertyNames.Name) = strProjectName & " Project *"
        Me.SearchProperties.Add(New PropertyExpression(WinWindow.PropertyNames.ClassName, "WindowsForms10.Window", PropertyExpressionOperator.Contains))
        Me.WindowTitles.Add(strProjectName & " Project *")
        m_strProjectName = strProjectName
    End Sub

#Region "Properties"
    Public ReadOnly Property UIStructure_1BodyWindow() As UIStructure_1BodyWindow
        Get
            If (Me.mUIStructure_1BodyWindow Is Nothing) Then
                Me.mUIStructure_1BodyWindow = New UIStructure_1BodyWindow(Me, m_strProjectName)
            End If
            Return Me.mUIStructure_1BodyWindow
        End Get
    End Property

    Public ReadOnly Property UIItemWindow() As UIItemWindow
        Get
            If (Me.mUIItemWindow Is Nothing) Then
                Me.mUIItemWindow = New UIItemWindow(Me, m_strProjectName)
            End If
            Return Me.mUIItemWindow
        End Get
    End Property
#End Region

#Region "Fields"
    Private mUIStructure_1BodyWindow As UIStructure_1BodyWindow

    Private mUIItemWindow As UIItemWindow
#End Region
End Class

Public Class UIStructure_1BodyWindow
    Inherits WinWindow

    Public m_strProjectName As String

    Public Sub New(ByVal searchLimitContainer As UITestControl, ByVal strProjectName As String)
        MyBase.New(searchLimitContainer)
        Me.SearchProperties(WinWindow.PropertyNames.ControlName) = "SimulationWindow_Toolstrips"
        Me.WindowTitles.Add(strProjectName & " Project *")
        m_strProjectName = strProjectName
    End Sub

#Region "Properties"
    Public ReadOnly Property UIStructure_1BodyClient() As WinClient
        Get
            If (Me.mUIStructure_1BodyClient Is Nothing) Then
                Me.mUIStructure_1BodyClient = New WinClient(Me)
                Me.mUIStructure_1BodyClient.SearchProperties(WinControl.PropertyNames.Name) = "Structure_1 Body"
                Me.mUIStructure_1BodyClient.WindowTitles.Add(m_strProjectName & " Project *")
            End If
            Return Me.mUIStructure_1BodyClient
        End Get
    End Property
#End Region

#Region "Fields"
    Private mUIStructure_1BodyClient As WinClient
#End Region
End Class

Public Class UIItemWindow
    Inherits WinWindow

    Public m_strProjectName As String

    Public Sub New(ByVal searchLimitContainer As UITestControl, ByVal strProjectName As String)
        MyBase.New(searchLimitContainer)
        Me.SearchProperties.Add(New PropertyExpression(WinWindow.PropertyNames.ClassName, "WindowsForms10.Window", PropertyExpressionOperator.Contains))
        Me.SearchProperties(WinWindow.PropertyNames.Instance) = "10"
        Me.WindowTitles.Add(strProjectName & " Project *")
        m_strProjectName = strProjectName
    End Sub

#Region "Properties"
    Public ReadOnly Property UIToolStripContainer1Client() As UIToolStripContainer1Client
        Get
            If (Me.mUIToolStripContainer1Client Is Nothing) Then
                Me.mUIToolStripContainer1Client = New UIToolStripContainer1Client(Me, m_strProjectName)
            End If
            Return Me.mUIToolStripContainer1Client
        End Get
    End Property
#End Region

#Region "Fields"
    Private mUIToolStripContainer1Client As UIToolStripContainer1Client
#End Region
End Class

<GeneratedCode("Coded UITest Builder", "10.0.30319.1")> _
Public Class UIToolStripContainer1Client
    Inherits WinClient

    Public m_strProjectName As String

    Public Sub New(ByVal searchLimitContainer As UITestControl, ByVal strProjectName As String)
        MyBase.New(searchLimitContainer)
        Me.WindowTitles.Add(strProjectName & " Project *")
        m_strProjectName = strProjectName
    End Sub

#Region "Properties"
    Public ReadOnly Property UISimulationControllerClient() As WinClient
        Get
            If (Me.mUISimulationControllerClient Is Nothing) Then
                Me.mUISimulationControllerClient = New WinClient(Me)
                Me.mUISimulationControllerClient.SearchProperties(WinControl.PropertyNames.Name) = "Simulation Controller"
                Me.mUISimulationControllerClient.WindowTitles.Add(m_strProjectName & "Project *")
            End If
            Return Me.mUISimulationControllerClient
        End Get
    End Property
#End Region

#Region "Fields"
    Private mUISimulationControllerClient As WinClient
#End Region
End Class