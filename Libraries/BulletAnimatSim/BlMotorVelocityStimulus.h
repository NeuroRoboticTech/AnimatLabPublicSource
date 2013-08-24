/**
\file	BlMotorVelocityStimulus.h

\brief	Declares the vs motor velocity stimulus class.
**/

#pragma once

namespace BulletAnimatSim
{
	namespace ExternalStimuli
	{

		class BULLET_PORT BlMotorVelocityStimulus  : public AnimatSim::ExternalStimuli::ExternalStimulus
		{
		protected:
			string m_strStructureID;
			string m_strJointID;
			MotorizedJoint *m_lpJoint;

			float *m_lpPosition;
			float *m_lpVelocity;

			string m_strVelocityEquation;
			CStdPostFixEval *m_lpEval;

			bool m_bDisableMotorWhenDone;

			float m_fltVelocity;
			float m_fltVelocityReport;

		public:
			BlMotorVelocityStimulus();
			virtual ~BlMotorVelocityStimulus();

			string StructureID() {return m_strStructureID;};
			void StructureID(string strVal) {m_strStructureID = strVal;};

			string JointID() {return m_strJointID;};
			void JointID(string strVal) {m_strJointID = strVal;};

			string VelocityEquation() {return m_strVelocityEquation;};
			void VelocityEquation(string strVal);

			bool DisableMotorWhenDone() {return m_bDisableMotorWhenDone;};
			void DisableMotorWhenDone(bool bVal) {m_bDisableMotorWhenDone = bVal;};

			virtual void Load(CStdXml &oXml);

			//ActiveItem overrides
			virtual string Type() {return "MotorVelocity";};
			virtual void Initialize();
			virtual void Activate();
			virtual void ResetSimulation();  
			virtual void StepSimulation();
			virtual void Deactivate();

			virtual float *GetDataPointer(const string &strDataType);
			virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
		};

	}			//ExternalStimuli
}				//BulletAnimatSim
