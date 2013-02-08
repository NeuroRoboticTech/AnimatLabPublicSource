/**
\file	ActivatedItem.cpp

\brief	Implements the activated item class. 
**/

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
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

namespace AnimatSim
{
/**
\fn	ActivatedItem::ActivatedItem()

\brief	Default constructor. 

\author	dcofer
\date	3/1/2011
**/
ActivatedItem::ActivatedItem()
{
	m_bEnabled = TRUE;
	m_lStartSlice = 0;
	m_lEndSlice = 0;
	m_fltStartTime = 0;
	m_fltEndTime = 0;
	m_bLoadedTime = FALSE;
	m_iStepInterval = 1;
	m_iStepIntervalCount = 1;
	m_bAlwaysActive = FALSE;
	m_bInitialized = FALSE;
	m_bIsActivated = FALSE;
}

/**
\fn	ActivatedItem::~ActivatedItem()

\brief	Destructor. 

\author	dcofer
\date	3/1/2011
**/
ActivatedItem::~ActivatedItem()
{

}

/**
\fn	BOOL ActivatedItem::Enabled()

\brief	Gets whether the item is enabled or not. 

\author	dcofer
\date	3/1/2011

\return	true if it enabled, false if not. 
**/
BOOL ActivatedItem::Enabled()
{return m_bEnabled;}

/**
\fn	void ActivatedItem::Enabled(BOOL bVal)

\brief	Enables the item. 

\author	dcofer
\date	3/1/2011

\param	bVal	true to enable, false to disable. 
**/
void ActivatedItem::Enabled(BOOL bVal)
{
	m_bEnabled = bVal;

	if(m_lpSim->Paused() && m_lpSim->TimeSlice() > 0)
	{
		if(!m_bEnabled && IsActivated())
			Deactivate();
		else if(m_bEnabled && !IsActivated() && NeedToActivate())
			Activate();
	}
}

/**
\fn	BOOL ActivatedItem::LoadedTime()

\brief	Gets whether time was loaded or time slices. 

\author	dcofer
\date	3/1/2011

\return	true if time loaded, false if timeslices loaded. 
**/
BOOL ActivatedItem::LoadedTime()
{return m_bLoadedTime;}

/**
\fn	void ActivatedItem::LoadedTime(BOOL bVal)

\brief	Sets whether time was loaded. 

\author	dcofer
\date	3/1/2011

\param	bVal	true if time loaded. 
**/
void ActivatedItem::LoadedTime(BOOL bVal)
{m_bLoadedTime = bVal;}

/**
\fn	long ActivatedItem::StartSlice()

\brief	Returns the starts slice for activation. 

\author	dcofer
\date	3/1/2011

\return	Start time slice for activation. 
**/
long ActivatedItem::StartSlice() 
{return m_lStartSlice;}

/**
\fn	void ActivatedItem::StartSlice(long lVal)

\brief	Sets the start time slice for activation. 

\author	dcofer
\date	3/1/2011

\param	lVal	The start time slice for activation. 
\param bReInit Used by other types of activated items like charts to tell if they need to reinitialize or not.
**/
void ActivatedItem::StartSlice(long lVal, BOOL bReInit) 
{
	Std_IsAboveMin((long) -1, lVal, TRUE, "StartSlice");
	Std_IsBelowMax(m_lEndSlice, lVal, TRUE, "StartSlice");

	m_lStartSlice = lVal;
	m_fltStartTime = m_lStartSlice* m_lpSim->TimeStep();
}

/**
\fn	long ActivatedItem::EndSlice()

\brief	Gets the end time slice for deactivation. 

\author	dcofer
\date	3/1/2011

\return	End time slice for deactivation. 
**/
long ActivatedItem::EndSlice() 
{return m_lEndSlice;}

/**
\fn	void ActivatedItem::EndSlice(long lVal)

\brief	Sets the ends time slice for deactivation. 

\author	dcofer
\date	3/1/2011

\param	lVal	The ends time slice for deactivation. 
\param bReInit Used by other types of activated items like charts to tell if they need to reinitialize or not.
**/
void ActivatedItem::EndSlice(long lVal, BOOL bReInit) 
{
	Std_IsAboveMin((long) -1, lVal, TRUE, "EndSlice");
	Std_IsAboveMin(m_lStartSlice, lVal, TRUE, "EndSlice");

	m_lEndSlice = lVal;
	m_fltEndTime = m_lEndSlice* m_lpSim->TimeStep();
}

/**
\fn	float ActivatedItem::StartTime()

\brief	Gets the simulation start time for activation. 

\author	dcofer
\date	3/1/2011

\return	Time for activation. 
**/
float ActivatedItem::StartTime() 
{return m_fltStartTime;}

/**
\fn	void ActivatedItem::StartTime(float fltVal)

\brief	Sets the start simulation time for activation. 

\author	dcofer
\date	3/1/2011

\param	fltVal	The simulation time for activation. 
\param bReInit Used by other types of activated items like charts to tell if they need to reinitialize or not.
**/
void ActivatedItem::StartTime(float fltVal, BOOL bReInit) 
{
	Std_IsAboveMin((float) -1, fltVal, TRUE, "StartTime");
	Std_IsBelowMax(m_fltEndTime, fltVal, TRUE, "StartTime");

	m_fltStartTime = fltVal;
	m_lStartSlice = (long) (m_fltStartTime / m_lpSim->TimeStep() + 0.5);
}

/**
\fn	float ActivatedItem::EndTime()

\brief	Gets the end simulation time for deactivation. 

\author	dcofer
\date	3/1/2011

\return	the end simulation time for deactivation.. 
**/
float ActivatedItem::EndTime() 
{return m_fltEndTime;}

/**
\fn	void ActivatedItem::EndTime(float fltVal)

\brief	Sets the ends simulation time for deactivation. 

\author	dcofer
\date	3/1/2011

\param	fltVal	The ends simulation time for deactivation. 
\param bReInit Used by other types of activated items like charts to tell if they need to reinitialize or not.
**/
void ActivatedItem::EndTime(float fltVal, BOOL bReInit) 
{
	Std_IsAboveMin((float) -1, fltVal, TRUE, "EndTime");
	Std_IsAboveMin(m_fltStartTime, fltVal, TRUE, "EndTime");

	m_fltEndTime = fltVal;
	m_lEndSlice = (long) (m_fltEndTime / m_lpSim->TimeStep() + 0.5);
}

/**
\fn	int ActivatedItem::StepInterval()

\brief	Gets the step interval. 

\author	dcofer
\date	3/1/2011

\return	Step Interval value. 
**/
int ActivatedItem::StepInterval() 
{return m_iStepInterval;}

/**
\fn	void ActivatedItem::StepInterval(int iVal)

\brief	Sets the step interval. 

\author	dcofer
\date	3/1/2011

\param	iVal The step interval value. 
**/
void ActivatedItem::StepInterval(int iVal) 
{
	Std_IsAboveMin((int) 0, iVal, TRUE, "StepInterval");	

	m_iStepInterval = iVal;
}

/**
\fn	int ActivatedItem::StepIntervalCount()

\brief	Gets the step interval count. 

\details Keeps track of how many iterations till we reach the StepInterval and call StepSimulation again.

\author	dcofer
\date	3/1/2011

\return	Step interval count. 
**/
int ActivatedItem::StepIntervalCount() 
{return m_iStepIntervalCount;}

/**
\fn	void ActivatedItem::StepIntervalCount(int iVal)

\brief	Step interval count. 

\author	dcofer
\date	3/1/2011

\param	iVal The step interval count value. 
**/
void ActivatedItem::StepIntervalCount(int iVal) 
{m_iStepIntervalCount = iVal;}

/**
\fn	BOOL ActivatedItem::AlwaysActive()

\brief	Gets whether this item is always active. 

\author	dcofer
\date	3/1/2011

\return	true if always active, false if not. 
**/
BOOL ActivatedItem::AlwaysActive()
{return m_bAlwaysActive;}

/**
\fn	void ActivatedItem::AlwaysActive(BOOL bVal)

\brief	Sets whether this item is always active. 

\author	dcofer
\date	3/1/2011

\param	bVal true if always active. 
**/
void ActivatedItem::AlwaysActive(BOOL bVal)
{m_bAlwaysActive = bVal;}

/**
\fn	BOOL ActivatedItem::IsActivated()

\brief	Query if this object is activated. 

\author	dcofer
\date	3/1/2011

\return	true if activated, false if not. 
**/
BOOL ActivatedItem::IsActivated() {return m_bIsActivated;}

/**
\fn	BOOL ActivatedItem::IsInitialized()

\brief	Query if this object is initialized. 

\author	dcofer
\date	3/1/2011

\return	true if initialized, false if not. 
**/
BOOL ActivatedItem::IsInitialized() {return m_bInitialized;}

/**
\fn	BOOL ActivatedItem::NeedToActivate()

\brief	Determines if this item needs to be activated. 

\author	dcofer
\date	3/1/2011

\return	true if the item is enabled and it is always active or if the current simulation time is
within the start and end times. 
**/

BOOL ActivatedItem::NeedToActivate()
{
	if(!m_bIsActivated && m_bEnabled && (m_bAlwaysActive || (m_lStartSlice <= m_lpSim->TimeSlice() && m_lEndSlice >= m_lpSim->TimeSlice())))
		return TRUE;
	return FALSE;
}

/**
\fn	BOOL ActivatedItem::NeedToDeactivate()

\brief	Determines if this item needs to be deactivated. 

\author	dcofer
\date	3/1/2011

\return	true if the item is not enabled or it is not always active and the current simulation
time is outside of the start and end times. 
**/

BOOL ActivatedItem::NeedToDeactivate()
{
	if(m_bIsActivated && !m_bEnabled)
		return TRUE;

	if(m_bIsActivated && !m_bAlwaysActive && !(m_lStartSlice <= m_lpSim->TimeSlice() && m_lEndSlice >= m_lpSim->TimeSlice()) )
		return TRUE;
	return FALSE;
}

/**
\fn	void ActivatedItem::Activate()

\brief	Activates this item. 

\author	dcofer
\date	3/1/2011
**/

void ActivatedItem::Activate()
{
	m_bIsActivated = TRUE;
}

/**
\fn	void ActivatedItem::Deactivate()

\brief	Deactivates this item. 

\author	dcofer
\date	3/1/2011
**/

void ActivatedItem::Deactivate()
{
	m_bIsActivated = FALSE;
}

void ActivatedItem::ResetSimulation()
{
	m_bIsActivated = FALSE;
}

/**
\fn	BOOL ActivatedItem::Overlaps(ActivatedItem *lpItem)

\brief	Query if this object overlaps the time period for another ActivatedItem. 

\author	dcofer
\date	3/1/2011

\param [in,out]	lpItem	Pointer to the item to test against. 

\return	true if it overlaps, false if not. 
**/
BOOL ActivatedItem::Overlaps(ActivatedItem *lpItem)
{
	if( (lpItem->StartSlice() >= this->StartSlice()) && (lpItem->StartSlice() <= this->EndSlice()) )
		return TRUE;

	if( (lpItem->EndSlice() >= this->StartSlice()) && (lpItem->EndSlice() <= this->EndSlice()) )
		return TRUE;

	return FALSE;
}


void ActivatedItem::Initialize()
{
	AnimatBase::Initialize();
	SetSliceData();
	m_bInitialized = TRUE;
}

void ActivatedItem::SetSliceData()
{
	if(m_bLoadedTime)
	{
		m_lStartSlice = (long) (m_fltStartTime / m_lpSim->TimeStep() + 0.5);
		m_lEndSlice = (long) (m_fltEndTime / m_lpSim->TimeStep() + 0.5);

		Std_IsAboveMin((long) -1, m_lStartSlice, TRUE, "StartSlice");
		Std_IsAboveMin(m_lStartSlice, m_lEndSlice, TRUE, "EndSlice");
	}
	else
	{
		m_fltStartTime = m_lStartSlice * m_lpSim->TimeStep();
		m_fltEndTime = m_lEndSlice * m_lpSim->TimeStep();
	}
}

void ActivatedItem::ReInitialize()
{
	if(!m_bInitialized)
		Initialize();
	else
		SetSliceData();
}

void ActivatedItem::TimeStepModified()
{
	ReInitialize();
}

/**
\fn	BOOL ActivatedItem::NeedToStep()

\brief	Tells if this item needs to call StepSimulation or not. 

\author	dcofer
\date	3/1/2011

\return	If StepInterval is less than 1 then it always returns true. 
Otherwise, it returns true if the StepIntervalCount is equal to the 
StepInterval. 
**/
BOOL ActivatedItem::NeedToStep()
{
	if(m_iStepInterval <= 1)
		return TRUE;
	else if(m_iStepIntervalCount == -1 || m_iStepIntervalCount == m_iStepInterval)
	{
		m_iStepIntervalCount = 1;
		return TRUE;
	}
	else
	{
		m_iStepIntervalCount++;
		return FALSE;
	}
}

BOOL ActivatedItem::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);
	
	if(AnimatBase::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "STARTTIME")
	{
		StartTime(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "ENDTIME")
	{
		EndTime(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "ALWAYSACTIVE")
	{
		AlwaysActive(Std_ToBool(strValue));
		return TRUE;
	}

	if(strType == "STEPINTERVAL")
	{
		StepInterval(atoi(strValue.c_str()));
		return TRUE;
	}

	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void ActivatedItem::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Item Element

	//Do not call the mutator here because it will run code we do not want to run on load.
	m_bEnabled = oXml.GetChildBool("Enabled", "True");

	if(oXml.FindChildElement("StartTime", FALSE))
	{
		m_bLoadedTime = TRUE;
		EndTime(oXml.GetChildFloat("EndTime"), FALSE);
		StartTime(oXml.GetChildFloat("StartTime"), FALSE);
	}
	else
	{
		m_bLoadedTime = FALSE;
		EndSlice(oXml.GetChildLong("EndSlice"), FALSE);
		StartSlice(oXml.GetChildLong("StartSlice"), FALSE);
	}

	StepInterval(oXml.GetChildInt("StepInterval", m_iStepInterval));
	AlwaysActive(oXml.GetChildBool("AlwaysActive", m_bAlwaysActive));

	oXml.OutOfElem(); //OutOf Item Element
}

/**
\fn	BOOL LessThanActivatedItemCompare(ActivatedItem *lpItem1, ActivatedItem *lpItem2)

\brief	Compares the start times of two activated items to see which is sooner.

\details This is primarily used for sorting of ActivateItems.

\author	dcofer
\date	3/1/2011

\param [in,out]	The first pointer to an item. 
\param [in,out]	The second pointer to an item. 

\return	true if it item1 less than item 2. 
**/

BOOL LessThanActivatedItemCompare(ActivatedItem *lpItem1, ActivatedItem *lpItem2)
{
	return lpItem1->operator<(lpItem2);
}


}			//AnimatSim