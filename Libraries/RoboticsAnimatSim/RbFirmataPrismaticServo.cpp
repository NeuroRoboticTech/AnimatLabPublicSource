// RbFirmataPrismaticServo.cpp: implementation of the RbFirmataPrismaticServo class.
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
#include "RbFirmataPart.h"
#include "RbFirmataPrismaticServo.h"

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

RbFirmataPrismaticServo::RbFirmataPrismaticServo() 
{
    m_lpHinge = NULL;
}

RbFirmataPrismaticServo::~RbFirmataPrismaticServo()
{
	try
	{
        //Do not delete because we do not own it.
        m_lpHinge = NULL;
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbDynamixelCM5USBUARTHingeController\r\n", "", -1, false, true);}
}

void RbFirmataPrismaticServo::SetupIO()
{
}

void RbFirmataPrismaticServo::StepIO()
{
}

void RbFirmataPrismaticServo::Initialize()
{
	RobotPartInterface::Initialize();

	m_lpHinge = dynamic_cast<RbHinge *>(m_lpPart);

	//if(!m_lpSim->InSimulation())
	//	InitMotorData();
}

void RbFirmataPrismaticServo::StepSimulation()
{
    RobotPartInterface::StepSimulation();

	//if(m_lpHinge)
	//{
	//	//Here we need to get the set velocity for this motor that is coming from the neural controller, and then make the real motor go that speed.
	//	if(m_lpThisMotorJoint->MotorType() == eJointMotorType::PositionControl || m_lpThisMotorJoint->MotorType() == eJointMotorType::PositionVelocityControl)
	//	{
	//		float fltSetVelocity = m_lpHinge->SetVelocity();
	//		SetGoalVelocity(fltSetVelocity);

	//		if(fltSetVelocity > 0)
	//			SetGoalPosition_FP(m_iMaxPos);
	//		else
	//			SetGoalPosition_FP(m_iMinPos);
	//	}
	//	else
	//	{
	//		float fltSetPosition = m_lpHinge->SetVelocity();
	//		SetGoalPosition(fltSetPosition);
	//		SetMaximumVelocity();
	//	}

	//	float fltActualPosition = GetActualPosition();
	//	float fltActualVelocity = GetActualVelocity();
	//	//float fltTemperature = GetActualTemperatureCelcius();
	//	//float fltVoltage = GetActualVoltage();
	//	//float fltLoad = GetActualLoad();

	//	m_lpHinge->JointPosition(fltActualPosition);
	//	m_lpHinge->JointVelocity(fltActualVelocity);
	//	//m_lpHinge->Temperature(fltTemperature);
	//	//m_lpHinge->Voltage(fltVoltage);
	//	//m_lpHinge->MotorTorqueToAMagnitude(fltLoad);
	//	//m_lpHinge->MotorTorqueToBMagnitude(fltLoad);
	//}
}

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

