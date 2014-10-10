// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#ifdef WIN32
    #define _SCL_SECURE_NO_WARNINGS
    #define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
    // Windows Header Files:
    #include <windows.h>

    #define HYBRID_PORT __declspec( dllexport )
#else
    #define HYBRID_PORT
#endif

//#define STD_TRACING_ON

#include "StdUtils.h"
#include "AnimatSim.h"

//Include the timer code and openthreads code from osg
#include <osg/Timer>
#include <OpenThreads/Thread>

#include "HybridInterfaceSimConstants.h"

//Simulation Objects
namespace HybridInterfaceSim
{
	class HiClassFactory;

    namespace Robotics
    {
		class HiSpike2;
		class HiC884Controller;
		class HiM110Actuator;
    }
}

using namespace HybridInterfaceSim;
using namespace HybridInterfaceSim::Robotics;
