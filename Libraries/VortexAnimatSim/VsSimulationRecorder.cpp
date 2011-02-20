// VsSimulationRecorder.cpp: implementation of the VsSimulationRecorder class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "VsSimulationRecorder.h"

namespace VortexAnimatLibrary
{
	namespace Recording
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsSimulationRecorder::VsSimulationRecorder()
{}

VsSimulationRecorder::~VsSimulationRecorder()
{}

void VsSimulationRecorder::Load(Simulator *lpSim, CStdXml &oXml)
{
	SimulationRecorder::Load(lpSim, oXml);

}

	}			//Recording
}				//VortexAnimatLibrary