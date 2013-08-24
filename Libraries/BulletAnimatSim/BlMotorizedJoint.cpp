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
	if(!m_vxJoint)
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

		//Only do anything if the velocity value has changed
		if(m_vxJoint && fabs(m_lpThisJoint->JointVelocity() - fltSetVelocity) > 1e-4)
		{
			if(fabs(fltSetVelocity) > 1e-4 && m_vxJoint)
			{
				if(m_vxJoint->getControl(m_iCoordID) == Vx::VxConstraint::CoordinateControlEnum::kControlLocked)
					Physics_EnableMotor(true, fltSetVelocity, fltMaxForce);
				else
					m_vxJoint->setMotorDesiredVelocity(m_iCoordID, fltSetVelocity);
			}
			else
			{ 
				if(m_vxJoint->getControl(m_iCoordID) != Vx::VxConstraint::CoordinateControlEnum::kControlLocked)
					m_lpThisMotorJoint->EnableLock(true, m_vxJoint->getCoordinateCurrentPosition(m_iCoordID), fltMaxForce);
			}
		}

		m_lpThisMotorJoint->PrevVelocity(fltSetVelocity);
	}
}

void BlMotorizedJoint::Physics_EnableLock(bool bOn, float fltPosition, float fltMaxLockForce)
{
	if (m_vxJoint)
	{ 		
		if(bOn)
		{
			//set the lock parameters
			m_vxJoint->setLockParameters(m_iCoordID, fltPosition, fltMaxLockForce);
			//turn on the lock (disabling motorized or free mode)
			m_vxJoint->setControl(m_iCoordID, VxConstraint::CoordinateControlEnum::kControlLocked);
		}
		else if (m_bMotorOn)
			Physics_EnableMotor(true, 0, fltMaxLockForce);
		else
			m_vxJoint->setControl(m_iCoordID, VxConstraint::CoordinateControlEnum::kControlFree);
	}
}

void BlMotorizedJoint::Physics_EnableMotor(bool bOn, float fltDesiredVelocity, float fltMaxForce)
{
	if (m_vxJoint)
	{   
		if(bOn)
		{
			if(m_vxJoint->getControl(m_iCoordID) != Vx::VxConstraint::CoordinateControlEnum::kControlMotorized)
				m_vxJoint->setControl(m_iCoordID, Vx::VxConstraint::CoordinateControlEnum::kControlMotorized);

			m_vxJoint->setMotorMaximumForce(m_iCoordID, fltMaxForce);
			m_vxJoint->setMotorDesiredVelocity(m_iCoordID, fltDesiredVelocity);
		}
		else
			m_vxJoint->setControl(m_iCoordID, Vx::VxConstraint::CoordinateControlEnum::kControlFree);

		m_bMotorOn = bOn;
	}
}

void BlMotorizedJoint::Physics_MaxForce(float fltVal)
{
	if(m_vxJoint)
		m_vxJoint->setMotorMaximumForce(m_iCoordID, fltVal);
}

	}			// Environment
}				//BulletAnimatSim