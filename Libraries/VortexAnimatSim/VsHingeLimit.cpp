
#include "StdAfx.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsHingeLimit.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

VsHingeLimit::VsHingeLimit()
{
	m_vxHinge = NULL;
}

VsHingeLimit::~VsHingeLimit()
{
}

void VsHingeLimit::HingeRef(Vx::VxHinge *vxHinge)
{
	m_vxHinge = vxHinge;
}

void VsHingeLimit::Alpha(float fltA)
{
	m_vColor.a(fltA);

    OsgHingeLimit::LimitAlpha(fltA);
}

void VsHingeLimit::SetLimitPos()
{
	Hinge *lpHinge = dynamic_cast<Hinge *>(m_lpJoint);

    if(lpHinge)
    {
		float fltHeight = lpHinge->CylinderHeight();
        OsgHingeLimit::SetLimitPos(fltHeight, m_fltLimitPos);
	}

	//Set the limit on the physics hinge object.
	SetLimitValues();
}

void VsHingeLimit::SetLimitValues()
{
	if(m_vxHinge)
	{
		if(m_bIsLowerLimit)
			m_vxHinge->setLowerLimit(m_vxHinge->kAngularCoordinate, m_fltLimitPos, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
		else
			m_vxHinge->setUpperLimit(m_vxHinge->kAngularCoordinate, m_fltLimitPos, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
	}
}

void VsHingeLimit::DeleteGraphics()
{
    OsgHingeLimit::DeleteLimitGraphics();
}

void VsHingeLimit::SetupGraphics()
{
	//The parent osg object for the joint is actually the child rigid body object.
	Hinge *lpHinge = dynamic_cast<Hinge *>(m_lpJoint);

	if(lpHinge)
        OsgHingeLimit::SetupLimitGraphics(lpHinge->FlapWidth(), lpHinge->CylinderHeight(), m_fltLimitPos);
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
