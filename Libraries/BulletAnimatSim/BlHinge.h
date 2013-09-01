/**
\file	BlHinge.h

\brief	Declares the vortex hinge class.
**/

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{

		/**
		\namespace	BulletAnimatSim::Environment::Joints

		\brief	Joint classes that use the vortex physics engine. 
		**/
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
			class BULLET_PORT BlHinge : public BlMotorizedJoint, public AnimatSim::Environment::Joints::Hinge, public OsgAnimatSim::Environment::Joints::OsgHinge     
			{
			protected:
				/// The bullet hinge class.
				btHingeConstraint *m_btHinge;

				/// The rotation of the hinge in degrees.
				float m_fltRotationDeg;

    			virtual void DeleteJointGraphics();
                virtual void CreateJointGraphics();
				virtual void SetupGraphics();
				virtual void UpdateData();
				virtual void SetupPhysics();
				virtual void DeletePhysics();

			public:
				BlHinge();
				virtual ~BlHinge();

				virtual void JointPosition(float fltPos);

				virtual void SetAlpha();

                virtual void SetLimitValues();

#pragma region DataAccesMethods

				virtual float *GetDataPointer(const string &strDataType);
				virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void EnableLimits(bool bVal);
				virtual void CreateJoint();
				virtual void StepSimulation();

			    virtual void Physics_SetVelocityToDesired();
			    virtual void Physics_EnableLock(bool bOn, float fltPosition, float fltMaxLockForce);
			    virtual void Physics_EnableMotor(bool bOn, float fltDesiredVelocity, float fltMaxForce);
			    virtual void Physics_MaxForce(float fltVal);
                virtual void Physics_CollectData();
            };

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
