// VsCylinder.cpp: implementation of the VsCylinder class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsCylinder.h"
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

VsCylinder::VsCylinder()
{
	SetThisPointers();
}

VsCylinder::~VsCylinder()
{

}

void VsCylinder::CreateParts()
{
	m_osgGeometry = CreateConeGeometry(m_fltHeight, m_fltRadius, m_fltRadius, 50, true, true, true);
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());
	m_osgNode = osgGroup;
	m_vxGeometry = new VxCylinder(m_fltRadius, m_fltHeight);

	//Lets get the volume and areas
	m_fltVolume = 2*VX_PI*m_fltRadius*m_fltRadius*m_fltHeight;
	m_fltXArea = 2*m_fltRadius*m_fltHeight;
	m_fltYArea = 2*m_fltRadius*m_fltHeight;
	m_fltZArea = 2*VX_PI*m_fltRadius*m_fltRadius;

	VsRigidBody::CreateItem();
	Cylinder::CreateParts();
	VsRigidBody::SetBody();
}

void VsCylinder::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Cylinder::CreateJoints();
	VsRigidBody::Initialize();
}
		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
