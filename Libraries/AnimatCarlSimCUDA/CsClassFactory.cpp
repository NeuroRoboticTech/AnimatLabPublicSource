/**
\file	CsClassFactory.cpp

\brief	Implements the class factory class.
**/

#include "StdAfx.h"

#include "CsNeuralModule.h"
#include "CsNeuronGroup.h"
#include "CsSynapseGroup.h"
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

CsNeuronGroup *CsClassFactory::CreateNeuron(std::string strType, bool bThrowError)
{
	CsNeuronGroup *lpNeuron=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "NEURONGROUP")
		lpNeuron = new CsNeuronGroup;
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


CsSynapseGroup *CsClassFactory::CreateSynapse(std::string strType, bool bThrowError)
{
	CsSynapseGroup *lpSynapse=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "SYNAPSEGROUP")
		lpSynapse = new CsSynapseGroup;
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
