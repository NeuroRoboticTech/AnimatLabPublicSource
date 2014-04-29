
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
				std::string m_strMeshFile;

				/// Type of the collision mesh. Can be Convex or Triangular
				std::string m_strCollisionMeshType;

				/// The collsion mesh file. If the type is convex then we load in this file instead of the graphical one.
				std::string m_strConvexMeshFile;

				CStdFPoint m_vScale;

			public:
				//This is a test comment.
				Mesh();
				virtual ~Mesh();
				
				virtual std::string MeshFile();
				virtual void MeshFile(std::string strFile);

				virtual std::string CollisionMeshType();
				virtual void CollisionMeshType(std::string strType);

				virtual std::string ConvexMeshFile();
				virtual void ConvexMeshFile(std::string strFile);

				virtual void SetMeshFile(std::string strXml);

				virtual void Scale(CStdFPoint &vScale, bool bUpdateMatrix = true);
				virtual void Scale(std::string strXml, bool bUpdateMatrix = true);
				virtual void Scale(float fltX, float fltY, float fltZ, bool bUpdateMatrix = true); 
				virtual CStdFPoint Scale();

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
