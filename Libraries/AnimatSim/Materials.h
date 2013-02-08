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
		This includes things like friction, slip, and so on. The Materials object also keeps track of all MaterialPair combinations. For each
		Material that is defined there must be MaterialPair objects defined for all combinations of that material and the other materials 
		that have been defined. This lets the simulator know how to react if a material of type 1 collides with a material of type 2. This 
		object keeps a list of all of these MaterialPairs and registers them with the Physics engine.
		
		\author	dcofer
		\date	3/23/2011
		**/
		class ANIMAT_PORT Materials : public AnimatBase 
		{
		protected:
			CStdPtrArray<MaterialType> m_aryMaterialTypes;
			CStdPtrArray<MaterialPair> m_aryMaterialPairs;

			virtual void Materials::LoadMaterialTypes(CStdXml &oXml);
			virtual MaterialType *Materials::LoadMaterialType(CStdXml &oXml);
			virtual void Materials::LoadMaterialPairs(CStdXml &oXml);
			virtual MaterialPair *LoadMaterialPair(CStdXml &oXml);

			virtual void AddMaterialType(string strXml, BOOL bDoNotInit);
			virtual void RemoveMaterialType(string strID, BOOL bThrowError = TRUE);
			
			virtual void AddMaterialPair(string strXml, BOOL bDoNotInit);
			virtual void RemoveMaterialPair(string strID, BOOL bThrowError = TRUE);

			virtual int FindTypeListPos(string strID, BOOL bThrowError = TRUE);
			virtual int FindPairListPos(string strID, BOOL bThrowError = TRUE);

			virtual void CreateDefaultMaterial();

		public:
			Materials();
			virtual ~Materials();

			virtual void Reset();
			virtual void Initialize();

#pragma region DataAccesMethods

			virtual BOOL AddItem(const string &strItemType, const string &strXml, BOOL bThrowError = TRUE, BOOL bDoNotInit = FALSE);
			virtual BOOL RemoveItem(const string &strItemType, const string &strID, BOOL bThrowError = TRUE);

#pragma endregion

			virtual void Load(CStdXml &oXml);
		};

	}			// Visualization
}				//VortexAnimatSim
