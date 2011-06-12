/**
\file	Ellipsoid.h

\brief	Declares the ellipsoid class. 
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
			class ANIMAT_PORT Ellipsoid : public RigidBody
			{
			protected:
				/// The radius of the major axis of the ellipsoid.
				float m_fltMajorRadius;

				/// The radius of the minor axis of the ellipsoid.
				float m_fltMinorRadius;
				
				/// The number of segments used to draw in the latitude direction.
				int m_iLatSegments;

				/// The number of segments used to draw in the longtitude direction.
				int m_iLongSegments;

			public:
				Ellipsoid();
				virtual ~Ellipsoid();

				/**
				\brief	Gets the major axis radius.

				\author	dcofer
				\date	3/4/2011

				\return	major axis radius. 
				**/
				virtual float MajorRadius();

				/**
				\brief	Sets the major axis radius.
				
				\author	dcofer
				\date	3/4/2011
				
				\param	fltVal		The new value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void MajorRadius(float fltVal, BOOL bUseScaling = TRUE);

				/**
				\brief	Gets the minor axis radius. 

				\author	dcofer
				\date	3/4/2011

				\return	the minor axis radius. 
				**/
				virtual float MinorRadius();

				/**
				\brief	Minor axis radius. 

				\author	dcofer
				\date	3/4/2011

				\param	fltVal		The new value. 
				\param	bUseScaling	true to use unit scaling on entered value. 
				**/
				virtual void MinorRadius(float fltVal, BOOL bUseScaling = TRUE);

				virtual int LatSegments();
				virtual void LatSegments(int iVal);
				
				virtual void LongSegments(int iVal);
				virtual int LongSegments();

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
