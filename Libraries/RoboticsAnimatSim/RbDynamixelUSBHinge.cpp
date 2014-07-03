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
	m_bQueryMotorData = true;
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

	if(strType == "UPDATEQUEUEINDEX")
	{
		UpdateQueueIndex((int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "QUERYMOTORDATA")
	{
		QueryMotorData(Std_ToBool(strValue));
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
	aryProperties.Add(new TypeProperty("UpdateQueueIndex", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("QueryMotorData", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
}

#pragma endregion

void RbDynamixelUSBHinge::MicroSleep(unsigned int iTime)
{
	if(m_lpSim)
	m_lpSim->MicroSleep(iTime);
}

float RbDynamixelUSBHinge::QuantizeServoPosition(float fltPos)
{
	int iPos = ConvertPosRadToFP(fltPos);
	return ConvertPosFPToRad(iPos);
}

float RbDynamixelUSBHinge::QuantizeServoVelocity(float fltVel)
{
	int iPos = (int) (fabs(fltVel)*m_fltConvertRadSToFP);
	return iPos*m_fltConvertFPToRadS;
}

void RbDynamixelUSBHinge::QueryMotorData(bool bVal) {m_bQueryMotorData = bVal;}

bool RbDynamixelUSBHinge::QueryMotorData() {return m_bQueryMotorData;}

bool RbDynamixelUSBHinge::IncludeInPartsCycle() {return m_bQueryMotorData;}

void RbDynamixelUSBHinge::Initialize()
{
	RobotPartInterface::Initialize();

	m_lpHinge = dynamic_cast<Hinge *>(m_lpPart);
	m_lpParentUSB = dynamic_cast<RbDynamixelUSB *>(m_lpParentIOControl);
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

void RbDynamixelUSBHinge::StepIO(int iPartIdx)
{	
	static int iTickCount=0;
	static double dblTotalTime=0;
	unsigned long long lStepStartTick = m_lpSim->GetTimerTick();

	if(!m_lpSim->InSimulation())
	{
		if(m_bQueryMotorData && (m_iUpdateQueueIndex == iPartIdx || m_iUpdateQueueIndex == -1))
		{
		//if(m_iUpdateIdx == m_iUpdateAllParamsCount)
		//{
		//	unsigned long long lStart = m_lpSim->GetTimerTick(), lEnd;
		//	ReadAllParams();
		//	lEnd = m_lpSim->GetTimerTick();
		//	double dblTime = m_lpSim->TimerDiff_m(lStart, lEnd);
		//	dblTotalTime += dblTime;
		//	iTickCount++;
		//	if(iTickCount == 20)
		//	{
		//		double dblAvg = dblTotalTime/20.0;
		//		std::cout << m_lpSim->Time() << " Avg RadTime: " << dblAvg << "\r\n";
		//		iTickCount=0;
		//		dblTotalTime = 0;
		//	}
		//}
		//else
		//{
			unsigned long long lStart = m_lpSim->GetTimerTick(), lEnd;
			//ReadAllParams();
			ReadKeyParams();
			lEnd = m_lpSim->GetTimerTick();
			double dblTime = m_lpSim->TimerDiff_m(lStart, lEnd);
			dblTotalTime += dblTime;
			iTickCount++;
			if(iTickCount == 20)
			{
				double dblAvg = dblTotalTime/20.0;
				std::cout << m_lpSim->Time() << " Avg RadTime: " << dblAvg << "\r\n";
				iTickCount=0;
				dblTotalTime = 0;
			}
		//}
		}

		//std::cout << m_lpSim->Time() << ", servo: " << m_iServoID <<  ", Pos: " << m_iNextGoalPos << ", LasPos: " << m_iLastGoalPos << ", Vel: " << m_iNextGoalVelocity << ", LastVel: " << m_iLastGoalVelocity << "\r\n";
		if(m_iNextGoalPos != m_iLastGoalPos ||  ( (m_iNextGoalVelocity != m_iLastGoalVelocity) && !(m_iNextGoalVelocity == -1 && m_iLastGoalVelocity == 1))  )
		{
			//std::cout << m_lpSim->Time() << ", servo: " << m_iServoID <<  ", Pos: " << m_iNextGoalPos << ", LasPos: " << m_iLastGoalPos << ", Vel: " << m_iNextGoalVelocity << ", LastVel: " << m_iLastGoalVelocity << "\r\n";
			//std::cout << "************" << m_lpSim->Time() << ", servo: " << m_iServoID <<  ", Pos: " << m_iNextGoalPos << ", Vel: " << m_iNextGoalVelocity << "\r\n";

			//If the next goal velocity was set to -1 then we are trying to set velocity to 0. So lets set the goal position to its current
			//loctation and velocity to lowest value.
			if(m_iNextGoalVelocity == -1)
			{
				m_iNextGoalPos = m_iPresentPos;
				m_iNextGoalVelocity = 1;
			}

			//Add a new update data so we can send the move command out synchronously to all motors.
			m_lpParentUSB->m_aryMotorData.Add(new RbDynamixelUSBMotorUpdateData(m_iServoID, m_iNextGoalPos, m_iNextGoalVelocity));
			m_iLastGoalPos = m_iNextGoalPos;
			m_iLastGoalVelocity = m_iNextGoalVelocity;

			m_fltIOValue = m_iNextGoalVelocity;
		}
	}

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

		////Test code
		//int i=5;
		//if(Std_ToLower(m_strID) == "600488ae-f6ce-44c9-bc01-c403d8b236de") // && m_lpSim->Time() > 1 
		//	i=6;

		if(m_lpHinge)
		{
			//Here we need to get the set velocity for this motor that is coming from the neural controller, and then make the real motor go that speed.
			//Here we are setting the values that will be used the next time the IO is processed for this servo.
			if(m_lpHinge->MotorType() == eJointMotorType::PositionVelocityControl)
			{
				float fltSetPosition = m_lpHinge->SetPosition();
				float fltSetVelocity = m_lpHinge->SetVelocity();
				SetNextGoalPosition(fltSetPosition);
				SetNextGoalVelocity(fltSetVelocity);
			}
			else if(m_lpHinge->MotorType() == eJointMotorType::PositionControl)
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
	UpdateQueueIndex(oXml.GetChildInt("UpdateQueueIndex", m_iUpdateQueueIndex));
	QueryMotorData(oXml.GetChildBool("QueryMotorData", m_bQueryMotorData));
	oXml.OutOfElem();
}

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

