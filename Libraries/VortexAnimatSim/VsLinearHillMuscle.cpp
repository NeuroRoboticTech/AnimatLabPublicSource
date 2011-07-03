// VsLinearHillMuscle.cpp: implementation of the VsLinearHillMuscle class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsLine.h"
#include "VsLinearHillMuscle.h"
#include "VsSimulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsLinearHillMuscle::VsLinearHillMuscle()
{
	SetThisPointers();
}

VsLinearHillMuscle::~VsLinearHillMuscle()
{

	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsLinearHillMuscle\r\n", "", -1, FALSE, TRUE);}
}

void VsLinearHillMuscle::CreateParts()
{
	//We do nothing in createparts because we cannot build the line until after all parts are created
	//so we can get a handle to the attachment points.
}

void VsLinearHillMuscle::CreateJoints()
{
	LinearHillMuscle::CreateJoints();
	VsLine::CreateParts();
}

void VsLinearHillMuscle::ResetSimulation()
{
	LinearHillMuscle::ResetSimulation();
	VsLine::ResetSimulation();
}

void VsLinearHillMuscle::AfterResetSimulation()
{
	LinearHillMuscle::AfterResetSimulation();
	VsLine::AfterResetSimulation();
}

void VsLinearHillMuscle::StepSimulation()
{
	CalculateTension();

	VsLine::StepSimulation(m_fltTension); 
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

