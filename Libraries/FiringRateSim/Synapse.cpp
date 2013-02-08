/**
\file	Synapse.cpp

\brief	Implements the synapse class.
**/

#include "stdafx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "FiringRateModule.h"

namespace FiringRateSim
{
	namespace Synapses
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
Synapse::Synapse()
{
	m_lpFRModule = NULL;

	m_lpFromNeuron = NULL;
	m_lpToNeuron = NULL;
	m_fltWeight=0;
	m_fltModulation=0;
	m_strType = "REGULAR";
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
Synapse::~Synapse()
{

try
{
	m_arySynapses.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Synapse\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the synaptic weight.

\author	dcofer
\date	3/29/2011

\return	synaptic weight.
**/
float Synapse::Weight()
{return m_fltWeight;}

/**
\brief	Sets the synaptic weight.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void Synapse::Weight(float fltVal)
{m_fltWeight=fltVal;}

/**
\brief	Gets a pointer to the synaptic weight.

\details This is so that other items can alter the synaptic weight.

\author	dcofer
\date	3/29/2011

\return	Pointer to the weight.
**/
float *Synapse::WeightPointer()
{return &m_fltWeight;}

/**
\brief	Gets the synaptic modulation.

\author	dcofer
\date	3/29/2011

\return	synaptic modulation.
**/
float Synapse::Modulation()
{return m_fltModulation;}

/**
\brief	Gets a pointer to the synaptic modulation.

\details This is so that other items can alter the synaptic modulation.

\author	dcofer
\date	3/29/2011

\return	pointer to the synaptic modulation.
**/
float *Synapse::ModulationPointer()
{return &m_fltModulation;}

/**
\brief	Gets a pointer to a compound synapse.

\author	dcofer
\date	3/29/2011

\param	iCompoundIndex	Zero-based index of the synapse. 

\return	Pointer to the compound synapse.
**/
Synapse *Synapse::GetCompoundSynapse(short iCompoundIndex)
{

	if( iCompoundIndex<0 || iCompoundIndex>=m_arySynapses.GetSize() ) 
		THROW_ERROR(Std_Err_lInvalidIndex, Std_Err_strInvalidIndex);
	return m_arySynapses[iCompoundIndex];
}

/**
\brief	Adds a compound synapse to this one. 

\author	dcofer
\date	3/29/2011

\param	strXml	The xml packet to load. 
**/
void Synapse::AddSynapse(string strXml, BOOL bDoNotInit)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("CompoundSynapse");

	Synapse *lpSynapse = LoadSynapse(oXml);

	if(!bDoNotInit)
		lpSynapse->Initialize();
}

/**
\brief	Removes the copmound synapse specified by ID.

\author	dcofer
\date	3/29/2011

\param	strID	   	GUID ID for the synaspe to remove. 
\param	bThrowError	true to throw error if synapse is not found. 
**/
void Synapse::RemoveSynapse(string strID, BOOL bThrowError)
{
	int iPos = FindSynapseListPos(strID, bThrowError);
	m_arySynapses.RemoveAt(iPos);
}

/**
\brief	Searches for the first synapse list position.

\author	dcofer
\date	3/29/2011

\param	strID	   	Identifier for the string. 
\param	bThrowError	true to throw error. 

\return	The found synapse list position.
**/
int Synapse::FindSynapseListPos(string strID, BOOL bThrowError)
{
	string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_arySynapses[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Nl_Err_lSynapseNotFound, Nl_Err_strSynapseNotFound, "ID");

	return -1;
}

/**
\brief	Calculates the synaptic modulation.

\author	dcofer
\date	3/29/2011

\param [in,out]	m_lpFRModule	Pointer to a fast module. 

\return	The calculated modulation.
**/
float Synapse::CalculateModulation(FiringRateModule *m_lpFRModule)
{
	m_fltModulation=1;
	int iCount, iIndex;
	Synapse *lpSynapse=NULL;

	iCount = m_arySynapses.GetSize();
	for(iIndex=0; iIndex<iCount; iIndex++)
	{
		lpSynapse = m_arySynapses[iIndex];
		m_fltModulation*=lpSynapse->CalculateModulation(m_lpFRModule);
	}

	return m_fltModulation;
}


void Synapse::Initialize()
{
	Link::Initialize();

	m_lpFromNeuron = dynamic_cast<Neuron *>(m_lpSim->FindByID(m_strFromID));
	if(!m_lpFromNeuron)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strFromID);

	int iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_arySynapses[iIndex]->Initialize();
}

void Synapse::SetSystemPointers(Simulator *m_lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify)
{
	Link::SetSystemPointers(m_lpSim, lpStructure, lpModule, lpNode, FALSE);

	m_lpFRModule = dynamic_cast<FiringRateModule *>(lpModule);

	if(bVerify) VerifySystemPointers();
}

void Synapse::VerifySystemPointers()
{
	Link::VerifySystemPointers();

	if(!m_lpFRModule)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastNeuralModuleToDesiredType, Al_Err_strUnableToCastNeuralModuleToDesiredType, "ID: ", m_lpFRModule->ID());

	if(!m_lpOrganism) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Link: ", m_strID);
}

#pragma region DataAccesMethods

float *Synapse::GetDataPointer(const string &strDataType)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "WEIGHT")
		return &m_fltWeight;
	else if(strType == "MODULATION")
		return &m_fltModulation;

	return NULL;
}

BOOL Synapse::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);
		
	if(Link::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "WEIGHT")
	{
		Weight(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Synapse::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	Link::QueryProperties(aryNames, aryTypes);

	aryNames.Add("Weight");
	aryTypes.Add("Float");
}

BOOL Synapse::AddItem(const string &strItemType, const string &strXml, BOOL bThrowError, BOOL bDoNotInit)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "SYNAPSE")
	{
		AddSynapse(strXml, bDoNotInit);
		return TRUE;
	}


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL Synapse::RemoveItem(const string &strItemType, const string &strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "SYNAPSE")
	{
		RemoveSynapse(strID, bThrowError);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

#pragma endregion

void Synapse::ResetSimulation()
{
	int iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_arySynapses[iIndex]->ResetSimulation();
}

void Synapse::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	m_lpToNeuron = dynamic_cast<Neuron *>(m_lpNode);
	if(!m_lpToNeuron)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastNodeToDesiredType, Al_Err_strUnableToCastNodeToDesiredType, "ID: ", m_lpNode->ID());

	oXml.IntoElem();  //Into Synapse Element

	m_strFromID = oXml.GetChildString("FromID");
	if(Std_IsBlank(m_strFromID)) 
		THROW_TEXT_ERROR(Std_Err_lBlankAttrib, Std_Err_strBlankAttrib, "Attribute: FromID");

	m_bEnabled = oXml.GetChildBool("Enabled", TRUE);
	m_fltWeight = oXml.GetChildFloat("Weight");

	m_arySynapses.RemoveAll();

	//*** Begin Loading CompoundSynapses. *****
	if(oXml.FindChildElement("CompoundSynapses", FALSE))
	{
		oXml.IntoChildElement("CompoundSynapses");

		int iCount = oXml.NumberOfChildren();
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadSynapse(oXml);
		}

		oXml.OutOfElem(); //OutOf CompoundSynapses Element
	}


	oXml.OutOfElem(); //OutOf Synapse Element
}

/**
\brief	Loads a synapse.

\author	dcofer
\date	3/29/2011

\param [in,out]	oXml	The xml packet to load. 

\return	Pointer to the loaded synapse.
**/
Synapse *Synapse::LoadSynapse(CStdXml &oXml)
{
	Synapse *lpSynapse=NULL;
	string strType;

try
{
	oXml.IntoElem();  //Into Synapse Element
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Synapse Element

	lpSynapse = dynamic_cast<Synapse *>(m_lpSim->CreateObject(Nl_NeuralModuleName(), "Synapse", strType));
	if(!lpSynapse)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Synapse");

	lpSynapse->SetSystemPointers(m_lpSim, m_lpStructure, m_lpFRModule, m_lpNode, TRUE);
	lpSynapse->Load(oXml);
	m_arySynapses.Add(lpSynapse);

	return lpSynapse;
}
catch(CStdErrorInfo oError)
{
	if(lpSynapse) delete lpSynapse;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpSynapse) delete lpSynapse;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

	}			//Synapses
}				//FiringRateSim






