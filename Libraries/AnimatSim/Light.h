/**
\file	Light.h

\brief	Declares a light object. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		/**
		\class	Light
		
		\brief	Base class for the light object.

		\details This is a light object. It is used to add light to a scene.
		
		\author	dcofer
		\date	6/29/2011
		**/
		class ANIMAT_PORT Light : public AnimatBase, public MovableItem
		{
		protected:
			/// The radius of the sphere
			float m_fltRadius;

			/// Number of segments along the latitude direction that are used to build the sphere.
			int m_iLatitudeSegments;

			/// Number of segments along the longtitude direction that are used to build the sphere.
			int m_iLongtitudeSegments;

			/// Zero-based index of the light number. OSG only allows 8 lights.
			int m_iLightNum;

			virtual void UpdateData();

		public:
			Light(void);
			virtual ~Light(void);

#pragma region AccessorMutators

			virtual void Resize();

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

			virtual void LightNumber(int iVal);
			virtual int LightNumber();

#pragma endregion

#pragma region DataAccesMethods

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify);
			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

#pragma endregion

			virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 
			virtual void VisualSelectionModeChanged(int iNewMode);
			virtual void Create();

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
