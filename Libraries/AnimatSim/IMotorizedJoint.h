#pragma once

namespace AnimatSim
{
	namespace Environment
	{


		class ANIMAT_PORT IMotorizedJoint
		{
		protected:

		public:
			IMotorizedJoint(void);
			virtual ~IMotorizedJoint(void);

			virtual BOOL EnableMotor() = 0;
			virtual void EnableMotor(BOOL bVal) = 0;

			virtual float MaxVelocity() = 0;
			virtual void MaxVelocity(float fltVal, BOOL bUseScaling = TRUE) = 0;

			virtual float SetVelocity() = 0;
			virtual float DesiredVelocity() = 0;
			virtual void DesiredVelocity(float fltVelocity) = 0;
			virtual void MotorInput(float fltInput) = 0;
		};

	}
}