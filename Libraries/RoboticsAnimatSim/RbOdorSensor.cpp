// RbOdorSensor.cpp: implementation of the RbOdorSensor class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbRigidBody.h"
#include "RbStructure.h"

#include "RbOdorSensor.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbOdorSensor::RbOdorSensor()
{
	SetThisPointers();
}

RbOdorSensor::~RbOdorSensor()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbOdorSensor/\r\n", "", -1, false, true);}
}
void RbOdorSensor::CreateParts()
{
	RbRigidBody::CreateItem();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

