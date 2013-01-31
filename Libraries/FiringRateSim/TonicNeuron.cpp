/**
\file	TonicNeuron.cpp

\brief	Implements the tonic neuron class.
**/

#include "stdafx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "TonicNeuron.h"
#include "FiringRateModule.h"

namespace FiringRateSim
{
	namespace Neurons
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
TonicNeuron::TonicNeuron()
{
	m_fltIh=0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
TonicNeuron::~TonicNeuron()
{

}

/**
\brief	Gets the tonic current.

\author	dcofer
\date	3/29/2011

\return	tonic current.
**/
float TonicNeuron::Ih()
{return m_fltIh;}

/**
\brief	Sets the tonic current.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void TonicNeuron::Ih(float fltVal)
{m_fltIh=fltVal;}

/**
\brief	Gets the neuron type.

\author	dcofer
\date	3/29/2011

\return	neuron type.
**/
unsigned char TonicNeuron::NeuronType()
{return TONIC_NEURON;}

float TonicNeuron::CalculateIntrinsicCurrent(FiringRateModule *lpModule, float fltInputCurrent)
{return m_fltIh;}

#pragma region DataAccesMethods

BOOL TonicNeuron::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Neuron::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "IH")
	{
		Ih(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void TonicNeuron::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	Neuron::QueryProperties(aryNames, aryTypes);

	aryNames.Add("Ih");
	aryTypes.Add("Float");
}

#pragma endregion

void TonicNeuron::Load(CStdXml &oXml)
{
	Neuron::Load(oXml);

	oXml.IntoElem();  //Into Neuron Element

	Ih(oXml.GetChildFloat("Ih"));

	oXml.OutOfElem(); //OutOf Neuron Element
}

	}			//Neurons
}				//FiringRateSim

