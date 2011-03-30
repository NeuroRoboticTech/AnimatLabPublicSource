/**
\file	NeuralUtils.cpp

\brief	Implements the neural utilities class.
**/

#include "stdafx.h"


#include "Synapse.h"
#include "GatedSynapse.h"
#include "ModulatedSynapse.h"
#include "Neuron.h"
#include "PacemakerNeuron.h"
#include "RandomNeuron.h"
#include "FiringRateModule.h"
#include "ClassFactory.h"

string Nl_NeuralModuleName()
{
	return "FiringRateSim";
	//#ifdef _DEBUG
	//	return "FastNeuralNet_vc9D.dll";
	//#else
	//	return "FastNeuralNet_vc9.dll";
	//#endif
}


extern "C" __declspec(dllexport) IStdClassFactory* __cdecl GetStdClassFactory() 
{
	IStdClassFactory *lpFactory = new ClassFactory;
	return lpFactory;
}



