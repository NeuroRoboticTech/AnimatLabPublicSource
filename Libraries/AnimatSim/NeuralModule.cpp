// NeuralModule.cpp: implementation of the NeuralModule class.
//
//////////////////////////////////////////////////////////////////////

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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
namespace AnimatSim
{
	namespace Behavior
	{

/*! \brief 
   Constructs an structure object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a structure. 
*/

NeuralModule::NeuralModule()
{
	m_iTargetAdapterCount = 0;
	m_iTimeStepInterval = 0;
	m_fltTimeStep = 0;
	m_iTimeStepCount = 0;
	m_lpClassFactory = NULL;
	m_lpSim = NULL;
	m_lpOrganism = NULL;
}


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

NeuralModule::~NeuralModule()
{

try
{
	if(m_lpClassFactory)
		{delete m_lpClassFactory; m_lpClassFactory = NULL;}

	m_arySourceAdapters.RemoveAll();
	m_aryTargetAdapters.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of NeuralModule\r\n", "", -1, FALSE, TRUE);}
}

short NeuralModule::TimeStepInterval()
{return m_iTimeStepInterval;}

void NeuralModule::TimeStepInterval(short iVal)
{
	Std_IsAboveMin((int) 0, (int) iVal, TRUE, "TimeStepInterval");
	m_iTimeStepInterval = iVal;
}

float NeuralModule::TimeStep()
{return m_fltTimeStep;}

void NeuralModule::TimeStep(float fltVal)
{
	Std_IsAboveMin((float) 0, (float) fltVal, TRUE, "TimeStep");
	m_fltTimeStep = fltVal;
}

string NeuralModule::ProjectPath()
{return m_strProjectPath;}

void NeuralModule::ProjectPath(string strPath)
{m_strProjectPath = strPath;}

string NeuralModule::NeuralNetworkFile()
{return m_strNeuralNetworkFile;}

void NeuralModule::NeuralNetworkFile(string strFile)
{m_strNeuralNetworkFile = strFile;}

BOOL NeuralModule::NeedToStep()
{
	m_iTimeStepCount++;

	if(m_iTimeStepInterval == m_iTimeStepCount)
	{
		m_iTimeStepCount = 0;
		return TRUE;
	}
	else
		return FALSE;
}


void NeuralModule::AttachSourceAdapter(Adapter *lpAdapter)
{
	m_arySourceAdapters.Add(lpAdapter);
}

void NeuralModule::AttachTargetAdapter(Adapter *lpAdapter)
{
	m_aryTargetAdapters.Add(lpAdapter);
	m_iTargetAdapterCount = m_aryTargetAdapters.GetSize();
}

void NeuralModule::Initialize(Simulator *lpSim, Structure *lpStructure)
{
	m_lpSim = lpSim;
	m_lpOrganism = dynamic_cast<Organism *>(lpStructure);
	if(!m_lpOrganism) 
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

	//Find the number of timeslices that need to occur before this module is updated
	m_iTimeStepInterval = m_fltTimeStep / lpSim->TimeStep();

	//Now recaculate the time step using the minimum time step as the base.
	m_fltTimeStep = lpSim->TimeStep() * m_iTimeStepInterval;
}

void NeuralModule::SetSystemPointers(Simulator *lpSim, Structure *lpStructure)
{
	m_lpSim = lpSim;
	m_lpOrganism = dynamic_cast<Organism *>(lpStructure);
	if(!m_lpOrganism) 
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");
}

void NeuralModule::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	for(int iIndex=0; iIndex<m_iTargetAdapterCount; iIndex++)
		m_aryTargetAdapters[iIndex]->StepSimulation(lpSim, lpStructure);
}

	}			//Behavior
}			//AnimatSim
