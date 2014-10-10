/**
\file	ModulateNeuronPropSynapse.cpp

\brief	Implements the neuron property modulatory synapse class.
**/

#include "StdAfx.h"

#include "Synapse.h"
#include "ModulateNeuronPropSynapse.h"
#include "Neuron.h"
#include "FiringRateModule.h"

namespace FiringRateSim
{
	namespace Synapses
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
ModulateNeuronPropSynapse::ModulateNeuronPropSynapse()
{
	m_lpGain = NULL;
	m_lpPropertyData = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
ModulateNeuronPropSynapse::~ModulateNeuronPropSynapse()
{

try
{
	if(m_lpGain) delete m_lpGain;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ModulateNeuronPropSynapse\r\n", "", -1, false, true);}
}

/**
\brief	Sets the current distribution.

\author	dcofer
\date	3/29/2011

\param [in,out]	lpGain	Pointer to a gain. 
**/
void ModulateNeuronPropSynapse::ModulationGain(AnimatSim::Gains::Gain *lpGain)
{
	if(lpGain)
	{
		if(m_lpGain) 
			{delete m_lpGain; m_lpGain = NULL;}
		m_lpGain = lpGain;
	}
}

/**
\brief	Sets the current distribution using an xml packet.

\author	dcofer
\date	3/29/2011

\param	strXml	The xml packet defining the gain. 
**/
void ModulateNeuronPropSynapse::ModulationGain(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Gain");
	ModulationGain(AnimatSim::Gains::LoadGain(m_lpSim, "Gain", oXml));
}

/**
\brief	Sets the name of the property that this adapter will be setting.

\author	dcofer
\date	2/6/2013

\return	nothing.
**/
void ModulateNeuronPropSynapse::PropertyName(std::string strPropName)
{
	m_strPropertyName = strPropName;

	//Reset the property name so we can get the property type setup correctly.
	//If it is not set then we need to assume that they will set it later.
	//Make sure the property type is set to invalid so the step sim method knows this.
	if(m_lpToNeuron && !Std_IsBlank(strPropName))
		m_lpPropertyData = m_lpToNeuron->GetDataPointer(strPropName);
}

/**
\brief	Gets the name of the property that this adapter will be setting.

\author	dcofer
\date	2/6/2013

\return	Name of property that will be set.
**/
std::string ModulateNeuronPropSynapse::PropertyName() 
{return m_strPropertyName;}

void ModulateNeuronPropSynapse::Initialize()
{
	Synapse::Initialize();

	if(m_lpToNeuron && !Std_IsBlank(m_strPropertyName))
		m_lpPropertyData = m_lpToNeuron->GetDataPointer(m_strPropertyName);
}

#pragma region DataAccesMethods

bool ModulateNeuronPropSynapse::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
		
	if(Link::SetData(strDataType, strValue, false))
		return true;

	if(strType == "GAIN")
	{
		ModulationGain(strValue);
		return true;
	}

	if(strType == "PROPERTYNAME")
	{
		PropertyName(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

/**
\brief	Processes this synapse.

\author	dcofer
\date	6/17/2014

\return	Synaptic current.
**/
void ModulateNeuronPropSynapse::Process(float &fltCurrent)
{
	if(m_lpPropertyData)
	{
		float fltFF = 	this->FromNeuron()->FiringFreq(m_lpFRModule);
		float fltScaled =  m_lpGain->CalculateGain(fltFF); 
		*m_lpPropertyData = fltScaled;
	}
}

void ModulateNeuronPropSynapse::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Synapse::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Gain", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("PropertyName", AnimatPropertyType::String, AnimatPropertyDirection::Set));
}

#pragma endregion

void ModulateNeuronPropSynapse::ResetSimulation()
{
}

void ModulateNeuronPropSynapse::Load(CStdXml &oXml)
{
	Synapse::Load(oXml);

	oXml.IntoElem();

	ModulationGain(AnimatSim::Gains::LoadGain(m_lpSim, "Gain", oXml));
	PropertyName(oXml.GetChildString("PropertyName", ""));

	oXml.OutOfElem(); //OutOf Synapse Element
}


	}			//Synapses
}				//FiringRateSim






