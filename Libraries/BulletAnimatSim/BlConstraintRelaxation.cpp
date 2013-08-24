// BlConstraintRelaxation.cpp: implementation of the BlConstraintRelaxation class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlConstraintRelaxation.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BlConstraintRelaxation::BlConstraintRelaxation()
{
}

BlConstraintRelaxation::~BlConstraintRelaxation()
{
}

void BlConstraintRelaxation::Initialize()
{
	ConstraintRelaxation::Initialize();

	SetRelaxationProperties();
}

void BlConstraintRelaxation::SetRelaxationProperties()
{
    if(m_lpSim && m_lpNode)
    {
        BlJoint *lpJoint = dynamic_cast<BlJoint *>(m_lpNode);
        if(lpJoint)
        {
            Vx::VxConstraint *vxConstraint = lpJoint->Constraint();

            if(vxConstraint)
               vxConstraint->setRelaxationParameters(m_iCoordinateID, m_fltStiffness, m_fltDamping, m_fltLoss, m_bEnabled);
        }
    }
}

	}			// Visualization
}				//BulletAnimatSim
