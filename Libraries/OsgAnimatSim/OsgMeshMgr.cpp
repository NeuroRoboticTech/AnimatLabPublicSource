/**
\file	OsgLight.cpp

\brief	Implements the vortex Light class.
**/

#include "StdAfx.h"
#include "OsgMeshMgr.h"

namespace OsgAnimatSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	4/25/2011
**/
OsgMeshMgr::OsgMeshMgr()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	4/25/2011
**/
OsgMeshMgr::~OsgMeshMgr()
{
	try
	{
		m_aryMeshes.RemoveAll();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of OsgMeshMgr\r\n", "", -1, false, true);}
}

string OsgMeshMgr::FileCreateTime(string strFilename)
{
    stringstream ss;

    //NEED TO TEST
#ifdef WIN32
    WIN32_FILE_ATTRIBUTE_DATA wfad;
    SYSTEMTIME st;
     
    GetFileAttributesEx(strFilename.c_str(), GetFileExInfoStandard, &wfad);
    FileTimeToSystemTime(&wfad.ftLastWriteTime, &st);
 
    ss << st.wMonth << '/' << st.wDay << '/' << st.wYear << " " << st.wHour << ":" << st.wMinute << ":" << st.wSecond;
#else
   struct stat attrib;
   stat(strFilename.c_str(), &attrib);

   
   ss << attrib.st_mtime.tm_yday << '/' << attrib.st_mtime.tm_wday << '/' << attrib.st_mtime.tm_year << " " << attrib.st_mtime.tm_hour << ":" << attrib.st_mtime.tm_minute << ":" << attrib.st_mtime.tm_sec;
#endif

    return ss.str();
}

osg::Node *OsgMeshMgr::LoadMesh(string strFilename)
{
	pair<string, osg::ref_ptr<osg::Node>> MeshPair;
	
	//If not found then load it.
	if(!FindMesh(strFilename, MeshPair, false))
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

osg::Node *OsgMeshMgr::AddMesh(string strFilename)
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

void OsgMeshMgr::ReleaseMesh(string strFilename)
{
	int iIndex=0;
	pair<string, osg::ref_ptr<osg::Node>> MeshPair;
	if(!FindMesh(strFilename, MeshPair, false)) return;
	
	MeshPair.second.release();

	m_aryMeshes.Remove(Std_CheckString(strFilename));
}

bool OsgMeshMgr::ContainesMesh(string strFilename)
{
	pair<string, osg::ref_ptr<osg::Node>> MeshPair;

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
bool OsgMeshMgr::FindMesh(string strFilename, pair<string, osg::ref_ptr<osg::Node>> &MeshPair, bool bThrowError)
{
	CStdMap<string, pair<string, osg::ref_ptr<osg::Node>> >::iterator oPos;
	oPos = m_aryMeshes.find(Std_CheckString(strFilename));

	if(oPos != m_aryMeshes.end())
	{
		MeshPair =  oPos->second;
		return true;
	}
	else if(bThrowError)
		THROW_TEXT_ERROR(Osg_Err_lMeshIDNotFound, Osg_Err_strMeshNotFound, " Mesh Filename: " + strFilename);

	return false;
}


}				//OsgAnimatSim
