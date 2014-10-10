/**
\file	Connexion.cpp

\brief	Implements the connexion class.
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
	namespace Synapses
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/31/2011
**/
Connexion::Connexion()
{
	m_iSource = -1;
	m_iTarget = -1;
	//m_dPartialBlock=1.0;
	m_lpSynType = NULL;

	m_dDelay = 0;

	m_dBaseG = 0.5f;
	m_dG = m_dBaseG;
	m_dGFacilCx = m_dBaseG;
	m_dTimeSincePrevHebbEvent = 0;
	m_dPreviousSpikeLatency = 0;
	//m_dPartialBlockHold = 0;
	m_fltGFailCxReport = m_dGFacilCx;
	m_fltGReport = m_dG;
}

/**
\brief	Constructor.

\author	dcofer
\date	3/31/2011

\param	type		The synapse type. 
\param	ID			The identifier. 
\param	delay   	The synaptic delay. 
\param	topBlock	The top block. 
\param	botBlock	The bottom block. 
**/
Connexion::Connexion(int type, int ID, double delay,float topBlock,float botBlock)
{
	m_iSource = -1;
	m_iTarget = -1;
	m_iType=type;
	m_iID=ID;
	m_dDelay=delay;

	//m_dPartialBlock=1.0;
	m_lpSynType = NULL;

	m_dBaseG = 0.5f;
	m_dG = m_dBaseG;
	m_dGFacilCx = m_dBaseG;
	m_dTimeSincePrevHebbEvent = 0;
	m_dPreviousSpikeLatency = 0;
	//m_dPartialBlockHold = 0;
	m_fltGFailCxReport = m_dGFacilCx;
	m_fltGReport = m_dG;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
Connexion::~Connexion()
{

}

#pragma region Accessor-Mutators

/**
\brief	Sets the base conductance.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void Connexion::BaseConductance(double dVal) 
{
	//The mempot variables are calculated, so we do not want to just re-set them to the new value.
	//instead lets adjust them by the difference between the old and new resting potential.
	double dDiff = dVal - m_dBaseG;

	m_dBaseG = dVal;
	m_dG += dDiff;
	m_dGFacilCx += dDiff;

	m_fltGFailCxReport = m_dGFacilCx;
	m_fltGReport = m_dG;
}

/**
\brief	Gets the base conductance.

\author	dcofer
\date	3/31/2011

\return	base conductance.
**/
double Connexion::BaseConductance() {return m_dBaseG;}

/**
\brief	Sets the synaptic delay.

\author	dcofer
\date	3/31/2011

\param	dVal	The new value. 
**/
void Connexion::Delay(double dVal) {m_dDelay = dVal;}

/**
\brief	Gets the synaptic delay.

\author	dcofer
\date	3/31/2011

\return	synaptic delay.
**/
double Connexion::Delay() {return m_dDelay;}

/**
\brief	Gets the synapse type identifier.

\author	dcofer
\date	3/31/2011

\return	synapse type identifier.
**/
std::string Connexion::SynapseTypeID() {return m_strSynapseTypeID;}

/**
\brief	Sets the synapse type identifier.

\author	dcofer
\date	3/31/2011

\param	strID	Synapse Type ID. 
**/
void Connexion::SynapseTypeID(std::string strID) {m_strSynapseTypeID = strID;}

/**
\brief	Gets the source neuron ID.

\author	dcofer
\date	3/31/2011

\return	ID.
**/
std::string Connexion::SourceID() {return m_strSourceID;}

/**
\brief	Sets the source neuron ID.

\author	dcofer
\date	3/31/2011

\param	strID	ID.
**/
void Connexion::SourceID(std::string strID) {m_strSourceID = strID;}

/**
\brief	Gets the target neuron ID.

\author	dcofer
\date	3/31/2011

\return	ID.
**/
std::string Connexion::TargetID() {return m_strTargetID;}

/**
\brief	Sets the target neuron ID.

\author	dcofer
\date	3/31/2011

\param	strID	ID. 
**/
void Connexion::TargetID(std::string strID) {m_strTargetID = strID;}

/**
\brief	Resets the IDs of the synapse types.

\author	dcofer
\date	3/31/2011
**/
void Connexion::ResetIDs()
{
	if(m_lpSynType)
		m_iID = m_lpSynType->SynapseTypeID();

	if(m_lpSource)
		m_iSource = m_lpSource->NeuronID();

	if(m_lpTarget)
		m_iTarget = m_lpTarget->NeuronID();
}

#pragma endregion

/**
\brief	Gets the facilitation decrement value.

\author	dcofer
\date	3/31/2011

\return	facilitation decrement.
**/
double Connexion::FacilD() 
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": FacilD");

		return lpSyn->FacilD();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": FacilD");
	return 0;
}

/**
\brief	Gets the relative facilitation.

\author	dcofer
\date	3/31/2011

\return	relative facilitation.
**/
double Connexion::RelFacil()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": RelFacil");

		return lpSyn->RelFacil();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": RelFacil");
	return 0;
}

/**
\brief	Decrements the facilitation.

\author	dcofer
\date	3/31/2011
**/
void Connexion::DecrementFacilitation() 
{
	if (RelFacil()==1) 
		return;
	else 
	{
		m_dGFacilCx=m_dG+(m_dGFacilCx-m_dG)*FacilD();
		m_fltGFailCxReport = m_dGFacilCx;
	}
}

/**
\brief	Gets whether the synapse type is voltage dependent.

\author	dcofer
\date	3/31/2011

\return	true if it is voltage dependent, false else.
**/
bool Connexion::VoltDep()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(lpSyn)
			return lpSyn->VoltDep();
		else
			return false;
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": VoltDep");
	return false;
}

/**
\brief	Gets if the synapse type is Hebbian.

\author	dcofer
\date	3/31/2011

\return	true if it is hebbian, false else.
**/
bool Connexion::Hebbian()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(lpSyn)
			return lpSyn->Hebbian();
		else
			return false;
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": Hebbian");
	return false;
}

/**
\brief	Increments the hebbian weight.

\author	dcofer
\date	3/31/2011

\return	new weight.
**/
double Connexion::HebbIncrement()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": HebbIncrement");

		return lpSyn->HebbIncrement();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": HebbIncrement");
	return 0;
}

/**
\brief	Gets the hebbian time window.

\author	dcofer
\date	3/31/2011

\return	hebbian time window.
**/
double Connexion::HebbTimeWindow()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": HebbTimeWindow");

		return lpSyn->HebbTimeWindow();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": HebbTimeWindow");
	return 0;
}

/**
\brief	Gets the maximum hebbian conductance.

\author	dcofer
\date	3/31/2011

\return	maximum hebbian conductance.
**/
double Connexion::MaxGHebb()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": MaxGHebb");

		return lpSyn->MaxGHebb();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": MaxGHebb");
	return 0;
}

/**
\brief	Gets whether forgetting is allowed.

\author	dcofer
\date	3/31/2011

\return	true if forgetting is allowed, false else.
**/
bool Connexion::AllowForgetting()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(lpSyn)
			return lpSyn->AllowForgetting();
		else
			return false;
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": AllowForgetting");
	return false;
}

/**
\brief	Gets the forgetting time window.

\author	dcofer
\date	3/31/2011

\return	forgetting time window.
**/
double Connexion::ForgettingWindow()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": ForgettingWindow");

		return lpSyn->ForgettingWindow();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": ForgettingWindow");
	return 0;
}

/**
\brief	Gets the consolidation factor.

\author	dcofer
\date	3/31/2011

\return	consolidation factor.
**/
double Connexion::Consolidation()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": Consolidation");

		return lpSyn->Consolidation();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": Consolidation");
	return 0;
}

/**
\brief	Gets the maximum conductance for voltage dependent.

\author	dcofer
\date	3/31/2011

\return	conductance.
**/
double Connexion::MaxGVoltDepRel()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": MaxGVoltDepRel");

		return lpSyn->MaxGVoltDepRel();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": MaxGVoltDepRel");
	return 0;
}

void Connexion::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into SpikingChemSyn Element
//	m_dLowCoup= oXml.GetChildDouble("dLowCoup");

	//m_iSource=oXml.GetChildInt("Source");
	m_strSourceID = oXml.GetChildString("SourceID");
	//m_iTarget=oXml.GetChildInt("Target");
	m_strTargetID = oXml.GetChildString("TargetID");
	m_iType=oXml.GetChildInt("Type");
	//m_iID=oXml.GetChildInt("SynapseID");
	m_strSynapseTypeID = oXml.GetChildString("SynapseTypeID");
	//m_dPartialBlockHold=oXml.GetChildDouble("PartialBlockHold", 0);

	BaseConductance(oXml.GetChildDouble("G"));
	Delay(oXml.GetChildDouble("Delay"));

	oXml.OutOfElem(); //OutOf Neuron Element
}

////////////////////////////////////
// WORKING

/**
\brief	Decrements latencies.

\author	dcofer
\date	3/31/2011

\param	dt			  	The time step. 
\param	FreezeLearning	true to freeze learning. 
**/
void Connexion::DecrementLatencies(double dt,bool FreezeLearning)
{
	double *pTimeToNext;
	while (1) 
	{			// decrease latency for all spikes in transit
		pTimeToNext=m_TransitCx.Iterate();
		if (pTimeToNext==NULL)		// end of spike list for connexion
			break;
		(*pTimeToNext)-=dt;
	}
	
	if (Hebbian() && !FreezeLearning)
	{
		bool bTimedOut=false;
		while (1) 
		{			// decrease time marker latency for all previous Hebb events
			pTimeToNext=m_HebbList.Iterate();
			if (pTimeToNext==NULL)		// end of Hebb list 
				break;
//TRACE("decrementing Hebbian time window\n");
			(*pTimeToNext)-=dt;
			if ((*pTimeToNext)<=0)		// can't delete now because resets iterator
				bTimedOut=true;
		}
		if (bTimedOut==true)
		{
//TRACE("a Hebb input timed out; deleting from list\n");
			m_HebbList.Del();
		}
		if (m_dTimeSincePrevHebbEvent<ForgettingWindow())		// prevent overflow
			m_dTimeSincePrevHebbEvent+=dt;
	}
}

/**
\brief	Increments hebbian values.

\author	dcofer
\date	3/31/2011
**/
void Connexion::IncrementHebbian()
{
	if (Hebbian())
	{
		double newG, *pTimeLeftInHebbWindow;
		while (1) 	// iterate through Hebb list
		{			
			pTimeLeftInHebbWindow=m_HebbList.Iterate();
			if (pTimeLeftInHebbWindow==NULL)		// end of Hebb list 
				break;

//TRACE("Incrementing Hebbian synapse: m_G was %lf, facil G was %lf\n",m_G,m_GFacilCx);
			newG=m_dG+(MaxGHebb()-m_dG)*HebbIncrement()*((*pTimeLeftInHebbWindow)/HebbTimeWindow());
			//ASSERT(newG<=m_dMaxGHebb);
// increment current facilitation state by same percentage as base conductance
			m_dGFacilCx=m_dGFacilCx*newG/m_dG;
			m_dG=newG;
			m_fltGFailCxReport = m_dGFacilCx;
			m_fltGReport = m_dG;

//TRACE("time left in Hebb window = %lf, increment factor = %lf\n",*pTimeLeftInHebbWindow,m_HebbIncrement*((*pTimeLeftInHebbWindow)/m_HebbTimeWindow));
//TRACE("and now m_G is %lf, facil G is %lf\n",m_G, m_GFacilCx);
			m_dTimeSincePrevHebbEvent=0;
		}
	}
}

/**
\brief	Process the output described of the connection.

\author	dcofer
\date	3/31/2011

\param	bFreezeHebb	true to freeze hebb. 

\return	.
**/
double Connexion::ProcessOutput(bool bFreezeHebb)
{
// if allowing forgetting, decrement Hebb augmentation by fraction of ForgettingWindow
// since last Hebb augmentation event
	if (Hebbian() && AllowForgetting() && !bFreezeHebb)
	{
		double newG;
		double dblForgettingWindow=ForgettingWindow();
		if (Consolidation()!=1)
			dblForgettingWindow=dblForgettingWindow*(1+((Consolidation()-1)*(m_dG-m_dBaseG)/(MaxGHebb()-m_dBaseG)));
		if (m_dTimeSincePrevHebbEvent>=dblForgettingWindow)
			newG=m_dBaseG;
		else
			newG=m_dG-(m_dG-m_dBaseG)*m_dTimeSincePrevHebbEvent/dblForgettingWindow;
//ASSERT(m_dG>=m_dBaseG);
// decrease current facilitation state by same percentage as base conductance
		m_dGFacilCx=m_dGFacilCx*newG/m_dG;
		m_dG=newG;
//TRACE("forgetting window = %lf\n",ForgettingWindow);
	}

	double G;
	G=STD_MAX((double) 0,m_dGFacilCx);	// get conductance, if not facil below 0
	m_dGFacilCx=(m_dGFacilCx-m_dG)+(m_dG*RelFacil());	// facilitate next response
	m_TransitCx.Del();				// remove spike from list

// if Hebbian, store set time window
	if (Hebbian())
	{
		m_HebbList.AddTail(HebbTimeWindow());
//		m_PreviousSpikeLatency=m_HebbTimeWindow;
//TRACE("appending Hebbian time window to Hebb list\n");

	}

	m_fltGFailCxReport = m_dGFacilCx;
	m_fltGReport = m_dG;

	return G;
}

/**
\brief	Gets a prospective condutance.

\author	dcofer
\date	3/31/2011

\param	bFreezeHebb	true to freeze hebb. 

\return	The prospective condutance.
**/
double Connexion::GetProspectiveCond(bool bFreezeHebb)
{
	double G=m_dGFacilCx;
// if allowing forgetting, decrement Hebb augmentation by fraction of ForgettingWindow
// since last Hebb augmentation event
	if (Hebbian() && AllowForgetting() && !bFreezeHebb)
	{
		double newG;
		double dblForgettingWindow=ForgettingWindow();
		if (Consolidation()!=1)
			dblForgettingWindow=dblForgettingWindow*(1+((Consolidation()-1)*(m_dG-m_dBaseG)/(MaxGHebb()-m_dBaseG)));
		if (m_dTimeSincePrevHebbEvent>=dblForgettingWindow)
			newG=m_dBaseG;
		else
			newG=m_dG-(m_dG-m_dBaseG)*m_dTimeSincePrevHebbEvent/dblForgettingWindow;
//ASSERT(m_dG>=m_dBaseG);
// decrease current facilitation state by same percentage as base conductance
		G=m_dGFacilCx*newG/m_dG;
	}

//TRACE("GetProspectiveCond = %lf\n",G);
	return(STD_MAX((double) 0,G));	// get conductance, if not facil below 0
}

void Connexion::ResetSimulation()
{
	m_dGFacilCx = m_dBaseG;
	m_dG = m_dBaseG;
	m_TransitCx.Release();
	m_HebbList.Release();
}

void Connexion::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, AnimatSim::Behavior::NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, false);
	
	m_lpIGFModule = dynamic_cast<IntegrateFireNeuralModule *>(lpModule);

	if(bVerify) VerifySystemPointers();
}

void Connexion::VerifySystemPointers()
{
	AnimatBase::VerifySystemPointers();

	if(!m_lpIGFModule)
		THROW_PARAM_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined, "IGFModule: ", m_strID);
}

#pragma region DataAccesMethods

float *Connexion::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "CONDUCTANCE")
		return &m_fltGReport;

	if(strType == "FACILITATION")
		return &m_fltGFailCxReport;

	//If it was not one of those above then we have a problem.
	THROW_PARAM_ERROR(Rn_Err_lInvalidNeuronDataType, Rn_Err_strInvalidNeuronDataType, "Neuron Data Type", strDataType);

	return NULL;
}

bool Connexion::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
				
	if(Link::SetData(strDataType, strValue, false))
		return true;

	if(strType == "SYNAPTICCONDUCTANCE")
	{
		BaseConductance(atof(strValue.c_str()));
		return true;
	}

	if(strType == "CONDUCTIONDELAY")
	{
		Delay(atof(strValue.c_str()));
		return true;
	}

	if(strType == "SYNAPSETYPEID")
	{
		SynapseTypeID(strValue);
		m_lpIGFModule->PreCalc();
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Connexion::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Link::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Conductance", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("Facilitation", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("SynapticConductance", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("ConductionDelay", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("SynapseTypeID", AnimatPropertyType::String, AnimatPropertyDirection::Set));
}

#pragma endregion


	}			//Synapses
}				//IntegrateFireSim

