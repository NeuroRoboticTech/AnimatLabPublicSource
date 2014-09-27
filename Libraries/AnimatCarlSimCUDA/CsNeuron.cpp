/**
\file	Neuron.cpp

\brief	Implements the neuron class.
**/

#include "StdAfx.h"

#include "CsSynapse.h"
#include "CsNeuron.h"
#include "CsNeuralModule.h"

namespace AnimatCarlSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
CsNeuron::CsNeuron()
{
	m_lpCsModule = NULL;

	m_bEnabled = true;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
CsNeuron::~CsNeuron()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CsNeuron\r\n", "", -1, false, true);}
}

///**
//\brief	Gets the membrane capacitance.
//
//\author	dcofer
//\date	3/29/2011
//
//\return	membrane capacitance.
//**/
//float CsNeuron::Cn()
//{return m_fltCn;}
//
///**
//\brief	Sets the membrane capacitance.
//
//\author	dcofer
//\date	3/29/2011
//
//\param	fltVal	The new value. 
//**/
//void CsNeuron::Cn(float fltVal)
//{
//	Std_IsAboveMin((float) 0, fltVal, true, "Cn");
//
//	m_fltCn=fltVal;
//	m_fltInvCn = 1/m_fltCn;
//	TemplateNodeChanged();
//}

///**
//\brief	Gets the CsNeuron type.
//
//\author	dcofer
//\date	3/29/2011
//
//\return	CsNeuron type.
//**/
//unsigned char CsNeuron::CsNeuronType()
//{return RUGULAR_CsNeuron;}

void CsNeuron::Copy(CStdSerialize *lpSource)
{
	Node::Copy(lpSource);

	CsNeuron *lpOrig = dynamic_cast<CsNeuron *>(lpSource);

	m_lpCsModule = lpOrig->m_lpCsModule;
}


void CsNeuron::Initialize()
{
	Node::Initialize();
}

void CsNeuron::StepSimulation()
{

}

void CsNeuron::AddExternalNodeInput(int iTargetDataType, float fltInput)
{
}

void CsNeuron::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	Node::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, false);

	m_lpCsModule = dynamic_cast<CsNeuralModule *>(lpModule);

	if(bVerify) VerifySystemPointers();
}

void CsNeuron::VerifySystemPointers()
{
	Node::VerifySystemPointers();

	if(!m_lpCsModule)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastNeuralModuleToDesiredType, Al_Err_strUnableToCastNeuralModuleToDesiredType, "ID: ", m_lpCsModule->ID());

	if(!m_lpOrganism) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Link: ", m_strID);
}

#pragma region DataAccesMethods

bool CsNeuron::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(Node::SetData(strDataType, strValue, false))
		return true;

	//if(strType == "INITTIME")
	//{
	//	InitTime(atof(strValue.c_str()));
	//	return true;
	//}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void CsNeuron::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Node::QueryProperties(aryProperties);

}

#pragma endregion

void CsNeuron::Load(CStdXml &oXml)
{
	Node::Load(oXml);

	oXml.IntoElem();  //Into Neuron Element


	Enabled(oXml.GetChildBool("Enabled", true));

	oXml.OutOfElem(); //OutOf Neuron Element
}


}				//AnimatCarlSim



