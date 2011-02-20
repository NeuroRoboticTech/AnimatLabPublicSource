// SpikingChemSyn.h: interface for the SpikingChemicalSynapse class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace IntegrateFireSim
{
	namespace Synapses
	{

		class ADV_NEURAL_PORT SpikingChemicalSynapse : public SynapseType  
		{
		public:
			SpikingChemicalSynapse();
			virtual ~SpikingChemicalSynapse();
			virtual void Load(CStdXml &oXml);

#pragma region Accessor-Mutators

		void EquilibriumPotential(double dVal) {m_dEquil = dVal;};
		double EquilibriumPotential() {return m_dEquil;};

		void SynapticConductance(double dVal) {m_dSynAmp = dVal;};
		double SynapticConductance() {return m_dSynAmp;};

		void DecayRate(double dVal) {m_dDecay = dVal;};
		double DecayRate() {return m_dDecay;};

		double FacilD() {return m_dFacilD;};

		void RelativeFacilitation(double dVal) {m_dRelFacil = dVal;};
		double RelativeFacilitation() {return m_dRelFacil;};
		double RelFacil() {return m_dRelFacil;};

		void FacilitationDecay(double dVal);
		double FacilitationDecay() {return m_dFacilDecay;};
		double FacilDecay() {return m_dFacilDecay;};

		void VoltageDependent(BOOL bVal) {m_bVoltDep = bVal;};
		BOOL VoltageDependent() {return m_bVoltDep;};
		BOOL VoltDep() {return m_bVoltDep;};

		void MaxRelativeConductance(double dVal) {m_dMaxRelCond = dVal;};
		double MaxRelativeConductance() {return m_dMaxRelCond;};
		double MaxGVoltDepRel() {return m_dMaxRelCond;};

		void SaturatePotential(double dVal) {m_dSatPSPot = dVal;};
		double SaturatePotential() {return m_dSatPSPot;};
		double SatPSPot() {return m_dSatPSPot;}; 

		void ThresholdPotential(double dVal) {m_dThreshPSPot = dVal;};
		double ThresholdPotential() {return m_dThreshPSPot;};
		double ThreshPSPot() {return m_dThreshPSPot;}; 

		void Hebbian(BOOL bVal) {m_bHebbian = bVal;};
		BOOL Hebbian() {return m_bHebbian;};

		void MaxAugmentedConductance(double dVal) {m_dMaxAugCond = dVal;};
		double MaxAugmentedConductance() {return m_dMaxAugCond;};
		double MaxGHebb() {return m_dMaxAugCond;};

		void LearningIncrement(double dVal) {m_dLearningInc = dVal;};
		double LearningIncrement() {return m_dLearningInc;};
		double HebbIncrement() {return m_dLearningInc;};

		void LearningTimeWindow(double dVal) {m_dLearningTime = dVal;};
		double LearningTimeWindow() {return m_dLearningTime;};
		double HebbTimeWindow() {return m_dLearningTime;};

		void AllowForgetting(BOOL bVal) {m_bAllowForget = bVal;};
		BOOL AllowForgetting() {return m_bAllowForget;};
		BOOL AllowForget() {return m_bAllowForget;}; 

		void ForgettingTimeWindow(double dVal) {m_dForgetTime = dVal;};
		double ForgettingTimeWindow() {return m_dForgetTime;};
		double ForgettingWindow() {return m_dForgetTime;};

		void ConsolidationFactor(double dVal) {m_dConsolidation = dVal;};
		double ConsolidationFactor() {return m_dConsolidation;};
		double Consolidation() {return m_dConsolidation;}; 

		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

#pragma endregion

		protected:
			//IntegrateFireNeuralModule *m_lpModule;

			//int m_iSynapseTypeID;
			//string m_strName;
			double m_dEquil;
			double m_dSynAmp;		// base syn amp, before vd or hebb
			double m_dDecay;
			double m_dFacilD;
			double m_dRelFacil;
			double m_dFacilDecay;

			BOOL m_bVoltDep;
			double m_dMaxRelCond;
			double m_dSatPSPot;
			double m_dThreshPSPot;

			BOOL m_bHebbian;
			double m_dMaxAugCond;
			double m_dLearningInc;
			double m_dLearningTime;
			BOOL m_bAllowForget;
			double m_dForgetTime;
			double m_dConsolidation;

		friend class IntegrateFireNeuralModule;
		friend class Neuron;
		};

	}			//Synapses
}				//IntegrateFireSim

