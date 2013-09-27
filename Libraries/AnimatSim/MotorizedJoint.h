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

			/// This is the variable that is reported to AnimatLab on what the set veloicty was. 
			float m_fltReportSetVelocity;

			/// The maximum velocity that the motorized joint can attain.
			/// Velocities can be in rad/sec or m/s depending on the type of joint. UsesRadians lets you know whether it is using radians.
			float m_fltMaxVelocity;

			/// The previous velocity of the motorized joint in the last time step.
			/// Velocities can be in rad/sec or m/s depending on the type of joint. UsesRadians lets you know whether it is using radians.
			float m_fltPrevVelocity;

			/// If true then the motor for this joint is enabled.
			bool m_bEnableMotor;

			/// Tells whether the motor was enabled when the sim started. This is used when 
			/// resetting the simulation back to its initial settings.
			bool m_bEnableMotorInit;

			/// The maximum force/torque that the motor can apply. Whether this is force or torque
			/// depends on whether the joint uses radians or not.
			float m_fltMaxForce;

			/// The gain of the servo motor.
			float m_ftlServoGain;

			/// true if this is a servo motor. A servo motor is position controlled instead of velocity controlled.
			bool m_bServoMotor;

		public:
			MotorizedJoint(void);
			virtual ~MotorizedJoint(void);

			virtual IMotorizedJoint *PhysicsMotorJoint();
			virtual void PhysicsMotorJoint(IMotorizedJoint *lpJoint);

			virtual bool EnableMotor();
			virtual void EnableMotor(bool bVal);

			virtual void ServoMotor(bool bServo);
			virtual bool ServoMotor();

			virtual void ServoGain(float fltVal);
			virtual float ServoGain();

			virtual void MaxForce(float fltVal, bool bUseScaling = true);
			virtual float MaxForce();

			virtual float MaxVelocity();
			virtual void MaxVelocity(float fltVal, bool bUseScaling = true);

			virtual float DesiredVelocity();
			virtual void DesiredVelocity(float fltVelocity);

			virtual float SetVelocity();
			virtual void SetVelocity(float fltVal);

			virtual float PrevVelocity();
			virtual void PrevVelocity(float fltVal);

			virtual void MotorInput(float fltInput);

			virtual void SetVelocityToDesired();
			virtual void EnableLock(bool bOn, float fltPosition, float fltMaxLockForce);

			virtual void ResetSimulation();

			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

			virtual void Load(CStdXml &oXml);
		};

	}
}