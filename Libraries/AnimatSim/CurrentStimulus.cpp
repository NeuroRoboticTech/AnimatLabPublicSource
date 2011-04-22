/**
\file	CurrentStimulus.cpp

\brief	Implements the current stimulus class. 
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBase.h"
#include "IPhysicsBody.h"
#include "MovableItem.h"
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
/**
\brief	Default constructor. 

\author	dcofer
\date	3/16/2011
**/
CurrentStimulus::CurrentStimulus()
{
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

/**
\brief	Destructor. 

\author	dcofer
\date	3/16/2011
**/
CurrentStimulus::~CurrentStimulus()
{

try
{
	m_lpNode = NULL;
	m_lpExternalCurrent = NULL;
	if(m_lpCurrentOnEval) 
		delete m_lpCurrentOnEval;

}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CurrentStimulus\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the integer value of current type. This is (Constant, repetitive, and bursting) 

\author	dcofer
\date	3/17/2011

\return	. 
**/
//int CurrentStimulus::Type() {return m_iType;}

/**
\brief	Gets the Stimulus type. 

\author	dcofer
\date	3/16/2011

\return	Stimulus type string descriptor. 
**/
string CurrentStimulus::Type() {return "CurrentStimulus";}

void CurrentStimulus::Type(string strType)
{
	m_strType = Std_ToUpper(Std_Trim(strType));
	if(m_strType == "TONIC")
		m_iType = AL_TONIC_CURRENT;
	else if(m_strType == "REPETITIVE")
		m_iType = AL_REPETITIVE_CURRENT;
	else if(m_strType == "BURST")
		m_iType = AL_BURST_CURRENT;
	else
		THROW_PARAM_ERROR(Al_Err_lInvalidCurrentType, Al_Err_strInvalidCurrentType, "Type", strType);
}

void CurrentStimulus::AlwaysActive(BOOL bVal)
{
	ActivatedItem::AlwaysActive(bVal);

	//If we have always active then we need to reset some of the durations to make sure
	//they last for a very long time.
	if(m_bAlwaysActive)
	{
		if(m_iType == AL_TONIC_CURRENT)
		{
			m_fltCycleOnDuration = 10000;
			m_fltBurstOnDuration = 10000;
		}
		else if(m_iType == AL_REPETITIVE_CURRENT)
			m_fltBurstOnDuration = 10000;
	}
}
/**
\brief	Gets the on cycle current value. 

\author	dcofer
\date	3/16/2011

\return	On cycle current value. 
**/
float CurrentStimulus::CurrentOn() {return m_fltCurrentOn;}

/**
\brief	Sets the on cycle current value. 

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void CurrentStimulus::CurrentOn(float fltVal)
{m_fltCurrentOn = fltVal;}

/**
\brief	Gets the ffn cycle current value. 

\author	dcofer
\date	3/16/2011

\return	Offn cycle current value. 
**/
float CurrentStimulus::CurrentOff() {return m_fltCurrentOff;}

/**
\brief	Sets the off cycle current value. 

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void CurrentStimulus::CurrentOff(float fltVal) {m_fltCurrentOff = fltVal;}

/**
\brief	Gets the off burst current value. 

\author	dcofer
\date	3/16/2011

\return	Off burst current value. 
**/
float CurrentStimulus::CurrentBurstOff() {return m_fltCurrentBurstOff;}

/**
\brief	Sets the of burst current value. 

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void CurrentStimulus::CurrentBurstOff(float fltVal) {m_fltCurrentBurstOff = fltVal;}

/**
\brief	Gets the on cycle current duration in time. 

\author	dcofer
\date	3/16/2011

\return	On cycle duration value. 
**/
float CurrentStimulus::CycleOnDuration() {return m_fltCycleOnDuration;}

/**
\brief	Sets the on cycle duration value in time. 

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void CurrentStimulus::CycleOnDuration(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "CycleOnDuration", FALSE);
	m_fltCycleOnDuration = fltVal;
	m_lCycleOnDuration = (long) (m_fltCycleOnDuration / m_lpSim->TimeStep() + 0.5);
}

/**
\brief	Gets the off cycle current duration in time. 

\author	dcofer
\date	3/16/2011

\return	Off cycle duration value. 
**/
float CurrentStimulus::CycleOffDuration() {return m_fltCycleOffDuration;}

/**
\brief	Sets the off cycle duration value in time. 

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void CurrentStimulus::CycleOffDuration(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "CycleOffDuration", TRUE);
	m_fltCycleOffDuration = fltVal;
	m_lCycleOffDuration = (long) (m_fltCycleOffDuration / m_lpSim->TimeStep() + 0.5);
}

/**
\brief	Gets the on burst current duration in time. 

\author	dcofer
\date	3/16/2011

\return	On burst duration value. 
**/
float CurrentStimulus::BurstOnDuration() {return m_fltBurstOnDuration;}
			
/**
\brief	Sets the on burst duration value in time. 

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void CurrentStimulus::BurstOnDuration(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "BurstOnDuration", FALSE);
	m_fltBurstOnDuration = fltVal;
	m_lBurstOnDuration = (long) (m_fltBurstOnDuration / m_lpSim->TimeStep() + 0.5);
}

/**
\brief	Gets the off burst current duration in time. 

\author	dcofer
\date	3/16/2011

\return	Off burst duration value. 
**/
float CurrentStimulus::BurstOffDuration() {return m_fltBurstOffDuration;}

/**
\brief	Sets the off burst duration value in time. 

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void CurrentStimulus::BurstOffDuration(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "BurstOffDuration", TRUE);
	m_fltBurstOffDuration = fltVal;
	m_lBurstOffDuration = (long) (m_fltBurstOffDuration / m_lpSim->TimeStep() + 0.5);
}

/**
\brief	Gets the GUID ID of the node that is being stimulated. 

\author	dcofer
\date	3/17/2011

\return	. 
**/
string CurrentStimulus::TargetNodeID() {return m_strTargetNodeID;}

/**
\brief	Sets the GUID ID of the node that will be stimulated. 

\author	dcofer
\date	3/17/2011

\param	strID	Identifier for the string. 
**/
void CurrentStimulus::TargetNodeID(string strID)
{
	if(Std_IsBlank(strID))
		THROW_PARAM_ERROR(Al_Err_lInvalidCurrentType, Al_Err_strInvalidCurrentType, "ID", strID);
	m_strTargetNodeID = strID;
}

/**
\brief	Gets the post-fix current equation string. If this is null then an equation is not used. 
If one is specified then that equation is used during the cycle on times.

\author	dcofer
\date	3/17/2011

\return	. 
**/
string CurrentStimulus::CurrentEquation() {return m_strCurrentEquation;}

/**
\brief	Sets the postfix current equation to use. If this is blank then the 
current constants (current on/off) are used. If it is not blank then this equation is
used during the cycle on periods.

\author	dcofer
\date	3/17/2011

\param	strEquation	The post-fix string equation. 
**/
void CurrentStimulus::CurrentEquation(string strEquation)
{
	m_strCurrentEquation = strEquation;

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
}


void CurrentStimulus::Initialize()
{
	ExternalStimulus::Initialize();

	m_lCycleOnDuration = (long) (m_fltCycleOnDuration / m_lpSim->TimeStep() + 0.5);
	m_lCycleOffDuration = (long) (m_fltCycleOffDuration / m_lpSim->TimeStep() + 0.5);
	m_lBurstOnDuration = (long) (m_fltBurstOnDuration / m_lpSim->TimeStep() + 0.5);
	m_lBurstOffDuration = (long) (m_fltBurstOffDuration / m_lpSim->TimeStep() + 0.5);

	//Lets try and get the node we will dealing with.
	m_lpNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strTargetNodeID));
	if(!m_lpNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetNodeID);

	m_lpExternalCurrent = m_lpNode->GetDataPointer("ExternalCurrent");

	if(!m_lpExternalCurrent)
		THROW_TEXT_ERROR(Al_Err_lDataPointNotFound, Al_Err_strDataPointNotFound, 
		("Stimulus: " + m_strID + "Node: " + m_strTargetNodeID + " DataType: ExternalCurrent"));
}

/**
\brief	Calculates the on cycle current at this time step. 

\author	dcofer
\date	3/17/2011

\return	The current on. 
**/
float CurrentStimulus::GetCurrentOn()
{
	if(m_lpCurrentOnEval)
	{
		m_lpCurrentOnEval->SetVariable("t", (m_lpSim->Time()-m_fltStartTime) );
		return 1e-9*m_lpCurrentOnEval->Solve();
	}
	else
		return m_fltCurrentOn;

}

void CurrentStimulus::Activate()
{
	ExternalStimulus::Activate();

	//Start a cycle and a burst.
	m_fltActiveCurrent = GetCurrentOn();

	m_lCycleStart = m_lpSim->TimeSlice();
	m_lBurstStart = m_lCycleStart;

	*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
}

void CurrentStimulus::StepSimulation()
{
	long lCycleDiff = m_lpSim->TimeSlice() - m_lCycleStart;
	long lBurstDiff = m_lpSim->TimeSlice() - m_lBurstStart;

	if(m_bBurstOn)
	{
		if( (m_bCycleOn && (lCycleDiff >= m_lCycleOnDuration)) )
		{
			m_bCycleOn = FALSE;
			m_lCycleStart = m_lpSim->TimeSlice();
			
			*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
			m_fltActiveCurrent = m_fltCurrentOff;
			*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
		}
		else if( (!m_bCycleOn && (lCycleDiff >= m_lCycleOffDuration)) )
		{
			m_bCycleOn = TRUE;
			m_lCycleStart = m_lpSim->TimeSlice();

			*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
			m_fltActiveCurrent = GetCurrentOn();
			*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
		}
	}

	if( (m_bBurstOn && (lBurstDiff >= m_lBurstOnDuration)) )
	{
		m_bCycleOn = FALSE;
		m_bBurstOn = FALSE;
		m_lBurstStart = m_lpSim->TimeSlice();

		*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
		m_fltActiveCurrent = m_fltCurrentBurstOff;
		*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
	}
	else if( (!m_bBurstOn && (lBurstDiff >= m_lBurstOffDuration)) )
	{
		m_bCycleOn = TRUE;
		m_bBurstOn = TRUE;
		m_lBurstStart = m_lpSim->TimeSlice();
		m_lCycleStart = m_lBurstStart;

		*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
		m_fltActiveCurrent = GetCurrentOn();
		*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
	}
	else if(m_iType == AL_TONIC_CURRENT && m_lpCurrentOnEval)
	{
		*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
		m_fltActiveCurrent = GetCurrentOn();
		*m_lpExternalCurrent = *m_lpExternalCurrent + m_fltActiveCurrent;
	}

}

void CurrentStimulus::Deactivate()
{		
	ExternalStimulus::Deactivate();
	*m_lpExternalCurrent = *m_lpExternalCurrent - m_fltActiveCurrent;
}

void CurrentStimulus::ResetSimulation()
{
	ExternalStimulus::ResetSimulation();

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

	Type(oXml.GetChildString("CurrentType", "TONIC"));
	TargetNodeID(oXml.GetChildString("TargetNodeID"));

	CycleOnDuration(oXml.GetChildFloat("CycleOnDuration", m_fltCycleOnDuration));
	CycleOffDuration(oXml.GetChildFloat("CycleOffDuration", m_fltCycleOffDuration));
	BurstOnDuration(oXml.GetChildFloat("BurstOnDuration", m_fltBurstOnDuration));
	BurstOffDuration(oXml.GetChildFloat("BurstOffDuration", m_fltBurstOffDuration));

	CurrentOn(oXml.GetChildFloat("CurrentOn", m_fltCurrentOn));
	CurrentOff(oXml.GetChildFloat("CurrentOff", m_fltCurrentOff));
	CurrentBurstOff(oXml.GetChildFloat("CurrentBurstOff", m_fltCurrentBurstOff));
	
	CurrentEquation(oXml.GetChildString("CurrentOnEquation", ""));

	oXml.OutOfElem(); //OutOf Simulus Element
}

	}			//ExternalStimuli
}				//VortexAnimatSim




