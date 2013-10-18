// BlMesh.cpp: implementation of the BlMesh class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlOsgGeometry.h"
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

/**
 \brief Freezes this object. In Bullet triangular meshes are not allowed to be dynamic, so I
 am overriding this method to always return true if mesh type is not convex. If it is convex 
 then it returns the Mesh::Freeze value.

 \author    David Cofer
 \date  10/18/2013

 \return    true if it Frozen or if it is other than convex, false if it is convex and not frozen.
 */

bool BlMesh::Freeze()
{
    if(m_strCollisionMeshType == "CONVEX")
        return Mesh::Freeze();
    else
        return true;
}

void BlMesh::CreateParts()
{
	CreateGeometry();
    
    //If this is a collision object then calculate the convex hull volume.
    if(m_lpThisRB->IsCollisionObject())
        m_fltVolume = OsgConvexHullVolume(m_osgMeshNode.get());
    else
        m_fltVolume = 0;

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
