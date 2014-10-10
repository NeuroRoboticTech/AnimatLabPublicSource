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

			static PhysicsNeuralModule *CastToDerived(AnimatBase *lpBase) {return static_cast<PhysicsNeuralModule*>(lpBase);}

			virtual float TimeStep();
			virtual void TimeStep(float fltVal);

			/**
			\brief	Gets the module name.
		
			\author	dcofer
			\date	3/29/2011
		
			\return	module name.
			**/
			virtual std::string ModuleName() {return "PhysicsNeuralModule";};

			virtual void AddAdapter(std::string strXml, bool bDoNotInit);
			virtual void RemoveAdapter(std::string strID);
			virtual int FindAdapterListPos(std::string strID, bool bThrowError = true);

			virtual void Kill(bool bState = true);
			virtual void Initialize();
			virtual void ResetSimulation();
			virtual void Load(CStdXml &oXml);

	#pragma region DataAccesMethods
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
			virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);
	#pragma endregion

	#pragma region SnapshotMethods
				virtual long CalculateSnapshotByteSize();
				virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
				virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
	#pragma endregion

		};

	}				//Behavior
}				//AnimatSim
