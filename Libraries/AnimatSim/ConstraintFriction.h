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
			/// The friction coefficient for this constraint.
			float m_fltCoefficient;

			/// The maximum force for this constraint.
			float m_fltMaxForce;

			/// The velocity loss for this constraint.
			float m_fltLoss;

            /// Tells if the friction force should be scaled based on the force applied to the joint.
            bool m_bProportional;

            /// The scale ration of static to dynamic friction
            float m_fltStaticFrictionScale;

			virtual void SetFrictionProperties() = 0;

		public:
			ConstraintFriction();
			virtual ~ConstraintFriction();
			
			static ConstraintFriction *CastToDerived(AnimatBase *lpBase) {return static_cast<ConstraintFriction*>(lpBase);}

            virtual bool Enabled();
            virtual void Enabled(bool bVal);

    		virtual float Coefficient();
			virtual void Coefficient(float fltVal);

			virtual float MaxForce();
			virtual void MaxForce(float fltVal, bool bUseScaling = true);

			virtual float Loss();
			virtual void Loss(float fltVal, bool bUseScaling = true);

    		virtual bool Proportional();
			virtual void Proportional(bool bVal);

			virtual float StaticFrictionScale();
			virtual void StaticFrictionScale(float fltVal);

			virtual void CreateDefaultUnits();
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

			virtual void Load(CStdXml &oXml);
        };

	}			// Visualization
}				//VortexAnimatSim
