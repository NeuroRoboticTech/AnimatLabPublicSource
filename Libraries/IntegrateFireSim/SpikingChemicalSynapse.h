/**
\file SpikingChemicalSynapse.h

\brief	Declares the spiking chemical synapse class.
**/

#pragma once

namespace IntegrateFireSim
{
	namespace Synapses
	{
		/**
		\brief	Spiking chemical synapse type.

		\details This synapse type increases the conductance to the max value when a spike occurs and then it
		decays exponentially back to zero.
		
		\author	dcofer
		\date	3/31/2011
		**/
		class ADV_NEURAL_PORT SpikingChemicalSynapse : public SynapseType  
		{
		public:
			SpikingChemicalSynapse();
			virtual ~SpikingChemicalSynapse();
			virtual void Load(CStdXml &oXml);

#pragma region Accessor-Mutators

			void EquilibriumPotential(double dVal);
			double EquilibriumPotential();

			void SynapticConductance(double dVal);
			double SynapticConductance();

			void DecayRate(double dVal);
			double DecayRate();

			double FacilD();

			void RelativeFacilitation(double dVal);
			double RelativeFacilitation();
			double RelFacil();

			void FacilitationDecay(double dVal);
			double FacilitationDecay();
			double FacilDecay();

			void VoltageDependent(BOOL bVal);
			BOOL VoltageDependent();
			BOOL VoltDep();

			void MaxRelativeConductance(double dVal);
			double MaxRelativeConductance();
			double MaxGVoltDepRel();

			void SaturatePotential(double dVal);
			double SaturatePotential();
			double SatPSPot();

			void ThresholdPotential(double dVal);
			double ThresholdPotential();
			double ThreshPSPot();

			void Hebbian(BOOL bVal);
			BOOL Hebbian();

			void MaxAugmentedConductance(double dVal);
			double MaxAugmentedConductance();
			double MaxGHebb() ;

			void LearningIncrement(double dVal);
			double LearningIncrement();
			double HebbIncrement();

			void LearningTimeWindow(double dVal);
			double LearningTimeWindow();
			double HebbTimeWindow();

			void AllowForgetting(BOOL bVal);
			BOOL AllowForgetting();
			BOOL AllowForget();

			void ForgettingTimeWindow(double dVal);
			double ForgettingTimeWindow();
			double ForgettingWindow();

			void ConsolidationFactor(double dVal);
			double ConsolidationFactor();
			double Consolidation();

#pragma endregion

			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

		protected:

			/// The equilibruim potential
			double m_dEquil;

			/// base syn amp, before vd or hebb
			double m_dSynAmp;		

			/// The synaptic decay
			double m_dDecay;

			/// The facilitation
			double m_dFacilD;

			/// The relative facilitation amount.
			double m_dRelFacil;

			/// The facilitation decay
			double m_dFacilDecay;

			/// true if voltage dependent
			BOOL m_bVoltDep;

			/// The maximum relative conductance
			double m_dMaxRelCond;

			/// The saturation post-synaptic potential
			double m_dSatPSPot;

			/// The threshold post-synaptic potential
			double m_dThreshPSPot;

			/// true if hebbian learning is used.
			BOOL m_bHebbian;

			/// The maximum augmented conductance
			double m_dMaxAugCond;

			/// The learning increment
			double m_dLearningInc;

			/// Learning time window.
			double m_dLearningTime;

			/// true if forgetting is allowed.
			BOOL m_bAllowForget;

			/// Forgetting time window
			double m_dForgetTime;

			/// The consolidation factor
			double m_dConsolidation;

		friend class IntegrateFireSim::IntegrateFireNeuralModule;
		friend class IntegrateFireSim::Neuron;
		};

	}			//Synapses
}				//IntegrateFireSim

