// VsSphere.cpp: implementation of the VsSphere class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsSphere.h"
#include "VsSimulator.h"

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
	SetThisPointers();
}

VsSphere::~VsSphere()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsSphere/\r\n", "", -1, false, true);}
}

void VsSphere::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateSphereGeometry(LatitudeSegments(), LongtitudeSegments(), m_fltRadius);
}

void VsSphere::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
		m_vxGeometry = new VxSphere(m_fltRadius);
}

void VsSphere::CreateParts()
{
	CreateGeometry();

	VsRigidBody::CreateItem();
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

void VsSphere::ResizePhysicsGeometry()
{
	if(m_vxGeometry)
	{
		VxSphere *vxSphere = dynamic_cast<VxSphere *>(m_vxGeometry);

		if(!vxSphere)
			THROW_TEXT_ERROR(Vs_Err_lGeometryMismatch, Vs_Err_strGeometryMismatch, m_lpThisAB->Name());
		
		vxSphere->setRadius(m_fltRadius);
	}
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
