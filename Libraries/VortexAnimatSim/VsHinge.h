/**
\file	VsHinge.h

\brief	Declares the vortex hinge class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{

		/**
		\namespace	VortexAnimatSim::Environment::Joints

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
			class VORTEX_PORT VsHinge : public VsMotorizedJoint, public AnimatSim::Environment::Joints::Hinge, public OsgAnimatSim::Environment::OsgHinge     
			{
			protected:
				/// The vortex hinge class.
				Vx::VxHinge *m_vxHinge;

				/// The rotation of the hinge in degrees.
				float m_fltRotationDeg;

    			virtual void DeleteJointGraphics();
                virtual void CreateJointGraphics();
				virtual void SetupGraphics();
				virtual void UpdateData();
				virtual void SetupPhysics();
				virtual void DeletePhysics();

			public:
				VsHinge();
				virtual ~VsHinge();

				virtual void JointPosition(float fltPos);

				virtual void SetAlpha();

#pragma region DataAccesMethods

				virtual float *GetDataPointer(const string &strDataType);
				virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void EnableLimits(bool bVal);
				virtual void CreateJoint();
				virtual void StepSimulation();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
