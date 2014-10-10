// RbCone.cpp: implementation of the RbCone class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbRigidBody.h"
#include "RbCone.h"
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

RbCone::RbCone()
{
	SetThisPointers();
}

RbCone::~RbCone()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbCone\r\n", "", -1, false, true);}
}

void RbCone::CreateParts()
{
	RbRigidBody::CreateItem();
	Cone::CreateParts();
}

void RbCone::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Cone::CreateJoints();
	RbRigidBody::Initialize();
}


		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
