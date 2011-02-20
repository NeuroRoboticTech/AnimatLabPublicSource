// VsLinearHillStretchReceptor.cpp: implementation of the VsLinearHillStretchReceptor class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsLine.h"
#include "VsLinearHillStretchReceptor.h"
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

VsLinearHillStretchReceptor::VsLinearHillStretchReceptor()
{
	m_lpThis = this;
	m_lpThisBody = this;
	m_lpPhysicsBody = this;
	m_lpLineBase = this;
}

VsLinearHillStretchReceptor::~VsLinearHillStretchReceptor()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsLinearHillStretchReceptor\r\n", "", -1, FALSE, TRUE);}
}


void VsLinearHillStretchReceptor::CreateParts(Simulator *lpSim, Structure *lpStructure)
{
	//We do nothing in createparts because we cannot build the line until after all parts are created
	//so we can get a handle to the attachment points.
}

void VsLinearHillStretchReceptor::CreateJoints(Simulator *lpSim, Structure *lpStructure)
{
	LinearHillStretchReceptor::CreateJoints(lpSim, lpStructure);
	VsLine::CreateParts(lpSim, lpStructure);

	m_fltIaRate = m_fltIaDischargeConstant*m_fltSeLength;
	m_fltIbRate = m_fltIbDischargeConstant*m_fltTension;
	m_fltIIRate = m_fltIIDischargeConstant*m_fltPeLength;
}

void VsLinearHillStretchReceptor::ResetSimulation(Simulator *lpSim, Structure *lpStructure)
{
	LinearHillStretchReceptor::ResetSimulation(lpSim, lpStructure);
	VsLine::ResetSimulation(lpSim, lpStructure);
}

void VsLinearHillStretchReceptor::AfterResetSimulation(Simulator *lpSim, Structure *lpStructure)
{
	LinearHillStretchReceptor::AfterResetSimulation(lpSim, lpStructure);
	VsLine::AfterResetSimulation(lpSim, lpStructure);
}

void VsLinearHillStretchReceptor::StepSimulation(Simulator *lpSim, Structure *lpStructure)
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

