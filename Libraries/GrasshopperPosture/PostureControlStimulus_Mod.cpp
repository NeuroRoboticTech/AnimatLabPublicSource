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
	m_lpStructure = NULL;

	m_fltStepRate = 0.0005f;
	m_fltDesiredBeta = 0;
	m_fltDesiredDelta = 0;
	m_fltDesiredPitch = 0;
	m_fltGain = 0.002f;

	m_lpThorax = NULL;
	m_lpLeftTendonLock = NULL;
	m_lpRightTendonLock = NULL;

	m_lpHeadAxisRef = NULL;
	m_lpTailAxisRef = NULL;
	m_lpCOMRef = NULL;
	m_lpRollAxisRef = NULL;

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

	m_fltLeftGammaD = 0;
	m_fltLeftGammaR = 0;
	m_fltRightGammaD = 0;
	m_fltRightGammaR = 0;

	m_fltLeftBetaD=0;
	m_fltLeftBetaR=0;
	m_fltRightBetaD=0;
	m_fltRightBetaR=0;

	m_bChangePitch = FALSE;
	m_bChangeBeta = FALSE;
	m_fltLeftBetaChange = 0;
	m_fltRightBetaChange = 0;
	m_fltPitchChange = 0;
	m_fltAlphaR = 0;
	m_fltAlphaD = 0;

	m_fltPitchD=0;
	m_fltPitchR=0;
	m_fltYawD=0;
	m_fltYawR=0;
	m_fltRollD=0;
	m_fltRollR=0;

	m_lpbLeftTendonLockEnabled = NULL;
	m_lpbRightTendonLockEnabled = NULL;
	m_fltTendonDisabledTime = 0;
	m_bTendonLockEnabledTime = 0;
	m_bMotorsDisabled = FALSE;
	m_bThrustThruCOM = FALSE;
	m_fltFrontRearLegJointHeight = 0;
	m_fltFrontLeftLegHeight = 0;
	m_fltFrontRightLegHeight = 0;
	m_fltEffectiveCoxaLength = 0;
	m_fltCoxaTibiaGapLength = 0;
	m_fltAvgCTGapLength = 0;

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
		m_vLeftRearTCJointPos[i] = NULL;
		m_vRightRearTCJointPos[i] = NULL;

		m_vHeadAxisRefPos[i] = NULL;
		m_vTailAxisRefPos[i] = NULL;
		m_vCOMAxisPos[i] = NULL;
		m_vRollAxisRefPos[i] = NULL;

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
	m_fltLeftRearFemurLength = 0;
	m_fltLeftRearTibiaLength = 0;
	m_fltDistComFront = 0;

	m_bTendonLockEnabled = FALSE;
	m_bTendonLockDisabled = FALSE;
	m_bRearCoxaFemurEnabled = FALSE;

	m_lpfltLeftFrontFootDown = NULL;
	m_lpfltLeftMiddleFootDown = NULL;
	m_lpfltRightFrontFootDown = NULL;
	m_lpfltRightMiddleFootDown = NULL;

	m_fltLeftFrontCoxaFemurPos = 0.7f;
	m_fltLeftFrontCoxaFemurPosTarget = 0.7f;
	m_fltLeftFrontFemurTibiaPos = 0.1f;
	m_fltLeftFrontFemurTibiaPosTarget = 0.1f;
	m_fltLeftFrontTibiaTarsusPos = 0.6f;

	m_fltLeftMiddleCoxaFemurPos = 0.42f;
	m_fltLeftMiddleFemurTibiaPos = 0.50f;
	m_fltLeftMiddleTibiaTarsusPos = 0.6f;

	m_fltLeftRearThoracicCoxaPos = 0;
	m_fltLeftRearCoxaFemurPos = 0.26f;
	m_fltLeftRearFemurTibiaPos = 0;
	m_fltLeftRearTibiaTarsusPos = -0.8f;

	m_fltRightFrontCoxaFemurPos = 0.7f;
	m_fltRightFrontFemurTibiaPos = -0.10f;
	m_fltRightFrontFemurTibiaPosTarget = -0.1f;
	m_fltRightFrontTibiaTarsusPos = 0.6f;
 
	m_fltRightMiddleCoxaFemurPos = -0.42f;
	m_fltRightMiddleFemurTibiaPos = -0.50f;
	m_fltRightMiddleTibiaTarsusPos = 0.6f;

	m_fltRightRearThoracicCoxaPos = 0;
	m_fltRightRearCoxaFemurPos = 0.26f;
	m_fltRightRearFemurTibiaPos = 0;
	m_fltRightRearTibiaTarsusPos = -0.8f;

	m_lpfltLeftFrontCoxaFemurRotation = NULL;
	m_lpfltLeftFrontFemurTibiaRotation = NULL;
	m_lpfltLeftFrontTibiaTarsusRotation = NULL;

	m_lpfltLeftMiddleCoxaFemurRotation = NULL;
	m_lpfltLeftMiddleFemurTibiaRotation = NULL;
	m_lpfltLeftMiddleTibiaTarsusRotation = NULL;

	m_lpfltLeftRearCoxaFemurRotation = NULL;
	m_lpfltLeftRearFemurTibiaRotation = NULL;
	m_lpfltLeftRearTibiaTarsusRotation = NULL;

	m_lpfltRightFrontCoxaFemurRotation = NULL;
	m_lpfltRightFrontFemurTibiaRotation = NULL;
	m_lpfltRightFrontTibiaTarsusRotation = NULL;

	m_lpfltRightMiddleCoxaFemurRotation = NULL;
	m_lpfltRightMiddleFemurTibiaRotation = NULL;
	m_lpfltRightMiddleTibiaTarsusRotation = NULL;

	m_lpfltRightRearCoxaFemurRotation = NULL;
	m_lpfltRightRearFemurTibiaRotation = NULL;
	m_lpfltRightRearTibiaTarsusRotation = NULL;

	m_fltRightRearCoxaFemurPosTarget = m_fltRightRearCoxaFemurPos;
	m_fltLeftRearCoxaFemurPosTarget = m_fltLeftRearCoxaFemurPos;
	m_fltLeftFrontCoxaFemurPosTarget = m_fltLeftFrontCoxaFemurPos;
	m_fltRightFrontCoxaFemurPosTarget = m_fltRightFrontCoxaFemurPos;
}

PostureControlStimulus::~PostureControlStimulus()
{

try
{
	m_lpStructure = NULL;
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
	m_lpStructure = lpSim->FindStructureFromAll(m_strStructureID);

	//Now lets get the body parts that will be needed to perform calculations.
	m_lpThorax = m_lpStructure->FindRigidBody(m_strThoraxID);
	m_lpLeftTendonLock = m_lpStructure->FindRigidBody(m_strLeftTendonLockID);
	m_lpRightTendonLock = m_lpStructure->FindRigidBody(m_strRightTendonLockID);

	m_lpHeadAxisRef = m_lpStructure->FindRigidBody(m_strHeadAxisRefID);
	m_lpTailAxisRef = m_lpStructure->FindRigidBody(m_strTailAxisRefID);
	m_lpCOMRef = m_lpStructure->FindRigidBody(m_strCOMRefID);
	m_lpRollAxisRef = m_lpStructure->FindRigidBody(m_strRollAxisRefID);

	m_lpLeftThoraxCoxaBeta = m_lpStructure->FindRigidBody(m_strLeftThoraxCoxaBetaID);
	m_lpLeftSemilunarBeta = m_lpStructure->FindRigidBody(m_strLeftSemilunarBetaID);
	m_lpLeftTibiaBeta = m_lpStructure->FindRigidBody(m_strLeftTibiaBetaID);
	m_lpRightThoraxCoxaBeta = m_lpStructure->FindRigidBody(m_strRightThoraxCoxaBetaID);
	m_lpRightSemilunarBeta = m_lpStructure->FindRigidBody(m_strRightSemilunarBetaID);
	m_lpRightTibiaBeta = m_lpStructure->FindRigidBody(m_strRightTibiaBetaID);

	m_lpLeftFrontFemur = dynamic_cast<Mesh *>(m_lpStructure->FindRigidBody(m_strLeftFrontFemurID));
	m_lpLeftFrontTibia = dynamic_cast<Mesh *>(m_lpStructure->FindRigidBody(m_strLeftFrontTibiaID));
	m_lpLeftMiddleFemur = dynamic_cast<Mesh *>(m_lpStructure->FindRigidBody(m_strLeftMiddleFemurID));
	m_lpLeftMiddleTibia = dynamic_cast<Mesh *>(m_lpStructure->FindRigidBody(m_strLeftMiddleTibiaID));
	m_lpLeftRearCoxa = dynamic_cast<Box *>(m_lpStructure->FindRigidBody(m_strLeftRearCoxaID));
	m_lpLeftRearFemur = dynamic_cast<Mesh *>(m_lpStructure->FindRigidBody(m_strLeftRearFemurID));
	m_lpLeftRearTibia = dynamic_cast<Mesh *>(m_lpStructure->FindRigidBody(m_strLeftRearTibiaID));

	//Now lets get the foot down sensors.
	m_lpLeftMiddleFootDown = m_lpStructure->FindRigidBody(m_strLeftMiddleFootDownID);
	m_lpLeftFrontFootDown = m_lpStructure->FindRigidBody(m_strLeftFrontFootDownID);
	m_lpRightMiddleFootDown = m_lpStructure->FindRigidBody(m_strRightMiddleFootDownID);
	m_lpRightFrontFootDown = m_lpStructure->FindRigidBody(m_strRightFrontFootDownID);

	//Left front leg joints.
	m_lpLeftFrontCoxaFemur = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strLeftFrontCoxaFemurID));
	m_lpLeftFrontFemurTibia = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strLeftFrontFemurTibiaID));
	m_lpLeftFrontTibiaTarsus = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strLeftFrontTibiaTarsusID));

	//Left Middle leg joints.
	m_lpLeftMiddleCoxaFemur = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strLeftMiddleCoxaFemurID));
	m_lpLeftMiddleFemurTibia = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strLeftMiddleFemurTibiaID));
	m_lpLeftMiddleTibiaTarsus = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strLeftMiddleTibiaTarsusID));

	//Left Rear leg joints.
	m_lpLeftRearThoracicCoxa = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strLeftRearThoracicCoxaID));
	m_lpLeftRearCoxaFemur = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strLeftRearCoxaFemurID));
	m_lpLeftSemilunarJoint = dynamic_cast<VsPrismatic *>(m_lpStructure->FindJoint(m_strLeftSemilunarJointID));
	m_lpLeftRearFemurTibia = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strLeftRearFemurTibiaID));
	m_lpLeftRearTibiaTarsus = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strLeftRearTibiaTarsusID));

	//Right front leg joints.
	m_lpRightFrontCoxaFemur = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strRightFrontCoxaFemurID));
	m_lpRightFrontFemurTibia = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strRightFrontFemurTibiaID));
	m_lpRightFrontTibiaTarsus = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strRightFrontTibiaTarsusID));

	//Right Middle leg joints.
	m_lpRightMiddleCoxaFemur = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strRightMiddleCoxaFemurID));
	m_lpRightMiddleFemurTibia = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strRightMiddleFemurTibiaID));
	m_lpRightMiddleTibiaTarsus = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strRightMiddleTibiaTarsusID));

	//Right Rear leg joints.
	m_lpRightRearThoracicCoxa = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strRightRearThoracicCoxaID));
	m_lpRightRearCoxaFemur = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strRightRearCoxaFemurID));
	m_lpRightSemilunarJoint = dynamic_cast<VsPrismatic *>(m_lpStructure->FindJoint(m_strRightSemilunarJointID));
	m_lpRightRearFemurTibia = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strRightRearFemurTibiaID));
	m_lpRightRearTibiaTarsus = dynamic_cast<VsHinge *>(m_lpStructure->FindJoint(m_strRightRearTibiaTarsusID));

	CStdFPoint oA = m_lpLeftFrontCoxaFemur->AbsolutePosition();
	CStdFPoint oB = m_lpLeftFrontFemurTibia->AbsolutePosition();
	m_fltLeftFrontFemurLength = Std_CalculateDistance(oA, oB)*lpSim->DistanceUnits();

	oA = m_lpLeftFrontFemurTibia->AbsolutePosition();
	oB = m_lpLeftFrontFootDown->AbsolutePosition();
	m_fltLeftFrontTibiaLength = Std_CalculateDistance(oA, oB)*lpSim->DistanceUnits();

	oA = m_lpLeftMiddleCoxaFemur->AbsolutePosition();
	oB = m_lpLeftMiddleFemurTibia->AbsolutePosition();
	m_fltLeftMiddleFemurLength = Std_CalculateDistance(oA, oB)*lpSim->DistanceUnits();

	oA = m_lpLeftMiddleFemurTibia->AbsolutePosition();
	oB = m_lpLeftMiddleFootDown->AbsolutePosition();
	m_fltLeftMiddleTibiaLength = Std_CalculateDistance(oA, oB)*lpSim->DistanceUnits();

	oA = m_lpLeftRearCoxaFemur->AbsolutePosition();
	oB = m_lpLeftRearFemurTibia->AbsolutePosition();
	m_fltLeftRearFemurLength = Std_CalculateDistance(oA, oB)*lpSim->DistanceUnits();

	oA = m_lpLeftRearFemurTibia->AbsolutePosition();
	oB = m_lpLeftTibiaBeta->AbsolutePosition();
	m_fltLeftRearTibiaLength = Std_CalculateDistance(oA, oB)*lpSim->DistanceUnits();

	oA = m_lpLeftFrontCoxaFemur->AbsolutePosition();
	oB = m_lpLeftRearThoracicCoxa->AbsolutePosition();
	m_fltDistComFront = (oA.z-oB.z)*lpSim->DistanceUnits();

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
		m_vLeftRearTCJointPos[i] = m_lpLeftRearThoracicCoxa->GetDataPointer(strPosData);
		m_vRightRearTCJointPos[i] = m_lpRightRearThoracicCoxa->GetDataPointer(strPosData);

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
	}

	m_lpbLeftTendonLockEnabled = m_lpLeftTendonLock->GetDataPointer("ENABLE");
	m_lpbRightTendonLockEnabled = m_lpRightTendonLock->GetDataPointer("ENABLE");

	m_lpfltLeftFrontFootDown = m_lpLeftFrontFootDown->GetDataPointer("ContactCount");
	m_lpfltLeftMiddleFootDown = m_lpLeftMiddleFootDown->GetDataPointer("ContactCount");
	m_lpfltRightFrontFootDown = m_lpRightFrontFootDown->GetDataPointer("ContactCount");
	m_lpfltRightMiddleFootDown = m_lpRightMiddleFootDown->GetDataPointer("ContactCount");

	m_lpfltLeftFrontCoxaFemurRotation = m_lpLeftFrontCoxaFemur->GetDataPointer("JointRotation");
	m_lpfltLeftFrontFemurTibiaRotation = m_lpLeftFrontFemurTibia->GetDataPointer("JointRotation");
	m_lpfltLeftFrontTibiaTarsusRotation = m_lpLeftFrontTibiaTarsus->GetDataPointer("JointRotation");

	m_lpfltLeftMiddleCoxaFemurRotation = m_lpLeftMiddleCoxaFemur->GetDataPointer("JointRotation");
	m_lpfltLeftMiddleFemurTibiaRotation = m_lpLeftMiddleFemurTibia->GetDataPointer("JointRotation");
	m_lpfltLeftMiddleTibiaTarsusRotation = m_lpLeftMiddleTibiaTarsus->GetDataPointer("JointRotation");

	m_lpfltLeftRearCoxaFemurRotation = m_lpLeftRearCoxaFemur->GetDataPointer("JointRotation");
	m_lpfltLeftRearFemurTibiaRotation = m_lpLeftRearFemurTibia->GetDataPointer("JointRotation");
	m_lpfltLeftRearTibiaTarsusRotation = m_lpLeftRearTibiaTarsus->GetDataPointer("JointRotation");

	m_lpfltRightFrontCoxaFemurRotation = m_lpRightFrontCoxaFemur->GetDataPointer("JointRotation");
	m_lpfltRightFrontFemurTibiaRotation = m_lpRightFrontFemurTibia->GetDataPointer("JointRotation");
	m_lpfltRightFrontTibiaTarsusRotation = m_lpRightFrontTibiaTarsus->GetDataPointer("JointRotation");

	m_lpfltRightMiddleCoxaFemurRotation = m_lpRightMiddleCoxaFemur->GetDataPointer("JointRotation");
	m_lpfltRightMiddleFemurTibiaRotation = m_lpRightMiddleFemurTibia->GetDataPointer("JointRotation");
	m_lpfltRightMiddleTibiaTarsusRotation = m_lpRightMiddleTibiaTarsus->GetDataPointer("JointRotation");

	m_lpfltRightRearCoxaFemurRotation = m_lpRightRearCoxaFemur->GetDataPointer("JointRotation");
	m_lpfltRightRearFemurTibiaRotation = m_lpRightRearFemurTibia->GetDataPointer("JointRotation");
	m_lpfltRightRearTibiaTarsusRotation = m_lpRightRearTibiaTarsus->GetDataPointer("JointRotation");

	m_fltLeftRearThoracicCoxaPos = -(m_fltDesiredDelta + m_fltDesiredPitch);
	m_fltRightRearThoracicCoxaPos = m_fltLeftRearThoracicCoxaPos;
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
	////ActivateMotor(lpSim, m_lpLeftRearCoxaFemur);
	//ActivateMotor(lpSim, m_lpLeftRearTibiaTarsus);

	ActivateMotor(lpSim, m_lpRightFrontCoxaFemur);
	ActivateMotor(lpSim, m_lpRightFrontFemurTibia);
	//ActivateMotor(lpSim, m_lpRightFrontTibiaTarsus);

	ActivateMotor(lpSim, m_lpRightMiddleCoxaFemur);
	ActivateMotor(lpSim, m_lpRightMiddleFemurTibia);
	//ActivateMotor(lpSim, m_lpRightMiddleTibiaTarsus);

	ActivateMotor(lpSim, m_lpRightRearThoracicCoxa);
	////ActivateMotor(lpSim, m_lpRightRearCoxaFemur);
	//ActivateMotor(lpSim, m_lpRightRearTibiaTarsus);
}

void PostureControlStimulus::StepSimulation(Simulator *lpSim)
{
	try
	{
		//Calculate Oft - The angle between the femur-tibia joint
		float a = sqrt( pow((double) (*m_vLeftThoraxCoxaBetaPos[0]-*m_vLeftSemilunarBetaPos[0]), 2.0) + pow((double) (*m_vLeftThoraxCoxaBetaPos[1]-*m_vLeftSemilunarBetaPos[1]), 2.0) + pow((double) (*m_vLeftThoraxCoxaBetaPos[2]-*m_vLeftSemilunarBetaPos[2]), 2.0) );
		float b = sqrt( pow((double) (*m_vLeftSemilunarBetaPos[0]-*m_vLeftTibiaBetaPos[0]), 2.0) +  pow((double) (*m_vLeftSemilunarBetaPos[1]-*m_vLeftTibiaBetaPos[1]), 2.0) + pow((double) (*m_vLeftSemilunarBetaPos[2]-*m_vLeftTibiaBetaPos[2]), 2.0) );
		float c = sqrt( pow((double) (*m_vLeftTibiaBetaPos[0]-*m_vLeftThoraxCoxaBetaPos[0]), 2.0) +  pow((double) (*m_vLeftTibiaBetaPos[1]-*m_vLeftThoraxCoxaBetaPos[1]), 2.0) + pow((double) (*m_vLeftTibiaBetaPos[2]-*m_vLeftThoraxCoxaBetaPos[2]), 2.0) );

		m_fltLeftRearFemurLength = a;
		m_fltLeftRearTibiaLength = b;

		m_fltLeftOftR = (PI/2) + *m_lpfltLeftRearFemurTibiaRotation;
		m_fltLeftOftD = (m_fltLeftOftR*180)/PI;

		m_fltLeftGammaR = acos( (a*a + c*c - b*b)/(2*a*c) );
		m_fltLeftGammaD = (m_fltLeftGammaR*180)/PI;

		a = sqrt( pow((double) (*m_vRightThoraxCoxaBetaPos[0]-*m_vRightSemilunarBetaPos[0]), 2.0) + pow((double) (*m_vRightThoraxCoxaBetaPos[1]-*m_vRightSemilunarBetaPos[1]), 2.0) + pow((double) (*m_vRightThoraxCoxaBetaPos[2]-*m_vRightSemilunarBetaPos[2]), 2.0) );
		b = sqrt( pow((double) (*m_vRightSemilunarBetaPos[0]-*m_vRightTibiaBetaPos[0]), 2.0) + pow((double) (*m_vRightSemilunarBetaPos[1]-*m_vRightTibiaBetaPos[1]), 2.0) + pow((double) (*m_vRightSemilunarBetaPos[2]-*m_vRightTibiaBetaPos[2]), 2.0) );
		c = sqrt( pow((double) (*m_vRightTibiaBetaPos[0]-*m_vRightThoraxCoxaBetaPos[0]), 2.0) + pow((double) (*m_vRightTibiaBetaPos[1]-*m_vRightThoraxCoxaBetaPos[1]), 2.0) + pow((double) (*m_vRightTibiaBetaPos[2]-*m_vRightThoraxCoxaBetaPos[2]), 2.0) );
		
		m_fltRightOftR = (PI/2) + *m_lpfltRightRearFemurTibiaRotation;
		m_fltRightOftD = (m_fltRightOftR*180)/PI;

		m_fltRightGammaR = acos( (a*a + c*c - b*b)/(2*a*c) );
		m_fltRightGammaD = (m_fltRightGammaR*180)/PI;

		a = sqrt( pow((double) (*m_vRightThoraxCoxaBetaPos[0]-*m_vRightRearTCJointPos[0]), 2.0) + pow((double) (*m_vRightThoraxCoxaBetaPos[1]-*m_vRightRearTCJointPos[1]), 2.0) + pow((double) (*m_vRightThoraxCoxaBetaPos[2]-*m_vRightRearTCJointPos[2]), 2.0) );
		b = *m_vRightThoraxCoxaBetaPos[0]-*m_vRightRearTCJointPos[0];

		m_fltRightOtcR = asin(b/a);
		m_fltRightOtcD = (m_fltRightOtcR*180)/PI;

		a = sqrt( pow((double) (*m_vLeftThoraxCoxaBetaPos[0]-*m_vLeftRearTCJointPos[0]), 2.0) + pow((double) (*m_vLeftThoraxCoxaBetaPos[1]-*m_vLeftRearTCJointPos[1]), 2.0) + pow((double) (*m_vLeftThoraxCoxaBetaPos[2]-*m_vLeftRearTCJointPos[2]), 2.0) );
		b = *m_vLeftThoraxCoxaBetaPos[1]-*m_vLeftRearTCJointPos[1];

		m_fltLeftOtcR = asin(b/a);
		m_fltLeftOtcD = (m_fltLeftOtcR*180)/PI;


		//Calculate Leftbeta. The angle between the ground and the femur-tibia joint.
		float vA[3];
		vA[0] = *m_vLeftThoraxCoxaBetaPos[0];
		vA[1] = *m_vLeftTibiaBetaPos[1];
		vA[2] = *m_vHeadAxisRefPos[2];

		a = sqrt( pow((double) (*m_vLeftThoraxCoxaBetaPos[0]-vA[0]), 2.0) + pow((double) (*m_vLeftThoraxCoxaBetaPos[1]-vA[1]), 2.0) + pow((double) (*m_vLeftThoraxCoxaBetaPos[2]-vA[2]), 2.0) );
		b = sqrt( pow((double) (*m_vLeftTibiaBetaPos[0]-vA[0]), 2.0) + pow((double) (*m_vLeftTibiaBetaPos[1]-vA[1]), 2.0) + pow((double) (*m_vLeftTibiaBetaPos[2]-vA[2]), 2.0) );
		c = sqrt( pow((double) (*m_vLeftTibiaBetaPos[0]-*m_vLeftThoraxCoxaBetaPos[0]), 2.0) + pow((double) (*m_vLeftTibiaBetaPos[1]-*m_vLeftThoraxCoxaBetaPos[1]), 2.0) + pow((double) (*m_vLeftTibiaBetaPos[2]-*m_vLeftThoraxCoxaBetaPos[2]), 2.0) );

		m_fltLeftBetaR = acos( (b*b + c*c - a*a)/(2*b*c) );
		m_fltLeftBetaD = (m_fltLeftBetaR*180)/PI;

		//Calculate Right Beta
		vA[0] = *m_vRightThoraxCoxaBetaPos[0];
		vA[1] = *m_vRightTibiaBetaPos[1];
		vA[2] = *m_vHeadAxisRefPos[2];

		a = sqrt( pow((double) (*m_vRightThoraxCoxaBetaPos[0]-vA[0]), 2.0) + pow((double) (*m_vRightThoraxCoxaBetaPos[1]-vA[1]), 2.0) + pow((double) (*m_vRightThoraxCoxaBetaPos[2]-vA[2]), 2.0) );
		b = sqrt( pow((double) (*m_vRightTibiaBetaPos[0]-vA[0]), 2.0) + pow((double) (*m_vRightTibiaBetaPos[1]-vA[1]), 2.0) + pow((double) (*m_vRightTibiaBetaPos[2]-vA[2]), 2.0) );
		c = sqrt( pow((double) (*m_vRightTibiaBetaPos[0]-*m_vRightThoraxCoxaBetaPos[0]), 2.0) + pow((double) (*m_vRightTibiaBetaPos[1]-*m_vRightThoraxCoxaBetaPos[1]), 2.0) + pow((double) (*m_vRightTibiaBetaPos[2]-*m_vRightThoraxCoxaBetaPos[2]), 2.0) );

		m_fltRightBetaR = acos( (b*b + c*c - a*a)/(2*b*c) );
		m_fltRightBetaD = (m_fltRightBetaR*180)/PI;

		
		//Calculate pitch
		vA[0] = *m_vHeadAxisRefPos[0];
		vA[1] = *m_vCOMAxisPos[1];
		vA[2] = *m_vHeadAxisRefPos[2];

		a = Std_Sign(*m_vHeadAxisRefPos[1]-*m_vCOMAxisPos[1])*sqrt( pow((double) (*m_vCOMAxisPos[0]-*m_vHeadAxisRefPos[0]), 2.0) + pow((double) (*m_vCOMAxisPos[1]-*m_vHeadAxisRefPos[1]), 2.0) + pow((double) (*m_vCOMAxisPos[2]-*m_vHeadAxisRefPos[2]), 2.0) );
		b = sqrt( pow((double) (*m_vHeadAxisRefPos[0]-vA[0]), 2.0) + pow((double) (*m_vHeadAxisRefPos[1]-vA[1]), 2.0) + pow((double) (*m_vHeadAxisRefPos[2]-vA[2]), 2.0) );
		
		if(a != 0)
			m_fltPitchR = asin(b/a);
		else
			m_fltPitchR = 0;
		m_fltPitchD = (m_fltPitchR*180)/PI;

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
		m_fltYawD = (m_fltYawR*180)/PI;

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
		m_fltRollD = (m_fltRollR*180)/PI;

		m_fltCoxaTibiaGapLength = sqrt( pow((double) (*m_vLeftTibiaBetaPos[0]-*m_vLeftThoraxCoxaBetaPos[0]), 2.0) + pow((double) (*m_vLeftTibiaBetaPos[1]-*m_vLeftThoraxCoxaBetaPos[1]), 2.0) + pow((double) (*m_vLeftTibiaBetaPos[2]-*m_vLeftThoraxCoxaBetaPos[2]), 2.0) );

		//Update the front femur-tibia positions to meet thier targets
		float fltDelta = m_fltLeftFrontFemurTibiaPos - m_fltLeftFrontFemurTibiaPosTarget;
		m_fltLeftFrontFemurTibiaPos -= (fltDelta*m_fltStepRate);
		fltDelta = m_fltLeftFrontCoxaFemurPos - m_fltLeftFrontCoxaFemurPosTarget;
		m_fltLeftFrontCoxaFemurPos -= (fltDelta*m_fltStepRate);
		fltDelta = m_fltRightFrontFemurTibiaPos - m_fltRightFrontFemurTibiaPosTarget;
		m_fltRightFrontFemurTibiaPos -= (fltDelta*m_fltStepRate);
		fltDelta = m_fltRightRearCoxaFemurPos - m_fltRightRearCoxaFemurPosTarget;
		m_fltRightRearCoxaFemurPos -= (fltDelta*m_fltStepRate);
		fltDelta = m_fltLeftRearCoxaFemurPos - m_fltLeftRearCoxaFemurPosTarget;
		m_fltLeftRearCoxaFemurPos -= (fltDelta*m_fltStepRate);
		fltDelta = m_fltLeftFrontCoxaFemurPos - m_fltLeftFrontCoxaFemurPosTarget;
		m_fltLeftFrontCoxaFemurPos -= (fltDelta*m_fltStepRate);
		fltDelta = m_fltRightFrontCoxaFemurPos - m_fltRightFrontCoxaFemurPosTarget;
		m_fltRightFrontCoxaFemurPos -= (fltDelta*m_fltStepRate);

		/*
		if( (lpSim->Time() > 100e-3) && (lpSim->Time() > 101e-3) )
		{
			m_fltStepRate = 0.005;
			m_fltLeftFrontCoxaFemurPosTarget = 0.8f;
			m_fltLeftFrontFemurTibiaPosTarget = -0.7f;
		}
		*/
		/*
		if( (lpSim->Time() > 200e-3) && (lpSim->Time() > 201e-3) )
		{
			m_fltLeftFrontCoxaFemurPosTarget = 1.0f;
			m_fltLeftFrontFemurTibiaPosTarget = 1.4f;
			m_fltRightFrontFemurTibiaPosTarget = 0.45f;		
		}
		*/

		//This code determines when a jump  is immiment.
		if(!m_bTendonLockEnabled)
		{
			//If we have not yet enabled the tendon lock then we are still in the cocking phase.
			if(*m_lpbLeftTendonLockEnabled > 0)
			{
				m_bTendonLockEnabled = TRUE;
				m_bTendonLockEnabledTime = lpSim->Time();
			}
		}
		else
		{
			//If we are here then we have finished cocking and we are now waiting for triggering.
			if(*m_lpbLeftTendonLockEnabled <= 0 && !m_bTendonLockDisabled)
			{
				m_bTendonLockDisabled = TRUE;
				m_fltTendonDisabledTime = lpSim->Time();
			}
		}

		//Once the tendon lock is enabled then the cocking phase is over, so we can now calculate
		//the joint angles we will need to get the correct beta angle.
		if(m_bTendonLockEnabled)
		{
			//Once the tendon lock is engaged then we want to control the coxa-femur joint to ensure the beta angle.
			if(!m_bRearCoxaFemurEnabled && (lpSim->Time() > 500e-3) )
			{
				ActivateMotor(lpSim, m_lpLeftRearCoxaFemur);
				ActivateMotor(lpSim, m_lpRightRearCoxaFemur);

				//De-activate the middle leg joints so they simply move freely while the front and rear
				//legs determine the posture.
				DeactivateMotor(lpSim, m_lpLeftMiddleCoxaFemur);
				DeactivateMotor(lpSim, m_lpRightMiddleCoxaFemur);

				m_bRearCoxaFemurEnabled = TRUE;

				CalculateJointAngles(lpSim, m_fltDesiredDelta);
			}
			/*
			if( (lpSim->Time() > 600e-3) && (lpSim->Time() > 601e-3) )
			{
				m_fltLeftFrontFemurTibiaPosTarget = 0.65f;
				m_fltRightFrontFemurTibiaPosTarget = 0.45f;		
			}
			*/
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
			if(lpSim->Time() - m_fltTendonDisabledTime > 100e-3)
			{
				//if(m_bMotorsDisabled)
				//	ReactiveInFlightMotors(lpSim);
				//else
				//{
				//	m_fltLeftRearCoxaFemurPos = 0;
				//	m_fltRightRearCoxaFemurPos = 0;
 
				//	SetMotorPosition(lpSim, m_lpLeftRearThoracicCoxa, m_fltLeftRearThoracicCoxaPos);
				//	SetMotorPosition(lpSim, m_lpLeftRearCoxaFemur, m_fltLeftRearCoxaFemurPos);
				//	SetMotorPosition(lpSim, m_lpLeftRearFemurTibia, m_fltLeftRearFemurTibiaPos);

				//	SetMotorPosition(lpSim, m_lpRightRearThoracicCoxa, m_fltRightRearThoracicCoxaPos);
				//	SetMotorPosition(lpSim, m_lpRightRearCoxaFemur, m_fltRightRearCoxaFemurPos);
				//	SetMotorPosition(lpSim, m_lpRightRearFemurTibia, m_fltRightRearFemurTibiaPos);
				//}
			}
			else if(!m_bMotorsDisabled)
				DeactivateMotors(lpSim);
		}
	}
	catch(...)
	{
		LOG_ERROR("Error Occurred while setting Joint Velocity");
	}
}

void PostureControlStimulus::CalculateJointAngles(Simulator *lpSim, float fltDelta)
{
	m_fltLeftFrontFemurTibiaPosTarget = m_fltLeftFrontFemurTibiaPos;  //0.65f;  //
	m_fltRightFrontFemurTibiaPosTarget = m_fltRightFrontFemurTibiaPos; //0.45f;		//

	float fltGamma = acos( (m_fltLeftRearFemurLength*m_fltLeftRearFemurLength + m_fltAvgCTGapLength*m_fltAvgCTGapLength - m_fltLeftRearTibiaLength*m_fltLeftRearTibiaLength)/(2*m_fltLeftRearFemurLength*m_fltAvgCTGapLength) );

	m_fltAlphaR = PI - fltDelta - m_fltDesiredBeta;
	m_fltAlphaD = (m_fltAlphaR*180)/PI;

	m_fltLeftRearCoxaFemurPosTarget = fltGamma + m_fltAlphaR - PI;
	m_fltRightRearCoxaFemurPosTarget = m_fltLeftRearCoxaFemurPosTarget;

	//Calcualte the thoracic-coxa, and femur-tibia angles of the front legs
	//to have a specified body pitch, yaw, and roll.

	//Find the height from the ground to the thoracic-coxa joint when the rear leg is at the specified
	//delta-pitch-beta angles.  
	float fltLeftFrontOft = (PI/2) - m_fltLeftFrontFemurTibiaPosTarget;
	float fltLeftRearH = m_fltAvgCTGapLength*sin(m_fltDesiredBeta) + m_fltEffectiveCoxaLength*sin(-fltDelta);
	
	//Caluclate the height we will need to add to get the body to have the correct pitch value.
	float fltLeftHDelta = m_fltDistComFront*tan(m_fltDesiredPitch);
	m_fltFrontLeftLegHeight = fltLeftRearH + m_fltFrontRearLegJointHeight+fltLeftHDelta;

	float fltLeftDiag = sqrt(m_fltLeftFrontFemurLength*m_fltLeftFrontFemurLength + m_fltLeftFrontTibiaLength*m_fltLeftFrontTibiaLength - 2*m_fltLeftFrontFemurLength*m_fltLeftFrontTibiaLength*cos(fltLeftFrontOft)); 
	float fltLeftO1 = asin( (m_fltLeftFrontTibiaLength * sin(fltLeftFrontOft))/fltLeftDiag );
	float fltLeftO2 = acos(m_fltFrontLeftLegHeight/fltLeftDiag);
	m_fltLeftFrontCoxaFemurPosTarget = fltLeftO1 + fltLeftO2 - (PI/2);


	//Do the same for the right leg
	float fltRightFrontOft = (PI/2) + m_fltRightFrontFemurTibiaPosTarget;
	float fltRightRearH = m_fltAvgCTGapLength*sin(m_fltDesiredBeta) + m_fltEffectiveCoxaLength*sin(-fltDelta);
	
	//Caluclate the height we will need to add to get the body to have the correct pitch value.
	float fltRightHDelta = m_fltDistComFront*tan(m_fltDesiredPitch);
	m_fltFrontRightLegHeight = fltRightRearH + m_fltFrontRearLegJointHeight+fltRightHDelta;

	float fltRightDiag = sqrt(m_fltLeftFrontFemurLength*m_fltLeftFrontFemurLength + m_fltLeftFrontTibiaLength*m_fltLeftFrontTibiaLength - 2*m_fltLeftFrontFemurLength*m_fltLeftFrontTibiaLength*cos(fltRightFrontOft)); 
	float fltRightO1 = asin( (m_fltLeftFrontTibiaLength * sin(fltRightFrontOft))/fltRightDiag );
	float fltRightO2 = acos(m_fltFrontRightLegHeight/fltRightDiag);
	m_fltRightFrontCoxaFemurPosTarget = fltRightO1 + fltRightO2 - (PI/2);

	//Put this back and we will move it slowly later.
	m_fltLeftFrontFemurTibiaPosTarget = m_fltLeftFrontFemurTibiaPos;
	m_fltRightFrontFemurTibiaPosTarget = m_fltRightFrontFemurTibiaPos;
}

void PostureControlStimulus::CalculateFeedbackAngles(Simulator *lpSim)
{}

//void PostureControlStimulus::CalculateFeedbackAngles(Simulator *lpSim)
//{
//	//m_fltLeftFrontFemurLength = sqrt( pow((double) (*m_vLeftFrontCoxaFemurPos[0]-*m_vLeftFrontFemurTibiaPos[0]), 2.0) +  pow((double) (*m_vLeftFrontCoxaFemurPos[1]-*m_vLeftFrontFemurTibiaPos[1]), 2.0) + pow((double) (*m_vLeftFrontCoxaFemurPos[2]-*m_vLeftFrontFemurTibiaPos[2]), 2.0) );
//	//m_fltLeftFrontTibiaLength = sqrt( pow((double) (*m_vLeftFrontFemurTibiaPos[0]-*m_vLeftFrontFootDownPos[0]), 2.0) +  pow((double) (*m_vLeftFrontFemurTibiaPos[1]-*m_vLeftFrontFootDownPos[1]), 2.0) + pow((double) (*m_vLeftFrontFemurTibiaPos[2]-*m_vLeftFrontFootDownPos[2]), 2.0) );
//
//	//Calculate the throacic-coxa, coxa-femur angles to have the rear legs
//	//at a desired beta angle given a specified pitch and throacic-coxa angle.
//	m_fltLeftRearThoracicCoxaPos = m_fltDesiredDelta + m_fltDesiredPitch; //m_fltPitchR;
//	m_fltRightRearThoracicCoxaPos = m_fltLeftRearThoracicCoxaPos;
//
//	m_fltAlphaR = PI - m_fltDesiredDelta - m_fltDesiredBeta;
//	m_fltAlphaD = (m_fltAlphaR*180)/PI;
//
//	m_fltLeftRearCoxaFemurPos = m_fltLeftGammaR + m_fltAlphaR - PI;
//	m_fltRightRearCoxaFemurPos = m_fltLeftRearCoxaFemurPos;
//
//	//Calcualte the thoracic-coxa, and femur-tibia angles of the front legs
//	//to have a specified body pitch, yaw, and roll.
//	//float fltRearH = *m_vLeftThoraxCoxaBetaPos[1] - *m_vLeftTibiaBetaPos[1];
//
//	//Find the height from the ground to the thoracic-coxa joint when the rear leg is at the specified
//	//delta-pitch-beta angles.
//	float fltRearH = m_fltCoxaTibiaGapLength*sin(m_fltLeftBetaR) + m_fltEffectiveCoxaLength*cos((PI/2)+m_fltDesiredDelta);
//	
//	//Caluclate the height we will need to add to get the body to have the correct pitch value.
//	float fltHDelta = m_fltDistComFront*tan(m_fltDesiredPitch);
//
//	m_fltFrontLeftLegHeight = fltRearH + m_fltFrontRearLegJointHeight+fltHDelta;
//	m_fltFrontRightLegHeight = m_fltFrontLeftLegHeight;
//
//	float fltFrontOft = (PI/2) - *m_lpfltLeftFrontFemurTibiaRotation;
//	float fltDiag = sqrt(m_fltLeftFrontFemurLength*m_fltLeftFrontFemurLength + m_fltLeftFrontTibiaLength*m_fltLeftFrontTibiaLength - 2*m_fltLeftFrontFemurLength*m_fltLeftFrontTibiaLength*cos(fltFrontOft)); 
//	float fltO1 = asin( (m_fltLeftFrontTibiaLength * sin(fltFrontOft))/fltDiag );
//
//	float fltO2 = acos(m_fltFrontLeftLegHeight/fltDiag);
//
//	m_fltLeftFrontCoxaFemurPos = fltO1 + fltO2 - (PI/2);
//	m_fltRightFrontCoxaFemurPos = m_fltLeftFrontCoxaFemurPos;
//
//
//
//	//float fltDelta=0;
//
//	////Since the middle legs are out of the way we should only be using the front and rear legs
//	////to support the locust. The angles of the rear thorax-coxa, and coxa-femur joints are going 
//	////to determine the beta angle, while the angles of the front coxa-femur will determine the 
//	////body elevation, or pitch.
//	//m_fltPitchChange = m_fltGain*(m_fltPitchR - m_fltDesiredPitch);
//
//	//if(m_fltPitchChange > MAX_CHANGE)
//	//	m_fltPitchChange = MAX_CHANGE;
//	//if(m_fltPitchChange < -MAX_CHANGE)
//	//	m_fltPitchChange = -MAX_CHANGE;
//
//	//m_fltLeftFrontCoxaFemurPos += m_fltPitchChange;
//	//m_fltRightFrontCoxaFemurPos += m_fltPitchChange;
//
//	//Lets ensure that the beta angles are the ones that the user specified.
//	//if(lpSim->Time() < 0.8f)
//	//{
//	//	m_fltLeftBetaChange = m_fltGain * (m_fltLeftBetaR - m_fltDesiredBeta);
//
//	//	if(m_fltLeftBetaChange > MAX_CHANGE)
//	//		m_fltLeftBetaChange = MAX_CHANGE;
//	//	if(m_fltLeftBetaChange < -MAX_CHANGE)
//	//		m_fltLeftBetaChange = -MAX_CHANGE;
//
//	//	m_fltLeftRearCoxaFemurPos+=m_fltLeftBetaChange;
//
//	//	m_fltRightBetaChange = m_fltGain * (m_fltRightBetaR - m_fltDesiredBeta);
//
//	//	if(m_fltRightBetaChange > MAX_CHANGE)
//	//		m_fltRightBetaChange = MAX_CHANGE;
//	//	if(m_fltRightBetaChange < -MAX_CHANGE)
//	//		m_fltRightBetaChange = -MAX_CHANGE;
//
//	//	m_fltRightRearCoxaFemurPos+=m_fltRightBetaChange;
//	//}
//
//}


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

	m_bMotorsDisabled = TRUE;
}

void PostureControlStimulus::ReactiveInFlightMotors(Simulator *lpSim)
{
	ActivateMotor(lpSim, m_lpLeftRearThoracicCoxa);
	ActivateMotor(lpSim, m_lpLeftRearCoxaFemur);
	ActivateMotor(lpSim, m_lpLeftRearFemurTibia);

	ActivateMotor(lpSim, m_lpRightRearThoracicCoxa);
	ActivateMotor(lpSim, m_lpRightRearCoxaFemur);
	ActivateMotor(lpSim, m_lpRightRearFemurTibia);

	m_fltLeftRearThoracicCoxaPos = 0;
	m_fltLeftRearCoxaFemurPos = 0;
	m_fltLeftRearFemurTibiaPos = -1.4f;

	m_fltRightRearThoracicCoxaPos = 0;
	m_fltRightRearCoxaFemurPos = 0;
	m_fltRightRearFemurTibiaPos = -1.4f;

	SetMotorPosition(lpSim, m_lpLeftRearThoracicCoxa, m_fltLeftRearThoracicCoxaPos);
	SetMotorPosition(lpSim, m_lpLeftRearCoxaFemur, m_fltLeftRearCoxaFemurPos);
	SetMotorPosition(lpSim, m_lpLeftRearFemurTibia, m_fltLeftRearFemurTibiaPos);

	SetMotorPosition(lpSim, m_lpRightRearThoracicCoxa, m_fltRightRearThoracicCoxaPos);
	SetMotorPosition(lpSim, m_lpRightRearCoxaFemur, m_fltRightRearCoxaFemurPos);
	SetMotorPosition(lpSim, m_lpRightRearFemurTibia, m_fltRightRearFemurTibiaPos);

	m_bMotorsDisabled = FALSE;
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
	else if(strType == "LEFTGAMMAD")
		lpData = &m_fltLeftGammaD;
	else if(strType == "LEFTGAMMAR")
		lpData = &m_fltLeftGammaR;
	else if(strType == "RIGHTGAMMAD")
		lpData = &m_fltRightGammaD;
	else if(strType == "RIGHTGAMMAR")
		lpData = &m_fltRightGammaR;
	else if(strType == "PITCHD")
		lpData = &m_fltPitchD;
	else if(strType == "PITCHR")
		lpData = &m_fltPitchR;
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
	else if(strType == "ALPHAR")
		lpData = &m_fltAlphaR;
	else if(strType == "ALPHAD")
		lpData = &m_fltAlphaD;
	else if(strType == "COXATIBIAGAP")
		lpData = &	m_fltCoxaTibiaGapLength;
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

	m_strThoraxID = oXml.GetChildString("Thorax");
	m_strLeftTendonLockID = oXml.GetChildString("LeftTendonLock");
	m_strRightTendonLockID = oXml.GetChildString("RightTendonLock");

	m_strHeadAxisRefID = oXml.GetChildString("HeadAxisRef");
	m_strTailAxisRefID = oXml.GetChildString("TailAxisRef");
	m_strCOMRefID = oXml.GetChildString("COMRef");
	m_strRollAxisRefID = oXml.GetChildString("RollAxisRef");

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
	m_strLeftRearCoxaID = oXml.GetChildString("LeftRearCoxa");
	m_strLeftRearFemurID = oXml.GetChildString("LeftRearFemur");
	m_strLeftRearTibiaID = oXml.GetChildString("LeftRearTibia");

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
 
	m_bThrustThruCOM = oXml.GetChildBool("ThrustThruCOM");

	m_fltFrontRearLegJointHeight = oXml.GetChildFloat("FrontRearLegJointHeight")*lpSim->InverseDistanceUnits()*0.0001;
	m_fltEffectiveCoxaLength = oXml.GetChildFloat("EffectiveCoxaLength")*lpSim->InverseDistanceUnits()*0.0001;
	m_fltAvgCTGapLength = oXml.GetChildFloat("CTGap");

	oXml.OutOfElem(); //OutOf Simulus Element

}

void PostureControlStimulus::Save(Simulator *lpSim, CStdXml &oXml)
{
}

	}			//ExternalStimuli
}				//VortexAnimatLibrary




