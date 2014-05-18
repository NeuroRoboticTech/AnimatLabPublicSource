#pragma once

namespace AnimatSim
{

	class ANIMAT_PORT SimulationMgr : public AnimatBase
	{
	protected:
		CStdPtrArray<SimulationThread> m_arySimThreads;

	public:
		SimulationMgr(void);
		virtual ~SimulationMgr(void);

		virtual SimulationThread *CreateSimulation(std::string strSimFile, bool bForceNoWindows = false);
		virtual void ShutdownAllSimulations();
		virtual AnimatBase *FindByID(std::string strID, bool bThrowError = true);
	};

}