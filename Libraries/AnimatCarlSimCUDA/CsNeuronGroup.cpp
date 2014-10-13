/**
\file	Neuron.cpp

\brief	Implements the neuron class.
**/

#include "StdAfx.h"

#include "CsSynapseGroup.h"
#include "CsNeuronGroup.h"
#include "CsNeuralModule.h"

namespace AnimatCarlSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
CsNeuronGroup::CsNeuronGroup()
{
	m_lpCsModule = NULL;

	m_bEnabled = true;

	m_uiNeuronCount = 10;
	m_iNeuralType = EXCITATORY_NEURON;
	m_iGroupID = -1;

	m_fltA = 0.02f;
	m_fltStdA = 0;
	m_fltB = 0.2f;
	m_fltStdB = 0;
	m_fltC = -65.0f;
	m_fltStdC = 0;
	m_fltD = 8.0f;
	m_fltStdD = 0;

	m_bEnableCOBA = true;
	m_fltTauAMPA = 5;
	m_fltTauNMDA = 150;
	m_fltTauGABAa = 6;
	m_fltTauGABAb = 150;

	m_fltGroupFiringRate = 0;
	m_fltGroupTotalSpikes = 0;
	m_fltSpikeFake = -99999;
	m_iLastUpdateTime = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
CsNeuronGroup::~CsNeuronGroup()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CsNeuronGroup\r\n", "", -1, false, true);}
}


void CsNeuronGroup::NeuronCount(unsigned int iVal)
{	
	Std_IsAboveMin((int) 0, (int) iVal, true, "NeuronCount");
	m_uiNeuronCount = iVal;
}

unsigned int CsNeuronGroup::NeuronCount() {return m_uiNeuronCount;}

void CsNeuronGroup::NeuralType(int iVal)
{
	if(iVal == 0)
		m_iNeuralType = EXCITATORY_NEURON;
	else
		m_iNeuralType = INHIBITORY_NEURON;
}

int CsNeuronGroup::NeuralType() {return m_iNeuralType;}

void CsNeuronGroup::GroupID(int iVal)
{
	if(iVal < 0)
		m_iGroupID = -1;
	else
		m_iGroupID = iVal;
}

int CsNeuronGroup::GroupID() {return m_iGroupID;}

void CsNeuronGroup::A(float fltVal) {m_fltA = fltVal;}

float CsNeuronGroup::A() {return m_fltA;}

void CsNeuronGroup::StdA(float fltVal) {m_fltStdA = fltVal;}

float CsNeuronGroup::StdA() {return m_fltStdA;}

void CsNeuronGroup::B(float fltVal) {m_fltB = fltVal;}

float CsNeuronGroup::B() {return m_fltB;}

void CsNeuronGroup::StdB(float fltVal) {m_fltStdB = fltVal;}

float CsNeuronGroup::StdB() {return m_fltStdB;}

void CsNeuronGroup::C(float fltVal) {m_fltC = fltVal;}

float CsNeuronGroup::C() {return m_fltC;}

void CsNeuronGroup::StdC(float fltVal) {m_fltStdC = fltVal;}

float CsNeuronGroup::StdC() {return m_fltStdC;}

void CsNeuronGroup::D(float fltVal) {m_fltD = fltVal;}

float CsNeuronGroup::D() {return m_fltD;}

void CsNeuronGroup::StdD(float fltVal) {m_fltStdD = fltVal;}

float CsNeuronGroup::StdD() {return m_fltStdD;}

void CsNeuronGroup::EnableCOBA(bool bVal) {m_bEnableCOBA = bVal;}

bool CsNeuronGroup::EnableCOBA() {return m_bEnableCOBA;}

void CsNeuronGroup::TauAMPA(float fltVal) {m_fltTauAMPA = fltVal;}

float CsNeuronGroup::TauAMPA() {return m_fltTauAMPA;}

void CsNeuronGroup::TauNMDA(float fltVal) {m_fltTauNMDA = fltVal;}

float CsNeuronGroup::TauNMDA() {return m_fltTauNMDA;}

void CsNeuronGroup::TauGABAa(float fltVal) {m_fltTauGABAa = fltVal;}

float CsNeuronGroup::TauGABAa() {return m_fltTauGABAa;}

void CsNeuronGroup::TauGABAb(float fltVal) {m_fltTauGABAb = fltVal;}

float CsNeuronGroup::TauGABAb() {return m_fltTauGABAb;}

std::multimap<int, int> *CsNeuronGroup::SpikeTimes() {return &m_arySpikeTimes;}

std::multimap<int, int> *CsNeuronGroup::LastSpikeTimes() {return &m_aryLastSpikeTimes;}

void CsNeuronGroup::IncrementCollectSpikeDataForNeuron(int iIdx)
{
	CStdMap<int, int>::iterator oPos;
	oPos = m_aryCollectSpikeData.find(iIdx);
	if(oPos != m_aryCollectSpikeData.end())
	{
		int iVal =  oPos->second;
		m_aryCollectSpikeData[iIdx] = iVal+1;
	}
	else
		m_aryCollectSpikeData.Add(iIdx, 1);
}

void CsNeuronGroup::DecrementCollectSpikeDataForNeuron(int iIdx)
{
	CStdMap<int, int>::iterator oPos;
	oPos = m_aryCollectSpikeData.find(iIdx);
	if(oPos != m_aryCollectSpikeData.end())
	{
		int iVal =  oPos->second-1;
		if(iVal <= 0)
			m_aryCollectSpikeData.Remove(iIdx);
		else
			m_aryCollectSpikeData[iIdx] = iVal;
	}
}

bool CsNeuronGroup::CollectingSpikeDataForNeuron(int iIdx)
{
	return (bool) m_aryCollectSpikeData.count(iIdx);
}

void CsNeuronGroup::Copy(CStdSerialize *lpSource)
{
	Node::Copy(lpSource);

	CsNeuronGroup *lpOrig = dynamic_cast<CsNeuronGroup *>(lpSource);

	m_lpCsModule = lpOrig->m_lpCsModule;
	m_uiNeuronCount = lpOrig->m_uiNeuronCount;
	m_iNeuralType  = lpOrig->m_iNeuralType;
	m_iGroupID  = lpOrig->m_iGroupID;

	m_fltA = lpOrig->m_fltA;
	m_fltStdA = lpOrig->m_fltStdA;
	m_fltB = lpOrig->m_fltB;
	m_fltStdB = lpOrig->m_fltStdB;
	m_fltC = lpOrig->m_fltC;
	m_fltStdC = lpOrig->m_fltStdC;
	m_fltD = lpOrig->m_fltD;
	m_fltStdD = lpOrig->m_fltStdD;

	m_bEnableCOBA = lpOrig->m_bEnableCOBA;
	m_fltTauAMPA = lpOrig->m_fltTauAMPA;
	m_fltTauNMDA = lpOrig->m_fltTauNMDA;
	m_fltTauGABAa = lpOrig->m_fltTauGABAa;
	m_fltTauGABAb = lpOrig->m_fltTauGABAb;
}

void CsNeuronGroup::SetCARLSimulation()
{
	if(m_lpCsModule && m_lpCsModule->SNN())
	{
		m_iGroupID = m_lpCsModule->SNN()->createGroup(m_strName, m_uiNeuronCount, m_iNeuralType);
		m_lpCsModule->SNN()->setNeuronParameters(m_iGroupID, m_fltA, m_fltStdA, m_fltB, m_fltStdB, m_fltC, m_fltStdC, m_fltD, m_fltStdD);
		m_lpCsModule->SNN()->setConductances(m_iGroupID, m_bEnableCOBA, m_fltTauAMPA, m_fltTauNMDA, m_fltTauGABAa, m_fltTauGABAb);

		//Set this up as a spike monitor.
		m_lpCsModule->SNN()->setSpikeMonitor(m_iGroupID, this);
	}
}

void CsNeuronGroup::update(CpuSNN* s, int grpId, unsigned int* NeuronIds, unsigned int *timeCounts, unsigned int total_spikes, float firing_Rate)
{
	m_fltGroupFiringRate = firing_Rate;
	m_fltGroupTotalSpikes = total_spikes;

	m_aryLastSpikeTimes.clear();

	if(total_spikes > 0)
	{
		int pos = 0;
		for (int t=0; t<10; t++)
		{
			for (int i=0; i<timeCounts[t]; i++)
			{
				int id = NeuronIds[pos];
				if (m_aryCollectSpikeData.count(id))
				{
					int iTime = m_iLastUpdateTime + t;
					//Add the spike times to the two lists.
					m_aryLastSpikeTimes.insert(std::pair<int, int>(id, t));
					m_arySpikeTimes.insert(std::pair<int, int>(id, iTime));

					//std::string strMsg = "Spike [" + STR(id) + ", "+ STR(iTime) + "]\r\n";
					//OutputDebugString(strMsg.c_str());
				}
				pos++;
			}
		}
	}

	m_iLastUpdateTime += s->getMonitorUpdateSteps();
}

void CsNeuronGroup::Initialize()
{
	Node::Initialize();
}

void CsNeuronGroup::StepSimulation()
{

}

void CsNeuronGroup::ResetSimulation()
{
	Node::ResetSimulation();

	m_arySpikeTimes.clear();
	m_aryLastSpikeTimes.clear();
	m_fltGroupFiringRate = 0;
	m_fltGroupTotalSpikes = 0;
	m_fltSpikeFake = -99999;
	m_iLastUpdateTime = 0;
}

void CsNeuronGroup::AddExternalNodeInput(int iTargetDataType, float fltInput)
{
}

void CsNeuronGroup::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	Node::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, false);

	m_lpCsModule = dynamic_cast<CsNeuralModule *>(lpModule);

	if(bVerify) VerifySystemPointers();
}

void CsNeuronGroup::VerifySystemPointers()
{
	Node::VerifySystemPointers();

	if(!m_lpCsModule)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastNeuralModuleToDesiredType, Al_Err_strUnableToCastNeuralModuleToDesiredType, "ID: ", m_lpCsModule->ID());

	if(!m_lpOrganism) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Link: ", m_strID);
}

#pragma region DataAccesMethods

bool CsNeuronGroup::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(Node::SetData(strDataType, strValue, false))
		return true;

	if(strType == "NEURONCOUNT")
	{
		NeuronCount((unsigned int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "NEURALTYPE")
	{
		NeuralType(atoi(strValue.c_str()));
		return true;
	}

	if(strType == "A")
	{
		A(atof(strValue.c_str()));
		return true;
	}

	if(strType == "STDA")
	{
		StdA(atof(strValue.c_str()));
		return true;
	}

	if(strType == "B")
	{
		B(atof(strValue.c_str()));
		return true;
	}

	if(strType == "STDB")
	{
		StdB(atof(strValue.c_str()));
		return true;
	}

	if(strType == "C")
	{
		C(atof(strValue.c_str()));
		return true;
	}

	if(strType == "STDC")
	{
		StdC(atof(strValue.c_str()));
		return true;
	}

	if(strType == "D")
	{
		D(atof(strValue.c_str()));
		return true;
	}

	if(strType == "STDD")
	{
		StdD(atof(strValue.c_str()));
		return true;
	}

	if(strType == "ENABLECOBA")
	{
		EnableCOBA(Std_ToBool(strValue));
		return true;
	}

	if(strType == "TAUAMPA")
	{
		TauAMPA(atof(strValue.c_str()));
		return true;
	}

	if(strType == "TAUNMDA")
	{
		TauNMDA(atof(strValue.c_str()));
		return true;
	}

	if(strType == "TAUGABAA")
	{
		TauGABAa(atof(strValue.c_str()));
		return true;
	}

	if(strType == "TAUGABAB")
	{
		TauGABAb(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void CsNeuronGroup::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Node::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("NeuronCount", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("NeuralType", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("A", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("StdA", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("B", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("StdB", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("C", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("StdC", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("D", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("StdD", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("EnableCOBA", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("TauAMPA", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("TauNMDA", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("TauGABAa", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("TauGABAb", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

float *CsNeuronGroup::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	float *lpData = NULL;

	if(strType == "ENABLE")
		return &m_fltEnabled;
	else if(strType == "GROUPFIRINGRATE")
		return &m_fltGroupFiringRate;
	else if(strType == "GROUPTOTALSPIKES")
		return &m_fltGroupTotalSpikes;
	else if(strType == "SPIKE")
		return &m_fltSpikeFake;

	return Node::GetDataPointer(strDataType);
}

#pragma endregion

void CsNeuronGroup::Load(CStdXml &oXml)
{
	Node::Load(oXml);

	oXml.IntoElem();  //Into Neuron Element

	Enabled(oXml.GetChildBool("Enabled", true));

	NeuronCount(oXml.GetChildInt("NeuronCount", m_uiNeuronCount));
	NeuralType(oXml.GetChildInt("NeuralType", m_iNeuralType));
	A(oXml.GetChildFloat("A", m_fltA));
	StdA(oXml.GetChildFloat("StdA", m_fltStdA));
	B(oXml.GetChildFloat("B", m_fltB));
	StdB(oXml.GetChildFloat("StdB", m_fltStdB));
	C(oXml.GetChildFloat("C", m_fltC));
	StdC(oXml.GetChildFloat("StdC", m_fltStdC));
	D(oXml.GetChildFloat("D", m_fltD));
	StdD(oXml.GetChildFloat("StdD", m_fltStdD));

	EnableCOBA(oXml.GetChildBool("EnableCOBA", m_bEnableCOBA));
	TauAMPA(oXml.GetChildFloat("TauAMPA", m_fltTauAMPA));
	TauNMDA(oXml.GetChildFloat("TauNMDA", m_fltTauNMDA));
	TauGABAa(oXml.GetChildFloat("TauGABAa", m_fltTauGABAa));
	TauGABAb(oXml.GetChildFloat("TauGABAb", m_fltTauGABAb));

	oXml.OutOfElem(); //OutOf Neuron Element
}


}				//AnimatCarlSim



