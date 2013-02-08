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
				virtual void LowerRadius(float fltVal, BOOL bUseScaling = TRUE);

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
				virtual void UpperRadius(float fltVal, BOOL bUseScaling = TRUE);

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
				virtual void Height(float fltVal, BOOL bUseScaling = TRUE);
				
				virtual void Sides(int iVal);
				virtual int Sides();

				virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
