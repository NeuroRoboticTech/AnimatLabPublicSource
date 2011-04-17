// VsBox.cpp: implementation of the VsBox class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsBox.h"
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsBox::VsBox()
{
	m_lpThis = this;
	m_lpThisBody = this;
	m_lpPhysicsBody = this;
}

VsBox::~VsBox()
{
}

void VsBox::CreateParts()
{
	m_osgGeometry = CreateBoxGeometry(Length(), Height(), Width(), LengthSegmentSize(), HeightSegmentSize(), WidthSegmentSize());
	m_osgGeometry->setName(m_lpThis->Name() + "_Geometry");

	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());
	m_osgNode = osgGroup;
	m_osgNode->setName(m_lpThis->Name() + "_Node");

	if(IsCollisionObject())
		m_vxGeometry = new VxBox(m_fltLength, m_fltHeight, m_fltWidth);

	//Lets get the volume and areas
	m_fltVolume = m_fltLength * m_fltHeight * m_fltWidth;
	m_fltXArea = m_fltHeight * m_fltWidth;
	m_fltYArea = m_fltLength * m_fltWidth;
	m_fltZArea = m_fltHeight * m_fltLength;

	VsRigidBody::CreateBody();
	Box::CreateParts();
	VsRigidBody::SetBody();
}

void VsBox::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Box::CreateJoints();
	VsRigidBody::Initialize();
}

void VsBox::Resize()
{
	//First lets get rid of the current current geometry and then put new geometry in place.
	if(m_osgNode.valid())
	{
		osg::Geode *osgGroup = dynamic_cast<osg::Geode *>(m_osgNode.get());
		if(!osgGroup)
			THROW_TEXT_ERROR(Vs_Err_lNodeNotGeode, Vs_Err_strNodeNotGeode, m_lpThis->Name());

		if(osgGroup && osgGroup->containsDrawable(m_osgGeometry.get()))
			osgGroup->removeDrawable(m_osgGeometry.get());

		m_osgGeometry.release();

		//Create a new box geometry with the new sizes.
		m_osgGeometry = CreateBoxGeometry(Length(), Height(), Width(), LengthSegmentSize(), HeightSegmentSize(), WidthSegmentSize());
		m_osgGeometry->setName(m_lpThis->Name() + "_Geometry");

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
		VxBox *vxBox = dynamic_cast<VxBox *>(m_vxGeometry);

		if(!vxBox)
			THROW_TEXT_ERROR(Vs_Err_lGeometryMismatch, Vs_Err_strGeometryMismatch, m_lpThis->Name());
		
		vxBox->setDimensions(m_fltLength, m_fltHeight, m_fltWidth);
		GetBaseValues();
	}
}


		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
