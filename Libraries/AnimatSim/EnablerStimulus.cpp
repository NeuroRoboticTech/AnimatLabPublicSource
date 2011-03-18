/**
\file	EnablerStimulus.cpp

\brief	Implements the enabler stimulus class. 
**/

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
#include "EnablerStimulus.h"
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
\date	3/17/2011
**/
EnablerStimulus::EnablerStimulus()
{
	m_bEnableWhenActive = TRUE;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/17/2011
**/
EnablerStimulus::~EnablerStimulus()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of EnablerStimulus\r\n", "", -1, FALSE, TRUE);}
}

string EnablerStimulus::Type() {return "EnablerInput";}

/**
\brief	Gets the GUID ID of the target node that will be enabled. 

\author	dcofer
\date	3/17/2011

\return	GUID ID of the node. 
**/
string EnablerStimulus::TargetNodeID() {return m_strTargetNodeID;}

/**
\brief	Sets the GUID ID of the target node to enable. 

\author	dcofer
\date	3/17/2011

\param	strID	GUID ID. 
**/
void EnablerStimulus::TargetNodeID(string strID)
{
	if(Std_IsBlank(strID)) 
		THROW_ERROR(Al_Err_lBodyIDBlank, Al_Err_strBodyIDBlank);
	m_strTargetNodeID = strID;
}

/**
\brief	Tells if the node is enabled when active. This is used to control if we are enabling the
node during the active period, or disabling it.

\author	dcofer
\date	3/17/2011

\return	true if it enabled while active, false otherwise. 
**/
BOOL EnablerStimulus::EnableWhenActive() {return m_bEnableWhenActive;}

/**
\brief	Sets whether the node is enabled when active. This is used to control if we are enabling the
node during the active period, or disabling it.

\author	dcofer
\date	3/17/2011

\param	bVal	true to enable when active. 
**/
void EnablerStimulus::EnableWhenActive(BOOL bVal) {m_bEnableWhenActive = bVal;}

void EnablerStimulus::Initialize()
{
	ExternalStimulus::Initialize();

	m_lpNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strTargetNodeID));
}

void EnablerStimulus::Activate()
{
	ExternalStimulus::Activate();

	if(m_bEnableWhenActive)
		m_lpNode->Enabled(TRUE);
	else
		m_lpNode->Enabled(FALSE);
}

void EnablerStimulus::StepSimulation()
{
}

void EnablerStimulus::Deactivate()
{
	ExternalStimulus::Deactivate();

	if(m_bEnableWhenActive)
		m_lpNode->Enabled(FALSE);
	else
		m_lpNode->Enabled(TRUE);
}

BOOL EnablerStimulus::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "ENABLEWHENACTIVE")
	{
		EnableWhenActive(Std_ToBool(strValue));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void EnablerStimulus::Load(CStdXml &oXml)
{
	ActivatedItem::Load(oXml);

	oXml.IntoElem();  //Into Simulus Element

	TargetNodeID(oXml.GetChildString("BodyID"));
	EnableWhenActive(oXml.GetChildBool("EnableWhenActive", m_bEnableWhenActive));

	oXml.OutOfElem(); //OutOf Simulus Element

}

	}			//ExternalStimuli
}				//VortexAnimatSim




