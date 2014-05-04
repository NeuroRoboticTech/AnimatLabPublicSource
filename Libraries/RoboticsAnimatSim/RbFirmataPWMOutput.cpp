// RbFirmataPWMOutput.cpp: implementation of the RbFirmataPWMOutput class.
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
#include "RbFirmataPWMOutput.h"

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

RbFirmataPWMOutput::RbFirmataPWMOutput() 
{
}

RbFirmataPWMOutput::~RbFirmataPWMOutput()
{
}

void RbFirmataPWMOutput::SetupIO()
{
}

void RbFirmataPWMOutput::StepIO()
{
}

void RbFirmataPWMOutput::StepSimulation()
{
    RobotPartInterface::StepSimulation();


}

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

