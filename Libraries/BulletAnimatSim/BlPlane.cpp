/**
\file	BlPlane.cpp

\brief	Implements the vortex plane class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlPlane.h"
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
\date	4/17/2011
**/
BlPlane::BlPlane()
{
	SetThisPointers();
	m_bCullBackfaces = true; //we want back face culling on by default for planes.
}

/**
\brief	Destructor.

\author	dcofer
\date	4/17/2011
**/
BlPlane::~BlPlane()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlPlane/\r\n", "", -1, false, true);}
}

void BlPlane::CreateGraphicsGeometry()
{
	m_osgGeometry = CreatePlaneGeometry(CornerX(), CornerY(), m_ptSize.x, m_ptSize.y, GridX(), GridY(), false);
}

void BlPlane::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
		m_vxGeometry = new VxPlane();
}

void BlPlane::CreateParts()
{ 
	CreateGeometry();

	//Create the geometry and osg drawable nodes.
	m_eControlType = DynamicsControlType::ControlNode;  //This is not a dynamic part.

	BlRigidBody::CreateItem();
	Plane::CreateParts();
	BlRigidBody::SetBody();
}

void BlPlane::ResizePhysicsGeometry()
{
	if(m_vxGeometry)
		m_vxGeometry = new VxPlane();
}
//Planes can never have fluid interactions/dynamics.
void BlPlane::Physics_FluidDataChanged()
{}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
