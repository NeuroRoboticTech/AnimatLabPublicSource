// GatedSynapse.h: interface for the GatedSynapse class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_GATEDSYNAPSE_H__21B27420_19DA_47AD_846E_7EBEDC8147D9__INCLUDED_)
#define AFX_GATEDSYNAPSE_H__21B27420_19DA_47AD_846E_7EBEDC8147D9__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace FiringRateSim
{
	namespace Synapses
	{

		class FAST_NET_PORT GatedSynapse : public Synapse    
		{
		protected:
			unsigned char m_iInitialGateValue;

		public:
			GatedSynapse();
			virtual ~GatedSynapse();

			unsigned char InitialGateValue() {return m_iInitialGateValue;};
			void InitialGateValue(unsigned char iVal) {m_iInitialGateValue = iVal;};

			virtual float CalculateModulation(FiringRateModule *lpModule);

#pragma region DataAccesMethods
			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
#pragma endregion

			virtual void Load(Simulator *lpSim, Structure *lpStructure, Neuron *lpNeuron, CStdXml &oXml);
		};

	}			//Synapses
}				//FiringRateSim

#endif // !defined(AFX_GATEDSYNAPSE_H__21B27420_19DA_47AD_846E_7EBEDC8147D9__INCLUDED_)
