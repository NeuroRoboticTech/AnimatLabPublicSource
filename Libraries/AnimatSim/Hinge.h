
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

			/*! \brief 
				A hinge type of joint.
			   
				\remarks
				This type of joint is constrained so that it can only
				rotate about one axis. You can define which axis it rotates
				around in the configuration file using the normalized 
				RotationAxis vector element. You can also specify the
				rotational constraints for this joint. This prevents it
				from rotating further than the constrained value.

				Also, this joint is motorized. So you can specify a desired
				velocity of motion at a given time step using the CNlInjectionMgr
				and the physics engine will automatically apply the forces
				necessary to move the joint at the desired velocity.

				\sa
				Joint, Hinge, CAlStaticJoint
				 
				\ingroup AnimatSim
			*/

			class ANIMAT_PORT Hinge : public Joint    
			{
			protected:
				ConstraintLimit *m_lpUpperLimit;
				ConstraintLimit *m_lpLowerLimit;
				ConstraintLimit *m_lpPosFlap;

				float m_fltMaxTorque;

				float m_ftlServoGain;
				BOOL m_bServoMotor;

				virtual void CreateCylinderGraphics(Simulator *lpSim, Structure *lpStructure) = 0;

			public:
				Hinge();
				virtual ~Hinge();

				float CylinderRadius();
				float CylinderHeight();
				float FlapWidth();

				virtual void Enabled(BOOL bValue) 
				{
					EnableMotor(bValue);
					m_bEnabled = bValue;
				};

				virtual ConstraintLimit *UpperLimit() {return m_lpUpperLimit;};
				virtual ConstraintLimit *LowerLimit() {return m_lpLowerLimit;};

				void ServoMotor(BOOL bServo) {m_bServoMotor = bServo;};
				BOOL ServoMotor() {return m_bServoMotor;};

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

				virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim
