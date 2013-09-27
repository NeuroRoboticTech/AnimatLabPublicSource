
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsTerrain : public AnimatSim::Environment::Bodies::Terrain, public VsMeshBase  
			{
			protected:
				osg::HeightField *m_osgHeightField;
				Vx::VxHeightField *m_vxHeightField;

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void LoadMeshNode();

			public:
				VsTerrain();
				virtual ~VsTerrain();

				virtual void SetTexture(std::string strTexture);
				virtual void Physics_FluidDataChanged();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
