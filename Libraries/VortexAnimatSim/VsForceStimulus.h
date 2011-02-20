// VsForceStimulus.h: interface for the VsForceStimulus class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_FORCE_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
#define AFX_FORCE_INPUT_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace VortexAnimatSim
{
	namespace ExternalStimuli
	{

		class VORTEX_PORT VsForceStimulus  : public AnimatSim::ExternalStimuli::ExternalStimulus
		{
		protected:
			Structure *m_lpStructure;
			string m_strStructureID;

			string m_strBodyID;
			RigidBody *m_lpBody;
			VsRigidBody *m_lpVsBody;

			CStdFPoint m_oRelativePosition;

			string m_strForceXEquation;
			string m_strForceYEquation;
			string m_strForceZEquation;

			CStdPostFixEval *m_lpForceXEval;
			CStdPostFixEval *m_lpForceYEval;
			CStdPostFixEval *m_lpForceZEval;

			float m_fltForceX;
			float m_fltForceY;
			float m_fltForceZ;

			float m_fltForceReportX;
			float m_fltForceReportY;
			float m_fltForceReportZ;

			string m_strTorqueXEquation;
			string m_strTorqueYEquation;
			string m_strTorqueZEquation;

			CStdPostFixEval *m_lpTorqueXEval;
			CStdPostFixEval *m_lpTorqueYEval;
			CStdPostFixEval *m_lpTorqueZEval;

			float m_fltTorqueX;
			float m_fltTorqueY;
			float m_fltTorqueZ;

			float m_fltTorqueReportX;
			float m_fltTorqueReportY;
			float m_fltTorqueReportZ;

			CStdPostFixEval *SetupEquation(string strEquation);

		public:
			VsForceStimulus();
			virtual ~VsForceStimulus();

			float RelativePositionX() {return m_oRelativePosition.x;};
			float RelativePositionY() {return m_oRelativePosition.y;};
			float RelativePositionZ() {return m_oRelativePosition.z;};

			void RelativePositionX(float fltVal);
			void RelativePositionY(float fltVal);
			void RelativePositionZ(float fltVal);

			string ForceXEquation() {return m_strForceXEquation;};
			string ForceYEquation() {return m_strForceYEquation;};
			string ForceZEquation() {return m_strForceZEquation;};

			void ForceXEquation(string strVal);
			void ForceYEquation(string strVal);
			void ForceZEquation(string strVal);

			string TorqueXEquation() {return m_strTorqueXEquation;};
			string TorqueYEquation() {return m_strTorqueYEquation;};
			string TorqueZEquation() {return m_strTorqueZEquation;};

			void TorqueXEquation(string strVal);
			void TorqueYEquation(string strVal);
			void TorqueZEquation(string strVal);
			
			virtual void Load(Simulator *lpSim, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, CStdXml &oXml);

			//ActiveItem overrides
			virtual string Type() {return "ForceInput";};
			virtual void Initialize(Simulator *lpSim);
			virtual void Activate(Simulator *lpSim);
			virtual void ResetSimulation(Simulator *lpSim);  
			virtual void StepSimulation(Simulator *lpSim);
			virtual void Deactivate(Simulator *lpSim);

			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		};

	}			//ExternalStimuli
}				//VortexAnimatSim

#endif // !defined(AFX_FORCE_INPUT_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
