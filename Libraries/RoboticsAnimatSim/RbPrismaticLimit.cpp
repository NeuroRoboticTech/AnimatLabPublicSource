
#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbPrismaticLimit.h"
#include "RbPrismatic.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

RbPrismaticLimit::RbPrismaticLimit()
{
    //OsgPrismaticLimit::SetConstraintLimit(this);
}

RbPrismaticLimit::~RbPrismaticLimit()
{
}

void RbPrismaticLimit::SetLimitPos()
{
	//CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 

	//Prismatic *lpPrismatic = dynamic_cast<Prismatic *>(m_lpJoint);
 //   if(lpPrismatic)
 //       OsgPrismaticLimit::SetLimitPos(lpPrismatic->CylinderRadius());

	////Set the limit on the physics Prismatic object.
	//SetLimitValues();
}

void RbPrismaticLimit::SetLimitValues()
{
	RbPrismatic *lpPrismatic = dynamic_cast<RbPrismatic *>(m_lpJoint);

	//Do not want to call this repeatedly if we are the flap limit.
    if(lpPrismatic && !m_bIsShowPosition)
        lpPrismatic->SetLimitValues();
}

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
