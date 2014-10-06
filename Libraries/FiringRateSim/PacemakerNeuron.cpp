/**
\file	PacemakerNeuron.cpp

\brief	Implements the pacemaker neuron class.
**/

#include "StdAfx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "PacemakerNeuron.h"
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

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
PacemakerNeuron::~PacemakerNeuron()
{

}

/**
\brief	Gets the low intrinsic current value.

\author	dcofer
\date	3/29/2011

\return	Current value.
**/
float PacemakerNeuron::Il()
{return m_fltIl;}

/**
\brief	Sets the low intrinsic current value.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void PacemakerNeuron::Il(float fltVal)
{
	m_fltIl=fltVal;
}

/**
\brief	Gets the high intrinsic current value.

\author	dcofer
\date	3/29/2011

\return	Current value.
**/
float PacemakerNeuron::Ih()
{return m_fltIh;}

/**
\brief	Sets the high intrinsic current value.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void PacemakerNeuron::Ih(float fltVal)
{
	m_fltIh=fltVal;
}

/**
\brief	Gets the lower steady state threshold.

\author	dcofer
\date	3/29/2011

\return	threshold value.
**/
float PacemakerNeuron::Vssm()
{return m_fltVssm;}

/**
\brief	Sets the lower steady state threshold.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void PacemakerNeuron::Vssm(float fltVal)
{
	m_fltVssm=fltVal;
}

/**
\brief	Gets the slope used to calculate length of time that Il current remains active.

\author	dcofer
\date	3/29/2011

\return	slope.
**/
float PacemakerNeuron::Mtl()
{return m_fltMtl;}

/**
\brief	Sets the slope used to calculate length of time that Il current remains active.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void PacemakerNeuron::Mtl(float fltVal)
{
	m_fltMtl=fltVal;
}

/**
\brief	Gets the intercept used to calculate length of time that Il current remains active.

\author	dcofer
\date	3/29/2011

\return	intercept.
**/
float PacemakerNeuron::Btl()
{return m_fltBtl;}

/**
\brief	Sets the intercept used to calculate length of time that Il current remains active.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void PacemakerNeuron::Btl(float fltVal)
{
	m_fltBtl=fltVal;
}

/**
\brief	Gets the time that the high current is active.

\author	dcofer
\date	3/29/2011

\return	Time for high current.
**/
float PacemakerNeuron::Th()
{return m_fltTh;}

/**
\brief	Sets the time that the high current is active.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void PacemakerNeuron::Th(float fltVal)
{
	m_fltTh=fltVal;
}

/**
\brief	Gets the duration for the current mode.

\author	dcofer
\date	3/29/2011

\return	duration.
**/
float PacemakerNeuron::ITime()
{return m_fltITime;}

/**
\brief	Sets the duration for the current mode.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void PacemakerNeuron::ITime(float fltVal)
{
	m_fltITime=fltVal;
}

/**
\brief	Gets the intrinsic current type. (HI or LOW)

\author	dcofer
\date	3/29/2011

\return	current type.
**/
unsigned char PacemakerNeuron::IntrinsicType()
{return m_iIntrinsicType;}

/**
\brief	Sets the intrinsic current type. (HI or LOW)

\author	dcofer
\date	3/29/2011

\param	iVal	The new value. 
**/
void PacemakerNeuron::IntrinsicType(unsigned char iVal)
{
	m_iIntrinsicType=iVal;
}

/**
\brief	Gets the neuron type.

\author	dcofer
\date	3/29/2011

\return	Neuron type.
**/
unsigned char PacemakerNeuron::NeuronType()
{return PACEMAKER_NEURON;}

void PacemakerNeuron::Copy(CStdSerialize *lpSource)
{
	Neuron::Copy(lpSource);

	PacemakerNeuron *lpOrig = dynamic_cast<PacemakerNeuron *>(lpSource);

	m_fltIl = lpOrig->m_fltIl;
	m_fltIh = lpOrig->m_fltIh;
	m_fltVssm = lpOrig->m_fltVssm;
	m_fltMtl = lpOrig->m_fltMtl;
	m_fltBtl = lpOrig->m_fltBtl;
	m_fltTh = lpOrig->m_fltTh;
	m_fltITime = lpOrig->m_fltITime;
	m_fltInterburstInterval = lpOrig->m_fltInterburstInterval;
	m_fltVss = lpOrig->m_fltVss;
	m_iIntrinsicType = lpOrig->m_iIntrinsicType;
}

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

/**
\brief	Turns the high current on.

\author	dcofer
\date	3/29/2011
**/
void PacemakerNeuron::HighCurrentOn()
{
	m_fltITime = m_fltTh;
	m_fltIntrinsicI = m_fltIh;
	m_iIntrinsicType=IH_CURRENT;
}

/**
\brief	Turns the low current on.

\author	dcofer
\date	3/29/2011

\param	fltVss	The steady state voltage. 
**/
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

void PacemakerNeuron::ResetSimulation()
{
	Neuron::ResetSimulation();

	m_fltVss = 0;
	m_fltITime=0;
	m_iIntrinsicType=IH_CURRENT;
	m_fltInterburstInterval = 0;
}

void PacemakerNeuron::StepSimulation()
{
	//float fltTime = lpSim->Time();
	//if(m_strName == "U Obstacle Mem" && fltTime > 3.672)
	//	fltTime=fltTime;

	Neuron::StepSimulation();

//	//Now we need to check if this neuron has crossed the firing threshold.
//	//If it has then we initiate High current.
		if( ((m_aryVn[m_lpFRModule->InactiveArray()] - m_fltVth) > 1e-6) && ((m_aryVn[m_lpFRModule->ActiveArray()] - m_fltVth) < 1e-6) )
			HighCurrentOn();

		if( ((m_aryVn[m_lpFRModule->InactiveArray()] - m_fltVth) < 1e-6) && ((m_aryVn[m_lpFRModule->ActiveArray()] - m_fltVth) > 1e-6) )
			LowCurrentOn(0);
}

long PacemakerNeuron::CalculateSnapshotByteSize()
{
	//We need bytes for the internal state variables for this neuron.
	return (Neuron::CalculateSnapshotByteSize() + sizeof(m_fltITime) + sizeof(m_iIntrinsicType));
}

#pragma region DataAccesMethods

float *PacemakerNeuron::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "STEADYSTATEVOLTAGE")
		return &m_fltVss;

	if(strType == "INTERBURSTINTERVAL")
		return &m_fltInterburstInterval;

	if(strType == "INTERBURSTTIMER")
		return &m_fltITime;

	return Neuron::GetDataPointer(strDataType);
}

bool PacemakerNeuron::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(Neuron::SetData(strDataType, strValue, false))
		return true;

	if(strType == "VSSM")
	{
		Vssm(atof(strValue.c_str()));
		return true;
	}

	if(strType == "BTL")
	{
		Btl(atof(strValue.c_str()));
		return true;
	}

	if(strType == "MTL")
	{
		Mtl(atof(strValue.c_str()));
		return true;
	}
	
	if(strType == "TH")
	{
		Th(atof(strValue.c_str()));
		return true;
	}

	if(strType == "IL")
	{
		Il(atof(strValue.c_str()));
		return true;
	}

	if(strType == "IH")
	{
		Ih(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void PacemakerNeuron::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Neuron::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("SteadStateVoltage", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("InterburstInterval", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("InterburstTimer", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("Vssm", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Btl", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Mtl", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Th", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Il", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Ih", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
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

void PacemakerNeuron::Load(CStdXml &oXml)
{
	Neuron::Load(oXml);

	oXml.IntoElem();  //Into Neuron Element

	Il(oXml.GetChildFloat("Il"));
	Ih(oXml.GetChildFloat("Ih"));
	Vssm(oXml.GetChildFloat("Vssm"));
	Mtl(oXml.GetChildFloat("Mtl"));
	Btl(oXml.GetChildFloat("Btl"));
	Th(oXml.GetChildFloat("Th"));

	oXml.OutOfElem(); //OutOf Neuron Element
}


	}			//Neurons
}				//FiringRateSim

