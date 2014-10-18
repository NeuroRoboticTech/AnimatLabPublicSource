/**
\file	CsClassFactory.cpp

\brief	Implements the class factory class.
**/

#include "StdAfx.h"

#include "CsNeuralModule.h"
#include "CsNeuronGroup.h"
#include "CsSpikeGeneratorGroup.h"
#include "CsSynapseGroup.h"
#include "CsSynapseOneToOne.h"
#include "CsSynapseFull.h"
#include "CsSynapseRandom.h"
#include "CsSynapseIndividual.h"
#include "CsSpikingCurrentSynapse.h"
#include "CsFiringRateStimulus.h"
#include "CsNeuronDataColumn.h"
#include "CsAdapter.h"
#include "CsClassFactory.h"

namespace AnimatCarlSim
{

/**
\brief	Default constructor.
	
\author	dcofer
\date	3/30/2011
**/
CsClassFactory::CsClassFactory()
{

}

/**
\brief	Destructor.

\author	dcofer
\date	3/30/2011
**/
CsClassFactory::~CsClassFactory()
{

}

Node *CsClassFactory::CreateNeuron(std::string strType, bool bThrowError)
{
	Node *lpNeuron=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "NEURONGROUP")
		lpNeuron = new CsNeuronGroup;
	else if(strType == "SPIKEGENERATORGROUP")
		lpNeuron = new CsSpikeGeneratorGroup;
	else 
	{
		lpNeuron = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Cs_Err_lInvalidNeuronType, Cs_Err_strInvalidNeuronType, "NeuronType", strType);
	}

	return lpNeuron;
}
catch(CStdErrorInfo oError)
{
	if(lpNeuron) delete lpNeuron;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpNeuron) delete lpNeuron;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


// ************* Neuron Type Conversion functions ******************************


// ************* Synapse Type Conversion functions ******************************


AnimatSim::Link *CsClassFactory::CreateSynapse(std::string strType, bool bThrowError)
{
	AnimatSim::Link *lpSynapse=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "ONETOONESYNAPSE")
		lpSynapse = new CsSynapseOneToOne;
	else if(strType == "FULLSYNAPSE")
		lpSynapse = new CsSynapseFull;
	else if(strType == "RANDOMSYNAPSE")
		lpSynapse = new CsSynapseRandom;
	else if(strType == "INDIVIDUALSYNAPSE")
		lpSynapse = new CsSynapseIndividual;
	else if(strType == "SPIKINGCURRENTSYNAPSE")
		lpSynapse = new CsSpikingCurrentSynapse;
	else
	{
		lpSynapse = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Cs_Err_lInvalidSynapseType, Cs_Err_strInvalidSynapseType, "SynapseType", strType);
	}

	return lpSynapse;
}
catch(CStdErrorInfo oError)
{
	if(lpSynapse) delete lpSynapse;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpSynapse) delete lpSynapse;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Synapse Type Conversion functions ******************************


// ************* External Neural Module Conversion functions ******************************


NeuralModule *CsClassFactory::CreateNeuralModule(std::string strType, bool bThrowError)
{
	NeuralModule *lpModule=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "CARLSIMNEURALMODULE")
		lpModule = new CsNeuralModule;
	else
	{
		lpModule = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidNeuralModuleType, Al_Err_strInvalidNeuralModuleType, "NeuralModule", strType);
	}

	return lpModule;
}
catch(CStdErrorInfo oError)
{
	if(lpModule) delete lpModule;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpModule) delete lpModule;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Neural Module Type Conversion functions ******************************


// ************* External Stimulus Type Conversion functions ******************************

ExternalStimulus *CsClassFactory::CreateExternalStimulus(std::string strType, bool bThrowError)
{
	ExternalStimulus *lpStimulus=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "FIRINGRATE")
		lpStimulus = new CsFiringRateStimulus;
	else
	{
		lpStimulus = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidExternalStimulusType, Al_Err_strInvalidExternalStimulusType, "ExternalStimulusType", strType);
	}

	return lpStimulus;
}
catch(CStdErrorInfo oError)
{
	if(lpStimulus) delete lpStimulus;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpStimulus) delete lpStimulus;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* External Stimulus Type Conversion functions ******************************


// ************* DataColumn Type Conversion functions ******************************

DataColumn *CsClassFactory::CreateDataColumn(std::string strType, bool bThrowError)
{
	DataColumn *lpColumn=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "NEURONDATACOLUMN")
		lpColumn = new CsNeuronDataColumn;
	else
	{
		lpColumn = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidDataColumnType, Al_Err_strInvalidDataColumnType, "DataColumnType", strType);
	}

	return lpColumn;
}
catch(CStdErrorInfo oError)
{
	if(lpColumn) delete lpColumn;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpColumn) delete lpColumn;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* DataColumn Type Conversion functions ******************************


// ************* Adapter Type Conversion functions ******************************

Adapter *CsClassFactory::CreateAdapter(std::string strType, bool bThrowError)
{
	Adapter *lpAdapter=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "NODETONODE")
		lpAdapter = new CsAdapter;
	else if(strType == "PHYSICALTONODE")
		lpAdapter = new CsAdapter;
	else
	{
		lpAdapter = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidAdapterType, Al_Err_strInvalidAdapterType, "AdapterType", strType);
	}

	return lpAdapter;
}
catch(CStdErrorInfo oError)
{
	if(lpAdapter) delete lpAdapter;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpAdapter) delete lpAdapter;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Adpater Type Conversion functions ******************************



// ************* IStdCsClassFactory functions ******************************

CStdSerialize *CsClassFactory::CreateObject(std::string strClassType, std::string strObjectType, bool bThrowError)
{
	CStdSerialize *lpObject=NULL;

	strClassType = Std_ToUpper(Std_Trim(strClassType));

	if(strClassType == "NEURON")
		lpObject = CreateNeuron(strObjectType, bThrowError);
	else if(strClassType == "SYNAPSE")
		lpObject = CreateSynapse(strObjectType, bThrowError);
	else if(strClassType == "NEURALMODULE")
		lpObject = CreateNeuralModule(strObjectType, bThrowError);
	else if(strClassType == "EXTERNALSTIMULUS")
		lpObject = CreateExternalStimulus(strObjectType, bThrowError);
	else if(strClassType == "DATACOLUMN")
		lpObject = CreateDataColumn(strObjectType, bThrowError);
	else if(strClassType == "ADAPTER")
		lpObject = CreateAdapter(strObjectType, bThrowError);
	else
	{
		lpObject = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Std_Err_lInvalidClassType, Std_Err_strInvalidClassType, "ClassType", strClassType);
	}

	return lpObject;
}

// ************* IStdCsClassFactory functions ******************************

}				//AnimatCarlSim

#ifdef WIN32
extern "C" __declspec(dllexport) IStdClassFactory* __cdecl GetStdClassFactory() 
#else
extern "C" IStdClassFactory* GetStdClassFactory() 
#endif
{
	IStdClassFactory *lpFactory = new CsClassFactory;
	return lpFactory;
}
