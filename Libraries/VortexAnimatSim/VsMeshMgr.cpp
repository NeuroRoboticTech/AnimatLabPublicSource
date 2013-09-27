/**
\file	VsLight.cpp

\brief	Implements the vortex Light class.
**/

#include "StdAfx.h"
#include "VsMeshMgr.h"

namespace VortexAnimatSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	4/25/2011
**/
VsMeshMgr::VsMeshMgr()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	4/25/2011
**/
VsMeshMgr::~VsMeshMgr()
{
	try
	{
		m_aryMeshes.RemoveAll();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsMeshMgr\r\n", "", -1, false, true);}
}

std::string VsMeshMgr::FileCreateTime(std::string strFilename)
{
    std::stringstream ss;
    WIN32_FILE_ATTRIBUTE_DATA wfad;
    SYSTEMTIME st;
     
    GetFileAttributesEx(strFilename.c_str(), GetFileExInfoStandard, &wfad);
    FileTimeToSystemTime(&wfad.ftLastWriteTime, &st);
 
    ss << st.wMonth << '/' << st.wDay << '/' << st.wYear << " " << st.wHour << ":" << st.wMinute << ":" << st.wSecond;
     
    return ss.str();
}

osg::Node *VsMeshMgr::LoadMesh(std::string strFilename)
{
	std::pair<std::string, osg::ref_ptr<osg::Node>> MeshPair;
	
	//If not found then load it.
	if(!FindMesh(strFilename, MeshPair, false))
		return AddMesh(strFilename);
	else
	{
		std::string strCreateDate = FileCreateTime(strFilename);

		//If the create dates match then use this one.
		//If they do not then lets reload it.
		if(strCreateDate == MeshPair.first)
			return MeshPair.second.get();
		else
		{
			ReleaseMesh(strFilename);
			return AddMesh(strFilename);
		}
	}
}

osg::Node *VsMeshMgr::AddMesh(std::string strFilename)
{
	osg::ref_ptr<osg::Node> lpTempMesh = osgDB::readNodeFile(strFilename.c_str());

	if(lpTempMesh)
	{
		std::string strFileCreateTime = FileCreateTime(strFilename);
		std::pair<std::string, osg::ref_ptr<osg::Node>> MeshPair2(strFileCreateTime, lpTempMesh);

		m_aryMeshes.Add(Std_CheckString(strFilename), MeshPair2);
		return lpTempMesh.get();
	}
	else
		return NULL;

}

void VsMeshMgr::ReleaseMesh(std::string strFilename)
{
	int iIndex=0;
	std::pair<std::string, osg::ref_ptr<osg::Node>> MeshPair;
	if(!FindMesh(strFilename, MeshPair, false)) return;
	
	MeshPair.second.release();

	m_aryMeshes.Remove(Std_CheckString(strFilename));
}

bool VsMeshMgr::ContainesMesh(std::string strFilename)
{
	std::pair<std::string, osg::ref_ptr<osg::Node>> MeshPair;

	if(FindMesh(strFilename, MeshPair, false))
		return true;
	else
		return false;
}

/**
\brief	Searches for a mesh that matches the specified filename.

\author	dcofer
\date	3/18/2011

\param	strFilename	filename of the mesh to find.
\param	bThrowError	If true and the ID is not found then it throws an error, otherwise it does nothing.

\return	null if it the column is not found and bThrowError is false, else a pointer to the found column.
**/
bool VsMeshMgr::FindMesh(std::string strFilename, std::pair<std::string, osg::ref_ptr<osg::Node>> &MeshPair, bool bThrowError)
{
	CStdMap<std::string, std::pair<std::string, osg::ref_ptr<osg::Node>> >::iterator oPos;
	oPos = m_aryMeshes.find(Std_CheckString(strFilename));

	if(oPos != m_aryMeshes.end())
	{
		MeshPair =  oPos->second;
		return true;
	}
	else if(bThrowError)
		THROW_TEXT_ERROR(Vs_Err_lMeshIDNotFound, Vs_Err_strMeshNotFound, " Mesh Filename: " + strFilename);

	return false;
}


}				//VortexAnimatSim
