/**
\file	ThreadedModule.cpp

\brief	Implements the base class for threaded neural module.
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "IMotorizedJoint.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
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

#include "ThreadedModule.h"

namespace AnimatSim
{
	namespace Behavior
	{

/**
\brief	Default constructor.

\author	dcofer
\date	9/8/2014
**/
ThreadedModule::ThreadedModule(void)
{
	m_bSetupStarted = false;
	m_bSetupComplete	= false;	// flag so we setup when its ready, you don't need to touch this :)
	m_bStopThread = false;
	m_bThreadProcessing = false;
	m_fltStepThreadDuration = 0;
	m_bPauseThread = false;
	m_bThreadPaused = false;
	m_bWaitingForThreadNotify = false;
}


/**
\brief	Destructor.

\author	dcofer
\date	9/8/2014
**/
ThreadedModule::~ThreadedModule(void)
{
try
{
	ShutdownThread();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ThreadedModule\r\n", "", -1, false, true);}
}

void ThreadedModule::PauseThread(bool bVal)
{
	m_bPauseThread = bVal;
	m_bThreadPaused = false;
}

bool ThreadedModule::PauseThread() {return m_bPauseThread;}

bool ThreadedModule::ThreadPaused() {return m_bThreadPaused;}

/**
\brief	Gets the time duration required to perform one step of the thread for all parts in this control.

\author	dcofer
\date	5/7/2014

\return	Step duration.
**/
float ThreadedModule::StepThreadDuration() {return m_fltStepThreadDuration;}

#pragma region DataAccesMethods

float *ThreadedModule::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "STEPTHREADDURATION")
		return &m_fltStepThreadDuration;
	else
		return NeuralModule::GetDataPointer(strDataType);

	return NULL;
}

void ThreadedModule::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	NeuralModule::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("StepThreadDuration", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
}

#pragma endregion

void ThreadedModule::StartThread()
{
	int iWaitTime = 30;
#ifdef _DEBUG
	iWaitTime = 200;
#endif

	//Reset key variables.
	m_bSetupStarted = false;
	m_bSetupComplete	= false;
	m_bStopThread = false;
	m_bThreadProcessing = false;
	m_fltStepThreadDuration = 0;
	m_bPauseThread = false;
	m_bThreadPaused = false;
	m_bWaitingForThreadNotify = false;

	boost::posix_time::ptime pt = boost::posix_time::microsec_clock::universal_time() +  boost::posix_time::seconds(iWaitTime);

	boost::interprocess::scoped_lock<boost::interprocess::interprocess_mutex> lock(m_WaitForSetupMutex);

	m_bWaitingForThreadNotify = false;
	m_Thread = boost::thread(&ThreadedModule::ProcessThread, this);

	std::cout << "Waiting for thread return\r\n";
	m_bWaitingForThreadNotify = true;
	bool bWaitRet = m_WaitForSetupCond.timed_wait(lock, pt);

	if(!bWaitRet)
	{
		std::cout << "Thread Timed out\r\n";
		ShutdownThread();
		THROW_ERROR(Al_Err_lErrorSettingUpIOThread, Al_Err_strErrorSettingUpIOThread);
	}

	std::cout << "Thread returned\r\n";
}

void ThreadedModule::WaitForThreadNotifyReady()
{
	//Wait to fire the signal until we get notification that it is waiting.
	while(!m_bWaitingForThreadNotify)
		boost::this_thread::sleep(boost::posix_time::microseconds(500));
}

void ThreadedModule::ProcessThread()
{
	try
	{
		m_bThreadProcessing = true;

		SetupThread();

		m_bSetupComplete = true;

		WaitForThreadNotifyReady();

		//Notify it back that we are ready.
		m_WaitForSetupCond.notify_all();

		while(!m_bStopThread)
		{
			if(m_bPauseThread || m_lpSim->Paused())
			{
				m_bThreadPaused = true;
				boost::this_thread::sleep(boost::posix_time::microseconds(1000));
			}
			else
			{
				m_bThreadPaused = false;
				StepThread();
			}
		}
	}
	catch(CStdErrorInfo oError)
	{
		m_bThreadProcessing = false;
	}
	catch(...)
	{
		m_bThreadProcessing = false;
	}

	m_bThreadProcessing = false;
}

void ThreadedModule::ExitThread()
{
	TRACE_DEBUG("ExitThread.");

	if(m_bThreadProcessing)
	{
		//Allow thread close calls.
		CloseThread();

		//Tell the thread to shutdown
		m_bStopThread = true;

		TRACE_DEBUG("Joint Thread.\r\n");

	bool bTryJoin = false;
#if (BOOST_VERSION >= 105000)
	bTryJoin = m_Thread.try_join_for(boost::chrono::seconds(30));
#else
	m_ioThread.join();
#endif
	}
}

/**
\brief	This method is called after all connections to whatever control board have been made. It calls
each parts SetupIO method. For example, We connect to a Firmata
microcontroller like an Arduino, and then do a setup that could take some time. We should not attempt to
setup any of the pins until after the board itself has been setup. After that we need to loop through and setup
all the parts.

\author	dcofer
\date	5/1/2014

**/
void ThreadedModule::SetupThread()
{
}

void ThreadedModule::CloseThread()
{
}

/**
\brief	This method is called from within the IO thread. It calls StepIO for each part.

\author	dcofer
\date	5/2/2014

**/
void ThreadedModule::StepThread()
{
}

/**
\brief	This method is waits until the m_bPauseIO flag is set back to false.

\author	dcofer
\date	5/12/2014

**/
void ThreadedModule::WaitWhilePaused()
{
	m_bThreadPaused = true;
	while(m_bPauseThread)		
		boost::this_thread::sleep(boost::posix_time::microseconds(1000));
	m_bThreadPaused = false;
}

/**
\brief	This method is waits until the m_bIOPaused flag is set to true.

\author	dcofer
\date	9/20/2014

**/
void ThreadedModule::WaitTillPaused()
{
	while(!m_bThreadPaused)		
		boost::this_thread::sleep(boost::posix_time::microseconds(1000));
}

/**
\brief	This method is waits until the m_bIOPaused flag is set to true.

\author	dcofer
\date	5/12/2014

**/
void ThreadedModule::StartPause()
{
	m_bPauseThread = true;

	if(!m_bEnabled || (m_lpSim->InSimulation() && m_lpSim->Paused()))
		m_bThreadPaused = true;

	while(!m_bThreadPaused)		
		boost::this_thread::sleep(boost::posix_time::microseconds(1000));
}

/**
\brief	This method is waits until the m_bIOPaused flag is set back to false.

\author	dcofer
\date	5/12/2014

**/
void ThreadedModule::ExitPause()
{
	m_bPauseThread = false;

	if(!m_bEnabled || (m_lpSim->InSimulation() && m_lpSim->Paused()))
		m_bThreadPaused = false;

	while(m_bThreadPaused)		
		boost::this_thread::sleep(boost::posix_time::microseconds(1000));
}

/**
\brief	This method is called just before the IO thread is closed down. It gives the IO objects a chance to do
any required cleanup.

\author	dcofer
\date	5/12/2014

**/
void ThreadedModule::ShutdownThread()
{
	//Prevent any more attempts to write to the comm channel.
	if(m_bThreadProcessing)
	{
		StartPause();
		WaitTillPaused();
	}

	ExitThread();
}

	}
}
