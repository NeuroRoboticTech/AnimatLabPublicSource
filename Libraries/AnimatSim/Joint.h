/**
\file	Joint.h

\brief	Declares the joint class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{

		/**
		\brief	The base class for all of the joint type of objects.
		
		\details This class provides the base functionality for a joint. 
		A joint is what glues the different rigid bodies together
		into a complete structure. Each object is connected by a 
		joint to its parent part. The only exception to this rule
		is the root rigid body of a structure/organism. The joint is
		positioned relative to the center of the parent object. 
		This can be a little confusing becuase in the configuration
		file the joint is actually contained within the child object
		tag. So you may be tempted to try and make the joint relative
		to the child object. The different sub classes will define 
		data parameters that specify the motion that they joint can 
		perform. For instance, a HingeJoint is constrained to rotate
		about one axis just like the hinge on a door. But if you simply
		wanted one part act as if it is phyiscally connected to its parent
		part then you would use a StaticJoint.<br><br>
		Some joints can be motorized, and some cannot. Hinge and Prismatic 
		joints are currently the only joints that are motorized. So all of the
		parameters related to motors are only relevant for those joint types.

		\author	dcofer
		\date	3/22/2011
		**/
		class ANIMAT_PORT Joint : public BodyPart  
		{
		protected:
			///The child rigid body for this joint. 
			RigidBody *m_lpChild;

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

			/// If true then any ConstraintLimits for this joint are enabled.
			BOOL m_bEnableLimits;

			/// The current position of the joint. This value can be in radians or meters 
			/// depending on the type of joint. UsesRadians lets you know whether it is using radians.
			float m_fltPosition;

			/// The current velocity of the joint.
			/// Velocities can be in rad/sec or m/s depending on the type of joint. UsesRadians lets you know whether it is using radians.
			float m_fltVelocity;

			/// The current force being applied to the joint by the motor.
			float m_fltForce;

			///Scales the size of the graphics for this joint.
			float m_fltSize;

			virtual void SetVelocityToDesired();

		public:
			Joint();
			virtual ~Joint();

			virtual BOOL UsesRadians();

			virtual float Size();
			virtual void Size(float fltVal, BOOL bUseScaling = TRUE);

			virtual BOOL EnableLimits();
			virtual void EnableLimits(BOOL bVal);

			virtual BOOL EnableMotor();
			virtual void EnableMotor(BOOL bVal);

			virtual float MaxVelocity();
			virtual void MaxVelocity(float fltVal, BOOL bUseScaling = TRUE);

			virtual int VisualSelectionType();

			virtual RigidBody *Child();
			virtual void Child(RigidBody *lpValue);

			virtual float JointPosition();
			virtual void JointPosition(float fltPos);
			virtual float JointVelocity();
			virtual void JointVelocity(float fltVel);
			virtual float JointForce();
			virtual void JointForce(float fltForce);

			virtual float SetVelocity();
			virtual float DesiredVelocity();
			virtual void DesiredVelocity(float fltVelocity);
			virtual void MotorInput(float fltInput);

			virtual void CreateJoint();

			virtual void AddExternalNodeInput(float fltInput);
			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
