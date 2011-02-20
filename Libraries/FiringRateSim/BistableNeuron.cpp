// BistableNeuron.cpp: implementation of the BistableNeuron class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "BistableNeuron.h"
#include "FiringRateModule.h"

namespace FiringRateSim
{
	namespace Neurons
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BistableNeuron::BistableNeuron()
{
	m_fltIntrinsic=0;
	m_fltVsth = 0.010f;
	m_fltIl=0;
	m_fltIh = 0;
}

BistableNeuron::~BistableNeuron()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of BistableNeuron\r\n", "", -1, FALSE, TRUE);}
}

float BistableNeuron::IntrinsicCurrent()
{return m_fltIntrinsic;}

void BistableNeuron::IntrinsicCurrent(float fltVal)
{m_fltIntrinsic=fltVal;}

float BistableNeuron::Il()
{return m_fltIl;}

void BistableNeuron::Il(float fltVal)
{m_fltIl=fltVal;}

float BistableNeuron::Ih()
{return m_fltIh;}

void BistableNeuron::Ih(float fltVal)
{m_fltIh=fltVal;}

float BistableNeuron::Vsth()
{return m_fltVsth;}

void BistableNeuron::Vsth(float fltVal)
{m_fltVsth=fltVal;}

unsigned char BistableNeuron::NeuronType()
{return BISTABLE_NEURON;}

float BistableNeuron::CalculateIntrinsicCurrent(FiringRateModule *lpModule, float fltInputCurrent)
{
	if(m_fltVn>=m_fltVsth)
		m_fltIntrinsic=m_fltIh;
	else
		m_fltIntrinsic=m_fltIl;

	return m_fltIntrinsic;
}
//
//long BistableNeuron::CalculateSnapshotByteSize()
//{
//	//We need bytes for the internal state variables for this neuron.
//	return (Neuron::CalculateSnapshotByteSize() + sizeof(m_iIntrinsicType));
//}
//
//void BistableNeuron::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
//{
//	Neuron::SaveKeyFrameSnapshot(aryBytes, lIndex);
//
//	memcpy((void *) (aryBytes+lIndex), (void *)&m_iIntrinsicType, sizeof(m_iIntrinsicType));
//  lIndex += sizeof(m_iIntrinsicType);
//}
//
//void BistableNeuron::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
//{
//	Neuron::LoadKeyFrameSnapshot(aryBytes, lIndex);
//
//	memcpy((void *)&m_iIntrinsicType, (void *) (aryBytes+lIndex), sizeof(m_iIntrinsicType));
//  lIndex += sizeof(m_iIntrinsicType);
//}

#pragma region DataAccesMethods

BOOL BistableNeuron::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Neuron::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "VSTH")
	{
		Vsth(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "IL")
	{
		Il(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "IH")
	{
		Ih(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

void BistableNeuron::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	Neuron::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into Neuron Element

	m_fltVsth= oXml.GetChildFloat("Vsth");
	m_fltIl = oXml.GetChildFloat("Il");
	m_fltIh = oXml.GetChildFloat("Ih");

	oXml.OutOfElem(); //OutOf Neuron Element
}

	}			//Neurons
}				//FiringRateSim

