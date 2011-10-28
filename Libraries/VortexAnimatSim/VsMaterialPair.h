// VsMaterialPair.h: interface for the VsMaterialPair class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{

		class VORTEX_PORT VsMaterialPair : public AnimatSim::Environment::MaterialPair
		{
		protected:
			VxMaterialTable *m_vxMaterialTable;

			virtual void SetMaterialProperties();

		public:
			VsMaterialPair();
			virtual ~VsMaterialPair();

			virtual int GetMaterialID(string strName);
			virtual void RegisterMaterialType(string strID);
			virtual void Initialize();
		};

	}			// Visualization
}				//VortexAnimatSim
