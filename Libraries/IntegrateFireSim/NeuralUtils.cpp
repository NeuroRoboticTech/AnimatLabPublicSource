/**
\file	NeuralUtils.cpp

\brief	Implements the neural utilities class.
**/

#include "stdafx.h"
#include "IonChannel.h"
#include "SynapseType.h"
#include "Connexion.h"
#include "CaActivation.h"
#include "Neuron.h"
#include "ElectricalSynapse.h"
#include "NonSpikingChemicalSynapse.h"
#include "SpikingChemicalSynapse.h"
#include "IntegrateFireModule.h"
#include "IonChannelSigmoid.h"
#include "ClassFactory.h"

#ifdef WIN32
extern "C" __declspec(dllexport) IStdClassFactory* __cdecl GetStdClassFactory() 
#else
extern "C" IStdClassFactory* GetStdClassFactory() 
#endif
{
	IStdClassFactory *lpFactory = new ClassFactory;
	return lpFactory;
}



