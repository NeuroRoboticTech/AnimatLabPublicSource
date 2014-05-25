// RbSimulator.cpp: implementation of the RbSimulator class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbClassFactory.h"
#include "RbSimulator.h"

namespace RoboticsAnimatSim
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


RbSimulator::RbSimulator()
{
	m_dblTotalStepTime = 0;
	m_lStepTimeCount = 0;
	m_dblTotalStepTime= 0;
	m_lStepTimeCount = 0;
	m_dblTotalVortexStepTime = 0;
	m_lStepVortexTimeCount = 0;

	if(!m_lpAnimatClassFactory)
		m_lpAnimatClassFactory = new RbClassFactory;
}

RbSimulator::~RbSimulator()
{

try
{
	m_bShuttingDown = true;

	Reset();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Simulator\r\n", "", -1, false, true);}
}

#pragma region AccessorMutatorOverrides

bool RbSimulator::InSimulation() {return false;}

#pragma endregion


SimulationRecorder *RbSimulator::CreateSimulationRecorder()
{
	return NULL;
}

void RbSimulator::Reset()
{
	Simulator::Reset();

	if(!m_lpAnimatClassFactory)
		m_lpAnimatClassFactory = new RbClassFactory;
}

void RbSimulator::ResetSimulation()
{
	Simulator::ResetSimulation();

	m_bSimRunning = false;
}

void RbSimulator::SetSimulationStabilityParams()
{

}

//Timer Methods
unsigned long long RbSimulator::GetTimerTick()
{
	m_lLastTickTaken = osg::Timer::instance()->tick();
	return m_lLastTickTaken;
}

double RbSimulator::TimerDiff_n(unsigned long long lStart, unsigned long long lEnd)
{return osg::Timer::instance()->delta_n(lStart, lEnd);}

double RbSimulator::TimerDiff_u(unsigned long long lStart, unsigned long long lEnd)
{return osg::Timer::instance()->delta_u(lStart, lEnd);}

double RbSimulator::TimerDiff_m(unsigned long long lStart, unsigned long long lEnd)
{return osg::Timer::instance()->delta_m(lStart, lEnd);}

double RbSimulator::TimerDiff_s(unsigned long long lStart, unsigned long long lEnd)
{return osg::Timer::instance()->delta_s(lStart, lEnd);}

void RbSimulator::MicroSleep(unsigned int iMicroTime)
{OpenThreads::Thread::microSleep(iMicroTime);}

void RbSimulator::WriteToConsole(std::string strMessage)
{
    std::cout << strMessage << "\r\n";
}

//This function initializes the robotics related
//classes and the vortex viewer.
void RbSimulator::InitializeRobotics(int argc, const char **argv)
{

	//int iObjectCount = 100 + m_iPhysicsBodyCount;
	//int iCollisionCount = iObjectCount*40;

 //   m_lpCollisionConfiguration = new btDefaultCollisionConfiguration();
 //   m_lpDispatcher = new RbAnimatCollisionDispatcher(m_lpCollisionConfiguration, this);
 //   m_lpSolver = new btSequentialImpulseConstraintSolver;

 //   btVector3 worldAabbMin( -10000, -10000, -10000 );
 //   btVector3 worldAabbMax( 10000, 10000, 10000 );
 //   m_lpBroadPhase = new btAxisSweep3( worldAabbMin, worldAabbMax, 1000 );

 //   m_lpDynamicsWorld = new btDiscreteDynamicsWorld( m_lpDispatcher, m_lpBroadPhase, m_lpSolver, m_lpCollisionConfiguration );
 //   m_lpDynamicsWorld->setGravity( btVector3( 0, m_fltGravity, 0 ) );

	//m_lpDynamicsWorld->setInternalTickCallback(ProcessTickCallback, static_cast<void *>(this));

 //   if(m_bDrawDebug)
 //   {
 //       this->OSGRoot()->addChild(m_dbgDraw.getSceneGraph());
 //       m_lpDynamicsWorld->setDebugDrawer( &m_dbgDraw );
 //   }

	////gContactAddedCallback = &AnimatContactCallback;
 //   gContactProcessedCallback = &AnimatContactCallback;
}


void RbSimulator::Initialize(int argc, const char **argv)
{
	InitializeRobotics(argc, argv);

	InitializeStructures();

	m_oDataChartMgr.Initialize();
	m_oExternalStimuliMgr.Initialize();
	if(m_lpSimRecorder) m_lpSimRecorder->Initialize();
}

void RbSimulator::StepSimulation()
{
	//Test Code
	//if(m_lTimeSlice > 10 && m_lTimeSlice < 5000 && !m_timePeriod.TimerStarted())
	//	m_timePeriod.StartTimer();


	try
	{
		//step the frame and update the windows
		if (!m_bPaused)
		{
			Simulator::StepSimulation();

			unsigned long long lStart = GetTimerTick();

   //         if( m_bDrawDebug )
   //             m_dbgDraw.BeginDraw();

   //         m_lpDynamicsWorld->stepSimulation(m_fltPhysicsTimeStep, m_iPhysicsSubsteps, m_fltPhysicsSubstepTime);

   //         if( m_bDrawDebug )
   //         {
   //             m_lpDynamicsWorld->debugDrawWorld();
   //             m_dbgDraw.EndDraw();
   //         }

            double dblVal = TimerDiff_s(lStart, GetTimerTick());
			m_fltPhysicsStepTime += dblVal;

			if(m_lTimeSlice > 10 && m_lTimeSlice < 5000)
			{
				m_dblTotalVortexStepTime += dblVal;
				m_lStepVortexTimeCount++;
			}
			else if(m_lTimeSlice == 5000)
			{
				double dblAvgStepTime = m_dblTotalVortexStepTime/m_lStepVortexTimeCount;
				//osg::notify(osg::NOTICE) << "Average step time: " << dblAvgStepTime << std::endl;
				//osg::notify(osg::NOTICE) << "Total vortex step time: " << m_dblTotalVortexStepTime << std::endl;
				//osg::notify(osg::NOTICE) << "Slice Time: " << m_lTimeSlice << std::endl;
				//osg::notify(osg::NOTICE) << "Sim Time: " << Time() << std::endl;
			}
		}

        //PauseSimulation();

	}
	catch(CStdErrorInfo oError)
	{
		std::string strError = "An error occurred while step the simulation.\nError: " + oError.m_strError;
		HandleNonCriticalError(strError);
	}


	//double dblVal2 = m_timeSimulationStep.StopTimer();
	//if(m_lTimeSlice > 10 && m_lTimeSlice < 5000)
	//{
	//	m_dblTotalStepTime += dblVal2;
	//	m_lStepTimeCount++;
	//}
	//else if(m_lTimeSlice == 5000)
	//{
	//	double dblAvgStepTime = m_dblTotalStepTime/m_lStepTimeCount;
	//	cout << "Average step time: " << dblAvgStepTime << std::endl;
	//	cout << "Total step time: " << m_dblTotalStepTime << ", " << m_lStepTimeCount << std::endl;
	//	cout << "Period time: " << m_timePeriod.StopTimer() << std::endl;
	//	cout << "Slice Time: " << m_lTimeSlice << std::endl;
	//	cout << "Sim Time: " << Time() << std::endl;
	//}

}

void RbSimulator::ShutdownSimulation()
{
	SimStopping();
	m_bForceSimulationStop = true;
}

bool RbSimulator::PauseSimulation()
{
	SimPausing();
	m_bPaused = true;
	return true;
}

bool RbSimulator::StartSimulation()
{
	m_lStartSimTick = GetTimerTick();

	SimStarting();
	m_bSimRunning = true;
	m_bPaused = false;
	return true;
}

void RbSimulator::ToggleSimulation()
{
	if(m_bPaused)
		SimStarting();
	else
		SimPausing();

	m_bPaused = !m_bPaused;
}

void RbSimulator::StopSimulation()
{
	SimStopping();
	if(!m_bPaused)
		ToggleSimulation();
	m_bSimRunning = false;
}

void RbSimulator::SimulateEnd()
{
    Reset();
}

}			//RoboticsAnimatSim
