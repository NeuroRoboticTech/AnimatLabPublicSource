// ExternalInputStimulus.h: interface for the ExternalInputStimulus class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_EXTERNAL_INPUT_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
#define AFX_EXTERNAL_INPUT_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatSim
{
	namespace ExternalStimuli
	{

		class ANIMAT_PORT ExternalInputStimulus  : public ExternalStimulus
		{
		protected:
			string m_strStructureID;
			Structure *m_lpStructure;

			string m_strNodeID;
			Node *m_lpNode;

			string m_strInputEquation;
			CStdPostFixEval *m_lpEval;

			float m_fltInput;

		public:
			ExternalInputStimulus();
			virtual ~ExternalInputStimulus();
			
			float Input() {return m_fltInput;};
			void Input(float fltVal) {m_fltInput = fltVal;};

			string InputEquation() {return m_strInputEquation;};
			void InputEquation(string strVal);

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, CStdXml &oXml);

			//ActiveItem overrides
			virtual string Type() {return "ExternalInput";};
			virtual void Activate(Simulator *lpSim);
			virtual void Initialize(Simulator *lpSim);
			virtual void StepSimulation(Simulator *lpSim);
			virtual void Deactivate(Simulator *lpSim);

			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		};

	}			//ExternalStimuli
}				//VortexAnimatSim

#endif // !defined(AFX_EXTERNAL_INPUT_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
