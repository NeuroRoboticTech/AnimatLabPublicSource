/**
\file	Synapse.cpp

\brief	Implements the synapse class.
**/

#include "StdAfx.h"

#include "CsSynapseGroup.h"
#include "CsNeuronGroup.h"
#include "CsNeuralModule.h"
#include "CsSynapseFull.h"

namespace AnimatCarlSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
CsSynapseFull::CsSynapseFull()
{
	m_bNoDirectConnect = false;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
CsSynapseFull::~CsSynapseFull()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CsSynapseOneToOne\r\n", "", -1, false, true);}
}

void CsSynapseFull::NoDirectConnect(bool bVal) {m_bNoDirectConnect = bVal;}

bool CsSynapseFull::NoDirectConnect() {return m_bNoDirectConnect;}

void CsSynapseFull::SetCARLSimulation()
{
	if(m_bEnabled && m_lpCsModule && m_lpCsModule->SNN() && m_lpFromNeuron && m_lpToNeuron && m_lpFromNeuron->GroupID() >= 0 && m_lpToNeuron->GroupID() >= 0)
	{
		if(m_bNoDirectConnect)
			m_iSynapsesCreated = m_lpCsModule->SNN()->connect(m_lpFromNeuron->GroupID(), m_lpToNeuron->GroupID(), "full-no-direct", m_fltInitWt, m_fltMaxWt, m_fltPconnect, m_iMinDelay, m_iMaxDelay, m_bPlastic);
		else
			m_iSynapsesCreated = m_lpCsModule->SNN()->connect(m_lpFromNeuron->GroupID(), m_lpToNeuron->GroupID(), "full", m_fltInitWt, m_fltMaxWt, m_fltPconnect, m_iMinDelay, m_iMaxDelay, m_bPlastic);
	}	
}

#pragma region DataAccesMethods

bool CsSynapseFull::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
		
	if(CsSynapseGroup::SetData(strDataType, strValue, false))
		return true;

	if(strType == "NODIRECTCONNECT")
	{
		NoDirectConnect(Std_ToBool(strValue));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void CsSynapseFull::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	CsSynapseGroup::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("NoDirectConnect", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
}

#pragma endregion

void CsSynapseFull::Load(CStdXml &oXml)
{
	CsSynapseGroup::Load(oXml);

	oXml.IntoElem();  //Into Synapse Element

	NoDirectConnect(oXml.GetChildBool("NoDirectConnect", m_bNoDirectConnect));

	oXml.OutOfElem(); //OutOf Synapse Element
}

}				//AnimatCarlSim






