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
	m_btJoint = NULL;

    m_lpVsParent = NULL;
	m_lpVsChild = NULL;
    m_lpVsSim = NULL;

    m_fltPrevJointPos = 0;
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
    if(m_btJoint)
        return true;
    else
        return false;
}

void BlJoint::UpdatePosition()
{	
    //FIX PHYSICS
	//Vx::VxReal3 vPos;
	//m_btJoint->getPartAttachmentPosition(0, vPos);

	//UpdateWorldMatrix();
	//m_lpThisMI->AbsolutePosition(vPos[0], vPos[1], vPos[2]);
}

void BlJoint::Physics_CollectData()
{
	if(m_lpThisJoint && m_btJoint && m_lpThisJoint->GetSimulator())
	{
		UpdatePosition();

		float fltDistanceUnits = m_lpThisAB->GetSimulator()->DistanceUnits();
		float fltMassUnits = m_lpThisAB->GetSimulator()->MassUnits();

        float fltCurrentJointPos = GetCurrentBtPosition();

		if(!m_lpThisJoint->UsesRadians())
		{
            fltCurrentJointPos * fltDistanceUnits;
			//m_lpThisJoint->JointForce(m_btJoint->getCoordinateForce(m_iCoordID) * fltMassUnits * fltDistanceUnits);
		}

        float fltJointVel = (fltCurrentJointPos - m_fltPrevJointPos)/(m_lpThisJoint->GetSimulator()->PhysicsTimeStep());

        m_fltPrevJointPos = fltCurrentJointPos;
		m_lpThisJoint->JointPosition(fltCurrentJointPos); 
		m_lpThisJoint->JointVelocity(fltJointVel);

        //FIX PHYSICS
		//m_lpThisJoint->JointForce(m_btJoint->getCoordinateForce(m_iCoordID) * fltMassUnits * fltDistanceUnits * fltDistanceUnits);
    }
}

void BlJoint::Physics_ResetSimulation()
{
    //FIX PHYSICS
 //   if(m_btJoint)
	//{
	//	m_btJoint->resetDynamics();
	//	UpdatePosition();
 //       OsgJoint::Physics_ResetSimulation();
	//}
}

    }			// Environment
}				//BulletAnimatSim