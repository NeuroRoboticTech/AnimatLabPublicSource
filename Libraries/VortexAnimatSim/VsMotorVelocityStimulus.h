// Injection.h: interface for the VsMotorVelocityStimulus class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_MOTOR_VELOCITY_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
#define AFX_MOTOR_VELOCITY_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace VortexAnimatSim
{
	namespace ExternalStimuli
	{

		class VORTEX_PORT VsMotorVelocityStimulus  : public ExternalStimulus
		{
		protected:
			string m_strStructureID;
			string m_strJointID;
			Joint *m_lpJoint;

			float *m_lpPosition;
			float *m_lpVelocity;

			string m_strVelocityEquation;
			CStdPostFixEval *m_lpEval;

			BOOL m_bDisableMotorWhenDone;

			float m_fltVelocity;
			float m_fltVelocityReport;

		public:
			VsMotorVelocityStimulus();
			virtual ~VsMotorVelocityStimulus();

			string StructureID() {return m_strStructureID;};
			void StructureID(string strVal) {m_strStructureID = strVal;};

			string JointID() {return m_strJointID;};
			void JointID(string strVal) {m_strJointID = strVal;};

			string VelocityEquation() {return m_strVelocityEquation;};
			void VelocityEquation(string strVal);

			BOOL DisableMotorWhenDone() {return m_bDisableMotorWhenDone;};
			void DisableMotorWhenDone(BOOL bVal) {m_bDisableMotorWhenDone = bVal;};

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, CStdXml &oXml);
			virtual void Trace(ostream &oOs);

			//ActiveItem overrides
			virtual string Type() {return "MotorVelocity";};
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

#endif // !defined(AFX_MOTOR_VELOCITY_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
