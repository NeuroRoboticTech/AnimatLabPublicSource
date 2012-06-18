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
        Namespace BodyPartTests
            Namespace RigidBodyTests

                <CodedUITest()>
                Public Class ReceptiveFieldConversionUITest
                    Inherits ConversionUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"

                    <TestMethod()>
                    Public Sub Test_ReceptiveFields()

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("ConeX", 0.04)
                        aryMaxErrors.Add("ConeY", 0.04)
                        aryMaxErrors.Add("ConeZ", 0.04)
                        aryMaxErrors.Add("default", 0.04)

                        m_strProjectName = "ReceptiveFields"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", "ReceptiveFieldSensor.ReceptiveCurrentGain.C", "100 n"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CurrentGain_C_100n_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", "ReceptiveFieldSensor.ReceptiveFieldGain.Width", "300 "})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FieldGain_Width_300_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", "ReceptiveFieldSensor.ReceptiveFieldGain.Width", "50 "})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FieldGain_Width_50_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Skin\Body Plan\Skin", "ReceptiveFieldSensor.ReceptiveFieldGain.Width", "150 "})

                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_5\Cone", "Height", "10 c"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_5\Cone", "UpperRadius", "5 c"})

                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "UpperRadius_5cm_")

                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_5\Cone", "UpperRadius", "0 c"})
                        'ExecuteMethodAssertError("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_5\Cone", "LowerRadius", "0 c"}, "Both the upper and lower radius cannot be zero.", enumErrorTextType.BeginsWith)

                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_5\Cone", "UpperRadius", "5 c"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_5\Cone", "LowerRadius", "0 c"})

                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "LowerRadius_0cm_")

                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_5\Cone", "UpperRadius", "1 c"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_5\Cone", "LowerRadius", "0 c"})
                        'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root\Joint_5\Cone", "Height", "25 c"})

                        'RunSimulationWaitToEnd()
                        'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "UpperRadius_1cm_")

                    End Sub

                    <TestMethod()>
                    Public Sub Test_ReceptiveFields_Kg_M()

                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("1", 0.1)
                        aryMaxErrors.Add("2", 0.1)
                        aryMaxErrors.Add("RootY", 0.1)
                        aryMaxErrors.Add("default", 0.1)

                        m_strProjectName = "ReceptiveFields_Kg_M"
                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root", "Rotation.X", "180 "})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FlipX_180_")

                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root", "Rotation.X", "0 "})
                        ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Root", "ReceptiveFieldSensor.ReceptiveFieldGain.Width", "1 "})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Field_Gain_Width_1_")

                    End Sub


#Region "Additional test attributes"
                    '
                    ' You can use the following additional attributes as you write your tests:
                    '
                    ' Use TestInitialize to run code before running each test
                    <TestInitialize()> Public Overrides Sub MyTestInitialize()
                        MyBase.MyTestInitialize()

                        'This conversion is different than others. The contact collisions are generated differently than before, so I cannot
                        'use the old version data as a template. I will check the output to make sure it is similar, but use the new version as the template.
                        'm_bGenerateTempates = False

                    End Sub

                    <TestCleanup()> Public Overrides Sub MyTestCleanup()
                        MyBase.MyTestCleanup()
                    End Sub
#End Region

#End Region

                End Class

            End Namespace
        End Namespace
    End Namespace
End Namespace
