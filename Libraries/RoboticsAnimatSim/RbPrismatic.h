/**
\file	RbPrismatic.h

\brief	Declares the vs prismatic class.
**/

#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class ROBOTICS_PORT RbPrismatic : public RbMotorizedJoint, public AnimatSim::Environment::Joints::Prismatic     
			{
			protected:

                virtual void TurnMotorOff();

            public:
				RbPrismatic();
				virtual ~RbPrismatic();

				virtual void JointPosition(float fltPos);

                virtual void SetLimitValues();

#pragma region DataAccesMethods

				virtual float *GetDataPointer(const std::string &strDataType);
				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

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
