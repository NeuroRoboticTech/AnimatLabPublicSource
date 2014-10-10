/**
\file	VsMeshMgr.h

\brief	Declares the vortex MeshMgr class.
**/

#pragma once

namespace VortexAnimatSim
{

	/**
	\brief	Vortex physical structure implementation. 
		
	\author	dcofer
	\date	4/25/2011
	**/
	class VORTEX_PORT VsMeshMgr   
	{
	protected:
		CStdMap<std::string, std::pair<std::string, osg::ref_ptr<osg::Node>> > m_aryMeshes;

		std::string FileCreateTime(std::string strFilename);
		osg::Node *AddMesh(std::string strFilename);

	public:
		VsMeshMgr();
		virtual ~VsMeshMgr();

		osg::Node *LoadMesh(std::string strFilename);
		void ReleaseMesh(std::string strFilename);
		bool ContainesMesh(std::string strFilename);
		bool VsMeshMgr::FindMesh(std::string strFilename, std::pair<std::string, osg::ref_ptr<osg::Node>> &MeshPair, bool bThrowError = true);
	};

}				//VortexAnimatSim
