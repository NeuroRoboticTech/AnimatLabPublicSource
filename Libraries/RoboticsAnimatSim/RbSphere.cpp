// RbSphere.cpp: implementation of the RbSphere class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbSphere.h"
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

RbSphere::RbSphere()
{
	SetThisPointers();
}

RbSphere::~RbSphere()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbSphere/\r\n", "", -1, false, true);}
}

void RbSphere::CreateParts()
{
	RbRigidBody::CreateItem();
	Sphere::CreateParts();
}

void RbSphere::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Sphere::CreateJoints();
	RbRigidBody::Initialize();
}

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
