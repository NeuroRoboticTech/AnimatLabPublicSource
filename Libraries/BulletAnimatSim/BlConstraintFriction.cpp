// BlConstraintFriction.cpp: implementation of the BlConstraintFriction class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlConstraintFriction.h"
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

BlConstraintFriction::BlConstraintFriction()
{
}

BlConstraintFriction::~BlConstraintFriction()
{
}

void BlConstraintFriction::Initialize()
{
	ConstraintFriction::Initialize();

	SetFrictionProperties();
}

void BlConstraintFriction::SetFrictionProperties()
{
    if(m_lpSim && m_lpNode)
    {
        BlJoint *lpJoint = dynamic_cast<BlJoint *>(m_lpNode);
        if(lpJoint)
        {
            Vx::VxConstraint *vxConstraint = lpJoint->Constraint();

            if(vxConstraint)
	        {
                VxConstraintFriction *vxCFriction = vxConstraint->getCoordinateFriction(0);

                if(vxCFriction)
                {
                    vxCFriction->setEnabled(m_bEnabled);
                    vxCFriction->setCoefficient(m_fltCoefficient);
                    vxCFriction->setMaxForce(m_fltMaxForce);
                    vxCFriction->setProportional(m_bProportional);
                    vxCFriction->setStaticFrictionScale(m_fltStaticFrictionScale);
                }
	        }

        }
    }
}

	}			// Visualization
}				//BulletAnimatSim
