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
	{Std_TraceMsg(0, "Caught Error in desctructor of VsMeshMgr\r\n", "", -1, FALSE, TRUE);}
}

string VsMeshMgr::FileCreateTime(string strFilename)
{
    stringstream ss;
    WIN32_FILE_ATTRIBUTE_DATA wfad;
    SYSTEMTIME st;
     
    GetFileAttributesEx(strFilename.c_str(), GetFileExInfoStandard, &wfad);
    FileTimeToSystemTime(&wfad.ftLastWriteTime, &st);
 
    ss << st.wMonth << '/' << st.wDay << '/' << st.wYear << " " << st.wHour << ":" << st.wMinute << ":" << st.wSecond;
     
    return ss.str();
}

osg::Node *VsMeshMgr::LoadMesh(string strFilename)
{
	pair<string, osg::ref_ptr<osg::Node>> MeshPair;
	
	//If not found then load it.
	if(!FindMesh(strFilename, MeshPair, FALSE))
		return AddMesh(strFilename);
	else
	{
		string strCreateDate = FileCreateTime(strFilename);

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

osg::Node *VsMeshMgr::AddMesh(string strFilename)
{
	osg::ref_ptr<osg::Node> lpTempMesh = osgDB::readNodeFile(strFilename.c_str());

	if(lpTempMesh)
	{
		string strFileCreateTime = FileCreateTime(strFilename);
		pair<string, osg::ref_ptr<osg::Node>> MeshPair2(strFileCreateTime, lpTempMesh);

		m_aryMeshes.Add(Std_CheckString(strFilename), MeshPair2);
		return lpTempMesh.get();
	}
	else
		return NULL;

}

void VsMeshMgr::ReleaseMesh(string strFilename)
{
	int iIndex=0;
	pair<string, osg::ref_ptr<osg::Node>> MeshPair;
	if(!FindMesh(strFilename, MeshPair, FALSE)) return;
	
	MeshPair.second.release();

	m_aryMeshes.Remove(Std_CheckString(strFilename));
}

bool VsMeshMgr::ContainesMesh(string strFilename)
{
	pair<string, osg::ref_ptr<osg::Node>> MeshPair;

	if(FindMesh(strFilename, MeshPair, FALSE))
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
bool VsMeshMgr::FindMesh(string strFilename, pair<string, osg::ref_ptr<osg::Node>> &MeshPair, BOOL bThrowError)
{
	CStdMap<string, pair<string, osg::ref_ptr<osg::Node>> >::iterator oPos;
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
