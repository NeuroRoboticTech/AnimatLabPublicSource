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

			/// The array of neural modules used within this nervous system.
			CStdPtrMap<std::string, NeuralModule> m_aryNeuralModules;
			
			NeuralModule *LoadNeuralModule(CStdXml &oXml);
			void AddNeuralModule(NeuralModule *lpModule);

			virtual void StepSim();
			virtual void StepAdapters();

		public:
			NervousSystem();
			virtual ~NervousSystem();

			virtual NeuralModule *FindNeuralModule(std::string strModuleName, bool bThrowError = true);
			virtual void AddNeuralModule(std::string strXml);
			virtual void RemoveNeuralModule(std::string strID);

			virtual void Kill(bool bState = true);
			virtual void ResetSimulation();
			virtual void MinTimeStep(float &fltMin);

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
