// Materials.cpp: implementation of the Materials class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

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
#include "Sensor.h"
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

Materials::Materials()
{
}

Materials::~Materials()
{

try
{
	m_aryMaterialTypes.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Materials\r\n", "", -1, FALSE, TRUE);}
}

void Materials::Reset()
{
	m_aryMaterialTypes.RemoveAll();
}

/**
\brief	Creates and adds a new material type. 

\author	dcofer
\date	3/2/2011

\param	strXml	The xml data packet for loading the type. 
**/
void Materials::AddMaterialType(string strXml, BOOL bDoNotInit)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("MaterialType");

	MaterialType *lpType = LoadMaterialType(oXml);

	if(!bDoNotInit)
		lpType->Initialize();

	m_aryMaterialTypes.Add(lpType);
}

/**
\brief	Removes the material type with the specified ID. 

\author	dcofer
\date	3/2/2011

\param	strID	ID of the material type to remove
\param	bThrowError	If true and ID is not found then it will throw an error.
\exception If bThrowError is true and ID is not found.
**/
void Materials::RemoveMaterialType(string strID, BOOL bThrowError)
{
	int iPos = FindTypeListPos(strID, bThrowError);
	m_aryMaterialTypes.RemoveAt(iPos);
}


/**
\brief	Finds the array index for the material type with the specified ID

\author	dcofer
\date	3/2/2011

\param	strID ID of material type to find
\param	bThrowError	If true and ID is not found then it will throw an error, else return NULL
\exception If bThrowError is true and ID is not found.

\return	If bThrowError is false and ID is not found returns NULL, 
else returns the pointer to the found part.
**/
int Materials::FindTypeListPos(string strID, BOOL bThrowError)
{
	string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryMaterialTypes.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryMaterialTypes[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lMaterialTypeIDNotFound, Al_Err_strMaterialTypeIDNotFound, "ID");

	return -1;
}


void Materials::Initialize()
{
	AnimatBase::Initialize();

	if(m_aryMaterialTypes.GetSize() == 0)
		THROW_ERROR(Al_Err_lDefaultMaterialNotFound, Al_Err_strDefaultMaterialNotFound);

	MaterialType *lpItem = NULL;
	int iCount = m_aryMaterialTypes.GetSize();
	for(int iIndex = 0; iIndex < iCount; iIndex++)
	{
		lpItem = m_aryMaterialTypes[iIndex];
		lpItem->Initialize();
	}
}

BOOL Materials::AddItem(const string &strItemType, const string &strXml, BOOL bThrowError, BOOL bDoNotInit)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "MATERIALTYPE")
	{
		AddMaterialType(strXml, bDoNotInit);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL Materials::RemoveItem(const string &strItemType, const string &strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "MATERIALTYPE")
	{
		RemoveMaterialType(strID);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

void Materials::CreateDefaultMaterial()
{
	MaterialType *lpType=NULL;

	m_aryMaterialTypes.RemoveAll();

	lpType = dynamic_cast<MaterialType *>(m_lpSim->CreateObject("", "Material", "Default"));
	if(!lpType)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Material");

	lpType->ID("DEFAULTMATERIAL");
	lpType->Name("Default");
	lpType->SetSystemPointers(m_lpSim, NULL, NULL, NULL, TRUE);
	m_aryMaterialTypes.Add(lpType);
}

void Materials::LoadMaterialTypes(CStdXml &oXml)
{
	oXml.FindChildElement("MaterialTypes");
	oXml.IntoElem(); //Into MaterialsTypes Element

	string strMaterial;
	MaterialType *lpItem = NULL;
	BOOL bDefaultFound = FALSE;
	int iCount = oXml.NumberOfChildren();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		oXml.FindChildByIndex(iIndex);
		lpItem = LoadMaterialType(oXml);
		m_aryMaterialTypes.Add(lpItem);

		if(lpItem->ID() == "DEFAULTMATERIAL")
			bDefaultFound = TRUE;
	}

	oXml.OutOfElem(); //Outof MaterialsTypes Element

	if(!bDefaultFound)
		THROW_ERROR(Al_Err_lDefaultMaterialNotFound, Al_Err_strDefaultMaterialNotFound);
}

void Materials::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	m_aryMaterialTypes.RemoveAll();

	if(oXml.FindChildElement("Materials", false))
	{
		oXml.IntoElem(); //Into Materials Element

		LoadMaterialTypes(oXml);

		oXml.OutOfElem();  //Outof Materials Element
	}
	else
		CreateDefaultMaterial(); //Always create a default material.
}

MaterialType *Materials::LoadMaterialType(CStdXml &oXml)
{
	MaterialType *lpItem=NULL;
	string strModuleName, strType;

try
{
	oXml.IntoElem();  //Into Column Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Column Element

	lpItem = dynamic_cast<MaterialType *>(m_lpSim->CreateObject(strModuleName, "Material", strType));
	if(!lpItem)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Material");

	lpItem->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL, TRUE);
	lpItem->Load(oXml);

	return lpItem;
}
catch(CStdErrorInfo oError)
{
	if(lpItem) delete lpItem;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpItem) delete lpItem;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


	}			// Visualization
}				//VortexAnimatSim
