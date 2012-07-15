Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Reflection
Imports System.Runtime.Remoting
Imports System.Runtime.InteropServices

Module AnimatLab

    Private Declare Function GetConsoleWindow Lib "kernel32.dll" () As IntPtr
    Private Declare Function ShowWindow Lib "user32.dll" (ByVal hwnd As IntPtr, ByVal nCmdShow As Int32) As Int32
    Private Const SW_SHOWMINNOACTIVE As Int32 = 7
    Private Const SW_SHOWNORMAL As Int32 = 1
    Private Const SW_HIDE As Int32 = 0

    Private m_strCmdLineProject As String = ""
    Private m_iCmdLinePort As Integer = -1
    Private m_oApplication As Object
    Private m_oServer As AnimatServer.Server

    Sub Main()
        Try
            ShowWindow(GetConsoleWindow(), SW_HIDE)

            ProcessArguments()

            Dim strAssemblyPath As String = "", strClassName As String = ""
            LoadConfigInfo(strAssemblyPath, strClassName)

            Dim oAssembly As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(strAssemblyPath)
            m_oApplication = oAssembly.CreateInstance(strClassName)
            Dim oStartApp As MethodInfo = m_oApplication.GetType().GetMethod("StartApplication")

            If m_iCmdLinePort > 0 Then
                m_oServer = New AnimatServer.Server()
                m_oServer.Initialize(m_oApplication, m_iCmdLinePort)
            End If

            'Start the app and block.
            oStartApp.Invoke(m_oApplication, New Object() {True})

        Catch ex As Exception
            If Not ex.InnerException Is Nothing Then
                MessageBox.Show(ex.InnerException.Message & vbCrLf & _
                                "Source: " & ex.InnerException.Source & vbCrLf & _
                                "Trace: " & ex.InnerException.StackTrace, _
                                "Error in AnimatLab", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show(ex.Message & vbCrLf & _
                                "Source: " & ex.Source & vbCrLf & _
                                "Trace: " & ex.StackTrace, _
                                "Error in AnimatLab", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try

    End Sub

    ''' \fn Private Shared Sub LoadConfigInfo(ByRef strAssemblyPath As String, ByRef strClassName As String)
    '''
    ''' \brief  Loads the AnimatLab.config xml configuration information. 
    '''
    ''' \author dcofer
    ''' \date   3/1/2011
    '''
    ''' \exception  ArgumentException   Thrown when one or more arguments have unsupported or illegal
    '''                                 values. 
    '''
    ''' \param [in,out] strAssemblyPath Full pathname of the string assembly file. 
    ''' \param [in,out] strClassName    Name of the string class. 
    Private Sub LoadConfigInfo(ByRef strAssemblyPath As String, ByRef strClassName As String)
        Dim strExePath As String = "", strFile As String = ""
        SplitPathAndFile(Application.ExecutablePath, strExePath, strFile)

        Dim strAssemblyName As String = System.Configuration.ConfigurationManager.AppSettings("AssemblyName")
        strClassName = System.Configuration.ConfigurationManager.AppSettings("ClassName")

        If IsFullPath(strAssemblyName) Then
            strAssemblyPath = strAssemblyName
        Else
            strAssemblyPath = strExePath & strAssemblyName
        End If

    End Sub

    ''' \fn Private Shared Sub SplitPathAndFile(ByVal strFullPath As String, ByRef strPath As String, ByRef strFile As String)
    '''
    ''' \brief  Splits a path and filename. 
    '''
    ''' \author dcofer
    ''' \date   3/1/2011
    '''
    ''' \param  strFullPath     Full pathname of the string full file. 
    ''' \param [in,out] strPath Full pathname of the string file. 
    ''' \param [in,out] strFile The string file. 

    Private Sub SplitPathAndFile(ByVal strFullPath As String, _
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
        If strPath.Trim.Length > 0 Then strPath += "\"
    End Sub

    ''' \fn Private Shared Function IsFullPath(ByVal strPath As String) As Boolean
    '''
    ''' \brief  Query if 'strPath' is full path. 
    '''
    ''' \author dcofer
    ''' \date   3/1/2011
    '''
    ''' \param  strPath Full pathname of the string file. 
    '''
    ''' \return true if full path, false if not. 
    Private Function IsFullPath(ByVal strPath As String) As Boolean
        Dim aryParts() As String = Split(strPath, "\")

        Dim iCount As Integer = aryParts.GetUpperBound(0)
        If iCount > 1 Then
            Return True
        Else
            Return False
        End If

    End Function

    ''' \brief  Removes the extension of a filename. 
    '''
    ''' \author dcofer
    ''' \date   3/9/2011
    '''
    ''' \param  strFile Filename with extension. 
    '''
    ''' \return Filename without extension. 
    Private Function RemoveExtension(ByVal strFile As String) As String
        Dim aryParts() As String = Split(strFile, ".")

        Dim iCount As Integer = aryParts.GetUpperBound(0)
        If iCount <= 0 Then
            Return "AnimatLab"
        Else
            Return aryParts(0)
        End If

    End Function

    Private Sub ProcessArguments()
        Dim args() As String = System.Environment.GetCommandLineArgs()

        Dim iCount As Integer = args.Length
        For iIdx As Integer = 0 To iCount - 1
            If args(iIdx).Trim.ToUpper = "-PROJECT" AndAlso iIdx < (iCount - 1) Then
                m_strCmdLineProject = args(iIdx + 1)
            End If

            If args(iIdx).Trim.ToUpper = "-PORT" AndAlso iIdx < (iCount - 1) Then
                Dim strPort As String = args(iIdx + 1)
                If IsNumeric(strPort) Then
                    Dim iPort As Integer = CInt(strPort)
                    If iPort > 0 Then
                        m_iCmdLinePort = iPort
                    End If
                End If
            End If
        Next


    End Sub
End Module
