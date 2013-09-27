/**
\file	Synapse.h

\brief	Declares the synapse class.
**/

#pragma once

namespace FiringRateSim
{

	/**
	\namespace	FiringRateSim::Synapses

	\brief	Contains all of the synapse classes for the firing rate neural model. 
	**/
	namespace Synapses
	{
		/**
		\brief	Firing rate synapse model.

		\details This synapse type has a weight that is a current value. It injects a portion of that weight
		into the post-synaptic neuron based on the pre-synaptic neurons firing rate. I = W*F. (Where W is the
		weight, F is the firing rate, and I is the current.)
		
		\author	dcofer
		\date	3/29/2011
		**/
		class FAST_NET_PORT Synapse : public AnimatSim::Link   
		{
		protected:
			/// Pointer to parent FiringRateModule.
			FiringRateModule *m_lpFRModule;

			/// Array of moduled/gated child synapses
			CStdPtrArray<Synapse> m_arySynapses;

			/// The weight of the synapse. This is a current value in nanoamps.
			float m_fltWeight;

			/// The modulation value to use for this synapse.
			float m_fltModulation;

			/// GUID ID of the pre-synaptic neruon.
			std::string m_strFromID;

			/// The pointer to pre-synaptic neuron
			Neuron *m_lpFromNeuron;

			/// The pointer to post-synaptic neuron
			Neuron *m_lpToNeuron;

			Synapse *LoadSynapse(CStdXml &oXml);

		public:
			Synapse();
			virtual ~Synapse();

			/**
			\brief	Gets the pre-synaptic neuron.
			
			\author	dcofer
			\date	3/29/2011
			
			\return	Pointer to the neuron.
			**/
			Neuron *FromNeuron() {return m_lpFromNeuron;};

			float Weight();
			void Weight(float fltVal);
			float *WeightPointer();

			float Modulation();
			float *ModulationPointer();
			virtual float CalculateModulation(FiringRateModule *lpModule);
			virtual Synapse *GetCompoundSynapse(short iCompoundIndex);
			virtual int FindSynapseListPos(std::string strID, bool bThrowError = true);
			virtual void AddSynapse(std::string strXml, bool bDoNotInit);
			virtual void RemoveSynapse(std::string strID, bool bThrowError = true);

#pragma region DataAccesMethods
			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);
			virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);
#pragma endregion

			/**
			\brief	Sets the system pointers.
		
			\details There are a number of system pointers that are needed for use in the objects. The
			primariy one being a pointer to the simulation object itself so that you can get global
			parameters like the scale units and so on. However, each object may need other types of pointers
			as well, for example neurons need to have a pointer to their parent structure/organism, and to
			the NeuralModule they reside within. So different types of objects will need different sets of
			system pointers. We call this method to set the pointers just after creation and before Load is
			called. We then call VerifySystemPointers here, during Load and during Initialize in order to
			ensure that the correct pointers have been set for each type of objects. These pointers can then
			be safely used throughout the rest of the system. 
		
			\author	dcofer
			\date	3/2/2011
		
			\param [in,out]	lpSim		The pointer to a simulation. 
			\param [in,out]	lpStructure	The pointer to the parent structure. 
			\param [in,out]	lpModule	The pointer to the parent module module. 
			\param [in,out]	lpNode		The pointer to the parent node. 
			\param	bVerify				true to call VerifySystemPointers. 
			**/
			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify);
			virtual void VerifySystemPointers();
			virtual void ResetSimulation();
			virtual void Initialize();
			virtual void Load(CStdXml &oXml);
		};

	}			//Synapses
}				//FiringRateSim
