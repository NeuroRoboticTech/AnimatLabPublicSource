// Connexion.cpp: implementation of the Connexion class.
//
//////////////////////////////////////////////////////////////////////

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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

Connexion::Connexion()
{
	m_iSource = -1;
	m_iTarget = -1;
	m_dPartialBlock=1.0;
	m_lpSynType = NULL;

	m_dDelay = 0;

	m_dBaseG = 0.5f;
	m_dG = m_dBaseG;
	m_dGFacilCx = m_dBaseG;
	m_dTimeSincePrevHebbEvent = 0;
	m_dPreviousSpikeLatency = 0;
	m_dPartialBlockHold = 0;
}
Connexion::Connexion(int type, int ID, double delay,float topBlock,float botBlock)
{
	m_iSource = -1;
	m_iTarget = -1;
	m_iType=type;
	m_iID=ID;
	m_dDelay=delay;

	m_dPartialBlock=1.0;
	m_lpSynType = NULL;

	m_dBaseG = 0.5f;
	m_dG = m_dBaseG;
	m_dGFacilCx = m_dBaseG;
	m_dTimeSincePrevHebbEvent = 0;
	m_dPreviousSpikeLatency = 0;
	m_dPartialBlockHold = 0;
}

Connexion::~Connexion()
{

}

void Connexion::ResetIDs()
{
	if(m_lpSynType)
		m_iID = m_lpSynType->SynapseTypeID();

	if(m_lpSource)
		m_iSource = m_lpSource->NeuronID();

	if(m_lpTarget)
		m_iTarget = m_lpTarget->NeuronID();
}

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

double Connexion::RelFacil()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": FacilD");

		return lpSyn->RelFacil();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": RelFacil");
	return 0;
}

void Connexion::DecrementFacilitation() 
{
	if (RelFacil()==1) 
		return;
	else 
		m_dGFacilCx=m_dG+(m_dGFacilCx-m_dG)*FacilD();
}

BOOL Connexion::VoltDep()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": FacilD");

		return lpSyn->VoltDep();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": VoltDep");
	return FALSE;
}

BOOL Connexion::Hebbian()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": FacilD");

		return lpSyn->Hebbian();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": Hebbian");
	return FALSE;
}

double Connexion::HebbIncrement()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": FacilD");

		return lpSyn->HebbIncrement();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": HebbIncrement");
	return 0;
}

double Connexion::HebbTimeWindow()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": FacilD");

		return lpSyn->HebbTimeWindow();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": HebbTimeWindow");
	return 0;
}

double Connexion::MaxGHebb()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": FacilD");

		return lpSyn->MaxGHebb();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": MaxGHebb");
	return 0;
}

BOOL Connexion::AllowForgetting()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": FacilD");

		return lpSyn->AllowForgetting();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": AllowForgetting");
	return FALSE;
}

double Connexion::ForgettingWindow()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": FacilD");

		return lpSyn->ForgettingWindow();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": ForgettingWindow");
	return 0;
}

double Connexion::Consolidation()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": FacilD");

		return lpSyn->Consolidation();
	}
	else
		THROW_TEXT_ERROR(Rn_Err_lSynpaseTypeNotDefined, Rn_Err_strSynpaseTypeNotDefined, "ID: " + m_strID + ": Consolidation");
	return 0;
}

double Connexion::MaxGVoltDepRel()
{
	if(m_lpSynType)
	{
		SpikingChemicalSynapse *lpSyn = dynamic_cast<SpikingChemicalSynapse *>(m_lpSynType);
		if(!lpSyn)
			THROW_TEXT_ERROR(Rn_Err_lNotChemSpikeSyn, Rn_Err_strNotChemSpikeSyn, "ID: " + m_strID + ": FacilD");

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
	m_dPartialBlockHold=oXml.GetChildDouble("PartialBlockHold", 0);

	BaseConductance(oXml.GetChildDouble("G"));
	Delay(oXml.GetChildDouble("Delay"));

	oXml.OutOfElem(); //OutOf Neuron Element
}

////////////////////////////////////
// WORKING

void Connexion::DecrementLatencies(double dt,BOOL FreezeLearning)
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
		BOOL bTimedOut=FALSE;
		while (1) 
		{			// decrease time marker latency for all previous Hebb events
			pTimeToNext=m_HebbList.Iterate();
			if (pTimeToNext==NULL)		// end of Hebb list 
				break;
//TRACE("decrementing Hebbian time window\n");
			(*pTimeToNext)-=dt;
			if ((*pTimeToNext)<=0)		// can't delete now because resets iterator
				bTimedOut=TRUE;
		}
		if (bTimedOut==TRUE)
		{
//TRACE("a Hebb input timed out; deleting from list\n");
			m_HebbList.Del();
		}
		if (m_dTimeSincePrevHebbEvent<ForgettingWindow())		// prevent overflow
			m_dTimeSincePrevHebbEvent+=dt;
	}
}

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
//TRACE("time left in Hebb window = %lf, increment factor = %lf\n",*pTimeLeftInHebbWindow,m_HebbIncrement*((*pTimeLeftInHebbWindow)/m_HebbTimeWindow));
//TRACE("and now m_G is %lf, facil G is %lf\n",m_G, m_GFacilCx);
			m_dTimeSincePrevHebbEvent=0;
		}
	}
}

double Connexion::ProcessOutput(BOOL bFreezeHebb)
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
	G=max(0,m_dGFacilCx);	// get conductance, if not facil below 0
	m_dGFacilCx=(m_dGFacilCx-m_dG)+(m_dG*RelFacil());	// facilitate next response
	m_TransitCx.Del();				// remove spike from list

// if Hebbian, store set time window
	if (Hebbian())
	{
		m_HebbList.AddTail(HebbTimeWindow());
//		m_PreviousSpikeLatency=m_HebbTimeWindow;
//TRACE("appending Hebbian time window to Hebb list\n");

	}
	return G;
}

double Connexion::GetProspectiveCond(BOOL bFreezeHebb)
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
	return(max(0,G));	// get conductance, if not facil below 0
}

void Connexion::ResetSimulation()
{
	m_dGFacilCx = m_dBaseG;
	m_dG = m_dBaseG;
	m_TransitCx.Release();
	m_HebbList.Release();
}


#pragma region DataAccesMethods

BOOL Connexion::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);
	
	if(strType == "SYNAPTICCONDUCTANCE")
	{
		BaseConductance(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "CONDUCTIONDELAY")
	{
		Delay(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "SYNAPSETYPEID")
	{
		SynapseTypeID(strValue);
		m_lpModule->PreCalc();
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}
#pragma endregion


	}			//Synapses
}				//IntegrateFireSim

