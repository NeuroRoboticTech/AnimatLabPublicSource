/**
\file	BlPrismatic.h

\brief	Declares the vs prismatic class.
**/

#pragma once

#include "btAnimatGeneric6DofConstraint.h"

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class BULLET_PORT BlPrismatic : public BlMotorizedJoint, public AnimatSim::Environment::Joints::Prismatic, public OsgAnimatSim::Environment::Joints::OsgPrismatic     
			{
			protected:
                btAnimatGeneric6DofConstraint *m_btPrismatic;

       			virtual void DeleteJointGraphics();
                virtual void CreateJointGraphics();

                virtual float GetCurrentBtPosition();

                virtual void TurnMotorOff();
                virtual bool NeedApplyAssist();
                virtual void ApplyMotorAssist();

                virtual void EnableFeedback();

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
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
				virtual bool JointIsLocked();

#pragma endregion

				virtual void EnableLimits(bool bVal);
				virtual void CreateJoint();
				virtual void StepSimulation();
                virtual void ResetSimulation();

                virtual void Physics_EnableLock(bool bOn, float fltPosition, float fltMaxLockForce);
                virtual void Physics_EnableMotor(bool bOn, float fltDesiredVelocity, float fltMaxForce, bool bForceWakeup);
                virtual void Physics_MaxForce(float fltVal);

                virtual void SetConstraintFriction();
                virtual void AxisConstraintSpringEnableChanged(bool bEnabled);
            };

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
