#ifndef __FAST_NEURAL_NET_LIB_DLL_H__
#define __FAST_NEURAL_NET_LIB_DLL_H__

#ifndef _FAST_NEURAL_NET_LIB_DLL_NOFORCELIBS
    #ifndef _WIN64
	    #ifdef _DEBUG
		    #pragma comment(lib, "FiringRateSim_vc10D.lib")
	    #else
		    #pragma comment(lib, "FiringRateSim_vc10.lib")
	    #endif      // _DEBUG  
    #else
	    #ifdef _DEBUG
		    #pragma comment(lib, "FiringRateSim_vc10D_x64.lib")
	    #else
		    #pragma comment(lib, "FiringRateSim_vc10_x64.lib")
	    #endif      // _DEBUG  
    #endif          // _WIN64
#endif          // _FAST_NEURAL_NET_LIB_DLL_NOFORCELIBS

#ifdef WIN32	
	#define FAST_NET_PORT __declspec( dllimport )
#else
	#define FAST_NET_PORT
#endif

#include "StdUtils.h"
#include "FiringRateSimConstants.h"

//Simulation Objects
namespace FiringRateSim
{
	class ClassFactory;
	class FiringRateModule;

	namespace Neurons
	{
		class Neuron;
		class PacemakerNeuron;
		class RandomNeuron;
		class BistableNeuron;
		class TonicNeuron;
	}

	namespace Synapses
	{
		class GatedSynapse;
		class ModulatedSynapse;
		class Synapse;
		class ModulateNeuronPropSynapse;
	}
}

using namespace FiringRateSim;
using namespace FiringRateSim::Neurons;
using namespace FiringRateSim::Synapses;

#include "Neuron.h"
#include "Synapse.h"
#include "PacemakerNeuron.h"
#include "RandomNeuron.h"
#include "BistableNeuron.h"
#include "TonicNeuron.h"
#include "GatedSynapse.h"
#include "ModulatedSynapse.h"
#include "ModulateNeuronPropSynapse.h"
#include "FiringRateModule.h"

#endif // __FAST_NEURAL_NET_LIB_DLL_H__
