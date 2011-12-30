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

Namespace UITests
    Namespace ConversionTests

        <CodedUITest()>
        Public MustInherit Class ConversionUITest
            Inherits AnimatUITest

#Region "Attributes"

            Protected m_strOldProjectFolder As String

            Protected m_aryWindowsToOpen As New ArrayList

#End Region

#Region "Properties"

#End Region

#Region "Methods"

            Protected Overridable Sub StartConversionProject()

                StartExistingProject()

                'Converted project should always ask to be converted.
                OpenDialogAndWait("Convert Project", Nothing, Nothing)

                'Click 'Ok' button
                ExecuteActiveDialogMethod("ClickOkButton", Nothing)

                Threading.Thread.Sleep(3000)

                OpenDialogAndWait("Project Conversion", Nothing, Nothing)

                'Click 'Ok' button
                ExecuteActiveDialogMethod("ClickOkButton", Nothing)

                Threading.Thread.Sleep(3000)

                'Open any data/sim windows that are needed.
                For Each strWindowPath As String In m_aryWindowsToOpen
                    'Open the Structure_1 body plan editor window
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {strWindowPath}, 2000)
                Next

                Threading.Thread.Sleep(3000)

            End Sub

            Protected Overrides Sub CleanupProjectDirectory()
                'Make sure any left over project directory is cleaned up before starting the test.
                If m_strRootFolder.Length > 0 AndAlso m_strProjectPath.Length > 0 AndAlso m_strProjectName.Length > 0 Then
                    DeleteDirectory(m_strRootFolder & m_strProjectPath & "\" & m_strProjectName)
                End If

                'Copy the old version project folder back so we can load it up.
                If m_strOldProjectFolder.Length > 0 Then
                    Util.CopyDirectory(m_strRootFolder & m_strOldProjectFolder, m_strRootFolder & m_strProjectPath & "\" & m_strProjectName)
                End If
            End Sub

#End Region

        End Class

    End Namespace
End Namespace


