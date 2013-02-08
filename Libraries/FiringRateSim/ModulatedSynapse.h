/**
\file	ModulatedSynapse.h

\brief	Declares the modulated synapse class.
**/

#pragma once

namespace FiringRateSim
{
	namespace Synapses
	{
		/**
		\brief	Modulated firing rate synapse. 

		\details A modulatory synapse modulates another, regular synapse that is also connected to the same post-synaptic neuron.
		It modulates its weight by amplyifing or reducing it. Regular synapses  keep a list of all of 
		synapses that modulate it. While it is calculating its synaptic current it calls CalculateModulation for all CompoundSynapses
		to determine how to modulate itself. If the weight of this synapse is less than one then it reduces the output of the modulated synapse, if its
		weight is above one then it amplifies the modulated synapse.

		\author	dcofer
		\date	3/30/2011
		**/
		class FAST_NET_PORT ModulatedSynapse : public Synapse    
		{
		public:
			ModulatedSynapse();
			virtual ~ModulatedSynapse();

#pragma region DataAccesMethods
			virtual float *GetDataPointer(const string &strDataType);
#pragma endregion

			virtual float CalculateModulation(FiringRateModule *lpModule);
		};

	}			//Synapses
}				//FiringRateSim
