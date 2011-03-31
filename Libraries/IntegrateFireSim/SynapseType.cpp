/**
\file	SynapseType.cpp

\brief	Implements the synapse type class.
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
	namespace Synapses
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/31/2011
**/
SynapseType::SynapseType()
{
	m_lpModule = NULL;
	m_iSynapseTypeID = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
SynapseType::~SynapseType()
{

}

#pragma region Accessor-Mutators

/**
\brief	Sets the NeuralModule pointer.

\author	dcofer
\date	3/31/2011

\param [in,out]	lpModule	Pointer to the NeuralModule. 
**/
void SynapseType::NeuralModule(IntegrateFireNeuralModule *lpModule) {m_lpModule = lpModule;}

/**
\brief	Gets the synapse type identifier.

\author	dcofer
\date	3/31/2011

\return	synapse type identifier.
**/
int SynapseType::SynapseTypeID() {return m_iSynapseTypeID;}

/**
\brief	Sets the synapse type identifier.

\author	dcofer
\date	3/31/2011

\param	iID	The identifier. 
**/
void SynapseType::SynapseTypeID(int iID) {m_iSynapseTypeID = iID;}

#pragma endregion

	}			//Synapses
}				//IntegrateFireSim
