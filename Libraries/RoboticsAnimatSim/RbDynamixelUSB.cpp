// RbDynamixelUSB.cpp: implementation of the RbDynamixelUSB class.
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
#include "RbDynamixelUSB.h"
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

RbDynamixelUSB::RbDynamixelUSB() 
{
	m_iPortNumber = 3;
	m_iBaudRate = 1; //Max
}

RbDynamixelUSB::~RbDynamixelUSB()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbDynamixelUSB\r\n", "", -1, false, true);}
}

void RbDynamixelUSB::PortNumber(int iPort)
{
	Std_IsAboveMin((int) 0, iPort, true, "PortNumber", true);
	m_iPortNumber = iPort;
}

int RbDynamixelUSB::PortNumber() {return m_iPortNumber;}

void RbDynamixelUSB::BaudRate(int iRate)
{
	if( !((iRate == 1) || (iRate == 3) || (iRate == 4) || (iRate == 7) || (iRate == 9) ||
		  (iRate == 9) || (iRate == 16) || (iRate == 34) || (iRate == 103) || (iRate == 207)) )
		THROW_PARAM_ERROR(Rb_Err_lInvalidBaudRate, Rb_Err_strInvalidBaudRate, "Baud rate", iRate);
	m_iBaudRate = iRate;
}

int RbDynamixelUSB::BaudRate() {return m_iBaudRate;}
			
void RbDynamixelUSB::SetRegister(unsigned char iServo, unsigned char reg, unsigned char length, unsigned int value)
{
	if(length == 2)
		dxl_write_word(iServo, reg, value);
	else
		dxl_write_byte(iServo, reg, value);
}

int RbDynamixelUSB::GetRegister(unsigned char iServo, unsigned char reg, unsigned char length)
{
	if(length == 2)
		return dxl_read_word(iServo, reg);
	else
		return dxl_read_byte(iServo, reg);
}

#pragma region DataAccesMethods

float *RbDynamixelUSB::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	return RobotIOControl::GetDataPointer(strDataType);
}

bool RbDynamixelUSB::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(RobotIOControl::SetData(strDataType, strValue, false))
		return true;

	if(strType == "PORTNUMBER")
	{
		PortNumber((int) atoi(strValue.c_str()));
		return true;
	}
	else if(strType == "BAUDRATE")
	{
		BaudRate((int) atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RbDynamixelUSB::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RobotIOControl::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("ComPort", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("BaudRate", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

#pragma endregion

bool RbDynamixelUSB::OpenIO()
{
	if(!dxl_initialize(m_iPortNumber, m_iBaudRate))
		THROW_PARAM_ERROR(Rb_Err_lFailedDynamixelConnection, Rb_Err_strFailedDynamixelConnection, "Port", m_iPortNumber);

	return true;
}

void RbDynamixelUSB::CloseIO()
{
	TRACE_DEBUG("CloseIO.");

	if(!m_lpSim->InSimulation())
		dxl_terminate();
}

void RbDynamixelUSB::ProcessIO()
{
	try
	{
		m_bIOThreadProcessing = true;

		SetupIO();

		m_bSetupComplete = true;
		m_WaitForIOSetupCond.notify_all();

		while(!m_bStopIO)
		{
			if(m_bPauseIO || m_lpSim->Paused())
			{
				m_bIOPaused = true;
				boost::this_thread::sleep(boost::posix_time::microseconds(1000));
			}
			else
			{
				m_bIOPaused = false;

				m_aryMotorData.RemoveAll();
				StepIO();
				SendSynchronousMoveCommand();
			}

#ifndef Win32
		//Not needed in windows, not sure in linux. Keep it in till verify.
		m_lpSim->MicroSleep(15000);
#endif
		}
	}
	catch(CStdErrorInfo oError)
	{
		m_bIOThreadProcessing = false;
	}
	catch(...)
	{
		m_bIOThreadProcessing = false;
	}

	m_bIOThreadProcessing = false;
}

bool RbDynamixelUSB::SendSynchronousMoveCommand()
{
	int iServos = m_aryMotorData.GetSize();
	int iDataPerServo = 4; 

	if(iServos > 0)
	{
		dxl_set_txpacket_id(BROADCAST_ID);
		dxl_set_txpacket_instruction(INST_SYNC_WRITE);
		dxl_set_txpacket_parameter(0, P_GOAL_POSITION_L);
		dxl_set_txpacket_parameter(1, iDataPerServo);

		for(int iServo=0; iServo<iServos; iServo++ )
		{
			RbDynamixelMotorUpdateData *lpServo = m_aryMotorData[iServo];

			int iOffset = (2+(iDataPerServo+1)*iServo);
			dxl_set_txpacket_parameter((iOffset+0), lpServo->m_iID);
			dxl_set_txpacket_parameter((iOffset+1), dxl_get_lowbyte(lpServo->m_iGoalPos));
			dxl_set_txpacket_parameter((iOffset+2), dxl_get_highbyte(lpServo->m_iGoalPos));
			dxl_set_txpacket_parameter((iOffset+3), dxl_get_lowbyte(lpServo->m_iGoalVelocity));
			dxl_set_txpacket_parameter((iOffset+4), dxl_get_highbyte(lpServo->m_iGoalVelocity));
		}

		dxl_set_txpacket_length((iDataPerServo+1)*iServos+4);

		dxl_txrx_packet();
		int CommStatus = dxl_get_result();
		if( CommStatus == COMM_RXSUCCESS )
			return true;
		else
		{
			std::cout << GetCommStatus(CommStatus);
		}
	}

	return false;
}			

/**
\brief	Checks the error code and returns an associated error message.

\author	dcofer
\date	5/7/2014

\return	Error message. 
**/
std::string RbDynamixelUSB::GetErrorCode()
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

std::string RbDynamixelUSB::GetCommStatus(int CommStatus)
{
	switch(CommStatus)
	{
	case COMM_TXFAIL:
		return "COMM_TXFAIL: Failed transmit instruction packet!\r\n";
		break;

	case COMM_TXERROR:
		return "COMM_TXERROR: Incorrect instruction packet!\r\n";
		break;

	case COMM_RXFAIL:
		return "COMM_RXFAIL: Failed get status packet from device!\r\n";
		break;

	case COMM_RXWAITING:
		return "COMM_RXWAITING: Now recieving status packet!\r\n";
		break;

	case COMM_RXTIMEOUT:
		return "COMM_RXTIMEOUT: There is no status packet!\r\n";
		break;

	case COMM_RXCORRUPT:
		return "COMM_RXCORRUPT: Incorrect status packet!\r\n";
		break;

	default:
		return "This is unknown error code!\r\n";
		break;
	}
}

void RbDynamixelUSB::Load(StdUtils::CStdXml &oXml)
{
	RobotIOControl::Load(oXml);

	oXml.IntoElem();
	PortNumber(oXml.GetChildInt("PortNumber", m_iPortNumber));
	BaudRate(oXml.GetChildInt("BaudRate", m_iBaudRate));
	oXml.OutOfElem();
}


			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

