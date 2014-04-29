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

				/// Number of segments along the latitude direction that are used to build the sphere.
				int m_iLatitudeSegments;

				/// Number of segments along the longtitude direction that are used to build the sphere.
				int m_iLongtitudeSegments;

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
				virtual void Radius(float fltVal, bool bUseScaling = true);

				virtual void LatitudeSegments(int iVal);
				virtual int LatitudeSegments();
				
				virtual void LongtitudeSegments(int iVal);
				virtual int LongtitudeSegments();

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim

