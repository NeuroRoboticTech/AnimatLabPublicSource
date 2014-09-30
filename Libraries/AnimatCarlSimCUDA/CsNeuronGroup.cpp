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

///**
//\brief	Gets the membrane capacitance.
//
//\author	dcofer
//\date	3/29/2011
//
//\return	membrane capacitance.
//**/
//float CsNeuronGroup::Cn()
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
//void CsNeuronGroup::Cn(float fltVal)
//{
//	Std_IsAboveMin((float) 0, fltVal, true, "Cn");
//
//	m_fltCn=fltVal;
//	m_fltInvCn = 1/m_fltCn;
//	TemplateNodeChanged();
//}

///**
//\brief	Gets the CsNeuronGroup type.
//
//\author	dcofer
//\date	3/29/2011
//
//\return	CsNeuronGroup type.
//**/
//unsigned char CsNeuronGroup::CsNeuronGroupType()
//{return RUGULAR_CsNeuronGroup;}

void CsNeuronGroup::Copy(CStdSerialize *lpSource)
{
	Node::Copy(lpSource);

	CsNeuronGroup *lpOrig = dynamic_cast<CsNeuronGroup *>(lpSource);

	m_lpCsModule = lpOrig->m_lpCsModule;
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

void CsNeuronGroup::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Node::QueryProperties(aryProperties);

}

#pragma endregion

void CsNeuronGroup::Load(CStdXml &oXml)
{
	Node::Load(oXml);

	oXml.IntoElem();  //Into Neuron Element


	Enabled(oXml.GetChildBool("Enabled", true));

	oXml.OutOfElem(); //OutOf Neuron Element
}


}				//AnimatCarlSim



