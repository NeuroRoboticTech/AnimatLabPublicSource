/**
\file	ConstraintFriction.h

\brief	Declares the material pair class.
**/

#pragma once

namespace AnimatSim
{

	namespace Environment
	{
		/**
		\brief	A material type name.
				
		\author	dcofer
		\date	3/22/2011
		**/
		class ANIMAT_PORT ConstraintFriction : public AnimatBase
		{
		protected:
            ///whether the relaxation is enabled or not
            BOOL m_bEnabled;

			/// The friction coefficient for this constraint.
			float m_fltCoefficient;

			/// The maximum force for this constraint.
			float m_fltMaxForce;

			/// The velocity loss for this constraint.
			float m_fltLoss;

            /// Tells if the friction force should be scaled based on the force applied to the joint.
            BOOL m_bProportional;

            /// The scale ration of static to dynamic friction
            float m_fltStaticFrictionScale;

			virtual void SetFrictionProperties() = 0;

		public:
			ConstraintFriction();
			virtual ~ConstraintFriction();

            virtual BOOL Enabled();
            virtual void Enabled(BOOL bVal);

    		virtual float Coefficient();
			virtual void Coefficient(float fltVal);

			virtual float MaxForce();
			virtual void MaxForce(float fltVal, BOOL bUseScaling = TRUE);

			virtual float Loss();
			virtual void Loss(float fltVal, BOOL bUseScaling = TRUE);

    		virtual BOOL Proportional();
			virtual void Proportional(BOOL bVal);

			virtual float StaticFrictionScale();
			virtual void StaticFrictionScale(float fltVal);

			virtual void CreateDefaultUnits();
			virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

			virtual void Load(CStdXml &oXml);
        };

	}			// Visualization
}				//VortexAnimatSim
