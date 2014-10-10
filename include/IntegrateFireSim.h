#ifndef __REALISTIC_NEURAL_NET_LIB_DLL_H__
#define __REALISTIC_NEURAL_NET_LIB_DLL_H__

#ifndef _IGF_NEURAL_NET_LIB_DLL_NOFORCELIBS
    #ifndef _WIN64
	    #ifdef _DEBUG
		    #pragma comment(lib, "IntegrateFireSim_vc10D.lib")
	    #else
		    #pragma comment(lib, "IntegrateFireSim_vc10.lib")
	    #endif      // _DEBUG  
    #else
	    #ifdef _DEBUG
		    #pragma comment(lib, "IntegrateFireSim_vc10D_x64.lib")
	    #else
		    #pragma comment(lib, "IntegrateFireSim_vc10_x64.lib")
	    #endif      // _DEBUG  
    #endif          // _WIN64
#endif          // _IGF_NEURAL_NET_LIB_DLL_NOFORCELIBS

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
