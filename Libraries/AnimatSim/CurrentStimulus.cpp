// CurrentStimulus.cpp: implementation of the CurrentStimulus class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Gain.h"
#include "Adapter.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "NervousSystem.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "ExternalStimulus.h"
#include "ExternalInputStimulus.h"
#include "CurrentStimulus.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Simulator.h"

namespace AnimatSim
{
	namespace ExternalStimuli
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CurrentStimulus::CurrentStimulus()
{
	m_lpOrganism = NULL;
	m_lpNode = NULL;
	m_lpExternalCurrent = NULL;
	m_fltCycleOnDuration = 0;
	m_fltCycleOffDuration = 0;
	m_fltBurstOnDuration = 0;
	m_fltBurstOffDuration = 0;
	m_lCycleOnDuration = 0;
	m_lCycleOffDuration = 0;
	m_lBurstOnDuration = 0;
	m_lBurstOffDuration = 0;
	m_fltCurrentOn = (float) 10e-9;
	m_fltCurrentOff = 0;
	m_fltCurrentBurstOff = 0;
	m_fltActiveCurrent = m_fltCurrentOn;
	m_fltInitialActiveCurrent = m_fltActiveCurrent;
	m_lCycleStart = 0;
	m_lBurstStart = 0;
	m_bCycleOn = TRUE;
	m_bBurstOn = TRUE;
	m_lpCurrentOnEval = NULL;
	m_iType = AL_TONIC_CURRENT;
}

CurrentStimulus::~CurrentStimulus()
{

try
{
	m_lpOrganism = NULL;
	m_lpNode = NULL;
	m_lpExternalCurrent = NULL;
	if(m_lpCurrentOnEval) 
		delete m_lpCurrentOnEval;

}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CurrentStimulus\r\n", "", -1, FALSE, TRUE);}
}

void CurrentStimulus::CurrentOn(float fltVal)
{m_fltCurrentOn = fltVal;}

void CurrentStimulus::CycleOnDuration(float fltVal)
{
	Simulator *lpSim = GetSimulator();
	m_fltCycleOnDuration = fltVal;
	m_lCycleOnDuration = (long) (m_fltCycleOnDuration / lpSim->TimeStep() + 0.5);
}

void CurrentStimulus::CycleOffDuration(float fltVal)
{
	Simulator *lpSim = GetSimulator();
	m_fltCycleOffDuration = fltVal;
	m_lCycleOffDuration = (long) (m_fltCycleOffDuration / lpSim->TimeStep() + 0.5);
}

void CurrentStimulus::BurstOnDuration(float fltVal)
{
	Simulator *lpSim = GetSimulator();
	m_fltBurstOnDuration = fltVal;
	m_lBurstOnDuration = (long) (m_fltBurstOnDuration / lpSim->TimeStep() + 0.5);
}

void CurrentStimulus::BurstOffDuration(float fltVal)
{
	Simulator *lpSim = GetSimulator();
	m_fltBurstOffDuration = fltVal;
	m_lBurstOffDuration = (long) (m_fltBurstOffDuration / lpSim->TimeStep() + 0.5);
}

void CurrentStimulus::Initialize(Simulator *lpSim)
{
	if(!lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	ExternalStimulus::Initialize(lpSim);

	m_lCycleOnDuration = (long) (m_fltCycleOnDuration / lpSim->TimeStep() + 0.5);
	m_lCycleOffDuration = (long) (m_fltCycleOffDuration / lpSim->TimeStep() + 0.5);
	m_lBurstOnDuration = (long) (m_fltBurstOnDuration / lpSim->TimeStep() + 0.5);
	m_lBurstOffDuration = (long) (m_fltBurstOffDuration / lpSim->TimeStep() + 0.5);

	//Lets try and get the node we will dealing with.
	m_lpOrganism = lpSim->FindOrganism(m_strOrganismID);
	m_lpNode = dynamic_cast<Node *>(lpSim->FindByID(m_strTargetNodeID));
	if(!m_lpNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetNodeID);

	m_lpExternalCurrent = m_lpNode->GetDataPointer("ExternalCurrent");

	if(!m_lpExternalCurrent)
		THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, 
		("Stimulus: " + m_strID + " OrganismID: " + m_strOrganismID + 
		"Node: " + m_strTargetNodeID + " DataType: ExternalCurrent"));
}

float CurrentStimulus::GetCurrentOn(Simulator *lpSim)
{
	if(m_lpCurrentOnEval)
	{
		m_lpCurrentOnEval->SetVariable("t", (lpSim->Time()-m_fltStartTime) );
		return 1e-9*m_lpCurrentOnEval->Solve();
	}
	else
		return m_fltCurrentOn;

}

void CurrentStimulus::Activate(Simulator *lpSim)
{
	ExternalStimulus::Activate(lpSim);

	//Start a cycle and a burst.
	m_fltActiveCurrent = GetCurrentOn(lpSim);

	m_lCycleStart = lpSim->TimeSlice();
	m_lBurstStart = m_lCycleStart;

	*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
}

void CurrentStimulus::StepSimulation(Simulator *lpSim)
{
	long lCycleDiff = lpSim->TimeSlice() - m_lCycleStart;
	long lBurstDiff = lpSim->TimeSlice() - m_lBurstStart;

	if(m_bBurstOn)
	{
		if( (m_bCycleOn && (lCycleDiff >= m_lCycleOnDuration)) )
		{
			m_bCycleOn = FALSE;
			m_lCycleStart = lpSim->TimeSlice();
			
			*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
			m_fltActiveCurrent = m_fltCurrentOff;
			*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
		}
		else if( (!m_bCycleOn && (lCycleDiff >= m_lCycleOffDuration)) )
		{
			m_bCycleOn = TRUE;
			m_lCycleStart = lpSim->TimeSlice();

			*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
			m_fltActiveCurrent = GetCurrentOn(lpSim);
			*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
		}
	}

	if( (m_bBurstOn && (lBurstDiff >= m_lBurstOnDuration)) )
	{
		m_bCycleOn = FALSE;
		m_bBurstOn = FALSE;
		m_lBurstStart = lpSim->TimeSlice();

		*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
		m_fltActiveCurrent = m_fltCurrentBurstOff;
		*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
	}
	else if( (!m_bBurstOn && (lBurstDiff >= m_lBurstOffDuration)) )
	{
		m_bCycleOn = TRUE;
		m_bBurstOn = TRUE;
		m_lBurstStart = lpSim->TimeSlice();
		m_lCycleStart = m_lBurstStart;

		*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
		m_fltActiveCurrent = GetCurrentOn(lpSim);
		*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
	}
	else if(m_iType == AL_TONIC_CURRENT && m_lpCurrentOnEval)
	{
		*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
		m_fltActiveCurrent = GetCurrentOn(lpSim);
		*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
	}

}

void CurrentStimulus::Deactivate(Simulator *lpSim)
{		
	ExternalStimulus::Deactivate(lpSim);
	*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
}

void CurrentStimulus::ResetSimulation(Simulator *lpSim)
{
	ExternalStimulus::ResetSimulation(lpSim);

	m_bCycleOn = TRUE;
	m_bBurstOn = TRUE;
	m_lCycleStart = 0;
	m_lBurstStart = 0;
	m_fltActiveCurrent = m_fltInitialActiveCurrent;
}

float *CurrentStimulus::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	if(strType == "ACTIVECURRENT")
		lpData = &m_fltActiveCurrent;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "StimulusName: " + STR(m_strName) + "  DataType: " + strDataType);

	return lpData;
} 

BOOL CurrentStimulus::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(ExternalStimulus::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "CURRENTON")
	{
		CurrentOn(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "CURRENTOFF")
	{
		CurrentOff(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "CURRENTBURSTOFF")
	{
		CurrentBurstOff(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "CYCLEONDURATION")
	{
		CycleOnDuration(atof(strValue.c_str()));
		return TRUE;
	}
	
	if(strType == "CYCLEOFFDURATION")
	{
		CycleOffDuration(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "BURSTONDURATION")
	{
		BurstOnDuration(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "BURSTOFFDURATION")
	{
		BurstOffDuration(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void CurrentStimulus::Load(CStdXml &oXml)
{
	ActivatedItem::Load(oXml);

	oXml.IntoElem();  //Into Simulus Element

	m_strOrganismID = oXml.GetChildString("OrganismID");

	if(Std_IsBlank(m_strOrganismID))
		THROW_PARAM_ERROR(Al_Err_lOrganismIDBlank, Al_Err_strOrganismIDBlank, "ID", m_strID);

	string strType = oXml.GetChildString("CurrentType", "TONIC");
	strType = Std_ToUpper(Std_Trim(strType));
	if(strType == "TONIC")
		m_iType = AL_TONIC_CURRENT;
	else if(strType == "REPETITIVE")
		m_iType = AL_REPETITIVE_CURRENT;
	else if(strType == "BURST")
		m_iType = AL_BURST_CURRENT;

	m_strTargetNodeID = oXml.GetChildString("TargetNodeID");

	m_fltCycleOnDuration = oXml.GetChildFloat("CycleOnDuration", m_fltCycleOnDuration);
	m_fltCycleOffDuration = oXml.GetChildFloat("CycleOffDuration", m_fltCycleOffDuration);
	m_fltBurstOnDuration = oXml.GetChildFloat("BurstOnDuration", m_fltBurstOnDuration);
	m_fltBurstOffDuration = oXml.GetChildFloat("BurstOffDuration", m_fltBurstOffDuration);

	m_fltCurrentOn = oXml.GetChildFloat("CurrentOn", m_fltCurrentOn);
	m_fltCurrentOff = oXml.GetChildFloat("CurrentOff", m_fltCurrentOff);
	m_fltCurrentBurstOff = oXml.GetChildFloat("CurrentBurstOff", m_fltCurrentBurstOff);
	
	string strEquation = oXml.GetChildString("CurrentOnEquation", "");

	if(!Std_IsBlank(strEquation))
	{
		//Initialize the postfix evaluator.
		if(m_lpCurrentOnEval) 
		{delete m_lpCurrentOnEval; m_lpCurrentOnEval = NULL;}

		m_lpCurrentOnEval = new CStdPostFixEval;

		m_lpCurrentOnEval->AddVariable("t");
		m_lpCurrentOnEval->Equation(strEquation);
		m_lpCurrentOnEval->SetVariable("t", 0);
		m_fltActiveCurrent = m_lpCurrentOnEval->Solve();
	}
	else
		m_fltActiveCurrent = m_fltCurrentOn;

	m_fltInitialActiveCurrent = m_fltActiveCurrent;

	Std_IsAboveMin((float) 0, m_fltCycleOnDuration, TRUE, "CycleOnDuration", FALSE);
	Std_IsAboveMin((float) 0, m_fltCycleOffDuration, TRUE, "CycleOffDuration", TRUE);

	Std_IsAboveMin((float) 0, m_fltBurstOnDuration, TRUE, "BurstOnDuration", FALSE);
	Std_IsAboveMin((float) 0, m_fltBurstOffDuration, TRUE, "BurstOffDuration", TRUE);

	//If we have always active then we need to reset some of the durations to make sure
	//they last for a very long time.
	if(m_bAlwaysActive)
	{
		if(strType == "TONIC")
		{
			m_fltCycleOnDuration = 10000;
			m_fltBurstOnDuration = 10000;
		}
		else if(strType == "REPETITIVE")
			m_fltBurstOnDuration = 10000;
	}

	oXml.OutOfElem(); //OutOf Simulus Element
}

	}			//ExternalStimuli
}				//VortexAnimatSim




