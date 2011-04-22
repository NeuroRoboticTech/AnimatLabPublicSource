// VsSphere.cpp: implementation of the VsSphere class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsRigidBody.h"
#include "VsSphere.h"
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

VsSphere::VsSphere()
{
	m_lpThis = this;
	m_lpThisBody = this;
	PhysicsBody(this);
	m_cgSphere = NULL;
}

VsSphere::~VsSphere()
{

}

void VsSphere::CreateParts()
{
	m_osgGeometry = CreateSphereGeometry(50, 50, m_fltRadius);
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());
	m_osgNode = osgGroup;
	m_vxGeometry = new VxSphere(m_fltRadius);

	//Lets get the volume and areas
	m_fltVolume = 1.33333*VX_PI*m_fltRadius*m_fltRadius*m_fltRadius;
	m_fltXArea = 2*VX_PI*m_fltRadius*m_fltRadius;
	m_fltYArea = m_fltXArea;
	m_fltZArea = m_fltXArea;

	VsRigidBody::CreateBody();
	Sphere::CreateParts();
	VsRigidBody::SetBody();
}

void VsSphere::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Sphere::CreateJoints();
	VsRigidBody::Initialize();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
