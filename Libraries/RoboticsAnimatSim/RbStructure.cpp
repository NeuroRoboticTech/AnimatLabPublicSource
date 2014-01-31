/**
\file	RbStructure.cpp

\brief	Implements the vortex structure class.
**/

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbRigidBody.h"
#include "RbJoint.h"
#include "RbStructure.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{

/**
\brief	Default constructor.

\author	dcofer
\date	4/25/2011
**/
RbStructure::RbStructure()
{
	m_lpRbBody = NULL;
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	4/25/2011
**/
RbStructure::~RbStructure()
{
}

void RbStructure::Body(RigidBody *lpBody)
{
	Structure::Body(lpBody);
	m_lpRbBody = dynamic_cast<RbMovableItem *>(lpBody);

	if(!m_lpRbBody)
		THROW_TEXT_ERROR(Rb_Err_lUnableToConvertToVsRigidBody, Rb_Err_strUnableToConvertToVsRigidBody, "ID: " + lpBody->Name());
}

void RbStructure::SetThisPointers()
{
	RbMovableItem::SetThisPointers();

	m_lpThisST = dynamic_cast<Structure *>(this);
	if(!m_lpThisST)
		THROW_TEXT_ERROR(Rb_Err_lThisPointerNotDefined, Rb_Err_strThisPointerNotDefined, "m_lpThisST, " + m_lpThisAB->Name());
}

void RbStructure::Create()
{
	CreateItem();

	Structure::Create();
}

void RbStructure::ResetSimulation()
{
	RbMovableItem::Physics_ResetSimulation();

	Structure::ResetSimulation();
}

	}			// Environment
}				//RoboticsAnimatSim
