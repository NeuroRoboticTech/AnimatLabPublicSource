// RbDynamixelCM5USBUARTHingeController.cpp: implementation of the RbDynamixelCM5USBUARTHingeController class.
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
#include "RbDynamixelCM5USBUARTHingeController.h"

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace MotorControlSystems
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbDynamixelCM5USBUARTHingeController::RbDynamixelCM5USBUARTHingeController() 
{
    m_lpHinge = NULL;
}

RbDynamixelCM5USBUARTHingeController::~RbDynamixelCM5USBUARTHingeController()
{
	try
	{
        //Do not delete because we do not own it.
        m_lpHinge = NULL;
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbDynamixelCM5USBUARTHingeController\r\n", "", -1, false, true);}
}

void RbDynamixelCM5USBUARTHingeController::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
    AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, bVerify);

    m_lpHinge = dynamic_cast<RbHinge *>(lpNode);
	if(!m_lpHinge) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "RbHinge: ", m_strID);
}

void RbDynamixelCM5USBUARTHingeController::StepSimulation()
{
    AnimatBase::StepSimulation();

    //Here we need to get the set velocity for this motor that is coming from the neural controller, and then make the real motor go that speed.
    float fltSetVelocity = m_lpHinge->SetVelocity();

    //Then we need to get the actual position and velocity of the joint and set it back on the joint part.
    //Also, do not forget to scale units if needed.
    float fltActualPosition = 0;
    float fltActualVelocity = 0;
    m_lpHinge->JointPosition(fltActualPosition);
    m_lpHinge->JointVelocity(fltActualVelocity);
}


		}		//MotorControlSystems
	}			// Robotics
}				//RoboticsAnimatSim

