/**
\file IntegrateFireModule.cpp

\brief	Implements the integrate fire module class.
**/

#include "stdafx.h"
#include "IonChannel.h"
#include "SynapseType.h"
#include "Connexion.h"
#include "CaActivation.h"
#include "Neuron.h"
#include "ElectricalSynapse.h"
#include "NonSpikingChemicalSynapse.h"
#include "SpikingChemicalSynapse.h"
#include "IntegrateFireModule.h"
#include "ClassFactory.h"
#include <time.h>

namespace IntegrateFireSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/30/2011
**/
IntegrateFireNeuralModule::IntegrateFireNeuralModule()
{
	m_lpClassFactory =  new IntegrateFireSim::ClassFactory;

//	m_fltStepSize = 0;
	m_dTimeStep =1.;

	m_bNeedInitialiseHebb=TRUE;		
	m_bRandomisedHebb=FALSE;
	m_bFreezeLearning=FALSE;	
	m_bUseCriticalPeriod = FALSE;
	m_dStartCriticalPeriod = 0;
	m_dEndCriticalPeriod = 0;
	m_bFreezeHebb = FALSE;
	m_bNeedInitialiseHebb = FALSE;		
	m_bRandomisedHebb = FALSE;
	m_bFreezeLearning = FALSE;
	m_dCurrentTime = 0;

	m_bTTX = FALSE;
	m_bCd = FALSE;
	m_bHH = FALSE;



}

/**
\brief	Destructor.

\author	dcofer
\date	3/30/2011
**/
IntegrateFireNeuralModule::~IntegrateFireNeuralModule()
{

	try
	{
		m_aryNeurons.RemoveAll();
		m_arySpikingChemSyn.RemoveAll();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of IntegrateFireNeuralModule\r\n", "", -1, FALSE, TRUE);}
}


#pragma region Accessor-Mutators

/**
\brief	Sets the current time.

\author	dcofer
\date	3/30/2011

\param	t	time. 
**/
void IntegrateFireNeuralModule::SetCurrentTime(double t) {m_dCurrentTime=t;}

/**
\brief	Gets the current time.

\author	dcofer
\date	3/30/2011

\return	The current time.
**/
double IntegrateFireNeuralModule::GetCurrentTime() {return m_dCurrentTime;}

/**
\brief	Gets the time step.

\author	dcofer
\date	3/30/2011

\return	The time step.
**/
double IntegrateFireNeuralModule::GetTimeStep() {return m_dTimeStep;}

/**
\brief	Gets the neuron count.

\author	dcofer
\date	3/30/2011

\return	The neuron count.
**/
int IntegrateFireNeuralModule::GetNeuronCount() {return m_aryNeurons.size();}

/**
\brief	Gets a neuron at specified index.

\author	dcofer
\date	3/30/2011

\param	i	The index. 

\return	Pointer to the neuron at.
**/
Neuron *IntegrateFireNeuralModule::GetNeuronAt(int i) {return m_aryNeurons[i];}

/**
\brief	Gets the connexion count.

\author	dcofer
\date	3/30/2011

\return	The connexion count.
**/
int IntegrateFireNeuralModule::GetConnexionCount() {return m_aryConnexion.size();}

/**
\brief	Gets a connexion at specified index.

\author	dcofer
\date	3/30/2011

\param	i	The index. 

\return	Pointer to the connexion at.
**/
Connexion *IntegrateFireNeuralModule::GetConnexionAt(int i) {return m_aryConnexion[i];}

/**
\brief	Sets whether Cadmium is applied to the nervous system.

\author	dcofer
\date	3/30/2011

\param	bVal	true to use cadmium. 
**/
void IntegrateFireNeuralModule::Cd(BOOL bVal) {m_bCd = bVal;}

/**
\brief	Gets whether Cadmium is applied to the nervous system.

\author	dcofer
\date	3/30/2011

\return	true if Cd applied, false else.
**/
BOOL IntegrateFireNeuralModule::Cd() {return m_bCd;}

/**
\brief	Sets whether ttx is applied to the nervous system.

\author	dcofer
\date	3/30/2011

\param	bVal	true to use ttx. 
**/
void IntegrateFireNeuralModule::TTX(BOOL bVal) {m_bTTX = bVal;}

/**
\brief	Gets whether ttx is applied to the nervous system.

\author	dcofer
\date	3/30/2011

\return	true if ttx applied, false else.
**/
BOOL IntegrateFireNeuralModule::TTX() {return m_bTTX;}

/**
\brief	Sets whether hodgkin-huxely model is used.

\author	dcofer
\date	3/30/2011

\param	bVal	true to use HH. 
**/
void IntegrateFireNeuralModule::HH(BOOL bVal) {m_bHH = bVal;}

/**
\brief	Gets whether hodgkin-huxely model is used.

\author	dcofer
\date	3/30/2011

\return	true if HH used, false else.
**/
BOOL IntegrateFireNeuralModule::HH() {return m_bHH;}

/**
\brief	Sets the Time step.

\author	dcofer
\date	3/30/2011

\param	fltVal	The new value. 
**/
void IntegrateFireNeuralModule::TimeStep(float fltVal)
{
	Std_InValidRange((float) 1e-10, (float) 1, fltVal, TRUE, "TimeStep");

	m_fltTimeStep = fltVal;
	m_dTimeStep = m_fltTimeStep * 1000;
	Neuron::m_dDT=m_dTimeStep;

	//We must update the facilitation decay constant when we change the timestep.
	SpikingChemicalSynapse *lpSyn;
	int iCount = m_arySpikingChemSyn.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpSyn = m_arySpikingChemSyn[iIndex];
		lpSyn->m_dFacilD = exp(-m_dTimeStep/lpSyn->m_dFacilDecay);
	}
}

/**
\brief	Gets the time step.

\author	dcofer
\date	3/30/2011

\return	time step.
**/
float IntegrateFireNeuralModule::TimeStep() {return m_fltTimeStep;}

/**
\brief	Sets whether to retain hebbian memory.

\author	dcofer
\date	3/30/2011

\param	bVal	true to retain. 
**/
void IntegrateFireNeuralModule::RetainHebbMemory(BOOL bVal) {m_bRetainHebbMemory = bVal;}

/**
\brief	Gets whether to  retain hebbian memory.

\author	dcofer
\date	3/30/2011

\return	true if retaining, false else.
**/
BOOL IntegrateFireNeuralModule::RetainHebbMemory() {return m_bRetainHebbMemory;}

/**
\brief	Sets whether to use critical period.

\author	dcofer
\date	3/30/2011

\param	bVal	true to use critical period. 
**/
void IntegrateFireNeuralModule::UseCriticalPeriod(BOOL bVal) {m_bUseCriticalPeriod = bVal;}

/**
\brief	Gets whether to use critical period.

\author	dcofer
\date	3/30/2011

\return	true if critical period used. 
**/
BOOL IntegrateFireNeuralModule::UseCriticalPeriod() {return m_bUseCriticalPeriod;}

/**
\brief	Sets the start time of the critical period.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void IntegrateFireNeuralModule::StartCriticalPeriod(double dVal) {m_dStartCriticalPeriod = dVal;}

/**
\brief	Gets the start time of the critical period.

\author	dcofer
\date	3/30/2011

\return	start time.
**/
double IntegrateFireNeuralModule::StartCriticalPeriod() {return m_dStartCriticalPeriod;}

/**
\brief	Sets the end time of the critical period.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void IntegrateFireNeuralModule::EndCriticalPeriod(double dVal) {m_dEndCriticalPeriod = dVal;}

/**
\brief	Gets the end time of the critical period.

\author	dcofer
\date	3/30/2011

\return	end time.
**/
double IntegrateFireNeuralModule::EndCriticalPeriod() {return m_dEndCriticalPeriod;}

/**
\brief	Sets whether to freeze hebbian learning.

\author	dcofer
\date	3/30/2011

\param	bVal	true to freeze. 
**/
void IntegrateFireNeuralModule::FreezeHebb(BOOL bVal) {m_bFreezeHebb = bVal;}

/**
\brief	Sets whether to freeze hebbian learning.

\author	dcofer
\date	3/30/2011

\return	true if frozen, false else.
**/
BOOL IntegrateFireNeuralModule::FreezeHebb() {return m_bFreezeHebb;}

/**
\brief	Sets the spike peak.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void IntegrateFireNeuralModule::SpikePeak(double dVal) {Neuron::m_dSpikePeak = dVal;}

/**
\brief	Gets the spike peak.

\author	dcofer
\date	3/30/2011

\return	spike peak.
**/
double IntegrateFireNeuralModule::SpikePeak() {return Neuron::m_dSpikePeak;}

/**
\brief	Sets the spike strength.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void IntegrateFireNeuralModule::SpikeStrength(double dVal) {Neuron::m_dSpikeStrength = dVal;}

/**
\brief	Gets the spike strength.

\author	dcofer
\date	3/30/2011

\return	spike strength.
**/
double IntegrateFireNeuralModule::SpikeStrength() {return Neuron::m_dSpikeStrength;}

/**
\brief	Sets the calcium equilibrium potential.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void IntegrateFireNeuralModule::CaEquilPot(double dVal) {Neuron::m_dCaEquilPot = dVal;}

/**
\brief	Gets the calcium equilibrium potential.

\author	dcofer
\date	3/30/2011

\return	potential.
**/
double IntegrateFireNeuralModule::CaEquilPot() {return Neuron::m_dCaEquilPot;}

/**
\brief	Sets the absolute refractory period.

\author	dcofer
\date	3/30/2011

\param	dVal	The new value. 
**/
void IntegrateFireNeuralModule::AbsoluteRefr(double dVal) {Neuron::m_dAbsoluteRefr = dVal;}

/**
\brief	Gets the absolute refractory period.

\author	dcofer
\date	3/30/2011

\return	refractory period.
**/
double IntegrateFireNeuralModule::AbsoluteRefr() {return Neuron::m_dAbsoluteRefr;}

/**
\brief	Gets the spiking chemical synapse count.

\author	dcofer
\date	3/30/2011

\return	The count.
**/
int IntegrateFireNeuralModule::GetSpikingChemSynCount() {return m_arySpikingChemSyn.size();}

/**
\brief	Gets a spiking chemical synapse at the specified array index.

\author	dcofer
\date	3/30/2011

\param	i	The index. 

\return	null if it fails, else the spiking chem syn at.
**/
SpikingChemicalSynapse *IntegrateFireNeuralModule::GetSpikingChemSynAt(int i) {return m_arySpikingChemSyn[i];}

/**
\brief	Gets the non-spiking chemical synapse count.

\author	dcofer
\date	3/30/2011

\return	The non-spiking chemical synapse count.
**/
int IntegrateFireNeuralModule::GetNonSpikingChemSynCount() {return m_aryNonSpikingChemSyn.size();}

/**
\brief	Gets a non-spiking chemical synapse at the specified index.

\author	dcofer
\date	3/30/2011

\param	i	The index. 

\return	null if it fails, else the non spiking chem syn at.
**/
NonSpikingChemicalSynapse *IntegrateFireNeuralModule::GetNonSpikingChemSynAt(int i) {return m_aryNonSpikingChemSyn[i];}

/**
\brief	Gets the electrical synapse count.

\author	dcofer
\date	3/30/2011

\return	The electrical synapse count.
**/
int IntegrateFireNeuralModule::GetElecSynCount() {return m_aryElecSyn.size();}

/**
\brief	Gets an electrical synapse at the specified index.

\author	dcofer
\date	3/30/2011

\param	i	The index. 

\return	null if it fails, else the elec syn at.
**/
ElectricalSynapse *IntegrateFireNeuralModule::GetElecSynAt(int i) {return m_aryElecSyn[i];}

/**
\brief	Gets the module name.

\author	dcofer
\date	3/30/2011

\return	module name.
**/
string IntegrateFireNeuralModule::ModuleName() {return Rn_NeuralModuleName();}

#pragma endregion

/**
\brief	Interal method to load the module data.

\author	dcofer
\date	3/30/2011

\param [in,out]	oXml	The xml to load. 
**/
void IntegrateFireNeuralModule::LoadInternal(CStdXml &oXml)
{
	int i;
	VerifySystemPointers();

	m_aryNeurons.RemoveAll();

	ID(oXml.GetChildString("ID", m_strID));
	Type(oXml.GetChildString("Type", m_strType));
	Name(oXml.GetChildString("Name", m_strName));

	//This will add this object to the object list of the simulation.
	Simulator *m_lpSim = GetSimulator();
	if(m_lpSim)
		m_lpSim->AddToObjectList(this);

	TimeStep(oXml.GetChildFloat("TimeStep"));
	m_dTimeStep = m_fltTimeStep * 1000;
	Std_InValidRange((float) 1e-10, (float) 1, m_fltTimeStep, TRUE, "TimeStep");

	TTX(oXml.GetChildBool("TTX", FALSE));
	Cd(oXml.GetChildBool("Cd", FALSE));
	HH(oXml.GetChildBool("HH", FALSE));

	RetainHebbMemory(oXml.GetChildBool("RetainHebbMemory", 0));
	UseCriticalPeriod(oXml.GetChildBool("UseCriticalPeriod"));
	StartCriticalPeriod(oXml.GetChildDouble("StartCriticalPeriod"));
	EndCriticalPeriod(oXml.GetChildDouble("EndCriticalPeriod"));
	FreezeHebb(oXml.GetChildBool("FreezeHebb", 0));

	Neuron::m_dSpikePeak= oXml.GetChildDouble("SpikePeak");
	Neuron::m_dSpikeStrength=oXml.GetChildDouble("SpikeStrength");
	Neuron::m_dAHPEquilPot=oXml.GetChildDouble("AHPEquilPot");
	Neuron::m_dCaEquilPot=oXml.GetChildDouble("CaEquilPot");
	Neuron::m_dAbsoluteRefr=oXml.GetChildDouble("AbsoluteRefr");

	oXml.IntoChildElement("Synapses");

	oXml.IntoChildElement("SpikingSynapses");
	int iTotalSpikingSynapses = oXml.NumberOfChildren();
	for(i=0; i<iTotalSpikingSynapses; i++)
	{
		oXml.FindChildByIndex(i);
		LoadSpikingChemSyn(oXml, i);
	}
	oXml.OutOfElem(); //OutOf SpikingSynapses Element
	
	oXml.IntoChildElement("NonSpikingSynapses");
	int iTotalNonSpikingSynapses = oXml.NumberOfChildren();
	for(i=0; i<iTotalNonSpikingSynapses; i++)
	{
		oXml.FindChildByIndex(i);
		LoadNonSpikingChemSyn(oXml, i);
	}
	oXml.OutOfElem(); //OutOf NonSpikingSynapses Element
	
	oXml.IntoChildElement("ElectricalSynapses");
	int iTotalElecSynapses = oXml.NumberOfChildren();
	for(i=0; i<iTotalElecSynapses; i++)
	{
		oXml.FindChildByIndex(i);
		LoadElecSyn(oXml, i);
	}
	oXml.OutOfElem(); //OutOf ElectricalSynapses Element

	oXml.OutOfElem(); //OutOf Synapses Element


	//*** Begin Loading Neurons. *****
	oXml.IntoChildElement("Neurons");
	int iTotalNeurons = oXml.NumberOfChildren();
	for(i=0; i<iTotalNeurons; i++)
	{
		oXml.FindChildByIndex(i);
		LoadNeuron(oXml);
	}
	oXml.OutOfElem(); //OutOf Neurons Element


	/*
	if(oXml.FindChildElement("Stimuli", false))
	{
		oXml.IntoChildElement("Stimuli");
		int iTotalStimuli = oXml.NumberOfChildren();
		for(i=0; i<iTotalStimuli; i++)
		{
			oXml.FindChildByIndex(i);
			LoadStim(oXml);
		}
		oXml.OutOfElem(); //OutOf Stimuli Element
	}
	*/

	oXml.IntoChildElement("Connexions");
	int iTotalConnexions = oXml.NumberOfChildren();
	for(i=0; i<iTotalConnexions; i++)
	{
		oXml.FindChildByIndex(i);
		//Std_TraceMsg(0, STR(i));
		LoadConnexion(oXml);
	}
	oXml.OutOfElem(); //OutOf Stimuli Element


//Std_TraceMsg(0,"neuron count = "+STR(GetNeuronCount()));
//Std_TraceMsg(0,"stim count = "+STR(GetStimCount()));
//Std_TraceMsg(0,"connexion count = "+STR(GetConnexionCount()));
}

/**
\brief	Loads a neuron.

\author	dcofer
\date	3/30/2011

\param [in,out]	oXml	The xml to use when loading the neuron. 

\return	Pointer to the neuron.
**/
Neuron *IntegrateFireNeuralModule::LoadNeuron(CStdXml &oXml)
{
	Neuron *lpNeuron=NULL;
	string strType;

	try
	{

		lpNeuron = new Neuron;

		lpNeuron->SetSystemPointers(m_lpSim, m_lpStructure, this, NULL, TRUE);
		lpNeuron->Load(oXml);

		m_aryNeurons.Add(lpNeuron);

		return lpNeuron;
	}
	catch(CStdErrorInfo oError)
	{
		if(lpNeuron) delete lpNeuron;
		RELAY_ERROR(oError);
		return NULL;
	}
	catch(...)
	{
		if(lpNeuron) delete lpNeuron;
		THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
		return NULL;
	}
}

/**
\brief	Loads a synapse type.

\author	dcofer
\date	3/30/2011

\param [in,out]	oXml	The xml to use when loading the synapse type. 

\return	Pointer to the synapse type.
**/
SynapseType *IntegrateFireNeuralModule::LoadSynapseType(CStdXml &oXml)
{
	oXml.IntoElem();
	string strType = Std_ToUpper(oXml.GetChildString("Type"));
	oXml.OutOfElem();

	SynapseType *lpType = NULL;

	if(strType == "SPIKINGCHEMICAL")
		lpType = LoadSpikingChemSyn(oXml, m_arySpikingChemSyn.GetSize());
	else if(strType == "NONSPIKINGCHEMICAL")
		lpType = LoadNonSpikingChemSyn(oXml, m_aryNonSpikingChemSyn.GetSize());
	else if(strType == "ELECTRICAL")
		lpType = LoadElecSyn(oXml, m_aryElecSyn.GetSize());
	else
		THROW_PARAM_ERROR(Rn_Err_lInvalidSynapseType, Rn_Err_strInvalidSynapseType, "Type", strType);
	
	return lpType;
}

/**
\brief	Loads a spiking chemical synapse.

\author	dcofer
\date	3/30/2011

\param [in,out]	oXml	The xml to use when loading the synapse. 
\param	iIndex			Index where the synpase will be added in the array. 

\return	Pointer to the spiking chemical synapse.
**/
SpikingChemicalSynapse *IntegrateFireNeuralModule::LoadSpikingChemSyn(CStdXml &oXml, int iIndex)
{
	SpikingChemicalSynapse *pSpikingChemSyn=NULL;
	string strType;
//

	try
	{

		pSpikingChemSyn = new SpikingChemicalSynapse;
		pSpikingChemSyn->SetSystemPointers(m_lpSim, m_lpStructure, this, NULL, TRUE);
		pSpikingChemSyn->Load(oXml);
		pSpikingChemSyn->SynapseTypeID(iIndex);

		m_arySpikingChemSyn.Add(pSpikingChemSyn);

		return pSpikingChemSyn;
	}
	catch(CStdErrorInfo oError)
	{
		if(pSpikingChemSyn) delete pSpikingChemSyn;
		RELAY_ERROR(oError);
		return NULL;
	}
	catch(...)
	{
		if(pSpikingChemSyn) delete pSpikingChemSyn;
		THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
		return NULL;
	}
}

/**
\brief	Loads a non-spiking chemical synapse.

\author	dcofer
\date	3/30/2011

\param [in,out]	oXml	The xml to use when loading the synapse. 
\param	iIndex			Index where the synpase will be added in the array. 

\return	Pointer to the synapse.
**/
NonSpikingChemicalSynapse *IntegrateFireNeuralModule::LoadNonSpikingChemSyn(CStdXml &oXml, int iIndex)
{
	NonSpikingChemicalSynapse *pNonSpikingChemSyn=NULL;
	string strType;
//

	try
	{

		pNonSpikingChemSyn = new NonSpikingChemicalSynapse;
		pNonSpikingChemSyn->SetSystemPointers(m_lpSim, m_lpStructure, this, NULL, TRUE);
		pNonSpikingChemSyn->Load(oXml);
		pNonSpikingChemSyn->SynapseTypeID(iIndex);

		m_aryNonSpikingChemSyn.Add(pNonSpikingChemSyn);

		return pNonSpikingChemSyn;
	}
	catch(CStdErrorInfo oError)
	{
		if(pNonSpikingChemSyn) delete pNonSpikingChemSyn;
		RELAY_ERROR(oError);
		return NULL;
	}
	catch(...)
	{
		if(pNonSpikingChemSyn) delete pNonSpikingChemSyn;
		THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
		return NULL;
	}
}

/**
\brief	Loads an electrical synapse.

\author	dcofer
\date	3/30/2011

\param [in,out]	oXml	The xml to use when loading the synapse. 
\param	iIndex			Index where the synpase will be added in the array. 

\return	Pointer to the synapse.
**/
ElectricalSynapse *IntegrateFireNeuralModule::LoadElecSyn(CStdXml &oXml, int iIndex)
{
	ElectricalSynapse  *pElecSyn=NULL;
	string strType;
//

	try
	{

		pElecSyn = new ElectricalSynapse;
		pElecSyn->SetSystemPointers(m_lpSim, m_lpStructure, this, NULL, TRUE);
		pElecSyn->Load(oXml);
		pElecSyn->SynapseTypeID(iIndex);

		m_aryElecSyn.Add(pElecSyn);

		return pElecSyn;
	}
	catch(CStdErrorInfo oError)
	{
		if(pElecSyn) delete pElecSyn;
		RELAY_ERROR(oError);
		return NULL;
	}
	catch(...)
	{
		if(pElecSyn) delete pElecSyn;
		THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
		return NULL;
	}
}

/**
\brief	Loads a connexion.

\author	dcofer
\date	3/30/2011

\param [in,out]	oXml	The xml to use when loading the connection. 

\return	Pointer to the connexion.
**/
Connexion *IntegrateFireNeuralModule::LoadConnexion(CStdXml &oXml)
{
	Connexion *pConnexion=NULL;
	string strType;

	try
	{

		pConnexion = new Connexion;
		pConnexion->SetSystemPointers(m_lpSim, m_lpStructure, this, NULL, TRUE);
		pConnexion->Load(oXml);

		m_aryConnexion.Add(pConnexion);

		return pConnexion;
	}
	catch(CStdErrorInfo oError)
	{
		if(pConnexion) delete pConnexion;
		RELAY_ERROR(oError);
		return NULL;
	}
	catch(...)
	{
		if(pConnexion) delete pConnexion;
		THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
		return NULL;
	}
}

/**
\brief	Resets the integer IDs of the various objects when things move around in the array for some reason.

\author	dcofer
\date	3/30/2011
**/
void IntegrateFireNeuralModule::ResetIDs()
{
	int iCount = m_arySpikingChemSyn.GetSize();
	for(int i=0; i<iCount; i++)
		m_arySpikingChemSyn[i]->SynapseTypeID(i);

	iCount = m_aryNonSpikingChemSyn.GetSize();
	for(int i=0; i<iCount; i++)
		m_aryNonSpikingChemSyn[i]->SynapseTypeID(i);

	iCount = m_aryElecSyn.GetSize();
	for(int i=0; i<iCount; i++)
		m_aryElecSyn[i]->SynapseTypeID(i);

	iCount = m_aryNeurons.GetSize();
	for(int i=0; i<iCount; i++)
		m_aryNeurons[i]->NeuronID(i);

	Connexion *pCx;
	for(int i=0; i<GetConnexionCount(); i++)
	{
		pCx=GetConnexionAt(i);
		pCx->ResetIDs();
	}
}

////////////////////////////
// THE ENGINE
//

/**
\brief	Gets a scale electrical conductance.

\author	dcofer
\date	3/30/2011

\param	minG	 	The minimum conductance. 
\param	maxG	 	The maximum conductance. 
\param	jV		 	The juntional voltage. 
\param	ThreshV  	The threshold voltage. 
\param	SaturateV	The saturate voltage. 

\return	The scale electrical conductance.
**/
double IntegrateFireNeuralModule::GetScaleElecCond(double minG,double maxG,double jV, double ThreshV,double SaturateV)
{
	if (maxG==minG)			// NON rectifying
		return minG;
	if (jV<=ThreshV)
		return minG;
	if (jV>SaturateV)
		return maxG;

	return minG+(maxG-minG)*(jV-ThreshV)/(SaturateV-ThreshV);
}

/**
\brief	Scale conductance for volt dependance.

\author	dcofer
\date	3/30/2011

\param [in,out]	G	The conductance to scale. 
\param	postV	 	The post-synaptic voltage. 
\param	maxV	 	The maximum voltage. 
\param	minV	 	The minimum voltage. 
\param	scl		 	The scale factor. 
**/
void IntegrateFireNeuralModule::ScaleCondForVoltDep(double& G,double postV,double maxV,double minV,double scl)
{
	//dwc There was an inconsistency here. If the post-Vm was less than maxV then it
	//could range from 1 to (1+scl), but as soon as the Vm goes to maxV or above it
	//suddenly jumps to scaling by scl instead of (1+scl). I changed it so that it scales correclty.
	if (postV >= maxV)
		G*=(1+scl);   
	else if (postV > minV && postV < maxV)
	{
#ifdef _DEBUG
double vScl=(postV-minV)/(maxV-minV);
//ASSERT(vScl>0 && vScl<1);
#endif
			G*=(1+(scl*(postV-minV)/(maxV-minV)));
	}
}

// pass in reference to maximum conductance

/**
\brief	Scale conductance for non-spiking chemical synapse.

\author	dcofer
\date	3/30/2011

\param [in,out]	G	The conductance to scale.
\param	PreV	 	The pre-synaptic voltage.
\param	ThreshV  	The threshold voltage. 
\param	SaturateV	The saturate voltage. 
**/
void IntegrateFireNeuralModule::ScaleCondForNonSpiking(double& G,double PreV,double ThreshV,double SaturateV)
{
	if (PreV<=ThreshV)
		G=0.0;
	else if (PreV<SaturateV)
		G=G*(PreV-ThreshV)/(SaturateV-ThreshV);
}

/**
\brief	performs post-step calculations.

\author	dcofer
\date	3/30/2011
**/
void IntegrateFireNeuralModule::PostCalc()
{
	int i;
	Connexion *pCx;
	
	for (i=0; i<GetNeuronCount(); i++)
	{
		GetNeuronAt(i)->PostCalc(this);
	}
	for (i=0; i<GetConnexionCount(); i++)
	{
		pCx=GetConnexionAt(i);
		pCx->m_TransitCx.Release();
		pCx->m_HebbList.Release();
	}
}

/**
\brief	Initialises the synapse.

\author	dcofer
\date	3/30/2011

\param [in,out]	pCx	Pointer to the connection to initialize. 
**/
void IntegrateFireNeuralModule::InitSynapse(Connexion *pCx)
{
	pCx->m_lpSynType = dynamic_cast<SynapseType *>(m_lpSim->FindByID(pCx->SynapseTypeID()));
	if(!pCx->m_lpSynType)
		THROW_PARAM_ERROR(Rn_Err_lInvalidSynapseID, Rn_Err_strInvalidSynapseID, "ID: ", pCx->SynapseTypeID());

	pCx->m_lpSource = dynamic_cast<Neuron *>(m_lpSim->FindByID(pCx->SourceID()));
	if(!pCx->m_lpSource)
		THROW_PARAM_ERROR(Rn_Err_lNeuronNotFound, Rn_Err_strNeuronNotFound, "ID: ", pCx->SourceID());

	pCx->m_lpTarget = dynamic_cast<Neuron *>(m_lpSim->FindByID(pCx->TargetID()));
	if(!pCx->m_lpTarget)
		THROW_PARAM_ERROR(Rn_Err_lNeuronNotFound, Rn_Err_strNeuronNotFound, "ID: ", pCx->TargetID());

	if (pCx->m_iType==0)	// spiking synapse
	{
		pCx->m_dPreviousSpikeLatency=-1;
		pCx->m_dGFacilCx = pCx->m_dBaseG;
		pCx->m_dG = pCx->m_dBaseG;

		if (pCx->Hebbian())
			pCx->m_dTimeSincePrevHebbEvent=0;
	}
}

/**
\brief	Performs pre-step calculations.

\author	dcofer
\date	3/30/2011
**/
void IntegrateFireNeuralModule::PreCalc()
{
	int i;
	Connexion *pCx;

	m_dCurrentTime=0;
	Neuron::m_dDT=m_dTimeStep;
	for (i=0; i<GetNeuronCount(); i++)
	{
		GetNeuronAt(i)->PreCalc(this);
	}


/*
FOLLOWING COMMENTS ARE FROM NEUROSIM - I don't really understand them any more!!!!
*/

/*
 THE LOGIC
 Normally need to set initial conductance of Hebb synapses to unaugmented 
 state at start of run.
 If RetainMemory, still need to set as above, until run once. 
 Problem: if do run, then change  synapses, then set RetainMem - 
 retains conditions of previous synapses.
 So set NeedInitHebb flag when change synapses or circuit (and in NewDoc).
 If load file, set NeedInit UNLESS memory was retained in loaded file.
 */

// could skip all pCx initialisation if retain memory, so long as done it at least once.
// since can't change circuit while retain memory, so no need to worry about
// params changing.
// set DoneInitHebb FALSE when setting RetainMemory
// BUT since may be loading from file, and don't want to save all m_FacilD etc,
// just skip initialising the bits that hold the memory trace

	for (i=0; i<GetConnexionCount(); i++)
	{
		pCx=GetConnexionAt(i);
		InitSynapse(pCx);
    }
	m_bNeedInitialiseHebb=FALSE;
	m_bRandomisedHebb=FALSE;     // randomising done in menu handler

	ResetIDs();
// do any zero-latency stimulus
	//for (i=0; i<GetStimCount(); i++)
	//{
	//	GetStimAt(i)->Reset();
	//}
}

/**
\brief	Calculates the update during the step of the simulation.

\author	dcofer
\date	3/30/2011
**/
void IntegrateFireNeuralModule::CalcUpdate()
{
	int i;
	Neuron *pSourceN;
	Neuron *pTargetN;
	Connexion *pCx;

	if(m_lpSim->TimeSlice() == 17)
		i = 0;

// could use 		
//	Sleep(m_SlowDownFactor);
// but has granularity of 1 msec, which is a bit thick

	m_dCurrentTime+=m_dTimeStep;


	//CurrentStimulus *pStim;
	//for (i=0; i<GetStimCount(); i++)
	//{
	//	pStim=GetStimAt(i);
	//	GetNeuronAt(pStim->GetNeuronID())->IncrementStim(pStim->GetStim(m_dCurrentTime));
	//}

// check if in Critical Period( if used)
	m_bFreezeLearning=m_bFreezeHebb;
	if (m_bFreezeLearning==FALSE && m_bUseCriticalPeriod)
	{
		if (m_dCurrentTime<m_dStartCriticalPeriod || m_dCurrentTime>m_dEndCriticalPeriod)
			m_bFreezeLearning=TRUE;
	}

	
// We calculate the conductance of each spiking synaptic type, and pass it to
// neuron. We cannot add to general conductance (as we can with non-spiking or 
// chemical, because of time dependence (time constant, facilitation etc) 
// which means that each type has to be treated separately

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


// go through connexion list for electrical and non-spiking chemical synapses
	double sourceV,targetV,junctV;		// source,target mempot & junctional pot (pre-post)
	double junctG,G;
	for (i=0; i<GetConnexionCount(); i++)
	{
		pCx=GetConnexionAt(i);

		pSourceN = pCx->m_lpSource;
		pTargetN = pCx->m_lpTarget;
		if (pSourceN->GetZapped() || pTargetN->GetZapped())
			continue;

		sourceV=pSourceN->GetMemPot();
		if (pCx->m_iType==0)
		{
			if (!m_bCd)			// Cadmium blocks all chemical synapses
			{
				double *pTimeToNext;
				pCx->DecrementLatencies(m_dTimeStep,m_bFreezeLearning);
				pCx->DecrementFacilitation();
				if ((pTimeToNext=pCx->GetTimeToNextSpikePtr())!=NULL) // got spike in transit
				{
					if ((*pTimeToNext)<0) 	// spike has arrived
						pTargetN->m_arySynG[pCx->m_iID]+=(pCx->m_dPartialBlock*pCx->ProcessOutput(m_bFreezeLearning));
				}
			}
		}
		else if (pCx->m_iType==1)						// non-spiking chemical
		{
			if (!m_bCd)			// Cadmium blocks all chemical synapses
			{
				G=pCx->m_dPartialBlock*m_aryNonSpikingChemSyn[pCx->m_iID]->m_dSynAmp;
	//ASSERT(G>=0);
				ScaleCondForNonSpiking(G,sourceV,
					m_aryNonSpikingChemSyn[pCx->m_iID]->m_dThreshV,
					m_aryNonSpikingChemSyn[pCx->m_iID]->m_dSaturateV);

				if (G==0.0)
					continue;

				pTargetN->IncNonSpikingSynCond(G);
				pTargetN->IncNonSpikingSynCurr(G * (m_aryNonSpikingChemSyn[pCx->m_iID]->m_dEquil-pTargetN->m_dRestingPot));
			}
		}
				
		else // electrical
		{
//ASSERT(pCx->m_iType==2);
			double sG,tG;

// if it has a delay, then it just injects into the post-synaptic neuron, no feedback onto pre-synaptic neurons
			if (pCx->m_dDelay>0.)
			{
				double *pTimeToNext;
				pCx->DecrementLatencies(m_dTimeStep);
				if ((pTimeToNext=pCx->GetTimeToNextSpikePtr())!=NULL) // got spike in transit
				{
					if ((*pTimeToNext)<0) 	// spike has arrived
					{
						pCx->m_TransitCx.Del();				// remove spike from list
						targetV=pTargetN->GetMemPot();
						junctV=sourceV-targetV;
						junctG=pCx->m_dPartialBlock*GetScaleElecCond(m_aryElecSyn[pCx->m_iID]->m_dLowCoup,
							m_aryElecSyn[pCx->m_iID]->m_dHiCoup,
							junctV,m_aryElecSyn[pCx->m_iID]->m_dTurnOnV,
							m_aryElecSyn[pCx->m_iID]->m_dSaturateV);

						tG=junctG/pTargetN->m_dSize;
						pTargetN->InElectricalSynapseCond(tG);
						pTargetN->InElectricalSynapseCurr(tG*(pSourceN->GetRestingPot()+(Neuron::m_dSpikePeak-pSourceN->GetRestingPot())*Neuron::m_dSpikeStrength-pTargetN->GetRestingPot()));
					}
				}
			}
			else
			{
				
				targetV=pTargetN->GetMemPot();
				junctV=sourceV-targetV;
				junctG=pCx->m_dPartialBlock*GetScaleElecCond(m_aryElecSyn[pCx->m_iID]->m_dLowCoup,
					m_aryElecSyn[pCx->m_iID]->m_dHiCoup,
					junctV,m_aryElecSyn[pCx->m_iID]->m_dTurnOnV,
					m_aryElecSyn[pCx->m_iID]->m_dSaturateV);
	// adjust for size of cell
	// for small cells, the electrical conductance is relatively bigger
				sG=junctG/pSourceN->m_dSize;
				tG=junctG/pTargetN->m_dSize;
				pSourceN->InElectricalSynapseCond(sG);
				pTargetN->InElectricalSynapseCond(tG);


				if (pTargetN->GetSpike())
					pSourceN->InElectricalSynapseCurr(sG*(pTargetN->GetRestingPot()+(Neuron::m_dSpikePeak-pTargetN->GetRestingPot())*Neuron::m_dSpikeStrength-pSourceN->m_dRestingPot));
				else
					pSourceN->InElectricalSynapseCurr(sG*(pTargetN->GetMemPot()-pSourceN->m_dRestingPot));

				
				if (pSourceN->GetSpike())
					pTargetN->InElectricalSynapseCurr(tG*(pSourceN->GetRestingPot()+(Neuron::m_dSpikePeak-pSourceN->GetRestingPot())*Neuron::m_dSpikeStrength-pTargetN->m_dRestingPot));
				else
					pTargetN->InElectricalSynapseCurr(tG*(pSourceN->GetMemPot()-pTargetN->m_dRestingPot));
			}
		}
	}

// Neuron::Update does stimulus, endogenous, + spontaneous spiking chem input. 
// it stores m_NewMemPot, so can update when all neurons have done their updates using memPot from previous iteration 
	for (i=0; i<GetNeuronCount(); i++)
	{
		GetNeuronAt(i)->CalcUpdate(this);
	}

// now switch newMemPot for memPot, decide if spiking etc
// Draws neuron if colour-from-potential
	for (i=0; i<GetNeuronCount(); i++)
	{
		GetNeuronAt(i)->CalcUpdateFinal(this);
	}

	
// get all spiking neurons after doing all updates 
// and save old membrane potentials for non-spiking and electrical synapses
// ZAPPED neurons never spike
	for (i=0; i<GetConnexionCount(); i++)
	{
		pCx=GetConnexionAt(i);
		pSourceN=pCx->m_lpSource;
		if (pSourceN->GetSpike())
			pCx->AppendTransitSpike();
		pTargetN=pCx->m_lpTarget;
		if (pTargetN->GetSpike() && !m_bFreezeLearning)
			pCx->IncrementHebbian();
	}
}

//NeuralModule overrides

/**
\brief	Searches for a neuron by the specified ID and returns its position in the list.

\author	dcofer
\date	3/30/2011

\param	strID	   	GUID ID for the neuron. 
\param	bThrowError	true to throw error if neuron not found. 

\return	The found neuron list position.
**/
int IntegrateFireNeuralModule::FindNeuronListPos(string strID, BOOL bThrowError)
{
	string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryNeurons.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNeurons[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Rn_Err_lNeuronNotFound, Rn_Err_strNeuronNotFound, "ID");

	return -1;
}

/**
\brief	Searches for a synapse by the specified ID and returns its position in the list.

\author	dcofer
\date	3/30/2011

\param	strID	   	GUID ID for the synapse. 
\param	bThrowError true to throw error if synapse not found. 

\return	The found synapse list position.
**/
int IntegrateFireNeuralModule::FindSynapseListPos(string strID, BOOL bThrowError)
{
	string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryConnexion.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryConnexion[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Rn_Err_lSynapseNotFound, Rn_Err_strSynapseNotFound, "ID");

	return -1;
}

/**
\brief	Searches for a spiking chemical synapse by the specified ID and returns its position in the list.

\author	dcofer
\date	3/30/2011

\param	strID	   	GUID ID for the synapse. 
\param	bThrowError true to throw error if synapse not found. 

\return	The found synapse list position.
**/
int IntegrateFireNeuralModule::FindSpikingChemListPos(string strID, BOOL bThrowError)
{
	string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_arySpikingChemSyn.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_arySpikingChemSyn[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Rn_Err_lSynapseNotFound, Rn_Err_strSynapseNotFound, "ID");

	return -1;
}

/**
\brief	Searches for a non-spiking chemical synapse by the specified ID and returns its position in the list.

\author	dcofer
\date	3/30/2011

\param	strID	   	GUID ID for the synapse. 
\param	bThrowError true to throw error if synapse not found. 

\return	The found synapse list position.
**/
int IntegrateFireNeuralModule::FindNonSpikingChemListPos(string strID, BOOL bThrowError)
{
	string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryNonSpikingChemSyn.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryNonSpikingChemSyn[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Rn_Err_lSynapseNotFound, Rn_Err_strSynapseNotFound, "ID");

	return -1;
}

/**
\brief	Searches for an electrical synapse by the specified ID and returns its position in the list.

\author	dcofer
\date	3/30/2011

\param	strID	   	GUID ID for the synapse. 
\param	bThrowError true to throw error if synapse not found. 

\return	The found synapse list position.
**/
int IntegrateFireNeuralModule::FindElectricalListPos(string strID, BOOL bThrowError)
{
	string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryElecSyn.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryElecSyn[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_TEXT_ERROR(Rn_Err_lSynapseNotFound, Rn_Err_strSynapseNotFound, "ID");

	return -1;
}

#pragma region DataAccesMethods

BOOL IntegrateFireNeuralModule::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "TIMESTEP")
	{
		TimeStep(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "TTX")
	{
		TTX(Std_ToBool(strValue));
		return TRUE;
	}

	if(strType == "CD")
	{
		Cd(Std_ToBool(strValue));
		return TRUE;
	}

	if(strType == "HH")
	{
		HH(Std_ToBool(strValue));
		return TRUE;
	}

	if(strType == "RETAINHEBBMEMORY")
	{
		RetainHebbMemory(Std_ToBool(strValue));
		return TRUE;
	}

	if(strType == "USECRITICALPERIOD")
	{
		UseCriticalPeriod(Std_ToBool(strValue));
		return TRUE;
	}

	if(strType == "STARTCRITICALPERIOD")
	{
		StartCriticalPeriod(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "ENDCRITICALPERIOD")
	{
		EndCriticalPeriod(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "FREEZEHEBB")
	{
		FreezeHebb(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "SPIKEPEAK")
	{
		SpikePeak(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "SPIKESTRENGTH")
	{
		SpikeStrength(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "CAEQUILPOT")
	{
		CaEquilPot(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "ABSOLUTEREFR")
	{
		AbsoluteRefr(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

/**
\brief	Adds a neuron by using xml. 

\author	dcofer
\date	3/30/2011

\param	strXml	The string xml to load. 
**/
void IntegrateFireNeuralModule::AddNeuron(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Neuron");

	Neuron *lpNeuron = LoadNeuron(oXml);
	PreCalc();
}

/**
\brief	Removes the neuron with the specified ID.

\author	dcofer
\date	3/30/2011

\param	strID	   	GUID ID for the neuron to remove. 
\param	bThrowError	true to throw error if no neuron is found. 
**/
void IntegrateFireNeuralModule::RemoveNeuron(string strID, BOOL bThrowError)
{
	int iPos = FindNeuronListPos(strID, bThrowError);
	m_aryNeurons.RemoveAt(iPos);
	PreCalc();
}

/**
\brief	Adds a synapse by xml. 

\author	dcofer
\date	3/30/2011

\param	strXml	The xml to load. 
**/
void IntegrateFireNeuralModule::AddSynapse(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Connexion");

	Connexion *pCx = LoadConnexion(oXml);
	PreCalc();
}

/**
\brief	Adds a synapse type by xml. 

\author	dcofer
\date	3/30/2011

\param	strXml	The xml to load. 
**/
void IntegrateFireNeuralModule::AddSynapseType(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("SynapseType");

	SynapseType *pType = LoadSynapseType(oXml);
	PreCalc();
}

/**
\brief	Removes the synapse with the specified ID.

\author	dcofer
\date	3/30/2011

\param	strID	   	GUUId for the string. 
\param	bThrowError	true to throw error. 
**/
void IntegrateFireNeuralModule::RemoveSynapse(string strID, BOOL bThrowError)
{
	int iPos = FindSynapseListPos(strID, bThrowError);
	m_aryConnexion.RemoveAt(iPos);
	PreCalc();
}

void IntegrateFireNeuralModule::RemoveSynapseType(string strID, BOOL bThrowError)
{
	SynapseType *lpType = dynamic_cast<SynapseType *>(GetSimulator()->FindByID(strID, bThrowError));

	if(lpType)
	{
		string strType = Std_ToUpper(lpType->Type());
		if(strType == "SPIKINGCHEMICAL")
		{
			int iPos = FindSpikingChemListPos(strID, bThrowError);
			m_arySpikingChemSyn.RemoveAt(iPos);
		}
		else if(strType == "NONSPIKINGCHEMICAL")
		{
			int iPos = FindNonSpikingChemListPos(strID, bThrowError);
			m_aryNonSpikingChemSyn.RemoveAt(iPos);
		}
		else if(strType == "NONSPIKINGCHEMICAL")
		{
			int iPos = FindElectricalListPos(strID, bThrowError);
			m_aryElecSyn.RemoveAt(iPos);
		}
		else
		{
			if(bThrowError)
				THROW_PARAM_ERROR(Rn_Err_lInvalidSynapseType, Rn_Err_strInvalidSynapseType, "Type", strType);
			return;
		}

		PreCalc();
	}
}

BOOL IntegrateFireNeuralModule::AddItem(string strItemType, string strXml, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "NEURON")
	{
		AddNeuron(strXml);
		return TRUE;
	}
	else if(strType == "SYNAPSE")
	{
		AddSynapse(strXml);
		return TRUE;
	}
	else if(strType == "SYNAPSETYPE")
	{
		AddSynapseType(strXml);
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL IntegrateFireNeuralModule::RemoveItem(string strItemType, string strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "NEURON")
	{
		RemoveNeuron(strID, bThrowError);
		return TRUE;
	}
	else if(strType == "SYNAPSE")
	{
		RemoveSynapse(strID, bThrowError);
		return TRUE;
	}
	else if(strType == "SYNAPSETYPE")
	{
		RemoveSynapseType(strID, bThrowError);
		return TRUE;
	}


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

#pragma endregion

void IntegrateFireNeuralModule::Kill(BOOL bState)
{
	int iSize = m_aryNeurons.GetSize(), iNeuron;
	for(iNeuron=0; iNeuron<iSize; iNeuron++)
		m_aryNeurons[iNeuron]->Kill(bState);
}

void IntegrateFireNeuralModule::ResetSimulation()
{
	m_dCurrentTime = 0;

	PostCalc();

	int iSize = m_aryNeurons.GetSize(), iIndex;
	for(iIndex=0; iIndex<iSize; iIndex++)
		m_aryNeurons[iIndex]->ResetSimulation();

	iSize = m_aryConnexion.GetSize();
	for(iIndex=0; iIndex<iSize; iIndex++)
		m_aryConnexion[iIndex]->ResetSimulation();

	PreCalc();	
}

void IntegrateFireNeuralModule::Initialize()
{
	srand(m_lpSim->ManualRandomSeed());

	NeuralModule::Initialize();
	PreCalc();
}

void IntegrateFireNeuralModule::StepSimulation()
{
	NeuralModule::StepSimulation();
	CalcUpdate();
}

void IntegrateFireNeuralModule::Load(CStdXml &oXml)
{
	CStdXml oNetXml;

	//if(Std_IsBlank(m_strProjectPath)) 
	//	THROW_ERROR(Al_Err_lProjectPathBlank, Al_Err_strProjectPathBlank);

	m_lpOrganism = dynamic_cast<Organism *>(m_lpStructure);
	if(!m_lpOrganism) 
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

	m_arySourceAdapters.RemoveAll();
	m_aryTargetAdapters.RemoveAll();

	oXml.IntoElem();  //Into NeuralModule Element

	m_strNeuralNetworkFile = oXml.GetChildString("NeuralNetFile", "");

	TRACE_DEBUG("Loading nervous system config file.\r\nProjectPath: " + m_strProjectPath + "\r\nFile: " + m_strNeuralNetworkFile);

	if(!Std_IsBlank(m_strNeuralNetworkFile)) 
	{
		oNetXml.Load(AnimatSim::GetFilePath(m_strProjectPath, m_strNeuralNetworkFile));

		oNetXml.FindElement("NeuralModule");
		oNetXml.FindChildElement("UseCriticalPeriod");

		LoadInternal(oNetXml);
	}
	else
		LoadInternal(oXml);

	oXml.OutOfElem(); //OutOf NeuralModule Element

	TRACE_DEBUG("Finished loading nervous system config file.");
}


}				//IntegrateFireSim

