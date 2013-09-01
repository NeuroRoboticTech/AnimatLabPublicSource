
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
            bool m_bJointLocked;

			virtual void SetThisPointers();
			virtual void CalculateServoVelocity();

		public:
			BlMotorizedJoint();
			virtual ~BlMotorizedJoint();

		    virtual void Physics_SetVelocityToDesired();
        };

	}			// Environment
}				//BulletAnimatSim
