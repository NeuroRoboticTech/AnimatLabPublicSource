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
#include "RbFirmataPart.h"
#include "RbFirmataDigitalInput.h"
#include "RbFirmataController.h"

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
}

RbFirmataDigitalInput::~RbFirmataDigitalInput()
{
}

void RbFirmataDigitalInput::SetupIO()
{
	if(!m_lpParentInterface->InSimulation())
		m_lpFirmata->sendDigitalPinMode(m_iIOComponentID, ARD_INPUT);
}

void RbFirmataDigitalInput::StepIO()
{
}

void RbFirmataDigitalInput::StepSimulation()
{
    RobotPartInterface::StepSimulation();


}

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

