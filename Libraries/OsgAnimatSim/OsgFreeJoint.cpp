/**
\file	OsgFreeJoint.cpp

\brief	Implements the vortex universal class.
**/

#include "StdAfx.h"
#include <stdarg.h>
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgJoint.h"
#include "OsgRigidBody.h"
#include "OsgStructure.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"

#include "OsgMouseSpring.h"
#include "OsgLight.h"
#include "OsgCameraManipulator.h"
#include "OsgDragger.h"

#include "OsgFreeJoint.h"


namespace OsgAnimatSim
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
OsgFreeJoint::OsgFreeJoint()
{
    m_bPhsyicsDefined = false;
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
OsgFreeJoint::~OsgFreeJoint()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of OsgFreeJoint/\r\n", "", -1, false, true);}
}

void OsgFreeJoint::DeletePhysics()
{
}

void OsgFreeJoint::SetupPhysics()
{
    m_bPhsyicsDefined = true;
}

void OsgFreeJoint::CreateJoint()
{
	SetupGraphics();
	SetupPhysics();
}

#pragma region DataAccesMethods


bool OsgFreeJoint::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(OsgJoint::Physics_SetData(strDataType, strValue))
		return true;

	if(BallSocket::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void OsgFreeJoint::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	OsgJoint::Physics_QueryProperties(aryNames, aryTypes);
	BallSocket::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

    	}			// Joints
	}			// Environment
}				//VortexAnimatSim
