// BlLinearHillMuscle.cpp: implementation of the BlLinearHillMuscle class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlLine.h"
#include "BlLinearHillMuscle.h"
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

BlLinearHillMuscle::BlLinearHillMuscle()
{
	SetThisPointers();
}

BlLinearHillMuscle::~BlLinearHillMuscle()
{

	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlLinearHillMuscle\r\n", "", -1, false, true);}
}

void BlLinearHillMuscle::CreateParts()
{
	//We do nothing in createparts because we cannot build the line until after all parts are created
	//so we can get a handle to the attachment points.
}

void BlLinearHillMuscle::CreateJoints()
{
	LinearHillMuscle::CreateJoints();
	BlLine::CreateParts();
}

void BlLinearHillMuscle::ResetSimulation()
{
	LinearHillMuscle::ResetSimulation();
	BlLine::ResetSimulation();
}

void BlLinearHillMuscle::AfterResetSimulation()
{
	LinearHillMuscle::AfterResetSimulation();
	BlLine::AfterResetSimulation();
}

void BlLinearHillMuscle::StepSimulation()
{
	CalculateTension();

	BlLine::StepSimulation(m_fltTension); 
}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim

