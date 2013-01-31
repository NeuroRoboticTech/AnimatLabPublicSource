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

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
