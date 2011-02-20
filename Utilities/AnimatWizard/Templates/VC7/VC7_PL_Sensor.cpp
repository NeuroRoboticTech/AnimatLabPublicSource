// [*BODY_PART_NAME*].cpp: implementation of the [*BODY_PART_NAME*] class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "StdAfx.h"
#include "[*BODY_PART_NAME*].h"

namespace [*PROJECT_NAME*]
{
	namespace Bodies
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

[*BODY_PART_NAME*]::[*BODY_PART_NAME*]()
{
}

[*BODY_PART_NAME*]::~[*BODY_PART_NAME*]()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of [*BODY_PART_NAME*]\r\n", "", -1, FALSE, TRUE);}
}

void [*BODY_PART_NAME*]::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	VsSensor::StepSimulation(lpSim, lpStructure);
}

void TestSensor::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	VsSensor::Load(lpSim, lpStructure, oXml);
}
	}		//Bodies
}				//[*PROJECT_NAME*]

