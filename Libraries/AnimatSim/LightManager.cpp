/**
\file	LightManager.cpp

\brief	Implements a LightManager object. 
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "Light.h"
#include "LightManager.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace Environment
	{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/2/2011
**/
LightManager::LightManager(void)
{
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/2/2011
**/
LightManager::~LightManager(void)
{
}

#pragma region AccessorMutators

#pragma endregion

#pragma region DataAccesMethods


#pragma endregion

void LightManager::SetupLights()
{}


/**
\brief	Creates and adds a light. 

\author	dcofer
\date	3/2/2011

\param	strXml	The xml data packet for loading the light. 
**/
void LightManager::AddLight(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Light");

	Light *lpLight = LoadLight(oXml);

	lpLight->Create();
}

/**
\brief	Removes the light with the specified ID. 

\author	dcofer
\date	3/2/2011

\param	strID	ID of the light to remove
\param	bThrowError	If true and ID is not found then it will throw an error.
\exception If bThrowError is true and ID is not found.
**/
void LightManager::RemoveLight(string strID, BOOL bThrowError)
{
	int iPos = FindChildListPos(strID, bThrowError);
	m_aryLights.RemoveAt(iPos);
}

BOOL LightManager::AddItem(const string &strItemType, const string &strXml, BOOL bThrowError, BOOL bDoNotInit)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "LIGHT")
	{
		AddLight(strXml);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL LightManager::RemoveItem(const string &strItemType, const string &strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "LIGHT")
	{
		RemoveLight(strID);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

/**
\brief	Finds the array index for the light with the specified ID

\author	dcofer
\date	3/2/2011

\param	strID ID of light to find
\param	bThrowError	If true and ID is not found then it will throw an error, else return NULL
\exception If bThrowError is true and ID is not found.

\return	If bThrowError is false and ID is not found returns NULL, 
else returns the pointer to the found part.
**/
int LightManager::FindChildListPos(string strID, BOOL bThrowError)
{
	string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryLights.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryLights[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lBodyOrJointIDNotFound, Al_Err_strBodyOrJointIDNotFound, "ID");

	return -1;
}

void LightManager::Initialize()
{
	int iCount = m_aryLights.GetSize();
	for(int iLight=0; iLight<iCount; iLight++)
		m_aryLights[iLight]->Create();
}

void LightManager::Load(CStdXml &oXml)
{
	if(oXml.FindChildElement("Lights", false))
	{
		AnimatBase::Load(oXml);
	
		oXml.IntoElem(); //Into Lights Element

		Light *lpLight = NULL;
		int iCount = oXml.NumberOfChildren();
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			lpLight = LoadLight(oXml);
		}

		oXml.OutOfElem();  //Outof Lights Element
	}
}


/**
\brief	Loads a Light.

\author	dcofer
\date	3/25/2011

\param [in,out]	oXml	The xml for the light to load. 

\return	Pointer to the new simulation window.
**/
Light *LightManager::LoadLight(CStdXml &oXml)
{
	Light *lpLight=NULL;
	string strModuleName, strType;

try
{
	oXml.IntoElem();  //Into Column Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Column Element

	lpLight = dynamic_cast<Light *>(m_lpSim->CreateObject(strModuleName, "Light", strType));
	if(!lpLight)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Light");

	lpLight->SetSystemPointers(m_lpSim, NULL, NULL, NULL, TRUE);
	lpLight->Load(oXml);

	m_aryLights.Add(lpLight);
    lpLight->LightNumber(m_aryLights.GetSize()-1);

	return lpLight;
}
catch(CStdErrorInfo oError)
{
	if(lpLight) delete lpLight;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpLight) delete lpLight;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

	}			//Environment
}				//AnimatSim