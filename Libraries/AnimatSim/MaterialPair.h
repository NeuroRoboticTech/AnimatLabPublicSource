/**
\file	MaterialPair.h

\brief	Declares the material pair class.
**/

#pragma once

namespace AnimatSim
{
	   // materialTable->registerMaterial("boxMaterial");

    //VxContactProperties *pM;
    //pM = materialTable->getContactProperties("groundMaterial", "boxMaterial");
    //pM->setFrictionType(VxContactProperties::kFrictionTypeTwoDirection);
    //pM->setFrictionModel(VxContactProperties::kFrictionModelScaledBox);
    //pM->setFrictionCoefficientPrimary(1.0f);
    //pM->setFrictionCoefficientSecondary(1.3f);
    //pM->setCompliance(5.0e-7f);
    //pM->setDamping(2.0e5f);
    //pM->setSlipPrimary(0.00001f);   // some slip is necessary to allow the vehicle to turn
    //pM->setSlipSecondary(0.00001f); // some slip is necessary to allow the vehicle to turn

	namespace Environment
	{
		/**
		\brief	Describes the interaction between two RigidBody parts that collide within the environment.

		\details Each RigidBody has an associated Material that is defined by a unique Material name. 
		The physics engine keeps a table with all combinations of Material types. The MaterialType
		object is used by us to populate the field of the table in the physics engine. It lists all
		of the parameters that define how to types of Materials interact. This includes things like friction
		compliance, etc.. This lets us control the behavior that occurs when two different RigidBodies collide.
		
		\author	dcofer
		\date	3/22/2011
		**/
		class ANIMAT_PORT MaterialPair : public AnimatBase
		{
		protected:
			/// The unique material name for first material
			string m_strMaterial1;

			/// The unique material name for second material
			string m_strMaterial2;

			/// The primary coefficient of friction parameter.
			float m_fltFrictionPrimary;

			/// The secondary coefficient of friction parameter.
			float m_fltFrictionSecondary;

			/// The maximum primary friction that can created.
			float m_fltMaxFrictionPrimary;

			/// The maximum secondary friction that can created.
			float m_fltMaxFrictionSecondary;

			/// The compliance of the collision between those two materials.
			float m_fltCompliance;

			/// The damping of the collision between those two materials.
			float m_fltDamping;

			/// The restitution of the collision between those two materials.
			float m_fltRestitution;

			/// The primary slip of the collision between those two materials.
			float m_fltSlipPrimary;

			/// The secondary slip of the collision between those two materials.
			float m_fltSlipSecondary;

			/// The primary slide of the collision between those two materials.
			float m_fltSlidePrimary;

			/// The primary slide of the collision between those two materials.
			float m_fltSlideSecondary;

			/// The maximum adhesion of the collision between those two materials.
			float m_fltMaxAdhesive;

		public:
			MaterialPair();
			virtual ~MaterialPair();
			
			virtual string Material1();
			virtual void Material1(string strMat);

			virtual string Material2();
			virtual void Material2(string strMat);

			virtual float FrictionPrimary();
			virtual void FrictionPrimary(float fltVal);
			virtual float FrictionSecondary();
			virtual void FrictionSecondary(float fltVal);

			virtual float MaxFrictionPrimary();
			virtual void MaxFrictionPrimary(float fltVal, BOOL bUseScaling = TRUE);
			virtual float MaxFrictionSecondary();
			virtual void MaxFrictionSecondary(float fltVal, BOOL bUseScaling = TRUE);

			virtual float SlipPrimary();
			virtual void SlipPrimary(float fltVal, BOOL bUseScaling = TRUE);
			virtual float SlipSecondary();
			virtual void SlipSecondary(float fltVal, BOOL bUseScaling = TRUE);

			virtual float SlidePrimary();
			virtual void SlidePrimary(float fltVal, BOOL bUseScaling = TRUE);
			virtual float SlideSecondary();
			virtual void SlideSecondary(float fltVal, BOOL bUseScaling = TRUE);

			virtual float Compliance();
			virtual void Compliance(float fltVal, BOOL bUseScaling = TRUE);

			virtual float Damping();
			virtual void Damping(float fltVal, BOOL bUseScaling = TRUE);

			virtual float Restitution();
			virtual void Restitution(float fltVal);

			virtual float MaxAdhesive();
			virtual void MaxAdhesive(float fltVal, BOOL bUseScaling = TRUE);

			/**
			\brief	Gets a material identifier used by the physics engine.

			\details This is a pure virtual function that must be overridden in the derived physics class.

			\author	dcofer
			\date	3/23/2011
			
			\param	strName	Name of the string. 
			
			\return	The material identifier.
			**/
			virtual int GetMaterialID(string strName) = 0;

			/**
			\brief	Registers the material types within the physics engine.

			\details This is a pure virtual function that must be overridden in the derived physics class.
			
			\author	dcofer
			\date	3/23/2011
			
			\param	aryMaterialTypes	List of types of the ary materials. 
			**/
			virtual void RegisterMaterialTypes(CStdArray<string> aryMaterialTypes) = 0;

			virtual void CreateDefaultUnits();

			virtual void Load(CStdXml &oXml);
		};

	}			// Visualization
}				//VortexAnimatSim
