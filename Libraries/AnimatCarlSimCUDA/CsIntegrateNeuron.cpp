/**
\file	CsIntegrateNeuron.cpp

\brief	Implements the neuron class.
**/

#include "StdAfx.h"

#include "CsSynapseGroup.h"
#include "CsNeuronGroup.h"
#include "CsSpikeGeneratorGroup.h"
#include "CsIntegrateNeuron.h"
#include "CsNeuralModule.h"

namespace AnimatCarlSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
CsIntegrateNeuron::CsIntegrateNeuron()
{
	m_lpCsModule = NULL;

	m_bEnabled = true;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
CsIntegrateNeuron::~CsIntegrateNeuron()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CsIntegrateNeuron\r\n", "", -1, false, true);}
}

void CsIntegrateNeuron::Copy(CStdSerialize *lpSource)
{
	Node::Copy(lpSource);

	CsIntegrateNeuron *lpOrig = dynamic_cast<CsIntegrateNeuron *>(lpSource);

	m_lpCsModule = lpOrig->m_lpCsModule;
}

void CsIntegrateNeuron::Initialize()
{
	Node::Initialize();
}

void CsIntegrateNeuron::StepSimulation()
{

}

void CsIntegrateNeuron::ResetSimulation()
{
	Node::ResetSimulation();

}

void CsIntegrateNeuron::AddExternalNodeInput(int iTargetDataType, float fltInput)
{
}

void CsIntegrateNeuron::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	Node::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, false);

	m_lpCsModule = dynamic_cast<CsNeuralModule *>(lpModule);

	if(bVerify) VerifySystemPointers();
}

void CsIntegrateNeuron::VerifySystemPointers()
{
	Node::VerifySystemPointers();

	if(!m_lpCsModule)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastNeuralModuleToDesiredType, Al_Err_strUnableToCastNeuralModuleToDesiredType, "ID: ", m_lpCsModule->ID());

	if(!m_lpOrganism) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Link: ", m_strID);
}

#pragma region DataAccesMethods

bool CsIntegrateNeuron::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(Node::SetData(strDataType, strValue, false))
		return true;

	/*
	if(strType == "NEURONCOUNT")
	{
		NeuronCount((unsigned int) atoi(strValue.c_str()));
		return true;
	}
*/
	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void CsIntegrateNeuron::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Node::QueryProperties(aryProperties);

	//aryProperties.Add(new TypeProperty("NeuronCount", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

float *CsIntegrateNeuron::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	float *lpData = NULL;
	/*
	if(strType == "ENABLE")
		return &m_fltEnabled;
	else if(strType == "GROUPFIRINGRATE")
		return &m_fltGroupFiringRate;
	else if(strType == "GROUPTOTALSPIKES")
		return &m_fltGroupTotalSpikes;
	else if(strType == "SPIKE")
		return &m_fltSpikeFake;
*/
	return Node::GetDataPointer(strDataType);
}

#pragma endregion

void CsIntegrateNeuron::Load(CStdXml &oXml)
{
	Node::Load(oXml);

	oXml.IntoElem();  //Into Neuron Element

	Enabled(oXml.GetChildBool("Enabled", true));


	oXml.OutOfElem(); //OutOf Neuron Element
}


}				//AnimatCarlSim



