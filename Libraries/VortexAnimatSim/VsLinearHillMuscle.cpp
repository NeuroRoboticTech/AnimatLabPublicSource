// VsLinearHillMuscle.cpp: implementation of the VsLinearHillMuscle class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
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
	m_lpThis = this;
	m_lpThisBody = this;
	m_lpPhysicsBody = this;
	m_lpLineBase = this;
}

VsLinearHillMuscle::~VsLinearHillMuscle()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsLinearHillMuscle\r\n", "", -1, FALSE, TRUE);}
}

void VsLinearHillMuscle::CreateParts(Simulator *lpSim, Structure *lpStructure)
{
	//We do nothing in createparts because we cannot build the line until after all parts are created
	//so we can get a handle to the attachment points.
}

void VsLinearHillMuscle::CreateJoints(Simulator *lpSim, Structure *lpStructure)
{
	LinearHillMuscle::CreateJoints(lpSim, lpStructure);
	VsLine::CreateParts(lpSim, lpStructure);
}

void VsLinearHillMuscle::ResetSimulation(Simulator *lpSim, Structure *lpStructure)
{
	LinearHillMuscle::ResetSimulation(lpSim, lpStructure);
	VsLine::ResetSimulation(lpSim, lpStructure);
}

void VsLinearHillMuscle::AfterResetSimulation(Simulator *lpSim, Structure *lpStructure)
{
	LinearHillMuscle::AfterResetSimulation(lpSim, lpStructure);
	VsLine::AfterResetSimulation(lpSim, lpStructure);
}

void VsLinearHillMuscle::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	if(m_bEnabled)
	{
		CalculateTension(lpSim);
		VsLine::StepSimulation(lpSim, lpStructure, m_fltTension); 
	}
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

