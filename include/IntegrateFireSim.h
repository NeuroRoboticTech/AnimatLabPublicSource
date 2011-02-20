#ifndef __REALISTIC_NEURAL_NET_LIB_DLL_H__
#define __REALISTIC_NEURAL_NET_LIB_DLL_H__

#ifndef _REALISTIC_NEURAL_NET_LIB_DLL_NOFORCELIBS
	#ifdef _DEBUG
		#pragma comment(lib, "RealisticNeuralNet_vc9D.lib")
	#else
		#pragma comment(lib, "RealisticNeuralNet_vc9.lib")
	#endif
#endif          // _REALISTIC_NEURAL_NET_DLL_NOFORCELIBS

#define ADV_NEURAL_PORT __declspec( dllimport )

#include "StdUtils.h"
#include "AdvancedNeuronConstants.h"

class CNervousSystem;
class CElecSyn;
class CNonSpikingChemSyn;
class CSpikingChemSyn;
class CNeuron;
class CStim;
class CConnexion;
//class CEventTrigger

//#include "EventTrigger.h"
#include "Connexion.h"
#include "EventTrigger.h"
#include "Stim.h"
#include "ElecSyn.h"
#include "NonSpikingChemSyn.h"
#include "SpikingChemSyn.h"
#include "Neuron.h"
#include "NervousSystem.h"

#endif // __REALISTIC_NEURAL_NET_LIB_DLL_H__
