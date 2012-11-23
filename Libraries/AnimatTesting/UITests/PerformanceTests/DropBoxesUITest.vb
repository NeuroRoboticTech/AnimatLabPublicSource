Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports AnimatTesting.Framework

Namespace UITests
    Namespace PerformanceTests

        <CodedUITest()>
        Public Class DropBoxesUITest
            Inherits PerformanceUITest

            Protected m_aryStepTimes As New ArrayList
            Protected m_fltSize As Single = 0.2
            Protected m_fltInterBoxDist As Single = m_fltSize * 3

            '<TestMethod()>
            Public Sub Test_DropBoxes()

                'Start the application.
                CleanupConversionProjectDirectory()

                StartExistingProject()

                WaitForProjectToOpen()

                'Open the Structure_1 body plan editor window
                ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan"})

                Dim iMaxBoxes As Integer = 30
                Dim iBoxes As Integer = 0
                For fltX As Single = -10 To 10 Step m_fltInterBoxDist
                    For fltZ As Single = -10 To 10 Step m_fltInterBoxDist
                        If iBoxes < iMaxBoxes Then

                            If iBoxes > 0 Then
                                AddChildPartTypeWithJoint("Box", "FreeJoint", "Simulation\Environment\Organisms\Organism_1\Body Plan\Root", False)
                                RepositionChildParts(iBoxes, fltX, fltZ)
                            Else
                                AddRootPartType(m_strStructureGroup, m_strStruct1Name, "Box", , False)
                                RepositionRootPart(fltX, fltZ)
                            End If

                            iBoxes = iBoxes + 1
                        End If
                    Next
                Next

                GetStepTime()

                Debug.WriteLine("Average step times")
                For Each dblAvg As Double In m_aryStepTimes
                    Debug.WriteLine(dblAvg)
                Next

            End Sub

            Protected Sub GetStepTime()
                'Run the simulation and wait for it to end.
                RunSimulationWaitToEnd()

                ExecuteMethod("ExportDataCharts", New Object() {"", ""})

                Dim dblAvgTime As Double = CalculateChartColumnAverage(m_strRootFolder & m_strProjectPath & "\" & m_strProjectName & "\DataTool_1.txt", 1)

                m_aryStepTimes.Add(dblAvgTime)
            End Sub

            Protected Sub RepositionChildParts(ByVal iChildIdx As Integer, ByVal fltX As Single, ByVal fltZ As Single)

                'Resize the child part
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx & "\Body_" & iChildIdx, "Width", CStr(m_fltSize)})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx & "\Body_" & iChildIdx, "Height", CStr(m_fltSize)})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx & "\Body_" & iChildIdx, "Length", CStr(m_fltSize)})

                'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx & "\Body_" & iChildIdx & "\Body_" & iChildIdx & "_Graphics", "Width", CStr(m_fltSize)})
                'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx & "\Body_" & iChildIdx & "\Body_" & iChildIdx & "_Graphics", "Height", CStr(m_fltSize)})
                'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx & "\Body_" & iChildIdx & "\Body_" & iChildIdx & "_Graphics", "Length", CStr(m_fltSize)})

                'Reposition the child part relative to the parent part
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx & "\Body_" & iChildIdx, "WorldPosition.X", CStr(fltX)})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx & "\Body_" & iChildIdx, "WorldPosition.Y", "5"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx & "\Body_" & iChildIdx, "WorldPosition.Z", CStr(fltZ)})

                'Reposition the joint relative to the parent part
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx, "LocalPosition.X", "0"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx, "LocalPosition.Y", "0"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_" & iChildIdx, "LocalPosition.Z", "0"})

            End Sub

            Protected Sub RepositionRootPart(ByVal fltX As Single, ByVal fltZ As Single)

                'Resize the child part
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Width", CStr(m_fltSize)})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Height", CStr(m_fltSize)})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Length", CStr(m_fltSize)})

                'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Width", CStr(m_fltSize)})
                'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Height", CStr(m_fltSize)})
                'ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Length", CStr(m_fltSize)})

                'Reposition the child part relative to the parent part
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "WorldPosition.X", CStr(fltX)})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "WorldPosition.Y", "5"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "WorldPosition.Z", CStr(fltZ)})

            End Sub

#Region "Additional test attributes"
            '
            ' You can use the following additional attributes as you write your tests:
            '
            ' Use TestInitialize to run code before running each test
            <TestInitialize()> Public Overrides Sub MyTestInitialize()
                MyBase.MyTestInitialize()

                m_strProjectName = "DropBoxesTest"
                m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\PerformanceTests"
                m_strTestDataPath = "\Libraries\AnimatTesting\TestProjects\PerformanceTests\" & m_strProjectName
                m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\PerformanceTests\" & m_strProjectName

                'Make sure any left over project directory is cleaned up before starting the test.
                DeleteDirectory(m_strRootFolder & m_strProjectPath & "\" & m_strProjectName)

                SetStructureNames("1", False)
            End Sub

            ' Use TestCleanup to run code after each test has run
            <TestCleanup()> Public Overrides Sub MyTestCleanup()
                MyBase.MyTestCleanup()

            End Sub

#End Region

        End Class

    End Namespace
End Namespace
