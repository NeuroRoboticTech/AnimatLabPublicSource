// SynapseType.h: interface for the SpikingChemicalSynapse class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace IntegrateFireSim
{
	namespace Synapses
	{

		class ADV_NEURAL_PORT SynapseType : public AnimatBase  
		{
		public:
			 SynapseType();
			 virtual ~SynapseType();

#pragma region Accessor-Mutators

			 void NeuralModule(IntegrateFireNeuralModule *lpModule) {m_lpModule = lpModule;};

			 int SynapseTypeID() {return m_iSynapseTypeID;};
			 void SynapseTypeID(int iID) {m_iSynapseTypeID = iID;};

#pragma endregion

		protected:
			IntegrateFireNeuralModule *m_lpModule;

			int m_iSynapseTypeID;
		};

	}			//Synapses
}				//IntegrateFireSim

