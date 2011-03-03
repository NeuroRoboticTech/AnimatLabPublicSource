// ExternalInputStimulus.cpp: implementation of the ExternalInputStimulus class.
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

ExternalInputStimulus::ExternalInputStimulus()
{
	m_lpStructure = NULL;
	m_lpNode = NULL;
	m_lpEval = NULL;
	m_fltInput = 0;
}

ExternalInputStimulus::~ExternalInputStimulus()
{

try
{
	m_lpNode = NULL;
	if(m_lpEval) delete m_lpEval;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ExternalInputStimulus\r\n", "", -1, FALSE, TRUE);}
}

void ExternalInputStimulus::InputEquation(string strVal)
{
	//Initialize the postfix evaluator.
	if(m_lpEval) 
	{delete m_lpEval; m_lpEval = NULL;}

	m_lpEval = new CStdPostFixEval;

	m_lpEval->AddVariable("t");
	m_lpEval->AddVariable("x");
	m_lpEval->Equation(m_strInputEquation);
}

void ExternalInputStimulus::Initialize(Simulator *lpSim)
{
	if(!lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	m_lpStructure = dynamic_cast<Structure *>(lpSim->FindByID(m_strStructureID));
	if(!m_lpStructure)
		THROW_PARAM_ERROR(Al_Err_lStructureNotFound, Al_Err_strStructureNotFound, "ID: ", m_strStructureID);

	m_lpNode = dynamic_cast<Node *>(lpSim->FindByID(m_strNodeID));
	if(!m_lpNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strNodeID);
}

void ExternalInputStimulus::Activate(Simulator *lpSim)
{
	ExternalStimulus::Activate(lpSim);
}

void ExternalInputStimulus::StepSimulation(Simulator *lpSim)
{
	try
	{
		m_lpEval->SetVariable("t", lpSim->Time());
		m_fltInput = m_lpEval->Solve();
		m_lpNode->AddExternalNodeInput(lpSim, m_lpStructure, m_fltInput);
	}
	catch(...)
	{
		LOG_ERROR("Error Occurred while setting Joint Velocity");
	}
}

void ExternalInputStimulus::Deactivate(Simulator *lpSim)
{
	ExternalStimulus::Deactivate(lpSim);
}

float *ExternalInputStimulus::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	if(strType == "INPUT")
		lpData = &m_fltInput;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "StimulusName: " + STR(m_strName) + "  DataType: " + strDataType);

	return lpData;
} 

BOOL ExternalInputStimulus::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "INPUTEQUATION")
	{
		InputEquation(strValue);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void ExternalInputStimulus::Load(CStdXml &oXml)
{
	ActivatedItem::Load(oXml);

	oXml.IntoElem();  //Into Simulus Element

	m_strStructureID = oXml.GetChildString("StructureID");
	if(Std_IsBlank(m_strStructureID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_strNodeID = oXml.GetChildString("NodeID");
	if(Std_IsBlank(m_strNodeID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	this->InputEquation(oXml.GetChildString("Input", "0"));

	oXml.OutOfElem(); //OutOf Simulus Element
}


	}			//ExternalStimuli
}				//VortexAnimatSim




