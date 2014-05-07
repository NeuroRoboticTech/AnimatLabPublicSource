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
#include <platformstl/synch/sleep_functions.h>

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

void RbDynamixelUSB::Initialize()
{
	// Open device. Do this before calling the Initialize on the parts so they can have communications.
	if(!m_lpParentInterface->InSimulation())
	{
		if(!dxl_initialize(m_iPortNumber, m_iBaudRate))
			THROW_PARAM_ERROR(Rb_Err_lFailedDynamixelConnection, Rb_Err_strFailedDynamixelConnection, "Port", m_iPortNumber);

		StartIOThread();
	}

	RobotIOControl::Initialize();
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
			m_aryMotorData.RemoveAll();
			StepIO();
			SendSynchronousMoveCommand();
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

void RbDynamixelUSB::ExitIOThread()
{
	RobotIOControl::ExitIOThread();

	if(m_lpParentInterface && !m_lpParentInterface->InSimulation())
		dxl_terminate();
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
			RbDynamixelUSBMotorUpdateData *lpServo = m_aryMotorData[iServo];

			int iOffset = (2+(iDataPerServo)*iServo);
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
		//else
		//{
		//	PrintCommStatus(CommStatus);
		//	break;
		//}

	}

	return false;
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

