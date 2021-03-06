/**
\file	Neuron.cpp

\brief	Implements the neuron class.
**/

#include "StdAfx.h"

#include "Synapse.h"
#include "Neuron.h"
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
Neuron::Neuron()
{
	m_lpFRModule = NULL;

	m_bEnabled = true;

	m_aryVn[0]=0.0;
	m_aryVn[1]=0.0;
	m_aryVth[0] = 0;
	m_aryVth[1] = 0;

	m_fltCn = (float) 75e-6;	//Membrane capacitance
	m_fltInvCn = 1/m_fltCn;
	m_fltGn = (float) 0.1e-6;	//Membrane conductance
	m_fltVth = (float) 0;	//Firing frequency voltage threshold
	m_fltVthi = m_fltVth;
	m_fltFmin = (float) 0.25;	//Minimum Firing frequency
	m_fltGain = (float) 0.1e-3;	//Firing frequency gain
	m_fltExternalI = 0;			//Externally injected current
	m_fltIntrinsicI = 0;
	m_fltSynapticI = 0;
	m_fltAdapterI = 0;
	m_fltAdapterMemoryI = 0;
	m_fltTotalMemoryI = 0;
	m_fltVn = 0;
	m_fltFiringFreq = 0;
	m_fltVndisp = 0;
	m_fltVthdisp = 0;
	m_fltVrest = 0;

	m_fltVNoiseMax = (float) 0.1e-4; //Max noise is 0.4 mV
	m_bUseNoise = true;
	m_fltVNoise = 0;

	m_fltDCTH = 0;
	m_fltAccomTimeMod = 0;
	m_fltAccomTimeConst = (float) 100e-3;
	m_fltRelativeAccom = 0;
	m_bUseAccom = false;
	m_fltVthadd = 0;

	m_bGainType = true;

	m_fltIinit = 0;
	m_fltInitTime = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
Neuron::~Neuron()
{

try
{
	m_arySynapses.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Neuron\r\n", "", -1, false, true);}
}

/**
\brief	Gets the membrane capacitance.

\author	dcofer
\date	3/29/2011

\return	membrane capacitance.
**/
float Neuron::Cn()
{return m_fltCn;}

/**
\brief	Sets the membrane capacitance.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void Neuron::Cn(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Cn");

	m_fltCn=fltVal;
	m_fltInvCn = 1/m_fltCn;
}

/**
\brief	Gets the membrane conductance.

\author	dcofer
\date	3/29/2011

\return	membrane conductance.
**/
float Neuron::Gn()
{return m_fltGn;}

/**
\brief	Sets the membrane conductance.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void Neuron::Gn(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Cn");

	m_fltGn=fltVal;
}

/**
\brief	Gets the voltage threshold for firing.

\author	dcofer
\date	3/29/2011

\return	voltage threshold.
**/
float Neuron::Vth()
{return m_fltVth;}

/**
\brief	Sets the voltage threshold for firing.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void Neuron::Vth(float fltVal)
{
	float fltDiff = fltVal - m_fltVrest;

	m_fltVthi = fltDiff;
	m_fltVth = fltDiff;
	m_fltVthdisp = m_fltVrest + m_fltVth;
	m_aryVth[0] = fltDiff;
	m_aryVth[1] = fltDiff;
	m_fltVthadd = fltDiff;
}

/**
\brief	Gets the minimum firing frequency.

\author	dcofer
\date	3/29/2011

\return	minimum firing frequency.
**/
float Neuron::Fmin()
{return m_fltFmin;}

/**
\brief	Sets the minimum firing frequency.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void Neuron::Fmin(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Fmin", true);

	m_fltFmin=fltVal;
}

/**
\brief	Gets the firing frequency gain.

\author	dcofer
\date	3/29/2011

\return	firing frequency gain.
**/
float Neuron::Gain()
{return m_fltGain;}

/**
\brief	Sets the firing frequency gain.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void Neuron::Gain(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Gain");
	m_fltGain=fltVal;
}

/**
\brief	Gets the external current.

\author	dcofer
\date	3/29/2011

\return	external current.
**/
float Neuron::ExternalI()
{return m_fltExternalI;}

/**
\brief	Sets the external current.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void Neuron::ExternalI(float fltVal)
{
	m_fltExternalI=fltVal;
}

/**
\brief	Adds to the external current.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value to add. 
**/
void Neuron::AddExternalI(float fltVal)
{
	m_fltExternalI+=fltVal;
}

/**
\brief	Gets the rest potential.

\author	dcofer
\date	3/29/2011

\return	rest potential.
**/
float Neuron::Vrest()
{return m_fltVrest;}

/**
\brief	Sets the rest potential.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void Neuron::Vrest(float fltVal)
{
	m_fltVrest = fltVal;
	Vth(m_fltVthi);

	if(!m_lpSim->SimRunning())
		m_fltVndisp = m_fltVrest;
}

/**
\brief	Gets the maximum noise voltage.

\author	dcofer
\date	3/29/2011

\return	maximum noise voltage.
**/
float Neuron::VNoiseMax()
{return m_fltVNoiseMax;}

/**
\brief	Gets whether to use noise.

\author	dcofer
\date	3/29/2011

\return	true if it uses noise, false else.
**/
bool Neuron::UseNoise() {return m_bUseNoise;}

/**
\brief	Sets whether to use noise.

\author	dcofer
\date	3/29/2011

\param	bVal	true to use noise. 
**/
void Neuron::UseNoise(bool bVal) 
{
	m_bUseNoise = bVal;
}

/**
\brief	Gets whether to use accommodation.

\author	dcofer
\date	3/29/2011

\return	true to use accommodation, false else.
**/
bool Neuron::UseAccom() {return m_bUseAccom;}

/**
\brief	Sets whether to use accommodation.

\author	dcofer
\date	3/29/2011

\param	bVal	true to use accommodation, false else.
**/
void Neuron::UseAccom(bool bVal)
{
	m_bUseAccom = bVal;

	if(m_bUseAccom && m_lpFRModule)
		m_fltDCTH = exp(-m_lpFRModule->TimeStep()/m_fltAccomTimeConst);
	else
		m_fltDCTH = 0;
}

/**
\brief	Sets the maximum noise voltage.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void Neuron::VNoiseMax(float fltVal)
{
	m_fltVNoiseMax = fltVal;

	if(m_fltVNoiseMax != 0)
		m_bUseNoise = true;
	else
		m_bUseNoise = false;
}

/**
\brief	Gets the relative accomodation.

\author	dcofer
\date	3/29/2011

\return	relative accomodation.
**/
float Neuron::RelativeAccommodation()
{return m_fltRelativeAccom;}

/**
\brief	Sets the relative accomodation.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void Neuron::RelativeAccommodation(float fltVal)
{
	Std_InValidRange((float) 0, (float) 1, fltVal, true, "RelativeAccomodation");

	m_fltRelativeAccom = fltVal;

	if(m_fltRelativeAccom != 0)
		m_bUseAccom = true;
	else
		m_bUseAccom = false;

	if(m_bUseAccom && m_lpFRModule)
		m_fltDCTH = exp(-m_lpFRModule->TimeStep()/m_fltAccomTimeConst);
	else
		m_fltDCTH = 0;
}

/**
\brief	Gets the accomodation time constant.

\author	dcofer
\date	3/29/2011

\return	accomodation time constant.
**/
float Neuron::AccommodationTimeConstant()
{return m_fltAccomTimeConst;}

/**
\brief	Sets the accomodation time constant.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void Neuron::AccommodationTimeConstant(float fltVal)
{
	m_fltAccomTimeConst = fltVal;

	if(m_bUseAccom && m_lpFRModule)
		m_fltDCTH = exp(-m_lpFRModule->TimeStep()/m_fltAccomTimeConst);
	else
		m_fltDCTH = 0;
}

float Neuron::Iinit() {return m_fltIinit;}

void Neuron::Iinit(float fltVal) {m_fltIinit = fltVal;}

float Neuron::InitTime() {return m_fltInitTime;}

void Neuron::InitTime(float fltVal)
{
	Std_InValidRange((float) 0, (float) 10, fltVal, true, "InitTime");
	m_fltInitTime = fltVal;
}

/**
\brief	Gets the gain type. (Old way or new way)

\author	dcofer
\date	3/29/2011

\return	true to use new way, false to use old way.
**/
bool Neuron::GainType()
{return m_bGainType;}

/**
\brief	Sets the gain type. (Old way or new way)

\author	dcofer
\date	3/29/2011

\param	bVal	true to use new way, false to use old way.
**/
void Neuron::GainType(bool bVal)
{
	m_bGainType = bVal;
}

/**
\brief	Gets the membrane voltage.

\author	dcofer
\date	3/29/2011

\return	membrane voltage.
**/
float Neuron::Vn()
{return m_fltVn;}

/**
\brief	Calculates the current firing frequency.

\author	dcofer
\date	3/29/2011

\param [in,out]	m_lpFRModule	Pointer to a fast module. 

\return	firing frequency.
**/
float Neuron::FiringFreq(FiringRateModule *m_lpFRModule)
{
	return CalculateFiringFrequency(m_aryVn[m_lpFRModule->ActiveArray()], m_aryVth[m_lpFRModule->ActiveArray()]);
}

/**
\brief	Gets the intrinsic current.

\author	dcofer
\date	3/29/2011

\return	intrinsic current.
**/
float Neuron::IntrinsicCurrent()
{return m_fltIntrinsicI;}

/**
\brief	Sets the intrinsic current.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new value. 
**/
void Neuron::IntrinsicCurrent(float fltVal)
{m_fltIntrinsicI = fltVal;}

/**
\brief	Gets the neuron type.

\author	dcofer
\date	3/29/2011

\return	neuron type.
**/
unsigned char Neuron::NeuronType()
{return RUGULAR_NEURON;}

void Neuron::Copy(CStdSerialize *lpSource)
{
	Node::Copy(lpSource);

	Neuron *lpOrig = dynamic_cast<Neuron *>(lpSource);

	m_lpFRModule = lpOrig->m_lpFRModule;
	m_fltCn = lpOrig->m_fltCn;
	m_fltInvCn = lpOrig->m_fltInvCn;
	m_fltGn = lpOrig->m_fltGn;
	m_fltFmin = lpOrig->m_fltFmin;
	m_fltGain = lpOrig->m_fltGain;
	m_fltExternalI = lpOrig->m_fltExternalI;
	m_fltIntrinsicI = lpOrig->m_fltIntrinsicI;
	m_fltSynapticI = lpOrig->m_fltSynapticI;
	m_fltAdapterI = lpOrig->m_fltAdapterI;
	m_fltAdapterMemoryI = lpOrig->m_fltAdapterMemoryI;
	m_fltTotalMemoryI = lpOrig->m_fltTotalMemoryI;
	m_fltVNoiseMax = lpOrig->m_fltVNoiseMax;
	m_bUseNoise = lpOrig->m_bUseNoise;
	m_bGainType = lpOrig->m_bGainType;
	m_fltDCTH = lpOrig->m_fltDCTH;
	m_fltAccomTimeMod = lpOrig->m_fltAccomTimeMod;
	m_fltAccomTimeConst = lpOrig->m_fltAccomTimeConst;
	m_fltRelativeAccom = lpOrig->m_fltRelativeAccom;
	m_bUseAccom = lpOrig->m_bUseAccom;
	m_fltVn = lpOrig->m_fltVn;
	m_fltFiringFreq = lpOrig->m_fltFiringFreq;
	m_aryVn[0] = lpOrig->m_aryVn[0];
	m_aryVn[1] = lpOrig->m_aryVn[1];
	m_fltVNoise = lpOrig->m_fltVNoise;
	m_fltVth = lpOrig->m_fltVth;
	m_fltVthi = lpOrig->m_fltVthi;
	m_fltVthadd = lpOrig->m_fltVthadd;
	m_aryVth[0] = lpOrig->m_aryVth[0];
	m_aryVth[1] = lpOrig->m_aryVth[1];
	m_fltVrest = lpOrig->m_fltVrest;
	m_fltVndisp = lpOrig->m_fltVndisp;
	m_fltIinit = lpOrig->m_fltIinit;
	m_fltInitTime = lpOrig->m_fltInitTime;
}

/**
\brief	Gets a pointer to the synapses array.

\author	dcofer
\date	3/29/2011

\return	Pointer to the synapses.
**/
CStdPtrArray<Synapse> *Neuron::GetSynapses()
{return &m_arySynapses;}


void Neuron::AddSynapse(Synapse *lpSynapse)
{
	if(!lpSynapse) 
		THROW_ERROR(Nl_Err_lSynapseToAddNull, Nl_Err_strSynapseToAddNull);
	m_arySynapses.Add(lpSynapse);
}

/**
\brief	Adds a synapse using an xml packet. 

\author	dcofer
\date	3/29/2011

\param	strXml	The xml of the synapse to add. 
**/
void Neuron::AddSynapse(std::string strXml, bool bDoNotInit)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Synapse");

	Synapse *lpSynapse = LoadSynapse(oXml);
	if(!bDoNotInit)
		lpSynapse->Initialize();
}

/**
\brief	Removes the synapse described by iIndex.

\author	dcofer
\date	3/29/2011

\param	iIndex	Zero-based index of the synapse in the array. 
**/
void Neuron::RemoveSynapse(int iIndex)
{
	if( iIndex<0 || iIndex>=m_arySynapses.GetSize() ) 
		THROW_ERROR(Std_Err_lInvalidIndex, Std_Err_strInvalidIndex);
	m_arySynapses.RemoveAt(iIndex);
}

/**
\brief	Removes the synapse by the GUID ID.

\author	dcofer
\date	3/29/2011

\param	strID	   	GUID ID for the synapse to remove. 
\param	bThrowError	true to throw error if synaspe not found. 
**/
void Neuron::RemoveSynapse(std::string strID, bool bThrowError)
{
	int iPos = FindSynapseListPos(strID, bThrowError);
	m_arySynapses.RemoveAt(iPos);
}

/**
\brief	Gets a synapse by its index in the array.

\author	dcofer
\date	3/29/2011

\param	iIndex	Zero-based index of the synaspe to return. 

\return	null if it fails, else the synapse.
**/
Synapse *Neuron::GetSynapse(int iIndex)
{
	if( iIndex<0 || iIndex>=m_arySynapses.GetSize() ) 
		THROW_ERROR(Std_Err_lInvalidIndex, Std_Err_strInvalidIndex);
	return m_arySynapses[iIndex];
}

/**
\brief	Searches for a synapse with the specified ID and returns its position in the list.

\author	dcofer
\date	3/29/2011

\param	strID	   	GUID ID of the synapse to find. 
\param	bThrowError	true to throw error if no synapse is found. 

\return	The found synapse list position.
**/
int Neuron::FindSynapseListPos(std::string strID, bool bThrowError)
{
	std::string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_arySynapses[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Nl_Err_lSynapseNotFound, Nl_Err_strSynapseNotFound, "ID");

	return -1;
}

/**
\brief	Gets the total number of synapses.

\author	dcofer
\date	3/29/2011

\return	The total number of synapses.
**/
int Neuron::TotalSynapses()
{return m_arySynapses.GetSize();}

/**
\brief	Clears the synapses list.

\author	dcofer
\date	3/29/2011
**/
void Neuron::ClearSynapses()
{m_arySynapses.RemoveAll();}

void Neuron::TimeStepModified()
{
	if(m_bUseAccom && m_lpFRModule)
		m_fltDCTH = exp(-m_lpFRModule->TimeStep()/m_fltAccomTimeConst);
	else
		m_fltDCTH = 0;
}

void Neuron::StepSimulation()
{

	if(m_bEnabled)
	{
		////Test code
		//int i=5;
		//if(m_strID == "742CB5DC-6BFB-4BAB-8CC2-36A4725A33D5")
		//	i=6;	

		//Lets get the Summation of synaptic inputs
		m_fltSynapticI = CalculateSynapticCurrent(m_lpFRModule);
		m_fltIntrinsicI = CalculateIntrinsicCurrent(m_lpFRModule, m_fltExternalI+m_fltSynapticI);

		//If we need to apply an init current then do so.
		if(m_fltInitTime > 0 && m_lpSim->Time() < m_fltInitTime)
			m_fltIntrinsicI += m_fltIinit;

		//if(m_fltInitTime > 0 && m_lpSim->Time() >= m_fltInitTime)
		//	m_fltIntrinsicI = m_fltIntrinsicI; //For testing only comment out!!

		if(m_bUseNoise)
			m_fltVNoise = Std_FRand(-m_fltVNoiseMax, m_fltVNoiseMax);
		
		//Get the total current being applied to the neuron.
		m_fltTotalMemoryI = m_fltSynapticI + m_fltIntrinsicI + m_fltExternalI + m_fltAdapterI;

		m_aryVn[m_lpFRModule->InactiveArray()] = m_aryVn[m_lpFRModule->ActiveArray()] + m_fltVNoise + 
							(m_lpFRModule->TimeStep() * m_fltInvCn * 
							(m_fltSynapticI + m_fltIntrinsicI + m_fltExternalI + m_fltAdapterI - 
							(m_aryVn[m_lpFRModule->ActiveArray()]*m_fltGn)));

		m_fltVn = m_aryVn[m_lpFRModule->InactiveArray()];
		m_fltVndisp = m_fltVrest + m_fltVn;

		if(m_bUseAccom)
		{
			if(m_fltAccomTimeMod != 0 && m_lpFRModule)
				m_fltDCTH = exp(-m_lpFRModule->TimeStep()/m_fltAccomTimeMod);

			m_fltVthadd = (m_aryVth[m_lpFRModule->ActiveArray()]-m_fltVthi)*m_fltDCTH + m_fltRelativeAccom*m_fltVn*(1-m_fltDCTH);
			m_aryVth[m_lpFRModule->InactiveArray()] = m_fltVthi + m_fltVthadd;
		}
		else
			m_aryVth[m_lpFRModule->InactiveArray()] = m_fltVthi;

		m_fltVth = m_aryVth[m_lpFRModule->InactiveArray()];
		m_fltVthdisp = m_fltVrest + m_fltVth;

		m_fltFiringFreq = CalculateFiringFrequency(m_fltVn, m_fltVth);
		m_fltAdapterI = 0;  //Reset the adapter current for the next cycle.
	}
}

/**
\brief	Calculates the firing frequency of the neuron.

\author	dcofer
\date	3/29/2011

\param	fltVn 	The membrane potential. 
\param	fltVth	The threshold potential. 

\return	The calculated firing frequency.
**/
float Neuron::CalculateFiringFrequency(float fltVn, float fltVth)
{
	float fltFreq;
	
	if(m_bGainType)
	{
		if(fltVn<fltVth)
			fltFreq=0;
		else
			fltFreq = (m_fltGain*(fltVn-fltVth)) + m_fltFmin;

		//Final insurance
		if(fltFreq<1e-6) fltFreq = 0;
		if(fltFreq>1) fltFreq = 1;
	}
	else
	{
		if(fltVn<fltVth)
			fltFreq = 0;
		else
		{
			fltFreq = m_fltFmin - m_fltGain * fltVth;
			if (fltVn < (1 - fltFreq)/m_fltGain)
				fltFreq = (m_fltGain * fltVn) + fltFreq;
			else
				fltFreq = 1;
		}

		//Final insurance
		if(fltFreq<1e-6) fltFreq = 0;
		if(fltFreq>1) fltFreq = 1;
	}

	return fltFreq;
}

/**
\brief	Calculates the intrinsic current.

\author	dcofer
\date	3/29/2011

\param [in,out]	m_lpFRModule	Pointer to the parent FiringRateModule. 
\param	fltInputCurrent		  	The input current. 

\return	The calculated intrinsic current.
**/
float Neuron::CalculateIntrinsicCurrent(FiringRateModule *m_lpFRModule, float fltInputCurrent)
{return 0;}

/**
\brief	Calculates the total incoming synaptic current.

\author	dcofer
\date	3/29/2011

\param [in,out]	m_lpFRModule	Pointer to the parent FiringRateModule. 

\return	The calculated synaptic current.
**/
float Neuron::CalculateSynapticCurrent(FiringRateModule *m_lpFRModule)
{
	unsigned char iSynapse, iCount;
	float fltSynapticI=0;
	Synapse *lpSynapse=NULL;

	iCount = m_arySynapses.GetSize();
	for(iSynapse=0; iSynapse<iCount; iSynapse++)
	{
		lpSynapse = m_arySynapses[iSynapse];

		if(lpSynapse->Enabled() && lpSynapse->FromNeuron())
			lpSynapse->Process(fltSynapticI); 
	}

	return fltSynapticI;
}

/**
\brief	Injects current into this neuron.

\author	dcofer
\date	3/29/2011

\param	fltVal	The new current to add. 
**/
void Neuron::InjectCurrent(float fltVal)
{m_fltExternalI+=fltVal;}

void Neuron::Initialize()
{
	Node::Initialize();

	if(m_bUseAccom)
		m_fltDCTH = exp(-m_lpFRModule->TimeStep()/m_fltAccomTimeConst);

	int iCount = m_arySynapses.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_arySynapses[iIndex]->Initialize();
} 

void Neuron::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	Node::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, false);

	m_lpFRModule = dynamic_cast<FiringRateModule *>(lpModule);

	if(bVerify) VerifySystemPointers();
}

void Neuron::VerifySystemPointers()
{
	Node::VerifySystemPointers();

	if(!m_lpFRModule)
		THROW_PARAM_ERROR(Al_Err_lUnableToCastNeuralModuleToDesiredType, Al_Err_strUnableToCastNeuralModuleToDesiredType, "ID: ", m_lpFRModule->ID());

	if(!m_lpOrganism) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Link: ", m_strID);
}

void Neuron::ResetSimulation()
{
	AnimatSim::Node::ResetSimulation();

	m_fltExternalI = 0;
	m_fltIntrinsicI = 0;
	m_fltSynapticI = 0;
	m_fltAdapterI = 0;
	m_fltAdapterMemoryI = 0;
	m_fltTotalMemoryI = 0;
	m_fltFiringFreq = 0;
	m_fltVNoise = 0;
	m_aryVn[0]=0;
	m_aryVn[1]=0;
	m_fltVn = 0;
	m_fltVth = m_fltVthi;
	m_fltVndisp = m_fltVrest;
	m_fltVthdisp = m_fltVrest + m_fltVth;
	m_aryVth[0] = m_aryVth[1] = m_fltVth;
	m_fltAccomTimeMod = 0;
	m_fltVthadd = 0;

	int iCount = m_arySynapses.GetSize();
	for(int iSynapse=0; iSynapse<iCount; iSynapse++)
		m_arySynapses[iSynapse]->ResetSimulation();
}

void Neuron::AddExternalNodeInput(int iTargetDataType, float fltInput)
{
	m_fltAdapterI += fltInput;
	m_fltAdapterMemoryI = m_fltAdapterI;
}

#pragma region DataAccesMethods

float *Neuron::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "INTRINSICCURRENT")
		return &m_fltIntrinsicI;

	if(strType == "EXTERNALCURRENT")
		return &m_fltExternalI;

	if(strType == "SYNAPTICCURRENT")
		return &m_fltSynapticI;

	if(strType == "ADAPTERCURRENT")
		return &m_fltAdapterMemoryI;

	if(strType == "TOTALCURRENT")
		return &m_fltTotalMemoryI;

	if(strType == "MEMBRANEVOLTAGE")
		return &m_fltVndisp;

	if(strType == "FIRINGFREQUENCY")
		return &m_fltFiringFreq;

	if(strType == "NOISEVOLTAGE")
		return &m_fltVNoise;

	if(strType == "THRESHOLD")
		return &m_fltVthdisp;

	if(strType == "Gm")
		return &m_fltGn;

	if(strType == "VREST")
		return &m_fltVrest;

	if(strType == "ACCOMTIMEMOD")
		return &m_fltAccomTimeMod;

	//If it was not one of those above then we have a problem.
	THROW_PARAM_ERROR(Nl_Err_lInvalidNeuronDataType, Nl_Err_strInvalidNeuronDataType, "Neuron Data Type", strDataType);

	return NULL;
}

bool Neuron::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(Node::SetData(strDataType, strValue, false))
		return true;

	if(strType == "CM")
	{
		Cn(atof(strValue.c_str()));
		return true;
	}

	if(strType == "GM")
	{
		Gn(atof(strValue.c_str()));
		return true;
	}

	if(strType == "VTH")
	{
		Vth(atof(strValue.c_str()));
		return true;
	}

	if(strType == "VREST")
	{
		Vrest(atof(strValue.c_str()));
		return true;
	}

	if(strType == "RELATIVEACCOMMODATION")
	{
		RelativeAccommodation(atof(strValue.c_str()));
		return true;
	}

	if(strType == "ACCOMMODATIONTIMECONSTANT")
	{
		AccommodationTimeConstant(atof(strValue.c_str()));
		return true;
	}

	if(strType == "VNOISEMAX")
	{
		VNoiseMax(atof(strValue.c_str()));
		return true;
	}

	if(strType == "FMIN")
	{
		Fmin(atof(strValue.c_str()));
		return true;
	}

	if(strType == "GAIN")
	{
		Gain(atof(strValue.c_str()));
		return true;
	}

	if(strType == "GAINTYPE")
	{
		GainType(Std_ToBool(strValue));
		return true;
	}

	if(strType == "ADDEXTERNALCURRENT")
	{
		AddExternalI(atof(strValue.c_str()));
		return true;
	}

	if(strType == "IINIT")
	{
		Iinit(atof(strValue.c_str()));
		return true;
	}

	if(strType == "INITTIME")
	{
		InitTime(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Neuron::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Node::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("IntrinsicCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("ExternalCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("SynapticCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("AdapterCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("TotalCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("MembraneVoltage", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("FiringFrequency", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("NoiseVoltage", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("Threshold", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("AccomTimeMod", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("Cm", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Gm", AnimatPropertyType::Float, AnimatPropertyDirection::Both));
	aryProperties.Add(new TypeProperty("Vth", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Vrest", AnimatPropertyType::Float, AnimatPropertyDirection::Both));
	aryProperties.Add(new TypeProperty("RelativeAccommodation", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("AccommodationTimeConstant", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("VNoiseMax", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Fmin", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Gain", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("GainType", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("AddExternalCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("Iinit", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("InitTime", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

bool Neuron::AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError, bool bDoNotInit)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "SYNAPSE")
	{
		AddSynapse(strXml, bDoNotInit);
		return true;
	}


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

bool Neuron::RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "SYNAPSE")
	{
		RemoveSynapse(strID, bThrowError);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

#pragma endregion

long Neuron::CalculateSnapshotByteSize()
{
	//We need bytes for the internal state variables for this neuron.
	return (sizeof(m_aryVn) + sizeof(m_fltExternalI) + sizeof(m_fltIntrinsicI) + sizeof(m_fltSynapticI) + sizeof(m_fltVn) + sizeof(m_fltFiringFreq));
}

void Neuron::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	memcpy((void *) (aryBytes+lIndex), (void *)m_aryVn, sizeof(m_aryVn));
  lIndex += sizeof(m_aryVn);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_fltExternalI, sizeof(m_fltExternalI));
  lIndex += sizeof(m_fltExternalI);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_fltIntrinsicI, sizeof(m_fltIntrinsicI));
  lIndex += sizeof(m_fltIntrinsicI);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_fltSynapticI, sizeof(m_fltSynapticI));
  lIndex += sizeof(m_fltSynapticI);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_fltVn, sizeof(m_fltVn));
  lIndex += sizeof(m_fltVn);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_fltFiringFreq, sizeof(m_fltFiringFreq));
  lIndex += sizeof(m_fltFiringFreq);
}

void Neuron::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	memcpy((void *)m_aryVn, (void *) (aryBytes+lIndex) , sizeof(m_aryVn));
  lIndex += sizeof(m_aryVn);

	memcpy((void *)&m_fltExternalI, (void *) (aryBytes+lIndex), sizeof(m_fltExternalI));
  lIndex += sizeof(m_fltExternalI);

	memcpy((void *)&m_fltIntrinsicI, (void *) (aryBytes+lIndex), sizeof(m_fltIntrinsicI));
  lIndex += sizeof(m_fltIntrinsicI);

	memcpy((void *)&m_fltSynapticI, (void *) (aryBytes+lIndex), sizeof(m_fltSynapticI));
  lIndex += sizeof(m_fltSynapticI);

	memcpy((void *)&m_fltVn, (void *) (aryBytes+lIndex), sizeof(m_fltVn));
  lIndex += sizeof(m_fltVn);

	memcpy((void *)&m_fltFiringFreq, (void *) (aryBytes+lIndex), sizeof(m_fltFiringFreq));
  lIndex += sizeof(m_fltFiringFreq);
}

void Neuron::Load(CStdXml &oXml)
{
	int iCount, iIndex;

	Node::Load(oXml);

	oXml.IntoElem();  //Into Neuron Element

	m_arySynapses.RemoveAll();

	Enabled(oXml.GetChildBool("Enabled", true));

	Cn(oXml.GetChildFloat("Cn"));
	Gn(oXml.GetChildFloat("Gn"));
	Vrest(oXml.GetChildFloat("Vrest", 0));
	Vth(oXml.GetChildFloat("Vth"));
	Fmin(oXml.GetChildFloat("Fmin"));
	Gain(oXml.GetChildFloat("Gain"));
	ExternalI(oXml.GetChildFloat("ExternalI"));
	VNoiseMax(fabs(oXml.GetChildFloat("VNoiseMax", m_fltVNoiseMax)));
	Iinit(oXml.GetChildFloat("Iinit", m_fltIinit));
	InitTime(oXml.GetChildFloat("InitTime", m_fltInitTime));

	m_fltVndisp = m_fltVrest;
	m_fltVthdisp = m_fltVrest + m_fltVth;

	GainType(oXml.GetChildBool("GainType", true));

	m_aryVth[0] = m_aryVth[1] = m_fltVth;

	if(m_fltVNoiseMax != 0)
		UseNoise(true);
	else
		UseNoise(false);

	RelativeAccommodation(fabs(oXml.GetChildFloat("RelativeAccom", m_fltRelativeAccom)));
	AccommodationTimeConstant(fabs(oXml.GetChildFloat("AccomTimeConst", m_fltAccomTimeConst)));

	if(m_fltRelativeAccom != 0)
		UseAccom(true);
	else
		UseAccom(false);

	//*** Begin Loading Synapses. *****
	if(oXml.FindChildElement("Synapses", false))
	{
		oXml.IntoElem();  //Into Synapses Element

		iCount = oXml.NumberOfChildren();
		for(iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadSynapse(oXml);
		}

		oXml.OutOfElem();
	}
	//*** End Loading Synapses. *****


	oXml.OutOfElem(); //OutOf Neuron Element
}

/**
\brief	Loads a synapse.

\author	dcofer
\date	3/29/2011

\param [in,out]	oXml	The xml to load. 

\return	Pointer to the created synapse.
**/
Synapse *Neuron::LoadSynapse(CStdXml &oXml)
{
	std::string strType;
	Synapse *lpSynapse=NULL;

try
{
	oXml.IntoElem();  //Into Synapse Element
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Synapse Element

	lpSynapse = dynamic_cast<Synapse *>(m_lpSim->CreateObject(Nl_NeuralModuleName(), "Synapse", strType));
	if(!lpSynapse)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Synapse");

	lpSynapse->SetSystemPointers(m_lpSim, m_lpStructure, m_lpFRModule, this, true);
	lpSynapse->Load(oXml);
	AddSynapse(lpSynapse);

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

	}			//Neurons
}				//FiringRateSim



