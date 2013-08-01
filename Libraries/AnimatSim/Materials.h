/**
\file	Materials.h

\brief	Declares the materials class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		/**
		\brief	Keeps track of all of the materials and the various material pair interaction parameters for the entire simulation. 

		\details Materials can be defined within the simulation that have a unique name. RigidBody parts can then specify that it uses that
		particular material using its unique name. The material defines how it interacts with the rest of the parts within the simulation. 
		This includes things like friction, slip, and so on.
		
		\author	dcofer
		\date	3/23/2011
		**/
		class ANIMAT_PORT Materials : public AnimatBase 
		{
		protected:
			CStdPtrArray<MaterialType> m_aryMaterialTypes;

			virtual void LoadMaterialTypes(CStdXml &oXml);
			virtual MaterialType *LoadMaterialType(CStdXml &oXml);

			virtual void AddMaterialType(string strXml, bool bDoNotInit);
			virtual void RemoveMaterialType(string strID, bool bThrowError = true);

			virtual int FindTypeListPos(string strID, bool bThrowError = true);

			virtual void CreateDefaultMaterial();

		public:
			Materials();
			virtual ~Materials();

			virtual void Reset();
			virtual void Initialize();

#pragma region DataAccesMethods

			virtual bool AddItem(const string &strItemType, const string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const string &strItemType, const string &strID, bool bThrowError = true);

#pragma endregion

			virtual void Load(CStdXml &oXml);
		};

	}			// Visualization
}				//VortexAnimatSim
