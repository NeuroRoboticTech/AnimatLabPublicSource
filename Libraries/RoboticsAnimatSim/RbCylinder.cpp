// RbCylinder.cpp: implementation of the RbCylinder class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbCylinder.h"
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

RbCylinder::RbCylinder()
{
	SetThisPointers();
}

RbCylinder::~RbCylinder()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbCylinder\r\n", "", -1, false, true);}
}

void RbCylinder::CreateParts()
{
	RbRigidBody::CreateItem();
	Cylinder::CreateParts();
}

void RbCylinder::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Cylinder::CreateJoints();
	RbRigidBody::Initialize();
}

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
