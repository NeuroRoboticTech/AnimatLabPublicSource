/**
\file	Torus.h

\brief	Declares the Torus class. 
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
			class ANIMAT_PORT Torus : public RigidBody
			{
			protected:
				/// The outside radius of the torus.
				float m_fltOutsideRadius;

				/// The inside radius of the torus.
				float m_fltInsideRadius;
				
				/// The number of sides used to draw the torus.
				int m_iSides;

				/// The number of rings used to draw the torus.
				int m_iRings;

			public:
				Torus();
				virtual ~Torus();

				/**
				\brief	Gets the outside radius.

				\author	dcofer
				\date	3/4/2011

				\return	outside radius. 
				**/
				virtual float OutsideRadius();

				/**
				\brief	Sets the outside radius.
				
				\author	dcofer
				\date	3/4/2011
				
				\param	fltVal		The new value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void OutsideRadius(float fltVal, BOOL bUseScaling = TRUE);

				/**
				\brief	Gets the inside radius. 

				\author	dcofer
				\date	3/4/2011

				\return	the inside radius. 
				**/
				virtual float InsideRadius();

				/**
				\brief	Inside radius. 

				\author	dcofer
				\date	3/4/2011

				\param	fltVal		The new value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void InsideRadius(float fltVal, BOOL bUseScaling = TRUE);

				virtual int Sides();
				virtual void Sides(int iVal);
				
				virtual void Rings(int iVal);
				virtual int Rings();

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
