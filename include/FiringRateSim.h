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

#define FAST_NET_PORT __declspec( dllimport )

#include "StdUtils.h"
#include "FiringRateSimConstants.h"

//Simulation Objects
namespace FiringRateSim
{
	class CNlClassFactory;
	class CNlNervousSystem;

	namespace DataColumns
	{
		class CNlNeuronDataColumn;
	}

	namespace ExternalStimuli
	{
		class CNlCurrentInjection;
	}

	namespace Neurons
	{
		class CNlNeuron;
		class CNlPacemakerNeuron;
		class CNlRandomNeuron;
		class CNlTonicNeuron;
	}

	namespace Synapses
	{
		class CNlCompoundSynapse;
		class CNlGatedSynapse;
		class CNlModulatedSynapse;
		class CNlSynapse;
	}
}

using namespace FiringRateSim;
using namespace FiringRateSim::DataColumns;
using namespace FiringRateSim::ExternalStimuli;
using namespace FiringRateSim::Neurons;
using namespace FiringRateSim::Synapses;

#include "NeuralUtils.h"

#endif // __FAST_NEURAL_NET_LIB_DLL_H__
