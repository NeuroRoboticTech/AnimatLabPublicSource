/**
\file	VsMouth.cpp

\brief	Implements the vortex mouth class.
**/

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsSphere.h"
#include "VsSimulator.h"
#include "VsFreeJoint.h"
#include "VsMouth.h"
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
\date	6/12/2011
**/
VsMouth::VsMouth()
{
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	6/12/2011
**/
VsMouth::~VsMouth()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsMouth/\r\n", "", -1, FALSE, TRUE);}
}

void VsMouth::CreateGraphicsGeometry() 
{
	m_osgGeometry = CreateSphereGeometry(LatitudeSegments(), LongtitudeSegments(), m_fltRadius);
}

void VsMouth::CreatePhysicsGeometry() {}

void VsMouth::ResizePhysicsGeometry() {}

void VsMouth::CreateParts()
{
	CreateGeometry();

	VsRigidBody::CreateItem();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

