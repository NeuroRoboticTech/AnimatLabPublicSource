// VsPlane.cpp: implementation of the VsPlane class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
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
//
//void VsPlane::Selected(BOOL bValue, BOOL bSelectMultiple)  
//{
//	Plane::Selected(bValue, bSelectMultiple);
//	VsRigidBody::Selected(bValue, bSelectMultiple);
//}
//
//void VsPlane::EnableCollision(Simulator *lpSim, RigidBody *lpBody)
//{VsRigidBody::EnableCollision(lpSim, lpBody);}
//
//void VsPlane::DisableCollision(Simulator *lpSim, RigidBody *lpBody)
//{VsRigidBody::DisableCollision(lpSim, lpBody);}


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
//
//void VsPlane::ResetSimulation(Simulator *lpSim, Structure *lpStructure)
//{
//	VsRigidBody::ResetSimulation(lpSim, lpStructure);
//	Plane::ResetSimulation(lpSim, lpStructure);
//}
		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
