// BlSpring.cpp: implementation of the BlSpring class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlLine.h"
#include "BlSpring.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BlSpring::BlSpring()
{
	SetThisPointers();
}

BlSpring::~BlSpring()
{
	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlSpring/\r\n", "", -1, false, true);}
}

void BlSpring::CreateJoints()
{
	Spring::CreateJoints();
	BlLine::CreateParts();

	SetupPhysics();
}

void BlSpring::ResetSimulation()
{
	Spring::ResetSimulation();
	BlLine::ResetSimulation();
}

void BlSpring::AfterResetSimulation()
{
	Spring::AfterResetSimulation();
	BlLine::AfterResetSimulation();
}

void BlSpring::StepSimulation()
{
	CalculateTension();
	BlLine::StepSimulation(m_fltTension); 
}


		}		//Joints
	}			// Environment
}				//BulletAnimatSim
