// RbMesh.cpp: implementation of the RbMesh class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbMesh.h"
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

RbMesh::RbMesh()
{
	SetThisPointers();
}

RbMesh::~RbMesh()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbMesh/\r\n", "", -1, false, true);}
}

void RbMesh::CreateParts()
{
	RbRigidBody::CreateItem();
	Mesh::CreateParts();
}

void RbMesh::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Mesh::CreateJoints();
	RbRigidBody::Initialize();
}

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
