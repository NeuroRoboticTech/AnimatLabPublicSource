/**
\file	TonicNeuron.cpp

\brief	Implements the tonic neuron class.
**/

#include "StdAfx.h"

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
{
	m_fltIh=fltVal;
}

/**
\brief	Gets the neuron type.

\author	dcofer
\date	3/29/2011

\return	neuron type.
**/
unsigned char TonicNeuron::NeuronType()
{return TONIC_NEURON;}

void TonicNeuron::Copy(CStdSerialize *lpSource)
{
	Neuron::Copy(lpSource);

	TonicNeuron *lpOrig = dynamic_cast<TonicNeuron *>(lpSource);

	m_fltIh = lpOrig->m_fltIh;
}

float TonicNeuron::CalculateIntrinsicCurrent(FiringRateModule *lpModule, float fltInputCurrent)
{return m_fltIh;}

#pragma region DataAccesMethods

bool TonicNeuron::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(Neuron::SetData(strDataType, strValue, false))
		return true;

	if(strType == "IH")
	{
		Ih(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void TonicNeuron::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Neuron::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Ih", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
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

