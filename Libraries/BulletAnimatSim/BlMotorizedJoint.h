
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

            btJointFeedback m_btJointFeedback;

			virtual void SetThisPointers();
			virtual void CalculateServoVelocity();

		public:
			BlMotorizedJoint();
			virtual ~BlMotorizedJoint();

			virtual bool JointIsLocked() = 0;

		    virtual void Physics_SetVelocityToDesired();
            virtual void Physics_CollectExtraData();
        };

	}			// Environment
}				//BulletAnimatSim
