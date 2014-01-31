/**
\file	RbRPRO.cpp

\brief	Implements the vortex ball socket class.
**/

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbRPRO.h"
#include "RbSimulator.h"

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
RbRPRO::RbRPRO()
{
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
RbRPRO::~RbRPRO()
{

	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbRPRO\r\n", "", -1, false, true);}
}

void RbRPRO::CreateJoint()
{
}

#pragma region DataAccesMethods

bool RbRPRO::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	//if(RbJoint::Physics_SetData(strDataType, strValue))
	//	return true;

	if(RPRO::SetData(strDataType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RbRPRO::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	//RbJoint::Physics_QueryProperties(aryNames, aryTypes);
	RPRO::QueryProperties(aryNames, aryTypes);
}

#pragma endregion

		}		//Joints
	}			// Environment
}				//RoboticsAnimatSim
