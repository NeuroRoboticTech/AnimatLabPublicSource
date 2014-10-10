/**
\file	RbOrganism.cpp

\brief	Implements the vortex organism class.
**/

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbRigidBody.h"
#include "RbJoint.h"
#include "RbOrganism.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{

/**
\brief	Default constructor.

\author	dcofer
\date	8/27/2011
**/
RbOrganism::RbOrganism()
{
	m_lpRbBody = NULL;
	SetThisPointers();
}

RbOrganism::~RbOrganism()
{
}

void RbOrganism::Body(RigidBody *lpBody)
{
	Organism::Body(lpBody);
	m_lpRbBody = dynamic_cast<RbMovableItem *>(lpBody);

	if(!m_lpRbBody)
		THROW_TEXT_ERROR(Rb_Err_lUnableToConvertToVsRigidBody, Rb_Err_strUnableToConvertToVsRigidBody, "ID: " + lpBody->Name());
}

void RbOrganism::SetThisPointers()
{
	RbMovableItem::SetThisPointers();

	m_lpThisST = dynamic_cast<Structure *>(this);
	if(!m_lpThisST)
		THROW_TEXT_ERROR(Rb_Err_lThisPointerNotDefined, Rb_Err_strThisPointerNotDefined, "m_lpThisST, " + m_lpThisAB->Name());

	m_lpThisOG = dynamic_cast<Organism *>(this);
	if(!m_lpThisOG)
		THROW_TEXT_ERROR(Rb_Err_lThisPointerNotDefined, Rb_Err_strThisPointerNotDefined, "m_lpThisOG, " + m_lpThisAB->Name());
}

void RbOrganism::Create()
{
	CreateItem();

	Organism::Create();
}

void RbOrganism::ResetSimulation()
{
	RbMovableItem::Physics_ResetSimulation();

	Organism::ResetSimulation();
}

    }			// Environment
}				//RoboticsAnimatSim
