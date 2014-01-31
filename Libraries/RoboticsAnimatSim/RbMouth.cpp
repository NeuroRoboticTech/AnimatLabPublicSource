/**
\file	RbMouth.cpp

\brief	Implements the vortex mouth class.
**/

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbRigidBody.h"
#include "RbStructure.h"

#include "RbMouth.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

/**
\brief	Default constructor.

\author	dcofer
\date	6/12/2011
**/
RbMouth::RbMouth()
{
	SetThisPointers();
}

/**
\brief	Destructor.

\author	dcofer
\date	6/12/2011
**/
RbMouth::~RbMouth()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbMouth/\r\n", "", -1, false, true);}
}
void RbMouth::CreateParts()
{
	RbRigidBody::CreateItem();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

