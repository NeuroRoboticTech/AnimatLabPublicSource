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

		class LOCUST_PORT PostureControlStimulus  : public AnimatLibrary::ExternalStimuli::ExternalStimulus
		{
		protected:
			Structure *m_lpStructure;
			string m_strStructureID;

			//IDs for body parts that will be needed to perform calculations.
			string m_strThoraxID;
			string m_strLeftTendonLockID;
			string m_strRightTendonLockID;

			string m_strHeadAxisRefID;
			string m_strTailAxisRefID;
			string m_strCOMRefID;
			string m_strRollAxisRefID;

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
			string m_strLeftRearCoxaID;
			string m_strLeftRearFemurID;
			string m_strLeftRearTibiaID;

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

			//Body parts that will be needed to perform calculations
			RigidBody *m_lpThorax;
			RigidBody *m_lpLeftTendonLock;
			RigidBody *m_lpRightTendonLock;

			RigidBody *m_lpHeadAxisRef;
			RigidBody *m_lpTailAxisRef;
			RigidBody *m_lpCOMRef;
			RigidBody *m_lpRollAxisRef;

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
			Box *m_lpLeftRearCoxa;
			Mesh *m_lpLeftRearFemur;
			Mesh *m_lpLeftRearTibia;

			//Joints of the left front leg.
			VsHinge *m_lpLeftFrontCoxaFemur;
			VsHinge *m_lpLeftFrontFemurTibia;
			VsHinge *m_lpLeftFrontTibiaTarsus;

			//Joints of the left middle leg.
			VsHinge *m_lpLeftMiddleCoxaFemur;
			VsHinge *m_lpLeftMiddleFemurTibia;
			VsHinge *m_lpLeftMiddleTibiaTarsus;

			//Joints of the left Rear leg.
			VsHinge *m_lpLeftRearThoracicCoxa;
			VsHinge *m_lpLeftRearCoxaFemur;
			VsPrismatic *m_lpLeftSemilunarJoint;
			VsHinge *m_lpLeftRearFemurTibia;
			VsHinge *m_lpLeftRearTibiaTarsus;

			//Joints of the Right front leg.
			VsHinge *m_lpRightFrontCoxaFemur;
			VsHinge *m_lpRightFrontFemurTibia;
			VsHinge *m_lpRightFrontTibiaTarsus;

			//Joints of the Right middle leg.
			VsHinge *m_lpRightMiddleCoxaFemur;
			VsHinge *m_lpRightMiddleFemurTibia;
			VsHinge *m_lpRightMiddleTibiaTarsus;

			//Joints of the Right Rear leg.
			VsHinge *m_lpRightRearThoracicCoxa;
			VsHinge *m_lpRightRearCoxaFemur;
			VsPrismatic *m_lpRightSemilunarJoint;
			VsHinge *m_lpRightRearFemurTibia;
			VsHinge *m_lpRightRearTibiaTarsus;

			//Foot Down sensors for front-middle legs
			RigidBody *m_lpLeftMiddleFootDown;
			RigidBody *m_lpLeftFrontFootDown;
			RigidBody *m_lpRightMiddleFootDown;
			RigidBody *m_lpRightFrontFootDown;

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

			float *m_vHeadAxisRefPos[3];
			float *m_vTailAxisRefPos[3];
			float *m_vCOMAxisPos[3];
			float *m_vRollAxisRefPos[3];

			float *m_vLeftFrontCoxaFemurPos[3];
			float *m_vLeftFrontFemurTibiaPos[3];
			float *m_vLeftFrontTibiaTarsusPos[3];
			float *m_vLeftMiddleCoxaFemurPos[3];
			float *m_vLeftMiddleFemurTibiaPos[3];
			float *m_vLeftMiddleTibiaTarsusPos[3];
			float *m_vLeftRearTCJointPos[3];

			float *m_vRightFrontCoxaFemurPos[3];
			float *m_vRightFrontFemurTibiaPos[3];
			float *m_vRightFrontTibiaTarsusPos[3];
			float *m_vRightMiddleCoxaFemurPos[3];
			float *m_vRightMiddleFemurTibiaPos[3];
			float *m_vRightMiddleTibiaTarsusPos[3];
			float *m_vRightRearTCJointPos[3];

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

			float *m_lpfltRightFrontCoxaFemurRotation;
			float *m_lpfltRightFrontFemurTibiaRotation;
			float *m_lpfltRightFrontTibiaTarsusRotation;

			float *m_lpfltRightMiddleCoxaFemurRotation;
			float *m_lpfltRightMiddleFemurTibiaRotation;
			float *m_lpfltRightMiddleTibiaTarsusRotation;

			float *m_lpfltRightRearCoxaFemurRotation;
			float *m_lpfltRightRearFemurTibiaRotation;
			float *m_lpfltRightRearTibiaTarsusRotation;


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

			//Keeps track of the Gamma
			float m_fltLeftGammaD;
			float m_fltLeftGammaR;
			float m_fltRightGammaD;
			float m_fltRightGammaR;

			//Keeps track of the Alpha
			float m_fltAlphaR;
			float m_fltAlphaD;

			//Keeps track of the Pitch-Yaw-Roll Angles
			float m_fltPitchD;
			float m_fltPitchR;
			float m_fltYawD;
			float m_fltYawR;
			float m_fltRollD;
			float m_fltRollR;

			float m_fltLeftFrontFemurLength;
			float m_fltLeftFrontTibiaLength;
			float m_fltLeftMiddleFemurLength;
			float m_fltLeftMiddleTibiaLength;
			float m_fltLeftRearFemurLength;
			float m_fltLeftRearTibiaLength;

			float m_fltStepRate;

			//The coxa is rotated slightly. This is the effective length taking into consideration this rotation.
			float m_fltEffectiveCoxaLength;
			//This is the distance from the coxa-femur joint to the end of the tibia when the rear leg is 
			//cocked at a specified beta angle.
			float m_fltCoxaTibiaGapLength;

			//The height difference between the thoracic-coxa joints of the rear and front legs.
			float m_fltFrontRearLegJointHeight;

			//This is the leg height for the front legs to give the correct pitch angles.
			float m_fltFrontLeftLegHeight;
			float m_fltFrontRightLegHeight;

			//This is the average gap between the coxa-tibia joint when the leg is fully flexed.
			//I am using this here instead of the calculated value during the jump because I want to
			//use a consistent value to calculate the joint angles. I was getting a lot of variance, and
			//I am trying to reduce that.
			float m_fltAvgCTGapLength;

			//This is the distance in the z direction from the COM to the front coxa-femur joint.
			//It is used to calculate the amount of height we need to add to the front legs to get them
			//to pitch the body to the correct amount.
			float m_fltDistComFront;

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
			BOOL m_bThrustThruCOM;

			float m_fltDesiredBeta;
			float m_fltDesiredPitch;
			float m_fltDesiredDelta;
			float m_fltGain;

			BOOL m_bChangePitch;
			BOOL m_bChangeBeta;

			float m_fltLeftBetaChange;
			float m_fltRightBetaChange;
			float m_fltPitchChange;

			float m_fltLeftFrontCoxaFemurPos;
			float m_fltLeftFrontCoxaFemurPosTarget;
			float m_fltLeftFrontFemurTibiaPos;
			float m_fltLeftFrontFemurTibiaPosTarget;
			float m_fltLeftFrontTibiaTarsusPos;

			float m_fltLeftMiddleCoxaFemurPos;
			float m_fltLeftMiddleFemurTibiaPos;
			float m_fltLeftMiddleTibiaTarsusPos;

			float m_fltLeftRearThoracicCoxaPos;
			float m_fltLeftRearCoxaFemurPos;
			float m_fltLeftRearCoxaFemurPosTarget;
			float m_fltLeftRearFemurTibiaPos;
			float m_fltLeftRearTibiaTarsusPos;

			float m_fltRightFrontCoxaFemurPos;
			float m_fltRightFrontCoxaFemurPosTarget;
			float m_fltRightFrontFemurTibiaPos;
			float m_fltRightFrontFemurTibiaPosTarget;
			float m_fltRightFrontTibiaTarsusPos;

			float m_fltRightMiddleCoxaFemurPos;
			float m_fltRightMiddleFemurTibiaPos;
			float m_fltRightMiddleTibiaTarsusPos;

			float m_fltRightRearThoracicCoxaPos;
			float m_fltRightRearCoxaFemurPos;
			float m_fltRightRearCoxaFemurPosTarget;
			float m_fltRightRearFemurTibiaPos;
			float m_fltRightRearTibiaTarsusPos;



			void ActivateMotor(Simulator *lpSim, Joint *lpJoint);
			void SetMotorPosition(Simulator *lpSim, Joint *lpJoint, float fltPos);
			void DeactivateMotor(Simulator *lpSim, Joint *lpJoint);
			void DeactivateMotors(Simulator *lpSim);
			void ReactiveInFlightMotors(Simulator *lpSim);
			void CalculateJointAngles(Simulator *lpSim, float fltDelta);
			void CalculateFeedbackAngles(Simulator *lpSim);

		public:
			PostureControlStimulus();
			virtual ~PostureControlStimulus();

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, CStdXml &oXml);

			virtual float *GetDataPointer(string strDataType);

			//ActiveItem overrides
			virtual string Type() {return "ExternalInput";};
			virtual void Activate(Simulator *lpSim);
			virtual void Initialize(Simulator *lpSim);  
			virtual void StepSimulation(Simulator *lpSim);
			virtual void Deactivate(Simulator *lpSim);
		};

	}			//ExternalStimuli
}				//VortexAnimatLibrary

#endif // !defined(AFX_POSTURE_CONTROL_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
