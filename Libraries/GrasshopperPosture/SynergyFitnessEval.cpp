#include "StdAfx.h"
#include "SynergyFitnessEval.h"

namespace GrasshopperPosture
{
	namespace ExternalStimuli
	{

SynergyFitnessEval::SynergyFitnessEval(void)
{
	m_fltTorsoHeightDiff = 0;
	m_fltTorsoOrientationRad = 0;
	m_fltTorsoOrientationDeg = 0;
	m_fltJointError = 0;
	m_fltTotalTension = 0;
	m_fltFitness = 0;

	m_lpStructure = NULL;
	m_lpTorso = NULL;
	m_lpTorsoAlign = NULL;
	
	m_lpLeftHip = NULL;
	m_lpLeftKnee = NULL;
	m_lpLeftAnkle = NULL;

	m_lpRightHip = NULL;
	m_lpRightKnee = NULL;
	m_lpRightAnkle = NULL;

	m_lpLeftBasm = NULL;
	m_lpLeftIP = NULL;
	m_lpLeftRF = NULL;
	m_lpLeftVast = NULL;
	m_lpLeftBpst = NULL;
	m_lpLeftGast = NULL;
	m_lpLeftPtf = NULL;
	m_lpLeftSol = NULL;

	m_lpRightBasm = NULL;
	m_lpRightIP = NULL;
	m_lpRightRF = NULL;
	m_lpRightVast = NULL;
	m_lpRightBpst = NULL;
	m_lpRightGast = NULL;
	m_lpRightPtf = NULL;
	m_lpRightSol = NULL;


	m_lpfltLeftBasmTension = NULL;
	m_lpfltLeftIPTension = NULL;
	m_lpfltLeftRFTension = NULL;
	m_lpfltLeftVastTension = NULL;
	m_lpfltLeftBpstTension = NULL;
	m_lpfltLeftGastTension = NULL;
	m_lpfltLeftPtfTension = NULL;
	m_lpfltLeftSolTension = NULL;

	m_lpfltRightBasmTension = NULL;
	m_lpfltRightIPTension = NULL;
	m_lpfltRightRFTension = NULL;
	m_lpfltRightVastTension = NULL;
	m_lpfltRightBpstTension = NULL;
	m_lpfltRightGastTension = NULL;
	m_lpfltRightPtfTension = NULL;
	m_lpfltRightSolTension = NULL;

	m_lpfltLeftHipRotation = 0;
	m_lpfltLeftKneeRotation = 0;
	m_lpfltLeftAnkleRotation = 0;

	m_lpfltRightHipRotation = 0;
	m_lpfltRightKneeRotation = 0;
	m_lpfltRightAnkleRotation = 0;

	m_fltDesiredLeftHipRotation = 0;
	m_fltDesiredLeftKneeRotation = 0;
	m_fltDesiredLeftAnkleRotation = 0;

	m_fltDesiredRightHipRotation = 0;
	m_fltDesiredRightKneeRotation = 0;
	m_fltDesiredRightAnkleRotation = 0;

	for(int i=0; i<3; i++)
	{
		m_vTorsoPos[i] = NULL;
		m_vTorsoAlignPos[i] = NULL;
		m_vDesiredTorsoPos[i] = 0;
	}
}

SynergyFitnessEval::~SynergyFitnessEval(void)
{

try
{
	m_lpStructure = NULL;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of SynergyFitnessEval\r\n", "", -1, FALSE, TRUE);}
}


void SynergyFitnessEval::Initialize()
{
	AnimatSim::ExternalStimuli::ExternalStimulus::Initialize();

	//Lets try and get the node we will dealing with.
	m_lpStructure = m_lpSim->FindStructureFromAll(m_strStructureID);
	m_lpTorso = m_lpStructure->FindRigidBody(m_strTorsoID);
	m_lpTorsoAlign = m_lpStructure->FindRigidBody(m_strTorsoAlignID);

	m_lpLeftHip = m_lpStructure->FindJoint(m_strLeftHipID);
	m_lpLeftKnee = m_lpStructure->FindJoint(m_strLeftKneeID);
	m_lpLeftAnkle = m_lpStructure->FindJoint(m_strLeftAnkleID);

	m_lpRightHip = m_lpStructure->FindJoint(m_strRightHipID);
	m_lpRightKnee = m_lpStructure->FindJoint(m_strRightKneeID);
	m_lpRightAnkle = m_lpStructure->FindJoint(m_strRightAnkleID);

	m_lpLeftBasm = m_lpStructure->FindRigidBody(m_strLeftBasmID);
	m_lpLeftIP = m_lpStructure->FindRigidBody(m_strLeftIPID);
	m_lpLeftRF = m_lpStructure->FindRigidBody(m_strLeftRFID);
	m_lpLeftVast = m_lpStructure->FindRigidBody(m_strLeftVastID);
	m_lpLeftBpst = m_lpStructure->FindRigidBody(m_strLeftBpstID);
	m_lpLeftGast = m_lpStructure->FindRigidBody(m_strLeftGastID);
	m_lpLeftPtf = m_lpStructure->FindRigidBody(m_strLeftPtfID);
	m_lpLeftSol = m_lpStructure->FindRigidBody(m_strLeftSolID);

	m_lpRightBasm = m_lpStructure->FindRigidBody(m_strRightBasmID);
	m_lpRightIP = m_lpStructure->FindRigidBody(m_strRightIPID);
	m_lpRightRF = m_lpStructure->FindRigidBody(m_strRightRFID);
	m_lpRightVast = m_lpStructure->FindRigidBody(m_strRightVastID);
	m_lpRightBpst = m_lpStructure->FindRigidBody(m_strRightBpstID);
	m_lpRightGast = m_lpStructure->FindRigidBody(m_strRightGastID);
	m_lpRightPtf = m_lpStructure->FindRigidBody(m_strRightPtfID);
	m_lpRightSol = m_lpStructure->FindRigidBody(m_strRightSolID);

	m_lpfltLeftHipRotation = m_lpLeftHip->GetDataPointer("JointRotation");
	m_lpfltLeftKneeRotation = m_lpLeftKnee->GetDataPointer("JointRotation");
	m_lpfltLeftAnkleRotation = m_lpLeftAnkle->GetDataPointer("JointRotation");

	m_lpfltRightHipRotation = m_lpRightHip->GetDataPointer("JointRotation");
	m_lpfltRightKneeRotation = m_lpRightKnee->GetDataPointer("JointRotation");
	m_lpfltRightAnkleRotation = m_lpRightAnkle->GetDataPointer("JointRotation");

	m_lpfltLeftBasmTension = m_lpLeftBasm->GetDataPointer("Tension");
	m_lpfltLeftIPTension = m_lpLeftIP->GetDataPointer("Tension");
	m_lpfltLeftRFTension = m_lpLeftRF->GetDataPointer("Tension");
	m_lpfltLeftVastTension = m_lpLeftVast->GetDataPointer("Tension");
	m_lpfltLeftBpstTension = m_lpLeftBpst->GetDataPointer("Tension");
	m_lpfltLeftGastTension = m_lpLeftGast->GetDataPointer("Tension");
	m_lpfltLeftPtfTension = m_lpLeftPtf->GetDataPointer("Tension");
	m_lpfltLeftSolTension = m_lpLeftSol->GetDataPointer("Tension");

	m_lpfltRightBasmTension = m_lpRightBasm->GetDataPointer("Tension");
	m_lpfltRightIPTension = m_lpRightIP->GetDataPointer("Tension");
	m_lpfltRightRFTension = m_lpRightRF->GetDataPointer("Tension");
	m_lpfltRightVastTension = m_lpRightVast->GetDataPointer("Tension");
	m_lpfltRightBpstTension = m_lpRightBpst->GetDataPointer("Tension");
	m_lpfltRightGastTension = m_lpRightGast->GetDataPointer("Tension");
	m_lpfltRightPtfTension = m_lpRightPtf->GetDataPointer("Tension");
	m_lpfltRightSolTension = m_lpRightSol->GetDataPointer("Tension");

	std::string strPosData, strLinVelData;
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

		m_vTorsoPos[i] = m_lpTorso->GetDataPointer(strPosData);
		m_vTorsoAlignPos[i] = m_lpTorsoAlign->GetDataPointer(strPosData);
	}
}

void SynergyFitnessEval::Activate()
{
	m_fltTorsoHeightDiff = 0;
	m_fltTorsoOrientationRad = 0;
	m_fltTorsoOrientationDeg = 0;
	m_fltJointError = 0;
	m_fltTotalTension = 0;
	m_fltFitness = 0;
}

void SynergyFitnessEval::StepSimulation()
{
	if(m_lpSim->Time() < 1)
	{
		//Before 1 lets get the current position of the torso in world coords.
		m_vDesiredTorsoPos[0] = *m_vTorsoPos[0];
		m_vDesiredTorsoPos[1] = *m_vTorsoPos[1];
		m_vDesiredTorsoPos[2] = *m_vTorsoPos[2];

		m_fltDesiredLeftHipRotation = *m_lpfltLeftHipRotation;
		m_fltDesiredLeftKneeRotation = *m_lpfltLeftKneeRotation;
		m_fltDesiredLeftAnkleRotation = *m_lpfltLeftAnkleRotation;

		m_fltDesiredRightHipRotation = *m_lpfltRightHipRotation;
		m_fltDesiredRightKneeRotation = *m_lpfltRightKneeRotation;
		m_fltDesiredRightAnkleRotation = *m_lpfltRightAnkleRotation;
	}
}

float SynergyFitnessEval::MapValue(float fltInput, float fltInMax, float fltInMin, float fltOutMax, float fltOutMin)
{
	if( (fltInput>=fltInMin) && (fltInput<=fltInMax) )
	{
		//Split it out this way because I was getting some weird rounding errors.
		float fltTest = fltInput-fltInMin;
		float fltSlope = (fltOutMax - fltOutMin) / (fltInMax - fltInMin);
		fltTest = fltSlope * fltTest;
		fltTest = fltTest + fltOutMin;
		return fltTest;
	}
	
	if(fltInput < fltInMin)
		return fltOutMin;
	
	if(fltInput > fltInMax)
		return fltOutMax;

	return fltOutMax;
}

void SynergyFitnessEval::EvaluatePostureFitness()
{
	//We will evaluate the posture fitness once it has reached steady-state
	//after the motors have been disabled. Fitness will be seperated into 
	//sections based on what is most important. These are the sections.
	//1. Torso height below the desired value. (Can it keep itself up? Height above is less important.)
	//2. Torso orientation. (Can it keep the torso oriented perpendicular to the ground?)
	//3. Torso height above the desired value. (this has a smaller error than heights below desired)
	//4. Joint angles. (Do the joint angles match the desired values?)
	//5. Tension levels. (We want to minimize the tension levels to produce a given posture.)
	//
	//Each level will add to the fitness value in the following way.
	//1. The torso is 46 cm above the ground to start. So the Y diff from desired will map (0 to -46) into (10000 to 0)
	//2. Torso orientation will be measured as the angle between the torso align attach and the center of the torso.
	//   A angle of 0 is fully vertical. An angle of 90 is when it has fallen onto its back.
	//   Angles will be mapped onto fitness like abs(0 to 90) into (1000 to 0)
	//3. Torso Height above desired. maps Y Diff from (0 to 15) into (500 to 0)
	//4. Joint Angles. The total error for the joint angles will be calculated. 6 joints * PI = 19 rads error maximum.
	//   so map error (0 to 20) into (100 to 0).
	//5. Muscle Tension. Max muscle tension = 16 muscles * 50 max newtons = 800 newtons.
	//   Map this from (0 to 800) into (50 to 0).

	//Calculate torso height difference.
	m_fltTorsoHeightDiff = *m_vTorsoPos[1] - m_vDesiredTorsoPos[1];

	//Calculate torso orientation angle.
	float fltX = sqrt( pow((double) (*m_vTorsoAlignPos[0]-*m_vTorsoPos[0]), 2.0) + pow((double) (*m_vTorsoAlignPos[2]-*m_vTorsoPos[2]), 2.0) );
	float fltY = *m_vTorsoPos[1]-*m_vTorsoAlignPos[1];
	
	if(fltY > 0)
		m_fltTorsoOrientationRad = atan(fltX/fltY);
	else
		m_fltTorsoOrientationRad = (MY_PI/2) - atan(fltX/fltY);

	m_fltTorsoOrientationDeg = fabs((m_fltTorsoOrientationRad*180)/MY_PI); 


	//Calculate total joint error.
	m_fltJointError = fabs(m_fltDesiredLeftHipRotation-*m_lpfltLeftHipRotation);
	m_fltJointError += fabs(m_fltDesiredLeftKneeRotation-*m_lpfltLeftKneeRotation);
	m_fltJointError += fabs(m_fltDesiredLeftAnkleRotation-*m_lpfltLeftAnkleRotation);
	
	m_fltJointError += fabs(m_fltDesiredRightHipRotation-*m_lpfltRightHipRotation);
	m_fltJointError += fabs(m_fltDesiredRightKneeRotation-*m_lpfltRightKneeRotation);
	m_fltJointError += fabs(m_fltDesiredRightAnkleRotation-*m_lpfltRightAnkleRotation);

	//Calculate Total muscle tension
	//We also want to minimize the amount of tension used to maintain a posture so lets
	//add the tensions of the muscles at the end of the run together as an error.
	m_fltTotalTension = *m_lpfltLeftBasmTension;
	m_fltTotalTension += *m_lpfltLeftIPTension;
	m_fltTotalTension += *m_lpfltLeftRFTension;
	m_fltTotalTension += *m_lpfltLeftVastTension;
	m_fltTotalTension += *m_lpfltLeftBpstTension;
	m_fltTotalTension += *m_lpfltLeftGastTension;
	m_fltTotalTension += *m_lpfltLeftPtfTension;
	m_fltTotalTension += *m_lpfltLeftSolTension;

	m_fltTotalTension += *m_lpfltRightBasmTension;
	m_fltTotalTension += *m_lpfltRightIPTension;
	m_fltTotalTension += *m_lpfltRightRFTension;
	m_fltTotalTension += *m_lpfltRightVastTension;
	m_fltTotalTension += *m_lpfltRightBpstTension;
	m_fltTotalTension += *m_lpfltRightGastTension;
	m_fltTotalTension += *m_lpfltRightPtfTension;
	m_fltTotalTension += *m_lpfltRightSolTension;

	//1. Torso height below desired value.
	if(m_fltTorsoHeightDiff < 0)
		m_fltFitness = MapValue(m_fltTorsoHeightDiff, 0, -0.35f, 10000, 0);
	else
		m_fltFitness = 10000;

	//2. Torso orientation
	m_fltFitness += MapValue(m_fltTorsoOrientationDeg, 90, 0, 0, 1000);

	//3. Torso height above desired value.
	if(m_fltTorsoHeightDiff > 0)
		m_fltFitness += MapValue(m_fltTorsoHeightDiff, 0.15f, 0, 0, 500);

	//4. Joint Angles.
	m_fltFitness += MapValue(m_fltJointError, 10, 0, 0, 100);

	//4. Muscle Tension.
	m_fltFitness += MapValue(m_fltTotalTension, 800, 0, 0, 50);
}

void SynergyFitnessEval::Deactivate(Simulator *lpSim)
{
	EvaluatePostureFitness();

	//And save the fitness value out for processing.
	std::ofstream oStream;
	std::string strFile = AnimatSim::GetFilePath(lpSim->ProjectPath(), "Fitness.txt");
	oStream.open(strFile.c_str());
	oStream << m_fltFitness << "\n";
	oStream.close();

}


float *SynergyFitnessEval::GetDataPointer(std::string strDataType)
{
	float *lpData=NULL;
	//std::string strType = Std_CheckString(strDataType);

	//if(strType == "LEFTBETAD")
	//	lpData = &m_fltLeftBetaD;
	//else if(strType == "LEFTBETAR")
	//	lpData = &m_fltLeftBetaR;
	//else if(strType == "RIGHTBETAD")
	//	lpData = &m_fltRightBetaD;
	//else if(strType == "RIGHTBETAR")
	//	lpData = &m_fltRightBetaR;
	//else if(strType == "LEFTOFTD")
	//	lpData = &m_fltLeftOftD;
	//else if(strType == "LEFTOFTR")
	//	lpData = &m_fltLeftOftR;
	//else if(strType == "RIGHTOFTD")
	//	lpData = &m_fltRightOftD;
	//else if(strType == "RIGHTOFTR")
	//	lpData = &m_fltRightOftR;
	//else if(strType == "LEFTGAMMAD")
	//	lpData = &m_fltLeftGammaD;
	//else if(strType == "LEFTGAMMAR")
	//	lpData = &m_fltLeftGammaR;
	//else if(strType == "RIGHTGAMMAD")
	//	lpData = &m_fltRightGammaD;
	//else if(strType == "RIGHTGAMMAR")
	//	lpData = &m_fltRightGammaR;
	//else if(strType == "PITCHD")
	//	lpData = &m_fltPitchD;
	//else if(strType == "PITCHR")
	//	lpData = &m_fltPitchR;
	//else if(strType == "YAWD")
	//	lpData = &m_fltYawD;
	//else if(strType == "YAWR")
	//	lpData = &m_fltYawR;
	//else if(strType == "ROLLD")
	//	lpData = &m_fltRollD;
	//else if(strType == "ROLLR")
	//	lpData = &m_fltRollR;
	//else if(strType == "LEFTBETACHANGE")
	//	lpData = &m_fltLeftBetaChange;
	//else if(strType == "RIGHTBETACHANGE")
	//	lpData = &m_fltRightBetaChange;
	//else if(strType == "PITCHCHANGE")
	//	lpData = &m_fltPitchChange;
	//else if(strType == "ALPHAR")
	//	lpData = &m_fltAlphaR;
	//else if(strType == "ALPHAD")
	//	lpData = &m_fltAlphaD;
	//else if(strType == "COXATIBIAGAP")
	//	lpData = &	m_fltCoxaTibiaGapLength;
	//else
	//	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "StimulusName: " + STR(m_strName) + "  DataType: " + strDataType);

	return lpData;
} 
void SynergyFitnessEval::Load(CStdXml &oXml)
{
	AnimatSim::ActivatedItem::Load(lpSim, oXml);

	oXml.IntoElem();  //Into Simulus Element

	m_strStructureID = oXml.GetChildString("StructureID");
	if(Std_IsBlank(m_strStructureID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_strTorsoID = oXml.GetChildString("TorsoID");
	m_strTorsoAlignID = oXml.GetChildString("TorsoAlignID");

	m_strLeftHipID = oXml.GetChildString("LeftHipID");
	m_strLeftKneeID = oXml.GetChildString("LeftKneeID");
	m_strLeftAnkleID = oXml.GetChildString("LeftAnkleID");

	m_strRightHipID = oXml.GetChildString("RightHipID");
	m_strRightKneeID = oXml.GetChildString("RightKneeID");
	m_strRightAnkleID = oXml.GetChildString("RightAnkleID");

	m_strLeftBasmID = oXml.GetChildString("LeftBasmID");
	m_strLeftIPID = oXml.GetChildString("LeftIPID");
	m_strLeftRFID = oXml.GetChildString("LeftRFID");
	m_strLeftVastID = oXml.GetChildString("LeftVastID");
	m_strLeftBpstID = oXml.GetChildString("LeftBpstID");
	m_strLeftGastID = oXml.GetChildString("LeftGastID");
	m_strLeftPtfID = oXml.GetChildString("LeftPtfID");
	m_strLeftSolID = oXml.GetChildString("LeftSolID");

	m_strRightBasmID = oXml.GetChildString("RightBasmID");
	m_strRightIPID = oXml.GetChildString("RightIPID");
	m_strRightRFID = oXml.GetChildString("RightRFID");
	m_strRightVastID = oXml.GetChildString("RightVastID");
	m_strRightBpstID = oXml.GetChildString("RightBpstID");
	m_strRightGastID = oXml.GetChildString("RightGastID");
	m_strRightPtfID = oXml.GetChildString("RightPtfID");
	m_strRightSolID = oXml.GetChildString("RightSolID");

	oXml.OutOfElem(); //OutOf Simulus Element

}

void SynergyFitnessEval::Save(Simulator *lpSim, CStdXml &oXml)
{
}

	}			//ExternalStimuli
}				//VortexAnimatSim

