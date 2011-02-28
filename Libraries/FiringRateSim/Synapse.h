#pragma once

namespace FiringRateSim
{

	/**
	\namespace	FiringRateSim::Synapses

	\brief	Contains all of the synapse classes for the firing rate neural model. 
	**/
	namespace Synapses
	{

		class FAST_NET_PORT Synapse : public AnimatSim::Link   
		{
		protected:
			CStdPtrArray<Synapse> m_arySynapses;

			FiringRateModule *m_lpFastModule;
			Organism *m_lpOrganism;

			BOOL m_bEnabled;
			float m_fltWeight;
			float m_fltModulation;

			string m_strFromID;
			Neuron *m_lpFromNeuron;
			Neuron *m_lpToNeuron;

			Synapse *LoadSynapse(Simulator *lpSim, Structure *lpStructure, Neuron *lpNeuron, CStdXml &oXml);

		public:
			Synapse();
			virtual ~Synapse();

			BOOL Enabled();
			void Enabled(BOOL bVal);

			Neuron *FromNeuron() {return m_lpFromNeuron;};

			float Weight();
			void Weight(float fltVal);
			float *WeightPointer();

			float Modulation();
			float *ModulationPointer();
			virtual float CalculateModulation(FiringRateModule *lpModule);
			virtual Synapse *GetCompoundSynapse(short iCompoundIndex);
			virtual int FindSynapseListPos(string strID, BOOL bThrowError = TRUE);
			virtual void AddSynapse(string strXml);
			virtual void RemoveSynapse(string strID, BOOL bThrowError = TRUE);

#pragma region DataAccesMethods
			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
			virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);
#pragma endregion

			virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure, Node *lpNode);
			virtual void Initialize(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode);
			virtual void Load(Simulator *lpSim, Structure *lpStructure, Node *lpNode, CStdXml &oXml);
			virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure, Node *lpNode) {}; //Not used here
		};

	}			//Synapses
}				//FiringRateSim
