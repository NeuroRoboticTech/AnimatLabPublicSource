/**
\file	Node.cpp

\brief	Implements the node class. 
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
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Simulator.h"

namespace AnimatSim
{

/**
\fn	Node::Node()

\brief	Default constructor. 

\author	dcofer
\date	2/24/2011
**/
Node::Node()
{
	m_bEnabledMem = TRUE;
	m_bEnabled = TRUE;
	m_fltEnabled = 0;
}

/**
\fn	Node::~Node()

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
\fn	BOOL Node::Enabled()

\brief	Tells whether this node is enabled.

\details Some types of nodes can be enabled/disabled. For example, joints or muscles. 
This tells what enabled state the node is in. This will not apply to every node object type.

\author	dcofer
\date	2/24/2011

\return	true if it enabled, false if not. 
**/
BOOL Node::Enabled() {return m_bEnabled;}

/**
\fn	void Node::Enabled(BOOL bValue)

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

/**
\fn	void Node::AttachSourceAdapter(Simulator *lpSim, Structure *lpStructure, Adapter *lpAdapter)

\brief	Attach this node to a source adapter. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpSim		The pointer to a simulation. 
\param [in,out]	lpStructure	The pointer to a structure. 
\param [in,out]	lpAdapter	The pointer to an adapter. 
**/
void Node::AttachSourceAdapter(Simulator *lpSim, Structure *lpStructure, Adapter *lpAdapter)
{
	lpSim->AttachSourceAdapter(lpStructure, lpAdapter);
}

/**
\fn	void Node::AttachTargetAdapter(Simulator *lpSim, Structure *lpStructure, Adapter *lpAdapter)

\brief	Attach this node to a target adapter. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpSim		The pointer to a simulation. 
\param [in,out]	lpStructure	The pointer to a structure. 
\param [in,out]	lpAdapter	The pointer to an adapter. 
**/
void Node::AttachTargetAdapter(Simulator *lpSim, Structure *lpStructure, Adapter *lpAdapter)
{
	lpSim->AttachTargetAdapter(lpStructure, lpAdapter);
}

/**
\fn	void Node::Kill(Simulator *lpSim, Organism *lpOrganism, BOOL bState)

\brief	Called when this organism is killed.

\details When an organism dies this method is called. The node is disabled. This means that
if the node is a joint then it is disabled (free moving), and if it is a neuron it will no 
longer function.The organism is dead.

\author	dcofer
\date	2/24/2011

\param [in,out]	lpSim		The pointer to a simulation. 
\param [in,out]	lpOrganism	The pointer to an organism. 
\param	bState				True to kill it, false to resurrect it. 
**/
void Node::Kill(Simulator *lpSim, Organism *lpOrganism, BOOL bState)
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
\fn	void Node::UpdateData(Simulator *lpSim, Structure *lpStructure)

\brief	Updates any reporting data for this time step. 

\author	dcofer
\date	3/2/2011

\param [in,out]	lpSim		The pointer to a simulation. 
\param [in,out]	lpStructure	The pointer to a structure. 
**/
void Node::UpdateData(Simulator *lpSim, Structure *lpStructure)
{}

/**
\fn	void Node::Initialize(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule)

\brief	Initializes this node. 

\author	dcofer
\date	2/24/2011

\param [in,out]	lpSim		The pointer to a simulation. 
\param [in,out]	lpStructure	The pointer to this parts parent structure. 
\param [in,out]	lpModule	The pointer to this parts parent module. 
**/
void Node::Initialize(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule)
{
	m_lpSim = lpSim;
	m_lpStructure = lpStructure;
	m_lpModule = lpModule;
}


}			//AnimatSim
