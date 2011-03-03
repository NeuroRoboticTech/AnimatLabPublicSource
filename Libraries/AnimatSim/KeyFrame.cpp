// AlKeyFramecpp: implementation of the KeyFrame class.
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
#include "Structure.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "KeyFrame.h"
#include "OdorType.h"
#include "Odor.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace Recording
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

KeyFrame::KeyFrame()
{
	m_iCollectInterval = 0;
}

KeyFrame::~KeyFrame()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of KeyFrame\r\n", "", -1, FALSE, TRUE);}
}

BOOL KeyFrame::operator<(ActivatedItem *lpItem)
{
	KeyFrame *lpFrame = dynamic_cast<KeyFrame *>(lpItem);

	if(!lpFrame)
		THROW_ERROR(Al_Err_lItemNotKeyFrameType, Al_Err_strItemNotKeyFrameType);

	if(m_lStartSlice < lpFrame->m_lStartSlice)
		return TRUE;

	if( (m_lStartSlice == lpFrame->m_lStartSlice) && (m_lEndSlice < lpFrame->m_lEndSlice))
		return TRUE;

	return FALSE;
}


/*
void KeyFrame::Initialize()
{
}


void KeyFrame::Activate()
{}

void KeyFrame::StepSimulation()
{}

void KeyFrame::Deactivate()
{
}
*/

void KeyFrame::GenerateID()
{
	//Lets verify the slice data is setup correctly.
	Std_IsAboveMin((long) -1, m_lStartSlice, TRUE, "StartSlice");
	Std_IsAboveMin(m_lStartSlice, m_lEndSlice, TRUE, "EndSlice");

	m_strID = Type();

	char strTail[20];
	sprintf(strTail, "%010d", m_lStartSlice);

	m_strID += strTail;
	m_strID = Std_CheckString(m_strID);
}

void KeyFrame::Load(CStdXml &oXml)
{
	oXml.IntoElem();  //Into Item Element

	m_lStartSlice = oXml.GetChildLong("StartSlice");
	m_lEndSlice = oXml.GetChildLong("EndSlice");

	Std_IsAboveMin((long) -1, m_lStartSlice, TRUE, "StartSlice");
	Std_IsAboveMin(m_lStartSlice, m_lEndSlice, TRUE, "EndSlice");

	m_iCollectInterval = oXml.GetChildInt("CollectInterval");
	Std_IsAboveMin((int) 0, m_iCollectInterval, TRUE, "CollectInterval");

	GenerateID();

	oXml.OutOfElem(); //OutOf KeyFrame Element
}


BOOL LessThanActivatedItemCompare(ActivatedItem *lpItem1, ActivatedItem *lpItem2)
{
	return lpItem1->operator<(lpItem2);
}

	}			//Recording
}				//AnimatSim
