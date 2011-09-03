Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects.Behavior

    Public Class NeuralModule
        Inherits AnimatGUI.DataObjects.Behavior.NeuralModule

#Region " Attributes "

        Protected m_snAHPEquilibriumPotential As AnimatGUI.Framework.ScaledNumber
        Protected m_snSpikePeak As AnimatGUI.Framework.ScaledNumber
        Protected m_fltSpikeStrength As Single
        Protected m_snCaEquilibriumPotential As AnimatGUI.Framework.ScaledNumber
        Protected m_snRefractoryPeriod As AnimatGUI.Framework.ScaledNumber
        Protected m_bUseCriticalPeriod As Boolean
        Protected m_snStartCriticalPeriod As AnimatGUI.Framework.ScaledNumber
        Protected m_snEndCriticalPeriod As AnimatGUI.Framework.ScaledNumber
        Protected m_bTTX As Boolean
        Protected m_bCd As Boolean
        Protected m_bHodgkinHuxley As Boolean

        Protected m_arySynapseTypes As New Collections.SynapseTypes(Me)

#End Region

#Region " Properties "

        <Browsable(False)> _
            Public Overrides ReadOnly Property NetworkFilename() As String
            Get
                If Not m_doOrganism Is Nothing Then
                    Return m_doOrganism.Name & ".arnn"
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides Property TimeStep() As ScaledNumber
            Get
                Return m_snTimeStep
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0.0000001 OrElse Value.ActualValue > 0.05 Then
                    Throw New System.Exception("The time step must be between the range 0.0001 to 50 ms.")
                End If

                SetSimData("TimeStep", Value.ActualValue.ToString, True)
                m_snTimeStep.CopyData(Value)
                Util.Application.SignalTimeStepChanged(Me)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property AHPEquilibriumPotential() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snAHPEquilibriumPotential
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < -0.15 OrElse Value.ActualValue > -0.01 Then
                    Throw New System.Exception("The after-hyperpolarizing equilibrium potential must be between the range -150 to -10 mV.")
                End If

                'TODO
                m_snAHPEquilibriumPotential.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property SpikePeak() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snSpikePeak
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < -0.03 OrElse Value.ActualValue > 0.05 Then
                    Throw New System.Exception("The spike peak must be between the range -30 to 50 mV.")
                End If

                'TODO
                m_snSpikePeak.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property SpikeStrength() As Single
            Get
                Return m_fltSpikeStrength
            End Get
            Set(ByVal Value As Single)
                If Value < 1 OrElse Value > 1000 Then
                    Throw New System.Exception("The spike strength must be between the range 1 to 1000.")
                End If

                'TODO
                m_fltSpikeStrength = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property CaEquilibriumPotential() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snCaEquilibriumPotential
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < -0.1 OrElse Value.ActualValue > 0.5 Then
                    Throw New System.Exception("The calcium equilibrium potential must be between the range -100 to 500 mV.")
                End If

                'TODO
                m_snCaEquilibriumPotential.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property RefractoryPeriod() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snRefractoryPeriod
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0.001 OrElse Value.ActualValue > 0.05 Then
                    Throw New System.Exception("The absolute refractory period must be between the range 1 to 50 ms.")
                End If

                'TODO
                m_snRefractoryPeriod.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property UseCriticalPeriod() As Boolean
            Get
                Return m_bUseCriticalPeriod
            End Get
            Set(ByVal Value As Boolean)
                'TODO
                m_bUseCriticalPeriod = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ApplyTTX() As Boolean
            Get
                Return m_bTTX
            End Get
            Set(ByVal Value As Boolean)
                'TODO
                m_bTTX = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ApplyCd() As Boolean
            Get
                Return m_bCd
            End Get
            Set(ByVal Value As Boolean)
                'TODO
                m_bCd = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property HodgkinHuxley() As Boolean
            Get
                Return m_bHodgkinHuxley
            End Get
            Set(ByVal Value As Boolean)
                'TODO
                m_bHodgkinHuxley = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property StartCriticalPeriod() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snStartCriticalPeriod
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 1000 Then
                    Throw New System.Exception("The critical period start time must be between the range 0 to 10000 s.")
                End If

                If Value.ActualValue >= m_snEndCriticalPeriod.ActualValue Then
                    Throw New System.Exception("The critical period start time must be less than the end time.")
                End If

                'TODO
                m_snStartCriticalPeriod.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property EndCriticalPeriod() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snStartCriticalPeriod
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 1000 Then
                    Throw New System.Exception("The critical period end time must be between the range 0 to 10000 s.")
                End If

                If Value.ActualValue <= m_snStartCriticalPeriod.ActualValue Then
                    Throw New System.Exception("The critical period end time must be greater than the start time.")
                End If

                'TODO
                m_snEndCriticalPeriod.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property SynapseTypes() As Collections.SynapseTypes
            Get
                Return m_arySynapseTypes
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property ModuleFilename() As String
            Get
#If Not Debug Then
                Return "IntegrateFireSim_VC10.dll"
#Else
                Return "IntegrateFireSim_VC10D.dll"
#End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_strModuleName = "IntegrateFireSim"
            m_strModuleType = "IntegrateFireSimModule"

            m_snTimeStep = New AnimatGUI.Framework.ScaledNumber(Me, "TimeStep", 0.2, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            m_snAHPEquilibriumPotential = New AnimatGUI.Framework.ScaledNumber(Me, "AHPEquilibriumPotential", -70, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snSpikePeak = New AnimatGUI.Framework.ScaledNumber(Me, "SpikePeak", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_fltSpikeStrength = 1
            m_snCaEquilibriumPotential = New AnimatGUI.Framework.ScaledNumber(Me, "CaEquilibriumPotential", 200, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            m_snRefractoryPeriod = New AnimatGUI.Framework.ScaledNumber(Me, "RefractoryPeriod", 2, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            m_bUseCriticalPeriod = False
            m_snStartCriticalPeriod = New AnimatGUI.Framework.ScaledNumber(Me, "StartCriticalPeriod", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_snEndCriticalPeriod = New AnimatGUI.Framework.ScaledNumber(Me, "EndCriticalPeriod", 5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_bTTX = False
            m_bCd = False
            m_bHodgkinHuxley = False

            'Now lets create the default link types.
            Dim slSynapse As DataObjects.Behavior.SynapseType

            Dim scSynapse As DataObjects.Behavior.SynapseTypes.SpikingChemical
            slSynapse = New DataObjects.Behavior.SynapseTypes.SpikingChemical(doParent)
            slSynapse.ArrowDestination.Style = AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Fork
            slSynapse.Name = "Nicotinic ACh"
            m_arySynapseTypes.Add(slSynapse.ID, slSynapse, False)

            slSynapse = New DataObjects.Behavior.SynapseTypes.SpikingChemical(doParent)
            scSynapse = DirectCast(slSynapse, DataObjects.Behavior.SynapseTypes.SpikingChemical)
            scSynapse.ArrowDestination.Style = AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Circle
            scSynapse.ArrowDestination.Filled = True
            scSynapse.Name = "Hyperpolarizing IPSP"
            scSynapse.EquilibriumPotential.ValueManual = -70
            scSynapse.DecayRate.ValueManual = 10
            m_arySynapseTypes.Add(slSynapse.ID, slSynapse, False)

            slSynapse = New DataObjects.Behavior.SynapseTypes.SpikingChemical(doParent)
            scSynapse = DirectCast(slSynapse, DataObjects.Behavior.SynapseTypes.SpikingChemical)
            scSynapse.ArrowDestination.Style = AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Circle
            scSynapse.ArrowDestination.Filled = False
            scSynapse.Name = "Depolarizing IPSP"
            scSynapse.EquilibriumPotential.ValueManual = -55
            scSynapse.RelativeFacilitation.ValueManual = 1
            m_arySynapseTypes.Add(slSynapse.ID, slSynapse, False)

            slSynapse = New DataObjects.Behavior.SynapseTypes.SpikingChemical(doParent)
            scSynapse = DirectCast(slSynapse, DataObjects.Behavior.SynapseTypes.SpikingChemical)
            scSynapse.ArrowDestination.Style = AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Circle
            scSynapse.ArrowDestination.Filled = False
            scSynapse.Name = "NMDA type"
            scSynapse.SynapticConductance.ValueManual = 0.1
            scSynapse.DecayRate.ValueManual = 50
            scSynapse.RelativeFacilitation.ValueManual = 1
            scSynapse.FacilitationDecay.ValueManual = 1
            scSynapse.VoltageDependent = True
            scSynapse.MaxRelativeConductance.ValueManual = 10
            m_arySynapseTypes.Add(slSynapse.ID, slSynapse, False)

            slSynapse = New DataObjects.Behavior.SynapseTypes.SpikingChemical(doParent)
            scSynapse = DirectCast(slSynapse, DataObjects.Behavior.SynapseTypes.SpikingChemical)
            scSynapse.ArrowDestination.Style = AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Circle
            scSynapse.ArrowDestination.Filled = False
            scSynapse.Name = "Hebbian ACh type"
            scSynapse.SynapticConductance.ValueManual = 0.4
            scSynapse.RelativeFacilitation.ValueManual = 1
            scSynapse.FacilitationDecay.ValueManual = 1
            scSynapse.Hebbian = True
            scSynapse.VoltageDependent = True
            scSynapse.LearningIncrement = 0.02
            scSynapse.MaxAugmentedConductance.ValueManual = 2
            scSynapse.LearningTimeWindow.ValueManual = 40
            scSynapse.ConsolidationFactor = 20
            m_arySynapseTypes.Add(slSynapse.ID, slSynapse, False)

            Dim ncSynapse As New DataObjects.Behavior.SynapseTypes.NonSpikingChemical(doParent)
            slSynapse = ncSynapse
            ncSynapse.Name = "Nicotinic ACh type"
            m_arySynapseTypes.Add(slSynapse.ID, slSynapse, False)

            ncSynapse = New DataObjects.Behavior.SynapseTypes.NonSpikingChemical(doParent)
            slSynapse = ncSynapse
            ncSynapse.Name = "Hyperpolarising IPSP"
            ncSynapse.ArrowDestination.Style = AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Circle
            ncSynapse.ArrowDestination.Filled = True
            ncSynapse.ArrowMiddle.Style = AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.One
            ncSynapse.EquilibriumPotential.ValueManual = -70
            ncSynapse.MaxSynapticConductance.ValueManual = 0.5
            ncSynapse.PreSynapticThreshold.ValueManual = -55
            ncSynapse.PreSynapticSaturationLevel.ValueManual = -40
            m_arySynapseTypes.Add(slSynapse.ID, slSynapse, False)

            ncSynapse = New DataObjects.Behavior.SynapseTypes.NonSpikingChemical(doParent)
            slSynapse = ncSynapse
            ncSynapse.Name = "Depolarising IPSP"
            ncSynapse.ArrowDestination.Style = AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Circle
            ncSynapse.ArrowDestination.Filled = True
            ncSynapse.ArrowMiddle.Style = AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.One
            ncSynapse.EquilibriumPotential.ValueManual = -55
            ncSynapse.MaxSynapticConductance.ValueManual = 0.5
            ncSynapse.PreSynapticThreshold.ValueManual = -60
            ncSynapse.PreSynapticSaturationLevel.ValueManual = -40
            m_arySynapseTypes.Add(slSynapse.ID, slSynapse, False)

            Dim esSynapse As New DataObjects.Behavior.SynapseTypes.Electrical(doParent)
            slSynapse = esSynapse
            esSynapse.Name = "Non-Rectifying Synapse"
            esSynapse.LowCoupling.ValueManual = 0.2
            esSynapse.HighCoupling.ValueManual = 0.2
            esSynapse.TurnOnThreshold.ValueManual = 0
            esSynapse.TurnOnSaturate.ValueManual = 0
            m_arySynapseTypes.Add(slSynapse.ID, slSynapse, False)

            esSynapse = New DataObjects.Behavior.SynapseTypes.Electrical(doParent)
            slSynapse = esSynapse
            esSynapse.Name = "Rectifying Synapse"
            esSynapse.LowCoupling.ValueManual = 0.1
            esSynapse.HighCoupling.ValueManual = 0.3
            esSynapse.TurnOnThreshold.ValueManual = 10
            esSynapse.TurnOnSaturate.ValueManual = 30
            m_arySynapseTypes.Add(slSynapse.ID, slSynapse, False)

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewModule As New DataObjects.Behavior.NeuralModule(doParent)
            oNewModule.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewModule.AfterClone(Me, bCutData, doRoot, oNewModule)
            Return oNewModule
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim nmOrig As DataObjects.Behavior.NeuralModule = DirectCast(doOriginal, DataObjects.Behavior.NeuralModule)

            m_snAHPEquilibriumPotential = DirectCast(nmOrig.m_snAHPEquilibriumPotential.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snSpikePeak = DirectCast(nmOrig.m_snSpikePeak.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_fltSpikeStrength = nmOrig.m_fltSpikeStrength
            m_snCaEquilibriumPotential = DirectCast(nmOrig.m_snCaEquilibriumPotential.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snRefractoryPeriod = DirectCast(nmOrig.m_snRefractoryPeriod.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_bUseCriticalPeriod = nmOrig.m_bUseCriticalPeriod
            m_snStartCriticalPeriod = DirectCast(nmOrig.m_snStartCriticalPeriod.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snEndCriticalPeriod = DirectCast(nmOrig.m_snEndCriticalPeriod.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_bTTX = nmOrig.m_bTTX
            m_bCd = nmOrig.m_bCd
            m_bHodgkinHuxley = nmOrig.m_bHodgkinHuxley

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()  'neuralmodule xml

            oXml.AddChildElement("SpikePeak", m_snSpikePeak.ValueFromDefaultScale)
            oXml.AddChildElement("SpikeStrength", m_fltSpikeStrength)
            oXml.AddChildElement("AHPEquilPot", m_snAHPEquilibriumPotential.ValueFromDefaultScale)
            oXml.AddChildElement("CaEquilPot", m_snCaEquilibriumPotential.ValueFromDefaultScale)
            oXml.AddChildElement("AbsoluteRefr", m_snRefractoryPeriod.ValueFromDefaultScale)

            oXml.AddChildElement("UseCriticalPeriod ", m_bUseCriticalPeriod)
            oXml.AddChildElement("StartCriticalPeriod", m_snStartCriticalPeriod.ValueFromDefaultScale)
            oXml.AddChildElement("EndCriticalPeriod", m_snEndCriticalPeriod.ValueFromDefaultScale)

            oXml.AddChildElement("TTX", m_bTTX)
            oXml.AddChildElement("Cd", m_bCd)
            oXml.AddChildElement("HH", m_bHodgkinHuxley)

            'First we need to get lists of the different types of synaptic types.
            Dim iIndex As Integer = 0

            Dim aryElectrical As New ArrayList
            Dim arySpiking As New ArrayList
            Dim aryNonSpiking As New ArrayList
            For Each deEntry As DictionaryEntry In m_arySynapseTypes
                If deEntry.Value.GetType() Is GetType(DataObjects.Behavior.SynapseTypes.SpikingChemical) Then
                    arySpiking.Add(deEntry.Value)
                ElseIf deEntry.Value.GetType() Is GetType(DataObjects.Behavior.SynapseTypes.NonSpikingChemical) Then
                    aryNonSpiking.Add(deEntry.Value)
                ElseIf deEntry.Value.GetType() Is GetType(DataObjects.Behavior.SynapseTypes.Electrical) Then
                    aryElectrical.Add(deEntry.Value)
                Else
                    Throw New System.Exception("An unknown system type was found '" & deEntry.Value.GetType.FullName & "'")
                End If
            Next

            'Go through and save each of the different types of synapses.
            oXml.AddChildElement("Synapses")
            oXml.IntoElem()

            oXml.AddChildElement("SpikingSynapses")
            oXml.IntoElem()
            For Each stSynapse As DataObjects.Behavior.SynapseType In arySpiking
                stSynapse.SaveSimulationXml(oXml, Me)
            Next
            oXml.OutOfElem() 'Outof SpikingSynapses

            oXml.AddChildElement("NonSpikingSynapses")
            oXml.IntoElem()
            For Each stSynapse As DataObjects.Behavior.SynapseType In aryNonSpiking
                stSynapse.SaveSimulationXml(oXml, Me)
            Next
            oXml.OutOfElem() 'Outof NonSpikingSynapses

            oXml.AddChildElement("ElectricalSynapses")
            oXml.IntoElem()
            For Each stSynapse As DataObjects.Behavior.SynapseType In aryElectrical
                stSynapse.SaveSimulationXml(oXml, Me)
            Next
            oXml.OutOfElem() 'Outof ElectricalSynapses

            oXml.OutOfElem() 'Outof Synapses

            'Go through and save the neurons
            Dim bnNode As AnimatGUI.DataObjects.Behavior.Node
            oXml.AddChildElement("Neurons")
            oXml.IntoElem()
            For Each deEntry As DictionaryEntry In m_aryNodes
                If Util.IsTypeOf(deEntry.Value.GetType(), GetType(IntegrateFireGUI.DataObjects.Behavior.Neurons.Spiking), False) Then
                    bnNode = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Node)
                    bnNode.SaveSimulationXml(oXml, Me)
                Else
                    Throw New System.Exception("There was a node in the realistic neural module that was not derived from a spiking neuron?")
                End If
            Next
            oXml.OutOfElem()

            'Go through and save the connections
            Dim blLink As IntegrateFireGUI.DataObjects.Behavior.Synapse
            oXml.AddChildElement("Connexions")
            oXml.IntoElem()
            For Each deEntry As DictionaryEntry In m_aryLinks
                If Util.IsTypeOf(deEntry.Value.GetType(), GetType(IntegrateFireGUI.DataObjects.Behavior.Synapse), False) Then
                    blLink = DirectCast(deEntry.Value, IntegrateFireGUI.DataObjects.Behavior.Synapse)
                    blLink.SaveSimulationXml(oXml, Me)
                Else
                    Throw New System.Exception("There was a link in the realistic neural module that was not derived from a synapse?")
                End If
            Next
            oXml.OutOfElem()

            oXml.OutOfElem()  'neuralmodule xml

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Spike Strength", m_fltSpikeStrength.GetType(), "SpikeStrength", _
                                        "Global Neural Properties", "Spikes drive current through electrical synapses, but because the spike " & _
                                        "is not modelled accurately, it may sometimes be necessary to increase the effective amplitude of the " & _
                                        "spike in order to drive a more realistic amount of current, This is accomplished by setting the spike " & _
                                        "'strength' to some value above 1. Acceptable values are in the range 1 to 1000.", m_fltSpikeStrength))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snAHPEquilibriumPotential.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("AHP Equil Potential", pbNumberBag.GetType(), "AHPEquilibriumPotential", _
                                        "Global Neural Properties", "Sets  the value of the afterhyperpolarisation equilibrium potential. This sets the " & _
                                        "equilibrium potential for the conductance increase (presumably to potassium) which follows a spike. " & _
                                        " Acceptable values are in the range -150 to -10 mV.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSpikePeak.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Spike Peak", pbNumberBag.GetType(), "SpikePeak", _
                                        "Global Neural Properties", "Sets the value of the membrane potential at the peak of the spike. " & _
                                        "Acceptable values are in the range -30 to 50 mV.", pbNumberBag, "", _
                                        GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snCaEquilibriumPotential.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Ca Equil Potential", pbNumberBag.GetType(), "CaEquilibriumPotential", _
                                        "Global Neural Properties", "Sets the value of the Ca equilibrium potential. " & _
                                        "Acceptable values are in the range -100 to 500 mV.", pbNumberBag, "", _
                                        GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snRefractoryPeriod.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Refractory Period", pbNumberBag.GetType(), "RefractoryPeriod", _
                                        "Global Neural Properties", "Sets the value of the absolute refractory period. This sets the time period " & _
                                        "following a spike during which it is impossible to elicit another spike. " & _
                                        "Acceptable values are in the range 1 to 50 ms.", pbNumberBag, "", _
                                        GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Use Critical Period", m_bUseCriticalPeriod.GetType(), "UseCriticalPeriod", _
                                        "Global Neural Properties", "Determines whether a critical period during which hebbian learning and forgetting occur in an experiment.", m_bUseCriticalPeriod))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Apply TTX", m_bTTX.GetType(), "ApplyTTX", _
                                        "Global Neural Properties", "Applies TTX to the nuerons in the network. This blocks spiking synapses from firing action potentials.", m_bTTX))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Apply Cadmium", m_bCd.GetType(), "ApplyCd", _
                                        "Global Neural Properties", "Applies Cadmium to the nuerons in the network. Cadmium blocks calcium currents. " & _
                                        "This blocks all chemical synapses (both spiking and non-spiking). It also blocks any calcium current defined in the neuron.", m_bCd))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Hodgkin Huxley", m_bHodgkinHuxley.GetType(), "HodgkinHuxley", _
                                        "Global Neural Properties", "If this is true the integrate-and-fire DE is replaced with a standard HH DE.", m_bHodgkinHuxley))

            pbNumberBag = m_snStartCriticalPeriod.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Critical Period Start", pbNumberBag.GetType(), "StartCriticalPeriod", _
                                        "Global Neural Properties", "Sets the start time of the critical period in seconds. " & _
                                        "Acceptable values are in the range 0 to 10000 s.", pbNumberBag, "", _
                                        GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snEndCriticalPeriod.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Critical Period End", pbNumberBag.GetType(), "EndCriticalPeriod", _
                                        "Global Neural Properties", "Sets the end time of the critical period in seconds. " & _
                                        "This value must be larger than the critical period start time. " & _
                                        "Acceptable values are in the range 0 to 10000 s.", pbNumberBag, "", _
                                        GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snAHPEquilibriumPotential Is Nothing Then m_snAHPEquilibriumPotential.ClearIsDirty()
            If Not m_snSpikePeak Is Nothing Then m_snSpikePeak.ClearIsDirty()
            If Not m_snCaEquilibriumPotential Is Nothing Then m_snCaEquilibriumPotential.ClearIsDirty()
            If Not m_snRefractoryPeriod Is Nothing Then m_snRefractoryPeriod.ClearIsDirty()
            If Not m_snStartCriticalPeriod Is Nothing Then m_snStartCriticalPeriod.ClearIsDirty()
            If Not m_snEndCriticalPeriod Is Nothing Then m_snEndCriticalPeriod.ClearIsDirty()
            m_arySynapseTypes.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()  'Into Module Element

            m_strID = oXml.GetChildString("ID", m_strID)
            m_snAHPEquilibriumPotential.LoadData(oXml, "AHPEquilibriumPotential")
            m_snSpikePeak.LoadData(oXml, "SpikePeak")
            m_fltSpikeStrength = oXml.GetChildFloat("SpikeStrength")
            m_snCaEquilibriumPotential.LoadData(oXml, "CaEquilibriumPotential")
            m_snRefractoryPeriod.LoadData(oXml, "RefractoryPeriod")

            m_bUseCriticalPeriod = oXml.GetChildBool("UseCriticalPeriod")
            m_snStartCriticalPeriod.LoadData(oXml, "StartCriticalPeriod")
            m_snEndCriticalPeriod.LoadData(oXml, "EndCriticalPeriod")

            m_bTTX = oXml.GetChildBool("TTX", m_bTTX)
            m_bCd = oXml.GetChildBool("Cd", m_bCd)
            m_bHodgkinHuxley = oXml.GetChildBool("HH", m_bHodgkinHuxley)

            m_arySynapseTypes.Clear()
            oXml.IntoChildElement("SynapseTypes")
            Dim iCount As Integer = oXml.NumberOfChildren() - 1
            Dim stType As SynapseType
            For iIndex As Integer = 0 To iCount
                stType = DirectCast(Util.LoadClass(oXml, iIndex, Me), SynapseType)
                stType.LoadData(oXml)
                'stType.NeuralModule = Me

                m_arySynapseTypes.Add(stType.ID, stType, False)
            Next
            oXml.OutOfElem()

            oXml.OutOfElem()  'Outof Module Element

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()  'Into Module Element

            oXml.AddChildElement("ID", Me.ID)
            m_snAHPEquilibriumPotential.SaveData(oXml, "AHPEquilibriumPotential")
            m_snSpikePeak.SaveData(oXml, "SpikePeak")
            oXml.AddChildElement("SpikeStrength", m_fltSpikeStrength)
            m_snCaEquilibriumPotential.SaveData(oXml, "CaEquilibriumPotential")
            m_snRefractoryPeriod.SaveData(oXml, "RefractoryPeriod")

            oXml.AddChildElement("UseCriticalPeriod", m_bUseCriticalPeriod)
            m_snStartCriticalPeriod.SaveData(oXml, "StartCriticalPeriod")
            m_snEndCriticalPeriod.SaveData(oXml, "EndCriticalPeriod")

            oXml.AddChildElement("TTX", m_bTTX)
            oXml.AddChildElement("Cd", m_bCd)
            oXml.AddChildElement("HH", m_bHodgkinHuxley)

            oXml.AddChildElement("SynapseTypes")
            oXml.IntoElem()
            Dim stType As SynapseType
            For Each deEntry As DictionaryEntry In m_arySynapseTypes
                stType = DirectCast(deEntry.Value, SynapseType)
                stType.SaveData(oXml)
            Next
            oXml.OutOfElem()

            oXml.OutOfElem()  'Outof Module Element

        End Sub


        Public Overrides Sub InitializeSimulationReferences()
            If Me.IsInitialized Then
                MyBase.InitializeSimulationReferences()

                Dim doObject As AnimatGUI.Framework.DataObject
                For Each deEntry As DictionaryEntry In m_arySynapseTypes
                    doObject = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                    doObject.InitializeSimulationReferences()
                Next

                For Each deEntry As DictionaryEntry In m_aryNodes
                    doObject = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                    doObject.InitializeSimulationReferences()
                Next

                For Each deEntry As DictionaryEntry In m_aryLinks
                    doObject = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                    doObject.InitializeSimulationReferences()
                Next
            End If
        End Sub

#End Region

#End Region

    End Class

End Namespace
