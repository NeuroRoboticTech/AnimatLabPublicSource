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
	m_lpDisplayData = NULL;
	m_lpUpdateData = NULL;
	m_fltUpdateInterval = 1;
}

HudText::HudText(float *aryColor, CStdFPoint &ptPosition, string strFont, int iCharSize, string strText, string strDisplayTargetID, string strDisplayDataType, string strUpdateTargetID, string strUpdateDataType, float fltUpdateInterval)
{
	m_aryColor.Set(aryColor[0], aryColor[1], aryColor[2], aryColor[3]);
	m_ptPosition = ptPosition;
	m_strFont = strFont;
	m_iCharSize = iCharSize;
	m_strText = strText;
	m_strDisplayTargetID = strDisplayTargetID;
	m_strDisplayDataType = strDisplayDataType;
	m_strUpdateTargetID = strUpdateTargetID;
	m_strUpdateDataType = strUpdateDataType;
	m_fltUpdateInterval = fltUpdateInterval;
}

HudText::~HudText()
{
}

void HudText::Initialize(void *lpVoidProjection)
{
	AnimatBase::Initialize();

	AnimatBase *lpBase = m_lpSim->FindByID(m_strDisplayTargetID);
	m_lpDisplayData = lpBase->GetDataPointer(m_strDisplayDataType);

	lpBase = m_lpSim->FindByID(m_strUpdateTargetID);
	m_lpUpdateData = lpBase->GetDataPointer(m_strUpdateDataType);
}

void HudText::Load(CStdXml &oXml)
{
	HudItem::Load(oXml);

	oXml.IntoElem();

	m_aryColor.Load(oXml, "Color", false);
	Std_LoadPoint(oXml, "Position", m_ptPosition, false);
	m_strFont = oXml.GetChildString("Font", m_strFont);
	m_iCharSize = oXml.GetChildInt("CharSize", m_iCharSize);
	m_strText = oXml.GetChildString("Text", m_strText);
	m_strDisplayTargetID = oXml.GetChildString("DisplayTargetID");
	m_strDisplayDataType = oXml.GetChildString("DisplayDataType");
	m_strUpdateTargetID = oXml.GetChildString("UpdateTargetID");
	m_strUpdateDataType = oXml.GetChildString("UpdateDataType");
	m_fltUpdateInterval = oXml.GetChildFloat("UpdateInterval");

	oXml.OutOfElem();
}

}				//AnimatSim
