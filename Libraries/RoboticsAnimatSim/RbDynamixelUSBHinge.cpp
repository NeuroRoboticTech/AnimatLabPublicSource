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
	m_iUpdateQueueIndex = -1;
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

int RbDynamixelUSBHinge::UpdateQueueIndex() {return m_iUpdateQueueIndex;}

void RbDynamixelUSBHinge::UpdateQueueIndex(int iVal)
{
	Std_IsAboveMin((int) -1, iVal, true, "UpdateQueueIndex", true);
	m_iUpdateQueueIndex = iVal;
}

#pragma region DataAccesMethods

float *RbDynamixelUSBHinge::GetDataPointer(const std::string &strDataType)
{
	float *fltVal = RbDynamixelUSBServo::GetDataPointer(strDataType);
	if(fltVal)
		return fltVal;
	else
		return RobotPartInterface::GetDataPointer(strDataType);
}

bool RbDynamixelUSBHinge::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(RobotPartInterface::SetData(strDataType, strValue, false))
		return true;

	if(strType == "UPDATEALLPARAMSCOUNT")
	{
		UpdateAllParamsCount((int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "UPDATEQUEUEINDEX")
	{
		UpdateQueueIndex((int) atoi(strValue.c_str()));
		return true;
	}

	if(RbDynamixelUSBServo::SetData(strDataType, strValue))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RbDynamixelUSBHinge::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RobotPartInterface::QueryProperties(aryProperties);
	RbDynamixelUSBServo::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("UpdateAllParamsCount", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("UpdateQueueIndex", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

#pragma endregion

void RbDynamixelUSBHinge::MicroSleep(unsigned int iTime)
{
	if(m_lpSim)
	m_lpSim->MicroSleep(iTime);
}

Simulator *RbDynamixelUSBHinge::GetSimulator()
{
	return m_lpSim;
}

float RbDynamixelUSBHinge::QuantizeServoPosition(float fltPos)
{
	return RbDynamixelUSBServo::QuantizeServoPosition(fltPos);
}

float RbDynamixelUSBHinge::QuantizeServoVelocity(float fltVel)
{
	return RbDynamixelUSBServo::QuantizeServoVelocity(fltVel);
}

bool RbDynamixelUSBHinge::IncludeInPartsCycle() {return m_bQueryMotorData;}

void RbDynamixelUSBHinge::Initialize()
{
	RobotPartInterface::Initialize();

	m_lpHinge = dynamic_cast<Hinge *>(m_lpPart);
	m_lpMotorJoint = dynamic_cast<MotorizedJoint *>(m_lpPart);
	m_lpParentUSB = dynamic_cast<RbDynamixelUSB *>(m_lpParentIOControl);

	RecalculateParams();
}

void RbDynamixelUSBHinge::SetupIO()
{
	if(!m_lpSim->InSimulation() && m_lpHinge)
	{
		SetMinSimPos(m_lpHinge->LowerLimit()->LimitPos());
		SetMaxSimPos(m_lpHinge->UpperLimit()->LimitPos());
		InitMotorData();

		//Set the next goal positions to the current ones.
		m_iNextGoalPos = m_iLastGoalPos;

		m_iNextGoalVelocity = m_iLastGoalVelocity;
	}
}

void RbDynamixelUSBHinge::AddMotorUpdate(int iServoID, int iPos, int iSpeed)
{
	if(m_lpParentUSB)
	{
		m_lpParentUSB->m_aryMotorData.Add(new RbDynamixelMotorUpdateData(iServoID, iPos, iSpeed));
		m_fltIOValue = iSpeed;
	}
}

void RbDynamixelUSBHinge::StepIO(int iPartIdx)
{	
	unsigned long long lStepStartTick = m_lpSim->GetTimerTick();

	if(!m_lpSim->InSimulation())
	{
		if(m_bQueryMotorData && (m_iUpdateQueueIndex == iPartIdx || m_iUpdateQueueIndex == -1))
		{
			if(m_iUpdateIdx == m_iUpdateAllParamsCount)
				ReadAllParams();
			else
				ReadKeyParams();
		}

	}

	SetMotorPosVel();

	unsigned long long lEndStartTick = m_lpSim->GetTimerTick();
	m_fltStepIODuration = m_lpSim->TimerDiff_m(lStepStartTick, lEndStartTick); 

	m_iUpdateIdx++;
}

void RbDynamixelUSBHinge::ShutdownIO()
{
	ShutdownMotor();
}

void RbDynamixelUSBHinge::StepSimulation()
{
	if(!m_lpSim->InSimulation())
	{
		RobotPartInterface::StepSimulation();
		RbDynamixelUSBServo::StepSimulation();
	}
}

void RbDynamixelUSBHinge::ResetSimulation()
{
	AnimatSim::Robotics::RobotPartInterface::ResetSimulation();
	m_fltReadParamTime = 0;
}

void RbDynamixelUSBHinge::Load(StdUtils::CStdXml &oXml)
{
	RobotPartInterface::Load(oXml);
	RbDynamixelServo::Load(oXml);

	oXml.IntoElem();
	UpdateAllParamsCount(oXml.GetChildInt("UpdateAllParamsCount", m_iUpdateAllParamsCount));
	UpdateQueueIndex(oXml.GetChildInt("UpdateQueueIndex", m_iUpdateQueueIndex));
	oXml.OutOfElem();
}

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

