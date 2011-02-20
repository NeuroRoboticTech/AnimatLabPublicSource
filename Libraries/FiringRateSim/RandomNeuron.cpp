// RandomNeuron.cpp: implementation of the RandomNeuron class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "RandomNeuron.h"
#include "FiringRateModule.h"

namespace FiringRateSim
{
	namespace Neurons
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RandomNeuron::RandomNeuron()
{
	m_fltITime=0;

	//decide randomly whether it starts high or low.
	if(Std_IRand(0, 100) > 50) 
		m_iIntrinsicType=IH_CURRENT;
	else
		m_iIntrinsicType=IL_CURRENT;

	m_fltIntrinsic=0;
	m_fltIl=0;

	m_lpCurrentGraph = NULL;
	m_lpBurstGraph = NULL;
	m_lpIBurstGraph = NULL;
}

RandomNeuron::~RandomNeuron()
{

try
{
	if(m_lpCurrentGraph) delete m_lpCurrentGraph;
	if(m_lpBurstGraph) delete m_lpBurstGraph;
	if(m_lpIBurstGraph) delete m_lpIBurstGraph;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of RandomNeuron\r\n", "", -1, FALSE, TRUE);}
}

float RandomNeuron::ITime()
{return m_fltITime;}

void RandomNeuron::ITime(float fltVal)
{m_fltITime=fltVal;}

unsigned char RandomNeuron::IntrinsicType()
{return m_iIntrinsicType;}

void RandomNeuron::IntrinsicType(unsigned char iVal)
{m_iIntrinsicType=iVal;}

float RandomNeuron::IntrinsicCurrent()
{return m_fltIntrinsic;}

void RandomNeuron::IntrinsicCurrent(float fltVal)
{m_fltIntrinsic=fltVal;}

float RandomNeuron::Il()
{return m_fltIl;}

void RandomNeuron::Il(float fltVal)
{m_fltIl=fltVal;}

unsigned char RandomNeuron::NeuronType()
{return RANDOM_NEURON;}

void RandomNeuron::CurrentDistribution(AnimatSim::Gains::Gain *lpGain)
{
	if(lpGain)
	{
		if(m_lpCurrentGraph) 
			{delete m_lpCurrentGraph; m_lpCurrentGraph = NULL;}
		m_lpCurrentGraph = lpGain;
	}
}

void RandomNeuron::CurrentDistribution(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Gain");
	CurrentDistribution(AnimatSim::Gains::LoadGain("CurrentGraph", oXml));
}

void RandomNeuron::BurstLengthDistribution(AnimatSim::Gains::Gain *lpGain)
{
	if(lpGain)
	{
		if(m_lpBurstGraph) 
			{delete m_lpBurstGraph; m_lpBurstGraph = NULL;}
		m_lpBurstGraph = lpGain;
	}
}

void RandomNeuron::BurstLengthDistribution(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Gain");
	BurstLengthDistribution(AnimatSim::Gains::LoadGain("BurstGraph", oXml));
}

void RandomNeuron::InterbusrtLengthDistribution(AnimatSim::Gains::Gain *lpGain)
{
	if(lpGain)
	{
		if(m_lpIBurstGraph) 
			{delete m_lpIBurstGraph; m_lpIBurstGraph = NULL;}
		m_lpIBurstGraph = lpGain;
	}
}

void RandomNeuron::InterbusrtLengthDistribution(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Gain");
	InterbusrtLengthDistribution(AnimatSim::Gains::LoadGain("InterBurstGraph", oXml));
}

void RandomNeuron::HighCurrentOn()
{
	double dblRand;

	m_iIntrinsicType=IH_CURRENT;

	//Lets get a random number for the time it will be high.
	dblRand = Std_DRand(0, 100);

	//Now run it through the random variable equation to get the actual value.
	m_fltITime = m_lpBurstGraph->CalculateGain((float) dblRand);  //Nl_EvalGraph(m_iBurstGraphType, m_fltBurstA, m_fltBurstB, m_fltBurstC, m_fltBurstD, (float) dblRand);

	//Lets get a random number for the current to use.
	dblRand = Std_DRand(0, 100);

	//Now run it through the random variable equation to get the actual value.
	m_fltIntrinsic =  m_lpCurrentGraph->CalculateGain((float) dblRand); //Nl_EvalGraph(m_iCurrentGraphType, m_fltCurrentA, m_fltCurrentB, m_fltCurrentC, m_fltCurrentD, (float) dblRand);
}

void RandomNeuron::LowCurrentOn()
{
	double dblRand;

	m_iIntrinsicType=IL_CURRENT;
	m_fltIntrinsic=m_fltIl;

	//Lets get a random number for the time it will be low.
	dblRand = Std_DRand(0, 100);

	//Now run it through the random variable equation to get the actual value.
	m_fltITime = m_lpIBurstGraph->CalculateGain((float) dblRand); //Nl_EvalGraph(m_iIBurstGraphType, m_fltIBurstA, m_fltIBurstB, m_fltIBurstC, m_fltIBurstD, (float) dblRand);
}

float RandomNeuron::CalculateIntrinsicCurrent(FiringRateModule *lpModule, float fltInputCurrent)
{

	m_fltITime-=lpModule->TimeStep();

	if(m_fltITime<0)
	{
		if(m_iIntrinsicType==IH_CURRENT)
			LowCurrentOn();
		else
			HighCurrentOn();
	}

	return m_fltIntrinsic;
}

long RandomNeuron::CalculateSnapshotByteSize()
{
	//We need bytes for the internal state variables for this neuron.
	return (Neuron::CalculateSnapshotByteSize() + sizeof(m_fltITime) + sizeof(m_iIntrinsicType));
}

void RandomNeuron::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	Neuron::SaveKeyFrameSnapshot(aryBytes, lIndex);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_fltITime, sizeof(m_fltITime));
  lIndex += sizeof(m_fltITime);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_iIntrinsicType, sizeof(m_iIntrinsicType));
  lIndex += sizeof(m_iIntrinsicType);
}

void RandomNeuron::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	Neuron::LoadKeyFrameSnapshot(aryBytes, lIndex);

	memcpy((void *)&m_fltITime, (void *) (aryBytes+lIndex), sizeof(m_fltITime));
  lIndex += sizeof(m_fltITime);

	memcpy((void *)&m_iIntrinsicType, (void *) (aryBytes+lIndex), sizeof(m_iIntrinsicType));
  lIndex += sizeof(m_iIntrinsicType);
}

#pragma region DataAccesMethods

BOOL RandomNeuron::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Neuron::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "IL")
	{
		Il(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "CURRENTDISTRIBUTION")
	{
		CurrentDistribution(strValue);
		return TRUE;
	}

	if(strType == "BURSTLENGTHDISTRIBUTION")
	{
		BurstLengthDistribution(strValue);
		return TRUE;
	}

	if(strType == "INTERBURSTLENGTHDISTRIBUTION")
	{
		InterbusrtLengthDistribution(strValue);
		return TRUE;
	}


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

#pragma endregion

void RandomNeuron::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{

	Neuron::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into Neuron Element

	m_fltIl = oXml.GetChildFloat("Il");

	CurrentDistribution(AnimatSim::Gains::LoadGain("CurrentGraph", oXml));
	BurstLengthDistribution(AnimatSim::Gains::LoadGain("BurstGraph", oXml));
	InterbusrtLengthDistribution(AnimatSim::Gains::LoadGain("InterBurstGraph", oXml));

	oXml.OutOfElem(); //OutOf Neuron Element
}

	}			//Neurons
}				//FiringRateSim

