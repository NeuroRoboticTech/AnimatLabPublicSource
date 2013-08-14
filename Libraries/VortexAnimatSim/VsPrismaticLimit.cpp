
#include "StdAfx.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsPrismaticLimit.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

VsPrismaticLimit::VsPrismaticLimit()
{
	m_vxPrismatic = NULL;
    OsgPrismaticLimit::ConstraintLimit(this);
}

VsPrismaticLimit::~VsPrismaticLimit()
{
}

void VsPrismaticLimit::PrismaticRef(Vx::VxPrismatic *vxPrismatic)
{
	m_vxPrismatic = vxPrismatic;
}

void VsPrismaticLimit::Alpha(float fltA)
{
	m_vColor.a(fltA);

    OsgPrismaticLimit::LimitAlpha(fltA);
}

void VsPrismaticLimit::SetLimitPos()
{
	CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 

	Prismatic *lpPrismatic = dynamic_cast<Prismatic *>(m_lpJoint);
    if(lpPrismatic)
        OsgPrismaticLimit::SetLimitPos(lpPrismatic->CylinderRadius());

	//Set the limit on the physics Prismatic object.
	SetLimitValues();
}

void VsPrismaticLimit::SetLimitValues()
{
	if(m_vxPrismatic)
	{
		if(m_bIsLowerLimit)
			m_vxPrismatic->setLowerLimit(m_vxPrismatic->kLinearCoordinate, m_fltLimitPos, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
		else
			m_vxPrismatic->setUpperLimit(m_vxPrismatic->kLinearCoordinate, m_fltLimitPos, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
	}
}

void VsPrismaticLimit::DeleteGraphics()
{
    OsgPrismaticLimit::DeleteLimitGraphics();
}

void VsPrismaticLimit::SetupGraphics()
{
	//The parent osg object for the joint is actually the child rigid body object.
	Prismatic *lpPrismatic = dynamic_cast<Prismatic *>(m_lpJoint);

	if(lpPrismatic)
        OsgPrismaticLimit::SetupLimitGraphics(lpPrismatic->BoxSize(), lpPrismatic->CylinderRadius());
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
