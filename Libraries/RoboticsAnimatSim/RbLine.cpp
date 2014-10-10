// RbLine.cpp: implementation of the RbLine class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbLine.h"
#include "RbBox.h"
#include "RbSimulator.h"


namespace RoboticsAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbLine::RbLine()
{
}

RbLine::~RbLine()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of RbLine\r\n", "", -1, false, true);}
}

void RbLine::SetThisPointers()
{
	RbRigidBody::SetThisPointers();
}

void RbLine::CreateParts()
{
	RbRigidBody::CreateItem();
}

void RbLine::StepSimulation(float fltTension)
{
}

void RbLine::ResetSimulation()
{
	//We do nothing in the reset simulation because we need the attachment points to be reset before we can do anything.
}

void RbLine::AfterResetSimulation()
{
}


	}			// Visualization
}				//RoboticsAnimatSim
