// NonSpikingChemSyn.cpp: implementation of the NonSpikingChemicalSynapse class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "SynapseType.h"
#include "NonSpikingChemicalSynapse.h"

namespace IntegrateFireSim
{
	namespace Synapses
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

NonSpikingChemicalSynapse::NonSpikingChemicalSynapse() : SynapseType()
{
	m_dEquil = 0;
	m_dSynAmp = 0;
	m_dThreshV = 0;
	m_dSaturateV = 0;
}

NonSpikingChemicalSynapse::~NonSpikingChemicalSynapse()
{

}

#pragma region DataAccesMethods

BOOL NonSpikingChemicalSynapse::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);
	
	if(strType == "EQUILIBRIUMPOTENTIAL")
	{
		EquilibriumPotential(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "MAXSYNAPTICCONDUCTANCE")
	{
		MaxSynapticConductance(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "PRESYNAPTICTHRESHOLD")
	{
		PreSynapticThreshold(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "PRESYNAPTICSATURATIONLEVEL")
	{
		PreSynapticSaturationLevel(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}
#pragma endregion

void NonSpikingChemicalSynapse::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into SpikingChemSyn Element

	EquilibriumPotential(oXml.GetChildDouble("Equil"));
	MaxSynapticConductance(oXml.GetChildDouble("SynAmp"));
	PreSynapticThreshold(oXml.GetChildDouble("ThreshV"));
	PreSynapticSaturationLevel(oXml.GetChildDouble("SaturateV"));

	oXml.OutOfElem(); //OutOf Neuron Element
}

	}			//Synapses
}				//IntegrateFireSim