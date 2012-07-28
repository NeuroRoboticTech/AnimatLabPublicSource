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
                Public Class TorusUITest
                    Inherits BodyPartUITest

#Region "Methods"

                    <TestMethod()>
                    Public Sub Test_Torus()
                        TestPart()
                    End Sub

                    Protected Overrides Sub TestMovableRigidBodyProperties(ByVal strStructure As String, ByVal strPart As String)
                        MyBase.TestMovableRigidBodyProperties(strStructure, strPart)

                        'Set the InnerRadius to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "InnerRadius", "0.02"})

                        'Set the InnerRadius to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "InnerRadius", "0"}, "The inner radius of the torus cannot be less than or equal to zero.")

                        'Set the InnerRadius to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "InnerRadius", "-0.2"}, "The inner radius of the torus cannot be less than or equal to zero.")

                        'Set the InnerRadius to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "InnerRadius", "0.2"}, "The inner radius of the torus cannot be larger or equal to the outer raidus.")

                        'Set the InnerRadius to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "InnerRadius", "0.01"})


                        'Set the OuterRadius to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "OuterRadius", "0.04"})

                        'Set the OuterRadius to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "OuterRadius", "0"}, "The outer radius of the torus cannot be less than or equal to zero.")

                        'Set the OuterRadius to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "OuterRadius", "-0.2"}, "The outer radius of the torus cannot be less than or equal to zero.")

                        'Set the OuterRadius to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "OuterRadius", "0.005"}, "The outer radius of the torus cannot be less than or equal to the inner raidus.")

                        'Set the OuterRadius to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "OuterRadius", "0.05"})


                        'Set the Rings to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Rings", "25"})

                        'Set the Rings to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Rings", "5"}, "The number of rings for the torus cannot be less than ten.")

                        'Set the Rings to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Rings", "-2"}, "The number of rings for the torus cannot be less than ten.")

                        'Set the Rings to original value
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Rings", "20"})



                        'Set the Sides to a valid value.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Sides", "25"})

                        'Set the Sides to zero
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Sides", "5"}, "The number of sides for the torus cannot be less than ten.")

                        'Set the Sides to a negative value
                        ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Sides", "-2"}, "The number of sides for the torus cannot be less than ten.")

                        'Set the Sides to original value
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & strStructure & "\Body Plan\" & strPart, "Sides", "20"})


                    End Sub

                    Protected Overrides Sub BeforeAddChildPart(ByVal strPartType As String, ByVal strJointType As String)
                        'Reset the rotation of the torus so it is 90 degrees rotated around Y
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Rotation.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Rotation.Y", "90"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Rotation.Z", "0"})
                    End Sub

                    Protected Overrides Sub AfterAddChildPartJoint(ByVal strPartType As String, ByVal strJointType As String)
                        'Reset the rotation of the torus so it is 90 degrees rotated around Y
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Rotation.X", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Rotation.Y", "0"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Rotation.Z", "0"})
                    End Sub


#Region "Additional test attributes"
                    '
                    ' You can use the following additional attributes as you write your tests:
                    '
                    ' Use TestInitialize to run code before running each test
                    <TestInitialize()> Public Overrides Sub MyTestInitialize()
                        MyBase.MyTestInitialize()

                        m_strPartType = "Torus"
                        m_strProjectName = "TorusTest"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\RigidBodyTests\" & m_strProjectName

                        m_ptClickToAddChild = New Point(775, 353)

                        CleanupProjectDirectory()
                    End Sub

#End Region

#End Region

                End Class

            End Namespace
        End Namespace
    End Namespace
End Namespace
