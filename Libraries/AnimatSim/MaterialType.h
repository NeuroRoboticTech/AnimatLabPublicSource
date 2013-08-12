/**
\file	MaterialType.h

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
		class ANIMAT_PORT MaterialType : public AnimatBase
		{
		protected:
			/// The primary linear coefficient of friction parameter.
			float m_fltFrictionLinearPrimary;

			/// The secondary linear coefficient of friction parameter.
			float m_fltFrictionLinearSecondary;

			/// The angular normal coefficient of friction parameter. (this simulates spinning resistance)
			float m_fltFrictionAngularNormal;

			/// The angular primary coefficient of friction parameter.(this simulates rolling resistance)
			float m_fltFrictionAngularPrimary;

			/// The angular secondary coefficient of friction parameter.(this simulates rolling resistance)
			float m_fltFrictionAngularSecondary;

			/// The maximum linear primary friction that can created.
			float m_fltFrictionLinearPrimaryMax;

			/// The maximum linear secondary friction that can created.
			float m_fltFrictionLinearSecondaryMax;

			/// The maximum angular normal friction that can created.
			float m_fltFrictionAngularNormalMax;

			/// The maximum angular primary friction that can created.
			float m_fltFrictionAngularPrimaryMax;

			/// The maximum angular secondary friction that can created.
			float m_fltFrictionAngularSecondaryMax;

			/// The compliance of the collision between those two materials.
			float m_fltCompliance;

			/// The damping of the collision between those two materials.
			float m_fltDamping;

			/// The restitution of the collision between those two materials.
			float m_fltRestitution;

			/// The primary linear slip of the collision between those two materials.
			float m_fltSlipLinearPrimary;

			/// The secondary linear slip of the collision between those two materials.
			float m_fltSlipLinearSecondary;

			/// The angular normal slip of the collision between those two materials.
			float m_fltSlipAngularNormal;

			/// The angular primary slip of the collision between those two materials.
			float m_fltSlipAngularPrimary;

			/// The angular secondary slip of the collision between those two materials.
			float m_fltSlipAngularSecondary;

			/// The primary linear slide of the collision between those two materials.
			float m_fltSlideLinearPrimary;

			/// The primary linear slide of the collision between those two materials.
			float m_fltSlideLinearSecondary;

			/// The angular normal slide of the collision between those two materials.
			float m_fltSlideAngularNormal;

			/// The angular primary slide of the collision between those two materials.
			float m_fltSlideAngularPrimary;

			/// The angular secondary slide of the collision between those two materials.
			float m_fltSlideAngularSecondary;

			/// The maximum adhesion of the collision between those two materials.
			float m_fltMaxAdhesive;

			virtual void SetMaterialProperties() = 0;

			/**
			\brief	Registers the material types within the physics engine.

			\details This is a pure virtual function that must be overridden in the derived physics class.
			
			\author	dcofer
			\date	3/23/2011
			
			\param	aryMaterialTypes	List of types of the ary materials. 
			**/
			virtual void RegisterMaterialType() = 0;

		public:
			MaterialType();
			virtual ~MaterialType();

			virtual float FrictionLinearPrimary();
			virtual void FrictionLinearPrimary(float fltVal);
			virtual float FrictionLinearSecondary();
			virtual void FrictionLinearSecondary(float fltVal);
			virtual float FrictionAngularNormal();
			virtual void FrictionAngularNormal(float fltVal);
			virtual float FrictionAngularPrimary();
			virtual void FrictionAngularPrimary(float fltVal);
			virtual float FrictionAngularSecondary();
			virtual void FrictionAngularSecondary(float fltVal);

			virtual float FrictionLinearPrimaryMax();
			virtual void FrictionLinearPrimaryMax(float fltVal, bool bUseScaling = true);
			virtual float FrictionLinearSecondaryMax();
			virtual void FrictionLinearSecondaryMax(float fltVal, bool bUseScaling = true);
			virtual float FrictionAngularNormalMax();
			virtual void FrictionAngularNormalMax(float fltVal, bool bUseScaling = true);
			virtual float FrictionAngularPrimaryMax();
			virtual void FrictionAngularPrimaryMax(float fltVal, bool bUseScaling = true);
			virtual float FrictionAngularSecondaryMax();
			virtual void FrictionAngularSecondaryMax(float fltVal, bool bUseScaling = true);

			virtual float SlipLinearPrimary();
			virtual void SlipLinearPrimary(float fltVal, bool bUseScaling = true);
			virtual float SlipLinearSecondary();
			virtual void SlipLinearSecondary(float fltVal, bool bUseScaling = true);
			virtual float SlipAngularNormal();
			virtual void SlipAngularNormal(float fltVal, bool bUseScaling = true);
			virtual float SlipAngularPrimary();
			virtual void SlipAngularPrimary(float fltVal, bool bUseScaling = true);
			virtual float SlipAngularSecondary();
			virtual void SlipAngularSecondary(float fltVal, bool bUseScaling = true);

			virtual float SlideLinearPrimary();
			virtual void SlideLinearPrimary(float fltVal, bool bUseScaling = true);
			virtual float SlideLinearSecondary();
			virtual void SlideLinearSecondary(float fltVal, bool bUseScaling = true);
			virtual float SlideAngularNormal();
			virtual void SlideAngularNormal(float fltVal, bool bUseScaling = true);
			virtual float SlideAngularPrimary();
			virtual void SlideAngularPrimary(float fltVal, bool bUseScaling = true);
			virtual float SlideAngularSecondary();
			virtual void SlideAngularSecondary(float fltVal, bool bUseScaling = true);

			virtual float Compliance();
			virtual void Compliance(float fltVal, bool bUseScaling = true);

			virtual float Damping();
			virtual void Damping(float fltVal, bool bUseScaling = true);

			virtual float Restitution();
			virtual void Restitution(float fltVal);

			virtual float MaxAdhesive();
			virtual void MaxAdhesive(float fltVal, bool bUseScaling = true);

			/**
			\brief	Gets a material identifier used by the physics engine.

			\details This is a pure virtual function that must be overridden in the derived physics class.

			\author	dcofer
			\date	3/23/2011
			
			\param	strName	Name of the string. 
			
			\return	The material identifier.
			**/
			virtual int GetMaterialID(string strName) = 0;

			virtual void CreateDefaultUnits();
			virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

			virtual void Load(CStdXml &oXml);
        };

	}			// Visualization
}				//VortexAnimatSim
