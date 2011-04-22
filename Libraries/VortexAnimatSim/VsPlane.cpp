/**
\file	VsPlane.cpp

\brief	Implements the vortex plane class.
**/

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

/**
\brief	Default constructor.

\author	dcofer
\date	4/17/2011
**/
VsPlane::VsPlane()
{
	m_lpThis = this;
	m_lpThisBody = this;
	PhysicsBody(this);
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

	VsRigidBody::CreateBody();
	Plane::CreateParts();
	VsRigidBody::SetBody();
}

void VsPlane::Resize()
{
	////First lets get rid of the current current geometry and then put new geometry in place.
	//if(m_osgNode.valid())
	//{
	//	osg::Geode *osgGroup = dynamic_cast<osg::Geode *>(m_osgNode.get());
	//	if(!osgGroup)
	//		THROW_TEXT_ERROR(Vs_Err_lNodeNotGeode, Vs_Err_strNodeNotGeode, m_lpThis->Name());

	//	if(osgGroup && osgGroup->containsDrawable(m_osgGeometry.get()))
	//		osgGroup->removeDrawable(m_osgGeometry.get());

	//	m_osgGeometry.release();

	//	//Create a new box geometry with the new sizes.
	//	m_osgGeometry = CreateBoxGeometry(Length(), Height(), Width(), LengthSegmentSize(), HeightSegmentSize(), WidthSegmentSize());
	//	m_osgGeometry->setName(m_lpThis->Name() + "_Geometry");

	//	//Add it to the geode.
	//	osgGroup->addDrawable(m_osgGeometry.get());

	//	//Now lets re-adjust the gripper size.
	//	if(m_osgDragger.valid())
	//		m_osgDragger->SetupMatrix();

	//	//Reset the user data for the new parts.
	//	osg::ref_ptr<VsOsgUserDataVisitor> osgVisitor = new VsOsgUserDataVisitor(this);
	//	osgVisitor->traverse(*m_osgNodeGroup);
	//}

	//if(m_vxGeometry)
	//{
	//	VxBox *vxBox = dynamic_cast<VxBox *>(m_vxGeometry);

	//	if(!vxBox)
	//		THROW_TEXT_ERROR(Vs_Err_lGeometryMismatch, Vs_Err_strGeometryMismatch, m_lpThis->Name());
	//	
	//	vxBox->setDimensions(m_fltLength, m_fltHeight, m_fltWidth);
	//	GetBaseValues();
	//}
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
