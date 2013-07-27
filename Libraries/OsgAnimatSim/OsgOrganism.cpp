/**
\file	OsgOrganism.cpp

\brief	Implements the vortex organism class.
**/

#include "StdAfx.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgJoint.h"
#include "OsgOrganism.h"
#include "OsgDragger.h"

namespace OsgAnimatSim
{
	namespace Environment
	{

/**
\brief	Default constructor.

\author	dcofer
\date	8/27/2011
**/
OsgOrganism::OsgOrganism()
{
	m_lpOsgBody = NULL;
	SetThisPointers();
}

OsgOrganism::~OsgOrganism()
{
	m_lpAssembly = NULL;
}

void OsgOrganism::Body(RigidBody *lpBody)
{
	Organism::Body(lpBody);
	m_lpOsgBody = dynamic_cast<VsRigidBody *>(lpBody);

	if(!m_lpOsgBody)
		THROW_TEXT_ERROR(Osg_Err_lUnableToConvertToVsRigidBody, Osg_Err_strUnableToConvertToVsRigidBody, "ID: " + lpBody->Name());
}

void OsgOrganism::SetThisPointers()
{
	OsgMovableItem::SetThisPointers();

	m_lpThisST = dynamic_cast<Structure *>(this);
	if(!m_lpThisST)
		THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpThisST, " + m_lpThisAB->Name());

	m_lpThisOG = dynamic_cast<Organism *>(this);
	if(!m_lpThisOG)
		THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpThisOG, " + m_lpThisAB->Name());
}

osg::Group *OsgOrganism::ParentOSG()
{
	return GetVsSimulator()->OSGRoot();
}

void OsgOrganism::Create()
{
	CreateItem();

	Organism::Create();
}

void OsgOrganism::SetupPhysics()
{
}

void OsgOrganism::ResetSimulation()
{
	OsgMovableItem::Physics_ResetSimulation();

	Organism::ResetSimulation();
}

void OsgOrganism::UpdatePositionAndRotationFromMatrix()
{
	OsgMovableItem::UpdatePositionAndRotationFromMatrix();

	if(m_lpOsgBody)
		m_lpOsgBody->EndGripDrag();
}

	}			// Environment
}				//OsgAnimatSim