/**
\file	VsMotorVelocityStimulus.h

\brief	Declares the vs motor velocity stimulus class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace ExternalStimuli
	{

		class VORTEX_PORT VsMotorVelocityStimulus  : public AnimatSim::ExternalStimuli::ExternalStimulus
		{
		protected:
			string m_strStructureID;
			string m_strJointID;
			MotorizedJoint *m_lpJoint;

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

			virtual void Load(CStdXml &oXml);

			//ActiveItem overrides
			virtual string Type() {return "MotorVelocity";};
			virtual void Initialize();
			virtual void Activate();
			virtual void ResetSimulation();  
			virtual void StepSimulation();
			virtual void Deactivate();

			virtual float *GetDataPointer(const string &strDataType);
			virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
		};

	}			//ExternalStimuli
}				//VortexAnimatSim
