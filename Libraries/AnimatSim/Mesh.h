
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
				/// The graphical mesh file to load.
				string m_strMeshFile;

				/// Type of the collision mesh. Can be Convex or Triangular
				string m_strCollisionMeshType;

				/// The collsion mesh file. If the type is convex then we load in this file instead of the graphical one.
				string m_strConvexMeshFile;

			public:
				//This is a test comment.
				Mesh();
				virtual ~Mesh();
				
				virtual string MeshFile();
				virtual void MeshFile(string strFile);

				virtual string CollisionMeshType();
				virtual void CollisionMeshType(string strType);

				virtual string ConvexMeshFile();
				virtual void ConvexMeshFile(string strFile);

				virtual void SetMeshFile(string strXml);

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
