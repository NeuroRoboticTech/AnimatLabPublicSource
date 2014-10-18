/**
\file	Synapse.cpp

\brief	Implements the synapse class.
**/

#include "StdAfx.h"

#include "CsSynapseGroup.h"
#include "CsNeuronGroup.h"
#include "CsNeuralModule.h"
#include "CsSynapseIndividual.h"

namespace AnimatCarlSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
CsSynapseIndividual::CsSynapseIndividual()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
CsSynapseIndividual::~CsSynapseIndividual()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CsSynapseOneToOne\r\n", "", -1, false, true);}
}
		
void CsSynapseIndividual::FromIdx(int iVal) 
{
	Std_IsAboveMin((int) -1, iVal, true, "FromIdx", true); 
	m_iFromIdx = iVal;
	m_vSynapseKey.first = iVal;
}

int CsSynapseIndividual::FromIdx() {return m_iFromIdx;}
		
void CsSynapseIndividual::ToIdx(int iVal) 
{
	Std_IsAboveMin((int) -1, iVal, true, "ToIdx", true); 
	m_iToIdx = iVal;
	m_vSynapseKey.second = iVal;
}

int CsSynapseIndividual::ToIdx() {return m_iToIdx;}

std::pair<int, int> CsSynapseIndividual::SynapseIndexKey() {return m_vSynapseKey;}

void CsSynapseIndividual::SetCARLSimulation()
{
	if(m_lpCsModule && m_lpCsModule->SNN() && m_lpFromNeuron && m_lpToNeuron && 
		m_iFromIdx >= 0 && m_iToIdx >= 0 &&
		m_lpFromNeuron->GroupID() >= 0 && m_lpToNeuron->GroupID() >= 0)
	{
		std::string strKey = this->GeneratorKey();

		//First try and find a generator if one exists
		CsConnectionGenerator *lpGen = m_lpCsModule->FindConnectionGenerator(strKey, false);

		//If one does not exist then create a new one.
		if(!lpGen)
		{
			lpGen = new CsConnectionGenerator(m_lpFromNeuron->GroupID(), m_lpToNeuron->GroupID(), m_bPlastic, m_lpSim, m_lpStructure, m_lpCsModule);
			m_lpCsModule->AddConnectionGenerator(strKey, lpGen);
		}

		//Add this synapse to the correct generator for later.
		lpGen->SynapseMap()->insert(std::pair<std::pair<int, int>, CsSynapseIndividual *>(m_vSynapseKey, this));
	}	
}

bool CsSynapseIndividual::SetCARLSimulation(int iFromIdx, int iToIdx, float& weight, float& maxWt, float& delay, bool& connected)
{
	if(iFromIdx == m_iFromIdx && iToIdx == m_iToIdx)
	{
		weight = m_fltInitWt;
		maxWt = m_fltMaxWt;
		delay = m_iMinDelay;
		connected = true;
	}
	else
		connected = false;

	return connected;
}

#pragma region DataAccesMethods

bool CsSynapseIndividual::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
		
	if(CsSynapseGroup::SetData(strDataType, strValue, false))
		return true;

	if(strType == "FROMIDX")
	{
		FromIdx(atoi(strValue.c_str()));
		return true;
	}
	
	if(strType == "TOIDX")
	{
		ToIdx(atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void CsSynapseIndividual::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	CsSynapseGroup::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("FromIdx", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("ToIdx", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

#pragma endregion

void CsSynapseIndividual::Load(CStdXml &oXml)
{
	CsSynapseGroup::Load(oXml);

	oXml.IntoElem();  //Into Synapse Element

	FromIdx(oXml.GetChildInt("FromIdx", m_iFromIdx));
	ToIdx(oXml.GetChildInt("ToIdx", m_iToIdx));

	oXml.OutOfElem(); //OutOf Synapse Element
}


}				//AnimatCarlSim






