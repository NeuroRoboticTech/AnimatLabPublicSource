
#pragma once


namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsMesh : public AnimatSim::Environment::Bodies::Mesh, public VsMeshBase
			{
			protected:

			public:
				VsMesh();
				virtual ~VsMesh();

				//Override the set color method so we can disable it. We do not want to set the color for
				//a mesh. Let the color be set in the mesh file.
				//virtual void SetColor(float *vAmbient, float *vDiffuse, float *vSpecular, float fltShininess) {};
				//virtual void SetTexture(std::string strTexture) {};

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
