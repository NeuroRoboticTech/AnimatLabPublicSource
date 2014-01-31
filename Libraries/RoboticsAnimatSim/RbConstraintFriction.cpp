// RbConstraintFriction.cpp: implementation of the RbConstraintFriction class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbConstraintFriction.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbConstraintFriction::RbConstraintFriction()
{
}

RbConstraintFriction::~RbConstraintFriction()
{
}

void RbConstraintFriction::Initialize()
{
	ConstraintFriction::Initialize();
}

void RbConstraintFriction::SetFrictionProperties()
{
}

	}			// Visualization
}				//RoboticsAnimatSim
