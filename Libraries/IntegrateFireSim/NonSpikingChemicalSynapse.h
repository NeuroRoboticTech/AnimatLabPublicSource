// NonSpikingChemSyn.h: interface for the NonSpikingChemicalSynapse class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_NONSPIKINGCHEMSYN_H__364646F7_CCAC_49C0_8B61_BFCCDC72E7F0__INCLUDED_)
#define AFX_NONSPIKINGCHEMSYN_H__364646F7_CCAC_49C0_8B61_BFCCDC72E7F0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace IntegrateFireSim
{
	namespace Synapses
	{

		class ADV_NEURAL_PORT NonSpikingChemicalSynapse : public SynapseType  
		{
		public:
			NonSpikingChemicalSynapse();
			virtual ~NonSpikingChemicalSynapse();
			virtual void Load(CStdXml &oXml);

#pragma region Accessor-Mutators

		void EquilibriumPotential(double dVal) {m_dEquil = dVal;};
		double EquilibriumPotential() {return m_dEquil;};

		void MaxSynapticConductance(double dVal) {m_dSynAmp = dVal;};
		double MaxSynapticConductance() {return m_dSynAmp;};

		void PreSynapticThreshold(double dVal) {m_dThreshV = dVal;};
		double PreSynapticThreshold() {return m_dThreshV;};

		void PreSynapticSaturationLevel(double dVal) {m_dSaturateV = dVal;};
		double PreSynapticSaturationLevel() {return m_dSaturateV;};

		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

#pragma endregion

		protected:
			//int m_iSynapseTypeID;
			//string m_strName;
			double m_dEquil;
			double m_dSynAmp;
			double m_dThreshV;
			double m_dSaturateV;

		friend class IntegrateFireNeuralModule;
		};

	}			//Synapses
}				//IntegrateFireSim

#endif // !defined(AFX_NONSPIKINGCHEMSYN_H__364646F7_CCAC_49C0_8B61_BFCCDC72E7F0__INCLUDED_)
