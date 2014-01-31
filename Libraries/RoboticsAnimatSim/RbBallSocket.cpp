/**
\file	RbBallSocket.cpp

\brief	Implements the vortex ball socket class.
**/

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbBallSocket.h"
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
RbBallSocket::RbBallSocket()
{
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
RbBallSocket::~RbBallSocket()
{

	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbBallSocket\r\n", "", -1, false, true);}
}

#pragma region DataAccesMethods

#pragma endregion

		}		//Joints
	}			// Environment
}				//RoboticsAnimatSim
