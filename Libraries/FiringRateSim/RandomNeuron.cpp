/**
\file	RandomNeuron.cpp

\brief	Implements the random neuron class.
**/

#include "StdAfx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "RandomNeuron.h"
#include "FiringRateModule.h"

namespace FiringRateSim
{
	namespace Neurons
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
RandomNeuron::RandomNeuron()
{
	//decide randomly whether it starts high or low.
	if(Std_IRand(0, 100) > 50) 
		m_iIntrinsicTypeDefault=IH_CURRENT;
	else
		m_iIntrinsicTypeDefault=IL_CURRENT;

	m_fltITime=0;
	m_iIntrinsicType = m_iIntrinsicTypeDefault;
	m_fltIntrinsic=0;
	m_fltIl=0;
	m_fltIlinit = 0;

	m_lpCurrentGraph = NULL;
	m_lpBurstGraph = NULL;
	m_lpIBurstGraph = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
RandomNeuron::~RandomNeuron()
{

try
{
	if(m_lpCurrentGraph) delete m_lpCurrentGraph;
	if(m_lpBurstGraph) delete m_lpBurstGraph;
	if(m_lpIBurstGraph) delete m_lpIBurstGraph;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of RandomNeuron\r\n", "", -1, false, true);}
}

/**
\brief	Gets the duration of the active current mode.

\author	dcofer
\date	3/29/2011

\return	duration.
**/
float RandomNeuron::ITime()
{return m_fltITime;}

/**
\brief	Sets the duration of the active current mode.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void RandomNeuron::ITime(float fltVal)
{m_fltITime=fltVal;}

/**
\brief	Gets the active intrinsic type (HI or LOW).

\author	dcofer
\date	3/29/2011

\return	intrinsic type.
**/
unsigned char RandomNeuron::IntrinsicType()
{return m_iIntrinsicType;}

/**
\brief	Sets the active intrinsic type (HI or LOW).

\author	dcofer
\date	3/29/2011

\param	iVal	The new value. 
**/
void RandomNeuron::IntrinsicType(unsigned char iVal)
{m_iIntrinsicType=iVal;}

/**
\brief	Gets the active intrinsic current.

\author	dcofer
\date	3/29/2011

\return	intrinsic current.
**/
float RandomNeuron::IntrinsicCurrent()
{return m_fltIntrinsic;}

/**
\brief	Sets the active intrinsic current.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void RandomNeuron::IntrinsicCurrent(float fltVal)
{m_fltIntrinsic=fltVal;}

/**
\brief	Gets the low current.

\author	dcofer
\date	3/29/2011

\return	low current.
**/
float RandomNeuron::Il()
{return m_fltIl;}

/**
\brief	Sets the low current.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void RandomNeuron::Il(float fltVal)
{m_fltIl=fltVal;}

/**
\brief	Gets the low current init value.

\author	dcofer
\date	3/29/2011

\return	low current init value.
**/
float RandomNeuron::Ilinit()
{return m_fltIlinit;}

/**
\brief	Sets the low current init value.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void RandomNeuron::Ilinit(float fltVal)
{
	m_fltIlinit=fltVal;
	m_fltIl = m_fltIlinit;
}

/**
\brief	Gets the neuron type.

\author	dcofer
\date	3/29/2011

\return	neuron type.
**/
unsigned char RandomNeuron::NeuronType()
{return RANDOM_NEURON;}

/**
\brief	Sets the current distribution.

\author	dcofer
\date	3/29/2011

\param [in,out]	lpGain	Pointer to a gain. 
**/
void RandomNeuron::CurrentDistribution(AnimatSim::Gains::Gain *lpGain)
{
	if(lpGain)
	{
		if(m_lpCurrentGraph) 
			{delete m_lpCurrentGraph; m_lpCurrentGraph = NULL;}
		m_lpCurrentGraph = lpGain;
	}
}

/**
\brief	Sets the current distribution using an xml packet.

\author	dcofer
\date	3/29/2011

\param	strXml	The xml packet defining the gain. 
**/
void RandomNeuron::CurrentDistribution(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("CurrentGraph");
	CurrentDistribution(AnimatSim::Gains::LoadGain(m_lpSim, "CurrentGraph", oXml));
}

/**
\brief	Sets the burst length distribution.

\author	dcofer
\date	3/29/2011

\param [in,out]	lpGain	Pointer to a gain. 
**/
void RandomNeuron::BurstLengthDistribution(AnimatSim::Gains::Gain *lpGain)
{
	if(lpGain)
	{
		if(m_lpBurstGraph) 
			{delete m_lpBurstGraph; m_lpBurstGraph = NULL;}
		m_lpBurstGraph = lpGain;
	}
}

/**
\brief	Sets the burst length distribution using an xml packet.

\author	dcofer
\date	3/29/2011

\param	strXml	The xml packet defining the gain. 
**/
void RandomNeuron::BurstLengthDistribution(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("BurstGraph");
	BurstLengthDistribution(AnimatSim::Gains::LoadGain(m_lpSim, "BurstGraph", oXml));
}

/**
\brief	Sets the interbusrt length distribution.

\author	dcofer
\date	3/29/2011

\param [in,out]	lpGain	Pointer to a gain. 
**/
void RandomNeuron::InterbusrtLengthDistribution(AnimatSim::Gains::Gain *lpGain)
{
	if(lpGain)
	{
		if(m_lpIBurstGraph) 
			{delete m_lpIBurstGraph; m_lpIBurstGraph = NULL;}
		m_lpIBurstGraph = lpGain;
	}
}

/**
\brief	Sets the interbusrt length distribution using an xml packet.

\author	dcofer
\date	3/29/2011

\param	strXml	The xml packet defining the gain. 
**/
void RandomNeuron::InterbusrtLengthDistribution(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("InterBurstGraph");
	InterbusrtLengthDistribution(AnimatSim::Gains::LoadGain(m_lpSim, "InterBurstGraph", oXml));
}

/**
\brief	Turns the high current on.

\author	dcofer
\date	3/29/2011
**/
void RandomNeuron::HighCurrentOn()
{
	double dblRand;

	m_iIntrinsicType=IH_CURRENT;

	//Lets get a random number for the time it will be high.
	dblRand = Std_DRand(0, 100);

	//Now run it through the random variable equation to get the actual value.
	m_fltITime = m_lpBurstGraph->CalculateGain((float) dblRand);  

	//Lets get a random number for the current to use.
	dblRand = Std_DRand(0, 100);

	//Now run it through the random variable equation to get the actual value.
	m_fltIntrinsic =  m_lpCurrentGraph->CalculateGain((float) dblRand); 
}

/**
\brief	Turns the low current on.

\author	dcofer
\date	3/29/2011
**/
void RandomNeuron::LowCurrentOn()
{
	double dblRand;

	m_iIntrinsicType=IL_CURRENT;
	m_fltIntrinsic=m_fltIl;

	//Lets get a random number for the time it will be low.
	dblRand = Std_DRand(0, 100);

	//Now run it through the random variable equation to get the actual value.
	m_fltITime = m_lpIBurstGraph->CalculateGain((float) dblRand); 
}

void RandomNeuron::ResetSimulation()
{
	Neuron::ResetSimulation();

	m_fltITime=0;
	m_iIntrinsicType = m_iIntrinsicTypeDefault;
	m_fltIntrinsic=0;
	m_fltIl=m_fltIlinit;
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

bool RandomNeuron::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(Neuron::SetData(strDataType, strValue, false))
		return true;

	if(strType == "IL")
	{
		Ilinit(atof(strValue.c_str()));
		return true;
	}

	if(strType == "CURRENTDISTRIBUTION")
	{
		CurrentDistribution(strValue);
		return true;
	}

	if(strType == "BURSTLENGTHDISTRIBUTION")
	{
		BurstLengthDistribution(strValue);
		return true;
	}

	if(strType == "INTERBURSTLENGTHDISTRIBUTION")
	{
		InterbusrtLengthDistribution(strValue);
		return true;
	}


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RandomNeuron::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
{
	Neuron::QueryProperties(aryNames, aryTypes);

	aryNames.Add("Il");
	aryTypes.Add("Float");

	aryNames.Add("CurrentDistribution");
	aryTypes.Add("Xml");

	aryNames.Add("BurstLengthDistribution");
	aryTypes.Add("Xml");

	aryNames.Add("InterbusrtLengthDistribution");
	aryTypes.Add("Xml");
}

#pragma endregion

void RandomNeuron::Load(CStdXml &oXml)
{

	Neuron::Load(oXml);

	oXml.IntoElem();  //Into Neuron Element

	Ilinit(oXml.GetChildFloat("Il"));

	CurrentDistribution(AnimatSim::Gains::LoadGain(m_lpSim, "CurrentGraph", oXml));
	BurstLengthDistribution(AnimatSim::Gains::LoadGain(m_lpSim, "BurstGraph", oXml));
	InterbusrtLengthDistribution(AnimatSim::Gains::LoadGain(m_lpSim, "InterBurstGraph", oXml));

	oXml.OutOfElem(); //OutOf Neuron Element
}

	}			//Neurons
}				//FiringRateSim

