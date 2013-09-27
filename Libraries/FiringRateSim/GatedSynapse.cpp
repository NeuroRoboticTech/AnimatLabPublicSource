/**
\file	GatedSynapse.cpp

\brief	Implements the gated synapse class.
**/

#include "StdAfx.h"

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

float *GatedSynapse::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "MODULATION")
		return &m_fltModulation;

	return Synapse::GetDataPointer(strDataType);
}

bool GatedSynapse::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(Synapse::SetData(strDataType, strValue, false))
		return true;

	if(strType == "GATEINITIALLYON")
	{
		InitialGateValue(Std_ToBool(strValue));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void GatedSynapse::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
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






