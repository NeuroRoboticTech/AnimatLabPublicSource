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
        Protected m_aryOBJ_Files() As String
        Protected m_aryMTL_Files() As String
        Protected m_aryASE_Files() As String
        Protected m_aryATVF_Files() As String
        Protected m_aryABEF_Files() As String
        Protected m_aryAFNN_Files() As String

        Protected m_xnProjectXml As New Framework.XmlDom

        Public MustOverride ReadOnly Property ConvertFrom() As Single
        Public MustOverride ReadOnly Property ConvertTo() As Single

        Sub New()

            'm_xnProjectXml.m_aryClassReplacements.Add(New Framework.XmlDom.ClassReplacementType("VortexAnimatTools.DataObjects.Physical.RigidBodies.Box", "AnimatGUI.DataObjects.Physical.Bodies.Box"))
            'm_xnProjectXml.m_aryClassReplacements.Add(New Framework.XmlDom.ClassReplacementType("Animatlibrary", "AnimatSim"))
            'm_xnProjectXml.m_aryClassReplacements.Add(New Framework.XmlDom.ClassReplacementType("VortexAnimatTools.", "AnimatGUI."))
            'm_xnProjectXml.m_aryClassReplacements.Add(New Framework.XmlDom.ClassReplacementType("VortexAnimatlibrary", "VortexAnimatSim"))
            'm_xnProjectXml.m_aryDllModuleReplacements.Add(New Framework.XmlDom.DllReplacementTypes("FastNeural", "AnimatGUI"))

        End Sub

        Public Overridable Sub ConvertFiles(ByVal strProjectFile As String)

            Dim strProjectFilename As String
            Util.SplitPathAndFile(strProjectFile, m_strProjectPath, strProjectFilename)
            m_strProjectName = strProjectFilename.Replace(".aproj", "")

            'Remove the last \ if it is there.
            If m_strProjectPath(m_strProjectPath.Length - 1) = "\" Then m_strProjectPath = m_strProjectPath.Remove(m_strProjectPath.Length - 1, 1)

            If File.Exists(m_strProjectPath & "\Test_" & m_strProjectName & ".aproj") Then
                File.Delete(m_strProjectPath & "\Test_" & m_strProjectName & ".aproj")
            End If

            BackupFiles()

            For Each strProjFile As String In m_aryAPROJ_Files
                ConvertProject(strProjFile)
            Next

            RemoveOldFiles()

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
            m_aryOBJ_Files = Directory.GetFiles(m_strProjectPath, "*.obj")
            m_aryMTL_Files = Directory.GetFiles(m_strProjectPath, "*.mtl")
            m_aryASE_Files = Directory.GetFiles(m_strProjectPath, "*.ase")
            m_aryATVF_Files = Directory.GetFiles(m_strProjectPath, "*.atvf")
            m_aryABEF_Files = Directory.GetFiles(m_strProjectPath, "*.abef")
            m_aryAFNN_Files = Directory.GetFiles(m_strProjectPath, "*.afnn")

            If Not Directory.Exists(m_strProjectPath & "\Backup") Then
                Directory.CreateDirectory(m_strProjectPath & "\Backup")
            End If

            CopyFiles(m_aryAPROJ_Files)
            CopyFiles(m_aryASTL_Files)
            CopyFiles(m_aryABPE_Files)
            CopyFiles(m_aryABSYS_Files)
            CopyFiles(m_aryASIM_Files)
            CopyFiles(m_aryOBJ_Files)
            CopyFiles(m_aryMTL_Files)
            CopyFiles(m_aryASE_Files)
            CopyFiles(m_aryATVF_Files)
            CopyFiles(m_aryABEF_Files)
            CopyFiles(m_aryAFNN_Files)

        End Sub

        Protected Sub CopyFiles(ByVal aryFiles() As String)

            Dim strFilename As String
            For Each strFile As String In aryFiles
                strFilename = Util.ExtractFilename(strFile)

                File.Copy(strFile, m_strProjectPath & "\Backup\" & strFilename, True)
            Next

        End Sub

        Protected Sub RemoveOldFiles()
            RemoveFiles(m_aryASTL_Files)
            RemoveFiles(m_aryABPE_Files)
            RemoveFiles(m_aryABSYS_Files)
            RemoveFiles(m_aryASIM_Files)
            RemoveFiles(m_aryOBJ_Files)
            RemoveFiles(m_aryMTL_Files)
            RemoveFiles(m_aryASE_Files)
            RemoveFiles(m_aryATVF_Files)
            RemoveFiles(m_aryABEF_Files)
            RemoveFiles(m_aryAFNN_Files)
        End Sub

        Protected Sub RemoveFiles(ByVal aryFiles() As String)

            For Each strFile As String In aryFiles
                If File.Exists(strFile) Then File.Delete(strFile)
            Next

        End Sub

        Protected Overridable Sub ConvertProject(ByVal strProjFile As String)

            m_xnProjectXml.Load(strProjFile)

            Dim xnProjectNode As XmlNode = m_xnProjectXml.GetRootNode("Project")

            ConvertProjectNode(xnProjectNode)

            m_xnProjectXml.Save(strProjFile)

            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\CylinderTest\Test_CylinderTest.aproj")
            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\MuscleTest\Test_MuscleTest.aproj")
            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\SphereTest\Test_SphereTest.aproj")
            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\NeuralTest\Test_NeuralTest.aproj")
            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\ConeTest\Test_ConeTest.aproj")
            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\ConeNN\Test_ConeNN.aproj")
            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\HingeTest\Test_HingeTest.aproj")
            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\MeshTest\Test_MeshTest.aproj")
            'm_xnProjectXml.Save("C:\Projects\AnimatLabSDK\Experiments\ConversionTests\Converted\PrismaticTest\Test_PrismaticTest.aproj")
            'm_xnProjectXml.Save(m_strProjectPath & "\Test_" & m_strProjectName & ".aproj")
        End Sub

        Protected Overridable Sub ConvertProjectNode(ByVal xnProject As XmlNode)

        End Sub

        Protected Overridable Function CreateReplaceStringList() As Hashtable

            Dim aryReplaceText As New Hashtable()
            aryReplaceText.Add(16, New ReplaceText("InterbusrtLengthDistribution", "InterburstLengthDistribution"))
            aryReplaceText.Add(15, New ReplaceText("LicensedAnimatTools", "LicensedAnimatGUI"))
            aryReplaceText.Add(14, New ReplaceText("FastNeuralNetTools", "FiringRateGUI"))
            aryReplaceText.Add(13, New ReplaceText("RealisticNeuralNetTools", "IntegrateFireGUI"))
            aryReplaceText.Add(12, New ReplaceText("VortexAnimatTools.DataObjects.Physical.RigidBodies", "AnimatGUI.DataObjects.Physical.Bodies"))
            aryReplaceText.Add(11, New ReplaceText("VortexAnimatTools.DataObjects.Physical.Joints", "AnimatGUI.DataObjects.Physical.Joints"))
            aryReplaceText.Add(10, New ReplaceText("VortexAnimatTools", "AnimatGUI"))
            aryReplaceText.Add(9, New ReplaceText("AnimatTools", "AnimatGUI"))
            aryReplaceText.Add(8, New ReplaceText("AHPConductance", "AHP_Conductance"))
            aryReplaceText.Add(7, New ReplaceText("AHPTimeConstant", "AHP_TimeConstant"))
            aryReplaceText.Add(6, New ReplaceText("BodyPositionX", "WorldPositionX"))
            aryReplaceText.Add(5, New ReplaceText("BodyPositionY", "WorldPositionY"))
            aryReplaceText.Add(4, New ReplaceText("BodyPositionZ", "WorldPositionZ"))
            aryReplaceText.Add(3, New ReplaceText("BodyRotationX", "RotationX"))
            aryReplaceText.Add(2, New ReplaceText("BodyRotationY", "RotationY"))
            aryReplaceText.Add(1, New ReplaceText("BodyRotationZ", "RotationZ"))

            Return aryReplaceText
        End Function

        Public Class ReplaceText
            Public m_strFind As String
            Public m_strReplace As String

            Public Sub New(ByVal strFind As String, ByVal strReplace As String)
                m_strFind = strFind
                m_strReplace = strReplace
            End Sub
        End Class

    End Class

End Namespace

