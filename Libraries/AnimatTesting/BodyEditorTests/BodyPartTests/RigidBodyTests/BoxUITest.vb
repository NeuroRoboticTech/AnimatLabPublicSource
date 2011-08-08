Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Windows.Input
Imports Microsoft.VisualStudio.TestTools.UITest.Extension
Imports Microsoft.VisualStudio.TestTools.UITesting
Imports Microsoft.VisualStudio.TestTools.UITesting.Keyboard

Namespace BodyEditorTests
    Namespace BodyPartTests
        Namespace RigidBodyTests

            <CodedUITest()>
            Public Class BoxUITest
                Inherits BodyPartUITest

#Region "Methods"

                <TestMethod()>
                Public Sub TestBox()
                    TestPart()
                End Sub

                Protected Overrides Sub TestMovableRigidBodyProperties(ByVal strStructure As String, ByVal strPart As String)
                    MyBase.TestMovableRigidBodyProperties(strStructure, strPart)

                    'Set the length to a valid value.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Length", "0.2"})

                    'Set the length to zero
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Length", "0"}, "The length of the box cannot be less than or equal to zero.")

                    'Set the length to a negative value
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Length", "-0.2"}, "The length of the box cannot be less than or equal to zero.")


                    'Set the height to a valid value.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Height", "0.2"})

                    'Set the height to zero
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Height", "0"}, "The height of the box cannot be less than or equal to zero.")

                    'Set the height to a negative value
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Height", "-0.2"}, "The height of the box cannot be less than or equal to zero.")


                    'Set the Width to a valid value.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Width", "0.2"})

                    'Set the Width to zero
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Width", "0"}, "The width of the box cannot be less than or equal to zero.")

                    'Set the Width to a negative value
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Width", "-0.2"}, "The width of the box cannot be less than or equal to zero.")


                    'Set the LengthSections to a valid value.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LengthSections", "2"})

                    'Set the LengthSections to zero
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LengthSections", "0"}, "The length sections of the box cannot be less than or equal to zero.")

                    'Set the LengthSections to a negative value
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "LengthSections", "-2"}, "The length sections of the box cannot be less than or equal to zero.")


                    'Set the HeightSections to a valid value.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "HeightSections", "2"})

                    'Set the HeightSections to zero
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "HeightSections", "0"}, "The height sections of the box cannot be less than or equal to zero.")

                    'Set the HeightSections to a negative value
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "HeightSections", "-2"}, "The height sections of the box cannot be less than or equal to zero.")


                    'Set the WidthSections to a valid value.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WidthSections", "2"})

                    'Set the WidthSections to zero
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WidthSections", "0"}, "The width sections of the box cannot be less than or equal to zero.")

                    'Set the WidthSections to a negative value
                    ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "WidthSections", "-2"}, "The width sections of the box cannot be less than or equal to zero.")

                    'Reset the length, width, and height properties back to their original values.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Length", "0.1"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Width", "0.1"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Structures\" & strStructure & "\Body Plan\" & strPart, "Height", "0.1"})

                End Sub

#Region "Additional test attributes"
                '
                ' You can use the following additional attributes as you write your tests:
                '
                ' Use TestInitialize to run code before running each test
                <TestInitialize()> Public Overrides Sub MyTestInitialize()
                    MyBase.MyTestInitialize()

                    m_strProjectName = "BoxTest"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\BodyEditorTests\BodyPartTests\RigidBodyTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\BodyEditorTests\BodyPartTests\RigidBodyTests\" & m_strProjectName

                    CleanupProjectDirectory()
                End Sub

#End Region

#End Region

            End Class


        End Namespace
    End Namespace
End Namespace
