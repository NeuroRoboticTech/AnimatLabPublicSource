// VsCone.cpp: implementation of the VsCone class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsRigidBody.h"
#include "VsCone.h"
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

VsCone::VsCone()
{
	m_lpThis = this;
	m_lpThisBody = this;
	m_lpPhysicsBody = this;
}

VsCone::~VsCone()
{

}

void VsCone::CreateParts()
{
	m_osgGeometry = CreateConeGeometry(m_fltHeight, m_fltUpperRadius, m_fltLowerRadius, 50, true, true, true);
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());

	m_osgNode = osgGroup;
	m_vxGeometry = VxConvexMesh::createFromNode(m_osgNode.get());

	//Lets get the volume and areas
	m_fltVolume = 2*VX_PI*m_fltLowerRadius*m_fltLowerRadius*m_fltHeight;
	m_fltXArea = 2*m_fltLowerRadius*m_fltHeight;
	m_fltYArea = 2*m_fltLowerRadius*m_fltHeight;
	m_fltZArea = 2*VX_PI*m_fltLowerRadius*m_fltLowerRadius;

	VsRigidBody::CreateBody();
	Cone::CreateParts();
	VsRigidBody::SetBody();
}

void VsCone::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Cone::CreateJoints();
	VsRigidBody::Initialize();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
