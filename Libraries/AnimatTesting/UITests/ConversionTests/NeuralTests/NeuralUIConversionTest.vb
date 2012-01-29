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
                Public Sub Test_FiringRate_BistableNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF", 0.01)
                    aryMaxErrors.Add("Vm", 0.0001)
                    aryMaxErrors.Add("Il", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_BistableNeuron"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.5 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_0_5ms_")

                    'Change the time step of the firing rate neural sim to 1 ms, physics time step to 0.5 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "1 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_1ms_PhysicsTimeStep_0_5ms_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "2 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    'Set Vth to 12 mv
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vsth", "12 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vsth_12mv_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Vsth", "10 m"})

                    'Set ih to 4na, Il to -1 na
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Ih", "4 n"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Il", "-1 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Ih_4na_Il_-1na_")

                    'Set stim_3 to -5 nA
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_3", "CurrentOn", "-5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Stim3_-5na_")

                End Sub

                <TestMethod()>
                Public Sub Test_FiringRate_GatedSynapse()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF_1a", 0.01)
                    aryMaxErrors.Add("FF_1b", 0.01)
                    aryMaxErrors.Add("FF_2", 0.01)
                    aryMaxErrors.Add("FF_3", 0.01)
                    aryMaxErrors.Add("FF_4", 0.01)
                    aryMaxErrors.Add("FF_5", 0.01)
                    aryMaxErrors.Add("FF_6b", 0.01)
                    aryMaxErrors.Add("External_1a", 0.0000000001)
                    aryMaxErrors.Add("External_3", 0.0000000001)
                    aryMaxErrors.Add("Synaptic_2", 0.0000000001)
                    aryMaxErrors.Add("External_1b", 0.0000000001)
                    aryMaxErrors.Add("External_5", 0.0000000001)
                    aryMaxErrors.Add("Synaptic_4", 0.0000000001)
                    aryMaxErrors.Add("External_6b", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_GatedSynapse"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.5 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_0_5ms_")

                    'Change the time step of the firing rate neural sim to 1 ms, physics time step to 0.5 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "1 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_1ms_PhysicsTimeStep_0_5ms_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "2 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 1] 1 (5 nA))", "Weight", "0.5"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -1] 1 (5 nA))", "Weight", "-0.5"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "W_0_5_")

                    'swap settings.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 0.5] 1 (5 nA))", "Weight", "-1"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W -1] 1 (5 nA))", "GateInitiallyOn", "True"})

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -0.5] 1 (5 nA))", "Weight", "1"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W 1] 1 (5 nA))", "GateInitiallyOn", "False"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SwapGates_")


                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W -1] 1 (5 nA))", _
                                                                 "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\6 (1 nA)"})
                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W 1] 1 (5 nA))", _
                                                                 "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\6 (1 nA)"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (5 nA)", "Enabled", "False"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\1 (5 nA)", "Enabled", "False"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_9", "Enabled", "True"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SwapLinkedSynapses_")

                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W -1] 6 (1 nA))", "Delete Link")
                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W 1] 6 (1 nA))", "Delete Link")
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeletedGates_")

                End Sub

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

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.5 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_0_5ms_")

                    'Change the time step of the firing rate neural sim to 1 ms, physics time step to 0.5 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "1 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_1ms_PhysicsTimeStep_0_5ms_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "2 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

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
                Public Sub Test_FiringRate_NormalSynapse()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF_1", 0.01)
                    aryMaxErrors.Add("FF_2", 0.01)
                    aryMaxErrors.Add("Vm_1", 0.0001)
                    aryMaxErrors.Add("Vm_2", 0.0001)
                    aryMaxErrors.Add("Ie_1", 0.0000000001)
                    aryMaxErrors.Add("Ii_1", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_NormalSynapse"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.5 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_0_5ms_")

                    'Change the time step of the firing rate neural sim to 1 ms, physics time step to 0.5 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "1 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_1ms_PhysicsTimeStep_0_5ms_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "2 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (-10 nAA)", "Weight", "-5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "W_-5na_")

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (-5 nAA)", "Weight", "-15 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "W_-15na_")

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (-15 nAA)", "Weight", "10 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "W_10na_")

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Vth", "-50 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Vrest", "-70 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "2_Vrest_-70mv_Vth_-50mv_")

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Ih", "-1 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Ih_-1na_")

                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (10 nAA)", "Delete Link")
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeletedSynapse_")

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

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.5 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_0_5ms_")

                    'Change the time step of the firing rate neural sim to 1 ms, physics time step to 0.5 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "1 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_1ms_PhysicsTimeStep_0_5ms_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "2 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

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


                <TestMethod()>
                Public Sub Test_FiringRate_RandomNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF", 0.01)
                    aryMaxErrors.Add("Vm", 0.0001)
                    aryMaxErrors.Add("Il", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_RandomNeuron"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.5 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_0_5ms_")

                    'Change the time step of the firing rate neural sim to 1 ms, physics time step to 0.5 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "1 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "FFmodTimeStep_1ms_PhysicsTimeStep_0_5ms_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\FiringRateSim", "TimeStep", "2 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    'Set Il to -5 na
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Il", "-5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Il_-5_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Il", "0"})

                    'I cannot do the tests below because the random number results are different from VS7 to VS10. It produces different double results.
                    'Now increase burst length duration to 0.023 
                    'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "BurstLengthDistribution.C", "0.023"})
                    'RunSimulationWaitToEnd()
                    'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "BLength_023_")
                    'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "BurstLengthDistribution.C", "0.013"})

                    ''Now increase Current size to 0.3 nA
                    'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CurrentDistribution.C", "0.3 n"})
                    'RunSimulationWaitToEnd()
                    'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CDist_03n_")
                    'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "CurrentDistribution.C", "0.1 n"})

                    ''Now increase inter-burst length duration to 0.022 
                    'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "InterburstLengthDistribution.C", "0.022"})
                    'RunSimulationWaitToEnd()
                    'CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IBLength_022_")
                    'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "InterburstLengthDistribution.C", "0.012"})

                End Sub


                <TestMethod()>
                Public Sub Test_FiringRate_TonicNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("FF", 0.01)
                    aryMaxErrors.Add("Vm", 0.0001)
                    aryMaxErrors.Add("Il", 0.0000000001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "FiringRate_TonicNeuron"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "Ih", "5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Ih_5na_")

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

