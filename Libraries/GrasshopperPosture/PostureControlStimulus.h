// POSTURE_CONTROLStimulus.h: interface for the RigidBodyInputStimulus class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_POSTURE_CONTROL_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
#define AFX_POSTURE_CONTROL_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace GrasshopperPosture
{
	namespace ExternalStimuli
	{

		class LOCUST_PORT PostureControlStimulus  : public AnimatSim::ExternalStimuli::ExternalStimulus
		{
		protected:
			Organism *m_lpOrganism;
			string m_strStructureID;

			//IDs for body parts that will be needed to perform calculations.
			string m_strThoraxID;
			string m_strAb1ID;
			string m_strLeftTendonLockID;
			string m_strRightTendonLockID;

			string m_strHeadAxisRefID;
			string m_strTailAxisRefID;
			string m_strCOMRefID;
			string m_strRollAxisRefID;
			string m_strPronotumFrontRefID;
			string m_strPronotumRearRefID;

			string m_strLeftThoraxCoxaBetaID;
			string m_strLeftSemilunarBetaID;
			string m_strLeftTibiaBetaID;
			string m_strRightThoraxCoxaBetaID;
			string m_strRightSemilunarBetaID;
			string m_strRightTibiaBetaID;

			string m_strLeftFrontFemurID;
			string m_strLeftFrontTibiaID;
			string m_strLeftMiddleFemurID;
			string m_strLeftMiddleTibiaID;

			//Joints of the left front leg.
			string m_strLeftFrontCoxaFemurID;
			string m_strLeftFrontFemurTibiaID;
			string m_strLeftFrontTibiaTarsusID;

			//Joints of the left Middle leg.
			string m_strLeftMiddleCoxaFemurID;
			string m_strLeftMiddleFemurTibiaID;
			string m_strLeftMiddleTibiaTarsusID;

			//Joints of the left rear leg.
			string m_strLeftRearThoracicCoxaID;
			string m_strLeftRearCoxaFemurID;
			string m_strLeftSemilunarJointID;
			string m_strLeftRearFemurTibiaID;
			string m_strLeftRearTibiaTarsusID;

			//Joints of the Right front leg.
			string m_strRightFrontCoxaFemurID;
			string m_strRightFrontFemurTibiaID;
			string m_strRightFrontTibiaTarsusID;

			//Joints of the Right middle leg.
			string m_strRightMiddleCoxaFemurID;
			string m_strRightMiddleFemurTibiaID;
			string m_strRightMiddleTibiaTarsusID;

			//Joints of the Right rear leg.
			string m_strRightRearThoracicCoxaID;
			string m_strRightRearCoxaFemurID;
			string m_strRightSemilunarJointID;
			string m_strRightRearFemurTibiaID;
			string m_strRightRearTibiaTarsusID;

			//Foot Down sensors for the front-middle legs.
			string m_strLeftMiddleFootDownID;
			string m_strLeftFrontFootDownID;
			string m_strRightMiddleFootDownID;
			string m_strRightFrontFootDownID;

			//Rear Tarsus Down Sensors
			string m_strLeftRearTarsusDownID;
			string m_strRightRearTarsusDownID;

			//Abdomen Joints.
			string m_strAb1JointID;
			string m_strAb2JointID;
			string m_strAb3JointID;
			string m_strAb4JointID;
			
			//Ab control neurons
			string m_strDorsalAbNeuronID;
			//long m_lVentralAbNeuronID;

			//Body parts that will be needed to perform calculations
			RigidBody *m_lpThorax;
			RigidBody *m_lpAb1;
			RigidBody *m_lpLeftTendonLock;
			RigidBody *m_lpRightTendonLock;

			RigidBody *m_lpHeadAxisRef;
			RigidBody *m_lpTailAxisRef;
			RigidBody *m_lpCOMRef;
			RigidBody *m_lpRollAxisRef;
			RigidBody *m_lpPronotumFrontRef;
			RigidBody *m_lpPronotumRearRef;

			RigidBody *m_lpLeftThoraxCoxaBeta;
			RigidBody *m_lpLeftSemilunarBeta;
			RigidBody *m_lpLeftTibiaBeta;
			RigidBody *m_lpRightThoraxCoxaBeta;
			RigidBody *m_lpRightSemilunarBeta;
			RigidBody *m_lpRightTibiaBeta;

			Mesh *m_lpLeftFrontFemur;
			Mesh *m_lpLeftFrontTibia;
			Mesh *m_lpLeftMiddleFemur;
			Mesh *m_lpLeftMiddleTibia;

			//Joints of the left front leg.
			Hinge *m_lpLeftFrontCoxaFemur;
			Hinge *m_lpLeftFrontFemurTibia;
			Hinge *m_lpLeftFrontTibiaTarsus;

			//Joints of the left middle leg.
			Hinge *m_lpLeftMiddleCoxaFemur;
			Hinge *m_lpLeftMiddleFemurTibia;
			Hinge *m_lpLeftMiddleTibiaTarsus;

			//Joints of the left Rear leg.
			Hinge *m_lpLeftRearThoracicCoxa;
			Hinge *m_lpLeftRearCoxaFemur;
			Prismatic *m_lpLeftSemilunarJoint;
			Hinge *m_lpLeftRearFemurTibia;
			Hinge *m_lpLeftRearTibiaTarsus;

			//Joints of the Right front leg.
			Hinge *m_lpRightFrontCoxaFemur;
			Hinge *m_lpRightFrontFemurTibia;
			Hinge *m_lpRightFrontTibiaTarsus;

			//Joints of the Right middle leg.
			Hinge *m_lpRightMiddleCoxaFemur;
			Hinge *m_lpRightMiddleFemurTibia;
			Hinge *m_lpRightMiddleTibiaTarsus;

			//Joints of the Right Rear leg.
			Hinge *m_lpRightRearThoracicCoxa;
			Hinge *m_lpRightRearCoxaFemur;
			Prismatic *m_lpRightSemilunarJoint;
			Hinge *m_lpRightRearFemurTibia;
			Hinge *m_lpRightRearTibiaTarsus;

			//Foot Down sensors for front-middle legs
			RigidBody *m_lpLeftMiddleFootDown;
			RigidBody *m_lpLeftFrontFootDown;
			RigidBody *m_lpRightMiddleFootDown;
			RigidBody *m_lpRightFrontFootDown;

			//Rear tarsus down sensors
			RigidBody *m_lpLeftRearTarsusDown;
			RigidBody *m_lpRightRearTarsusDown;

			//Ab joints
			Hinge *m_lpAb1Joint;
			Hinge *m_lpAb2Joint;
			Hinge *m_lpAb3Joint;
			Hinge *m_lpAb4Joint;
			
			//Ab control neurons
			Node *m_lpDorsalAbNeuron;
			//Node *m_lpVentralAbNeuron;

			float *m_lpDorsalAbCurrent;
			//float *m_lpVentralAbCurrent;
			float m_lpAbCurrentAdd;

			//Variables for the data pointers to perform the calculations
			float *m_vLeftThoraxCoxaBetaPos[3];
			float *m_vLeftSemilunarBetaPos[3];
			float *m_vLeftTibiaBetaPos[3];
			float *m_vRightThoraxCoxaBetaPos[3];
			float *m_vRightSemilunarBetaPos[3];
			float *m_vRightTibiaBetaPos[3];
			float *m_vThoraxPos[3];
			float *m_vThoraxVel[3];
			float *m_lpbLeftTendonLockEnabled;
			float *m_lpbRightTendonLockEnabled;
			
			float *m_lpfltLeftFrontFootDown;
			float *m_lpfltLeftMiddleFootDown;
			float *m_lpfltRightFrontFootDown;
			float *m_lpfltRightMiddleFootDown;

			float *m_lpfltLeftRearTarsusDown;
			float *m_lpfltRightRearTarsusDown;

			float *m_vHeadAxisRefPos[3];
			float *m_vTailAxisRefPos[3];
			float *m_vCOMAxisPos[3];
			float *m_vRollAxisRefPos[3];
			float *m_vPronotumFrontRefPos[3];
			float *m_vPronotumRearRefPos[3];

			float *m_vLeftFrontCoxaFemurPos[3];
			float *m_vLeftFrontFemurTibiaPos[3];
			float *m_vLeftFrontTibiaTarsusPos[3];
			float *m_vLeftMiddleCoxaFemurPos[3];
			float *m_vLeftMiddleFemurTibiaPos[3];
			float *m_vLeftMiddleTibiaTarsusPos[3];

			float *m_vRightFrontCoxaFemurPos[3];
			float *m_vRightFrontFemurTibiaPos[3];
			float *m_vRightFrontTibiaTarsusPos[3];
			float *m_vRightMiddleCoxaFemurPos[3];
			float *m_vRightMiddleFemurTibiaPos[3];
			float *m_vRightMiddleTibiaTarsusPos[3];

			float *m_vLeftFrontFootDownPos[3];
			float *m_vLeftMiddleFootDownPos[3];
			float *m_vRightFrontFootDownPos[3];
			float *m_vRightMiddleFootDownPos[3];

			float *m_lpfltLeftFrontCoxaFemurRotation;
			float *m_lpfltLeftFrontFemurTibiaRotation;
			float *m_lpfltLeftFrontTibiaTarsusRotation;

			float *m_lpfltLeftMiddleCoxaFemurRotation;
			float *m_lpfltLeftMiddleFemurTibiaRotation;
			float *m_lpfltLeftMiddleTibiaTarsusRotation;

			float *m_lpfltLeftRearCoxaFemurRotation;
			float *m_lpfltLeftRearFemurTibiaRotation;
			float *m_lpfltLeftRearTibiaTarsusRotation;
			float *m_lpfltLeftRearCoxaFemurVelocity;

			float *m_lpfltRightFrontCoxaFemurRotation;
			float *m_lpfltRightFrontFemurTibiaRotation;
			float *m_lpfltRightFrontTibiaTarsusRotation;

			float *m_lpfltRightMiddleCoxaFemurRotation;
			float *m_lpfltRightMiddleFemurTibiaRotation;
			float *m_lpfltRightMiddleTibiaTarsusRotation;

			float *m_lpfltRightRearCoxaFemurRotation;
			float *m_lpfltRightRearFemurTibiaRotation;
			float *m_lpfltRightRearTibiaTarsusRotation;
			float *m_lpfltRightRearCoxaFemurVelocity;

			float *m_lpfltAb1JointRotation;
			float *m_lpfltAb2JointRotation;
			float *m_lpfltAb3JointRotation;
			float *m_lpfltAb4JointRotation;
	
			float *m_lpfltAb1JointVelocity;

			float *m_lpfltThoraxTorque;
			float *m_lpfltAb1Torque;

			float *m_lpbRearFootDown;
			BOOL m_bRelockCFJoint;

			//Keeps track of the beta angles
			float m_fltLeftBetaD;
			float m_fltLeftBetaR;
			float m_fltRightBetaD;
			float m_fltRightBetaR;

			//Keeps track of the Otc  (thorax-coxa angle)
			float m_fltLeftOtcD;
			float m_fltLeftOtcR;
			float m_fltRightOtcD;
			float m_fltRightOtcR;

			//Keeps track of the Ocf  (coxa-femur angle)
			float m_fltLeftOcfD;
			float m_fltLeftOcfR;
			float m_fltRightOcfD;
			float m_fltRightOcfR;

			//Keeps track of the Oft (femur-tibia angle)
			float m_fltLeftOftD;
			float m_fltLeftOftR;
			float m_fltRightOftD;
			float m_fltRightOftR;

			//Keeps track of the H
			float m_fltLeftHD;
			float m_fltLeftHR;
			float m_fltRightHD;
			float m_fltRightHR;

			//Keeps track of the Pitch-Yaw-Roll Angles
			float m_fltPitchD;
			float m_fltPitchR;
			float m_fltYawD;
			float m_fltYawR;
			float m_fltRollD;
			float m_fltRollR;
			float m_fltHeadPitchD;
			float m_fltHeadPitchR;

			float m_fltLeftFrontFemurLength;
			float m_fltLeftFrontTibiaLength;
			float m_fltLeftMiddleFemurLength;
			float m_fltLeftMiddleTibiaLength;

			//These variables are used to determine when to release the motors holding the joints just before a jump.
			//When the left rear tibia rotates enough to engage the tendon lock then TendonLockEnabled is set to true.
			//After this happens, then when the tendon lock is disabled we use this as the signal that a jump is imminent
			//and set TendonLockDisabled to true. This lets the control system know it should disable all of the motors.
			BOOL m_bTendonLockEnabled;
			BOOL m_bTendonLockDisabled;
			BOOL m_bMotorsDisabled;
			float m_fltTendonDisabledTime;
			float m_bTendonLockEnabledTime;
			BOOL m_bRearCoxaFemurEnabled;
			BOOL m_bActiveInFlightMotor;
			BOOL RearFemurTibiaDisabled;
			BOOL m_bMoveTarsusStarted;
			int m_iTarsusCounter;
			BOOL m_bTumblingSetup;

			float m_fltDesiredBeta;
			float m_fltDesiredPitch;
			float m_fltDesiredDelta;
			float m_fltGain;
			float m_fltPitchPointOffset;

			BOOL m_bChangePitch;
			BOOL m_bChangeBeta;
			BOOL m_bActivateAbFlexMotor;

			float m_fltAbDelay;
			float m_fltAbPropGain;
			AnimatSim::DelayLine m_PitchDelay;
			BOOL m_bEnableAbControl;
			BOOL m_bLockAbJump;
			float m_fltAbPeriod;
			//float m_fltAbStart;
			float m_fltAbVel;
			float m_fltAbPitchDiff;
			float m_fltAbMagnitude;
			float m_fltLegPeriod;
			BOOL m_bSetAbMag;  //Tells if we have set the leg magnitude yet.
			BOOL m_bDeactivateAbMag;
			float m_fltAbMag;
			float m_fltRightCFMag;
			float m_fltAbStart;
			//float m_fltAbVel;
			float m_fltRightCFVel;
			float m_fltFootTime;
			float m_fltAbTime;


			float m_fltLeftBetaChange;
			float m_fltRightBetaChange;
			float m_fltPitchChange;
			float m_fltPitchVelR;
			float m_fltPitchVelD;

			float m_fltLeftFrontCoxaFemurPos;
			float m_fltLeftFrontFemurTibiaPos;
			float m_fltLeftFrontTibiaTarsusPos;

			float m_fltLeftMiddleCoxaFemurPos;
			float m_fltLeftMiddleFemurTibiaPos;
			float m_fltLeftMiddleTibiaTarsusPos;

			float m_fltLeftRearThoracicCoxaPos;
			float m_fltLeftRearCoxaFemurPos;
			float m_fltLeftRearFemurTibiaPos;
			float m_fltLeftRearTibiaTarsusPos;

			float m_fltRightFrontCoxaFemurPos;
			float m_fltRightFrontFemurTibiaPos;
			float m_fltRightFrontTibiaTarsusPos;

			float m_fltRightMiddleCoxaFemurPos;
			float m_fltRightMiddleFemurTibiaPos;
			float m_fltRightMiddleTibiaTarsusPos;

			float m_fltRightRearThoracicCoxaPos;
			float m_fltRightRearCoxaFemurPos;
			float m_fltRightRearFemurTibiaPos;
			float m_fltRightRearTibiaTarsusPos;

			void ActivateMotor(Simulator *m_lpSim, MotorizedJoint *lpJoint);
			void SetMotorPosition(Simulator *m_lpSim, MotorizedJoint *lpJoint, float fltPos);
			void DeactivateMotor(Simulator *m_lpSim, MotorizedJoint *lpJoint);
			void DeactivateMotors(Simulator *m_lpSim);
			void ReactiveInFlightMotors(Simulator *m_lpSim);
			void DeactiveInFlightMotors(Simulator *m_lpSim);
			void CalculateInitialJointAngles(Simulator *m_lpSim);
			void CalculateFeedbackAngles(Simulator *m_lpSim);
			void SetAbdomenPositions(Simulator *m_lpSim);
			void CalculatePitch(Simulator *m_lpSim);
			void CalculateNewPitch(Simulator *m_lpSim);
			void CalculateOldPitch(Simulator *m_lpSim);

			void ClearValues();

		public:
			PostureControlStimulus();
			virtual ~PostureControlStimulus();

			virtual void Load(CStdXml &oXml);

			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

			//ActiveItem overrides
			virtual string Type() {return "GrasshopperPosture";};
			virtual void Initialize();
			virtual void Activate();
			virtual void ResetSimulation();  
			virtual void StepSimulation();
			virtual void Deactivate();
		};

	}			//ExternalStimuli
}				//VortexAnimatSim

#endif // !defined(AFX_POSTURE_CONTROL_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
