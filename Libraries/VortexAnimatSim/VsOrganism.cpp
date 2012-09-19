/**
\file	VsOrganism.cpp

\brief	Implements the vortex organism class.
**/

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsOrganism.h"
#include "VsSimulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

/**
\brief	Default constructor.

\author	dcofer
\date	8/27/2011
**/
VsOrganism::VsOrganism()
{
	m_lpVsBody = NULL;
	SetThisPointers();
}

VsOrganism::~VsOrganism()
{
	m_lpAssembly = NULL;
}

void VsOrganism::Body(RigidBody *lpBody)
{
	Organism::Body(lpBody);
	m_lpVsBody = dynamic_cast<VsRigidBody *>(lpBody);
	//TODO: FIX BACK
	//if(!m_lpVsBody)
	//	THROW_TEXT_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody, "ID: " + lpBody->Name());
}

void VsOrganism::SetThisPointers()
{
	VsMovableItem::SetThisPointers();

	m_lpThisST = dynamic_cast<Structure *>(this);
	if(!m_lpThisST)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisST, " + m_lpThisAB->Name());

	m_lpThisOG = dynamic_cast<Organism *>(this);
	if(!m_lpThisOG)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisOG, " + m_lpThisAB->Name());
}

osg::Group *VsOrganism::ParentOSG()
{
	return GetVsSimulator()->OSGRoot();
}

void VsOrganism::Create()
{
	CreateItem();

	Organism::Create();
}

void VsOrganism::SetupPhysics()
{
}

void VsOrganism::ResetSimulation()
{
	VsMovableItem::Physics_ResetSimulation();

	Organism::ResetSimulation();
}

void VsOrganism::UpdatePositionAndRotationFromMatrix()
{
	VsMovableItem::UpdatePositionAndRotationFromMatrix();

	if(m_lpVsBody)
		m_lpVsBody->EndGripDrag();
}

	}			// Environment
}				//VortexAnimatSim
