
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
				string m_strMeshFile;
				string m_strCollisionMeshType;

			public:
				//This is a test comment.
				Mesh();
				virtual ~Mesh();
				
				virtual string MeshFile();
				virtual void MeshFile(string strFile);

				virtual string CollisionMeshType();
				virtual void CollisionMeshType(string strType);

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
