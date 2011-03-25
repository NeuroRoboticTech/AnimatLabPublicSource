// Materials.cpp: implementation of the Materials class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
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

	MaterialPair *lpItem = NULL;
	int iCount = m_aryMaterialPairs.GetSize();
	for(int iIndex = 0; iIndex < iCount; iIndex++)
	{
		lpItem = m_aryMaterialPairs[iIndex];
		lpItem->Initialize();
	}
}

void Materials::CreateDefaultMaterial()
{
	MaterialPair *lpItem=NULL;

	//Add a default pair association for all material pairs.
	string strType;
	int iCount = m_aryMaterialTypes.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		strType = m_aryMaterialTypes[iIndex];

		lpItem = dynamic_cast<MaterialPair *>(m_lpSim->CreateObject("", "Material", "Default"));
		if(!lpItem)
			THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Material");

		lpItem->SetSystemPointers(m_lpSim, NULL, NULL, NULL, TRUE);
		lpItem->Material1("DEFAULT");
		lpItem->Material2(strType);
		lpItem->CreateDefaultUnits();

		m_aryMaterialPairs.Add(lpItem);
	}
}

void Materials::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	m_aryMaterialTypes.RemoveAll();
	m_aryMaterialPairs.RemoveAll();
	m_aryMaterialTypes.Add("DEFAULT");

	if(oXml.FindChildElement("Materials", false))
	{
		oXml.IntoElem(); //Into Materials Element

		if(oXml.FindChildElement("MaterialTypes", false))
		{
			oXml.IntoElem(); //Into MaterialsTypes Element

			string strMaterial;
			int iCount = oXml.NumberOfChildren();
			for(int iIndex=0; iIndex<iCount; iIndex++)
			{
				oXml.FindChildByIndex(iIndex);
				strMaterial = Std_ToUpper(oXml.GetChildString());
				m_aryMaterialTypes.Add(strMaterial);
			}

			oXml.OutOfElem(); //Outof MaterialsTypes Element
		}

		if(oXml.FindChildElement("MaterialPairs", false))
		{
			//*** Begin Loading HudItems. *****
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

		oXml.OutOfElem();  //Outof Materials Element
	}

	CreateDefaultMaterial(); //Always create a default material.
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
