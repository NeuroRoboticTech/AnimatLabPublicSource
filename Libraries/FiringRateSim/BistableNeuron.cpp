/**
\file	BistableNeuron.cpp

\brief	Implements the bistable neuron class.
**/

#include "StdAfx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "BistableNeuron.h"
#include "FiringRateModule.h"

namespace FiringRateSim
{
	namespace Neurons
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/30/2011
**/
BistableNeuron::BistableNeuron()
{
	m_fltIntrinsic=0;
	m_fltVsth = 0.010f;
	m_fltIl=0;
	m_fltIh = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/30/2011
**/
BistableNeuron::~BistableNeuron()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of BistableNeuron\r\n", "", -1, false, true);}
}

/**
\brief	Gets the intrinsic current.

\author	dcofer
\date	3/30/2011

\return	intrinsic current.
**/
float BistableNeuron::IntrinsicCurrent()
{return m_fltIntrinsic;}

/**
\brief	Sets the intrinsic current.

\author	dcofer
\date	3/30/2011

\param	fltVal	The new value. 
**/
void BistableNeuron::IntrinsicCurrent(float fltVal)
{m_fltIntrinsic=fltVal;}

/**
\brief	Gets the low current.

\author	dcofer
\date	3/30/2011

\return	low current.
**/
float BistableNeuron::Il()
{return m_fltIl;}

/**
\brief	Sets the low current

\author	dcofer
\date	3/30/2011

\param	fltVal	The new value. 
**/
void BistableNeuron::Il(float fltVal)
{m_fltIl=fltVal;}

/**
\brief	Gets the high current.

\author	dcofer
\date	3/30/2011

\return	The high current.
**/
float BistableNeuron::Ih()
{return m_fltIh;}

/**
\brief	Sets the high current.

\author	dcofer
\date	3/30/2011

\param	fltVal	The new value. 
**/
void BistableNeuron::Ih(float fltVal)
{m_fltIh=fltVal;}

/**
\brief	Gets the threshold voltage.

\author	dcofer
\date	3/30/2011

\return	threshold voltage.
**/
float BistableNeuron::Vsth()
{return m_fltVsth;}

/**
\brief	Sets the threshold voltage.

\author	dcofer
\date	3/30/2011

\param	fltVal	The new value. 
**/
void BistableNeuron::Vsth(float fltVal)
{m_fltVsth=fltVal;}

/**
\brief	Gets the neuron type.

\author	dcofer
\date	3/30/2011

\return	neuron type.
**/
unsigned char BistableNeuron::NeuronType()
{return BISTABLE_NEURON;}

float BistableNeuron::CalculateIntrinsicCurrent(FiringRateModule *lpModule, float fltInputCurrent)
{
	if(m_fltVn>=m_fltVsth)
		m_fltIntrinsic=m_fltIh;
	else
		m_fltIntrinsic=m_fltIl;

	return m_fltIntrinsic;
}

void BistableNeuron::ResetSimulation()
{
	Neuron::ResetSimulation();

	m_fltIntrinsic=0;
}

#pragma region DataAccesMethods

bool BistableNeuron::SetData(const string &strDataType, const string &strValue, bool bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Neuron::SetData(strDataType, strValue, false))
		return true;

	if(strType == "VSTH")
	{
		Vsth(atof(strValue.c_str()));
		return true;
	}

	if(strType == "IL")
	{
		Il(atof(strValue.c_str()));
		return true;
	}

	if(strType == "IH")
	{
		Ih(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void BistableNeuron::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	Neuron::QueryProperties(aryNames, aryTypes);

	aryNames.Add("Vsth");
	aryTypes.Add("Float");

	aryNames.Add("Il");
	aryTypes.Add("Float");

	aryNames.Add("Ih");
	aryTypes.Add("Float");
}

#pragma endregion

void BistableNeuron::Load(CStdXml &oXml)
{
	Neuron::Load(oXml);

	oXml.IntoElem();  //Into Neuron Element

	Vsth(oXml.GetChildFloat("Vsth"));
	Il(oXml.GetChildFloat("Il"));
	Ih(oXml.GetChildFloat("Ih"));

	oXml.OutOfElem(); //OutOf Neuron Element
}

	}			//Neurons
}				//FiringRateSim

