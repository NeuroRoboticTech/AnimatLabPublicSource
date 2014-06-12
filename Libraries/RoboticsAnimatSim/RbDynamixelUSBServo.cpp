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

	m_iMinSimPos = m_iMinPos;
	m_iMaxSimPos = m_iMaxPos;
	m_fltMinSimPos = m_fltMinPos;
	m_fltMaxSimPos = m_fltMaxPos;

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
\brief	Sets the fixed point value of the servo velocity for the next time the IO for this servo is processed. 

\author	dcofer
\date	4/25/2014

\param	iVelocity	The new value fixed point velocity.
**/
void RbDynamixelUSBServo::SetNextGoalVelocity_FP(int iVelocity)
{
	//Verify we are not putting invalid values in.
	if(iVelocity != -1 && iVelocity < m_iMinVelocity) iVelocity = m_iMinVelocity;
	if(iVelocity != -1 && iVelocity > m_iMaxVelocity) iVelocity = m_iMaxVelocity;

	m_iNextGoalVelocity = iVelocity;
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
\brief	Sets the servo goal speed to 0 for maximum velocity the next time the IO for this servo is processed. 

\author	dcofer
\date	4/25/2014
**/
void RbDynamixelUSBServo::SetNextMaximumVelocity()
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

	SetGoalVelocity_FP(iVel);
}

/**
\brief	Sets the floating point value of the servo velocity the next time the IO for this servo is processed. This is the velocity in rad/s. It will be converted to a fixed
point value based on the motor configuration and then used to set the velocity.

\author	dcofer
\date	4/25/2014

\param	fltVelocity	The new value velocity in rad/s.
**/
void RbDynamixelUSBServo::SetNextGoalVelocity(float fltVelocity)
{
	int iVel = (int) (fabs(fltVelocity)*m_fltConvertRadSToFP);

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
	if(iPos < m_iMinSimPos) iPos = m_iMinSimPos;
	if(iPos > m_iMaxSimPos) iPos = m_iMaxSimPos;

	//If the position we are setting is the same as we just set then no point sending it.
	if(m_iLastGoalPos == iPos)
		return;

	m_iLastGoalPos = iPos;

	dxl_write_word(m_iServoID, P_GOAL_POSITION_L, iPos );
}

/**
\brief	Sets the fixed point value of the servo goal position that will be used the next time the IO for this servo is processed. This is the exact value that will be sent to the servo. 

\author	dcofer
\date	4/25/2014

\param	iPos	The new value fixed point position.
**/
void RbDynamixelUSBServo::SetNextGoalPosition_FP(int iPos)
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
\brief	Sets the floating point value of the servo goal position that will be used the next time the iO for this servo is processed. 
This is the position in radians. It will be converted to a fixed
point value based on the motor configuration and then used to set the position. This method assumes that center value is 0 and that
it uses +/- values on either side.

\author	dcofer
\date	4/25/2014

\param	fltPos	The new value position in radians.
**/
void RbDynamixelUSBServo::SetNextGoalPosition(float fltPos)
{
	int iPos = ConvertPosRadToFP(fltPos);

	SetNextGoalPosition_FP(iPos);
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

\param	iVel	The fixed point velocity.

\return	Floating point value of velocity from the servo in rad/s. 
**/
float RbDynamixelUSBServo::ConvertFPVelocity(int iVel)
{
	int iDir = 1;
	if(iVel > m_iMaxVelocity)
	{
		iVel -= m_iMaxVelocity;
		iDir = -1;
	}

	float fltVel = iDir*iVel*m_fltConvertFPToRadS;
	return fltVel;
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
	return ConvertFPVelocity(iVel);
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
\brief	Gets the floating point value of the servo load. The fixed point value is retrieved from the servo and then converted. 

\author	dcofer
\date	4/25/2014

\param	iVel	The fixed point load.

\return	Floating point value of velocity from the servo in rad/s. 
**/
float RbDynamixelUSBServo::ConvertFPLoad(int iLoad)
{
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
\brief	Gets the floating point value of the servo actual torque. The fixed point value is retrieved from the servo and then converted to Nm. 

\author	dcofer
\date	4/25/2014

\return	Floating point value of actual torque from the servo in Nm. 
**/
float RbDynamixelUSBServo::GetActualLoad()
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
	int iMinPos = GetCWAngleLimit_FP();
	int iMaxPos = GetCCWAngleLimit_FP();
	int iRetDelay = GetReturnDelayTime();

	if(iMinPos != m_iMinSimPos)
		SetCWAngleLimit_FP(m_iMinSimPos);

	if(iMaxPos != m_iMaxSimPos)
		SetCCWAngleLimit_FP(m_iMaxSimPos);

	if(iRetDelay > 1)
		SetReturnDelayTime(1);

	SetMaximumVelocity();
	SetGoalPosition(0);

	do
	{
#ifdef Win32
		//Do not attempt to sleep in linux while in a spinlock. Windows is fine with it.
		MicroSleep(5000);
#endif
	} while(GetIsMoving());

	m_iLastGoalPos = GetActualPosition_FP();

	//Reset the goal velocity to the minimum value.
	SetGoalVelocity_FP(1);

	std::cout << "Reset Position: " << m_iLastGoalPos << "\r\n";
}

/**
\brief	Shuts the motor down cleanly and ensures that it is not continuing to move after processing has stopped. 

\author	dcofer
\date	4/25/2014
**/
void RbDynamixelUSBServo::ShutdownMotor()
{
	int iCurPos = GetActualPosition_FP();

	//Reset the goal velocity to the minimum value.
	SetGoalVelocity_FP(1);
	SetGoalPosition_FP(iCurPos);

	std::cout << "Shutting down servo: " << m_iServoID << ", Pos: " << iCurPos << "\r\n";
}

/**
\brief	Reads all major data parameters from the servo in one read packet. This include present position, 
speed, load, temperature, and voltage. It sets the corresponding internal variables.

\author	dcofer
\date	5/7/2014
**/
void RbDynamixelUSBServo::ReadAllParams()
{
	std::vector<int> aryData;

	if(dxl_read_block(m_iServoID, P_PRESENT_POSITION_L, 8, aryData) && aryData.size() == 8)
	{
		m_iPresentPos = dxl_makeword(aryData[0], aryData[1]);
		m_iPresentVelocity = dxl_makeword(aryData[2], aryData[3]);
		m_iLoad = dxl_makeword(aryData[4], aryData[5]);
		m_iVoltage = aryData[6];
		m_iTemperature = aryData[7];

		m_fltPresentPos = ConvertPosFPToRad(m_iPresentPos);
		m_fltPresentVelocity = ConvertFPVelocity(m_iPresentVelocity);
		m_fltLoad = ConvertFPLoad(m_iLoad);
		m_fltVoltage = m_iVoltage/100.0;
		m_fltTemperature = (float) m_iTemperature;
	}
}

/**
\brief	Reads only the key data parameters from the servo in one read packet. This include present position and speed. 
It sets the corresponding internal variables.

\author	dcofer
\date	5/7/2014
**/
void RbDynamixelUSBServo::ReadKeyParams()
{
	std::vector<int> aryData;

	if(dxl_read_block(m_iServoID, P_PRESENT_POSITION_L, 6, aryData) && aryData.size() == 6)
	{
		m_iPresentPos = dxl_makeword(aryData[0], aryData[1]);
		m_iPresentVelocity = dxl_makeword(aryData[2], aryData[3]);
		m_iLoad = dxl_makeword(aryData[4], aryData[5]);

		m_fltPresentPos = ConvertPosFPToRad(m_iPresentPos);
		m_fltPresentVelocity = ConvertFPVelocity(m_iPresentVelocity);
		m_fltLoad = ConvertFPLoad(m_iLoad);
	}
}

/**
\brief	Reads an entire block of data from a dynamixel servo. This is used to allow us to read back a number of 
params at one time instead of having to seperate them into different packets.

\author	dcofer
\date	5/7/2014

\param	id	ID of the servo to query.
\param	address	address in the control block to start reading.
\param	length	length of the data to read.
\param	aryData	Data that was read.

\return	True if it succedded in the read. 
**/
bool RbDynamixelUSBServo::dxl_read_block( int id, int address, int length, std::vector<int> &aryData)
{
	// Make a packet to read a bunch of data at once.
	dxl_set_txpacket_id(id);
	dxl_set_txpacket_instruction(INST_READ);
	dxl_set_txpacket_parameter(0, address);
	dxl_set_txpacket_parameter(1, length);
	dxl_set_txpacket_length(4);

	dxl_txrx_packet();

	int CommStatus = dxl_get_result();

	if(!CommStatus)
	{
		return false;
	}

	aryData.clear();
	for(int iIdx=0; iIdx<length; iIdx++)
	{
		int iParam = dxl_get_rxpacket_parameter(iIdx);
		aryData.push_back(iParam);
	}

	return true;
}

/**
\brief	Checks the error code and returns an associated error message.

\author	dcofer
\date	5/7/2014

\return	Error message. 
**/
std::string RbDynamixelUSBServo::GetErrorCode()
{
	if(dxl_get_rxpacket_error(ERRBIT_VOLTAGE) == 1)
		return "Input voltage error!";

	if(dxl_get_rxpacket_error(ERRBIT_ANGLE) == 1)
		return "Angle limit error!\n";

	if(dxl_get_rxpacket_error(ERRBIT_OVERHEAT) == 1)
		return "Overheat error!\n";

	if(dxl_get_rxpacket_error(ERRBIT_RANGE) == 1)
		return "Out of range error!\n";

	if(dxl_get_rxpacket_error(ERRBIT_CHECKSUM) == 1)
		return "Checksum error!\n";

	if(dxl_get_rxpacket_error(ERRBIT_OVERLOAD) == 1)
		return "Overload error!\n";

	if(dxl_get_rxpacket_error(ERRBIT_INSTRUCTION) == 1)
		return "Instruction code error!\n";	

	return "Unknown error";
}

std::string RbDynamixelUSBServo::GetCommStatus(int CommStatus)
{
	switch(CommStatus)
	{
	case COMM_TXFAIL:
		return "COMM_TXFAIL: Failed transmit instruction packet!";
		break;

	case COMM_TXERROR:
		return "COMM_TXERROR: Incorrect instruction packet!";
		break;

	case COMM_RXFAIL:
		return "COMM_RXFAIL: Failed get status packet from device!";
		break;

	case COMM_RXWAITING:
		return "COMM_RXWAITING: Now recieving status packet!";
		break;

	case COMM_RXTIMEOUT:
		return "COMM_RXTIMEOUT: There is no status packet!";
		break;

	case COMM_RXCORRUPT:
		return "COMM_RXCORRUPT: Incorrect status packet!";
		break;

	default:
		return "This is unknown error code!";
		break;
	}
}

/**
\brief	Sets the return delay time of the servo.

\author	dcofer
\date	5/7/2014

\param	iVal	delay time
**/
void RbDynamixelUSBServo::SetReturnDelayTime(int iVal)
{
	if(iVal >= 0 && iVal < 256)
		dxl_write_byte(m_iServoID, P_RETURN_DELAY_TIME, iVal);
}

/**
\brief	Gets the return delay time of the servo.

\author	dcofer
\date	5/7/2014

\return	delay time 
**/
int RbDynamixelUSBServo::GetReturnDelayTime()
{
	return dxl_read_byte(m_iServoID, P_RETURN_DELAY_TIME);
}


/**
\brief	Sets the limit for the CCW limit of the servo using fixed point value.

\author	dcofer
\date	5/9/2014

\param	iVal	limit
**/
void RbDynamixelUSBServo::SetCCWAngleLimit_FP(int iVal)
{
	if(iVal >= m_iMinPos && iVal <= m_iMaxPos)
		dxl_write_word(m_iServoID, P_CCW_ANGLE_LIMIT_L, iVal);
}

/**
\brief	Sets the limit for the CCW limit of the servo using radians.

\author	dcofer
\date	5/9/2014

\param	fltVal	limit
**/
void RbDynamixelUSBServo::SetCCWAngleLimit(float fltVal)
{
	int iPos = ConvertPosRadToFP(fltVal);
	SetCCWAngleLimit_FP(iPos);
}

/**
\brief	Gets the limit for the CCW limit of the servo in fixed point value.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
int RbDynamixelUSBServo::GetCCWAngleLimit_FP()
{
	return dxl_read_word(m_iServoID, P_CCW_ANGLE_LIMIT_L);
}

/**
\brief	Gets the limit for the CCW limit of the servo in radian values.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
float RbDynamixelUSBServo::GetCCWAngleLimit()
{
	int iPos = dxl_read_word(m_iServoID, P_CCW_ANGLE_LIMIT_L);
	return ConvertPosFPToRad(iPos);
}

/**
\brief	Sets the limit for the CW limit of the servo using fixed point value.

\author	dcofer
\date	5/9/2014

\param	iVal	limit
**/
void RbDynamixelUSBServo::SetCWAngleLimit_FP(int iVal)
{
	if(iVal >= m_iMinPos && iVal <= m_iMaxPos)
		dxl_write_word(m_iServoID, P_CW_ANGLE_LIMIT_L, iVal);
}

/**
\brief	Sets the limit for the CW limit of the servo using radians.

\author	dcofer
\date	5/9/2014

\param	fltVal	limit
**/
void RbDynamixelUSBServo::SetCWAngleLimit(float fltVal)
{
	int iPos = ConvertPosRadToFP(fltVal);
	SetCWAngleLimit_FP(iPos);
}

/**
\brief	Gets the limit for the CW limit of the servo in fixed point value.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
int RbDynamixelUSBServo::GetCWAngleLimit_FP()
{
	return dxl_read_word(m_iServoID, P_CW_ANGLE_LIMIT_L);
}

/**
\brief	Gets the limit for the CW limit of the servo in radian values.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
float RbDynamixelUSBServo::GetCWAngleLimit()
{
	int iPos = dxl_read_word(m_iServoID, P_CW_ANGLE_LIMIT_L);
	return ConvertPosFPToRad(iPos);
}

/**
\brief	Gets the minimum position in fixed point that the simulation will allow for the joint.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
int RbDynamixelUSBServo::GetMinSimPos_FP() {return m_iMinSimPos;}

/**
\brief	Gets the minimum position in radians that the simulation will allow for the joint.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
float RbDynamixelUSBServo::GetMinSimPos()  {return m_fltMinSimPos;}

/**
\brief	Sets the minimum position that the simulation will allow for this joint in radians.

\author	dcofer
\date	5/9/2014

\param	fltVal	limit
**/
void RbDynamixelUSBServo::SetMinSimPos(float fltVal)
{
	m_iMinSimPos = ConvertPosRadToFP(fltVal);

	if(m_iMinSimPos < m_iMinPos) m_iMinSimPos = m_iMinPos;
	if(m_iMinSimPos > m_iMaxPos) m_iMinSimPos = m_iMaxPos;

	m_fltMinSimPos = ConvertPosFPToRad(m_iMinSimPos);
}

/**
\brief	Gets the maximum position in fixed point that the simulation will allow for the joint.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
int RbDynamixelUSBServo::GetMaxSimPos_FP() {return m_iMaxSimPos;}

/**
\brief	Gets the maximum position in radians that the simulation will allow for the joint.

\author	dcofer
\date	5/9/2014

\return	limit 
**/
float RbDynamixelUSBServo::GetMaxSimPos()  {return m_fltMaxSimPos;}

/**
\brief	Sets the maximum position that the simulation will allow for this joint in radians.

\author	dcofer
\date	5/9/2014

\param	fltVal	limit
**/
void RbDynamixelUSBServo::SetMaxSimPos(float fltVal)
{
	m_iMaxSimPos = ConvertPosRadToFP(fltVal);

	if(m_iMaxSimPos < m_iMinPos) m_iMaxSimPos = m_iMinPos;
	if(m_iMaxSimPos > m_iMaxPos) m_iMaxSimPos = m_iMaxPos;

	m_fltMaxSimPos = ConvertPosFPToRad(m_iMaxSimPos);
}

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

