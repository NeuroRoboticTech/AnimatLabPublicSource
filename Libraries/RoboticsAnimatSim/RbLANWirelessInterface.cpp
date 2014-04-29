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
}

RbLANWirelessInterface::~RbLANWirelessInterface()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbLANWirelessInterface\r\n", "", -1, false, true);}
}

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
	
	if(RobotInterface::SetData(strDataType, strValue, false))
		return true;

	//if(strType == "PORT")
	//{
	//	PortNumber((int) atoi(strValue.c_str()));
	//	return true;
	//}
	//else if(strType == "BAUDRATE")
	//{
	//	BaudRate((int) atoi(strValue.c_str()));
	//	return true;
	//}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RbLANWirelessInterface::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RobotInterface::QueryProperties(aryProperties);

	//aryNames.Add("ComPort");
	//aryTypes.Add("Integer");

	//aryNames.Add("BaudRate");
	//aryTypes.Add("Integer");
}

#pragma endregion


void RbLANWirelessInterface::Initialize()
{
	RobotInterface::Initialize();
}

void RbLANWirelessInterface::StepSimulation()
{
	RobotInterface::StepSimulation();
}

void RbLANWirelessInterface::Load(StdUtils::CStdXml &oXml)
{
	RobotInterface::Load(oXml);

}
		}		//RobotInterfaces
	}			// Robotics
}				//RoboticsAnimatSim

