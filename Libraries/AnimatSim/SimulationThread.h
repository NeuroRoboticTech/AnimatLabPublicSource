#pragma once

namespace AnimatSim
{

	class ANIMAT_PORT SimulationThread : public ISimGUICallback
	{
	protected:
		Simulator *m_lpSim;
		bool m_bThreadProcessing;

#ifndef STD_DO_NOT_ADD_BOOST
		boost::thread m_SimThread; 
#endif

		virtual void ProcessSimulation();

	public:
		SimulationThread(void);
		virtual ~SimulationThread(void);

		virtual Simulator *Sim();

		virtual void StartSimulation(std::string strSimFile, bool bForceNoWindows = false);
		virtual void Simulate(float fltTime = -1);
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