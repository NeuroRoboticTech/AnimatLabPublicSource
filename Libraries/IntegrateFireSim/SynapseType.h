/**
\file	SynapseType.h

\brief	Declares the synapse type class.
**/

#pragma once

namespace IntegrateFireSim
{
	namespace Synapses
	{
		/**
		\brief	Synapse type base class. 
		
		\details All of the other synapse types are derived from this common base. 
		This includes the SpikingChemicalSynapse, NonSpikingChemicalSynapse, and ElectricalSynapse.

		\author	dcofer
		\date	3/31/2011
		**/
		class ADV_NEURAL_PORT SynapseType : public AnimatSim::AnimatBase  
		{
		public:
			 SynapseType();
			 virtual ~SynapseType();

#pragma region Accessor-Mutators

			 void NeuralModule(IntegrateFireNeuralModule *lpModule);

			 int SynapseTypeID();
			 void SynapseTypeID(int iID);

#pragma endregion

		protected:
			/// Pointer to the parent IntegrateFireNeuralModule.
			IntegrateFireNeuralModule *m_lpModule;

			/// Integre ID for the synapse type
			int m_iSynapseTypeID;
		};

	}			//Synapses
}				//IntegrateFireSim

