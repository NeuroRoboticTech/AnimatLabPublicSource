Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.ComponentModel.Design.Serialization
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects.Behavior.SynapseTypes

    Public Class SpikingChemical
        Inherits DataObjects.Behavior.SynapseType

#Region " Attributes "

        'Basic Synaptic Properties
        Protected m_snEquilibriumPotential As AnimatGUI.Framework.ScaledNumber
        Protected m_snSynapticConductance As AnimatGUI.Framework.ScaledNumber
        Protected m_snDecayRate As AnimatGUI.Framework.ScaledNumber
        Protected m_snRelativeFacilitation As AnimatGUI.Framework.ScaledNumber
        Protected m_snFacilitationDecay As AnimatGUI.Framework.ScaledNumber

        'Voltage Dependent Properties
        Protected m_bVoltageDependent As Boolean
        Protected m_snMaxRelativeConductance As AnimatGUI.Framework.ScaledNumber
        Protected m_snSaturatePotential As AnimatGUI.Framework.ScaledNumber
        Protected m_snThresholdPotential As AnimatGUI.Framework.ScaledNumber

        'Hebbian Properties
        Protected m_bHebbian As Boolean
        Protected m_snMaxAugmentedConductance As AnimatGUI.Framework.ScaledNumber
        Protected m_fltLearningIncrement As Single
        Protected m_snLearningTimeWindow As AnimatGUI.Framework.ScaledNumber
        Protected m_bAllowForgetting As Boolean
        Protected m_snForgettingTimeWindow As AnimatGUI.Framework.ScaledNumber
        Protected m_fltConsolidationFactor As Single

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Spiking Chemical Synapse"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property EquilibriumPotential() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snEquilibriumPotential
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < -0.1 OrElse Value.ActualValue > 0.3 Then
                    Throw New System.Exception("The equilibrium potential must be between the range -100 to 300 mV.")
                End If

                SetSimData("EquilibriumPotential", Value.ValueFromScale(ScaledNumber.enumNumericScale.milli).ToString, True)
                m_snEquilibriumPotential.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property SynapticConductance() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snSynapticConductance
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 0.0001 Then
                    Throw New System.Exception("The synaptic conductance must be between the range 0 to 100 uS/size.")
                End If

                SetSimData("SynapticConductance", Value.ValueFromScale(ScaledNumber.enumNumericScale.micro).ToString, True)
                m_snSynapticConductance.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DecayRate() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snDecayRate
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0.00001 OrElse Value.ActualValue > 1.0 Then
                    Throw New System.Exception("The decay rate must be between the range 0.01 to 1000 ms.")
                End If

                SetSimData("DecayRate", Value.ValueFromScale(ScaledNumber.enumNumericScale.milli).ToString, True)
                m_snDecayRate.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property RelativeFacilitation() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snRelativeFacilitation
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 10 Then
                    Throw New System.Exception("The relative facilitation must be between the range 0 to 10.")
                End If

                SetSimData("RelativeFacilitation", Value.ActualValue.ToString, True)
                m_snRelativeFacilitation.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property FacilitationDecay() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snFacilitationDecay
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0.00001 OrElse Value.ActualValue > 1.0 Then
                    Throw New System.Exception("The facilitation decay must be between the range 0.01 to 1000 ms.")
                End If

                SetSimData("FacilitationDecay", Value.ValueFromScale(ScaledNumber.enumNumericScale.milli).ToString, True)
                m_snFacilitationDecay.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property VoltageDependent() As Boolean
            Get
                Return m_bVoltageDependent
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("VoltageDependent", Value.ToString, True)
                m_bVoltageDependent = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property MaxRelativeConductance() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snMaxRelativeConductance
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0.000001 OrElse Value.ActualValue > 0.001 Then
                    Throw New System.Exception("The maximum unblocked relative conductance must be between the range 1 to 1000 uS.")
                End If

                SetSimData("MaxRelativeConductance", Value.ValueFromScale(ScaledNumber.enumNumericScale.micro).ToString, True)
                m_snMaxRelativeConductance.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property SaturatePotential() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snSaturatePotential
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < -0.1 OrElse Value.ActualValue > 0.1 Then
                    Throw New System.Exception("The saturate post-synaptic potential must be between the range -100 to 100 mV.")
                End If

                SetSimData("SaturatePotential", Value.ValueFromScale(ScaledNumber.enumNumericScale.milli).ToString, True)
                m_snSaturatePotential.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ThresholdPotential() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snThresholdPotential
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < -0.1 OrElse Value.ActualValue > 0.1 Then
                    Throw New System.Exception("The threshold post-synaptic potential must be between the range -100 to 100 mV.")
                End If

                SetSimData("ThresholdPotential", Value.ValueFromScale(ScaledNumber.enumNumericScale.milli).ToString, True)
                m_snThresholdPotential.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Hebbian() As Boolean
            Get
                Return m_bHebbian
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("Hebbian", Value.ToString, True)
                m_bHebbian = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property MaxAugmentedConductance() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snMaxAugmentedConductance
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 0.001 Then
                    Throw New System.Exception("The maximum augmented conductance must be between the range 0 to 1000 uS.")
                End If

                SetSimData("MaxAugmentedConductance", Value.ValueFromScale(ScaledNumber.enumNumericScale.micro).ToString, True)
                m_snMaxAugmentedConductance.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LearningIncrement() As Single
            Get
                Return m_fltLearningIncrement
            End Get
            Set(ByVal Value As Single)
                If Value < 0.001 OrElse Value > 100.0 Then
                    Throw New System.Exception("The learning increment must be between the range 0.001 to 100 %.")
                End If

                SetSimData("LearningIncrement", Value.ToString, True)
                m_fltLearningIncrement = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LearningTimeWindow() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snLearningTimeWindow
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0.001 OrElse Value.ActualValue > 1.0 Then
                    Throw New System.Exception("The learning time window must be between the range 1 to 1000 msec.")
                End If

                SetSimData("LearningTimeWindow", Value.ValueFromScale(ScaledNumber.enumNumericScale.milli).ToString, True)
                m_snLearningTimeWindow.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property AllowForgetting() As Boolean
            Get
                Return m_bAllowForgetting
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("AllowForgetting", Value.ToString, True)
                m_bAllowForgetting = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ForgettingTimeWindow() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snForgettingTimeWindow
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 10000.0 Then
                    Throw New System.Exception("The forgetting time window must be between the range 0 to 10000000 msec.")
                End If

                SetSimData("ForgettingTimeWindow", Value.ValueFromScale(ScaledNumber.enumNumericScale.milli).ToString, True)
                m_snForgettingTimeWindow.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ConsolidationFactor() As Single
            Get
                Return m_fltConsolidationFactor
            End Get
            Set(ByVal Value As Single)
                If Value < 1 OrElse Value > 1000 Then
                    Throw New System.Exception("The consolidation factor must be between the range 1 to 1000.")
                End If

                SetSimData("ConsolidationFactor", Value.ToString, True)
                m_fltConsolidationFactor = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property SynapseType() As Integer
            Get
                Return 0 'For Spiking Chemical synapse
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "IntegrateFireGUI.SpikingSynapse_Treeview.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            'Basic Synaptic Properties
            m_snEquilibriumPotential = New AnimatGUI.Framework.ScaledNumber(Me, "EquilibriumPotential", -10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snSynapticConductance = New AnimatGUI.Framework.ScaledNumber(Me, "SynapticConductance", 0.5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.micro, "Siemens", "S")
            m_snDecayRate = New AnimatGUI.Framework.ScaledNumber(Me, "DecayRate", 3, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            m_snRelativeFacilitation = New AnimatGUI.Framework.ScaledNumber(Me, "RelativeFacilitation", 1.5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snFacilitationDecay = New AnimatGUI.Framework.ScaledNumber(Me, "FacilitationDecay", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")

            'Voltage Dependent Properties
            m_bVoltageDependent = False
            m_snMaxRelativeConductance = New AnimatGUI.Framework.ScaledNumber(Me, "MaxRelativeConductance", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.micro, "Siemens", "S")
            m_snSaturatePotential = New AnimatGUI.Framework.ScaledNumber(Me, "SaturatePotential", -30, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snThresholdPotential = New AnimatGUI.Framework.ScaledNumber(Me, "ThresholdPotentia", -60, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")

            'Hebbian Properties
            m_bHebbian = False
            m_snMaxAugmentedConductance = New AnimatGUI.Framework.ScaledNumber(Me, "MaxAugmentedConductance", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.micro, "Siemens", "S")
            m_fltLearningIncrement = 0.1
            m_snLearningTimeWindow = New AnimatGUI.Framework.ScaledNumber(Me, "LearningTimeWindow", 20, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            m_bAllowForgetting = True
            m_snForgettingTimeWindow = New AnimatGUI.Framework.ScaledNumber(Me, "ForgettingTimeWindow", 10000, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            m_fltConsolidationFactor = 1

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("IntegrateFireGUI")

            Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "IntegrateFireGUI.SpikingSynapse.gif", False)
            Me.Name = "Spiking Chemical Synapse"

            Me.Font = New Font("Arial", 12)
            Me.Description = "Adds a spiking chemical synapse between two neurons. A spiking chemical synapse is " & _
                            "modelled as a sudden increase in post-synaptic conductance which occurs when, and only when, the pre-synaptic neuron spikes."

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewLink As New SpikingChemical(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim blOrig As SynapseTypes.SpikingChemical = DirectCast(doOriginal, SynapseTypes.SpikingChemical)

            'Basic Synaptic Properties
            m_snEquilibriumPotential = DirectCast(blOrig.m_snEquilibriumPotential.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snSynapticConductance = DirectCast(blOrig.m_snSynapticConductance.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snDecayRate = DirectCast(blOrig.m_snDecayRate.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snRelativeFacilitation = DirectCast(blOrig.m_snRelativeFacilitation.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snFacilitationDecay = DirectCast(blOrig.m_snFacilitationDecay.Clone(Me, bCutData, doRoot), ScaledNumber)

            'Voltage Dependent Properties
            m_bVoltageDependent = blOrig.m_bVoltageDependent
            m_snMaxRelativeConductance = DirectCast(blOrig.m_snMaxRelativeConductance.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snSaturatePotential = DirectCast(blOrig.m_snSaturatePotential.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snThresholdPotential = DirectCast(blOrig.m_snThresholdPotential.Clone(Me, bCutData, doRoot), ScaledNumber)

            'Hebbian Properties
            m_bHebbian = blOrig.m_bHebbian
            m_snMaxAugmentedConductance = DirectCast(blOrig.m_snMaxAugmentedConductance.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_fltLearningIncrement = blOrig.m_fltLearningIncrement
            m_snLearningTimeWindow = DirectCast(blOrig.m_snLearningTimeWindow.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_bAllowForgetting = blOrig.m_bAllowForgetting
            m_snForgettingTimeWindow = DirectCast(blOrig.m_snForgettingTimeWindow.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_fltConsolidationFactor = blOrig.m_fltConsolidationFactor

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("SynapseType")
            oXml.IntoElem()

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Type", "SpikingChemical")
            oXml.AddChildElement("Equil", m_snEquilibriumPotential.ValueFromScale(ScaledNumber.enumNumericScale.milli))
            oXml.AddChildElement("SynAmp", m_snSynapticConductance.ValueFromScale(ScaledNumber.enumNumericScale.micro))
            oXml.AddChildElement("Decay", m_snDecayRate.ValueFromScale(ScaledNumber.enumNumericScale.milli))
            oXml.AddChildElement("RelFacil", m_snRelativeFacilitation.ActualValue)
            oXml.AddChildElement("FacilDecay", m_snFacilitationDecay.ValueFromScale(ScaledNumber.enumNumericScale.milli))
            oXml.AddChildElement("VoltDep", m_bVoltageDependent)
            oXml.AddChildElement("MaxRelCond", m_snMaxRelativeConductance.ValueFromScale(ScaledNumber.enumNumericScale.micro))
            oXml.AddChildElement("SatPSPot", m_snSaturatePotential.ValueFromScale(ScaledNumber.enumNumericScale.milli))
            oXml.AddChildElement("ThreshPSPot", m_snThresholdPotential.ValueFromScale(ScaledNumber.enumNumericScale.milli))
            oXml.AddChildElement("Hebbian", m_bHebbian)
            oXml.AddChildElement("MaxAugCond", m_snMaxAugmentedConductance.ValueFromScale(ScaledNumber.enumNumericScale.micro))
            oXml.AddChildElement("LearningInc", m_fltLearningIncrement)
            oXml.AddChildElement("LearningTime", m_snLearningTimeWindow.ValueFromScale(ScaledNumber.enumNumericScale.milli))
            oXml.AddChildElement("AllowForget", m_bAllowForgetting)
            oXml.AddChildElement("ForgetTime", m_snForgettingTimeWindow.ValueFromScale(ScaledNumber.enumNumericScale.milli))
            oXml.AddChildElement("Consolidation", m_fltConsolidationFactor)

            oXml.OutOfElem() 'Outof Neuron

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            ''Now lets add the properties for this neuron
            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snEquilibriumPotential.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Equilibrium Potential", pbNumberBag.GetType(), "EquilibriumPotential", _
                                        "Synapse Properties", "Sets the equilibrium (reversal) potential for this synaptic type. " & _
                                        "Acceptable values are in the range -100 to 300 mV", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSynapticConductance.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Synaptic Conductance", pbNumberBag.GetType(), "SynapticConductance", _
                                        "Synapse Properties", "Sets the amplitude of the post-synaptic conductance change which this synapse mediates. " & _
                                        "Acceptable values are in the range 0 to 100 uS/size.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snDecayRate.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Decay Rate", pbNumberBag.GetType(), "DecayRate", _
                                        "Synapse Properties", "Sets the time constant for the decay rate of the synaptic conductance. " & _
                                        "When a synapse is activated there is an initial step-increase in conductance followed by an " & _
                                        "exponential decline back to zero with a time constant set by this Decay rate parameter. " & _
                                        "Acceptable values are in the range 0.01 to 1000 ms.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snRelativeFacilitation.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Relative Facilitation", pbNumberBag.GetType(), "RelativeFacilitation", _
                                        "Synapse Properties", "Sets the relative facilitation rate of the synapse. A rate greater than 1 means that the " & _
                                        "synapse post-synaptic conductance tends to increase with high-frequency repetitive activation (facilitation), " & _
                                        "while a facilitation rate less than 1 means that the post-synaptic conductance tends to decrease with " & _
                                        "repetitive activation (decrement). Acceptable values are in the range0 to 10.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snFacilitationDecay.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Facilitation Decay", pbNumberBag.GetType(), "FacilitationDecay", _
                                        "Synapse Properties", "Sets the time constant of the rate of decay of facilitation. A long time constant " & _
                                        "means that the synapse shows facilitation or decrement at relatively low frequencies of activation, " & _
                                        "while a short time constant means that the synapse only facilitates or decrements with high frequency activation. " & _
                                        "Acceptable values are in the range 0.01 to 1000 ms.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Voltage Dependent", m_bVoltageDependent.GetType(), "VoltageDependent", _
                                        "Voltage Dependent Properties", "If True it makes the synaptic type voltage dependent (i.e. the conductance depends on the " & _
                                        "post-synaptic membrane potential)", m_bVoltageDependent))

            pbNumberBag = m_snMaxRelativeConductance.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Relative Conductance", pbNumberBag.GetType(), "MaxRelativeConductance", _
                                        "Voltage Dependent Properties", "Sets  the amplitude of the maximum synaptic conductance as a multiple of the Baseline conductance. " & _
                                        "Acceptable values are in the range 1 to 1000.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSaturatePotential.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Saturate Potential", pbNumberBag.GetType(), "SaturatePotential", _
                                        "Voltage Dependent Properties", "Sets the post-synaptic membrane potential at which all voltage-dependent post-synaptic " & _
                                        "conductance block is fully removed, so that the conductance becomes equal to the Baseline conductance multiplied " & _
                                        "by the Maximum relative conductance. Acceptable values are in the range -100 to 100 mV.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snThresholdPotential.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Threshold Potential", pbNumberBag.GetType(), "ThresholdPotential", _
                                        "Voltage Dependent Properties", "Sets the post-synaptic membrane potential at which the voltage-dependent " & _
                                        "post-synaptic conductance block starts to be removed. Acceptable values are in the range -100 to 100 mV.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Hebbian", m_bHebbian.GetType(), "Hebbian", _
                                        "Hebbian Properties", "make the synapse Hebbian. This means that the conductance of the " & _
                                        "synapse increases when the post-synaptic neuron spikes within a certain time-window " & _
                                        "of the pre-synaptic input.", m_bHebbian))

            pbNumberBag = m_snMaxAugmentedConductance.Properties
            propTable.Properties.Add(New PropertySpec("Max Augmented Conductance", pbNumberBag.GetType(), "MaxAugmentedConductance", _
                                        "Hebbian Properties", "Sets the maximum synaptic conductance of the fully augmented (trained) synapse " & _
                                        "Acceptable values are in the range 0 to 1000 uS/size.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Learning Increment", m_fltLearningIncrement.GetType(), "LearningIncrement", _
                                        "Hebbian Properties", "Sets the relative amount by which the synaptic strength is augmented when the post-synaptic " & _
                                        "neuron spikes immediately after a Hebbian pre-synaptic input. " & _
                                        "Acceptable values are in the range 0.001 to 100%", m_fltLearningIncrement))

            pbNumberBag = m_snLearningTimeWindow.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Learning Time Window", pbNumberBag.GetType(), "LearningTimeWindow", _
                                        "Hebbian Properties", "Sets he time-window for which a post-synaptic neuron 'remembers' a Hebbian pre-synaptic input. " & _
                                        "Acceptable values are in the range 1 to 1000 msec.", pbNumberBag, "", _
                                        GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Allow Forgetting", m_bAllowForgetting.GetType(), "AllowForgetting", _
                                        "Hebbian Properties", "If True it the synapse 'forgets' its training unless it is periodically reinforced. " & _
                                        "If False then training is a one-way process; a trained synapse maintains its augmented " & _
                                        "conductance state permanently.", m_bAllowForgetting))

            pbNumberBag = m_snForgettingTimeWindow.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Forgetting Time Window", pbNumberBag.GetType(), "ForgettingTimeWindow", _
                                        "Hebbian Properties", "Sets the baseline of the time window during which reinforcement of training must " & _
                                        "occur. After a Hebbian synaptic augmentation, the synaptic conductance declines linearly back to its baseline " & _
                                        "value during the course of the forgetting time window. " & _
                                        "Acceptable values are in the range 0 to 10000000 msec.", pbNumberBag, "", _
                                        GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Consolidation Factor", m_fltConsolidationFactor.GetType(), "ConsolidationFactor", _
                                        "Hebbian Properties", "If this factor is greater than 1, then well-trained synapses forget more slowly than poorly trained synapses. " & _
                                        "Acceptable values are in the range 1 to 1000.", m_fltConsolidationFactor))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snEquilibriumPotential Is Nothing Then m_snEquilibriumPotential.ClearIsDirty()
            If Not m_snSynapticConductance Is Nothing Then m_snSynapticConductance.ClearIsDirty()
            If Not m_snDecayRate Is Nothing Then m_snDecayRate.ClearIsDirty()
            If Not m_snRelativeFacilitation Is Nothing Then m_snRelativeFacilitation.ClearIsDirty()
            If Not m_snFacilitationDecay Is Nothing Then m_snFacilitationDecay.ClearIsDirty()
            If Not m_snMaxRelativeConductance Is Nothing Then m_snMaxRelativeConductance.ClearIsDirty()
            If Not m_snSaturatePotential Is Nothing Then m_snSaturatePotential.ClearIsDirty()
            If Not m_snThresholdPotential Is Nothing Then m_snThresholdPotential.ClearIsDirty()
            If Not m_snMaxAugmentedConductance Is Nothing Then m_snMaxAugmentedConductance.ClearIsDirty()
            If Not m_snLearningTimeWindow Is Nothing Then m_snLearningTimeWindow.ClearIsDirty()
            If Not m_snForgettingTimeWindow Is Nothing Then m_snForgettingTimeWindow.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            Try
                MyBase.LoadData(oXml)

                oXml.IntoElem()

                'Basic Synaptic Properties
                m_snEquilibriumPotential.LoadData(oXml, "EquilibriumPotential")
                m_snSynapticConductance.LoadData(oXml, "SynapticConductance")
                m_snDecayRate.LoadData(oXml, "DecayRate")
                m_snRelativeFacilitation.LoadData(oXml, "RelativeFacilitation")
                m_snFacilitationDecay.LoadData(oXml, "FacilitationDecay")

                'Voltage Dependent Properties
                m_bVoltageDependent = oXml.GetChildBool("VoltageDependent")
                m_snMaxRelativeConductance.LoadData(oXml, "MaxRelativeConductance")
                m_snSaturatePotential.LoadData(oXml, "SaturatePotential")
                m_snThresholdPotential.LoadData(oXml, "ThresholdPotential")

                'Hebbian Properties
                m_bHebbian = oXml.GetChildBool("Hebbian")
                m_snMaxAugmentedConductance.LoadData(oXml, "MaxAugmentedConductance")
                m_fltLearningIncrement = oXml.GetChildFloat("LearningIncrement")
                m_snLearningTimeWindow.LoadData(oXml, "LearningTimeWindow")
                m_bAllowForgetting = oXml.GetChildBool("AllowForgetting")
                m_snForgettingTimeWindow.LoadData(oXml, "ForgettingTimeWindow")
                m_fltConsolidationFactor = oXml.GetChildFloat("ConsolidationFactor")

                oXml.OutOfElem()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            Try
                MyBase.SaveData(oXml)

                oXml.IntoElem() 'Into Node Element

                'Basic Synaptic Properties
                m_snEquilibriumPotential.SaveData(oXml, "EquilibriumPotential")
                m_snSynapticConductance.SaveData(oXml, "SynapticConductance")
                m_snDecayRate.SaveData(oXml, "DecayRate")
                m_snRelativeFacilitation.SaveData(oXml, "RelativeFacilitation")
                m_snFacilitationDecay.SaveData(oXml, "FacilitationDecay")

                'Voltage Dependent Properties
                oXml.AddChildElement("VoltageDependent", m_bVoltageDependent)
                m_snMaxRelativeConductance.SaveData(oXml, "MaxRelativeConductance")
                m_snSaturatePotential.SaveData(oXml, "SaturatePotential")
                m_snThresholdPotential.SaveData(oXml, "ThresholdPotential")

                'Hebbian Properties
                oXml.AddChildElement("Hebbian", m_bHebbian)
                m_snMaxAugmentedConductance.SaveData(oXml, "MaxAugmentedConductance")
                oXml.AddChildElement("LearningIncrement", m_fltLearningIncrement)
                m_snLearningTimeWindow.SaveData(oXml, "LearningTimeWindow")
                oXml.AddChildElement("AllowForgetting", m_bAllowForgetting)
                m_snForgettingTimeWindow.SaveData(oXml, "ForgettingTimeWindow")
                oXml.AddChildElement("ConsolidationFactor", m_fltConsolidationFactor)

                oXml.OutOfElem() ' Outof Node Element
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#End Region

    End Class

End Namespace
