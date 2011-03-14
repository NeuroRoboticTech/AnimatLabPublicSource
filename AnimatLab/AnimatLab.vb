Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Reflection

''' \namespace AnimatLab::Forms 
'''
''' \brief  Contains classes related to showing the AnimatLab main form. 
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

    ''' \class  AnimatLab
    '''
    ''' \brief  Main class for showing AnimatLab form.
    ''' 		
    ''' \details The AnimatLab executable is essentially a small shell program that contains
    ''' very little code. All of the main code is located in the DLL and assembly modules. 
    ''' All this exe shell does is load up the AnimatLab.exe.config xml file to get the name of the
    ''' assembly it is to load and the name of the class within that assembly that it must start.
    ''' It then uses reflection to create an instance of that object and invoke the StartApplication
    ''' method.
    '''
    ''' \author dcofer
    ''' \date   3/1/2011
    Public Class AnimatLab

        ''' \fn Public Shared Sub Main()
        '''
        ''' \brief  Main entry-point for the AnimatLab application.
        ''' 
        ''' \details This function is the entry point. It loads up the config file, creates
        ''' the AnimatLab object and invokes the necessary methods to start the application. 
        '''
        ''' \author dcofer
        ''' \date   3/1/2011
        Public Shared Sub Main()
            Try
                Dim strAssemblyPath As String, strClassName As String
                LoadConfigInfo(strAssemblyPath, strClassName)

                Dim oAssembly As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(strAssemblyPath)
                Dim oApplication As Object = oAssembly.CreateInstance(strClassName)

                Dim oStartApp As MethodInfo = oApplication.GetType().GetMethod("StartApplication")
                oStartApp.Invoke(oApplication, New Object() {True})

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
        Private Shared Sub LoadConfigInfo(ByRef strAssemblyPath As String, ByRef strClassName As String)
            Dim strExePath As String, strFile As String
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
        Private Shared Function IsFullPath(ByVal strPath As String) As Boolean
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
        Private Shared Function RemoveExtension(ByVal strFile As String) As String
            Dim aryParts() As String = Split(strFile, ".")

            Dim iCount As Integer = aryParts.GetUpperBound(0)
            If iCount <= 0 Then
                Return "AnimatLab"
            Else
                Return aryParts(0)
            End If

        End Function
    End Class

End Namespace
