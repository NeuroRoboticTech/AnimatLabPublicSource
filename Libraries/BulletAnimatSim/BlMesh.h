
#pragma once


namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class BULLET_PORT BlMesh : public AnimatSim::Environment::Bodies::Mesh, public BlMeshBase
			{
			protected:
                virtual void CalculateVolumeAndAreas();

			public:
				BlMesh();
				virtual ~BlMesh();

    			virtual bool Freeze();
				virtual BoundingBox Physics_GetBoundingBox();

				//Override the set color method so we can disable it. We do not want to set the color for
				//a mesh. Let the color be set in the mesh file.
				//virtual void SetColor(float *vAmbient, float *vDiffuse, float *vSpecular, float fltShininess) {};
				//virtual void SetTexture(std::string strTexture) {};

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
