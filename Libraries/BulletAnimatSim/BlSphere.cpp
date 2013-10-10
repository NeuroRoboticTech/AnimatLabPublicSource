// BlSphere.cpp: implementation of the BlSphere class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlSphere.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BlSphere::BlSphere()
{
	SetThisPointers();
}

BlSphere::~BlSphere()
{
	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlSphere/\r\n", "", -1, false, true);}
}

void BlSphere::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateSphereGeometry(LatitudeSegments(), LongtitudeSegments(), m_fltRadius);
}

void BlSphere::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
    {
        DeleteCollisionGeometry();
    
        m_fltMass = m_fltDensity * 4/3 * osg::PI * m_fltRadius * m_fltRadius * m_fltRadius;
        m_btCollisionShape = new btSphereShape( m_fltRadius );
    }
}

void BlSphere::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	Sphere::CreateParts();
}

void BlSphere::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Sphere::CreateJoints();
	BlRigidBody::Initialize();
}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
