// EnablerStimulus.h: interface for the EnablerStimulus class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ENABLER_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
#define AFX_ENABLER_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatSim
{
	namespace ExternalStimuli
	{

		class ANIMAT_PORT EnablerStimulus  : public ExternalStimulus
		{
		protected:
			Structure *m_lpStructure;
			string m_strStructureID;

			Node *m_lpNode;

			string m_strBodyID;
			BOOL m_bEnableWhenActive;

		public:
			EnablerStimulus();
			virtual ~EnablerStimulus();
			
			BOOL EnableWhenActive() {return m_bEnableWhenActive;};
			void EnableWhenActive(BOOL bVal) {m_bEnableWhenActive = bVal;};

			virtual void Activate(Simulator *lpSim);
			virtual void StepSimulation(Simulator *lpSim);
			virtual void Deactivate(Simulator *lpSim);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

			virtual void Load(CStdXml &oXml);

			//ActiveItem overrides
			virtual string Type() {return "EnablerInput";};
			virtual void Initialize(Simulator *lpSim);
		};

	}			//ExternalStimuli
}				//VortexAnimatSim

#endif // !defined(AFX_ENABLER_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
