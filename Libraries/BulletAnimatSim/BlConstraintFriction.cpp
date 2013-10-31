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
            lpJoint->SetConstraintFriction();
    }
}

	}			// Visualization
}				//BulletAnimatSim
