// RbDynamixelUSBPrismatic.cpp: implementation of the RbDynamixelUSBPrismatic class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbPrismaticLimit.h"
#include "RbPrismatic.h"
#include "RbRigidBody.h"
#include "RbStructure.h"
#include "RbDynamixelUSBServo.h"
#include "RbDynamixelUSBPrismatic.h"

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

RbDynamixelUSBPrismatic::RbDynamixelUSBPrismatic() 
{
    m_lpPrismatic = NULL;
}

RbDynamixelUSBPrismatic::~RbDynamixelUSBPrismatic()
{
	try
	{
        //Do not delete because we do not own it.
        m_lpPrismatic = NULL;
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbDynamixelCM5USBUARTPrismaticController\r\n", "", -1, false, true);}
}

void RbDynamixelUSBPrismatic::MicroSleep(unsigned int iTime)
{
	if(m_lpSim)
	m_lpSim->MicroSleep(iTime);
}

void RbDynamixelUSBPrismatic::StepSimulation()
{
	if(!m_lpSim->InSimulation())
	{
		RobotPartInterface::StepSimulation();

		//if(m_lpPrismatic)
		//{
		//	//Here we need to get the set velocity for this motor that is coming from the neural controller, and then make the real motor go that speed.
		//	float fltSetVelocity = m_lpPrismatic->SetVelocity();

		//	//Then we need to get the actual position and velocity of the joint and set it back on the joint part.
		//	//Also, do not forget to scale units if needed.
		//	float fltActualPosition = 0;
		//	float fltActualVelocity = 0;
		//	m_lpPrismatic->JointPosition(fltActualPosition);
		//	m_lpPrismatic->JointVelocity(fltActualVelocity);
		//}
	}
}

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

