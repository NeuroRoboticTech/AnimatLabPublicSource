#ifndef __REAL_NET_INCLUDES_H__
#define __REAL_NET_INCLUDES_H__

#ifdef WIN32
	#define ADV_NEURAL_PORT __declspec( dllexport )
#else
	#define ADV_NEURAL_PORT
#endif

#include "AnimatSim.h"
#include "IntegrateFireSimConstants.h"

//Simulation Objects
namespace IntegrateFireSim
{
	//class ClassFactory;
	class IntegrateFireNeuralModule;
	class Neuron;

	namespace Synapses
	{
		class Connexion;
		class ElectricalSynapse;
		class NonSpikingChemicalSynapse;
		class SpikingChemicalSynapse;
	}

	namespace Utilities
	{
		class DoubleList;
	}
}

using namespace IntegrateFireSim;
using namespace IntegrateFireSim::Synapses;
using namespace IntegrateFireSim::Utilities;

#include "DoubleList.h"
#include "NeuralUtils.h"

#endif // __REAL_NET_INCLUDES_H__
