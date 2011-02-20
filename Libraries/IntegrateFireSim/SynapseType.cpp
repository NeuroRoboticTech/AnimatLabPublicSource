// SynapseType.cpp: implementation of the SpikingChemicalSynapse class.
//
//////////////////////////////////////////////////////////////////////

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
	namespace Synapses
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


SynapseType::SynapseType()
{
	m_lpModule = NULL;
	m_iSynapseTypeID = 0;
}

SynapseType::~SynapseType()
{

}

	}			//Synapses
}				//IntegrateFireSim
