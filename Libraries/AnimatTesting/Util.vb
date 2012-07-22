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

    Protected Shared m_dblRadiansToDegreeRatio As Double = (180 / Math.PI)
    Protected Shared m_dblDegreeToRadiansRatio As Double = (Math.PI / 180)

    Public Shared ReadOnly Property LastPort() As Integer
        Get
            Return m_iLastPort
        End Get
    End Property

    Public Shared Function GetNewPort() As Integer
        m_iLastPort = m_iLastPort + 1
        Return m_iLastPort
    End Function

    Public Shared ReadOnly Property RadiansToDegreesRatio() As Double
        Get
            Return m_dblRadiansToDegreeRatio
        End Get
    End Property

    Public Shared ReadOnly Property DegreesToRadiansRatio() As Double
        Get
            Return m_dblDegreeToRadiansRatio
        End Get
    End Property

    Public Shared Function DegreesToRadians(ByVal fltDegrees As Single) As Single

        If fltDegrees > 360 OrElse fltDegrees < -360 Then
            Dim iMod As Integer = CInt(Math.Abs(fltDegrees / 360))
            fltDegrees = fltDegrees / iMod
        End If

        If fltDegrees = -360 OrElse fltDegrees = 360 Then fltDegrees = 0

        Return CSng((fltDegrees / 180.0) * Math.PI)
    End Function

    Public Shared Function RadiansToDegrees(ByVal fltRadians As Single) As Single
        Return CSng((fltRadians / Math.PI) * 180.0)
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


    Public Shared Sub ReadCSVFileToList(ByVal strFilename As String, ByRef aryColumns() As String, ByRef aryData As List(Of List(Of Double)), ByVal bIncludesHeader As Boolean)
        Dim num_rows As Integer
        Dim num_cols As Integer
        Dim iCol As Integer
        Dim iRow As Integer

        ' Load the file.
        'Check if file exist
        If File.Exists(strFilename) Then
            Dim tmpstream As StreamReader = File.OpenText(strFilename)
            Dim aryLines() As String
            Dim aryLine() As String

            'Load content of file to strLines array
            Dim strData As String = tmpstream.ReadToEnd()
            aryLines = strData.Split(vbLf.ToCharArray())
            tmpstream.Close()

            If (aryLines.Length < 2) Then
                Throw New System.Exception("No data in file: " & strFilename)
            End If

            aryColumns = aryLines(0).Split(vbTab.ToCharArray)

            'Remove one for the header and one to make the index work.
            Dim iHeaderRow As Integer = 0
            If bIncludesHeader Then iHeaderRow = 1
            num_rows = aryLines.Length - 1
            num_cols = aryColumns.Length - 1

            For iIdx = 0 To num_cols
                aryData.Add(New List(Of Double))
            Next

            ' Copy the data into the array. Skip the header row.
            Dim iStartIdx As Integer
            If bIncludesHeader Then iStartIdx = 1 Else iStartIdx = 0
            For iRow = iStartIdx To num_rows
                If aryLines(iRow).Trim.Length > 0 Then
                    aryLine = aryLines(iRow).Split(vbTab.ToCharArray)

                    For iCol = 0 To num_cols
                        If aryLine(iCol).Trim.Length > 0 AndAlso IsNumeric(aryLine(iCol)) Then
                            aryData(iCol).Add(CDbl(aryLine(iCol)))
                        Else
                            Dim iVal As Integer = 4
                        End If
                    Next
                End If
            Next
        Else
            Throw New System.Exception("File '" & strFilename & "' was not found.")
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
        Debug.WriteLine("Attempting to copy directory. Orig: " & strOrigFolder & ", New: " & strNewFolder)

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

    Public Shared Function ParamsToString(ByVal aryParams() As Object) As String

        Dim strParams As String = ""
        If Not aryParams Is Nothing Then
            For Each oParam In aryParams
                If Not oParam Is Nothing Then
                    strParams = strParams & oParam.ToString & ", "
                Else
                    strParams = strParams & "Nothing, "
                End If
            Next
            If strParams.Length >= 2 Then
                strParams = strParams.Substring(0, strParams.Length - 2)
            End If
        End If

        Return strParams
    End Function

    Public Shared Function ParamsToString(ByVal aryParams As Hashtable) As String

        Dim strParams As String = ""
        If Not aryParams Is Nothing Then
            For Each oParam In aryParams
                If Not oParam Is Nothing Then
                    strParams = strParams & oParam.ToString & ", "
                Else
                    strParams = strParams & "Nothing, "
                End If
            Next
            If strParams.Length >= 2 Then
                strParams = strParams.Substring(0, strParams.Length - 2)
            End If
        End If

        Return strParams
    End Function

    Public Shared Function FindColumnNamed(ByVal aryChartColumns() As String, ByVal strName As String) As Integer

        For iIdx = 0 To aryChartColumns.Length - 1
            If aryChartColumns(iIdx) = strName Then
                Return iIdx
            End If
        Next

        Throw New System.Exception("No column named '" & strName & "' was found.")
    End Function

End Class
