/**
\file	IntegrateFireSim\Neuron.cpp

\brief	Implements the neuron class.
**/

#include "stdafx.h"
#include "IonChannel.h"
#include "SynapseType.h"
#include "IntegrateFireModule.h"
#include "Connexion.h"
#include "CaActivation.h"
#include "Neuron.h"
#include "ElectricalSynapse.h"
#include "NonSpikingChemicalSynapse.h"
#include "SpikingChemicalSynapse.h"

namespace IntegrateFireSim
{


double Neuron::m_dSpikePeak=0;
double Neuron::m_dSpikeStrength=1;
double Neuron::m_dAHPEquilPot=-70;		// equil pot for K
double Neuron::m_dCaEquilPot=200;
double Neuron::m_dAbsoluteRefr=2;
long Neuron::m_lAbsoluteRefr=0;
double Neuron::m_dDT=0.5;

/**
\brief	Default constructor.

\author	dcofer
\date	3/30/2011
**/
Neuron::Neuron()
{

	m_bZapped = false;
	m_dRestingPot = -70;
	m_dSize = 1;
	m_dTimeConst = 0;
	m_dInitialThresh = 0;
	m_dRelativeAccom = 0;
	m_dAccomTimeConst = 0;
	m_dAHPAmp = 0;
	m_dAHPTimeConst = 0;
	m_dGMaxCa = 0;
	m_dVM = 0;		
	m_dSM = 0;		
	m_dMTimeConst = 0;	
	m_dVH = 0;
	m_dSH = 0;
	m_dHTimeConst = 0;
	m_fltGTotal = 0;

	m_dToniCurrentStimulusulus = 0;
	m_dNoise = 0;

	m_dMemPot = 0;
	m_dNewMemPot = 0;
	m_dThresh = 0;
	m_bSpike = false;
	m_fltEMemory = 0;

	m_dElecSynCur = 0;
	m_dElecSynCond = 0;
	m_dNonSpikingSynCur = 0;
	m_dNonSpikingSynCond = 0;
	m_lRefrCountDown = 0;
 	m_dDCTH = 0;	
	m_dDGK = 0;	
	m_dGK = 0;
	m_dGTot = 0;

	m_dStim = 0;;

	m_dM = 0;
	m_dH = 0;
	m_fltEMemory = 0;
	m_bBurstInitAtBottom = true;

	m_iNeuronID = 0;
	m_fltAdapterI = 0;
	m_fltAdapterMemoryI = 0;
	m_fltExternalI = 0;
	m_fltChannelI = 0;
	m_fltChannelMemoryI = 0;
	m_fltICaMemory = 0;
	m_fltMemPot = 0;
	m_fltThresholdMemory = 0;
	m_fltLastSpikeTime = 0;
	m_fltFiringFreq = 0;
	m_fltElecSynCurMemory = 0;
	m_fltSpikingSynCurMemory = 0;
	m_fltNonSpikingSynCurMemory = 0;
	m_iIonChannels = 0;
	m_fltSpike = 0;
	m_dCm = 0;
	m_fltGm = 0;
	m_fltVrest = 0;
	m_fltTotalI = 0;
	m_fltTotalMemoryI = 0;
	m_lpCaActive = NULL;
	m_lpCaInactive = NULL;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/30/2011
**/
Neuron::~Neuron()
{
	if(m_lpCaActive)
		{delete m_lpCaActive; m_lpCaActive = NULL;}

	if(m_lpCaInactive)
		{delete m_lpCaInactive; m_lpCaInactive = NULL;}
}


#pragma region Accessor-Mutators

/**
\brief	Gets the neuron ID.

\author	dcofer
\date	3/30/2011

\return	Neuron ID.
**/
int Neuron::NeuronID() {return m_iNeuronID;}

/**
\brief	Sets the Neuron ID.

\author	dcofer
\date	3/30/2011

\param	iID	The identifier. 
**/
void Neuron::NeuronID(int iID) {m_iNeuronID = iID;}

bool Neuron::Enabled() {return m_bZapped;}

void Neuron::Enabled(bool bValue) 
{
	Node::Enabled(bValue);
	m_bZapped = !bValue;
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
\brief	Gets the resting potential.

\author	dcofer
\date	3/30/2011

\return	The resting potential.
**/
double Neuron::GetRestingPot() {return m_dRestingPot;}

/**
\brief	Gets the membrane potential.

\author	dcofer
\date	3/30/2011

\return	The membrane potential.
**/
double Neuron::GetMemPot() {return m_bZapped?0:(m_bSpike?m_dSpikePeak:m_dMemPot);}

/**
\brief	Gets the votlge threshold.

\author	dcofer
\date	3/30/2011

\return	The votlge threshold.
**/
double Neuron::GetThresh() {return m_bZapped?0:m_dThresh;}

/**
\brief	Gets whether a spike occured.

\author	dcofer
\date	3/30/2011

\return	true if it spiked, false else.
**/
bool Neuron::GetSpike() {return m_bZapped?false:m_bSpike;}

/**
\brief	Gets if the neuron is disabled.

\author	dcofer
\date	3/30/2011

\return	true if it is disabled, false else.
**/
bool Neuron::GetZapped() {return m_bZapped;}

/**
\brief	Increments the current stimulus.

\author	dcofer
\date	3/30/2011

\param	stim	The stim. 
**/
void Neuron::IncrementStim(double stim) {m_dStim+=stim;}

/**
\brief	Increments electrical synapse current.

\author	dcofer
\date	3/30/2011

\param	cur	The current. 
**/
void Neuron::InElectricalSynapseCurr(double cur) {m_dElecSynCur+=cur;}

/**
\brief	Increments electrical synapse conductance.

\author	dcofer
\date	3/30/2011

\param	cond	The conductance. 
**/
void Neuron::InElectricalSynapseCond(double cond) {m_dElecSynCond+=cond;}

/**
\brief	Increment non-spiking syn current.

\author	dcofer
\date	3/30/2011

\param	cur	The current. 
**/
void Neuron::IncNonSpikingSynCurr(double cur) {m_dNonSpikingSynCur+=cur;}

/**
\brief	Increment non-spiking syn conductance.

\author	dcofer
\date	3/30/2011

\param	cond	The conductance. 
**/
void Neuron::IncNonSpikingSynCond(double cond) {m_dNonSpikingSynCond+=cond;}

/**
\brief	Gets the ion channels.

\author	dcofer
\date	3/30/2011

\return	Pointer to array of ion channels.
**/
CStdPtrArray<IonChannel> *Neuron::IonChannels() {return &m_aryIonChannels;}

/**
\brief	Sets the resting potential.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::RestingPotential(double dVal) 
{
	//The mempot variables are calculated, so we do not want to just re-set them to the new value.
	//instead lets adjust them by the difference between the old and new resting potential.
	double dDiff = dVal - m_dRestingPot;

	m_dRestingPot = dVal;
	m_dMemPot += dDiff;
	m_dNewMemPot += dDiff;
}

/**
\brief	Gets the resting potential.

\author	dcofer
\date	3/30/2011

\return	resting potential.
**/
double Neuron::RestingPotential() {return m_dRestingPot;}

/**
\brief	Sets the size of the neuron.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::Size(double dVal) 
{
	m_dSize = dVal;
	m_fltGm = (float) (1/(m_dSize*1e6));
	m_dCm = m_dTimeConst*m_dSize;
}

/**
\brief	Gets the size of the neuron.

\author	dcofer
\date	3/30/2011

\return	size.
**/
double Neuron::Size() {return m_dSize;}

/**
\brief	Sets the time constant.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::TimeConstant(double dVal) 
{
	m_dTimeConst = dVal;
	m_dCm = m_dTimeConst*m_dSize;
}

/**
\brief	Gets the time constant.

\author	dcofer
\date	3/30/2011

\return	time constant.
**/
double Neuron::TimeConstant() {return m_dTimeConst;}

/**
\brief	Sets the initial threshold.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::InitialThreshold(double dVal) 
{
	m_dInitialThresh = dVal;
	m_dThresh=m_dInitialThresh;
	m_fltThresholdMemory = (float) m_dThresh * 0.001;
}

/**
\brief	Gets the initial threshold.

\author	dcofer
\date	3/30/2011

\return	initial threshold.
**/
double Neuron::InitialThreshold() {return m_dInitialThresh;}

/**
\brief	Sets the relative accomodation value.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::RelativeAccomodation(double dVal) {m_dRelativeAccom = dVal;}

/**
\brief	Gets the relative accomodation.

\author	dcofer
\date	3/30/2011

\return	relative accomodation.
**/
double Neuron::RelativeAccomodation() {return m_dRelativeAccom;}

/**
\brief	Sets the accomodation time constant.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::AccomodationTimeConstant(double dVal) 
{
	m_dAccomTimeConst = dVal;
	m_dDCTH=exp(-m_dDT/m_dAccomTimeConst);
}

/**
\brief	Gets the accomodation time constant.

\author	dcofer
\date	3/30/2011

\return	accomodation time constant.
**/
double Neuron::AccomodationTimeConstant() {return m_dAccomTimeConst;}

/**
\brief	Sets the after-hyperpolarizing conductance amplitude.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::AHPAmplitude(double dVal) {m_dAHPAmp = dVal;}

/**
\brief	Gets the after-hyperpolarizing conductance amplitude.

\author	dcofer
\date	3/30/2011

\return	AHP Amplitude.
**/
double Neuron::AHPAmplitude() {return m_dAHPAmp;}

/**
\brief	Sets the after-hyperpolarizing time constant.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::AHPTimeConstant(double dVal) 
{
	m_dAHPTimeConst = dVal;
	m_dDGK=exp(-m_dDT/m_dAHPTimeConst);
}

/**
\brief	Gets the after-hyperpolarizing time constant.

\author	dcofer
\date	3/30/2011

\return	AHP time constant.
**/
double Neuron::AHPTimeConstant() {return m_dAHPTimeConst;}

/**
\brief	Sets the burst maximum calcium conductance.

\author	dcofer
\date	3/30/2011

\param	dVal	The value. 
**/
void Neuron::BurstGMaxCa(double dVal) 
{
	m_dGMaxCa = dVal;

	if (m_dGMaxCa>0 && m_bBurstInitAtBottom) 
	{
		m_dMemPot=m_dRestingPot+7.408;
		m_dM=0.0945;
		m_dH=0.0208;
	}
	else
	{
		m_dMemPot=m_dRestingPot;
		m_dM=0.0f;
		m_dH=0.0f;
	}

}

/**
\brief	Gets the burst maximum calcium conductance.

\author	dcofer
\date	3/30/2011

\return	conductance.
**/
double Neuron::BurstGMaxCa() {return m_dGMaxCa;}

/**
\brief	Sets the burst activation mid point.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::BurstVm(double dVal) {m_dVM = dVal;}

/**
\brief	Gets the burst activation mid point.

\author	dcofer
\date	3/30/2011

\return	Mid-point.
**/
double Neuron::BurstVm() {return m_dVM;}

/**
\brief	Sets the burst Activation slope.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::BurstSm(double dVal) {m_dSM = dVal;}

/**
\brief	Gets the burst Activation slope.

\author	dcofer
\date	3/30/2011

\return	slope.
**/
double Neuron::BurstSm() {return m_dSM;}

/**
\brief	Gets the burst activation time constant.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::BurstMTimeConstant(double dVal) {m_dMTimeConst = dVal;}

/**
\brief	Gets the burst activation time constant.

\author	dcofer
\date	3/30/2011

\return	time constant.
**/
double Neuron::BurstMTimeConstant() {return m_dMTimeConst;}

/**
\brief	Sets the burst inactivation mid point.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::BurstVh(double dVal) {m_dVH = dVal;}

/**
\brief	Gets the burst inactivation mid point.

\author	dcofer
\date	3/30/2011

\return	mid point.
**/
double Neuron::BurstVh() {return m_dVH;}

/**
\brief	Sets the burst inactivation slope.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::BurstSh(double dVal) {m_dSH = dVal;}

/**
\brief	Gets the burst inactivation slope.

\author	dcofer
\date	3/30/2011

\return	slope.
**/
double Neuron::BurstSh() {return m_dSH;}

/**
\brief	Sets the burst inactivation time constant.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::BurstHTimeConstant(double dVal) {m_dHTimeConst = dVal;}

/**
\brief	Gets the burst inactivation time constant.

\author	dcofer
\date	3/30/2011

\return	time constant.
**/
double Neuron::BurstHTimeConstant() {return m_dHTimeConst;}

/**
\brief	Sets the burst inactivation time constant.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void Neuron::BurstInitAtBottom(bool bVal) 
{
	m_bBurstInitAtBottom = bVal;
	BurstGMaxCa(m_dGMaxCa);
}

/**
\brief	Gets the burst inactivation time constant.

\author	dcofer
\date	3/30/2011

\return	time constant.
**/
bool Neuron::BurstInitAtBottom() {return m_bBurstInitAtBottom;}

/**
\brief	Sets the neurons tonic stimulus current.

\author	dcofer
\date	2/2/2012

\param	dblVal	The stimulus value.
**/
void Neuron::TonicStimulus(double dblVal)
{
	m_dToniCurrentStimulusulus = dblVal;
}

/**
\brief	Gets the tonic stimulus current of the neuron.

\author	dcofer
\date	2/2/2012

\return	Tonic stimulus current.
**/
double Neuron::TonicStimulus() {return m_dToniCurrentStimulusulus;}

void Neuron::TonicNoise(double dblVal)
{
	m_dNoise = dblVal;
}

double Neuron::TonicNoise() {return m_dNoise;}

#pragma endregion


void Neuron::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, AnimatSim::Behavior::NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	Node::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, false);
	
	m_lpIGFModule = dynamic_cast<IntegrateFireNeuralModule *>(lpModule);

	if(bVerify) VerifySystemPointers();
}

void Neuron::VerifySystemPointers()
{
	Node::VerifySystemPointers();

	if(!m_lpIGFModule)
		THROW_PARAM_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined, "IGFModule: ", m_strID);
}

void Neuron::Load(CStdXml &oXml)
{
	int i;
	int j;
	double d;

	m_aryTonicInputPeriod.RemoveAll();
	m_aryTonicInputPeriodType.RemoveAll();

	Node::Load(oXml);

	oXml.IntoElem();  //Into Neuron Element
	Enabled(oXml.GetChildBool("Enabled", true));
	m_dToniCurrentStimulusulus=oXml.GetChildDouble("TonicStimulus");
	m_dNoise=oXml.GetChildDouble("Noise");
	m_dRestingPot=oXml.GetChildDouble("RestingPot");
	m_dSize=oXml.GetChildDouble("Size");
	m_dTimeConst=oXml.GetChildDouble("TimeConst");
	m_dInitialThresh=oXml.GetChildDouble("InitialThresh");
	m_dRelativeAccom=oXml.GetChildDouble("RelativeAccom");
	m_dAccomTimeConst=oXml.GetChildDouble("AccomTimeConst");
	m_dAHPAmp=oXml.GetChildDouble("AHPAmp");
	m_dAHPTimeConst=oXml.GetChildDouble("AHPTimeConst");
	m_dGMaxCa=oXml.GetChildDouble("GMaxCa");
	m_bBurstInitAtBottom=oXml.GetChildBool("BurstInitAtBottom", m_bBurstInitAtBottom);

	if(oXml.FindChildElement("CaActivation", false))
	{
		if(m_lpCaActive)
		{delete m_lpCaActive; m_lpCaActive = NULL;}
		m_lpCaActive = new CaActivation(this, "ACTIVE");
		m_lpCaActive->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, this, true);
		m_lpCaActive->Load(oXml);
	}

	if(oXml.FindChildElement("CaDeactivation", false))
	{
		if(m_lpCaInactive)
		{delete m_lpCaInactive; m_lpCaInactive = NULL;}
		m_lpCaInactive = new CaActivation(this, "INACTIVE");
		m_lpCaInactive->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, this, true);
		m_lpCaInactive->Load(oXml);
	}

	m_fltGm = (float) (1/(m_dSize*1e6));
	m_fltVrest = (float) (m_dRestingPot*1e-3);
	
	if(oXml.FindChildElement("NeuronTonicInputs", false) )
	{
		oXml.IntoElem();
		int iTotalNeuronTonicInputs = oXml.NumberOfChildren();
		for(i=0; i<iTotalNeuronTonicInputs; i++)
		{
			oXml.FindChildByIndex(i);
			oXml.IntoElem();  //Into NeuronTonicInput Element
			d=oXml.GetChildDouble("TonicInputPeriod");
			j=oXml.GetChildInt("TonicInputPeriodType");
			m_aryTonicInputPeriod.Add(d);
			m_aryTonicInputPeriodType.Add(j);
			oXml.OutOfElem(); //OutOf NeuronTonicInput Element
		}
		oXml.OutOfElem(); //OutOf NeuronTonicInputs Element
	}
	else
	{
		int iSpikingChemSynCount=m_lpIGFModule->GetSpikingChemSynCount();
		for(i=0; i<iSpikingChemSynCount; i++)
		{
			m_aryTonicInputPeriod.Add(0);
			m_aryTonicInputPeriodType.Add(0);
		}
	}


	m_aryIonChannels.RemoveAll();
	if(oXml.FindChildElement("IonChannels", false) )
	{
		oXml.IntoElem();

		m_iIonChannels = oXml.NumberOfChildren();
		for(int iIndex=0; iIndex<m_iIonChannels; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadIonChannel(oXml);		
		}

		oXml.OutOfElem();
	}

	oXml.OutOfElem(); //OutOf Neuron Element
}

/**
\brief	Loads an ion channel.

\author	dcofer
\date	3/30/2011

\param [in,out]	oXml	The xml to load. 

\return	Pointer to the loaded ion channel.
**/
IonChannel *Neuron::LoadIonChannel(CStdXml &oXml)
{
	IonChannel *lpIonChannel = NULL;

try
{
	lpIonChannel = new IonChannel();
	lpIonChannel->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, this, true);
	lpIonChannel->Load(oXml);
	m_aryIonChannels.Add(lpIonChannel);

	return lpIonChannel;
}
catch(CStdErrorInfo oError)
{
	if(lpIonChannel) delete lpIonChannel;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpIonChannel) delete lpIonChannel;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

//void Neuron::ClearSpikeTimes()
//{
//	for(int iIndex=0; iIndex<m_iSpikesToKeepForFreqAnal; iIndex++)
//		m_arySpikeTimes[iIndex] = -1;
//}
/*
void Neuron::StoreSpikeForFreqAnalysis(IntegrateFireNeuralModule *lpNS)
{
	//First push all of the current values down the list.
	for(int iIndex=(m_iSpikesToKeepForFreqAnal-1); iIndex>0; iIndex--)
		m_arySpikeTimes[iIndex] = m_arySpikeTimes[iIndex-1];

	//Now put this one on as the first one.
	m_arySpikeTimes[0] = lpNS->GetCurrentTime();
}
*/

//void Neuron::CalculateFiringFreq(IntegrateFireNeuralModule *lpNS)
//{
//	int iIndex = 0, iSpikeCount = 0;
//	double fltDiff=0, dblLastSpike=0;
//
//	//First push all of the current values down the list.
//	for(iIndex=(m_iSpikesToKeepForFreqAnal-1); iIndex>0; iIndex--)
//		m_arySpikeTimes[iIndex] = m_arySpikeTimes[iIndex-1];
//
//	//Now put this one on as the first one.
//	m_arySpikeTimes[0] = lpNS->GetCurrentTime();
//
//
//	//Lets loop through the stack of past spike times to count how many to use.
//	iIndex = 0;
//	while(iIndex<m_iSpikesToKeepForFreqAnal && m_arySpikeTimes[iIndex] >= 0)
//	{
//		fltDiff = lpNS->GetCurrentTime() - m_arySpikeTimes[iIndex];
//
//		//Only use spikes that occurred within 1 second of the current time.
//		if(fltDiff <= 1000)
//		{
//			iSpikeCount++;
//			dblLastSpike = m_arySpikeTimes[iIndex];
//		}
//		else
//			m_arySpikeTimes[iIndex] = -1;
//
//		iIndex++;
//	}
//
//	if(iSpikeCount>5 && dblLastSpike >0)
//	{
//		if(dblLastSpike == lpNS->GetCurrentTime())
//			m_fltFiringFreq = 1;
//		else
//			m_fltFiringFreq = iSpikeCount/((lpNS->GetCurrentTime()-dblLastSpike)/1000);
//	}
//	else
//		m_fltFiringFreq = 0;
//
//}

/**
\brief	Calculates the firing freq.

\author	dcofer
\date	3/30/2011

\param [in,out]	lpNS	Pointer to the neural module. 
**/
void Neuron::CalculateFiringFreq(IntegrateFireNeuralModule *lpNS)
{
	if(m_bSpike)
	{
		double dblDiff = (lpNS->GetCurrentTime() - m_fltLastSpikeTime)/(double) 1000.0;
		
		if(dblDiff > 0)
			m_fltFiringFreq = 1/dblDiff;
		else
			m_fltFiringFreq = NO_FREQ_DATA;

		m_fltLastSpikeTime = lpNS->GetCurrentTime();
	}
	else
		m_fltFiringFreq = NO_FREQ_DATA;
}

///////////////////////////////////////
// ENGINE

/**
\brief	Initialization routine.

\author	dcofer
\date	3/30/2011

\param [in,out]	lpNS	Pointer to the neural module. 
**/
void Neuron::PreCalc(IntegrateFireNeuralModule *lpNS)
{
//Std_TraceMsg(0,"In Neuron::PreCalc");

	int i;
	int iSpikingChemSynCount=lpNS->GetSpikingChemSynCount();

	//Size is in Mohms, Current is in na, and volt is in mv, Time is in milliseconds.
	//so T=RC => C = T/R, where R = 1/Size. You make the size smaller it increases Rm.
	//So C = T*Size; C needs to be nF, (mS/Mohm) = nF
	m_dCm = m_dTimeConst*m_dSize;

	m_arySynG.SetSize(iSpikingChemSynCount);
    m_aryDG.SetSize(iSpikingChemSynCount);
	m_aryFacilD.SetSize(iSpikingChemSynCount);

    m_aryFacilSponSynG.SetSize(iSpikingChemSynCount);
	m_aryNextSponSynTime.SetSize(iSpikingChemSynCount);
	m_aryTonicInputPeriodType.SetSize(iSpikingChemSynCount);
	m_aryTonicInputPeriod.SetSize(iSpikingChemSynCount);

	SpikingChemicalSynapse *lpSCSyn=NULL;
	for (i=0; i<iSpikingChemSynCount; i++) 
	{
		lpSCSyn = lpNS->GetSpikingChemSynAt(i);
		if (lpSCSyn->m_dSynAmp==0)
			continue;
		m_arySynG[i]=0.;
		if (m_aryTonicInputPeriodType[i]==0)
			m_aryNextSponSynTime[i]=0.;
		else
			m_aryNextSponSynTime[i]=(-m_aryTonicInputPeriod[i]*log(double (Std_LRand(0, RAND_MAX))/double(RAND_MAX))+1);
		m_aryFacilSponSynG[i]=lpSCSyn->m_dSynAmp;

		m_aryDG[i]=exp(-m_dDT/lpSCSyn->m_dDecay);
		m_aryFacilD[i]=exp(-m_dDT/lpSCSyn->m_dFacilDecay);
		
        //Std_TraceMsg(0, "I: " + STR(i) + "  SynG: " + STR(m_arySynG[i]) + "  NextSyn: " + STR(m_aryNextSponSynTime[i]) + "  FacilG: " + STR(m_aryFacilSponSynG[i]) + "  DG: " + STR(m_aryDG[i]) + "  FacilD: " + STR(m_aryFacilD[i]));
	}

	m_dGK=0;
	m_bSpike=false;
	m_dDCTH=exp(-m_dDT/m_dAccomTimeConst);
	m_dDGK=exp(-m_dDT/m_dAHPTimeConst);
	m_lRefrCountDown=0;
	
	m_dMemPot=m_dNewMemPot=m_dRestingPot;
	m_dThresh=m_dInitialThresh;
	m_fltThresholdMemory = (float) m_dThresh * 0.001;

// burster bits
// initialise to bottom of burst??
	if (m_dGMaxCa>0 && m_bBurstInitAtBottom) 
	{
		m_dMemPot=m_dRestingPot+7.408;
		m_dM=0.0945;
		m_dH=0.0208;
	}
	else
	{
		m_dMemPot=m_dRestingPot;
		m_dM=0.0f;
		m_dH=0.0f;
	}

	m_dElecSynCur=m_dElecSynCond=0;
	m_dNonSpikingSynCur=m_dNonSpikingSynCond=0;

	//if(m_arySpikeTimes)
	//{
	//	delete m_arySpikeTimes;
	//	m_arySpikeTimes = NULL;
	//}

	//m_arySpikeTimes = new double[m_iSpikesToKeepForFreqAnal];
	//ClearSpikeTimes();

	m_dStim=0;
	m_fltAdapterI = 0;
	m_fltAdapterMemoryI = 0;
	m_fltMemPot = m_dMemPot * 0.001;
}

/**
\brief	Calculates the final update during a step.

\author	dcofer
\date	3/30/2011

\param [in,out]	lpNS	Pointer to the neural module. 
**/
void Neuron::CalcUpdateFinal(IntegrateFireNeuralModule *lpNS)
{
	int i;
	m_dMemPot=m_dNewMemPot;

	if (m_lRefrCountDown>0)
	{
		m_lRefrCountDown--;
		m_bSpike=false;
	}
	else if (lpNS->TTX() || lpNS->HH())
		m_bSpike=false;
	else
		m_bSpike=(m_dMemPot>m_dThresh) ? true : false;
	
	if (m_bSpike)
		m_lRefrCountDown=m_lAbsoluteRefr;

	m_fltSpike = (float) m_bSpike;
	CalculateFiringFreq(lpNS);

	for (i=0; i<m_arySynG.size(); i++)
	{
		if (m_arySynG[i]>0)
			m_arySynG[i]*=m_aryDG[i];	// decrease previous syn G exponentially with time, unless never increased
	}

	m_fltMemPot = (m_bZapped?0:(m_bSpike?m_dSpikePeak:m_dMemPot)) * 0.001;
	m_fltThresholdMemory = (float) m_dThresh * 0.001;

	//ASSERT(m_dStim==0.);	// ready for next iteration
}

/**
\brief	Called when simulation is ended.

\author	dcofer
\date	3/30/2011

\param [in,out]	lpNS	Pointer to the neural module. 
**/
void Neuron::PostCalc(IntegrateFireNeuralModule *lpNS)
{
	m_arySynG.RemoveAll();	// current conductance of each synaptic type
    m_aryFacilSponSynG.RemoveAll();	// facilitated initial g increase caused by input
    m_aryDG.RemoveAll();	// exponential decline factor in syn G
	m_aryFacilD.RemoveAll();		// exponential decline factor in facilitation
	m_aryNextSponSynTime.RemoveAll();	// time to next occurrence of this syn type
}

/**
\brief	Calculates the update during a time step.

\author	dcofer
\date	3/30/2011

\param [in,out]	lpNS	Pointer to the neural module. 
**/
void Neuron::CalcUpdate(IntegrateFireNeuralModule *lpNS)
{
	double GS,GSI;		// total synaptic cond, current
	double gCa,iCa;		// Ca cond, current
	double E,DCE;			// mempot-restpot,expon decline factor in mem pot to resting pot
	int i;

	double testD;

	if (m_bZapped)		// don't bother calculating anything else if zapped
		return;
 
// do tonic current input
	m_dStim+= (m_dToniCurrentStimulusulus + m_fltAdapterI + (m_fltExternalI*1e9));
	m_fltAdapterI = 0;
	m_fltElecSynCurMemory = m_dElecSynCur * 1e-9;
	m_fltNonSpikingSynCurMemory = m_dNonSpikingSynCur * 1e-9;

//Go through the ion channels and calculate their currents.
//Convert time from ms to s, and membrane voltage from mv to v
	m_fltChannelMemoryI = 0;
	float fltStep = lpNS->GetTimeStep()*1e-3f;
	float fltVm = m_dMemPot*1e-3f;
	for(int iChannel=0; iChannel<m_iIonChannels; iChannel++)
		m_fltChannelMemoryI+=m_aryIonChannels[iChannel]->CalculateCurrent(fltStep, fltVm);
	m_fltChannelI=m_fltChannelMemoryI*1e9f;  //Currents are always in nA in this model. 

// adjust current injection for size of cell
	m_dStim/=m_dSize;
	m_fltChannelI/=m_dSize;

// Do spontaneous spiking synaptic input
// Each neuron can get tonic input from any synaptic type which
// acts as if a single unknown pre-synaptic neuron of that type inputs either
// regular (type==0) or random (type==1) PSPs.

	if (!lpNS->Cd())		// cadmium blocks all chem synapses & gCa
	{
		int iSpikingChemSynCount=lpNS->GetSpikingChemSynCount();
		for (i=0; i<iSpikingChemSynCount; i++)
		{
			SpikingChemicalSynapse *pSyn=lpNS->GetSpikingChemSynAt(i);
			if (pSyn->m_dSynAmp==0)
				continue;
			
			if (m_aryTonicInputPeriod[i]>0)	// if there is spontaneous input
			{
				if (pSyn->m_dRelFacil!=1)	// if facil exists, decrease its amount with time
					m_aryFacilSponSynG[i]=pSyn->m_dSynAmp+(m_aryFacilSponSynG[i]-pSyn->m_dSynAmp)*m_aryFacilD[i];

				testD=m_aryNextSponSynTime[i];

				if (m_aryNextSponSynTime[i]<=0)		// due another input
				{
	// check that synapse has not decremented below zero 
	//(you can't take away existing G)				
					if (m_aryFacilSponSynG[i]>0)
					{
						double block=1.;
	#if 0
						if (m_pNetworkData->m_bBlockedList[i])
						{
							//ASSERT(m_pNetworkData->GetPartialBlockInUse());
							double noise =double (Std_LRand(0, RAND_MAX))/ RAND_MAX;
							float top=m_pNetworkData->GetPartialBlockTop();
							float bottom=m_pNetworkData->GetPartialBlockBottom();
							block=bottom/100+noise*(top-bottom)/100;
							//ASSERT(block>=0. && block<=1.);
						}
	#endif
						m_arySynG[i]+=block*m_aryFacilSponSynG[i];	// add new synaptic occurrence
					}

	// facilitate next response, add residual facil to new syn amp
					m_aryFacilSponSynG[i]=(m_aryFacilSponSynG[i]-pSyn->m_dSynAmp) + (pSyn->m_dSynAmp*pSyn->m_dRelFacil);	// if <0, ignore
					
					if (m_aryTonicInputPeriodType[i]==0)
						m_aryNextSponSynTime[i]=m_aryTonicInputPeriod[i];
					else
						m_aryNextSponSynTime[i]=(-m_aryTonicInputPeriod[i]*log(double (Std_LRand(0, RAND_MAX))/double(RAND_MAX))+1);
				}
			m_aryNextSponSynTime[i]-=m_dDT;
			}
		}


	// adjust for voltage dependency by scaling conductance on Neuron
	// need local storage, so can do volt dep without altering basic cond
	// could do this within m_pInputCx, but since several cx may use same voltage dependent
	// synaptic type, more efficient to do outside??
		CStdArray<double> arySynG;
		arySynG.SetSize(iSpikingChemSynCount);
		for (i=0; i<iSpikingChemSynCount; i++)
		{
			SpikingChemicalSynapse *pSyn=lpNS->GetSpikingChemSynAt(i);
			arySynG[i]=m_arySynG[i];
			if (pSyn->m_bVoltDep)
				lpNS->ScaleCondForVoltDep(arySynG[i],GetMemPot(),
					pSyn->m_dSatPSPot,pSyn->m_dThreshPSPot,pSyn->m_dMaxRelCond);

		}

	// sum spiking synaptic stuff
		GS=GSI=0;
		m_fltSpikingSynCurMemory = 0;
		for (i=0; i<iSpikingChemSynCount; i++)
		{
			SpikingChemicalSynapse *pSyn=lpNS->GetSpikingChemSynAt(i);
			if (pSyn->m_dSynAmp==0)
				continue;
			GS+=arySynG[i];
	// NOTE: the following looks wrong (driving force should be relative to current mempot,
	// not resting pot), but is actually right.  It's the exponential predictor maths
	// (all currents relative to rest ????)
			GSI+=(arySynG[i]*(pSyn->m_dEquil-m_dRestingPot));
			m_fltSpikingSynCurMemory += (arySynG[i]*(pSyn->EquilibriumPotential()-GetMemPot()));
		}
		m_fltSpikingSynCurMemory *= (float) 1e-9;

	// do burster
		if (m_dGMaxCa>0)
		{
			double z,tau,Minf,Hinf,dM,dH;
			gCa=m_dGMaxCa*m_dM*m_dH;
			//ASSERT(m_dM>=0 && m_dM <=1 && m_dH>=0 && m_dH<=1);
	// again, looks wrong but is right
			iCa=gCa*(m_dCaEquilPot-m_dRestingPot);
			m_fltICaMemory = iCa*1e-9;
	// update M & H variables
			z=exp(-m_dSM*(m_dMemPot-m_dVM));
			Minf=1/(1+z);
			tau=Minf*sqrt(z)*m_dMTimeConst;
			dM=(Minf-m_dM)*(1-exp(-m_dDT/tau));
		
			z=exp(-m_dSH*(m_dMemPot-m_dVH))*0.5;
			Hinf=1/(1+z);
			tau=Hinf*sqrt(z)*m_dHTimeConst;
			dH=(Hinf-m_dH)*(1-exp(-m_dDT/tau));
			
			m_dM+=dM;
			m_dH+=dH;
	//TRACE("M= %lf\t\tH=%lf\tpot = %lf\n",m_M,m_H,m_MemPot);
		}
		else
			m_fltICaMemory=gCa=iCa=0;
	}
	else		// cadmium applied, no chem input or g/iCa
		m_fltICaMemory=gCa=iCa=GS=GSI=0;

	// do membrane potential
	//If the HH flag is not set then calculate E in standard way outlined by Heitler.
	//If HH flag is set then do not do integrate and fire portion, but instead just use basic equation of dividing
	//currents by capacitance.
	m_dGK=m_dGK*m_dDGK+m_bSpike*m_dAHPAmp;		// cummulative AHP cond
	m_dGTot=1+GS+m_dGK+gCa+m_dElecSynCond+m_dNonSpikingSynCond; // total membrane cond
	DCE=exp(-m_dGTot*m_dDT/m_dTimeConst);
	m_fltGTotal = m_dGTot/(1-DCE);

	if(!lpNS->HH())
	{
		//E=(m_dMemPot-m_dRestingPot)*DCE+
		//	(m_dStim+m_fltChannelI+GSI+m_dGK*(m_dAHPEquilPot-m_dRestingPot)+iCa+m_dElecSynCur+m_dNonSpikingSynCur)
		//	*(1-DCE)/m_dGTot;
		m_fltTotalI = (m_dStim+m_fltChannelI+GSI+m_dGK*(m_dAHPEquilPot-m_dRestingPot)+iCa+m_dElecSynCur+m_dNonSpikingSynCur);
		E=(m_dMemPot-m_dRestingPot)*DCE+(m_fltTotalI*(1-DCE)/m_dGTot);
	}
	else
	{	
		m_fltTotalI = (m_dStim+m_fltChannelI+GSI+m_dGK*(m_dAHPEquilPot-m_dRestingPot)+iCa+m_dElecSynCur+m_dNonSpikingSynCur);
		//E=(m_dStim+m_fltChannelI+GSI+m_dGK*(m_dAHPEquilPot-m_dRestingPot)+iCa+m_dElecSynCur+m_dNonSpikingSynCur)/m_dCm;
		E=m_fltTotalI/m_dCm;
	}

	m_fltTotalMemoryI = m_fltTotalI*1e-9;

	m_dElecSynCur=m_dElecSynCond=0;
	m_dNonSpikingSynCur=m_dNonSpikingSynCond=0;
	m_dStim=0.;
// to add electrical synapse
// 1. sum together all elect syn conds (problem with scaling different sized cells here)
// (let neurons have a size factor (relative to 1), then scale defined cond by size 
// (increase if < 1, decrease if > 1), so get more current into small cells.
// do same thing for stimulus amp
// this means that chemical conds are per unit area, and so changes in cell size
// have no effect on voltage, but elect syn and injected current have voltage effects
// which scale with size
// 2. add total elect synaptic conductance to m_GTot 
// 3. sum all elect syn currents (G_Elec*(other_neuron_mempot-this_neuron_restpot)) 
// 4. put sum into current part of eqn E= ...

		
	// do threshold
	m_dThresh=m_dInitialThresh + (m_dThresh-m_dInitialThresh)*m_dDCTH + m_dRelativeAccom*E*(1-m_dDCTH);
	// do spike
	m_dNewMemPot=E+m_dRestingPot;
	m_fltEMemory = m_dNewMemPot;

// add noise as mempot fluctuation
	if (m_dNoise!=0)
	{
		double noise =(double) Std_LRand(0, RAND_MAX)/ RAND_MAX;
		noise-=0.5;
		noise*=m_dNoise;
		m_dNewMemPot+=noise;
	}

}

//Node Overrides

void Neuron::AddExternalNodeInput(int iTargetDataType, float fltInput)
{
	if(!m_bZapped)
	{
		m_fltAdapterI += (fltInput*1e9);
		m_fltAdapterMemoryI = (m_fltAdapterI * 1e-9);
	}
}

/**
\brief	Searches for an ion channel with the matching ID.

\author	dcofer
\date	3/30/2011

\param	strID	   	GUID ID of the channel to find. 
\param	bThrowError	true to throw error if channel is not found. 

\return	Pointer to the ion channel.
**/
IonChannel *Neuron::FindIonChannel(std::string strID, bool bThrowError)
{
	for(int iChannel=0; iChannel<m_iIonChannels; iChannel++)
		if(m_aryIonChannels[iChannel]->ID() == strID)
			return m_aryIonChannels[iChannel];

	if(bThrowError)
		THROW_PARAM_ERROR(Rn_Err_lIonChannelNotFound, Rn_Err_strIonChannelNotFound, "ID", strID);

	return NULL;
}	

/**
\brief	Searches for an ion channel with the specified ID and returns its position in the array.

\author	dcofer
\date	3/30/2011

\param	strID	   	GUID ID for of the channel to find. 
\param	bThrowError	true to throw error if channel is not found. 

\return	The found ion channel list position.
**/
int Neuron::FindIonChannelListPos(std::string strID, bool bThrowError)
{
	std::string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryIonChannels.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryIonChannels[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Rn_Err_lIonChannelNotFound, Rn_Err_strIonChannelNotFound, "ID");

	return -1;
}

void Neuron::ResetSimulation()
{
	m_fltAdapterI = 0;
	m_fltAdapterMemoryI = 0;
	m_fltExternalI = 0;
	m_fltChannelI = 0;
	m_fltChannelMemoryI = 0;
	m_fltICaMemory = 0;
	m_fltMemPot = 0;
	m_fltThresholdMemory = 0;
	m_fltLastSpikeTime = 0;
	m_fltFiringFreq = 0;
	m_fltElecSynCurMemory = 0;
	m_fltSpikingSynCurMemory = 0;
	m_fltNonSpikingSynCurMemory = 0;
	m_fltSpike = 0;
	m_fltTotalI = 0;
	m_fltTotalMemoryI = 0;
	m_fltSpike = 0;
	m_dCm = m_dTimeConst*m_dSize;
	m_fltGm = (float) (1/(m_dSize*1e6));
	m_fltVrest = (float) (m_dRestingPot*1e-3);
	m_fltEMemory = 0;

	m_dGK=0;
	m_bSpike=false;
	m_dDCTH=exp(-m_dDT/m_dAccomTimeConst);
	m_dDGK=exp(-m_dDT/m_dAHPTimeConst);
	m_lRefrCountDown=0;
	
	m_dMemPot=m_dNewMemPot=m_dRestingPot;
	m_dThresh=m_dInitialThresh;
	m_fltThresholdMemory = (float) m_dThresh * 0.001;
	if (m_dGMaxCa>0 && m_bBurstInitAtBottom) 
	{
		m_dMemPot=m_dRestingPot+7.408;
		m_dM=0.0945;
		m_dH=0.0208;
	}
	else
	{
		m_dMemPot=m_dRestingPot;
		m_dM=0.0f;
		m_dH=0.0f;
	}

	m_dElecSynCur=m_dElecSynCond=0;
	m_dNonSpikingSynCur=m_dNonSpikingSynCond=0;

	m_dStim=0;
	m_fltAdapterI = 0;
	m_fltAdapterMemoryI = 0;
	m_fltMemPot = m_dMemPot * 0.001;

	m_dGTot = 0;
	m_dStim=0;

	m_fltLastSpikeTime = 0;
	m_fltFiringFreq = 0;

	m_arySynG.RemoveAll();	// current conductance of each synaptic type
    m_aryFacilSponSynG.RemoveAll();	// facilitated initial g increase caused by input
    m_aryDG.RemoveAll();	// exponential decline factor in syn G
	m_aryFacilD.RemoveAll();		// exponential decline factor in facilitation
	m_aryNextSponSynTime.RemoveAll();	// time to next occurrence of this syn type

	int iSize = m_aryTonicInputPeriod.GetSize();
	for(int iIndex = 0; iIndex<iSize; iIndex++)
	{
		m_aryTonicInputPeriod[iIndex] = 0;
		m_aryTonicInputPeriodType[iIndex] = 0;
	}

	m_iIonChannels = m_aryIonChannels.GetSize();
	for(int iIndex = 0; iIndex<m_iIonChannels; iIndex++)
		m_aryIonChannels[iIndex]->ResetSimulation();
}

#pragma region DataAccesMethods

float *Neuron::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "MEMBRANEVOLTAGE")
		return &m_fltMemPot;

	if(strType == "ADAPTERCURRENT")
		return &m_fltAdapterMemoryI;

	if(strType == "EXTERNALCURRENT")
		return &m_fltExternalI;

	if(strType == "FIRINGFREQUENCY")
		return &m_fltFiringFreq;

	if(strType == "THRESHOLD")
		return &m_fltThresholdMemory;

	if(strType == "ELECTRICALSYNAPTICCURRENT")
		return &m_fltElecSynCurMemory;

	if(strType == "NONSPIKINGSYNAPTICCURRENT")
		return &m_fltNonSpikingSynCurMemory;

	if(strType == "SPIKINGSYNAPTICCURRENT")
		return &m_fltSpikingSynCurMemory;

	if(strType == "IONCHANNELCURRENT")
		return &m_fltChannelMemoryI;

	if(strType == "CACURRENT")
		return &m_fltICaMemory;

	if(strType == "TOTALCURRENT")
		return &m_fltTotalMemoryI;

	if(strType == "SPIKE")
		return &m_fltSpike;

	if(strType == "GM")
		return &m_fltGm;

	if(strType == "VREST")
		return &m_fltVrest;

	if(strType == "GTOTAL")
		return &m_fltGTotal;

	if(strType == "E")
		return &m_fltEMemory;

	//If it was not one of those above then we have a problem.
	THROW_PARAM_ERROR(Rn_Err_lInvalidNeuronDataType, Rn_Err_strInvalidNeuronDataType, "Neuron Data Type", strDataType);

	return NULL;
}

bool Neuron::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
			
	if(Node::SetData(strDataType, strValue, false))
		return true;

	if(strType == "RESTINGPOTENTIAL")
	{
		RestingPotential(atof(strValue.c_str()));
		return true;
	}

	if(strType == "RELATIVESIZE")
	{
		Size(atof(strValue.c_str()));
		return true;
	}

	if(strType == "TIMECONSTANT")
	{
		TimeConstant(atof(strValue.c_str()));
		return true;
	}

	if(strType == "INITIALTHRESHOLD")
	{
		InitialThreshold(atof(strValue.c_str()));
		return true;
	}

	if(strType == "RELATIVEACCOMODATION")
	{
		RelativeAccomodation(atof(strValue.c_str()));
		return true;
	}

	if(strType == "ACCOMODATIONTIMECONSTANT")
	{
		AccomodationTimeConstant(atof(strValue.c_str()));
		return true;
	}

	if(strType == "AHP_CONDUCTANCE")
	{
		AHPAmplitude(atof(strValue.c_str()));
		return true;
	}

	if(strType == "AHP_TIMECONSTANT")
	{
		AHPTimeConstant(atof(strValue.c_str()));
		return true;
	}

	if(strType == "MAXCACONDUCTANCE")
	{
		BurstGMaxCa(atof(strValue.c_str()));
		return true;
	}

	if(strType == "BURSTINITATBOTTOM")
	{
		BurstInitAtBottom(Std_ToBool(strValue));
		return true;
	}

	if(strType == "TONICSTIMULUS")
	{
		TonicStimulus(atof(strValue.c_str()));
		return true;
	}

	if(strType == "TONICNOISE")
	{
		TonicNoise(atof(strValue.c_str()));
		return true;
	}

	if(strType == "ADDEXTERNALCURRENT")
	{
		AddExternalI(atof(strValue.c_str()));
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

	aryProperties.Add(new TypeProperty("MembraneVoltage", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("AdapterCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("ExternalCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("FiringFrequency", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("Threshold", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("ElectricalSynapticCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("NonSpikingSynapticCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("SpikingSynapticCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("IonChannelCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("CaCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("TotalCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("Spike", AnimatPropertyType::Boolean, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("Gm", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("Vrest", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("Gtotal", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("E", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("RestingPotential", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("RelativeSize", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("TimeConstant", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("InitialThreshold", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("RelativeAccomodation", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("AccomodationTimeConstant", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("AHP_Conductance", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("AHP_TimeConstant", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MaxCAConductance", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("BurstInitAtBottom", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("TonicStimulus", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("TonicNoise", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("AddExternalCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

/**
\brief	Adds an ion channel from an xml packet definition. 

\author	dcofer
\date	3/30/2011

\param	strXml	The xml to load. 
**/
void Neuron::AddIonChannel(std::string strXml, bool bDoNotInit)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("IonChannel");

	IonChannel *lpChannel = LoadIonChannel(oXml);

	if(!bDoNotInit)
		lpChannel->Initialize();
}

/**
\brief	Removes the ion channel.

\author	dcofer
\date	3/30/2011

\param	strID	   	GUID ID for the channel to remove. 
\param	bThrowError	true to throw error if channel is not found. 
**/
void Neuron::RemoveIonChannel(std::string strID, bool bThrowError)
{
	int iPos = FindIonChannelListPos(strID, bThrowError);
	m_aryIonChannels.RemoveAt(iPos);
}

bool Neuron::AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError, bool bDoNotInit)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "IONCHANNEL")
	{
		AddIonChannel(strXml, bDoNotInit);
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

	if(strType == "IONCHANNEL")
	{
		RemoveIonChannel(strID, bThrowError);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

#pragma endregion


//Node Overrides

}				//IntegrateFireSim
