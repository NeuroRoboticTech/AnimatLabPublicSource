/**
\file	VsLight.cpp

\brief	Implements the vortex Light class.
**/

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsLight.h"
#include "VsSimulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

/**
\brief	Default constructor.

\author	dcofer
\date	4/25/2011
**/
VsLight::VsLight()
{
	m_lpThisLI = NULL;
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	4/25/2011
**/
VsLight::~VsLight()
{
}

void VsLight::SetThisPointers()
{
	VsMovableItem::SetThisPointers();

	m_lpThisLI = dynamic_cast<Light *>(this);
	if(!m_lpThisLI)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisLI, " + m_lpThisAB->Name());
}

void VsLight::Create()
{
	//m_osgGeometry = CreateSphereGeometry(15, 15,  m_lpThisST->Size());
	//osg::Geode *osgGroup = new osg::Geode;
	//osgGroup->addDrawable(m_osgGeometry.get());
	//m_osgNode = osgGroup;

	CreateItem();

	Light::Create();
}

void VsLight::ResetSimulation()
{
	VsMovableItem::Physics_ResetSimulation();

	Light::ResetSimulation();
}


	}			// Environment
}				//VortexAnimatSim
