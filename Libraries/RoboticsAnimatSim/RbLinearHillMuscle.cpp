// RbLinearHillMuscle.cpp: implementation of the RbLinearHillMuscle class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbLine.h"
#include "RbLinearHillMuscle.h"
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

RbLinearHillMuscle::RbLinearHillMuscle()
{
	SetThisPointers();
}

RbLinearHillMuscle::~RbLinearHillMuscle()
{

	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbLinearHillMuscle\r\n", "", -1, false, true);}
}

void RbLinearHillMuscle::CreateParts()
{
	//We do nothing in createparts because we cannot build the line until after all parts are created
	//so we can get a handle to the attachment points.
}

void RbLinearHillMuscle::CreateJoints()
{
	LinearHillMuscle::CreateJoints();
	RbLine::CreateParts();
}

void RbLinearHillMuscle::ResetSimulation()
{
	LinearHillMuscle::ResetSimulation();
	RbLine::ResetSimulation();
}

void RbLinearHillMuscle::AfterResetSimulation()
{
	LinearHillMuscle::AfterResetSimulation();
	RbLine::AfterResetSimulation();
}

void RbLinearHillMuscle::StepSimulation()
{
	CalculateTension();

	RbLine::StepSimulation(m_fltTension); 
}

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim

