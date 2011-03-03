#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"
#include "SimulationWindow.h"

namespace AnimatSim
{

SimulationWindow::SimulationWindow(void)
{
	m_HWND = NULL;
	m_ptPosition.Set(0, 0, 0);
	m_ptSize.Set(500, 500, 0);
	m_bTrackCamera = FALSE;
	m_bStandAlone = TRUE;
}

SimulationWindow::SimulationWindow(HWND win)
{
	m_HWND = win;
	m_ptPosition.Set(0, 0, 0);
	m_ptSize.Set(500, 500, 0);
	m_bTrackCamera = FALSE;
	m_bStandAlone = TRUE;
}

SimulationWindow::~SimulationWindow(void)
{
}

#pragma region DataAccesMethods

float *SimulationWindow::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	//if(strType == "TIME")
	//	lpData = &m_fltTime;
	//else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Simulator DataType: " + strDataType);

	return lpData;
}

BOOL SimulationWindow::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

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

	if(strType == "USINGTRACKCAMERA")
	{
		UsingTrackCamera(Std_ToBool(strValue));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

void SimulationWindow::SetupTrackCamera(BOOL bTrackCamera, string strLookAtStructureID, string strLookAtBodyID)
{
	m_bTrackCamera = bTrackCamera;
	m_strLookAtStructureID = strLookAtStructureID;
	m_strLookAtBodyID = strLookAtBodyID;
	SetupTrackCamera();
}

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