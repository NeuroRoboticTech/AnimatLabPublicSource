// BlCylinder.cpp: implementation of the BlCylinder class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlCylinder.h"
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

BlCylinder::BlCylinder()
{
	SetThisPointers();
}

BlCylinder::~BlCylinder()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlCylinder\r\n", "", -1, false, true);}
}

void BlCylinder::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateConeGeometry(m_fltHeight, m_fltRadius, m_fltRadius, m_iSides, true, true, true);
	
	//We need to setup a geometry rotation to make the cylinder graphics geometry match the physics geometry.
	CStdFPoint vPos(0, 0, 0), vRot(-(osg::PI/2), 0, 0);
	GeometryRotationMatrix(SetupMatrix(vPos, vRot));
}

void BlCylinder::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
    {
        m_fltMass = m_fltDensity * osg::PI * m_fltRadius * m_fltRadius * m_fltHeight;
        m_btCollisionShape = new btCylinderShapeZ( btVector3( m_fltRadius, m_fltRadius, (m_fltHeight/2.0f) ) );
    }
}

void BlCylinder::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	Cylinder::CreateParts();
	BlRigidBody::SetBody();
}

void BlCylinder::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Cylinder::CreateJoints();
	BlRigidBody::Initialize();
}

void BlCylinder::ResizePhysicsGeometry()
{
    //FIX PHYSICS
	//if(m_vxGeometry)
	//{
		//VxCylinder *vxCylinder = dynamic_cast<VxCylinder *>(m_vxGeometry);

		//if(!vxCylinder)
		//	THROW_TEXT_ERROR(Bl_Err_lGeometryMismatch, Bl_Err_strGeometryMismatch, m_lpThisAB->Name());
		//
		//vxCylinder->setRadius(m_fltRadius);
		//vxCylinder->setHeight(m_fltHeight);
	//}
}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
