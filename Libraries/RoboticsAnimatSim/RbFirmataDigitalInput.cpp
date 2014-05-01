// RbFirmataDigitalInput.cpp: implementation of the RbFirmataDigitalInput class.
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
#include "RbFirmataDigitalInput.h"

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace Firmata
			{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbFirmataDigitalInput::RbFirmataDigitalInput() 
{
    m_lpHinge = NULL;
}

RbFirmataDigitalInput::~RbFirmataDigitalInput()
{
	try
	{
        //Do not delete because we do not own it.
        m_lpHinge = NULL;
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbDynamixelCM5USBUARTHingeController\r\n", "", -1, false, true);}
}

void RbFirmataDigitalInput::ServoID(int iID)
{
	Std_IsAboveMin((int) 0, iID, true, "ServoID");
	//RbDynamixelUSBServo::ServoID(iID);
}

#pragma region DataAccesMethods

float *RbFirmataDigitalInput::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	//if(strType == "LIMITPOS")
	//	return &m_fltLimitPos;
	//else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Robot Interface ID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

bool RbFirmataDigitalInput::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(RobotPartInterface::SetData(strDataType, strValue, false))
		return true;

	if(strType == "SERVOID")
	{
		ServoID((int) atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RbFirmataDigitalInput::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RobotPartInterface::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("ServoID", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

#pragma endregion

void RbFirmataDigitalInput::Initialize()
{
	RobotPartInterface::Initialize();

	m_lpHinge = dynamic_cast<RbHinge *>(m_lpBodyPart);

}

void RbFirmataDigitalInput::StepSimulation()
{
    RobotPartInterface::StepSimulation();


}

void RbFirmataDigitalInput::Load(StdUtils::CStdXml &oXml)
{
	RobotPartInterface::Load(oXml);

	oXml.IntoElem();
	//ServoID(oXml.GetChildInt("ServoID", m_iServoID));
	oXml.OutOfElem();
}

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

