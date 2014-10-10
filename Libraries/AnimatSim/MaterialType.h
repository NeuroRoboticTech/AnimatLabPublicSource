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

			static MaterialType *CastToDerived(AnimatBase *lpBase) {return static_cast<MaterialType*>(lpBase);}

			/**
			\brief	Gets a material identifier used by the physics engine.

			\details This is a pure virtual function that must be overridden in the derived physics class.

			\author	dcofer
			\date	3/23/2011
			
			\param	strName	Name of the string. 
			
			\return	The material identifier.
			**/
			virtual int GetMaterialID(std::string strName) = 0;

			virtual void CreateDefaultUnits() = 0;
        };

	}			// Visualization
}				//VortexAnimatSim
