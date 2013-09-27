#include "stdafx.h"
//#include "ILogger.h"
#include "Util.h"
#include "PropertyUpdateException.h"
//#include "ISimulatorInterface.h"
#include "SimulatorInterface.h"
#include "SimGUICallback.h"
//#include "IDataObjectInterface.h"
#include "DataObjectInterface.h"

namespace AnimatGUI
{
	namespace Interfaces
	{

SimGUICallback::SimGUICallback(ManagedAnimatInterfaces::ISimulatorInterface ^doSim)
{
	m_doSim = doSim;
}

SimGUICallback::~SimGUICallback(void)
{
}

void SimGUICallback::NeedToStopSimulation()
{
	m_doSim->FireNeedToStopSimulationEvent();
}

void SimGUICallback::HandleNonCriticalError(std::string strError)
{
	System::String ^sError = gcnew String(strError.c_str());

	m_doSim->FireHandleNonCriticalErrorEvent(sError);
}

void SimGUICallback::HandleCriticalError(std::string strError)
{
	System::String ^sError = gcnew String(strError.c_str());

	m_doSim->FireHandleCriticalErrorEvent(sError);
}

	}
}
