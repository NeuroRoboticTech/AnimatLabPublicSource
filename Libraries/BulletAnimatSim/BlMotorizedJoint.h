
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{

		class BULLET_PORT BlMotorizedJoint : public BlJoint, public IMotorizedJoint
		{
		protected:
			MotorizedJoint *m_lpThisMotorJoint;
			bool m_bMotorOn;

			virtual void SetThisPointers();
			virtual void CalculateServoVelocity();

		public:
			BlMotorizedJoint();
			virtual ~BlMotorizedJoint();

			virtual void Physics_SetVelocityToDesired();
			virtual void Physics_EnableLock(bool bOn, float fltPosition, float fltMaxLockForce);
			virtual void Physics_EnableMotor(bool bOn, float fltDesiredVelocity, float fltMaxForce);
			virtual void Physics_MaxForce(float fltVal);
		};

	}			// Environment
}				//BulletAnimatSim
