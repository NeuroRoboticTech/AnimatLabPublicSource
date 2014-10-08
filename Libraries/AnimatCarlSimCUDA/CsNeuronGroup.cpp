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
	m_iNeuronType = EXCITATORY_NEURON;
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
	m_fltT_AMPA = 5;
	m_fltT_NMDA = 150;
	m_fltT_GABAa = 6;
	m_fltT_GABAb = 150;
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
	m_uiNeuronCount = iVal;
}

unsigned int CsNeuronGroup::NeuronCount() {return m_uiNeuronCount;}

void CsNeuronGroup::NeuronType(int iVal)
{
	if(iVal == 0)
		m_iNeuronType = EXCITATORY_NEURON;
	else
		m_iNeuronType = INHIBITORY_NEURON;
}

int CsNeuronGroup::NeuronType() {return m_iNeuronType;}

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

void CsNeuronGroup::T_AMPA(float fltVal) {m_fltT_AMPA = fltVal;}

float CsNeuronGroup::T_AMPA() {return m_fltT_AMPA;}

void CsNeuronGroup::T_NMDA(float fltVal) {m_fltT_NMDA = fltVal;}

float CsNeuronGroup::T_NMDA() {return m_fltT_NMDA;}

void CsNeuronGroup::T_GABAa(float fltVal) {m_fltT_GABAa = fltVal;}

float CsNeuronGroup::T_GABAa() {return m_fltT_GABAa;}

void CsNeuronGroup::T_GABAb(float fltVal) {m_fltT_GABAb = fltVal;}

float CsNeuronGroup::T_GABAb() {return m_fltT_GABAb;}

void CsNeuronGroup::Copy(CStdSerialize *lpSource)
{
	Node::Copy(lpSource);

	CsNeuronGroup *lpOrig = dynamic_cast<CsNeuronGroup *>(lpSource);

	m_lpCsModule = lpOrig->m_lpCsModule;
	m_uiNeuronCount = lpOrig->m_uiNeuronCount;
	m_iNeuronType  = lpOrig->m_iNeuronType;
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
	m_fltT_AMPA = lpOrig->m_fltT_AMPA;
	m_fltT_NMDA = lpOrig->m_fltT_NMDA;
	m_fltT_GABAa = lpOrig->m_fltT_GABAa;
	m_fltT_GABAb = lpOrig->m_fltT_GABAb;
}

void CsNeuronGroup::SetCARLSimulation()
{
	if(m_lpCsModule && m_lpCsModule->SNN())
	{
		m_iGroupID = m_lpCsModule->SNN()->createGroup(m_strName, m_uiNeuronCount, m_iNeuronType);
		m_lpCsModule->SNN()->setNeuronParameters(m_iGroupID, m_fltA, m_fltStdA, m_fltB, m_fltStdB, m_fltC, m_fltStdC, m_fltD, m_fltStdD);
		m_lpCsModule->SNN()->setConductances(m_iGroupID, m_bEnableCOBA, m_fltT_AMPA, m_fltT_NMDA, m_fltT_GABAa, m_fltT_GABAb);
	}
}

void CsNeuronGroup::Initialize()
{
	Node::Initialize();
}

void CsNeuronGroup::StepSimulation()
{

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

	if(strType == "NEURONTYPE")
	{
		NeuronType(atoi(strValue.c_str()));
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

	if(strType == "T_AMPA")
	{
		T_AMPA(atof(strValue.c_str()));
		return true;
	}

	if(strType == "T_NMDA")
	{
		T_NMDA(atof(strValue.c_str()));
		return true;
	}

	if(strType == "T_GABAA")
	{
		T_GABAa(atof(strValue.c_str()));
		return true;
	}

	if(strType == "T_GABAB")
	{
		T_GABAb(atof(strValue.c_str()));
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
	aryProperties.Add(new TypeProperty("NeuronType", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("A", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("StdA", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("B", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("StdB", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("C", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("StdC", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("D", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("StdD", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("EnableCOBA", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("T_AMPA", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("T_NMDA", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("T_GABAa", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("T_GABAb", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

#pragma endregion

void CsNeuronGroup::Load(CStdXml &oXml)
{
	Node::Load(oXml);

	oXml.IntoElem();  //Into Neuron Element

	Enabled(oXml.GetChildBool("Enabled", true));

	NeuronCount(oXml.GetChildInt("NeuronCount", m_uiNeuronCount));
	NeuronType(oXml.GetChildInt("NeuronType", m_iNeuronType));
	A(oXml.GetChildFloat("A", m_fltA));
	StdA(oXml.GetChildFloat("StdA", m_fltStdA));
	B(oXml.GetChildFloat("B", m_fltB));
	StdB(oXml.GetChildFloat("StdB", m_fltStdB));
	C(oXml.GetChildFloat("C", m_fltC));
	StdC(oXml.GetChildFloat("StdC", m_fltStdC));
	D(oXml.GetChildFloat("D", m_fltD));
	StdD(oXml.GetChildFloat("StdD", m_fltStdD));

	EnableCOBA(oXml.GetChildBool("EnableCOBA", m_bEnableCOBA));
	T_AMPA(oXml.GetChildFloat("T_AMPA", m_fltT_AMPA));
	T_NMDA(oXml.GetChildFloat("T_NMDA", m_fltT_NMDA));
	T_GABAa(oXml.GetChildFloat("T_GABAa", m_fltT_GABAa));
	T_GABAb(oXml.GetChildFloat("T_GABAb", m_fltT_GABAb));

	oXml.OutOfElem(); //OutOf Neuron Element
}


}				//AnimatCarlSim



