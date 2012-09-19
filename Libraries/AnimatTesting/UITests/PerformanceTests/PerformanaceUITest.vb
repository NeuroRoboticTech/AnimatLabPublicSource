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
Imports AnimatTesting.Framework
Imports System.Xml

Namespace UITests
    Namespace PerformanceTests

        <CodedUITest()>
        Public MustInherit Class PerformanceUITest
            Inherits AnimatUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"

            Protected Overridable Sub TestConversionProject(ByVal strDataPrefix As String, Optional iMaxRows As Integer = -1, Optional ByVal dblMaxError As Double = 0.05)

                Dim aryMaxErrors As New Hashtable
                aryMaxErrors.Add("default", dblMaxError)

                TestConversionProject(strDataPrefix, aryMaxErrors, iMaxRows)
            End Sub

            Protected Overridable Sub TestConversionProject(ByVal strDataPrefix As String, ByVal aryMaxErrors As Hashtable, Optional iMaxRows As Integer = -1, Optional ByVal aryIgnoreRows As ArrayList = Nothing)
                Debug.WriteLine("TestConversionProject. DataPrefix: " & strDataPrefix & ", MaxErrors: " & Util.ParamsToString(aryMaxErrors) & ", MaxRows: " & iMaxRows)


                ConvertProject()

                Threading.Thread.Sleep(3000)

                If strDataPrefix.Length > 0 Then
                    'Run the simulation and wait for it to end.
                    RunSimulationWaitToEnd()

                    'Compare chart data to verify simulation results.
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, strDataPrefix, iMaxRows, aryIgnoreRows)
                End If

            End Sub

            Protected Overridable Sub ConvertProject()
                Debug.WriteLine("ConvertProject")

                CleanupConversionProjectDirectory()

                StartExistingProject()

                'Converted project should always ask to be converted.
                OpenDialogAndWait("Convert Project", Nothing, Nothing)

                'Click 'Ok' button
                ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, , , True)

                Threading.Thread.Sleep(3000)

                OpenDialogAndWait("Project Conversion", Nothing, Nothing)

                'Click 'Ok' button
                ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing, , , True)

                WaitForProjectToOpen()

                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation", "PlaybackControlMode", "FastestPossible"})
            End Sub


#End Region

        End Class

    End Namespace
End Namespace


