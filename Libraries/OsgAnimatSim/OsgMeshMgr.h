/**
\file	OsgMeshMgr.h

\brief	Declares the vortex MeshMgr class.
**/

#pragma once

namespace OsgAnimatSim
{

	/**
	\brief	Vortex physical structure implementation. 
		
	\author	dcofer
	\date	4/25/2011
	**/
	class ANIMAT_OSG_PORT OsgMeshMgr   
	{
	protected:
		CStdMap<std::string, std::pair<std::string, osg::ref_ptr<osg::Node>> > m_aryMeshes;

		std::string FileCreateTime(std::string strFilename);
		osg::Node *AddMesh(std::string strFilename);

	public:
		OsgMeshMgr();
		virtual ~OsgMeshMgr();

		osg::Node *LoadMesh(std::string strFilename);
		void ReleaseMesh(std::string strFilename);
		bool ContainesMesh(std::string strFilename);
		bool FindMesh(std::string strFilename, std::pair<std::string, osg::ref_ptr<osg::Node>> &MeshPair, bool bThrowError = true);
	};

}				//OsgAnimatSim
