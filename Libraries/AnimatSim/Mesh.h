
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

				CStdFPoint m_vScale;

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

				virtual void Scale(CStdFPoint &vScale, bool bUpdateMatrix = true);
				virtual void Scale(string strXml, bool bUpdateMatrix = true);
				virtual void Scale(float fltX, float fltY, float fltZ, bool bUpdateMatrix = true); 
				virtual CStdFPoint Scale();

				virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
