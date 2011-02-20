// ElecSyn.cpp: implementation of the ElectricalSynapse class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "SynapseType.h"
#include "ElectricalSynapse.h"

namespace IntegrateFireSim
{
	namespace Synapses
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

ElectricalSynapse::ElectricalSynapse() : SynapseType()
{
	m_dLowCoup = 0;
	m_dHiCoup = 0;
	m_dTurnOnV = 0;
	m_dSaturateV = 0;
}

ElectricalSynapse::~ElectricalSynapse()
{

}

#pragma region DataAccesMethods

BOOL ElectricalSynapse::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);
	
	if(strType == "LOWCOUPLING")
	{
		LowCoupling(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "HIGHCOUPLING")
	{
		HighCoupling(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "TURNONTHRESHOLD")
	{
		TurnOnThreshold(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "TURNONSATURATE")
	{
		TurnOnSaturate(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}
#pragma endregion

void ElectricalSynapse::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into SpikingChemSyn Element

	LowCoupling(oXml.GetChildDouble("LowCoup"));
	HighCoupling(oXml.GetChildDouble("HiCoup"));
	TurnOnThreshold(oXml.GetChildDouble("TurnOnV"));
	TurnOnSaturate(oXml.GetChildDouble("SaturateV"));

	oXml.OutOfElem(); //OutOf Neuron Element
}

	}			//Synapses
}				//IntegrateFireSim