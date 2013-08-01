/**
\file	VsDistanceJoint.cpp

\brief	Implements the vortex distance joint class.
**/

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsSphere.h"
#include "VsSimulator.h"
#include "VsDistanceJoint.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

/**
\brief	Default constructor.

\author	dcofer
\date	4/15/2011
**/
VsDistanceJoint::VsDistanceJoint()
{
	SetThisPointers();
	m_vxDistance = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
VsDistanceJoint::~VsDistanceJoint()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsDistanceJoint/\r\n", "", -1, false, true);}
}

/**
\brief	Sets up the graphics for the joint.

\details This joint shows no graphics, so I am overriding this method and leaving it blank to show nothing.

\author	dcofer
\date	4/16/2011
**/
void VsDistanceJoint::SetupGraphics()
{
}

void VsDistanceJoint::DeletePhysics()
{
	if(!m_vxDistance)
		return;

	if(GetVsSimulator() && GetVsSimulator()->Universe())
	{
		GetVsSimulator()->Universe()->removeConstraint(m_vxDistance);
		delete m_vxDistance;

		if(m_lpChild && m_lpParent)
			m_lpChild->EnableCollision(m_lpParent);
	}

	m_vxDistance = NULL;
	m_vxJoint = NULL;
}

void VsDistanceJoint::SetupPhysics()
{
	if(m_vxDistance)
		DeletePhysics();

	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!m_lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpParent);
	if(!lpVsParent)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

	VsRigidBody *lpVsChild = dynamic_cast<VsRigidBody *>(m_lpChild);
	if(!lpVsChild)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

	float fltDistance = Std_CalculateDistance(m_lpParent->AbsolutePosition(), m_lpChild->AbsolutePosition());

	m_vxDistance = new VxDistanceJoint(lpVsParent->Part(), lpVsChild->Part(), fltDistance);
	m_vxDistance->setName(m_strID.c_str());

	GetVsSimulator()->Universe()->addConstraint(m_vxDistance);

	//Disable collisions between this object and its parent
	m_lpChild->DisableCollision(m_lpParent);

	m_vxJoint = m_vxDistance;
	m_iCoordID = -1; //Not used fo
}

void VsDistanceJoint::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods


bool VsDistanceJoint::SetData(const string &strDataType, const string &strValue, bool bThrowError)
{
	if(VsJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(Joint::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void VsDistanceJoint::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	VsJoint::Physics_QueryProperties(aryNames, aryTypes);
	Joint::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
