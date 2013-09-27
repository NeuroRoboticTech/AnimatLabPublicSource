/**
\file NonSpikingChemicalSynapse.cpp

\brief	Implements the non spiking chemical synapse class.
**/

#include "stdafx.h"
#include "SynapseType.h"
#include "NonSpikingChemicalSynapse.h"

namespace IntegrateFireSim
{
	namespace Synapses
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/31/2011
**/
NonSpikingChemicalSynapse::NonSpikingChemicalSynapse() : SynapseType()
{
	m_dEquil = 0;
	m_dSynAmp = 0;
	m_dThreshV = 0;
	m_dSaturateV = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
NonSpikingChemicalSynapse::~NonSpikingChemicalSynapse()
{

}

#pragma region Accessor-Mutators

/**
\brief	Sets the equilibrium potential.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void NonSpikingChemicalSynapse::EquilibriumPotential(double dVal) {m_dEquil = dVal;}

/**
\brief	Gets the equilibrium potential.

\author	dcofer
\date	3/31/2011

\return	equilibrium potential.
**/
double NonSpikingChemicalSynapse::EquilibriumPotential() {return m_dEquil;}

/**
\brief	Sets the maximum synaptic conductance.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void NonSpikingChemicalSynapse::MaxSynapticConductance(double dVal) {m_dSynAmp = dVal;}

/**
\brief	Gets the maximum synaptic conductance.

\author	dcofer
\date	3/31/2011

\return	synaptic conductance.
**/
double NonSpikingChemicalSynapse::MaxSynapticConductance() {return m_dSynAmp;}

/**
\brief	Sets the pre-synaptic threshold.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void NonSpikingChemicalSynapse::PreSynapticThreshold(double dVal) {m_dThreshV = dVal;}

/**
\brief	Gets the pre-synaptic threshold.

\author	dcofer
\date	3/31/2011

\return	pre-synaptic threshold.
**/
double NonSpikingChemicalSynapse::PreSynapticThreshold() {return m_dThreshV;}

/**
\brief	Sets the pre-synaptic saturation level.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void NonSpikingChemicalSynapse::PreSynapticSaturationLevel(double dVal) {m_dSaturateV = dVal;}

/**
\brief	Gets the pre-synaptic saturation level.

\author	dcofer
\date	3/31/2011

\return	saturation level.
**/
double NonSpikingChemicalSynapse::PreSynapticSaturationLevel() {return m_dSaturateV;}

#pragma endregion

#pragma region DataAccesMethods

bool NonSpikingChemicalSynapse::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
			
	if(AnimatBase::SetData(strDataType, strValue, false))
		return true;

	if(strType == "EQUILIBRIUMPOTENTIAL")
	{
		EquilibriumPotential(atof(strValue.c_str()));
		return true;
	}

	if(strType == "MAXSYNAPTICCONDUCTANCE")
	{
		MaxSynapticConductance(atof(strValue.c_str()));
		return true;
	}

	if(strType == "PRESYNAPTICTHRESHOLD")
	{
		PreSynapticThreshold(atof(strValue.c_str()));
		return true;
	}

	if(strType == "PRESYNAPTICSATURATIONLEVEL")
	{
		PreSynapticSaturationLevel(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void NonSpikingChemicalSynapse::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("EquilibriumPotential");
	aryTypes.Add("Float");

	aryNames.Add("MaxSynapticConductance");
	aryTypes.Add("Float");

	aryNames.Add("PreSynapticThreshold");
	aryTypes.Add("Float");

	aryNames.Add("PreSynapticSaturationLevel");
	aryTypes.Add("Float");
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