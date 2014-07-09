#ifndef __HYBRID_ANIMAT_LIB_DLL_H__
#define __HYBRID_ANIMAT_LIB_DLL_H__

#ifndef _WIN64
	#ifdef _DEBUG
	    #pragma comment(lib, "HybridInterfaceSim_vc10D.lib")
    #else
	    #pragma comment(lib, "HybridInterfaceSim_vc10.lib")
	#endif      // _DEBUG  
#else
	#ifdef _DEBUG
	    #pragma comment(lib, "HybridInterfaceSim_vc10D_x64.lib")
    #else
	    #pragma comment(lib, "HybridInterfaceSim_vc10_x64.lib")
	#endif      // _DEBUG  
#endif          // _WIN64

#ifdef WIN32
	#define HYBRID_PORT __declspec( dllimport )
#else
	#define HYBRID_PORT
#endif

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
        namespace RobotInterfaces
        {
			class HiSpike2Input;
			class HiC884Controller;
			class HiM110Actuator;
        }

    }
}

using namespace HybridInterfaceSim;
using namespace HybridInterfaceSim::Robotics;


#include "HiClassFactory.h"

#include "HiSpike2Input.h"
#include "HiC884Controller.h"
#include "HiM110Actuator.h"

#endif // __HYBRID_ANIMAT_LIB_DLL_H__
