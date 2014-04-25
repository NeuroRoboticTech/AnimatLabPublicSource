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
    m_fltPos = 0;
    m_iCounter = 0;
    m_iSign = -1;
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

void RbDynamixelUSBHinge::StepSimulation()
{
    AnimatBase::StepSimulation();

    //Here we need to get the set velocity for this motor that is coming from the neural controller, and then make the real motor go that speed.
    float fltSetVelocity = m_lpHinge->SetVelocity();

    //Then we need to get the actual position and velocity of the joint and set it back on the joint part.
    //Also, do not forget to scale units if needed.

    // We will fake out some position movement here to show how it propogate to the neuron code.
    m_iCounter++;
    if(m_iCounter >= 250)
    {
        m_iSign *= -1;
        m_iCounter=0;
    }
    m_fltPos+= (0.01*m_iSign);

    float fltActualPosition = m_fltPos;
    float fltActualVelocity = 0;
    m_lpHinge->JointPosition(fltActualPosition);
    m_lpHinge->JointVelocity(fltActualVelocity);
}


			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

