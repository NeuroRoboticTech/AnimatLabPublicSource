// TonicNeuron.cpp: implementation of the TonicNeuron class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "TonicNeuron.h"
#include "FiringRateModule.h"

namespace FiringRateSim
{
	namespace Neurons
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

TonicNeuron::TonicNeuron()
{
	m_fltIh=0;
}

TonicNeuron::~TonicNeuron()
{

}


float TonicNeuron::Ih()
{return m_fltIh;}

void TonicNeuron::Ih(float fltVal)
{m_fltIh=fltVal;}

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

#pragma endregion

void TonicNeuron::Load(CStdXml &oXml)
{
	Neuron::Load(oXml);

	oXml.IntoElem();  //Into Neuron Element

	m_fltIh = oXml.GetChildFloat("Ih");

	oXml.OutOfElem(); //OutOf Neuron Element
}

	}			//Neurons
}				//FiringRateSim

