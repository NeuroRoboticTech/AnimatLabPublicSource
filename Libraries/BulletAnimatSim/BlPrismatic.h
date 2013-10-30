/**
\file	BlPrismatic.h

\brief	Declares the vs prismatic class.
**/

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class BULLET_PORT BlPrismatic : public BlMotorizedJoint, public AnimatSim::Environment::Joints::Prismatic, public OsgAnimatSim::Environment::Joints::OsgPrismatic     
			{
			protected:
				btSliderConstraint *m_btPrismatic;

       			virtual void DeleteJointGraphics();
                virtual void CreateJointGraphics();

                virtual float GetCurrentBtPosition();

            public:
				BlPrismatic();
				virtual ~BlPrismatic();

				virtual void JointPosition(float fltPos);

				virtual void SetAlpha();

                virtual void SetLimitValues();

				virtual void SetupPhysics();

                virtual void TimeStepModified();

#pragma region DataAccesMethods

				virtual float *GetDataPointer(const std::string &strDataType);
				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

#pragma endregion

				virtual void EnableLimits(bool bVal);
				virtual void CreateJoint();
				virtual void StepSimulation();

                virtual void Physics_EnableLock(bool bOn, float fltPosition, float fltMaxLockForce);
                virtual void Physics_EnableMotor(bool bOn, float fltDesiredVelocity, float fltMaxForce);
                virtual void Physics_MaxForce(float fltVal);
            };

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
