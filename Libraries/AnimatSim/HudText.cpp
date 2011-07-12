/**
\file	HudText.cpp

\brief	Implements the heads-up display text class.
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"
#include "HudItem.h"
#include "HudText.h"

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

HudText::HudText()
{
	//Default color is white
	m_aryColor.Set(1, 1, 1, 1);
	m_ptPosition.Set(10,10, 0); //Default to the lower left corner
	m_strFont = "fonts/Arial.ttf";
	m_iCharSize = 30;
	m_strText = "";
	m_lpData = NULL;
}

HudText::HudText(float *aryColor, CStdFPoint &ptPosition, string strFont, int iCharSize, string strText, string strTargetID, string strDataType)
{
	m_aryColor.Set(aryColor[0], aryColor[1], aryColor[2], aryColor[3]);
	m_ptPosition = ptPosition;
	m_strFont = strFont;
	m_iCharSize = iCharSize;
	m_strText = strText;
	m_strTargetID = strTargetID;
	m_strDataType = strDataType;
}

HudText::~HudText()
{
}

void HudText::Initialize(void *lpVoidProjection)
{
	AnimatBase::Initialize();

	AnimatBase *lpBase = m_lpSim->FindByID(m_strTargetID);
	m_lpData = lpBase->GetDataPointer(m_strDataType);
}

void HudText::Load(CStdXml &oXml)
{
	HudItem::Load(oXml);

	oXml.IntoElem();

	m_aryColor.Load(oXml, "Color", FALSE);
	Std_LoadPoint(oXml, "Position", m_ptPosition, FALSE);
	m_strFont = oXml.GetChildString("Font", m_strFont);
	m_iCharSize = oXml.GetChildInt("CharSize", m_iCharSize);
	m_strText = oXml.GetChildString("Text", m_strText);
	m_strTargetID = oXml.GetChildString("TargetID");
	m_strDataType = oXml.GetChildString("DataType");

	oXml.OutOfElem();
}

}				//AnimatSim
