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
	m_lpSim = NULL;
	m_lpStructure = NULL;
	m_lpModule = NULL;

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
\fn	Simulator *Node::GetSimulator()

\brief	Gets the simulator pointer.

\author	dcofer
\date	2/24/2011

\return	pointer the Simulator object for this simulation. 
**/
Simulator *Node::GetSimulator() {return m_lpSim;}

/**
\fn	Structure *Node::GetStructure()

\brief	Gets the structure for this node. 

\author	dcofer
\date	2/24/2011

\return	returns the Structure pointer for this node. 
**/
Structure *Node::GetStructure() {return m_lpStructure;}

/**
\fn	NeuralModule *Node::GetNeuralModule()

\brief	Gets the neural module. 

\author	dcofer
\date	2/24/2011

\return	Returns the NeuralModule pointer associated with this node.
This only applies to neural network nodes. All others will return NULL.
**/
NeuralModule *Node::GetNeuralModule() {return m_lpModule;}

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
\fn	void Node::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule)

\brief	Sets a system pointers for this node. 

\details Sometimes it is necessary to set the system pointers before Initialize is called. (eg. in load). 
This allows us to do that.

\author	dcofer
\date	2/24/2011

\param [in,out]	lpSim		If non-null, the pointer to a simulation. 
\param [in,out]	lpStructure	If non-null, the pointer to a structure. 
\param [in,out]	lpModule	If non-null, the pointer to a module. 
**/

void Node::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule)
{
	m_lpSim = lpSim;
	m_lpStructure = lpStructure;
	m_lpModule = lpModule;
}

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

/**
\fn	void Node::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)

\brief	Loads the item using an XML data packet.

\details This method is responsible for loading the structure from a XMl configuration file. You
should call this method even in your overriden function becuase it loads all of the base
properties for this object like ID and Name. It also includes this object in the simulators
AddToObjectList so that the simulator knows about this object when you do a FindObject call. If
you do not call this base method then it is up to you to add your item to the simulators list of
objects. 

\author	dcofer
\date	3/1/2011

\param [in,out]	lpSim		The pointer to a simulation. 
\param [in,out]	lpStructure	The pointer to this parts parent structure. 
\param [in,out]	oXml		The CStdXml xml data packet to load. 
**/
void Node::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	AnimatBase::Load(oXml);
}


}			//AnimatSim
