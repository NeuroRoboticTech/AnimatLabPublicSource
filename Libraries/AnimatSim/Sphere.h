/**
\file	Sphere.h

\brief	Declares the sphere class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
			/**
			\brief	Sphere. 

			\details This is a Sphere type of rigid body. You can specify the dimensions of 
				the radius for the model.
			
			\author	dcofer
			\date	3/10/2011
			**/
			class ANIMAT_PORT Sphere : public RigidBody
			{
			protected:
				/// The radius of the sphere
				float m_fltRadius;

			public:
				Sphere();
				virtual ~Sphere();
				
				/**
				\brief	Gets the radius. 

				\author	dcofer
				\date	3/4/2011

				\return	the radius. 
				**/
				virtual float Radius();

				/**
				\brief	Sets the radius. 

				\author	dcofer
				\date	3/4/2011

				\param	fltVal		The new value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void Radius(float fltVal, BOOL bUseScaling = TRUE);

				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim

