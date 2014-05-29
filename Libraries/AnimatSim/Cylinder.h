/**
\file	Cylinder.h

\brief	Declares the cylinder class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
			/**
			\brief	the Cylinder base class. 
			
			\details This is a cylinder type of rigid body. You can specify the dimensions of 
		     the radius and height model.

			\author	dcofer
			\date	3/10/2011
			**/
			class ANIMAT_PORT Cylinder : public RigidBody
			{
			protected:
				/// The radius of the cylinder
				float m_fltRadius;

				/// The height of the cylinder
				float m_fltHeight;

				/// The number of sides used to draw the cylinder.
				int m_iSides;

			public:
				Cylinder();
				virtual ~Cylinder();
			
				static Cylinder *CastToDerived(AnimatBase *lpBase) {return static_cast<Cylinder*>(lpBase);}

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

				/**
				\brief	Gets the height. 

				\author	dcofer
				\date	3/4/2011

				\return	The height. 
				**/
				virtual float Height();

				/**
				\brief	Sets the Height. 

				\author	dcofer
				\date	3/4/2011

				\param	fltVal		The new value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void Height(float fltVal, bool bUseScaling = true);

				virtual void Sides(int iVal);
				virtual int Sides();

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
