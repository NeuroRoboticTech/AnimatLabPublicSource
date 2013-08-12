/**
\file ElectricalSynapse.cpp

\brief	Implements the electrical synapse class.
**/

#include "stdafx.h"
#include "SynapseType.h"
#include "ElectricalSynapse.h"

namespace IntegrateFireSim
{
	namespace Synapses
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/31/2011
**/
ElectricalSynapse::ElectricalSynapse() : SynapseType()
{
	m_dLowCoup = 0;
	m_dHiCoup = 0;
	m_dTurnOnV = 0;
	m_dSaturateV = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
ElectricalSynapse::~ElectricalSynapse()
{

}

#pragma region Accessor-Mutators

/**
\brief	Sets the low coupling voltage.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void ElectricalSynapse::LowCoupling(double dVal) {m_dLowCoup = dVal;}

/**
\brief	Gets the low coupling voltage.

\author	dcofer
\date	3/31/2011

\return	low coupling voltage.
**/
double ElectricalSynapse::LowCoupling() {return m_dLowCoup;}

/**
\brief	Sets the high coupling voltage.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void ElectricalSynapse::HighCoupling(double dVal) {m_dHiCoup = dVal;}

/**
\brief	Gets the high coupling voltage.

\author	dcofer
\date	3/31/2011

\return	high coupling voltage.
**/
double ElectricalSynapse::HighCoupling() {return m_dHiCoup;}

/**
\brief	Sets the turn-on threshold voltage.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void ElectricalSynapse::TurnOnThreshold(double dVal) {m_dTurnOnV = dVal;}

/**
\brief	Gets the turn-on threshold voltage.

\author	dcofer
\date	3/31/2011

\return	threshold voltage.
**/
double ElectricalSynapse::TurnOnThreshold() {return m_dTurnOnV;}

/**
\brief	Sets the turn-on saturation voltage.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void ElectricalSynapse::TurnOnSaturate(double dVal) {m_dSaturateV = dVal;}

/**
\brief	Gets the turn-on saturation voltage.

\author	dcofer
\date	3/31/2011

\return	saturation voltage.
**/
double ElectricalSynapse::TurnOnSaturate() {return m_dSaturateV;}

#pragma endregion

#pragma region DataAccesMethods

bool ElectricalSynapse::SetData(const string &strDataType, const string &strValue, bool bThrowError)
{
	string strType = Std_CheckString(strDataType);
			
	if(AnimatBase::SetData(strDataType, strValue, false))
		return true;

	if(strType == "LOWCOUPLING")
	{
		LowCoupling(atof(strValue.c_str()));
		return true;
	}

	if(strType == "HIGHCOUPLING")
	{
		HighCoupling(atof(strValue.c_str()));
		return true;
	}

	if(strType == "TURNONTHRESHOLD")
	{
		TurnOnThreshold(atof(strValue.c_str()));
		return true;
	}

	if(strType == "TURNONSATURATE")
	{
		TurnOnSaturate(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void ElectricalSynapse::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	AnimatBase::QueryProperties(aryNames, aryTypes);

	aryNames.Add("LowCoupling");
	aryTypes.Add("Float");

	aryNames.Add("HighCoupling");
	aryTypes.Add("Float");

	aryNames.Add("TurnOnThreshold");
	aryTypes.Add("Float");

	aryNames.Add("TurnOnSaturate");
	aryTypes.Add("Float");
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