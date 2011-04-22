/**
\file	Link.cpp

\brief	Implements the link class. 
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
#include "Link.h"
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
\brief	Default constructor. 

\author	dcofer
\date	3/16/2011
**/
Link::Link()
{
	m_lpOrganism = NULL;

	m_bEnabled = TRUE;
	m_fltEnabled = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/16/2011
**/
Link::~Link()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Link\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets whether the link is enabled. 

\author	dcofer
\date	3/16/2011

\return	true if enabled, false if not. 
**/
BOOL Link::Enabled() {return m_bEnabled;};

/**
\brief	Sets whether the link is Enabled. 

\author	dcofer
\date	3/16/2011

\param	bValue	true to enable. 
**/
void Link::Enabled(BOOL bValue) 
{
	m_bEnabled = bValue;
	m_fltEnabled = (float) m_bEnabled;
}

/**
\brief	Called during the StepSimulation method to allow the link to update any internal data for reporting purposes. 

\author	dcofer
\date	3/16/2011
**/
void Link::UpdateData()
{}

void Link::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify)
{
	AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, FALSE);

	m_lpOrganism = dynamic_cast<Organism *>(lpStructure);

	if(bVerify) VerifySystemPointers();
}

void Link::VerifySystemPointers()
{
	AnimatBase::VerifySystemPointers();

	if(!m_lpStructure)
		THROW_PARAM_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined, "Link: ", m_strID);

	if(!m_lpOrganism) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Link: ", m_strID);

	if(!m_lpModule) 
		THROW_PARAM_ERROR(Al_Err_lNeuralModuleNotDefined, Al_Err_strNeuralModuleNotDefined, "Link: ", m_strID);

	if(!m_lpNode) 
		THROW_PARAM_ERROR(Al_Err_lNodeNotDefined, Al_Err_strNodeNotDefined, "Link: ", m_strID);
}

}			//AnimatSim
