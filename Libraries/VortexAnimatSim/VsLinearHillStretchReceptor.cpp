// VsLinearHillStretchReceptor.cpp: implementation of the VsLinearHillStretchReceptor class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
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
	SetThisPointers();
}

VsLinearHillStretchReceptor::~VsLinearHillStretchReceptor()
{

	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsLinearHillStretchReceptor\r\n", "", -1, false, true);}
}

void VsLinearHillStretchReceptor::CreateParts()
{
	//We do nothing in createparts because we cannot build the line until after all parts are created
	//so we can get a handle to the attachment points.
}

void VsLinearHillStretchReceptor::CreateJoints()
{
	LinearHillStretchReceptor::CreateJoints();
	VsLine::CreateParts();

	m_fltIaRate = m_fltIaDischargeConstant*m_fltSeLength;
	m_fltIbRate = m_fltIbDischargeConstant*m_fltTension;
	m_fltIIRate = m_fltIIDischargeConstant*m_fltPeLength;
}

void VsLinearHillStretchReceptor::ResetSimulation()
{
	LinearHillStretchReceptor::ResetSimulation();
	VsLine::ResetSimulation();
}

void VsLinearHillStretchReceptor::AfterResetSimulation()
{
	LinearHillStretchReceptor::AfterResetSimulation();
	VsLine::AfterResetSimulation();
}

void VsLinearHillStretchReceptor::StepSimulation()
{
	CalculateTension();

	VsLine::StepSimulation(m_fltTension); 
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

