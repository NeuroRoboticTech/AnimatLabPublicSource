// Materials.cpp: implementation of the Materials class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
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
	m_lpPair = NULL;
}

Materials::~Materials()
{

try
{
	if(m_lpPair)
		{delete m_lpPair; m_lpPair = NULL;}

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

int Materials::GetMaterialID(string strName)
{
	if(!m_lpPair)
		THROW_ERROR(Al_Err_lMaterial_Pair_Not_Defined, Al_Err_strMaterial_Pair_Not_Defined);
	return m_lpPair->GetMaterialID(strName);
}

void Materials::Initialize(Simulator *lpSim)
{
	RegisterMaterials(lpSim);

	MaterialPair *lpItem = NULL;
	int iCount = m_aryMaterialPairs.GetSize();
	for(int iIndex = 0; iIndex < iCount; iIndex++)
	{
		lpItem = m_aryMaterialPairs[iIndex];
		lpItem->Initialize(lpSim);
	}
}

void Materials::RegisterMaterials(Simulator *lpSim)
{
	if(m_lpPair)
		{delete m_lpPair; m_lpPair = NULL;}	

	m_lpPair = dynamic_cast<MaterialPair *>(lpSim->CreateObject("", "Material", "Default"));
	if(!m_lpPair)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Material");

	m_lpPair->RegisterMaterialTypes(lpSim, m_aryMaterialTypes);
}

void Materials::CreateDefaultMaterial(Simulator *lpSim)
{
	MaterialPair *lpItem=NULL;

	//Add a default pair association for all material pairs.
	string strType;
	int iCount = m_aryMaterialTypes.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		strType = m_aryMaterialTypes[iIndex];

		lpItem = dynamic_cast<MaterialPair *>(lpSim->CreateObject("", "Material", "Default"));
		if(!lpItem)
			THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Material");

		lpItem->Material1("DEFAULT");
		lpItem->Material2(strType);
		lpItem->ScaleUnits(lpSim);

		m_aryMaterialPairs.Add(lpItem);
	}
}

void Materials::Load(Simulator *lpSim, CStdXml &oXml)
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
				lpItem = LoadMaterialPair(lpSim, oXml);
				m_aryMaterialPairs.Add(lpItem);
			}

			oXml.OutOfElem();  //Outof MaterialPairs Element
		}

		oXml.OutOfElem();  //Outof Materials Element
	}

	CreateDefaultMaterial(lpSim); //Always create a default material.
}

MaterialPair *Materials::LoadMaterialPair(Simulator *lpSim, CStdXml &oXml)
{
	MaterialPair *lpItem=NULL;
	string strModuleName, strType;

try
{
	oXml.IntoElem();  //Into Column Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Column Element

	lpItem = dynamic_cast<MaterialPair *>(lpSim->CreateObject(strModuleName, "Material", strType));
	if(!lpItem)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Material");

	lpItem->Load(lpSim, oXml);

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
