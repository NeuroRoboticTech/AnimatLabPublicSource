/**
\file	ClassFactory.cpp

\brief	Implements the class factory class.
**/

#include "StdAfx.h"

#include "Synapse.h"
#include "GatedSynapse.h"
#include "ModulatedSynapse.h"
#include "ModulateNeuronPropSynapse.h"
#include "Neuron.h"
#include "PacemakerNeuron.h"
#include "TonicNeuron.h"
#include "RandomNeuron.h"
#include "BistableNeuron.h"
#include "FiringRateModule.h"
#include "ClassFactory.h"

namespace FiringRateSim
{

/**
\brief	Default constructor.
	
\author	dcofer
\date	3/30/2011
**/
ClassFactory::ClassFactory()
{

}

/**
\brief	Destructor.

\author	dcofer
\date	3/30/2011
**/
ClassFactory::~ClassFactory()
{

}

Neuron *ClassFactory::CreateNeuron(std::string strType, bool bThrowError)
{
	Neuron *lpNeuron=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "REGULAR")
		lpNeuron = new Neuron;
	else if(strType == "PACEMAKER")
		lpNeuron = new PacemakerNeuron;
	else if(strType == "TONIC")
		lpNeuron = new TonicNeuron;
	else if(strType == "RANDOM")
		lpNeuron = new RandomNeuron;
	else if(strType == "BISTABLE")
		lpNeuron = new BistableNeuron;
	else 
	{
		lpNeuron = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Nl_Err_lInvalidNeuronType, Nl_Err_strInvalidNeuronType, "NeuronType", strType);
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


Synapse *ClassFactory::CreateSynapse(std::string strType, bool bThrowError)
{
	Synapse *lpSynapse=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "REGULAR")
		lpSynapse = new Synapse;
	else if(strType == "COMPOUND")
		lpSynapse = new Synapse;
	else if(strType == "GATED")
		lpSynapse = new GatedSynapse;
	else if(strType == "MODULATED")
		lpSynapse = new ModulatedSynapse;
	else if(strType == "MODULATENEURONPROP")
		lpSynapse = new ModulateNeuronPropSynapse;
	else
	{
		lpSynapse = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Nl_Err_lInvalidSynapseType, Nl_Err_strInvalidSynapseType, "SynapseType", strType);
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


// ************* DataColumn Type Conversion functions ******************************


DataColumn *ClassFactory::CreateDataColumn(std::string strType, bool bThrowError)
{
	DataColumn *lpColumn=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	//if(strType == "NEURONDATA")
	//	lpColumn = new NeuronData;
	//else
	//{
		lpColumn = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidDataColumnType, Al_Err_strInvalidDataColumnType, "DataColumnType", strType);
	//}

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


// ************* External Stimulus Type Conversion functions ******************************


ExternalStimulus *ClassFactory::CreateExternalStimulus(std::string strType, bool bThrowError)
{
	ExternalStimulus *lpStimulus=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	//if(strType == "CURRENTINJECTION")
	//	lpStimulus = new CurrentInjection;
	if(strType == "CURRENT")
		lpStimulus = new AnimatSim::ExternalStimuli::CurrentStimulus;
	else if(strType == "VOLTAGECLAMP")
		lpStimulus = new AnimatSim::ExternalStimuli::VoltageClamp;
	else
	{
		lpStimulus = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Nl_Err_lInvalidExternalStimulusType, Nl_Err_strInvalidExternalStimulusType, "ExternalStimulusType", strType);
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


// ************* External Neural Module Conversion functions ******************************


NeuralModule *ClassFactory::CreateNeuralModule(std::string strType, bool bThrowError)
{
	NeuralModule *lpModule=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "FIRINGRATESIMMODULE")
		lpModule = new FiringRateModule;
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


// ************* IStdClassFactory functions ******************************

CStdSerialize *ClassFactory::CreateObject(std::string strClassType, std::string strObjectType, bool bThrowError)
{
	CStdSerialize *lpObject=NULL;

	strClassType = Std_ToUpper(Std_Trim(strClassType));

	if(strClassType == "DATACOLUMN")
		lpObject = CreateDataColumn(strObjectType, bThrowError);
	else if(strClassType == "EXTERNALSTIMULUS")
		lpObject = CreateExternalStimulus(strObjectType, bThrowError);
	else if(strClassType == "NEURON")
		lpObject = CreateNeuron(strObjectType, bThrowError);
	else if(strClassType == "SYNAPSE")
		lpObject = CreateSynapse(strObjectType, bThrowError);
	else if(strClassType == "NEURALMODULE")
		lpObject = CreateNeuralModule(strObjectType, bThrowError);
	else
	{
		lpObject = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Std_Err_lInvalidClassType, Std_Err_strInvalidClassType, "ClassType", strClassType);
	}

	return lpObject;
}

// ************* IStdClassFactory functions ******************************

}				//FiringRateSim
