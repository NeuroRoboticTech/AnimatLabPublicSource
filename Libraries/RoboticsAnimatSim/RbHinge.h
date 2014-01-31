/**
\file	RbHinge.h

\brief	Declares the vortex hinge class.
**/

#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{
			/**
			\brief	Vortex hinge joint class.

			\details This class implements a hinge joint. This type of joint
			prevents linear motion for all three dimensions and it prevents angular
			motion for two axises. Allowing the two connected bodies to rotate about
			one axis freely. You can define constraint limits to prevent the motion
			beyond certain angular limits. This type of joint is also motorized and thus
			implements the IMotorized interface using the VsMotorized class. This allows
			the user to control the movement of this joint as if it were a servo motor or
			a velocity controlled motor.
			
			\author	dcofer
			\date	4/15/2011
			**/
			class ROBOTICS_PORT RbHinge : public RbMotorizedJoint, public AnimatSim::Environment::Joints::Hinge     
			{
			protected:
				/// The rotation of the hinge in degrees.
				float m_fltRotationDeg;

				virtual void UpdateData();
                virtual void TurnMotorOff();

			public:
				RbHinge();
				virtual ~RbHinge();

				virtual void JointPosition(float fltPos);

                virtual void SetLimitValues();

#pragma region DataAccesMethods

				virtual float *GetDataPointer(const std::string &strDataType);
				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

#pragma endregion

				virtual void EnableLimits(bool bVal);
				virtual void CreateJoint();
				virtual void StepSimulation();
                virtual void ResetSimulation();

                virtual void Physics_EnableLock(bool bOn, float fltPosition, float fltMaxLockForce);
			    virtual void Physics_EnableMotor(bool bOn, float fltDesiredVelocity, float fltMaxForce, bool bForceWakeup);
			    virtual void Physics_MaxForce(float fltVal);

                virtual void SetConstraintFriction();
            };

		}		//Joints
	}			// Environment
}				//RoboticsAnimatSim
