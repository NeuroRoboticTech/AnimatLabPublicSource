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
#include "RbDynamixelUSB.h"
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
	m_iUpdateAllParamsCount = 10;
	m_iUpdateIdx = 0;
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

void RbDynamixelUSBHinge::UpdateAllParamsCount(int iVal)
{
	Std_IsAboveMin((int) 0, iVal, true, "UpdateAllParamsCount");
	m_iUpdateAllParamsCount = iVal;
}

int RbDynamixelUSBHinge::UpdateAllParamsCount() {return m_iUpdateAllParamsCount;}

#pragma region DataAccesMethods

float *RbDynamixelUSBHinge::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	//if(strType == "LIMITPOS")
	//	return &m_fltLimitPos;
	//else
	return RobotPartInterface::GetDataPointer(strDataType);
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

	if(strType == "UPDATEALLPARAMSCOUNT")
	{
		UpdateAllParamsCount((int) atoi(strValue.c_str()));
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
	aryProperties.Add(new TypeProperty("UpdateAllParamsCount", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

#pragma endregion

void RbDynamixelUSBHinge::Initialize()
{
	RobotPartInterface::Initialize();

	m_lpHinge = dynamic_cast<Hinge *>(m_lpPart);
	m_lpParentUSB = dynamic_cast<RbDynamixelUSB *>(m_lpParentIOControl);
}

void RbDynamixelUSBHinge::SetupIO()
{
	if(!m_lpParentInterface->InSimulation())
	{
		SetMinSimPos(m_lpHinge->LowerLimit()->LimitPos());
		SetMaxSimPos(m_lpHinge->UpperLimit()->LimitPos());
		InitMotorData();

		//Set the next goal positions to the current ones.
		m_iNextGoalPos = m_iLastGoalPos;
		m_iNextGoalVelocity = m_iLastGoalVelocity;
	}
}

void RbDynamixelUSBHinge::StepIO()
{	
	unsigned long long lStepStartTick = m_lpSim->GetTimerTick();

	if(!m_lpParentInterface->InSimulation())
	{
		if(m_iNextGoalPos != m_iLastGoalPos ||  m_iNextGoalVelocity != m_iLastGoalVelocity)
		{
			std::cout << m_lpSim->Time() <<  ", Pos: " << m_iNextGoalPos << ", Vel: " << m_iNextGoalVelocity << "\r\n";

			//Add a new update data so we can send the move command out synchronously to all motors.
			m_lpParentUSB->m_aryMotorData.Add(new RbDynamixelUSBMotorUpdateData(m_iServoID, m_iNextGoalPos, m_iNextGoalVelocity));
			m_iLastGoalPos = m_iNextGoalPos;
			m_iLastGoalVelocity = m_iNextGoalVelocity;

			m_fltIOValue = m_iNextGoalVelocity;
		}

		if(m_iUpdateIdx == m_iUpdateAllParamsCount)
			ReadAllParams();
		else
			ReadKeyParams();
	}

	unsigned long long lEndStartTick = m_lpSim->GetTimerTick();
	m_fltStepIODuration = m_lpSim->TimerDiff_m(lStepStartTick, lEndStartTick); 

	m_iUpdateIdx++;
}

void RbDynamixelUSBHinge::StepSimulation()
{
	if(!m_lpParentInterface->InSimulation())
	{
		RobotPartInterface::StepSimulation();

		if(m_lpHinge)
		{
			//Here we need to get the set velocity for this motor that is coming from the neural controller, and then make the real motor go that speed.
			//Here we are setting the values that will be used the next time the IO is processed for this servo.
			if(!m_lpHinge->ServoMotor())
			{
				float fltSetVelocity = m_lpHinge->SetVelocity();
				SetNextGoalVelocity(fltSetVelocity);
				//m_fltIOValue = m_iNextGoalVelocity;

				if(fltSetVelocity == 0)
					SetNextGoalPosition_FP(m_iLastGoalPos);
				else if(fltSetVelocity > 0)
					SetNextGoalPosition_FP(m_iMaxPos);
				else
					SetNextGoalPosition_FP(m_iMinPos);
			}
			else
			{
				float fltSetPosition = m_lpHinge->SetVelocity();
				SetNextGoalPosition(fltSetPosition);
				SetNextMaximumVelocity();

				//m_fltIOValue = m_iNextGoalPos;
			}

			//Retrieve the values that we got from the last time the IO for this servo was read in.
			m_lpHinge->JointPosition(m_fltPresentPos);
			m_lpHinge->JointVelocity(m_fltPresentVelocity);
			m_lpHinge->Temperature(m_fltVoltage);
			m_lpHinge->Voltage(m_fltTemperature);
			m_lpHinge->MotorTorqueToAMagnitude(m_fltLoad);
			m_lpHinge->MotorTorqueToBMagnitude(m_fltLoad);
		}
	}
}

void RbDynamixelUSBHinge::Load(StdUtils::CStdXml &oXml)
{
	RobotPartInterface::Load(oXml);

	oXml.IntoElem();
	UpdateAllParamsCount(oXml.GetChildInt("UpdateAllParamsCount", m_iUpdateAllParamsCount));
	oXml.OutOfElem();
}

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

