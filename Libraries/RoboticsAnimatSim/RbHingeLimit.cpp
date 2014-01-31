
#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbHinge.h"
#include "RbHingeLimit.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

RbHingeLimit::RbHingeLimit()
{
}

RbHingeLimit::~RbHingeLimit()
{
}

void RbHingeLimit::SetLimitPos()
{
	//Hinge *lpHinge = dynamic_cast<Hinge *>(m_lpJoint);

 //   if(lpHinge)
 //       RbHingeLimit::SetLimitPos(lpHinge->CylinderHeight());

	////Set the limit on the physics hinge object.
	//SetLimitValues();
}

void RbHingeLimit::SetLimitValues()
{
	RbHinge *lpHinge = dynamic_cast<RbHinge *>(m_lpJoint);

	//Do not want to call this repeatedly if we are the flap limit.
    if(lpHinge && !m_bIsShowPosition)
        lpHinge->SetLimitValues();
}

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
