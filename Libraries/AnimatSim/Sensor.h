/**
\file	Sensor.h

\brief	Declares the sensor class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
				/**
				\brief	Sensor base class.

				\details This is a base class for sensor parts. The sensor is shown in the editor as a sphere.
				This part is a pure base class that is not meant to be used stand-alone, only for derived class.
				This includes things like odor sensors.
				
				\author	dcofer
				\date	3/10/2011
				**/
				class ANIMAT_PORT Sensor : public RigidBody   
				{
				protected:
					/// The radius of the sensor part. It is shown as a sphere object
					float m_fltRadius;

					/// Number of segments along the latitude direction that are used to build the sphere.
					int m_iLatitudeSegments;

					/// Number of segments along the longtitude direction that are used to build the sphere.
					int m_iLongtitudeSegments;

				public:
					Sensor();
					virtual ~Sensor();
									
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

					virtual void LatitudeSegments(int iVal);
					virtual int LatitudeSegments();
					
					virtual void LongtitudeSegments(int iVal);
					virtual int LongtitudeSegments();

					virtual BOOL AllowRotateDragX();
					virtual BOOL AllowRotateDragY();
					virtual BOOL AllowRotateDragZ();

					virtual void CreateParts();
					virtual void CreateJoints();

					virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
					virtual void Load(CStdXml &oXml);
				};

		}		//Bodies
	}			// Environment
}				//AnimatSim
