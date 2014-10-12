/**
\file	CsFiringRateStimulus.cpp

\brief	Implements the current stimulus class. 
**/

#include "StdAfx.h"
#include "CsSynapseGroup.h"
#include "CsNeuronGroup.h"
#include "CsNeuralModule.h"
#include "CsSpikeGeneratorGroup.h"
#include "CsFiringRateStimulus.h"

namespace AnimatCarlSim
{

	/**
\brief	Default constructor. 

\author	dcofer
\date	3/16/2011
**/
CsFiringRateStimulus::CsFiringRateStimulus()
{
	m_lpNode = NULL;
	m_lpEval = NULL;
	m_lpSpikeGen = NULL;
	m_fltActiveRate = 0;
	m_fltPrevAppliedRate = 0;
	m_fltConstantRate = 0;
	m_bStimWholePopulation = true;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/16/2011
**/
CsFiringRateStimulus::~CsFiringRateStimulus()
{

try
{
	m_lpNode = NULL;
	m_lpSpikeGen = NULL;
	if(m_lpEval) 
		delete m_lpEval;

}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CsFiringRateStimulus\r\n", "", -1, false, true);}
}

/**
\brief	Gets the GUID ID of the node that is being stimulated. 

\author	dcofer
\date	3/17/2011

\return	. 
**/
std::string CsFiringRateStimulus::TargetNodeID() {return m_strTargetNodeID;}

/**
\brief	Sets the GUID ID of the node that will be stimulated. 

\author	dcofer
\date	3/17/2011

\param	strID	Identifier for the string. 
**/
void CsFiringRateStimulus::TargetNodeID(std::string strID)
{
	if(Std_IsBlank(strID))
		THROW_PARAM_ERROR(Al_Err_lInvalidCurrentType, Al_Err_strInvalidCurrentType, "ID", strID);
	m_strTargetNodeID = strID;
}

/**
\brief	Gets the post-fix current equation string. If this is null then an equation is not used. 
If one is specified then that equation is used during the cycle on times.

\author	dcofer
\date	3/17/2011

\return	. 
**/
std::string CsFiringRateStimulus::Equation() {return m_strEquation;}

/**
\brief	Sets the postfix current equation to use. If this is blank then the 
current constants (current on/off) are used. If it is not blank then this equation is
used during the cycle on periods.

\author	dcofer
\date	3/17/2011

\param	strEquation	The post-fix string equation. 
**/
void CsFiringRateStimulus::Equation(std::string strEquation)
{
	m_strEquation = strEquation;

	if(!Std_IsBlank(strEquation))
	{

		//Initialize the postfix evaluator.
		if(m_lpEval) 
			{delete m_lpEval; m_lpEval = NULL;}

		m_lpEval = new CStdPostFixEval;

		m_lpEval->AddVariable("t");
		m_lpEval->Equation(strEquation);
		m_lpEval->SetVariable("t", 0);

		if(m_bIsActivated)
			m_fltActiveRate = m_lpEval->Solve();
	}
	else
	{
		//Remove the current eval.
		if(m_lpEval) 
			{delete m_lpEval; m_lpEval = NULL;}

		if(m_bIsActivated)
			m_fltActiveRate = m_fltConstantRate;
	}
}


float CsFiringRateStimulus::ConstantRate() {return m_fltConstantRate;}

void CsFiringRateStimulus::ConstantRate(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "ConstantRate", true);
	m_fltConstantRate = fltVal;
}

float CsFiringRateStimulus::ActiveRate() {return m_fltActiveRate;}

float CsFiringRateStimulus::PrevAppliedRate() {return m_fltPrevAppliedRate;}

void CsFiringRateStimulus::Coverage(std::string strType)
{
	std::string strVal = Std_CheckString(strType);
	if(strVal == "INDIVIDUALS")
		m_bStimWholePopulation = false;
	else if(strVal == "WHOLEPOPULATION")
		m_bStimWholePopulation = true;
	else
		THROW_PARAM_ERROR(Cs_Err_lInvalidFiringRateCoverage, Cs_Err_strInvalidFiringRateCoverage, "Coverage", strType);
}

bool CsFiringRateStimulus::StimWholePopulation() {return m_bStimWholePopulation;}

void CsFiringRateStimulus::StimWholePopulation(bool bVal) {m_bStimWholePopulation = bVal;}

void CsFiringRateStimulus::CellsToStim(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Cells");

	LoadCellsToStim(oXml);
}

void CsFiringRateStimulus::Initialize()
{
	ExternalStimulus::Initialize();

	//Lets try and get the node we will dealing with.
	m_lpNode = dynamic_cast<Node *>(m_lpSim->FindByID(m_strTargetNodeID));
	if(!m_lpNode)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strTargetNodeID);

	m_lpSpikeGen = dynamic_cast<CsSpikeGeneratorGroup *>(m_lpNode);
	if(!m_lpSpikeGen)
		THROW_PARAM_ERROR(Cs_Err_lNotSpikeGeneratorType, Cs_Err_strNotSpikeGeneratorType, "ID: ", m_strTargetNodeID);

	if(m_lpSpikeGen && m_lpSpikeGen->GetCsModule())
		this->StepInterval(m_lpSpikeGen->GetCsModule()->SimulationStepInterval());
}

/**
\brief	Calculates the on cycle current at this time step. 

\author	dcofer
\date	3/17/2011

\return	The current on. 
**/
float CsFiringRateStimulus::CalculateFiringRate()
{
	if(m_lpEval)
	{
		m_lpEval->SetVariable("t", (m_lpSim->Time()-m_fltStartTime) );
		m_fltActiveRate = m_lpEval->Solve();
	}
	else
		m_fltActiveRate = m_fltConstantRate;

	return m_fltActiveRate;
}

bool CsFiringRateStimulus::RateChanged()
{
	if(fabs(m_fltActiveRate-m_fltPrevAppliedRate) > 0.001)
		return true;
	else
		return false;
}

void CsFiringRateStimulus::ApplyRateChange()
{
	if(m_bStimWholePopulation)
	{
		for(int iIdx=0; iIdx<m_lpSpikeGen->NeuronCount(); iIdx++)
		{
			//First remove the previous active rate from this group
			m_lpSpikeGen->SpikeRates()->rates[iIdx] -= m_fltPrevAppliedRate; 
			//Then add the next active rate to this group
			m_lpSpikeGen->SpikeRates()->rates[iIdx] += m_fltActiveRate; 
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
				m_lpSpikeGen->SpikeRates()->rates[iIdx] += m_fltActiveRate;
			}
		}
	}

	m_fltPrevAppliedRate = m_fltActiveRate;

	m_lpSpikeGen->SetSpikeRatesUpdated();
}

void CsFiringRateStimulus::StepSimulation()
{
	//If we are using a constant rate then there is no reason to recalculate and apply
	//Only do this if we are using an equation.
	if(m_lpEval && m_lpSpikeGen && m_lpSpikeGen->SpikeRates())
	{
		//First calculate the new rate.
		CalculateFiringRate();

		if(RateChanged())
			ApplyRateChange();
	}
}

void CsFiringRateStimulus::Activate()
{
	ExternalStimulus::Activate();

	//Only apply the changes here if we are using a constant rate.
	//If we are using a constant rate then we only apply it once in activate.
	//If using an equation it will get applied in StepSimulation.
	if(!m_lpEval)
	{
		CalculateFiringRate();
		ApplyRateChange();
	}
}

void CsFiringRateStimulus::Deactivate()
{		
	ExternalStimulus::Deactivate();

	//Reset the data
	m_fltActiveRate = 0;

	//Apply this change
	ApplyRateChange();

	//Reset our variables.
	m_fltActiveRate = 0;
	m_fltPrevAppliedRate = 0;
}

void CsFiringRateStimulus::ResetSimulation()
{
	ExternalStimulus::ResetSimulation();
	m_fltActiveRate = 0;
	m_fltPrevAppliedRate = 0;
}

float *CsFiringRateStimulus::GetDataPointer(const std::string &strDataType)
{
	float *lpData=NULL;
	std::string strType = Std_CheckString(strDataType);

	if(strType == "ACTIVERATE")
		lpData = &m_fltActiveRate;
	else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "StimulusName: " + STR(m_strName) + "  DataType: " + strDataType);

	return lpData;
} 

bool CsFiringRateStimulus::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(ExternalStimulus::SetData(strDataType, strValue, false))
		return true;

	if(strType == "CONSTANTRATE")
	{
		ConstantRate((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "COVERAGE")
	{
		Coverage(strValue);
		return true;
	}

	if(strType == "EQUATION")
	{
		Equation(strValue);
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

void CsFiringRateStimulus::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	ExternalStimulus::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("ActiveRate", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("ConstantRate", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Equation", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

void CsFiringRateStimulus::LoadCellsToStim(CStdXml &oXml)
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

void CsFiringRateStimulus::Load(CStdXml &oXml)
{
	ActivatedItem::Load(oXml);

	oXml.IntoElem();  //Into Simulus Element

	TargetNodeID(oXml.GetChildString("TargetNodeID"));

	ConstantRate(oXml.GetChildFloat("ConstantRate", m_fltConstantRate));
	Equation(oXml.GetChildString("Equation", ""));
	Coverage(oXml.GetChildString("Coverage", "WholePopulation"));

	if(oXml.FindChildElement("Cells", false))
		LoadCellsToStim(oXml);

	oXml.OutOfElem(); //OutOf Simulus Element
}

}				//AnimatCarlSim




