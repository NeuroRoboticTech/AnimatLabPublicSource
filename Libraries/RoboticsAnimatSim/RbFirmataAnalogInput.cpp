// RbFirmataAnalogInput.cpp: implementation of the RbFirmataAnalogInput class.
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
#include "RbFirmataAnalogInput.h"
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

RbFirmataAnalogInput::RbFirmataAnalogInput() 
{
}

RbFirmataAnalogInput::~RbFirmataAnalogInput()
{
}

void RbFirmataAnalogInput::SetupIO()
{
	if(!m_lpParentInterface->InSimulation())
		m_lpFirmata->sendAnalogPinReporting(m_iIOComponentID, ARD_ANALOG);
}

void RbFirmataAnalogInput::StepIO()
{
}

void RbFirmataAnalogInput::StepSimulation()
{
    RobotPartInterface::StepSimulation();


}

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

