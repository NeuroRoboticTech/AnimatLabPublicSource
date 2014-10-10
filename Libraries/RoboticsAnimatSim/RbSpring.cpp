// RbSpring.cpp: implementation of the RbSpring class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbLine.h"
#include "RbSpring.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbSpring::RbSpring()
{
	SetThisPointers();
}

RbSpring::~RbSpring()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbSpring/\r\n", "", -1, false, true);}
}

void RbSpring::CreateJoints()
{
	Spring::CreateJoints();
	RbLine::CreateParts();
}

void RbSpring::ResetSimulation()
{
	Spring::ResetSimulation();
	RbLine::ResetSimulation();
}

void RbSpring::AfterResetSimulation()
{
	Spring::AfterResetSimulation();
	RbLine::AfterResetSimulation();
}

void RbSpring::StepSimulation()
{
	CalculateTension();
	RbLine::StepSimulation(m_fltTension); 
}


		}		//Joints
	}			// Environment
}				//RoboticsAnimatSim
