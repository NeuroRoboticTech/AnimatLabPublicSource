/**
\file	ReceptiveFieldPair.cpp

\brief	Implements the receptive field pair class.
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Gain.h"
#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ReceptiveFieldPair.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Sensor.h"
#include "Attachment.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "Adapter.h"
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
	namespace Environment
	{
/**
\brief	Default constructor.

\author	dcofer
\date	3/24/2011
**/
ReceptiveFieldPair::ReceptiveFieldPair()
{
	m_lpNode = NULL;
	m_lpField = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/24/2011
**/
ReceptiveFieldPair::~ReceptiveFieldPair()
{
}

void ReceptiveFieldPair::FieldID(string strID)
{
	if(Std_IsBlank(strID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_strFieldID = strID;
}

string ReceptiveFieldPair::FieldID() {return m_strFieldID;}

void ReceptiveFieldPair::TargetNodeID(string strID)
{
	if(Std_IsBlank(strID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_strTargetNodeID = strID;
}

string ReceptiveFieldPair::TargetNodeID() {return m_strTargetNodeID;}

ReceptiveField *ReceptiveFieldPair::Field() {return m_lpField;}

void ReceptiveFieldPair::Initialize()
{
	AnimatBase::Initialize();

	m_lpNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strTargetNodeID));
	if(!m_lpNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetNodeID);

	m_lpField = dynamic_cast<ReceptiveField *>(m_lpSim->FindByID(m_strFieldID));
	if(!m_lpField)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strFieldID);
}

void ReceptiveFieldPair::StepSimulation()
{
	if(m_lpField && m_lpNode)
		m_lpNode->AddExternalNodeInput(m_lpField->m_fltCurrent);
}


void ReceptiveFieldPair::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	FieldID(oXml.GetChildString("FieldID"));
	TargetNodeID(oXml.GetChildString("TargetNodeID"));

	oXml.OutOfElem(); //OutOf Adapter Element
}

	}			//Environment
}				//AnimatSim