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

float BlFluidPlane::Height()
{
    CStdFPoint vPos = GetOSGWorldCoords();
    return vPos.y;
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

void BlFluidPlane::UpdateFluidPlaneHeight()
{
    BlSimulator *lpSim = GetBlSimulator();
    //Remove this fluid plane from the simulation.
    if(lpSim)
        lpSim->SortFluidPlanes();
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
    //Remove this fluid plane from the simulation.
    BlSimulator *lpSim = GetBlSimulator();
    if(lpSim)
        lpSim->RemoveFluidPlane(this);
}

void BlFluidPlane::SetupPhysics()
{
    //Add this fluid plane to the simulation.
    BlSimulator *lpSim = GetBlSimulator();
    if(lpSim)
        lpSim->AddFluidPlane(this);
}


		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
