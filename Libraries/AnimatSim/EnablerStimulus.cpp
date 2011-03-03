// EnablerStimulus.cpp: implementation of the EnablerStimulus class.
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

EnablerStimulus::EnablerStimulus()
{
	m_bEnableWhenActive = TRUE;
}

EnablerStimulus::~EnablerStimulus()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of EnablerStimulus\r\n", "", -1, FALSE, TRUE);}
}

void EnablerStimulus::Initialize(Simulator *lpSim)
{
	if(!lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	ExternalStimulus::Initialize(lpSim);

	//Lets try and get the node we will dealing with.
	m_lpStructure = lpSim->FindStructureFromAll(m_strStructureID);

	m_lpNode = m_lpStructure->FindNode(m_strBodyID);
}

void EnablerStimulus::Activate(Simulator *lpSim)
{
	ExternalStimulus::Activate(lpSim);

	if(m_bEnableWhenActive)
		m_lpNode->Enabled(TRUE);
	else
		m_lpNode->Enabled(FALSE);
}

void EnablerStimulus::StepSimulation(Simulator *lpSim)
{
}

void EnablerStimulus::Deactivate(Simulator *lpSim)
{
	ExternalStimulus::Deactivate(lpSim);

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

	m_strStructureID = oXml.GetChildString("StructureID");
	if(Std_IsBlank(m_strStructureID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_strBodyID = oXml.GetChildString("BodyID");
	if(Std_IsBlank(m_strBodyID)) 
		THROW_ERROR(Al_Err_lBodyIDBlank, Al_Err_strBodyIDBlank);

	m_bEnableWhenActive = oXml.GetChildBool("EnableWhenActive", m_bEnableWhenActive);

	oXml.OutOfElem(); //OutOf Simulus Element

}

	}			//ExternalStimuli
}				//VortexAnimatSim




