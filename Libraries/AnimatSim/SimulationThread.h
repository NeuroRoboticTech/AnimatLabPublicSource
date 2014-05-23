#pragma once

namespace AnimatSim
{

	class ANIMAT_PORT SimulationThread : public ISimGUICallback
	{
	protected:
		Simulator *m_lpSim;
		bool m_bThreadProcessing;
		bool m_bNeedToStop;

#ifndef STD_DO_NOT_ADD_BOOST
		boost::thread m_SimThread; 

		boost::interprocess::interprocess_mutex m_WaitForInitEndMutex;
		boost::interprocess::interprocess_condition  m_WaitForInitEndCond;

		boost::interprocess::interprocess_mutex m_WaitForSimEndMutex;
		boost::interprocess::interprocess_condition  m_WaitForSimEndCond;
#endif

		virtual void ProcessSimulation();

	public:
		SimulationThread(void);
		virtual ~SimulationThread(void);

		virtual bool NeedToStopSim();
		virtual Simulator *Sim();

		virtual void StartSimulation(std::string strSimFile, bool bForceNoWindows = false);
		virtual void Simulate(float fltTime = -1, bool bBlocking = true, float fltWaitTime = -1);
		virtual void PauseSimulation();
		virtual void ResumeSimulation();
		virtual void ResetSimulation();
		virtual void StopSimulation();
		virtual void ShutdownSimulation();
		virtual void NeedToStopSimulation();
		virtual void HandleNonCriticalError(std::string strError);
		virtual void HandleCriticalError(std::string strError);
	};

}