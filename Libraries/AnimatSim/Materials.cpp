// Materials.cpp: implementation of the Materials class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
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
	m_aryMaterialPairs.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Materials\r\n", "", -1, FALSE, TRUE);}
}

void Materials::Reset()
{
	m_aryMaterialTypes.RemoveAll();
	m_aryMaterialPairs.RemoveAll();
}

void Materials::Initialize()
{
	AnimatBase::Initialize();

	if(m_aryMaterialTypes.GetSize() == 0 && m_aryMaterialPairs.GetSize() == 0)
		THROW_ERROR(Al_Err_lDefaultMaterialNotFound, Al_Err_strDefaultMaterialNotFound);

	//Get the first matieralpair so we can use it to register the material types.
	MaterialPair *lpPair = m_aryMaterialPairs[0];

	MaterialType *lpItem = NULL;
	int iCount = m_aryMaterialTypes.GetSize();
	for(int iIndex = 0; iIndex < iCount; iIndex++)
	{
		lpItem = m_aryMaterialTypes[iIndex];
		lpItem->Initialize();
		lpPair->RegisterMaterialType(lpItem->ID());
	}

	iCount = m_aryMaterialPairs.GetSize();
	for(int iIndex = 0; iIndex < iCount; iIndex++)
	{
		lpPair = m_aryMaterialPairs[iIndex];
		lpPair->Initialize();
	}
}

void Materials::CreateDefaultMaterial()
{
	MaterialType *lpType=NULL;
	MaterialPair *lpPair=NULL;

	m_aryMaterialTypes.RemoveAll();
	lpType = new MaterialType();
	lpType->ID("DEFAULTMATERIAL");
	lpType->Name("Default");
	lpType->SetSystemPointers(m_lpSim, NULL, NULL, NULL, TRUE);
	m_aryMaterialTypes.Add(lpType);

	lpPair = dynamic_cast<MaterialPair *>(m_lpSim->CreateObject("", "Material", "DEFAULT"));
	if(!lpPair)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Material");

	lpPair->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL, TRUE);
	lpPair->Material1ID(lpType->ID());
	lpPair->Material2ID(lpType->ID());
	m_aryMaterialPairs.Add(lpPair);
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
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Material");
}

void Materials::LoadMaterialPairs(CStdXml &oXml)
{
	oXml.FindChildElement("MaterialPairs");
	oXml.IntoElem(); //Into MaterialPairs Element

	int iCount = oXml.NumberOfChildren();
	MaterialPair *lpItem = NULL;
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		oXml.FindChildByIndex(iIndex);
		lpItem = LoadMaterialPair(oXml);
		m_aryMaterialPairs.Add(lpItem);
	}

	oXml.OutOfElem();  //Outof MaterialPairs Element
}

void Materials::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	m_aryMaterialTypes.RemoveAll();
	m_aryMaterialPairs.RemoveAll();

	if(oXml.FindChildElement("Materials", false))
	{
		oXml.IntoElem(); //Into Materials Element

		LoadMaterialTypes(oXml);
		LoadMaterialPairs(oXml);

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
	lpItem = new MaterialType();
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

MaterialPair *Materials::LoadMaterialPair(CStdXml &oXml)
{
	MaterialPair *lpItem=NULL;
	string strModuleName, strType;

try
{
	oXml.IntoElem();  //Into Column Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Column Element

	lpItem = dynamic_cast<MaterialPair *>(m_lpSim->CreateObject(strModuleName, "Material", strType));
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
