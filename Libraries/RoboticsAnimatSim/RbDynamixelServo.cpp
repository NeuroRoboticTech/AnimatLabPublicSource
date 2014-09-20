// RbDynamixelServo.cpp: implementation of the RbDynamixelServo class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbHingeLimit.h"
#include "RbHinge.h"
#include "RbRigidBody.h"
#include "RbStructure.h"
#include "RbDynamixelServo.h"

#define DYN_ID					(2)
#define DYN_LENGTH				(3)
#define DYN_INSTRUCTION			(4)
#define DYN_ERRBIT				(4)
#define DYN_PARAMETER			(5)
#define DYN_DEFAULT_BAUDNUMBER	(1)

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbDynamixelServo::RbDynamixelServo() 
{
	m_iServoID = 0;
	m_iMinPosFP = 0;
	m_iMaxPosFP = 1023;

	m_iLastGoalPos = -1;

	m_iMinVelocityFP = 1;
	m_iMaxVelocityFP = 1023;
	m_fltMaxRotMin = 0.111;  //max rotations (rad) per fixed point unit.
	m_iLastGoalVelocity = -10000;

	m_fltMinAngle = -150;
	m_fltMaxAngle = 150;

	m_iMinLoadFP = 0;
	m_iMaxLoadFP = 1023;

	m_iNextGoalPos = 0;
	m_iNextGoalVelocity = 0;

	m_iPresentPos = 0;
	m_iPresentVelocity = 0;
	m_iLoad = 0;
	m_iVoltage = 0;
	m_iTemperature = 0;

	m_fltPresentPos = 0;
	m_fltPresentVelocity = 0;
	m_fltLoad = 0;
	m_fltVoltage = 0;
	m_fltTemperature = 0;

	m_fltReadParamTime = 0;

	m_bQueryMotorData = true;

	m_lpMotorJoint = NULL;

	m_bIsHinge = true;
	m_fltTranslationRange = 1;
	m_fltFPToFloatTranslation = 1;
	m_fltFloatToFPTranslation = 1;

	m_fltHiLimit = RB_PI/2;
	m_fltLowLimit = -RB_PI/2;

	m_fltIOPos = 0;
	m_fltIOVelocity = 0;

	m_bNeedSetVelStopPos = false;
	m_bVelStopPosSet = false;
	m_bResetToStartPos = false;

	RecalculateParams();
}

RbDynamixelServo::~RbDynamixelServo()
{
        //Do not delete because we do not own it.
        m_lpMotorJoint = NULL;
}

void RbDynamixelServo::QueryMotorData(bool bVal) {m_bQueryMotorData = bVal;}

bool RbDynamixelServo::QueryMotorData() {return m_bQueryMotorData;}

void RbDynamixelServo::RecalculateParams()
{
	m_iTotalAngle = (m_iMaxPosFP - m_iMinPosFP);
	m_iCenterPosFP = round(m_iTotalAngle/2.0);

	m_fltTotalAngle = (float) (((m_fltMaxAngle-m_fltMinAngle)/180.0)*RB_PI);

	m_iMinSimPos = m_iMinPosFP;
	m_iMaxSimPos = m_iMaxPosFP;
	m_fltMinSimPos = m_fltMinAngle;
	m_fltMaxSimPos = m_fltMaxAngle;

	if(m_iTotalAngle > 0)
		m_fltPosFPToFloatSlope = (m_fltTotalAngle/m_iTotalAngle);

	m_fltPosFPToFloatIntercept = -(m_fltPosFPToFloatSlope*m_iCenterPosFP);

	if(m_fltTotalAngle > 0)
		m_fltPosFloatToFPSlope = (m_iTotalAngle/m_fltTotalAngle);

	m_fltPosFloatToFPIntercept = m_iCenterPosFP;

	m_fltMaxPosSec = (m_fltMaxRotMin*2*RB_PI)/60.0f; 
	m_fltConvertFPToPosS = m_fltMaxPosSec; //0.01162389281828223498231178051813f;  //0.111 rot/min = 0.0116 rad/s

	if(m_fltConvertFPToPosS > 0)
		m_fltConvertPosSToFP = 1/m_fltConvertFPToPosS;

	int iTotalLoad = (m_iMaxLoadFP - m_iMinLoadFP);
	if(iTotalLoad > 0)
		m_fltConvertFPToLoad = 100.0/iTotalLoad;

	if(!m_bIsHinge)
	{
		if(m_fltTotalAngle >= 0)
			m_fltFPToFloatTranslation = (m_fltTranslationRange / m_fltTotalAngle);

		if(m_fltFPToFloatTranslation >= 0)
			m_fltFloatToFPTranslation = 1 / m_fltFPToFloatTranslation;
	}
	else
	{
		m_fltFPToFloatTranslation = 1;
		m_fltFloatToFPTranslation = 1;
	}

}

void RbDynamixelServo::MinPosFP(int iVal)
{
	Std_IsBelowMax((int) m_iMaxPosFP, iVal, true, "MinPosFP");
	m_iMinPosFP = iVal;
	RecalculateParams();
}

int RbDynamixelServo::MinPosFP() {return m_iMinPosFP;}

void RbDynamixelServo::MaxPosFP(int iVal)
{
	Std_IsAboveMin((int) m_iMinPosFP, iVal, true, "MinPosFP");
	m_iMaxPosFP = iVal;
	RecalculateParams();
}

int RbDynamixelServo::MaxPosFP() {return m_iMaxPosFP;}

void RbDynamixelServo::MinAngle(float fltVal)
{
	Std_IsBelowMax((float) m_fltMaxAngle, fltVal, true, "MinAngle");
	m_fltMinAngle = fltVal;
	RecalculateParams();
}

float RbDynamixelServo::MinAngle() {return m_fltMinAngle;}

void RbDynamixelServo::MaxAngle(float fltVal)
{
	Std_IsAboveMin((float) m_fltMinAngle, fltVal, true, "MaxAngle");
	m_fltMaxAngle = fltVal;
	RecalculateParams();
}

float RbDynamixelServo::MaxAngle() {return m_fltMaxAngle;}

void RbDynamixelServo::MinVelocityFP(int iVal)
{
	Std_IsBelowMax((int) m_iMaxVelocityFP, iVal, true, "MinVelocityFP");
	m_iMinVelocityFP = iVal;
	RecalculateParams();
}

int RbDynamixelServo::MinVelocityFP() {return m_iMinVelocityFP;}

void RbDynamixelServo::MaxVelocityFP(int iVal)
{
	Std_IsAboveMin((int) m_iMinVelocityFP, iVal, true, "MaxVelocityFP");
	m_iMaxVelocityFP = iVal;
	RecalculateParams();
}

int RbDynamixelServo::MaxVelocityFP() {return m_iMaxVelocityFP;}

void RbDynamixelServo::MaxRotMin(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "MaxRotMin");
	m_fltMaxRotMin = fltVal;
	RecalculateParams();
}

float RbDynamixelServo::MaxRotMin() {return m_fltMaxRotMin;}

void RbDynamixelServo::MinLoadFP(int iVal)
{
	Std_IsBelowMax((int) m_iMaxLoadFP, iVal, true, "MinLoadFP");
	m_iMinLoadFP = iVal;
	RecalculateParams();
}

int RbDynamixelServo::MinLoadFP() {return m_iMinLoadFP;}

void RbDynamixelServo::MaxLoadFP(int iVal)
{
	Std_IsAboveMin((int) m_iMinLoadFP, iVal, true, "MaxLoadFP");
	m_iMaxLoadFP = iVal;
	RecalculateParams();
}

int RbDynamixelServo::MaxLoadFP() {return m_iMaxLoadFP;}

float RbDynamixelServo::ConvertPosFPToFloat(int iPos)
{
	float fltAngle = (m_fltPosFPToFloatSlope*iPos) + m_fltPosFPToFloatIntercept;
	float fltPos = m_fltFPToFloatTranslation * fltAngle;
	return fltPos;
}

int RbDynamixelServo::ConvertPosFloatToFP(float fltPos)
{
	float fltAngle = m_fltFloatToFPTranslation * fltPos;
	int iPos = (m_fltPosFloatToFPSlope*fltAngle) + m_fltPosFloatToFPIntercept;
	return iPos;
}

/**
\brief	Sets the ID that will be used for communications with this servo. This is NOT writing an ID to the servo. It is just 
specifying which one to use for com access.

\author	dcofer
\date	4/25/2014

\param	iID	The ID value to use for communications.
**/void RbDynamixelServo::ServoID(int iID)
{
	if(iID >= 0)
		m_iServoID = iID;
}

/**
\brief	Gets the servo ID being used for communications. 

\author	dcofer
\date	4/25/2014

\return	ID Value used for communications. 
**/
int RbDynamixelServo::ServoID() {return m_iServoID;}

/**
\brief	Sets the fixed point value of the servo velocity. This is the exact value that will be sent to the servo. 
This method excludes the velocity of 0 which defaults it to move at its fastest speed.  If you want to do that
then use the SetMaximumVelocity method.

\author	dcofer
\date	4/25/2014

\param	iVelocity	The new value fixed point velocity.
**/
void RbDynamixelServo::SetGoalVelocity_FP(int iVelocity)
{
	//Verify we are not putting invalid values in.
	if(iVelocity < m_iMinVelocityFP) iVelocity = m_iMinVelocityFP;
	if(iVelocity > m_iMaxVelocityFP) iVelocity = m_iMaxVelocityFP;

	//If the velocity we are setting is the same as we just set then no point sending it.
	if(m_iLastGoalVelocity == iVelocity)
		return;

	m_iLastGoalVelocity = iVelocity;

	WriteMovingSpeed(m_iServoID, iVelocity);
}

/**
\brief	Sets the fixed point value of the servo velocity for the next time the IO for this servo is processed. 

\author	dcofer
\date	4/25/2014

\param	iVelocity	The new value fixed point velocity.
**/
void RbDynamixelServo::SetNextGoalVelocity_FP(int iVelocity)
{
	//Verify we are not putting invalid values in.
	if(iVelocity != -1 && iVelocity < m_iMinVelocityFP) iVelocity = m_iMinVelocityFP;
	if(iVelocity != -1 && iVelocity > m_iMaxVelocityFP) iVelocity = m_iMaxVelocityFP;

	m_iNextGoalVelocity = iVelocity;
}


/**
\brief	Sets the servo goal speed to 0 for maximum velocity. 

\author	dcofer
\date	4/25/2014
**/
void RbDynamixelServo::SetMaximumVelocity()
{
	//If the velocity we are setting is the same as we just set then no point sending it.
	if(m_iLastGoalVelocity == 0)
		return;

	m_iLastGoalVelocity = 0;

	WriteMovingSpeed(m_iServoID, 0);
}

/**
\brief	Sets the servo goal speed to 0 for maximum velocity the next time the IO for this servo is processed. 

\author	dcofer
\date	4/25/2014
**/
void RbDynamixelServo::SetNextMaximumVelocity()
{
	//If the velocity we are setting is the same as we just set then no point sending it.
	if(m_iLastGoalVelocity == 0)
		return;

	m_iNextGoalVelocity = 0;
}

/**
\brief	Gets the fixed point value of the servo velocity. This is the exact value returned from the servo. 

\author	dcofer
\date	4/25/2014

\return	Fixed point value of velocity from the servo. 
**/
int RbDynamixelServo::GetGoalVelocity_FP()
{
	int iGoalVel = ReadMovingSpeed(m_iServoID);

	return iGoalVel;
}


/**
\brief	Sets the floating point value of the servo velocity. This is the velocity in rad/s. It will be converted to a fixed
point value based on the motor configuration and then used to set the velocity.

\author	dcofer
\date	4/25/2014

\param	fltVelocity	The new value velocity in rad/s.
**/
void RbDynamixelServo::SetGoalVelocity(float fltVelocity)
{
	int iVel = ConvertFloatVelocity(fltVelocity);

	SetGoalVelocity_FP(iVel);
}

/**
\brief	Sets the floating point value of the servo velocity the next time the IO for this servo is processed. This is the velocity in rad/s. It will be converted to a fixed
point value based on the motor configuration and then used to set the velocity.

\author	dcofer
\date	4/25/2014

\param	fltVelocity	The new value velocity in rad/s.
**/
void RbDynamixelServo::SetNextGoalVelocity(float fltVelocity)
{
	int iVel = ConvertFloatVelocity(fltVelocity);

	//If the velocity is being set to 0 then code this special.
	if(fltVelocity == 0)
		iVel = -1;

	SetNextGoalVelocity_FP(iVel);
}

/**
\brief	Gets the floating point value of the servo velocity. The fixed point value is retrieved from the servo and then converted to rad/s. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of velocity from the servo in rad/s. 
**/
float RbDynamixelServo::GetGoalVelocity()
{
	int iGoal = GetGoalVelocity_FP();

	float fltPos = iGoal*m_fltConvertFPToPosS;

	return fltPos;
}

/**
\brief	Gets the last goal velocity that was sent to the servo. This is used so we can see the last value we set without having to requery it from the servo. 

\author	dcofer
\date	4/25/2014

\return	Last goal velocity set. 
**/
int RbDynamixelServo::LastGoalVelocity_FP() {return m_iLastGoalVelocity;}

/**
\brief	Sets the fixed point value of the servo goal position. This is the exact value that will be sent to the servo. 

\author	dcofer
\date	4/25/2014

\param	iPos	The new value fixed point position.
**/
void RbDynamixelServo::SetGoalPosition_FP(int iPos)
{
	//Verify we are not putting invalid values in.
	if(iPos < m_iMinSimPos) iPos = m_iMinSimPos;
	if(iPos > m_iMaxSimPos) iPos = m_iMaxSimPos;

	//If the position we are setting is the same as we just set then no point sending it.
	if(m_iLastGoalPos == iPos)
		return;

	m_iLastGoalPos = iPos;

	WriteGoalPosition(m_iServoID, iPos);
}

/**
\brief	Sets the fixed point value of the servo goal position that will be used the next time the IO for this servo is processed. This is the exact value that will be sent to the servo. 

\author	dcofer
\date	4/25/2014

\param	iPos	The new value fixed point position.
**/
void RbDynamixelServo::SetNextGoalPosition_FP(int iPos)
{
	//Verify we are not putting invalid values in.
	if(iPos < m_iMinSimPos) iPos = m_iMinSimPos;
	if(iPos > m_iMaxSimPos) iPos = m_iMaxSimPos;

	m_iNextGoalPos = iPos;
}

/**
\brief	Gets the fixed point value of the servo goal position. This is the exact value returned from the servo. 

\author	dcofer
\date	4/25/2014

\return	Fixed point value of goal position from the servo. 
**/
int RbDynamixelServo::GetGoalPosition_FP()
{
	int iGoalPos = ReadGoalPosition(m_iServoID);
	return iGoalPos;
}

/**
\brief	Sets the floating point value of the servo goal position. This is the position in radians. It will be converted to a fixed
point value based on the motor configuration and then used to set the position. This method assumes that center value is 0 and that
it uses +/- values on either side.

\author	dcofer
\date	4/25/2014

\param	fltPos	The new value position in radians.
**/
void RbDynamixelServo::SetGoalPosition(float fltPos)
{
	int iPos = ConvertPosFloatToFP(fltPos);

	SetGoalPosition_FP(iPos);
}

/**
\brief	Sets the floating point value of the servo goal position that will be used the next time the iO for this servo is processed. 
This is the position in radians. It will be converted to a fixed
point value based on the motor configuration and then used to set the position. This method assumes that center value is 0 and that
it uses +/- values on either side.

\author	dcofer
\date	4/25/2014

\param	fltPos	The new value position in radians.
**/
void RbDynamixelServo::SetNextGoalPosition(float fltPos)
{
	int iPos = ConvertPosFloatToFP(fltPos);

	SetNextGoalPosition_FP(iPos);
}

/**
\brief	Gets the floating point value of the servo goal position. The fixed point value is retrieved from the servo and then converted to radians. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of goal position from the servo in radians. 
**/
float RbDynamixelServo::GetGoalPosition()
{
	int iGoal = GetGoalPosition_FP();

	float fltPos = ConvertPosFPToFloat(iGoal);

	return fltPos;
}

/**
\brief	Gets the last goal position that was sent to the servo. This is used so we can see the last value we set without having to requery it from the servo. 

\author	dcofer
\date	4/25/2014

\return	Last goal position set. 
**/
int RbDynamixelServo::LastGoalPosition_FP() {return m_iLastGoalPos;}

/**
\brief	Gets the fixed point value of the servo actual position. This is the exact value returned from the servo. 

\author	dcofer
\date	4/25/2014

\return	Fixed point value of actual position from the servo. 
**/
int RbDynamixelServo::GetActualPosition_FP()
{
	int iPos = ReadPresentPosition(m_iServoID);
	return iPos;
}

/**
\brief	Gets the floating point value of the servo actual position. The fixed point value is retrieved from the servo and then converted to radians. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of goal position from the servo in radians. 
**/
float RbDynamixelServo::GetActualPosition()
{
	int iPos = GetActualPosition_FP();

	float fltPos = ConvertPosFPToFloat(iPos);

	return fltPos;
}

/**
\brief	Gets the fixed point value of the servo actual velocity. This is the exact value returned from the servo. 

\author	dcofer
\date	4/25/2014

\return	Fixed point value of actual velocity from the servo. 
**/
int RbDynamixelServo::GetActualVelocity_FP()
{
	int iVel = ReadPresentSpeed(m_iServoID);
	return iVel;
}

/**
\brief	Gets the floating point value of the servo actual velocity. The fixed point value is retrieved from the servo and then converted to rad/s. 

\author	dcofer
\date	4/25/2014

\param	iVel	The fixed point velocity.

\return	Floating point value of velocity from the servo in rad/s. 
**/
float RbDynamixelServo::ConvertFPVelocity(int iVel)
{
	int iDir = 1;
	if(iVel > m_iMaxVelocityFP)
	{
		iVel -= m_iMaxVelocityFP;
		iDir = -1;
	}

	float fltVel = iDir*iVel*(m_fltFPToFloatTranslation*m_fltConvertFPToPosS);
	return fltVel;
}

/**
\brief	Gets the fixed point velocity form the floating point 

\author	dcofer
\date	4/25/2014

\param	fltVelocity	Floating point value of velocity from the servo in rad/s.

\return	The fixed point velocity. 
**/
int RbDynamixelServo::ConvertFloatVelocity(float fltVelocity)
{
	if(fltVelocity < -0.4)
		fltVelocity = fltVelocity;

	int iVel = (int) (fabs(fltVelocity)*(m_fltFloatToFPTranslation*m_fltConvertPosSToFP));

	if(iVel < m_iMinVelocityFP)
		iVel = m_iMinVelocityFP;
	if(iVel > m_iMaxVelocityFP)
		iVel = m_iMaxVelocityFP;

	return iVel;
}


/**
\brief	Gets the floating point value of the servo actual velocity. The fixed point value is retrieved from the servo and then converted to rad/s. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of actual velocity from the servo in rad/s. 
**/
float RbDynamixelServo::GetActualVelocity()
{
	int iVel = GetActualVelocity_FP();
	return ConvertFPVelocity(iVel);
}

/**
\brief	Gets the fixed point value of the servo actual load. This is the exact value returned from the servo. 

\author	dcofer
\date	4/25/2014

\return	Fixed point value of actual load from the servo. 
**/
int RbDynamixelServo::GetActualLoad_FP()
{
	int iLoad = ReadPresentLoad(m_iServoID);
	return iLoad;
}

/**
\brief	Gets the floating point value of the servo load. The fixed point value is retrieved from the servo and then converted. 

\author	dcofer
\date	4/25/2014

\param	iVel	The fixed point load.

\return	Floating point value of velocity from the servo in rad/s. 
**/
float RbDynamixelServo::ConvertFPLoad(int iLoad)
{
	int iDir = -1;
	if(iLoad > m_iMaxLoadFP)
	{
		iLoad -= m_iMaxLoadFP;
		iDir = 1;
	}

	float fltLoad = iDir*iLoad*m_fltConvertFPToLoad;

	return fltLoad;
}

/**
\brief	Gets the floating point value of the servo actual torque. The fixed point value is retrieved from the servo and then converted to Nm. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of actual torque from the servo in Nm. 
**/
float RbDynamixelServo::GetActualLoad()
{
	int iLoad = GetActualLoad_FP();
	return ConvertFPLoad(iLoad);
}

/**
\brief	Gets the fixed point value of the servo actual voltage. This is the exact value returned from the servo. 

\author	dcofer
\date	4/25/2014

\return	Fixed point value of actual voltage from the servo. 
**/
int RbDynamixelServo::GetActualVoltage_FP()
{
	int iVoltage = ReadPresentVoltage(m_iServoID);
	return iVoltage;
}

/**
\brief	Gets the floating point value of the servo actual voltage. The fixed point value is retrieved from the servo and then converted to V. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of actual voltage from the servo in V. 
**/
float RbDynamixelServo::GetActualVoltage()
{
	int iVoltage = GetActualVoltage_FP();
	float fltVoltage = iVoltage/100.0;
	return fltVoltage;
}

/**
\brief	Gets the floating point value of the servo actual temperature. The fixed point value is retrieved from the servo and then converted to fahrenheit. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of actual temperature from the servo in F. 
**/
float RbDynamixelServo::GetActualTemperatureFahrenheit()
{
	float fltTemp = ((GetActualTemperatureCelcius()*(9.0/5.0)) + 32.0);
	return fltTemp;
}

/**
\brief	Gets the floating point value of the servo actual temperature. The fixed point value is retrieved from the servo and then converted to celcius. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of actual temperature from the servo in C. 
**/
float RbDynamixelServo::GetActualTemperatureCelcius()
{
	float fltTemp = (float) ReadPresentTemperature(m_iServoID);
	return fltTemp;
}

/**
\brief	Returns whether the servo is currently moving or has already reached its goal state. 

\author	dcofer
\date	4/25/2014

\return	True for moving. 
**/
bool RbDynamixelServo::GetIsMoving()
{
	int iMoving = ReadIsMoving(m_iServoID);

	if(iMoving == 0)
		return false;
	else
		return true;
}

/**
\brief	Returns whether the servo blue led is currently on. 

\author	dcofer
\date	4/25/2014

\param	bIsBlueOn	True if blue LED is on.
\param	bIsGreenOn	True if green LED is on.
\param	bIsRedOn	True if red LED is on.
**/
void RbDynamixelServo::GetIsLEDOn(bool &bIsBlueOn, bool &bIsGreenOn, bool &bIsRedOn)
{
	int iLED = ReadLED(m_iServoID);

	bIsBlueOn = iLED & 0x04;
	bIsGreenOn =  iLED & 0x02;
	bIsRedOn =  iLED & 0x01;
}

/**
\brief	Returns whether the alarm shutdown state is active. 

\author	dcofer
\date	4/25/2014

\return	True if shutdown. 
**/
bool RbDynamixelServo::GetIsAlarmShutdown()
{
	int iAlarm = ReadAlarmShutdown(m_iServoID);

	if(iAlarm)
		return true;
	else
		return false;
}

/**
\brief	Returns whether the model number of the servo. 

\author	dcofer
\date	4/25/2014

\return	Model number. 
**/
int RbDynamixelServo::GetModelNumber()
{
	int iModel = ReadModelNumber(m_iServoID);
	return iModel;
}

/**
\brief	Returns whether the servo ID number. 

\author	dcofer
\date	4/25/2014

\return	ID Number. 
**/
int RbDynamixelServo::GetIDNumber()
{
	int iID = ReadID(m_iServoID);
	return iID;
}

/**
\brief	Returns whether the servo firmware version number. 

\author	dcofer
\date	4/25/2014

\return	Firmware number. 
**/
int RbDynamixelServo::GetFirmwareVersion()
{
	int iID = ReadFirmwareVersion(m_iServoID);
	return iID;
}

void RbDynamixelServo::WaitForMoveToFinish()
{
	do
	{
#ifdef Win32
		//Do not attempt to sleep in linux while in a spinlock. Windows is fine with it.
		MicroSleep(5000);
#endif
	} while(GetIsMoving());
}

/**
\brief	Attempts to stop the motor at its current position. 

\author	dcofer
\date	8/28/2014
**/
void RbDynamixelServo::Stop()
{
	SetNextGoalPosition_FP(m_iPresentPos);
}

/**
\brief	Moves the motor to the specified position at the given speed. 

\author	dcofer
\date	9/11/2014
**/
void RbDynamixelServo::Move(float fltPos, float fltVel)
{
	if(fltVel < 0)
		SetMaximumVelocity();
	else
		SetGoalVelocity(fltVel);
	boost::this_thread::sleep(boost::posix_time::microseconds(100));
		  
	SetGoalPosition(fltPos);
	boost::this_thread::sleep(boost::posix_time::microseconds(100));
}

/**
\brief	Checks the current limits on the motor and sets them according to the simulation params. 

\author	dcofer
\date	9/11/2014
**/
void RbDynamixelServo::ConfigureServo()
{
	int iMinPos = GetCWAngleLimit_FP();
	int iMaxPos = GetCCWAngleLimit_FP();
	int iRetDelay = GetReturnDelayTime_FP();
	int iRetTorqueLimit = GetTorqueLimit_FP();

	if(iMinPos != m_iMinSimPos)
		SetCWAngleLimit_FP(m_iMinSimPos);

	if(iMaxPos != m_iMaxSimPos)
		SetCCWAngleLimit_FP(m_iMaxSimPos);

	if(iRetDelay > 1)
		SetReturnDelayTime_FP(1);

	if(iRetTorqueLimit != 1023)
		SetTorqueLimit_FP(1023);
}

/**
\brief	Initializes the internal data on position and velocity from the actual motor. 

\author	dcofer
\date	4/25/2014
**/
void RbDynamixelServo::InitMotorData()
{
	ConfigureServo();

	if(m_bResetToStartPos)
	{
		//Move to the reset pos at max speed.
		Move(0, -1);
		WaitForMoveToFinish();
	}

	m_iLastGoalPos = GetActualPosition_FP();

	//Reset the goal velocity to the minimum value.
	SetGoalVelocity_FP(1);

	std::cout << "Reset Servo " << m_iServoID << " Position: " << m_iLastGoalPos << "\r\n";
}

/**
\brief	Shuts the motor down cleanly and ensures that it is not continuing to move after processing has stopped. 

\author	dcofer
\date	4/25/2014
**/
void RbDynamixelServo::ShutdownMotor()
{
	int iCurPos = GetActualPosition_FP();

	//Reset the goal velocity to the minimum value.
	SetGoalVelocity_FP(1);
	SetGoalPosition_FP(iCurPos);

	std::cout << "Shutting down servo: " << m_iServoID << ", Pos: " << iCurPos << "\r\n";
}

/**
\brief	Sets the return delay time of the servo.

\author	dcofer
\date	5/7/2014

\param	iVal	delay time
**/
void RbDynamixelServo::SetReturnDelayTime_FP(int iVal)
{
	if(iVal >= 0 && iVal < 256)
		WriteReturnDelayTime(m_iServoID, iVal);
}

/**
\brief	Gets the return delay time of the servo.

\author	dcofer
\date	5/7/2014

\return	delay time 
**/
int RbDynamixelServo::GetReturnDelayTime_FP()
{
	return ReadReturnDelayTime(m_iServoID);
}


/**
\brief	Sets the limit for the CCW limit of the servo using fixed point value.

\author	dcofer
\date	5/9/2014

\param	iVal	limit
**/
void RbDynamixelServo::SetCCWAngleLimit_FP(int iVal)
{
	if(iVal >= m_iMinPosFP && iVal <= m_iMaxPosFP)
		WriteCCWAngleLimit(m_iServoID, iVal);
}

/**
\brief	Sets the limit for the CCW limit of the servo using radians.

\author	dcofer
\date	5/9/2014

\param	fltVal	limit
**/
void RbDynamixelServo::SetCCWAngleLimit(float fltVal)
{
	int iPos = ConvertPosFloatToFP(fltVal);
	SetCCWAngleLimit_FP(iPos);
}

/**
\brief	Gets the the total range of translation for a prismatic joint (meters).

\author	dcofer
\date	8/8/2014

\return	range 
**/
float RbDynamixelServo::TranslationRange() {return m_fltTranslationRange;}

/**
\brief	Sets the total range for translation of a prismatic joint.

\author	dcofer
\date	8/8/2014

\param	fltVal	range
**/
void RbDynamixelServo::TranslationRange(float fltVal)
{
	if(!m_bIsHinge)
	{
		Std_IsAboveMin((float) 0, fltVal, true, "TranslationRange", true);

		m_fltTranslationRange = fltVal;
		RecalculateParams();
	}
}

/**
\brief	Gets the limit for the CCW limit of the servo in fixed point value.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
int RbDynamixelServo::GetCCWAngleLimit_FP()
{
	return ReadCCWAngleLimit(m_iServoID);
}

/**
\brief	Gets the limit for the CCW limit of the servo in radian values.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
float RbDynamixelServo::GetCCWAngleLimit()
{
	int iPos = GetCCWAngleLimit_FP();
	return ConvertPosFPToFloat(iPos);
}

/**
\brief	Sets the limit for the CW limit of the servo using fixed point value.

\author	dcofer
\date	5/9/2014

\param	iVal	limit
**/
void RbDynamixelServo::SetCWAngleLimit_FP(int iVal)
{
	if(iVal >= m_iMinPosFP && iVal <= m_iMaxPosFP)
		WriteCWAngleLimit(m_iServoID, iVal);
}

/**
\brief	Sets the limit for the CW limit of the servo using radians.

\author	dcofer
\date	5/9/2014

\param	fltVal	limit
**/
void RbDynamixelServo::SetCWAngleLimit(float fltVal)
{
	int iPos = ConvertPosFloatToFP(fltVal);
	SetCWAngleLimit_FP(iPos);
}

/**
\brief	Gets the limit for the CW limit of the servo in fixed point value.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
int RbDynamixelServo::GetCWAngleLimit_FP()
{
	return ReadCWAngleLimit(m_iServoID);
}

/**
\brief	Gets the limit for the CW limit of the servo in radian values.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
float RbDynamixelServo::GetCWAngleLimit()
{
	int iPos = GetCWAngleLimit_FP();
	return ConvertPosFPToFloat(iPos);
}

/**
\brief	Gets the minimum position in fixed point that the simulation will allow for the joint.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
int RbDynamixelServo::GetMinSimPos_FP() {return m_iMinSimPos;}

/**
\brief	Gets the minimum position in radians that the simulation will allow for the joint.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
float RbDynamixelServo::GetMinSimPos()  {return m_fltMinSimPos;}

/**
\brief	Sets the minimum position that the simulation will allow for this joint in radians.

\author	dcofer
\date	5/9/2014

\param	fltVal	limit
**/
void RbDynamixelServo::SetMinSimPos(float fltVal)
{
	m_iMinSimPos = ConvertPosFloatToFP(fltVal);

	if(m_iMinSimPos < m_iMinPosFP) m_iMinSimPos = m_iMinPosFP;
	if(m_iMinSimPos > m_iMaxPosFP) m_iMinSimPos = m_iMaxPosFP;

	m_fltMinSimPos = ConvertPosFPToFloat(m_iMinSimPos);
}

/**
\brief	Gets the maximum position in fixed point that the simulation will allow for the joint.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
int RbDynamixelServo::GetMaxSimPos_FP() {return m_iMaxSimPos;}

/**
\brief	Gets the maximum position in radians that the simulation will allow for the joint.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
float RbDynamixelServo::GetMaxSimPos()  {return m_fltMaxSimPos;}

/**
\brief	Sets the maximum position that the simulation will allow for this joint in radians.

\author	dcofer
\date	5/9/2014

\param	fltVal	limit
**/
void RbDynamixelServo::SetMaxSimPos(float fltVal)
{
	m_iMaxSimPos = ConvertPosFloatToFP(fltVal);

	if(m_iMaxSimPos < m_iMinPosFP) m_iMaxSimPos = m_iMinPosFP;
	if(m_iMaxSimPos > m_iMaxPosFP) m_iMaxSimPos = m_iMaxPosFP;

	m_fltMaxSimPos = ConvertPosFPToFloat(m_iMaxSimPos);
}

/**
\brief	Sets the torque limit of the servo using fixed point value.

\author	dcofer
\date	6/12/2014

\param	iVal	limit
**/
void RbDynamixelServo::SetTorqueLimit_FP(int iVal)
{
	if(iVal >= m_iMinLoadFP && iVal <= m_iMaxLoadFP)
		WriteTorqueLimit(m_iServoID, iVal);
}

/**
\brief	Gets the torque limit of the servo in fixed point value.

\author	dcofer
\date	6/12/2014

\return	limit 
**/
int RbDynamixelServo::GetTorqueLimit_FP()
{
	return ReadTorqueLimit(m_iServoID);
}

bool RbDynamixelServo::ResetToStartPos() {return m_bResetToStartPos;}

void RbDynamixelServo::ResetToStartPos(bool bVal) {m_bResetToStartPos = bVal;}

void RbDynamixelServo::SetMotorPosVel()
{
	//std::cout << m_lpSim->Time() << ", servo: " << m_iServoID <<  ", Pos: " << m_iNextGoalPos << ", LasPos: " << m_iLastGoalPos << ", Vel: " << m_iNextGoalVelocity << ", LastVel: " << m_iLastGoalVelocity << "\r\n";

	if(m_bNeedSetVelStopPos && !m_bVelStopPosSet)
	{
		m_bVelStopPosSet = true;

		//Only set the goal position to the present position when the velocity
		//is changing to zero. If you keep setting it then the part skates.
		Stop();
	}

	if(m_iNextGoalPos != m_iLastGoalPos ||  ( (m_iNextGoalVelocity != m_iLastGoalVelocity) && !(m_iNextGoalVelocity == -1 && m_iLastGoalVelocity == 1))  )
	{
		//std::cout << GetSimulator()->Time() << ", servo: " << m_iServoID <<  ", Pos: " << m_iNextGoalPos << ", LasPos: " << m_iLastGoalPos << ", Vel: " << m_iNextGoalVelocity << ", LastVel: " << m_iLastGoalVelocity << "\r\n";
		//std::cout << "************" << GetSimulator()->Time() << ", servo: " << m_iServoID <<  ", Pos: " << m_iNextGoalPos << ", Vel: " << m_iNextGoalVelocity << "\r\n";

		//If the next goal velocity was set to -1 then we are trying to set velocity to 0. So lets set the goal position to its current
		//loctation and velocity to lowest value.
		if(m_iNextGoalVelocity == -1)
		{
			m_iNextGoalPos = m_iPresentPos;
			m_iNextGoalVelocity = 1;
		}

		//Add a new update data so we can send the move command out synchronously to all motors.
		AddMotorUpdate(m_iNextGoalPos, m_iNextGoalVelocity);

		m_iLastGoalPos = m_iNextGoalPos;
		m_iLastGoalVelocity = m_iNextGoalVelocity;

		m_fltIOPos = m_iNextGoalPos;
		m_fltIOVelocity = m_iNextGoalVelocity;
	}
}

float RbDynamixelServo::QuantizeServoPosition(float fltPos)
{
	int iPos = ConvertPosFloatToFP(fltPos);
	return ConvertPosFPToFloat(iPos);
}

float RbDynamixelServo::QuantizeServoVelocity(float fltVel)
{
	int iPos = (int) (fabs(fltVel)*m_fltConvertPosSToFP);
	return iPos*m_fltConvertFPToPosS;
}

float *RbDynamixelServo::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "READPARAMTIME")
		return &m_fltReadParamTime;
	else if(strType == "IOPOS")
		return &m_fltIOPos;
	else if(strType == "IOVELOCITY")
		return &m_fltIOVelocity;
	
	return NULL;
}

bool RbDynamixelServo::SetData(const std::string &strDataType, const std::string &strValue)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(strType == "SERVOID")
	{
		ServoID((int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "QUERYMOTORDATA")
	{
		QueryMotorData(Std_ToBool(strValue));
		return true;
	}

	if(strType == "MINPOSFP")
	{
		MinPosFP((int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "MAXPOSFP")
	{
		MaxPosFP((int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "MINANGLE")
	{
		MinAngle((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "MAXANGLE")
	{
		MaxAngle((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "MINVELOCITYFP")
	{
		MinVelocityFP((int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "MAXVELOCITYFP")
	{
		MaxVelocityFP((int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "MAXROTMIN")
	{
		MaxRotMin((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "MINLOADFP")
	{
		MinLoadFP((int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "MAXLOADFP")
	{
		MaxLoadFP((int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "TRANSLATIONRANGE")
	{
		TranslationRange(atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "RESETTOSTARTPOS")
	{
		ResetToStartPos(Std_ToBool(strValue));
		return true;
	}

	return false;
}

void RbDynamixelServo::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	aryProperties.Add(new TypeProperty("ReadParamTime", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("IOPos", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("IOVelocity", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("ServoID", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("QueryMotorData", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));

	aryProperties.Add(new TypeProperty("MinPosFP", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MaxPosFP", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MinAngle", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MaxAngle", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MinVelocityFP", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MaxVelocityFP", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MaxRotMin", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MinLoadFP", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MaxLoadFP", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("TranslationRange", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("ResetToStartPos", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
}

void RbDynamixelServo::ResetSimulation()
{
	m_fltReadParamTime = 0;
	m_fltIOPos = 0;
	m_fltIOVelocity = 0;
	m_bNeedSetVelStopPos = false;
	m_bVelStopPosSet = false;
}

void RbDynamixelServo::StepSimulation()
{
	////Test code
	//int i=5;
	//if(m_iServoID == 3 && GetSimulator()->Time() > 1) // &&  
	//	i=6;
	//if(!m_bIsHinge) // && m_lpSim->Time() > 1 
	//	i=6;

	if(m_lpMotorJoint)
	{
		//Here we need to get the set velocity for this motor that is coming from the neural controller, and then make the real motor go that speed.
		//Here we are setting the values that will be used the next time the IO is processed for this servo.
		if(m_lpMotorJoint->MotorType() == eJointMotorType::PositionVelocityControl)
		{
			float fltSetPosition = m_lpMotorJoint->SetPosition();
			float fltSetVelocity = m_lpMotorJoint->SetVelocity();
			SetNextGoalPosition(fltSetPosition);
			SetNextGoalVelocity(fltSetVelocity);
		}
		else if(m_lpMotorJoint->MotorType() == eJointMotorType::VelocityControl)
		{
			float fltSetVelocity = m_lpMotorJoint->SetVelocity();
			float fltPrevSetVel = m_lpMotorJoint->PrevSetVelocity();
			SetNextGoalVelocity(fltSetVelocity);


			if(fabs(fltSetVelocity) < 1e-4)
			{
				if(!m_bNeedSetVelStopPos)
				{
					m_bNeedSetVelStopPos = true;

					//if(m_iServoID == 3)
					//std::cout << "Time: " << GetSimulator()->TimeSlice() << " Servo: " << m_iServoID << " Pos: " <<  m_iPresentPos << "\r\n";
				}
			}
			else if(fltSetVelocity > 0)
			{
				SetNextGoalPosition_FP(m_iMaxPosFP);
				m_bNeedSetVelStopPos = false;
				m_bVelStopPosSet = false;
			}
			else
			{
				SetNextGoalPosition_FP(m_iMinPosFP);
				m_bNeedSetVelStopPos = false;
				m_bVelStopPosSet = false;
			}
		}
		else
		{
			float fltSetPosition = m_lpMotorJoint->SetPosition();
			SetNextGoalPosition(fltSetPosition);
			SetNextMaximumVelocity();
		}

		//only do this part if we are not in a simulation
		if(!GetSimulator()->InSimulation())
		{
			//Retrieve the values that we got from the last time the IO for this servo was read in.
			m_lpMotorJoint->JointPosition(m_fltPresentPos);
			m_lpMotorJoint->JointVelocity(m_fltPresentVelocity);
			m_lpMotorJoint->Temperature(m_fltVoltage);
			m_lpMotorJoint->Voltage(m_fltTemperature);
			m_lpMotorJoint->MotorTorqueToAMagnitude(m_fltLoad);
			m_lpMotorJoint->MotorTorqueToBMagnitude(m_fltLoad);
		}
		else
		{
			//If we are in a simulation then write out what the IO values should be
			m_fltIOPos = m_iNextGoalPos;
			m_fltIOVelocity = m_iNextGoalVelocity;
		}
	}
}

void RbDynamixelServo::GetLimitValues()
{
	Hinge *lpHinge = dynamic_cast<Hinge *>(m_lpMotorJoint);
	if(lpHinge)
	{
		m_fltLowLimit = lpHinge->LowerLimit()->LimitPos();
		m_fltHiLimit = lpHinge->UpperLimit()->LimitPos();
	}
	else
	{
		Prismatic *lpPrismatic = dynamic_cast<Prismatic *>(m_lpMotorJoint);
		if(lpPrismatic)
		{
			m_fltLowLimit = lpPrismatic->LowerLimit()->LimitPos();
			m_fltHiLimit = lpPrismatic->UpperLimit()->LimitPos();
		}
	}
}

void RbDynamixelServo::Load(StdUtils::CStdXml &oXml)
{
	oXml.IntoElem();

	QueryMotorData(oXml.GetChildBool("QueryMotorData", m_bQueryMotorData));
    MinPosFP(oXml.GetChildInt("MinPosFP", m_iMinPosFP));
    MaxPosFP(oXml.GetChildInt("MaxPosFP", m_iMaxPosFP));
    MinAngle(oXml.GetChildFloat("MinAngle", m_fltMinAngle));
    MaxAngle(oXml.GetChildFloat("MaxAngle", m_fltMaxAngle));
    MinVelocityFP(oXml.GetChildInt("MinVelocityFP", m_iMinVelocityFP));
    MaxVelocityFP(oXml.GetChildInt("MaxVelocityFP", m_iMaxVelocityFP));
    MaxRotMin(oXml.GetChildFloat("MaxRotMin", m_fltMaxRotMin));
    MinLoadFP(oXml.GetChildInt("MinLoadFP", m_iMinLoadFP));
    MaxLoadFP(oXml.GetChildInt("MaxLoadFP", m_iMaxLoadFP));
	m_bIsHinge = oXml.GetChildBool("IsHinge", m_bIsHinge);
    TranslationRange(oXml.GetChildFloat("TranslationRange", m_fltTranslationRange));
	ResetToStartPos(oXml.GetChildBool("ResetToStartPos", m_bResetToStartPos));

	oXml.OutOfElem();
}


		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

