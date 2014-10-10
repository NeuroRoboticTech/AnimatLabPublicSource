
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{

		class VORTEX_PORT VsMotorizedJoint : public VsJoint, public IMotorizedJoint
		{
		protected:
			MotorizedJoint *m_lpThisMotorJoint;
			bool m_bMotorOn;

			virtual void SetThisPointers();
			virtual void CalculateServoVelocity();

		public:
			VsMotorizedJoint();
			virtual ~VsMotorizedJoint();

			virtual void Physics_SetVelocityToDesired();
			virtual void Physics_EnableLock(bool bOn, float fltPosition, float fltMaxLockForce);
			virtual void Physics_EnableMotor(bool bOn, float fltDesiredVelocity, float fltMaxForce, bool bForceWakeup);
			virtual void Physics_MaxForce(float fltVal);
		};

	}			// Environment
}				//VortexAnimatSim
