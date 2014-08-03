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
}

RbDynamixelUSBServo::~RbDynamixelUSBServo()
{
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
	unsigned long long lStart = GetSimulator()->GetTimerTick(), lEnd;

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

	lEnd = GetSimulator()->GetTimerTick();
	m_fltReadParamTime = GetSimulator()->TimerDiff_m(lStart, lEnd);
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
	unsigned long long lStart = GetSimulator()->GetTimerTick(), lEnd;

	if(dxl_read_block(m_iServoID, P_PRESENT_POSITION_L, 6, aryData) && aryData.size() == 6)
	{
		m_iPresentPos = dxl_makeword(aryData[0], aryData[1]);
		m_iPresentVelocity = dxl_makeword(aryData[2], aryData[3]);
		m_iLoad = dxl_makeword(aryData[4], aryData[5]);
		
		m_fltPresentPos = ConvertPosFPToRad(m_iPresentPos);
		m_fltPresentVelocity = ConvertFPVelocity(m_iPresentVelocity);
		m_fltLoad = ConvertFPLoad(m_iLoad);
	}

	lEnd = GetSimulator()->GetTimerTick();
	m_fltReadParamTime = GetSimulator()->TimerDiff_m(lStart, lEnd);
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

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

