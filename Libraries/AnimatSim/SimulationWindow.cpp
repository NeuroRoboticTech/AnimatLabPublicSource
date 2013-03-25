/**
\file	SimulationWindow.cpp

\brief	Implements the simulation Windows Form.
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"
#include "SimulationWindow.h"

namespace AnimatSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/24/2011
**/
SimulationWindow::SimulationWindow(void)
{
	m_HWND = NULL;
	m_ptPosition.Set(0, 0, 0);
	m_ptSize.Set(500, 500, 0);
	m_bTrackCamera = FALSE;
	m_bStandAlone = TRUE;
}

/**
\brief	Constructor.

\author	dcofer
\date	3/24/2011

\param	win	HWND Handle of the window. 
**/
SimulationWindow::SimulationWindow(HWND win)
{
	m_HWND = win;
	m_ptPosition.Set(0, 0, 0);
	m_ptSize.Set(500, 500, 0);
	m_bTrackCamera = FALSE;
	m_bStandAlone = TRUE;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/24/2011
**/
SimulationWindow::~SimulationWindow(void)
{
}

/**
\brief	Sets the GUID ID of the structure to look at with the camera

\author	dcofer
\date	3/24/2011

\param	strID	GUID ID of the structure. 
**/
void SimulationWindow::LookAtStructureID(string strID) 
{
    if(Std_IsBlank(strID))
        m_bTrackCamera = FALSE;
    else
        m_bTrackCamera = TRUE;

	m_strLookAtStructureID = strID;
	SetupTrackCamera(FALSE);
}

/**
\brief	Gets the look at structure GUID ID.

\author	dcofer
\date	3/24/2011

\return	GUID ID.
**/
string SimulationWindow::LookAtStructureID() {return m_strLookAtStructureID;}

/**
\brief	Sets the RigidBody GUID ID to look at within the specified structure.

\author	dcofer
\date	3/24/2011

\param	strID	GUID ID. 
**/
void SimulationWindow::LookAtBodyID(string strID) 
{
    m_strLookAtBodyID = strID;
	SetupTrackCamera(FALSE);
}

/**
\brief	Gets the GUID ID of the RigidBody to look at.

\author	dcofer
\date	3/24/2011

\return	GUID ID.
**/
string SimulationWindow::LookAtBodyID() {return m_strLookAtBodyID;}

/**
\brief	Sets whether camera tracking should be used.

\author	dcofer
\date	3/24/2011

\param	bVal	true to use camera tracking. 
**/
void SimulationWindow::UsingTrackCamera(BOOL bVal) 
{
	m_bTrackCamera = bVal;
	SetupTrackCamera(TRUE);
}

/**
\brief	Gets wheter camera tracking is being used.

\author	dcofer
\date	3/24/2011

\return	true if camera tracking is being used.
**/
BOOL SimulationWindow::UsingTrackCamera() {return m_bTrackCamera;}

/**
\brief	Gets the HWND window identifier.

\author	dcofer
\date	3/24/2011

\return	The handle of the window.
**/
HWND SimulationWindow::WindowID() {return m_HWND;}

/**
\brief	Sets the HWND Window identifier.

\author	dcofer
\date	3/24/2011

\param	win	Handle of the window. 
**/
void SimulationWindow::WindowID(HWND win) {m_HWND = win;}

/**
\brief	Gets whether this window stands alone from all others.

\author	dcofer
\date	3/24/2011

\return	true if it stands alone, false else.
**/
BOOL SimulationWindow::StandAlone() {return m_bStandAlone;}

/**
\brief	Sets whether this window stands alone.

\author	dcofer
\date	3/24/2011

\param	bVal	true make stand alone. 
**/
void SimulationWindow::StandAlone(BOOL bVal) {m_bStandAlone = bVal;}

#pragma region DataAccesMethods

float *SimulationWindow::GetDataPointer(const string &strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	//if(strType == "TIME")
	//	lpData = &m_fltTime;
	//else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Simulator DataType: " + strDataType);

	return lpData;
}

BOOL SimulationWindow::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);
	
	if(AnimatBase::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "LOOKATSTRUCTUREID")
	{
		LookAtStructureID(strValue);
		return TRUE;
	}

	if(strType == "LOOKATBODYID")
	{
		LookAtBodyID(strValue);
		return TRUE;
	}

	if(strType == "TRACKCAMERA")
	{
		UsingTrackCamera(Std_ToBool(strValue));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void SimulationWindow::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("LookAtStructureID");
	aryTypes.Add("String");

	aryNames.Add("LookAtBodyID");
	aryTypes.Add("String");

	aryNames.Add("TrackCamera");
	aryTypes.Add("Boolean");
}

#pragma endregion

/**
\brief	Sets up the track camera.

\author	dcofer
\date	3/24/2011

\param	bTrackCamera			true to track camera. 
\param	strLookAtStructureID	Identifier for the string look at structure. 
\param	strLookAtBodyID			Identifier for the string look at body. 
**/
void SimulationWindow::SetupTrackCamera(BOOL bTrackCamera, string strLookAtStructureID, string strLookAtBodyID)
{
	m_bTrackCamera = bTrackCamera;
	m_strLookAtStructureID = strLookAtStructureID;
	m_strLookAtBodyID = strLookAtBodyID;
	SetupTrackCamera(TRUE);
}

/**
\brief	Manually sets a camera look at position.

\details This can be used to manually set where the camera is looking at.

\author	dcofer
\date	4/19/2011

\param	oTarget	Target position to look at.
\param	bResetEyePos	If true it will set the eye position new, if not it will use the current position.
**/
void SimulationWindow::SetCameraLookAt(CStdFPoint oTarget, BOOL bResetEyePos) {}

/**
\brief	Manually sets a camera position and look at position.

\details This can be used to manually set where the camera is looking at.

\author	dcofer
\date	4/19/2011

\param	oTarget	Target position to look at.
**/
void SimulationWindow::SetCameraPositionAndLookAt(CStdFPoint oCameraPos, CStdFPoint oTarget) {}

/**
\brief	Manually sets a camera position.

\details This can be used to manually set where the camera is positioned.
This will only set the position if there is a track body defined. It uses the track body to 
set the look at position.

\author	dcofer
\date	4/19/2011

\param	vCameraPos	new Camera position.
**/
void SimulationWindow::SetCameraPostion(CStdFPoint vCameraPos) {};

void SimulationWindow::UpdateBackgroundColor() {}

void SimulationWindow::OnGetFocus()
{}

void SimulationWindow::OnLoseFocus()
{}

void SimulationWindow::Load(string strXml)
{
	//If it is blank then skip the loading.
	if(Std_IsBlank(strXml))
		return;

	CStdXml oXml;
	oXml.Deserialize(strXml);

	oXml.FindElement("WindowMgr");
	oXml.FindChildElement("Window");

	Load(oXml);
}

void SimulationWindow::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem(); //Into Window Element

	if(oXml.FindChildElement("Position", FALSE))
		Std_LoadPoint(oXml, "Position", m_ptPosition);
	if(oXml.FindChildElement("Size", FALSE))
		Std_LoadPoint(oXml, "Size", m_ptSize);

	m_bStandAlone = oXml.GetChildBool("StandAlone", m_bStandAlone);

	m_strLookAtStructureID = oXml.GetChildString("LookAtStructureID", m_strLookAtStructureID);	
	m_strLookAtBodyID = oXml.GetChildString("LookAtBodyID", m_strLookAtBodyID);	
	m_bTrackCamera = oXml.GetChildBool("TrackCamera", m_bTrackCamera);

	oXml.OutOfElem(); //OutOf Window Element

	if(m_ptPosition.x < 0 || m_ptPosition.y < 0)
		THROW_TEXT_ERROR(Al_Err_lSimWinPosInvalid, Al_Err_strSimWinPosInvalid, "POS: (" + STR(m_ptPosition.x) + ", " + STR(m_ptPosition.y) + ")");
	if(m_ptSize.x < 0 || m_ptSize.y < 0)
		THROW_TEXT_ERROR(Al_Err_lSimWinSizeInvalid, Al_Err_strSimWinSizeInvalid, "Size: (" + STR(m_ptSize.x) + ", " + STR(m_ptSize.y) + ")");
}

}//end namespace AnimatSim