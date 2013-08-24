// BlMesh.h: interface for the BlMesh class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class BULLET_PORT BlMeshBase : public BulletAnimatSim::Environment::BlRigidBody
			{
			protected:
				Mesh *m_lpThisMesh;
				osg::ref_ptr<osg::Node> m_osgBaseMeshNode;
				osg::ref_ptr<osg::MatrixTransform> m_osgMeshNode;

				virtual void SetThisPointers();
				virtual void CreateGeometry();
				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void LoadMeshNode();
				virtual void ResizePhysicsGeometry();
				virtual void CreateDefaultMesh();

			public:
				BlMeshBase();
				virtual ~BlMeshBase();

				//Override the set color method so we can disable it. We do not want to set the color for
				//a mesh. Let the color be set in the mesh file.
				//virtual void SetColor(float *vAmbient, float *vDiffuse, float *vSpecular, float fltShininess) {};
				//virtual void SetTexture(string strTexture) {};

				virtual void Physics_Resize();
			};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
