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

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 0.5] 1 (5 nA))", "Enabled", "False"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -0.5] 1 (5 nA))", "Enabled", "False"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Disabled_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 0.5] 1 (5 nA))", "Enabled", "True"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -0.5] 1 (5 nA))", "Enabled", "True"})

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
                Public Sub Test_FiringRate_ModulatedSynapse()

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

                    m_strProjectName = "FiringRate_ModulatedSynapse"
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


                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 2] 1 (5 nA))", "Enabled", "False"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -2] 1 (5 nA))", "Enabled", "False"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Disabled_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 2] 1 (5 nA))", "Enabled", "True"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -2] 1 (5 nA))", "Enabled", "True"})


                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 2] 1 (5 nA))", "Gain", "2.5"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -2] 1 (5 nA))", "Gain", "-2.5"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "W_2_5_")

                    'swap settings.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W 2.5] 1 (5 nA))", "Gain", "-2"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W -2.5] 1 (5 nA))", "Gain", "2"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SwapGates_")


                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W -2] 1 (5 nA))", _
                                                                 "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\6 (1 nA)"})
                    ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W 2] 1 (5 nA))", _
                                                                 "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\6 (1 nA)"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (5 nA)", "Enabled", "False"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\1 (5 nA)", "Enabled", "False"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_9", "Enabled", "True"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SwapLinkedSynapses_")

                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\3 ([W -2] 6 (1 nA))", "Delete Link")
                    DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\4\5 ([W 2] 6 (1 nA))", "Delete Link")
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeletedSynapses_")

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

#Region "IGF Methods"

                <TestMethod()>
                Public Sub Test_IGF_NonspikingChemicalSynapses()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.0001)
                    aryMaxErrors.Add("2", 0.0001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "IGF_NonspikingChemicalSynapses"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.1 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_1ms_")

                    'Change the time step of the firing rate neural sim to 0.5 ms, physics time step to 0.1 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.5 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_5ms_PhysicsTimeStep_0_1ms_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.2 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (A)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "EquilibriumPotential", "50 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_EqPot_50mv_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (A)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "EquilibriumPotential", "-10 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "MaxSynapticConductance", "2 u"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_MaxCond_2uS_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (A)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "MaxSynapticConductance", "0.5 u"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "PreSynapticSaturationLevel", "-50 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_SatPot_-50mv_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (A)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "PreSynapticSaturationLevel", "-20 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Nicotinic ACh type", "PreSynapticThreshold", "-35 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_ThreshPot_-35mv_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2\1 (A)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SelectItemInTreeView", New Object() {"Synapses Classes\Non-Spiking Chemical Synapses\Hyperpolarising IPSP"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "HypIPSP_")

                End Sub

                <TestMethod()>
                Public Sub Test_IGF_SpikingChemicalSynapses()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("1", 0.0001)
                    aryMaxErrors.Add("2", 0.0001)
                    aryMaxErrors.Add("3", 0.0001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "IGF_SpikingChemicalSynapses"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.1 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_1ms_")

                    'Change the time step of the firing rate neural sim to 0.5 ms, physics time step to 0.1 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.5 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_5ms_PhysicsTimeStep_0_1ms_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.2 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapticConductance", "2 u"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_2uS_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (2 uS)", "SynapticConductance", "1.5 u"})

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapticConductance", "2 u"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S2_2uS_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (2 uS)", "SynapticConductance", "0.5 u"})

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "ConductionDelay", "1.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "1_Delay_1_5_ms_")

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "ConductionDelay", "1.5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "2_Delay_1_5_ms_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "ConductionDelay", "0"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "ConductionDelay", "0"})

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "DecayRate", "5 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_Decay_5ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "DecayRate", "30 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_Decay_30ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "DecayRate", "15 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "DecayRate", "2 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S2_Decay_2ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "DecayRate", "50 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S2_Decay_50ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "EquilibriumPotential", "-30 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "DecayRate", "10 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "EquilibriumPotential", "-90 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_EqPot_-30mv_S2_EqPot_-90mv_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "EquilibriumPotential", "-10 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "FacilitationDecay", "20 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "EquilibriumPotential", "-70 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "FacilitationDecay", "300 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_FacilDecay_20ms_S2_FacilDecay_300ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "FacilitationDecay", "300 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "FacilitationDecay", "20 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_FacilDecay_300ms_S2_FacilDecay_20ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "FacilitationDecay", "100 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "RelativeFacilitation", "5"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "FacilitationDecay", "100 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "RelativeFacilitation", "5"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1S2_Facil_5_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Nicotinic ACh", "RelativeFacilitation", "0.9"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\2 (0.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hyperpolarizing IPSP", "RelativeFacilitation", "0.9"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1S2_Facil_0_9_")

                    'Voltage dependent tests
                    'Disabled Neuron 2, switch synapse 1 to NMDA type, REset stim to go from 50-60ms at 30 nA, reset chart to 0 to 200 ms.
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Enabled", "False"})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SelectItemInTreeView", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_B", "Enabled", "False"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "StartTime", "50 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "EndTime", "60 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "CurrentOn", "30 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "N2_Disabled_S1_NMDA_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.1 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type", "SaturatePotential", "-40 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_SatPot_-40mv_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.1 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type", "MaxRelativeConductance", "15 u"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_MaxCond_15us_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.1 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type", "MaxRelativeConductance", "10 u"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\Neural Subsystem\3", "Stimulus_C", "Stimulus_1")
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "StartTime", "20 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "EndTime", "170 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "CurrentOn", "5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Stim3_")

                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "CurrentOn", "-5 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Stim3_-5nA_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.1 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type", "ThresholdPotential", "-80 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_Thresh_-80mv_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.1 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type", "VoltageDependent", "False"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_NoVoltageDep_")

                    ''Only used if commenting out upper portion to test hebbian.
                    'ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Enabled", "False"})
                    'ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (1.5 uS)", "SynapseType"}, 500)
                    'ExecuteActiveDialogMethod("SelectItemInTreeView", New Object() {"Synapses Classes\Spiking Chemical Synapses\NMDA type"})
                    'ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    'AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\Neural Subsystem\3", "Stimulus_C", "Stimulus_1")
                    ''Only used if commenting out upper portion to test hebbian.


                    'Hebbian tests
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeAccomodation", "0"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3", "AHP_Conductance", "1.5 u"})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.1 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SelectItemInTreeView", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "StartTime", "10 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "EndTime", "260 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_A", "CurrentOn", "21 n"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "Enabled", "False"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_Hebbian_")

                    AddStimulus("Tonic Current", m_strStruct1Name, "\Behavioral System\Neural Subsystem\1", "Stimulus_D", "Stimulus_2")
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_D", "StartTime", "110 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_D", "EndTime", "160 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_D", "CurrentOn", "10 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_LowFreq_Increase_")

                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_D", "Enabled", "False"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "Enabled", "True"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "StartTime", "110 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "EndTime", "160 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "CurrentOn", "10 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_N2_10nA_")

                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "CurrentOn", "30 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_N2_30nA_")

                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "CurrentOn", "50 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_N2_50nA_")

                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_C", "CurrentOn", "10 n"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.4 uS)", "SynapticConductance", "0.6 u"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_Cond_0_6uS_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.6 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "MaxAugmentedConductance", "1.5 u"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_MaxAugCond_1_5uS_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.6 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "MaxAugmentedConductance", "2 u"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "LearningTimeWindow", "4 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_LearnWindow_4ms_")

                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.6 uS)", "SynapticConductance", "0.4 u"})
                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.4 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "MaxAugmentedConductance", "2 u"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "LearningTimeWindow", "30 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "LearningIncrement", "0.8"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_LearnIncr_0_8_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.4 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "ForgettingTimeWindow", "10 m"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_ForgetWindow_10ms_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.4 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "ConsolidationFactor", "1"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_Consolidation_1_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.4 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "ForgettingTimeWindow", "10 m"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "ConsolidationFactor", "20"})
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "AllowForgetting", "False"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_NoForgetting_")

                    ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\3\1 (0.4 uS)", "SynapseType"}, 500)
                    ExecuteActiveDialogMethod("SetTreeNodeObjectProperty", New Object() {"Synapses Classes\Spiking Chemical Synapses\Hebbian ACh type", "Hebbian", "False"})
                    ExecuteActiveDialogMethod("ClickOkButton", Nothing)
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "S1_NotHebbian_")


                End Sub

                <TestMethod()>
                Public Sub Test_IGF_SpikingNeuron()

                    Dim aryMaxErrors As New Hashtable
                    aryMaxErrors.Add("Time", 0.001)
                    aryMaxErrors.Add("Vm", 0.0001)
                    aryMaxErrors.Add("default", 0.0001)

                    m_strProjectName = "IGF_SpikingNeuron"
                    m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\NeuralTests"
                    m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\NeuralTests\" & m_strProjectName
                    m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\NeuralTests\" & m_strProjectName

                    'Load and convert the project.
                    TestConversionProject("AfterConversion_", aryMaxErrors)

                    'Run the same sim a second time to check for changes between sims.
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                    'Change the time step of the firing rate neural sim to 0.1 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_1ms_")

                    'Change the time step of the firing rate neural sim to 0.5 ms, physics time step to 0.1 ms
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.5 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.1 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_5ms_PhysicsTimeStep_0_1ms_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.2 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})

                    ''Change AHP Eq Pot to -90 mv: AHP_EqPot_-90mv_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "AHPEquilibriumPotential", "-90 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AHP_EqPot_-90mv_")

                    ''Change AHP Eq Pot to -70 mv, Vth=-55mv, Refractory Period=5ms: Vth_-55mv_Refr_5ms_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "InitialThreshold", "-55 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "AHPEquilibriumPotential", "-70 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "RefractoryPeriod", "5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vth_-55mv_Refr_5ms_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "RefractoryPeriod", "2 m"})

                    ''Change spike peak to 10 mv: SpikePeak_10mv_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "SpikePeak", "10 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SpikePeak_10mv_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "SpikePeak", "0 m"})

                    ''Apply TTX: TTX_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "ApplyTTX", "True"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "TTX_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "ApplyTTX", "False"})

                    ''Change Vth to -50 mv: Vth_-50mv_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "InitialThreshold", "-50 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vth_-50mv_")

                    'Vth=-50mv, Tc=50ms: Vth_-50_Tc_50ms_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "TimeConstant", "50 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vth_-50_Tc_50ms_")

                    'Vth=-50mv, Tc=50ms, Size: 0.5: Vth_-50mv_Tc_5ms_Size_0_5_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "TimeConstant", "5 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeSize", "0.5"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vth_-50mv_Tc_5ms_Size_0_5_")

                    'Vth=-50mv, Tc=50ms, Size: 1.5: Vth_-50mv_Tc_5ms_Size_1_5
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeSize", "1.5"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vth_-50mv_Tc_5ms_Size_1_5_")
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeSize", "1"})

                    'Vth=-50mv, Tc=5ms, Accom=0,7: Tc_5ms_Accom_7_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeAccomodation", "0.7"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Tc_5ms_Accom_7_")

                    'Accom=0.7, Accom Tc=5ms: Accom_7_ATc_5ms_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AccomodationTimeConstant", "5 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Accom_7_ATc_5ms_")

                    'Accom=0.7, Accom Tc=15ms: Accom_7_ATc_15ms_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AccomodationTimeConstant", "15 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Accom_7_ATc_15ms_")

                    'Accom=0.3, Accom Tc=10ms, AHP G=10uS: Accom_3_ATc_10ms_AHPG_10uS_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RelativeAccomodation", "0.3"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AccomodationTimeConstant", "10 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AHP_Conductance", "10 u"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Accom_3_ATc_10ms_AHPG_10uS_")

                    'AHP G=10uS, AHP Tc=10ms: AHPG_10uS_AHPTc_10ms_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AHP_TimeConstant", "10 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AHPG_10uS_AHPTc_10ms_")

                    'AHP G=10uS, AHP Tc=10ms, Vrest=-55mv: AHPG_10uS_AHPTc_10ms_Vrest_-55mv_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RestingPotential", "-55 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AHPG_10uS_AHPTc_10ms_Vrest_-55mv_")

                    'AHP G=1uS, AHP Tc=3ms, Vrest=-45mv: AHPG_1uS_AHPTc_3ms_Vrest_-45mv_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AHP_Conductance", "1 u"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "AHP_TimeConstant", "3 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RestingPotential", "-45 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AHPG_1uS_AHPTc_3ms_Vrest_-45mv_")

                    'Vrest=-70mv: Vrest_-70mv_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "RestingPotential", "-70 m"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Vrest_-70mv_")

                    'ITonic=10nA: Itonic_10na_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "TonicStimulus", "10 n"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Itonic_10na_")

                    'ITonic=0nA, INoise=5mV, Stim disabled: Itonic_10na_INoise_5mv_NoStim_
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "TonicStimulus", "0 n"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\1", "TonicNoise", "5 m"})
                    ExecuteMethod("SetObjectProperty", New Object() {"Stimuli\Stimulus_1", "Enabled", "False"})
                    RunSimulationWaitToEnd()
                    CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Itonic_10na_INoise_5mv_NoStim_")

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
                    SetStructureNames("1", False)

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

