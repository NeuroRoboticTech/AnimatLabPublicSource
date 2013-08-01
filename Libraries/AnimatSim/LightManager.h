/**
\file	LightManager.h

\brief	Declares a light manager object. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		/**
		\class	LightManager
		
		\brief	Light manager class.

		\details This class manages the lights within the scene.
		
		\author	dcofer
		\date	6/29/2011
		**/
		class ANIMAT_PORT LightManager : public AnimatBase
		{
		protected:
			/// Array of Light objects.
			CStdPtrArray<Light> m_aryLights;
			
			/// The maximum  number of lights that can be added to the scene.
			int m_iMaxLights;

			Light *LoadLight(CStdXml &oXml);

			virtual void SetupLights();
			virtual void AddLight(string strXml);
			virtual void RemoveLight(string strID, bool bThrowError = true);

		public:
			LightManager(void);
			virtual ~LightManager(void);

#pragma region AccessorMutators

			/**
			\brief	Gets the lights array.
			
			\author	dcofer
			\date	3/25/2011
			
			\return	pointer to the array.
			**/
			virtual CStdPtrArray<Light> *Lights() {return &m_aryLights;};

#pragma endregion

#pragma region DataAccesMethods

			virtual bool AddItem(const string &strItemType, const string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const string &strItemType, const string &strID, bool bThrowError = true);
			virtual int FindChildListPos(string strID, bool bThrowError = true);

#pragma endregion

			virtual void Initialize();
			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
