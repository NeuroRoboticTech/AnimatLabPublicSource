/**
\file	NervousSystem.h

\brief	Declares the nervous system class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Behavior
	{
		/**
		\class	NervousSystem
		
		\brief	Nervous system for an organism. 

		\details An organism is a Structure that has a nervous system. The nervous system is made up of 
		one or more NeuralModules, where each NeuralModule implements a given neural simulation method, 
		firing rate vs. integrate and fire. Each of these neural modules is simulated independently with 
		different integration time steps. You can connect elements in one module with elements in another
		by using an Adapter. This allow you to convert the firing rate, or other variable of interest, 
		of one neuron into a current injected into a neuron in the other module. 
		
		\author	dcofer
		\date	2/24/2011
		**/
		class ANIMAT_PORT NervousSystem : public AnimatBase 
		{
		protected:
			/// The pointer to this node's organism
			Organism *m_lpOrganism;

			CStdPtrMap<string, NeuralModule> m_aryNeuralModules;
			CStdPtrArray<Adapter> m_aryAdapters;

			NeuralModule *LoadNeuralModule(CStdXml &oXml);
			void AddNeuralModule(NeuralModule *lpModule);

			Adapter *LoadAdapter(CStdXml &oXml);

		public:
			NervousSystem();
			virtual ~NervousSystem();

			virtual NeuralModule *FindNeuralModule(string strModuleName, BOOL bThrowError = TRUE);
			virtual void AddNeuralModule(string strXml);
			virtual void RemoveNeuralModule(string strID);

			virtual void Kill(BOOL bState = TRUE);
			virtual void ResetSimulation();

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify = TRUE);
			virtual void VerifySystemPointers();
			virtual void Initialize();
			virtual void StepSimulation();

#pragma region SnapshotMethods
			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
#pragma endregion

			virtual void Load(CStdXml &oXml);
		};

	}			//Behavior
}			//AnimatSim
