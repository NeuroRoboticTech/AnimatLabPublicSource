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

Namespace Framework

    <CodedUITest()>
    Public MustInherit Class AnimatUITest
        Inherits AnimatTest

#Region "enums"

        Public Enum enumMatchType
            Equals
            Contains
            BeginsWith
            EndsWith
        End Enum

#End Region

#Region "Attributes"

        Protected m_UIBoxTestProjectWindow As UIProjectWindow
        Protected m_szOriginalResoution As New Size(1424, 791)
        Protected m_szCurrrentResoution As New Size(0, 0)
        Protected m_dblResScaleWidth As Double = 1
        Protected m_dblResScaleHeight As Double = 1
        Protected m_bCreateStructure As Boolean = True
        Protected m_strStructureGroup As String = "Structures"
        Protected m_strStruct1Name As String = "Structure_1"
        Protected m_strRootNeuralSystem As String = "Neural Subsystem"

        Protected m_strAmbient As String = "#1E1E1E"
        Protected m_strDiffuse As String = "#FFFFFF"
        Protected m_strSpecular As String = "#1E1E1E"
        Protected m_iShininess As Integer = 70

        Protected m_bTestTexture As Boolean = True

        Protected m_strTextureFile As String = "Bricks.bmp"
        Protected m_strMeshFile As String = "TestMesh.osg"

        Dim m_aryChartColumns() As String
        Dim m_aryChartData(,) As Double

        Protected m_strJointChartMovementName As String = "Rotation"
        Protected m_strJointChartMovementType As String = "JointRotationDeg"

        Protected m_strJointChartVelocityName As String = "JointVelocity"
        Protected m_strJointChartVelocityType As String = "JointActualVelocity"

        Protected m_iStimCounter As Integer = 1

#End Region

#Region "Properties"

        Public Overridable ReadOnly Property UIProjectWindow(ByVal strProjectName As String) As UIProjectWindow
            Get
                If (Me.m_UIBoxTestProjectWindow Is Nothing) Then
                    Me.m_UIBoxTestProjectWindow = New UIProjectWindow(strProjectName)
                End If
                Return Me.m_UIBoxTestProjectWindow
            End Get
        End Property

        Public Overridable ReadOnly Property HasChildPart() As Boolean
            Get
                Return True
            End Get
        End Property

        Protected Overridable ReadOnly Property HasRootGraphic() As Boolean
            Get
                Return True
            End Get
        End Property

        Protected Overridable ReadOnly Property AllowRootRotations() As Boolean
            Get
                Return True
            End Get
        End Property

        Protected Overridable ReadOnly Property AllowChildRotations() As Boolean
            Get
                Return True
            End Get
        End Property

#End Region

#Region "Methods"

        '<TestMethod()>
        'Public Sub CodedUITestMethod1()

        '    Me.UIMap.OpenUIEditor()

        '    Me.UIMap.OpenProject()

        '    'Me.UIMap.MoveChartWindowToSideBySideView()

        '    'Me.UIMap.AssertNewProjectAlreadyExists()
        '    'Me.UIMap.CloseNewProjectErrorWindow()

        '    'Me.UIMap.AddRootPartToChart()

        '    'Me.UIMap.AddLineChart()

        '    'Me.UIMap.NewProjectDlg_EnterNameAndPath()
        '    'Me.UIMap.AddRootPartType()
        '    'Me.UIMap.AddChildPartTypeWithJoint()
        'End Sub

#Region "Additional test attributes"

        'You can use the following additional attributes as you write your tests:

        ' Use TestInitialize to run code before running each test
        <TestInitialize()> Public Overridable Sub MyTestInitialize()
            InitializeConfiguration()
        End Sub

        Protected Overridable Sub StartNewProject()
            'Delete the project directory
            CleanupProjectDirectory()

            'Start the application.
            StartApplication("", m_bAttachServerOnly)

            CreateNewProject(m_strProjectName, m_strProjectPath, m_dblSimEndTime, m_bCreateStructure)
        End Sub

        Protected Overridable Sub StartExistingProject()
            'Start the application.
            StartApplication("", m_bAttachServerOnly)

            ExecuteIndirectMethod("LoadProject", New Object() {m_strRootFolder & m_strProjectPath & "\" & m_strProjectName & "\" & m_strProjectName & ".aproj"})

         End Sub

        ' Use TestCleanup to run code after each test has run
        <TestCleanup()> Public Overridable Sub MyTestCleanup()
            Try
                Debug.WriteLine("Starting test cleanup.")

                System.Threading.Thread.Sleep(1000)

                'Close any active dialog boxes before closing app.
                Try
                    ExecuteDirectMethod("CloseActiveDialogs", Nothing)
                Catch ex As Exception
                    Debug.WriteLine("Error: " & ex.Message)
                    If Not ex.InnerException Is Nothing Then
                        Debug.WriteLine("Inner error: " & ex.InnerException.Message)
                    End If
                End Try

                'Save the project
                ExecuteMethod("ClickToolbarItem", New Object() {"SaveToolStripButton"})

                'Close the project
                ExecuteMethod("Close", Nothing)

                'Detach from the server.
                DetachServer()

                'Delete the project directory
                CleanupProjectDirectory()
            Catch ex As Exception
            Finally
                Try
                    Debug.WriteLine("Caught exception while doing cleanup.")
                    Threading.Thread.Sleep(1000)

                    'Now check to see if the process is still running. If it is then we need to kill it.
                    Dim aryProcesses() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcessesByName("AnimatLab2")
                    Debug.WriteLine("Found " & aryProcesses.Length & " animatlab 2 processes to kill.")

                    If aryProcesses.Length > 0 Then
                        For Each oProc As System.Diagnostics.Process In aryProcesses
                            Debug.WriteLine("Directly killing process AnimatLab2.")
                            oProc.Kill()
                        Next
                    End If

                    Debug.WriteLine("Exiting test cleanup exeption handling.")
                Catch ex As Exception
                    Debug.WriteLine("Caught exception within exception handling for cleanup code. Eating it and going on.")
                End Try
            End Try
        End Sub

#End Region


        Protected Overridable Sub CompareSimulation(ByVal strTestDataPath As String, Optional ByVal strPrefix As String = "", _
                                                    Optional ByVal dblMaxError As Double = 0.1, Optional ByVal iMaxRows As Integer = -1)
            Debug.WriteLine("Comparing simulation output. Test Data Path: '" & strTestDataPath & "', Prefix: '" & strPrefix & "', MaxError: " & dblMaxError & ", MaxRows: " & iMaxRows)

            'No prefix on the exported chart.
            ExecuteMethod("ExportDataCharts", New Object() {"", ""})

            'Prep the hashchart
            Dim aryMaxErrors As New Hashtable
            aryMaxErrors.Add("default", dblMaxError)

            'If we are flagged as needing to generate the template files then lets do that. Otherwise, lets compare the charts to the templates.
            If m_bGenerateTempates Then
                ExecuteMethod("CopyChartData", New Object() {strTestDataPath, strPrefix})
            Else
                ExecuteMethod("CompareExportedDataCharts", New Object() {strPrefix, strTestDataPath, aryMaxErrors, iMaxRows})
            End If

        End Sub

        Protected Overridable Sub CompareSimulation(ByVal strTestDataPath As String, ByVal aryMaxErrors As Hashtable, _
                                                    Optional ByVal strPrefix As String = "", Optional ByVal iMaxRows As Integer = -1, Optional aryIgnoreRows As ArrayList = Nothing)
            Debug.WriteLine("Comparing simulation output. Test Data Path: '" & strTestDataPath & "', Prefix: '" & strPrefix & "', MaxError: '" & Util.ParamsToString(aryMaxErrors) & "', MaxRows: " & iMaxRows)

            'No prefix on the exported chart.
            ExecuteMethod("ExportDataCharts", New Object() {"", ""})

            Threading.Thread.Sleep(500)

            If aryIgnoreRows Is Nothing Then
                'If no arraylist passed in then just create an empty one.
                aryIgnoreRows = New ArrayList
            End If

            'If we are flagged as needing to generate the template files then lets do that. Otherwise, lets compare the charts to the templates.
            If m_bGenerateTempates Then
                ExecuteMethod("CopyChartData", New Object() {strTestDataPath, strPrefix})
            Else
                ExecuteMethod("CompareExportedDataCharts", New Object() {strPrefix, strTestDataPath, aryMaxErrors, iMaxRows, aryIgnoreRows})
            End If

        End Sub

        Protected Overridable Sub CompareSimulationAnalysis(ByVal strProjectPath As String, ByVal strFilename As String, ByVal strTestDataPath As String, ByVal strPrefix As String, _
                                                            ByVal strColName As String, Optional ByVal iStartIdx As Integer = -1, Optional ByVal iEndIdx As Integer = -1)
            Debug.WriteLine("Comparing simulation analysis. Project Path: '" & strProjectPath & "', Filename: '" & strFilename & "', TestDataPath: '" & _
                            strTestDataPath & "', Prefix: '" & strPrefix & "', ColName: '" & strColName & "', iStartIdx: '" & iStartIdx & "', iEndIdx: '" & iEndIdx)

            'No prefix on the exported chart.
            ExecuteMethod("ExportDataCharts", New Object() {"", ""})

            Dim strFile As String = m_strRootFolder & strProjectPath & "\" & strFilename & ".txt"
            Dim strTemplate As String = strTestDataPath & "\" & strPrefix & strFilename & "_Analysis" & ".txt"

            Dim aryChartColumns() As String = {""}
            Dim aryChartData As New List(Of List(Of Double))
            Util.ReadCSVFileToList(strFile, aryChartColumns, aryChartData, True)

            Dim iTimeIdx As Integer = Util.FindColumnNamed(aryChartColumns, "Time")
            Dim iDataIdx As Integer = Util.FindColumnNamed(aryChartColumns, strColName)

            Dim aryTime As List(Of Double) = aryChartData(iTimeIdx)
            Dim aryData As List(Of Double) = aryChartData(iDataIdx)

            Dim oAnalysis As New DataAnalyzer()
            oAnalysis.FindCriticalPoints(aryTime, aryData, iStartIdx, iEndIdx)

            'Now load comparison template.
            Dim oTemplate As New DataAnalyzer
            oTemplate = DataAnalyzer.LoadData(strTemplate)

            oTemplate.CompareData(oAnalysis, strFilename)

        End Sub

        Protected Overridable Sub LoadDataChart(ByVal strTestDataPath As String, ByVal strChartFileName As String, Optional ByVal strPrefix As String = "")
            Debug.WriteLine("Load Data Chartt. Test Data Path: '" & strTestDataPath & "', Prefix: '" & strPrefix & "', ChartFileName: '" & strChartFileName & "'")

            'Export all charts.
            ExecuteMethod("ExportDataCharts", New Object() {"", ""})
            ExecuteMethod("CopyChartData", New Object() {strTestDataPath, strPrefix})

            Threading.Thread.Sleep(200)

            'Load the template file data.
            Util.ReadCSVFileToArray(m_strRootFolder & m_strTestDataPath & "\" & strPrefix & strChartFileName, m_aryChartColumns, m_aryChartData)

            If m_aryChartColumns Is Nothing OrElse m_aryChartData Is Nothing Then
                Throw New System.Exception("Could not read the template file. ('" & strChartFileName & "')")
            End If

        End Sub

        Protected Overridable Sub CompareColummData(ByVal iColumn As Integer, ByVal iRowStart As Integer, ByVal iRowEnd As Integer, ByVal eCompareType As enumDataComparisonType, _
                                                    ByVal dblRange1 As Double, Optional ByVal dblRange2 As Double = 0, Optional ByVal dblMaxError As Double = 0.05)
            Debug.WriteLine("Compare Columm Data. iColumn: " & iColumn & ", iRowStart: " & iRowStart & ", iRowEnd: " & iRowEnd & ", eCompareType: '" & eCompareType.ToString & _
                            "', dblRange1: " & dblRange1 & ", dblRange2: " & dblRange2 & ", dblMaxError: " & dblMaxError)

            If m_aryChartData Is Nothing OrElse m_aryChartData.Length <= 0 Then
                Throw New System.Exception("The chart data has not been loaded.")
            End If

            If iRowStart <> -1 AndAlso iRowEnd <> -1 Then
                If iRowStart <= 0 Then
                    Throw New System.Exception("Invalid row start index. Row start must be greater than zero.")
                End If

                If iRowEnd <= 0 OrElse iRowEnd <= iRowStart Then
                    Throw New System.Exception("Invalid row end index. Row end must be greater than the row start index.")
                End If

                If iRowEnd >= m_aryChartData.GetLength(1) Then
                    Throw New System.Exception("The row end index cannot be larger than the size of the chart data.")
                End If
            Else
                iRowStart = 0
                iRowEnd = m_aryChartData.GetLength(1) - 1
            End If

            Dim dblMin As Double = 999999
            Dim dblMax As Double = -999999
            Dim dblAvg As Double = 0
            Dim dblAvgTotal As Double = 0
            Dim dblData As Double = 0

            For iRow As Integer = iRowStart To iRowEnd
                dblData = m_aryChartData(iColumn, iRow)

                If dblData < dblMin Then dblMin = dblData
                If dblData > dblMax Then dblMax = dblData
                dblAvgTotal = dblAvgTotal + dblData
            Next

            dblAvg = dblAvgTotal / CDbl(iRowEnd - iRowStart)

            If eCompareType = enumDataComparisonType.Max Then
                If Math.Abs(dblMax - dblRange1) > dblMaxError Then
                    Throw New System.Exception("The data within the range execeeds the maximum value specified. Data: " & dblMax & ", Max: " & dblRange1)
                End If
            ElseIf eCompareType = enumDataComparisonType.Min Then
                If Math.Abs(dblMin - dblRange1) > dblMaxError Then
                    Throw New System.Exception("The data within the range execeeds the minimum value specified. Data: " & dblMin & ", Max: " & dblRange1)
                End If
            ElseIf eCompareType = enumDataComparisonType.Average Then
                If Math.Abs(dblAvg - dblRange1) > dblMaxError Then
                    Throw New System.Exception("The data within the range execeeds the average value specified. Data: " & dblAvg & ", Max: " & dblRange1)
                End If
            ElseIf eCompareType = enumDataComparisonType.WithinRange Then
                If dblMin < dblRange1 OrElse dblMax > dblRange2 Then
                    Throw New System.Exception("The data was not within the range specified. Data: (" & dblMin & ", " & dblMax & ") Range: (" & dblRange1 & ", " & dblRange2 & ")")
                End If
            End If

            Debug.WriteLine("Compare Successful.")
        End Sub

        Protected Sub SetStructureNames(ByVal strPostFix As String, ByVal bIsStructure As Boolean)
            If bIsStructure Then
                m_strStructureGroup = "Structures"
                m_strStruct1Name = "Structure_" & strPostFix
            Else
                m_strStructureGroup = "Organisms"
                m_strStruct1Name = "Organism_" & strPostFix
            End If

            Debug.WriteLine("Set structure names. Struct Group: '" & m_strStructureGroup & "', Struct1 Name: '" & m_strStruct1Name & "'")
        End Sub

        Protected Overridable Sub CreateNewProject(ByVal strProjectName As String, ByVal strProjectPath As String, ByVal dblSimEnd As Double, ByVal bCreateStructure As Boolean)

            Debug.WriteLine("Creating a new project. Project name: '" & strProjectName & "', Project Path: '" & m_strProjectPath & "', Sim End: " & dblSimEnd & ", CreateStructure: " & bCreateStructure)

            OpenDialogAndWait("New Project", Me.GetType.GetMethod("ClickToolbarItem"), New Object() {"NewToolStripButton"})

            'Set params and hit ok button
            ExecuteActiveDialogMethod("SetProjectParams", New Object() {strProjectName, m_strRootFolder & strProjectPath})
            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

            'Set simulation to automatically end at a specified time.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation", "SetSimulationEnd", "True"})

            'Set simulation end time.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation", "SimulationEndTime", dblSimEnd.ToString})

            SetStructureNames("1", bCreateStructure)
            CreateStructure(m_strStructureGroup, m_strStruct1Name, m_strStruct1Name, bCreateStructure)

            Threading.Thread.Sleep(2000)

        End Sub

        Protected Overridable Sub CreateStructure(ByVal strStructGroup As String, ByVal strOrigStructureName As String, ByVal strNewStructureName As String, ByVal bCreateStructure As Boolean)

            Debug.WriteLine("Creating structure. Struct Group: '" & strStructGroup & "', Orig Name: '" & strOrigStructureName & "', New Name: '" & strNewStructureName & "', Create Struct: " & bCreateStructure)

            'Click the add structure button.
            If bCreateStructure Then
                ExecuteMethod("ClickToolbarItem", New Object() {"AddStructureToolStripButton"})
            Else
                ExecuteMethod("ClickToolbarItem", New Object() {"AddOrganismStripButton"})
            End If

            'Set the name of the structure
            If strOrigStructureName <> strNewStructureName Then
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & strStructGroup & "\" & strOrigStructureName, "Name", strNewStructureName})
            End If

            'Open the Structure_1 body plan editor window
            ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\" & strStructGroup & "\" & strNewStructureName & "\Body Plan"}, 2000)

        End Sub

        Protected Overridable Sub CreateChartAndAddBodies()
            Debug.WriteLine("Create Chart And Add Bodies")

            'Select the LineChart to add.
            AddChart("Line Chart")

            'Select the Chart axis
            ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1", False})

            'Change the end time of the data chart to 45 seconds.
            ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart", "CollectEndTime", m_dblChartEndTime.ToString})

            'Now add items to the chart to plot the y position of the root, child part, and joint.
            'Add root part.
            AddItemToChart(m_strStruct1Name & "\Body Plan\Root")

            'Set the name of the data chart item to root_y.
            ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Root", "Name", "Root_Y"})

            'Change the data type to track the world Y position.
            ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Root_Y", "DataTypeID", "WorldPositionY"})

            If Me.HasChildPart Then
                'Add child body part
                AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_1")

                'Set the name of the data chart item to root_y.
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Body_1", "Name", "Child_Y"})

                'Change the data type to track the world Y position.
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Child_Y", "DataTypeID", "WorldPositionY"})

                'Add joint body part
                AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Joint_1")

                'Set the name of the data chart item to root_y.
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Joint_1", "Name", "Joint_Y"})

                'Change the data type to track the world Y position.
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Joint_Y", "DataTypeID", "WorldPositionY"})
            End If

            'Select the simulation window tab so it is visible now.
            ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name}, 1000)
        End Sub


        Protected Overridable Sub AssertInRange(ByVal strPartType As String, ByVal strAxis As String, ByVal strOperation As String, _
                                                     ByVal dblDiff As Double, ByVal dblMinRange As Double, ByVal dblMaxRange As Double)

            If dblDiff < dblMinRange OrElse dblDiff > dblMaxRange Then
                Throw New System.Exception(strPartType & " " & strAxis & " " & strOperation & " is not within the specified range. Value: " & dblDiff & ", Range: (" & dblMinRange & " < " & dblMaxRange & ")")
            End If
        End Sub


        Protected Overridable Sub MovePartAxis(ByVal strStructure As String, ByVal strPart As String, _
                                               ByVal strWorldAxis As String, ByVal strLocalAxis As String, _
                                               ByVal ptAxisStart As Point, ByVal ptAxisEnd As Point, _
                                               ByVal dblMinPartRange As Double, ByVal dblMaxPartRange As Double, _
                                               ByVal dblMinStructRange As Double, ByVal dblMaxStructRange As Double, _
                                               ByVal dblMinLocalRange As Double, ByVal dblMaxLocalRange As Double)
            Debug.WriteLine("MovePartAxis. Structure: " & strStructure & ", Part: " & strPart & ", World Axis: " & strWorldAxis & ", Local Axis: " & strLocalAxis & _
                            "Axis Start: " & ptAxisStart.ToString & ", Axis End: " & ptAxisEnd.ToString & " dblMinPartRange: " & dblMinPartRange & ", dblMaxPartRange" & dblMaxPartRange & _
                            ", dblMinStructRange: " & dblMinStructRange & ", dblMaxStructRange: " & dblMaxStructRange & ", dblMinLocalRange: " & dblMinLocalRange & ", dblMaxLocalRange: " & dblMaxLocalRange)

            'Get the part and structure x position before movement
            Dim dblBeforePartPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WorldPosition." & strWorldAxis & ".ActualValue"), Double)
            Dim dblBeforeStructPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure, "LocalPosition." & strWorldAxis & ".ActualValue"), Double)
            Dim dblBeforeLocalPartPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "LocalPosition." & strLocalAxis & ".ActualValue"), Double)

            'Move axis
            DragMouse(ptAxisStart, ptAxisEnd, MouseButtons.Left)

            Threading.Thread.Sleep(200)

            'Get the part and structure x position after movement
            Dim dblAfterPartPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WorldPosition." & strWorldAxis & ".ActualValue"), Double)
            Dim dblAfterStructPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure, "LocalPosition." & strWorldAxis & ".ActualValue"), Double)
            Dim dblAfterLocalPartPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "LocalPosition." & strLocalAxis & ".ActualValue"), Double)

            AssertInRange("Part", strWorldAxis, "position", (dblAfterPartPos - dblBeforePartPos), dblMinPartRange, dblMaxPartRange)
            AssertInRange("Structure", strWorldAxis, "position", (dblAfterStructPos - dblBeforeStructPos), dblMinStructRange, dblMaxStructRange)
            AssertInRange("Part Local", strLocalAxis, "position", (dblAfterLocalPartPos - dblBeforeLocalPartPos), dblMinLocalRange, dblMaxLocalRange)

        End Sub

        Protected Overridable Sub RotatePartAxis(ByVal strStructure As String, ByVal strPart As String, _
                                                 ByVal strAxis As String, ByVal ptAxisStart As Point, ByVal ptAxisEnd As Point, _
                                                ByVal dblMinRange As Double, ByVal dblMaxRange As Double, Optional ByVal bResetPos As Boolean = True)
            Debug.WriteLine("RotatePartAxis. Structure: " & strStructure & ", Part: " & strPart & ", Axis: " & strAxis & _
                "Axis Start: " & ptAxisStart.ToString & ", Axis End: " & ptAxisEnd.ToString & " dblMinRange: " & dblMinRange & ", dblMaxRange" & dblMaxRange & _
                ", bResetPos: " & bResetPos)

            'Get the part rotation before movement
            Dim dblBeforePartPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis & ".ActualValue"), Double)

            'Move axis
            DragMouse(ptAxisStart, ptAxisEnd, MouseButtons.Left)

            Threading.Thread.Sleep(200)

            'Get the part and structure x position before movement
            Dim dblAfterPartPos As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis & ".ActualValue"), Double)

            AssertInRange("Part", strAxis, "rotation", (dblAfterPartPos - dblBeforePartPos), dblMinRange, dblMaxRange)

            'Reset the rotation to 0.
            If bResetPos Then
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis, dblBeforePartPos.ToString})
            End If

        End Sub

        Protected Overridable Sub ResetStructurePosition(ByVal strStructure As String, ByVal strPart As String, _
                                                         ByVal dblPosX As Double, ByVal dblPosY As Double, ByVal dblPosZ As Double, _
                                                         Optional ByVal bVerifyRoot As Boolean = True, Optional ByVal dblMaxError As Double = 0.001)
            Debug.WriteLine("ResetStructurePosition. Structure: " & strStructure & ", Part: " & strPart & ", dblPosX: " & dblPosX & ", dblPosY: " & dblPosY & _
                            "dblPosZ: " & dblPosZ & ", bVerifyRoot: " & bVerifyRoot & " dblMaxError: " & dblMaxError)

            'Reset the position of the Structure.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure, "LocalPosition.X", dblPosX.ToString})
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure, "LocalPosition.Y", dblPosY.ToString})
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure, "LocalPosition.Z", dblPosZ.ToString})

            Threading.Thread.Sleep(50)

            'Verify that the structure position is correct now.
            Dim dblStructPosX As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure, "LocalPosition.X.ActualValue"), Double)
            Dim dblStructPosY As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure, "LocalPosition.Y.ActualValue"), Double)
            Dim dblStructPosZ As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure, "LocalPosition.Z.ActualValue"), Double)

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
                Dim dblRootPosX As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WorldPosition.X.ActualValue"), Double)
                Dim dblRootPosY As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WorldPosition.Y.ActualValue"), Double)
                Dim dblRootPosZ As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WorldPosition.Z.ActualValue"), Double)

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

        Protected Overridable Sub ManualMovePartAxis(ByVal strStructure As String, ByVal strPart As String, _
                                                     ByVal strWorldAxis As String, ByVal strLocalAxis As String, _
                                                     ByVal dblWorldPos As Double, ByVal dblWorldTest As Double, _
                                                     ByVal dblWorldLocalTest As Double, ByVal bTestLocal As Boolean, ByVal dblLocalPos As Double, _
                                                     ByVal dblLocalTest As Double, ByVal dblLocalWorldTest As Double, Optional ByVal dblMaxError As Double = 0.001)
            Debug.WriteLine("ManualMovePartAxis. Structure: " & strStructure & ", Part: " & strPart & ", World Axis: " & strWorldAxis & ", Local Axis: " & strLocalAxis & _
                             "dblWorldPos: " & dblWorldPos & ", dblWorldTest: " & dblWorldTest & " dblWorldLocalTest: " & dblWorldLocalTest & ", bTestLocal" & bTestLocal & _
                             ", dblLocalPos: " & dblLocalPos & ", dblLocalTest: " & dblLocalTest & ", dblLocalWorldTest: " & dblLocalWorldTest & ", dblMaxError: " & dblMaxError)

            'Move the root part along the axis using world coordinates.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WorldPosition." & strWorldAxis, dblWorldPos.ToString})

            'Now get the world position and verify it.
            Dim dblPosWorld As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WorldPosition." & strWorldAxis & ".ActualValue"), Double)
            Dim dblPosLocal As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "LocalPosition." & strLocalAxis & ".ActualValue"), Double)

            Threading.Thread.Sleep(50)

            If Math.Abs(dblPosWorld - dblWorldTest) > dblMaxError Then
                Throw New System.Exception("Body part position does not match the world target value: " & dblPosWorld & ", recorded value: " & dblWorldTest)
            End If

            If Math.Abs(dblPosLocal - dblWorldLocalTest) > dblMaxError Then
                Throw New System.Exception("Body part position does not match the local target value: " & dblPosLocal & ", recorded value: " & dblWorldLocalTest)
            End If

            'Move the root part along the axis using local coordinates.
            If bTestLocal Then
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "LocalPosition." & strLocalAxis, dblLocalPos.ToString})

                'Now get the world position and verify it.
                dblPosWorld = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WorldPosition." & strWorldAxis & ".ActualValue"), Double)
                dblPosLocal = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "LocalPosition." & strLocalAxis & ".ActualValue"), Double)

                If Math.Abs(dblPosLocal - dblLocalTest) > dblMaxError Then
                    Throw New System.Exception("Body part position does not match the local target value: " & dblPosLocal & ", recorded value: " & dblLocalTest)
                End If

                If Math.Abs(dblPosWorld - dblLocalWorldTest) > dblMaxError Then
                    Throw New System.Exception("Body part position does not match the world target value: " & dblPosWorld & ", recorded value: " & dblLocalWorldTest)
                End If
            End If

        End Sub

        Protected Overridable Function ManualRotatePartAxis(ByVal strStructure As String, ByVal strPart As String, ByVal strAxis As String, _
                                                       ByVal dblRotation As Double, Optional ByVal bReset As Boolean = True, Optional ByVal dblMaxError As Double = 0.001) As Double
            Debug.WriteLine("ManualRotatePartAxis. Structure: " & strStructure & ", Part: " & strPart & ", Axis: " & strAxis & ", dblRotation: " & dblRotation & _
                 "bReset: " & bReset & ", dblMaxError: " & dblMaxError)

            'Now beginning rotation
            Dim dblOrigRot As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis & ".ActualValue"), Double)

            'Move the root part along the axis using world coordinates.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis, dblRotation.ToString})

            Threading.Thread.Sleep(50)

            'Now get the world position and verify it.
            Dim dblRealRot As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis & ".ActualValue"), Double)

            If Math.Abs(dblRealRot - dblRotation) > dblMaxError Then
                Throw New System.Exception("Body part rotation does not match the target value: " & dblRealRot & ", recorded value: " & dblRotation)
            End If

            If bReset Then
                'Reset the rotation
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Rotation." & strAxis, dblOrigRot.ToString})
            End If

            Return dblOrigRot
        End Function

        Protected Overridable Sub RecalculatePositionsUsingResolution()
            Dim uIStructure_1BodyClient As WinClient = Me.UIProjectWindow(m_strProjectName).UIStructure_1BodyWindow.UIStructure_1BodyClient(m_strStruct1Name)

            m_szCurrrentResoution = New Size(uIStructure_1BodyClient.BoundingRectangle.Width, uIStructure_1BodyClient.BoundingRectangle.Height)

            m_dblResScaleWidth = m_szCurrrentResoution.Width / m_szOriginalResoution.Width
            m_dblResScaleHeight = m_szCurrrentResoution.Height / m_szOriginalResoution.Height

            Debug.WriteLine("RecalculatePositionsUsingResolution. m_dblResScaleWidth: " & m_dblResScaleWidth & ", m_dblResScaleHeight: " & m_dblResScaleHeight)
        End Sub

        Protected Sub VerifyPropertyValue(ByVal strPath As String, ByVal strProperty As String, ByVal dblValue As Double, Optional ByVal dblMaxError As Double = 0.001)
            Dim dblPosX As Double = DirectCast(GetSimObjectProperty(strPath, strProperty), Double)

            If Math.Abs(dblPosX - dblValue) > dblMaxError Then
                Throw New System.Exception("Property does not match the target value: Path: " & strPath & ", Property: " & strProperty & ", Actual Val: " & dblPosX & ", Test value: " & dblValue)
            End If

        End Sub

        Protected Overridable Sub VerifyChildPosAfterRotate(ByVal strStructure As String, ByVal strAxis As String, ByVal strPart As String, _
                                                            ByVal dblRotation As Double, ByVal dblWorldXTest As Double, _
                                                            ByVal dblWorldYTest As Double, ByVal dblWorldZTest As Double, _
                                                            Optional ByVal dblMaxError As Double = 0.001)
            Debug.WriteLine("VerifyChildPosAfterRotate. Structure: " & strStructure & ", Part: " & strPart & ", Axis: " & strAxis & ", dblRotation: " & dblRotation & _
                 "dblWorldXTest: " & dblWorldXTest & ", dblWorldYTest: " & dblWorldYTest & " dblWorldZTest: " & dblWorldZTest & ", dblMaxError: " & dblMaxError)

            'rotate the root and verify the child position.
            Dim dblOrigRot As Double = ManualRotatePartAxis(strStructure, "Root", strAxis, dblRotation, False)

            Dim dblWorldX As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WorldPosition.X.ActualValue"), Double)
            Dim dblWorldY As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WorldPosition.Y.ActualValue"), Double)
            Dim dblWorldZ As Double = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "WorldPosition.Z.ActualValue"), Double)

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
            ManualRotatePartAxis(strStructure, "Root", strAxis, dblOrigRot, False)

        End Sub


        Protected Overridable Sub TestMovableItemProperties(ByVal strStructure As String, ByVal strPart As String)
            Debug.WriteLine("TestMovableItemProperties. Structure: " & strStructure & ", Part: " & strPart)

            TestSettingBodyColors(strStructure, strPart)

            TestSettingBodyTexture(strStructure, strPart)

            TestSettingBodyVisibility(strStructure, strPart)

            'Set the Description to a valid value.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Description", "Test"})

            'Set the Name to a valid value.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Name", "Test"})

            'Set the Name to an valid value.
            ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\Test", "Name", ""}, "The name property can not be blank.")

            'Reset the name to root.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\Test", "Name", strPart})

        End Sub

        Protected Overridable Sub TestSettingBodyColors(ByVal strStructure As String, ByVal strPart As String)
            Debug.WriteLine("TestSettingBodyColors. Structure: " & strStructure & ", Part: " & strPart)

            'Set the ambient to a valid value.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Ambient", m_strAmbient})

            'Set the diffuse to a valid value.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Diffuse", m_strDiffuse})

            'Set the specular to a valid value.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Specular", m_strSpecular})

            'Set the shininess to a valid value.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Shininess", m_iShininess.ToString})

            'Set the shininess to an valid value.
            ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Shininess", "-1"}, "Shininess must be greater than or equal to zero.")

            'Set the shininess to an valid value.
            ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Shininess", "129"}, "Shininess must be less than 128.")
        End Sub

        Protected Overridable Sub TestSettingBodyTexture(ByVal strStructure As String, ByVal strPart As String)
            Debug.WriteLine("TestSettingBodyTexture. Structure: " & strStructure & ", Part: " & strPart)

            If m_bTestTexture Then
                'Set the texture to an valid value.
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Texture", _
                                                                            (m_strRootFolder & "\bin\Resources\" & m_strTextureFile)})
                'Set the texture to an invalid value.
                ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Texture", _
                                                                            (m_strRootFolder & "\bin\Resources\Bricks.gif")}, "The specified file does not exist: ", enumErrorTextType.BeginsWith)

                'Set the texture to an invalid value.
                ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Texture", _
                                                                            (m_strRootFolder & "\bin\Resources\Test.txt")}, "Unable to load the texture file. This does not appear to be a vaild image file.", enumErrorTextType.BeginsWith)
            End If

        End Sub

        Protected Overridable Sub TestSettingHeightMap(ByVal strStructure As String, ByVal strPart As String)
            Debug.WriteLine("TestSettingHeightMap. Structure: " & strStructure & ", Part: " & strPart)

            'Set the texture to an valid value.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "MeshFile", _
                                                                        (m_strRootFolder & "\bin\Resources\" & m_strMeshFile)})
            'Set the texture to an invalid value.
            ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "MeshFile", _
                                                                        (m_strRootFolder & "\bin\Resources\Bricks.gif")}, "The specified file does not exist: ", enumErrorTextType.BeginsWith)

            'Set the texture to an invalid value.
            ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "MeshFile", _
                                                                        (m_strRootFolder & "\bin\Resources\Test.txt")}, "Unable to load the height map file. This does not appear to be a vaild image file.", enumErrorTextType.BeginsWith)

        End Sub

        Protected Overridable Sub TestSettingBodyVisibility(ByVal strStructure As String, ByVal strPart As String)
            Debug.WriteLine("TestSettingBodyVisibility. Structure: " & strStructure & ", Part: " & strPart)

            'Set the visible to a valid value.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Visible", "False"})

            'Turn visible back on.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Visible", "True"})

            TestSettingTransparency(strStructure, strPart, "Transparencies.GraphicsTransparency")
            TestSettingTransparency(strStructure, strPart, "Transparencies.CollisionsTransparency")
            TestSettingTransparency(strStructure, strPart, "Transparencies.JointsTransparency")
            TestSettingTransparency(strStructure, strPart, "Transparencies.ReceptiveFieldsTransparency")
            TestSettingTransparency(strStructure, strPart, "Transparencies.SimulationTransparency")

            'If this is the root part then set its child graphics object to be clear for the rest of the tests so it does not look funky.
            If strPart = "Root" AndAlso HasRootGraphic() Then
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart & "\Root_Graphics", "Transparencies.CollisionsTransparency", "100"})
            End If
        End Sub

        Protected Overridable Sub TestSettingTransparency(ByVal strStructure As String, ByVal strPart As String, ByVal strTransparency As String)
            Debug.WriteLine("TestSettingTransparency. Structure: " & strStructure & ", Part: " & strPart)

            'Get original value
            Dim fltOrigRot As Single = DirectCast(GetSimObjectProperty("Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, strTransparency), Single)

            'Set the Transparencies.GraphicsTransparency to a valid value.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, strTransparency, "50"})

            'Set the Transparencies.GraphicsTransparency to high.
            ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, strTransparency, "150"}, "Transparency values cannot be greater than 100%.")

            'Set the Transparencies.GraphicsTransparency too low
            ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, strTransparency, "-50"}, "Transparency values cannont be less than 0%.")

            'reset original value
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, strTransparency, fltOrigRot.ToString})

        End Sub


#Region "GenerateCode"

        Protected Overridable Sub ProcessExtraAddRootMethods(ByVal strPartType As String)

        End Sub

        '''<summary>
        '''AddRootPartType - Use 'AddRootPartTypeParams' to pass parameters into this method.
        '''</summary>
        Protected Overridable Sub AddRootPartType(ByVal strStructGroup As String, ByVal strStructure As String, ByVal strPartType As String, Optional ByVal strName As String = "")
            Debug.WriteLine("AddRootPartType. Structure Group:, " & strStructGroup & ", Structure: " & strStructure & ", PartType: " & strPartType & ", Name: " & strName)

            OpenDialogAndWait("Select Part Type", Me.GetType.GetMethod("ClickToolbarItem"), New Object() {"AddPartToolStripButton"})

            ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strPartType})

            'Click 'Ok' button
            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

            ProcessExtraAddRootMethods(strPartType)

            Threading.Thread.Sleep(2000)

            'There is some kind of weird timing bug in the testing code here. When I add a part manually it goes to SelectCollisions mode just fine,
            'but when I do it here for some reason I have to set it to something else first and then back. I think it has something to do with the 
            ' timing of the call or something. Regardless, it does not really matter here, I just need it in Collisions mode and that works when done
            ' manually, so I am using this trick to get it to work in the test.
            ExecuteMethod("ClickToolbarItem", New Object() {"SelGraphicsToolStripButton"})
            ExecuteMethod("ClickToolbarItem", New Object() {"SelCollisionToolStripButton"})
            ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\" & strStructGroup & "\" & strStructure & "\Body Plan\Root", False})

            If strName.Length > 0 Then
                ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & strStructGroup & "\" & strStructure & "\Body Plan\Root", "Name", strName})
            End If
        End Sub

        Public Overridable Sub ClickToAddBody(ByVal ptClick As Point)
            Debug.WriteLine("ClickToAddBody. ptClick:, " & ptClick.ToString)

            Dim uIStructure_1BodyClient As WinClient = Me.UIProjectWindow(m_strProjectName).UIStructure_1BodyWindow.UIStructure_1BodyClient(m_strStruct1Name)
            Mouse.Click(uIStructure_1BodyClient, ptClick)
        End Sub

        Public Overridable Sub AutomatedClickToAddBody(ByVal strPath As String, ByVal fltPosX As Single, ByVal fltPosY As Single, ByVal fltPosZ As Single, _
                                                   ByVal fltNormX As Single, ByVal fltNormY As Single, ByVal fltNormZ As Single)
            Debug.WriteLine("AutomatedClickToAddBody. Path: '" & strPath & "', PosX: " & fltPosX & ", PosY: " & fltPosY & ", PosZ: " & fltPosZ & ", NormX: " & fltNormX & ", NormY: " & fltNormY & ", NormZ: " & fltNormZ)

            Dim aryInnerParams As Object() = New Object() {fltPosX, fltPosY, fltPosZ, fltNormX, fltNormY, fltNormZ}
            Dim aryParams As Object() = New Object() {strPath, "Automation_AddBodyClicked", aryInnerParams}

            ExecuteDirectMethod("ExecuteIndirectMethodOnObject", aryParams)
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

        Protected Overridable Sub AssertErrorDialogShown(ByVal strErrorMsg As String, ByVal eMatchType As enumMatchType)
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
                Case enumMatchType.Equals
                    If strError <> strErrorMsg Then
                        Throw New System.Exception("Error did not match.")
                    End If

                Case enumMatchType.Contains
                    If Not strError.Contains(strErrorMsg) Then
                        Throw New System.Exception("Error did not match.")
                    End If

                Case enumMatchType.BeginsWith
                    If Not strError.StartsWith(strErrorMsg) Then
                        Throw New System.Exception("Error did not match.")
                    End If

                Case enumMatchType.EndsWith
                    If Not strError.EndsWith(strErrorMsg) Then
                        Throw New System.Exception("Error did not match.")
                    End If

                Case Else
                    Throw New System.Exception("Inavlid match type provided: " & eMatchType.ToString)
            End Select

            Threading.Thread.Sleep(1000)
            Debug.WriteLine("Error dialog shown correctly.")
        End Sub

        Protected Overridable Sub BeforeAddChildPart(ByVal strPartType As String, ByVal strJointType As String)

        End Sub

        Protected Overridable Sub AfterAddChildPart(ByVal strPartType As String, ByVal strJointType As String)

        End Sub

        Protected Overridable Sub AfterAddChildPartJoint(ByVal strPartType As String, ByVal strJointType As String)

        End Sub

        '''<summary>
        '''AddChildPartTypeWithJoint - Use 'AddChildPartTypeWithJointParams' to pass parameters into this method.
        '''</summary>
        Protected Overridable Sub AddChildPartTypeWithJoint(ByVal strPartType As String, ByVal strJointType As String, ByVal ptAddClick As Point)
            Debug.WriteLine("AddChildPartTypeWithJoint. Part type: " & strPartType & ", Joint Type: " & strJointType & ", AddClick: " & ptAddClick.ToString)

            BeforeAddChildPart(strPartType, strJointType)

            'Click 'Add Part' button
            ExecuteMethod("ClickToolbarItem", New Object() {"AddPartToolStripButton"}, 2000)

            OpenDialogAndWait("Select Part Type", Me.GetType.GetMethod("ClickToAddBody"), New Object() {ptAddClick})

            ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strPartType})

            'Click 'Ok' button
            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

            AfterAddChildPart(strPartType, strJointType)

            OpenDialogAndWait("Select Part Type", Nothing, Nothing)

            ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strJointType})

            'Click 'Ok' button
            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

            AfterAddChildPartJoint(strPartType, strJointType)

            Threading.Thread.Sleep(1000)
        End Sub

        Protected Overridable Sub AddChildPartTypeWithJoint(ByVal strPartType As String, ByVal strJointType As String, ByVal strPath As String)
            Debug.WriteLine("AddChildPartTypeWithJoint. Part type: " & strPartType & ", Joint Type: " & strJointType & ", Path: " & strPath)

            BeforeAddChildPart(strPartType, strJointType)

            'Click 'Add Part' button
            ExecuteMethod("ClickToolbarItem", New Object() {"AddPartToolStripButton"}, 2000)

            AutomatedClickToAddBody(strPath, 0.04, 0.55, -0.5, 0.0, 0.0, -1.0)

            ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strPartType})

            'Click 'Ok' button
            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

            AfterAddChildPart(strPartType, strJointType)

            OpenDialogAndWait("Select Part Type", Nothing, Nothing)

            ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strJointType})

            'Click 'Ok' button
            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

            AfterAddChildPartJoint(strPartType, strJointType)

            Threading.Thread.Sleep(1000)
        End Sub

        '''<summary>
        '''AddChildPartTypeWithJoint - Use 'AddChildPartTypeWithJointParams' to pass parameters into this method.
        '''</summary>
        Protected Overridable Sub AddChildPartTypeWithoutJoint(ByVal strPartType As String, ByVal ptAddClick As Point)
            Debug.WriteLine("AddChildPartTypeWithJoint. Part type: " & strPartType & ", AddClick: " & ptAddClick.ToString)

            BeforeAddChildPart(strPartType, "")

            'Click 'Add Part' button
            ExecuteMethod("ClickToolbarItem", New Object() {"AddPartToolStripButton"}, 2000)

            OpenDialogAndWait("Select Part Type", Me.GetType.GetMethod("ClickToAddBody"), New Object() {ptAddClick})

            ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strPartType})

            'Click 'Ok' button
            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

            AfterAddChildPart(strPartType, "")

            Threading.Thread.Sleep(1000)
        End Sub

        '''<summary>
        '''AddChildPartTypeWithJoint - Use 'AddChildPartTypeWithJointParams' to pass parameters into this method.
        '''</summary>
        Protected Overridable Sub PasteChildPartTypeWithJoint(ByVal strJointType As String, ByVal strPath As String, ByVal fltPosX As Single, ByVal fltPosY As Single, ByVal fltPosZ As Single, _
                                                              ByVal fltNormX As Single, ByVal fltNormY As Single, ByVal fltNormZ As Single, ByVal bHasJoint As Boolean)
            Debug.WriteLine("PasteChildPartTypeWithJoint. Joint Type: " & strJointType & ", Path: '" & strPath & "', PosX: " & fltPosX & ", PosY: " & fltPosY & ", PosZ: " & fltPosZ & _
                            ", NormX: " & fltNormX & ", NormY: " & fltNormY & ", NormZ: " & fltNormZ & ", bHasJoint: " & bHasJoint)

            BeforeAddChildPart("", strJointType)

            'Click 'Add Part' button
            ExecuteMethod("ClickToolbarItem", New Object() {"PasteToolStripButton"}, 2000)

            AutomatedClickToAddBody(strPath, fltPosX, fltPosY, fltPosZ, fltNormX, fltNormY, fltNormZ)

            AfterAddChildPart("", strJointType)

            If bHasJoint Then
                OpenDialogAndWait("Select Part Type", Nothing, Nothing)

                ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strJointType})

                'Click 'Ok' button
                ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                AfterAddChildPartJoint("", strJointType)
            End If

            Threading.Thread.Sleep(1000)
        End Sub

        '''<summary>
        '''AddLineChart - Use 'AddLineChartParams' to pass parameters into this method.
        '''</summary>
        Protected Overridable Sub AddChart(ByVal strChartType As String)
            Debug.WriteLine("Adding chart: '" & strChartType & "'")

            OpenDialogAndWait("Select Data Tool Type", Me.GetType.GetMethod("ClickToolbarItem"), New Object() {"AddToolToolStripButton"})

            ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strChartType})

            'Click 'Ok' button
            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

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
            Debug.WriteLine("Adding item to chart: '" & strPath & "'")

            'Click 'Add Chart Item' button
            OpenDialogAndWait("Select Data Item", Me.GetType.GetMethod("ClickToolbarItem"), New Object() {"AddDataItemToolStripButton"})

            ExecuteActiveDialogMethod("SelectItem", New Object() {strPath})
            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

            Threading.Thread.Sleep(2000)
        End Sub

        Protected Overridable Sub AddItemToChart(ByVal strAxisPath As String, ByVal strChartPath As String, ByVal strOldName As String, ByVal strNewName As String, Optional ByVal strDataType As String = "")
            ExecuteMethod("SelectWorkspaceItem", New Object() {strAxisPath, False})
            AddItemToChart(strChartPath)
            ExecuteMethod("SetObjectProperty", New Object() {strAxisPath & "\" & strOldName, "Name", strNewName})

            If strDataType.Trim.Length > 0 Then
                ExecuteMethod("SetObjectProperty", New Object() {strAxisPath & "\" & strNewName, "DataType", strDataType})
            End If
        End Sub

        '''<summary>
        '''AddLineChart - Use 'AddLineChartParams' to pass parameters into this method.
        '''</summary>
        Protected Overridable Sub AddStimulus(ByVal strStimulusType As String, ByVal strStructure As String, ByVal strPart As String, _
                                              Optional ByVal strName As String = "", Optional ByVal strOldName As String = "")
            Debug.WriteLine("AddStimulus. StimulusType: " & strStimulusType & ", Structure: " & strStructure & ", Name: " & strName & ", Old Name: " & strOldName)

            If strOldName.Trim.Length = 0 Then
                strOldName = "Stimulus_" & m_iStimCounter
            End If
            m_iStimCounter = m_iStimCounter + 1

            ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & strPart, False})

            OpenDialogAndWait("Select Stimulus Type", Me.GetType.GetMethod("ClickToolbarItem"), New Object() {"AddStimulusToolStripButton"})

            ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strStimulusType})

            'Click 'Ok' button
            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

            If strName.Length > 0 Then
                ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strOldName, "Name", strName})
            End If

            Threading.Thread.Sleep(1000)
        End Sub

        Protected Overridable Sub SetForceStimulus(ByVal strStimName As String, ByVal bAlwaysActive As Boolean, ByVal bEnabled As Boolean, _
                                                   ByVal dblStartTime As Double, ByVal dblEndTime As Double, _
                                                   ByVal dblPosX As Double, ByVal dblPosY As Double, ByVal dblPosZ As Double, _
                                                   ByVal dblForceX As Double, ByVal dblForceY As Double, ByVal dblForceZ As Double, _
                                                   ByVal dblTorqueX As Double, ByVal dblTorqueY As Double, ByVal dblTorqueZ As Double)
            Debug.WriteLine("SetForceStimulus. strStimName: " & strStimName & ", bAlwaysActive: " & bAlwaysActive & ", bEnabled: " & bEnabled & ", dblStartTime: " & dblStartTime & _
                             "dblEndTime: " & dblEndTime & ", dblPosX: " & dblPosX & " dblPosY: " & dblPosY & ", dblPosZ" & dblPosZ & _
                             ", dblForceX: " & dblForceX & ", dblForceY: " & dblForceY & ", dblForceZ: " & dblForceZ & _
                             ", dblTorqueX: " & dblTorqueX & ", dblTorqueY: " & dblTorqueY & ", dblTorqueZ: " & dblTorqueZ)

            SetBaseStimulusProperties(strStimName, bAlwaysActive, bEnabled, dblStartTime, dblEndTime)

            'Set the force position.
            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "PositionX", dblPosX.ToString})
            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "PositionY", dblPosY.ToString})
            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "PositionZ", dblPosZ.ToString})

            'Set the force vector.
            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "ForceX", dblForceX.ToString})
            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "ForceY", dblForceY.ToString})
            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "ForceZ", dblForceZ.ToString})

            'Set the force vector.
            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "TorqueX", dblTorqueX.ToString})
            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "TorqueY", dblTorqueY.ToString})
            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "TorqueZ", dblTorqueZ.ToString})

        End Sub

        Protected Overridable Sub SetMotorVelocityStimulus(ByVal strStimName As String, ByVal bAlwaysActive As Boolean, ByVal bEnabled As Boolean, _
                                                 ByVal dblStartTime As Double, ByVal dblEndTime As Double, ByVal bDisableWhenDone As Boolean, _
                                                 ByVal bConstantValueType As Boolean, ByVal dblVelocity As Double, ByVal strEquation As String)
            Debug.WriteLine("SetMotorVelocityStimulus. strStimName: " & strStimName & ", bAlwaysActive: " & bAlwaysActive & ", bEnabled: " & bEnabled & ", dblStartTime: " & dblStartTime & _
                            "dblEndTime: " & dblEndTime & ", bDisableWhenDone: " & bDisableWhenDone & " bConstantValueType: " & bConstantValueType & ", dblVelocity" & dblVelocity & _
                            ", strEquation: " & strEquation)

            SetBaseStimulusProperties(strStimName, bAlwaysActive, bEnabled, dblStartTime, dblEndTime)

            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "DisableWhenDone", bDisableWhenDone.ToString})

            If bConstantValueType Then
                ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "ValueType", "Constant"})
                ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "Velocity", dblVelocity.ToString})
            Else
                ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "ValueType", "Equation"})
                ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "Equation", strEquation})
            End If

        End Sub

        Protected Overridable Sub SetBaseStimulusProperties(ByVal strStimName As String, ByVal bAlwaysActive As Boolean, _
                                                            ByVal bEnabled As Boolean, ByVal dblStartTime As Double, ByVal dblEndTime As Double)

            'Set the stim properties
            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "AlwaysActive", bAlwaysActive.ToString})
            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "Enabled", bEnabled.ToString})
            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "StartTime", dblStartTime.ToString})
            ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\" & strStimName, "EndTime", dblEndTime.ToString})
        End Sub


        Protected Overridable Sub CreateArmature(ByVal strPartType As String, ByVal strSecondaryPartType As String, _
                                                 ByVal strJointType As String, ByVal ptClickToAddChild As Point, _
                                                 ByVal ptZoomStart As Point, ByVal iZoom1 As Integer, ByVal iZoom2 As Integer, _
                                                 ByVal bAddAttachments As Boolean, ByVal strAttachType As String, _
                                                 ByVal ptRootAttach As Point, ByVal ptArmAttach As Point)
            Debug.WriteLine("CreateArmature")

            'Add a root part.
            AddRootPartType(m_strStructureGroup, m_strStruct1Name, strPartType)

            RecalculatePositionsUsingResolution()

            'Zoom in on the part so we can try and move it with the mouse.
            ZoomInOnPart(ptZoomStart, iZoom1, iZoom2)

            If strSecondaryPartType.Trim.Length = 0 Then
                strSecondaryPartType = strPartType
            End If

            'We have tested moving/rotating the root part, now test doing it on a child part.
            AddChildPartTypeWithJoint(strSecondaryPartType, strJointType, ptClickToAddChild)

            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_1\Body_1_Graphics", "Name", "Arm_Graphics"})
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Body_1", "Name", "Arm"})

            RepositionChildPart()

            Dim iBlockerIndex As Integer = 2
            If bAddAttachments Then
                AddChildPartTypeWithoutJoint(strAttachType, ptRootAttach)
                AddChildPartTypeWithoutJoint(strAttachType, ptRootAttach)
                AddChildPartTypeWithoutJoint(strAttachType, ptArmAttach)
                RepositionArmatureAttachments()
                iBlockerIndex = 5
            End If

            AddChildPartTypeWithJoint("Box", "Hinge", ptRootAttach)

            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Body_" & iBlockerIndex & "\Body_" & iBlockerIndex & "_Graphics", "Name", "Blocker_Graphics"})
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Body_" & iBlockerIndex, "Name", "Blocker"})

            RepositionBlockerPart()

            'Add motor velocity to joint. Set it to no velocity and always enabled. We want to lock this joint. 
            'We cannot use a static part here because it is part of the geometry of the root, so collisions between it and arm will be disabled.
            AddStimulus("Motor Velocity", m_strStruct1Name, "\Body Plan\Root\Joint_2", "BlockLock") ', "Stimulus_1"
            SetMotorVelocityStimulus("BlockLock", True, True, 0, 5, False, True, 0, "")
        End Sub

        Protected Overridable Sub RepositionArmatureAttachments()
            Debug.WriteLine("RepositionArmatureAttachments")

            'First rename the attachments.
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Body_2", "Name", "RootAttach1"})
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Body_3", "Name", "RootAttach2"})
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\Body_4", "Name", "ArmAttach"})

            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\RootAttach1", "LocalPosition.X", "0.025"})
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\RootAttach1", "LocalPosition.Y", "0.06"})
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\RootAttach1", "LocalPosition.Z", "0"})

            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\RootAttach2", "LocalPosition.X", "0.070"})
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\RootAttach2", "LocalPosition.Y", "0.06"})
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\RootAttach2", "LocalPosition.Z", "0"})

            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\ArmAttach", "LocalPosition.X", "0.075"})
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\ArmAttach", "LocalPosition.Y", "0.025"})
            ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\ArmAttach", "LocalPosition.Z", "0"})
        End Sub

        Protected Overridable Sub RepositionRootArmatureAttach1()

        End Sub

        Protected Overridable Sub RepositionBlockerPart()

        End Sub

        Protected Overridable Sub CreateArmatureChart(ByVal bChartAttachments As Boolean)
            Debug.WriteLine("CreateArmatureChart")

            'Select the LineChart to add.
            AddChart("Line Chart")

            'Select the Chart axis
            ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1", False})

            'Change the end time of the data chart to 45 seconds.
            ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart", "CollectEndTime", m_dblChartEndTime.ToString})

            AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm")
            ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Arm", "Name", "Arm_X"})
            ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Arm_X", "DataTypeID", "WorldPositionX"})

            AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm")
            ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Arm", "Name", "Arm_Y"})
            ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Arm_Y", "DataTypeID", "WorldPositionY"})

            AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm")
            ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Arm", "Name", "Arm_Z"})
            ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\Arm_Z", "DataTypeID", "WorldPositionZ"})

            If bChartAttachments Then
                AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\ArmAttach")
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\ArmAttach", "Name", "ArmAttach_X"})
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\ArmAttach_X", "DataTypeID", "WorldPositionX"})

                AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\ArmAttach")
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\ArmAttach", "Name", "ArmAttach_Y"})
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\ArmAttach_Y", "DataTypeID", "WorldPositionY"})

                AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\ArmAttach")
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\ArmAttach", "Name", "ArmAttach_Z"})
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 1\ArmAttach_Z", "DataTypeID", "WorldPositionZ"})
            End If

            If m_strJointChartMovementName.Length > 0 Then
                'Add a new axis to chart the joint rotation.
                ExecuteMethod("ClickToolbarItem", New Object() {"AddAxisToolStripButton"})

                AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Joint_1")
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 2\Joint_1", "Name", m_strJointChartMovementName})
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 2\" & m_strJointChartMovementName, "DataTypeID", m_strJointChartMovementType})
            End If

            If m_strJointChartVelocityName.Length > 0 Then
                'Add a new axis to chart the joint velocity.
                ExecuteMethod("ClickToolbarItem", New Object() {"AddAxisToolStripButton"})

                AddItemToChart(m_strStruct1Name & "\Body Plan\Root\Joint_1")
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 3\Joint_1", "Name", m_strJointChartVelocityName})
                ExecuteMethod("SetObjectProperty", New Object() {"Tool Viewers\DataTool_1\LineChart\Y Axis 3\" & m_strJointChartVelocityName, "DataTypeID", m_strJointChartVelocityType})
            End If

            'Select the simulation window tab so it is visible now.
            ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name}, 1000)
        End Sub

        Protected Overridable Sub RepositionChildPart()

        End Sub

        Protected Overridable Sub DeletePart(ByVal strPath As String, ByVal strDlgName As String, Optional ByVal bCut As Boolean = False)
            Debug.WriteLine("DeletePart. Path: '" & strPath & "', DlgName: " & strDlgName & ", Cut: " & bCut)

            ExecuteMethod("SelectWorkspaceItem", New Object() {strPath, False})

            DeleteSelectedParts(strDlgName, bCut)
        End Sub

        Protected Overridable Sub DeleteSelectedParts(ByVal strDlgName As String, Optional ByVal bCut As Boolean = False)
            Debug.WriteLine("DeleteSelectedParts. DlgName: " & strDlgName & ", Cut: " & bCut)

            If bCut Then
                ExecuteMethod("ClickMenuItem", New Object() {"CutToolStripMenuItem"})
            Else
                ExecuteMethod("ClickToolbarItem", New Object() {"DeleteToolStripButton"})
            End If

            OpenDialogAndWait(strDlgName, Nothing, Nothing)
            ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, 1000)
        End Sub

        '''<summary>
        '''ZoomInOnRootPart
        '''</summary>
        Public Sub ZoomInOnPart(ByVal ptStart As Point, ByVal iAmount1 As Integer, Optional ByVal iAmount2 As Integer = 0, _
                                Optional ByVal bVertical As Boolean = True, Optional ByVal eButton As System.Windows.Forms.MouseButtons = MouseButtons.Right, _
                                Optional ByVal eKeys As System.Windows.Input.ModifierKeys = ModifierKeys.None)
            Dim uIStructure_1BodyClient As WinClient = Me.UIProjectWindow(m_strProjectName).UIStructure_1BodyWindow.UIStructure_1BodyClient(m_strStruct1Name)

            If Math.Abs(iAmount1) > 0 Then
                'Move using Right button 'Structure_1 Body' client
                Mouse.StartDragging(uIStructure_1BodyClient, ptStart, eButton, eKeys)
                If bVertical Then
                    Mouse.StopDragging(uIStructure_1BodyClient, 0, iAmount1)
                Else
                    Mouse.StopDragging(uIStructure_1BodyClient, iAmount1, 0)
                End If
            End If

            'Move using Right button 'Structure_1 Body' client 
            If Math.Abs(iAmount2) > 0 Then
                Mouse.StartDragging(uIStructure_1BodyClient, ptStart, eButton, eKeys)
                If bVertical Then
                    Mouse.StopDragging(uIStructure_1BodyClient, 0, iAmount2)
                Else
                    Mouse.StopDragging(uIStructure_1BodyClient, iAmount2, 0)
                End If
            End If

            If Math.Abs(iAmount1) > 0 OrElse Math.Abs(iAmount2) > 0 Then
                Mouse.Click(uIStructure_1BodyClient, eButton, ModifierKeys.None, ptStart)
            End If

        End Sub

        Public Sub DragMouse(ByVal ptStart As Point, ByVal ptEnd As Point, ByVal mButton As MouseButtons, _
                             Optional ByVal mModifiers As ModifierKeys = ModifierKeys.None, _
                             Optional ByVal bEndClick As Boolean = False)
            Debug.WriteLine("Draggin Mouse. ptStart: " & ptStart.ToString & ", ptEnd: " & ptEnd.ToString & ", mButton: " & mButton.ToString & _
                            "mModifiers: " & mModifiers.ToString & ", bEndClick: " & bEndClick)

            Dim uIStructure_1BodyClient As WinClient = Me.UIProjectWindow(m_strProjectName).UIStructure_1BodyWindow.UIStructure_1BodyClient(m_strStruct1Name)

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
            Debug.WriteLine("CheckForErrorDialog. EndError: " & strEndError)

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


        '''<summary>
        '''OpenProject - Use 'OpenProjectParams' to pass parameters into this method.
        '''</summary>
        Public Sub OpenProject()
            Dim uIOpenButton As WinButton = Me.UIMap.UIToolStripContainer1Window.UIAnimatToolStripWindow.UIOpenButton
            Dim uIAddressCProjectsAnimToolBar As WinToolBar = Me.UIMap.UIOpenanAnimatLabProjeWindow.UIAddressCProjectsAnimWindow.UIAddressCProjectsAnimToolBar
            Dim uIAddressComboBox As WinComboBox = Me.UIMap.UIOpenanAnimatLabProjeWindow.UIItemWindow.UIAddressComboBox
            Dim uIGotoCProjectsAnimatLButton As WinButton = Me.UIMap.UIOpenanAnimatLabProjeWindow.UIItemWindow2.UIItemToolBar.UIGotoCProjectsAnimatLButton
            Dim uIFilenameComboBox As WinComboBox = Me.UIMap.UIOpenanAnimatLabProjeWindow.UIItemWindow3.UIFilenameComboBox
            Dim uIOpenSplitButton As WinSplitButton = Me.UIMap.UIOpenanAnimatLabProjeWindow.UIOpenWindow.UIOpenSplitButton

            'Click 'Open' button
            Mouse.Click(uIOpenButton, New Point(13, 12))

            'Click 'Address: C:\Projects\AnimatLabSDK\Experiments\Conv...' tool bar
            Mouse.Click(uIAddressCProjectsAnimToolBar, New Point(285, 9))

            'Select 'C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\JointTests\HingeTest' in 'Address' combo box
            uIAddressComboBox.EditableItem = Me.UIMap.OpenProjectParams.UIAddressComboBoxEditableItem

            'Click 'Go to "C:\Projects\AnimatLabSDK\AnimatLabPublicSou...' button
            Mouse.Click(uIGotoCProjectsAnimatLButton, New Point(13, 10))

            'Select 'HingeTest.aproj' in 'File name:' combo box
            uIFilenameComboBox.EditableItem = Me.UIMap.OpenProjectParams.UIFilenameComboBoxEditableItem

            'Click '&Open' split button
            Mouse.Click(uIOpenSplitButton, New Point(53, 12))
        End Sub

#Region "Neural Methods"

        Protected Overridable Sub OpenRootBehavioralSubsystem()
            'Open the Structure_1 body plan editor window
            ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem}, 2000)
        End Sub

        Protected Overridable Sub AddBehavioralNode(ByVal strSubsystem As String, ByVal strClassName As String, ByVal ptPosition As Point, ByVal strName As String)

            ExecuteMethod("AddBehavioralNode", New Object() {strSubsystem, strClassName, ptPosition, strName}, 2000)

        End Sub

        Protected Overridable Sub AddBehavioralLink(ByVal strOrigin As String, ByVal strDestination As String, ByVal strName As String, _
                                                    ByVal strSynapseType As String, ByVal bInTree As Boolean)
            Debug.WriteLine("AddBehavioralLink")

            ExecuteMethod("AddBehavioralLink", New Object() {strOrigin, strDestination, strName}, 2000)

            If strSynapseType.Length > 0 Then
                If bInTree Then
                    ExecuteActiveDialogMethod("SelectItemInTreeView", New Object() {strSynapseType}, 2000)
                Else
                    ExecuteActiveDialogMethod("SelectItemInListView", New Object() {strSynapseType}, 2000)
                End If

                ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, 1000)
            End If

        End Sub
#End Region

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
        Public ReadOnly Property UIStructure_1BodyClient(ByVal strStructName As String) As WinClient
            Get
                If (Me.mUIStructure_1BodyClient Is Nothing) Then
                    Me.mUIStructure_1BodyClient = New WinClient(Me)
                    Me.mUIStructure_1BodyClient.SearchProperties(WinControl.PropertyNames.Name) = strStructName & " Body"
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

End Namespace
