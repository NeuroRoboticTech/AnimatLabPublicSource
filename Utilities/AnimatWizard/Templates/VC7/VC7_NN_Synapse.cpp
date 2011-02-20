// Synapse.cpp: implementation of the Synapse class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "[*PROJECT_NAME*]NeuralModule.h"

namespace [*PROJECT_NAME*]
{
	namespace Synapses
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

Synapse::Synapse()
{
	m_bEnabled = TRUE;
	m_iFromX=0;
	m_iFromY=0;
	m_iFromZ=0;
	m_lpNeuron = NULL;
}

Synapse::~Synapse()
{

}

BOOL Synapse::Enabled()
{return m_bEnabled;}

void Synapse::Enabled(BOOL bVal)
{m_bEnabled=bVal;}

unsigned char Synapse::FromX()
{return m_iFromX;}

void Synapse::FromX(unsigned char iVal)
{m_iFromX=iVal;}

unsigned char Synapse::FromY()
{return m_iFromY;}

void Synapse::FromY(unsigned char iVal)
{m_iFromY=iVal;}

unsigned char Synapse::FromZ()
{return m_iFromZ;}

void Synapse::FromZ(unsigned char iVal)
{m_iFromZ=iVal;}

float *Synapse::GetDataPointer(short iCompoundIndex, short iDataType)
{
	float *lpData = NULL;

	//switch(iDataType)
	//{
	//default:
		THROW_PARAM_ERROR(Nl_Err_lInvalidNeuronDataType, Nl_Err_strInvalidNeuronDataType, "Synapse Data Type", (long) iDataType);
	//}

	return lpData;
}

float Synapse::CalculateCurrent()
{
	return 0;
}

void Synapse::Initialize(Simulator *lpSim, Organism *lpOrganism, TestNeuralModule *lpModule)
{
	m_lpNeuron = lpModule->GetNeuron(m_iFromX, m_iFromY, m_iFromZ); 
} 

void Synapse::Load(Simulator *lpSim, Structure *lpStructure, Neuron *lpNeuron, CStdXml &oXml)
{

	oXml.IntoElem();  //Into Synapse Element

	CStdIPoint oFrom;
	Std_LoadPoint(oXml, "From", oFrom);

	m_bEnabled = oXml.GetChildBool("Enabled", TRUE);
	m_iFromX = (unsigned char) oFrom.x;
	m_iFromY = (unsigned char) oFrom.y;
	m_iFromZ = (unsigned char) oFrom.z;

	oXml.OutOfElem(); //OutOf Synapse Element
}



void Synapse::Save(Simulator *lpSim, Structure *lpStructure, Neuron *lpNeuron, CStdXml &oXml)
{

	oXml.AddChildElement("Synapse");
	oXml.IntoElem();  //Into Synapse Element

	oXml.AddChildElement("FromX", m_iFromX);
	oXml.AddChildElement("FromY", m_iFromY);
	oXml.AddChildElement("FromZ", m_iFromZ);

	oXml.OutOfElem();  //OutOf Synapse Element
}

	}			//Synapses
}				//[*PROJECT_NAME*]






