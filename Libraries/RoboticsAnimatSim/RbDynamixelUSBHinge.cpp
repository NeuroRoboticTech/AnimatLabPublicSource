// RbDynamixelUSBHinge.cpp: implementation of the RbDynamixelUSBHinge class.
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
#include "RbDynamixelUSBHinge.h"

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

RbDynamixelUSBHinge::RbDynamixelUSBHinge() 
{
    m_lpHinge = NULL;
}

RbDynamixelUSBHinge::~RbDynamixelUSBHinge()
{
	try
	{
        //Do not delete because we do not own it.
        m_lpHinge = NULL;
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbDynamixelCM5USBUARTHingeController\r\n", "", -1, false, true);}
}

void RbDynamixelUSBHinge::IOComponentID(int iID)
{
	Std_IsAboveMin((int) 0, iID, true, "ServoID");
	RobotPartInterface::IOComponentID(iID);
	RbDynamixelUSBServo::ServoID(iID);
}

#pragma region DataAccesMethods

float *RbDynamixelUSBHinge::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	//if(strType == "LIMITPOS")
	//	return &m_fltLimitPos;
	//else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Robot Interface ID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

bool RbDynamixelUSBHinge::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
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

void RbDynamixelUSBHinge::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RobotPartInterface::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("ServoID", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

#pragma endregion

void RbDynamixelUSBHinge::Initialize()
{
	RobotPartInterface::Initialize();

	m_lpHinge = dynamic_cast<RbHinge *>(m_lpPart);

	if(!m_lpParentInterface->InSimulation())
		InitMotorData();
}

void RbDynamixelUSBHinge::StepSimulation()
{
	if(!m_lpParentInterface->InSimulation())
	{
		RobotPartInterface::StepSimulation();

		if(m_lpHinge)
		{
			//Here we need to get the set velocity for this motor that is coming from the neural controller, and then make the real motor go that speed.
			if(!m_lpHinge->ServoMotor())
			{
				float fltSetVelocity = m_lpHinge->SetVelocity();
				SetGoalVelocity(fltSetVelocity);

				if(fltSetVelocity > 0)
					SetGoalPosition_FP(m_iMaxPos);
				else
					SetGoalPosition_FP(m_iMinPos);
			}
			else
			{
				float fltSetPosition = m_lpHinge->SetVelocity();
				SetGoalPosition(fltSetPosition);
				SetMaximumVelocity();
			}

			float fltActualPosition = GetActualPosition();
			float fltActualVelocity = GetActualVelocity();
			//float fltTemperature = GetActualTemperatureCelcius();
			//float fltVoltage = GetActualVoltage();
			//float fltLoad = GetActualLoad();

			m_lpHinge->JointPosition(fltActualPosition);
			m_lpHinge->JointVelocity(fltActualVelocity);
			//m_lpHinge->Temperature(fltTemperature);
			//m_lpHinge->Voltage(fltVoltage);
			//m_lpHinge->MotorTorqueToAMagnitude(fltLoad);
			//m_lpHinge->MotorTorqueToBMagnitude(fltLoad);
		}
	}
}

void RbDynamixelUSBHinge::Load(StdUtils::CStdXml &oXml)
{
	RobotPartInterface::Load(oXml);

	oXml.IntoElem();
	ServoID(oXml.GetChildInt("ServoID", m_iServoID));
	oXml.OutOfElem();
}

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

