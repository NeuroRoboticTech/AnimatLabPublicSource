// PacemakerNeuron.cpp: implementation of the PacemakerNeuron class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "PacemakerNeuron.h"
#include "FiringRateModule.h"

namespace FiringRateSim
{
	namespace Neurons
	{ 

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

PacemakerNeuron::PacemakerNeuron()
{
	m_fltIl=(float) 0;
	m_fltIh=(float) 0;
	m_fltVssm=(float) -20e-3;
	m_fltMtl=(float) -31.25;
	m_fltBtl=(float) 175e-3;
	m_fltTh=(float) 75e-3;
	m_fltVss = 0;

	//Start off with Ih current.
	m_fltITime=0;
	m_iIntrinsicType=IH_CURRENT;
	m_fltIntrinsicI=m_fltIh;
	m_fltInterburstInterval = 0;
}

PacemakerNeuron::~PacemakerNeuron()
{

}


float PacemakerNeuron::Il()
{return m_fltIl;}

void PacemakerNeuron::Il(float fltVal)
{m_fltIl=fltVal;}

float PacemakerNeuron::Ih()
{return m_fltIh;}

void PacemakerNeuron::Ih(float fltVal)
{m_fltIh=fltVal;}

float PacemakerNeuron::Vssm()
{return m_fltVssm;}

void PacemakerNeuron::Vssm(float fltVal)
{m_fltVssm=fltVal;}

float PacemakerNeuron::Mtl()
{return m_fltMtl;}

void PacemakerNeuron::Mtl(float fltVal)
{m_fltMtl=fltVal;}

float PacemakerNeuron::Btl()
{return m_fltBtl;}

void PacemakerNeuron::Btl(float fltVal)
{m_fltBtl=fltVal;}

float PacemakerNeuron::Th()
{return m_fltTh;}

void PacemakerNeuron::Th(float fltVal)
{m_fltTh=fltVal;}

float PacemakerNeuron::ITime()
{return m_fltITime;}

void PacemakerNeuron::ITime(float fltVal)
{m_fltITime=fltVal;}

unsigned char PacemakerNeuron::IntrinsicType()
{return m_iIntrinsicType;}

void PacemakerNeuron::IntrinsicType(unsigned char iVal)
{m_iIntrinsicType=iVal;}

unsigned char PacemakerNeuron::NeuronType()
{return PACEMAKER_NEURON;}

float PacemakerNeuron::CalculateIntrinsicCurrent(FiringRateModule *lpModule, float fltInputCurrent)
{
	m_fltITime-=lpModule->TimeStep();

	m_fltVss=(fltInputCurrent/m_fltGn);

	if(m_fltITime<0)
	{
		if(m_iIntrinsicType==IH_CURRENT)
			LowCurrentOn(m_fltVss);
		else
			HighCurrentOn();
	}

	return m_fltIntrinsicI;
}


void PacemakerNeuron::HighCurrentOn()
{
	m_fltITime = m_fltTh;
	m_fltIntrinsicI = m_fltIh;
	m_iIntrinsicType=IH_CURRENT;
}

void PacemakerNeuron::LowCurrentOn(float fltVss)
{
	if(m_iIntrinsicType == IL_CURRENT)
		return;

	m_iIntrinsicType=IL_CURRENT;
	m_fltIntrinsicI=m_fltIl;


	if(fltVss>=m_fltVssm)
	{
		m_fltITime = m_fltMtl*fltVss + m_fltBtl;

		if(m_fltITime<=0)
		{
			m_fltITime=0;
			m_iIntrinsicType=IH_CURRENT;
			m_fltIntrinsicI=m_fltIh;
		}

		m_fltInterburstInterval = m_fltITime;
	}
	else
	{
		m_fltITime=0;
		m_iIntrinsicType=IH_CURRENT;
	}
}


void PacemakerNeuron::StepSimulation(Simulator *lpSim, Organism *lpOrganism, FiringRateModule *lpModule)
{
	//float fltTime = lpSim->Time();
	//if(m_strName == "U Obstacle Mem" && fltTime > 3.672)
	//	fltTime=fltTime;

	Neuron::StepSimulation(lpSim, lpOrganism, lpModule);

//	//Now we need to check if this neuron has crossed the firing threshold.
//	//If it has then we initiate High current.
		if( ((m_aryVn[lpModule->InactiveArray()] - m_fltVth) > 1e-6) && ((m_aryVn[lpModule->ActiveArray()] - m_fltVth) < 1e-6) )
			HighCurrentOn();

		if( ((m_aryVn[lpModule->InactiveArray()] - m_fltVth) < 1e-6) && ((m_aryVn[lpModule->ActiveArray()] - m_fltVth) > 1e-6) )
			LowCurrentOn(0);

	//I was having problems where Vnactive = 2e-9 and vth=0 it would not reset the high current on when you hit it with a current inject.
	//This was because of round off error. I used this subtraction stuff to get around it instead of the straight <> compares.
	//if(m_iIntrinsicType == IL_CURRENT)
	//{
	//	if( ((m_aryVn[lpModule->InactiveArray()] - m_fltVth) > 1e-6) && ((m_aryVn[lpModule->ActiveArray()] - m_fltVth) < 1e-6) )
	//		HighCurrentOn();

	//	//if( (m_aryVn[lpModule->InactiveArray()] >= m_fltVth) && (m_aryVn[lpModule->ActiveArray()] < m_fltVth) )
	//	//	HighCurrentOn();
	//}
	//else
	//{
	//	if( ((m_aryVn[lpModule->InactiveArray()] - m_fltVth) < 1e-6) && ((m_aryVn[lpModule->ActiveArray()] - m_fltVth) > 1e-6) )
	//		LowCurrentOn(0);

	//	//if( (m_aryVn[lpModule->InactiveArray()] < m_fltVth) && (m_aryVn[lpModule->ActiveArray()] >= m_fltVth) )
	//	//	LowCurrentOn(0);
	//}
}

long PacemakerNeuron::CalculateSnapshotByteSize()
{
	//We need bytes for the internal state variables for this neuron.
	return (Neuron::CalculateSnapshotByteSize() + sizeof(m_fltITime) + sizeof(m_iIntrinsicType));
}

#pragma region DataAccesMethods

float *PacemakerNeuron::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "STEADYSTATEVOLTAGE")
		return &m_fltVss;

	if(strType == "INTERBURSTINTERVAL")
		return &m_fltInterburstInterval;

	if(strType == "INTERBURSTTIMER")
		return &m_fltITime;

	return Neuron::GetDataPointer(strDataType);
}

BOOL PacemakerNeuron::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Neuron::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "VSSM")
	{
		Vssm(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "BTL")
	{
		Btl(atof(strValue.c_str()));
		return TRUE;
	}
	
	if(strType == "TH")
	{
		Th(atof(strValue.c_str()));
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

void PacemakerNeuron::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	Neuron::SaveKeyFrameSnapshot(aryBytes, lIndex);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_fltITime, sizeof(m_fltITime));
  lIndex += sizeof(m_fltITime);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_iIntrinsicType, sizeof(m_iIntrinsicType));
  lIndex += sizeof(m_iIntrinsicType);
}

void PacemakerNeuron::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	Neuron::LoadKeyFrameSnapshot(aryBytes, lIndex);

	memcpy((void *)&m_fltITime, (void *) (aryBytes+lIndex), sizeof(m_fltITime));
  lIndex += sizeof(m_fltITime);

	memcpy((void *)&m_iIntrinsicType, (void *) (aryBytes+lIndex), sizeof(m_iIntrinsicType));
  lIndex += sizeof(m_iIntrinsicType);
}

void PacemakerNeuron::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	Neuron::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into Neuron Element

	m_fltIl = oXml.GetChildFloat("Il");
	m_fltIh = oXml.GetChildFloat("Ih");
	m_fltVssm = oXml.GetChildFloat("Vssm");
	m_fltMtl = oXml.GetChildFloat("Mtl");
	m_fltBtl = oXml.GetChildFloat("Btl");
	m_fltTh = oXml.GetChildFloat("Th");

	oXml.OutOfElem(); //OutOf Neuron Element
}


	}			//Neurons
}				//FiringRateSim

