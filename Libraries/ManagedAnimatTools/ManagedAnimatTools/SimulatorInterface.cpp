#include "stdafx.h"
#include "Util.h"
#include "Logger.h"
#include "PropertyUpdateException.h"
#include "SimulatorInterface.h"

namespace AnimatGUI
{
	namespace Interfaces
	{

#pragma region EventCallbacks

		//********************* Stand Alone Callback Functions *******************************
		//These functions are the callbacks that get executed by the CAlSimulator object. For example, the 
		//UpdateDataCallback is called whenever the simulator has collected enough data. When it calls this
		//method it passes back the handle to an instance of a managed simulator object as a void pointer.
		//We then cast that void to a specific instance of a simulator and then fire the update events.

		void UpdateDataCallback(void *lpSim)
		{
			//SimulatorInterface ^lpSimulator = safe_cast<SimulatorInterface*>((GCHandle::op_Explicit(lpSim)).Target);
			IntPtr sim(lpSim);
			SimulatorInterface ^lpSimulator = safe_cast<SimulatorInterface^>(GCHandle::FromIntPtr(sim).Target);
			if(lpSimulator)
				lpSimulator->FireUpdateDataEvent();
		}

		void StartSimulationCallback(void *lpSim)
		{
			//SimulatorInterface *lpSimulator = __try_cast<SimulatorInterface*>((GCHandle::op_Explicit(lpSim)).Target);
			IntPtr sim(lpSim);
			SimulatorInterface ^lpSimulator = safe_cast<SimulatorInterface^>(GCHandle::FromIntPtr(sim).Target);
			if(lpSimulator)
				lpSimulator->FireStartSimulationEvent();
		}

		void PauseSimulationCallback(void *lpSim)
		{
			//SimulatorInterface *lpSimulator = __try_cast<SimulatorInterface*>((GCHandle::op_Explicit(lpSim)).Target);
			IntPtr sim(lpSim);
			SimulatorInterface ^lpSimulator = safe_cast<SimulatorInterface^>(GCHandle::FromIntPtr(sim).Target);
			if(lpSimulator)
				lpSimulator->FirePauseSimulationEvent();
		}

		void EndingSimulationCallback(void *lpSim)
		{
			//SimulatorInterface *lpSimulator = __try_cast<SimulatorInterface*>((GCHandle::op_Explicit(lpSim)).Target);
			IntPtr sim(lpSim);
			SimulatorInterface ^lpSimulator = safe_cast<SimulatorInterface^>(GCHandle::FromIntPtr(sim).Target);
			if(lpSimulator)
				lpSimulator->FireEndingSimulationEvent();
		}

		void ResetSimulationCallback(void *lpSim)
		{
			//SimulatorInterface *lpSimulator = __try_cast<SimulatorInterface*>((GCHandle::op_Explicit(lpSim)).Target);
			IntPtr sim(lpSim);
			SimulatorInterface ^lpSimulator = safe_cast<SimulatorInterface^>(GCHandle::FromIntPtr(sim).Target);
			if(lpSimulator)
				lpSimulator->FireResetSimulationEvent();
		}

#pragma endregion

	}
}