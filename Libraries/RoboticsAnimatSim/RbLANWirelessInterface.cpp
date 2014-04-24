// RbLANWirelessInterface.cpp: implementation of the RbLANWirelessInterface class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbRigidBody.h"
#include "RbStructure.h"
#include "RbLANWirelessInterface.h"

// Defulat setting
#define DEFAULT_PORTNUM		3 // COM3
#define DEFAULT_BAUDNUM		1 // 1Mbps
#define DEFAULT_ID			1

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotInterfaces
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbLANWirelessInterface::RbLANWirelessInterface() 
{
	m_iPortNumber = 3;
	m_iBaudRate = 1; //Max
}

RbLANWirelessInterface::~RbLANWirelessInterface()
{
	try
	{
		dxl_terminate();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbLANWirelessInterface\r\n", "", -1, false, true);}
}

void RbLANWirelessInterface::PortNumber(int iPort)
{
	Std_IsAboveMin((int) 0, iPort, true, "PortNumber", true);
	m_iPortNumber = iPort;
}

int RbLANWirelessInterface::PortNumber() {return m_iPortNumber;}

void RbLANWirelessInterface::BaudRate(int iRate)
{
	if( !((iRate == 1) || (iRate == 3) || (iRate == 4) || (iRate == 7) || (iRate == 9) ||
		  (iRate == 9) || (iRate == 16) || (iRate == 34) || (iRate == 103) || (iRate == 207)) )
		THROW_PARAM_ERROR(Rb_Err_lInvalidBaudRate, Rb_Err_strInvalidBaudRate, "Baud rate", iRate);
	m_iBaudRate = iRate;
}

int RbLANWirelessInterface::BaudRate() {return m_iBaudRate;}

#pragma region DataAccesMethods

float *RbLANWirelessInterface::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	//if(strType == "LIMITPOS")
	//	return &m_fltLimitPos;
	//else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Robot Interface ID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

bool RbLANWirelessInterface::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(AnimatBase::SetData(strDataType, strValue, false))
		return true;

	if(strType == "PORT")
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

void RbLANWirelessInterface::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("ComPort");
	aryTypes.Add("Integer");

	aryNames.Add("BaudRate");
	aryTypes.Add("Integer");
}

#pragma endregion


void RbLANWirelessInterface::Initialize()
{
	// Open device
	if(!dxl_initialize(m_iPortNumber, m_iBaudRate))
		THROW_PARAM_ERROR(Rb_Err_lFailedDynamixelConnection, Rb_Err_strFailedDynamixelConnection, "Port", m_iPortNumber);
}

void RbLANWirelessInterface::StepSimulation()
{
}

void RbLANWirelessInterface::Load(StdUtils::CStdXml &oXml)
{
}
		}		//RobotInterfaces
	}			// Robotics
}				//RoboticsAnimatSim

