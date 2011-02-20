// ActivatedItem.cpp: implementation of the ActivatedItem class.
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

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

ActivatedItem::~ActivatedItem()
{

}

BOOL ActivatedItem::Enabled()
{return m_bEnabled;}

void ActivatedItem::Enabled(BOOL bVal)
{m_bEnabled = bVal;}

BOOL ActivatedItem::LoadedTime()
{return m_bLoadedTime;}

void ActivatedItem::LoadedTime(BOOL bVal)
{m_bLoadedTime = bVal;}

long ActivatedItem::StartSlice() 
{return m_lStartSlice;}

void ActivatedItem::StartSlice(long lVal) 
{m_lStartSlice = lVal;}

long ActivatedItem::EndSlice() 
{return m_lEndSlice;}

void ActivatedItem::EndSlice(long lVal) 
{m_lEndSlice = lVal;}

float ActivatedItem::StartTime() 
{return m_fltStartTime;}

void ActivatedItem::StartTime(float fltVal) 
{
	Simulator *lpSim = GetSimulator();
	m_fltStartTime = fltVal;
	m_lStartSlice = (long) (m_fltStartTime / lpSim->TimeStep() + 0.5);
}

float ActivatedItem::EndTime() 
{return m_fltEndTime;}

void ActivatedItem::EndTime(float fltVal) 
{
	Simulator *lpSim = GetSimulator();
	m_fltEndTime = fltVal;
	m_lEndSlice = (long) (m_fltEndTime / lpSim->TimeStep() + 0.5);
}

int ActivatedItem::StepInterval() 
{return m_iStepInterval;}

void ActivatedItem::StepInterval(int iVal) 
{m_iStepInterval = iVal;}

int ActivatedItem::StepIntervalCount() 
{return m_iStepIntervalCount;}

void ActivatedItem::StepIntervalCount(int iVal) 
{m_iStepIntervalCount = iVal;}

BOOL ActivatedItem::AlwaysActive()
{return m_bAlwaysActive;}

void ActivatedItem::AlwaysActive(BOOL bVal)
{m_bAlwaysActive = bVal;}

BOOL ActivatedItem::NeedToActivate(Simulator *lpSim)
{
	if(!m_bIsActivated && m_bEnabled && (m_bAlwaysActive || (m_lStartSlice <= lpSim->TimeSlice() && m_lEndSlice >= lpSim->TimeSlice())))
		return TRUE;
	return FALSE;
}

BOOL ActivatedItem::NeedToDeactivate(Simulator *lpSim)
{
	if(m_bIsActivated && !m_bEnabled)
		return TRUE;

	if(m_bIsActivated && !m_bAlwaysActive && !(m_lStartSlice <= lpSim->TimeSlice() && m_lEndSlice >= lpSim->TimeSlice()) )
		return TRUE;
	return FALSE;
}

void ActivatedItem::Activate(Simulator *lpSim)
{
	m_bIsActivated = TRUE;
}

void ActivatedItem::Deactivate(Simulator *lpSim)
{
	m_bIsActivated = FALSE;
}

void ActivatedItem::ResetSimulation(Simulator *lpSim)
{
	m_bIsActivated = FALSE;
}

BOOL ActivatedItem::Overlaps(ActivatedItem *lpItem)
{
	if( (lpItem->StartSlice() >= this->StartSlice()) && (lpItem->StartSlice() <= this->EndSlice()) )
		return TRUE;

	if( (lpItem->EndSlice() >= this->StartSlice()) && (lpItem->EndSlice() <= this->EndSlice()) )
		return TRUE;

	return FALSE;
}

void ActivatedItem::Initialize(Simulator *lpSim)
{
	if(m_bLoadedTime)
	{
		m_lStartSlice = (long) (m_fltStartTime / lpSim->TimeStep() + 0.5);
		m_lEndSlice = (long) (m_fltEndTime / lpSim->TimeStep() + 0.5);

		Std_IsAboveMin((long) -1, m_lStartSlice, TRUE, "StartSlice");
		Std_IsAboveMin(m_lStartSlice, m_lEndSlice, TRUE, "EndSlice");
	}
	else
	{
		m_fltStartTime = m_lStartSlice * lpSim->TimeStep();
		m_fltEndTime = m_lEndSlice * lpSim->TimeStep();
	}

	m_bInitialized = TRUE;
}

void ActivatedItem::ReInitialize(Simulator *lpSim)
{
	if(!m_bInitialized)
		Initialize(lpSim);
}

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

BOOL ActivatedItem::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

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

void ActivatedItem::Load(Simulator *lpSim, CStdXml &oXml)
{
	if(!lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);
		
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Item Element

	m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled);

	if(oXml.FindChildElement("StartTime", FALSE))
	{
		m_bLoadedTime = TRUE;
		m_fltStartTime = oXml.GetChildFloat("StartTime");
		m_fltEndTime = oXml.GetChildFloat("EndTime");

		m_lStartSlice = (long) (m_fltStartTime / lpSim->TimeStep() + 0.5);
		m_lEndSlice = (long) (m_fltEndTime / lpSim->TimeStep() + 0.5);

		Std_IsAboveMin((float) -1, m_fltStartTime, TRUE, "StartTime");
		Std_IsAboveMin(m_fltStartTime, m_fltEndTime, TRUE, "EndTime");
	}
	else
	{
		m_bLoadedTime = FALSE;
		m_lStartSlice = oXml.GetChildLong("StartSlice");
		m_lEndSlice = oXml.GetChildLong("EndSlice");

		Std_IsAboveMin((long) -1, m_lStartSlice, TRUE, "StartSlice");
		Std_IsAboveMin(m_lStartSlice, m_lEndSlice, TRUE, "EndSlice");
	}

	m_iStepInterval = oXml.GetChildInt("StepInterval", m_iStepInterval);
	Std_IsAboveMin((int) 0, m_iStepInterval, TRUE, "StepInterval");	

	m_bAlwaysActive = oXml.GetChildBool("AlwaysActive", m_bAlwaysActive);

	oXml.OutOfElem(); //OutOf Item Element
}

void ActivatedItem::Trace(ostream &oOs)
{
	oOs << "Name: " << m_strName << " Time (" << m_fltStartTime << ", " << m_fltEndTime << ") Slice: (" << m_lStartSlice << ", " << m_lEndSlice << ")";
}

BOOL LessThanActivatedItemCompare(ActivatedItem *lpItem1, ActivatedItem *lpItem2)
{
	return lpItem1->operator<(lpItem2);
}


}			//AnimatSim