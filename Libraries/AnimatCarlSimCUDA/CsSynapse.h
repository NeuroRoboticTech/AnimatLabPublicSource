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
	class ANIMAT_CARL_SIM_PORT CsSynapse : public AnimatSim::Link   
	{
	protected:
		/// Pointer to parent CsNeuralModule.
		//CsNeuralModule *m_lpCsModule;

		/// GUID ID of the pre-synaptic neruon.
		std::string m_strFromID;

		/// The pointer to pre-synaptic neuron
		CsNeuron *m_lpFromNeuron;

		/// The pointer to post-synaptic neuron
		CsNeuron *m_lpToNeuron;

	public:
		CsSynapse();
		virtual ~CsSynapse();

		/**
		\brief	Gets the pre-synaptic neuron.
			
		\author	dcofer
		\date	3/29/2011
			
		\return	Pointer to the neuron.
		**/
		CsNeuron *FromNeuron() {return m_lpFromNeuron;};

#pragma region DataAccesMethods
		virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
		virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
#pragma endregion

		virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify);
		virtual void VerifySystemPointers();
		virtual void Initialize();
		virtual void Load(CStdXml &oXml);
	};

}				//AnimatCarlSim
