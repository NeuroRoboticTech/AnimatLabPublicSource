// BlMaterialType.h: interface for the BlMaterialType class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{

		class BULLET_PORT BlMaterialType : public AnimatSim::Environment::MaterialType
		{
		protected:
            //FIX PHYSICS
			//VxMaterialTable *m_vxMaterialTable;
   //         VxMaterial *m_vxMaterial;

			virtual void SetMaterialProperties();
			virtual void RegisterMaterialType();

		public:
			BlMaterialType();
			virtual ~BlMaterialType();

			virtual int GetMaterialID(string strName);
			virtual void Initialize();
		};

	}			// Visualization
}				//BulletAnimatSim
