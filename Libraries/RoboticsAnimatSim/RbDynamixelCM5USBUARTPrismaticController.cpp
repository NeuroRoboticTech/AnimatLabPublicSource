// RbDynamixelCM5USBUARTPrismaticController.cpp: implementation of the RbDynamixelCM5USBUARTPrismaticController class.
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
#include "RbDynamixelCM5USBUARTPrismaticController.h"

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace MotorControlSystems
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbDynamixelCM5USBUARTPrismaticController::RbDynamixelCM5USBUARTPrismaticController() 
{
    m_lpPrismatic = NULL;
}

RbDynamixelCM5USBUARTPrismaticController::~RbDynamixelCM5USBUARTPrismaticController()
{
	try
	{
        //Do not delete because we do not own it.
        m_lpPrismatic = NULL;
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbDynamixelCM5USBUARTPrismaticController\r\n", "", -1, false, true);}
}

void RbDynamixelCM5USBUARTPrismaticController::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
    AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, bVerify);

    m_lpPrismatic = dynamic_cast<RbPrismatic *>(lpNode);
	if(!m_lpPrismatic) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "RbPrismatic: ", m_strID);
}

void RbDynamixelCM5USBUARTPrismaticController::StepSimulation()
{
    AnimatBase::StepSimulation();

    //Here we need to get the set velocity for this motor that is coming from the neural controller, and then make the real motor go that speed.
    float fltSetVelocity = m_lpPrismatic->SetVelocity();

    //Then we need to get the actual position and velocity of the joint and set it back on the joint part.
    //Also, do not forget to scale units if needed.
    float fltActualPosition = 0;
    float fltActualVelocity = 0;
    m_lpPrismatic->JointPosition(fltActualPosition);
    m_lpPrismatic->JointVelocity(fltActualVelocity);
}

		}		//MotorControlSystems
	}			// Robotics
}				//RoboticsAnimatSim

