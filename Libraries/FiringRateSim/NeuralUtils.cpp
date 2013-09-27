/**
\file	NeuralUtils.cpp

\brief	Implements the neural utilities class.
**/

#include "StdAfx.h"


#include "Synapse.h"
#include "GatedSynapse.h"
#include "ModulatedSynapse.h"
#include "Neuron.h"
#include "PacemakerNeuron.h"
#include "RandomNeuron.h"
#include "FiringRateModule.h"
#include "ClassFactory.h"

std::string Nl_NeuralModuleName()
{
	return "FiringRateSim";
	//#ifdef _DEBUG
	//	return "FastNeuralNet_VC10D.dll";
	//#else
	//	return "FastNeuralNet_VC10.dll";
	//#endif
}

#ifdef WIN32
extern "C" __declspec(dllexport) IStdClassFactory* __cdecl GetStdClassFactory() 
#else
extern "C" IStdClassFactory* GetStdClassFactory() 
#endif
{
	IStdClassFactory *lpFactory = new ClassFactory;
	return lpFactory;
}



