
#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
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
    //FIX PHYSICS
	//m_vxHinge = NULL;
    OsgHingeLimit::ConstraintLimit(this);
}

BlHingeLimit::~BlHingeLimit()
{
}

//FIX PHYSICS
//void BlHingeLimit::HingeRef(Vx::VxHinge *vxHinge)
//{
//	m_vxHinge = vxHinge;
//}

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
    //FIX PHYSICS
	//if(m_vxHinge)
	//{
	//	if(m_bIsLowerLimit)
	//		m_vxHinge->setLowerLimit(m_vxHinge->kAngularCoordinate, m_fltLimitPos, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
	//	else
	//		m_vxHinge->setUpperLimit(m_vxHinge->kAngularCoordinate, m_fltLimitPos, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
	//}
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
