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
			class ANIMAT_PORT Hinge : public AnimatSim::Environment::MotorizedJoint    
			{
			protected:
				/// Upper limit constring pointer.
				ConstraintLimit *m_lpUpperLimit;

				/// Lower limit constring pointer.
				ConstraintLimit *m_lpLowerLimit;

				/// Pointer to a constraint that is used to represent the position flap.
				ConstraintLimit *m_lpPosFlap;

				/// The rotation of the hinge in degrees.
				float m_fltRotationDeg;

				/// The desired rotation of the hinge in degrees.
				float m_fltDesiredPositionDeg;

			public:
				Hinge();
				virtual ~Hinge();
			
				static Hinge *CastToDerived(AnimatBase *lpBase) {return static_cast<Hinge*>(lpBase);}

				float CylinderRadius();
				float CylinderHeight();
				float FlapWidth();

				virtual void Enabled(bool bValue);

				virtual ConstraintLimit *UpperLimit() ;
				virtual ConstraintLimit *LowerLimit();

				virtual float GetPositionWithinLimits(float fltPos);
				virtual float GetLimitRange();

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void AddExternalNodeInput(int iTargetDataType, float fltInput);
				virtual int GetTargetDataTypeIndex(const std::string &strDataType);
				virtual void UpdateData();
				virtual void ResetSimulation();

				virtual void Load(CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim
