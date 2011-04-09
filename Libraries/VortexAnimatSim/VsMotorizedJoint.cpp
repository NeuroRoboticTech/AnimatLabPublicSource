/**
\file	VsMotorizedJoint.cpp

\brief	Implements the vs motorized joint class.
**/

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsSimulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{

VsMotorizedJoint::VsMotorizedJoint()
{
	m_lpThisMotorJoint = NULL;
	m_bMotorOn = FALSE;
}

VsMotorizedJoint::~VsMotorizedJoint()
{
}

//If this is a servo motor then the "velocity" signal is not really a velocity signal in this case. 
//It is the desired position and we must convert it to the velocity needed to reach and maintian that position.
void VsMotorizedJoint::CalculateServoVelocity()
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


void VsMotorizedJoint::Physics_SetVelocityToDesired()
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
		if( fabs(m_lpThisJoint->JointVelocity() - fltSetVelocity) > 1e-4)
		{
			if(fabs(fltSetVelocity) > 1e-4 && m_vxJoint)
				m_vxJoint->setMotorDesiredVelocity(m_iCoordID, fltSetVelocity);
			else
				m_lpThisMotorJoint->EnableLock(TRUE, m_lpThisJoint->JointPosition(), fltMaxForce);
		}

		m_lpThisMotorJoint->PrevVelocity(fltSetVelocity);
	}
}

void VsMotorizedJoint::Physics_EnableLock(BOOL bOn, float fltPosition, float fltMaxLockForce)
{
	if (m_vxJoint)
	{ 		
		if(bOn)
		{
			//set the lock parameters
			m_vxJoint->setLockParameters(m_iCoordID, fltPosition, -fltMaxLockForce, fltMaxLockForce);
			//turn on the lock (disabling motorized or free mode)
			m_vxJoint->setControl(m_iCoordID, VxConstraint::CoordinateControlEnum::kControlLocked);
		}
		else if (m_bMotorOn)
			Physics_EnableMotor(TRUE, 0, fltMaxLockForce);
		else
			m_vxJoint->setControl(m_iCoordID, VxConstraint::CoordinateControlEnum::kControlFree);
	}
}

void VsMotorizedJoint::Physics_EnableMotor(BOOL bOn, float fltDesiredVelocity, float fltMaxForce)
{
	if (m_vxJoint)
	{   
		if(bOn)
		{
			if(m_vxJoint->getControl(m_iCoordID) != Vx::VxConstraint::CoordinateControlEnum::kControlMotorized)
				m_vxJoint->setControl(m_iCoordID, Vx::VxConstraint::CoordinateControlEnum::kControlMotorized);

			m_vxJoint->setMotorDesiredVelocity(m_iCoordID, fltDesiredVelocity);

			//DWC Need to fix the stuff to set the min/max force for motor. Does not work right now.
			//m_vxJoint->setMotorParameters(m_iCoordID, fltDesiredVelocity, -fltMaxForce, fltMaxForce);
			//turn on the motor (disabling lock or free mode)
		}
		else
			m_vxJoint->setControl(m_iCoordID, Vx::VxConstraint::CoordinateControlEnum::kControlFree);

		m_bMotorOn = bOn;
	}
}

void VsMotorizedJoint::Physics_MaxForce(float fltVal)
{
	if(m_vxJoint)
		m_vxJoint->setMotorMaximumForce(m_iCoordID, fltVal);
}

	}			// Environment
}				//VortexAnimatSim