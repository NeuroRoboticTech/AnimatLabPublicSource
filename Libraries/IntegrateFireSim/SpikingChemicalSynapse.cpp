/**
\file SpikingChemicalSynapse.cpp

\brief	Implements the spiking chemical synapse class.
**/

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

/**
\brief	Default constructor.

\author	dcofer
\date	3/31/2011
**/
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

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
SpikingChemicalSynapse::~SpikingChemicalSynapse()
{

}

#pragma region Accessor-Mutators

/**
\brief	Sets the equilibrium potential.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void SpikingChemicalSynapse::EquilibriumPotential(double dVal) {m_dEquil = dVal;}

/**
\brief	Gets the equilibrium potential.

\author	dcofer
\date	3/31/2011

\return	equilibrium potential.
**/
double SpikingChemicalSynapse::EquilibriumPotential() {return m_dEquil;}

/**
\brief	Sets the synaptic conductance.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void SpikingChemicalSynapse::SynapticConductance(double dVal) {m_dSynAmp = dVal;}

/**
\brief	Gets the synaptic conductance.

\author	dcofer
\date	3/31/2011

\return	synaptic conductance.
**/
double SpikingChemicalSynapse::SynapticConductance() {return m_dSynAmp;}

/**
\brief	Sets the decay rate.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void SpikingChemicalSynapse::DecayRate(double dVal) {m_dDecay = dVal;}

/**
\brief	Gets the decay rate.

\author	dcofer
\date	3/31/2011

\return	decay rate.
**/
double SpikingChemicalSynapse::DecayRate() {return m_dDecay;}

/**
\brief	Gets the facilitation decrement value.

\author	dcofer
\date	3/31/2011

\return	decrement.
**/
double SpikingChemicalSynapse::FacilD() {return m_dFacilD;}

/**
\brief	Sets the relative facilitation.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void SpikingChemicalSynapse::RelativeFacilitation(double dVal) {m_dRelFacil = dVal;}

/**
\brief	Gets the relative facilitation.

\author	dcofer
\date	3/31/2011

\return	relative facilitation.
**/
double SpikingChemicalSynapse::RelativeFacilitation() {return m_dRelFacil;}

/**
\brief	Gets the relative facilitation.

\author	dcofer
\date	3/31/2011

\return	relative facilitation.
**/
double SpikingChemicalSynapse::RelFacil() {return m_dRelFacil;}

/**
\brief	Sets the facilitation decay.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void SpikingChemicalSynapse::FacilitationDecay(double dVal) 
{
	Std_IsAboveMin((double) 0, dVal, TRUE, "FacilitationDecay");
	m_dFacilDecay = dVal;
	m_dFacilD = exp(-m_lpModule->TimeStep()/m_dFacilDecay);
}

/**
\brief	Gets the facilitation decay.

\author	dcofer
\date	3/31/2011

\return	facilitation decay.
**/
double SpikingChemicalSynapse::FacilitationDecay() {return m_dFacilDecay;}

/**
\brief	Gets the facilitation decay.

\author	dcofer
\date	3/31/2011

\return	facilitation decay.
**/
double SpikingChemicalSynapse::FacilDecay() {return m_dFacilDecay;}

/**
\brief	Sets whether this synapse is voltage dependent.

\author	dcofer
\date	3/31/2011

\param	bVal	true if voltage dependent. 
**/
void SpikingChemicalSynapse::VoltageDependent(BOOL bVal) {m_bVoltDep = bVal;}

/**
\brief	Gets whether this synapse is voltage dependent.

\author	dcofer
\date	3/31/2011

\return	true if voltage dependent, false else.
**/
BOOL SpikingChemicalSynapse::VoltageDependent() {return m_bVoltDep;}

/**
\brief	Gets whether this synapse is voltage dependent.

\author	dcofer
\date	3/31/2011

\return	true if voltage dependent, false else.
**/
BOOL SpikingChemicalSynapse::VoltDep() {return m_bVoltDep;}

/**
\brief	Sets the maximum relative conductance.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void SpikingChemicalSynapse::MaxRelativeConductance(double dVal) {m_dMaxRelCond = dVal;}

/**
\brief	Gets the maximum relative conductance.

\author	dcofer
\date	3/31/2011

\return	maximum relative conductance.
**/
double SpikingChemicalSynapse::MaxRelativeConductance() {return m_dMaxRelCond;}

/**
\brief	Gets the maximum relative conductance.

\author	dcofer
\date	3/31/2011

\return	maximum relative conductance.
**/
double SpikingChemicalSynapse::MaxGVoltDepRel() {return m_dMaxRelCond;}

/**
\brief	Sets the saturation potential.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void SpikingChemicalSynapse::SaturatePotential(double dVal) {m_dSatPSPot = dVal;}

/**
\brief	Gets the saturation potential.

\author	dcofer
\date	3/31/2011

\return	saturation potential.
**/
double SpikingChemicalSynapse::SaturatePotential() {return m_dSatPSPot;}

/**
\brief	Gets the saturation potential.

\author	dcofer
\date	3/31/2011

\return	saturation potential.
**/
double SpikingChemicalSynapse::SatPSPot() {return m_dSatPSPot;}

/**
\brief	Sets the threshold potential.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void SpikingChemicalSynapse::ThresholdPotential(double dVal) {m_dThreshPSPot = dVal;}

/**
\brief	Gets the threshold potential.

\author	dcofer
\date	3/31/2011

\return	threshold potential.
**/
double SpikingChemicalSynapse::ThresholdPotential() {return m_dThreshPSPot;}

/**
\brief	Gets the threshold potential.

\author	dcofer
\date	3/31/2011

\return	threshold potential.
**/
double SpikingChemicalSynapse::ThreshPSPot() {return m_dThreshPSPot;}

/**
\brief	Sets whether this synapse uses Hebbian learning.

\author	dcofer
\date	3/31/2011

\param	bVal	true to use Hebbian learning. 
**/
void SpikingChemicalSynapse::Hebbian(BOOL bVal) {m_bHebbian = bVal;}

/**
\brief	Gets whether this synapse uses Hebbian learning.

\author	dcofer
\date	3/31/2011

\return	true if it uses Hebbian learning, false else.
**/
BOOL SpikingChemicalSynapse::Hebbian() {return m_bHebbian;}

/**
\brief	Sets the maximum augmented conductance.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void SpikingChemicalSynapse::MaxAugmentedConductance(double dVal) {m_dMaxAugCond = dVal;}

/**
\brief	Gets the maximum augmented conductance.

\author	dcofer
\date	3/31/2011

\return	conductance.
**/
double SpikingChemicalSynapse::MaxAugmentedConductance() {return m_dMaxAugCond;}

/**
\brief	Gets the maximum augmented conductance.

\author	dcofer
\date	3/31/2011

\return	conductance.
**/
double SpikingChemicalSynapse::MaxGHebb() {return m_dMaxAugCond;}

/**
\brief	Sets the learning increment.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void SpikingChemicalSynapse::LearningIncrement(double dVal) {m_dLearningInc = dVal;}

/**
\brief	Gets the learning increment.

\author	dcofer
\date	3/31/2011

\return	learning increment.
**/
double SpikingChemicalSynapse::LearningIncrement() {return m_dLearningInc;}

/**
\brief	Gets the hebbian increment.

\author	dcofer
\date	3/31/2011

\return	increment.
**/
double SpikingChemicalSynapse::HebbIncrement() {return m_dLearningInc;}

/**
\brief	Sets the learning time window.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void SpikingChemicalSynapse::LearningTimeWindow(double dVal) {m_dLearningTime = dVal;}

/**
\brief	Gets the learning time window.

\author	dcofer
\date	3/31/2011

\return	window.
**/
double SpikingChemicalSynapse::LearningTimeWindow() {return m_dLearningTime;}

/**
\brief	Gets the learning time window.

\author	dcofer
\date	3/31/2011

\return	window.
**/
double SpikingChemicalSynapse::HebbTimeWindow() {return m_dLearningTime;}

/**
\brief	Sets whether forgetting is allowed.

\author	dcofer
\date	3/31/2011

\param	bVal	true to allow forgetting. 
**/
void SpikingChemicalSynapse::AllowForgetting(BOOL bVal) {m_bAllowForget = bVal;}

/**
\brief	Gets whether forgetting is allowed.

\author	dcofer
\date	3/31/2011

\return	true if forgetting allowed, false else.
**/
BOOL SpikingChemicalSynapse::AllowForgetting() {return m_bAllowForget;}

/**
\brief	Gets whether forgetting is allowed.

\author	dcofer
\date	3/31/2011

\return	true if forgetting allowed, false else.
**/
BOOL SpikingChemicalSynapse::AllowForget() {return m_bAllowForget;}

/**
\brief	Sets the forgetting time window.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void SpikingChemicalSynapse::ForgettingTimeWindow(double dVal) {m_dForgetTime = dVal;}

/**
\brief	Gets the forgetting time window.

\author	dcofer
\date	3/31/2011

\return	window.
**/
double SpikingChemicalSynapse::ForgettingTimeWindow() {return m_dForgetTime;}

/**
\brief	Gets the forgetting time window.

\author	dcofer
\date	3/31/2011

\return	window.
**/
double SpikingChemicalSynapse::ForgettingWindow() {return m_dForgetTime;}

/**
\brief	Sets the consolidation factor.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void SpikingChemicalSynapse::ConsolidationFactor(double dVal) {m_dConsolidation = dVal;}

/**
\brief	Gets the consolidation factor.

\author	dcofer
\date	3/31/2011

\return	factor.
**/
double SpikingChemicalSynapse::ConsolidationFactor() {return m_dConsolidation;}

/**
\brief	Gets the consolidation factor.

\author	dcofer
\date	3/31/2011

\return	factor.
**/
double SpikingChemicalSynapse::Consolidation() {return m_dConsolidation;}

#pragma endregion

#pragma region DataAccesMethods


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
