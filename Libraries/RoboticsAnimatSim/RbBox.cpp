// RbBox.cpp: implementation of the RbBox class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbBox.h"
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

RbBox::RbBox()
{
	SetThisPointers();
}

RbBox::~RbBox()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbBox\r\n", "", -1, false, true);}
}

void RbBox::CreateParts()
{
	RbRigidBody::CreateItem();
	Box::CreateParts();
}

void RbBox::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Box::CreateJoints();
	RbRigidBody::Initialize();
}


		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
