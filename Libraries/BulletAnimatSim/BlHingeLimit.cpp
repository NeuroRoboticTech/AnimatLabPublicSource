
#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlHinge.h"
#include "BlHingeLimit.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

BlHingeLimit::BlHingeLimit()
{
    OsgHingeLimit::ConstraintLimit(this);
}

BlHingeLimit::~BlHingeLimit()
{
}

void BlHingeLimit::Alpha(float fltA)
{
	m_vColor.a(fltA);

    OsgHingeLimit::LimitAlpha(fltA);
}

void BlHingeLimit::SetLimitPos()
{
	Hinge *lpHinge = dynamic_cast<Hinge *>(m_lpJoint);

    if(lpHinge)
        OsgHingeLimit::SetLimitPos(lpHinge->CylinderHeight());

	//Set the limit on the physics hinge object.
	SetLimitValues();
}

void BlHingeLimit::SetLimitValues()
{
	BlHinge *lpHinge = dynamic_cast<BlHinge *>(m_lpJoint);

    if(lpHinge)
        lpHinge->SetLimitValues();
}

void BlHingeLimit::DeleteGraphics()
{
    OsgHingeLimit::DeleteLimitGraphics();
}

void BlHingeLimit::SetupGraphics()
{
	//The parent osg object for the joint is actually the child rigid body object.
	Hinge *lpHinge = dynamic_cast<Hinge *>(m_lpJoint);

	if(lpHinge)
        OsgHingeLimit::SetupLimitGraphics(lpHinge->FlapWidth(), lpHinge->CylinderHeight());
}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
