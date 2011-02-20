// ElecSyn.h: interface for the ElectricalSynapse class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ELECSYN_H__3D41A66D_6746_48F6_8F0C_685EF62CC07C__INCLUDED_)
#define AFX_ELECSYN_H__3D41A66D_6746_48F6_8F0C_685EF62CC07C__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace IntegrateFireSim
{
	namespace Synapses
	{

		class ADV_NEURAL_PORT ElectricalSynapse : public SynapseType  
		{
		public:
			ElectricalSynapse();
			virtual ~ElectricalSynapse();
			virtual void Load(CStdXml &oXml);

#pragma region Accessor-Mutators

		void LowCoupling(double dVal) {m_dLowCoup = dVal;};
		double LowCoupling() {return m_dLowCoup;};

		void HighCoupling(double dVal) {m_dHiCoup = dVal;};
		double HighCoupling() {return m_dHiCoup;};

		void TurnOnThreshold(double dVal) {m_dTurnOnV = dVal;};
		double TurnOnThreshold() {return m_dTurnOnV;};

		void TurnOnSaturate(double dVal) {m_dSaturateV = dVal;};
		double TurnOnSaturate() {return m_dSaturateV;};

		//int SynapseTypeID() {return m_iSynapseTypeID;};
		//void SynapseTypeID(int iID) {m_iSynapseTypeID = iID;};

		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

#pragma endregion

		protected:
			//int m_iSynapseTypeID;
			//string m_strName;
			double m_dLowCoup;
			double m_dHiCoup;
			double m_dTurnOnV;
			double m_dSaturateV;

		friend class IntegrateFireNeuralModule;
		};

	}			//Synapses
}				//IntegrateFireSim

#endif // !defined(AFX_ELECSYN_H__3D41A66D_6746_48F6_8F0C_685EF62CC07C__INCLUDED_)
