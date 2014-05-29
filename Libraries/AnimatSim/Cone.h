/**
\file	Cone.h

\brief	Declares the cone class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
			/**
			\brief	The Cone base class.
			
			\details This is a Cone type of rigid body. You can specify the dimensions of the radius and
			height for both the collision model and for the graphics model. 
			
			\author	dcofer
			\date	3/4/2011
			**/
			class ANIMAT_PORT Cone : public RigidBody
			{
			protected:
				/// The lower radius of the cone
				float m_fltLowerRadius;

				/// The upper radius of the cone
				float m_fltUpperRadius;
				
				/// The height of the cone
				float m_fltHeight;

				/// The number of sides used to draw the cone.
				int m_iSides;

			public:
				Cone();
				virtual ~Cone();

				static Cone *CastToDerived(AnimatBase *lpBase) {return static_cast<Cone*>(lpBase);}

				/**
				\brief	Gets the lower radius. 

				\author	dcofer
				\date	3/4/2011

				\return	Lower radius. 
				**/
				virtual float LowerRadius();

				/**
				\brief	Sets the lower radius. 
				
				\author	dcofer
				\date	3/4/2011
				
				\param	fltVal		The new value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void LowerRadius(float fltVal, bool bUseScaling = true);

				/**
				\brief	Gets the upper radius. 

				\author	dcofer
				\date	3/4/2011

				\return	the upper radius. 
				**/
				virtual float UpperRadius();

				/**
				\brief	Upper radius. 

				\author	dcofer
				\date	3/4/2011

				\param	fltVal		The new value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void UpperRadius(float fltVal, bool bUseScaling = true);

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
