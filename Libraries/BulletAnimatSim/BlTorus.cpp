/**
\file	BlTorus.cpp

\brief	Implements the vortex Torus class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlRigidBody.h"
#include "BlTorus.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
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
BlTorus::BlTorus()
{
	SetThisPointers();
}

BlTorus::~BlTorus()
{
	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlTorus/\r\n", "", -1, false, true);}
}

void BlTorus::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateTorusGeometry(m_fltInsideRadius, m_fltOutsideRadius, m_iSides, m_iRings);
}

void BlTorus::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
	{
        DeleteCollisionGeometry();

        m_fltMass = 1; //FIX PHYSICS
        m_btCollisionShape = osgbCollision::btTriMeshCollisionShapeFromOSG(m_osgNode.get());
        m_bDisplayDebugCollisionGraphic = true;
	}
}

void BlTorus::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	Torus::CreateParts();
}

void BlTorus::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Torus::CreateJoints();
	BlRigidBody::Initialize();
}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
