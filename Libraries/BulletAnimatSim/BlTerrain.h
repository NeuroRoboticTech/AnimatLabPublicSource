
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

                /// Standard bullet motion state pointer.
                btDefaultMotionState *m_btMotionState;

                /// The minimum height returned by the CreateHeightField method.
                float m_fltMinTerrainHeight;

                /// The maximum height returned by the CreateHeightField method.
                float m_fltMaxTerrainHeight;

                //The adjustment height to set the btPart position so it will match the
                //position of the osg terrain graphics.
                float m_fltTerrainHeightAdjust;

                /// The array of height data used by the btHeightfieldTerrainShape
                float *m_aryHeightData;

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void LoadMeshNode();

                virtual void CreateDynamicPart();
                virtual void DeleteDynamicPart();

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
