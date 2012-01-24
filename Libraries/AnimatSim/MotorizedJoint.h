#pragma once

namespace AnimatSim
{
	namespace Environment
	{


		class ANIMAT_PORT MotorizedJoint : public Joint
		{
		protected:
			/// The pointer to the physics joint instance.
			IMotorizedJoint *m_lpPhysicsMotorJoint;

			///This is the velocity to use for the motorized joint. The motor must be enabled
			///for this parameter to have any effect. 
			/// Velocities can be in rad/sec or m/s depending on the type of joint. UsesRadians lets you know whether it is using radians.
			float m_fltSetVelocity;

			/// This is the desired velocity of the motorized joint. IE the target we are shooting for.
			/// The Desired velocity must get reset at each time step to zero, so it can be added to using AddExternalInput for the
			/// next time step. m_fltSetVelocity then keeps track of what we set the velocity of the motor to be.
			/// Velocities can be in rad/sec or m/s depending on the type of joint. UsesRadians lets you know whether it is using radians.
			float m_fltDesiredVelocity;

			/// The maximum velocity that the motorized joint can attain.
			/// Velocities can be in rad/sec or m/s depending on the type of joint. UsesRadians lets you know whether it is using radians.
			float m_fltMaxVelocity;

			/// The previous velocity of the motorized joint in the last time step.
			/// Velocities can be in rad/sec or m/s depending on the type of joint. UsesRadians lets you know whether it is using radians.
			float m_fltPrevVelocity;

			/// If true then the motor for this joint is enabled.
			BOOL m_bEnableMotor;

			/// Tells whether the motor was enabled when the sim started. This is used when 
			/// resetting the simulation back to its initial settings.
			BOOL m_bEnableMotorInit;

			/// The maximum force/torque that the motor can apply. Whether this is force or torque
			/// depends on whether the joint uses radians or not.
			float m_fltMaxForce;

			/// The gain of the servo motor.
			float m_ftlServoGain;

			/// true if this is a servo motor. A servo motor is position controlled instead of velocity controlled.
			BOOL m_bServoMotor;

		public:
			MotorizedJoint(void);
			virtual ~MotorizedJoint(void);

			virtual IMotorizedJoint *PhysicsMotorJoint();
			virtual void PhysicsMotorJoint(IMotorizedJoint *lpJoint);

			virtual BOOL EnableMotor();
			virtual void EnableMotor(BOOL bVal);

			virtual void ServoMotor(BOOL bServo);
			virtual BOOL ServoMotor();

			virtual void ServoGain(float fltVal);
			virtual float ServoGain();

			virtual void MaxForce(float fltVal, BOOL bUseScaling = TRUE);
			virtual float MaxForce();

			virtual float MaxVelocity();
			virtual void MaxVelocity(float fltVal, BOOL bUseScaling = TRUE);

			virtual float DesiredVelocity();
			virtual void DesiredVelocity(float fltVelocity);

			virtual float SetVelocity();
			virtual void SetVelocity(float fltVal);

			virtual float PrevVelocity();
			virtual void PrevVelocity(float fltVal);

			virtual void MotorInput(float fltInput);

			virtual void SetVelocityToDesired();
			virtual void EnableLock(BOOL bOn, float fltPosition, float fltMaxLockForce);

			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

			virtual void Load(CStdXml &oXml);
		};

	}
}