// RbLinearHillStretchReceptor.cpp: implementation of the RbLinearHillStretchReceptor class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbLine.h"
#include "RbLinearHillStretchReceptor.h"
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

RbLinearHillStretchReceptor::RbLinearHillStretchReceptor()
{
	SetThisPointers();
}

RbLinearHillStretchReceptor::~RbLinearHillStretchReceptor()
{

	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbLinearHillStretchReceptor\r\n", "", -1, false, true);}
}

void RbLinearHillStretchReceptor::CreateParts()
{
	//We do nothing in createparts because we cannot build the line until after all parts are created
	//so we can get a handle to the attachment points.
}

void RbLinearHillStretchReceptor::CreateJoints()
{
	LinearHillStretchReceptor::CreateJoints();
	RbLine::CreateParts();

	m_fltIaRate = m_fltIaDischargeConstant*m_fltSeLength;
	m_fltIbRate = m_fltIbDischargeConstant*m_fltTension;
	m_fltIIRate = m_fltIIDischargeConstant*m_fltPeLength;
}

void RbLinearHillStretchReceptor::ResetSimulation()
{
	LinearHillStretchReceptor::ResetSimulation();
	RbLine::ResetSimulation();
}

void RbLinearHillStretchReceptor::AfterResetSimulation()
{
	LinearHillStretchReceptor::AfterResetSimulation();
	RbLine::AfterResetSimulation();
}

void RbLinearHillStretchReceptor::StepSimulation()
{
	CalculateTension();

	RbLine::StepSimulation(m_fltTension); 
}

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim

