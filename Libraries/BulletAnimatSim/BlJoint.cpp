// BlJoint.cpp: implementation of the BlJoint class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
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

BlJoint::BlJoint()
{
	m_vxJoint = NULL;
	m_lpVsParent = NULL;
	m_lpVsChild = NULL;
    m_lpVsSim = NULL;

	m_iCoordID = -1; //-1 if not used.
}

BlJoint::~BlJoint()
{
}

BlSimulator *BlJoint::GetBlSimulator()
{
    if(!m_lpVsSim)
    {
    	m_lpVsSim = dynamic_cast<BlSimulator *>(m_lpThisAB->GetSimulator());
	    if(!m_lpThisVsMI)
		    THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpVsSim, " + m_lpThisAB->Name());
    }
	return m_lpVsSim;
}

bool BlJoint::Physics_IsDefined()
{
    if(m_vxJoint)
        return true;
    else
        return false;
}

void BlJoint::UpdatePosition()
{	
	Vx::VxReal3 vPos;
	m_vxJoint->getPartAttachmentPosition(0, vPos);

	UpdateWorldMatrix();
	m_lpThisMI->AbsolutePosition(vPos[0], vPos[1], vPos[2]);
}

void BlJoint::Physics_CollectData()
{
	if(m_lpThisJoint && m_vxJoint)
	{
		UpdatePosition();

		//Only attempt to make these calls if the coordinate ID is a valid number.
		if(m_iCoordID >= 0)
		{
			float fltDistanceUnits = m_lpThisAB->GetSimulator()->DistanceUnits();
			float fltMassUnits = m_lpThisAB->GetSimulator()->MassUnits();

			if(m_vxJoint->isAngular(m_iCoordID) == true)
			{
				m_lpThisJoint->JointPosition(m_vxJoint->getCoordinateCurrentPosition (m_iCoordID)); 
				m_lpThisJoint->JointVelocity(m_vxJoint->getCoordinateVelocity (m_iCoordID));
				m_lpThisJoint->JointForce(m_vxJoint->getCoordinateForce(m_iCoordID) * fltMassUnits * fltDistanceUnits * fltDistanceUnits);
			}
			else
			{
				m_lpThisJoint->JointPosition(m_vxJoint->getCoordinateCurrentPosition (m_iCoordID) * fltDistanceUnits); 
				m_lpThisJoint->JointVelocity(m_vxJoint->getCoordinateVelocity(m_iCoordID) * fltDistanceUnits);
				m_lpThisJoint->JointForce(m_vxJoint->getCoordinateForce(m_iCoordID) * fltMassUnits * fltDistanceUnits);
			}
		}
	}
}

void BlJoint::Physics_ResetSimulation()
{
	if(m_vxJoint)
	{
		m_vxJoint->resetDynamics();
		UpdatePosition();
        OsgJoint::Physics_ResetSimulation();
	}
}

    }			// Environment
}				//BulletAnimatSim