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
		CStdMap<string, pair<string, osg::ref_ptr<osg::Node>> > m_aryMeshes;

		string FileCreateTime(string strFilename);
		osg::Node *AddMesh(string strFilename);

	public:
		VsMeshMgr();
		virtual ~VsMeshMgr();

		osg::Node *LoadMesh(string strFilename);
		void ReleaseMesh(string strFilename);
		bool ContainesMesh(string strFilename);
		bool VsMeshMgr::FindMesh(string strFilename, pair<string, osg::ref_ptr<osg::Node>> &MeshPair, BOOL bThrowError = TRUE);
	};

}				//VortexAnimatSim
