/**
\file	LightManager.cpp

\brief	Implements a LightManager object. 
**/

#include "stdafx.h"
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

BOOL LightManager::AddItem(string strItemType, string strXml, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "RIGIDBODY")
	{
		//AddRigidBody(strXml);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL LightManager::RemoveItem(string strItemType, string strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "RIGIDBODY")
	{
		//RemoveRigidBody(strID);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

void LightManager::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);
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