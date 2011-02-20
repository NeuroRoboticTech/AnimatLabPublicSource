// ClassFactory.cpp: implementation of the ClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"

#include "[*BODY_PART_NAME*].h"
#include "[*STIMULUS_NAME*].h"
#include "[*MUSCLE_NAME*].h"
#include "Synapse.h"
#include "Neuron.h"
#include "[*PROJECT_NAME*]NeuralModule.h"
#include "NeuronData.h"
#include "ClassFactory.h"


namespace [*PROJECT_NAME*]
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

ClassFactory::ClassFactory()
{

}

ClassFactory::~ClassFactory()
{

}


// ************* Neuron Type Conversion functions ******************************

Neuron *ClassFactory::CreateNeuron(string strType, BOOL bThrowError)
{
	Neuron *lpNeuron=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "REGULAR")
		lpNeuron = new Neuron;
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

Synapse *ClassFactory::CreateSynapse(string strType, BOOL bThrowError)
{
	Synapse *lpSynapse=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "REGULAR")
		lpSynapse = new Synapse;
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

// ************* DataColumn Type Conversion functions ******************************

DataColumn *ClassFactory::CreateDataColumn(string strType, BOOL bThrowError)
{
	DataColumn *lpColumn=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "NEURONDATA")
		lpColumn = new NeuronData;
	else
	{
		lpColumn = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidDataColumnType, Al_Err_strInvalidDataColumnType, "DataColumnType", strType);
		return FALSE;
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


// ************* External Stimulus Type Conversion functions ******************************

ExternalStimulus *ClassFactory::CreateExternalStimulus(string strType, BOOL bThrowError)
{
	ExternalStimulus *lpStimulus=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "CURRENT")
		lpStimulus = new AnimatLibrary::ExternalStimuli::CurrentStimulus;
	else if(strType == "[*STIMULUS_TYPE*]")
		lpStimulus = new [*STIMULUS_NAME*];
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

NeuralModule *ClassFactory::CreateNeuralModule(string strType, BOOL bThrowError)
{
	NeuralModule *lpModule=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "[*PROJECT_NAME_CAPS*]NEURALMODULE")
		lpModule = new [*PROJECT_NAME*]NeuralModule;
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

// ************* Body Type Conversion functions ******************************

RigidBody *ClassFactory::CreateRigidBody(string strType, BOOL bThrowError)
{
	RigidBody *lpPart=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "[*BODY_PART_NAME_CAPS*]")
		lpPart = new [*BODY_PART_NAME*];
	else if(strType == "[*MUSCLE_NAME_CAPS*]")
		lpPart = new [*MUSCLE_NAME*];
	else
	{
		lpPart = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidPartType, Al_Err_strInvalidPartType, "PartType", strType);
	}

	return lpPart;
}
catch(CStdErrorInfo oError)
{
	if(lpPart) delete lpPart;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpPart) delete lpPart;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Body Type Conversion functions ******************************


// ************* Body Joint Conversion functions ******************************

Joint *ClassFactory::CreateJoint(string strType, BOOL bThrowError)
{
	Joint *lpJoint=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	//No joints are defined for this library yet.
	//if(strType == "HINGE")
	//	lpJoint = NULL;
	//else
	//{
		lpJoint = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidJointType, Al_Err_strInvalidJointType, "JointType", strType);
	//}

	return lpJoint;
}
catch(CStdErrorInfo oError)
{
	if(lpJoint) delete lpJoint;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpJoint) delete lpJoint;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


// ************* Body Joint Conversion functions ******************************

// ************* IStdClassFactory functions ******************************

CStdSerialize *ClassFactory::CreateObject(string strClassType, string strObjectType, BOOL bThrowError)
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
	else if(strClassType == "RIGIDBODY")
		lpObject = CreateRigidBody(strObjectType, bThrowError);
	else if(strClassType == "JOINT")
		lpObject = CreateJoint(strObjectType, bThrowError);
	else if(strClassType == "EXTERNALSTIMULUS")
		lpObject = CreateExternalStimulus(strObjectType, bThrowError);
	else
	{
		lpObject = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Std_Err_lInvalidClassType, Std_Err_strInvalidClassType, "ClassType", strClassType);
	}

	return lpObject;
}

// ************* IStdClassFactory functions ******************************


}			//[*PROJECT_NAME*]

extern "C" __declspec(dllexport) IStdClassFactory* __cdecl GetStdClassFactory() 
{
	IStdClassFactory *lpFactory = new ClassFactory;
	return lpFactory;
}

