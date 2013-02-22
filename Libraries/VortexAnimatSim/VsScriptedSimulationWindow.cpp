#include "stdafx.h"
#include "VsMouseSpring.h"
#include "VsCameraManipulator.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsStructure.h"
#include "VsLight.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsSimulationWindow.h"
#include "VsScriptedSimulationWindow.h"
#include "VsMouseSpring.h"
#include "VsDraggerHandler.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

VsScriptedSimulationWindow::VsScriptedSimulationWindow()
{
}

VsScriptedSimulationWindow::~VsScriptedSimulationWindow(void)
{
}


/**
\brief	Creates and adds a camera path. 

\details This method is primarily used by the GUI to add a new root body to the camera paths

\author	dcofer
\date	2/25/2011

\param	strXml	The xml configuration data packet. 
**/
void VsScriptedSimulationWindow::AddCameraPath(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("CameraPath");

	OsgCubicSpline *lpPath = LoadCameraPath(oXml);
	lpPath->Initialize();
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
void VsScriptedSimulationWindow::RemoveCameraPath(string strID, BOOL bThrowError)
{
	int iIdx = FindCameraPath(strID);
	m_aryCameraPaths.RemoveAt(iIdx);
}

int VsScriptedSimulationWindow::FindCameraPath(string strID, BOOL bThrowError)
{
	int iCount = m_aryCameraPaths.GetSize();
	for(int iIdx=0; iIdx<iCount; iIdx++)
		if(m_aryCameraPaths[iIdx]->ID() == strID)
			return iIdx;

	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lItemNotFound, Al_Err_strItemNotFound, ("ID: " + strID));

	return -1;
}

BOOL VsScriptedSimulationWindow::AddItem(const string &strItemType, const string &strXml, BOOL bThrowError, BOOL bDoNotInit)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "CAMERAPATH")
	{
		AddCameraPath(strXml);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL VsScriptedSimulationWindow::RemoveItem(const string &strItemType, const string &strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "CAMERAPATH")
	{
		RemoveCameraPath(strID);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

void VsScriptedSimulationWindow::Initialize()
{
	VsSimulationWindow::Initialize();

	int iCount = m_aryCameraPaths.GetSize();
	for(int iIdx=0; iIdx<iCount; iIdx++)
		m_aryCameraPaths[iIdx]->Initialize();
}


void VsScriptedSimulationWindow::Load(CStdXml &oXml)
{
	VsSimulationWindow::Load(oXml);

	oXml.IntoElem(); //Into Window Element

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
}

/**
\brief	Loads the a camera path. 

\author	dcofer
\date	2/25/2011

\param [in,out]	oXml The xml data packet to load. 

\return	The root. 
**/
OsgCubicSpline *VsScriptedSimulationWindow::LoadCameraPath(CStdXml &oXml)
{
	OsgCubicSpline *lpSpline = NULL;

try
{
	lpSpline = new OsgCubicSpline;

	lpSpline->SetSystemPointers(m_lpSim, NULL, NULL, NULL, TRUE);

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
}// end VortexAnimatSim