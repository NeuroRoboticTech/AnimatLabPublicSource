// RbConstraintRelaxation.cpp: implementation of the RbConstraintRelaxation class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbConstraintRelaxation.h"
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

RbConstraintRelaxation::RbConstraintRelaxation()
{
}

RbConstraintRelaxation::~RbConstraintRelaxation()
{
}

void RbConstraintRelaxation::Initialize()
{
	ConstraintRelaxation::Initialize();

	SetRelaxationProperties();
}

void RbConstraintRelaxation::SetRelaxationProperties()
{
}

	}			// Visualization
}				//RoboticsAnimatSim
