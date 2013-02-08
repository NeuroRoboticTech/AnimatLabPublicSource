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

			virtual float TimeStep();
			virtual void TimeStep(float fltVal);

			/**
			\brief	Gets the module name.
		
			\author	dcofer
			\date	3/29/2011
		
			\return	module name.
			**/
			virtual string ModuleName() {return "PhysicsNeuralModule";};

			virtual void AddAdapter(string strXml, BOOL bDoNotInit);
			virtual void RemoveAdapter(string strID);
			virtual int FindAdapterListPos(string strID, BOOL bThrowError = TRUE);

			virtual void Kill(BOOL bState = TRUE);
			virtual void Initialize();
			virtual void ResetSimulation();
			virtual void Load(CStdXml &oXml);

	#pragma region DataAccesMethods
			virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
			virtual BOOL AddItem(const string &strItemType, const string &strXml, BOOL bThrowError = TRUE, BOOL bDoNotInit = FALSE);
			virtual BOOL RemoveItem(const string &strItemType, const string &strID, BOOL bThrowError = TRUE);
	#pragma endregion

	#pragma region SnapshotMethods
				virtual long CalculateSnapshotByteSize();
				virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
				virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
	#pragma endregion

		};

	}				//Behavior
}				//AnimatSim
