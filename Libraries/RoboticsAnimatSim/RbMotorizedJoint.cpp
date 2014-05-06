/**
\file	RbMotorizedJoint.cpp

\brief	Implements the vs motorized joint class.
**/

#include "StdAfx.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{

RbMotorizedJoint::RbMotorizedJoint()
{
	m_lpThisMotorJoint = NULL;
	m_bMotorOn = false;
    m_bJointLocked = false;
    m_fltPredictedPos = 0;
    m_fltNextPredictedPos = 0;
}

RbMotorizedJoint::~RbMotorizedJoint()
{
}

void RbMotorizedJoint::SetThisPointers()
{
	RbJoint::SetThisPointers();

	m_lpThisMotorJoint = dynamic_cast<MotorizedJoint *>(this);
	if(!m_lpThisMotorJoint)
		THROW_TEXT_ERROR(Rb_Err_lThisPointerNotDefined, Rb_Err_strThisPointerNotDefined, "m_lpThisMotorJoint, " + m_lpThisAB->Name());

	m_lpThisMotorJoint->PhysicsMotorJoint(this);
}

//If this is a servo motor then the "velocity" signal is not really a velocity signal in this case. 
//It is the desired position and we must convert it to the velocity needed to reach and maintian that position.
void RbMotorizedJoint::CalculateServoVelocity()
{
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

void RbMotorizedJoint::Physics_SetVelocityToDesired()
{
	if(m_lpThisMotorJoint->EnableMotor())
	{			
		if(m_lpThisMotorJoint->ServoMotor())
		{
			m_lpThisJoint->JointPosition(m_lpThisMotorJoint->DesiredVelocity());
			m_lpThisMotorJoint->DesiredVelocity(0);
		}
		else
		{
			float fltDesiredVel = m_lpThisMotorJoint->DesiredVelocity();
			float fltMaxVel = m_lpThisMotorJoint->MaxVelocity();
			float fltMaxForce = m_lpThisMotorJoint->MaxForce();

			float fltSetVelocity = fltDesiredVel;

			m_lpThisMotorJoint->SetVelocity(fltSetVelocity);
			m_lpThisMotorJoint->DesiredVelocity(0);

			float fltHalfPercVel = fabs(fltSetVelocity * 0.01);
            m_fltPredictedPos = m_fltNextPredictedPos;
            m_fltNextPredictedPos = m_fltPredictedPos +  (fltSetVelocity*m_lpThisAB->GetSimulator()->PhysicsTimeStep());

			m_lpThisJoint->JointPosition(m_fltNextPredictedPos);

			float fltJointVel = m_lpThisJoint->JointVelocity();

			if(!m_lpThisJoint->UsesRadians())
				fltJointVel *= m_lpThisAB->GetSimulator()->InverseDistanceUnits();;

			float fltVelDiff = fabs(fltJointVel - fltSetVelocity);

			//Only do anything if the velocity value has changed
			if(fltVelDiff > 1e-4)
			{
				if(fabs(fltSetVelocity) > 1e-4)
					Physics_EnableMotor(true, fltSetVelocity, fltMaxForce, false);
				else if(!m_bJointLocked)
					Physics_EnableLock(true, m_lpThisJoint->JointPosition(), fltMaxForce);
			}

			m_lpThisMotorJoint->PrevVelocity(fltSetVelocity);
		}
	}
}

void RbMotorizedJoint::Physics_ResetSimulation()
{
	m_fltPredictedPos = 0;
	m_fltNextPredictedPos = 0;
}

void RbMotorizedJoint::Physics_CollectExtraData()
{
 //   OsgSimulator *lpSim = GetOsgSimulator();
	//if(m_btJoint && lpSim && m_lpThisMotorJoint)
	//{
	//    float fDisUnits = m_lpThisAB->GetSimulator()->DistanceUnits();
	//    float fMassUnits = m_lpThisAB->GetSimulator()->MassUnits();
 //       CStdFPoint vVal, vAssist;

 //       float fltRatio = fMassUnits * fDisUnits;
 //       vAssist = m_lpThisMotorJoint->MotorAssistForceToAReport();
	//	vVal[0] = (m_btJointFeedback.m_appliedForceBodyA[0] * fltRatio) + vAssist[0];
	//	vVal[1] = (m_btJointFeedback.m_appliedForceBodyA[1] * fltRatio) + vAssist[1];
	//	vVal[2] = (m_btJointFeedback.m_appliedForceBodyA[2] * fltRatio) + vAssist[2];
 //       m_lpThisMotorJoint->MotorForceToA(vVal);

 //       vAssist = m_lpThisMotorJoint->MotorAssistForceToBReport();
	//	vVal[0] = (m_btJointFeedback.m_appliedForceBodyB[0] * fltRatio) + vAssist[0];
	//	vVal[1] = (m_btJointFeedback.m_appliedForceBodyB[1] * fltRatio) + vAssist[0];
	//	vVal[2] = (m_btJointFeedback.m_appliedForceBodyB[2] * fltRatio) + vAssist[0];
 //       m_lpThisMotorJoint->MotorForceToB(vVal);

 //       fltRatio = fMassUnits * fDisUnits * fDisUnits;
 //       vAssist = m_lpThisMotorJoint->MotorAssistTorqueToAReport();
	//	vVal[0] = (m_btJointFeedback.m_appliedTorqueBodyA[0] * fltRatio) + vAssist[0];
	//	vVal[1] = (m_btJointFeedback.m_appliedTorqueBodyA[1] * fltRatio) + vAssist[0];
	//	vVal[2] = (m_btJointFeedback.m_appliedTorqueBodyA[2] * fltRatio) + vAssist[0];
 //       m_lpThisMotorJoint->MotorTorqueToA(vVal);

 //       vAssist = m_lpThisMotorJoint->MotorAssistTorqueToBReport();
	//	vVal[0] = (m_btJointFeedback.m_appliedTorqueBodyB[0] * fltRatio) + vAssist[0];
	//	vVal[1] = (m_btJointFeedback.m_appliedTorqueBodyB[1] * fltRatio) + vAssist[0];
	//	vVal[2] = (m_btJointFeedback.m_appliedTorqueBodyB[2] * fltRatio) + vAssist[0];
 //       m_lpThisMotorJoint->MotorTorqueToB(vVal);
	//}
}


	}			// Environment
}				//RoboticsAnimatSim