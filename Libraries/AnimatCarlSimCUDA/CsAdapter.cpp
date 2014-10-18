/**
\file	CsAdapter.cpp

\brief	Implements the current stimulus class. 
**/

#include "StdAfx.h"
#include "CsSynapseGroup.h"
#include "CsNeuronGroup.h"
#include "CsNeuralModule.h"
#include "CsSpikeGeneratorGroup.h"
#include "CsAdapter.h"

namespace AnimatCarlSim
{

	/**
\brief	Default constructor. 

\author	dcofer
\date	3/16/2011
**/
CsAdapter::CsAdapter()
{
	m_lpSpikeGen = NULL;
	m_bStimWholePopulation = true;
	m_fltPrevAppliedRate = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/16/2011
**/
CsAdapter::~CsAdapter()
{

try
{
	m_lpSpikeGen = NULL;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CsAdapter\r\n", "", -1, false, true);}
}

void CsAdapter::Coverage(std::string strType)
{
	std::string strVal = Std_CheckString(strType);
	if(strVal == "INDIVIDUALS")
		m_bStimWholePopulation = false;
	else if(strVal == "WHOLEPOPULATION")
		m_bStimWholePopulation = true;
	else
		THROW_PARAM_ERROR(Cs_Err_lInvalidFiringRateCoverage, Cs_Err_strInvalidFiringRateCoverage, "Coverage", strType);
}

bool CsAdapter::StimWholePopulation() {return m_bStimWholePopulation;}

void CsAdapter::StimWholePopulation(bool bVal) {m_bStimWholePopulation = bVal;}

void CsAdapter::CellsToStim(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Cells");

	LoadCellsToStim(oXml);
}

void CsAdapter::Initialize()
{
	Adapter::Initialize();

	m_lpSpikeGen = dynamic_cast<CsSpikeGeneratorGroup *>(m_lpTargetNode);
	if(!m_lpSpikeGen)
		THROW_PARAM_ERROR(Cs_Err_lNotSpikeGeneratorType, Cs_Err_strNotSpikeGeneratorType, "ID: ", m_strTargetID);

	//if(m_lpSpikeGen && m_lpSpikeGen->GetCsModule())
	//	this->StepInterval(m_lpSpikeGen->GetCsModule()->SimulationStepInterval());
}

bool CsAdapter::RateChanged(float fltRate)
{
	if(fabs(fltRate-m_fltPrevAppliedRate) > 0.001)
		return true;
	else
		return false;
}

void CsAdapter::ApplyExternalNodeInput(int iTargetDataType, float fltRate)
{
	if(m_lpSpikeGen && fltRate >= 0 && RateChanged(fltRate))
	{
		if(m_bStimWholePopulation)
		{
			for(int iIdx=0; iIdx<m_lpSpikeGen->NeuronCount(); iIdx++)
			{
				//First remove the previous active rate from this group
				m_lpSpikeGen->SpikeRates()->rates[iIdx] -= m_fltPrevAppliedRate; 
				//Then add the next active rate to this group
				m_lpSpikeGen->SpikeRates()->rates[iIdx] += fltRate; 
			}
		}
		else
		{
			for( CStdMap<int,int>::iterator ii=m_aryCellsToStim.begin(); ii!=m_aryCellsToStim.end(); ++ii)
			{
				int iIdx = (*ii).first;
				if(iIdx >= 0 && iIdx < m_lpSpikeGen->NeuronCount())
				{
					//First remove the previous active rate from this group
					m_lpSpikeGen->SpikeRates()->rates[iIdx] -= m_fltPrevAppliedRate; 
					//Then add the next active rate to this group
					m_lpSpikeGen->SpikeRates()->rates[iIdx] += fltRate;
				}
			}
		}

		m_fltPrevAppliedRate = fltRate;

		m_lpSpikeGen->SetSpikeRatesUpdated();
	}
}

void CsAdapter::ResetSimulation()
{
	Adapter::ResetSimulation();
	m_fltPrevAppliedRate = 0;
}

bool CsAdapter::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(Adapter::SetData(strDataType, strValue, false))
		return true;

	if(strType == "COVERAGE")
	{
		Coverage(strValue);
		return true;
	}

	if(strType == "CELLSTOSTIM")
	{
		CellsToStim(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void CsAdapter::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Adapter::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Coverage", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("CellsToStim", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
}

void CsAdapter::LoadCellsToStim(CStdXml &oXml)
{
	oXml.IntoElem();
	m_aryCellsToStim.RemoveAll();
	int iCount = oXml.NumberOfChildren();
	for(int iIdx=0; iIdx<iCount; iIdx++)
	{
		oXml.FindChildByIndex(iIdx);
		int iNeuronIdx = oXml.GetChildInt();

		if(iNeuronIdx < 0)
			THROW_PARAM_ERROR(Cs_Err_lInvalidNeuralIndex, Cs_Err_strInvalidNeuralIndex, "Neuron Index", iNeuronIdx);

		if(!m_aryCellsToStim.count(iNeuronIdx))
			m_aryCellsToStim.Add(iNeuronIdx, 1);
	}
	oXml.OutOfElem();
}

void CsAdapter::Load(CStdXml &oXml)
{
	Adapter::Load(oXml);

	oXml.IntoElem();  //Into Simulus Element

	Coverage(oXml.GetChildString("Coverage", "WholePopulation"));

	if(oXml.FindChildElement("Cells", false))
		LoadCellsToStim(oXml);

	oXml.OutOfElem(); //OutOf Simulus Element
}

}				//AnimatCarlSim




