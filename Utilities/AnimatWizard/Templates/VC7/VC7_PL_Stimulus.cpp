// EnablerStimulus.cpp: implementation of the EnablerStimulus class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "Simulator.h"
#include "TestStimulus.h"

namespace [*PROJECT_NAME*]
{
	namespace ExternalStimuli
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

[*STIMULUS_NAME*]::[*STIMULUS_NAME*]()
{
}

[*STIMULUS_NAME*]::~[*STIMULUS_NAME*]()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of [*STIMULUS_NAME*]\r\n", "", -1, FALSE, TRUE);}
}

void [*STIMULUS_NAME*]::Initialize(Simulator *lpSim)
{
	RigidBodyInputStimulus::Initialize(lpSim);
}

void [*STIMULUS_NAME*]::Activate(Simulator *lpSim)
{
	RigidBodyInputStimulus::Activate(lpSim);
}

void [*STIMULUS_NAME*]::StepSimulation(Simulator *lpSim)
{
	RigidBodyInputStimulus::StepSimulation(lpSim);
}

void [*STIMULUS_NAME*]::Deactivate(Simulator *lpSim)
{
	RigidBodyInputStimulus::Deactivate(lpSim);
}

void [*STIMULUS_NAME*]::Load(Simulator *lpSim, CStdXml &oXml)
{
	RigidBodyInputStimulus::Load(lpSim, oXml);

	oXml.IntoElem();  //Into Simulus Element

	//Load any variables specific for this class.

	oXml.OutOfElem(); //OutOf Simulus Element
}

	}			//ExternalStimuli
}				//[*PROJECT_NAME*]




