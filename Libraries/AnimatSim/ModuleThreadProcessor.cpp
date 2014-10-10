/**
\file	ModuleThreadProcessor.cpp

\brief	Implements the thread processing class for a specific module. 
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
\fn	ModuleThreadProcessor::ModuleThreadProcessor()

\brief	Default constructor. 

\author	dcofer
\date	4/14/2014
**/
ModuleThreadProcessor::ModuleThreadProcessor(std::string strModuleName)
{
    m_strName = strModuleName;
}

/**
\fn	ModuleThreadProcessor::~ModuleThreadProcessor()

\brief	Destructor. 

\author	dcofer
\date	4/14/2014
**/
ModuleThreadProcessor::~ModuleThreadProcessor()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ModuleThreadProcessor\r\n", "", -1, false, true);}
}

void ModuleThreadProcessor::StepSimulation()
{}

void ModuleThreadProcessor::StepAdapters()
{
}

}			//AnimatSim
