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
#include "SimulationThread.h"
#include "SimulationMgr.h"

#include "RobotInterface.h"
#include "RobotIOControl.h"
#include "RobotPartInterface.h"

namespace AnimatSim
{

SimulationThread::SimulationThread(void)
{
	m_lpSim = NULL;
	m_bThreadProcessing = false;
	m_bNeedToStop = false;
}

SimulationThread::~SimulationThread(void)
{
try
{
	ShutdownSimulation();

	if(m_lpSim)
	{
		delete m_lpSim;
		m_lpSim = NULL;
	}

}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of SimulationThread\r\n", "", -1, false, true);}
}

bool SimulationThread::NeedToStopSim() {return m_bNeedToStop;}

Simulator *SimulationThread::Sim() {return m_lpSim;}

void SimulationThread::StartSimulation(std::string strSimFile, bool bForceNoWindows)
{
	m_lpSim = Simulator::CreateSimulator(strSimFile, bForceNoWindows);
	
	//Set the most recently created simulation to be the active one.
	ActiveSim(m_lpSim);

	m_lpSim->Load();
    m_lpSim->VisualSelectionMode(SIMULATION_SELECTION_MODE);
	m_lpSim->SimCallBack(this);
	m_lpSim->PauseSimulation();

	m_SimThread = boost::thread(&SimulationThread::ProcessSimulation, this);

	//If we want to block till the sim is over then lets wait here until we get the notification that it has 
	//gone past the set simulation time and then stop the simulation.
	boost::posix_time::ptime pt = boost::posix_time::microsec_clock::universal_time() +  boost::posix_time::seconds(10);

	boost::interprocess::scoped_lock<boost::interprocess::interprocess_mutex> lock(m_WaitForInitEndMutex);
	bool bWaitRet = m_WaitForInitEndCond.timed_wait(lock, pt);

	if(!bWaitRet)
		THROW_PARAM_ERROR(Al_Err_lTimedOutWaitingForSimToStop, Al_Err_strTimedOutWaitingForSimToStop, "Sim ID", m_lpSim->ID());

	//Give it just smidge of time to start processing in sumulation loop. This is not strictly necessary, but I wanted to give it the time anyway.
	boost::this_thread::sleep(boost::posix_time::milliseconds(100));
}

void SimulationThread::ProcessSimulation()
{
	try
	{
		m_bThreadProcessing = true;

		if(m_lpSim)
		{
			m_lpSim->Initialize();

			m_WaitForInitEndCond.notify_all();
			m_lpSim->Simulate();
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

void SimulationThread::Simulate(float fltTime, bool bBlocking, float fltWaitTime)
{
	if(m_lpSim && m_lpSim->Paused())
	{
		if(fltTime > 0)
			m_lpSim->EndSimTime(fltTime);
		else
			m_lpSim->EndSimTime(-1);

		if(m_lpSim->SimRunning())
			m_lpSim->ToggleSimulation();
		else
			m_lpSim->StartSimulation();
	}

	if(bBlocking && fltTime > 0)
	{
		if(fltWaitTime <= 0)
			fltWaitTime = fltTime+120;

		//If we want to block till the sim is over then lets wait here until we get the notification that it has
		//gone past the set simulation time and then stop the simulation.
		boost::posix_time::ptime pt = boost::posix_time::microsec_clock::universal_time() +  boost::posix_time::seconds(fltWaitTime);

		boost::interprocess::scoped_lock<boost::interprocess::interprocess_mutex> lock(m_WaitForSimEndMutex);
		bool bWaitRet = m_WaitForSimEndCond.timed_wait(lock, pt);

		StopSimulation();

		if(!bWaitRet)
			THROW_PARAM_ERROR(Al_Err_lTimedOutWaitingForSimToStop, Al_Err_strTimedOutWaitingForSimToStop, "Sim ID", m_lpSim->ID());
	}
}

void SimulationThread::PauseSimulation()
{
	if(m_lpSim)
		m_lpSim->PauseSimulation();
}

void SimulationThread::ResumeSimulation()
{
	if(m_lpSim && m_lpSim->Paused())
		m_lpSim->ToggleSimulation();
}

void SimulationThread::ResetSimulation()
{
	if(m_lpSim->Paused())
		m_lpSim->ResetSimulation();
	else
		StopSimulation();
}

void SimulationThread::StopSimulation()
{
	if(m_lpSim)
	{
		m_lpSim->PauseSimulation();

		if(m_lpSim->WaitForSimulationBlock())
		{
			m_lpSim->ResetSimulation();
			m_lpSim->UnblockSimulation();
		}
		else
			THROW_PARAM_ERROR(Al_Err_lTimedOutWaitingForSimToStop, Al_Err_strTimedOutWaitingForSimToStop, "SimID", m_lpSim->ID());
	}
}

void SimulationThread::ShutdownSimulation()
{
	if(m_bThreadProcessing)
	{
		if(m_lpSim)
			m_lpSim->ShutdownSimulation();

	bool bTryJoin = false;
#if (BOOST_VERSION >= 105000)
	bTryJoin = m_SimThread.try_join_for(boost::chrono::seconds(10));
#else
	m_SimThread.join();
#endif
		//if(m_lpSim)
		//{
		//	delete m_lpSim;
		//	m_lpSim = NULL;
		//}
	}
}

void SimulationThread::NeedToStopSimulation()
{
	m_WaitForSimEndCond.notify_all();
}

void SimulationThread::HandleNonCriticalError(std::string strError)
{
	std::cout << "Critical error has occured. Shutting down the simulation.\r\n";
	std::cout << strError << "\r\n";
	ShutdownSimulation();
}

void SimulationThread::HandleCriticalError(std::string strError)
{
	std::cout << "Critical error has occured. Shutting down the simulation.\r\n";
	std::cout << strError << "\r\n";
	ShutdownSimulation();
}



}
