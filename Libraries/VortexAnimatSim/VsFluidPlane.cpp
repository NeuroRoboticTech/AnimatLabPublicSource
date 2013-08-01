/**
\file	VsFluidPlane.cpp

\brief	Implements the vortex fluid plane class.
**/

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsFluidPlane.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
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
VsFluidPlane::VsFluidPlane()
{
	SetThisPointers();
	m_vxFluidPlane = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	6/30/2011
**/
VsFluidPlane::~VsFluidPlane()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsFluidPlane/\r\n", "", -1, false, true);}
}

void VsFluidPlane::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateBoxGeometry(m_ptSize.x, m_ptSize.y, 0.1f, (m_ptSize.x/GridX()), (m_ptSize.y/GridY()), 0.1f);
}

void VsFluidPlane::CreatePhysicsGeometry()
{
}

void VsFluidPlane::CreateParts()
{ 
	CreateGeometry();

	//Create the geometry and osg drawable nodes.
	m_eControlType = DynamicsControlType::ControlNode;  //This is not a dynamic part.

	VsRigidBody::CreateItem();
	FluidPlane::CreateParts();
	VsRigidBody::SetBody();
}

void VsFluidPlane::ResizePhysicsGeometry()
{
}

//Planes can never have fluid interactions/dynamics.
void VsFluidPlane::Physics_FluidDataChanged()
{}

void VsFluidPlane::SetGravity()
{
	if(m_vxFluidPlane)
		m_vxFluidPlane->setGravity(VxVector3(0, m_lpSim->Gravity(), 0));
}

void VsFluidPlane::Velocity(CStdFPoint &oPoint, bool bUseScaling)
{
	FluidPlane::Velocity(oPoint, bUseScaling);

	if(m_vxFluidPlane)
	    m_vxFluidPlane->setDefaultVelocity(VxVector3(m_vVelocity.x, m_vVelocity.y, m_vVelocity.z));
}

void VsFluidPlane::Physics_SetDensity(float fltVal)
{
	if(m_vxFluidPlane)
	    m_vxFluidPlane->setDefaultDensity(fltVal);
}

void VsFluidPlane::UpdateFluidPlaneHeight()
{
	if(m_vxFluidPlane)
	{
		CStdFPoint vPos;
		
		if(m_lpThisRB->IsRoot())
			vPos = m_lpThisAB->GetStructure()->Position();
		else
			vPos = m_lpThisRB->AbsolutePosition();

		m_vxFluidPlane->setLevel(vPos.y);
	}
}

void VsFluidPlane::Position(CStdFPoint &oPoint, bool bUseScaling, bool bFireChangeEvent, bool bUpdateMatrix)
{
	FluidPlane::Position(oPoint, bUseScaling, bFireChangeEvent, bUpdateMatrix);
	UpdateFluidPlaneHeight();
}

void VsFluidPlane::Physics_PositionChanged()
{
	VsRigidBody::Physics_PositionChanged();
	UpdateFluidPlaneHeight();
}

void VsFluidPlane::DeletePhysics()
{
	if(!m_vxFluidPlane)
		return;

	if(GetVsSimulator() && GetVsSimulator()->Universe())
	{
		GetVsSimulator()->Universe()->removeFluidState(m_vxFluidPlane);
		delete m_vxFluidPlane;
	}

	m_vxFluidPlane = NULL;
}

void VsFluidPlane::SetupPhysics()
{
	if(m_vxFluidPlane)
		DeletePhysics();

	m_vxFluidPlane = new VxPlanarFluidState(m_oAbsPosition.y);
    m_vxFluidPlane->setDefaultDensity(m_fltDensity);
    m_vxFluidPlane->setDefaultVelocity(VxVector3(m_vVelocity.x, m_vVelocity.y, m_vVelocity.z));
	m_vxFluidPlane->setGravity(VxVector3(0, m_lpSim->Gravity(), 0));
	m_vxFluidPlane->setName(m_lpThisAB->ID().c_str());
	GetVsSimulator()->Universe()->addFluidState(m_vxFluidPlane);
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
