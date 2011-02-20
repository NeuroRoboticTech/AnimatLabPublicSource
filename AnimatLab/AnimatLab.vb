Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Reflection


Namespace Forms

    'Okay. Big Explanation
    'We can NOT, ABSOLUTELY NOT, add a static reference to anitmat tools here. If we do then we can no longer bind any of our
    'assemblies dynamically and cast the forms to AnimatForm. The reason can be found on this web site: 
    'http://www.gotdotnet.com/team/clr/LoadFromIsolation.aspx
    'Roughly, .net keeps two different versions of the assemblies. The first is the ones that are statically 
    'linked and the second are the dynamically linked ones. There is no way to cast objects created using CreateInstance
    'on a dynamically loaded dll into the object types of the statically linked dll. It is impossible by design. 
    'So the only way I could come up with to get around this f***ed up situation is to make all references to 
    'AnimatTools dynamically loaded in the dll. All dynamically loaded instances share the same assembly so they 
    'can all be cast to each other with no problem. 
    'Thanks alot microsoft. This should have been easy, but I just spent hours trying to figure this one out.
    '@#$##@@#$$%$##@

    Public Class AnimatLab
        Public Shared Sub Main()
            Try
                Dim strAssemblyPath As String, strClassName As String
                LoadConfigInfo(strAssemblyPath, strClassName)

                Dim s() As String = System.Environment.GetCommandLineArgs()

                Dim oAssembly As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(strAssemblyPath)
                Dim oApplication As Object = oAssembly.CreateInstance(strClassName)

                If s.Length = 2 Then
                    Dim oLoadInfo As MethodInfo = oApplication.GetType().GetMethod("CommandLineParams")
                    oLoadInfo.Invoke(oApplication, s)
                End If

                Dim oShow As MethodInfo = oApplication.GetType().GetMethod("ShowDialog", New Type() {})
                oShow.Invoke(oApplication, Nothing)

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

        Private Shared Sub LoadConfigInfo(ByRef strAssemblyPath As String, ByRef strClassName As String)
            Dim strExePath As String, strFile As String
            SplitPathAndFile(Application.ExecutablePath, strExePath, strFile)

            'Open existing file
            Dim strConfigFile As String = strExePath & "Animatlab.config"
            Dim fs As System.IO.FileStream = New System.IO.FileStream(strConfigFile, System.IO.FileMode.Open, IO.FileAccess.Read)
            Dim XmlIn As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(fs)

            XmlIn.WhitespaceHandling = System.Xml.WhitespaceHandling.None

            'Moves the reader to the root element.
            XmlIn.MoveToContent()

            ' Double check this has the correct element name
            If XmlIn.Name <> "AnimatLabConfig" Then Throw New ArgumentException("Root element must be 'AnimatLabConfig'")

            If Not XmlIn.Read() Then Throw New ArgumentException("An element was expected but could not be read in")
            If XmlIn.Name <> "AssemblyName" Then Throw New ArgumentException("Child element must be 'AssemblyName'")
            Dim strAssemblyName As String = XmlIn.ReadString

            If IsFullPath(strAssemblyName) Then
                strAssemblyPath = strAssemblyName
            Else
                strAssemblyPath = strExePath & strAssemblyName
            End If

            If Not XmlIn.Read() Then Throw New ArgumentException("An element was expected but could not be read in")
            If XmlIn.Name <> "ClassName" Then Throw New ArgumentException("Child element must be 'ClassName'")
            strClassName = XmlIn.ReadString

            fs.Close()
        End Sub

        Private Shared Sub SplitPathAndFile(ByVal strFullPath As String, _
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

        Private Shared Function IsFullPath(ByVal strPath As String) As Boolean
            Dim aryParts() As String = Split(strPath, "\")

            Dim iCount As Integer = aryParts.GetUpperBound(0)
            If iCount > 1 Then
                Return True
            Else
                Return False
            End If

        End Function

    End Class

End Namespace
