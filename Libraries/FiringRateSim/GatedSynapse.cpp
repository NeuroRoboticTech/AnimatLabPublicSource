/**
\file	GatedSynapse.cpp

\brief	Implements the gated synapse class.
**/

#include "stdafx.h"

#include "Synapse.h"
#include "GatedSynapse.h"
#include "Neuron.h" 
#include "FiringRateModule.h"

namespace FiringRateSim
{
	namespace Synapses
	{
/**
\brief	Default constructor.

\author	dcofer
\date	3/30/2011
**/
GatedSynapse::GatedSynapse()
{
	m_iInitialGateValue=0;
	m_strType = "GATED";
}

/**
\brief	Destructor.

\author	dcofer
\date	3/30/2011
**/
GatedSynapse::~GatedSynapse()
{

}

/**
\brief	Gets the initial gate value.

\author	dcofer
\date	3/30/2011

\return	initial gate value.
**/
unsigned char GatedSynapse::InitialGateValue() {return m_iInitialGateValue;}

/**
\brief	Sets the initial gate value.

\author	dcofer
\date	3/30/2011

\param	iVal	The new value. 
**/
void GatedSynapse::InitialGateValue(unsigned char iVal) {m_iInitialGateValue = iVal;}

float GatedSynapse::CalculateModulation(FiringRateModule *lpModule)
{
	m_fltModulation=0;

	if(m_bEnabled && m_lpFromNeuron)
		m_fltModulation = m_iInitialGateValue + (m_lpFromNeuron->FiringFreq(lpModule) * m_fltWeight);
	else
		m_fltModulation = 1;

	return m_fltModulation;
}

#pragma region DataAccesMethods

float *GatedSynapse::GetDataPointer(const string &strDataType)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "MODULATION")
		return &m_fltModulation;

	return Synapse::GetDataPointer(strDataType);
}

BOOL GatedSynapse::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Synapse::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "GATEINITIALLYON")
	{
		InitialGateValue(Std_ToBool(strValue));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void GatedSynapse::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	Synapse::QueryProperties(aryNames, aryTypes);

	aryNames.Add("GateInitiallyOn");
	aryTypes.Add("Boolean");
}

#pragma endregion

void GatedSynapse::Load(CStdXml &oXml)
{
	Synapse::Load(oXml);

	oXml.IntoElem();  //Into GatedSynapse Element

	InitialGateValue((unsigned char) oXml.GetChildInt("InitialGateValue"));

	oXml.OutOfElem(); //OutOf GatedSynapse Element
}

	}			//Synapses
}				//FiringRateSim






