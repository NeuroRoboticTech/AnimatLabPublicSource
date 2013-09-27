/**
\file	ContactAdapter.cpp

\brief	Implements the contact adapter class.
**/

#include "StdAfx.h"
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
#include "Light.h"
#include "LightManager.h"
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
{Std_TraceMsg(0, "Caught Error in desctructor of ContactAdapter\r\n", "", -1, false, true);}
}

/**
\brief	Gets the GUID ID of the source RigidBody.

\author	dcofer
\date	3/18/2011

\return	GUID ID.
**/
std::string ContactAdapter::SourceBodyID() {return m_strSourceBodyID;};

/**
\brief	Sets the GUID ID of the source RigidBody.

\author	dcofer
\date	3/18/2011

\param	strID	GUID ID. 
**/
void ContactAdapter::SourceBodyID(std::string strID)
{
	if(Std_IsBlank(strID)) 
		THROW_ERROR(Al_Err_lBodyIDBlank, Al_Err_strBodyIDBlank);
	m_strSourceBodyID = strID;
}

std::string ContactAdapter::SourceModule()
{return "AnimatLab";}

std::string ContactAdapter::TargetModule()
{return m_strTargetModule;}

/**
\brief	Sets the target NeuralModule name.

\author	dcofer
\date	3/18/2011

\param	strModule	The new Target Neuralmodule. 
**/
void ContactAdapter::TargetModule(std::string strModule)
{
	if(Std_IsBlank(strModule)) 
		THROW_TEXT_ERROR(Al_Err_lModuleNameBlank, Al_Err_strModuleNameBlank, " Target Module");
	m_strTargetModule = strModule;
}


void ContactAdapter::AddFieldPair(std::string strXml, bool bDoNotInit)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("FieldPair");

	ReceptiveFieldPair *lpPair = LoadFieldPair(oXml);
	if(!bDoNotInit)
		lpPair->Initialize();
}

void ContactAdapter::RemoveFieldPair(std::string strID, bool bThrowError)
{
	int iPos = FindFieldPairListPos(strID, bThrowError);
	m_aryFieldPairs.RemoveAt(iPos);
}

int ContactAdapter::FindFieldPairListPos(std::string strID, bool bThrowError)
{
	std::string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryFieldPairs.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryFieldPairs[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Al_Err_lReceptiveFieldIDNotFound, Al_Err_strReceptiveFieldIDNotFound, "ID");

	return -1;
}

void ContactAdapter::Initialize()
{
	Node::Initialize();

	m_lpSourceNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strSourceBodyID));
	if(!m_lpSourceNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strSourceBodyID);

	m_lpSim->AttachSourceAdapter(m_lpStructure, this);
	m_lpSim->AttachTargetAdapter(m_lpStructure, this);

	int iCount = m_aryFieldPairs.GetSize();
	ReceptiveFieldPair *lpPair=NULL;
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpPair = m_aryFieldPairs[iIndex];
		lpPair->Initialize();
	}
}

bool ContactAdapter::AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError, bool bDoNotInit)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "FIELDPAIR")
	{
		AddFieldPair(strXml, bDoNotInit);
		return true;
	}
	
	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

bool ContactAdapter::RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "FIELDPAIR")
	{
		RemoveFieldPair(strID);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
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

	if(oXml.FindChildElement("FieldPairs", false))
	{
		oXml.IntoElem(); //Into FieldPairs Element
		int iCount = oXml.NumberOfChildren();

		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadFieldPair(oXml);
		}
		oXml.OutOfElem(); //OutOf FieldPairs Element
	}

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
	lpPair->SetSystemPointers(m_lpSim, m_lpStructure, NULL, m_lpNode, true);
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
