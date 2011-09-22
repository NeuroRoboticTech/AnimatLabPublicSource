/**
\file	Node.cpp

\brief	Implements the node class. 
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
	m_bEnabledMem = TRUE;
	m_bEnabled = TRUE;
	m_fltEnabled = 0;
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
{Std_TraceMsg(0, "Caught Error in desctructor of Node\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Tells whether this node is enabled.

\details Some types of nodes can be enabled/disabled. For example, joints or muscles. This tells
what enabled state the node is in. This will not apply to every node object type. 

\author	dcofer
\date	2/24/2011

\return	true if it enabled, false if not. 
**/
BOOL Node::Enabled() {return m_bEnabled;}

/**
\brief	Enables the node.

\details Some types of nodes can be enabled/disabled. This sets the enabled state of the object. 

\author	dcofer
\date	2/24/2011

\param	bValue	true to enable. 
**/
void Node::Enabled(BOOL bValue) 
{
	m_bEnabled = bValue;
	m_fltEnabled = (float) m_bEnabled;
}

void Node::Kill(BOOL bState)
{
	if(bState)
	{
		m_bEnabledMem = m_bEnabled;
		Enabled(FALSE);
	}
	else
		Enabled(m_bEnabledMem);

}

/**
\brief	Updates any reporting data for this time step. 

\author	dcofer
\date	3/2/2011
**/
void Node::UpdateData()
{}

void Node::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify)
{
	AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, FALSE);

	m_lpOrganism = dynamic_cast<Organism *>(lpStructure);

	if(bVerify) VerifySystemPointers();
}

void Node::VerifySystemPointers()
{
	AnimatBase::VerifySystemPointers();

	if(!m_lpStructure)
		THROW_PARAM_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined, "Link: ", m_strID);
}

BOOL Node::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(AnimatBase::SetData(strType, strValue, FALSE))
		return true;

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

}			//AnimatSim
