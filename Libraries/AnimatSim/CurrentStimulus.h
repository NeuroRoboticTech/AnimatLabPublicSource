// CurrentStimulus.h: interface for the CurrentStimulus class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_CURRENT_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
#define AFX_CURRENT_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatSim
{
	namespace ExternalStimuli
	{

		class ANIMAT_PORT CurrentStimulus  : public ExternalStimulus
		{
		protected:
			Organism *m_lpOrganism;
			string m_strOrganismID;
			string m_strTargetNodeID;

			Node *m_lpNode;
			float *m_lpExternalCurrent;

			BOOL m_bUseEquation;
			CStdPostFixEval *m_lpCurrentOnEval;
			int m_iType;  //Current type

			float m_fltActiveCurrent;
			float m_fltCurrentOn;
			float m_fltCurrentOff;
			float m_fltCurrentBurstOff;
			float m_fltInitialActiveCurrent;

			//There are the durations in time.
			float m_fltCycleOnDuration;
			float m_fltCycleOffDuration;
			float m_fltBurstOnDuration;
			float m_fltBurstOffDuration;

			//The durations are converted to time slice values for easier comparisons.
			long m_lCycleOnDuration;
			long m_lCycleOffDuration;
			long m_lBurstOnDuration;
			long m_lBurstOffDuration;

			long m_lCycleStart;
			long m_lBurstStart;

			BOOL m_bCycleOn;
			BOOL m_bBurstOn;

			float GetCurrentOn(Simulator *lpSim);

		public:
			CurrentStimulus();
			virtual ~CurrentStimulus();

			float CurrentOn() {return m_fltCurrentOn;};
			void CurrentOn(float fltVal);

			float CurrentOff() {return m_fltCurrentOff;};
			void CurrentOff(float fltVal) {m_fltCurrentOff = fltVal;};

			float CurrentBurstOff() {return m_fltCurrentBurstOff;};
			void CurrentBurstOff(float fltVal) {m_fltCurrentBurstOff = fltVal;};

			float CycleOnDuration() {return m_fltCycleOnDuration;};
			void CycleOnDuration(float fltVal);

			float CycleOffDuration() {return m_fltCycleOffDuration;};
			void CycleOffDuration(float fltVal);

			float BurstOnDuration() {return m_fltBurstOnDuration;};
			void BurstOnDuration(float fltVal);

			float BurstOffDuration() {return m_fltBurstOffDuration;};
			void BurstOffDuration(float fltVal);

			virtual void Load(CStdXml &oXml);

			//ActiveItem overrides
			virtual string Type() {return "CurrentStimulus";};
			virtual void Initialize(Simulator *lpSim);  
			virtual void ResetSimulation(Simulator *lpSim);  
			virtual void Activate(Simulator *lpSim);
			virtual void StepSimulation(Simulator *lpSim);
			virtual void Deactivate(Simulator *lpSim);

			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		};

	}			//ExternalStimuli
}				//VortexAnimatSim

#endif // !defined(AFX_CURRENT_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
