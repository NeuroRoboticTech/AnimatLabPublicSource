#pragma once

namespace AnimatSim
{

	class ANIMAT_PORT SimulationMgr : public AnimatBase
	{
	protected:
		CStdPtrArray<SimulationThread> m_arySimThreads;

		SimulationMgr(void);
		virtual ~SimulationMgr(void);

	public:
		/// Singleton accessor for this class.
		static SimulationMgr &Instance();
						
		static SimulationMgr *CastToDerived(AnimatBase *lpBase) {return static_cast<SimulationMgr*>(lpBase);}

		virtual CStdPtrArray<SimulationThread> &SimThreads() {return m_arySimThreads;};
		virtual SimulationThread *CreateSimulation(std::string strSimFile, bool bForceNoWindows = false);
		virtual void ShutdownAllSimulations();
		virtual AnimatBase *FindByID(std::string strID, bool bThrowError = true);
	};

	void ANIMAT_PORT ActiveSim(Simulator *lpActive);
	Simulator ANIMAT_PORT *ActiveSim();

}