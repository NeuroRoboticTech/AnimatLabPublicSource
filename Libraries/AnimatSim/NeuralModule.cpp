/**
\file	NeuralModule.cpp

\brief	Implements the neural module class. 
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
	\namespace	AnimatSim::Behavior

	\brief	Contains objects related to neural networks.

	\details This contains all of the objects related to the nervous system
	and neural networks. 

	**/
	namespace Behavior
	{

/**
\fn	NeuralModule::NeuralModule()

\brief	Default constructor. 

\author	dcofer
\date	2/24/2011
**/
NeuralModule::NeuralModule()
{
	m_iTargetAdapterCount = 0;
	m_iTimeStepInterval = 0;
	m_fltTimeStep = -1;
	m_iTimeStepCount = 0;
	m_lpClassFactory = NULL;
	m_lpSim = NULL;
	m_lpOrganism = NULL;
}

/**
\fn	NeuralModule::~NeuralModule()

\brief	Destructor. 

\details This deletes the class factory when destroyed.

\author	dcofer
\date	2/24/2011
**/
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
{Std_TraceMsg(0, "Caught Error in desctructor of NeuralModule\r\n", "", -1, false, true);}
}

/**
\fn	IStdClassFactory *NeuralModule::ClassFactory()

\brief	Gets the class factory. 

\author	dcofer
\date	2/24/2011

\return	null if it fails, else. 
**/
IStdClassFactory *NeuralModule::ClassFactory() {return m_lpClassFactory;}

/**
\brief	Sets the class factory for this neural module.

\author	dcofer
\date	9/9/2011

\param [in,out]	lpFactory	The pointer to a factory.
**/
void NeuralModule::ClassFactory(IStdClassFactory *lpFactory) {m_lpClassFactory = lpFactory;}

/**
\fn	Simulator *NeuralModule::GetSimulator()

\brief	Gets the simulator. 

\author	dcofer
\date	2/24/2011

\return	returns the simulator. 
**/
Simulator *NeuralModule::GetSimulator() {return m_lpSim;}

/**
\fn	Organism *NeuralModule::GetOrganism()

\brief	Gets the organism. 

\author	dcofer
\date	2/24/2011

\return	returns the organism. 
**/
Organism *NeuralModule::GetOrganism() {return m_lpOrganism;}

/**
\fn	short NeuralModule::TimeStepInterval()

\brief	Gets the time step interval. 

\details The m_iTimeStepInterval is the number of time slices between
that this module must wait before stepping again.

\author	dcofer
\date	2/24/2011

\return	. 
**/
short NeuralModule::TimeStepInterval()
{return m_iTimeStepInterval;}

/**
\fn	void NeuralModule::TimeStepInterval(short iVal)

\brief	Sets ime step interval. 

\author	dcofer
\date	2/24/2011

\param	iVal	New time step interval. 
\exception new value must be greater than zero.
**/

void NeuralModule::TimeStepInterval(short iVal)
{
	if(iVal == 0) iVal = 1;

	Std_IsAboveMin((int) 0, (int) iVal, true, "TimeStepInterval");
	m_iTimeStepInterval = iVal;
}

/**
\brief	Gets the time step for this moudle in time units.

\author	dcofer
\date	3/18/2011

\return	Time units for this modules time step.
**/
float NeuralModule::TimeStep()
{return m_fltTimeStep;}

/**
\brief	Sets the Time step for this moudle in time units.

\details This method calculates the required m_iTimeStepInterval.

\author	dcofer
\date	3/18/2011

\param	fltVal	The flt value. 
**/
void NeuralModule::TimeStep(float fltVal)
{
	Std_IsAboveMin((float) 0, (float) fltVal, true, "TimeStep");

	//Set it so that it will be taken into consideration when finding min value.
	m_fltTimeStep = fltVal;

	//Find min time step.
	float fltMin = m_lpSim->MinTimeStep();

	//Division
	int iDiv = (int) ((fltVal / fltMin) + 0.5f);

	//Find the number of timeslices that need to occur before this module is updated
	TimeStepInterval(iDiv);

	//Now recaculate the time step using the minimum time step as the base.
	m_fltTimeStep = m_lpSim->TimeStep() * m_iTimeStepInterval;

	//Now reset the m_fltTimeStep of the sim.
	if(m_iTimeStepInterval == 1) fltMin = m_lpSim->MinTimeStep();
}

void NeuralModule::Initialize()
{
	//We need to rerun the code to set the  time step here in initialize. The reason is that we set this when 
	//loading the simulator and neural modules, but if one of the neural modules has the miniumum time step then
	//we need to recalculate the time slice per step for all modules in initialize after everything has loaded.
	// Once everything is loaded and initialized, then if a given time step is changed then that one is changed in
	// the sim, and events will change it for the rest of them afterwards, so the values should be correct. 
	TimeStep(m_fltTimeStep);
}

/**
\brief	Tells whether this NeuralModule needs to call StepSimulation.

\details This is determined by the m_iTimeStepInterval. We only step on some whole number interval of the physics time step.

\author	dcofer
\date	3/18/2011

\return	true if it succeeds, false if it fails.
**/
bool NeuralModule::NeedToStep(bool bIncrement)
{
	if(bIncrement)
		m_iTimeStepCount++;

	if(m_iTimeStepInterval == m_iTimeStepCount)
		return true;
	else
		return false;
}

void NeuralModule::ResetStepCounter()
{
	m_iTimeStepCount = 0;
}

int NeuralModule::FindAdapterListIndex(CStdArray<Adapter *> aryAdapters, string strID, bool bThrowError)
{
	int iCount = aryAdapters.GetSize();
	for(int iIdx=0; iIdx<iCount; iIdx++)
		if(aryAdapters[iIdx]->ID() == strID)
			return iIdx;

	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lAdapterIDNotFound, Al_Err_strAdapterIDNotFound, "ID", strID);

	return -1;
}

void NeuralModule::AttachSourceAdapter(Adapter *lpAdapter)
{
	if(FindAdapterListIndex(m_arySourceAdapters, lpAdapter->ID(), false) == -1)
		m_arySourceAdapters.Add(lpAdapter);
}

void NeuralModule::RemoveSourceAdapter(Adapter *lpAdapter)
{
	int iIdx = FindAdapterListIndex(m_arySourceAdapters, lpAdapter->ID(), false);
	if(iIdx > -1)
		m_arySourceAdapters.RemoveAt(iIdx);
}

void NeuralModule::AttachTargetAdapter(Adapter *lpAdapter)
{
	if(FindAdapterListIndex(m_aryTargetAdapters, lpAdapter->ID(), false) == -1)
	{
		m_aryTargetAdapters.Add(lpAdapter);
		m_iTargetAdapterCount = m_aryTargetAdapters.GetSize();
	}
}

void NeuralModule::RemoveTargetAdapter(Adapter *lpAdapter)
{
	int iIdx = FindAdapterListIndex(m_aryTargetAdapters, lpAdapter->ID(), false);
	if(iIdx > -1)
	{
		m_aryTargetAdapters.RemoveAt(iIdx);
		m_iTargetAdapterCount = m_aryTargetAdapters.GetSize();
	}
}

void NeuralModule::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, false);

	m_lpOrganism = dynamic_cast<Organism *>(lpStructure);

	if(bVerify) VerifySystemPointers();
}

void NeuralModule::VerifySystemPointers()
{
	AnimatBase::VerifySystemPointers();

	if(!m_lpStructure)
		THROW_PARAM_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined, "ConstraintLimit: ", m_strName);

	if(!m_lpOrganism) 
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");
}


float *NeuralModule::GetDataPointer(const string &strDataType)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "TIMESTEP")
		return &m_fltTimeStep;

	return AnimatBase::GetDataPointer(strDataType);
}

void NeuralModule::StepSimulation()
{
}

void NeuralModule::StepAdapters()
{
	for(int iIndex=0; iIndex<m_iTargetAdapterCount; iIndex++)
		if(m_aryTargetAdapters[iIndex]->Enabled())
			m_aryTargetAdapters[iIndex]->StepSimulation();
}

	}			//Behavior
}			//AnimatSim
