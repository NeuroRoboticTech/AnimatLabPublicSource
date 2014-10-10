// RbFirmataAnalogOutput.cpp: implementation of the RbFirmataAnalogOutput class.
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
#include "RbFirmataAnalogOutput.h"
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

RbFirmataAnalogOutput::RbFirmataAnalogOutput() 
{
}

RbFirmataAnalogOutput::~RbFirmataAnalogOutput()
{
}

void RbFirmataAnalogOutput::SetupIO()
{

}

void RbFirmataAnalogOutput::StepIO(int iPartIdx)
{
}

void RbFirmataAnalogOutput::StepSimulation()
{
    RobotPartInterface::StepSimulation();


}

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

