// ActivatedItemMgr.cpp: implementation of the ActivatedItemMgr class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

//#include "ClassFactory.h"
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

ActivatedItemMgr::ActivatedItemMgr()
{
}

ActivatedItemMgr::~ActivatedItemMgr()
{

try
{
	Reset();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ActivatedItemMgr\r\n", "", -1, FALSE, TRUE);}
}

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

void ActivatedItemMgr::Add(Simulator *lpSim, ActivatedItem *lpItem)
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

void ActivatedItemMgr::Remove(Simulator *lpSim, string strID, BOOL bThrowError)
{
	int iIndex=0;
	ActivatedItem *lpItem = Find(strID, iIndex, bThrowError);

	if(lpItem)
	{
		if(lpItem->IsActivated())
			lpItem->Deactivate(lpSim);

		m_aryItems.RemoveAt(iIndex);

		//Must remove it from here last because this stores the actual pointer.
		m_aryItemsMap.Remove(Std_CheckString(strID));
	}

	stable_sort(m_aryItems.begin(), m_aryItems.end(), LessThanActivatedItemCompare);
}

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

void ActivatedItemMgr::Initialize(Simulator *lpSim)
{
	int iCount = m_aryItems.GetSize();
	for(int iChart=0; iChart<iCount; iChart++)
		m_aryItems[iChart]->Initialize(lpSim);

	stable_sort(m_aryItems.begin(), m_aryItems.end(), LessThanActivatedItemCompare);
}

void ActivatedItemMgr::ReInitialize(Simulator *lpSim)
{
	int iCount = m_aryItems.GetSize();
	for(int iChart=0; iChart<iCount; iChart++)
		m_aryItems[iChart]->ReInitialize(lpSim);

	stable_sort(m_aryItems.begin(), m_aryItems.end(), LessThanActivatedItemCompare);
}

void ActivatedItemMgr::ResetSimulation(Simulator *lpSim)
{
	int iCount = m_aryItems.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryItems[iIndex]->ResetSimulation(lpSim);
}

void ActivatedItemMgr::StepSimulation(Simulator *lpSim)
{
	ActivatedItem *lpItem;
	int iCount = m_aryItems.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpItem = m_aryItems[iIndex];

		if(lpItem->NeedToActivate(lpSim))
			lpItem->Activate(lpSim);
		else if(lpItem->NeedToDeactivate(lpSim))
			lpItem->Deactivate(lpSim);

		if(lpItem->IsActivated())
			lpItem->StepSimulation(lpSim);
	}
}

}			//AnimatSim
