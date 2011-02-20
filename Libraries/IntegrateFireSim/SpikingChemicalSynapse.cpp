// SpikingChemSyn.cpp: implementation of the SpikingChemicalSynapse class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IonChannel.h"
#include "SynapseType.h"
#include "Connexion.h"
#include "CaActivation.h"
#include "Neuron.h"
#include "ElectricalSynapse.h"
#include "NonSpikingChemicalSynapse.h"
#include "SpikingChemicalSynapse.h"
#include "IntegrateFireModule.h"
#include "ClassFactory.h"
#include <time.h>

namespace IntegrateFireSim
{
	namespace Synapses
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

SpikingChemicalSynapse::SpikingChemicalSynapse() : SynapseType()
{
	m_dEquil = 0;
	m_dSynAmp = 0;	
	m_dDecay = 0;
	m_dFacilD = 0;
	m_dRelFacil = 0;
	m_dFacilDecay = 0;

	m_bVoltDep = FALSE;
	m_dMaxRelCond = 0;
	m_dSatPSPot = 0;
	m_dThreshPSPot = 0;

	m_bHebbian = FALSE;
	m_dMaxAugCond = 0;
	m_dLearningInc = 0;
	m_dLearningTime = 0;
	m_bAllowForget = FALSE;
	m_dForgetTime = 0;
	m_dConsolidation = 0;
}

SpikingChemicalSynapse::~SpikingChemicalSynapse()
{

}

#pragma region DataAccesMethods

void SpikingChemicalSynapse::FacilitationDecay(double dVal) 
{
	m_dFacilDecay = dVal;
	m_dFacilD = exp(-m_lpModule->TimeStep()/m_dFacilDecay);
}


BOOL SpikingChemicalSynapse::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);
	

	if(strType == "EQUILIBRIUMPOTENTIAL")
	{
		EquilibriumPotential(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "SYNAPTICCONDUCTANCE")
	{
		SynapticConductance(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "DECAYRATE")
	{
		DecayRate(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "RELATIVEFACILITATION")
	{
		RelativeFacilitation(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "FACILITATIONDECAY")
	{
		FacilitationDecay(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "VOLTAGEDEPENDENT")
	{
		VoltageDependent(Std_ToBool(strValue));
		return TRUE;
	}

	if(strType == "MAXRELATIVECONDUCTANCE")
	{
		MaxRelativeConductance(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "SATURATEPOTENTIAL")
	{
		SaturatePotential(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "THRESHOLDPOTENTIAL")
	{
		ThresholdPotential(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "HEBBIAN")
	{
		Hebbian(Std_ToBool(strValue));
		return TRUE;
	}

	if(strType == "MAXAUGMENTEDCONDUCTANCE")
	{
		MaxAugmentedConductance(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "LEARNINGINCREMENT")
	{
		LearningIncrement(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "LEARNINGTIMEWINDOW")
	{
		LearningTimeWindow(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "ALLOWFORGETTING")
	{
		AllowForgetting(Std_ToBool(strValue));
		return TRUE;
	}

	if(strType == "FORGETTINGTIMEWINDOW")
	{
		ForgettingTimeWindow(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "CONSOLIDATIONFACTOR")
	{
		ConsolidationFactor(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}
#pragma endregion

void SpikingChemicalSynapse::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into SpikingChemSyn Element
	
	EquilibriumPotential(oXml.GetChildDouble("Equil"));
	SynapticConductance(oXml.GetChildDouble("SynAmp"));
	DecayRate(oXml.GetChildDouble("Decay"));
	RelativeFacilitation(oXml.GetChildDouble("RelFacil"));
	FacilitationDecay(oXml.GetChildDouble("FacilDecay"));

	VoltageDependent(oXml.GetChildBool("VoltDep"));
	MaxRelativeConductance(oXml.GetChildDouble("MaxRelCond"));
	SaturatePotential(oXml.GetChildDouble("SatPSPot"));
	ThresholdPotential(oXml.GetChildDouble("ThreshPSPot"));

	Hebbian(oXml.GetChildBool("Hebbian"));
	MaxAugmentedConductance(oXml.GetChildDouble("MaxAugCond"));
	LearningIncrement(oXml.GetChildDouble("LearningInc"));
	LearningTimeWindow(oXml.GetChildDouble("LearningTime"));
	AllowForgetting(oXml.GetChildBool("AllowForget"));
	ForgettingTimeWindow(oXml.GetChildDouble("ForgetTime"));
	ConsolidationFactor(oXml.GetChildDouble("Consolidation"));

	oXml.OutOfElem(); //OutOf Neuron Element
}

	}			//Synapses
}				//IntegrateFireSim
