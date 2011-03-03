// GatedSynapse.cpp: implementation of the GatedSynapse class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "Synapse.h"
#include "GatedSynapse.h"
#include "Neuron.h" 
#include "FiringRateModule.h"

namespace FiringRateSim
{
	namespace Synapses
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

GatedSynapse::GatedSynapse()
{
	m_iInitialGateValue=0;
	m_strType = "GATED";
}

GatedSynapse::~GatedSynapse()
{

}

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

float *GatedSynapse::GetDataPointer(string strDataType)
{
	float *fltVal = Synapse::GetDataPointer(strDataType);
	if(fltVal) return fltVal;

	string strType = Std_CheckString(strDataType);

	if(strType == "MODULATION")
		return &m_fltModulation;

	return NULL;
}
BOOL GatedSynapse::SetData(string strDataType, string strValue, BOOL bThrowError)
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

#pragma endregion

void GatedSynapse::Load(CStdXml &oXml)
{
	Synapse::Load(oXml);

	oXml.IntoElem();  //Into GatedSynapse Element

	m_iInitialGateValue = (unsigned char) oXml.GetChildInt("InitialGateValue");

	oXml.OutOfElem(); //OutOf GatedSynapse Element
}

	}			//Synapses
}				//FiringRateSim






