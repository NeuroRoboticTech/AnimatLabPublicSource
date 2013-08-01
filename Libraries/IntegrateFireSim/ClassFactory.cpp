/**
\file	ClassFactory.cpp

\brief	Implements the class factory class.
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

namespace IntegrateFireSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/31/2011
**/
ClassFactory::ClassFactory()
{

}

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
ClassFactory::~ClassFactory()
{

}


// ************* External Stimulus Type Conversion functions ******************************


ExternalStimulus *ClassFactory::CreateExternalStimulus(string strType, bool bThrowError)
{
	ExternalStimulus *lpStimulus=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "CURRENT")
		lpStimulus = new AnimatSim::ExternalStimuli::CurrentStimulus;
	else if(strType == "VOLTAGECLAMP")
		lpStimulus = new AnimatSim::ExternalStimuli::VoltageClamp;
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


// ************* External Neural Module Conversion functions ******************************


NeuralModule *ClassFactory::CreateNeuralModule(string strType, bool bThrowError)
{
	NeuralModule *lpModule=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "INTEGRATEFIRESIMMODULE")
		lpModule = new IntegrateFireNeuralModule;
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


// ************* DataColumn Type Conversion functions ******************************


DataColumn *ClassFactory::CreateDataColumn(string strType, bool bThrowError)
{
	DataColumn *lpColumn=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	//if(strType == "NEURONDATA")
	//	lpColumn = new NeuronData;
	//else if(strType == "IONCHANNELDATA")
	//	lpColumn = new ChannelData;
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


// ************* Gain Type Conversion functions ******************************


Gain *ClassFactory::CreateGain(string strType, bool bThrowError)
{
	Gain *lpGain=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "IONCHANNELSIGMOID")
		lpGain = new IntegrateFireSim::Gains::IonChannelSigmoid;
	else
	{
		lpGain = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidGainType, Al_Err_strInvalidGainType, "GainType", strType);
	}

	return lpGain;
}
catch(CStdErrorInfo oError)
{
	if(lpGain) delete lpGain;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpGain) delete lpGain;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


// ************* Gain Conversion functions ******************************


// ************* Neuron Type Conversion functions ******************************


Neuron *ClassFactory::CreateNeuron(string strType, bool bThrowError)
{
	Neuron *lpNeuron=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "NEURON")
		lpNeuron = new Neuron;
	else 
	{
		lpNeuron = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Rn_Err_lInvalidNeuronType, Rn_Err_strInvalidNeuronType, "NeuronType", strType);
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


Connexion *ClassFactory::CreateSynapse(string strType, bool bThrowError)
{
	Connexion *lpSynapse=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "SYNAPSE")
		lpSynapse = new Connexion;
	else
	{
		lpSynapse = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Rn_Err_lInvalidSynapseType, Rn_Err_strInvalidSynapseType, "SynapseType", strType);
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


// ************* Synapse Type Conversion functions ******************************


SynapseType *ClassFactory::CreateSynapseType(string strType, bool bThrowError)
{
	SynapseType *lpSynapseType=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "SPIKINGCHEMICAL")
		lpSynapseType = new SpikingChemicalSynapse;
	else if(strType == "NONSPIKINGCHEMICAL")
		lpSynapseType = new NonSpikingChemicalSynapse;
	else if(strType == "ELECTRICAL")
		lpSynapseType = new ElectricalSynapse;
	else
	{
		lpSynapseType = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Rn_Err_lInvalidSynapseType, Rn_Err_strInvalidSynapseType, "SynapseType", strType);
	}

	return lpSynapseType;
}
catch(CStdErrorInfo oError)
{
	if(lpSynapseType) delete lpSynapseType;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpSynapseType) delete lpSynapseType;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Synapse Type Conversion functions ******************************


// ************* Ion Channel Conversion functions ******************************


IonChannel *ClassFactory::CreateIonChannel(string strType, bool bThrowError)
{
	IonChannel *lpChannel=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "IONCHANNEL")
		lpChannel = new IonChannel;
	else
	{
		lpChannel = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Rn_Err_lInvalidIonChannelType, Rn_Err_strInvalidIonChannelType, "IonChannel", strType);
	}

	return lpChannel;
}
catch(CStdErrorInfo oError)
{
	if(lpChannel) delete lpChannel;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpChannel) delete lpChannel;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Ion Channel Conversion functions ******************************


// ************* IStdClassFactory functions ******************************

CStdSerialize *ClassFactory::CreateObject(string strClassType, string strObjectType, bool bThrowError)
{
	CStdSerialize *lpObject=NULL;

	strClassType = Std_ToUpper(Std_Trim(strClassType));

	if(strClassType == "EXTERNALSTIMULUS")
		lpObject = CreateExternalStimulus(strObjectType, bThrowError);
	else if(strClassType == "NEURALMODULE")
		lpObject = CreateNeuralModule(strObjectType, bThrowError);
	else if(strClassType == "DATACOLUMN")
		lpObject = CreateDataColumn(strObjectType, bThrowError);
	else if(strClassType == "GAIN")
		lpObject = CreateGain(strObjectType, bThrowError);
	else if(strClassType == "NEURON")
		lpObject = CreateNeuron(strObjectType, bThrowError);
	else if(strClassType == "SYNAPSE")
		lpObject = CreateSynapse(strObjectType, bThrowError);
	else if(strClassType == "SYNAPSETYPE")
		lpObject = CreateSynapseType(strObjectType, bThrowError);
	else if(strClassType == "IONCHANNEL")
		lpObject = CreateIonChannel(strObjectType, bThrowError);
	else
	{
		lpObject = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Std_Err_lInvalidClassType, Std_Err_strInvalidClassType, "ClassType", strClassType);
	}

	return lpObject;
}

// ************* IStdClassFactory functions ******************************

}				//IntegrateFireSim
