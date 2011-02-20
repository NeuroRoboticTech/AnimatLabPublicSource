// [*PROJECT_NAME*]NeuralModule.h: interface for the [*PROJECT_NAME*]NeuralModule class.
//
//////////////////////////////////////////////////////////////////////

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace [*PROJECT_NAME*]
{

	class [*TAG_NAME*]_PORT [*PROJECT_NAME*]NeuralModule : public AnimatLibrary::Behavior::NeuralModule  
	{
	protected:
		CStdIPoint m_oNetworkSize;
		CStdPtrArray<Neuron> m_aryNeurons;

		BOOL m_bActiveArray;

		Neuron *LoadNeuron(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		void LoadNetworkXml(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);

		void GenerateAutoSeed();

	public:
		[*PROJECT_NAME*]NeuralModule();
		virtual ~[*PROJECT_NAME*]NeuralModule();

		//NeuralModule overrides
		virtual string ModuleName() {return "[*PROJECT_NAME*]";};

		virtual Node *FindNode(long lNodeID);

		virtual void Kill(Simulator *lpSim, Organism *lpOrganism, BOOL bState = TRUE);
		virtual void Reset(Simulator *lpSim, Organism *lpOrganism);

		virtual void Initialize(Simulator *lpSim, Structure *lpStructure);
		virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
		virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		virtual void Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);

		BOOL ActiveArray();
		void ActiveArray(BOOL bVal);
		BOOL InactiveArray();
		void InactiveArray(BOOL bVal);

		Neuron *GetNeuron(short iXPos, 
											short iYPos, 
											short iZPos);
		void SetNeuron(unsigned char iXPos, 
									unsigned char iYPos, 
									unsigned char iZPos, 
									Neuron *lpNeuron);

		virtual long CalculateSnapshotByteSize();
		virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
		virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
	};

}				//[*PROJECT_NAME*]

