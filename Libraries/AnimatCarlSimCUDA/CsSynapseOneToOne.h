/**
\file	Synapse.h

\brief	Declares the synapse class.
**/

#pragma once

namespace AnimatCarlSim
{

	/**
	\brief	Firing rate synapse model.

	\details This synapse type has a weight that is a current value. It injects a portion of that weight
	into the post-synaptic neuron based on the pre-synaptic neurons firing rate. I = W*F. (Where W is the
	weight, F is the firing rate, and I is the current.)
		
	\author	dcofer
	\date	3/29/2011
	**/
	class ANIMAT_CARL_SIM_PORT CsSynapseOneToOne : public CsSynapseGroup   
	{
	protected:

	public:
		CsSynapseOneToOne();
		virtual ~CsSynapseOneToOne();

		virtual void SetCARLSimulation();

	};

}				//AnimatCarlSim
