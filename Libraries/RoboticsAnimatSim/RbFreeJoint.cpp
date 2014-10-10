/**
\file	RbFreeJoint.cpp

\brief	Implements the vortex universal class.
**/

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbRigidBody.h"
#include "RbStructure.h"
#include "RbFreeJoint.h"


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
RbFreeJoint::RbFreeJoint()
{
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	4/15/2011
**/
RbFreeJoint::~RbFreeJoint()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbFreeJoint/\r\n", "", -1, false, true);}
}

void RbFreeJoint::CreateJoint()
{
}

#pragma region DataAccesMethods


#pragma endregion

    	}			// Joints
	}			// Environment
}				//VortexAnimatSim
