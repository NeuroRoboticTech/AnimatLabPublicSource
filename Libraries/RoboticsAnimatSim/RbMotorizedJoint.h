
#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{

		class ROBOTICS_PORT RbMotorizedJoint : public RbJoint, public IMotorizedJoint
		{
		protected:
			MotorizedJoint *m_lpThisMotorJoint;
			bool m_bMotorOn;
            bool m_bJointLocked;

            float m_fltPredictedPos;
            float m_fltNextPredictedPos;

			virtual void SetThisPointers();
			virtual void CalculateServoVelocity();

		public:
			RbMotorizedJoint();
			virtual ~RbMotorizedJoint();

			virtual void Physics_ResetSimulation();
		    virtual void Physics_SetVelocityToDesired();
            virtual void Physics_CollectExtraData();
        };

	}			// Environment
}				//RoboticsAnimatSim
