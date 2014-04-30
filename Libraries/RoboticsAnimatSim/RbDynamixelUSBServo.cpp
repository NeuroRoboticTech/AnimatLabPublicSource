// RbDynamixelUSBServo.cpp: implementation of the RbDynamixelUSBServo class.
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
#include "RbDynamixelUSBServo.h"

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace DynamixelUSB
			{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbDynamixelUSBServo::RbDynamixelUSBServo() 
{
	m_iServoID = 0;
	m_iMinPos = 0;
	m_iMaxPos = 1023;
	m_iMaxAngle = (m_iMaxPos - m_iMinPos);
	m_iCenterPosFP = round(m_iMaxAngle/2.0);

	m_fltMaxAngle = (float) ((300.0/180.0)*RB_PI);
	m_fltMinPos = -(m_fltMaxAngle/2);
	m_fltMaxPos = (m_fltMaxAngle/2);

	m_fltPosFPToRadSlope = (m_fltMaxAngle/m_iMaxAngle);
	m_fltPosFPToRadIntercept = -(m_fltPosFPToRadSlope*m_iCenterPosFP);

	m_fltPosRadToFPSlope = (m_iMaxAngle/m_fltMaxAngle);
	m_fltPosRadToFPIntercept = m_iCenterPosFP;

	m_iLastGoalPos = -1;

	m_iMinVelocity = 1;
	m_iMaxVelocity = 1023;
	m_fltConvertFPToRadS = 0.01162389281828223498231178051813f;  //0.111 rot/min = 0.0116 rad/s
	m_fltConvertRadSToFP = 1/m_fltConvertFPToRadS;
	m_iLastGoalVelocity = -10000;

	m_iMinLoad = 0;
	m_iMaxLoad = 1023;
	m_fltConvertFPToLoad = 100.0/(m_iMaxLoad - m_iMinLoad);


	
}

RbDynamixelUSBServo::~RbDynamixelUSBServo()
{
}

float RbDynamixelUSBServo::ConvertPosFPToRad(int iPos)
{
	float fltPos = (m_fltPosFPToRadSlope*iPos) + m_fltPosFPToRadIntercept;
	return fltPos;
}

int RbDynamixelUSBServo::ConvertPosRadToFP(float fltPos)
{
	int iPos = (m_fltPosRadToFPSlope*fltPos) + m_fltPosRadToFPIntercept;
	return iPos;
}

/**
\brief	Sets the ID that will be used for communications with this servo. This is NOT writing an ID to the servo. It is just 
specifying which one to use for com access.

\author	dcofer
\date	4/25/2014

\param	iID	The ID value to use for communications.
**/void RbDynamixelUSBServo::ServoID(int iID)
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
int RbDynamixelUSBServo::ServoID() {return m_iServoID;}

/**
\brief	Sets the fixed point value of the servo velocity. This is the exact value that will be sent to the servo. 
This method excludes the velocity of 0 which defaults it to move at its fastest speed.  If you want to do that
then use the SetMaximumVelocity method.

\author	dcofer
\date	4/25/2014

\param	iVelocity	The new value fixed point velocity.
**/
void RbDynamixelUSBServo::SetGoalVelocity_FP(int iVelocity)
{
	//Verify we are not putting invalid values in.
	if(iVelocity < m_iMinVelocity) iVelocity = m_iMinVelocity;
	if(iVelocity > m_iMaxVelocity) iVelocity = m_iMaxVelocity;

	//If the velocity we are setting is the same as we just set then no point sending it.
	if(m_iLastGoalVelocity == iVelocity)
		return;

	m_iLastGoalVelocity = iVelocity;

	dxl_write_word(m_iServoID, P_MOVING_SPEED_L, iVelocity );
}

/**
\brief	Sets the servo goal speed to 0 for maximum velocity. 

\author	dcofer
\date	4/25/2014
**/
void RbDynamixelUSBServo::SetMaximumVelocity()
{
	//If the velocity we are setting is the same as we just set then no point sending it.
	if(m_iLastGoalVelocity == 0)
		return;

	m_iLastGoalVelocity = 0;

	dxl_write_word(m_iServoID, P_MOVING_SPEED_L, 0 );
}

/**
\brief	Gets the fixed point value of the servo velocity. This is the exact value returned from the servo. 

\author	dcofer
\date	4/25/2014

\return	Fixed point value of velocity from the servo. 
**/
int RbDynamixelUSBServo::GetGoalVelocity_FP()
{
	int iGoalVel = dxl_read_word(m_iServoID, P_MOVING_SPEED_L);

	return iGoalVel;
}

/**
\brief	Sets the floating point value of the servo velocity. This is the velocity in rad/s. It will be converted to a fixed
point value based on the motor configuration and then used to set the velocity.

\author	dcofer
\date	4/25/2014

\param	fltVelocity	The new value velocity in rad/s.
**/
void RbDynamixelUSBServo::SetGoalVelocity(float fltVelocity)
{
	int iVel = (int) (fabs(fltVelocity)*m_fltConvertRadSToFP);

	SetGoalVelocity_FP(iVel);}

/**
\brief	Gets the floating point value of the servo velocity. The fixed point value is retrieved from the servo and then converted to rad/s. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of velocity from the servo in rad/s. 
**/
float RbDynamixelUSBServo::GetGoalVelocity()
{
	int iGoal = GetGoalVelocity_FP();

	float fltPos = iGoal*m_fltConvertFPToRadS;

	return fltPos;
}

/**
\brief	Gets the last goal velocity that was sent to the servo. This is used so we can see the last value we set without having to requery it from the servo. 

\author	dcofer
\date	4/25/2014

\return	Last goal velocity set. 
**/
int RbDynamixelUSBServo::LastGoalVelocity_FP() {return m_iLastGoalVelocity;}

/**
\brief	Sets the fixed point value of the servo goal position. This is the exact value that will be sent to the servo. 

\author	dcofer
\date	4/25/2014

\param	iPos	The new value fixed point position.
**/
void RbDynamixelUSBServo::SetGoalPosition_FP(int iPos)
{
	//Verify we are not putting invalid values in.
	if(iPos < m_iMinPos) iPos = m_iMinPos;
	if(iPos > m_iMaxPos) iPos = m_iMaxPos;

	//If the position we are setting is the same as we just set then no point sending it.
	if(m_iLastGoalPos == iPos)
		return;

	m_iLastGoalPos = iPos;

	dxl_write_word(m_iServoID, P_GOAL_POSITION_L, iPos );
}

/**
\brief	Gets the fixed point value of the servo goal position. This is the exact value returned from the servo. 

\author	dcofer
\date	4/25/2014

\return	Fixed point value of goal position from the servo. 
**/
int RbDynamixelUSBServo::GetGoalPosition_FP()
{
	int iGoalPos = dxl_read_word(m_iServoID, P_GOAL_POSITION_L);
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
void RbDynamixelUSBServo::SetGoalPosition(float fltPos)
{
	int iPos = ConvertPosRadToFP(fltPos);

	SetGoalPosition_FP(iPos);
}

/**
\brief	Gets the floating point value of the servo goal position. The fixed point value is retrieved from the servo and then converted to radians. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of goal position from the servo in radians. 
**/
float RbDynamixelUSBServo::GetGoalPosition()
{
	int iGoal = GetGoalPosition_FP();

	float fltPos = ConvertPosFPToRad(iGoal);

	return fltPos;
}

/**
\brief	Gets the last goal position that was sent to the servo. This is used so we can see the last value we set without having to requery it from the servo. 

\author	dcofer
\date	4/25/2014

\return	Last goal position set. 
**/
int RbDynamixelUSBServo::LastGoalPosition_FP() {return m_iLastGoalPos;}

/**
\brief	Gets the fixed point value of the servo actual position. This is the exact value returned from the servo. 

\author	dcofer
\date	4/25/2014

\return	Fixed point value of actual position from the servo. 
**/
int RbDynamixelUSBServo::GetActualPosition_FP()
{
	int iPos = dxl_read_word(m_iServoID, P_PRESENT_POSITION_L);
	return iPos;
}

/**
\brief	Gets the floating point value of the servo actual position. The fixed point value is retrieved from the servo and then converted to radians. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of goal position from the servo in radians. 
**/
float RbDynamixelUSBServo::GetActualPosition()
{
	int iPos = GetActualPosition_FP();

	float fltPos = ConvertPosFPToRad(iPos);

	return fltPos;
}

/**
\brief	Gets the fixed point value of the servo actual velocity. This is the exact value returned from the servo. 

\author	dcofer
\date	4/25/2014

\return	Fixed point value of actual velocity from the servo. 
**/
int RbDynamixelUSBServo::GetActualVelocity_FP()
{
	int iVel = dxl_read_word(m_iServoID, P_PRESENT_SPEED_L);
	return iVel;
}

/**
\brief	Gets the floating point value of the servo actual velocity. The fixed point value is retrieved from the servo and then converted to rad/s. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of actual velocity from the servo in rad/s. 
**/
float RbDynamixelUSBServo::GetActualVelocity()
{
	int iVel = GetActualVelocity_FP();

	int iDir = 1;
	if(iVel > m_iMaxVelocity)
	{
		iVel -= m_iMaxVelocity;
		iDir = -1;
	}

	float fltPos = iDir*iVel*m_fltConvertFPToRadS;

	return fltPos;
}

/**
\brief	Gets the fixed point value of the servo actual load. This is the exact value returned from the servo. 

\author	dcofer
\date	4/25/2014

\return	Fixed point value of actual load from the servo. 
**/
int RbDynamixelUSBServo::GetActualLoad_FP()
{
	int iLoad = dxl_read_word(m_iServoID, P_PRESENT_SPEED_L);
	return iLoad;
}

/**
\brief	Gets the floating point value of the servo actual torque. The fixed point value is retrieved from the servo and then converted to Nm. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of actual torque from the servo in Nm. 
**/
float RbDynamixelUSBServo::GetActualLoad()
{
	int iLoad = GetActualLoad_FP();

	int iDir = -1;
	if(iLoad > m_iMaxLoad)
	{
		iLoad -= m_iMaxLoad;
		iDir = 1;
	}

	float fltLoad = iDir*iLoad*m_fltConvertFPToLoad;

	return fltLoad;
}

/**
\brief	Gets the fixed point value of the servo actual voltage. This is the exact value returned from the servo. 

\author	dcofer
\date	4/25/2014

\return	Fixed point value of actual voltage from the servo. 
**/
int RbDynamixelUSBServo::GetActualVoltage_FP()
{
	int iVoltage = dxl_read_byte(m_iServoID, P_PRESENT_VOLTAGE);
	return iVoltage;
}

/**
\brief	Gets the floating point value of the servo actual voltage. The fixed point value is retrieved from the servo and then converted to V. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of actual voltage from the servo in V. 
**/
float RbDynamixelUSBServo::GetActualVoltage()
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
float RbDynamixelUSBServo::GetActualTemperatureFahrenheit()
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
float RbDynamixelUSBServo::GetActualTemperatureCelcius()
{
	float fltTemp = (float) dxl_read_byte(m_iServoID, P_PRESENT_TEMPERATURE);
	return fltTemp;
}

/**
\brief	Returns whether the servo is currently moving or has already reached its goal state. 

\author	dcofer
\date	4/25/2014

\return	True for moving. 
**/
bool RbDynamixelUSBServo::GetIsMoving()
{
	int iMoving = dxl_read_byte(m_iServoID, P_MOVING);

	if(iMoving)
		return true;
	else
		return false;
}

/**
\brief	Returns whether the servo blue led is currently on. 

\author	dcofer
\date	4/25/2014

\param	bIsBlueOn	True if blue LED is on.
\param	bIsGreenOn	True if green LED is on.
\param	bIsRedOn	True if red LED is on.
**/
void RbDynamixelUSBServo::GetIsLEDOn(bool &bIsBlueOn, bool &bIsGreenOn, bool &bIsRedOn)
{
	int iLED = dxl_read_byte(m_iServoID, P_LED);

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
bool RbDynamixelUSBServo::GetIsAlarmShutdown()
{
	int iAlarm = dxl_read_byte(m_iServoID, P_ALARM_SHUTDOWN);

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
int RbDynamixelUSBServo::GetModelNumber()
{
	int iModel = dxl_read_word(m_iServoID, P_MODEL_NUMBER_L);
	return iModel;
}

/**
\brief	Returns whether the servo ID number. 

\author	dcofer
\date	4/25/2014

\return	ID Number. 
**/
int RbDynamixelUSBServo::GetIDNumber()
{
	int iID = dxl_read_byte(m_iServoID, P_ID);
	return iID;
}

/**
\brief	Returns whether the servo firmware version number. 

\author	dcofer
\date	4/25/2014

\return	Firmware number. 
**/
int RbDynamixelUSBServo::GetFirmwareVersion()
{
	int iID = dxl_read_byte(m_iServoID, P_FIRMWARE_VERSION);
	return iID;
}

/**
\brief	Initializes the internal data on position and velocity from the actual motor. 

\author	dcofer
\date	4/25/2014
**/
void RbDynamixelUSBServo::InitMotorData()
{
	m_iLastGoalPos = GetActualPosition_FP();
	SetMaximumVelocity();
	SetGoalPosition(0);

	do
	{
		Std_Sleep(100);
	} while(GetIsMoving());
}


			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

