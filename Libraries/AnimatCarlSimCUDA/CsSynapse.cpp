/**
\file	Synapse.cpp

\brief	Implements the synapse class.
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
CsSynapse::CsSynapse()
{
	//m_lpCsModule = NULL;

	m_lpFromNeuron = NULL;
	m_lpToNeuron = NULL;

	m_bEnabled = true;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
CsSynapse::~CsSynapse()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Synapse\r\n", "", -1, false, true);}
}

void CsSynapse::Initialize()
{
	Link::Initialize();

	m_lpFromNeuron = dynamic_cast<CsNeuron *>(m_lpSim->FindByID(m_strFromID));
	if(!m_lpFromNeuron)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strFromID);
}

void CsSynapse::SetSystemPointers(Simulator *m_lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	Link::SetSystemPointers(m_lpSim, lpStructure, lpModule, lpNode, false);

	//m_lpCsModule = dynamic_cast<CsNeuralModule *>(lpModule);

	if(bVerify) VerifySystemPointers();
}

void CsSynapse::VerifySystemPointers()
{
	Link::VerifySystemPointers();

	//if(!m_lpCsModule)
	//	THROW_PARAM_ERROR(Al_Err_lUnableToCastNeuralModuleToDesiredType, Al_Err_strUnableToCastNeuralModuleToDesiredType, "ID: ", m_lpCsModule->ID());

	if(!m_lpOrganism) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Link: ", m_strID);
}

#pragma region DataAccesMethods

bool CsSynapse::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
		
	if(Link::SetData(strDataType, strValue, false))
		return true;

	//if(strType == "WEIGHT")
	//{
	//	Weight(atof(strValue.c_str()));
	//	return true;
	//}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void CsSynapse::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Link::QueryProperties(aryProperties);

}

#pragma endregion

void CsSynapse::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	m_lpToNeuron = dynamic_cast<CsNeuron *>(m_lpNode);
	if(!m_lpToNeuron)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastNodeToDesiredType, Al_Err_strUnableToCastNodeToDesiredType, "ID: ", m_lpNode->ID());

	oXml.IntoElem();  //Into Synapse Element

	m_strFromID = oXml.GetChildString("FromID");
	if(Std_IsBlank(m_strFromID)) 
		THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, "Attribute: FromID");

	oXml.OutOfElem(); //OutOf Synapse Element
}


}				//AnimatCarlSim






