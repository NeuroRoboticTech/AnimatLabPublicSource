#include "StdAfx.h"
#include "OsgCameraManipulator.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgStructure.h"
#include "OsgLight.h"
#include "OsgUserData.h"
#include "OsgSimulationWindow.h"
#include "OsgScriptedSimulationWindow.h"
#include "OsgMouseSpring.h"
#include "OsgDraggerHandler.h"
#include "OsgDragger.h"
#include "OsgSimulator.h"

namespace OsgAnimatSim
{
	namespace Visualization
	{

OsgScriptedSimulationWindow::OsgScriptedSimulationWindow()
{
	m_lpCurrentPath = NULL;
	//Scripted sim window always has track camera as true.
	m_bTrackCamera = true;
	m_lpDefaultTrackBody = NULL;
	m_lpOriginalTrackBody = NULL;
}

OsgScriptedSimulationWindow::~OsgScriptedSimulationWindow(void)
{
}
/**
\brief	Gets the local position. (m_oPosition) 

\author	dcofer
\date	3/2/2011

\return	returns m_oPosition. 
**/
CStdFPoint OsgScriptedSimulationWindow::DefaultPosition() {return m_vDefaultPosition;}

/**
\brief	Sets the local position. (m_oPosition) 

\author	dcofer
\date	3/2/2011

\param [in,out]	oPoint		The new point to use to set the local position. 
\param	bUseScaling			If true then the position values that are passed in will be scaled by
							the unit scaling values. 
**/
void OsgScriptedSimulationWindow::DefaultPosition(CStdFPoint &oPoint, bool bUseScaling) 
{
	CStdFPoint oNewPoint, oReportPosition;
	if(bUseScaling && m_lpSim)
		oNewPoint = oPoint * m_lpSim->InverseDistanceUnits();
	else
		oNewPoint = oPoint;

	m_vDefaultPosition = oNewPoint;

	if(m_lpTrackBody)
	{
		CStdFPoint vTargetPos = m_lpTrackBody->AbsolutePosition();
		SetCameraPositionAndLookAt(m_vDefaultPosition, vTargetPos);			
	}
}

/**
\brief	Sets the local position. (m_oPosition) 

\author	dcofer
\date	3/2/2011

\param	fltX				The x coordinate. 
\param	fltY				The y coordinate. 
\param	fltZ				The z coordinate. 
\param	bUseScaling			If true then the position values that are passed in will be scaled by
							the unit scaling values. 
**/
void OsgScriptedSimulationWindow::DefaultPosition(float fltX, float fltY, float fltZ, bool bUseScaling) 
{
	CStdFPoint vPos(fltX, fltY, fltZ);
	DefaultPosition(vPos, bUseScaling);
}

/**
\brief	Sets the local position. (m_oPosition). This method is primarily used by the GUI to
reset the local position using an xml data packet. 

\author	dcofer
\date	3/2/2011

\param	strXml				The xml string with the data to load in the position. 
\param	bUseScaling			If true then the position values that are passed in will be scaled by
							the unit scaling values. 
**/
void OsgScriptedSimulationWindow::DefaultPosition(string strXml, bool bUseScaling)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Position");
	
	CStdFPoint vPos;
	Std_LoadPoint(oXml, "Position", vPos);
	DefaultPosition(vPos, bUseScaling);
}
		
string OsgScriptedSimulationWindow::DefaultPartID() {return m_strDefaultPartID;}

void OsgScriptedSimulationWindow::DefaultPartID(string strID)
{
	m_strDefaultPartID = strID;

	if(m_lpSim)
	{
		if(Std_IsBlank(m_strDefaultPartID))
			m_lpDefaultTrackBody = NULL;
		else
			m_lpDefaultTrackBody = dynamic_cast<BodyPart *>(m_lpSim->FindByID(m_strDefaultPartID));
	}
}

bool OsgScriptedSimulationWindow::SetData(const string &strDataType, const string &strValue, bool bThrowError)
{
	string strType = Std_CheckString(strDataType);
	
	if(OsgSimulationWindow::SetData(strDataType, strValue, false))
		return true;

	if(strType == "POSITION")
	{
		DefaultPosition(strValue);
		return true;
	}

	if(strType == "DEFAULTPARTID")
	{
		DefaultPartID(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

/**
\brief	Creates and adds a camera path. 

\details This method is primarily used by the GUI to add a new root body to the camera paths

\author	dcofer
\date	2/25/2011

\param	strXml	The xml configuration data packet. 
**/
void OsgScriptedSimulationWindow::AddCameraPath(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("CameraPath");

	OsgLinearPath *lpPath = LoadCameraPath(oXml);
	lpPath->Initialize();
	SortPaths();
}

/**
\brief	Removes the camera path based on ID. 

\details This is primarily used by the GUI to remove the root from the camera path when 
the user does this in the GUI.

\author	dcofer
\date	2/25/2011

\param	strID	GUI ID of the path to remove
\param	bThrowError	If true then throw an error if there is a problem, otherwise return false

\return	true if it succeeds, false if it fails. 
**/
void OsgScriptedSimulationWindow::RemoveCameraPath(string strID, bool bThrowError)
{
	int iIdx = FindCameraPath(strID);
	m_aryCameraPaths.RemoveAt(iIdx);
	SortPaths();
}

int OsgScriptedSimulationWindow::FindCameraPath(string strID, bool bThrowError)
{
	int iCount = m_aryCameraPaths.GetSize();
	for(int iIdx=0; iIdx<iCount; iIdx++)
		if(m_aryCameraPaths[iIdx]->ID() == strID)
			return iIdx;

	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lItemNotFound, Al_Err_strItemNotFound, ("ID: " + strID));

	return -1;
}

void OsgScriptedSimulationWindow::SortPaths()
{
	m_arySortedCameraPaths.RemoveAll();

	int iCount = m_aryCameraPaths.GetSize();
	for(int iIdx=0; iIdx<iCount; iIdx++)
	{
		m_arySortedCameraPaths.Add(m_aryCameraPaths[iIdx]->StartTime(), m_aryCameraPaths[iIdx]); 
	}
}

bool OsgScriptedSimulationWindow::AddItem(const string &strItemType, const string &strXml, bool bThrowError, bool bDoNotInit)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "CAMERAPATH")
	{
		AddCameraPath(strXml);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

bool OsgScriptedSimulationWindow::RemoveItem(const string &strItemType, const string &strID, bool bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "CAMERAPATH")
	{
		RemoveCameraPath(strID);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

void OsgScriptedSimulationWindow::Initialize()
{
	OsgSimulationWindow::Initialize();

	//Try to get the default part to track.
	DefaultPartID(m_strDefaultPartID);

	int iCount = m_aryCameraPaths.GetSize();
	for(int iIdx=0; iIdx<iCount; iIdx++)
		m_aryCameraPaths[iIdx]->Initialize();
}

void OsgScriptedSimulationWindow::SimStarting()
{
	m_lpOriginalTrackBody = m_lpTrackBody;
	m_lpTrackBody = m_lpDefaultTrackBody;

	m_iCurrentPathIter = m_arySortedCameraPaths.begin();

	if(m_arySortedCameraPaths.GetSize() > 0)
		m_lpCurrentPath =  m_iCurrentPathIter->second;
	else
		m_lpCurrentPath = NULL;

	SetCameraPostion(m_vDefaultPosition);
}

void OsgScriptedSimulationWindow::ResetSimulation()
{
	m_lpTrackBody = m_lpDefaultTrackBody;
	TrackCamera();

	m_lpCurrentPath =  NULL;

	int iCount = m_aryCameraPaths.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryCameraPaths[iIndex]->ResetSimulation();
}

void OsgScriptedSimulationWindow::FindNextCameraPath()
{
	if(m_iCurrentPathIter != m_arySortedCameraPaths.end())
	{
		m_iCurrentPathIter++;
		if(m_iCurrentPathIter != m_arySortedCameraPaths.end())
			m_lpCurrentPath = m_iCurrentPathIter->second;
	}
}

void OsgScriptedSimulationWindow::TrackCamera()
{
	if(m_lpSim->SimRunning())
	{
		if(m_lpCurrentPath && m_lpCurrentPath->AfterPathTime(m_lpSim->Time()))
			FindNextCameraPath();
		
		if(m_lpCurrentPath && m_lpCurrentPath->WithinPathTime(m_lpSim->Time()))
		{
			if(m_lpCurrentPath->Interpolate((double) m_lpSim->Time()) && m_lpCurrentPath->TrackBody())
			{
				CStdFPoint oPos = m_lpCurrentPath->TrackBody()->AbsolutePosition();
				osg::Vec3d vTargetPos(oPos.x, oPos.y, oPos.z);

				osg::Vec3d vPos = m_lpCurrentPath->GetInterpPosition();

				//string strMesage = "Time: " + STR(m_lpSim->Time()) + ", Pos: [" + STR(vPos.x()) + ", " +  STR(vPos.y()) + ", " +  STR(vPos.z()) + "]\n";
				//OutputDebugString(strMesage.c_str());

				SetCameraPositionAndLookAt(vPos, vTargetPos);			
			}
		}
	}
	else
		OsgSimulationWindow::TrackCamera();
}

void OsgScriptedSimulationWindow::Update()
{
	if(m_lpTrackBody)
		TrackCamera();

	m_osgViewer->frame(); 
}


void OsgScriptedSimulationWindow::Load(CStdXml &oXml)
{
	OsgSimulationWindow::Load(oXml);

	oXml.IntoElem(); //Into Window Element

	string m_strDefaultStructureID;
	string m_strDefaultPartID;

	DefaultPartID(oXml.GetChildString("LookAtBodyID", m_strDefaultPartID));

	CStdFPoint vPos;
	Std_LoadPoint(oXml, "DefaultPosition", vPos);
	DefaultPosition(vPos);

	if(oXml.FindChildElement("CameraPaths", false))
	{
		oXml.IntoElem(); //IntoOf CameraPaths Element

		int iCount = oXml.NumberOfChildren();
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadCameraPath(oXml);
		}

		oXml.OutOfElem(); //OutOf CameraPaths Element
	}

	oXml.OutOfElem(); //OutOf Window Element

	SortPaths();
}

/**
\brief	Loads the a camera path. 

\author	dcofer
\date	2/25/2011

\param [in,out]	oXml The xml data packet to load. 

\return	The root. 
**/
OsgLinearPath *OsgScriptedSimulationWindow::LoadCameraPath(CStdXml &oXml)
{
	OsgLinearPath *lpSpline = NULL;

try
{
	lpSpline = new OsgLinearPath;

	lpSpline->SetSystemPointers(m_lpSim, NULL, NULL, NULL, true);
	lpSpline->ParentWindow(this);

	lpSpline->Load(oXml);
	m_aryCameraPaths.Add(lpSpline);

	return lpSpline;
}
catch(CStdErrorInfo oError)
{
	if(lpSpline) delete lpSpline;
	lpSpline = NULL;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpSpline) delete lpSpline;
	lpSpline = NULL;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


	}// end Visualization
}// end OsgAnimatSim