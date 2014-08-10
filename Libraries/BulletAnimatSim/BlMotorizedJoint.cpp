/**
\file	BlMotorizedJoint.cpp

\brief	Implements the vs motorized joint class.
**/

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{

BlMotorizedJoint::BlMotorizedJoint()
{
	m_lpThisMotorJoint = NULL;
	m_bMotorOn = false;
    m_bJointLocked = false;
    m_fltPredictedPos = 0;
    m_fltNextPredictedPos = 0;
}

BlMotorizedJoint::~BlMotorizedJoint()
{
}

void BlMotorizedJoint::SetThisPointers()
{
	BlJoint::SetThisPointers();

	m_lpThisMotorJoint = dynamic_cast<MotorizedJoint *>(this);
	if(!m_lpThisMotorJoint)
		THROW_TEXT_ERROR(Bl_Err_lThisPointerNotDefined, Bl_Err_strThisPointerNotDefined, "m_lpThisMotorJoint, " + m_lpThisAB->Name());

	m_lpThisMotorJoint->PhysicsMotorJoint(this);
}

//If this is a servo motor then the "velocity" signal is not really a velocity signal in this case. 
//It is the desired position and we must convert it to the velocity needed to reach and maintian that position.
void BlMotorizedJoint::CalculateServoVelocity()
{
	if(!m_btJoint)
		return;

	float fltTargetPos = m_lpThisJoint->GetPositionWithinLimits(m_lpThisMotorJoint->DesiredPosition());
	float fltError = fltTargetPos - m_lpThisJoint->JointPosition();
	m_lpThisMotorJoint->SetPosition(fltTargetPos);

	//Test Code
	int i=5;
	if(Std_ToLower(m_lpThisMotorJoint->ID()) == "5c9f7a20-c7e0-44a9-b97f-9de1132363ad") // && fabs(fltTargetPos) > 0  && GetSimulator()->Time() >= 2.5
		i=6;

	AnimatSim::Environment::eJointMotorType MotorType = m_lpThisMotorJoint->MotorType();
	if(MotorType == eJointMotorType::PositionControl || (MotorType == eJointMotorType::PositionVelocityControl && m_lpThisMotorJoint->ReachedSetPosition()) )
	{
		//Lock this joint position.
		m_lpThisMotorJoint->DesiredVelocity(0); 
	}
	else
	{
		//If we set the desired velocity and position then make sure the desired velocity is in the right direction
		float fltDesiredVel = fabs(m_lpThisMotorJoint->DesiredVelocity()) * Std_Sign(fltError);

		float fltPosError = fabs(fltError);
		if(fltPosError > 0 && fltPosError < 0.05)
		{
			float fltDesiredVel2 = fltDesiredVel * (fabs(fltError)*m_lpThisMotorJoint->ServoGain());
			//Only do this if the new desired velocity is less than the original one to slow it down. Never speed it up.
			if(fabs(fltDesiredVel2) <= fabs(fltDesiredVel))
				fltDesiredVel = fltDesiredVel2;
		}

		m_lpThisMotorJoint->DesiredVelocity(fltDesiredVel);

		if(fabs(fltError) < 1e-4)
			m_lpThisMotorJoint->ReachedSetPosition(true);
	}
}

void BlMotorizedJoint::Physics_SetVelocityToDesired()
{
	if(m_lpThisMotorJoint->EnableMotor())
	{		
		AnimatSim::Environment::eJointMotorType MotorType = m_lpThisMotorJoint->MotorType();
		if(MotorType == eJointMotorType::PositionControl ||MotorType == eJointMotorType::PositionVelocityControl)
			CalculateServoVelocity();
		
		float fltDesiredVel = m_lpThisMotorJoint->DesiredVelocity();
		float fltMaxVel = m_lpThisMotorJoint->MaxVelocity();
		float fltMaxForce = m_lpThisMotorJoint->MaxForce();

		if(fltDesiredVel>fltMaxVel)
			fltDesiredVel = fltMaxVel;

		if(fltDesiredVel < -fltMaxVel)
			fltDesiredVel = -fltMaxVel;

		float fltSetVelocity = fltDesiredVel;

		m_lpThisMotorJoint->SetVelocity(fltSetVelocity);
		m_lpThisMotorJoint->DesiredVelocity(0);
		m_lpThisMotorJoint->DesiredPosition(0);

        float fltHalfPercVel = fabs(fltSetVelocity * 0.01);
        //If the actual velocity matches the set velocity within 1% then lets set the predicted position based on 
        //current position going forward. If we are not neear the set velocity then continue to use predicted positions.
        //if(fabs(fltSetVelocity - m_lpThisJoint->JointVelocity()) < fltHalfPercVel)
        //{
        //    m_fltPredictedPos = m_lpThisJoint->JointPosition();
        //    m_fltNextPredictedPos = m_fltPredictedPos +  (fltSetVelocity*m_lpThisAB->GetSimulator()->PhysicsTimeStep());
        //}
        //else
        {
            m_fltPredictedPos = m_fltNextPredictedPos;
            m_fltNextPredictedPos = m_fltPredictedPos +  (fltSetVelocity*m_lpThisAB->GetSimulator()->PhysicsTimeStep());
        }

        float fltJointVel = m_lpThisJoint->JointVelocity();

		if(!m_lpThisJoint->UsesRadians())
			fltJointVel *= m_lpThisAB->GetSimulator()->InverseDistanceUnits();;

		float fltVelDiff = fabs(fltJointVel - fltSetVelocity);

		//Only do anything if the velocity value has changed
        if(m_btJoint && fltVelDiff > 1e-4)
		{
			if(fabs(fltSetVelocity) > 1e-4 && m_btJoint)
				Physics_EnableMotor(true, fltSetVelocity, fltMaxForce, false);
            else if(!m_bJointLocked)
                Physics_EnableLock(true, GetCurrentBtPosition(), fltMaxForce);
		}

		m_lpThisMotorJoint->PrevVelocity(fltSetVelocity);
	}
}

void BlMotorizedJoint::Physics_CollectExtraData()
{
    OsgSimulator *lpSim = GetOsgSimulator();
	if(m_btJoint && lpSim && m_lpThisMotorJoint)
	{
	    float fDisUnits = m_lpThisAB->GetSimulator()->DistanceUnits();
	    float fMassUnits = m_lpThisAB->GetSimulator()->MassUnits();
        CStdFPoint vVal, vAssist;

        float fltRatio = fMassUnits * fDisUnits;
        vAssist = m_lpThisMotorJoint->MotorAssistForceToAReport();
		vVal[0] = (m_btJointFeedback.m_appliedForceBodyA[0] * fltRatio) + vAssist[0];
		vVal[1] = (m_btJointFeedback.m_appliedForceBodyA[1] * fltRatio) + vAssist[1];
		vVal[2] = (m_btJointFeedback.m_appliedForceBodyA[2] * fltRatio) + vAssist[2];
        m_lpThisMotorJoint->MotorForceToA(vVal);

        vAssist = m_lpThisMotorJoint->MotorAssistForceToBReport();
		vVal[0] = (m_btJointFeedback.m_appliedForceBodyB[0] * fltRatio) + vAssist[0];
		vVal[1] = (m_btJointFeedback.m_appliedForceBodyB[1] * fltRatio) + vAssist[0];
		vVal[2] = (m_btJointFeedback.m_appliedForceBodyB[2] * fltRatio) + vAssist[0];
        m_lpThisMotorJoint->MotorForceToB(vVal);

        fltRatio = fMassUnits * fDisUnits * fDisUnits;
        vAssist = m_lpThisMotorJoint->MotorAssistTorqueToAReport();
		vVal[0] = (m_btJointFeedback.m_appliedTorqueBodyA[0] * fltRatio) + vAssist[0];
		vVal[1] = (m_btJointFeedback.m_appliedTorqueBodyA[1] * fltRatio) + vAssist[0];
		vVal[2] = (m_btJointFeedback.m_appliedTorqueBodyA[2] * fltRatio) + vAssist[0];
        m_lpThisMotorJoint->MotorTorqueToA(vVal);

        vAssist = m_lpThisMotorJoint->MotorAssistTorqueToBReport();
		vVal[0] = (m_btJointFeedback.m_appliedTorqueBodyB[0] * fltRatio) + vAssist[0];
		vVal[1] = (m_btJointFeedback.m_appliedTorqueBodyB[1] * fltRatio) + vAssist[0];
		vVal[2] = (m_btJointFeedback.m_appliedTorqueBodyB[2] * fltRatio) + vAssist[0];
        m_lpThisMotorJoint->MotorTorqueToB(vVal);
	}
}


	}			// Environment
}				//BulletAnimatSim