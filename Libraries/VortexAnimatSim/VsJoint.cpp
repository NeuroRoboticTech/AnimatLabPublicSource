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

	m_iCoordID = -1; //-1 if not used.
}

VsJoint::~VsJoint()
{
}

VsSimulator *VsJoint::GetVsSimulator()
{
	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpThisAB->GetSimulator());
	return lpVsSim;
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

//REFACTOR
void VsJoint::UpdatePositionAndRotationFromMatrix(osg::Matrix osgMT)
{
	LocalMatrix(osgMT);

	//Lets get the current world coordinates for this body part and then recalculate the 
	//new local position for the part and then finally reset its new local position.
	osg::Vec3 vL = osgMT.getTrans();
	CStdFPoint vLocal(vL.x(), vL.y(), vL.z());
	vLocal.ClearNearZero();
	m_lpThisMI->Position(vLocal, FALSE, TRUE, FALSE);
		
	//Now lets get the euler angle rotation
	Vx::VxReal44 vxTM;
	VxOSG::copyOsgMatrix_to_VxReal44(osgMT, vxTM);
	Vx::VxTransform vTrans(vxTM);
	Vx::VxReal3 vEuler;
	vTrans.getRotationEulerAngles(vEuler);
	CStdFPoint vRot(vEuler[0], vEuler[1] ,vEuler[2]);
	vRot.ClearNearZero();
	m_lpThisMI->Rotation(vRot, TRUE, FALSE);

	if(m_osgDragger.valid())
		m_osgDragger->SetupMatrix();

	//Test the matrix to make sure they match. I will probably get rid of this code after full testing.
	osg::Matrix osgTest = SetupMatrix(vLocal, vRot);
	if(!OsgMatricesEqual(osgTest, m_osgLocalMatrix))
		THROW_ERROR(Vs_Err_lUpdateMatricesDoNotMatch, Vs_Err_strUpdateMatricesDoNotMatch);
}

	}			// Environment
}				//VortexAnimatSim