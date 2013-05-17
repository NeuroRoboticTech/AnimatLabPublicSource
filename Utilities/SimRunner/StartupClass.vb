Imports System.IO

Public Class StartupClass

    Public Shared Sub Main()
        Dim bRunImmediate As Boolean = False
        Dim strSimFiles As String = ""
        Dim strResultFiles As String = ""
        Dim strCommonFiles As String = ""
        Dim strSourceFiles As String = ""

        Try
            Dim args() As String = System.Environment.GetCommandLineArgs()

            Dim iCount As Integer = args.Length
            For iIdx As Integer = 0 To iCount - 1
                If args(iIdx).Trim.ToUpper = "-SIMFILES" AndAlso iIdx < (iCount - 1) Then
                    strSimFiles = args(iIdx + 1)
                End If

                If args(iIdx).Trim.ToUpper = "-RESULTFILES" AndAlso iIdx < (iCount - 1) Then
                    strResultFiles = args(iIdx + 1)
                End If

                If args(iIdx).Trim.ToUpper = "-COMMONFILES" AndAlso iIdx < (iCount - 1) Then
                    strCommonFiles = args(iIdx + 1)
                End If

                If args(iIdx).Trim.ToUpper = "-SOURCEFILES" AndAlso iIdx < (iCount - 1) Then
                    strSourceFiles = args(iIdx + 1)
                End If

                If args(iIdx).Trim.ToUpper = "-RUN" Then
                    bRunImmediate = True
                End If
            Next

            If bRunImmediate Then
                Console.WriteLine("Starting Sim Runner")
                ProcessFiles(strSimFiles, strResultFiles, strCommonFiles, strSourceFiles, bRunImmediate, Nothing, Nothing)
            Else
                Dim frmRunner As New Form1
                frmRunner.m_bRunImmediate = bRunImmediate
                frmRunner.m_strCommonFiles = strCommonFiles
                frmRunner.m_strResultFiles = strResultFiles
                frmRunner.m_strSimFiles = strSimFiles
                frmRunner.m_strSourceFiles = strSourceFiles
                frmRunner.ShowDialog()
            End If

        Catch ex As Exception
            If bRunImmediate Then
                Console.WriteLine(ex.Message)
            Else
                MessageBox.Show(ex.Message)
            End If
        Finally
            Console.WriteLine("Finished SimRunner")
        End Try
    End Sub

    Public Shared Sub ProcessFiles(ByVal strSimFiles As String, ByVal strResultFiles As String, _
                                   ByVal strCommonFiles As String, ByVal strSourceFiles As String, _
                                   ByVal bRunImmediate As Boolean, ByVal barProgress As System.Windows.Forms.ProgressBar, _
                                   ByVal lblStatus As System.Windows.Forms.Label)
        Try
            'Dim strText As String = strSourceFiles & "\Animatsimulator -d3d -runtime 2.2 -library vortexanimatlibrary_vc7.dll -project """ & strCommonFiles & "\BetaPitch_012209_30_6.asim"""
            Dim strFile As String
            Dim strOutFile As String
            Dim strProg As String = strSourceFiles & "\Animatsimulator"
            Dim strArg As String
            Dim procSim As System.Diagnostics.Process

            Dim aryFiles As String() = Directory.GetFiles(strSimFiles)

            Dim fltPerc As Single = 1 / aryFiles.Length
            Dim iCount As Integer = 0
            If Not barProgress Is Nothing Then barProgress.Value = 0

            For Each strFFile As String In aryFiles
                strFile = GetFilename(strFFile)
                File.Copy(strSimFiles & "\" & strFile, strCommonFiles & "\" & strFile)
                strArg = "-library VortexAnimatPrivateSim_vc10.dll -project """ & strCommonFiles & "\" & strFile & """"

                Dim strStatus As String = "Processing " & (iCount + 1) & " of " & aryFiles.Length & "   File: " & strFile
                If Not lblStatus Is Nothing Then lblStatus.Text = strStatus
                Console.WriteLine(strStatus)
                Application.DoEvents()
                procSim = System.Diagnostics.Process.Start(strProg, strArg)
                procSim.WaitForExit()

                Dim aryOutFiles As String() = Directory.GetFiles(strCommonFiles, "*.txt")

                For Each strFOutFile As String In aryOutFiles
                    strOutFile = GetFilename(strFOutFile)
                    File.Copy(strCommonFiles & "\" & strOutFile, strResultFiles & "\" & GetBaseFilename(strFile) & "_" & GetBaseFilename(strOutFile) & ".txt")
                    File.Delete(strCommonFiles & "\" & strOutFile)
                Next

                File.Delete(strCommonFiles & "\" & strFile)

                iCount = iCount + 1
                If Not barProgress Is Nothing Then barProgress.Value = (iCount * fltPerc)
                Application.DoEvents()
            Next

            If bRunImmediate Then
                Console.WriteLine("Finished")
            Else
                MessageBox.Show("Finished")
            End If

        Catch ex As System.Exception
            If bRunImmediate Then
                Console.WriteLine("Finished")
            Else
                MessageBox.Show(ex.Message)
            End If
        End Try
    End Sub


    Public Shared Function GetFilename(ByVal strFile As String) As String
        Dim aryParts As String() = Split(strFile, "\")
        Return aryParts(aryParts.Length - 1)
    End Function

    Public Shared Function GetBaseFilename(ByVal strFile As String) As String
        Dim aryParts As String() = Split(strFile, ".")
        Return aryParts(0)
    End Function

End Class
