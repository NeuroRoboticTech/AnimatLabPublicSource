// VsMesh.cpp: implementation of the VsMesh class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsMeshBase.h"
#include "VsMesh.h"
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

VsMesh::VsMesh()
{
	SetThisPointers();
}

VsMesh::~VsMesh()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsMesh/\r\n", "", -1, FALSE, TRUE);}
}

void VsMesh::CreateParts()
{
	CreateGeometry();

	VsMeshBase::CreateItem();
	Mesh::CreateParts();
	VsMeshBase::SetBody();
}

void VsMesh::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Mesh::CreateJoints();
	VsMeshBase::Initialize();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
