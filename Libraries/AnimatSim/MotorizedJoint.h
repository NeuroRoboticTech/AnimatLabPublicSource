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

			/// The un=scaled maximum force.
			float m_fltMaxForceNotScaled;

			/// The gain of the servo motor.
			float m_ftlServoGain;

			/// true if this is a servo motor. A servo motor is position controlled instead of velocity controlled.
			bool m_bServoMotor;

            /// Countdown timer till we can begin applying motor assist. Once a motor is turned on we wait for this
            /// many time steps before checking if its velocity matches the desired velocity. This is so the part
            /// has a chanct to start moving and so we do not apply additional forces if it is not required.
            int m_iAssistCountdown;

            /// Force vector that the motor is applying to body A. (un-scaled units). 
            /// This includes any motor assist within it. 
            CStdFPoint m_vMotorForceToA;

            /// Force vector that the motor assist is applying to body A. (scaled units).
            CStdFPoint m_vMotorAssistForceToA;

            /// Force vector that the motor assist is applying to body A. (un-scaled units).
            CStdFPoint m_vMotorAssistForceToAReport;

            /// Force vector that the motor is applying to body B. (un-scaled units). 
            /// This includes any motor assist within it. 
            CStdFPoint m_vMotorForceToB;

            /// Force vector that the motor assist is applying to body B. (scaled units).
            CStdFPoint m_vMotorAssistForceToB;

            /// Force vector that the motor assist is applying to body B. (un-scaled units).
            CStdFPoint m_vMotorAssistForceToBReport;

            /// Torque vector that the motor is applying to body A. (un-scaled units). 
            /// This includes any motor assist within it. 
            CStdFPoint m_vMotorTorqueToA;

            /// Torque vector that the motor assist is applying to body A. (scaled units).
            CStdFPoint m_vMotorAssistTorqueToA;

            /// Torque vector that the motor assist is applying to body A. (un-scaled units).
            CStdFPoint m_vMotorAssistTorqueToAReport;

            /// Torque vector that the motor is applying to body B. (un-scaled units). 
            /// This includes any motor assist within it. 
            CStdFPoint m_vMotorTorqueToB;

            /// Torque vector that the motor assist is applying to body B. (scaled units).
            CStdFPoint m_vMotorAssistTorqueToB;

            /// Torque vector that the motor assist is applying to body B. (un-scaled units).
            CStdFPoint m_vMotorAssistTorqueToBReport;

            /// The PID controller for the motor assist system.
            CStdPID *m_lpAssistPid;

            virtual void ClearAssistForces();
            virtual void ApplyMotorAssist();
            virtual void EnableFeedback();

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
			virtual float MaxForceNotScaled();

			virtual float MaxVelocity();
			virtual void MaxVelocity(float fltVal, bool bUseScaling = true);

			virtual float DesiredVelocity();
			virtual void DesiredVelocity(float fltVelocity);

			virtual float SetVelocity();
			virtual void SetVelocity(float fltVal);

			virtual float PrevVelocity();
			virtual void PrevVelocity(float fltVal);

            virtual int AssistCountdown();
            virtual void AssistCountdown(int iVal);

            virtual CStdFPoint MotorForceToA();
            virtual void MotorForceToA(CStdFPoint &vVal);

            virtual CStdFPoint MotorAssistForceToA();
            virtual void MotorAssistForceToA(CStdFPoint &vVal);

            virtual CStdFPoint MotorAssistForceToAReport();
            virtual void MotorAssistForceToAReport(CStdFPoint &vVal);

            virtual CStdFPoint MotorForceToB();
            virtual void MotorForceToB(CStdFPoint &vVal);

            virtual CStdFPoint MotorAssistForceToB();
            virtual void MotorAssistForceToB(CStdFPoint &vVal);

            virtual CStdFPoint MotorAssistForceToBReport();
            virtual void MotorAssistForceToBReport(CStdFPoint &vVal);

            virtual CStdFPoint MotorTorqueToA();
            virtual void MotorTorqueToA(CStdFPoint &vVal);

            virtual CStdFPoint MotorAssistTorqueToA();
            virtual void MotorAssistTorqueToA(CStdFPoint &vVal);

            virtual CStdFPoint MotorAssistTorqueToAReport();
            virtual void MotorAssistTorqueToAReport(CStdFPoint &vVal);

            virtual CStdFPoint MotorTorqueToB();
            virtual void MotorTorqueToB(CStdFPoint &vVal);

            virtual CStdFPoint MotorAssistTorqueToB();
            virtual void MotorAssistTorqueToB(CStdFPoint &vVal);

            virtual CStdFPoint MotorAssistTorqueToBReport();
            virtual void MotorAssistTorqueToBReport(CStdFPoint &vVal);

            virtual CStdPID *AssistPid();

			virtual void MotorInput(float fltInput);

			virtual void SetVelocityToDesired();
			virtual void EnableLock(bool bOn, float fltPosition, float fltMaxLockForce);

			virtual void ResetSimulation();

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

			virtual void Load(CStdXml &oXml);
		};

	}
}