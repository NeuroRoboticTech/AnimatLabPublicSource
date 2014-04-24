/**
\file	ThreadProcessor.cpp

\brief	Implements the thread processing class. 
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
\fn	ThreadProcessor::ThreadProcessor()

\brief	Default constructor. 

\author	dcofer
\date	4/14/2014
**/
ThreadProcessor::ThreadProcessor()
{
}

/**
\fn	ThreadProcessor::~ThreadProcessor()

\brief	Destructor. 

\author	dcofer
\date	4/14/2014
**/
ThreadProcessor::~ThreadProcessor()
{

try
{
    m_aryModules.Clear();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ThreadProcessor\r\n", "", -1, false, true);}
}

int ThreadProcessor::FindModuleProcessorIndex(std::string strModuleName)
{
	int iSize = m_aryModules.GetSize();
	for(int iIndex=0; iIndex<iSize; iIndex++)
        if(m_aryModules[iIndex]->Name() == strModuleName)
            return iIndex;

    return -1;
}

void ThreadProcessor::CreateModuleProcessor(std::string strModuleName)
{
    if(FindModuleProcessorIndex(strModuleName) == -1)
    {
        ModuleThreadProcessor *lpModule = new ModuleThreadProcessor(strModuleName);
        m_aryModules.Add(lpModule);
    }
}

void ThreadProcessor::RemoveModuleProcessor(std::string strModuleName)
{
    int iIndex = FindModuleProcessorIndex(strModuleName);
    if(iIndex >= 0)
        m_aryModules.RemoveAt(iIndex);
}

void ThreadProcessor::StartThread()
{
}

void ThreadProcessor::EndThread()
{
}

void ThreadProcessor::StepSimulation()
{
	int iSize = m_aryModules.GetSize();
	for(int iIndex=0; iIndex<iSize; iIndex++)
		m_aryModules[iIndex]->StepSimulation();

	for(int iIndex=0; iIndex<iSize; iIndex++)
		m_aryModules[iIndex]->StepAdapters();
}

}			//AnimatSim
