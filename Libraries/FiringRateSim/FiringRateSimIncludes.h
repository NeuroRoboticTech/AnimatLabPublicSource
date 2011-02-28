#ifndef __FAST_NET_INCLUDES_H__
#define __FAST_NET_INCLUDES_H__

#define FAST_NET_PORT __declspec( dllexport )

#include "AnimatSim.h"
#include "FiringRateSimConstants.h"

//Simulation Objects
namespace FiringRateSim
{
	class ClassFactory;
	class FiringRateModule;

	namespace Neurons
	{
		class MotorNeuron;
		class Neuron;
		class PacemakerNeuron;
		class RandomNeuron;
		class SensoryNeuron;
		class TonicNeuron;
	}

	namespace Synapses
	{
		class GatedSynapse;
		class ModulatedSynapse;
		class Synapse;
	}
}

using namespace FiringRateSim;
using namespace FiringRateSim::Neurons;
using namespace FiringRateSim::Synapses;

#include "NeuralUtils.h"

#endif // __FAST_NET_INCLUDES_H__
