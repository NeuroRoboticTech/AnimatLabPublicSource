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


			string m_strStructureID;
			string m_strTorsoID;
			string m_strTorsoAlignID;

			string m_strLeftHipID;
			string m_strLeftKneeID;
			string m_strLeftAnkleID;

			string m_strRightHipID;
			string m_strRightKneeID;
			string m_strRightAnkleID;

			string m_strLeftBasmID;
			string m_strLeftIPID;
			string m_strLeftRFID;
			string m_strLeftVastID;
			string m_strLeftBpstID;
			string m_strLeftGastID;
			string m_strLeftPtfID;
			string m_strLeftSolID;

			string m_strRightBasmID;
			string m_strRightIPID;
			string m_strRightRFID;
			string m_strRightVastID;
			string m_strRightBpstID;
			string m_strRightGastID;
			string m_strRightPtfID;
			string m_strRightSolID;

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

			virtual float *GetDataPointer(string strDataType);

			//ActiveItem overrides
			virtual string Type() {return "SynergyFitnessEveal";};
			virtual void Initialize();
			virtual void Activate();
			virtual void ResetSimulation();  
			virtual void StepSimulation();
			virtual void Deactivate();
		};

	}			//ExternalStimuli
}				//VortexAnimatSim
