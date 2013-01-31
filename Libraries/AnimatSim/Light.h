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
			///Determines if light is enabled or not.
			BOOL m_bEnabled;

			/// The radius of the sphere
			float m_fltRadius;

			/// Number of segments along the latitude direction that are used to build the sphere.
			int m_iLatitudeSegments;

			/// Number of segments along the longtitude direction that are used to build the sphere.
			int m_iLongtitudeSegments;

			/// Zero-based index of the light number. OSG only allows 8 lights.
			int m_iLightNum;

			/// The constant attenuation ratio
			float m_fltConstantAttenRatio;

			/// The linear attenuation distance
			float m_fltLinearAttenDistance;

			/// The quadratic attenuation distance
			float m_fltQuadraticAttenDistance;

			virtual void UpdateData();

		public:
			Light(void);
			virtual ~Light(void);

#pragma region AccessorMutators

			virtual BOOL Enabled();
			virtual void Enabled(BOOL bValue);

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

			virtual void ConstantAttenRatio(float fltVal);
			virtual float ConstantAttenRatio();

			virtual void LinearAttenDistance(float fltVal, BOOL bUseScaling = TRUE);
			virtual float LinearAttenDistance();

			virtual void QuadraticAttenDistance(float fltVal, BOOL bUseScaling = TRUE);
			virtual float QuadraticAttenDistance();

#pragma endregion

#pragma region DataAccesMethods

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify);
			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

			virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 
			virtual void VisualSelectionModeChanged(int iNewMode);
			virtual void Create();

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
