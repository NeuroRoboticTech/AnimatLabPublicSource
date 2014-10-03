/**
\file	Synapse.cpp

\brief	Implements the synapse class.
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
CsSynapseGroup::CsSynapseGroup()
{
	m_lpCsModule = NULL;

	m_lpFromNeuron = NULL;
	m_lpToNeuron = NULL;

	m_bEnabled = true;
	m_fltInitWt = 1;
	m_fltMaxWt = 1;
	m_fltPconnect = 0.1f;
	m_iMinDelay = 1;
	m_iMaxDelay = 20;
	m_bPlastic = true;
	m_iSynapsesCreated = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
CsSynapseGroup::~CsSynapseGroup()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CsSynapseGroup\r\n", "", -1, false, true);}
}

void CsSynapseGroup::InitWt(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "InitWt", true);
	m_fltInitWt = fltVal;
}

float CsSynapseGroup::InitWt() {return m_fltInitWt;}

void CsSynapseGroup::MaxWt(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "MaxWt", true);
	m_fltMaxWt = fltVal;
}

float CsSynapseGroup::MaxWt() {return m_fltMaxWt;}

void CsSynapseGroup::Pconnect(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Pconnect", true);
	Std_IsBelowMax((float) 1, fltVal, true, "Pconnect", true);
	m_fltPconnect = fltVal;
}

float CsSynapseGroup::Pconnect() {return m_fltPconnect;}

void CsSynapseGroup::MinDelay(unsigned char iVal)
{
	m_iMinDelay = iVal;
}

unsigned char CsSynapseGroup::MinDelay() {return m_iMinDelay;}

void CsSynapseGroup::MaxDelay(unsigned char iVal)
{
	m_iMaxDelay = iVal;
}

unsigned char CsSynapseGroup::MaxDelay() {return m_iMaxDelay;}

void CsSynapseGroup::Plastic(bool bVal)
{
	m_bPlastic = bVal;
}

bool CsSynapseGroup::Plastic() {return m_bPlastic;}

int CsSynapseGroup::SynapsesCreated() {return m_iSynapsesCreated;}

std::string CsSynapseGroup::GeneratorKey()
{
	if(m_lpFromNeuron && m_lpToNeuron)
		if(m_bPlastic)
			return (STR(m_lpFromNeuron->GroupID()) + "_" + STR(m_lpToNeuron->GroupID() + "_Plastic"));
		else
			return (STR(m_lpFromNeuron->GroupID()) + "_" + STR(m_lpToNeuron->GroupID() + "_Fixed"));
	else
		return "_";
}


void CsSynapseGroup::SetCARLSimulation()
{
}

void CsSynapseGroup::Initialize()
{
	Link::Initialize();

	m_lpFromNeuron = dynamic_cast<CsNeuronGroup *>(m_lpSim->FindByID(m_strFromID));
	if(!m_lpFromNeuron)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strFromID);

	m_lpToNeuron = dynamic_cast<CsNeuronGroup *>(m_lpSim->FindByID(m_strToID));
	if(!m_lpToNeuron)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strToID);
}

void CsSynapseGroup::SetSystemPointers(Simulator *m_lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	Link::SetSystemPointers(m_lpSim, lpStructure, lpModule, lpNode, false);

	m_lpCsModule = dynamic_cast<CsNeuralModule *>(lpModule);

	if(bVerify) VerifySystemPointers();
}

void CsSynapseGroup::VerifySystemPointers()
{
	AnimatBase::VerifySystemPointers();

	if(!m_lpStructure)
		THROW_PARAM_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined, "Link: ", m_strID);

	if(!m_lpOrganism) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Link: ", m_strID);

	if(!m_lpModule) 
		THROW_PARAM_ERROR(Al_Err_lNeuralModuleNotDefined, Al_Err_strNeuralModuleNotDefined, "Link: ", m_strID);

	if(!m_lpCsModule)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastNeuralModuleToDesiredType, Al_Err_strUnableToCastNeuralModuleToDesiredType, "ID: ", m_lpCsModule->ID());
}

#pragma region DataAccesMethods

bool CsSynapseGroup::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
		
	if(Link::SetData(strDataType, strValue, false))
		return true;

	if(strType == "INITWT")
	{
		InitWt(atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "MAXWT")
	{
		MaxWt(atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "PCONNECT")
	{
		Pconnect(atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "MINDELAY")
	{
		MinDelay((unsigned char) atoi(strValue.c_str()));
		return true;
	}
	
	if(strType == "MAXDELAY")
	{
		MaxDelay((unsigned char) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "PLASTIC")
	{
		Plastic(Std_ToBool(strValue));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void CsSynapseGroup::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Link::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("InitWt", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MaxWt", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Pconnect", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MinDelay", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MaxDelay", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Plastic", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
}

#pragma endregion

void CsSynapseGroup::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into Synapse Element

	m_strFromID = oXml.GetChildString("FromID");
	if(Std_IsBlank(m_strFromID)) 
		THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, "Attribute: FromID");

	m_strToID = oXml.GetChildString("ToID");
	if(Std_IsBlank(m_strToID)) 
		THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, "Attribute: ToID");

	InitWt(oXml.GetChildFloat("InitWt", m_fltInitWt));
	MaxWt(oXml.GetChildFloat("MaxWt", m_fltMaxWt));
	Pconnect(oXml.GetChildFloat("Pconnect", m_fltPconnect));
	MinDelay(oXml.GetChildInt("MinDelay", m_iMinDelay));
	MaxDelay(oXml.GetChildInt("MaxDelay", m_iMaxDelay));
	Plastic(oXml.GetChildBool("Plastic", m_bPlastic));

	oXml.OutOfElem(); //OutOf Synapse Element
}


}				//AnimatCarlSim






