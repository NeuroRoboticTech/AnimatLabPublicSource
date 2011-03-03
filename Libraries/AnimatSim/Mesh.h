
#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class ANIMAT_PORT Mesh : public RigidBody
			{
			protected:
				CStdFPoint m_oCollisionBoxSize;
				CStdFPoint m_oGraphicsBoxSize;

				string m_strGraphicsMesh;
				string m_strCollisionMesh;
				string m_strReceptiveFieldMesh;
				string m_strCollisionMeshType;

			public:
				//This is a test comment.
				Mesh();
				virtual ~Mesh();

				CStdFPoint CollisionBoxSize() {return m_oCollisionBoxSize;};
				CStdFPoint GraphicsBoxSize() {return m_oGraphicsBoxSize;};

				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
