// BlConstraintRelaxation.h: interface for the BlConstraintRelaxation class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

#include "btAnimatGeneric6DofConstraint.h"

namespace BulletAnimatSim
{
	namespace Environment
	{

		class BULLET_PORT BlConstraintRelaxation : public AnimatSim::Environment::ConstraintRelaxation
		{
		protected:
            btAnimatGeneric6DofConstraint *m_lpConstraint;

            //Pointer to the bullet joint
            BlJoint *m_lpBlJoint;

            /// The maximum amount that this relaxation is allowed to move.
			float m_fltMaxLimit;

            /// The minimum amount that this relaxation is allowed to move.
			float m_fltMinLimit;

            /// The equilibrium position of the spring that controls this relaxation.
			float m_fltEqPos;

            //Keeps the inverse damping constant to actually use.
            float m_fltInvDamping;

            //For joints like hinge and prismatic I am allowing them to set a spring in the direction of the joint motion (Z axis). However, the joint
            //itself needs to be the one to determine when to enable/disable the spring because it has to control the motor as well. This flag tells
            // the relaxation that it should not directly set the spring enabled/disabled, that it should allow the joint to do that itself.
            bool m_bDisallowSpringEnable;

			virtual void SetRelaxationProperties();

		public:
			BlConstraintRelaxation();
			virtual ~BlConstraintRelaxation();

			virtual float MinLimit();
			virtual void MinLimit(float fltVal, bool bUseScaling = true);

			virtual float MaxLimit();
			virtual void MaxLimit(float fltVal, bool bUseScaling = true);

			virtual float EqPos();
			virtual void EqPos(float fltVal, bool bUseScaling = true);

			virtual void Damping(float fltVal, bool bUseScaling = true);

            virtual bool DisallowSpringEnable();
            virtual void DisallowSpringEnable(bool bVal);

            virtual void Initialize();

			virtual void CreateDefaultUnits();
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

			virtual void Load(CStdXml &oXml);
		};

	}			// Visualization
}				//BulletAnimatSim
