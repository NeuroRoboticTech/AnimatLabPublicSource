/**
\file	Synapse.cpp

\brief	Implements the synapse class.
**/

#include "StdAfx.h"

#include "CsSynapseGroup.h"
#include "CsNeuronGroup.h"
#include "CsNeuralModule.h"
#include "CsConnectionGenerator.h"

namespace AnimatCarlSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
CsConnectionGenerator::CsConnectionGenerator()
{
	m_lpCsModule = NULL;
	m_iFromGroupID = -1;
	m_iToGroupID = -1;
}

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
CsConnectionGenerator::CsConnectionGenerator(int iFromGroupID, int iToGroupID, bool bPlastic, Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule)
{
	m_iFromGroupID = iFromGroupID;
	m_iToGroupID = iToGroupID;
	m_bPlastic = bPlastic;

	SetSystemPointers(lpSim, lpStructure, lpModule, NULL, true);
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
CsConnectionGenerator::~CsConnectionGenerator()
{

try
{
	//m_arySynapseMap.clear();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CsConnectionGenerator\r\n", "", -1, false, true);}
}

void CsConnectionGenerator::FromGroupID(int iVal) {m_iFromGroupID = iVal;}

int CsConnectionGenerator::FromGroupID() {return m_iFromGroupID;}

void CsConnectionGenerator::ToGroupID(int iVal) {m_iToGroupID = iVal;}

int CsConnectionGenerator::ToGroupID() {return m_iToGroupID;}

void CsConnectionGenerator::Plastic(bool bVal) {m_bPlastic = bVal;}

bool CsConnectionGenerator::Plastic() {return m_bPlastic;}

void CsConnectionGenerator::SetSystemPointers(Simulator *m_lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	AnimatBase::SetSystemPointers(m_lpSim, lpStructure, lpModule, lpNode, false);

	m_lpCsModule = dynamic_cast<CsNeuralModule *>(lpModule);

	if(bVerify) VerifySystemPointers();
}

void CsConnectionGenerator::VerifySystemPointers()
{
	AnimatBase::VerifySystemPointers();

	if(!m_lpCsModule)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastNeuralModuleToDesiredType, Al_Err_strUnableToCastNeuralModuleToDesiredType, "ID: ", m_lpCsModule->ID());

	if(!m_lpStructure) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Structure: ", m_strID);
}

void CsConnectionGenerator::SetCARLSimulation()
{
	if(m_lpCsModule && m_lpCsModule->SNN() && m_iFromGroupID >= 0 && m_iToGroupID >= 0)
		m_lpCsModule->SNN()->connect(m_iFromGroupID, m_iToGroupID, this, m_bPlastic);
}

void CsConnectionGenerator::connect(CpuSNN* s, int srcGrpId, int i, int destGrpId, int j, float& weight, float& maxWt, float& delay, bool& connected)
{
	std::pair<int, int> vKey(i, j);

	//If we find it then go through the list and create the connection.
	if(m_arySynapseMap.count(vKey) > 0)
	{
		//Get a list of items with this key
		std::pair<std::multimap<std::pair<int, int>, CsSynapseIndividual *>::iterator, std::multimap<std::pair<int, int>, CsSynapseIndividual *>::iterator> itRange = m_arySynapseMap.equal_range(vKey);

		//Then iterate through them
		for (std::multimap<std::pair<int, int>, CsSynapseIndividual *>::iterator it2 = itRange.first; it2 != itRange.second; ++it2)
		{
			CsSynapseIndividual *lpSynapse = (CsSynapseIndividual *)((*it2).second);
			
			if(lpSynapse->SetCARLSimulation(i, j, weight, maxWt, delay, connected))
				return;
		}
	}
	else
		connected = false;
}


}				//AnimatCarlSim






