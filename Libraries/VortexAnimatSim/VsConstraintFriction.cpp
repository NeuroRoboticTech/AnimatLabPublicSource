// VsConstraintFriction.cpp: implementation of the VsConstraintFriction class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsConstraintFriction.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsConstraintFriction::VsConstraintFriction()
{
}

VsConstraintFriction::~VsConstraintFriction()
{
}

void VsConstraintFriction::Initialize()
{
	ConstraintFriction::Initialize();

	SetFrictionProperties();
}

void VsConstraintFriction::SetFrictionProperties()
{
    if(m_lpSim && m_lpNode)
    {
        VsJoint *lpJoint = dynamic_cast<VsJoint *>(m_lpNode);
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
}				//VortexAnimatSim
