/**
\file	PhysicaNeuralModule.h

\brief	Declares the Physicales module class.
**/

#pragma once

namespace AnimatSim
{
	namespace Behavior
	{

		/**
		\brief	Physics neural module.

		\details This is the basic neural module that holds the adapters used during the simulation.
	
		\author	dcofer
		\date	3/29/2011
		**/
		class ANIMAT_PORT PhysicsNeuralModule : public AnimatSim::Behavior::NeuralModule  
		{
		protected:
			/// The array of adapters in this module.
			CStdPtrArray<Adapter> m_aryAdapters;

			Adapter *LoadAdapter(CStdXml &oXml);

		public:
			PhysicsNeuralModule();
			virtual ~PhysicsNeuralModule();

			/**
			\brief	Gets the module name.
		
			\author	dcofer
			\date	3/29/2011
		
			\return	.
			**/
			virtual string ModuleName() {return "PhysicsNeuralModule";};

			virtual void AddAdapter(string strXml);
			virtual void RemoveAdapter(string strID);
			virtual int FindAdapterListPos(string strID, BOOL bThrowError = TRUE);

			virtual void Kill(BOOL bState = TRUE);
			virtual void Initialize();
			virtual void ResetSimulation();
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);

	#pragma region DataAccesMethods
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
			virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);
	#pragma endregion

			virtual void AddNeuron(string strXml);
			virtual void RemoveNeuron(string strID, BOOL bThrowError = TRUE);
			virtual int FindNeuronListPos(string strID, BOOL bThrowError = TRUE);

	#pragma region SnapshotMethods
				virtual long CalculateSnapshotByteSize();
				virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
				virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
	#pragma endregion

		};

	}				//Behavior
}				//AnimatSim
