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
		if(m_lpParentInterface && !m_lpParentInterface->InSimulation())
			dxl_terminate();
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

	//if(strType == "LIMITPOS")
	//	return &m_fltLimitPos;
	//else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Robot Interface ID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
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

void RbDynamixelUSB::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	RobotIOControl::QueryProperties(aryNames, aryTypes);

	aryNames.Add("ComPort");
	aryTypes.Add("Integer");

	aryNames.Add("BaudRate");
	aryTypes.Add("Integer");
}

#pragma endregion

void RbDynamixelUSB::Initialize()
{
	// Open device. Do this before calling the Initialize on the parts so they can have communications.
	if(!m_lpParentInterface->InSimulation())
	{
		if(!dxl_initialize(m_iPortNumber, m_iBaudRate))
			THROW_PARAM_ERROR(Rb_Err_lFailedDynamixelConnection, Rb_Err_strFailedDynamixelConnection, "Port", m_iPortNumber);
	}

	RobotIOControl::Initialize();
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

