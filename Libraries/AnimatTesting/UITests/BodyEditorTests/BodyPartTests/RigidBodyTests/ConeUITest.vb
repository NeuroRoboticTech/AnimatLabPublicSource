Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard
Imports AnimatTesting.Framework

Namespace UITests
    Namespace BodyEditorTests
        Namespace BodyPartTests
            Namespace RigidBodyTests

                <CodedUITest()>
                Public Class ConeUITest
                    Inherits BodyPartUITest

#Region "Methods"

                    <TestMethod()>
                    Public Sub TestCone()
                        TestPart()
                    End Sub

                    Protected Overrides Sub TestMovableRigidBodyProperties(ByVal strStructure As String, ByVal strPart As String)
                        MyBase.TestMovableRigidBodyProperties(strStructure, strPart)

                        'Set the height to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Height", "0.2"})

                        'Set the height to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Height", "0"}, "The height of the cone cannot be less than or equal to zero.")

                        'Set the height to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Height", "-0.2"}, "The height of the cone cannot be less than or equal to zero.")

                        'Set the Upper radius to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Height", "0.1"})


                        'Set the Upper radius to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "UpperRadius", "0"})

                        'Set the height to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "LowerRadius", "0"}, "Both the upper and lower radius cannot be zero.")

                        'Set the height to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "UpperRadius", "-0.2"}, "The upper radius of the cone cannot be less than zero.")

                        'Set the Upper radius to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "UpperRadius", "0.1"})


                        'Set the lower radius to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "LowerRadius", "0"})

                        'Set the lower radius to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "UpperRadius", "0"}, "Both the upper and lower radius cannot be zero.")

                        'Set the lower radius to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "LowerRadius", "-0.2"}, "The lower radius of the cone cannot be less than zero.")

                        'Set the lower radius to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "LowerRadius", "0.1"})


                        'Set the Sides to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Sides", "35"})

                        'Set the Sides to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Sides", "5"}, "The number of sides for the cone cannot be less than ten.")

                        'Set the Sides to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Sides", "-2"}, "The number of sides for the cone cannot be less than ten.")

                        'Set the Sides to original value
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Sides", "30"})

                    End Sub

#Region "Additional test attributes"
                    '
                    ' You can use the following additional attributes as you write your tests:
                    '
                    ' Use TestInitialize to run code before running each test
                    <TestInitialize()> Public Overrides Sub MyTestInitialize()
                        MyBase.MyTestInitialize()

                        m_strPartType = "Cone"
                        m_strProjectName = "ConeTest"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\RigidBodyTests\" & m_strProjectName

                        m_iInitialZoomDist2 = 200
                        m_iSecondaryZoomDist2 = 200

                        m_ptTranslateZAxisStart = New Point(553, 394)
                        m_ptTranslateZAxisEnd = New Point(296, 392)

                        CleanupProjectDirectory()
                    End Sub

#End Region

#End Region

                End Class


            End Namespace
        End Namespace
    End Namespace
End Namespace