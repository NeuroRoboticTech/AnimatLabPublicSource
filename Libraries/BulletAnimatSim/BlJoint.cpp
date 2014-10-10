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
    m_bt6DofJoint = NULL;

    m_lpVsParent = NULL;
	m_lpVsChild = NULL;
    m_lpVsSim = NULL;
    m_lpBlParent = NULL;
    m_lpBlChild = NULL;
    m_btParent = NULL;
    m_btChild = NULL;

    m_fltPrevBtJointPos = 0;
    m_fltPrevJointPos = 0;

    for(int iIdx=0; iIdx<6; iIdx++)
        m_aryBlRelaxations[iIdx] = NULL;
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

void BlJoint::InitBaseJointPointers(RigidBody *lpParent, RigidBody *lpChild, ConstraintRelaxation **aryRelaxations, int iDisallowSpringIndex)
{
	if(!lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	m_lpBlParent = dynamic_cast<BlRigidBody *>(lpParent);
	if(!m_lpBlParent)
		THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

	m_lpBlChild = dynamic_cast<BlRigidBody *>(lpChild);
	if(!m_lpBlChild)
		THROW_ERROR(Bl_Err_lUnableToConvertToBlRigidBody, Bl_Err_strUnableToConvertToBlRigidBody);

    m_btParent = m_lpBlParent->Part();
    m_btChild = m_lpBlChild->Part();

    for(int iIdx=0; iIdx<6; iIdx++)
    {
        if(aryRelaxations[iIdx])
        {
            BlConstraintRelaxation *lpRelax = dynamic_cast<BlConstraintRelaxation *>(aryRelaxations[iIdx]);
            m_aryBlRelaxations[iIdx] = lpRelax;
        }
        else
            m_aryBlRelaxations[iIdx] = NULL;
    }

    if(iDisallowSpringIndex >= 0 && iDisallowSpringIndex <6 && m_aryBlRelaxations[iDisallowSpringIndex])
        m_aryBlRelaxations[iDisallowSpringIndex]->DisallowSpringEnable(true);
}


void BlJoint::SetLimitValues()
{
    if(m_bt6DofJoint)
    {
        GetLimitsFromRelaxations(m_vLowerLinear, m_vUpperLinear, m_vLowerAngular, m_vUpperAngular);

        m_bt6DofJoint->setLinearLowerLimit(m_vLowerLinear);
        m_bt6DofJoint->setLinearUpperLimit(m_vUpperLinear);
		m_bt6DofJoint->setAngularLowerLimit(m_vLowerAngular);
		m_bt6DofJoint->setAngularUpperLimit(m_vUpperAngular);
    }
}

void BlJoint::DeletePhysics(bool bIncludeChildren)
{
	if(!m_btJoint)
		return;

	if(GetBlSimulator() && GetBlSimulator()->DynamicsWorld())
	{
		GetBlSimulator()->DynamicsWorld()->removeConstraint(m_btJoint);
		delete m_btJoint;
	}

	m_btJoint = NULL;
}

void BlJoint::Physics_CollectData()
{
    OsgJoint::Physics_CollectData();

	if(m_lpThisJoint && m_btJoint && m_lpThisJoint->GetSimulator())
	{
		float fltDistanceUnits = m_lpThisAB->GetSimulator()->DistanceUnits();
		float fltMassUnits = m_lpThisAB->GetSimulator()->MassUnits();

        float fltCurrentJointPos = GetCurrentBtPosition();

        //if(GetSimulator()->Time() > 5.64)
        //    fltCurrentJointPos = fltCurrentJointPos;

        //If this joint uses radians then at the +/- PI boundaries the sign can flip. 
        //So we need to keep an internal representation of its position and update this with a delta of the change in position
        // that the physics engine is telling us. This allows the joint position to roll-over past 2PI so we have a steady velocity
        // and position without discontinuties. It also allows us to see how many times the joint has finished a complete revolution.
        if(m_lpThisJoint->UsesRadians())
        {
            int iPrevPosSign = Std_Sign(m_fltPrevBtJointPos);
            float fltDelta = 0;
            if(Std_Sign(fltCurrentJointPos) != iPrevPosSign && fabs(fltCurrentJointPos - m_fltPrevBtJointPos) > 0.1)
                fltDelta = fltCurrentJointPos - (-m_fltPrevBtJointPos);
            else
                fltDelta = fltCurrentJointPos - m_fltPrevBtJointPos;

           fltCurrentJointPos =  m_lpThisJoint->JointPosition() + fltDelta;
        }

		if(!m_lpThisJoint->UsesRadians())
            fltCurrentJointPos *= fltDistanceUnits;

        float fltJointVel = (fltCurrentJointPos - m_fltPrevJointPos)/(m_lpThisJoint->GetSimulator()->PhysicsTimeStep());

        m_fltPrevBtJointPos = GetCurrentBtPosition();
        m_fltPrevJointPos = fltCurrentJointPos;
		m_lpThisJoint->JointPosition(fltCurrentJointPos); 
		m_lpThisJoint->JointVelocity(fltJointVel);
    }
}

void BlJoint::Physics_ResetSimulation()
{
    if(m_btJoint)
	{
        m_lpThisJoint->WakeDynamics();
        m_fltPrevBtJointPos = 0;
        m_fltPrevJointPos = 0;
        OsgJoint::Physics_ResetSimulation();
	}
}

void BlJoint::CalculateRelativeJointMatrices(btTransform &mtJointRelToParent, btTransform &mtJointRelToChild)
{
	CStdFPoint vRot(0, 0, 0);
	CalculateRelativeJointMatrices(vRot, mtJointRelToParent,mtJointRelToChild);
}

void BlJoint::CalculateRelativeJointMatrices(CStdFPoint vAdditionalRot, btTransform &mtJointRelToParent, btTransform &mtJointRelToChild)
{
    osg::Matrix mtParent = GetParentPhysicsWorldMatrix();
    osg::Matrix mtChild = GetChildWorldMatrix();
    CStdFPoint vPos1 = m_lpThisMI->Position();
    CStdFPoint vRot1 = m_lpThisMI->Rotation() + vAdditionalRot;
    osg::Matrix osgJointRelChild = SetupMatrix(vPos1, vRot1);

    osg::Matrix mtJointMTFromChild = osgJointRelChild * mtChild;
    osg::Matrix mtLocalRelToParent = mtJointMTFromChild * osg::Matrix::inverse(mtParent);

    osg::Matrix mtChildCom = GetChildComMatrix(true);
    osg::Matrix mtJointRelChild = osgJointRelChild * mtChildCom;

    mtJointRelToParent = osgbCollision::asBtTransform(mtLocalRelToParent);
    mtJointRelToChild = osgbCollision::asBtTransform(mtJointRelChild);
}

void BlJoint::GetLimitsFromRelaxations(btVector3 &vLowerLinear, btVector3 &UpperLinear, btVector3 &vLowerAngular, btVector3 &vUpperAngular)
{
    btVector3 vLimits;

    for(int iIdx=0, iBlIdx=0; iIdx<3; iIdx++, iBlIdx++)
    {
        if(m_aryBlRelaxations[iBlIdx] && m_aryBlRelaxations[iBlIdx]->Enabled())
        {
            vLowerLinear[iIdx] = m_aryBlRelaxations[iBlIdx]->MaxLimit();
            UpperLinear[iIdx] = m_aryBlRelaxations[iBlIdx]->MinLimit();
        }
        else
        {
            vLowerLinear[iIdx] = 0;
            UpperLinear[iIdx] = 0;
        }
    }

    for(int iIdx=0, iBlIdx=3; iIdx<3; iIdx++, iBlIdx++)
    {
        if(m_aryBlRelaxations[iBlIdx] && m_aryBlRelaxations[iBlIdx]->Enabled())
        {
            vLowerAngular[iIdx] = m_aryBlRelaxations[iBlIdx]->MaxLimit();
            vUpperAngular[iIdx] = m_aryBlRelaxations[iBlIdx]->MinLimit();
        }
        else
        {
            vLowerAngular[iIdx] = 0;
            vUpperAngular[iIdx] = 0;
        }
    }
}


    }			// Environment
}				//BulletAnimatSim