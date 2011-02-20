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

			virtual MaterialPair *LoadMaterialPair(Simulator *lpSim, CStdXml &oXml);

			virtual void CreateDefaultMaterial(Simulator *lpSim);
			virtual void RegisterMaterials(Simulator *lpSim);

		public:
			Materials();
			virtual ~Materials();

			virtual int GetMaterialID(string strID);

			virtual void Reset();
			virtual void Initialize(Simulator *lpSim);

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
		};

	}			// Visualization
}				//VortexAnimatSim
