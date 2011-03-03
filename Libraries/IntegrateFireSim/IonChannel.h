// IonChannel.h: interface for the IonChannel class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ION_CHANNEL_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_ION_CHANNEL_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace IntegrateFireSim
{
	class ADV_NEURAL_PORT IonChannel : public AnimatBase 
	{
	protected:
		string m_strID;
		string m_strName;

		float m_fltGInit;
		float m_fltMInit;
		float m_fltHInit;

		BOOL m_bEnabled;
		float m_fltGmax;
		float m_fltG;
		float m_fltMPower;
		float m_fltHPower;
		float m_fltEquilibriumPotential;

		float m_fltM;
		float m_fltNm;
		AnimatSim::Gains::Gain *m_lpMinf;
		AnimatSim::Gains::Gain *m_lpTm;

		float m_fltH;
		float m_fltNh;
		AnimatSim::Gains::Gain *m_lpHinf;
		AnimatSim::Gains::Gain *m_lpTh;

		//Calculated variables.
		float m_fltTotalAct;
		float m_fltI;

		float m_fltMinf;
		float m_fltHinf;
		float m_fltTm;
		float m_fltTh;

	public:
		IonChannel();
		virtual ~IonChannel();

#pragma region Accessor-Mutators

		string ID() {return m_strID;};

		void Enabled(BOOL bVal) {m_bEnabled = bVal;}; 
		BOOL Enabled() {return m_bEnabled;};

		void Gmax(float fltVal) {m_fltGmax = fltVal;}; 
		float Gmax() {return m_fltGmax;};

		void Ginit(float fltVal);
		float Ginit() {return m_fltGInit;};

		void Hinit(float fltVal);
		float Hinit() {return m_fltHInit;};

		void Minit(float fltVal);
		float Minit() {return m_fltMInit;};

		void MPower(float fltVal) {m_fltMPower = fltVal;};
		float MPower() {return m_fltMPower;};

		void HPower(float fltVal) {m_fltHPower = fltVal;};
		float HPower() {return m_fltHPower;};

		void EquilibriumPotential(float fltVal) {m_fltEquilibriumPotential = fltVal;};
		float EquilibriumPotential() {return m_fltEquilibriumPotential;};

		void Nm(float fltVal) {m_fltNm = fltVal;};
		float Nm() {return m_fltNm;};

		void Nh(float fltVal) {m_fltNh = fltVal;};
		float Nh() {return m_fltNh;};

#pragma endregion

#pragma region DataAccesMethods
		virtual float *GetDataPointer(string strDataType);
		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
#pragma endregion


		virtual void Initialize(Simulator *lpSim, Structure *lpStructure);
		virtual void Load(CStdXml &oXml);
		virtual float CalculateCurrent(float fltStep, float fltVm);
		virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure);
	};
}				//AnimatSim

#endif // !defined(AFX_ION_CHANNEL_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
