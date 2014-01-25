
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class BULLET_PORT BlTerrain : public AnimatSim::Environment::Bodies::Terrain, public BlMeshBase  
			{
			protected:
				osg::HeightField *m_osgHeightField;
				btHeightfieldTerrainShape *m_btHeightField;

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void LoadMeshNode();

                virtual void CreateDynamicPart();

			public:
				BlTerrain();
				virtual ~BlTerrain();

				virtual void SetTexture(std::string strTexture);
				virtual void Physics_FluidDataChanged();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
