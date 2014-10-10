/**
\file	RbTorus.cpp

\brief	Implements the vortex Torus class.
**/

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbRigidBody.h"
#include "RbTorus.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

/**
\brief	Default constructor.

\author	dcofer
\date	6/12/2011
**/
RbTorus::RbTorus()
{
	SetThisPointers();
}

RbTorus::~RbTorus()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbTorus/\r\n", "", -1, false, true);}
}
void RbTorus::CreateParts()
{
	RbRigidBody::CreateItem();
	Torus::CreateParts();
}

void RbTorus::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Torus::CreateJoints();
	RbRigidBody::Initialize();
}

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
