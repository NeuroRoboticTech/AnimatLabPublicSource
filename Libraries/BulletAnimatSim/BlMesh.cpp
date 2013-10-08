// BlMesh.cpp: implementation of the BlMesh class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlMeshBase.h"
#include "BlMesh.h"
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

BlMesh::BlMesh()
{
	SetThisPointers();
}

BlMesh::~BlMesh()
{
	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlMesh/\r\n", "", -1, false, true);}
}

void BlMesh::CreateParts()
{
	CreateGeometry();

	BlMeshBase::CreateItem();
	Mesh::CreateParts();
}

void BlMesh::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Mesh::CreateJoints();
	BlMeshBase::Initialize();
}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
