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

	float fltTargetPos = m_lpThisJoint->GetPositionWithinLimits(m_lpThisMotorJoint->DesiredVelocity());
	float fltError = fltTargetPos - m_lpThisJoint->JointPosition();

	if(m_lpThisJoint->EnableLimits())
	{
		float fltProp = fltError / m_lpThisJoint->GetLimitRange();
		m_lpThisMotorJoint->DesiredVelocity(fltProp * m_lpThisMotorJoint->ServoGain()); 
	}
	else
		m_lpThisMotorJoint->DesiredVelocity(fltError * m_lpThisMotorJoint->MaxVelocity()); 
}

void BlMotorizedJoint::Physics_SetVelocityToDesired()
{
	if(m_lpThisMotorJoint->EnableMotor())
	{			
		if(m_lpThisMotorJoint->ServoMotor())
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

        float fltJointVel = m_lpThisJoint->JointVelocity();

		//Only do anything if the velocity value has changed
        if(m_btJoint && fabs(m_lpThisJoint->JointVelocity() - fltSetVelocity) > 1e-4)
		{
			if(fabs(fltSetVelocity) > 1e-4 && m_btJoint)
				Physics_EnableMotor(true, fltSetVelocity, fltMaxForce, false);
            else if(!m_bJointLocked)
                Physics_EnableLock(true, GetCurrentBtPosition(), fltMaxForce);
		}

		m_lpThisMotorJoint->PrevVelocity(fltSetVelocity);
	}
}


	}			// Environment
}				//BulletAnimatSim