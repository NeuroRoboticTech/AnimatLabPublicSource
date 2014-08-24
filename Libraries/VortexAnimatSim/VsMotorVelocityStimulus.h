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
			std::string m_strStructureID;
			std::string m_strJointID;
			MotorizedJoint *m_lpJoint;
			int m_iTargetID;

			float *m_lpPosition;
			float *m_lpVelocity;

			std::string m_strVelocityEquation;
			CStdPostFixEval *m_lpEval;

			bool m_bDisableMotorWhenDone;

			float m_fltVelocity;
			float m_fltVelocityReport;

		public:
			VsMotorVelocityStimulus();
			virtual ~VsMotorVelocityStimulus();

			std::string StructureID() {return m_strStructureID;};
			void StructureID(std::string strVal) {m_strStructureID = strVal;};

			std::string JointID() {return m_strJointID;};
			void JointID(std::string strVal) {m_strJointID = strVal;};

			std::string VelocityEquation() {return m_strVelocityEquation;};
			void VelocityEquation(std::string strVal);

			bool DisableMotorWhenDone() {return m_bDisableMotorWhenDone;};
			void DisableMotorWhenDone(bool bVal) {m_bDisableMotorWhenDone = bVal;};
			
			int TargetID() {return m_iTargetID;}
			void TargetID(int iID);
			void TargetID(std::string strID);

			virtual void Load(CStdXml &oXml);

			//ActiveItem overrides
			virtual std::string Type() {return "MotorVelocity";};
			virtual void Initialize();
			virtual void Activate();
			virtual void ResetSimulation();  
			virtual void StepSimulation();
			virtual void Deactivate();

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
		};

	}			//ExternalStimuli
}				//VortexAnimatSim
