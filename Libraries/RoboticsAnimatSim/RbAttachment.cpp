// RbAttachment.cpp: implementation of the RbAttachment class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbRigidBody.h"
#include "RbStructure.h"

#include "RbAttachment.h"


namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbAttachment::RbAttachment()
{
	SetThisPointers();
}

RbAttachment::~RbAttachment()
{

	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbAttachment\r\n", "", -1, false, true);}
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

