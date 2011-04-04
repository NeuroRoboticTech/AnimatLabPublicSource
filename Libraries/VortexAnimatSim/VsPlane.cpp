// VsPlane.cpp: implementation of the VsPlane class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsPlane.h"
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

VsPlane::VsPlane()
{
	m_lpThis = this;
	m_lpThisBody = this;
	m_lpPhysicsBody = this;
	m_bCullBackfaces = TRUE; //we want back face culling on by default for planes.
}

VsPlane::~VsPlane()
{
}

void VsPlane::CreateParts()
{ 
	m_osgGeometry = CreatePlaneGeometry(m_ptCorner.x, m_ptCorner.y, m_ptSize.x, m_ptSize.y, m_ptGrid.x, m_ptGrid.y);
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());
	m_osgNode = osgGroup;
	m_vxGeometry = new VxPlane();

	//Create the geometry and osg drawable nodes.
	m_oRotation.x = 1.57;  //The plane part is created with Z being up, but we use Y up.
	m_eControlType = VxEntity::kControlNode;  //This is not a dynamic part.

	VsRigidBody::CreateBody();
	Plane::CreateParts();
	VsRigidBody::SetBody();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
