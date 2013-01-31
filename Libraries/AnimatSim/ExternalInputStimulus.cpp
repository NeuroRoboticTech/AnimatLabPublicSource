/**
\file	ExternalInputStimulus.cpp

\brief	Implements the external input stimulus class. 
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
#include "Light.h"
#include "LightManager.h"
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
ExternalInputStimulus::ExternalInputStimulus()
{
	m_lpNode = NULL;
	m_lpEval = NULL;
	m_fltInput = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/17/2011
**/
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

string ExternalInputStimulus::Type() {return "ExternalInput";}

/**
\brief	Gets the GUID ID of the target node that will be stimulated. 

\author	dcofer
\date	3/17/2011

\return	GUID ID of the node. 
**/
string ExternalInputStimulus::TargetNodeID() {return m_strTargetNodeID;}

/**
\brief	Sets the GUID ID of the target node to stimulate. 

\author	dcofer
\date	3/17/2011

\param	strID	GUID ID. 
**/
void ExternalInputStimulus::TargetNodeID(string strID)
{
	if(Std_IsBlank(strID)) 
		THROW_ERROR(Al_Err_lBodyIDBlank, Al_Err_strBodyIDBlank);
	m_strTargetNodeID = strID;
}

/**
\brief	Gets the current input value. 

\author	dcofer
\date	3/17/2011

\return	Current input value. 
**/
float ExternalInputStimulus::Input() {return m_fltInput;}

/**
\brief	Sets the current input value. 

\author	dcofer
\date	3/17/2011

\param	fltVal	The new value. 
**/
void ExternalInputStimulus::Input(float fltVal) {m_fltInput = fltVal;}

/**
\brief	Gets the post-fix input equation used for the stimulus. 

\author	dcofer
\date	3/17/2011

\return	Post-fix input equation string. 
**/
string ExternalInputStimulus::InputEquation() {return m_strInputEquation;}

/**
\brief	Sets the post-fix input equation string. 

\author	dcofer
\date	3/17/2011

\param	strVal	The new equation string. 
**/
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

void ExternalInputStimulus::Initialize()
{
	ExternalStimulus::Initialize();

	m_lpNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strTargetNodeID));
	if(!m_lpNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetNodeID);
}

void ExternalInputStimulus::Activate()
{
	ExternalStimulus::Activate();
}

void ExternalInputStimulus::StepSimulation()
{
	try
	{
		m_lpEval->SetVariable("t", m_lpSim->Time());
		m_fltInput = m_lpEval->Solve();
		m_lpNode->AddExternalNodeInput(m_fltInput);
	}
	catch(...)
	{
		LOG_ERROR("Error Occurred while setting Joint Velocity");
	}
}

void ExternalInputStimulus::Deactivate()
{
	ExternalStimulus::Deactivate();
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
	
	if(ExternalStimulus::SetData(strDataType, strValue, FALSE))
		return TRUE;

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

void ExternalInputStimulus::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	ExternalStimulus::QueryProperties(aryNames, aryTypes);

	aryNames.Add("InputEquation");
	aryTypes.Add("String");
}

void ExternalInputStimulus::Load(CStdXml &oXml)
{
	ActivatedItem::Load(oXml);

	oXml.IntoElem();  //Into Simulus Element

	TargetNodeID(oXml.GetChildString("NodeID"));
	InputEquation(oXml.GetChildString("Input", "0"));

	oXml.OutOfElem(); //OutOf Simulus Element
}


	}			//ExternalStimuli
}				//VortexAnimatSim




