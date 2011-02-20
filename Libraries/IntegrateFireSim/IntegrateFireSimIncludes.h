#ifndef __REAL_NET_INCLUDES_H__
#define __REAL_NET_INCLUDES_H__

#define ADV_NEURAL_PORT __declspec( dllexport )

#include "AnimatSim.h"
#include "IntegrateFireSimConstants.h"

//Simulation Objects
namespace IntegrateFireSim
{
	//class ClassFactory;
	class IntegrateFireNeuralModule;
	class Neuron;

	namespace DataColumns
	{
		class NeuronData;
	}

	namespace ExternalStimuli
	{
		//class EventTrigger;
		//class CurrentStimulus;
	}

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
using namespace IntegrateFireSim::DataColumns;
using namespace IntegrateFireSim::ExternalStimuli;
using namespace IntegrateFireSim::Synapses;
using namespace IntegrateFireSim::Utilities;

#include "DoubleList.h"
#include "NeuralUtils.h"

#endif // __REAL_NET_INCLUDES_H__
