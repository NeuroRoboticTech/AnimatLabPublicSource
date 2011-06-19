/**
\file	SimulationWindowMgr.cpp

\brief	Implements the simulation window manager class.
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Gain.h"
#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "Adapter.h"
#include "NervousSystem.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataColumn.h"
#include "DataChart.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "SimulationWindow.h"
#include "SimulationWindowMgr.h"
#include "Simulator.h"


namespace AnimatSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/25/2011
**/
SimulationWindowMgr::SimulationWindowMgr(void)
{
}

/**
\brief	Destructor.

\author	dcofer
\date	3/25/2011
**/
SimulationWindowMgr::~SimulationWindowMgr(void)
{

try
{
	CloseAllWindows();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of SimulationWindowMgr\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Adds a simulation window.

\author	dcofer
\date	3/25/2011

\param	strModule	The dll module name. 
\param	strType  	Type ID of window to create. 
\param	bInit	 	true to initialise the window here. 
\param	win		 	HWND Handle of the window. 
\param	strXml   	The xml packet to use when creating the window. 

\return	Pointer to the created SimulationWindow.
**/
SimulationWindow *SimulationWindowMgr::AddSimulationWindow(string strModule, string strType, BOOL bInit, HWND win, string strXml)
{
	SimulationWindow *lpWin=NULL;

try
{
	//Create a simulation window from the graphics library being used
	lpWin = dynamic_cast<SimulationWindow *>(m_lpSim->CreateObject(strModule, "SimulationWindow", strType));

	lpWin->SetSystemPointers(m_lpSim, NULL, NULL, NULL, TRUE);
	lpWin->Load(strXml);

	//initialize the window
	lpWin->WindowID(win);
	if(bInit)
		lpWin->Initialize();

	//add the window to the list of windows
	m_aryWindows.Add(lpWin);

	return lpWin;
}
catch(CStdErrorInfo oError)
{
	if(lpWin) delete lpWin;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpWin) delete lpWin;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

/**
\brief	Searches for simulation window with the specified HWND.

\author	dcofer
\date	3/25/2011

\param	win			  	Handle of the window to search for. 
\param [in,out]	iIndex	Zero-based index of the found window in the array. 
\param	bThrowError   	If true and no matching window is found then it throws an error, else it returns NULL. 

\return	null if it fails and bThrowError is false, else the found simulation window or NULL.
**/
SimulationWindow *SimulationWindowMgr::FindSimulationWindow(HWND win, int &iIndex, BOOL bThrowError)
{
	int iCount = m_aryWindows.GetSize();
	for(int iIdx=0; iIdx<iCount; iIdx++)
	{
		if(m_aryWindows[iIdx]->WindowID() == win)
		{
			iIndex = iIdx;
			return m_aryWindows[iIdx];
		}
	}

	iIndex = -1;
	if(bThrowError)
		THROW_ERROR(Al_Err_lSimWindowNotFound, Al_Err_strSimWindowNotFound);
	return NULL;

}

/**
\brief	Removes the simulation window described by the HWND win.

\author	dcofer
\date	3/25/2011

\param	win	Handle of the window. 
**/
void SimulationWindowMgr::RemoveSimulationWindow(HWND win)
{
	int iIndex = 0;
	SimulationWindow *lpWin = FindSimulationWindow(win, iIndex, FALSE);

	if(lpWin)
	{
		lpWin->Close();
		m_aryWindows.RemoveAt(iIndex);
	}
}

/**
\brief	Updates all of the windows.

\author	dcofer
\date	3/25/2011

\return	true if it succeeds, false if it fails.
**/
BOOL SimulationWindowMgr::Update()
{
	int iCount = m_aryWindows.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryWindows[iIndex]->Update();

	return TRUE;
}

/**
\brief	Closes all windows.

\author	dcofer
\date	3/25/2011
**/
void SimulationWindowMgr::Close()
{
	CloseAllWindows();
}

/**
\brief	Closes all windows internally.

\author	dcofer
\date	3/25/2011
**/
void SimulationWindowMgr::CloseAllWindows()
{
	int iCount = m_aryWindows.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryWindows[iIndex]->Close();
	m_aryWindows.clear();
}

/**
\brief	Sets up the cameras for each window.

\author	dcofer
\date	3/25/2011
**/
void SimulationWindowMgr::SetupCameras()
{
	int iCount = m_aryWindows.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryWindows[iIndex]->SetupTrackCamera();
}

/**
\brief	Query if this object has contained window.

\author	dcofer
\date	3/25/2011

\return	true if contained window, false if not.
**/
BOOL SimulationWindowMgr::HasContainedWindow()
{
	int iCount = m_aryWindows.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		if(!m_aryWindows[iIndex]->StandAlone())
			return TRUE;
	}
	return FALSE;
}

void SimulationWindowMgr::Load(CStdXml &oXml)
{
	VerifySystemPointers();

	m_aryWindows.RemoveAll();

	if(oXml.FindChildElement("WindowMgr", false))
	{
		AnimatBase::Load(oXml);
	
		oXml.IntoElem(); //Into WindowMgr Element

		Std_LoadPoint(oXml, "Position", m_ptPosition);
		Std_LoadPoint(oXml, "Size", m_ptSize);
		if(m_ptPosition.x < 0 || m_ptPosition.y < 0)
			THROW_TEXT_ERROR(Al_Err_lSimWinPosInvalid, Al_Err_strSimWinPosInvalid, "POS: (" + STR(m_ptPosition.x) + ", " + STR(m_ptPosition.y) + ")");
		if(m_ptSize.x < 0 || m_ptSize.y < 0)
			THROW_TEXT_ERROR(Al_Err_lSimWinSizeInvalid, Al_Err_strSimWinSizeInvalid, "Size: (" + STR(m_ptSize.x) + ", " + STR(m_ptSize.y) + ")");

		oXml.IntoChildElement("Windows"); //There must be a windows section if we get here.
		int iCount = oXml.NumberOfChildren();
		SimulationWindow *lpItem = NULL;
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			lpItem = LoadSimulationWindow(oXml);
			m_aryWindows.Add(lpItem);
		}
		oXml.OutOfElem(); //OutOf Windows Element


		oXml.OutOfElem(); //OutOf WindowMgr Element
	}
}

/**
\brief	Loads a simulation window.

\author	dcofer
\date	3/25/2011

\param [in,out]	oXml	The xml for the window to load. 

\return	Pointer to the new simulation window.
**/
SimulationWindow *SimulationWindowMgr::LoadSimulationWindow(CStdXml &oXml)
{
	SimulationWindow *lpWin=NULL;
	string strModuleName, strType;

try
{
	oXml.IntoElem();  //Into Column Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Column Element

	lpWin = dynamic_cast<SimulationWindow *>(m_lpSim->CreateObject(strModuleName, "SimulationWindow", strType));
	if(!lpWin)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "SimulationWindow");

	lpWin->SetSystemPointers(m_lpSim, NULL, NULL, NULL, TRUE);
	lpWin->Load(oXml);

	return lpWin;
}
catch(CStdErrorInfo oError)
{
	if(lpWin) delete lpWin;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpWin) delete lpWin;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

}// end AnimatSim