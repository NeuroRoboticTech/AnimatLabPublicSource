#ifndef __ANIMAT_CARL_SIM_INCLUDES_H__
#define __ANIMAT_CARL_SIM_INCLUDES_H__

#ifdef WIN32
	#define ANIMAT_CARL_SIM_PORT __declspec( dllexport )
#else
	#define ANIMAT_CARL_SIM_PORT
#endif

#include "AnimatSim.h"
#include "AnimatCarlSimConstants.h"
#include "snn.h"

#include <boost/timer.hpp>
#include <boost/signals2/signal.hpp>
#include <boost/bind.hpp>

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
	class CsNeuronDataColumn;
}

using namespace AnimatCarlSim;

#endif // __FAST_NET_INCLUDES_H__
