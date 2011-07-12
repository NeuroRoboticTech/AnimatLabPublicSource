/**
// file:	Hud.cpp
//
// summary:	Implements the heads-up display class
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"
#include "HudItem.h"
#include "Hud.h"

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

/**
\brief	Default constructor.

\author	dcofer
\date	7/7/2011
**/
Hud::Hud()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	7/7/2011
**/
Hud::~Hud()
{

try
{
	m_aryHudItems.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Hud\r\n", "", -1, FALSE, TRUE);}
}

void Hud::Reset()
{
	m_aryHudItems.RemoveAll();
}

void Hud::Update()
{
	HudItem *lpItem = NULL;
	int iCount = m_aryHudItems.GetSize();
	for(int iIndex = 0; iIndex < iCount; iIndex++)
	{
		lpItem = m_aryHudItems[iIndex];
		lpItem->Update();
	}
}

void Hud::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	m_aryHudItems.RemoveAll();

	oXml.IntoElem();  //Into Hud Element

	if(oXml.FindChildElement("HudItems", false))
	{
		//*** Begin Loading HudItems. *****
		oXml.IntoChildElement("HudItems");

		int iCount = oXml.NumberOfChildren();
		HudItem *lpItem = NULL;
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			lpItem = LoadHudItem(oXml);
			m_aryHudItems.Add(lpItem);
		}

		oXml.OutOfElem();
		//*** End Loading HudItems. *****
	}

	oXml.OutOfElem();  //Outof Hud Element
}

HudItem *Hud::LoadHudItem(CStdXml &oXml)
{
	HudItem *lpItem=NULL;
	string strModuleName, strType;

try
{
	oXml.IntoElem();  //Into Column Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem();  //OutOf Column Element

	lpItem = dynamic_cast<HudItem *>(m_lpSim->CreateObject(strModuleName, "HudItem", strType));
	if(!lpItem)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "HudItem");

	lpItem->SetSystemPointers(m_lpSim, NULL, NULL, NULL, TRUE);
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


}				//AnimatSim
