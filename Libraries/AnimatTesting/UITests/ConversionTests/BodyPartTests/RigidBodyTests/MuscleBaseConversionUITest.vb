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
                Public Class MuscleBaseConversionUITest
                    Inherits ConversionUITest

#Region "Attributes"


#End Region

#Region "Properties"

#End Region

#Region "Methods"


                    Public Overridable Sub MuscleTest()
                        Dim aryMaxErrors As New Hashtable
                        aryMaxErrors.Add("Time", 0.001)
                        aryMaxErrors.Add("MV", 0.002)
                        aryMaxErrors.Add("MN", 0.002)
                        aryMaxErrors.Add("Tension", 0.3)
                        aryMaxErrors.Add("Muscle2", 0.3)
                        aryMaxErrors.Add("Length", 0.04)
                        aryMaxErrors.Add("A", 0.3)
                        aryMaxErrors.Add("Len", 0.003)
                        aryMaxErrors.Add("default", 0.04)

                        m_bIgnoreSimAndCompare = True

                        m_strProjectPath = "\Libraries\AnimatTesting\TestProjects\ConversionTests\BodyPartTests\RigidBodyTests"
                        m_strTestDataPath = "\Libraries\AnimatTesting\TestData\ConversionTests\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strOldProjectFolder = "\Libraries\AnimatTesting\TestProjects\ConversionTests\OldVersions\BodyPartTests\RigidBodyTests\" & m_strProjectName
                        m_strStructureGroup = "Organisms"
                        m_strStruct1Name = "Organism_1"

                        m_aryWindowsToOpen.Clear()
                        m_aryWindowsToOpen.Add("Tool Viewers\BodyData")

                        'Load and convert the project.
                        TestConversionProject("AfterConversion_", aryMaxErrors)

                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "Length.ActualValue", 0.5)

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "AfterConversion_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MV_Stim", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MV_Stim_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stretch", "EndTime", "5.075"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stretch", "Velocity", "-10 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Relax", "EndTime", "8.075 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Relax", "Velocity", "10 c"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "SlowStretch_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stretch", "EndTime", "5.025 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stretch", "Velocity", "-40 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Relax", "EndTime", "8.025 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Relax", "Velocity", "40 c"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Enabled", "True"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Damping", "1 k"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "NaturalLength", "0.5 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1", "EnableMotor", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stretch", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Relax", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MV_Stim1", "Enabled", "True"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MV_Stim2", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Spring_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Spring", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1", "EnableMotor", "True"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MV_Stim1", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MV_Stim2", "Enabled", "False"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MN_Stim", "Enabled", "True"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MV_Stim", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stretch", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Relax", "Enabled", "False"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MN_Stim_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MN_Stim", "CurrentOn", "7 n"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MN_7na_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MN_Stim", "CurrentOn", "16 n"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MN_16na_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\MV\MN (0.5 uS)", "SynapticConductance", "5 u"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MN_5us_")

                        'Change the time step of the firing rate neural sim to 0.1 ms
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.1 m"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_1ms_")

                        'Change the time step of the firing rate neural sim to 0.5 ms, physics time step to 0.1 ms
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.5 m"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "0.1 m"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "IGFmodTimeStep_0_5ms_PhysicsTimeStep_0_1ms_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Neural Modules\IntegrateFireSim", "TimeStep", "0.2 m"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment", "PhysicsTimeStep", "1 m"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MN_5us_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "Kse", "1000 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Kse_1kn_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "Kse", "500 "})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "Kpe", "525 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Kpe_525_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "Kpe", "125 "})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "B", "500 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "B_500_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "B", "100 "})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "MaxTension", "5 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "MaxTen_5N_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "MaxTension", "100 "})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "LengthTension.RestingLength", "0.55 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Lrest_0_55_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "LengthTension.Lwidth", "7 c"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Lrest_0_55_Lwidth_7_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "LengthTension.Lwidth", "10 c"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "LengthTension.RestingLength", "0.5 "})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "StimulusTension.Amplitude", "1.5 k"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Amp_1_5kN_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "StimulusTension.Amplitude", "1 k"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "StimulusTension.Steepness", "200 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Steep_200_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "StimulusTension.Steepness", "150 "})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "StimulusTension.XOffset", "-35 m"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Xoff_-35mv_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "StimulusTension.XOffset", "-40 m"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "StimulusTension.YOffset", "-30 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "YOff_-30N_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "StimulusTension.YOffset", "-13 c"})

                        'Verify we can set the following properties without getting an error.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "IbDischargeConstant", "200 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "IbDischargeConstant", "100 "})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "LengthTension.PeLengthPercentage", "80 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "LengthTension.PeLengthPercentage", "90 "})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "LengthTension.MinPeLengthPercentage", "10 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "LengthTension.MinPeLengthPercentage", "5 "})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "Visible", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "Visible", "True"})

                        aryMaxErrors("Length") = 5  'Length is not reported correctly in the old version when disabled.
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "Enabled", "False"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Disabled_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "Enabled", "True"})
                        aryMaxErrors("Length") = 0.04  'Put the real length check back.

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C", "Enabled", "False"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Disabled_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C", "Enabled", "True"})

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C", "Gain.C", "1.1 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Ad_C_1_1_")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C", "Gain.C", "1 "})

                        ExecuteMethod("OpenUITypeEditor", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "AttachmentPoints"}, 500)
                        ExecuteActiveDialogMethod("Automation_AddAttachment", New Object() {"LeftAttach2"})
                        ExecuteActiveDialogMethod("Automation_AttachmentUp", New Object() {"LeftAttach2"})
                        ExecuteIndirectActiveDialogMethod("ClickOkButton", Nothing)
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "Length.ActualValue", 0.947)
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MN_Stim", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stretch", "Enabled", "True"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Relax", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "LeftAttach2_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MN_Stim", "Enabled", "True"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stretch", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Relax", "Enabled", "False"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "LengthTension.RestingLength", "0.947 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Lrest_0_947_")


                        'Test cut/copy/paste.
                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan"}, 2000)

                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", False})
                        ClickMenuItem("CopyToolStripMenuItem", True)

                        PasteChildPartTypeWithJoint("", "Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm", 0.04, 0.55, -0.5, 0.0, 0.0, -1.0, False)
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "Name", "Muscle1"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle", "Name", "Muscle2"})

                        ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Tool Viewers\BodyData"}, 1000)
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Tool Viewers\BodyData\LineChart\Tension", False})
                        AddItemToChart("Simulation\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle2")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Tool Viewers\BodyData\LineChart\Tension\Muscle2", "DataTypeID", "Tension"})
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle2", "Length.ActualValue", 0.947)
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Muscle2_")

                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\MN_Stim", "Enabled", "True"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stretch", "Enabled", "True"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Relax", "Enabled", "True"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Stretch_muscle2_")

                        DeletePart("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle1", "Delete Body Part")
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle1"})) Then
                            Throw New System.Exception("Muscle1 node was not removed correctly.")
                        End If
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Tool Viewers\BodyData\LineChart\Tension\Tension"})) Then
                            Throw New System.Exception("Muscle1 chart node was not removed correctly.")
                        End If
                        If Not GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Muscle", "LinkedPart.BodyPart") Is Nothing Then
                            Throw New System.Exception("Linked body part not removed.")
                        End If
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C"})) Then
                            Throw New System.Exception("Adapter node was not removed correctly.")
                        End If
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "Delete_Muscle1_")

                        m_bIgnoreSimAndCompare = False

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem"}, 2000)

                        AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\MV", _
                                          "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Muscle", "", "", False, , True)
                        AssertErrorDialogShown("You must specify a linked body part before you can add an adapter to this node.", enumErrorTextType.Equals)

                        ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Muscle", _
                                                                     "Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle2"})

                        AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\MV", _
                                          "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Muscle", "", "", False)
                        If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2"})) Then
                            Throw New System.Exception("A_B adapter node was not removed created.")
                        End If
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\2", "Name", "A_B"})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CreateA_B_")

                        'Add subsystem.
                        AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem", _
                                          "AnimatGUI.DataObjects.Behavior.Nodes.Subsystem", New Point(316, 30), "S2")


                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\MN", False})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\MV", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_B", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Muscle", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\MV\MN (5 uS)", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\A_B\MV", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\Muscle\A_B", True})
                        DeleteSelectedParts("Delete Group", True)
                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2"}, 2000)
                        ClickMenuItem("PasteInPlaceToolStripMenuItem", True)

                        AddStimulus("Tonic Current", "Organism_1", "\Behavioral System\Neural Subsystem\S2\MN", "Stim_MN")
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_MN", "EndTime", "20 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_MN", "StartTime", "0 "})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Stimuli\Stim_MN", "CurrentOn", "16 n"})

                        'Add these neurons to the chart.
                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Tool Viewers\BodyData"}, 2000)
                        ClickToolbarItem("AddAxisToolStripButton", True)
                        AddItemToChart("Simulation\Organism_1\Behavioral System\Neural Subsystem\Nodes\S2\Nodes\MV")

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CreateA_B_")


                        DeletePart("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\A_B", "Delete Node")
                        AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2", _
                                          "AnimatGUI.DataObjects.Behavior.Nodes.OffPage", New Point(150, 50), "OP")

                        AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\OP", _
                                          "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\Muscle", "", "", False, False, True)
                        AssertErrorDialogShown("The off-page connector node 'OP' must be associated with another node before you can connect it with a link.", enumErrorTextType.Equals)


                        ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\OP", _
                                                                     "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\MV"})

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2"}, 2000)

                        AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\OP", _
                                          "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\Muscle", "", "", False)

                        If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\5"})) Then
                            Throw New System.Exception("5 adapter was not added")
                        End If
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\5", "Name", "A_B"})

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CreateA_B_")


                        AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2", _
                                  "IntegrateFireGUI.DataObjects.Behavior.Neurons.NonSpiking", New Point(450, 100), "Len")

                        AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2", _
                                          "AnimatGUI.DataObjects.Behavior.Nodes.OffPage", New Point(450, 50), "OP2")

                        AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\Muscle", _
                                          "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\OP2", "", "", False, , True)
                        AssertErrorDialogShown("The off-page connector node 'OP2' must be associated with another node before you can connect it with a link.", enumErrorTextType.Equals)


                        ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\OP2", _
                                                                     "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\Len"})

                        AddBehavioralLink("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\Muscle", _
                                          "Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\OP2", "", "", False)

                        If Not CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\8"})) Then
                            Throw New System.Exception("8 adapter was not added")
                        End If
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\8", "Name", "B_A"})
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\B_A", "Gain.C", "1 n"})

                        ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Tool Viewers\BodyData"}, 1000)
                        ClickToolbarItem("AddAxisToolStripButton", True)
                        AddItemToChart("Simulation\Organism_1\Behavioral System\Neural Subsystem\Nodes\S2\Nodes\Len")

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CreateB_A_")

                        ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2"}, 2000)

                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\OP", False})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\A_B", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\Muscle", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\A_B\MV", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\Muscle\A_B", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\OP2", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\B_A", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\B_A\Muscle", True})
                        ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\Len\B_A", True})
                        DeleteSelectedParts("Delete Group", True)
                        ClickMenuItem("PasteInPlaceToolStripMenuItem", True)

                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "CreateB_A_")

                        SpindleTest(aryMaxErrors)

                        DeletePart("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_3\LeftSide\LeftAttach2", "Delete Body Part")
                        Threading.Thread.Sleep(1000)
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_3\LeftSide\LeftAttach2"})) Then
                            Throw New System.Exception("LeftAttach node was not removed correctly.")
                        End If
                        If DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle2", "AttachmentPoints.Count"), Integer) <> 2 Then
                            Throw New System.Exception("Attachment not removed from spring.")
                        End If
                        VerifyPropertyValue("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle2", "Length.ActualValue", 0.5)
                        ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle2", "LengthTension.RestingLength", "0.5 "})
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "LeftAttach2Remove_")

                        DeletePart("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_3\LeftSide", "Delete Body Part")
                        Threading.Thread.Sleep(1000)
                        If CBool(ExecuteDirectMethod("DoesObjectExist", New Object() {"Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_3\LeftSide"})) Then
                            Throw New System.Exception("LeftSide node was not removed correctly.")
                        End If
                        If DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Body Plan\Base\Joint_1\Arm\Muscle2", "AttachmentPoints.Count"), Integer) <> 1 Then
                            Throw New System.Exception("Attachment not removed from muscle.")
                        End If
                        RunSimulationWaitToEnd()
                        CompareSimulation(m_strRootFolder & m_strTestDataPath, aryMaxErrors, "DeleteLeftSide_")

                    End Sub

                    Protected Overridable Sub SpindleTest(ByVal aryMaxErrors As Hashtable)

                    End Sub

#End Region

                End Class

            End Namespace
        End Namespace
    End Namespace
End Namespace
