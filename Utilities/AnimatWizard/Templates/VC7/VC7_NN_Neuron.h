// Neuron.h: interface for the Neuron class.
//
//////////////////////////////////////////////////////////////////////

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace [*PROJECT_NAME*]
{
	namespace Neurons
	{

		class [*TAG_NAME*]_PORT Neuron : public AnimatLibrary::Node   
		{
		protected:

			float m_fltExternalI;	//Externally injected current
			float m_fltSynapticI;	//Current synaptic current.
			float m_fltAdapterI; //current added from all of the adapters.
			float m_fltAdapterMemoryI;  //Used to allow datacharts to track current input from adapters.
			float m_fltFiringFreq;  //Current firing frequency.
			float m_fltVndisp;      // this is the membrane voltage that is reported back to animatlab.
			float m_fltVrest;      //Rest potential.

			CStdPtrArray<Synapse> m_arySynapses;

			virtual float CalculateSynapticCurrent(Simulator *lpSim, Organism *lpOrganism, [*PROJECT_NAME*]NeuralModule *lpModule);

			Synapse *LoadSynapse(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);

		public:
			Neuron();
			virtual ~Neuron();

			virtual unsigned char NeuronType();

			virtual CStdPtrArray<Synapse> *GetSynapses();
			virtual void AddSynapse(Synapse *lpSynapse);
			virtual void RemoveSynapse(int iIndex);
			virtual Synapse *GetSynapse(int iIndex);
			virtual int TotalSynapses();
			virtual void ClearSynapses();

			virtual void Initialize(Simulator *lpSim, Organism *lpOrganism, [*PROJECT_NAME*]NeuralModule *lpModule);
			virtual void StepSimulation(Simulator *lpSim, Organism *lpOrganism, [*PROJECT_NAME*]NeuralModule *lpModule, unsigned char iXPos, unsigned char iYPos, unsigned char iZPos);

			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);

			//Node Overrides
			virtual void AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput);
			virtual float *GetDataPointer(string strDataType);

			//This is not used. The one above is used because we have to pass in the neuron indexes
			virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure) {};
			virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		};

	}			//Neurons
}				//[*PROJECT_NAME*]
