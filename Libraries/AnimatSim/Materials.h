// Materials.h: interface for the Materials class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace AnimatSim
{
	namespace Environment
	{

		class ANIMAT_PORT Materials : public AnimatBase 
		{
		protected:
			MaterialPair *m_lpPair;

			CStdArray<string> m_aryMaterialTypes;
			CStdPtrArray<MaterialPair> m_aryMaterialPairs;

			virtual MaterialPair *LoadMaterialPair(CStdXml &oXml);

			virtual void CreateDefaultMaterial();
			virtual void RegisterMaterials();

		public:
			Materials();
			virtual ~Materials();

			virtual int GetMaterialID(string strID);

			virtual void Reset();
			virtual void Initialize();

			virtual void Load(CStdXml &oXml);
		};

	}			// Visualization
}				//VortexAnimatSim
