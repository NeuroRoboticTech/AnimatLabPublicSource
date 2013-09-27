
#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlPrismaticLimit.h"
#include "BlPrismatic.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

BlPrismaticLimit::BlPrismaticLimit()
{
    OsgPrismaticLimit::SetConstraintLimit(this);
}

BlPrismaticLimit::~BlPrismaticLimit()
{
}

void BlPrismaticLimit::Alpha(float fltA)
{
	m_vColor.a(fltA);

    OsgPrismaticLimit::LimitAlpha(fltA);
}

void BlPrismaticLimit::SetLimitPos()
{
	CStdFPoint vPos(0, 0, 0), vRot(0, 0, 0); 

	Prismatic *lpPrismatic = dynamic_cast<Prismatic *>(m_lpJoint);
    if(lpPrismatic)
        OsgPrismaticLimit::SetLimitPos(lpPrismatic->CylinderRadius());

	//Set the limit on the physics Prismatic object.
	SetLimitValues();
}

void BlPrismaticLimit::SetLimitValues()
{
	BlPrismatic *lpPrismatic = dynamic_cast<BlPrismatic *>(m_lpJoint);

    if(lpPrismatic)
        lpPrismatic->SetLimitValues();
}

void BlPrismaticLimit::DeleteGraphics()
{
    OsgPrismaticLimit::DeleteLimitGraphics();
}

void BlPrismaticLimit::SetupGraphics()
{
	//The parent osg object for the joint is actually the child rigid body object.
	Prismatic *lpPrismatic = dynamic_cast<Prismatic *>(m_lpJoint);

	if(lpPrismatic)
        OsgPrismaticLimit::SetupLimitGraphics(lpPrismatic->BoxSize(), lpPrismatic->CylinderRadius());
}

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
