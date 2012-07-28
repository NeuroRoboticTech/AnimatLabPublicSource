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

Namespace UITests
    Namespace BehavioralEditorTests

        <CodedUITest()>
        Public MustInherit Class NeuralUITest
            Inherits AnimatUITest

#Region "Attributes"

            Protected m_strPartType As String = "Box"
            Protected m_strSecondaryPartType As String = ""
            Protected m_strJointType As String = "Hinge"

            Protected m_strAddArmPath As String = ""
            'Protected m_ptAddArmClick As New Point(751, 362)
            Protected m_ptZoomStart As New Point(877, 100)
            Protected m_iZoom1 As Integer = 300
            Protected m_iZoom2 As Integer = 300

            Protected m_strAddRootAttach As String = ""
            Protected m_strAddArmAttach As String = ""

            Protected m_ptRotateArmStart As New Point(100, 420)
            Protected m_iRotateArm1 As Integer = 700
            Protected m_iRotateArm2 As Integer = 700

            Protected m_ptRotateArm2Start As New Point(1196, 135)
            Protected m_ptRotateArm2End As New Point(1064, 288)

            Protected m_ptTranslateZAxisStart As New Point(781, 642)  'Point(790, 634)
            Protected m_ptTranslateZAxisEnd As New Point(700, 725) 'Point(741, 669)

            Protected m_ptRotateJoint1Start As New Point(687, 428)
            Protected m_ptRotatejoint1End As New Point(652, 608)

            Protected m_ptMoveJoint1Start As New Point(687, 428)
            Protected m_ptMovejoint1End As New Point(652, 608)

            Protected m_ptTransJointYAxisStart As New Point(790, 634)
            Protected m_ptTransJointYAxisEnd As New Point(741, 669)

            Protected m_strInitialJointXPos As String = "-0.125"
            Protected m_strInitialJointYPos As String = "0"
            Protected m_strInitialJointZPos As String = "0"

            Protected m_strInitialJointXRot As String = "90"
            Protected m_strInitialJointYRot As String = "0"
            Protected m_strInitialJointZRot As String = "90"

#End Region

#Region "Properties"

#End Region

#Region "Methods"

            <TestMethod()>
            Public Sub Test_Neuron_Synapse_UI()

                m_strStructureGroup = "Organisms"
                m_strStruct1Name = "Organism_1"

                m_strAddArmPath = "Simulation\Environment\" & m_strStructureGroup & _
                                   "\" & m_strStruct1Name & "\Body Plan\Root"

                m_strAddRootAttach = "Simulation\Environment\" & m_strStructureGroup & _
                                   "\" & m_strStruct1Name & "\Body Plan\Root"
                m_strAddArmAttach = "Simulation\Environment\" & m_strStructureGroup & _
                                   "\" & m_strStruct1Name & "\Body Plan\Root"

                StartNewProject()

                OpenRootBehavioralSubsystem()

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "IntegrateFireGUI.DataObjects.Behavior.Neurons.NonSpiking", New Point(100, 100), "A")

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "IntegrateFireGUI.DataObjects.Behavior.Neurons.NonSpiking", New Point(200, 100), "B")

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\B", "2", _
                          "Synapses Classes\Non-Spiking Chemical Synapses\Hyperpolarising IPSP", True, True)

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\B", "2", _
                          "Synapses Classes\Non-Spiking Chemical Synapses\Hyperpolarising IPSP", True)

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\B", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A", "3", _
                          "Synapses Classes\Electrical Synapses\Rectifying Synapse", True)


                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "IntegrateFireGUI.DataObjects.Behavior.Neurons.Spiking", New Point(100, 150), "C")

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "IntegrateFireGUI.DataObjects.Behavior.Neurons.Spiking", New Point(200, 150), "D")

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\C", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\D", "2", _
                          "Synapses Classes\Spiking Chemical Synapses\NMDA type", True)

                If Math.Abs(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\D\C (2)", "SynapticConductance.ActualValue"), Double) - 0.0000001) > 0.000001 Then
                    Throw New System.Exception("Incorrect synaptic conductance for spiking synapse.")
                End If

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\C", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\D", "3", _
                          "Synapses Classes\Non-Spiking Chemical Synapses\Hyperpolarising IPSP", True)

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\D", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\C", "4", _
                          "Synapses Classes\Electrical Synapses\Rectifying Synapse", True)


                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "FiringRateGUI.DataObjects.Behavior.Neurons.Bistable", New Point(100, 200), "A1")

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "FiringRateGUI.DataObjects.Behavior.Neurons.Normal", New Point(200, 200), "B1")

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "FiringRateGUI.DataObjects.Behavior.Neurons.Pacemaker", New Point(300, 200), "C1")

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "FiringRateGUI.DataObjects.Behavior.Neurons.Random", New Point(400, 200), "D1")

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "FiringRateGUI.DataObjects.Behavior.Neurons.Tonic", New Point(500, 200), "E1")

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A1", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\B1", "3", _
                          "Normal Synapse", False)

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\C1", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\B1", "4", _
                          "Gated Synapse", False)

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\D1", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\B1", "5", _
                          "Modulatory Synapse", False)

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A1", "6", _
                          "", False)

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "AnimatGUI.DataObjects.Behavior.Nodes.OffPage", New Point(100, 300), "OP")

                AddBehavioralNode("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem, _
                          "AnimatGUI.DataObjects.Behavior.Nodes.Joint", New Point(200, 300), "HJ")

                ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\OP", _
                          "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A"})

                'Select the simulation window tab so it is visible now.
                ExecuteMethod("SelectWorkspaceTabPage", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name}, 1000)

                'Create the test armature.
                CreateArmature(m_strPartType, m_strSecondaryPartType, m_strJointType, _
                               m_strAddArmPath, m_ptZoomStart, m_iZoom1, m_iZoom2, _
                                False, "Attachment", m_strAddRootAttach, m_strAddArmAttach)

                ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\" & m_strStructureGroup & _
                           "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\HJ", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                           "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1"})

                ExecuteMethod("SetLinkedItem", New Object() {"Simulation\Environment\" & m_strStructureGroup & _
                           "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\OP", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A"})

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\HJ", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\C", "7", _
                          "", False)

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\C", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\HJ", "8", _
                          "", False)

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\OP", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A1", "9", _
                          "", False)

                AddBehavioralLink("Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\A1", _
                           "Simulation\Environment\" & m_strStructureGroup & _
                          "\" & m_strStruct1Name & "\Behavioral System\" & m_strRootNeuralSystem & "\OP", "10", _
                          "", False)

                'Add subsystem.
                AddBehavioralNode("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem", _
                                  "AnimatGUI.DataObjects.Behavior.Nodes.Subsystem", New Point(300, 300), "S2")

                ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem"}, 2000)
                ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\C", False})
                ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\D", True})
                ExecuteMethod("SelectWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\D\C (2)", True})
                ClickMenuItem("CopyToolStripMenuItem", True)
                ExecuteMethod("DblClickWorkspaceItem", New Object() {"Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2"}, 2000)
                ClickMenuItem("PasteInPlaceToolStripMenuItem", True)

                If Math.Abs(DirectCast(GetSimObjectProperty("Simulation\Environment\Organisms\Organism_1\Behavioral System\Neural Subsystem\S2\D\C (2)", "SynapticConductance.ActualValue"), Double) - 0.0000001) > 0.000001 Then
                    Throw New System.Exception("Incorrect synaptic conductance for copied spiking synapse.")
                End If


            End Sub

            'Protected Overrides Sub RecalculatePositionsUsingResolution()
            '    MyBase.RecalculatePositionsUsingResolution()

            '    m_ptRotateArmStart.X = CInt(m_ptRotateArmStart.X * m_dblResScaleWidth)
            '    m_ptRotateArmStart.Y = CInt(m_ptRotateArmStart.Y * m_dblResScaleHeight)

            '    m_ptRotateArm2Start.X = CInt(m_ptRotateArm2Start.X * m_dblResScaleWidth)
            '    m_ptRotateArm2Start.Y = CInt(m_ptRotateArm2Start.Y * m_dblResScaleHeight)

            '    m_ptRotateArm2End.X = CInt(m_ptRotateArm2End.X * m_dblResScaleWidth)
            '    m_ptRotateArm2End.Y = CInt(m_ptRotateArm2End.Y * m_dblResScaleHeight)

            '    m_ptAddRootAttach.X = CInt(m_ptAddRootAttach.X * m_dblResScaleWidth)
            '    m_ptAddRootAttach.Y = CInt(m_ptAddRootAttach.Y * m_dblResScaleHeight)

            '    m_ptAddArmAttach.X = CInt(m_ptAddArmAttach.X * m_dblResScaleWidth)
            '    m_ptAddArmAttach.Y = CInt(m_ptAddArmAttach.Y * m_dblResScaleHeight)

            '    m_ptTranslateZAxisStart.X = CInt(m_ptTranslateZAxisStart.X * m_dblResScaleWidth)
            '    m_ptTranslateZAxisStart.Y = CInt(m_ptTranslateZAxisStart.Y * m_dblResScaleHeight)

            '    m_ptTranslateZAxisEnd.X = CInt(m_ptTranslateZAxisEnd.X * m_dblResScaleWidth)
            '    m_ptTranslateZAxisEnd.Y = CInt(m_ptTranslateZAxisEnd.Y * m_dblResScaleHeight)

            '    m_ptRotateJoint1Start.X = CInt(m_ptRotateJoint1Start.X * m_dblResScaleWidth)
            '    m_ptRotateJoint1Start.Y = CInt(m_ptRotateJoint1Start.Y * m_dblResScaleHeight)

            '    m_ptRotatejoint1End.X = CInt(m_ptRotatejoint1End.X * m_dblResScaleWidth)
            '    m_ptRotatejoint1End.Y = CInt(m_ptRotatejoint1End.Y * m_dblResScaleHeight)

            '    m_ptMoveJoint1Start.X = CInt(m_ptMoveJoint1Start.X * m_dblResScaleWidth)
            '    m_ptMoveJoint1Start.Y = CInt(m_ptMoveJoint1Start.Y * m_dblResScaleHeight)

            '    m_ptMovejoint1End.X = CInt(m_ptMovejoint1End.X * m_dblResScaleWidth)
            '    m_ptMovejoint1End.Y = CInt(m_ptMovejoint1End.Y * m_dblResScaleHeight)

            '    m_ptMoveJoint1Start.X = CInt(m_ptMoveJoint1Start.X * m_dblResScaleWidth)
            '    m_ptMoveJoint1Start.Y = CInt(m_ptMoveJoint1Start.Y * m_dblResScaleHeight)

            '    m_ptTransJointYAxisStart.X = CInt(m_ptTransJointYAxisStart.X * m_dblResScaleWidth)
            '    m_ptTransJointYAxisEnd.Y = CInt(m_ptTransJointYAxisEnd.Y * m_dblResScaleHeight)

            'End Sub


            Protected Overrides Sub RepositionChildPart()

                'ZoomInOnPart(m_ptRotateArmStart, m_iRotateArm1, m_iRotateArm2, False, MouseButtons.Left)

                'DragMouse(m_ptRotateArm2Start, m_ptRotateArm2End, MouseButtons.Left, ModifierKeys.None, True)

                'Set the root part to be frozen.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Freeze", "True"})

                'Resize the root part and graphic.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Height", "0.2"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Width", "0.05"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root", "Length", "0.05"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Height", "0.2"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Width", "0.05"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Root_Graphics", "Length", "0.05"})

                'Resize the child part and graphic.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "Height", "0.05"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "Width", "0.05"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "Length", "0.2"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\Arm_Graphics", "Height", "0.05"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\Arm_Graphics", "Width", "0.05"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm\Arm_Graphics", "Length", "0.2"})

                'Reposition the child part relative to the parent part
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "LocalPosition.X", "0.125"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "LocalPosition.Y", "-0.075"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1\Arm", "LocalPosition.Z", "0"})

                'Reposition the joint relative to the parent part
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "LocalPosition.X", m_strInitialJointXPos})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "LocalPosition.Y", m_strInitialJointYPos})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "LocalPosition.Z", m_strInitialJointZPos})

                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.X", m_strInitialJointXRot})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.Y", m_strInitialJointYRot})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_1", "Rotation.Z", m_strInitialJointZRot})

            End Sub

            Protected Overrides Sub RepositionBlockerPart()
                'Resize the child part and graphic.
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "Height", "0.05"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "Width", "0.05"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "Length", "0.05"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker\Blocker_Graphics", "Height", "0.05"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker\Blocker_Graphics", "Width", "0.05"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker\Blocker_Graphics", "Length", "0.05"})

                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.X", "0"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Y", "0.125"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2\Blocker", "LocalPosition.Z", "0"})

                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2", "LocalPosition.X", "0"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2", "LocalPosition.Y", "0"})
                ExecuteIndirectMethod("SetObjectProperty", New Object() {"Simulation\Environment\" & m_strStructureGroup & "\" & m_strStruct1Name & "\Body Plan\Root\Joint_2", "LocalPosition.Z", "0"})
            End Sub

#End Region

        End Class

    End Namespace
End Namespace


