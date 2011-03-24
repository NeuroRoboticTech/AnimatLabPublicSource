/**
\file	Hinge.h

\brief	Declares the hinge class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{

		/**
		\namespace	AnimatSim::Environment::Joints

		\brief	Contains all of the different joint types that can be used to connect body parts. 
		**/
		namespace Joints
		{

			/**
			\brief	A hinge type of joint.
			   
			\details This type of joint is constrained so that it can only
			rotate about one axis. You can define which axis it rotates
			around in the configuration file using the normalized 
			RotationAxis vector element. You can also specify the
			rotational constraints for this joint. This prevents it
			from rotating further than the constrained value.<br>
			Also, this joint is motorized. So you can specify a desired
			velocity of motion at a given time step using a motor velocity stimulus
			and the physics engine will automatically apply the forces
			necessary to move the joint at the desired velocity.
			
			\author	dcofer
			\date	3/24/2011
			**/
			class ANIMAT_PORT Hinge : public Joint    
			{
			protected:
				/// Upper limit constring pointer.
				ConstraintLimit *m_lpUpperLimit;

				/// Lower limit constring pointer.
				ConstraintLimit *m_lpLowerLimit;

				/// Pointer to a constraint that is used to represent the position flap.
				ConstraintLimit *m_lpPosFlap;

				/// The maximum torque that the motor can apply.
				float m_fltMaxTorque;

				/// The gain of the servo motor.
				float m_ftlServoGain;

				/// true if this is a servo motor. A servo motor is position controlled instead of velocity controlled.
				BOOL m_bServoMotor;

				/**
				\brief	Creates the cylinder graphics for the visible hinge.
				
				\author	dcofer
				\date	3/24/2011
				**/
				virtual void CreateCylinderGraphics() = 0;

			public:
				Hinge();
				virtual ~Hinge();

				float CylinderRadius();
				float CylinderHeight();
				float FlapWidth();

				virtual void Enabled(BOOL bValue);

				virtual ConstraintLimit *UpperLimit() ;
				virtual ConstraintLimit *LowerLimit();

				virtual void ServoMotor(BOOL bServo);
				virtual BOOL ServoMotor();

				virtual void ServoGain(float fltVal);
				virtual float ServoGain();

				virtual void MaxTorque(float fltVal, BOOL bUseScaling = TRUE);
				virtual float MaxTorque();

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

				virtual void Load(CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim
