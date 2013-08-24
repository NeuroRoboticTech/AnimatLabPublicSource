
#pragma once

namespace BulletAnimatSim
{

	/**
	\namespace	BulletAnimatSim::ExternalStimuli

	\brief	Classes that produce external stimuli. 
	**/
	namespace ExternalStimuli
	{

		class BULLET_PORT BlForceStimulus  : public AnimatSim::ExternalStimuli::ExternalStimulus
		{
		protected:
			Structure *m_lpStructure;
			string m_strStructureID;

			string m_strBodyID;
			RigidBody *m_lpBody;
			BlRigidBody *m_lpVsBody;

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
			BlForceStimulus();
			virtual ~BlForceStimulus();

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
			
			virtual void Load(CStdXml &oXml);

			//ActiveItem overrides
			virtual string Type() {return "ForceInput";};
			virtual void Initialize();
			virtual void ResetSimulation();  
			virtual void StepSimulation();
			virtual void Deactivate();

			virtual float *GetDataPointer(const string &strDataType);
			virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
		};

	}			//ExternalStimuli
}				//BulletAnimatSim
