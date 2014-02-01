// RbLANWirelessInterface.cpp: implementation of the RbLANWirelessInterface class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbRigidBody.h"
#include "RbStructure.h"
#include "RbLANWirelessInterface.h"

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotInterfaces
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbLANWirelessInterface::RbLANWirelessInterface() 
{
}

RbLANWirelessInterface::~RbLANWirelessInterface()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbLANWirelessInterface\r\n", "", -1, false, true);}
}

		}		//RobotInterfaces
	}			// Robotics
}				//RoboticsAnimatSim

