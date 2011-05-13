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

void VsPlane::CreateParts()
{ 
	m_osgGeometry = CreatePlaneGeometry(CornerX(), CornerY(), m_ptSize.x, m_ptSize.y, GridX(), GridY());
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());
	m_osgNode = osgGroup;
	m_vxGeometry = new VxPlane();

	//Create the geometry and osg drawable nodes.
	m_eControlType = VxEntity::kControlNode;  //This is not a dynamic part.

	VsRigidBody::CreateItem();
	Plane::CreateParts();
	VsRigidBody::SetBody();
}

void VsPlane::Resize()
{
	//First lets get rid of the current current geometry and then put new geometry in place.
	if(m_osgNode.valid())
	{
		osg::Geode *osgGroup = dynamic_cast<osg::Geode *>(m_osgNode.get());
		if(!osgGroup)
			THROW_TEXT_ERROR(Vs_Err_lNodeNotGeode, Vs_Err_strNodeNotGeode, m_lpThisAB->Name());

		if(osgGroup && osgGroup->containsDrawable(m_osgGeometry.get()))
			osgGroup->removeDrawable(m_osgGeometry.get());

		m_osgGeometry.release();

		//Create a new box geometry with the new sizes.
		m_osgGeometry = CreatePlaneGeometry(CornerX(), CornerY(), m_ptSize.x, m_ptSize.y, GridX(), GridY());
		m_osgGeometry->setName(m_lpThisAB->Name() + "_Geometry");

		//Add it to the geode.
		osgGroup->addDrawable(m_osgGeometry.get());

		//Now lets re-adjust the gripper size.
		if(m_osgDragger.valid())
			m_osgDragger->SetupMatrix();

		//Reset the user data for the new parts.
		osg::ref_ptr<VsOsgUserDataVisitor> osgVisitor = new VsOsgUserDataVisitor(this);
		osgVisitor->traverse(*m_osgNodeGroup);
	}

	if(m_vxGeometry)
	{
		m_vxGeometry = new VxPlane();

		GetBaseValues();
	}
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
