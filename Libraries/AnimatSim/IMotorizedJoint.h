#pragma once

namespace AnimatSim
{
	namespace Environment
	{


		class ANIMAT_PORT IMotorizedJoint
		{
		public:
			IMotorizedJoint(void);
			virtual ~IMotorizedJoint(void);

			virtual void Physics_SetVelocityToDesired() = 0;
			virtual void Physics_EnableLock(bool bOn, float fltPosition, float fltMaxLockForce) = 0;

			/**
			\brief	Enables\disables the motor.

			\details If this is a motorized joint then when you turn it on the physics engine will calculate the
			torque that needs to be applied to this joint in order for it to have the desired Velocity for
			its current load.

			\author	dcofer
			\date	4/3/2011

			\param	bOn				  	true to enable. 
			\param	fltDesiredVelocity	The desired motor velocity. 
			\param	fltMaxForce		  	The maximum motor force. 
            \param  bForceWakeup        force a call to wakup the joint.
			**/
			virtual void Physics_EnableMotor(bool bOn, float fltDesiredVelocity, float fltMaxForce, bool bForceWakeup) = 0;
	
			/**
			\brief	Sets the maximum forces allowed by the motorized joint.

			\author	dcofer
			\date	3/22/2011

			\param	fltVal	   	The new value. 
			\param	bUseScaling	true to use unit scaling. 
			**/
			virtual void Physics_MaxForce(float fltVal) = 0;
		};

	}
}