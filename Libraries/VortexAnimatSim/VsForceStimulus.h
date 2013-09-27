
#pragma once

namespace VortexAnimatSim
{

	/**
	\namespace	VortexAnimatSim::ExternalStimuli

	\brief	Classes that produce external stimuli. 
	**/
	namespace ExternalStimuli
	{

		class VORTEX_PORT VsForceStimulus  : public AnimatSim::ExternalStimuli::ExternalStimulus
		{
		protected:
			Structure *m_lpStructure;
			std::string m_strStructureID;

			std::string m_strBodyID;
			RigidBody *m_lpBody;
			VsRigidBody *m_lpVsBody;

			CStdFPoint m_oRelativePosition;

			std::string m_strForceXEquation;
			std::string m_strForceYEquation;
			std::string m_strForceZEquation;

			CStdPostFixEval *m_lpForceXEval;
			CStdPostFixEval *m_lpForceYEval;
			CStdPostFixEval *m_lpForceZEval;

			float m_fltForceX;
			float m_fltForceY;
			float m_fltForceZ;

			float m_fltForceReportX;
			float m_fltForceReportY;
			float m_fltForceReportZ;

			std::string m_strTorqueXEquation;
			std::string m_strTorqueYEquation;
			std::string m_strTorqueZEquation;

			CStdPostFixEval *m_lpTorqueXEval;
			CStdPostFixEval *m_lpTorqueYEval;
			CStdPostFixEval *m_lpTorqueZEval;

			float m_fltTorqueX;
			float m_fltTorqueY;
			float m_fltTorqueZ;

			float m_fltTorqueReportX;
			float m_fltTorqueReportY;
			float m_fltTorqueReportZ;

			CStdPostFixEval *SetupEquation(std::string strEquation);

		public:
			VsForceStimulus();
			virtual ~VsForceStimulus();

			float RelativePositionX() {return m_oRelativePosition.x;};
			float RelativePositionY() {return m_oRelativePosition.y;};
			float RelativePositionZ() {return m_oRelativePosition.z;};

			void RelativePositionX(float fltVal);
			void RelativePositionY(float fltVal);
			void RelativePositionZ(float fltVal);

			std::string ForceXEquation() {return m_strForceXEquation;};
			std::string ForceYEquation() {return m_strForceYEquation;};
			std::string ForceZEquation() {return m_strForceZEquation;};

			void ForceXEquation(std::string strVal);
			void ForceYEquation(std::string strVal);
			void ForceZEquation(std::string strVal);

			std::string TorqueXEquation() {return m_strTorqueXEquation;};
			std::string TorqueYEquation() {return m_strTorqueYEquation;};
			std::string TorqueZEquation() {return m_strTorqueZEquation;};

			void TorqueXEquation(std::string strVal);
			void TorqueYEquation(std::string strVal);
			void TorqueZEquation(std::string strVal);
			
			virtual void Load(CStdXml &oXml);

			//ActiveItem overrides
			virtual std::string Type() {return "ForceInput";};
			virtual void Initialize();
			virtual void ResetSimulation();  
			virtual void StepSimulation();
			virtual void Deactivate();

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);
		};

	}			//ExternalStimuli
}				//VortexAnimatSim
