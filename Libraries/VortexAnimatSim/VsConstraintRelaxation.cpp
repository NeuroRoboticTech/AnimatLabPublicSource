// VsConstraintRelaxation.cpp: implementation of the VsConstraintRelaxation class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsConstraintRelaxation.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsConstraintRelaxation::VsConstraintRelaxation()
{
}

VsConstraintRelaxation::~VsConstraintRelaxation()
{
}

void VsConstraintRelaxation::Initialize()
{
	ConstraintRelaxation::Initialize();

	SetRelaxationProperties();
}

void VsConstraintRelaxation::SetRelaxationProperties()
{
    if(m_lpSim && m_lpNode)
    {
        VsJoint *lpJoint = dynamic_cast<VsJoint *>(m_lpNode);
        if(lpJoint)
        {
            Vx::VxConstraint *vxConstraint = lpJoint->Constraint();

            if(vxConstraint)
               vxConstraint->setRelaxationParameters(m_iCoordinateID, m_fltStiffness, m_fltDamping, m_fltLoss, m_bEnabled);
        }
    }
}

	}			// Visualization
}				//VortexAnimatSim
