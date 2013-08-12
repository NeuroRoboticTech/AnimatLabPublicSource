// VsCylinder.cpp: implementation of the VsCylinder class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsCylinder.h"
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

VsCylinder::VsCylinder()
{
	SetThisPointers();
}

VsCylinder::~VsCylinder()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsCylinder\r\n", "", -1, false, true);}
}

void VsCylinder::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateConeGeometry(m_fltHeight, m_fltRadius, m_fltRadius, m_iSides, true, true, true);
	
	//We need to setup a geometry rotation to make the cylinder graphics geometry match the physics geometry.
	CStdFPoint vPos(0, 0, 0), vRot(-(osg::PI/2), 0, 0);
	GeometryRotationMatrix(SetupMatrix(vPos, vRot));
}

void VsCylinder::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
		m_vxGeometry = new VxCylinder(m_fltRadius, m_fltHeight);
}

void VsCylinder::CreateParts()
{
	CreateGeometry();

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

void VsCylinder::ResizePhysicsGeometry()
{
	if(m_vxGeometry)
	{
		VxCylinder *vxCylinder = dynamic_cast<VxCylinder *>(m_vxGeometry);

		if(!vxCylinder)
			THROW_TEXT_ERROR(Vs_Err_lGeometryMismatch, Vs_Err_strGeometryMismatch, m_lpThisAB->Name());
		
		vxCylinder->setRadius(m_fltRadius);
		vxCylinder->setHeight(m_fltHeight);
	}
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
