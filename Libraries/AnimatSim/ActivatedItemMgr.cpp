/**
\file	ActivatedItemMgr.cpp

\brief	Implements the activated item manager class. 
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Adapter.h"
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

/**
\fn	ActivatedItemMgr::ActivatedItemMgr()

\brief	Default constructor. 

\author	dcofer
\date	3/1/2011
**/
ActivatedItemMgr::ActivatedItemMgr()
{
}

/**
\fn	ActivatedItemMgr::~ActivatedItemMgr()

\brief	Destructor. 

\author	dcofer
\date	3/1/2011
**/
ActivatedItemMgr::~ActivatedItemMgr()
{

try
{
	Reset();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ActivatedItemMgr\r\n", "", -1, FALSE, TRUE);}
}

/**
\fn	void ActivatedItemMgr::Reset()

\brief	Resets this manager.

\details This releases all ActivatedItem's.

\author	dcofer
\date	3/1/2011
**/
void ActivatedItemMgr::Reset()
{
try
{
	m_aryItems.RemoveAll();
	m_aryItemsMap.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of DataChart\r\n", "", -1, FALSE, TRUE);}
}

/**
\fn	void ActivatedItemMgr::Add(ActivatedItem *lpItem)

\brief	Adds a new ActivatedItem to be managed. 

\author	dcofer
\date	3/1/2011

\param [in,out]	lpItem	The pointer to the item to add. 
**/
void ActivatedItemMgr::Add(ActivatedItem *lpItem)
{
	if(!lpItem)
		THROW_ERROR(Al_Err_lActivatedItemNull, Al_Err_strActivatedItemNull);

	//Lets make sure the ID of the item is in upper case.
	lpItem->ID(Std_CheckString(lpItem->ID()));

	//lets make sure this is a unique item key.
	try
	{
		m_aryItemsMap.Add(lpItem->ID(), lpItem);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " Duplicate activate item Key: " + lpItem->ID(); 
		THROW_ERROR(oError.m_lError, oError.m_strError);
	}
	
	m_aryItems.Add(lpItem);

	stable_sort(m_aryItems.begin(), m_aryItems.end(), LessThanActivatedItemCompare);
}

/**
\fn	void ActivatedItemMgr::Remove(string strID, BOOL bThrowError)

\brief	Removes the item with the specified ID. 

\author	dcofer
\date	3/1/2011

\param	strID		ID of the item to remove. 
\param	bThrowError	true to throw error if not found. 
**/

void ActivatedItemMgr::Remove(string strID, BOOL bThrowError)
{
	int iIndex=0;
	ActivatedItem *lpItem = Find(strID, iIndex, bThrowError);

	if(lpItem)
	{
		if(lpItem->IsActivated())
			lpItem->Deactivate();

		m_aryItems.RemoveAt(iIndex);

		//Must remove it from here last because this stores the actual pointer.
		m_aryItemsMap.Remove(Std_CheckString(strID));
	}

	stable_sort(m_aryItems.begin(), m_aryItems.end(), LessThanActivatedItemCompare);
}

/**
\fn	ActivatedItem *ActivatedItemMgr::Find(string strID, int &iIndex, BOOL bThrowError)

\brief	Searches for an item with the specified ID and sets its index in the array. 

\author	dcofer
\date	3/1/2011

\param	strID			ID of item to find. 
\param [in,out]	iIndex	Zero-based index of the item in m_aryItems. 
\param	bThrowError		true to throw error if not found. 

\return	If bThrowError is false and item is not found it returns NULL, otherwise
if it is found then it returns pointer to the item.
\exception If bThrowError is true and no item with the specified ID is found then
an exception is thrown.
**/
ActivatedItem *ActivatedItemMgr::Find(string strID, int &iIndex, BOOL bThrowError)
{
	int iCount = m_aryItems.GetSize();
	ActivatedItem *lpItem = NULL;
	strID = Std_CheckString(strID);
	for(iIndex=0; iIndex<iCount; iIndex++)
	{
		lpItem = m_aryItems[iIndex];

		if(lpItem->ID() == strID)
			return lpItem;
	}

	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lActivatedItemIDNotFound, Al_Err_strActivatedItemIDNotFound, " ActivatedItemID: " + strID);

	return NULL;
}

/**
\fn	ActivatedItem *ActivatedItemMgr::Find(string strID, BOOL bThrowError)

\brief	Searches for an item with the specified ID. 

\author	dcofer
\date	3/1/2011

\param	strID			ID of item to find. 
\param	bThrowError		true to throw error if not found. 

\return	If bThrowError is false and item is not found it returns NULL, otherwise
if it is found then it returns pointer to the item.
\exception If bThrowError is true and no item with the specified ID is found then
an exception is thrown.
**/
ActivatedItem *ActivatedItemMgr::Find(string strID, BOOL bThrowError)
{
	ActivatedItem *lpItem = NULL;
	CStdMap<string, ActivatedItem *>::iterator oPos;
	oPos = m_aryItemsMap.find(Std_CheckString(strID));

	if(oPos != m_aryItemsMap.end())
		lpItem =  oPos->second;
	else if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lActivatedItemIDNotFound, Al_Err_strActivatedItemIDNotFound, " ActivatedItemID: " + strID);

	return lpItem;
}

/**
\fn	int ActivatedItemMgr::FindListPos(string strID, BOOL bThrowError)

\brief	Searches for the item with the specified ID and returns its position in the m_aryItems array. 

\author	dcofer
\date	3/1/2011

\param	strID			ID of item to find. 
\param	bThrowError		true to throw error if not found. 

\return	Items position in the m_aryItems array. 
**/
int ActivatedItemMgr::FindListPos(string strID, BOOL bThrowError)
{
	string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryItems.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryItems[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lActivatedItemIDNotFound, Al_Err_strActivatedItemIDNotFound, "ID", m_strID);

	return -1;
}


void ActivatedItemMgr::Initialize()
{
	AnimatBase::Initialize();

	int iCount = m_aryItems.GetSize();
	for(int iChart=0; iChart<iCount; iChart++)
		m_aryItems[iChart]->Initialize();

	stable_sort(m_aryItems.begin(), m_aryItems.end(), LessThanActivatedItemCompare);
}

void ActivatedItemMgr::ReInitialize()
{
	int iCount = m_aryItems.GetSize();
	for(int iChart=0; iChart<iCount; iChart++)
		m_aryItems[iChart]->ReInitialize();

	stable_sort(m_aryItems.begin(), m_aryItems.end(), LessThanActivatedItemCompare);
}

void ActivatedItemMgr::ResetSimulation()
{
	int iCount = m_aryItems.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryItems[iIndex]->ResetSimulation();
}

void ActivatedItemMgr::StepSimulation()
{
	ActivatedItem *lpItem;
	int iCount = m_aryItems.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpItem = m_aryItems[iIndex];

		if(lpItem->NeedToActivate())
			lpItem->Activate();
		else if(lpItem->NeedToDeactivate())
			lpItem->Deactivate();

		if(lpItem->IsActivated())
			lpItem->StepSimulation();
	}
}

}			//AnimatSim
