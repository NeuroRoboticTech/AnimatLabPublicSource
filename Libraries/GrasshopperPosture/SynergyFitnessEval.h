#pragma once


namespace GrasshopperPosture
{
	namespace ExternalStimuli
	{

		class LOCUST_PORT SynergyFitnessEval : public AnimatSim::ExternalStimuli::ExternalStimulus
		{
		protected:
			Structure *m_lpStructure;
			RigidBody *m_lpTorso;
			RigidBody *m_lpTorsoAlign;

			Joint *m_lpLeftHip;
			Joint *m_lpLeftKnee;
			Joint *m_lpLeftAnkle;

			Joint *m_lpRightHip;
			Joint *m_lpRightKnee;
			Joint *m_lpRightAnkle;

			RigidBody *m_lpLeftBasm;
			RigidBody *m_lpLeftIP;
			RigidBody *m_lpLeftRF;
			RigidBody *m_lpLeftVast;
			RigidBody *m_lpLeftBpst;
			RigidBody *m_lpLeftGast;
			RigidBody *m_lpLeftPtf;
			RigidBody *m_lpLeftSol;

			RigidBody *m_lpRightBasm;
			RigidBody *m_lpRightIP;
			RigidBody *m_lpRightRF;
			RigidBody *m_lpRightVast;
			RigidBody *m_lpRightBpst;
			RigidBody *m_lpRightGast;
			RigidBody *m_lpRightPtf;
			RigidBody *m_lpRightSol;


			std::string m_strStructureID;
			std::string m_strTorsoID;
			std::string m_strTorsoAlignID;

			std::string m_strLeftHipID;
			std::string m_strLeftKneeID;
			std::string m_strLeftAnkleID;

			std::string m_strRightHipID;
			std::string m_strRightKneeID;
			std::string m_strRightAnkleID;

			std::string m_strLeftBasmID;
			std::string m_strLeftIPID;
			std::string m_strLeftRFID;
			std::string m_strLeftVastID;
			std::string m_strLeftBpstID;
			std::string m_strLeftGastID;
			std::string m_strLeftPtfID;
			std::string m_strLeftSolID;

			std::string m_strRightBasmID;
			std::string m_strRightIPID;
			std::string m_strRightRFID;
			std::string m_strRightVastID;
			std::string m_strRightBpstID;
			std::string m_strRightGastID;
			std::string m_strRightPtfID;
			std::string m_strRightSolID;

			float *m_lpfltLeftHipRotation;
			float *m_lpfltLeftKneeRotation;
			float *m_lpfltLeftAnkleRotation;

			float *m_lpfltRightHipRotation;
			float *m_lpfltRightKneeRotation;
			float *m_lpfltRightAnkleRotation;

			float *m_lpfltLeftBasmTension;
			float *m_lpfltLeftIPTension;
			float *m_lpfltLeftRFTension;
			float *m_lpfltLeftVastTension;
			float *m_lpfltLeftBpstTension;
			float *m_lpfltLeftGastTension;
			float *m_lpfltLeftPtfTension;
			float *m_lpfltLeftSolTension;

			float *m_lpfltRightBasmTension;
			float *m_lpfltRightIPTension;
			float *m_lpfltRightRFTension;
			float *m_lpfltRightVastTension;
			float *m_lpfltRightBpstTension;
			float *m_lpfltRightGastTension;
			float *m_lpfltRightPtfTension;
			float *m_lpfltRightSolTension;

			float *m_vTorsoPos[3];
			float *m_vTorsoAlignPos[3];
			float m_vDesiredTorsoPos[3];

			float m_fltDesiredLeftHipRotation;
			float m_fltDesiredLeftKneeRotation;
			float m_fltDesiredLeftAnkleRotation;

			float m_fltDesiredRightHipRotation;
			float m_fltDesiredRightKneeRotation;
			float m_fltDesiredRightAnkleRotation;

			float m_fltTorsoHeightDiff;
			float m_fltTorsoOrientationRad;
			float m_fltTorsoOrientationDeg;
			float m_fltJointError;
			float m_fltTotalTension;
			float m_fltFitness;

			float MapValue(float fltInput, float fltInMax, float fltInMin, float fltOutMax, float fltOutMin);
			void EvaluatePostureFitness();

		public:
			SynergyFitnessEval(void);
			~SynergyFitnessEval(void);

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, CStdXml &oXml);

			virtual float *GetDataPointer(std::string strDataType);

			//ActiveItem overrides
			virtual std::string Type() {return "SynergyFitnessEveal";};
			virtual void Initialize();
			virtual void Activate();
			virtual void ResetSimulation();  
			virtual void StepSimulation();
			virtual void Deactivate();
		};

	}			//ExternalStimuli
}				//VortexAnimatSim
