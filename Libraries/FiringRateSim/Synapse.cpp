// Synapse.cpp: implementation of the Synapse class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "FiringRateModule.h"

namespace FiringRateSim
{
	namespace Synapses
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

Synapse::Synapse()
{
	m_lpOrganism = NULL;
	m_lpFastModule = NULL;

	m_bEnabled = TRUE;
	m_lpFromNeuron = NULL;
	m_lpToNeuron = NULL;
	m_fltWeight=0;
	m_fltModulation=0;
	m_strType = "REGULAR";
}

Synapse::~Synapse()
{

try
{
	m_arySynapses.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Synapse\r\n", "", -1, FALSE, TRUE);}
}

BOOL Synapse::Enabled()
{return m_bEnabled;}

void Synapse::Enabled(BOOL bVal)
{m_bEnabled=bVal;}

float Synapse::Weight()
{return m_fltWeight;}

void Synapse::Weight(float fltVal)
{m_fltWeight=fltVal;}

float *Synapse::WeightPointer()
{return &m_fltWeight;}

float Synapse::Modulation()
{return m_fltModulation;}

float *Synapse::ModulationPointer()
{return &m_fltModulation;}

Synapse *Synapse::GetCompoundSynapse(short iCompoundIndex)
{

	if( iCompoundIndex<0 || iCompoundIndex>=m_arySynapses.GetSize() ) 
		THROW_ERROR(Std_Err_lInvalidIndex, Std_Err_strInvalidIndex);
	return m_arySynapses[iCompoundIndex];
}

void Synapse::AddSynapse(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("CompoundSynapse");

	Synapse *lpSynapse = LoadSynapse(m_lpSim, m_lpOrganism, m_lpToNeuron, oXml);
	lpSynapse->Initialize(m_lpSim, m_lpOrganism, m_lpFastModule, m_lpToNeuron);
}

void Synapse::RemoveSynapse(string strID, BOOL bThrowError)
{
	int iPos = FindSynapseListPos(strID, bThrowError);
	m_arySynapses.RemoveAt(iPos);
}

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

float Synapse::CalculateModulation(FiringRateModule *lpModule)
{
	m_fltModulation=1;
	int iCount, iIndex;
	Synapse *lpSynapse=NULL;

	iCount = m_arySynapses.GetSize();
	for(iIndex=0; iIndex<iCount; iIndex++)
	{
		lpSynapse = m_arySynapses[iIndex];
		m_fltModulation*=lpSynapse->CalculateModulation(lpModule);
	}

	return m_fltModulation;
}


void Synapse::Initialize(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode)
{
	Link::Initialize(lpSim, lpStructure, lpModule, lpNode);

	m_lpFastModule = dynamic_cast<FiringRateModule *>(lpModule);
	if(!m_lpFastModule)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastNeuralModuleToDesiredType, Al_Err_strUnableToCastNeuralModuleToDesiredType, "ID: ", lpModule->ID());

	m_lpOrganism = dynamic_cast<AnimatSim::Environment::Organism *>(lpStructure);
	if(!m_lpOrganism)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastOrganismToDesiredType, Al_Err_strUnableToCastOrganismToDesiredType, "ID: ", lpStructure->ID());

	m_lpFromNeuron = dynamic_cast<Neuron *>(lpSim->FindByID(m_strFromID));
	if(!m_lpFromNeuron)
		THROW_PARAM_ERROR(Al_Err_lNodeNotFound, Al_Err_strNodeNotFound, "ID: ", m_strFromID);

	int iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_arySynapses[iIndex]->Initialize(lpSim, m_lpOrganism, lpModule, lpNode);
}

#pragma region DataAccesMethods

float *Synapse::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "WEIGHT")
		return &m_fltWeight;
	else if(strType == "MODULATION")
		return &m_fltModulation;

	return NULL;
}

BOOL Synapse::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

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

BOOL Synapse::AddItem(string strItemType, string strXml, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "SYNAPSE")
	{
		AddSynapse(strXml);
		return TRUE;
	}


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL Synapse::RemoveItem(string strItemType, string strID, BOOL bThrowError)
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

void Synapse::ResetSimulation(Simulator *lpSim, Structure *lpStructure, Node *lpNode)
{
	int iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_arySynapses[iIndex]->ResetSimulation(lpSim, lpStructure, lpNode);
}

void Synapse::Load(Simulator *lpSim, Structure *lpStructure, Node *lpNode, CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	m_lpToNeuron = dynamic_cast<Neuron *>(lpNode);
	if(!m_lpToNeuron)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastNodeToDesiredType, Al_Err_strUnableToCastNodeToDesiredType, "ID: ", lpNode->ID());

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
			LoadSynapse(lpSim, lpStructure, m_lpToNeuron, oXml);
		}

		oXml.OutOfElem(); //OutOf CompoundSynapses Element
	}


	oXml.OutOfElem(); //OutOf Synapse Element
}

Synapse *Synapse::LoadSynapse(Simulator *lpSim, Structure *lpStructure, Neuron *lpNeuron, CStdXml &oXml)
{
	Synapse *lpSynapse=NULL;
	string strType;

try
{
	oXml.IntoElem();  //Into Synapse Element
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Synapse Element

	lpSynapse = dynamic_cast<Synapse *>(lpSim->CreateObject(Nl_NeuralModuleName(), "Synapse", strType));
	if(!lpSynapse)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Synapse");

	lpSynapse->Load(lpSim, lpStructure, lpNeuron, oXml);
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






