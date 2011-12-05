Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects

    Public MustInherit Class FileConverter

        Protected m_strProjectPath As String
        Protected m_strProjectName As String

        Protected m_aryAPROJ_Files() As String
        Protected m_aryASTL_Files() As String
        Protected m_aryABSYS_Files() As String
        Protected m_aryABPE_Files() As String
        Protected m_aryASIM_Files() As String

        Protected m_xnProjectXml As New Framework.XmlDom

        Public MustOverride ReadOnly Property ConvertFrom() As String
        Public MustOverride ReadOnly Property ConvertTo() As String

        Sub New()

            'm_xnProjectXml.m_aryClassReplacements.Add(New Framework.XmlDom.ClassReplacementType("VortexAnimatTools.DataObjects.Physical.RigidBodies.Box", "AnimatGUI.DataObjects.Physical.Bodies.Box"))
            'm_xnProjectXml.m_aryClassReplacements.Add(New Framework.XmlDom.ClassReplacementType("Animatlibrary", "AnimatSim"))
            'm_xnProjectXml.m_aryClassReplacements.Add(New Framework.XmlDom.ClassReplacementType("VortexAnimatTools.", "AnimatGUI."))
            'm_xnProjectXml.m_aryClassReplacements.Add(New Framework.XmlDom.ClassReplacementType("VortexAnimatlibrary", "VortexAnimatSim"))
            'm_xnProjectXml.m_aryDllModuleReplacements.Add(New Framework.XmlDom.DllReplacementTypes("FastNeural", "AnimatGUI"))

        End Sub

        Public Overridable Sub ConvertFiles(ByVal strProjectPath As String, ByVal strProjectName As String)

            m_strProjectPath = strProjectPath
            m_strProjectName = strProjectName

            If File.Exists(m_strProjectPath & "\Test_" & m_strProjectName & ".aproj") Then
                File.Delete(m_strProjectPath & "\Test_" & m_strProjectName & ".aproj")
            End If

            BackupFiles()

            For Each strProjFile As String In m_aryAPROJ_Files
                ConvertProject(strProjFile)
            Next

        End Sub

        Protected Sub BackupFiles()

            If Not Directory.Exists(m_strProjectPath) Then
                Throw New System.Exception("The specified project directory does not exist: '" & m_strProjectPath & "'.")
            End If

            m_aryAPROJ_Files = Directory.GetFiles(m_strProjectPath, "*.aproj")
            m_aryASTL_Files = Directory.GetFiles(m_strProjectPath, "*.astl")
            m_aryABPE_Files = Directory.GetFiles(m_strProjectPath, "*.abpe")
            m_aryABSYS_Files = Directory.GetFiles(m_strProjectPath, "*.absys")
            m_aryASIM_Files = Directory.GetFiles(m_strProjectPath, "*.asim")

            If Not Directory.Exists(m_strProjectPath & "\Backup") Then
                Directory.CreateDirectory(m_strProjectPath & "\Backup")
            End If

            CopyFiles(m_aryAPROJ_Files)
            CopyFiles(m_aryASTL_Files)
            CopyFiles(m_aryABPE_Files)
            CopyFiles(m_aryABSYS_Files)
            CopyFiles(m_aryASIM_Files)

        End Sub

        Protected Sub CopyFiles(ByVal aryFiles() As String)

            Dim strFilename As String
            For Each strFile As String In aryFiles
                strFilename = Util.ExtractFilename(strFile)

                File.Copy(strFile, m_strProjectPath & "\Backup\" & strFilename, True)
            Next

        End Sub

        Protected Overridable Sub ConvertProject(ByVal strProjFile As String)

            m_xnProjectXml.Load(strProjFile)


            Dim xnProjectNode As XmlNode = m_xnProjectXml.GetRootNode("Project")

            ConvertProjectNode(xnProjectNode)

            m_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\MuscleTest\Test_MuscleTest.aproj")
            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\SphereTest\Test_SphereTest.aproj")
            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\NeuralTest\Test_NeuralTest.aproj")
            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\ConeTest\Test_ConeTest.aproj")
            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\ConeNN\Test_ConeNN.aproj")
            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\SimpleTest\Test_SimpleTest.aproj")
            'm_xnProjectXml.Save(m_strProjectPath & "\Test_" & m_strProjectName & ".aproj")
        End Sub

        Protected Overridable Sub ConvertProjectNode(ByVal xnProject As XmlNode)

        End Sub

    End Class

End Namespace

