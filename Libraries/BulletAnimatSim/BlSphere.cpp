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
		DeletePhysics();
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
        m_fltMass = 0; //m_fltDensity * 4/3 * osg::PI * m_fltRadius * m_fltRadius * m_fltRadius;
        m_btCollisionShape= osgbCollision::btSphereCollisionShapeFromOSG( m_osgNode.get() );
    }
}

void BlSphere::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	Sphere::CreateParts();
	BlRigidBody::SetBody();
}

void BlSphere::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Sphere::CreateJoints();
	BlRigidBody::Initialize();
}

void BlSphere::ResizePhysicsGeometry()
{
    //FIX PHYSICS
	//if(m_vxGeometry)
	//{
	//	VxSphere *vxSphere = dynamic_cast<VxSphere *>(m_vxGeometry);

	//	if(!vxSphere)
	//		THROW_TEXT_ERROR(Bl_Err_lGeometryMismatch, Bl_Err_strGeometryMismatch, m_lpThisAB->Name());
	//	
	//	vxSphere->setRadius(m_fltRadius);
	//}
}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
