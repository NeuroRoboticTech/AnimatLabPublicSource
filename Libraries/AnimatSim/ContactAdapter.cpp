/**
\file	ContactAdapter.cpp

\brief	Implements the contact adapter class.
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
#include "ReceptiveField.h"
#include "ReceptiveFieldPair.h"
#include "ContactAdapter.h"
#include "Joint.h"
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
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Simulator.h"


namespace AnimatSim
{
	namespace Adapters
	{
/**
\brief	Default constructor.

\author	dcofer
\date	3/18/2011
**/
ContactAdapter::ContactAdapter()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	3/18/2011
**/
ContactAdapter::~ContactAdapter()
{

try
{
	m_aryFieldPairs.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ContactAdapter\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the GUID ID of the source RigidBody.

\author	dcofer
\date	3/18/2011

\return	GUID ID.
**/
string ContactAdapter::SourceBodyID() {return m_strSourceBodyID;};

/**
\brief	Sets the GUID ID of the source RigidBody.

\author	dcofer
\date	3/18/2011

\param	strID	GUID ID. 
**/
void ContactAdapter::SourceBodyID(string strID)
{
	if(Std_IsBlank(strID)) 
		THROW_ERROR(Al_Err_lBodyIDBlank, Al_Err_strBodyIDBlank);
	m_strSourceBodyID = strID;
}

string ContactAdapter::SourceModule()
{return "AnimatLab";}

string ContactAdapter::TargetModule()
{return m_strTargetModule;}

/**
\brief	Sets the target NeuralModule name.

\author	dcofer
\date	3/18/2011

\param	strModule	The new Target Neuralmodule. 
**/
void ContactAdapter::TargetModule(string strModule)
{
	if(Std_IsBlank(strModule)) 
		THROW_TEXT_ERROR(Al_Err_lModuleNameBlank, Al_Err_strModuleNameBlank, " Target Module");
	m_strTargetModule = strModule;
}

void ContactAdapter::Initialize()
{
	Adapter::Initialize();

	m_lpSourceNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strSourceBodyID));
	if(!m_lpSourceNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strSourceBodyID);

	m_lpSourceNode->AttachSourceAdapter(m_lpStructure, this);
	m_lpSim->AttachTargetAdapter(m_lpStructure, this);

	int iCount = m_aryFieldPairs.GetSize();
	ReceptiveFieldPair *lpPair=NULL;
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpPair = m_aryFieldPairs[iIndex];
		lpPair->Initialize();
	}
}

void ContactAdapter::StepSimulation()
{
	int iCount = m_aryFieldPairs.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryFieldPairs[iIndex]->StepSimulation();
}

void ContactAdapter::Load(CStdXml &oXml)
{
	Node::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	SourceBodyID(oXml.GetChildString("SourceBodyID"));
	TargetModule(oXml.GetChildString("TargetModule"));

	m_aryFieldPairs.RemoveAll();

	oXml.FindChildElement("FieldPairs");
	oXml.IntoElem(); //Into FieldPairs Element
	int iCount = oXml.NumberOfChildren();

	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		oXml.FindChildByIndex(iIndex);
		LoadFieldPair(oXml);
	}
	oXml.OutOfElem(); //OutOf FieldPairs Element

	oXml.OutOfElem(); //OutOf Adapter Element
}

/**
\brief	Loads a ReceptiveFieldPair object.

\author	dcofer
\date	3/18/2011

\param [in,out]	oXml	The xml that defines the ReceptiveFieldPair. 

\return	Pointer to the ReceptiveFieldPair.
\exception Throws an exception if there is a problem creating or loading the ReceptiveFieldPair.
**/
ReceptiveFieldPair *ContactAdapter::LoadFieldPair(CStdXml &oXml)
{
	ReceptiveFieldPair *lpPair = NULL;

try
{
	lpPair = new ReceptiveFieldPair();
	lpPair->SetSystemPointers(m_lpSim, m_lpStructure, NULL, m_lpNode, TRUE);
	lpPair->Load(oXml);
	m_aryFieldPairs.Add(lpPair);

	return lpPair;
}
catch(CStdErrorInfo oError)
{
	if(lpPair) delete lpPair;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpPair) delete lpPair;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

	}			//Adapters
}			//AnimatSim
