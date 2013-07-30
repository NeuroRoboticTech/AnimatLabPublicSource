// VsJoint.cpp: implementation of the VsJoint class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
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

VsJoint::VsJoint()
{
	m_vxJoint = NULL;
	m_lpVsParent = NULL;
	m_lpVsChild = NULL;
    m_lpVsSim = NULL;

	m_iCoordID = -1; //-1 if not used.
}

VsJoint::~VsJoint()
{
}

VsSimulator *VsJoint::GetVsSimulator()
{
    if(!m_lpVsSim)
    {
    	m_lpVsSim = dynamic_cast<VsSimulator *>(m_lpThisAB->GetSimulator());
	    if(!m_lpThisVsMI)
		    THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpVsSim, " + m_lpThisAB->Name());
    }
	return m_lpVsSim;
}

BOOL VsJoint::Physics_IsDefined()
{
    if(m_vxJoint)
        return TRUE;
    else
        return FALSE;
}

void VsJoint::UpdatePosition()
{	
	Vx::VxReal3 vPos;
	m_vxJoint->getPartAttachmentPosition(0, vPos);

	UpdateWorldMatrix();
	m_lpThisMI->AbsolutePosition(vPos[0], vPos[1], vPos[2]);
}

void VsJoint::Physics_CollectData()
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

void VsJoint::Physics_ResetSimulation()
{
	if(m_vxJoint)
	{
		m_vxJoint->resetDynamics();
		UpdatePosition();
        OsgJoint::Physics_ResetSimulation();
	}
}

    }			// Environment
}				//VortexAnimatSim