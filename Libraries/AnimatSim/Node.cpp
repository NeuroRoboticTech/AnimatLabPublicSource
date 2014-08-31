/**
\file	Node.cpp

\brief	Implements the node class. 
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
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

namespace AnimatSim
{
/**
\brief	Default constructor. 

\author	dcofer
\date	2/24/2011
**/
Node::Node()
{
	m_lpOrganism = NULL;	
	m_bEnabled = true;
	m_bInitEnabled = m_bEnabled;
	m_fltEnabled = 0;
	m_bTemplateNode = false;
	m_iTemplateNodeCount = 1;
}

/**
\brief	Destructor. 

\author	dcofer
\date	2/24/2011
**/
Node::~Node()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Node\r\n", "", -1, false, true);}
}

/**
\brief	Tells whether this node is enabled.

\details Some types of nodes can be enabled/disabled. For example, joints or muscles. This tells
what enabled state the node is in. This will not apply to every node object type. 

\author	dcofer
\date	2/24/2011

\return	true if it enabled, false if not. 
**/
bool Node::Enabled() {return m_bEnabled;}

/**
\brief	Enables the node.

\details Some types of nodes can be enabled/disabled. This sets the enabled state of the object. 

\author	dcofer
\date	2/24/2011

\param	bValue	true to enable. 
**/
void Node::Enabled(bool bValue) 
{
	m_bEnabled = bValue;
	m_fltEnabled = (float) m_bEnabled;

	//If the sim is running then we do not set the history flag. Only set it if changed while the sim is not running.
	if(!m_lpSim->SimRunning())
		m_bInitEnabled = m_bEnabled;
}

void Node::ResetSimulation()
{
	AnimatBase::ResetSimulation();

	//Reset the enabled state to the value that it had before the sim started.
	//It is possible that stimuli turned off the enabled state during the simulation.
	Enabled(m_bInitEnabled);
}

void Node::Kill(bool bState)
{
	if(bState)
		Enabled(false);
	else
		Enabled(m_bInitEnabled);
}


bool Node::TemplateNode() {return m_bTemplateNode;}

void Node::TemplateNode(bool bVal) 
{
	m_bTemplateNode = bVal;

	if(bVal)
		SetupTemplateNodes();
	else
		DestroyTemplateNodes();
}

int Node::TemplateNodeCount() {return m_iTemplateNodeCount;}

void Node::TemplateNodeCount(int iVal)
{
	Std_IsAboveMin((int) 1, iVal, true, "TemplateNodeCount", true);
	m_iTemplateNodeCount = iVal;

	SetupTemplateNodes();
}

std::string Node::TemplateChangeScript() {return m_strTemplateChangeScript;}

void Node::TemplateChangeScript(std::string strVal)
{
	m_strTemplateChangeScript = strVal;

	TemplateNodeChanged();
}

void Node::Copy(CStdSerialize *lpSource)
{
	AnimatBase::Copy(lpSource);

	Node *lpOrig = dynamic_cast<Node *>(lpSource);

	m_lpOrganism = lpOrig->m_lpOrganism;
	m_bInitEnabled = lpOrig->m_bInitEnabled;
	m_fltEnabled = lpOrig->m_fltEnabled;
	m_bTemplateNode = false;
	m_iTemplateNodeCount = 1;
	m_strTemplateChangeScript = "";
	m_aryTemplateChildNodes.RemoveAll();
}

/**
\brief	Updates any reporting data for this time step. 

\author	dcofer
\date	3/2/2011
**/
void Node::UpdateData()
{}

/**
\brief	Creates and initializes all of the nodes that are baesd on this template node. 

\author	dcofer
\date	7/25/2014
**/
void Node::SetupTemplateNodes()
{}

/**
\brief	Destroys all of the nodes that are baesd on this template node. 

\author	dcofer
\date	7/25/2014
**/
void Node::DestroyTemplateNodes()
{}

/**
\brief	Called anytime that a key param of this node is modified. 

\author	dcofer
\date	7/25/2014
**/
void Node::TemplateNodeChanged()
{}


/**
\brief	Used to convert a string target data type into an integer index.

\details We do not want to be doing any string comparisons within the main simulation loop.
To avoid this we need to convert the target data type into an index to use when AddExternalNodeInput
is called so it knows to which input we are adding.

\author	dcofer
\date	6/16/2014

\param	strDataType	String descriptor of the target data we want. 

\return	index. Zero is the default. 
**/
int Node::GetTargetDataTypeIndex(const std::string &strDataType)
{
	return 0;
}

void Node::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, false);

	m_lpOrganism = dynamic_cast<Organism *>(lpStructure);

	if(bVerify) VerifySystemPointers();
}

void Node::VerifySystemPointers()
{
	AnimatBase::VerifySystemPointers();

	if(!m_lpStructure)
		THROW_PARAM_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined, "Link: ", m_strID);
}

bool Node::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, false))
		return true;

	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return true;
	}
	else if(strType == "TEMPLATENODE")
	{
		TemplateNode(Std_ToBool(strValue));
		return true;
	}
	else if(strType == "TEMPLATENODECOUNT")
	{
		TemplateNodeCount(atoi(strValue.c_str()));
		return true;
	}
	else if(strType == "TEMPLATECHANGESCRIPT")
	{
		TemplateChangeScript(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Node::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatBase::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Enabled", AnimatPropertyType::Boolean, AnimatPropertyDirection::Both));
	aryProperties.Add(new TypeProperty("TemplateNode", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("TemplateNodeCount", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("TemplateChangeScript", AnimatPropertyType::String, AnimatPropertyDirection::Set));
}

}			//AnimatSim
