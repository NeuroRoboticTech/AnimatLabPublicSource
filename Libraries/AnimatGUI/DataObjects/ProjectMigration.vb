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

    Public MustInherit Class ProjectMigration

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
        Protected m_aryARNN_Files() As String
        Protected m_aryAFORM_Files() As String

        Protected m_xnProjectXml As New Framework.XmlDom

        Protected m_hashRigidBodies As New Hashtable
        Protected m_hashJoints As New Hashtable

        Public MustOverride ReadOnly Property ConvertFrom() As Integer
        Public MustOverride ReadOnly Property ConvertTo() As Integer

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
            m_aryARNN_Files = Directory.GetFiles(m_strProjectPath, "*.arnn")
            m_aryAFORM_Files = Directory.GetFiles(m_strProjectPath, "*.aform")

            'Copy all files in the current directory to the new one.
            Util.CopyDirectory(m_strProjectPath, m_strProjectPath & "\Backup", False)

        End Sub

        Protected Sub CopyFiles(ByVal aryFiles() As String)

            Try
                Dim strFilename As String
                For Each strFile As String In aryFiles
                    strFilename = Util.ExtractFilename(strFile)
                    If File.Exists(strFile) Then
                        File.Copy(strFile, m_strProjectPath & "\Backup\" & strFilename, True)
                    End If
                Next

            Catch ex As Exception
                Util.DisplayError(ex)
            End Try
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
            RemoveFiles(m_aryARNN_Files)
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
        End Sub

        Protected Overridable Sub ConvertProjectNode(ByVal xnProject As XmlNode)

        End Sub

        Protected Overridable Function CreateReplaceStringList() As Hashtable

            Dim aryReplaceText As New Hashtable()
            aryReplaceText.Add(20, New ReplaceText("CylinderContactSensor", "Cylinder"))
            aryReplaceText.Add(19, New ReplaceText("BoxContactSensor", "Box"))
            aryReplaceText.Add(18, New ReplaceText("InterbusrtLengthDistribution", "InterburstLengthDistribution"))
            aryReplaceText.Add(17, New ReplaceText("LicensedAnimatTools", "LicensedAnimatGUI"))
            aryReplaceText.Add(16, New ReplaceText("FastNeuralNetTools", "FiringRateGUI"))
            aryReplaceText.Add(15, New ReplaceText("RealisticNeuralNetTools", "IntegrateFireGUI"))
            aryReplaceText.Add(14, New ReplaceText("VortexAnimatTools.DataObjects.Physical.RigidBodies", "AnimatGUI.DataObjects.Physical.Bodies"))
            aryReplaceText.Add(13, New ReplaceText("VortexAnimatTools.DataObjects.Physical.Joints", "AnimatGUI.DataObjects.Physical.Joints"))
            aryReplaceText.Add(12, New ReplaceText("VortexAnimatTools.DataObjects.Behavior.Nodes.MuscleSpindle", "AnimatGUI.DataObjects.Behavior.Nodes.StretchReceptor"))
            aryReplaceText.Add(11, New ReplaceText("VortexAnimatTools.DataObjects.Physical.RigidBodies.BodyMesh", "AnimatGUI.DataObjects.Physical.RigidBodies.Mesh"))
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

