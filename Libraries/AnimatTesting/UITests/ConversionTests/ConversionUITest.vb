Imports System.Windows.Forms
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Tcp
Imports System
Imports System.CodeDom.Compiler
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.WinControls
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse
Imports MouseButtons = System.Windows.Forms.MouseButtons
Imports AnimatTesting.Framework
Imports System.Xml

Namespace UITests
    Namespace ConversionTests

        <CodedUITest()>
        Public MustInherit Class ConversionUITest
            Inherits AnimatUITest

#Region "Attributes"

            Protected m_strOldProjectFolder As String

            Protected m_aryWindowsToOpen As New ArrayList

#End Region

#Region "Properties"

#End Region

#Region "Methods"

            Protected Overridable Sub SetWindowsToOpen()
            End Sub

            Protected Overridable Sub TestConversionProject(ByVal strDataPrefix As String, Optional iMaxRows As Integer = -1, Optional ByVal dblMaxError As Double = 0.05)

                Dim aryMaxErrors As New Hashtable
                aryMaxErrors.Add("default", dblMaxError)

                TestConversionProject(strDataPrefix, aryMaxErrors, iMaxRows)
            End Sub

            Protected Overridable Sub TestConversionProject(ByVal strDataPrefix As String, ByVal aryMaxErrors As Hashtable, Optional iMaxRows As Integer = -1, Optional ByVal aryIgnoreRows As ArrayList = Nothing)
                Debug.WriteLine("TestConversionProject. DataPrefix: " & strDataPrefix & ", MaxErrors: " & Util.ParamsToString(aryMaxErrors) & ", MaxRows: " & iMaxRows)

                SetWindowsToOpen()

                ConvertProject()

                'Open any data/sim windows that are needed.
                For Each strWindowPath As String In m_aryWindowsToOpen
                    'Open the Structure_1 body plan editor window
                    ExecuteMethod("DblClickWorkspaceItem", New Object() {strWindowPath}, 2000)
                Next

                Threading.Thread.Sleep(3000)

                'Run the simulation and wait for it to end.
                RunSimulationWaitToEnd()

                ''Compare chart data to verify simulation results.
                CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, strDataPrefix, iMaxRows, aryIgnoreRows)

            End Sub

            Protected Overridable Sub ConvertProject()
                Debug.WriteLine("ConvertProject")

                CleanupConversionProjectDirectory()

                StartExistingProject()

                'Converted project should always ask to be converted.
                OpenDialogAndWait("Convert Project", Nothing, Nothing)

                'Click 'Ok' button
                ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                Threading.Thread.Sleep(3000)

                OpenDialogAndWait("Project Conversion", Nothing, Nothing)

                'Click 'Ok' button
                ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)

                Threading.Thread.Sleep(3000)
            End Sub

            Protected Overridable Sub CleanupConversionProjectDirectory()
                'Make sure any left over project directory is cleaned up before starting the test.
                If m_strRootFolder.Length > 0 AndAlso m_strProjectPath.Length > 0 AndAlso m_strProjectName.Length > 0 Then
                    DeleteDirectory(m_strRootFolder & m_strProjectPath & "\" & m_strProjectName)
                End If

                'Copy the old version project folder back so we can load it up.
                If m_strOldProjectFolder.Length > 0 Then
                    Util.CopyDirectory(m_strRootFolder & m_strOldProjectFolder, m_strRootFolder & m_strProjectPath & "\" & m_strProjectName)
                End If
            End Sub


            Protected Overridable Sub ModifyJointRotationInProjectFile(ByVal strPath As String, _
                                                                       ByVal dblJointRotX As Double, _
                                                                       ByVal dblJointRotY As Double, _
                                                                       ByVal dblJointRotZ As Double, _
                                                                       ByVal strOrientation As String)
                Debug.WriteLine("ModifyJointRotationInProjectFile. Path: '" & strPath & "', dblJointRotX: " & dblJointRotX & ", dblJointRotY: " & dblJointRotY & ", dblJointRotZ: " & dblJointRotZ & ", strOrientation: " & strOrientation)

                Dim strFile As String = m_strRootFolder & strPath & "\Structure_1.astl"
                Dim xnProject As New XmlDom()
                xnProject.Load(strFile)

                Dim xnStruct As XmlNode = xnProject.GetRootNode("Structure")
                Dim xnRoot As XmlNode = xnProject.GetNode(xnStruct, "RigidBody")
                Dim xnChildren As XmlNode = xnProject.GetNode(xnRoot, "ChildBodies")
                Dim xnChild As XmlNode = xnChildren.FirstChild
                Dim xnJoint As XmlNode = xnProject.GetNode(xnChild, "Joint")
                xnProject.UpdateSingleNodeValue(xnJoint, "OrientationMatrix", strOrientation)

                xnProject.Save(strFile)

            End Sub

            Protected Overridable Sub ModifyJointConstraintsInProjectFile(ByVal strPath As String, _
                                                                        ByVal dblMin As Single, _
                                                                        ByVal dblMax As Single, _
                                                                        ByVal bRotational As Boolean, _
                                                                        ByVal dblDamping As Single, _
                                                                        ByVal dblRestitution As Single, _
                                                                        ByVal dblStiffness As Single)
                Debug.WriteLine("ModifyJointConstraintsInProjectFile. Path: '" & strPath & "', dblMin: " & dblMin & ", dblMax: " & dblMax & ", bRotational: " & bRotational & ", dblDamping: " & dblDamping & ", dblRestitution: " & dblRestitution & ", dblStiffness: " & dblStiffness)

                Dim strFile As String = m_strRootFolder & strPath & "\Structure_1.astl"
                Dim xnProject As New XmlDom()
                xnProject.Load(strFile)

                Dim xnStruct As XmlNode = xnProject.GetRootNode("Structure")
                Dim xnRoot As XmlNode = xnProject.GetNode(xnStruct, "RigidBody")
                Dim xnChildren As XmlNode = xnProject.GetNode(xnRoot, "ChildBodies")
                Dim xnChild As XmlNode = xnChildren.FirstChild
                Dim xnJoint As XmlNode = xnProject.GetNode(xnChild, "Joint")
                Dim xnConstraint As XmlNode = xnProject.GetNode(xnJoint, "Constraint")

                If bRotational Then
                    xnProject.UpdateSingleNodeAttribute(xnConstraint, "Low", Util.DegreesToRadians(dblMin).ToString)
                    xnProject.UpdateSingleNodeAttribute(xnConstraint, "High", Util.DegreesToRadians(dblMax).ToString)
                Else
                    xnProject.UpdateSingleNodeAttribute(xnConstraint, "Low", dblMin.ToString)
                    xnProject.UpdateSingleNodeAttribute(xnConstraint, "High", dblMax.ToString)
                End If

                xnProject.RemoveNode(xnConstraint, "Damping")
                xnProject.RemoveNode(xnConstraint, "Restitution")
                xnProject.RemoveNode(xnConstraint, "Stiffness")

                xnProject.AddScaledNumber(xnConstraint, "Damping", dblDamping, "None", dblDamping)
                xnProject.AddScaledNumber(xnConstraint, "Restitution", dblRestitution, "None", dblRestitution)
                xnProject.AddScaledNumber(xnConstraint, "Stiffness", dblStiffness, "None", dblStiffness)

                xnProject.Save(strFile)

            End Sub

            Protected Overridable Sub ModifyRootRotationInProjectFile(ByVal strPath As String, _
                                                                       ByVal dblJointRotX As Double, _
                                                                       ByVal dblJointRotY As Double, _
                                                                       ByVal dblJointRotZ As Double, _
                                                                       ByVal strOrientation As String)
                Debug.WriteLine("ModifyRootRotationInProjectFile. Path: '" & strPath & "', dblJointRotX: " & dblJointRotX & ", dblJointRotY: " & dblJointRotY & ", dblJointRotZ: " & dblJointRotZ & ", strOrientation: " & strOrientation)

                Dim strFile As String = m_strRootFolder & strPath & "\Structure_1.astl"
                Dim xnProject As New XmlDom()
                xnProject.Load(strFile)

                Dim xnStruct As XmlNode = xnProject.GetRootNode("Structure")
                Dim xnRoot As XmlNode = xnProject.GetNode(xnStruct, "RigidBody")
                xnProject.UpdateSingleNodeValue(xnRoot, "OrientationMatrix", strOrientation)

                xnProject.Save(strFile)

            End Sub

            Protected Overridable Sub ModifyChild1RotationInProjectFile(ByVal strPath As String, _
                                                                        ByVal strChild1ID As String, _
                                                                       ByVal dblJointRotX As Double, _
                                                                       ByVal dblJointRotY As Double, _
                                                                       ByVal dblJointRotZ As Double, _
                                                                       ByVal strOrientation As String)
                Debug.WriteLine("ModifyRootRotationInProjectFile. Path: '" & strPath & "', strChild1ID: " & strChild1ID & ", dblJointRotY: " & dblJointRotX & ", dblJointRotY: " & dblJointRotY & ", dblJointRotZ: " & dblJointRotZ & ", strOrientation: " & strOrientation)

                Dim strFile As String = m_strRootFolder & strPath & "\Structure_1.astl"
                Dim xnProject As New XmlDom()
                xnProject.Load(strFile)

                Dim xnStruct As XmlNode = xnProject.GetRootNode("Structure")
                Dim xnRoot As XmlNode = xnProject.GetNode(xnStruct, "RigidBody")
                Dim xnChild As XmlNode = xnProject.FindChildDataObject(xnRoot, strChild1ID)
                xnProject.UpdateSingleNodeValue(xnChild, "OrientationMatrix", strOrientation)

                xnProject.Save(strFile)

            End Sub

#End Region

        End Class

    End Namespace
End Namespace


