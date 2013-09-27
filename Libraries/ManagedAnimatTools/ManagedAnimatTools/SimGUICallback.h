#pragma once

#include <vcclr.h>

#using <mscorlib.dll>
using namespace System;

namespace AnimatGUI
{
	namespace Interfaces
	{

class SimGUICallback : public AnimatSim::ISimGUICallback
{
protected:
	gcroot<ManagedAnimatInterfaces::ISimulatorInterface ^>  m_doSim;

public:
	SimGUICallback(ManagedAnimatInterfaces::ISimulatorInterface ^doSim);
	SimGUICallback();
	virtual ~SimGUICallback(void);

	virtual void NeedToStopSimulation();
	virtual void HandleNonCriticalError(std::string strError);
	virtual void HandleCriticalError(std::string strError);
};

	}
}
