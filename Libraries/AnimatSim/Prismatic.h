/**
\file	Prismatic.h

\brief	Declares the prismatic class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			/**
			\brief	A Prismatic type of joint.
						   
			\details This type of joint is constrained so that it can only
			move linearly up and down one axis. You can define which axis it moves along
			around in the configuration file using the normalized 
			RotationAxis vector element. You can also specify the
			translational constraints for this joint. This prevents it
			from moving further than the constrained value.<br>
			Also, this joint is motorized. So you can specify a desired
			velocity of motion at a given time step using a motor velocity stimulus
			and the physics engine will automatically apply the forces
			necessary to move the joint at the desired velocity.

			\author	dcofer
			\date	3/24/2011
			**/
			class ANIMAT_PORT Prismatic : public Joint    
			{
			protected:
				///This is the minimum radian value that the joint can rotate about its axis.
				///Its orginal position is used as zero radians.
				float m_fltConstraintLow;

				///This is the maximum radian value that the joint can rotate about its axis.
				///Its orginal position is used as zero radians.
				float m_fltConstraintHigh;

				float m_fltMaxForce;

				float m_ftlServoGain;
				BOOL m_bServoMotor;

				void CalculateServoVelocity();

			public:
				Prismatic();
				virtual ~Prismatic();

				virtual BOOL UsesRadians() {return FALSE;};

				virtual void Load(CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim
