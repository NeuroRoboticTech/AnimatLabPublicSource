/**
\file	KeyFrame.cpp

\brief	Implements the key frame class.
**/

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
	
/**
\brief	Default constructor.

\author	dcofer
\date	3/24/2011
**/
KeyFrame::KeyFrame()
{
	m_iCollectInterval = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/24/2011
**/
KeyFrame::~KeyFrame()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of KeyFrame\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the collect interval.

\author	dcofer
\date	3/24/2011

\return	Collect interval.
**/
int KeyFrame::CollectInterval() {return m_iCollectInterval;}

/**
\brief	Sets the Collect interval.

\author	dcofer
\date	3/24/2011

\param	iVal	The new value. 
**/
void KeyFrame::CollectInterval(int iVal) {m_iCollectInterval = iVal;}

/**
\brief	 Less than comparison of two activated items.

\return	true if start slice of this key frame is less than the one passed in.
**/
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

/**
\brief	Generates an identifier.

\author	dcofer
\date	3/24/2011
**/
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

/**
\brief	Less than activated item compare.

\author	dcofer
\date	3/24/2011

\param [in,out]	lpItem1	If non-null, the first pointer to an item. 
\param [in,out]	lpItem2	If non-null, the second pointer to an item. 

\return	true if it succeeds, false if it fails.
**/
BOOL LessThanActivatedItemCompare(ActivatedItem *lpItem1, ActivatedItem *lpItem2)
{
	return lpItem1->operator<(lpItem2);
}

	}			//Recording
}				//AnimatSim
