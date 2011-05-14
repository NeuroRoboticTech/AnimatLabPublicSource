/**
\file	VsPlane.cpp

\brief	Implements the vortex plane class.
**/

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsPlane.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

/**
\brief	Default constructor.

\author	dcofer
\date	4/17/2011
**/
VsPlane::VsPlane()
{
	SetThisPointers();
	m_bCullBackfaces = TRUE; //we want back face culling on by default for planes.
}

/**
\brief	Destructor.

\author	dcofer
\date	4/17/2011
**/
VsPlane::~VsPlane()
{
}

void VsPlane::CreateGraphicsGeometry()
{
	m_osgGeometry = CreatePlaneGeometry(CornerX(), CornerY(), m_ptSize.x, m_ptSize.y, GridX(), GridY());
}

void VsPlane::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
		m_vxGeometry = new VxPlane();
}

void VsPlane::CreateParts()
{ 
	CreateGeometry();

	//Create the geometry and osg drawable nodes.
	m_eControlType = VxEntity::kControlNode;  //This is not a dynamic part.

	VsRigidBody::CreateItem();
	Plane::CreateParts();
	VsRigidBody::SetBody();
}

void VsPlane::ResizePhysicsGeometry()
{
	if(m_vxGeometry)
		m_vxGeometry = new VxPlane();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
