/**
\file	GatedSynapse.h

\brief	Declares the gated synapse class.
**/

#pragma once

namespace FiringRateSim
{
	namespace Synapses
	{
		/**
		\brief	Gated firing rate synapse. 

		\details A gated synapse modulates another, regular synapse that is also connected to the same post-synaptic neuron.
		It gates it, or turns it on or off. Regular synapses  keep a list of all of synapses that modulate it. While it is 
		calculating its synaptic current it calls CalculateModulation for all CompoundSynapses
		to determine how to modulate itself. The user can have the gate initially on or off, and activity in the pre-synaptic neuron
		controls closing/opening of the gate.
		
		\author	dcofer
		\date	3/30/2011
		**/
		class FAST_NET_PORT GatedSynapse : public Synapse    
		{
		protected:
			/// Tells whether the gate is initially open or closed.
			unsigned char m_iInitialGateValue;

		public:
			GatedSynapse();
			virtual ~GatedSynapse();

			virtual unsigned char InitialGateValue();
			virtual void InitialGateValue(unsigned char iVal);

			virtual float CalculateModulation(FiringRateModule *lpModule);

#pragma region DataAccesMethods
			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
#pragma endregion

			virtual void Load(CStdXml &oXml);
		};

	}			//Synapses
}				//FiringRateSim
