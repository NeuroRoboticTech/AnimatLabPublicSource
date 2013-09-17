// BlCone.cpp: implementation of the BlCone class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlOsgGeometry.h"
#include "BlJoint.h"
#include "BlRigidBody.h"
#include "BlCone.h"
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

BlCone::BlCone()
{
	SetThisPointers();
}

BlCone::~BlCone()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlCone\r\n", "", -1, false, true);}
}

void BlCone::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateConeGeometry(m_fltHeight, m_fltUpperRadius, m_fltLowerRadius, m_iSides, true, true, true);
}

void BlCone::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
	{
        m_fltMass = 1; //NEED TO FIX
        m_btCollisionShape = OsgMeshToConvexHull(m_osgNode.get(), true);
        m_bDisplayDebugCollisionGraphic = true;
	}
}

void BlCone::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	Cone::CreateParts();
}

void BlCone::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Cone::CreateJoints();
	BlRigidBody::Initialize();
}


		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
