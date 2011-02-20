Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO

Module AnimatLabScripter

    Public Class Param
        Public m_strID As String
        Public m_strPropName As String
        Public m_strNewPropVal As String
        Public m_strOldPropVal As String
        Public m_doObj As AnimatTools.Framework.DataObject
        Public m_oProp As System.Reflection.PropertyInfo

        Private Sub SetPropVal(ByVal strNewVal As String)

            Dim typeCode As TypeCode = Type.GetTypeCode(m_oProp.PropertyType)

            Select Case typeCode
                Case typeCode.Boolean
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CBool(strNewVal), Nothing)

                Case typeCode.Byte
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CByte(strNewVal), Nothing)

                Case typeCode.Char
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CChar(strNewVal), Nothing)

                Case typeCode.DateTime
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CDate(strNewVal), Nothing)

                Case typeCode.Decimal
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CDec(strNewVal), Nothing)

                Case typeCode.Double
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CDbl(strNewVal), Nothing)

                Case typeCode.Int16
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CInt(strNewVal), Nothing)

                Case typeCode.Int32
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CLng(strNewVal), Nothing)

                Case typeCode.Int64
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CLng(strNewVal), Nothing)

                Case typeCode.Object
                    If m_oProp.PropertyType.Name = "ScaledNumber" Then
                        Dim oScaledNum As AnimatTools.Framework.ScaledNumber = DirectCast(m_oProp.GetValue(m_doObj, Nothing), AnimatTools.Framework.ScaledNumber)
                        m_strOldPropVal = CStr(oScaledNum.ActualValue)
                        oScaledNum.ActualValue = CDbl(strNewVal)
                    Else
                        Throw New System.Exception("Invalid object type. ID: " & m_strID & ", PropName: " & m_strPropName & ", Object Type: " & m_oProp.PropertyType.Name)
                    End If

                Case typeCode.SByte
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CByte(strNewVal), Nothing)

                Case typeCode.Single
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CSng(strNewVal), Nothing)

                Case typeCode.String
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, strNewVal, Nothing)

                Case typeCode.UInt16
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CInt(strNewVal), Nothing)

                Case typeCode.UInt32
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CInt(strNewVal), Nothing)

                Case typeCode.UInt64
                    m_strOldPropVal = CStr(m_oProp.GetValue(m_doObj, Nothing))
                    m_oProp.SetValue(m_doObj, CInt(strNewVal), Nothing)

                Case Else
            End Select


        End Sub

        Public Sub SetValue()

            m_doObj = g_oApp.Simulation.FindObjectByID(m_strID)
            If m_doObj Is Nothing Then
                Throw New System.Exception("No object with ID '" & m_strID & "' was found in the project.")
            End If

            m_oProp = m_doObj.GetType().GetProperty(m_strPropName)
            If m_oProp Is Nothing Then
                Throw New System.Exception("No property was found with the name '" & m_strPropName & "' for object ID '" & m_strID & "'")
            End If

            SetPropVal(m_strNewPropVal)

        End Sub

        Public Sub ResetValue()

            SetPropVal(m_strOldPropVal)

        End Sub


    End Class

    Dim g_strProjectPath As String = ""
    Dim g_strProjectFile As String = ""
    Dim g_strStandaloneProjectFile As String = ""
    Dim g_strAPIFile As String = ""
    Dim g_oApp As AnimatTools.Forms.AnimatApplication
    Dim g_oWatcher As FileSystemWatcher
    Dim g_bExit As Boolean = False
    Dim g_aryParams As New Collections.ArrayList

    Sub Main(ByVal args As String())

        Try
            If args.Length < 1 Then
                Throw New System.Exception("You must provide a path to a project on the command line to use the scripting tool.")
            End If
            If args.Length < 2 Then
                Throw New System.Exception("You must provide a project file name on the command line to use the scripting tool.")
            End If
            If args.Length < 3 Then
                Throw New System.Exception("You must provide an API script file name on the command line to use the scripting tool.")
            End If
            If args.Length < 4 Then
                Throw New System.Exception("You must provide a stand-alone simulation file name on the command line to use the scripting tool.")
            End If

            g_strProjectPath = args(0) & "\"
            g_strProjectFile = args(1)
            g_strAPIFile = args(2)
            g_strStandaloneProjectFile = args(3)

            Console.WriteLine("Loading Project '" & g_strProjectPath & g_strProjectFile & "'")

            g_oApp = New AnimatTools.Forms.AnimatApplication
            g_oApp.LoadProject(g_strProjectPath & g_strProjectFile)

            ' Create a new FileSystemWatcher and set its properties.
            g_oWatcher = New FileSystemWatcher
            g_oWatcher.Path = g_strProjectPath
            ' Watch for changes in LastAccess and LastWrite times, and
            ' the renaming of files or directories. 
            g_oWatcher.NotifyFilter = (NotifyFilters.LastWrite Or NotifyFilters.FileName Or NotifyFilters.DirectoryName)
            ' Only watch text files.
            g_oWatcher.Filter = g_strAPIFile

            ' Add event handlers.
            'AddHandler g_oWatcher.Changed, AddressOf OnChanged
            AddHandler g_oWatcher.Created, AddressOf OnCreated
            'AddHandler g_oWatcher.Deleted, AddressOf OnDeleted
            'AddHandler g_oWatcher.Renamed, AddressOf OnRenamed

            ' Begin watching.
            g_oWatcher.EnableRaisingEvents = True

            ' Wait for the user to quit the program.
            Console.WriteLine("Waiting for File input")

            'Delete the API file to signal that we are ready.
            If System.IO.File.Exists(g_strProjectPath & g_strAPIFile) Then
                System.IO.File.Delete(g_strProjectPath & g_strAPIFile)
            End If

            While g_bExit = False
                System.Threading.Thread.Sleep(10)
            End While

        Catch ex As System.Exception
            Console.WriteLine("Error: " & ex.Message)
            Console.WriteLine("Closing Application")
            Try
                If g_strProjectPath.Length > 0 Then
                    Dim oFile As System.IO.File
                    Dim oWrite As System.IO.StreamWriter
                    oWrite = oFile.CreateText(g_strProjectPath & "ScriptError.txt")
                    oWrite.WriteLine(ex.Message)
                    oWrite.Close()
                End If
            Catch ex2 As System.Exception
                Console.WriteLine("Error while writing error log file: " & ex2.Message)
            End Try
        Finally
            If Not g_oApp Is Nothing Then
                Console.WriteLine("Exiting")
                g_oApp.CloseProject(False)
                g_oApp = Nothing
            End If
        End Try
    End Sub

    Sub ParseAPI(ByRef oRead As System.IO.StreamReader)
        Dim strLine As String = ""
        Dim iLine As Integer = 1
        Dim oParam As Param

        g_aryParams.Clear()
        Do
            strLine = oRead.ReadLine()

            If Not strLine Is Nothing Then
                If strLine.Trim.ToUpper = "EXIT" Then
                    Console.WriteLine("Exit file detected.")
                    strLine = Nothing
                    g_bExit = True
                Else
                    Dim aryParams As String() = strLine.Split(",")
                    If aryParams.Length < 3 Then
                        Throw New System.Exception("Inavlid parameters on line " & iLine & ", Line Text: " & strLine)
                    End If

                    oParam = New Param
                    oParam.m_strID = aryParams(0)
                    oParam.m_strPropName = aryParams(1)
                    oParam.m_strNewPropVal = aryParams(2)
                    oParam.SetValue()

                    g_aryParams.Add(oParam)

                    iLine = iLine + 1
                End If
            End If
        Loop Until strLine Is Nothing

        'Now save out the project file.
        If Not g_bExit Then
            g_oApp.ExportStandAloneSim(g_strProjectPath & g_strStandaloneProjectFile)

            'Now reset the original param values back
            For Each oParam In g_aryParams
                oParam.ResetValue()
            Next
        End If

        g_aryParams.Clear()

    End Sub

    Sub OnCreated(ByVal source As Object, ByVal e As FileSystemEventArgs)
        Dim oFile As System.IO.File
        Dim oRead As System.IO.StreamReader

        Try
            System.Threading.Thread.Sleep(100)

            Console.WriteLine("Beginning Parsing of '" & g_strProjectPath & "\" & g_strAPIFile & "'")
            oRead = oFile.OpenText(g_strProjectPath & "\" & g_strAPIFile)

            'Pause a bit to allow them to finish writing to the file.
            System.Threading.Thread.Sleep(10)

            'Then parse it
            ParseAPI(oRead)

            'Now save out the project file.
            'g_oApp.ExportStandAloneSim(g_strProjectPath & g_strStandaloneProjectFile)

            Console.WriteLine("Parsing Successful!")

        Catch ex As System.Exception
            Console.WriteLine("Error: " & ex.Message)
            Try
                Dim oFile2 As System.IO.File
                Dim oWrite As System.IO.StreamWriter
                oWrite = oFile2.CreateText(g_strProjectPath & "ScriptError.txt")
                oWrite.WriteLine(ex.Message)
                oWrite.Close()
            Catch ex2 As System.Exception
                Console.WriteLine("Error while writing error log file: " & ex2.Message)
            End Try
        Finally
            Try
                oRead.Close()
                Console.WriteLine("")
            Catch ex3 As System.Exception
            End Try
        End Try
    End Sub

    'Sub OnCreated(ByVal source As Object, ByVal e As FileSystemEventArgs)
    '    ' Specify what is done when a file is changed, created, or deleted.
    '    Console.WriteLine("File: " & e.FullPath & " " & e.ChangeType)
    'End Sub

    'Sub OnDeleted(ByVal source As Object, ByVal e As FileSystemEventArgs)
    '    ' Specify what is done when a file is changed, created, or deleted.
    '    Console.WriteLine("File: " & e.FullPath & " " & e.ChangeType)
    'End Sub

    'Sub OnRenamed(ByVal source As Object, ByVal e As RenamedEventArgs)
    '    ' Specify what is done when a file is renamed.
    '    Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath)
    'End Sub


    'Dim doArm As AnimatTools.DataObjects.Physical.Organism = DirectCast(AnimatTools.Framework.Util.Environment.FindOrganismByName("Arm"), AnimatTools.DataObjects.Physical.Organism)
    'Dim doAtttach As VortexAnimatTools.DataObjects.Physical.RigidBodies.MuscleAttachment = DirectCast(doArm.FindBodyPartByName("Bicep-Ulna"), VortexAnimatTools.DataObjects.Physical.RigidBodies.MuscleAttachment)
    'Dim doUlna As VortexAnimatTools.DataObjects.Physical.RigidBodies.Box = DirectCast(doArm.FindBodyPartByName("Ulna"), VortexAnimatTools.DataObjects.Physical.RigidBodies.Box)

    '        doAtttach.ZLocalLocationScaled.Value = -15
    '        doUlna.XRotationScaled.Value = 45
    '        doUlna.YRotationScaled.Value = 45

    '        oApp.ExportStandAloneSim("C:\Projects\bin\Experiments\Program Modules\AnimatLab Job Submitter\bin\LimbStiffness.asim")


End Module
