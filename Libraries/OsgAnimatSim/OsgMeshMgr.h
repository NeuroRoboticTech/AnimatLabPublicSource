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
		CStdMap<string, pair<string, osg::ref_ptr<osg::Node>> > m_aryMeshes;

		string FileCreateTime(string strFilename);
		osg::Node *AddMesh(string strFilename);

	public:
		OsgMeshMgr();
		virtual ~OsgMeshMgr();

		osg::Node *LoadMesh(string strFilename);
		void ReleaseMesh(string strFilename);
		bool ContainesMesh(string strFilename);
		bool OsgMeshMgr::FindMesh(string strFilename, pair<string, osg::ref_ptr<osg::Node>> &MeshPair, bool bThrowError = true);
	};

}				//OsgAnimatSim
