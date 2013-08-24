// BlLinearHillStretchReceptor.cpp: implementation of the BlLinearHillStretchReceptor class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlLine.h"
#include "BlLinearHillStretchReceptor.h"
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

BlLinearHillStretchReceptor::BlLinearHillStretchReceptor()
{
	SetThisPointers();
}

BlLinearHillStretchReceptor::~BlLinearHillStretchReceptor()
{

	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlLinearHillStretchReceptor\r\n", "", -1, false, true);}
}

void BlLinearHillStretchReceptor::CreateParts()
{
	//We do nothing in createparts because we cannot build the line until after all parts are created
	//so we can get a handle to the attachment points.
}

void BlLinearHillStretchReceptor::CreateJoints()
{
	LinearHillStretchReceptor::CreateJoints();
	BlLine::CreateParts();

	m_fltIaRate = m_fltIaDischargeConstant*m_fltSeLength;
	m_fltIbRate = m_fltIbDischargeConstant*m_fltTension;
	m_fltIIRate = m_fltIIDischargeConstant*m_fltPeLength;
}

void BlLinearHillStretchReceptor::ResetSimulation()
{
	LinearHillStretchReceptor::ResetSimulation();
	BlLine::ResetSimulation();
}

void BlLinearHillStretchReceptor::AfterResetSimulation()
{
	LinearHillStretchReceptor::AfterResetSimulation();
	BlLine::AfterResetSimulation();
}

void BlLinearHillStretchReceptor::StepSimulation()
{
	CalculateTension();

	BlLine::StepSimulation(m_fltTension); 
}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim

