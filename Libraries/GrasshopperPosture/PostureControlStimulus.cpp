// PostureControlStimulus.cpp: implementation of the PostureControlStimulus class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "PostureControlStimulus.h"

#define MAX_CHANGE 100e-6f

namespace GrasshopperPosture
{
	namespace ExternalStimuli
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

PostureControlStimulus::PostureControlStimulus()
{
	m_lpOrganism = NULL;

	m_fltDesiredBeta = 0;
	m_fltDesiredDelta = 0; 
	m_fltDesiredPitch = 0;
	m_fltGain = 0.002f;
	m_bTumblingSetup = FALSE;

	m_fltAbDelay = 0;
	m_fltAbPropGain = 0;
	m_bEnableAbControl = TRUE;
	m_bLockAbJump = TRUE;
	m_bRelockCFJoint = FALSE;

	m_fltAbPitchDiff = 0;
	m_fltAbStart = 0;
	m_fltAbPeriod = 35e-3f;
	m_fltAbVel = 0;
	m_fltLegPeriod = 40e-3f;
	m_bSetAbMag = FALSE;
	m_bDeactivateAbMag = FALSE;
	m_fltAbMag = 0;
	m_fltRightCFMag = 0;
	m_fltAbStart = 0;
	m_fltAbVel = 0;
	m_fltRightCFVel = 0;
	m_fltPitchPointOffset = 0;
	m_fltFootTime = -1;
	m_fltAbTime = 0;

	m_lpThorax = NULL;
	m_lpLeftTendonLock = NULL;
	m_lpRightTendonLock = NULL;

	m_lpHeadAxisRef = NULL;
	m_lpTailAxisRef = NULL;
	m_lpCOMRef = NULL;
	m_lpRollAxisRef = NULL;
	m_lpPronotumFrontRef = NULL;
	m_lpPronotumRearRef = NULL;

	m_lpLeftThoraxCoxaBeta = NULL;
	m_lpLeftSemilunarBeta = NULL;
	m_lpLeftTibiaBeta = NULL;
	m_lpRightThoraxCoxaBeta = NULL;
	m_lpRightSemilunarBeta = NULL;
	m_lpRightTibiaBeta = NULL;

	m_lpLeftFrontCoxaFemur = NULL;
	m_lpLeftFrontFemurTibia = NULL;
	m_lpLeftFrontTibiaTarsus = NULL;

	m_lpLeftMiddleCoxaFemur = NULL;
	m_lpLeftMiddleFemurTibia = NULL;
	m_lpLeftMiddleTibiaTarsus = NULL;

	m_lpLeftRearThoracicCoxa = NULL;
	m_lpLeftRearCoxaFemur = NULL;
	m_lpLeftSemilunarJoint = NULL;
	m_lpLeftRearFemurTibia = NULL;
	m_lpLeftRearTibiaTarsus = NULL;

	m_lpRightFrontCoxaFemur = NULL;
	m_lpRightFrontFemurTibia = NULL;
	m_lpRightFrontTibiaTarsus = NULL;

	m_lpRightMiddleCoxaFemur = NULL;
	m_lpRightMiddleFemurTibia = NULL;
	m_lpRightMiddleTibiaTarsus = NULL;

	m_lpRightRearThoracicCoxa = NULL;
	m_lpRightRearCoxaFemur = NULL;
	m_lpRightSemilunarJoint = NULL;
	m_lpRightRearFemurTibia = NULL;
	m_lpRightRearTibiaTarsus = NULL;

	m_lpLeftMiddleFootDown = NULL;
	m_lpLeftFrontFootDown = NULL;
	m_lpRightMiddleFootDown = NULL;
	m_lpRightFrontFootDown = NULL;

	m_lpLeftRearTarsusDown = NULL;
	m_lpRightRearTarsusDown = NULL;

	m_lpDorsalAbNeuron = NULL;
	//m_lpVentralAbNeuron = NULL;
	m_lpDorsalAbCurrent = NULL;
	//m_lpVentralAbCurrent = NULL;
	m_lpAbCurrentAdd = 0;

	m_lpAb1Joint = NULL;
	m_lpAb2Joint = NULL;
	m_lpAb3Joint = NULL;
	m_lpAb4Joint = NULL;

	m_fltLeftOtcD = 0;
	m_fltLeftOtcR = 0;
	m_fltRightOtcD = 0;
	m_fltRightOtcR = 0;

	m_fltLeftOcfD = 0;
	m_fltLeftOcfR = 0;
	m_fltRightOcfD = 0;
	m_fltRightOcfR = 0;

	m_fltLeftOftD = 0;
	m_fltLeftOftR = 0;
	m_fltRightOftD = 0;
	m_fltRightOftR = 0;

	m_fltLeftHD = 0;
	m_fltLeftHR = 0;
	m_fltRightHD = 0;
	m_fltRightHR = 0;

	m_fltLeftBetaD=0;
	m_fltLeftBetaR=0;
	m_fltRightBetaD=0;
	m_fltRightBetaR=0;

	m_bChangePitch = FALSE;
	m_bChangeBeta = FALSE;
	m_fltLeftBetaChange = 0;
	m_fltRightBetaChange = 0;
	m_fltPitchChange = 0;
	m_fltPitchVelR = 0;
	m_fltPitchVelD = 0;

	m_fltPitchD=0;
	m_fltPitchR=0;
	m_fltYawD=0;
	m_fltYawR=0;
	m_fltRollD=0;
	m_fltRollR=0;
	m_fltHeadPitchD = 0;
	m_fltHeadPitchR = 0;

	m_lpbLeftTendonLockEnabled = NULL;
	m_lpbRightTendonLockEnabled = NULL;
	m_fltTendonDisabledTime = 0;
	m_bTendonLockEnabledTime = 0;
	m_bMotorsDisabled = FALSE;
	m_bActiveInFlightMotor = FALSE;
	m_bMoveTarsusStarted = FALSE;
	m_iTarsusCounter = 0;

	m_lpLeftFrontFemur = NULL;
	m_lpLeftFrontTibia = NULL;
	m_lpLeftMiddleFemur = NULL;
	m_lpLeftMiddleTibia = NULL;

	for(int i=0; i<3; i++)
	{
		m_vLeftThoraxCoxaBetaPos[i] = NULL;
		m_vLeftSemilunarBetaPos[i] = NULL;
		m_vLeftTibiaBetaPos[i] = NULL;
		m_vRightThoraxCoxaBetaPos[i] = NULL;
		m_vRightSemilunarBetaPos[i] = NULL;
		m_vRightTibiaBetaPos[i] = NULL;
		m_vThoraxPos[i] = NULL;
		m_vThoraxVel[i] = NULL;

		m_vHeadAxisRefPos[i] = NULL;
		m_vTailAxisRefPos[i] = NULL;
		m_vCOMAxisPos[i] = NULL;
		m_vRollAxisRefPos[i] = NULL;
		m_vPronotumFrontRefPos[i] = NULL;
		m_vPronotumRearRefPos[i] = NULL;

		m_vLeftFrontCoxaFemurPos[i] = NULL;
		m_vLeftFrontFemurTibiaPos[i] = NULL;
		m_vLeftFrontTibiaTarsusPos[i] = NULL;
		m_vLeftMiddleCoxaFemurPos[i] = NULL;
		m_vLeftMiddleFemurTibiaPos[i] = NULL;
		m_vLeftMiddleTibiaTarsusPos[i] = NULL;

		m_vRightFrontCoxaFemurPos[i] = NULL;
		m_vRightFrontFemurTibiaPos[i] = NULL;
		m_vRightFrontTibiaTarsusPos[i] = NULL;
		m_vRightMiddleCoxaFemurPos[i] = NULL;
		m_vRightMiddleFemurTibiaPos[i] = NULL;
		m_vRightMiddleTibiaTarsusPos[i] = NULL;

		m_vLeftFrontFootDownPos[i] = NULL;
		m_vLeftMiddleFootDownPos[i] = NULL;
		m_vRightFrontFootDownPos[i] = NULL;
		m_vRightMiddleFootDownPos[i] = NULL;
	}

	m_fltLeftFrontFemurLength = 0;
	m_fltLeftFrontTibiaLength = 0;
	m_fltLeftMiddleFemurLength = 0;
	m_fltLeftMiddleTibiaLength = 0;

	m_bTendonLockEnabled = FALSE;
	m_bTendonLockDisabled = FALSE;
	m_bRearCoxaFemurEnabled = FALSE;

	m_lpfltLeftFrontFootDown = NULL;
	m_lpfltLeftMiddleFootDown = NULL;
	m_lpfltRightFrontFootDown = NULL;
	m_lpfltRightMiddleFootDown = NULL;

	m_lpfltLeftRearTarsusDown = NULL;
	m_lpfltRightRearTarsusDown = NULL;

	m_fltLeftFrontCoxaFemurPos = 0.7f;
	m_fltLeftFrontFemurTibiaPos = 0.1f;
	m_fltLeftFrontTibiaTarsusPos = 0.6f;

	m_fltLeftMiddleCoxaFemurPos = 0.82f; //0.42f;
	m_fltLeftMiddleFemurTibiaPos = 0.50f;
	m_fltLeftMiddleTibiaTarsusPos = 0.6f;

	m_fltLeftRearThoracicCoxaPos = -0.4f; //0.873;
	m_fltLeftRearCoxaFemurPos = 1.0f; //0.5f; //0.26f;
	m_fltLeftRearFemurTibiaPos = 0;
	m_fltLeftRearTibiaTarsusPos = -1.5f;

	m_fltRightFrontCoxaFemurPos = 0.7f;
	m_fltRightFrontFemurTibiaPos = -0.1f;
	m_fltRightFrontTibiaTarsusPos = 0.6f;
 
	m_fltRightMiddleCoxaFemurPos = -0.82f; //-0.42f;
	m_fltRightMiddleFemurTibiaPos = -0.50f;
	m_fltRightMiddleTibiaTarsusPos = 0.6f; 

	m_fltRightRearThoracicCoxaPos = -0.4f; //0.873;
	m_fltRightRearCoxaFemurPos =  1.0f; //0.5f; //0.26f;
	m_fltRightRearFemurTibiaPos = 0;
	m_fltRightRearTibiaTarsusPos = -1.5f;

	m_lpfltLeftFrontCoxaFemurRotation = NULL;
	m_lpfltLeftFrontFemurTibiaRotation = NULL;
	m_lpfltLeftFrontTibiaTarsusRotation = NULL;

	m_lpfltLeftMiddleCoxaFemurRotation = NULL;
	m_lpfltLeftMiddleFemurTibiaRotation = NULL;
	m_lpfltLeftMiddleTibiaTarsusRotation = NULL;

	m_lpfltLeftRearCoxaFemurRotation = NULL;
	m_lpfltLeftRearFemurTibiaRotation = NULL;
	m_lpfltLeftRearTibiaTarsusRotation = NULL;
	m_lpfltLeftRearCoxaFemurVelocity = NULL;

	m_lpfltRightFrontCoxaFemurRotation = NULL;
	m_lpfltRightFrontFemurTibiaRotation = NULL;
	m_lpfltRightFrontTibiaTarsusRotation = NULL;

	m_lpfltRightMiddleCoxaFemurRotation = NULL;
	m_lpfltRightMiddleFemurTibiaRotation = NULL;
	m_lpfltRightMiddleTibiaTarsusRotation = NULL;

	m_lpfltRightRearCoxaFemurRotation = NULL;
	m_lpfltRightRearFemurTibiaRotation = NULL;
	m_lpfltRightRearTibiaTarsusRotation = NULL;
	m_lpfltRightRearCoxaFemurVelocity = NULL;

	m_lpfltAb1JointRotation = NULL;
	m_lpfltAb2JointRotation = NULL;
	m_lpfltAb3JointRotation = NULL;
	m_lpfltAb4JointRotation = NULL;

	m_lpfltThoraxTorque = 0;
	m_lpfltAb1Torque = 0;
	m_lpAb1 = NULL;

	RearFemurTibiaDisabled = FALSE;
	m_bActivateAbFlexMotor = FALSE;
}

PostureControlStimulus::~PostureControlStimulus()
{

try
{
	m_lpOrganism = NULL;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of PostureControlStimulus\r\n", "", -1, FALSE, TRUE);}
}

void PostureControlStimulus::Initialize(Simulator *lpSim)
{
	if(!lpSim) 
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	AnimatLibrary::ExternalStimuli::ExternalStimulus::Initialize(lpSim);

	//Lets try and get the node we will dealing with.
	m_lpOrganism = dynamic_cast<Organism *>(lpSim->FindOrganism(m_strStructureID));

	//Now lets get the body parts that will be needed to perform calculations.
	m_lpThorax = m_lpOrganism->FindRigidBody(m_strThoraxID);
	m_lpLeftTendonLock = m_lpOrganism->FindRigidBody(m_strLeftTendonLockID);
	m_lpRightTendonLock = m_lpOrganism->FindRigidBody(m_strRightTendonLockID);

	m_lpHeadAxisRef = m_lpOrganism->FindRigidBody(m_strHeadAxisRefID);
	m_lpTailAxisRef = m_lpOrganism->FindRigidBody(m_strTailAxisRefID);
	m_lpCOMRef = m_lpOrganism->FindRigidBody(m_strCOMRefID);
	m_lpRollAxisRef = m_lpOrganism->FindRigidBody(m_strRollAxisRefID);

	if(!Std_IsBlank(m_strAb1ID))
		m_lpAb1 = m_lpOrganism->FindRigidBody(m_strAb1ID);

	if(!Std_IsBlank(m_strPronotumFrontRefID))
	{
		m_lpPronotumFrontRef = m_lpOrganism->FindRigidBody(m_strPronotumFrontRefID);
		m_lpPronotumRearRef = m_lpOrganism->FindRigidBody(m_strPronotumRearRefID);
	}

	m_lpLeftThoraxCoxaBeta = m_lpOrganism->FindRigidBody(m_strLeftThoraxCoxaBetaID);
	m_lpLeftSemilunarBeta = m_lpOrganism->FindRigidBody(m_strLeftSemilunarBetaID);
	m_lpLeftTibiaBeta = m_lpOrganism->FindRigidBody(m_strLeftTibiaBetaID);
	m_lpRightThoraxCoxaBeta = m_lpOrganism->FindRigidBody(m_strRightThoraxCoxaBetaID);
	m_lpRightSemilunarBeta = m_lpOrganism->FindRigidBody(m_strRightSemilunarBetaID);
	m_lpRightTibiaBeta = m_lpOrganism->FindRigidBody(m_strRightTibiaBetaID);

	m_lpLeftFrontFemur = dynamic_cast<Mesh *>(m_lpOrganism->FindRigidBody(m_strLeftFrontFemurID));
	m_lpLeftFrontTibia = dynamic_cast<Mesh *>(m_lpOrganism->FindRigidBody(m_strLeftFrontTibiaID));
	m_lpLeftMiddleFemur = dynamic_cast<Mesh *>(m_lpOrganism->FindRigidBody(m_strLeftMiddleFemurID));
	m_lpLeftMiddleTibia = dynamic_cast<Mesh *>(m_lpOrganism->FindRigidBody(m_strLeftMiddleTibiaID));

	CStdFPoint oPoint = m_lpLeftMiddleFemur->CollisionBoxSize();
	m_fltLeftMiddleFemurLength = oPoint.y*lpSim->InverseDistanceUnits()*0.001;
	oPoint = m_lpLeftMiddleTibia->CollisionBoxSize(); 
	m_fltLeftMiddleTibiaLength = oPoint.y*lpSim->InverseDistanceUnits()*0.001;

	oPoint = m_lpLeftFrontFemur->CollisionBoxSize();
	m_fltLeftFrontFemurLength = oPoint.z*lpSim->InverseDistanceUnits()*0.0001;
	oPoint = m_lpLeftFrontTibia->CollisionBoxSize();
	m_fltLeftFrontTibiaLength = oPoint.z*lpSim->InverseDistanceUnits()*0.0001;

	//Now lets get the foot down sensors.
	m_lpLeftMiddleFootDown = m_lpOrganism->FindRigidBody(m_strLeftMiddleFootDownID);
	m_lpLeftFrontFootDown = m_lpOrganism->FindRigidBody(m_strLeftFrontFootDownID);
	m_lpRightMiddleFootDown = m_lpOrganism->FindRigidBody(m_strRightMiddleFootDownID);
	m_lpRightFrontFootDown = m_lpOrganism->FindRigidBody(m_strRightFrontFootDownID);

	if(!Std_IsBlank(m_strLeftRearTarsusDownID) && !Std_IsBlank(m_strRightRearTarsusDownID))
	{
		m_lpLeftRearTarsusDown = m_lpOrganism->FindRigidBody(m_strLeftRearTarsusDownID);
		m_lpRightRearTarsusDown = m_lpOrganism->FindRigidBody(m_strRightRearTarsusDownID);
	}

	//Left front leg joints.
	m_lpLeftFrontCoxaFemur = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strLeftFrontCoxaFemurID));
	m_lpLeftFrontFemurTibia = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strLeftFrontFemurTibiaID));
	m_lpLeftFrontTibiaTarsus = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strLeftFrontTibiaTarsusID));

	//Left Middle leg joints.
	m_lpLeftMiddleCoxaFemur = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strLeftMiddleCoxaFemurID));
	m_lpLeftMiddleFemurTibia = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strLeftMiddleFemurTibiaID));
	m_lpLeftMiddleTibiaTarsus = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strLeftMiddleTibiaTarsusID));

	//Left Rear leg joints.
	m_lpLeftRearThoracicCoxa = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strLeftRearThoracicCoxaID));
	m_lpLeftRearCoxaFemur = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strLeftRearCoxaFemurID));
	m_lpLeftSemilunarJoint = dynamic_cast<VsPrismatic *>(m_lpOrganism->FindJoint(m_strLeftSemilunarJointID));
	m_lpLeftRearFemurTibia = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strLeftRearFemurTibiaID));
	m_lpLeftRearTibiaTarsus = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strLeftRearTibiaTarsusID));

	//Right front leg joints.
	m_lpRightFrontCoxaFemur = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strRightFrontCoxaFemurID));
	m_lpRightFrontFemurTibia = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strRightFrontFemurTibiaID));
	m_lpRightFrontTibiaTarsus = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strRightFrontTibiaTarsusID));

	//Right Middle leg joints.
	m_lpRightMiddleCoxaFemur = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strRightMiddleCoxaFemurID));
	m_lpRightMiddleFemurTibia = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strRightMiddleFemurTibiaID));
	m_lpRightMiddleTibiaTarsus = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strRightMiddleTibiaTarsusID));

	//Right Rear leg joints.
	m_lpRightRearThoracicCoxa = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strRightRearThoracicCoxaID));
	m_lpRightRearCoxaFemur = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strRightRearCoxaFemurID));
	m_lpRightSemilunarJoint = dynamic_cast<VsPrismatic *>(m_lpOrganism->FindJoint(m_strRightSemilunarJointID));
	m_lpRightRearFemurTibia = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strRightRearFemurTibiaID));
	m_lpRightRearTibiaTarsus = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strRightRearTibiaTarsusID));

	//Ab Joints.
	m_lpAb1Joint = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strAb1JointID));
	m_lpAb2Joint = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strAb2JointID));
	m_lpAb3Joint = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strAb3JointID));
	m_lpAb4Joint = dynamic_cast<VsHinge *>(m_lpOrganism->FindJoint(m_strAb4JointID));

	//Ab control neurons
	if(m_lDorsalAbNeuronID>=0)
	{
		m_lpDorsalAbNeuron = m_lpOrganism->NervousSystem()->FindNode("FastNeuralNet", m_lDorsalAbNeuronID);
		m_lpDorsalAbCurrent = m_lpDorsalAbNeuron->GetDataPointer("ExternalCurrent");
	}

	//if(m_lVentralAbNeuronID>=0)
	//{
	//	m_lpVentralAbNeuron = m_lpOrganism->NervousSystem()->FindNode("FastNeuralNet", m_lVentralAbNeuronID);
	//	m_lpVentralAbCurrent = m_lpDorsalAbNeuron->GetDataPointer("ExternalCurrent");
	//}


	//Lets get the data pointers for the positions of our body parts.
	string strPosData, strLinVelData;
	for(int i=0; i<3; i++)
	{
		if(i==0)
		{
			strPosData = "BodyPositionX";
			strLinVelData = "BodyLinearVelocityX";
		}
		else if(i==1)
		{
			strPosData = "BodyPositionY";
			strLinVelData = "BodyLinearVelocityY";
		}
		else
		{
			strPosData = "BodyPositionZ";
			strLinVelData = "BodyLinearVelocityZ";
		}

		m_vLeftThoraxCoxaBetaPos[i] = m_lpLeftThoraxCoxaBeta->GetDataPointer(strPosData);
		m_vLeftSemilunarBetaPos[i] = m_lpLeftSemilunarBeta->GetDataPointer(strPosData);
		m_vLeftTibiaBetaPos[i] = m_lpLeftTibiaBeta->GetDataPointer(strPosData);
		m_vRightThoraxCoxaBetaPos[i] = m_lpRightThoraxCoxaBeta->GetDataPointer(strPosData);
		m_vRightSemilunarBetaPos[i] = m_lpRightSemilunarBeta->GetDataPointer(strPosData);
		m_vRightTibiaBetaPos[i] = m_lpRightTibiaBeta->GetDataPointer(strPosData);
		m_vThoraxPos[i] = m_lpThorax->GetDataPointer(strPosData);
		m_vThoraxVel[i] = m_lpThorax->GetDataPointer(strLinVelData);

		m_vLeftFrontCoxaFemurPos[i] = m_lpLeftFrontCoxaFemur->GetDataPointer(strPosData);
		m_vLeftFrontFemurTibiaPos[i] = m_lpLeftFrontFemurTibia->GetDataPointer(strPosData);
		m_vLeftFrontTibiaTarsusPos[i] = m_lpLeftFrontTibiaTarsus->GetDataPointer(strPosData);
		m_vLeftMiddleCoxaFemurPos[i] = m_lpLeftMiddleCoxaFemur->GetDataPointer(strPosData);
		m_vLeftMiddleFemurTibiaPos[i] = m_lpLeftMiddleFemurTibia->GetDataPointer(strPosData);
		m_vLeftMiddleTibiaTarsusPos[i] = m_lpLeftMiddleTibiaTarsus->GetDataPointer(strPosData);

		m_vRightFrontCoxaFemurPos[i] = m_lpRightFrontCoxaFemur->GetDataPointer(strPosData);
		m_vRightFrontFemurTibiaPos[i] = m_lpRightFrontFemurTibia->GetDataPointer(strPosData);
		m_vRightFrontTibiaTarsusPos[i] = m_lpRightFrontTibiaTarsus->GetDataPointer(strPosData);
		m_vRightMiddleCoxaFemurPos[i] = m_lpRightMiddleCoxaFemur->GetDataPointer(strPosData);
		m_vRightMiddleFemurTibiaPos[i] = m_lpRightMiddleFemurTibia->GetDataPointer(strPosData);
		m_vRightMiddleTibiaTarsusPos[i] = m_lpRightMiddleTibiaTarsus->GetDataPointer(strPosData);

		m_vLeftFrontFootDownPos[i] = m_lpLeftFrontFootDown->GetDataPointer(strPosData);
		m_vLeftMiddleFootDownPos[i] = m_lpLeftMiddleFootDown->GetDataPointer(strPosData);
		m_vRightFrontFootDownPos[i] = m_lpRightFrontFootDown->GetDataPointer(strPosData);
		m_vRightMiddleFootDownPos[i] = m_lpRightMiddleFootDown->GetDataPointer(strPosData);

		m_vHeadAxisRefPos[i] = m_lpHeadAxisRef->GetDataPointer(strPosData);
		m_vTailAxisRefPos[i] = m_lpTailAxisRef->GetDataPointer(strPosData);
		m_vCOMAxisPos[i] = m_lpCOMRef->GetDataPointer(strPosData);
		m_vRollAxisRefPos[i] = m_lpRollAxisRef->GetDataPointer(strPosData);

		if(m_lpPronotumFrontRef)
		{
			m_vPronotumFrontRefPos[i] = m_lpPronotumFrontRef->GetDataPointer(strPosData);
			m_vPronotumRearRefPos[i] = m_lpPronotumRearRef->GetDataPointer(strPosData);
		}
	}

	//Determine the offset for the derived point to calculate the pitch angle
	m_fltPitchPointOffset = *m_vPronotumFrontRefPos[2] - *m_vPronotumRearRefPos[2];

	m_lpbLeftTendonLockEnabled = m_lpLeftTendonLock->GetDataPointer("ENABLE");
	m_lpbRightTendonLockEnabled = m_lpRightTendonLock->GetDataPointer("ENABLE");

	m_lpfltLeftFrontFootDown = m_lpLeftFrontFootDown->GetDataPointer("ContactCount");
	m_lpfltLeftMiddleFootDown = m_lpLeftMiddleFootDown->GetDataPointer("ContactCount");
	m_lpfltRightFrontFootDown = m_lpRightFrontFootDown->GetDataPointer("ContactCount");
	m_lpfltRightMiddleFootDown = m_lpRightMiddleFootDown->GetDataPointer("ContactCount");

	if(m_lpLeftRearTarsusDown && m_lpRightRearTarsusDown)
	{
		m_lpfltLeftRearTarsusDown = m_lpLeftRearTarsusDown->GetDataPointer("ContactCount");
		m_lpfltRightRearTarsusDown = m_lpRightRearTarsusDown->GetDataPointer("ContactCount");
	}

	m_lpfltLeftFrontCoxaFemurRotation = m_lpLeftFrontCoxaFemur->GetDataPointer("JointRotation");
	m_lpfltLeftFrontFemurTibiaRotation = m_lpLeftFrontFemurTibia->GetDataPointer("JointRotation");
	m_lpfltLeftFrontTibiaTarsusRotation = m_lpLeftFrontTibiaTarsus->GetDataPointer("JointRotation");

	m_lpfltLeftMiddleCoxaFemurRotation = m_lpLeftMiddleCoxaFemur->GetDataPointer("JointRotation");
	m_lpfltLeftMiddleFemurTibiaRotation = m_lpLeftMiddleFemurTibia->GetDataPointer("JointRotation");
	m_lpfltLeftMiddleTibiaTarsusRotation = m_lpLeftMiddleTibiaTarsus->GetDataPointer("JointRotation");

	m_lpfltLeftRearCoxaFemurRotation = m_lpLeftRearCoxaFemur->GetDataPointer("JointRotation");
	m_lpfltLeftRearFemurTibiaRotation = m_lpLeftRearFemurTibia->GetDataPointer("JointRotation");
	m_lpfltLeftRearTibiaTarsusRotation = m_lpLeftRearTibiaTarsus->GetDataPointer("JointRotation");
	m_lpfltLeftRearCoxaFemurVelocity = m_lpLeftRearCoxaFemur->GetDataPointer("JointActualVelocity");

	m_lpfltRightFrontCoxaFemurRotation = m_lpRightFrontCoxaFemur->GetDataPointer("JointRotation");
	m_lpfltRightFrontFemurTibiaRotation = m_lpRightFrontFemurTibia->GetDataPointer("JointRotation");
	m_lpfltRightFrontTibiaTarsusRotation = m_lpRightFrontTibiaTarsus->GetDataPointer("JointRotation");

	m_lpfltRightMiddleCoxaFemurRotation = m_lpRightMiddleCoxaFemur->GetDataPointer("JointRotation");
	m_lpfltRightMiddleFemurTibiaRotation = m_lpRightMiddleFemurTibia->GetDataPointer("JointRotation");
	m_lpfltRightMiddleTibiaTarsusRotation = m_lpRightMiddleTibiaTarsus->GetDataPointer("JointRotation");

	m_lpfltRightRearCoxaFemurRotation = m_lpRightRearCoxaFemur->GetDataPointer("JointRotation");
	m_lpfltRightRearFemurTibiaRotation = m_lpRightRearFemurTibia->GetDataPointer("JointRotation");
	m_lpfltRightRearTibiaTarsusRotation = m_lpRightRearTibiaTarsus->GetDataPointer("JointRotation");
	m_lpfltRightRearCoxaFemurVelocity = m_lpRightRearCoxaFemur->GetDataPointer("JointActualVelocity");

	m_lpfltAb1JointRotation = m_lpAb1Joint->GetDataPointer("JointRotation");
	m_lpfltAb2JointRotation = m_lpAb2Joint->GetDataPointer("JointRotation");
	m_lpfltAb3JointRotation = m_lpAb3Joint->GetDataPointer("JointRotation");
	m_lpfltAb4JointRotation = m_lpAb4Joint->GetDataPointer("JointRotation");

	m_lpfltAb1JointVelocity = m_lpAb1Joint->GetDataPointer("JointActualVelocity");

	m_lpfltThoraxTorque = m_lpThorax->GetDataPointer("BodyTorqueX");

	if(m_lpAb1)
		m_lpfltAb1Torque = m_lpAb1->GetDataPointer("BodyTorqueX");

	m_lpbRearFootDown = m_lpLeftTibiaBeta->GetDataPointer("ContactCount");

	m_PitchDelay.Initialize(m_fltAbDelay, lpSim->PhysicsTimeStep());

	//Adjust the desired beta angle by the current thoracic coxa position.
	//m_fltDesiredBeta+=m_fltDesiredDelta;
}

void PostureControlStimulus::Activate(Simulator *lpSim)
{
	ActivateMotor(lpSim, m_lpLeftFrontCoxaFemur);
	ActivateMotor(lpSim, m_lpLeftFrontFemurTibia);
	//ActivateMotor(lpSim, m_lpLeftFrontTibiaTarsus);

	ActivateMotor(lpSim, m_lpLeftMiddleCoxaFemur);
	ActivateMotor(lpSim, m_lpLeftMiddleFemurTibia);
	//ActivateMotor(lpSim, m_lpLeftMiddleTibiaTarsus);

	ActivateMotor(lpSim, m_lpLeftRearThoracicCoxa);
	ActivateMotor(lpSim, m_lpLeftRearCoxaFemur);
	ActivateMotor(lpSim, m_lpLeftRearFemurTibia);
	ActivateMotor(lpSim, m_lpLeftRearTibiaTarsus);

	ActivateMotor(lpSim, m_lpRightFrontCoxaFemur);
	ActivateMotor(lpSim, m_lpRightFrontFemurTibia);
	//ActivateMotor(lpSim, m_lpRightFrontTibiaTarsus);

	ActivateMotor(lpSim, m_lpRightMiddleCoxaFemur);
	ActivateMotor(lpSim, m_lpRightMiddleFemurTibia);
	ActivateMotor(lpSim, m_lpRightRearFemurTibia);
	ActivateMotor(lpSim, m_lpRightRearTibiaTarsus);

	ActivateMotor(lpSim, m_lpRightRearThoracicCoxa);
	ActivateMotor(lpSim, m_lpRightRearCoxaFemur);
	//ActivateMotor(lpSim, m_lpRightRearTibiaTarsus);

	//if(m_bEnableAbControl)
	//{
	//	ActivateMotor(lpSim, m_lpAb1Joint);
	//	ActivateMotor(lpSim, m_lpAb2Joint);
	//	ActivateMotor(lpSim, m_lpAb3Joint);
	//	ActivateMotor(lpSim, m_lpAb4Joint);
	//}
}

void PostureControlStimulus::StepSimulation(Simulator *lpSim)
{
	try
	{
		//Calculate Oft - The angle between the femur-tibia joint
		float a = sqrt( pow((double) (*m_vLeftThoraxCoxaBetaPos[0]-*m_vLeftSemilunarBetaPos[0]), 2.0) + pow((double) (*m_vLeftThoraxCoxaBetaPos[1]-*m_vLeftSemilunarBetaPos[1]), 2.0) + pow((double) (*m_vLeftThoraxCoxaBetaPos[2]-*m_vLeftSemilunarBetaPos[2]), 2.0) );
		float b = sqrt( pow((double) (*m_vLeftSemilunarBetaPos[0]-*m_vLeftTibiaBetaPos[0]), 2.0) +  pow((double) (*m_vLeftSemilunarBetaPos[1]-*m_vLeftTibiaBetaPos[1]), 2.0) + pow((double) (*m_vLeftSemilunarBetaPos[2]-*m_vLeftTibiaBetaPos[2]), 2.0) );
		float c = sqrt( pow((double) (*m_vLeftTibiaBetaPos[0]-*m_vLeftThoraxCoxaBetaPos[0]), 2.0) +  pow((double) (*m_vLeftTibiaBetaPos[1]-*m_vLeftThoraxCoxaBetaPos[1]), 2.0) + pow((double) (*m_vLeftTibiaBetaPos[2]-*m_vLeftThoraxCoxaBetaPos[2]), 2.0) );

		m_fltLeftOftR = acos( (a*a + b*b - c*c)/(2*a*b) );
		m_fltLeftOftD = (m_fltLeftOftR*180)/3.141;

		m_fltLeftHR = acos( (a*a + c*c - b*b)/(2*a*c) );
		m_fltLeftHD = (m_fltLeftHR*180)/3.141;

		m_fltLeftOftR = m_fltLeftHR; // - m_fltDesiredDelta;
		m_fltLeftOftD = (m_fltLeftOftR*180)/3.141;

		a = sqrt( pow((double) (*m_vRightThoraxCoxaBetaPos[0]-*m_vRightSemilunarBetaPos[0]), 2.0) + pow((double) (*m_vRightThoraxCoxaBetaPos[1]-*m_vRightSemilunarBetaPos[1]), 2.0) + pow((double) (*m_vRightThoraxCoxaBetaPos[2]-*m_vRightSemilunarBetaPos[2]), 2.0) );
		b = sqrt( pow((double) (*m_vRightSemilunarBetaPos[0]-*m_vRightTibiaBetaPos[0]), 2.0) + pow((double) (*m_vRightSemilunarBetaPos[1]-*m_vRightTibiaBetaPos[1]), 2.0) + pow((double) (*m_vRightSemilunarBetaPos[2]-*m_vRightTibiaBetaPos[2]), 2.0) );
		c = sqrt( pow((double) (*m_vRightTibiaBetaPos[0]-*m_vRightThoraxCoxaBetaPos[0]), 2.0) + pow((double) (*m_vRightTibiaBetaPos[1]-*m_vRightThoraxCoxaBetaPos[1]), 2.0) + pow((double) (*m_vRightTibiaBetaPos[2]-*m_vRightThoraxCoxaBetaPos[2]), 2.0) );

		m_fltRightOftR = acos( (a*a + b*b - c*c)/(2*a*b) );
		m_fltRightOftD = (m_fltRightOftR*180)/3.141;

		m_fltRightHR = acos( (a*a + c*c - b*b)/(2*a*c) );
		m_fltRightHD = (m_fltRightHR*180)/3.141;

		m_fltRightOftR = m_fltRightHR; // - m_fltDesiredDelta;
		m_fltRightOftD = (m_fltRightOftR*180)/3.141;

		//Calculate Leftbeta. The angle between the ground and the femur-tibia joint.
		float vA[3];
		vA[0] = *m_vLeftThoraxCoxaBetaPos[0];
		vA[1] = *m_vLeftTibiaBetaPos[1];
		vA[2] = *m_vHeadAxisRefPos[2];

		a = sqrt( pow((double) (*m_vLeftThoraxCoxaBetaPos[0]-vA[0]), 2.0) + pow((double) (*m_vLeftThoraxCoxaBetaPos[1]-vA[1]), 2.0) + pow((double) (*m_vLeftThoraxCoxaBetaPos[2]-vA[2]), 2.0) );
		b = sqrt( pow((double) (*m_vLeftTibiaBetaPos[0]-vA[0]), 2.0) + pow((double) (*m_vLeftTibiaBetaPos[1]-vA[1]), 2.0) + pow((double) (*m_vLeftTibiaBetaPos[2]-vA[2]), 2.0) );
		c = sqrt( pow((double) (*m_vLeftTibiaBetaPos[0]-*m_vLeftThoraxCoxaBetaPos[0]), 2.0) + pow((double) (*m_vLeftTibiaBetaPos[1]-*m_vLeftThoraxCoxaBetaPos[1]), 2.0) + pow((double) (*m_vLeftTibiaBetaPos[2]-*m_vLeftThoraxCoxaBetaPos[2]), 2.0) );

		m_fltLeftBetaR = acos( (b*b + c*c - a*a)/(2*b*c) );
		m_fltLeftBetaD = (m_fltLeftBetaR*180)/3.141;

		//Calculate Right Beta
		vA[0] = *m_vRightThoraxCoxaBetaPos[0];
		vA[1] = *m_vRightTibiaBetaPos[1];
		vA[2] = *m_vHeadAxisRefPos[2];

		a = sqrt( pow((double) (*m_vRightThoraxCoxaBetaPos[0]-vA[0]), 2.0) + pow((double) (*m_vRightThoraxCoxaBetaPos[1]-vA[1]), 2.0) + pow((double) (*m_vRightThoraxCoxaBetaPos[2]-vA[2]), 2.0) );
		b = sqrt( pow((double) (*m_vRightTibiaBetaPos[0]-vA[0]), 2.0) + pow((double) (*m_vRightTibiaBetaPos[1]-vA[1]), 2.0) + pow((double) (*m_vRightTibiaBetaPos[2]-vA[2]), 2.0) );
		c = sqrt( pow((double) (*m_vRightTibiaBetaPos[0]-*m_vRightThoraxCoxaBetaPos[0]), 2.0) + pow((double) (*m_vRightTibiaBetaPos[1]-*m_vRightThoraxCoxaBetaPos[1]), 2.0) + pow((double) (*m_vRightTibiaBetaPos[2]-*m_vRightThoraxCoxaBetaPos[2]), 2.0) );

		m_fltRightBetaR = acos( (b*b + c*c - a*a)/(2*b*c) );
		m_fltRightBetaD = (m_fltRightBetaR*180)/3.141;

		
		//Calculate pitch
		CalculatePitch(lpSim);

		//Calculate Yaw
		vA[0] = *m_vTailAxisRefPos[0];
		vA[1] = *m_vHeadAxisRefPos[1];
		vA[2] = *m_vHeadAxisRefPos[2];

		a = Std_Sign(*m_vHeadAxisRefPos[0]-*m_vTailAxisRefPos[0])*sqrt( pow((double) (*m_vTailAxisRefPos[0]-*m_vHeadAxisRefPos[0]), 2.0) + pow((double) (*m_vTailAxisRefPos[1]-*m_vHeadAxisRefPos[1]), 2.0) + pow((double) (*m_vTailAxisRefPos[2]-*m_vHeadAxisRefPos[2]), 2.0) );
		b = sqrt( pow((double) (*m_vHeadAxisRefPos[0]-vA[0]), 2.0) + pow((double) (*m_vHeadAxisRefPos[1]-vA[1]), 2.0) + pow((double) (*m_vHeadAxisRefPos[2]-vA[2]), 2.0) );

		if(a != 0)
			m_fltYawR = asin(b/a);
		else
			m_fltYawR = 0;
		m_fltYawD = (m_fltYawR*180)/3.141;

		//Calculate Roll
		vA[0] = *m_vRollAxisRefPos[0];
		vA[1] = *m_vCOMAxisPos[1];
		vA[2] = *m_vRollAxisRefPos[2];

		a = Std_Sign(*m_vRollAxisRefPos[1]-*m_vCOMAxisPos[1])*sqrt( pow((double) (*m_vRollAxisRefPos[0]-*m_vCOMAxisPos[0]), 2.0) + pow((double) (*m_vRollAxisRefPos[1]-*m_vCOMAxisPos[1]), 2.0) + pow((double) (*m_vRollAxisRefPos[2]-*m_vCOMAxisPos[2]), 2.0) );
		b = sqrt( pow((double) (*m_vRollAxisRefPos[0]-vA[0]), 2.0) + pow((double) (*m_vRollAxisRefPos[1]-vA[1]), 2.0) + pow((double) (*m_vRollAxisRefPos[2]-vA[2]), 2.0) );

		if(a != 0)
			m_fltRollR = asin(b/a);
		else
			m_fltRollR = 0;
		m_fltRollD = (m_fltRollR*180)/3.141;

		
		if(!RearFemurTibiaDisabled && (lpSim->Time() > 50e-3) )
		{
			RearFemurTibiaDisabled = TRUE;
			DeactivateMotor(lpSim, m_lpRightRearFemurTibia);
			DeactivateMotor(lpSim, m_lpLeftRearFemurTibia);
		}

		//This code determines when a jump  is immiment.
		if(!m_bTendonLockEnabled)
		{
			//If we have not yet enabled the tendon lock then we are still in the cocking phase.
			if(*m_lpbLeftTendonLockEnabled > 0 && *m_lpbRightTendonLockEnabled > 0)
			{
				m_fltLeftRearThoracicCoxaPos = 0;
				m_fltRightRearThoracicCoxaPos = 0;
				m_bTendonLockEnabled = TRUE;
				m_bTendonLockEnabledTime = lpSim->Time();
			}
		}
		else
		{
			//If we are here then we have finished cocking and we are now waiting for triggering.
			if( (*m_lpbLeftTendonLockEnabled <= 0 && *m_lpbRightTendonLockEnabled <= 0) && !m_bTendonLockDisabled)
			{
				m_bTendonLockDisabled = TRUE;
				m_fltTendonDisabledTime = lpSim->Time();
			}
		}

		//Once the tendon lock is enabled then the cocking phase is over, so we can now calculate
		//the joint angles we will need to get the correct beta angle.
		if(m_bTendonLockEnabled)
		{
			if(lpSim->Time() - m_bTendonLockEnabledTime > 10e-3)
			{
				//Once the tendon lock is engaged then we want to control the coxa-femur joint to ensure the beta angle.
				if(!m_bRearCoxaFemurEnabled)
				{
					//Adjust the thorax-coxa joint to the desired delta angle.
					m_fltLeftRearThoracicCoxaPos = m_fltDesiredDelta;
					m_fltRightRearThoracicCoxaPos = m_fltDesiredDelta;

					ActivateMotor(lpSim, m_lpLeftRearCoxaFemur);
					ActivateMotor(lpSim, m_lpRightRearCoxaFemur);

					m_bRearCoxaFemurEnabled = TRUE;
				}
			}

			if(lpSim->Time() - m_bTendonLockEnabledTime > 30e-3)
				CalculateFeedbackAngles(lpSim);

			//After 500 ms move the rear tarsus down so it touches the ground
			if(lpSim->Time() - m_bTendonLockEnabledTime > 800e-3)
			{
				if(m_lpfltLeftRearTarsusDown && m_lpfltLeftRearTarsusDown && !m_bMoveTarsusStarted && (*m_lpfltLeftRearTarsusDown <= 0) && (*m_lpfltLeftRearTarsusDown <= 0) )
				{
					DeactivateMotor(lpSim, m_lpLeftRearTibiaTarsus);
					DeactivateMotor(lpSim, m_lpRightRearTibiaTarsus);
					//DeactivateMotor(lpSim, m_lpLeftMiddleCoxaFemur);
					//DeactivateMotor(lpSim, m_lpRightMiddleCoxaFemur);
					m_bMoveTarsusStarted = TRUE;
				}
				
				////When the left or right tarsus touches the ground and is at rest for ten steps then lock it down.
				//if(*m_lpfltLeftRearTarsusDown >= 1.0f && *m_lpfltRightRearTarsusDown >= 1.0f)
				//	m_iTarsusCounter++;
				//if(m_iTarsusCounter == 10)
				//{
				//	m_fltLeftRearTibiaTarsusPos = *m_lpfltLeftRearTibiaTarsusRotation;
				//	m_fltRightRearTibiaTarsusPos = *m_lpfltRightRearTibiaTarsusRotation;
				//	SetMotorPosition(lpSim, m_lpLeftRearTibiaTarsus, m_fltLeftRearTibiaTarsusPos);
				//	SetMotorPosition(lpSim, m_lpRightRearTibiaTarsus, m_fltRightRearTibiaTarsusPos);
				//	ActivateMotor(lpSim, m_lpLeftRearTibiaTarsus);
				//	ActivateMotor(lpSim, m_lpRightRearTibiaTarsus);
				//}
			}
		}

		if(!m_bTendonLockDisabled)
		{
			//This code controls the joints until the tendon lock is disabled so the jump will occur.
			SetMotorPosition(lpSim, m_lpLeftFrontCoxaFemur, m_fltLeftFrontCoxaFemurPos);
			SetMotorPosition(lpSim, m_lpLeftFrontFemurTibia, m_fltLeftFrontFemurTibiaPos);
			SetMotorPosition(lpSim, m_lpLeftFrontTibiaTarsus, m_fltLeftFrontTibiaTarsusPos);

			SetMotorPosition(lpSim, m_lpLeftMiddleCoxaFemur, m_fltLeftMiddleCoxaFemurPos);
			SetMotorPosition(lpSim, m_lpLeftMiddleFemurTibia, m_fltLeftMiddleFemurTibiaPos);
			SetMotorPosition(lpSim, m_lpLeftMiddleTibiaTarsus, m_fltLeftMiddleTibiaTarsusPos);

			SetMotorPosition(lpSim, m_lpLeftRearThoracicCoxa, m_fltLeftRearThoracicCoxaPos);
			SetMotorPosition(lpSim, m_lpLeftRearCoxaFemur, m_fltLeftRearCoxaFemurPos);
			SetMotorPosition(lpSim, m_lpLeftRearTibiaTarsus, m_fltLeftRearTibiaTarsusPos);

			SetMotorPosition(lpSim, m_lpRightFrontCoxaFemur, m_fltRightFrontCoxaFemurPos);
			SetMotorPosition(lpSim, m_lpRightFrontFemurTibia, m_fltRightFrontFemurTibiaPos);
			SetMotorPosition(lpSim, m_lpRightFrontTibiaTarsus, m_fltRightFrontTibiaTarsusPos);

			SetMotorPosition(lpSim, m_lpRightMiddleCoxaFemur, m_fltRightMiddleCoxaFemurPos);
			SetMotorPosition(lpSim, m_lpRightMiddleFemurTibia, m_fltRightMiddleFemurTibiaPos);
			SetMotorPosition(lpSim, m_lpRightMiddleTibiaTarsus, m_fltRightMiddleTibiaTarsusPos);

			SetMotorPosition(lpSim, m_lpRightRearThoracicCoxa, m_fltRightRearThoracicCoxaPos);
			SetMotorPosition(lpSim, m_lpRightRearCoxaFemur, m_fltRightRearCoxaFemurPos);
			SetMotorPosition(lpSim, m_lpRightRearTibiaTarsus, m_fltRightRearTibiaTarsusPos);
		}
		else
		{

			//As soon as the tendon lock is disabled then disable all the motors controlling the posture.
			//Then, 40 ms after the locust has taken off lets move the rear femur to be even with the body, and 
			//pull the tibia in.
			if(lpSim->Time() - m_fltTendonDisabledTime > 6e-3)
			{
				if(m_bActiveInFlightMotor)
				{
					if(lpSim->Time() - m_fltTendonDisabledTime < 150e-3 && m_bMotorsDisabled)
						ReactiveInFlightMotors(lpSim);
				
					if(lpSim->Time() - m_fltTendonDisabledTime > 150e-3 && !m_bMotorsDisabled)
						DeactiveInFlightMotors(lpSim);
				}
			}
			else if(!m_bMotorsDisabled)
				DeactivateMotors(lpSim);

			//After the tendon lock is disabled we want the abdomen control system to become active.
			if(m_bEnableAbControl)
				SetAbdomenPositions(lpSim);
		}
	}
	catch(...)
	{
		LOG_ERROR("Error Occurred while setting Joint Velocity");
	}
}

void PostureControlStimulus::CalculatePitch(Simulator *lpSim)
{
	CalculateNewPitch(lpSim);
	CalculateOldPitch(lpSim);
}


void PostureControlStimulus::CalculateNewPitch(Simulator *lpSim)
{
	float vA[3];
	float a,b,c;

	//if(lpSim->Time() > 0.235)
	//	a = 0;

	vA[0] = *m_vPronotumFrontRefPos[0];
	vA[1] = *m_vPronotumRearRefPos[1];
	vA[2] = *m_vPronotumRearRefPos[2]+m_fltPitchPointOffset;

	a = sqrt( pow((double) (*m_vPronotumFrontRefPos[0]-vA[0]), 2.0) + pow((double) (*m_vPronotumFrontRefPos[1]-vA[1]), 2.0) + pow((double) (*m_vPronotumFrontRefPos[2]-vA[2]), 2.0) );
	b = sqrt( pow((double) (*m_vPronotumRearRefPos[0]-vA[0]), 2.0) + pow((double) (*m_vPronotumRearRefPos[1]-vA[1]), 2.0) + pow((double) (*m_vPronotumRearRefPos[2]-vA[2]), 2.0) );
	c = sqrt( pow((double) (*m_vPronotumRearRefPos[0]-*m_vPronotumFrontRefPos[0]), 2.0) + pow((double) (*m_vPronotumRearRefPos[1]-*m_vPronotumFrontRefPos[1]), 2.0) + pow((double) (*m_vPronotumRearRefPos[2]-*m_vPronotumFrontRefPos[2]), 2.0) );

	float fltOldPitchR = m_fltHeadPitchR;
	float fltOldPitchD = m_fltHeadPitchD;

	m_fltHeadPitchR = Std_Sign(*m_vPronotumFrontRefPos[1]-*m_vPronotumRearRefPos[1])*acos( (b*b + c*c - a*a)/(2*b*c) );
	m_fltHeadPitchD = (m_fltHeadPitchR*180)/3.141;

	m_fltPitchVelR = (m_fltHeadPitchR - fltOldPitchR)/lpSim->TimeStep();
	m_fltPitchVelD = (m_fltHeadPitchD - fltOldPitchD)/lpSim->TimeStep();

	//Add this pitch to the delay line.
	m_PitchDelay.AddValue(*m_lpfltAb1JointVelocity);
}


//This is the old method of calculating the body pitch. I am leaving this in here for backwards compatibility with old projects
void PostureControlStimulus::CalculateOldPitch(Simulator *lpSim)
{
	float vA[3];
	float a,b;

	vA[0] = *m_vHeadAxisRefPos[0];
	vA[1] = *m_vCOMAxisPos[1];
	vA[2] = *m_vHeadAxisRefPos[2];

	a = Std_Sign(*m_vHeadAxisRefPos[1]-*m_vCOMAxisPos[1])*sqrt( pow((double) (*m_vCOMAxisPos[0]-*m_vHeadAxisRefPos[0]), 2.0) + pow((double) (*m_vCOMAxisPos[1]-*m_vHeadAxisRefPos[1]), 2.0) + pow((double) (*m_vCOMAxisPos[2]-*m_vHeadAxisRefPos[2]), 2.0) );
	b = sqrt( pow((double) (*m_vHeadAxisRefPos[0]-vA[0]), 2.0) + pow((double) (*m_vHeadAxisRefPos[1]-vA[1]), 2.0) + pow((double) (*m_vHeadAxisRefPos[2]-vA[2]), 2.0) );
	
	if(a != 0)
		m_fltPitchR = asin(b/a);
	else
		m_fltPitchR = 0;
	m_fltPitchD = (m_fltPitchR*180)/3.141;

	//Add this pitch to the delay line.
//	m_PitchDelay.AddValue(m_fltPitchR);
}

void PostureControlStimulus::CalculateInitialJointAngles(Simulator *lpSim)
{
	//m_fltLeftRearThoracicCoxaPos = (PI/2) - (PI - m_fltDesiredDelta - (PI/2) + m_fltDesiredBeta);
	//m_fltRightRearThoracicCoxaPos = (PI/2) - (PI - m_fltDesiredDelta - (PI/2) + m_fltDesiredBeta);

	//m_fltLeftRearThoracicCoxaPos = 0.873;
	//m_fltRightRearThoracicCoxaPos = 0.873;

	m_fltLeftRearCoxaFemurPos = m_fltLeftOftR + m_fltLeftRearThoracicCoxaPos;
	m_fltRightRearCoxaFemurPos = m_fltRightOftR + m_fltRightRearThoracicCoxaPos;

	//Adjust the desired beta angle by the current thoracic coxa position.
	m_fltDesiredBeta+=m_fltLeftRearThoracicCoxaPos;

	//m_fltRightRearCoxaFemurPos = 0.6f;
	//m_fltLeftRearCoxaFemurPos = 0.6f;

	//Adjust Middle legs
	float fltLeftD = *m_vLeftMiddleCoxaFemurPos[1] - *m_vLeftTibiaBetaPos[1];
	//float fltLeftA = sqrt( pow((double) (*m_vLeftMiddleCoxaFemurPos[0]-*m_vLeftMiddleFemurTibiaPos[0]), 2.0) + pow((double) (*m_vLeftMiddleCoxaFemurPos[1]-*m_vLeftMiddleFemurTibiaPos[1]), 2.0) + pow((double) (*m_vLeftMiddleCoxaFemurPos[2]-*m_vLeftMiddleFemurTibiaPos[2]), 2.0) );
	//float fltLeftB = sqrt( pow((double) (*m_vLeftMiddleFemurTibiaPos[0]-*m_vLeftMiddleTibiaTarsusPos[0]), 2.0) + pow((double) (*m_vLeftMiddleFemurTibiaPos[1]-*m_vLeftMiddleTibiaTarsusPos[1]), 2.0) + pow((double) (*m_vLeftMiddleFemurTibiaPos[2]-*m_vLeftMiddleTibiaTarsusPos[2]), 2.0) );

	//float fltRightD = *m_vRightMiddleCoxaFemurPos[1] - *m_vRightTibiaBetaPos[1];
	//float fltRightA = sqrt( pow((double) (*m_vRightMiddleCoxaFemurPos[0]-*m_vRightMiddleFemurTibiaPos[0]), 2.0) + pow((double) (*m_vRightMiddleCoxaFemurPos[1]-*m_vRightMiddleFemurTibiaPos[1]), 2.0) + pow((double) (*m_vRightMiddleCoxaFemurPos[2]-*m_vRightMiddleFemurTibiaPos[2]), 2.0) );
	//float fltRightB = sqrt( pow((double) (*m_vRightMiddleFemurTibiaPos[0]-*m_vRightMiddleTibiaTarsusPos[0]), 2.0) + pow((double) (*m_vRightMiddleFemurTibiaPos[1]-*m_vRightMiddleTibiaTarsusPos[1]), 2.0) + pow((double) (*m_vRightMiddleFemurTibiaPos[2]-*m_vRightMiddleTibiaTarsusPos[2]), 2.0) );
 
	m_fltLeftMiddleCoxaFemurPos = asin( (m_fltLeftMiddleTibiaLength*cos(PI/4)-fltLeftD)/m_fltLeftMiddleFemurLength );
	m_fltRightMiddleCoxaFemurPos = -m_fltLeftMiddleCoxaFemurPos;
	//m_fltRightMiddleCoxaFemurPos = -asin( (fltRightB*cos(PI/4)-fltRightD)/fltRightA );

	m_fltLeftMiddleFemurTibiaPos = 0;
	m_fltRightMiddleFemurTibiaPos = 0;


	fltLeftD = *m_vLeftFrontCoxaFemurPos[1] - *m_vLeftTibiaBetaPos[1];

	m_fltLeftFrontCoxaFemurPos = asin( (m_fltLeftFrontTibiaLength*cos(PI/4)-fltLeftD)/m_fltLeftFrontFemurLength );
	m_fltRightFrontCoxaFemurPos = m_fltLeftFrontCoxaFemurPos;

	m_fltLeftFrontFemurTibiaPos = 0;
	m_fltRightFrontFemurTibiaPos = 0;

	//Override the calculated values with pre-set ones for the moment
	//m_fltLeftFrontCoxaFemurPos = 0.7f;
	//m_fltRightFrontCoxaFemurPos = m_fltLeftFrontCoxaFemurPos;
	//m_fltLeftMiddleCoxaFemurPos = 0.67f;
	//m_fltRightMiddleCoxaFemurPos = -m_fltLeftMiddleCoxaFemurPos;

	//m_fltLeftFrontCoxaFemurPos = 0.8f;
	//m_fltRightFrontCoxaFemurPos = m_fltLeftFrontCoxaFemurPos;
	//m_fltLeftMiddleCoxaFemurPos = 0.67f;
	//m_fltRightMiddleCoxaFemurPos = -m_fltLeftMiddleCoxaFemurPos;

}

/*
void PostureControlStimulus::CalculateFeedbackAngles(Simulator *lpSim)
{
	float fltDelta=0;

	m_bChangeBeta = FALSE;
	m_bChangePitch = FALSE;
	m_fltPitchChange=0;
	m_fltLeftBetaChange=0;
	m_fltRightBetaChange=0;

	if( !m_bChangeBeta && !m_bChangePitch && (fabs(m_fltRightBetaR - m_fltDesiredBeta) > 1e-4f || fabs(m_fltLeftBetaR - m_fltDesiredBeta) > 1e-4f) )
		m_bChangeBeta = TRUE;

	if( !m_bChangeBeta && !m_bChangePitch && fabs(m_fltPitchR - m_fltDesiredPitch) > 1e-4f )
		m_bChangePitch = TRUE;

	//Since the middle legs are out of the way we should only be using the front and rear legs
	//to support the locust. The angles of the rear thorax-coxa, and coxa-femur joints are going 
	//to determine the beta angle, while the angles of the front coxa-femur will determine the 
	//body elevation, or pitch.
	if(m_bChangePitch)
	{
		m_fltPitchChange = m_fltGain*10*(m_fltPitchR - m_fltDesiredPitch);

		if(m_fltPitchChange > MAX_CHANGE)
			m_fltPitchChange = MAX_CHANGE;
		if(m_fltPitchChange < -MAX_CHANGE)
			m_fltPitchChange = -MAX_CHANGE;

		m_fltLeftFrontCoxaFemurPos += m_fltPitchChange;
		m_fltRightFrontCoxaFemurPos += m_fltPitchChange;
	}

	//Lets ensure that the beta angles are the ones that the user specified.
	if(m_bChangeBeta)
	{
		m_fltLeftBetaChange = m_fltGain * (m_fltLeftBetaR - m_fltDesiredBeta);

		//if(m_fltLeftBetaChange > MAX_CHANGE)
		//	m_fltLeftBetaChange = MAX_CHANGE;
		//if(m_fltLeftBetaChange < -MAX_CHANGE)
		//	m_fltLeftBetaChange = -MAX_CHANGE;

		m_fltLeftRearCoxaFemurPos+=m_fltLeftBetaChange;

		m_fltRightBetaChange = m_fltGain * (m_fltRightBetaR - m_fltDesiredBeta);

		//if(m_fltRightBetaChange > MAX_CHANGE)
		//	m_fltRightBetaChange = MAX_CHANGE;
		//if(m_fltRightBetaChange < -MAX_CHANGE)
		//	m_fltRightBetaChange = -MAX_CHANGE;

		m_fltRightRearCoxaFemurPos+=m_fltRightBetaChange;
	}
}
*/ 


void PostureControlStimulus::CalculateFeedbackAngles(Simulator *lpSim)
{
	float fltDelta=0;

	//Since the middle legs are out of the way we should only be using the front and rear legs
	//to support the locust. The angles of the rear thorax-coxa, and coxa-femur joints are going 
	//to determine the beta angle, while the angles of the front coxa-femur will determine the 
	//body elevation, or pitch.
	m_fltPitchChange = m_fltGain*(m_fltPitchR - m_fltDesiredPitch);

	if(m_fltPitchChange > MAX_CHANGE)
		m_fltPitchChange = MAX_CHANGE;
	if(m_fltPitchChange < -MAX_CHANGE)
		m_fltPitchChange = -MAX_CHANGE;

	m_fltLeftFrontCoxaFemurPos += m_fltPitchChange;
	m_fltRightFrontCoxaFemurPos += m_fltPitchChange;
 
	//Lets ensure that the beta angles are the ones that the user specified.
	if(lpSim->Time() - m_bTendonLockEnabledTime < 600e-3)
	{ 
		m_fltLeftBetaChange = m_fltGain * (m_fltLeftBetaR - m_fltDesiredBeta);

		if(m_fltLeftBetaChange > MAX_CHANGE)
			m_fltLeftBetaChange = MAX_CHANGE;
		if(m_fltLeftBetaChange < -MAX_CHANGE)
			m_fltLeftBetaChange = -MAX_CHANGE;

		m_fltLeftRearCoxaFemurPos+=m_fltLeftBetaChange;

		m_fltRightBetaChange = m_fltGain * (m_fltRightBetaR - m_fltDesiredBeta);

		if(m_fltRightBetaChange > MAX_CHANGE)
			m_fltRightBetaChange = MAX_CHANGE;
		if(m_fltRightBetaChange < -MAX_CHANGE)
			m_fltRightBetaChange = -MAX_CHANGE;

		m_fltRightRearCoxaFemurPos+=m_fltRightBetaChange;
	}
	else
	{ 
		m_fltLeftBetaChange = m_fltLeftBetaChange;
	}

}


void PostureControlStimulus::Deactivate(Simulator *lpSim)
{
	DeactivateMotors(lpSim);
}

void PostureControlStimulus::DeactivateMotors(Simulator *lpSim)
{
	DeactivateMotor(lpSim, m_lpLeftFrontCoxaFemur);
	DeactivateMotor(lpSim, m_lpLeftFrontFemurTibia);
	DeactivateMotor(lpSim, m_lpLeftFrontTibiaTarsus);

	DeactivateMotor(lpSim, m_lpLeftMiddleCoxaFemur);
	DeactivateMotor(lpSim, m_lpLeftMiddleFemurTibia);
	DeactivateMotor(lpSim, m_lpLeftMiddleTibiaTarsus);

	//DeactivateMotor(lpSim, m_lpLeftRearThoracicCoxa);
	DeactivateMotor(lpSim, m_lpLeftRearCoxaFemur);
	DeactivateMotor(lpSim, m_lpLeftRearTibiaTarsus);

	DeactivateMotor(lpSim, m_lpRightFrontCoxaFemur);
	DeactivateMotor(lpSim, m_lpRightFrontFemurTibia);
	DeactivateMotor(lpSim, m_lpRightFrontTibiaTarsus);

	DeactivateMotor(lpSim, m_lpRightMiddleCoxaFemur);
	DeactivateMotor(lpSim, m_lpRightMiddleFemurTibia);
	DeactivateMotor(lpSim, m_lpRightMiddleTibiaTarsus);

	//DeactivateMotor(lpSim, m_lpRightRearThoracicCoxa);
	DeactivateMotor(lpSim, m_lpRightRearCoxaFemur);
	DeactivateMotor(lpSim, m_lpRightRearTibiaTarsus);

	if(m_bLockAbJump && !m_bEnableAbControl)
	{
		ActivateMotor(lpSim, m_lpAb1Joint);
		ActivateMotor(lpSim, m_lpAb2Joint);
		ActivateMotor(lpSim, m_lpAb3Joint);
		ActivateMotor(lpSim, m_lpAb4Joint);
	}

	m_bMotorsDisabled = TRUE;
}

void PostureControlStimulus::ReactiveInFlightMotors(Simulator *lpSim)
{
	
	ActivateMotor(lpSim, m_lpLeftRearThoracicCoxa);
	ActivateMotor(lpSim, m_lpLeftRearCoxaFemur);
	//ActivateMotor(lpSim, m_lpLeftRearFemurTibia);

	ActivateMotor(lpSim, m_lpRightRearThoracicCoxa);
	ActivateMotor(lpSim, m_lpRightRearCoxaFemur);
	//ActivateMotor(lpSim, m_lpRightRearFemurTibia);

	m_fltLeftRearThoracicCoxaPos = 0;
	m_fltLeftRearCoxaFemurPos = 0;
	m_fltLeftRearFemurTibiaPos = *m_lpfltLeftRearFemurTibiaRotation;

	m_fltRightRearThoracicCoxaPos = 0;
	m_fltRightRearCoxaFemurPos = 0;
	m_fltRightRearFemurTibiaPos = *m_lpfltRightRearFemurTibiaRotation;

	SetMotorPosition(lpSim, m_lpLeftRearThoracicCoxa, m_fltLeftRearThoracicCoxaPos);
	SetMotorPosition(lpSim, m_lpLeftRearCoxaFemur, m_fltLeftRearCoxaFemurPos);
	//SetMotorPosition(lpSim, m_lpLeftRearFemurTibia, m_fltLeftRearFemurTibiaPos);

	SetMotorPosition(lpSim, m_lpRightRearThoracicCoxa, m_fltRightRearThoracicCoxaPos);
	SetMotorPosition(lpSim, m_lpRightRearCoxaFemur, m_fltRightRearCoxaFemurPos);
	//SetMotorPosition(lpSim, m_lpRightRearFemurTibia, m_fltRightRearFemurTibiaPos);

	m_bMotorsDisabled = FALSE;
}

void PostureControlStimulus::DeactiveInFlightMotors(Simulator *lpSim)
{
	DeactivateMotor(lpSim, m_lpLeftRearThoracicCoxa);
	DeactivateMotor(lpSim, m_lpLeftRearCoxaFemur);
	//DeactivateMotor(lpSim, m_lpLeftRearFemurTibia);
	
	DeactivateMotor(lpSim, m_lpRightRearThoracicCoxa);
	DeactivateMotor(lpSim, m_lpRightRearCoxaFemur);
	//DeactivateMotor(lpSim, m_lpRightRearFemurTibia);

	m_bMotorsDisabled = TRUE;
}

//Torque ab control
//void PostureControlStimulus::SetAbdomenPositions(Simulator *lpSim)
//{
//	float fltTime = (lpSim->Time() - m_fltTendonDisabledTime);
//	if( fltTime >= m_fltAbStart && fltTime <= (m_fltAbStart+m_fltAbPeriod))
//	{
//		m_lpAb1->AddTorque(lpSim, -m_fltAbPropGain*(*m_lpfltAb1Torque), 0, 0);
//		m_lpThorax->AddTorque(lpSim, -m_fltAbPropGain*(*m_lpfltThoraxTorque), 0, 0);
//	}
//}


//Motor ab control
void PostureControlStimulus::SetAbdomenPositions(Simulator *lpSim)
{
	//Abdomen Control system
	//float fltPitchVel = m_PitchDelay.ReadValue();
	//float fltTime = (lpSim->Time() - m_fltTendonDisabledTime);

	//if(*m_lpbRearFootDown == TRUE)
	//{
	//	//if(fltPitchVel < -5)
	//	//{
	//		*m_lpDorsalAbCurrent -= m_lpAbCurrentAdd;
	//		m_lpAbCurrentAdd = (-m_fltAbPropGain*fltPitchVel*1e-9);
 //			*m_lpDorsalAbCurrent += m_lpAbCurrentAdd;
	//	//}
	//	//else
	//	//{
	//	//	*m_lpDorsalAbCurrent -= m_lpAbCurrentAdd;
	//	//	m_lpAbCurrentAdd = 0;
	//	//}
	//}

	//if(*m_lpbRearFootDown == FALSE && m_fltFootTime < 0)
	//	m_fltFootTime = fltTime;

	////Once the legs have left the ground re-activate the motors and get the current leg amplitudes
	//if(*m_lpbRearFootDown == FALSE && (fltTime-m_fltFootTime) >= 0)
	//{
	//	if(!m_bSetAbMag)
	//	{
	//		if(*m_lpfltAb1JointVelocity > 0)
	//		{
	//			m_fltAbMag = *m_lpfltAb1JointVelocity;
	//			m_lpAb1Joint->ServoMotor(FALSE);
	//			ActivateMotor(lpSim, m_lpAb1Joint);
	//			SetMotorPosition(lpSim, m_lpAb1Joint, m_fltAbMag);
	//			m_fltAbTime = fltTime;
	//			m_bSetAbMag = TRUE;
	//		}
	//	}
	//	else if(*m_lpbRearFootDown == FALSE  && fltTime <= (m_fltAbTime+m_fltLegPeriod) && !m_bDeactivateAbMag)
	//	{
	//		m_fltAbVel = m_fltAbMag*cos( (PI*(fltTime-m_fltAbTime))/(2*m_fltLegPeriod) );
	//		SetMotorPosition(lpSim, m_lpAb1Joint, m_fltAbVel);
	//	}
	//	else if(*m_lpbRearFootDown == FALSE  && fltTime > (m_fltAbTime+m_fltLegPeriod) && !m_bDeactivateAbMag)
	//	{
	//		//DeactivateMotor(lpSim, m_lpAb1Joint);
	//		m_bDeactivateAbMag = TRUE;
	//	}
	//}
}

void PostureControlStimulus::ActivateMotor(Simulator *lpSim, Joint *lpJoint)
{
	lpJoint->EnableMotor(TRUE);
	lpJoint->DesiredVelocity(0);
}

void PostureControlStimulus::SetMotorPosition(Simulator *lpSim, Joint *lpJoint, float fltPos)
{
	try
	{
		if(!lpJoint->UsesRadians())
			fltPos *= lpSim->InverseDistanceUnits();

		lpJoint->DesiredVelocity(fltPos);
	}
	catch(...)
	{
		LOG_ERROR("Error Occurred while setting Joint Velocity");
	}
}


void PostureControlStimulus::DeactivateMotor(Simulator *lpSim, Joint *lpJoint)
{
	lpJoint->DesiredVelocity(0);
	lpJoint->EnableMotor(FALSE);
}

float *PostureControlStimulus::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	if(strType == "LEFTBETAD")
		lpData = &m_fltLeftBetaD;
	else if(strType == "LEFTBETAR")
		lpData = &m_fltLeftBetaR;
	else if(strType == "RIGHTBETAD")
		lpData = &m_fltRightBetaD;
	else if(strType == "RIGHTBETAR")
		lpData = &m_fltRightBetaR;
	else if(strType == "LEFTOFTD")
		lpData = &m_fltLeftOftD;
	else if(strType == "LEFTOFTR")
		lpData = &m_fltLeftOftR;
	else if(strType == "RIGHTOFTD")
		lpData = &m_fltRightOftD;
	else if(strType == "RIGHTOFTR")
		lpData = &m_fltRightOftR;
	else if(strType == "PITCHD")
		lpData = &m_fltHeadPitchD;
	else if(strType == "PITCHR")
		lpData = &m_fltHeadPitchR;
	else if(strType == "PITCHVELD")
		lpData = &m_fltPitchVelD;
	else if(strType == "PITCHVELR")
		lpData = &m_fltPitchVelR;
	else if(strType == "YAWD")
		lpData = &m_fltYawD;
	else if(strType == "YAWR")
		lpData = &m_fltYawR;
	else if(strType == "ROLLD")
		lpData = &m_fltRollD;
	else if(strType == "ROLLR")
		lpData = &m_fltRollR;
	else if(strType == "LEFTBETACHANGE")
		lpData = &m_fltLeftBetaChange;
	else if(strType == "RIGHTBETACHANGE")
		lpData = &m_fltRightBetaChange;
	else if(strType == "PITCHCHANGE")
		lpData = &m_fltPitchChange;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "StimulusName: " + STR(m_strName) + "  DataType: " + strDataType);

	return lpData;
} 
void PostureControlStimulus::Load(Simulator *lpSim, CStdXml &oXml)
{
	AnimatLibrary::ActivatedItem::Load(lpSim, oXml);

	oXml.IntoElem();  //Into Simulus Element

	m_strStructureID = oXml.GetChildString("StructureID");
	if(Std_IsBlank(m_strStructureID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_fltDesiredBeta = oXml.GetChildFloat("Beta");
	m_fltDesiredDelta = oXml.GetChildFloat("Delta");
	m_fltDesiredPitch = oXml.GetChildFloat("Pitch");

	m_fltAbDelay = oXml.GetChildFloat("AbDelay");
	m_fltAbPropGain = oXml.GetChildFloat("AbPropGain");
	m_fltAbPeriod = oXml.GetChildFloat("AbPeriod");
	m_fltLegPeriod = oXml.GetChildFloat("LegPeriod");

	m_strThoraxID = oXml.GetChildString("Thorax");
	m_strAb1ID = oXml.GetChildString("Ab1", "");
	m_strLeftTendonLockID = oXml.GetChildString("LeftTendonLock");
	m_strRightTendonLockID = oXml.GetChildString("RightTendonLock");

	m_strHeadAxisRefID = oXml.GetChildString("HeadAxisRef");
	m_strTailAxisRefID = oXml.GetChildString("TailAxisRef");
	m_strCOMRefID = oXml.GetChildString("COMRef");
	m_strRollAxisRefID = oXml.GetChildString("RollAxisRef");
	m_strPronotumFrontRefID = oXml.GetChildString("PronotumFrontRef", "");
	m_strPronotumRearRefID = oXml.GetChildString("PronotumRearRef", "");

	m_strLeftThoraxCoxaBetaID = oXml.GetChildString("LeftThoraxCoxaBeta");
	m_strLeftSemilunarBetaID = oXml.GetChildString("LeftSemilunarBeta");
	m_strLeftTibiaBetaID = oXml.GetChildString("LeftTibiaBeta");
	m_strRightThoraxCoxaBetaID = oXml.GetChildString("RightThoraxCoxaBeta");
	m_strRightSemilunarBetaID = oXml.GetChildString("RightSemilunarBeta");
	m_strRightTibiaBetaID = oXml.GetChildString("RightTibiaBeta");

	m_strLeftFrontFemurID = oXml.GetChildString("LeftFrontFemur");
	m_strLeftFrontTibiaID = oXml.GetChildString("LeftFrontTibia");
	m_strLeftMiddleFemurID = oXml.GetChildString("LeftMiddleFemur");
	m_strLeftMiddleTibiaID = oXml.GetChildString("LeftMiddleTibia");

	//Joints of the left front leg.
	m_strLeftFrontCoxaFemurID = oXml.GetChildString("LeftFrontCoxaFemur");
	m_strLeftFrontFemurTibiaID = oXml.GetChildString("LeftFrontFemurTibia");
	m_strLeftFrontTibiaTarsusID = oXml.GetChildString("LeftFrontTibiaTarsus");

	//Joints of the left middle leg.
	m_strLeftMiddleCoxaFemurID = oXml.GetChildString("LeftMiddleCoxaFemur");
	m_strLeftMiddleFemurTibiaID = oXml.GetChildString("LeftMiddleFemurTibia");
	m_strLeftMiddleTibiaTarsusID = oXml.GetChildString("LeftMiddleTibiaTarsus");

	//Joints of the left rear leg.
	m_strLeftRearThoracicCoxaID = oXml.GetChildString("LeftRearThoracicCoxa");
	m_strLeftRearCoxaFemurID = oXml.GetChildString("LeftRearCoxaFemur");
	m_strLeftSemilunarJointID = oXml.GetChildString("LeftSemilunarJoint");
	m_strLeftRearFemurTibiaID = oXml.GetChildString("LeftRearFemurTibia");
	m_strLeftRearTibiaTarsusID = oXml.GetChildString("LeftRearTibiaTarsus");

	//Joints of the Right front leg.
	m_strRightFrontCoxaFemurID = oXml.GetChildString("RightFrontCoxaFemur");
	m_strRightFrontFemurTibiaID = oXml.GetChildString("RightFrontFemurTibia");
	m_strRightFrontTibiaTarsusID = oXml.GetChildString("RightFrontTibiaTarsus");

	//Joints of the Right middle leg.
	m_strRightMiddleCoxaFemurID = oXml.GetChildString("RightMiddleCoxaFemur");
	m_strRightMiddleFemurTibiaID = oXml.GetChildString("RightMiddleFemurTibia");
	m_strRightMiddleTibiaTarsusID = oXml.GetChildString("RightMiddleTibiaTarsus");

	//Joints of the Right rear leg.
	m_strRightRearThoracicCoxaID = oXml.GetChildString("RightRearThoracicCoxa");
	m_strRightRearCoxaFemurID = oXml.GetChildString("RightRearCoxaFemur");
	m_strRightSemilunarJointID = oXml.GetChildString("RightSemilunarJoint");
	m_strRightRearFemurTibiaID = oXml.GetChildString("RightRearFemurTibia");
	m_strRightRearTibiaTarsusID = oXml.GetChildString("RightRearTibiaTarsus");

	m_strLeftMiddleFootDownID = oXml.GetChildString("LeftMiddleFootDown");
	m_strLeftFrontFootDownID = oXml.GetChildString("LeftFrontFootDown");
	m_strRightMiddleFootDownID = oXml.GetChildString("RightMiddleFootDown");
	m_strRightFrontFootDownID = oXml.GetChildString("RightFrontFootDown");

	m_strLeftRearTarsusDownID = oXml.GetChildString("LeftRearTarsusDown", "");
	m_strRightRearTarsusDownID = oXml.GetChildString("RightRearTarsusDown", "");

	m_strAb1JointID = oXml.GetChildString("Ab1Joint");
	m_strAb2JointID = oXml.GetChildString("Ab2Joint");
	m_strAb3JointID = oXml.GetChildString("Ab3Joint");
	m_strAb4JointID = oXml.GetChildString("Ab4Joint");

	m_lDorsalAbNeuronID = oXml.GetChildLong("DorsalAbNeuron", 0);
	//m_lVentralAbNeuronID = oXml.GetChildLong("VentralAbNeuron");
	
	m_bEnableAbControl = oXml.GetChildBool("EnableAbControl", FALSE);
	m_bLockAbJump = oXml.GetChildBool("LockAbJump", FALSE);
	m_bTumblingSetup = oXml.GetChildBool("TumblingSetup", FALSE);

	if(m_bTumblingSetup)
	{
		m_fltLeftRearCoxaFemurPos = 0.5f;
		m_fltRightRearCoxaFemurPos =  0.5f;
	}
	else
	{
		m_fltLeftRearCoxaFemurPos = 1.0f;
		m_fltRightRearCoxaFemurPos =  1.0f;
	}

	oXml.OutOfElem(); //OutOf Simulus Element

}

void PostureControlStimulus::Save(Simulator *lpSim, CStdXml &oXml)
{
}

	}			//ExternalStimuli
}				//VortexAnimatLibrary




