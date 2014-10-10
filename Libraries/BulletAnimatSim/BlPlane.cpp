/**
\file	BlPlane.cpp

\brief	Implements the vortex plane class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlPlane.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
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
BlPlane::BlPlane()
{
	SetThisPointers();
	m_bCullBackfaces = true; //we want back face culling on by default for planes.
}

/**
\brief	Destructor.

\author	dcofer
\date	4/17/2011
**/
BlPlane::~BlPlane()
{
	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlPlane/\r\n", "", -1, false, true);}
}

void BlPlane::CreateGraphicsGeometry()
{
	m_osgGeometry = CreatePlaneGeometry(CornerX(), CornerY(), m_ptSize.x, m_ptSize.y, GridX(), GridY(), false);
}

void BlPlane::CreatePhysicsGeometry()
{
    if(IsCollisionObject())
    {
        DeleteCollisionGeometry();

        m_fltMass = 0;  //Plane is always a static object.

		if(m_osgMT.valid())
			OsgMovableItem::UpdatePositionAndRotationFromMatrix(m_osgMT->getMatrix());

		CStdFPoint vPos = m_lpStructure->Position();
        m_eBodyType = STATIC_PLANE_PROXYTYPE;
        m_btCollisionShape =  new btStaticPlaneShape(btVector3(0,1,0), vPos.y);
    }
}

void BlPlane::CreateParts()
{ 
	CreateGeometry();

	//Create the geometry and osg drawable nodes.
	m_eControlType = DynamicsControlType::ControlNode;  //This is not a dynamic part.

	BlRigidBody::CreateItem();
	Plane::CreateParts();
}

void BlPlane::CreateDynamicPart()
{
    BlSimulator *lpSim = GetBlSimulator();

	if(lpSim && m_lpThisRB && m_lpThisAB)
	{
        //m_btCollisionShape = new btStaticPlaneShape(btVector3(0,1,0), 1);
        btRigidBody::btRigidBodyConstructionInfo rbInfo( 0., NULL, m_btCollisionShape, btVector3(0,0,0) );

		rbInfo.m_friction = m_lpMaterial->FrictionLinearPrimary();
        rbInfo.m_rollingFriction = m_lpMaterial->FrictionAngularPrimaryConverted();
		rbInfo.m_restitution = m_lpMaterial->Restitution();
        rbInfo.m_linearDamping = m_lpThisRB->LinearVelocityDamping();
        rbInfo.m_angularDamping = m_lpThisRB->AngularVelocityDamping();

        m_btPart = new btRigidBody(rbInfo);

        if(!m_lpBulletData)
            m_lpBulletData = new BlBulletData(this, false);
        m_btPart->setUserPointer((void *) m_lpBulletData);

        lpSim->DynamicsWorld()->addRigidBody( m_btPart, AnimatCollisionTypes::RIGID_BODY, ALL_COLLISIONS );

        m_osgbMotion = dynamic_cast<osgbDynamics::MotionState *>(m_btPart->getMotionState());
	}
}

//
//void BlPlane::ResizePhysicsGeometry()
//{
//}

//Planes can never have fluid interactions/dynamics.
void BlPlane::Physics_FluidDataChanged()
{}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
