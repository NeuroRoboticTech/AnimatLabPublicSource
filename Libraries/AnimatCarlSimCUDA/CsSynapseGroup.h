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
	class ANIMAT_CARL_SIM_PORT CsSynapseGroup : public AnimatSim::Link   
	{
	protected:
		/// Pointer to parent CsNeuralModule.
		CsNeuralModule *m_lpCsModule;

		/// GUID ID of the pre-synaptic neruon.
		std::string m_strFromID;

		/// GUID ID of the pre-synaptic neruon.
		std::string m_strToID;

		/// The pointer to pre-synaptic neuron
		CsNeuronGroup *m_lpFromNeuron;

		/// The pointer to post-synaptic neuron
		CsNeuronGroup *m_lpToNeuron;

		/// initial weight strength (arbitrary units); should be negative for inhibitory connections 
		float m_fltInitWt;

		///upper bound on weight strength (arbitrary units); should be negative for inhibitory connections 
		float m_fltMaxWt;

		///connection probability 
		float m_fltPconnect;

		///the minimum delay allowed (ms) 
		unsigned char m_iMinDelay;

		///the maximum delay allowed (ms) 
		unsigned char m_iMaxDelay;

		///connection type, either SYN_FIXED or SYN_PLASTIC
		bool m_bPlastic;

		///The total number of synapses created.
		int m_iSynapsesCreated;

	public:
		CsSynapseGroup();
		virtual ~CsSynapseGroup();

		CsNeuralModule *GetCsModule() {return m_lpCsModule;};

		/**
		\brief	Gets the pre-synaptic neuron.
			
		\author	dcofer
		\date	3/29/2011
			
		\return	Pointer to the neuron.
		**/
		CsNeuronGroup *FromNeuron() {return m_lpFromNeuron;};

		virtual void InitWt(float fltVal);
		virtual float InitWt();

		virtual void MaxWt(float fltVal);
		virtual float MaxWt();

		virtual void Pconnect(float fltVal);
		virtual float Pconnect();

		virtual void MinDelay(unsigned char iVal);
		virtual unsigned char MinDelay();

		virtual void MaxDelay(unsigned char iVal);
		virtual unsigned char MaxDelay();

		virtual void Plastic(bool bVal);
		virtual bool Plastic();

		virtual int SynapsesCreated();

		virtual std::string GeneratorKey();

		virtual void SetCARLSimulation();

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
