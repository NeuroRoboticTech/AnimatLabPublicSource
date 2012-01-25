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
        Namespace NeuralTests

            <CodedUITest()>
            Public Class NeuralUIConversionUITest
                Inherits ConversionUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"
                '

#Region "Firing Rate Methods"

                <TestMethod()>
                Public Sub Test_FiringRate_NormalNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Ie", 0.0000000001)
                    aryMaxErrors.Add("FF", 0.01)
                    aryMaxErrors.Add("Vm", 0.0001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_NormalNeuron"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Now decrease Cm from 3nf to 1nf
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Cm", "1 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Cm1nf_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Cm", "3 n"})

                    'Now increase Gm from 100nS to 200nS
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Gm", "200 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Gm200ns_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Gm", "100 n"})

                    'Now increase Vth from 0 to 20mV
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vth", "20 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vth20mv_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vth", "0 m"})

                    'Now increase FMin from 0 to 0.2 Hz
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Fmin", "0.2"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Fmin2_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Fmin", "0"})

                    'Now increase Gain from 15 to 20
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Gain", "20"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Gain20_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Gain", "15"})

                    'Now decrease Vrest from 0 to -70 mv and Vth
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vrest", "-70 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vth", "-70 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vrest-70mv_")

                    'Now increase accomodation from 0 to 1
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeAccommodation", "1"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Accom1_")

                    'Now decrease accomodation tc from 200 ms to 100 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AccommodationTimeConstant", "100 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AccomTc100ms_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeAccommodation", "0"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AccommodationTimeConstant", "200 m"})

                    'Change Vnoise to 5 mv
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "VNoiseMax", "5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vnoise5mv_")

                End Sub

                <TestMethod()>
                Public Sub Test_FiringRate_PacemakerNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF", 0.01)
                    aryMaxErrors.Add("Vm", 0.0001)
                    aryMaxErrors.Add("Intrinsic", 0.0000000001)
                    aryMaxErrors.Add("External", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_PacemakerNeuron"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Now decrease Cm from 3nf to 1nf
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Ih", "2.5 n"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Il", "-2.5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IhIl_2_5_")

                    'Now increase Gm from 100nS to 200nS
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Th", "0.5"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Th_0_5_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Th", "1"})

                    'Now increase Vth from 0 to 20mV
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Mtl", "-50"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Mtl_-50_")

                    'Now increase FMin from 0 to 0.2 Hz
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Btl", "1"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Btl_1_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Mtl", "-100"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Btl", "2"})

                    'Now increase Gain from 15 to 20
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vssm", "0.5 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\C", "CurrentOn", "-2 n"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\D", "CurrentOn", "2 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vsm_0_5_")

                End Sub

#End Region

#Region "Additional test attributes"
                '
                ' You can use the following additional attributes as you write your tests:
                '
                ' Use TestInitialize to run code before running each test
                <TestInitialize()> Public Overrides Sub MyTestInitialize()
                    MyBase.MyTestInitialize()

                    'This test compares data to that generated from the old version. We never re-generate the data in V2, so this should always be false 
                    'regardless of the setting in app.config.
                    m_bGenerateTempates = False

                End Sub

                <TestCleanup()> Public Overrides Sub MyTestCleanup()
                    MyBase.MyTestCleanup()
                End Sub

                Protected Overrides Sub SetWindowsToOpen()
                    m_aryWindowsToOpen.Add("Tool Viewers\NeuralData")
                End Sub

#End Region

#End Region

            End Class

        End Namespace
    End Namespace
End Namespace

