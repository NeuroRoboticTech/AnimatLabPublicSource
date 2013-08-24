/**
\file	BlDistanceJoint.cpp

\brief	Implements the vortex distance joint class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlSphere.h"
#include "BlSimulator.h"
#include "BlDistanceJoint.h"

namespace BulletAnimatSim
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
BlDistanceJoint::BlDistanceJoint()
{
	SetThisPointers();
	m_vxDistance = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
BlDistanceJoint::~BlDistanceJoint()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlDistanceJoint/\r\n", "", -1, false, true);}
}

/**
\brief	Sets up the graphics for the joint.

\details This joint shows no graphics, so I am overriding this method and leaving it blank to show nothing.

\author	dcofer
\date	4/16/2011
**/
void BlDistanceJoint::SetupGraphics()
{
}

void BlDistanceJoint::DeletePhysics()
{
	if(!m_vxDistance)
		return;

	if(GetBlSimulator() && GetBlSimulator()->Universe())
	{
		GetBlSimulator()->Universe()->removeConstraint(m_vxDistance);
		delete m_vxDistance;

		if(m_lpChild && m_lpParent)
			m_lpChild->EnableCollision(m_lpParent);
	}

	m_vxDistance = NULL;
	m_vxJoint = NULL;
}

void BlDistanceJoint::SetupPhysics()
{
	if(m_vxDistance)
		DeletePhysics();

	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!m_lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	BlRigidBody *lpVsParent = dynamic_cast<BlRigidBody *>(m_lpParent);
	if(!lpVsParent)
		THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

	BlRigidBody *lpVsChild = dynamic_cast<BlRigidBody *>(m_lpChild);
	if(!lpVsChild)
		THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

	CStdFPoint vParentPos = m_lpParent->AbsolutePosition();
	CStdFPoint vChildPos = m_lpChild->AbsolutePosition();
	float fltDistance = Std_CalculateDistance(vParentPos, vChildPos);

	m_vxDistance = new VxDistanceJoint(lpVsParent->Part(), lpVsChild->Part(), fltDistance);
	m_vxDistance->setName(m_strID.c_str());

	GetBlSimulator()->Universe()->addConstraint(m_vxDistance);

	//Disable collisions between this object and its parent
	m_lpChild->DisableCollision(m_lpParent);

	m_vxJoint = m_vxDistance;
	m_iCoordID = -1; //Not used fo
}

void BlDistanceJoint::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods


bool BlDistanceJoint::SetData(const string &strDataType, const string &strValue, bool bThrowError)
{
	if(BlJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(Joint::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void BlDistanceJoint::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	BlJoint::Physics_QueryProperties(aryNames, aryTypes);
	Joint::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
