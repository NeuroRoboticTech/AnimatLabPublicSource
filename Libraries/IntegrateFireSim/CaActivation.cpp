/**
\file	CaActivation.cpp

\brief	Implements the ca activation class.
**/

#include "stdafx.h"
#include "IonChannel.h"
#include "SynapseType.h"
#include "IntegrateFireModule.h"
#include "Connexion.h"
#include "CaActivation.h"
#include "Neuron.h"
#include "ElectricalSynapse.h"
#include "NonSpikingChemicalSynapse.h"
#include "SpikingChemicalSynapse.h"

namespace IntegrateFireSim
{

CaActivation::CaActivation(Neuron *lpParent, string strActivationType)
{
	if(!lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	m_lpParent = lpParent;
	m_strActivationType = Std_ToUpper(Std_Trim(strActivationType));
}

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
CaActivation::~CaActivation()
{
}

BOOL CaActivation::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);
			
	if(AnimatBase::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "MIDPOINT")
	{
		if(m_strActivationType == "ACTIVE")
			m_lpParent->BurstVm(atof(strValue.c_str()));
		else
			m_lpParent->BurstVh(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "SLOPE")
	{
		if(m_strActivationType == "ACTIVE")
			m_lpParent->BurstSm(atof(strValue.c_str()));
		else
			m_lpParent->BurstSh(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "TIMECONSTANT")
	{
		if(m_strActivationType == "ACTIVE")
			m_lpParent->BurstMTimeConstant(atof(strValue.c_str()));
		else
			m_lpParent->BurstHTimeConstant(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void CaActivation::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("Midpoint");
	aryTypes.Add("Float");

	aryNames.Add("Slope");
	aryTypes.Add("Float");

	aryNames.Add("TimeConstant");
	aryTypes.Add("Float");
}

void CaActivation::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();

	float fltVal = oXml.GetChildFloat("MidPoint");
	if(m_strActivationType == "ACTIVE")
		m_lpParent->BurstVm(fltVal);
	else
		m_lpParent->BurstVh(fltVal);

	fltVal = oXml.GetChildFloat("Slope");
	if(m_strActivationType == "ACTIVE")
		m_lpParent->BurstSm(fltVal);
	else
		m_lpParent->BurstSh(fltVal);


	fltVal = oXml.GetChildFloat("TimeConstant");
	if(m_strActivationType == "ACTIVE")
		m_lpParent->BurstMTimeConstant(fltVal);
	else
		m_lpParent->BurstHTimeConstant(fltVal);

	oXml.OutOfElem();
}

//Node Overrides

}				//IntegrateFireSim
