// VsMaterialType.h: interface for the VsMaterialType class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{

		class VORTEX_PORT VsMaterialType : public AnimatSim::Environment::MaterialType
		{
		protected:
			VxMaterialTable *m_vxMaterialTable;
            VxMaterial *m_vxMaterial;

			virtual void SetMaterialProperties();
			virtual void RegisterMaterialType();

		public:
			VsMaterialType();
			virtual ~VsMaterialType();

			virtual int GetMaterialID(std::string strName);
			virtual void Initialize();
		};

	}			// Visualization
}				//VortexAnimatSim
