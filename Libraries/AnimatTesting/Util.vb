Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Util

    Protected Shared m_iLastPort As Integer = 8079

    Public Shared ReadOnly Property LastPort() As Integer
        Get
            Return m_iLastPort
        End Get
    End Property

    Public Shared Function GetNewPort() As Integer
        m_iLastPort = m_iLastPort + 1
        Return m_iLastPort
    End Function


    Public Shared Sub ReadCSVFileToArray(ByVal strFilename As String, ByRef aryColumns() As String, ByRef aryData(,) As Double)
        Dim num_rows As Integer
        Dim num_cols As Integer
        Dim iCol As Integer
        Dim iRow As Integer

        ' Load the file.
        'Check if file exist
        If File.Exists(strfilename) Then
            Dim tmpstream As StreamReader = File.OpenText(strFilename)
            Dim aryLines() As String
            Dim aryLine() As String

            'Load content of file to strLines array
            Dim strData As String = tmpstream.ReadToEnd()
            aryLines = strData.Split(vbLf.ToCharArray())

            If (aryLines.Length < 2) Then
                Throw New System.Exception("No data in file: " & strFilename)
            End If

            aryColumns = aryLines(0).Split(vbTab.ToCharArray)
            ReDim Preserve aryColumns(aryColumns.Length - 2)

            'Remove one for the header and one to make the index work.
            num_rows = aryLines.Length - 1 - 1
            num_cols = aryColumns.Length - 1

            ReDim aryData(num_cols, num_rows)

            ' Copy the data into the array. Skip the header row.
            For iRow = 1 To num_rows
                aryLine = aryLines(iRow).Split(vbTab.ToCharArray)

                For iCol = 0 To num_cols
                    aryData(iCol, iRow) = CDbl(aryLine(iCol))
                Next
            Next

        End If

    End Sub

    Public Shared Function IsBlank(ByVal strVal As String) As Boolean
        If strVal.Trim.Length > 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Shared Sub SplitPathAndFile(ByVal strFullPath As String, _
                                       ByRef strPath As String, _
                                       ByRef strFile As String)
        Dim aryParts() As String = Split(strFullPath, "\")

        Dim iCount As Integer = aryParts.GetUpperBound(0)
        If iCount <= 0 Then
            strPath = ""
            strFile = strFullPath
        Else
            strFile = aryParts(iCount)
            ReDim Preserve aryParts(iCount - 1)
        End If

        strPath = Join(aryParts, "\")
        If Not IsBlank(strPath) Then strPath += "\"
    End Sub

    Public Shared Function ExtractFilename(ByVal strFullPath As String) As String
        Dim strPath As String = "", strFile As String = ""
        SplitPathAndFile(strFullPath, strPath, strFile)
        Return strFile
    End Function

    Public Shared Sub CopyDirectory(ByVal strOrigFolder As String, ByVal strNewFolder As String)

        If Not Directory.Exists(strNewFolder) Then
            Directory.CreateDirectory(strNewFolder)

            Dim aryFiles As String() = Directory.GetFiles(strOrigFolder)

            Dim strFilePath As String
            Dim strFile As String
            For Each strFilePath In aryFiles
                strFile = Util.ExtractFilename(strFilePath)
                File.Copy((strOrigFolder & "\" & strFile), (strNewFolder & "\" & strFile))
            Next

            'Now copy any directories that exist in that folder also.
            Dim aryDirs As String() = Directory.GetDirectories(strOrigFolder)
            For Each strDir As String In aryDirs
                Dim aryDirNames As String() = Split(strDir, "\")
                Dim strActualDir As String = aryDirNames(UBound(aryDirNames))
                If strActualDir.ToUpper.Trim <> ".SVN" Then
                    CopyDirectory(strDir, strNewFolder & "\" & strActualDir)
                End If
            Next
        Else
            Throw New System.Exception("The folder '" & strOrigFolder & "' can not be copied to a new directory '" & _
                                       strNewFolder & "' becuase a directory with that name already exists.")
        End If

    End Sub

End Class
