// RbJoint.cpp: implementation of the RbJoint class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
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

RbJoint::RbJoint()
{
    m_lpRbParent = NULL;
	m_lpRbChild = NULL;
    m_lpRbSim = NULL;
    m_lpRbParent = NULL;
    m_lpRbChild = NULL;

    m_fltPrevBtJointPos = 0;
    m_fltPrevJointPos = 0;
}

RbJoint::~RbJoint()
{
}

RbSimulator *RbJoint::GetRbSimulator()
{
    if(!m_lpRbSim)
    {
    	m_lpRbSim = dynamic_cast<RbSimulator *>(m_lpThisAB->GetSimulator());
	    if(!m_lpThisRbMI)
		    THROW_TEXT_ERROR(Rb_Err_lThisPointerNotDefined, Rb_Err_strThisPointerNotDefined, "m_lpRbSim, " + m_lpThisAB->Name());
    }
	return m_lpRbSim;
}
void RbJoint::Physics_CollectData()
{
    RbBody::Physics_CollectData();

}

void RbJoint::Physics_ResetSimulation()
{
    m_fltPrevBtJointPos = 0;
    m_fltPrevJointPos = 0;
    RbBody::Physics_ResetSimulation();
}


    }			// Environment
}				//RoboticsAnimatSim