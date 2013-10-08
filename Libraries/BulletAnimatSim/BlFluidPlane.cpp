/**
\file	BlFluidPlane.cpp

\brief	Implements the vortex fluid plane class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlFluidPlane.h"
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
\date	6/30/2011
**/
BlFluidPlane::BlFluidPlane()
{
	SetThisPointers();
    //FIX PHYSICS
	//m_vxFluidPlane = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	6/30/2011
**/
BlFluidPlane::~BlFluidPlane()
{
	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlFluidPlane/\r\n", "", -1, false, true);}
}

void BlFluidPlane::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateBoxGeometry(m_ptSize.x, m_ptSize.y, 0.1f, (m_ptSize.x/GridX()), (m_ptSize.y/GridY()), 0.1f);
}

void BlFluidPlane::CreatePhysicsGeometry()
{
}

void BlFluidPlane::CreateParts()
{ 
	CreateGeometry();

	//Create the geometry and osg drawable nodes.
	m_eControlType = DynamicsControlType::ControlNode;  //This is not a dynamic part.

	BlRigidBody::CreateItem();
	FluidPlane::CreateParts();
}

void BlFluidPlane::ResizePhysicsGeometry()
{
}

//Planes can never have fluid interactions/dynamics.
void BlFluidPlane::Physics_FluidDataChanged()
{}

void BlFluidPlane::SetGravity()
{
    //FIX PHYSICS
	//if(m_vxFluidPlane)
	//	m_vxFluidPlane->setGravity(VxVector3(0, m_lpSim->Gravity(), 0));
}

void BlFluidPlane::Velocity(CStdFPoint &oPoint, bool bUseScaling)
{
	FluidPlane::Velocity(oPoint, bUseScaling);

    //FIX PHYSICS
	//if(m_vxFluidPlane)
	//    m_vxFluidPlane->setDefaultVelocity(VxVector3(m_vVelocity.x, m_vVelocity.y, m_vVelocity.z));
}

void BlFluidPlane::Physics_SetDensity(float fltVal)
{
    //FIX PHYSICS
	//if(m_vxFluidPlane)
	//    m_vxFluidPlane->setDefaultDensity(fltVal);
}

void BlFluidPlane::UpdateFluidPlaneHeight()
{
    //FIX PHYSICS
	//if(m_vxFluidPlane)
	//{
	//	CStdFPoint vPos;
	//	
	//	if(m_lpThisRB->IsRoot())
	//		vPos = m_lpThisAB->GetStructure()->Position();
	//	else
	//		vPos = m_lpThisRB->AbsolutePosition();

	//	m_vxFluidPlane->setLevel(vPos.y);
	//}
}

void BlFluidPlane::Position(CStdFPoint &oPoint, bool bUseScaling, bool bFireChangeEvent, bool bUpdateMatrix)
{
	FluidPlane::Position(oPoint, bUseScaling, bFireChangeEvent, bUpdateMatrix);
	UpdateFluidPlaneHeight();
}

void BlFluidPlane::Physics_PositionChanged()
{
	BlRigidBody::Physics_PositionChanged();
	UpdateFluidPlaneHeight();
}

void BlFluidPlane::DeletePhysics(bool bIncludeChildren)
{
    //FIX PHYSICS
	//if(!m_vxFluidPlane)
	//	return;

	//if(GetBlSimulator() && GetBlSimulator()->Universe())
	//{
	//	GetBlSimulator()->Universe()->removeFluidState(m_vxFluidPlane);
	//	delete m_vxFluidPlane;
	//}

	//m_vxFluidPlane = NULL;
}

void BlFluidPlane::SetupPhysics()
{
    //FIX PHYSICS
	//if(m_vxFluidPlane)
	//	DeletePhysics(false);

	//m_vxFluidPlane = new VxPlanarFluidState(m_oAbsPosition.y);
 //   m_vxFluidPlane->setDefaultDensity(m_fltDensity);
 //   m_vxFluidPlane->setDefaultVelocity(VxVector3(m_vVelocity.x, m_vVelocity.y, m_vVelocity.z));
	//m_vxFluidPlane->setGravity(VxVector3(0, m_lpSim->Gravity(), 0));
	//m_vxFluidPlane->setName(m_lpThisAB->ID().c_str());
	//GetBlSimulator()->Universe()->addFluidState(m_vxFluidPlane);
}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
