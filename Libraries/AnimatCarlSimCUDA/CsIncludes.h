#ifndef __ANIMAT_CARL_SIM_INCLUDES_H__
#define __ANIMAT_CARL_SIM_INCLUDES_H__

#ifdef WIN32
	#define ANIMAT_CARL_SIM_PORT __declspec( dllexport )
#else
	#define ANIMAT_CARL_SIM_PORT
#endif

#include "AnimatSim.h"
#include "AnimatCarlSimConstants.h"

//Simulation Objects
namespace AnimatCarlSim
{
	class CsClassFactory;
	class CsNeuralModule;
	class CsNeuron;
	class CsSynapse;
}

using namespace AnimatCarlSim;

#endif // __FAST_NET_INCLUDES_H__
