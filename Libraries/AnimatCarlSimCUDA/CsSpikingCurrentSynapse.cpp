/**
\file	CsSpikingCurrentSynapse.cpp

\brief	Implements the synapse class.
**/

#include "StdAfx.h"

#include "CsNeuronGroup.h"
#include "CsNeuralModule.h"
#include "CsSpikingCurrentSynapse.h"

namespace AnimatCarlSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
CsSpikingCurrentSynapse::CsSpikingCurrentSynapse()
{
	m_lpFromNeuron = NULL;
	m_lpToNeuron = NULL;
	m_fltPulseDecay = 0;
	m_fltPulseMagnitude = 0;
	m_fltPulseSign = 1;
	m_bWholePopulation = true;
	m_fltPulseTC = 0;
	m_fltCurrentMagnitude = 0;
	m_fltAppliedCurrent = 0;
	m_fltDecrementCurrent = 0;
	m_ulSpikeTestTime = 0;
	m_iStepsPerTest = 0;
	m_iStepsPerTestCount = -1;
	m_lTotalSpikesAdded = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
CsSpikingCurrentSynapse::~CsSpikingCurrentSynapse()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CsSpikingCurrentSynapse\r\n", "", -1, false, true);}
}

void CsSpikingCurrentSynapse::PulseDecay(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "PulseDecay");
	m_fltPulseDecay = fltVal;
	if(m_lpModule && m_lpModule->TimeStep() > 0)
	{
		float fltTm = m_lpModule->TimeStep() / (1 - exp(-m_lpModule->TimeStep() / m_fltPulseDecay));
		m_fltPulseTC =  m_lpModule->TimeStep() / fltTm;
	}
}

float CsSpikingCurrentSynapse::PulseDecay() {return m_fltPulseDecay;}

void CsSpikingCurrentSynapse::PulseCurrent(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "PulseCurrent");
	m_fltPulseMagnitude = fabs(fltVal);
	m_fltPulseSign = Std_Sign(fltVal);
}

float CsSpikingCurrentSynapse::PulseCurrent() {return (m_fltPulseMagnitude*m_fltPulseSign);}

void CsSpikingCurrentSynapse::Coverage(std::string strType)
{
	std::string strVal = Std_CheckString(strType);
	if(strVal == "INDIVIDUALS")
		WholePopulation(false);
	else if(strVal == "WHOLEPOPULATION")
		WholePopulation(true);
	else
		THROW_PARAM_ERROR(Cs_Err_lInvalidFiringRateCoverage, Cs_Err_strInvalidFiringRateCoverage, "Coverage", strType);
}

bool CsSpikingCurrentSynapse::WholePopulation() {return m_bWholePopulation;}

void CsSpikingCurrentSynapse::WholePopulation(bool bVal) 
{
	m_bWholePopulation = bVal;

	if(m_lpFromNeuron)
		m_lpFromNeuron->CollectFromWholePopulation(bVal);
}

void CsSpikingCurrentSynapse::Cells(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Cells");

	LoadCells(oXml);
}

void CsSpikingCurrentSynapse::CalculateStepsPerTest()
{
	m_ulSpikeTestTime = 0;
	m_iStepsPerTestCount = -1;
	m_ulSpikeTestTime = 0;

	if(m_lpModule && m_lpModule->TimeStep() > 0)
	{
		m_iStepsPerTest = (int) ((CARLSIM_STEP_INCREMENT/m_lpModule->TimeStep())+0.5);
		if(m_iStepsPerTest>0) m_iStepsPerTest--;
	}
	else
		m_iStepsPerTest = 0;
}

void CsSpikingCurrentSynapse::TimeStepModified()
{
	Link::TimeStepModified();

	//Reset the exponential pulse decay constant
	PulseDecay(m_fltPulseDecay);

	CalculateStepsPerTest();
}

void CsSpikingCurrentSynapse::Initialize()
{
	Link::Initialize();

	m_lpFromNeuron = dynamic_cast<CsNeuronGroup *>(m_lpSim->FindByID(m_strFromID));
	if(!m_lpFromNeuron)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strFromID);

	m_lpToNeuron = dynamic_cast<AnimatSim::Node *>(m_lpSim->FindByID(m_strToID));
	if(!m_lpToNeuron)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strToID);

	CalculateStepsPerTest();

	if(m_lpFromNeuron)
		m_lpFromNeuron->CollectFromWholePopulation(m_bWholePopulation);
}

#pragma region DataAccesMethods

float *CsSpikingCurrentSynapse::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	float *lpData = NULL;

	if(strType == "APPLIEDCURRENT")
		return &m_fltAppliedCurrent;
	else if(strType == "DECREMENTCURRENT")
		return &m_fltDecrementCurrent;

	return Link::GetDataPointer(strDataType);
}

void CsSpikingCurrentSynapse::VerifySystemPointers()
{
	AnimatBase::VerifySystemPointers();

	if(!m_lpStructure)
		THROW_PARAM_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined, "Link: ", m_strID);

	if(!m_lpOrganism) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Link: ", m_strID);

	if(!m_lpModule) 
		THROW_PARAM_ERROR(Al_Err_lNeuralModuleNotDefined, Al_Err_strNeuralModuleNotDefined, "Link: ", m_strID);
}

bool CsSpikingCurrentSynapse::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
		
	if(Link::SetData(strDataType, strValue, false))
		return true;

	if(strType == "PULSEDECAY")
	{
		PulseDecay(atof(strValue.c_str()));
		return true;
	}
	else if(strType == "PULSECURRENT")
	{
		PulseCurrent(atof(strValue.c_str()));
		return true;
	}
	else if(strType == "COVERAGE")
	{
		Coverage(strValue);
		return true;
	}
	else if(strType == "CELLS")
	{
		Cells(strValue);
		return true;
	}


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void CsSpikingCurrentSynapse::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Link::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("AppliedCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("DecrementCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("PulseDecay", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("PulseCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Coverage", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Cells", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
}

#pragma endregion

void CsSpikingCurrentSynapse::ResetSimulation()
{
	Link::ResetSimulation();

	m_fltCurrentMagnitude = 0;
	m_fltAppliedCurrent = 0;
	m_fltDecrementCurrent = 0;
	m_iStepsPerTestCount = -1;
	m_ulSpikeTestTime = 0;
	m_lTotalSpikesAdded = 0;
}

void CsSpikingCurrentSynapse::ProcessSpikes()
{
	//If the steps per count is < 0 it is the first time so run it then, then count normally.
	if(m_iStepsPerTestCount < 0 || m_iStepsPerTestCount == m_iStepsPerTest)
	{
		//std::string strMsg = "ProcessSpike Compare: Time: " + STR(m_lpSim->Time()) + ", ITime: " + STR(m_ulSpikeTestTime) + "\r\n";
		//OutputDebugString(strMsg.c_str());

		std::pair<std::multimap<unsigned long, int>::iterator, std::multimap<unsigned long, int>::iterator> itSpikesFortime;

		itSpikesFortime = m_lpFromNeuron->LastRecentSpikeTimes()->equal_range(m_ulSpikeTestTime);

		for (std::multimap<unsigned long, int>::iterator it2 = itSpikesFortime.first; it2 != itSpikesFortime.second; ++it2)
		{
			int iNeuronID = (int) (*it2).second;

			//If we are doing the whole population, or if not doing whole population and it is one the cells we are doing
			//Then add the pulse magnitude to the current.
			if(m_bWholePopulation || (!m_bWholePopulation && m_aryCells.count(iNeuronID)))
			{
				m_fltCurrentMagnitude+=m_fltPulseMagnitude;
				m_lTotalSpikesAdded++;
				//std::string strMsg = "Add Spike NeuronID: " + STR(iNeuronID) + " Time: " + STR(m_lpSim->Time()) + "]\r\n";
				//OutputDebugString(strMsg.c_str());
			}
		}

		m_ulSpikeTestTime++;
		m_iStepsPerTestCount = 0;
	}
	else
		m_iStepsPerTestCount++;
}

void CsSpikingCurrentSynapse::StepSimulation()
{
	if(m_bEnabled && m_lpFromNeuron && m_lpFromNeuron->LastRecentSpikeTimes())
	{

		m_fltDecrementCurrent = (m_fltCurrentMagnitude*m_fltPulseTC);
		m_fltCurrentMagnitude -= m_fltDecrementCurrent;

		if(m_fltCurrentMagnitude <= 0)
			m_fltCurrentMagnitude = 0;

		ProcessSpikes();

		m_fltAppliedCurrent = m_fltCurrentMagnitude * m_fltPulseSign;

		m_lpToNeuron->AddExternalNodeInput(0, m_fltAppliedCurrent);
	}
}

void CsSpikingCurrentSynapse::LoadCells(CStdXml &oXml)
{
	oXml.IntoElem();
	m_aryCells.RemoveAll();
	int iCount = oXml.NumberOfChildren();
	for(int iIdx=0; iIdx<iCount; iIdx++)
	{
		oXml.FindChildByIndex(iIdx);
		int iNeuronIdx = oXml.GetChildInt();

		if(iNeuronIdx < 0)
			THROW_PARAM_ERROR(Cs_Err_lInvalidNeuralIndex, Cs_Err_strInvalidNeuralIndex, "Neuron Index", iNeuronIdx);

		if(!m_aryCells.count(iNeuronIdx))
			m_aryCells.Add(iNeuronIdx, 1);
	}
	oXml.OutOfElem();
}

void CsSpikingCurrentSynapse::Load(CStdXml &oXml)
{
	Link::Load(oXml);

	oXml.IntoElem();  //Into Synapse Element

	m_strFromID = oXml.GetChildString("FromID");
	if(Std_IsBlank(m_strFromID)) 
		THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, "Attribute: FromID");

	m_strToID = oXml.GetChildString("ToID");
	if(Std_IsBlank(m_strToID)) 
		THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, "Attribute: ToID");

	PulseDecay(oXml.GetChildFloat("PulseDecay", m_fltPulseDecay));
	PulseCurrent(oXml.GetChildFloat("PulseCurrent", m_fltPulseMagnitude));

	Coverage(oXml.GetChildString("Coverage", "WholePopulation"));

	if(oXml.FindChildElement("Cells", false))
		LoadCells(oXml);

	oXml.OutOfElem(); //OutOf Synapse Element
}


}				//AnimatCarlSim






