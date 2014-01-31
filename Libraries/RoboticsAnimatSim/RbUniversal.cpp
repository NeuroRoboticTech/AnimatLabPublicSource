/**
\file	RbUniversal.cpp

\brief	Implements the vortex universal class.
**/

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbSphere.h"
#include "RbSimulator.h"
#include "RbUniversal.h"


namespace RoboticsAnimatSim
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
RbUniversal::RbUniversal()
{
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
RbUniversal::~RbUniversal()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbUniversal/\r\n", "", -1, false, true);}
}

void RbUniversal::CreateJoint()
{
}

#pragma region DataAccesMethods


bool RbUniversal::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	//if(RbJoint::Physics_SetData(strDataType, strValue))
	//	return true;

	if(BallSocket::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RbUniversal::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	//RbJoint::Physics_QueryProperties(aryNames, aryTypes);
	BallSocket::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//RoboticsAnimatSim
