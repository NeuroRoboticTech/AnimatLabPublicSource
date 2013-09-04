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
		DeletePhysics();
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
        m_btCollisionShape =  new btStaticPlaneShape(btVector3(0,1,0), 0);
}

void BlPlane::CreateParts()
{ 
	CreateGeometry();

	//Create the geometry and osg drawable nodes.
	m_eControlType = DynamicsControlType::ControlNode;  //This is not a dynamic part.

	BlRigidBody::CreateItem();
	Plane::CreateParts();
	BlRigidBody::SetBody();
}

void BlPlane::CreateDynamicPart()
{
    BlSimulator *lpSim = GetBlSimulator();

	if(lpSim && m_lpThisRB && m_lpThisAB)
	{
        //m_osgDebugNode = osgbDynamics::generateGroundPlane( osg::Vec4( 0, 1, 0, 0 ), lpSim->DynamicsWorld());
        //m_osgDebugNode->setName(m_lpThisRB->Name() + "_Debug");
		//m_osgNodeGroup->addChild(m_osgDebugNode.get());	

       // GetBlSimulator()->OSGRoot()->addChild(m_osgDebugNode.get());
       // m_osgNodeGroup->removeChild(m_osgNode.get());

        //m_btCollisionShape = new btStaticPlaneShape(btVector3(0,1,0), 1);
        btRigidBody::btRigidBodyConstructionInfo rbInfo( 0., NULL, m_btCollisionShape, btVector3(0,0,0) );
        m_btPart = new btRigidBody(rbInfo);
        lpSim->DynamicsWorld()->addRigidBody( m_btPart );

        m_osgbMotion = dynamic_cast<osgbDynamics::MotionState *>(m_btPart->getMotionState());
        //m_btCollisionShape = m_btPart->getCollisionShape();
	}
}

void BlPlane::ResizePhysicsGeometry()
{
    if(IsCollisionObject())
        m_btCollisionShape =  new btStaticPlaneShape(btVector3(0,1,0), 0);
}

//Planes can never have fluid interactions/dynamics.
void BlPlane::Physics_FluidDataChanged()
{}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim