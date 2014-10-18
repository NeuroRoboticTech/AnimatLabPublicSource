#ifndef __ANIMAT_CARL_SIM_LIB_DLL_H__
#define __ANIMAT_CARL_SIM_LIB_DLL_H__

#ifndef _ANIMAT_CARL_SIM_LIB_DLL_NOFORCELIBS
    #ifndef _WIN64
	    #ifdef _DEBUG
		    #pragma comment(lib, "AnimatCarlSim_vc10D.lib")
	    #else
		    #pragma comment(lib, "AnimatCarlSim_vc10.lib")
	    #endif      // _DEBUG  
    #else
	    #ifdef _DEBUG
		    #pragma comment(lib, "AnimatCarlSim_vc10D_x64.lib")
	    #else
		    #pragma comment(lib, "AnimatCarlSim_vc10_x64.lib")
	    #endif      // _DEBUG  
    #endif          // _WIN64
#endif          // _ANIMAT_CARL_SIM_LIB_DLL_NOFORCELIBS

#ifdef WIN32	
	#define ANIMAT_CARL_SIM_PORT __declspec( dllimport )
#else
	#define ANIMAT_CARL_SIM_PORT
#endif

#include "StdUtils.h"
#include "AnimatCarlSimConstants.h"

//Simulation Objects
namespace AnimatCarlSim
{
	class CsClassFactory;
	class CsNeuralModule;
	class CsNeuronGroup;
	class CsSpikeGeneratorGroup;
	class CsSynapseGroup;
	class CsSynapseOneToOne;
	class CsSynapseRandom;
	class CsSynapseFull;
	class CsSynapseIndividual;
	class CsConnectionGenerator;
}

using namespace AnimatCarlSim;

#include "CarlSimModule.h"

#endif // __ANIMAT_CARL_SIM_LIB_DLL_H__
