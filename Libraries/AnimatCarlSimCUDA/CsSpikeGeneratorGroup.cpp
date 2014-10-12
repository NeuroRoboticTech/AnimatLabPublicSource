/**
\file	Neuron.cpp

\brief	Implements the neuron class.
**/

#include "StdAfx.h"

#include "CsSynapseGroup.h"
#include "CsNeuronGroup.h"
#include "CsSpikeGeneratorGroup.h"
#include "CsNeuralModule.h"

namespace AnimatCarlSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
CsSpikeGeneratorGroup::CsSpikeGeneratorGroup()
{
	m_lpSpikeRates = NULL;
	m_uiRefPeriod = 1;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
CsSpikeGeneratorGroup::~CsSpikeGeneratorGroup()
{

try
{
	DeletePoissonRates();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CsSpikeGeneratorGroup\r\n", "", -1, false, true);}
}

void CsSpikeGeneratorGroup::Copy(CStdSerialize *lpSource)
{
	CsNeuronGroup::Copy(lpSource);

	CsSpikeGeneratorGroup *lpOrig = dynamic_cast<CsSpikeGeneratorGroup *>(lpSource);

}

void CsSpikeGeneratorGroup::DeletePoissonRates()
{
	if(m_lpSpikeRates)
	{
		delete m_lpSpikeRates;
		m_lpSpikeRates = NULL;
	}
}

void CsSpikeGeneratorGroup::SetSpikeRatesUpdated()
{
	m_lpCsModule->SNN()->setSpikeRateUpdated();
}

void CsSpikeGeneratorGroup::SetCARLSimulation()
{
	if(m_lpCsModule && m_lpCsModule->SNN())
	{
		m_iGroupID = m_lpCsModule->SNN()->createSpikeGeneratorGroup(m_strName, m_uiNeuronCount, m_iNeuralType);

		DeletePoissonRates();

		// set Poisson rates for all neurons
		m_lpSpikeRates = new PoissonRate(m_uiNeuronCount);
		for (int i=0; i<m_uiNeuronCount; i++)
			m_lpSpikeRates->rates[i] = 0;
		m_lpCsModule->SNN()->setSpikeRate(m_iGroupID, m_lpSpikeRates, m_uiRefPeriod);

		//Set this up as a spike monitor.
		m_lpCsModule->SNN()->setSpikeMonitor(m_iGroupID, this);
	}
}

void CsSpikeGeneratorGroup::Initialize()
{
	CsNeuronGroup::Initialize();
}

void CsSpikeGeneratorGroup::StepSimulation()
{

}

void CsSpikeGeneratorGroup::AddExternalNodeInput(int iTargetDataType, float fltInput)
{
}

#pragma region DataAccesMethods

bool CsSpikeGeneratorGroup::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(CsNeuronGroup::SetData(strDataType, strValue, false))
		return true;


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void CsSpikeGeneratorGroup::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	CsNeuronGroup::QueryProperties(aryProperties);

}

#pragma endregion

void CsSpikeGeneratorGroup::Load(CStdXml &oXml)
{
	CsNeuronGroup::Load(oXml);

	oXml.IntoElem();  //Into Neuron Element


	oXml.OutOfElem(); //OutOf Neuron Element
}


}				//AnimatCarlSim



