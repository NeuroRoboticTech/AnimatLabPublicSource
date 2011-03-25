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
	gcroot<SimulatorInterface ^>  m_doSim;

public:
	SimGUICallback(SimulatorInterface ^doSim);
	SimGUICallback();
	virtual ~SimGUICallback(void);

	virtual void NeedToStopSimulation();
};

	}
}
