/**
\file	OsgStructure.cpp

\brief	Implements the vortex structure class.
**/

#include "StdAfx.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgStructure.h"
#include "OsgDragger.h"
#include "OsgSimulator.h"

namespace OsgAnimatSim
{
	namespace Environment
	{

/**
\brief	Default constructor.

\author	dcofer
\date	4/25/2011
**/
OsgStructure::OsgStructure()
{
	m_lpOsgBody = NULL;
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	4/25/2011
**/
OsgStructure::~OsgStructure()
{
}

void OsgStructure::Body(RigidBody *lpBody)
{
	Structure::Body(lpBody);
	m_lpOsgBody = dynamic_cast<OsgMovableItem *>(lpBody);

	if(!m_lpOsgBody)
		THROW_TEXT_ERROR(Osg_Err_lUnableToConvertToVsRigidBody, Osg_Err_strUnableToConvertToVsRigidBody, "ID: " + lpBody->Name());
}

void OsgStructure::SetThisPointers()
{
	OsgMovableItem::SetThisPointers();

	m_lpThisST = dynamic_cast<Structure *>(this);
	if(!m_lpThisST)
		THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpThisST, " + m_lpThisAB->Name());
}

osg::MatrixTransform *OsgStructure::ParentOSG()
{
	return GetOsgSimulator()->OSGRoot();
}

void OsgStructure::Create()
{
	CreateItem();

	Structure::Create();
}

void OsgStructure::SetupPhysics()
{
}

void OsgStructure::ResetSimulation()
{
	OsgMovableItem::Physics_ResetSimulation();

	Structure::ResetSimulation();
}

void OsgStructure::UpdatePositionAndRotationFromMatrix()
{
	OsgMovableItem::UpdatePositionAndRotationFromMatrix();

	if(m_lpOsgBody)
    {
		m_lpOsgBody->EndGripDrag();

        //Get the new world matrix from the root body and update our world position.
        m_osgFinalMatrix = m_lpOsgBody->FinalMatrix();
        Physics_UpdateAbsolutePosition();
    }
}


	}			// Environment
}				//OsgAnimatSim
